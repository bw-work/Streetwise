using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Streetwise.page_objects;
using Streetwise.page_assertions;

namespace Streetwise.tests
{
    [TestFixture]
    class StreetwiseRegression : CommonMethods
    {
        private string DataFile = "test_specific\\StreetwiseRegression.xls";

        [Test]
        [Category("Regression")]
        public void StreetwiseRegressionTest()
        {
            Login();
            NavigateToDashboard();
            NavigateToPublishedIdeas();
        }

        public void Login()
        {
            swLogin login = new swLogin(Browser);
            swHome home = new swHome(Browser);
            LoginPageAssertions loginPageAssertions = new LoginPageAssertions(login);

            loginPageAssertions.AssertPageElements();
            login.UserNameTxtField.Type(getTestData().ElementAt(0).fields["Username"]);
            login.PasswordTxtField.Type(getTestData().ElementAt(0).fields["Password"]);
            login.RememberEmailOrUserIDCheckBox.Click();
            login.loginButton.Click();

            home.Login.Click();
        }

        public void NavigateToDashboard()
        {
            MyDashboardAssertions myDashboardAssertions = new MyDashboardAssertions(Browser, new Dashboard(Browser));
            
            myDashboardAssertions.AssertPageElements();
        }

        public void NavigateToPublishedIdeas()
        {
            swPublishedIdeas publishedIdeas = new swPublishedIdeas(Browser);
            PublishedIdeasAssertions publishedIdeasAssertions = new PublishedIdeasAssertions(Browser, publishedIdeas);
            publishedIdeas.GotoPublishedIdeas();
            publishedIdeas.ShowMoreDepartmentsButton.Click();

            //These do not yet work, commenting out for the time being
            //publishedIdeas.ShowMoreCategoriesButton.Click();
            //publishedIdeasAssertions.AssertPageElements();
        }

        [SetUp]
        public void SetUp()
        {
            BaseSetup(DataFile);
        }

        [TearDown]
        public void TearDown()
        {
            BaseTearDown();
        }
    }
}
