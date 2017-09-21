using NUnit.Framework;
using AutomationCore;
using Coypu;
using IdeaManagement.page_objects;
using System;

namespace IdeaManagement.tests
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

            Login();
        }

        public void Login()
        {
            Browser.FillIn("UserName").With("Ben");
            Browser.FillIn("Password").With("12345");
            Browser.ClickButton("Login");
        }

        [SetUp]
        public void SetUp()
        {
            SessConfiguration = new SessionConfiguration()
            {
                Browser = Coypu.Drivers.Browser.Chrome,
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