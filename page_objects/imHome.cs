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
    class imHome : imMaster
    {
        public imHome(BrowserSession currentBrowser) : base(currentBrowser)
        {
        }

        #region Objects
        
        public enum Role
        {
            Admin,
            SME,
            Boss,
            Standard,
            ReadOnly, 
            FacilityApprover,
            FacilitySavings
        }

        private readonly string[] roles = new string[] { "Admin", "SME", "DCRD", "Standard", "Read Only", "Facility Approver", "Facility Savings" };

        public HpgElement RoleDropDown
        {
            get
            {
                return new HpgElement(browser.FindId("role"));
            }
        }
        
        #endregion

        #region Actions

        public void SelectRole(Role role, bool inPlace=true)
        {
            if(!inPlace) GotoHomePage();
            if (!RoleDropDown.Element.SelectedOption.Equals(roles[(int)role]))
                RoleDropDown.SelectListOptionByText(roles[(int)role]);
            AutomationCore.base_tests.BaseTest.WriteReport("Selected role " + roles[(int)role]);
            System.Threading.Thread.Sleep(2000);
            WaitForThrobber(360);
        }

        public void goHomeTest()
        {
            SuperTest.SessConfiguration.AppHost = "http://hcatest.xpxcloud.com/";
            browser.Visit("http://hcatest.xpxcloud.com/");
            HpgAssert.Contains(pageHeader.Text, "Home Page", "Verify 'Home Page' is loaded");
        }

        public void goHomeDev()
        {
            SuperTest.SessConfiguration.AppHost = "http://dev-im.healthtrustpg.com";
            browser.Visit("/");
            HpgAssert.Contains(pageHeader.Text, "Home Page", "Verify 'Home Page' is loaded");
        }

        public void goHomeProd()
        {
            SuperTest.SessConfiguration.AppHost = "http://streetwise.healthtrustpg.com/";
            browser.Visit("/");
            HpgAssert.Contains(pageHeader.Text, "Home Page", "Verify 'Home Page' is loaded");
        }

        public void loginIdeaManagement(string baseURL = "http://sbx-im.healthtrustpg.com", string username = "")
        {
            loginIdeaManagement(baseURL, username, AutomationCore.base_tests.BaseTest.GetPasswordForUser(username));
        }

        public void loginIdeaManagement(string baseURL = "http://sbx-im.healthtrustpg.com", string username = "", string password = "")
        {
            AutomationCore.base_tests.BaseTest.WriteReport("Loggin into " + baseURL + "...");
            OpenQA.Selenium.Remote.RemoteWebDriver rwd = ((OpenQA.Selenium.Remote.RemoteWebDriver)browser.Native);
            rwd.Manage().Cookies.DeleteAllCookies();
            SuperTest.SessConfiguration.AppHost = baseURL;
            rwd.Manage().Window.Size = new Size(800, 600);
            System.Threading.Thread.Sleep(2000);
            rwd.Manage().Window.Maximize();
            if (!string.IsNullOrEmpty(username))
            {
                browser.Visit(baseURL + "/Account/Login");
                if (rwd.Capabilities.BrowserName.ToLower().Contains("internet"))
                {
                    DateTime exitTime = DateTime.Now.AddMinutes(15);
                    while (DateTime.Now <= exitTime)
                    {
                        //if (browser.HasDialog(""))
                        if (true)
                        {
                            SuperTest.WriteReport("Enter credentials into " + rwd.Capabilities.BrowserName);
                            SendKeys.SendWait(username);
                            System.Threading.Thread.Sleep(2000);
                            SendKeys.SendWait("{TAB}");
                            System.Threading.Thread.Sleep(2000);
                            if (Control.IsKeyLocked(Keys.CapsLock))
                            {
                                SendKeys.SendWait("{CAPSLOCK}" + password);
                            }
                            else
                            {
                                SendKeys.SendWait(password);
                            }
                            System.Threading.Thread.Sleep(2000);
                            SendKeys.SendWait("{ENTER}");
                        }
                        if (!browser.HasDialog(""))
                        {
                            break;
                        }
                    }
                }
                else
                {
                    SuperTest.WriteReport("Enter credentials into " + rwd.Capabilities.BrowserName);
                    System.Threading.Thread.Sleep(10000);
                    SendKeys.SendWait(username);
                    System.Threading.Thread.Sleep(2000);
                    SendKeys.SendWait("{TAB}");
                    System.Threading.Thread.Sleep(2000);
                    if (Control.IsKeyLocked(Keys.CapsLock))
                    {
                        SendKeys.SendWait("{CAPSLOCK}" + password);
                    }
                    else
                    {
                        SendKeys.SendWait(password);
                    }
                    System.Threading.Thread.Sleep(2000);
                    SendKeys.SendWait("{ENTER}");
                }
                try
                {
                    HpgAssert.True(browser.HasNoDialog(""), "Verify no dialog is present");
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(60000);                    
                }
                HpgAssert.True(browser.HasNoDialog(""), "Verify no dialog is present");
            }
            //HpgAssert.Contains(pageHeader.Text, "Home Page", "Verify 'Home Page' is loaded");
            //TODO: Determine if home page header text is coming back or not.
            HpgAssert.True(browser.FindXPath("//a[@class='logo']/img").Exists(), "Verify page is loaded");
        }

        public void goHomeHCADev(string username, string password)
        {
            AutomationCore.base_tests.BaseTest.WriteReport("Loggin into sbx-im.healthtrustpg.com...");
            OpenQA.Selenium.Remote.RemoteWebDriver rwd = ((OpenQA.Selenium.Remote.RemoteWebDriver)browser.Native);
            rwd.Manage().Cookies.DeleteAllCookies();
            SuperTest.SessConfiguration.AppHost = "http://sbx-im.healthtrustpg.com";
            rwd.Manage().Window.Size = new Size(800, 600);
            rwd.Manage().Window.Maximize();
            browser.Visit("http://sbx-im.healthtrustpg.com/Account/Login");
            if (rwd.Capabilities.BrowserName.ToLower().Contains("internet"))
            {
                DateTime exitTime = DateTime.Now.AddMinutes(15);
                while (DateTime.Now <= exitTime)
                {
                    //if (browser.HasDialog(""))
                    if (true)
                    {
                        SuperTest.WriteReport("Enter credentials");
                        SendKeys.SendWait(username);
                        System.Threading.Thread.Sleep(2000);
                        SendKeys.SendWait("{TAB}");
                        System.Threading.Thread.Sleep(2000);
                        SendKeys.SendWait(password);
                        System.Threading.Thread.Sleep(2000);
                        SendKeys.SendWait("{ENTER}");
                    }
                    if (!browser.HasDialog(""))
                    {
                        break;
                    }
                }
            }
            else
            {
                SuperTest.WriteReport("Enter credentials");
                System.Threading.Thread.Sleep(10000);
                SendKeys.SendWait(username);
                System.Threading.Thread.Sleep(2000);
                SendKeys.SendWait("{TAB}");
                System.Threading.Thread.Sleep(2000);
                SendKeys.SendWait(password);
                System.Threading.Thread.Sleep(2000);
                SendKeys.SendWait("{ENTER}");
            }
            HpgAssert.False(browser.HasDialog(""), "Verify no dialog is present");
            HpgAssert.Contains(pageHeader.Text, "Home Page", "Verify 'Home Page' is loaded");
        }

        public void goHomeHCADev(bool useCredentials)
        {
            AutomationCore.base_tests.BaseTest.WriteReport("Loggin into sbx-im.healthtrustpg.com...");
            OpenQA.Selenium.Remote.RemoteWebDriver rwd = ((OpenQA.Selenium.Remote.RemoteWebDriver)browser.Native);
            rwd.Manage().Cookies.DeleteAllCookies();
            SuperTest.SessConfiguration.AppHost = "http://sbx-im.healthtrustpg.com";
            rwd.Manage().Window.Size = new Size(800, 600);
            rwd.Manage().Window.Maximize();
            browser.Visit("http://sbx-im.healthtrustpg.com/");
            HeaderLoginLink.Click();
            HpgAssert.False(browser.HasDialog(""), "Verify no dialog is present");
            HpgAssert.Contains(pageHeader.Text, "Home Page", "Verify 'Home Page' is loaded");
        }

        #endregion

    }
}
