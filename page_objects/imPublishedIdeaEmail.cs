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



namespace Streetwise.page_objects
{
    class imPublishedIdeaEmail : swPublishedIdeaDetails
    {
        public imPublishedIdeaEmail(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        #region Objects

        public HpgElement EmailDialog
        {
            get
            {
                return new HpgElement(browser.FindXPath("//div[div[@id='emailDialog']]"));
            }
        }

        public HpgElement ToField
        {
            get
            {
                return new HpgElement(browser.FindId("to-field"));
            }
        }

        public HpgElement FromField
        {
            get
            {
                return new HpgElement(browser.FindId("From"));
            }
        }

        public HpgElement SubjectField
        {
            get
            {
                return new HpgElement(browser.FindId("Subject"));
            }
        }

        public HpgElement MessageField
        {
            get
            {
                return new HpgElement(browser.FindId("email-content"));
            }
        }

        public HpgElement SendButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Send Email"));
            }
        }

        public HpgElement CancelButton
        {
            get
            {
                return new HpgElement(browser.FindButton("cancel"));
            }
        }

        #endregion

        #region Actions



        #endregion
    }
}
