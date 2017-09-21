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
    class imIdeaDetailsAdmin : imIdeaDetails
    {
        public imIdeaDetailsAdmin(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        #region Objects

        public HpgElement AcceptButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Accept"));
            }
        }

        public HpgElement RejectButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Reject"));
            }
        }

        public HpgElement ReassignButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Reassign"));
            }
        }

        public HpgElement PublishIdeaButton
        {
            get
            {
                return  new HpgElement(browser.FindButton("Publish Idea"));
            }
        }

        public HpgElement AssociateIdeaButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Associate Idea"));
            }
        }

        #endregion
    }
}
