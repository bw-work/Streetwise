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
    class imLogin : swMaster
    {
        public imLogin(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        #region Objects

        public HpgElement loginDropDown
        {
            get
            {
                return new HpgElement(browser.FindId("UserId"));
            }
        }

        public HpgElement loginButton
        {
            get
            {
                return  new HpgElement(browser.FindButton("Login"));
            }
        }



        #endregion

        #region Actions

        public void HCATestLoginAs(string username)
        {
            browser.Visit("/User/LogOff");
            System.Threading.Thread.Sleep(2000);
            browser.Visit("/User/Login");
            loginDropDown.Click();
            loginDropDown.Element.SendKeys(username);
            loginDropDown.Element.FindXPath(".//*[contains(.,'" + username + "')]").Click();
            browser.Select(username).From("UserId");
            loginButton.Click();
            HpgAssert.True(browser.FindXPath("//div[@id='content']//div[@class='stripeInner' and contains(.,'Welcome " + username + "')]").Exists(), "Verify user is logged in");
        }

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
