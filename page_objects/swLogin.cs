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

namespace Streetwise.page_objects
{
    class swLogin : swMaster
    {
        public swLogin(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        #region Objects

        public HpgElement HealthtrustLogo
        {
            get
            {
                return new HpgElement(browser.FindCss("#content > div:nth-child(2) > div:nth-child(1) > div.span4.text-center > a:nth-child(1) > img"));
            }
        }

        public HpgElement CoretrustLogo
        {
            get
            {
                return new HpgElement(browser.FindCss("#content > div:nth-child(2) > div:nth-child(1) > div.span4.text-center > a:nth-child(2) > img"));
            }
        }

        public HpgElement LoginForm
        {
            get
            {
                return new HpgElement(browser.FindCss("#externalForm > form > div:nth-child(1) > div.span4.externalWell"));
            }
        }

        public HpgElement LoginTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#externalForm > form > div:nth-child(1) > div.span4.externalWell > div.externalWellHeading > h1"));
            }
        }

        public HpgElement EmailOrUserIDTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#externalForm > form > div:nth-child(1) > div.span4.externalWell > div.well > div:nth-child(2) > label"));
            }
        }

        public HpgElement UserNameTxtField
        {
            get
            {
                return new HpgElement(browser.FindId("UserName"));
            }
        }

        public HpgElement PasswordTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#externalForm > form > div:nth-child(1) > div.span4.externalWell > div.well > div.control-group.marginBottom0 > label"));
            }
        }

        public HpgElement ForgotPasswordLnk
        {
            get
            {
                return new HpgElement(browser.FindCss("#externalForm > form > div:nth-child(1) > div.span4.externalWell > div.well > div.control-group.marginBottom0 > label > small > a"));
            }
        }

        public HpgElement PasswordTxtField
        {
            get
            {
                return new HpgElement(browser.FindId("Password"));
            }
        }

        public HpgElement RememberEmailOrUserIDCheckBox
        {
            get
            {
                return new HpgElement(browser.FindId("rememberMe"));
            }
        }

        public HpgElement RememberEmailOrUserIDTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#externalForm > form > div:nth-child(1) > div.span4.externalWell > div.well > div:nth-child(4) > label > span"));
            }
        }

        public HpgElement loginButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Login"));
            }
        }

        public HpgElement Footer
        {
            get
            {
                return new HpgElement(browser.FindCss("#content > div:nth-child(3)"));
            }
        }

        #endregion

        #region Actions

        public void LoginAs(string username, string password = "")
        {
            if (!browser.HasDialog(""))
            {
                browser.Visit("/Home/Contact");
            }
            DateTime exitTime = DateTime.Now.AddMinutes(15);
            while (DateTime.Now <= exitTime)
            {
                if (browser.HasDialog(""))
                {
                    SendKeys.SendWait(username);
                    System.Threading.Thread.Sleep(2000);
                    SendKeys.Send("[TAB]");
                    System.Threading.Thread.Sleep(2000);
                    SendKeys.Send(password);
                    System.Threading.Thread.Sleep(2000);
                    SendKeys.Send("[ENTER]");
                }
                if (!browser.HasDialog(""))
                {
                    break;
                }
            }
            HpgAssert.False(browser.HasDialog(""));
        }

        #endregion

    }
}
