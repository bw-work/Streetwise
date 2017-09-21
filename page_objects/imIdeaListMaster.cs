using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coypu;
using AutomationCore;
using AutomationCore.utility;
using System.Windows.Forms;
using HtmlAgilityPack;
using OpenQA.Selenium.IE;
using System.Xml;
using System.Xml.Linq;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace IdeaManagement.page_objects
{
    class imIdeaListMaster : imMaster
    {
        public imIdeaListMaster(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        #region CustomClasses

        public class AllIdea
        {
            public int IdeaId;
            public HpgElement Title;
            public bool InfoRequested;
            public DateTime Updated;
            public DateTime Created;
            public string Status;
            public string AssignedTo;
            public string Category;
            public string Department;
            public string Level;
        }

        public class Refinement
        {
            public string Type;
            public string Value;
            public HpgElement Delete;
        }

        #endregion

        #region Objects

        public HpgElement sortIdeaID
        {
            get
            {
                return new HpgElement(browser.FindLink("Idea Number"));
            }
        }

        public HpgElement sortIdeaName
        {
            get
            {
                return new HpgElement(browser.FindLink("Idea Name"));
            }
        }

        public HpgElement sortAssigned
        {
            get
            {
                return new HpgElement(browser.FindLink("Assigned"));
            }
        }

        public HpgElement sortStatus
        {
            get
            {
                return new HpgElement(browser.FindLink("Status"));
            }
        }

        public HpgElement sortCreated
        {
            get
            {
                return new HpgElement(browser.FindLink("Created"));
            }
        }

        public HpgElement sortUpddated
        {
            get
            {
                return new HpgElement(browser.FindLink("Updated"));
            }
        }


        public HpgElement FilterButton
        {
            get
            {
                return new HpgElement(browser.FindXPath("//input[@type='submit' or @type='Submit']", new Options() { Match = Match.First }));
                //return new HpgElement(browser.FindId("btnSearch"));
            }
        }

        public HpgElement ClearFilterButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Clear Refinements"));
            }
        }

        public void ClearRefinements()
        {
            ClearFilterButton.Click();
            WaitForThrobber();
            HpgAssert.True(!GetAllRefinements().Any(), "No refinements are applied");
        }

        public HpgElement SortByDropDown
        {
            get
            {
                return new HpgElement(browser.FindXPath("//select[@ng-init='setupSort()']"));
            }
        }

        public HpgElement RecordCount
        {
            get
            {
                return new HpgElement(browser.FindId("Recordcount"));
            }
        }

        #endregion


        #region Actions

        public List<AllIdea> GetAllIdeas(bool ScrollToBottom = true)
        {
            if(ScrollToBottom)base.ScrollToBottom();

            List<AllIdea> returnList = new List<AllIdea>();
            if (!browser.FindXPath("//div[@class='cardResult']", Options.First).Exists())
            {
                return returnList;
            }
            foreach (SnapshotElementScope row in browser.FindAllXPath("//div[@class='cardResult']"))
            {
                AllIdea addIdea = new AllIdea();
                addIdea.Title = new HpgElement(row.FindXPath(".//h3[contains(@class, 'autoTitle')]/a"));
                //addIdea.Title = new HpgElement(row.FindXPath("h3/a[contains(@href, 'Published/Create')]"));
                addIdea.InfoRequested = row.FindXPath("h3/a[@ng-show='idea.infoNeededBy']").Exists(new Options() { Timeout = TimeSpan.FromSeconds(.1) });
                addIdea.IdeaId = int.Parse(addIdea.Title.Text.Trim().Split('-')[0].Trim());
                addIdea.Updated = DateTime.Parse(row.FindXPath(".//p[contains(@class, 'autoUpdated')]").Text.Replace("Updated:", "").Trim());
                addIdea.Status = row.FindXPath(".//div[contains(@class, 'autoStatus')]").Text.Trim();
                if (row.FindXPath(".//h5[contains(@class, 'autoAssigned')]", new Options() {Timeout = TimeSpan.FromSeconds(1)}).Exists())
                {
                    //AssignedTo can be optional (as of 02/18/2015)
                    addIdea.AssignedTo = row.FindXPath(".//h5[contains(@class, 'autoAssigned')]").Text.Replace("Assigned To:", "").Trim();
                }
                addIdea.Category = row.FindXPath(".//p[contains(@class, 'autoCategory')]").Text.Replace("Category:", "").Trim();
                addIdea.Department = row.FindXPath(".//p[contains(@class, 'autoDepartment')]").Text.Replace("Department:", "").Trim();
                addIdea.Created = DateTime.Parse(row.FindXPath(".//p[contains(@class, 'autoCreatedDate')]").Text.Replace("Created:", "").Trim());
                addIdea.Level = "SAVED";
                if (row.FindXPath(".//div[@class='levelInfo']//h1", new Options() {Timeout = TimeSpan.FromSeconds(2)}).Exists())
                    addIdea.Level = row.FindXPath(".//div[@class='levelInfo']//h1").Text.Trim();
                returnList.Add(addIdea);
            }
            return returnList;
        }

        public List<Refinement> GetAllRefinements()
        {
            List<Refinement> returnList = new List<Refinement>();
            foreach (SnapshotElementScope refinement in browser.FindAllXPath("//div[@class='filterSummary']/span"))
            {
                Refinement addNew = new Refinement();
                addNew.Type = refinement.FindXPath("./span[contains(@class, 'refinementType')]").Text.Trim();
                addNew.Value = refinement.FindXPath("./span[contains(@class, 'refinementValue')]").Text.Trim();
                addNew.Delete = new HpgElement(refinement.FindXPath("./a"));
                returnList.Add(addNew);
            }
            return returnList;
        }

        public void SortBy(string sortBy)
        {
            SortByDropDown.SelectListOptionByText(sortBy);
        }

        public HpgElement GetFilter(string filterValue)
        {
            return new HpgElement(browser.FindField(filterValue));
        }

        public void AllSortBy(string columnName, string order = "ASCENDING")
        {
            SuperTest.WriteReport("Sorting by " + columnName + " " + order);
            HpgElement headerLink = new HpgElement(browser.FindId("allIdeasTable").FindLink(columnName));
            HpgAssert.Exists(headerLink, "Verify header link exists");
            headerLink.Click();
            Thread.Sleep(20000);
            if (order.ToLower().Contains("desc"))
            {
                //Sort Descending
                while (!browser.Location.ToString().ToLower().Contains("desc"))
                {
                    //Keep clicking until URL contains "desc"
                    headerLink.Click();
                    Thread.Sleep(5000);
                }
            }
            else
            {
                //Sort Ascending
                while (browser.Location.ToString().ToLower().Contains("desc"))
                {
                    //Keep clicking until URL does not contain "desc"
                    headerLink.Click();
                    Thread.Sleep(5000);
                }
            }
        }

        public DataTable GetMyIdeasDT()
        {
            DataTable returnTable = new DataTable("MyIdeas");

            returnTable.Columns.Add("IdeaNumber", typeof(int));
            returnTable.Columns["IdeaNumber"].Unique = true;
            returnTable.Columns.Add("IdeaName", typeof(string));
            returnTable.Columns.Add("Assigned", typeof(string));
            returnTable.Columns.Add("Status", typeof(string));
            returnTable.Columns.Add("CreatedDate", typeof(DateTime));
            returnTable.Columns.Add("UpdatedDate", typeof(DateTime));

            var doc = new HtmlDocument();
            doc.LoadHtml(browser.FindId("myIdeasTable")["outerHTML"]);
            foreach (HtmlNode ideaRow in doc.DocumentNode.SelectNodes("//tr[td]"))
            {
                DataRow idea = returnTable.NewRow();
                string[] ideaColumns = (from c in ideaRow.SelectNodes("./td") select c.InnerText).ToArray();
                idea["IdeaNumber"] = Int32.Parse(ideaColumns[0]);
                idea["IdeaName"] = ideaColumns[1];
                idea["Assigned"] = ideaColumns[2];
                idea["Status"] = ideaColumns[3];
                idea["CreatedDate"] = DateTime.Parse(ideaColumns[4]);
                idea["UpdatedDate"] = DateTime.Parse(ideaColumns[5]);
                returnTable.Rows.Add(idea);
            }
            return returnTable;
        }

        public DataTable GetAllIdeasDT()
        {
            DataTable returnTable = new DataTable("AllIdeas");

            returnTable.Columns.Add("IdeaNumber", typeof(int));
            returnTable.Columns["IdeaNumber"].Unique = true;
            returnTable.Columns.Add("IdeaName", typeof(string));
            returnTable.Columns.Add("Author", typeof(string));
            returnTable.Columns.Add("Assigned", typeof(string));
            returnTable.Columns.Add("Status", typeof(string));
            returnTable.Columns.Add("CreatedDate", typeof(DateTime));
            returnTable.Columns.Add("UpdatedDate", typeof(DateTime));

            var doc = new HtmlDocument();
            doc.LoadHtml(browser.FindId("allIdeasTable")["outerHTML"]);
            foreach (HtmlNode ideaRow in doc.DocumentNode.SelectNodes("//tr[td]"))
            {
                DataRow idea = returnTable.NewRow();
                string[] ideaColumns = (from c in ideaRow.SelectNodes("./td") select c.InnerText).ToArray();
                idea["IdeaNumber"] = Int32.Parse(ideaColumns[0]);
                idea["IdeaName"] = ideaColumns[1];
                idea["Author"] = ideaColumns[2];
                idea["Assigned"] = ideaColumns[3];
                idea["Status"] = ideaColumns[4];
                idea["CreatedDate"] = DateTime.Parse(ideaColumns[5]);
                idea["UpdatedDate"] = DateTime.Parse(ideaColumns[6]);
                returnTable.Rows.Add(idea);
            }
            return returnTable;
        }

        

        #endregion
    }
}
