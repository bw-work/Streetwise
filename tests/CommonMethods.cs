using NUnit.Framework;
using AutomationCore;
using Coypu;
using Streetwise.page_objects;
using System;
using OpenQA.Selenium;
using AutomationCore.utility;
using AutomationCore.input_objects;
using System.Collections.Generic;
using System.Linq;

namespace Streetwise.tests
{
    public abstract class CommonMethods : SuperTest
    {
        private IEnumerable<InputObject> TestData;

        //NOTE - change BaseURL to QA environment once access is granted
        public void BaseSetup(string Spreadsheet, string BaseURL = "streetwise.healthtrustpg.com", string Sheet = "Sheet1")
        {
            SessConfiguration = new SessionConfiguration()
            {
                Browser = Coypu.Drivers.Browser.Chrome,
                AppHost = BaseURL
            };
            Browser = new BrowserSession(SessConfiguration);
            Browser.MaximiseWindow();
            Browser.Visit("");
            SessConfiguration.Match = Match.First;
            TestData = FileReader.getInputObjects(Spreadsheet, Sheet);
        }

        public void BaseTearDown()
        {
            ((BrowserSession)CurrentBrowser).Dispose();
        }

        public IEnumerable<InputObject> getTestData()
        {
            return TestData;
        }

        public void Login()
        {
            swLogin login = new swLogin(Browser);
            login.UserNameTxtField.Type(getTestData().ElementAt(0).fields["Username"]);
            login.PasswordTxtField.Type(getTestData().ElementAt(0).fields["Password"]);
            login.RememberEmailOrUserIDCheckBox.Click();
            login.loginButton.Click();
        }
    }
}