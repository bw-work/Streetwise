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
    class imReassign : imIdeaDetails
    {
        public imReassign(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        #region Objects

        public HpgElement UserSelect
        {
            get
            {
                return new HpgElement(browser.FindId("Users"));
            }
        }

        public HpgElement CommentTextBox
        {
            get
            {
                return new HpgElement(browser.FindId("IdeaComment_Comment"));
            }
        }

        public HpgElement ComfirmButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Confirm"));
            }
        }

        public HpgElement CancelButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Cancel"));
            }
        }

        #endregion

        #region Actions

        public void SelectReassignTo(string username)
        {
            UserSelect.Click();
            UserSelect.Element.SendKeys(username);
            UserSelect.Element.FindXPath(".//*[contains(.,'" + username + "')]").Click();
            browser.Select(username).From(UserSelect.Element.Id);
        }

        #endregion
    }
}
