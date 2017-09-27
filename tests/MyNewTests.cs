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
    [TestFixture]
    public class MyNewTests : CommonMethods
    {
        string BaseURL = @"executeautomation.com/demosite/Login.html";
        string DataFile = @"test_specific\MyNewTests.xls";

        [Test]
        public void ThisIsMyNewTest()
        {
            ExecuteAutomationLogin executeAutomationLogin = new ExecuteAutomationLogin(Browser);

            AssertPageElements(executeAutomationLogin);
            Login(executeAutomationLogin);
        }

        public void AssertPageElements(ExecuteAutomationLogin executeAutomationLogin)
        {
            HpgAssert.AreEqual(executeAutomationLogin.LoginLink.Text, "LOGIN");
            HpgAssert.AreEqual(executeAutomationLogin.PageTitle.Text, "Execute Automation Selenium Test Site");
            HpgAssert.AreEqual(executeAutomationLogin.LoginTitle.Text, "Login");
            HpgAssert.AreEqual(executeAutomationLogin.UserNameLbl.Text, "UserName  ");
            HpgAssert.AreEqual(executeAutomationLogin.PasswordLbl.Text, "Password    ");
            HpgAssert.Exists(executeAutomationLogin.UserNameTxtFld);
            HpgAssert.Exists(executeAutomationLogin.PasswordTxtFld);
            HpgAssert.Exists(executeAutomationLogin.LoginBtn);
        }

        public void Login(ExecuteAutomationLogin executeAutomationLogin)
        {
            executeAutomationLogin.InputUserName(getTestData().ElementAt(1).fields["Username"]);
            executeAutomationLogin.InputPassword(getTestData().ElementAt(1).fields["Password"]);
            executeAutomationLogin.ClickLoginBtn();
        }

        [SetUp]
        public void SetUp()
        {
            BaseSetup(BaseURL, DataFile);
        }

        [TearDown]
        public void TearDown()
        {
            BaseTearDown();
        }
    }
}