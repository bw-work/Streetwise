using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coypu;
using AutomationCore;
using AutomationCore.utility;
using System.Windows.Forms;
using OpenQA.Selenium.IE;

namespace IdeaManagement.page_objects
{
    class imHomePageCMS : imMaster
    {
        public imHomePageCMS(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        #region Objects

        public HpgElement editTitle
        {
            get
            {
                return new HpgElement(browser.FindId("Title"));
            }
        }

        public HpgElement editBody
        {
            get
            {
                return new HpgElement(browser.FindId("mceu_21"));
            }
        }

        public HpgElement editBodyMenuTools
        {
            get
            {
                return new HpgElement(browser.FindXPath("//div[button/span[.='Tools']]"));
            }
        }

        public HpgElement editBodyMenuToolsSourceCode
        {
            get
            {
                return new HpgElement(browser.FindXPath("//div[@role='menuitem' and span[.='Source code']]"));
            }
        }

        public HpgElement editSourceCodeTextarea
        {
            get
            {
                return new HpgElement(browser.FindXPath("//div[@role='application']//textarea"));
            }
        }

        public HpgElement editSourceCodeOKButton
        {
            get
            {
                return new HpgElement(browser.FindXPath("//div[@role='application']//button[.='Ok']"));
            }
        }

        public HpgElement editSourceCodeCancelButton
        {
            get
            {
                return new HpgElement(browser.FindXPath("//div[@role='application']//button[.='Cancel']"));
            }
        }

        public HpgElement editSaveButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Save"));
            }
        }

        public HpgElement editCancelButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Cancel"));
            }
        }

        #endregion



        #region Actions

        public HpgElement GetCMSRow(string rowName)
        {
            return
                new HpgElement(
                    browser.FindXPath(
                        "//form[@action='/Admin/Category/Create']//table//tr[.//td[.='" + rowName + "']]"));
        }

        #endregion
    }
}
