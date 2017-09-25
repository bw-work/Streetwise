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
using OpenQA.Selenium.Chrome;

namespace Streetwise.page_objects
{
    public class MyNewTestsPageObject 
    {
        BrowserSession browser;

        public MyNewTestsPageObject(BrowserSession currentBrowser)
        {
            this.browser = currentBrowser;
        }

        public OpenQA.Selenium.Remote.RemoteWebDriver remoteWebDriver
        {
            get { return (OpenQA.Selenium.Remote.RemoteWebDriver)browser.Native; }
        }

        public HpgElement LoginLink
        {
            get
            {
                return new HpgElement(browser.FindCss("#cssmenu > ul > li > a > span"));
            }
        }

        public HpgElement PageTitle
        {
            get
            {
                return new HpgElement(browser.FindCss("body > h1"));
            }
        }

        public HpgElement LoginTitle
        {
            get
            {
                return new HpgElement(browser.FindCss("body > h2"));
            }
        }

        public HpgElement UserNameLbl
        {
            get
            {
                return new HpgElement(browser.FindCss("#userName > p:nth-child(1)"));
            }
        }

        public HpgElement UserNameTxtFld
        {
            get
            {
                return new HpgElement(browser.FindField("UserName"));
            }
        }

        public HpgElement PasswordLbl
        {
            get
            {
                return new HpgElement(browser.FindCss("#userName > p:nth-child(2)"));
            }
        }

        public HpgElement PasswordTxtFld
        {
            get
            {
                return new HpgElement(browser.FindField("Password"));
            }
        }

        public HpgElement LoginBtn
        {
            get
            {
                return new HpgElement(browser.FindButton("Login"));
            }
        }

        public void InputUserName(string username)
        {
            UserNameTxtFld.Type(username);
        }

        public void InputPassword(string password)
        {
            PasswordTxtFld.Type(password);
        }

        public void ClickLoginBtn()
        {
            LoginBtn.Click();
        }
    }
}
