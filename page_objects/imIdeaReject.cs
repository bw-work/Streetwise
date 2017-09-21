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
    class imIdeaReject : imMaster
    {
        public imIdeaReject(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        #region Objects

        public HpgElement RejectComments
        {
            get
            {
                return new HpgElement(browser.FindId("IdeaComment_Comment"));
            }
        }

        public HpgElement ConfirmButton
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
    }
}
