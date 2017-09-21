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

namespace IdeaManagement.page_objects
{
    class imEditCategory : imMaster
    {
        public imEditCategory(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        #region Objects

        public HpgElement CategoryName
        {
            get
            {
                return  new HpgElement(browser.FindId("Name"));
            }
        }

        public HpgElement CategoryOpportunity
        {
            get
            {
                return new HpgElement(browser.FindId("OpportunityId"));
            }
        }

        public HpgElement SaveButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Save"));
            }
        }

        public HpgElement BackButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Back"));
            }
        }

        #endregion
    }
}
