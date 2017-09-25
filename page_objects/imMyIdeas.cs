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

namespace Streetwise.page_objects
{
    class imMyIdeas : swIdeaListMaster
    {
        public imMyIdeas(BrowserSession currentBrowser) : base(currentBrowser)
        {
        }


        #region Objects

        public class MyIdea
        {
            public HpgElement IdeaNumber;
            public HpgElement IdeaName;
            public string Assigned;
            public string Status;
            public DateTime Created;
            public DateTime Updated;
            public HpgElement Action;
        }

        #endregion



        #region Actions
        
        public void MySortBy(string columnName, string order = "ASCENDING")
        {
            SuperTest.WriteReport("Sorting by " + columnName + " " + order);
            HpgElement headerLink = new HpgElement(browser.FindId("myIdeasTable").FindLink(columnName));
            HpgAssert.Exists(headerLink, "Verify header link exists");
            headerLink.Click();
            System.Threading.Thread.Sleep(20000);
            if (order.ToLower().Contains("desc"))
            {
                //Sort Descending
                while (!browser.Location.ToString().ToLower().Contains("desc"))
                {
                    //Keep clicking until URL contains "desc"
                    headerLink.Click();
                    System.Threading.Thread.Sleep(5000);
                }
            }
            else
            {
                //Sort Ascending
                while (browser.Location.ToString().ToLower().Contains("desc"))
                {
                    //Keep clicking until URL does not contain "desc"
                    headerLink.Click();
                    System.Threading.Thread.Sleep(5000);
                }
            }
        }

        public List<MyIdea> GetMyIdeas()
        {
            browser.FindXPath("//*[@id='myIdeasTable']//tr[td]", Options.First).Exists();
            List<MyIdea> returnList = new List<MyIdea>();
            foreach (SnapshotElementScope row in browser.FindAllXPath("//*[@id='myIdeasTable']//tr[td]"))
            {
                MyIdea addIdea = new MyIdea();
                addIdea.IdeaNumber = new HpgElement(row.FindXPath("td[1]"));
                addIdea.IdeaName = new HpgElement(row.FindXPath("td[2]"));
                addIdea.Assigned = row.FindXPath("td[3]").Text.Trim();
                addIdea.Status = row.FindXPath("td[4]").Text.Trim();
                addIdea.Created = DateTime.Parse(string.IsNullOrEmpty(row.FindXPath("td[5]").Text.Trim()) ? "01/01/1950" : row.FindXPath("td[6]").Text.Trim());
                addIdea.Updated = DateTime.Parse(string.IsNullOrEmpty(row.FindXPath("td[6]").Text.Trim()) ? "01/01/1950" : row.FindXPath("td[6]").Text.Trim());
                addIdea.Action = new HpgElement(row.FindXPath("td[7]"));
                returnList.Add(addIdea);
            }
            return returnList;
        }

        #endregion
    }
}
