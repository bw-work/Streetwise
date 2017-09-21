using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Coypu;
using AutomationCore.utility;
using IdeaManagement.Utility;

namespace IdeaManagement.page_objects
{
    class imMaster
    {
        public BrowserSession browser;
        public imMaster(BrowserSession browser)
        {
            this.browser = browser;
        }

        public OpenQA.Selenium.Remote.RemoteWebDriver remoteWebDriver
        {
            get { return (OpenQA.Selenium.Remote.RemoteWebDriver) browser.Native; }
        }

        #region Objects

        public HpgElement masterHeader
        {
            get
            {
                return new HpgElement(browser.FindXPath("//div[@class='pageHeader' and .//a[@class='logo']]"));
            }
        }

        public HpgElement pageHeader
        {
            get
            {
                return new HpgElement(browser.FindId("pageTitle"));
            }
        }

        public HpgElement TabSubmitAnIdea
        {
            get
            {
                return new HpgElement(browser.FindId("SubmitIdea"));
            }
        }

        public HpgElement TabMyIdeas
        {
            get
            {
                return new HpgElement(browser.FindId("GetMyIdeas"));
            }
        }

        public HpgElement TabAllIdeas
        {
            get
            {
                return new HpgElement(browser.FindId("GetAllIdeas"));
            }
        }


        public HpgElement TabPublishedIdeas
        {
            get
            {
                return new HpgElement(browser.FindId("PublishedIdea"));
            }
        }

        public HpgElement HeaderLoginLink
        {
            get
            {
                return new HpgElement(browser.FindXPath("//div[@class='stripe']//a[.='Login']"));
            }
        }

        public HpgElement AdminHeaderMenu
        {
            get
            {
                return new HpgElement(browser.FindId("menu2"));
            }
        }

        public HpgElement SearchFilterForm
        {
            get
            {
                return new HpgElement(browser.FindXPath("//input[@placeholder='Idea Number/Name/Description']"));
            }
        }

        public HpgElement SearchFilterButton
        {
            get
            {
                return new HpgElement(browser.FindXPath("//button[@value='Search']"));
            }
        }

        public HpgElement HomeButton
        {
            get
            {
                return new HpgElement(browser.FindId("homeMenuTab"));
            }
        }

        public HpgElement HeaderUserInfo
        {
            get
            {
                return new HpgElement(browser.FindXPath("//*[@id='content']//div[@class='stripe']/div[@class='container']/div[contains(.,'Welcome ')]"));
            }
        }

        public HpgElement HeaderRegisterLink
        {
            get
            {
                return new HpgElement(browser.FindXPath("//*[@id='content']//div[@class='stripe']/div[@class='container']//a[.='Register']"));
            }
        }

        public HpgElement pageFooter
        {
            get
            {
                return new HpgElement(browser.FindXPath("//div[@class='footer']"));
            }
        }
        
        #endregion

        #region Actions

        public void CheckRetry(HpgElement checkbox, int retryTimes = 3, int SecondsBetween = 1, bool assertion = true)
        {
            for (int i = 0; i < retryTimes; i++)
            {
                try
                {
                    AutomationCore.base_tests.BaseTest.WriteReport("Checking box (#" + (i + 1).ToString() + ")");                    
                    checkbox.Element.Now();
                    checkbox.Element.Hover();
                    checkbox.Element.Check();
                    if (checkbox.Element.Selected) break;
                    checkbox.Element.SendKeys(OpenQA.Selenium.Keys.Space);
                    if (checkbox.Element.Selected) break;
                    System.Threading.Thread.Sleep(SecondsBetween * 1000);
                }
                catch (Exception e)
                {
                    AutomationCore.base_tests.BaseTest.WriteReport("Error checking box - " + e.Message);
                }
            }
            if (assertion) HpgAssert.True(checkbox.Element.Selected, "Checkbox is checked");
        }

        public string AZCharOnly(string input)
        {
            return Regex.Replace(input, "[^a-zA-Z0-9]+", "");
        }

        public void Refresh()
        {
            try
            {
                ((OpenQA.Selenium.Remote.RemoteWebDriver)browser.Native).Navigate().Refresh();
            }
            catch (Exception e)
            {
                AutomationCore.base_tests.BaseTest.WriteReport("Error during browser refresh! " + e.Message);
            }
        }

        public void GotoHomePage()
        {
            browser.Visit("/");
            WaitForThrobber();
            //HpgAssert.Contains(pageHeader.Text, "Streetwise", "Verify Home Page is loaded");
            AutomationCore.base_tests.BaseTest.WriteReport("Navigated to Home Page");
        }

        public void GotoDashboard()
        {
            browser.Visit("/Dashboard");
            WaitForThrobber(600);
            HpgAssert.Contains(pageHeader.Text, "My Dashboard", "Verify Dashboard is loaded");
            AutomationCore.base_tests.BaseTest.WriteReport("Navigated to Dashboard");
        }

        public void GotoSubmitAnIdea()
        {
            TabSubmitAnIdea.Click();
            //HpgAssert.Contains(pageHeader.Text, "Submit an Idea", "Verify 'Submit Idea' page is loaded");
            WaitForThrobber();
            HpgAssert.True(browser.FindId("ideaSubmit").Exists(), "Verify 'Submit Idea' page is loaded");
            AutomationCore.base_tests.BaseTest.WriteReport("Navigated to Submit Idea page");
        }

        public void ClickAdminHeaderMenu()
        {
            AdminHeaderMenu.Element.FindLink("Admin").Hover();
            AdminHeaderMenu.Element.FindLink("Admin").Click();
            HpgAssert.True(AdminHeaderMenu.Element.FindXPath(".//li").Exists(), "Verify Admin Header Menu is displayed");
        }

        public void GotoResultTypes()
        {
            browser.Visit("/ResultType");
            WaitForThrobber();
            HpgAssert.Contains(pageHeader.Text, "Result Types", "Verify 'Result Type' page is loaded");
        }

        public void GotoQuestions()
        {
            browser.Visit("/IdeaQuestion");
            WaitForThrobber();
            HpgAssert.Contains(pageHeader.Text, "Questions", "Verify 'Questions' page is loaded");
        }

        public void GotoCategories()
        {
            browser.Visit("/Category");
            WaitForThrobber();
            HpgAssert.Contains(pageHeader.Text, "Categories", "Verify 'Categories' page is loaded");
        }

        public void GotoDepartments()
        {
            browser.Visit("/Department");
            WaitForThrobber();
            HpgAssert.Contains(pageHeader.Text, "Departments", "Verify 'Departments' page is loaded");
        }

        public void GotoFacilitySavings()
        {
            browser.Visit("/Facility/FacilitySavings");
            WaitForThrobber();
            HpgAssert.Contains(pageHeader.Text, "Facility Savings", "Verify 'Savings' page is loaded");
        }

        public void GotoPublishedIdeas(bool showAll = true)
        {
            browser.Visit("/QualifiedIdea");
            WaitForThrobber(90);
            HpgAssert.Contains(pageHeader.Text, "Published Ideas", "Verify 'Published Ideas' page is loaded");
        }

        public void GoToPublishedIdeaNumber(string ideaNumber)
        {
            SendKeys.SendWait("{ESC}");
            SendKeys.SendWait("{ESC}");
            System.Threading.Thread.Sleep(2000);
            browser.Now();
            //browser.Visit("/Idea/Published/" + ideaNumber);
            browser.Visit("QualifiedIdea/Details/" + ideaNumber);
            browser.FindId("QualifiedIdea_IdeaId").Now();
            //HpgAssert.True(browser.FindXPath(".//*[@id='content']/div[4]/div/div[2]/div[1]/div/div/div[1]/h2").Text.Trim().StartsWith(ideaNumber), "Verify Published Idea Detail Page is displayed");
            HpgAssert.True(browser.FindId("QualifiedIdea_IdeaId").Text.Trim().Equals(ideaNumber), "Verify Published Idea Detail Page is displayed");
            AutomationCore.base_tests.BaseTest.WriteReport("Navigated to Published Idea " + ideaNumber);
        }

        public void GotoMyIdeas()
        {
            //TabMyIdeas.Element.Hover();
            //TabMyIdeas.Click();
            browser.Visit("/Idea/GetMyIdeas");
            WaitForThrobber();
            HpgAssert.Contains(pageHeader.Text, "My Ideas", "Verify 'My Ideas' page is loaded");
        }

        public void GotoAllIdeas()
        {
            //TabAllIdeas.Click();
            browser.Visit("/Idea/GetAllIdeas");
            //TODO: Change to an ID after the pages have been updated with IDs
            WaitForThrobber(240);
            HpgAssert.Contains(pageHeader.Text, "All Ideas", "Verify 'All Ideas' page is loaded");
        }

        public void GotoHomePageCMS()
        {
            browser.Visit("/Admin/CMS");
            HpgAssert.Contains(pageHeader.Text, "CMS Home Page", "Verify 'Edit Home Page' page is loaded");
        }

        public void GotoNewStreetwiseIdeas()
        {
            browser.Visit("/StreetwiseImportedIdea");
            WaitForThrobber();
            HpgAssert.Contains(pageHeader.Text, "Streetwise Imported Ideas", "Verify 'Streetwise Imported Ideas' page is loaded");
        }

        public void WaitForThrobber(int secondsToWait = 60)
        {
            try
            {
                browser.FindId("throbberContainer", new Options() { Timeout = TimeSpan.FromSeconds(5) }).Now(); //Give it 5 seconds to show up first
            }
            catch (Exception)
            {
            }
            var timeEnd = DateTime.Now.AddSeconds(secondsToWait);
            while (DateTime.Now < timeEnd)
            {
                try
                {
                    if (browser.FindId("throbberContainer").Missing(new Options() { Timeout = TimeSpan.FromSeconds(secondsToWait) })) break;
                }
                catch (Exception)
                {
                }
                System.Threading.Thread.Sleep(1000);
            }
        }

        public string FillFormField(string fieldID, string fieldValue)
        {
            HpgElement field = new HpgElement(browser.FindId(fieldID));
            return FillFormField(field, fieldValue);
        }
        
        public string FillFormField(HpgElement field, string fieldValue)
        {
            //HpgAssert.Exists(field, "Verify field '" + field.Element.Id + "' exists");
            string originalValue = "";
            string elementType = field.Element["outerHTML"];
            elementType = elementType.Substring(1, elementType.IndexOf(' ')).Trim().ToLower();
            switch (elementType)
            {
                case "input":
                    //Text, Check, or Radio
                    switch (field.Element["type"].ToLower())
                    {
                        case "text":
                            //text box
                            originalValue = field.Text;
                            field.Type(fieldValue);
                            break;
                        case "checkbox":
                            //check box
                            if (field.Element.Selected)
                            {
                                originalValue = "TRUE";
                            }
                            if (fieldValue.Trim().Equals(""))
                            {
                                field.Element.Click();
                                field.UnCheck();
                            }
                            else
                            {
                                field.Element.Click();
                                field.Check();
                            }
                            break;
                        case "radio":
                            //radio button
                            ElementScope originalSelection = browser.FindXPath("//input[@name='" + field.Element.Id + "' and @checked='true']");
                            if (originalSelection.Exists())
                            {
                                originalValue = originalSelection.Value;
                            }
                            field.Element.Click();
                            browser.Choose(field.Element.Id);
                            break;
                    }
                    break;
                case "select":
                    //Drop-Down
                    originalValue = field.Element.SelectedOption;
                    field.Click();
                    field.Element.SendKeys(fieldValue);
                    field.Element.FindXPath(".//*[.='" + fieldValue + "']").Click();
                    browser.Select(fieldValue).From(field.Element.Id);
                    break;
                case "textarea":
                    //Large text area
                    originalValue = field.Text;
                    field.Type(fieldValue);
                    break;
            }
            return originalValue;
        }

        public static void SetCheckBox(HpgElement box, bool check)
        {
            if (box.Element.Selected != check) //Only perform action if the box is currently different
                if (check)
                {
                    box.Check();
                }
                else
                {
                    box.UnCheck();
                }
        }

        public void ScrollToBottom(int secondsToTry = 90)
        {
            DateTime exitTime = DateTime.Now.AddSeconds(secondsToTry);
            do
            {
                pageFooter.Element.Hover();
                pageFooter.Element.FindXPath("//a[@title='HealthTrust']").SendKeys(OpenQA.Selenium.Keys.End);
                pageFooter.Element.FindXPath("//a[@title='HealthTrust']").SendKeys(OpenQA.Selenium.Keys.End);
                if (DateTime.Now > exitTime) break;
                System.Threading.Thread.Sleep(500);
            } while (!bool.Parse(browser.ExecuteScript("return $(document).height() == ($(window).height() + $(window).scrollTop());").ToString()));
            pageFooter.Element.FindXPath("//a[@title='HealthTrust']").SendKeys(OpenQA.Selenium.Keys.End);
            pageFooter.Element.FindXPath("//a[@title='HealthTrust']").SendKeys(OpenQA.Selenium.Keys.End);
            System.Threading.Thread.Sleep(2000);
            pageFooter.Element.FindXPath("//a[@title='HealthTrust']").SendKeys(OpenQA.Selenium.Keys.Home);
        }

        public void SearchFor(string q)
        {
            SearchFilterForm.Type(q);
            SearchFilterButton.Click(2);
            WaitForThrobber();
        }

        public string SelectOptionOnDropDown(HpgElement dd, string option)
        {
            dd.Click();
            HpgElement op = new HpgElement(dd.Element.FindXPath(".//Option[.='" + option + "']"));
            dd.Element.SendKeys(op.Text.Trim());
            op.Click();
            browser.Select(op.Text.Trim()).From(dd.Element.Id);
            return op.Value;
        }

        public string SelectLastOptionOnDropDown(HpgElement dd)
        {
            dd.Click();
            var options = (from o in dd.Element.FindAllXPath(".//Option") where !o.Text.Trim().Equals("") select o);
            dd.Element.SendKeys(options.Last().Text.Trim());
            options.Last().Click();
            browser.Select(options.Last().Text.Trim()).From(dd.Element.Id);
            return options.Last().Text;
        }

        public string SelectFirstOptionOnDropDown(HpgElement dd)
        {
            dd.Click();
            var options = (from o in dd.Element.FindAllXPath(".//Option") where !o.Text.Trim().Equals("") select o);
            dd.Element.SendKeys(options.First().Text.Trim());
            options.First().Click();
            browser.Select(options.First().Text.Trim()).From(dd.Element.Id);
            return options.First().Text;
        }

        public void GoToIdeaNumber(string ideaNumber)
        {
            browser.Visit("/Idea/Details/" + ideaNumber);
            System.Threading.Thread.Sleep(10000);
            HpgAssert.AreEqual(ideaNumber, browser.FindXPath("//div[@id='StandardDetails']/h3[.='Idea Number']/following-sibling::p[1]").Text.Trim(), "Verify Idea Detail Page is displayed");
            AutomationCore.base_tests.BaseTest.WriteReport("Navigated to Idea " + ideaNumber);
        }

        public void GoToPackageIdea(int ideaNumber)
        {
            browser.Visit("/QualifiedIdea/Create/" + ideaNumber.ToString());
            System.Threading.Thread.Sleep(10000);
            HpgAssert.AreEqual(ideaNumber.ToString(), browser.FindId("Idea_IdeaId").Text.Trim(), "Verify Package Idea Page is displayed");
            AutomationCore.base_tests.BaseTest.WriteReport("Navigated to Idea " + ideaNumber);       
        }

        public bool longClickElement(Coypu.ElementScope clickThis, int waitSeconds = 30, int retrySeconds = 5)
        {
            DateTime quitTime = DateTime.Now.AddSeconds(waitSeconds);
            while (DateTime.Now <= quitTime)
            {
                try
                {
                    clickThis.Click();
                    clickThis.Now();
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(retrySeconds * 1000);
                }
            }
            return false;
        }

        public void IESaveResource(string url, string filename)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                client.Headers.Clear();
                client.Proxy = new System.Net.WebProxy("localhost:8878", false);
                client.BaseAddress = url;
                //foreach (KeyValuePair<string, string> currentRequestHeader in AutomationCore.base_tests.BaseTest.CurrentRequestHeaders)
                //{
                //    client.Headers.Add(currentRequestHeader.Key, currentRequestHeader.Value);
                //}
                client.Headers.Remove("Proxy-Connection");
                client.DownloadFile(url, filename);
            }
        }

        #endregion
    }
}
