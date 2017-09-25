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
    class imIdeaDelete : swMaster
    {
        public imIdeaDelete(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        #region Objects

        public HpgElement ConfirmDeleteButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Confirm Delete"));
            }
        }

        #endregion
    }
}
