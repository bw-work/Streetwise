using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coypu;
using AutomationCore;
using AutomationCore.utility;
using System.Windows.Forms;
using HtmlAgilityPack;
using OpenQA.Selenium.IE;
using System.Xml;
using System.Xml.Linq;

namespace IdeaManagement.page_objects
{
    class imNewStreetwiseIdeas : imMaster
    {
        public imNewStreetwiseIdeas(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        public class NewIdea
        {
            public string IdeaID;
            public HpgElement IdeaName;
            public DateTime CreatedDate;
            public string Department;
            public string Status;
            public DateTime UpdatedDate;
            public HpgElement Action;
            public string CurrentAction;
        }

        public enum FilterDates
        {
            Last7Days,
            Last30Days,
            Last60Days,
            Last90Days,
            Last6Months,
            LastLogin
        }

        public class IdeaText
        {
            public string IdeaID;
            public string IdeaName;
            public string IdeaDescription;
        }

        #region Objects

        public HpgElement SearchBox
        {
            get
            {
                return new HpgElement(browser.FindXPath("//input[contains(@class,'searchBox')]"));
            }
        }

        public HpgElement SearchButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Search"));
            }
        }

        public HpgElement SearchReset
        {
            get
            {
                return new HpgElement(browser.FindButton("Reset Search"));
            }
        }

        public HpgElement FilterIdeaViewNoAction
        {
            get
            {
                return new HpgElement(browser.FindId("filter").FindId("statusPending"));
            }
        }

        //public Utility.AngularElements.CheckBox FilterIdeaViewNoAction
        //{
        //    get
        //    {
        //        return new Utility.AngularElements.CheckBox(browser.FindXPath("//div[@id='filter']//a[contains(.,'No Action')]"));
        //        //return new
        //    }
        //}

        //public Utility.AngularElements.CheckBox FilterIdeaViewAccepted
        //{
        //    get
        //    {
        //        return new Utility.AngularElements.CheckBox(browser.FindXPath("//div[@id='filter']//a[contains(.,'Accepted')]"));
        //    }
        //}
        public HpgElement FilterIdeaViewAccepted
        {
            get
            {
                return new HpgElement(browser.FindId("filter").FindId("statusImported"));
            }
        }

        public HpgElement FilterIdeaViewRejected
        {
            get
            {
                return new HpgElement(browser.FindId("filter").FindId("statusExcluded"));
            }
        }

        public HpgElement FilterIdeaViewUpdated
        {
            get
            {
                return new HpgElement(browser.FindId("filter").FindId("statusAcceptPending"));
            }
        }

        //public Utility.AngularElements.CheckBox FilterIdeaViewRejected
        //{
        //    get
        //    {
        //        return new Utility.AngularElements.CheckBox(browser.FindXPath("//div[@id='filter']//a[contains(.,'Rejected')]"));
        //    }
        //}

        public HpgElement FilterShowMoreDepartments
        {
            get
            {
                return new HpgElement(browser.FindId("showMoreDepartments"));
            }
        }

        /// <summary>
        /// Returns the element for the header to sort by
        /// </summary>
        /// <param name="header">Options are ID, Name, CreatedDate, Department, Status, UpdatedDate</param>
        /// <returns></returns>
        public HpgElement SortHeader(string header = "ID")
        {
            return new HpgElement(browser.FindId("tableIdea" + header));
        }

        public Utility.AngularElements.CheckBox FilterCreatedDate(FilterDates filterDate)
        {
            return new Utility.AngularElements.CheckBox(browser.FindId("createdDate" + filterDate));
        }

        public Utility.AngularElements.CheckBox FilterDepartment(string department)
        {
            return new Utility.AngularElements.CheckBox(browser.FindXPath("//*[@id='departments']//a[contains(.,'" + department + "')]"));
        }

        public HpgElement SubmitButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Submit", new Options(){Match = Match.First}));
            }
        }

        public HpgElement BackButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Back"));
            }
        }

        public HpgElement ViewFilterButton
        {
            get
            {
                return new HpgElement(browser.FindXPath("//div[@id='content']//a[contains(.,'View')]"));
            }
        }

        //public HpgElement PageCurrent
        //{
        //    get
        //    {
        //        return new HpgElement(browser.FindXPath("//a[@class='btn btn-default blue selected']"));
        //    }
        //}

        //public HpgElement PageNext
        //{
        //    get
        //    {
        //        return new HpgElement(browser.FindXPath("//a[@class='btn btn-default blue selected']/following-sibling::a[.!='Last'][1]"));
        //    }
        //}

        //public HpgElement PagePrevious
        //{
        //    get
        //    {
        //        return new HpgElement(browser.FindXPath("//a[@class='btn btn-default blue selected']/preceding-sibling::a[1]"));
        //    }
        //}

        //public HpgElement PageLast
        //{
        //    get
        //    {
        //        return new HpgElement(browser.FindXPath("//a[.='Last']"));
        //    }
        //}

        //public HpgElement PageFirst
        //{
        //    get
        //    {
        //        return new HpgElement(browser.FindXPath("//a[.='First']"));
        //    }
        //}

        public HpgElement ResultsTable
        {
            get
            {
                return new HpgElement(browser.FindId("ideaResults"));
            }
        }

        public HpgElement ResultCount
        {
            get
            {
                return new HpgElement(browser.FindId("Recordcount"));
            }
        }

        public HpgElement popupCloseButton
        {
            get
            {
                if (browser.FindId("ideaDetails").Exists()) return new HpgElement(browser.FindId("ideaDetails").FindButton("Close"));
                return new HpgElement(browser.FindId("ideaDetailsCompare").FindButton("Close"));
            }
        }

        public HpgElement popupCompareOriginalDescription
        {
            get
            {
                return new HpgElement(browser.FindId("currentDescription"));
            }
        }

        public HpgElement popupCompareUpdatedDescription
        {
            get
            {
                return new HpgElement(browser.FindId("updatedDescription"));
            }
        }

        public HpgElement popupUpdatedStatus
        {
            get
            {
                return new HpgElement(browser.FindId("updatedStatus"));
            }
        }

        public HpgElement popupUpdatedTitle
        {
            get
            {
                return new HpgElement(browser.FindId("updatedTitle"));
            }
        }

        public HpgElement popupCurrentTitle
        {
            get
            {
                return new HpgElement(browser.FindId("currentTitle"));
            }
        }

        public HpgElement popupPublishedDescription
        {
            get
            {
                return new HpgElement(browser.FindId("publishedDescription"));
            }
        }

        public HpgElement popupPublishedTitle
        {
            get
            {
                return new HpgElement(browser.FindId("publishedTitle"));
            }
        }

        #endregion


        #region Actions

        public void Submit()
        {
            SubmitButton.Hover();
            SubmitButton.Hover();
            SubmitButton.Click();
            WaitForThrobber();
        }

        public void SearchFor(string q)
        {
            SearchBox.Type(q);
            SearchButton.Click();
            HpgAssert.True(browser.FindXPath("//div[@class='filterSummary']").HasContent("Search: " + q), "Verify search for '" + q + "' was performed");
        }

        public void FilterAllIdeas()
        {
            AutomationCore.base_tests.BaseTest.WriteReport("Filter All Ideas...");            
            FilterIdeaViewAccepted.UnCheck();
            FilterIdeaViewNoAction.UnCheck();
            FilterIdeaViewRejected.UnCheck();
        }

        public void FilterNewIdeas()
        {
            AutomationCore.base_tests.BaseTest.WriteReport("Filter New (No Action) Ideas...");
            //FilterAllIdeas();
            SearchReset.Click();
            FilterIdeaViewNoAction.Check();
        }

        public void FilterAcceptedIdeas()
        {
            AutomationCore.base_tests.BaseTest.WriteReport("Filter Accepted Ideas...");
            //FilterAllIdeas();
            SearchReset.Click();
            FilterIdeaViewAccepted.Check();
        }

        public void FilterUpdatedIdeas()
        {
            AutomationCore.base_tests.BaseTest.WriteReport("Filter Updated Ideas...");
            SearchReset.Click();
            FilterIdeaViewUpdated.Check();
        }

        public void FilterRejectedIdeas()
        {
            AutomationCore.base_tests.BaseTest.WriteReport("Filter Rejected Ideas...");
            //FilterAllIdeas();
            SearchReset.Click();
            FilterIdeaViewRejected.Check();
        }

        public List<NewIdea> GetAllIdeas(int limit=0)
        {
            WaitForThrobber();
            List<NewIdea> returnList = new List<NewIdea>();
            if (!ResultsTable.Element.HasNoContent("No results found for the selected criteria. Please modify your search criteria.", new Options(){Timeout = TimeSpan.FromSeconds(2)}))
            {
                return returnList;
            }
            foreach (SnapshotElementScope row in ResultsTable.Element.FindAllXPath(".//tr[td[6]]"))
            {
                NewIdea addIdea = new NewIdea();
                addIdea.IdeaID = row.FindXPath("td[1]").Text.Trim();
                addIdea.IdeaName = new HpgElement(row.FindXPath("td[2]/a"));
                addIdea.CreatedDate = DateTime.Parse(row.FindXPath("td[5]").Text);
                addIdea.Department = row.FindXPath("td[3]").Text.Trim();
                addIdea.Status = row.FindXPath("td[4]").Text.Trim();
                addIdea.UpdatedDate = DateTime.Parse(row.FindXPath("td[6]").Text);
                addIdea.Action = new HpgElement(row.FindXPath("td[7]//select"));
                if (row.FindXPath("td[7]/span[.='Imported']", new Options() { Timeout = TimeSpan.FromSeconds(.5) }).Exists())
                {
                    if (row.FindXPath("td[7]/span[.='Imported']")["class"] == "ng-hide")
                    {
                        addIdea.CurrentAction = row.FindXPath("td[7]//select").SelectedOption.ToLower();
                    }
                }
                else
                {
                    addIdea.CurrentAction = row.FindXPath("td[7]//select").SelectedOption.ToLower();
                }
                if (string.IsNullOrEmpty(addIdea.CurrentAction)) addIdea.CurrentAction = "imported";
                returnList.Add(addIdea);
                if (returnList.Count >= limit && limit > 0) break;
            }
            return returnList;
        }

        public List<NewIdea> GetAllIdeas(bool allPages, int limit = 0)
        {
            if (allPages) 
                ScrollToBottom();
            return GetAllIdeas(limit);
        }

        public DataTable GetAllIdeasDT(bool allPages = true)
        {
            AutomationCore.base_tests.BaseTest.WriteReport("Getting all ideas from Streetwise Idea Page...");

            DataTable returnTable = new DataTable("AllIdeas");
            ScrollToBottom();
            returnTable.Columns.Add("IdeaID", typeof(int));
            returnTable.Columns["IdeaID"].Unique = true;
            returnTable.Columns.Add("IdeaName", typeof(string));
            returnTable.Columns.Add("Created", typeof (DateTime));
            returnTable.Columns.Add("Department", typeof(string));
            returnTable.Columns.Add("Status", typeof(string));
            returnTable.Columns.Add("Updated", typeof(DateTime));
            returnTable.Columns.Add("Action", typeof(string));

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(ResultsTable.Element["outerHTML"]);
            foreach (HtmlNode ideaRow in doc.DocumentNode.SelectNodes("//tr[td]"))
            {
                DataRow idea = returnTable.NewRow();
                string[] ideaColumns = (from c in ideaRow.SelectNodes("./td") select c.InnerText).ToArray();
                idea["IdeaID"] = int.Parse(ideaColumns[0]);
                idea["IdeaName"] = ideaColumns[1];
                idea["Created"] = DateTime.Parse(ideaColumns[4].Trim());
                idea["Department"] = ideaColumns[2];
                idea["Status"] = ideaColumns[3];
                idea["Updated"] = DateTime.Parse(ideaColumns[5].Trim());
                idea["Action"] = "accepted";
                if (ideaRow.SelectNodes("./td").ElementAt(6).SelectNodes("./span[.='Accepted']").First().Attributes["class"] != null)
                {
                    if (ideaRow.SelectNodes("./td").ElementAt(6).SelectNodes("./span[.='Accepted']").First().Attributes["class"].Value.ToLower() == "ng-hide")
                    {
                        idea["Action"] = browser.FindXPath("/" + ideaRow.SelectNodes("./td").ElementAt(6).XPath).SelectedOption.ToLower();
                    }
                }
                if (string.IsNullOrEmpty(idea["Action"].ToString())) idea["Action"] = "accepted";
                returnTable.Rows.Add(idea);
            }
            AutomationCore.base_tests.BaseTest.WriteReport(returnTable.Rows.Count + " ideas returned");
            return returnTable;
        }

        #endregion
    }
}
