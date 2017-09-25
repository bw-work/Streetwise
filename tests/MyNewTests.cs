using NUnit.Framework;
using AutomationCore;
using Coypu;
using Streetwise.page_objects;
using System;
using OpenQA.Selenium;

namespace Streetwise.tests
{
    [TestFixture]
    public class MyNewTests : SuperTest
    {
        [TestCase(@"executeautomation.com/demosite/Login.html")]
        public void ThisIsMyNewTest(string BaseURL)
        {
            SessConfiguration.AppHost = BaseURL;
            Browser = new BrowserSession(SessConfiguration);
            Browser.MaximiseWindow();
            Browser.Visit("");
            MyNewTestsPageObject myNewTestsPageObject = new MyNewTestsPageObject(Browser);

            AssertPageElements(myNewTestsPageObject);
            Login(myNewTestsPageObject);
        }

        public void AssertPageElements(MyNewTestsPageObject myNewTestsPageObject)
        {
            Assert.AreEqual(myNewTestsPageObject.LoginLink.Text, "LOGIN");
            Assert.AreEqual(myNewTestsPageObject.PageTitle.Text, "Execute Automation Selenium Test Site");
            Assert.AreEqual(myNewTestsPageObject.LoginTitle.Text, "Login");
            Assert.AreEqual(myNewTestsPageObject.UserNameLbl.Text, "UserName  ");
            Assert.AreEqual(myNewTestsPageObject.PasswordLbl.Text, "Password    ");
            Assert.NotNull(myNewTestsPageObject.UserNameTxtFld);
            Assert.NotNull(myNewTestsPageObject.PasswordTxtFld);
            Assert.NotNull(myNewTestsPageObject.LoginBtn);
        }

        public void Login(MyNewTestsPageObject myNewTestsPageObject)
        {
            myNewTestsPageObject.InputUserName("Ben");
            myNewTestsPageObject.InputPassword("1234abc");
            myNewTestsPageObject.ClickLoginBtn();
        }

        [SetUp]
        public void SetUp()
        {
            SessConfiguration = new SessionConfiguration()
            {
                Browser = Coypu.Drivers.Browser.Chrome
            };
            //DisposeBrowsers();
            SessConfiguration.Match = Match.First;
        }

        [TearDown]
        public void TearDown()
        {
            ((BrowserSession)CurrentBrowser).Dispose();
        }
    }
}