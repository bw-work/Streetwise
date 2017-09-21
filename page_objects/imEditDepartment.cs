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
    class imEditDepartment : imMaster
    {
        public imEditDepartment(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        #region Objects

        public HpgElement DepartmentName
        {
            get
            {
                return new HpgElement(browser.FindId("Name"));
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
