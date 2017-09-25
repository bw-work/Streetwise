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

namespace Streetwise.page_objects
{
    class imCategories : swIdeaDetails
    {
        public imCategories(BrowserSession currentBrowser) : base(currentBrowser)
        {
        }

        #region Objects

        public HpgElement FilterButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Filter"));
            }
        }

        public HpgElement ClearFilterButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Clear Filter"));
            }
        }

        public HpgElement searchCategory
        {
            get
            {
                return new HpgElement(browser.FindId("searchString"));
            }
        }

        public HpgElement newCategoryName
        {
            get
            {
                return new HpgElement(browser.FindXPath("//div[@id='content']//input[@name='categoryName']"));
            }
        }

        public HpgElement newCategoryOpportunity
        {
            get
            {
                return new HpgElement(browser.FindId("opportunity"));
            }
        }

        public HpgElement newCategoryCreateButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Create Category"));
            }
        }

        public HpgElement categoryRow(string categoryName)
        {
            return new HpgElement(browser.FindXPath("//form[@action='/Admin/Category/Create']/table//tr[td[.='" + categoryName + "']]", new Options(){Match = Match.First}));
        }



        #endregion
    }
}
