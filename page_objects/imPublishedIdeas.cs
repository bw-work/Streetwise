using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coypu;
using AutomationCore;
using AutomationCore.utility;
using System.Windows.Forms;
using OpenQA.Selenium.IE;
using System.Xml;
using System.Xml.Linq;
using System.Data;
using HtmlAgilityPack;
using Streetwise.Utility;


namespace Streetwise.page_objects
{
    class imPublishedIdeas : swMaster
    {
        public imPublishedIdeas(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        

        public class PublishedIdea
        {
            public HpgElement Bookmark;
            public int IdeaNumber;
            public HpgElement IdeaName;
            public string Department;
            public string Category;
            public string EffortLevel;
            public int Effort;
            public string ImpactLevel;
            public int Impact;
            public DateTime Updated;
        }

        public class CFApPublishedIdea : PublishedIdea
        {
            public HpgElement ImplementSelect;
            public Enums.ImplementedStatus ImplementedStatus;
        }

        #region Objects

        public HpgElement SearchIdeaID
        {
            get
            {
                return new HpgElement(browser.FindId("Search"));
            }
        }

        public HpgElement SearchIdeaName
        {
            get
            {
                return new HpgElement(browser.FindId("ideaNameSearch"));
            }
        }

        public HpgElement FilterButton
        {
            get
            {
                return new HpgElement(browser.FindId("btnSearch"));
            }
        }

        public HpgElement ClearFilterButton
        {
            get
            {
                return new HpgElement(browser.FindId("btnReset"));
            }
        }

        public HpgElement FilterMoreDepartments
        {
            get
            {
                return new HpgElement(browser.FindId("showMoreDepartments"));
            }
        }

        public HpgElement FilterMoreCategories
        {
            get
            {
                return new HpgElement(browser.FindId("showMoreCategories"));
            }
        }

        public HpgElement FilterRecordCount
        {
            get
            {
                return new HpgElement(browser.FindId("Recordcount"));
            }
        }

        public int FilterRecordCountInt
        {
            get
            {
                return int.Parse(new string(FilterRecordCount.Text.Where(Char.IsDigit).ToArray()));
            }
        }

        public HpgElement LinkExcel
        {
            get
            {
                return new HpgElement(browser.FindXPath("//*[@id='PublishedIdeaForm']//a[contains(@href, 'ExportExcel')]"));
            }
        }

        public HpgElement LinkPDF
        {
            get
            {
                return new HpgElement(browser.FindXPath("//*[@id='PublishedIdeaForm']//a[contains(@href, 'ExportPDF')]"));
            }
        }

        public HpgElement PageSize
        {
            get
            {
                return new HpgElement(browser.FindId("pageSize"));
            }
        }


        public HpgElement BulkEditLink
        {
            get
            {
                return new HpgElement(browser.FindLink("Bulk Update"));
            }
        }

        public HpgElement BulkEditSelectAllCheckbox
        {
            get
            {
                return new HpgElement(browser.FindXPath("//input[@ng-model='selectAllChecked']"));
            }
        }

        public HpgElement BulkEditImplementDropdown
        {
            get
            {
                return new HpgElement(browser.FindField("bulkImplementationStatusSelect"));
            }
        }

        public HpgElement BulkEditImplementDateDropdown
        {
            get
            {
                return new HpgElement(browser.FindId("bulkImplementationDate"));
            }
        }

        public HpgElement BulkEditApplyButton
        {
            get
            {
                return new HpgElement(browser.FindXPath("//button[@ng-click='applyImplementationStatus()']"));
            }
        }

        public HpgElement BulkEditSuccessMessage
        {
            get
            {
                return new HpgElement(browser.FindXPath("//div[@ng-show='postbackSucceeded']/span"));
            }
        }

        #endregion



        #region Actions

        public void BulkEditApplyCuttonClick()
        {
            BulkEditApplyButton.Element.Hover();
            BulkEditApplyButton.Click();
            System.Threading.Thread.Sleep(10000);
        }

        public void ShowBulkEdit()
        {
            if(!BulkEditImplementDropdown.Element.Exists(new Options(){Timeout = TimeSpan.FromSeconds(1)})) BulkEditLink.Click();
            HpgAssert.True(BulkEditImplementDropdown.Element.Exists(), "Bulk Change Drop Down is visible");
        }

        public void SaveExcel(string saveFileName)
        {
            browser.SaveWebResource(LinkExcel.Element["href"], saveFileName);
        }

        public void ShowAllDepartments()
        {
            for (int i = 0; i < 5; i++)
            {
                FilterMoreDepartments.Element.Hover();
                FilterMoreDepartments.Click();
                if (FilterMoreDepartments.Element.Missing())
                {
                    break;
                }
            }
            HpgAssert.True(FilterMoreDepartments.Element.Missing(), "Show More Departments selected");
        }

        public void ShowAllCategories()
        {
            for (int i = 0; i < 5; i++)
            {
                FilterMoreCategories.Element.Hover();
                FilterMoreCategories.Click();
                if (FilterMoreCategories.Element.Missing())
                {
                    break;
                }
            }
            HpgAssert.True(FilterMoreCategories.Element.Missing(), "Show More Categories selected");
        }

        public HpgElement GetDepartmentFilter(string department)
        {
            return new HpgElement(browser.FindXPath("//ul[@id='departments']//a[.='" + department + "']"));
        }

        public HpgElement GetCategoryFilter(string category)
        {
            return new HpgElement(browser.FindXPath("//ul[@id='categories']//a[.='" + category + "']"));
        }

        public HpgElement GetEffortFilter(int level)
        {
            return new HpgElement(browser.FindXPath("//li[count(span[@id='level'])=" + level.ToString() + "]/a[contains(@id, 'Effort')]"));
        }

        public HpgElement GetImpactFilter(int level)
        {
            return new HpgElement(browser.FindXPath("//li[count(span[@id='level'])=" + level.ToString() + "]/a[contains(@id, 'Impact')]"));
        }

        public HpgElement GetUpdatedDateFilter(int filter)
        {
            return new HpgElement(browser.FindXPath("//ul[@id='updateDiate']/li[" + filter.ToString() + "]/a"));
        }

        public DataTable GetPublishedIdeasDT()
        {
            DataTable returnTable = new DataTable("PublishedIdeas");

            returnTable.Columns.Add("IdeaNumber", typeof(int));
            returnTable.Columns["IdeaNumber"].Unique = true;
            returnTable.Columns.Add("IdeaName", typeof(string));
            returnTable.Columns.Add("Assigned", typeof(string));
            returnTable.Columns.Add("Effort", typeof(int));
            returnTable.Columns.Add("Impact", typeof(int));
            returnTable.Columns.Add("UpdatedDate", typeof(DateTime));
            //returnTable.Columns.Add("PublishedDate", typeof(DateTime));
            returnTable.Columns.Add("Category", typeof(string));
            returnTable.Columns.Add("Department", typeof(string));

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(browser.FindId("publishIdeasTable")["outerHTML"]);
            foreach (HtmlNode ideaRow in doc.DocumentNode.SelectNodes("//tr[td]"))
            {
                DataRow idea = returnTable.NewRow();
                string[] ideaColumns = (from c in ideaRow.SelectNodes("./td") select c.InnerText).ToArray();
                idea["IdeaNumber"] = int.Parse(ideaColumns[0]);
                idea["IdeaName"] = ideaColumns[1];
                idea["Department"] = ideaColumns[2];
                idea["Category"] = ideaColumns[3];
                idea["Effort"] = ideaRow.SelectNodes("./td")[4].SelectNodes(".//i").Count;
                idea["Impact"] = ideaRow.SelectNodes("./td")[5].SelectNodes(".//i").Count;
                //idea["PublishedDate"] = DateTime.Parse(ideaColumns[6]);
                idea["UpdatedDate"] = DateTime.Parse(string.IsNullOrEmpty(ideaColumns[6]) ? "01/01/1900" : ideaColumns[6]);
                returnTable.Rows.Add(idea);
            }
            return returnTable;
        }

        public List<PublishedIdea> GetPublishedIdeas(bool ScrollToBottom = true)
        {
            if(ScrollToBottom) base.ScrollToBottom();

            browser.FindXPath("//div[@class='cardResult']", new Options() { Match = Match.First, Timeout = TimeSpan.FromSeconds(90) }).Exists();
            List<PublishedIdea> returnList = new List<PublishedIdea>();
            foreach (SnapshotElementScope row in browser.FindAllXPath("//div[@class='cardResult']"))
            {
                PublishedIdea addIdea = new PublishedIdea();
                addIdea.Bookmark = new HpgElement(row.FindXPath(".//a[@ng-tooltip='Bookmark Idea']"));
                addIdea.IdeaName = new HpgElement(row.FindXPath(".//h3[contains(@class, 'autoTitle')]/a"));
                addIdea.IdeaNumber = int.Parse(addIdea.IdeaName.Text.Trim().Split('-')[0].Trim());
                addIdea.Updated = DateTime.Parse(row.FindXPath(".//p[contains(@class, 'autoUpdated')]").Text.Replace("Updated:", "").Trim());
                addIdea.Category = row.FindXPath(".//p[contains(@class, 'autoCategory')]").Text.Replace("Category:", "").Trim();
                addIdea.Department = row.FindXPath(".//p[contains(@class, 'autoDepartment')]").Text.Replace("Department:", "").Trim();
                addIdea.Impact = row.FindAllXPath(".//i[contains(@class, 'icon-usd')]").Count();
                addIdea.ImpactLevel = Enums.levels[addIdea.Impact];
                addIdea.Effort = row.FindAllXPath(".//i[contains(@class, 'icon-wrench')]").Count();
                addIdea.EffortLevel = Enums.levels[addIdea.Effort];
                returnList.Add(addIdea);
            }
            return returnList;
        }

        public int[] GetPublishedIdeasIds()
        {
            ScrollToBottom();
            browser.FindXPath("//div[@class='cardResult']", new Options() { Match = Match.First, Timeout = TimeSpan.FromSeconds(90) }).Exists();
            HtmlAgilityPack.HtmlDocument reader = new HtmlAgilityPack.HtmlDocument();
            reader.LoadHtml(browser.FindXPath("//div[@ng-repeat='cardRow in cardRows']/..").OuterHTML);
            return (from t in reader.DocumentNode.SelectNodes("//h3[contains(@class, 'autoTitle')]")
                    select int.Parse(t.InnerText.Split('-').First().Trim())).ToArray();
        }

        public List<CFApPublishedIdea> GetCFApPublishedIdeas(bool ScrollToBottom = true)
        {
            if (ScrollToBottom) base.ScrollToBottom();

            browser.FindXPath("//div[@class='cardResult']", new Options() { Match = Match.First, Timeout = TimeSpan.FromSeconds(90) }).Exists();
            List<CFApPublishedIdea> returnList = new List<CFApPublishedIdea>();
            foreach (SnapshotElementScope row in browser.FindAllXPath("//div[@class='cardResult']"))
            {
                returnList.Add(ToPublishedIdea(new HpgElement(row)));
            }
            return returnList;
        }

        public List<CFApPublishedIdea> GetCFApPublishedIdeas(int start, int count, bool ScrollToBottom = true)
        {
            if (ScrollToBottom) base.ScrollToBottom();
            System.Threading.Thread.Sleep(5000);
            browser.FindXPath("//div[@class='cardResult']", new Options() { Match = Match.First, Timeout = TimeSpan.FromSeconds(90) }).Exists();
            List<CFApPublishedIdea> returnList = new List<CFApPublishedIdea>();
            foreach (SnapshotElementScope row in browser.FindAllXPath("//div[@class='cardResult']").Skip(start).Take(count))
            {
                returnList.Add(ToPublishedIdea(new HpgElement(row)));
            }
            return returnList;
        }

        private CFApPublishedIdea ToPublishedIdea(HpgElement card)
        {
            CFApPublishedIdea addIdea = new CFApPublishedIdea();
            addIdea.Bookmark = new HpgElement(card.Element.FindXPath(".//a[@ng-tooltip='Bookmark Idea']"));
            addIdea.IdeaName = new HpgElement(card.Element.FindXPath(".//h3[contains(@class, 'autoTitle')]/a"));
            addIdea.IdeaNumber = int.Parse(addIdea.IdeaName.Text.Trim().Split('-')[0].Trim());
            addIdea.Updated = DateTime.Parse(card.Element.FindXPath(".//p[contains(@class, 'autoUpdated')]").Text.Replace("Updated:", "").Trim());
            addIdea.Category = card.Element.FindXPath(".//p[contains(@class, 'autoCategory')]").Text.Replace("Category:", "").Trim();
            addIdea.Department = card.Element.FindXPath(".//p[contains(@class, 'autoDepartment')]").Text.Replace("Department:", "").Trim();
            addIdea.Impact = card.Element.FindAllXPath(".//i[contains(@class, 'icon-usd')]").Count();
            addIdea.ImpactLevel = Enums.levels[addIdea.Impact];
            addIdea.Effort = card.Element.FindAllXPath(".//i[contains(@class, 'icon-wrench')]").Count();
            addIdea.EffortLevel = Enums.levels[addIdea.Effort];
            addIdea.ImplementSelect = new HpgElement(card.Element.FindXPath(".//input[@ng-model='idea.updateImplementationStatus']"));
            addIdea.ImplementedStatus = Enums.ImplementedStatusString[card.Element.FindXPath(".//p[@class='autoImplementedStatus']/span").Text.Trim()];
            return addIdea;
        }
        
        public void SortIdeasBy(string columnName, string order = "ASCENDING")
        {
            SuperTest.WriteReport("Sorting by " + columnName + " " + order);
            AutomationCore.base_tests.BaseTest.AdjustMaxTimeout(240);
            var headerLink = browser.FindXPath("//a[contains(@id,'sortOrder') and .='" + columnName + "'][1]", new Options(){Match = Match.First});
            //HpgElement headerLink = new HpgElement(browser.FindId("publishIdeasTable").FindLink(columnName));
            //HpgAssert.True(headerLink.Element.Exists(), "Verify header link exists");
            headerLink.SendKeys(OpenQA.Selenium.Keys.Home);
            headerLink.Hover();
            headerLink.Hover();
            System.Threading.Thread.Sleep(2000);
            headerLink.SendKeys(OpenQA.Selenium.Keys.Enter);
            //headerLink.Click();
            System.Threading.Thread.Sleep(20000);
            if (order.ToLower().Contains("desc"))
            {
                //Sort Descending
                while (!headerLink.FindXPath("./..").Text.Contains("▼"))
                {
                    headerLink.Hover();
                    headerLink.Hover();
                    System.Threading.Thread.Sleep(2000);
                    headerLink.SendKeys(OpenQA.Selenium.Keys.Enter);
                    //headerLink.Click();
                    System.Threading.Thread.Sleep(5000);
                }
            }
            else
            {
                //Sort Ascending
                while (!headerLink.FindXPath("./..").Text.Contains("▲"))
                {
                    headerLink.Hover();
                    headerLink.Hover();
                    System.Threading.Thread.Sleep(2000);
                    headerLink.SendKeys(OpenQA.Selenium.Keys.Enter);
                    //headerLink.Click();
                    System.Threading.Thread.Sleep(5000);
                }
            }
            AutomationCore.base_tests.BaseTest.ResetMaxTimeout();
        }

        #endregion
    }
}
