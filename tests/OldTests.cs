using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using AutomationCore;
using AutomationCore.input_objects;
using AutomationCore.utility;
using Coypu.Drivers;
using IdeaManagement.Utility;
using IdeaManagement.page_objects;
using NUnit.Framework;
using Coypu;
using System.Data;
using AutomationCore.base_tests;
using System.Windows.Forms;
using OpenQA.Selenium.Remote;
using Match = Coypu.Match;

namespace IdeaManagement.tests
{
    [TestFixture]
    public class OldTests : SuperTest
    {
        private string BaseURL = "http://sbx-im.healthtrustpg.com";
        //private string BaseURL = "http://deploy-streetwise.healthtrustpg.com";
        //private string BaseURL = "http://dev-streetwise.healthtrustpg.com";
        //private string BaseURL = "http://jeremy-im.healthtrustpg.com";
        //private string BaseURL = "http://robert-im.healthtrustpg.com";
        public bool RallyUpload = false;
        Dictionary<string, string> linksList = new Dictionary<string, string>()
            {
                {"Fox News", @"http://www.foxnews.com" },
                {"CNN", @"http://www.cnn.com"},
                {"Xpanxion", @"http://www.xpanxion.com"},
                {"HealthTrust", @"http://www.healthtrustpg.com"}
            };
        private readonly string TestUserDomain = "nonaffildev";
        private readonly string StandardUser = "gpouserd532fa7c0db94";
        private readonly string DCRDUser = "gpouser9b0c2ee2a37c4";
        private readonly string SMEUser = "gpo-user-69af331cbd5";
        private readonly string AdminUser = "gpouser004cdb99fd124";
        private readonly string ABQUser = "gpo-user-22d8f9d6214";
        private readonly string AcadiaUser = "gpo-user-7e8ed18bb81";
        private readonly string AcumenUser = "gpo-user-549d0f025e3";
        private readonly string CentennialUser = "gpo-user-dac4f155327"; //HCA Centennial Heart at Skyline








        //private Dictionary<string, Browser> browsersToBeTested = new Dictionary<string, Browser>()
        //    {
        //        {"Internet Explorer", Coypu.Drivers.Browser.InternetExplorer},
        //        {"Firefox", Coypu.Drivers.Browser.Firefox},
        //        {"Chrome", Coypu.Drivers.Browser.Chrome}
        //    };

        private Dictionary<string, Browser> ffChromeOnly = new Dictionary<string, Browser>()
            {
                {"Chrome", Coypu.Drivers.Browser.Chrome},
                {"Firefox", Coypu.Drivers.Browser.Firefox}
            };

        private Dictionary<string, Browser> ffOnly = new Dictionary<string, Browser>()
            {
                {"Firefox", Coypu.Drivers.Browser.Firefox}
            };

        private Dictionary<string, int> impactEffort = new Dictionary<string, int>()
            {
                {"Low", 1},
                {"Medium", 2},
                {"High", 3}
            };

        public int stepNumber = 0;
            
        [Test][Ignore]
        public void Test_TC4286()
        {
            RallyTestID = "TC4286";
            RallyFailVerdict = Rally.Verdict.Blocked;
            HpgAssert.Fail("Currently unable to create new idea via UI");
            //US6019: Verify Save, Cancel, Submit idea on User Form
            //Create an idea and cancel it. Create an idea and save it. Create an idea and submit it. Verify the cancelled one does not show on "My Ideas". Verify only the submitted idea shows for admin.

            AutomationCore.utility.ews ews = new AutomationCore.utility.ews();
            ews.FilterContains.Clear();
            ews.FilterContains.Add(new KeyValuePair<string, string>("subject", "Idea Submitted Successfully"));
            ews.FilterContains.Add(new KeyValuePair<string, string>("from", "donotreply@xpanxion.com"));
            

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");
                ews.FilterGreaterThan.Clear();
                ews.FilterGreaterThan.Add(new KeyValuePair<string, object>("sent", DateTime.Now));

                //Step 1. Login as user
                //Expected Result: User Dashboard should be displayed
                page_objects.imHome IMHome =
                    new page_objects.imHome((BrowserSession) BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                //step 1 end

                //Step 2. Navigate to Submit Idea page (http://hcatest.xpxcloud.com/Idea/Create)
                //Expected Result: Submit Idea page should be displayed
                IMLogin.GotoSubmitAnIdea();
                page_objects.imSubmitAnIdea IMSubmitIdea = new page_objects.imSubmitAnIdea(IMLogin.browser);
                //step 2 end

                //Step 3. Verify Submit Idea form contains the following fields: Idea Name, Idea Description, Contact Name, Contact Phone, Contact email
                //Expected Result: Submit Idea form is displayed and contains the following fields: Idea Name, Idea Description, Contact Name, Contact Phone, Contact email
                // -- Achieved via Step 4
                //step 3 end

                //Step 4. Fill in Idea Name, Idea Description, Contact Name, Contact Email, Contact Phone and Select Cancel.
                //Expected Result: Idea will be not be saved to list of 'My Ideas'
                IMSubmitIdea.FillFormField("IdeaName", "Automation Idea Name CANCEL " + nameSuffix);
                IMSubmitIdea.FillFormField("Description", "Automation Description " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactName", "Automation Contact Name " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactEmail", "HPG.automation@HCAHealthcare.com");
                IMSubmitIdea.FillFormField("ContactPhone", "615-344-3000");
                IMSubmitIdea.SubmitCancelButton.Click();
                //step 4 end

                //Step 5. Select 'Submit an Idea' at the top of the page.
                //Expected Result: 'Submit an Idea' form should be displayed
                IMLogin.GotoSubmitAnIdea();
                //step 5 end

                //Step 6. Fill in Idea Name, Idea Description, Contact Name, Contact Email, Contact Phone and Select Save.
                //Expected Result: Idea will be saved to list of 'My Ideas' with proper information
                IMSubmitIdea.FillFormField("IdeaName", "Automation Idea Name SAVED " + nameSuffix);
                IMSubmitIdea.FillFormField("Description", "Automation Description " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactName", "Automation Contact Name " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactEmail", "HPG.automation@HCAHealthcare.com");
                IMSubmitIdea.FillFormField("ContactPhone", "615-344-3000");
                IMSubmitIdea.SubmitSaveButton.Click();
                //step 6 end

                //Step 7. Select 'Submit an Idea' at the top of the page.
                //Expected Result: 'Submit an Idea' form should be displayed
                IMLogin.GotoSubmitAnIdea();
                //step 7 end

                //Step 8. Fill in Idea Name, Idea Description, Contact Name, Contact Email, Contact Phone and Select Submit.
                //Expected Result: Idea will be saved to list of 'My Ideas' and email will be sent to Administrator.
                IMSubmitIdea.FillFormField("IdeaName", "Automation Idea Name SUBMITTED " + nameSuffix);
                IMSubmitIdea.FillFormField("Description", "Automation Description " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactName", "Automation Contact Name " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactEmail", "HPG.automation@HCAHealthcare.com");
                IMSubmitIdea.FillFormField("ContactPhone", "615-344-3000");
                ews.FilterGreaterThan.Clear();
                ews.FilterGreaterThan.Add(new KeyValuePair<string, object>("sent", DateTime.Now.AddSeconds(-15)));
                IMSubmitIdea.SubmitSubmitButton.Click();
                //check email
                //TODO: Verify email sent (uncomment below code)
                //DataTable msg = ews.GetMessagesDTWait(true, 2, 180);
                //HpgAssert.AreEqual("1", msg.Rows.Count.ToString(), "Verify single email was received");
                //ews.DeleteMessage(msg.Rows[0]["ID"].ToString());
                //step 8 end

                //Step 9. Navigate to 'My Ideas' page (http://hcatest.xpxcloud.com/Idea/All)
                //Expected Result: 'My Ideas' page is displayed
                IMHome.GotoMyIdeas();
                page_objects.imMyIdeas IMMyIdeas = new page_objects.imMyIdeas(IMHome.browser);
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                //step 9 end

                //Step 10. Verify cancelled idea is not displayed
                //Expected Result: Cancelled idea is not displayed
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'CANCEL')]").Missing(), "Verify CANCEL idea does not display on User");
                //step 10 end

                //Step 11. Verify saved idea is marked as 'saved'
                //Expected Result: Saved idea should be in 'Saved' status with proper information
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'SAVED')]").Exists(), "Verify SAVED idea is displayed on User");
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'SAVED')]").Click();
                page_objects.imIdeaDetails IMIdeaDetails = new page_objects.imIdeaDetails(IMMyIdeas.browser);
                HpgAssert.Contains(IMIdeaDetails.pageHeader.Text, "Idea Details", "Verify Idea Details page is loaded");
                HpgAssert.AreEqual("Automation Idea Name SAVED " + nameSuffix, IMIdeaDetails.IdeaName.Text.Trim(), "Verify Idea Name is correct");
                HpgAssert.AreEqual("Automation Description " + nameSuffix, IMIdeaDetails.IdeaDescription.Text.Trim(), "Verify Idea Description is correct");
                HpgAssert.AreEqual("Wes Phillips", IMIdeaDetails.IdeaAuthor.Text.Trim(), "Verify Idea Description is correct");
                //step 11 end

                //Step 12. Verify Submitted idea is marked as 'Submitted'
                //Expected Result: Submitted idea should be in 'Submitted' status with proper information
                IMHome.GotoMyIdeas();
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'SUBMITTED')]").Exists(), "Verify SUBMITTED idea is displayed on User");
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'SUBMITTED')]").Click();
                HpgAssert.Contains(IMIdeaDetails.pageHeader.Text, "Idea Details", "Verify Idea Details page is loaded");
                HpgAssert.AreEqual("Automation Idea Name SUBMITTED " + nameSuffix, IMIdeaDetails.IdeaName.Text.Trim(), "Verify Idea Name is correct");
                HpgAssert.AreEqual("Automation Description " + nameSuffix, IMIdeaDetails.IdeaDescription.Text.Trim(), "Verify Idea Description is correct");
                HpgAssert.AreEqual("Wes Phillips", IMIdeaDetails.IdeaAuthor.Text.Trim(), "Verify Idea Description is correct");
                //step 12 end

                //Step 13. Login as Admin
                //Expected Result: Admin Dashboard should be displayed
                //IMLogin.LoginAs("Automation-Admin");
                //step 13 end

                //Step 14. Verify submitted idea is displayed
                //Expected Result: submitted idea should be displayed with proper information
                IMHome.GotoAllIdeas();
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'CANCEL')]").Missing(), "Verify CANCEL idea does not display on Admin");
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'SAVED')]").Missing(), "Verify SAVED idea does not display on Admin");
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'SUBMITTED')]").Exists(), "Verify SUBMITTED idea is displayed on Admin");
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'SUBMITTED')]").Click();
                HpgAssert.Contains(IMIdeaDetails.pageHeader.Text, "Idea Details", "Verify Idea Details page is loaded");
                HpgAssert.AreEqual("Automation Idea Name SUBMITTED " + nameSuffix, IMIdeaDetails.IdeaName.Text.Trim(), "Verify Idea Name is correct");
                HpgAssert.AreEqual("Automation Description " + nameSuffix, IMIdeaDetails.IdeaDescription.Text.Trim(), "Verify Idea Description is correct");
                HpgAssert.AreEqual("Wes Phillips", IMIdeaDetails.IdeaAuthor.Text.Trim(), "Verify Idea Description is correct");
                //step 14 end

                //Step 15. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 15 end
            }
        }

        [Test][Ignore]
        public void Test_TC4287()
        {
            RallyTestID = "TC4287";
            RallyFailVerdict = Rally.Verdict.Blocked;
            HpgAssert.Fail("Currently unable to create new idea via UI");
            //US6019: Upon submission status will change to Submitted.
            //Create an idea and save it. Then Submit the idea and verify the idea status has changed.
            
            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");

                //Step 1. Navigate to http://hcatest.xpxcloud.com/ 
                //Expected Result: Healthtrust Homepage should be dispalyed 
                page_objects.imHome IMHome =
                    new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;

                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 1 end

                //Step 2. Login as User
                //Expected Result: User Dashboard is displayed
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                //IMLogin.LoginAs("Automation-S1");
                //step 2 end

                //Step 3. Select 'Submit an Idea' at the top of the page. 
                //Expected Result: 'Submit an Idea' form should be displayed 
                //((OpenQA.Selenium.Remote.RemoteWebDriver)IMHome.browser.Native).Manage().Cookies.DeleteAllCookies();
                
                IMLogin.GotoSubmitAnIdea();
                page_objects.imSubmitAnIdea IMSubmitIdea = new page_objects.imSubmitAnIdea(IMLogin.browser);
                //step 3 end

                //Step 4. Fill in Idea Name, Idea Description, Contact Name, Contact Email, Contact Phone and Select Save.
                //Expected Result: Idea should be saved 
                IMSubmitIdea.FillFormField("IdeaName", "Automation Idea Name SAVED " + nameSuffix);
                IMSubmitIdea.FillFormField("Description", "Idea saved, then submitted " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactName", "Automation Contact Name " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactEmail", "HPG.automation@HCAHealthcare.com");
                IMSubmitIdea.FillFormField("ContactPhone", "615-344-3000");
                IMSubmitIdea.SubmitSaveButton.Click();
                //step 4 end

                //Step 5. Navigate to 'My Ideas'
                //Expected Result: Idea should be marked as 'Saved'
                IMHome.GotoMyIdeas();
                page_objects.imMyIdeas IMMyIdeas = new page_objects.imMyIdeas(IMHome.browser);
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'SAVED')]").Exists(), "Verify SAVED idea is displayed on User");
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'SAVED')]").Click();
                page_objects.imIdeaDetails IMIdeaDetails = new page_objects.imIdeaDetails(IMMyIdeas.browser);
                //step 5 end

                //Step 6. Submit saved idea
                //Expected Result: Idea should be marked as 'Submitted'
                IMIdeaDetails.EditButton.Click();
                HpgAssert.Contains(IMIdeaDetails.pageHeader.Text, "Edit", "Verify Edit Idea page is loaded");
                IMSubmitIdea.SubmitSubmitButton.Click();
                IMHome.GotoMyIdeas();
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'SAVED')]").Exists(), "Verify SAVED idea is displayed on User");
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'SAVED')]").Click();
                HpgAssert.Contains(IMIdeaDetails.IdeaStatus.Text, "Submitted", "Verify status is Submitted");
                //step 6 end

                //Step 7. Verify email was sent notifying of status change
                //Expected Result: Email is sent notifying of status change 
                // -- Step has been deleted as per scoring session 04/09/2014
                //step 7 end

                //Step 8. Verify the same behavior in IE, FF, & Chrome
                //Expected Result: Behavior is the same
                // -- Achieved via foreach browser loop
                //step 8 end
            }
        }
        
        [Test][Ignore]
        public void Test_TC4301()
        {
            RallyTestID = "TC4301";
            RallyFailVerdict = Rally.Verdict.Blocked;
            HpgAssert.Fail("Currently unable to create new idea via UI");
            //US6044: Verify user can only view their own ideas
            //Create an idea from User1; Create an idea from User2; Verify User1 can not see User2 ideas; Verify User2 can not see User1 ideas; Verify fields; Verify delete idea function

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");

                //Step 1. Login as User2
                //Expected Result: User dashboard is displayed
                page_objects.imHome IMHome =
                    new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                IMHome.goHomeTest();
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMLogin.LoginAs("Automation-S2");
                //step 1 end

                //Step 2. Create idea and save
                //Expected Result: Idea is saved.
                IMLogin.GotoSubmitAnIdea();
                page_objects.imSubmitAnIdea IMSubmitIdea = new page_objects.imSubmitAnIdea(IMLogin.browser);
                IMSubmitIdea.FillFormField("IdeaName", "Automation Idea Name USER2 " + nameSuffix);
                IMSubmitIdea.FillFormField("Description", "Automation Description " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactName", "Automation Contact Name " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactEmail", "HPG.automation@HCAHealthcare.com");
                IMSubmitIdea.FillFormField("ContactPhone", "615-344-3000");
                IMSubmitIdea.SubmitSaveButton.Click();
                //step 2 end

                //Step 3. Login as User1
                //Expected Result: User dashboard is displayed
                IMLogin.LoginAs("Automation-S1");
                //step 3 end

                //Step 4. Create idea and save
                //Expected Result: Idea is saved.
                IMLogin.GotoSubmitAnIdea();
                IMSubmitIdea.FillFormField("IdeaName", "Automation Idea Name USER1 " + nameSuffix);
                IMSubmitIdea.FillFormField("Description", "Automation Description " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactName", "Automation Contact Name " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactEmail", "HPG.automation@HCAHealthcare.com");
                IMSubmitIdea.FillFormField("ContactPhone", "615-344-3000");
                IMSubmitIdea.SubmitSaveButton.Click();
                //step 4 end

                //Step 5. Navigate to View List of My Ideas page.
                //Expected Result: View List of My Ideas page should be displayed.
                IMHome.GotoMyIdeas();
                page_objects.imMyIdeas IMMyIdeas = new page_objects.imMyIdeas(IMHome.browser);
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                //step 5 end

                //Step 6. Verify List of My Ideas contains the following information: Idea, Status, Contact Name, Submission Date, Submission Time
                //Expected Result: Verify List of My Ideas is displayed and contains the following information: Idea, Status, Contact Name, Submission Date, Submission Time
                HpgAssert.Exists(IMMyIdeas.sortIdeaID, "Verify 'Idea Number' field is present");
                HpgAssert.Exists(IMMyIdeas.sortIdeaName, "Verify 'Idea Name' field is present");
                HpgAssert.Exists(IMMyIdeas.sortAssigned, "Verify 'Assigned' field is present");
                HpgAssert.Exists(IMMyIdeas.sortStatus, "Verify 'Status' field is present");
                HpgAssert.Exists(IMMyIdeas.sortCreated, "Verify 'Created' field is present");
                HpgAssert.Exists(IMMyIdeas.sortUpddated, "Verify 'Updated' field is present");
                //step 6 end

                //Step 7. Verify only ideas User1 has created are displayed
                //Expected Result: Idea from User1 is displayed. Idea from User2 is NOT displayed.
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'USER2')]").Missing(), "Verify USER2 idea does not display on USER1");
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'USER1')]").Exists(), "Verify USER1 idea is displayed on USER1");
                //step 7 end

                //Step 8. Click Action on a listed Idea and select Details from the dropdown list 
                //Expected Result: The Idea Details are displayed 
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'USER1')]/../../td[7]").FindLink("Action").Click();  //Click on Action
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'USER1')]/../../td[7]").FindLink("Details").Click(); //Click on Details
                page_objects.imIdeaDetails IMIdeaDetails = new page_objects.imIdeaDetails(IMMyIdeas.browser);
                IMIdeaDetails.DeleteButton.Click();
                page_objects.imIdeaDelete IMIdeaDelete = new page_objects.imIdeaDelete(IMMyIdeas.browser);
                IMIdeaDelete.ConfirmDeleteButton.Click();
                IMHome.GotoMyIdeas();
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'USER1')]").Missing(), "Verify USER1 idea is no longer displayed on USER1");
                //step 8 end

                //Step 9. Login as User2 and Navigate to View List of My Ideas page.
                //Expected Result: View List of My Ideas page should be displayed.
                IMLogin.LoginAs("Automation-S2");
                IMHome.GotoMyIdeas();
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'USER1')]").Missing(), "Verify USER1 idea does not display on USER2");
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'USER2')]").Exists(), "Verify USER2 idea is displayed on USER2");
                //step 9 end

                //Step 10. Click Action on Idea and select Delete from the dropdown list. 
                //Expected Result: The Idea Details are displayed with a Confirm Delete option
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'USER2')]/../../td[7]").FindLink("Action").Click();
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'USER2')]/../../td[7]").FindLink("Delete").Click();
                //step 10 end

                //Step 11. Click Confirm Delete 
                //Expected Result: The list of My Ideas is displayed without the Deleted Idea. 
                IMIdeaDelete.ConfirmDeleteButton.Click();
                //step 11 end

                //Step 12. Navigate to View List of My Ideas page.
                //Expected Result: View List of My Ideas page should be displayed, Verify deleted idea is not displayed
                IMHome.GotoMyIdeas();
                //step 12 end

                //Step 13. Verify deleted idea is not displayed
                //Expected Result: Idea is not displayed
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'USER2')]").Missing(), "Verify USER2 idea is no longer displayed on USER2");
                //step 13 end

                //Step 14. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 14 end
            }
        }

        [Test][Ignore]
        public void Test_TC4446()
        {
            RallyTestID = "TC4446";
            RallyFailVerdict = Rally.Verdict.Blocked;
            HpgAssert.Fail("Currently unable to create new idea via UI");
            //BUG: DE1974
            //US6047: Verify creator/assigned user ability to edit idea
            //Create idea, verify ability to edit saved idea, verify cancel does not save changes, verify no ability to edit submitted idea, check log.

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");
                //Step 1. Navigate to http://hcatest.xpxcloud.com/Idea
                //Expected Result: The list of My Ideas is displayed
                page_objects.imHome IMHome =
                    new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 1 end

                //Step 2. Create idea and save
                //Expected Result: Idea is saved with all information
                IMLogin.GotoSubmitAnIdea();
                page_objects.imSubmitAnIdea IMSubmitIdea = new page_objects.imSubmitAnIdea(IMLogin.browser);
                IMSubmitIdea.FillFormField("IdeaName", "Automation TC4446 " + nameSuffix);
                IMSubmitIdea.FillFormField("Description", "Automation Description for TC4446" + nameSuffix);
                IMSubmitIdea.FillFormField("ContactName", "Automation Contact Name " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactEmail", "HPG.automation@HCAHealthcare.com");
                IMSubmitIdea.FillFormField("ContactPhone", "615-344-3000");
                IMSubmitIdea.SubmitSaveButton.Click();
                //step 2 end

                //Step 3. Navigate to View My Ideas page - http://hcatest.xpxcloud.com/Idea 
                //Expected Result: The list of My Ideas is displayed 
                IMHome.GotoMyIdeas();
                page_objects.imMyIdeas IMMyIdeas = new page_objects.imMyIdeas(IMHome.browser);
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4446')]").Exists(), "Verify TC4447 idea is displayed");
                //step 3 end

                //Step 4. Click Action on created Idea and select Edit from the dropdown list. 
                //Expected Result: The Idea Details are displayed 
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4446')]/../../td[7]").FindLink("Action").Click();  //Click on Action
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4446')]/../../td[7]").FindLink("Edit").Click(); //Click on Edit
                page_objects.imEditIdea IMEditIdea = new imEditIdea(IMMyIdeas.browser);
                //step 4 end

                //Step 5. Edit the information saved in the Idea Name, Idea Description, Contact Name, Contact Email and Phone #. Click Save. 
                //Expected Result: The information listed in Idea Name, Idea Description, Contact Name, Contact Email and Phone # will be updated for that Idea. The message 'The idea was saved successfully .' will be displayed.
                IMEditIdea.FillFormField("IdeaName", "CHANGE TC4446 " + nameSuffix);
                IMEditIdea.FillFormField("Description", "CHANGE Description for TC4446 " + nameSuffix);
                IMEditIdea.FillFormField("ContactName", "CHANGE Contact Name " + nameSuffix);
                IMEditIdea.FillFormField("ContactEmail", "HPG.automationCHANGE@HCAHealthcare.com");
                IMEditIdea.FillFormField("ContactPhone", "615-344-3001");
                IMEditIdea.SubmitSaveButton.Click();
                //step 5 end

                //Step 6. Navigate to View My Ideas page - http://hcatest.xpxcloud.com/Idea 
                //Expected Result: The list of My Ideas is displayed 
                IMHome.GotoMyIdeas();
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4446')]").Exists(), "Verify TC4447 idea is displayed");
                //step 6 end

                //Step 7. Click Action on the same Idea and select Details from the dropdown list.
                //Expected Result: The information listed in the idea should reflect the most recent update.
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4446')]/../../td[7]").FindLink("Action").Click();  //Click on Action
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4446')]/../../td[7]").FindLink("Details").Click(); //Click on Details
                page_objects.imIdeaDetails IMIdeaDetails = new page_objects.imIdeaDetails(IMMyIdeas.browser);
                HpgAssert.AreEqual("CHANGE TC4446 " + nameSuffix, IMIdeaDetails.IdeaName.Text.Trim(), "Verify change in Idea Name");
                HpgAssert.AreEqual("CHANGE Description for TC4446 " + nameSuffix, IMIdeaDetails.IdeaDescription.Text.Trim(), "Verify change in Idea Name");
                //step 7 end

                //Step 8. Navigate to View My Ideas page - http://hcatest.xpxcloud.com/Idea 
                //Expected Result: The list of My Ideas is displayed 
                // -- Using edit button on details page
                //step 8 end

                //Step 9. Click Action on a listed Idea with the Status of Saved and select Edit from the dropdown list. 
                //Expected Result: The Idea Details are displayed 
                // -- Using edit button on details page
                IMIdeaDetails.EditButton.Click();
                //step 9 end

                //Step 10. Edit the information saved in the Idea Name, Idea Description, Contact Name, Contact Email and Phone #. Click Cancel. 
                //Expected Result: The list of My Ideas is displayed. 
                IMEditIdea.FillFormField("IdeaName", "Automation TC4446 " + nameSuffix);
                IMEditIdea.FillFormField("Description", "Automation Description for TC4446" + nameSuffix);
                IMEditIdea.FillFormField("ContactName", "Automation Contact Name " + nameSuffix);
                IMEditIdea.FillFormField("ContactEmail", "HPG.automation@HCAHealthcare.com");
                IMEditIdea.FillFormField("ContactPhone", "615-344-3000");
                IMEditIdea.SubmitCancelButton.Click();
                IMHome.GotoMyIdeas();
                //step 10 end

                //Step 11. Click Action on the same Idea and select Details from the dropdown list. 
                //Expected Result: The Idea Details are displayed. The information listed in Idea Name, Idea Description, Contact Name, Contact Email and Phone # will not be updated with the canceled changes.
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4446')]").Exists(), "Verify TC4447 idea is displayed");
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4446')]/../../td[7]").FindLink("Action").Click();  //Click on Action
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4446')]/../../td[7]").FindLink("Details").Click(); //Click on Details
                HpgAssert.AreEqual("CHANGE TC4446 " + nameSuffix, IMIdeaDetails.IdeaName.Text.Trim(), "Verify no change in Idea Name");
                HpgAssert.AreEqual("CHANGE Description for TC4446 " + nameSuffix, IMIdeaDetails.IdeaDescription.Text.Trim(), "Verify no change in Idea Name");
                //step 11 end

                //Step 12. Navigate to View My Ideas page - http://hcatest.xpxcloud.com/Idea 
                //Expected Result: The list of My Ideas is displayed 
                IMHome.GotoMyIdeas();
                //step 12 end

                //Step 13. Click Action on the same Idea and select Details from the dropdown list. 
                //Expected Result: The Idea Details are displayed 
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4446')]").Exists(), "Verify TC4447 idea is displayed");
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4446')]/../../td[7]").FindLink("Action").Click();  //Click on Action
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4446')]/../../td[7]").FindLink("Edit").Click(); //Click on Edit
                //step 13 end

                //Step 14. Submit idea
                //Expected Result: Idea should be marked as Submitted
                IMEditIdea.SubmitSubmitButton.Click();
                //step 14 end

                //Step 15. Click Action on an Idea with the Status of Submitted.
                //Expected Result: The only available option for the user is 'Details'.
                IMHome.GotoMyIdeas();
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4446')]").Exists(), "Verify TC4447 idea is displayed");
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4446')]/../../td[7]").FindLink("Action").Click();  //Click on Action
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4446')]/../../td[7]").FindLink("Edit").Missing(), "Verify 'Edit' buton is no longer available in Action DropDown Menu");
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4446')]/../../td[7]").FindLink("Delete").Missing(), "Verify 'Delete' buton is no longer available in Action DropDown Menu");
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4446')]/../../td[7]").FindLink("Attachment").Missing(), "Verify 'Attachment' buton is no longer available in Action DropDown Menu");
                //step 15 end

                //Step 16. Click Details from the Action menu for that Idea. 
                //Expected Result: The Idea Detail page is displayed. Verify that the user is unable to edit the idea. 
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4446')]/../../td[7]").FindLink("Details").Click();  //Click on Details
                HpgAssert.True(IMIdeaDetails.EditButton.Element.Missing(), "Verify 'Edit' button does not exist on detail page");
                HpgAssert.True(IMIdeaDetails.DeleteButton.Element.Missing(), "Verify 'Delete' button does not exist on detail page");
                //step 16 end
            }
        }

        [Test][Ignore]
        public void Test_TC4447()
        {
            RallyTestID = "TC4447";
            RallyFailVerdict = Rally.Verdict.Blocked;
            HpgAssert.Fail("Currently unable to delete an idea via UI");
            //US6055: Standard User Can delete idea
            //Create idea, delete, verify deletion.

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");
                DBUtility dbUtility = new DBUtility();

                //Step 1. Navigate to webpage http://hcadev.xpxcloud.com/Idea
                //Expected Result:View My Ideas page is displayed.
                page_objects.imHome IMHome =
                    new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 1 end

                //Step 2. Create Idea and Save
                //Expected Result:Idea is created with all information
                //IMLogin.GotoSubmitAnIdea();
                //page_objects.imSubmitAnIdea IMSubmitIdea = new page_objects.imSubmitAnIdea(IMLogin.browser);
                //IMSubmitIdea.FillFormField("IdeaName", "Automation TC4447 " + nameSuffix);
                //IMSubmitIdea.FillFormField("Description", "Automation Description for TC4447" + nameSuffix);
                //IMSubmitIdea.FillFormField("ContactName", "Automation Contact Name " + nameSuffix);
                //IMSubmitIdea.FillFormField("ContactEmail", "HPG.automation@HCAHealthcare.com");
                //IMSubmitIdea.FillFormField("ContactPhone", "615-344-3000");
                //IMSubmitIdea.SubmitSaveButton.Click();
                int newIdeaID = dbUtility.CreateSavedIdea("Automation TC4447 " + nameSuffix,
                                          "Automation Description for TC4447" + nameSuffix,
                                          "",
                                          "Automation Contact Name " + nameSuffix);
                //step 2 end

                //Step 3. Select idea from the list of available Ideas by clicking on the Idea Name.
                //Expected Result:The Idea Details page will be displayed.
                IMHome.GotoMyIdeas();
                page_objects.imMyIdeas IMMyIdeas = new page_objects.imMyIdeas(IMHome.browser);
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4447')]").Exists(), "Verify TC4447 idea is displayed");
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4447')]").Click();
                page_objects.imIdeaDetails IMIdeaDetails = new page_objects.imIdeaDetails(IMMyIdeas.browser);
                string itemID = IMIdeaDetails.IdeaID.Text.Trim();
                //step 3 end

                //Step 4. Click the Delete icon near the bottom right side of the Details view.
                //Expected Result:The 'Confirm Delete' icon should be displayed near the bottom left side of the Details view.
                IMIdeaDetails.DeleteButton.Click();
                page_objects.imIdeaDelete IMIdeaDelete = new page_objects.imIdeaDelete(IMMyIdeas.browser);
                //step 4 end

                //Step 5. Click the 'Confirm Delete' icon near the bottom left side of the Details view.
                //Expected Result:View My Ideas page is displayed without the Idea that was deleted.
                IMIdeaDelete.ConfirmDeleteButton.Click();
                IMHome.GotoMyIdeas();
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.Contains(IMMyIdeas.browser.Location.ToString(), "/Idea/All?", "Verify browser is still on results page");
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4447')]").Missing(), "Verify idea is no longer displayed on My Ideas");
                //step 5 end

                //Step 6. Verify deletion details are saved in the database
                //Expected Result:Idea deletion details are saved in the database
                DataTable ideaList = dbUtility.GetIdeasByNumber(int.Parse(itemID));
                HpgAssert.AreEqual("true", ideaList.Rows[0]["deleted"].ToString().ToLower(), "Verify Idea is marked as Deleted in database");
                //step 6 end

                //Step 7. Verify idea is no longer searchable
                //Expected Result:Item does not show up in search results
                IMMyIdeas.SearchFilterForm.Type(itemID);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.Contains(IMMyIdeas.browser.Location.ToString(), "/Idea/All?", "Verify browser is still on results page");
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4447')]").Missing(), "Verify idea is no longer displayed when surching for ItemID");
                //step 7 end

                //Clean up
                dbUtility.DeleteIdeaByIdeaNumber(int.Parse(itemID));
                //clean up end

                //Step 8. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 8 end
            }
        }

        [Test]
        public void Test_TC4475()
        {
            RallyTestID = "TC4475";
            //US6246: Verify existence and function of Search. ON MY IDEAS
            //Create ideas, search, and verify search results

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");
                DBUtility dbUtility = new DBUtility();

                //Step 1. Navigate to webpage http://hcadev.xpxcloud.com/Idea
                //Expected Result:View My Ideas page is displayed.
                page_objects.imHome IMHome =
                    new page_objects.imHome((BrowserSession) BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 1 end

                //Step 2. Create Idea and Save
                //Expected Result: Idea is created with all information
                string newItemID = dbUtility.CreateSavedIdea("Automation TC4475 " + nameSuffix,
                                          "Automation Description for TC4475" + nameSuffix,
                                          "",
                                          "Automation Contact Name " + nameSuffix).ToString();
                //step 2 end

                //Step 3. Verify the existence of a Search option at the top of the page.
                //Expected Result: Search field is available to the user.
                IMHome.GotoMyIdeas();
                page_objects.imMyIdeas IMMyIdeas = new page_objects.imMyIdeas(IMHome.browser);
                HpgAssert.True(IMMyIdeas.SearchFilterForm.Element.Exists(), "Verify Search Field is present on My Ideas page");
                //-- Achived via the searches in Step 4-5
                //step 2 end

                //Step 4. Enter valid value into Search fields (IdeaID, IdeaName, IdeaStatus)
                //Expected Result: Ideas associated with logged in user that match will be displayed in results.
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.Contains(IMMyIdeas.browser.Location.ToString(), "/Idea/All?", "Verify browser is still on results page");
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4475')]").Exists(), "Verify TC4475 idea is displayed");
                IMHome.GotoMyIdeas();                
                IMMyIdeas.SearchFilterForm.Type(newItemID);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.Contains(IMMyIdeas.browser.Location.ToString(), "/Idea/All?", "Verify browser is still on results page");
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4475')]").Exists(), "Verify TC4475 idea is displayed");
                //step 4 end

                //Step 5. Enter invalid value into search field
                //Expected Result: No ideas are returned in results.
                IMMyIdeas.SearchFilterForm.Type(nameSuffix + nameSuffix);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.Contains(IMMyIdeas.browser.Location.ToString(), "/Idea/All?", "Verify browser is still on results page");
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4475')]").Missing(), "Verify TC4475 idea is NOT displayed");
                IMHome.GotoMyIdeas();
                IMMyIdeas.SearchFilterForm.Type(newItemID + nameSuffix);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.Contains(IMMyIdeas.browser.Location.ToString(), "/Idea/All?", "Verify browser is still on results page");
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4475')]").Missing(), "Verify TC4475 idea is NOT displayed");
                //step 5 end

                //Clean up
                dbUtility.DeleteIdeaByIdeaNumber(int.Parse(newItemID));
                //end Clean Up

                //Step 6. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 6 end
            }
        }

        [Test][Ignore]
        public void Test_TC4577()
        {
            RallyTestID = "TC4577";
            RallyFailVerdict = Rally.Verdict.Blocked;
            HpgAssert.Fail("Currently unable to create an idea via UI");
            //US6058: Verify Reassign Function
            //Create Idea, Admin reassigns idea, verify email and ability to edit idea and resubmit

            AutomationCore.utility.ews ews = new AutomationCore.utility.ews();
            ews.FilterContains.Clear();
            ews.FilterContains.Add(new KeyValuePair<string, string>("subject", "Idea Reassigned"));
            ews.FilterContains.Add(new KeyValuePair<string, string>("from", "donotreply@xpanxion.com"));

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");

                //Step 1. Navigate to Homepage http://hcatest.xpxcloud.com/
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome =
                    new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Standard User1
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Create Idea and Submit
                //Expected Result: Idea is created with all information
                IMLogin.GotoSubmitAnIdea();
                page_objects.imSubmitAnIdea IMSubmitIdea = new page_objects.imSubmitAnIdea(IMLogin.browser);
                IMSubmitIdea.FillFormField("IdeaName", "Automation TC4577 " + nameSuffix);
                IMSubmitIdea.FillFormField("Description", "Automation Description for TC4577 " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactName", "Automation Contact Name " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactEmail", "HPG.automation@HCAHealthcare.com");
                IMSubmitIdea.FillFormField("ContactPhone", "615-344-3000");
                IMSubmitIdea.SubmitSaveButton.Click();
                page_objects.imEditIdea IMEditIdea = new page_objects.imEditIdea(IMSubmitIdea.browser);
                string newItemID = IMEditIdea.IdeaID.Text.Trim();
                IMEditIdea.SubmitSubmitButton.Click();
                //step 3 end

                //Step 4. Login as Administrator
                //Expected Result: Administrator is logged in
                IMLogin.LoginAs("Automation-Admin");
                //step 4 end

                //Step 5. Reassign created idea to User2
                //Expected Result: Idea is reassigned to User2
                IMHome.GotoAllIdeas();
                page_objects.imMyIdeas IMMyIdeas = new page_objects.imMyIdeas(IMHome.browser);
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4577')]").Click();
                page_objects.imIdeaDetailsAdmin IMIdeaDetails = new page_objects.imIdeaDetailsAdmin(IMMyIdeas.browser);
                HpgAssert.Contains(IMIdeaDetails.pageHeader.Text, "Idea Details", "Verify Idea Details page is loaded");
                IMIdeaDetails.ReassignButton.Click();
                page_objects.imReassign IMReassign = new page_objects.imReassign(IMIdeaDetails.browser);
                IMReassign.SelectReassignTo("Automation-S2");
                IMReassign.CommentTextBox.Type(nameSuffix + "COMMENT");
                ews.FilterGreaterThan.Clear();
                ews.FilterGreaterThan.Add(new KeyValuePair<string, object>("sent", DateTime.Now));
                IMReassign.ComfirmButton.Click();
                //step 5 end

                //Step 6. Verify email sent to User1 and User2
                //Expected Result: Email sent to each user
                DataTable msg = ews.GetMessagesDTWait(true, 10, 120);
                HpgAssert.AreEqual("1", msg.Rows.Count.ToString(), "Verify single email was received");
                ews.DeleteMessage(msg.Rows[0]["ID"].ToString());
                //step 6 end

                //Step 7. Verify Reassignment comments are present
                //Expected Result: Comments are present for the reassignment
                IMHome.GotoAllIdeas();
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4577')]").Click();
                IMIdeaDetails.CommentsTab.Element.Hover();
                IMIdeaDetails.CommentsTab.Click();
                HpgAssert.AreEqual(nameSuffix + "COMMENT", IMIdeaDetails.IdeaComment.Text.Trim(), "Verify Idea Comments");
                //step 7 end

                //Step 8. Verify User2 can now edit and resubmitt Idea
                //Expected Result: User2 edits and resubmitts idea
                IMLogin.LoginAs("Automation-S2");
                IMHome.GotoMyIdeas();
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4577')]").Exists(), "Verify idea is now displayed on User2");
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4577')]/../../td[7]").FindLink("Action").Click();  //Click on Action
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4577')]/../../td[7]").FindLink("Edit").Click(); //Click on Edit
                IMEditIdea.FillFormField("Description", "CHANGE Description for TC4577 " + nameSuffix);
                IMEditIdea.FillFormField("Comments", "RESUBMITTED TC4577 " + DateTime.Now.ToString("F"));
                IMEditIdea.EditResubmit.Click();
                //step 8 end

                //Step 9. Login as Standard User1 and Verify User1 can still view idea under My Ideas
                //Expected Result: User1 My Ideas still lists idea
                IMLogin.LoginAs("Automation-S1");
                IMHome.GotoMyIdeas();
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4577')]").Exists(), "Verify idea is still displayed on User1");
                //step 9 end

                //Step 10. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 10 end
            }
        }

        [Test][Ignore]
        public void Test_TC4608()
        {
            RallyTestID = "TC4608";
            RallyFailVerdict = Rally.Verdict.Blocked;
            HpgAssert.Fail("Currently unable to reject an idea via UI");
            //US6056: Verify ability to reject submitted idea
            //Create idea, submit, reject, verify status & email

            AutomationCore.utility.ews ews = new AutomationCore.utility.ews();
            ews.FilterContains.Clear();
            ews.FilterContains.Add(new KeyValuePair<string, string>("subject", "Idea Rejected"));
            ews.FilterContains.Add(new KeyValuePair<string, string>("from", "donotreply@xpanxion.com"));

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");

                //Step 1. Navigate to webpage http://hcatest.xpxcloud.com/
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome =
                    new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Standard User1
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Create Idea and Submit
                //Expected Result: Idea is created with all information
                IMLogin.GotoSubmitAnIdea();
                page_objects.imSubmitAnIdea IMSubmitIdea = new page_objects.imSubmitAnIdea(IMLogin.browser);
                IMSubmitIdea.FillFormField("IdeaName", "Automation TC4608 " + nameSuffix);
                IMSubmitIdea.FillFormField("Description", "Automation Description for TC4608 " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactName", "Automation Contact Name " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactEmail", "HPG.automation@HCAHealthcare.com");
                IMSubmitIdea.FillFormField("ContactPhone", "615-344-3000");
                IMSubmitIdea.SubmitSaveButton.Click();
                page_objects.imEditIdea IMEditIdea = new page_objects.imEditIdea(IMSubmitIdea.browser);
                string newItemID = IMEditIdea.IdeaID.Text.Trim();
                IMEditIdea.SubmitSubmitButton.Click();
                //step 3 end

                //Step 4. Login as Administrator
                //Expected Result: Administrator is logged in
                IMLogin.LoginAs("Automation-Admin");
                //step 4 end

                //Step 5. Reject submitted idea
                //Expected Result: Idea is marked as 'Rejected' with comments
                IMHome.GotoAllIdeas();
                page_objects.imMyIdeas IMMyIdeas = new page_objects.imMyIdeas(IMHome.browser);
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4608')]").Click();
                page_objects.imIdeaDetailsAdmin IMIdeaDetails = new page_objects.imIdeaDetailsAdmin(IMMyIdeas.browser);
                HpgAssert.Contains(IMIdeaDetails.pageHeader.Text, "Idea Details", "Verify Idea Details page is loaded");
                IMIdeaDetails.RejectButton.Click();
                page_objects.imIdeaReject IMIdeaReject = new page_objects.imIdeaReject(IMIdeaDetails.browser);
                IMIdeaReject.RejectComments.Type("Idea rejected by Automation-Admin on " + DateTime.Now.ToString("F"));
                ews.FilterGreaterThan.Clear();
                ews.FilterGreaterThan.Add(new KeyValuePair<string, object>("sent", DateTime.Now));
                IMIdeaReject.ConfirmButton.Click();
                //step 5 end

                //Step 6. Verify email sent to Idea Author
                //Expected Result: Email is sent to Idea Author
                DataTable msg = ews.GetMessagesDTWait(true, 10, 120);
                HpgAssert.AreEqual("1", msg.Rows.Count.ToString(), "Verify single email was received");
                ews.DeleteMessage(msg.Rows[0]["ID"].ToString());
                //step 6 end

                //Step 7. Login as Standard User1
                //Expected Result: User is logged in
                IMLogin.LoginAs("Automation-S1");
                //step 7 end

                //Step 8. Verify Idea is marked as Rejected
                //Expected Result: Idea is marked as 'Rejected' with comments
                IMHome.GotoMyIdeas();
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4608')]").Exists(), "Verify Idea is in search results");
                HpgAssert.AreEqual("Rejected", IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4608')]/../../td[4]").Text, "Verify search result shows status as 'Rejected'");
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4608')]").Click();
                HpgAssert.AreEqual("Rejected", IMIdeaDetails.IdeaStatus.Text.Trim(), "Verify status is 'Rejected'");
                //step 8 end

                //Step 9. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 9 end
            }
        }

        [Test][Ignore]
        public void Test_TC4609()
        {
            RallyTestID = "TC4609";
            RallyFailVerdict = Rally.Verdict.Blocked;
            HpgAssert.Fail("Currently unable to accept idea.");
            //US6060: Verify ability to Approve submitted idea
            //Create idea, submit, approve, verify status & email

            AutomationCore.utility.ews ews = new AutomationCore.utility.ews();
            ews.FilterContains.Clear();
            ews.FilterContains.Add(new KeyValuePair<string, string>("subject", "Idea Accepted"));
            ews.FilterContains.Add(new KeyValuePair<string, string>("from", "donotreply@xpanxion.com"));

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");
                DBUtility dbUtility = new DBUtility();

                //Step 1. Navigate to webpage http://hcatest.xpxcloud.com/
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Standard User1
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Create Idea and Submit
                //Expected Result: Idea is created with all information
                //IMLogin.GotoSubmitAnIdea();
                //page_objects.imSubmitAnIdea IMSubmitIdea = new page_objects.imSubmitAnIdea(IMLogin.browser);
                //IMSubmitIdea.FillFormField("IdeaName", "Automation TC4609 " + nameSuffix);
                //IMSubmitIdea.FillFormField("Description", "Automation Description for TC4609 " + nameSuffix);
                //IMSubmitIdea.FillFormField("ContactName", "Automation Contact Name " + nameSuffix);
                //IMSubmitIdea.FillFormField("ContactEmail", "HPG.automation@HCAHealthcare.com");
                //IMSubmitIdea.FillFormField("ContactPhone", "615-344-3000");
                //IMSubmitIdea.SubmitSaveButton.Click();
                //page_objects.imEditIdea IMEditIdea = new page_objects.imEditIdea(IMSubmitIdea.browser);
                //string newItemID = IMEditIdea.IdeaID.Text.Trim();
                //IMEditIdea.SubmitSubmitButton.Click();
                string newItemID = dbUtility.CreateSavedIdea("Automation TC4609 " + nameSuffix,
                                          "Automation Description for TC4609" + nameSuffix,
                                          "",
                                          "Automation Contact Name " + nameSuffix).ToString();
                dbUtility.SubmitIdea(int.Parse(newItemID));
                //step 3 end

                //Step 4. Login as Administrator
                //Expected Result: Administrator is logged in
                //IMLogin.LoginAs("Automation-Admin");
                //step 4 end

                //Step 5. Approve submitted idea
                //Expected Result: Idea is marked as 'Approved' with comments
                IMHome.GotoAllIdeas();
                page_objects.imMyIdeas IMMyIdeas = new page_objects.imMyIdeas(IMHome.browser);
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4609')]").Click();
                page_objects.imIdeaDetailsAdmin IMIdeaDetails = new page_objects.imIdeaDetailsAdmin(IMMyIdeas.browser);
                HpgAssert.Contains(IMIdeaDetails.pageHeader.Text, "Idea Details", "Verify Idea Details page is loaded");
                IMIdeaDetails.AcceptButton.Click();
                page_objects.imIdeaAccept IMIdeaAccept = new page_objects.imIdeaAccept(IMIdeaDetails.browser);
                IMIdeaAccept.AcceptComments.Type("Idea Accepted by Automation-Admin on " + DateTime.Now.ToString("F"));
                ews.FilterGreaterThan.Clear();
                ews.FilterGreaterThan.Add(new KeyValuePair<string, object>("sent", DateTime.Now.AddSeconds(-10)));
                IMIdeaAccept.ConfirmButton.Click();
                //step 5 end

                //Step 6. Verify email sent to Idea Author
                //Expected Result: Email is sent to Idea Author
                //DataTable msg = ews.GetMessagesDTWait(true, 10, 180);
                //HpgAssert.AreEqual("1", msg.Rows.Count.ToString(), "Verify single email was received");
                //ews.DeleteMessage(msg.Rows[0]["ID"].ToString());
                //step 6 end

                //Step 7. Login as Standard User1
                //Expected Result: User is logged in
                //IMLogin.LoginAs("Automation-S1");
                //step 7 end

                //Step 8. Verify Idea is marked as 'Approved'
                //Expected Result: Idea is marked as 'Approved' with comments
                IMHome.GotoMyIdeas();
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.True(IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4609')]").Exists(), "Verify Idea is in search results");
                HpgAssert.AreEqual("Accepted", IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4609')]/../../td[4]").Text, "Verify search result shows status as 'Accepted'");
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4609')]").Click();
                HpgAssert.AreEqual("Accepted", IMIdeaDetails.IdeaStatus.Text.Trim(), "Verify status is 'Accepted'");
                //step 8 end

                //Step 9. Verify details in Database
                //Expected Result: Approved Idea details are correct in Database
                DataTable ideas = dbUtility.GetIdeasByNumber(int.Parse(newItemID));
                HpgAssert.AreEqual("7", ideas.Rows[0]["StatusID"].ToString(), "Verify status of Idea is now 7 (Approved)");
                //step 9 end

                //Clean-up
                dbUtility.DeleteIdeaByIdeaNumber(int.Parse(newItemID));
                //end clean-up

                //Step 10. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 10 end
            }
        }

        [Test][Ignore]
        public void Test_TC4610()
        {
            //TODO: Determine if test is still valid
            RallyTestID = "TC4610";
            RallyFailVerdict = Rally.Verdict.Blocked;
            HpgAssert.Fail("Currently unable to create an idea via UI");
            //US6060: Verify ability to Approve submitted idea
            //US6066: Edit Idea Detail of Approved Idea
            //Create idea, submit, Approve, verify ability to edit details, mark ready for publishing

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");
                
                //Step 1. Navigate to webpage http://hcatest.xpxcloud.com/
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Standard User1
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Create 2 Ideas and Submit
                WriteReport("Create 2 Ideas and Submit");
                //Expected Result: Ideas are created with all information
                IMLogin.GotoSubmitAnIdea();
                page_objects.imSubmitAnIdea IMSubmitIdea = new page_objects.imSubmitAnIdea(IMLogin.browser);
                IMSubmitIdea.FillFormField("IdeaName", "Automation ACCEPT TC4610 " + nameSuffix);
                IMSubmitIdea.FillFormField("Description", "Automation Description ACCEPTED IDEA for TC4610 " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactName", "Automation Contact Name " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactEmail", "HPG.automation@HCAHealthcare.com");
                IMSubmitIdea.FillFormField("ContactPhone", "615-344-3000");
                IMSubmitIdea.SubmitSaveButton.Click();
                page_objects.imEditIdea IMEditIdea = new page_objects.imEditIdea(IMSubmitIdea.browser);
                string submitIdeaID = IMEditIdea.IdeaID.Text.Trim();
                IMEditIdea.SubmitSubmitButton.Click();
                IMLogin.GotoSubmitAnIdea();
                IMSubmitIdea.FillFormField("IdeaName", "Automation SUBMIT TC4610 " + nameSuffix);
                IMSubmitIdea.FillFormField("Description", "Automation Description SUBMITTED IDEA for TC4610 " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactName", "Automation Contact Name " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactEmail", "HPG.automation@HCAHealthcare.com");
                IMSubmitIdea.FillFormField("ContactPhone", "615-344-3000");
                IMSubmitIdea.SubmitSaveButton.Click();
                string savedIdeaID = IMEditIdea.IdeaID.Text.Trim();
                IMEditIdea.SubmitSubmitButton.Click();
                //step 3 end

                //Step 4. Login as Administrator
                //Expected Result: Administrator is logged in
                //IMLogin.LoginAs("Automation-Admin");
                //step 4 end

                //Step 5. Approve one submitted idea
                //Expected Result: Idea is marked as 'Approved' with comments
                WriteReport("Approve one submitted idea");
                page_objects.imMyIdeas IMMyIdeas = new page_objects.imMyIdeas(IMHome.browser);
                IMMyIdeas.GoToIdeaNumber(submitIdeaID);
                page_objects.imIdeaDetailsAdmin IMIdeaDetails = new page_objects.imIdeaDetailsAdmin(IMMyIdeas.browser);
                HpgAssert.Contains(IMIdeaDetails.pageHeader.Text, "Idea Details", "Verify Idea Details page is loaded");
                IMIdeaDetails.AcceptButton.Click();
                page_objects.imIdeaAccept IMIdeaAccept = new page_objects.imIdeaAccept(IMIdeaDetails.browser);
                IMIdeaAccept.AcceptComments.Type("Idea Accepted by Automation-Admin on " + DateTime.Now.ToString("F"));
                IMIdeaAccept.ConfirmButton.Click();
                //step 5 end

                //Step 6. Change details of Idea and Cancel Changes
                //Expected Result: Changes are not saved to Idea
                WriteReport("Change details of Idea and Cancel Changes");
                IMMyIdeas.GoToIdeaNumber(submitIdeaID);
                for (int i = 0; i < 5; i++)
                {
                    if (IMIdeaDetails.PublishIdeaButton.Exists())
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(5000);
                    IMMyIdeas.GoToIdeaNumber(submitIdeaID);
                    IMMyIdeas.pageHeader.Element.SendKeys(OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.F5);
                }
                HpgAssert.Exists(IMIdeaDetails.PublishIdeaButton, "Verify Publish Button is present");
                IMIdeaDetails.PublishIdeaButton.Click();
                page_objects.im3PV im3Pv = new page_objects.im3PV(IMIdeaDetails.browser);
                //im3PV.IdeaName.Type("NAME NOCHANGE");
                //im3PV.IdeaDescription.Type("DESCRIPTION NOCHANGE");
                //im3PV.SelectLastOptionOnDropDown(im3PV.IdeaCategory);
                //im3PV.SelectLastOptionOnDropDown(im3PV.IdeaDepartment);
                //im3PV.SelectLastOptionOnDropDown(im3PV.IdeaImpact);
                //im3PV.SelectLastOptionOnDropDown(im3PV.IdeaEffort);
                //im3PV.IdeaFirstName.Type("NOCHANGE");
                //im3PV.IdeaLastName.Type("NOCHANGE");
                //im3PV.IdeaPhone.Type("987-654-3210");
                //im3PV.IdeaEmail.Type("NOCHANGE@hcahealthcare.com");
                //im3PV.BackButton.Click();
                //IMMyIdeas.GoToIdeaNumber(submitIdeaID);
                //IMIdeaDetails.PublishIdeaButton.Click();
                //HpgAssert.False(im3PV.IdeaName.Value.Contains("NOCHANGE"), "Verify change in name is NOT present");
                //HpgAssert.False(im3PV.IdeaDescription.Value.Contains("NOCHANGE"), "Verify change in description is NOT present");
                //HpgAssert.False(im3PV.IdeaFirstName.Value.Contains("NOCHANGE"), "Verify change in first name is NOT present");
                //HpgAssert.False(im3PV.IdeaLastName.Value.Contains("NOCHANGE"), "Verify change in last name is NOT present");
                //HpgAssert.False(im3PV.IdeaPhone.Value.Contains("987-654-3210"), "Verify change in phone is NOT present");
                //HpgAssert.False(im3PV.IdeaEmail.Value.Contains("NOCHANGE"), "Verify change in email is NOT present");
                //step 6 end

                //Step 7. Change details of Idea and Save Changes
                //Expected Result: Changes are saved to Idea and logged to DB
                WriteReport("Change details of Idea and Save Changes");
                //im3PV.IdeaName.Type("NAME CHANGE " + nameSuffix);
                //im3PV.IdeaDescription.Type("DESCRIPTION CHANGE " + DateTime.Now.ToString("F"));
                //im3PV.SelectLastOptionOnDropDown(im3PV.IdeaCategory);
                //im3PV.SelectLastOptionOnDropDown(im3PV.IdeaDepartment);
                //im3PV.SelectLastOptionOnDropDown(im3PV.IdeaImpact);
                //im3PV.SelectLastOptionOnDropDown(im3PV.IdeaEffort);
                //im3PV.IdeaFirstName.Type("Robot");
                //im3PV.IdeaLastName.Type("Automation");
                //im3PV.IdeaPhone.Type("515-555-1212");
                //im3PV.IdeaEmail.Type("hpg.automation@hcahealthcare.com");
                //im3PV.SaveButton.Click();
                ////TODO: Verify changes in DB (unable to inability to access DB at this time)
                //IMMyIdeas.GoToIdeaNumber(submitIdeaID);
                //IMIdeaDetails.PublishIdeaButton.Click();
                //HpgAssert.AreEqual("NAME CHANGE " + nameSuffix, im3PV.IdeaName.Value, "Verify saved change in name is present");
                //HpgAssert.Contains(im3PV.IdeaDescription.Value, "DESCRIPTION CHANGE ", "Verify saved change in description is present");
                //HpgAssert.AreEqual("Robot", im3PV.IdeaFirstName.Value, "Verify saved change in name is present");
                //HpgAssert.AreEqual("Automation", im3PV.IdeaLastName.Value, "Verify saved change in name is present");
                //HpgAssert.AreEqual("515-555-1212", im3PV.IdeaPhone.Value, "Verify saved change in name is present");
                //HpgAssert.AreEqual("hpg.automation@hcahealthcare.com", im3PV.IdeaEmail.Value, "Verify saved change in name is present");
                //step 7 end

                //Step 8. Publish Idea
                WriteReport("Publish Idea");
                //Expected Result: Idea is published with details Contact Name, Phone, and Email
                //im3PV.PublishButton.Click();
                //step 8 end

                //Step 9. Verify inability to publish Idea that was not Approved
                WriteReport("Verify inability to publish Idea that was not Approved");
                //Expected Result: Non-approved Ideas can not be published
                IMHome.GotoAllIdeas();
                IMMyIdeas.SearchFilterForm.Type(savedIdeaID);
                IMMyIdeas.FilterButton.Click();
                HpgAssert.True(IMIdeaDetails.PublishIdeaButton.Element.Missing(), "Verify Publish button is not available on non-accepted idea");
                //step 9 end

                //Step 10. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 10 end
            }
        }

        [Test][Ignore]
        public void Test_TC4611()
        {
            //TODO: Determine if test is still valid
            RallyTestID = "TC4611";
            RallyFailVerdict = Rally.Verdict.Blocked;
            HpgAssert.Fail("Currently unable to publish an idea");
            //US6214: View Details of Published Idea
            //Create idea, Submit, Approve, Publish, verify details of Published Idea

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");
                DBUtility dbUtility = new DBUtility();

                //Step 1. Navigate to webpage http://hcatest.xpxcloud.com/
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Standard User1
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Create an Idea and Submit
                WriteReport("3 - Create an Idea and Submit");
                //Expected Result: Idea is created with all information
                string newItemID = dbUtility.CreateSavedIdea("Automation TC4611 " + nameSuffix,
                                          "Automation Description for TC4611" + nameSuffix,
                                          "",
                                          "Automation Contact Name " + nameSuffix).ToString();
                //step 3 end

                //Step 4. Login as Administrator
                //Expected Result: Administrator is logged in
                //step 4 end

                //Step 5. Approve the submitted idea
                WriteReport("5 - Approve the submitted idea");
                //Expected Result: Idea is marked as 'Approved'
                page_objects.imMyIdeas IMMyIdeas = new page_objects.imMyIdeas(IMHome.browser);
                dbUtility.AcceptIdea(int.Parse(newItemID));
                //step 5 end

                //Step 6. Publish Idea
                WriteReport("6 - Publish Idea");
                //Expected Result: Idea is published
                IMMyIdeas.GoToIdeaNumber(newItemID);
                page_objects.imIdeaDetailsAdmin IMIdeaDetails = new page_objects.imIdeaDetailsAdmin(IMMyIdeas.browser);
                for (int i = 0; i < 5; i++)
                {
                    if (IMIdeaDetails.PublishIdeaButton.Exists())
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(5000);
                    IMMyIdeas.GoToIdeaNumber(newItemID);
                    IMMyIdeas.pageHeader.Element.SendKeys(OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.F5);
                }
                //TODO: Utilize new publish page
                HpgAssert.Exists(IMIdeaDetails.PublishIdeaButton, "Verify Publish Button is present");
                IMIdeaDetails.PublishIdeaButton.Click();
                page_objects.im3PV im3Pv = new page_objects.im3PV(IMIdeaDetails.browser);
                //string ideaCategory = im3PV.SelectLastOptionOnDropDown(im3PV.IdeaCategory);
                //string ideaDepartment = im3PV.SelectLastOptionOnDropDown(im3PV.IdeaDepartment);
                //string ideaEffort = im3PV.SelectLastOptionOnDropDown(im3PV.IdeaEffort);
                //string ideaImpact = im3PV.SelectLastOptionOnDropDown(im3PV.IdeaImpact);
                //im3PV.IdeaFirstName.Type("Robot");
                //im3PV.IdeaLastName.Type("Automation");
                //im3PV.IdeaPhone.Type("515-555-1212");
                //im3PV.IdeaEmail.Type("hpg.automation@hcahealthcare.com");
                //try
                //{
                //    im3PV.PublishButton.Element.Click(new Options()
                //        {
                //            Timeout = TimeSpan.FromMinutes(5),
                //            RetryInterval = TimeSpan.FromSeconds(10)
                //        });
                //}
                //catch (Exception e)
                //{
                //    WriteReport("----Waiting on browser to publish (" + e.Message + ")----");
                //}
                //HpgAssert.True(im3PV.browser.FindXPath("//div[@class='validation-summary-errors']/span[contains(.,'fix the following errors')]").Missing(), "Verify no errors are present after publish");
                //SendKeys.SendWait("{ESC}");
                //SendKeys.SendWait("{ESC}");
                //im3PV.browser.Now();
                //step 6 end

                //Step 7. Login as Standard User2
                //Expected Result: User is logged in
                //IMLogin.LoginAs("Automation-S2");
                //step 7 end

                //Step 8. Navigate to Published Idea
                WriteReport("8 - Navigate to Published Idea");
                //Expected Result: Published Idea is displayed
                //IMMyIdeas.GoToIdeaNumber(newItemID);
                page_objects.imPublishedIdeas IMPublishedIdeas = new page_objects.imPublishedIdeas(IMHome.browser);
                IMPublishedIdeas.GoToPublishedIdeaNumber(newItemID);
                page_objects.imPublishedIdea IMPublishedIdea = new page_objects.imPublishedIdea(IMPublishedIdeas.browser);
                //step 8 end

                //Step 9. Verify details of Published Idea
                WriteReport("9 - Verify details of Published Idea");
                //Expected Result: Details include Name, Description, Current Status, Comments, Contact Name, Contact Phone, Contact Email, Category
                //HpgAssert.AreEqual(newItemID, IMPublishedIdea.IdeaNumber.Text.Trim(), "Verify Idea Number");
                //HpgAssert.Contains(IMPublishedIdea.IdeaTitle.Text.Trim(), "Automation TC4611 " + nameSuffix, "Verify Idea Name");
                //HpgAssert.AreEqual(ideaCategory, IMPublishedIdea.Category.Text.Replace("Category:", "").Trim(), "Verify Category");
                //HpgAssert.AreEqual(ideaDepartment, IMPublishedIdea.Department.Text.Replace("Department:", "").Trim(), "Verify Department");
                //HpgAssert.AreEqual(ideaEffort, IMPublishedIdea.EffortLevel.Text.Trim(), "Verify Effort");
                //HpgAssert.AreEqual(ideaImpact, IMPublishedIdea.ImpactLevel.Text.Trim(), "Verify Impact");
                //HpgAssert.AreEqual("Automation Description for TC4611 " + nameSuffix, IMPublishedIdea.Description.Text.Trim(), "Verify Description");
                //HpgAssert.True((DateTime.Now - DateTime.Parse(IMPublishedIdea.PublishedDate.Text.Replace("Published:", "").Trim()) <= TimeSpan.FromDays(1)), "Verify Published Date (1 day tolerence due to server time differences)");
                //step 9 end

                //Clean-up
                dbUtility.DeleteIdeaByIdeaNumber(int.Parse(newItemID));
                //end clean-up

                //Step 10. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 10 end
            }
        }

        [Test]
        public void Test_TC4705()
        {
            RallyTestID = "TC4705";
            //US6723: Search by Idea Number
            //Create idea, Submit, Approve, Publish, Verify ability to search for idea.

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");
                DBUtility dbUtility = new DBUtility();
                
                //Step 1. Navigate to webpage http://hcatest.xpxcloud.com/
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Standard User1
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Create an Idea and Submit
                //Expected Result: Idea is created with all information
                int newIdeaID = dbUtility.CreateSavedIdea("Automation TC4705 " + nameSuffix,
                                          "Automation Description for TC4705" + nameSuffix,
                                          "",
                                          "Automation Contact Name " + nameSuffix);
                //step 3 end

                //Step 4. Login as Administrator
                //Expected Result: Administrator is logged in
                //IMLogin.LoginAs("Automation-Admin");
                //step 4 end

                //Step 5. Approve the submitted idea
                //Expected Result: Idea is marked as 'Approved'
                dbUtility.AcceptIdea(newIdeaID);
                //step 5 end

                //Step 6. Publish Idea
                //Expected Result: Idea is published
                IMHome.GoToIdeaNumber(newIdeaID.ToString());
                page_objects.imIdeaDetailsAdmin IMIdeaDetails = new page_objects.imIdeaDetailsAdmin(IMHome.browser);
                IMIdeaDetails.PublishIdeaButton.Click();
                page_objects.im3PV publishIdea = new page_objects.im3PV(IMIdeaDetails.browser);
                publishIdea.SaveAllButton.Click();
                System.Threading.Thread.Sleep(3000);
                publishIdea.SaveAllButton.Click();
                System.Threading.Thread.Sleep(10000);
                HpgAssert.True(dbUtility.PublishQualifiedIdea(newIdeaID) > 0, "Publish idea " + newIdeaID.ToString());
                System.Threading.Thread.Sleep(3000);
                //step 6 end

                //Step 7. Login as Standard User2
                //Expected Result: User is logged in
                //step 7 end

                //Step 8. Search in All Ideas for Published Idea
                //Expected Result: User is taken directly to Published Idea
                IMHome.GotoAllIdeas();
                page_objects.imAllIdeas allIdeas = new imAllIdeas(IMHome.browser);
                allIdeas.SearchFilterForm.Type(newIdeaID.ToString());
                allIdeas.FilterButton.Element.Hover();
                allIdeas.FilterButton.Click();
                System.Threading.Thread.Sleep(5000);
                List<imAllIdeas.AllIdea> ideaList = allIdeas.GetAllIdeas();
                //HpgAssert.True(ideaList.Select(i => i.IdeaNumber.Text.Equals(newIdeaID.ToString())).Count().Equals(1), "Verify idea is in results");
                //TODO: Verify idea is first result
                //HpgAssert.AreEqual(newIdeaID.ToString(), ideaList.ElementAt(0).IdeaNumber.Text, "Verify idea is first result");
                //step 8 end

                //Step 9. Search in Published Ideas for Published Idea
                //Expected Result: User is taken directly to Published Idea
                IMHome.GotoPublishedIdeas(false);
                page_objects.imPublishedIdeas IMPublishedIdeas = new page_objects.imPublishedIdeas(IMHome.browser);
                IMPublishedIdeas.SearchIdeaID.Type(newIdeaID.ToString());
                IMPublishedIdeas.FilterButton.Element.Hover();
                IMPublishedIdeas.FilterButton.Click();
                System.Threading.Thread.Sleep(5000);
                List<imPublishedIdeas.PublishedIdea> publishedIdeaList = IMPublishedIdeas.GetPublishedIdeas();
                //HpgAssert.True(publishedIdeaList.Select(i => i.IdeaNumber.Text.Equals(newIdeaID.ToString())).Count().Equals(1), "Verify idea is in results");
                //HpgAssert.AreEqual(newIdeaID.ToString(), publishedIdeaList.ElementAt(0).IdeaNumber.Text, "Verify idea is first result");
                //step 9 end

                //Step 10. Search in My Ideas for Published Idea
                //Expected Result: Search results are displayed for Ideas containing IdeaID
                IMHome.GotoMyIdeas();
                page_objects.imMyIdeas myIdeas = new imMyIdeas(IMHome.browser);
                myIdeas.SearchFilterForm.Type(newIdeaID.ToString());
                myIdeas.FilterButton.Element.Hover();
                myIdeas.FilterButton.Click();
                System.Threading.Thread.Sleep(5000);
                List<imMyIdeas.MyIdea> myIdeaList = myIdeas.GetMyIdeas();
                HpgAssert.True(myIdeaList.Select(i => i.IdeaNumber.Text.Equals(newIdeaID.ToString())).Count().Equals(1), "Verify idea is in results");
                //step 10 end

                //Step 11. Search in All Ideas for Non-Existent Idea
                //Expected Result: Search results are displayed for Ideas containing IdeaID
                IMHome.GotoAllIdeas();
                allIdeas.SearchFilterForm.Type(nameSuffix + newIdeaID.ToString() + nameSuffix);
                allIdeas.FilterButton.Element.Hover();
                allIdeas.FilterButton.Click();
                System.Threading.Thread.Sleep(5000);
                ideaList = allIdeas.GetAllIdeas();
                //HpgAssert.True(ideaList.Select(i => i.IdeaNumber.Text.Equals(newIdeaID.ToString())).Count().Equals(0), "Verify idea is not in results");
                //step 11 end

                //Step 12. Search in Published Ideas for Non-Existent Idea
                //Expected Result: Search results are displayed for Ideas containing IdeaID
                IMHome.GotoPublishedIdeas(false);
                IMPublishedIdeas.SearchIdeaID.Type(nameSuffix + newIdeaID.ToString() + nameSuffix);
                IMPublishedIdeas.FilterButton.Element.Hover();
                IMPublishedIdeas.FilterButton.Click();
                System.Threading.Thread.Sleep(5000);
                publishedIdeaList = IMPublishedIdeas.GetPublishedIdeas();
                if (publishedIdeaList.Count > 0)
                {
                    //HpgAssert.True(publishedIdeaList.Select(i => i.IdeaNumber.Text.Equals(newIdeaID.ToString())).Count().Equals(0), "Verify idea is not in results");
                }
                else
                {
                    WriteReport("0 results returned: Idea is not in results.");
                }

                //step 12 end

                //Step 13. Login as Standard User1
                //Expected Result: User is logged in
                //step 13 end

                //Step 14. Search in My Ideas for Published Idea
                //Expected Result: User is taken directly to Published Idea
                //step 14 end
                
                //Step 15. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 15 end

                //Clean-up
                dbUtility.DeleteIdeaByIdeaNumber(newIdeaID);
                //clean-up end
            }
        }

        [Test]
        public void Test_TC4706()
        {
            RallyTestID = "TC4706";
            //US6652: Edit List of Categories
            //Create category, edit, and delete
            
            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");

                //Step 1. Navigate to webpage http://hcatest.xpxcloud.com/
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                
                //step 1 end

                //Step 2. Login as Administrator
                //Expected Result: Administrator is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Navigate to Categories
                //Expected Result: Categories tab is displayed
                IMHome.GotoCategories();
                //step 3 end

                //Step 4. Create category
                //Expected Result: Category is created
                page_objects.imCategories IMCategories = new page_objects.imCategories(IMHome.browser);
                IMCategories.newCategoryName.Type("TC4706-" + nameSuffix);
                string createOp = IMCategories.SelectLastOptionOnDropDown(IMCategories.newCategoryOpportunity);
                IMCategories.newCategoryCreateButton.Click();
                HpgElement categoryRow = IMCategories.categoryRow("TC4706-" + nameSuffix);
                //HpgAssert.Exists(categoryRow, "Verify Category was created and exists in the list");
                HpgAssert.True(categoryRow.Element.Exists(), "Verify Category was created and exists in the list");
                //step 4 end

                //Step 5. Edit category and Save
                //Expected Result: Changes are saved
                categoryRow.Element.FindLink("Edit").Click();
                page_objects.imEditCategory IMEditCategory = new page_objects.imEditCategory(IMCategories.browser);
                HpgAssert.Contains(IMEditCategory.pageHeader.Text, "Edit Category", "Verify 'Edit Category' page is loaded");
                IMEditCategory.CategoryName.Type("CHANGE - TC4706-" + nameSuffix);
                IMEditCategory.SaveButton.Click();
                categoryRow = IMCategories.categoryRow("CHANGE - TC4706-" + nameSuffix);
                HpgAssert.True(categoryRow.Element.Exists(), "Verify Category was changed and exists in the list");
                //step 5 end

                //Step 6. Edit category and Cancel
                //Expected Result: Changes are NOT saved
                categoryRow.Element.FindLink("Edit").Click();
                HpgAssert.Contains(IMEditCategory.pageHeader.Text, "Edit Category", "Verify 'Edit Category' page is loaded");
                IMEditCategory.CategoryName.Type("NOCHANGE - TC4706-" + nameSuffix);
                IMEditCategory.BackButton.Click();
                HpgAssert.True(categoryRow.Element.Exists(), "Verify idea was not changed and exists in the list");
                //step 6 end

                //Step 7. Delete category
                //Expected Result: Category is no longer listed
                categoryRow.Element.FindLink("Delete").Click();
                IMEditCategory.browser.FindButton("Confirm Delete").Click();
                HpgAssert.True(categoryRow.Element.Missing(), "Verify category is no longer displayed");
                //step 7 end
                
                //Step 8. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                //step 8 end
            }
        }

        [Test][Ignore]
        public void Test_TC4707()
        {
            //TODO: Determine if test is still valid            
            RallyTestID = "TC4707";
            RallyFailVerdict = Rally.Verdict.Blocked;
            HpgAssert.Fail("Currently unable to publish an idea");
            //US6653: Create Category while publishing Idea
            //Create idea, Submit, Approve, Publish, Verify ability to create new category

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");

                //Step 1. Navigate to webpage http://hcatest.xpxcloud.com/
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Standard User1
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                IMLogin.GotoSubmitAnIdea();
                page_objects.imSubmitAnIdea IMSubmitIdea = new page_objects.imSubmitAnIdea(IMLogin.browser);
                IMSubmitIdea.FillFormField("IdeaName", "Automation TC4707 " + nameSuffix);
                IMSubmitIdea.FillFormField("Description", "Automation Description for TC4707 " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactName", "Automation Contact Name " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactEmail", "HPG.automation@HCAHealthcare.com");
                IMSubmitIdea.FillFormField("ContactPhone", "615-344-3000");
                IMSubmitIdea.SubmitSaveButton.Click();
                page_objects.imEditIdea IMEditIdea = new page_objects.imEditIdea(IMSubmitIdea.browser);
                string newItemID = IMEditIdea.IdeaID.Text.Trim();
                IMEditIdea.SubmitSubmitButton.Click();
                //IMLogin.LoginAs("Automation-Admin");

                //Step 3. Approve the submitted idea
                //Expected Result: Idea is marked as 'Approved'
                page_objects.imMyIdeas IMMyIdeas = new page_objects.imMyIdeas(IMHome.browser);
                IMMyIdeas.GoToIdeaNumber(newItemID);
                page_objects.imIdeaDetailsAdmin IMIdeaDetails = new page_objects.imIdeaDetailsAdmin(IMMyIdeas.browser);
                HpgAssert.Contains(IMIdeaDetails.pageHeader.Text, "Idea Details", "Verify Idea Details page is loaded");
                IMIdeaDetails.AcceptButton.Click();
                page_objects.imIdeaAccept IMIdeaAccept = new page_objects.imIdeaAccept(IMIdeaDetails.browser);
                string ideaComments = "Idea Accepted by Admin on " + DateTime.Now.ToString("F");
                IMIdeaAccept.AcceptComments.Type(ideaComments);
                IMIdeaAccept.ConfirmButton.Click();
                IMIdeaAccept.GotoMyIdeas();
                System.Threading.Thread.Sleep(5000);
                //step 3 end

                //Step 4. Go to Publish Idea
                //Expected Result: Publish Idea page is displayed
                IMMyIdeas.GoToIdeaNumber(newItemID);
                for (int i = 0; i < 5; i++)
                {
                    if (IMIdeaDetails.PublishIdeaButton.Exists())
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(5000);
                    IMMyIdeas.GoToIdeaNumber(newItemID);
                    IMMyIdeas.pageHeader.Element.SendKeys(OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.F5);
                }
                HpgAssert.Exists(IMIdeaDetails.PublishIdeaButton, "Verify Publish Button is present");
                IMIdeaDetails.PublishIdeaButton.Click();
                page_objects.im3PV im3Pv = new page_objects.im3PV(IMIdeaDetails.browser);
                //step 4 end

                //Step 5. Verify Category Create/Edit button is available and click
                //Expected Result: Button is available and user it taken to Category Edit Page
                //HpgAssert.Exists(im3PV.CreateEditCategoriesButton, "Verify the Create/Edit Categories button is available");
                //im3PV.CreateEditCategoriesButton.Click();
                page_objects.imCategories IMCategories = new page_objects.imCategories(im3Pv.browser);
                HpgAssert.Contains(IMCategories.pageHeader.Text, "List of Categories", "Verify user is taken to Categories Page");
                //step 5 end

                //Step 6. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                //step 6 end
            }
        }

        [Test][Ignore]
        public void Test_TC4709()
        {
            RallyTestID = "TC4709";
            RallyFailVerdict = Rally.Verdict.Blocked;
            HpgAssert.Fail("No longer a Published Date displayed on Published Ideas List");
            //US6808: Verify sort order of Published Date
            //Go to Published Ideas, Sort by Published Date, Verify sort order

            foreach (KeyValuePair<string, Browser> testBrowser in ffChromeOnly)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();

                //Step 1. Navigate to webpage http://hcatest.xpxcloud.com/
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Standard User1
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Navigate to Published Ideas
                //Expected Result: Published Ideas page is displayed
                //IMHome.browser.Visit("/Idea/Published/");
                //System.Threading.Thread.Sleep(30000);
                //waitForBrowser((BrowserSession) CurrentBrowser);
                //System.Threading.Thread.Sleep(30000);
                IMHome.GotoPublishedIdeas();
                page_objects.imPublishedIdeas IMPublishedIdeas = new page_objects.imPublishedIdeas(IMHome.browser);
                //step 3 end

                //Step 4. Go to Publish Idea
                //Expected Result: Publish Idea page is displayed
                //step 4 end

                //Step 5. Sort ascending and verify
                //Expected Result: Ideas are sorted ascending by Published Date
                RallyFailVerdict = Rally.Verdict.Fail;
                IMPublishedIdeas.SortIdeasBy("Published Date");
                //waitForBrowser((BrowserSession)CurrentBrowser);
                WriteReport("Gathering DataTable at " + DateTime.Now.ToString("yyyyMMddhhmmss"));
                DataTable allIdeas = IMPublishedIdeas.GetPublishedIdeasDT();
                WriteReport("DataTable gathered at " + DateTime.Now.ToString("yyyyMMddhhmmss"));
                DataView dv = allIdeas.DefaultView;
                DataView sDV = allIdeas.DefaultView;
                HpgAssert.True(dv.Table.Rows.Count == sDV.Table.Rows.Count, "Verify the number of rows in each table are equal");
                for (int i = 0; i < dv.Table.Rows.Count; i++)
                {
                    if (!dv[i]["PublishedDate"].Equals(sDV[i]["PublishedDate"]))
                    {
                        HpgAssert.Fail("No match on line " + i + " of table (" + dv[i]["PublishedDate"].ToString() + " != " + sDV[i]["PublishedDate"].ToString() + ")");
                    }
                }
                WriteReport("Sorted table matches sorted results (Ascending)");
                //step 5 end

                //Step 6. Sort descending and verify
                //Expected Result: Ideas are sorted descending by Published Date
                IMPublishedIdeas.SortIdeasBy("Published Date", "descending");
                WriteReport("Gathering DataTable at " + DateTime.Now.ToString("yyyyMMddhhmmss"));
                allIdeas = IMPublishedIdeas.GetPublishedIdeasDT();
                WriteReport("DataTable gathered at " + DateTime.Now.ToString("yyyyMMddhhmmss"));
                dv = allIdeas.DefaultView;
                sDV = allIdeas.DefaultView;
                dv.Sort = "PublishedDate DESC";
                HpgAssert.True(dv.Table.Rows.Count == sDV.Table.Rows.Count, "Verify the number of rows in each table are equal");
                for (int i = 0; i < dv.Table.Rows.Count; i++)
                {
                    if (!dv[i]["PublishedDate"].Equals(sDV[i]["PublishedDate"]))
                    {
                        HpgAssert.Fail("No match on line " + i + " of table (" + dv[i]["PublishedDate"].ToString() + " != " + sDV[i]["PublishedDate"].ToString() + ")");
                    }
                }
                WriteReport("Sorted table matches sorted results (Descending)");
                //step 6 end

                //Step 7. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // --  Achieved via foreach browser loop
                //step 7 end
            }
        }

        [Test][Ignore]
        public void Test_TC4710()
        {
            //TODO: Determine if test is still valid            
            //US6210: Suspend Published Idea
            //Create idea, Submit, Approve, Publish, Suspend, Verify idea is suspended

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");

                //Step 1. Navigate to webpage http://hcatest.xpxcloud.com/
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Standard User1
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Create an Idea and Submit
                //Expected Result: Idea is created with all information
                IMLogin.GotoSubmitAnIdea();
                page_objects.imSubmitAnIdea IMSubmitIdea = new page_objects.imSubmitAnIdea(IMLogin.browser);
                IMSubmitIdea.FillFormField("IdeaName", "Automation TC4710 " + nameSuffix);
                IMSubmitIdea.FillFormField("Description", "Automation Description for TC4710 " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactName", "Automation Contact Name " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactEmail", "HPG.automation@HCAHealthcare.com");
                IMSubmitIdea.FillFormField("ContactPhone", "615-344-3000");
                IMSubmitIdea.SubmitSaveButton.Click();
                page_objects.imEditIdea IMEditIdea = new page_objects.imEditIdea(IMSubmitIdea.browser);
                string newItemID = IMEditIdea.IdeaID.Text.Trim();
                IMEditIdea.SubmitSubmitButton.Click();
                //step 3 end

                //Step 4. Login as Administrator
                //Expected Result: Administrator is logged in
                IMLogin.LoginAs("Automation-Admin");
                //step 4 end

                //Step 5. Approve the submitted idea
                //Expected Result: Idea is marked as 'Approved'
                IMHome.GotoAllIdeas();
                page_objects.imMyIdeas IMMyIdeas = new page_objects.imMyIdeas(IMHome.browser);
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4710')]").Click();
                page_objects.imIdeaDetailsAdmin IMIdeaDetails = new page_objects.imIdeaDetailsAdmin(IMMyIdeas.browser);
                HpgAssert.Contains(IMIdeaDetails.pageHeader.Text, "Idea Details", "Verify Idea Details page is loaded");
                IMIdeaDetails.AcceptButton.Click();
                page_objects.imIdeaAccept IMIdeaAccept = new page_objects.imIdeaAccept(IMIdeaDetails.browser);
                string ideaComments = "Idea Accepted by Automation-Admin on " + DateTime.Now.ToString("F");
                IMIdeaAccept.AcceptComments.Type(ideaComments);
                IMIdeaAccept.ConfirmButton.Click();
                //step 5 end

                //Step 6. Publish Idea
                //Expected Result: Idea is published
                IMMyIdeas.GoToIdeaNumber(newItemID);
                IMIdeaDetails.PublishIdeaButton.Click();
                page_objects.im3PV im3Pv = new page_objects.im3PV(IMIdeaDetails.browser);
                //im3PV.SelectLastOptionOnDropDown(im3PV.IdeaCategory);
                //im3PV.SelectLastOptionOnDropDown(im3PV.IdeaDepartment);
                //im3PV.SelectLastOptionOnDropDown(im3PV.IdeaEffort);
                //im3PV.SelectLastOptionOnDropDown(im3PV.IdeaImpact);
                //im3PV.IdeaFirstName.Type("Robot");
                //im3PV.IdeaLastName.Type("Automation");
                //im3PV.IdeaPhone.Type("515-555-1212");
                //im3PV.IdeaEmail.Type("hpg.automation@hcahealthcare.com");
                //im3PV.PublishButton.Click();
                HpgAssert.True(im3Pv.browser.FindXPath("//div[@class='validation-summary-errors']/span[contains(.,'fix the following errors')]").Missing(), "Verify no errors are present after publish");
                //step 6 end

                //Step 7. Suspend Idea with comments
                //Expected Result: Idea is suspended with comments
                IMMyIdeas.GoToIdeaNumber(newItemID);
                //IMIdeaDetails.SuspendButton.Click();
                page_objects.imSuspendIdea IMSuspendIdea = new page_objects.imSuspendIdea(IMIdeaDetails.browser);
                IMSuspendIdea.comment.Type("Suspended by Automation-Admin on " + DateTime.Now.ToString("F"));
                IMSuspendIdea.ConfirmButton.Click();
                //step 7 end

                //Step 8. Login as Standard User1
                //Expected Result: User is logged in
                IMLogin.LoginAs("Automation-S1");
                //step 8 end

                //Step 9. Verify idea is marked as Suspended
                //Expected Result: Idea is marked as 'Suspended' with comments
                IMMyIdeas.GoToIdeaNumber(newItemID);
                HpgAssert.Contains(IMIdeaDetails.IdeaID.Text, newItemID, "Verify new Idea Details Page is loaded (IdeaID)");
                HpgAssert.Contains(IMIdeaDetails.IdeaStatus.Text, "Suspended", "Verify status is marked as 'Suspended'");
                //step 9 end

                //Step 10. Verify User1 is unable to edit idea
                //Expected Result: Idea is not able to be edited
                HpgAssert.True(IMIdeaDetails.EditButton.Element.Missing(), "Verify edit button does not exists");
                //step 10 end

                //Step 11. Navigate to Published Idea
                //Expected Result: Published Idea is no longer displayed
                IMHome.GotoPublishedIdeas();
                page_objects.imPublishedIdeas IMPublishedIdeas = new page_objects.imPublishedIdeas(IMHome.browser);
                IMPublishedIdeas.ClearFilterButton.Click();
                IMPublishedIdeas.SearchIdeaName.Type(nameSuffix);
                IMPublishedIdeas.FilterButton.Click();
                HpgAssert.Contains(IMPublishedIdeas.browser.Location.ToString(), "/Idea/Published?", "Verify browser is still on results page");
                HpgAssert.True(IMPublishedIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4710')]").Missing(), "Verify Idea is not on search results");
                //step 11 end

                //Step 12. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 12 end
            }
        }

        [Test]
        public void Test_TC4711()
        {
            //US6812: Verify sort order of Category
            //Go to Published Ideas, Sort by Category, Verify sort order

            foreach (KeyValuePair<string, Browser> testBrowser in ffChromeOnly)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();

                //Step 1. Navigate to webpage http://hcatest.xpxcloud.com/
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Standard User1
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Navigate to Published Ideas
                //Expected Result: Published Ideas page is displayed
                IMHome.GotoPublishedIdeas();
                waitForBrowser((BrowserSession) CurrentBrowser);
                page_objects.imPublishedIdeas IMPublishedIdeas = new page_objects.imPublishedIdeas(IMHome.browser);
                //step 3 end

                //Step 4. Go to Publish Idea
                //Expected Result: Publish Idea page is displayed
                //step 4 end

                //Step 5. Sort ascending and verify
                //Expected Result: Ideas are sorted ascending by Category
                IMPublishedIdeas.SortIdeasBy("Category");
                WriteReport("Gathering DataTable at " + DateTime.Now.ToString("yyyyMMddhhmmss"));
                DataTable allIdeas = IMPublishedIdeas.GetPublishedIdeasDT();
                WriteReport("DataTable gathered at " + DateTime.Now.ToString("yyyyMMddhhmmss"));
                DataView dv = allIdeas.DefaultView;
                DataView sDV = allIdeas.DefaultView;
                dv.Sort = "Category";
                for (int i = 0; i < dv.Table.Rows.Count; i++)
                {
                    if (!dv[i]["Category"].Equals(sDV[i]["Category"]))
                    {
                        HpgAssert.Fail("No match on line " + i + " of table (" + dv[i]["Category"].ToString() + " != " + sDV[i]["Category"].ToString() + ")");
                    }
                }
                WriteReport("Sorted table matches sorted results (Ascending)");
                //step 5 end

                //Step 6. Sort descending and verify
                //Expected Result: Ideas are sorted descending by Category
                IMPublishedIdeas.SortIdeasBy("Category", "descending");
                WriteReport("Gathering DataTable at " + DateTime.Now.ToString("yyyyMMddhhmmss"));
                allIdeas = IMPublishedIdeas.GetPublishedIdeasDT();
                WriteReport("DataTable gathered at " + DateTime.Now.ToString("yyyyMMddhhmmss"));
                dv = allIdeas.DefaultView;
                sDV = allIdeas.DefaultView;
                dv.Sort = "Category DESC";
                for (int i = 0; i < dv.Table.Rows.Count; i++)
                {
                    if (!dv[i]["Category"].Equals(sDV[i]["Category"]))
                    {
                        HpgAssert.Fail("No match on line " + i + " of table (" + dv[i]["Category"].ToString() + " != " + sDV[i]["Category"].ToString() + ")");
                    }
                }
                WriteReport("Sorted table matches sorted results (Descending)");
                //step 6 end

                //Step 7. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // --  Achieved via foreach browser loop
                //step 7 end
            }
        }

        [Test]
        public void Test_TC4826()
        {
            //US6809: Verify Publish Idea values on Published Idea details page
            //Create idea, Submit, Approve, Publish, verify details match

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");
                DBUtility dbUtility = new DBUtility();

                //Step 1. Navigate to webpage http://hcatest.xpxcloud.com/
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Standard User1
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Create an Idea and Submit
                //Expected Result: Idea is created with all information
                int newIdeaID = dbUtility.CreateSavedIdea("Automation TC4826 " + nameSuffix,
                                          "Automation Description for TC4826" + nameSuffix,
                                          "",
                                          "Automation Contact Name " + nameSuffix);
                //step 3 end

                //Step 4. Login as Administrator
                //Expected Result: Administrator is logged in
                // -- Already done via step 2
                //step 4 end

                //Step 5. Approve the submitted idea
                //Expected Result: Idea is marked as 'Approved'
                dbUtility.AcceptIdea(newIdeaID);
                //step 5 end

                //Step 6. Navigate to Publish Idea page
                //Expected Result: Publish Idea Page is displayed
                IMHome.GoToIdeaNumber(newIdeaID.ToString());
                page_objects.imIdeaDetailsAdmin IMIdeaDetails = new page_objects.imIdeaDetailsAdmin(IMHome.browser);
                IMIdeaDetails.PublishIdeaButton.Click();
                page_objects.im3PV publishIdea = new page_objects.im3PV(IMIdeaDetails.browser);
                //step 6 end

                //Step 7. Edit details and publish idea
                //Expected Result: Idea is saved and published
                Dictionary<string, string> newValues = new Dictionary<string, string>();
                Dictionary<string, im3PV.IdeaValues> ideaValues = new Dictionary<string, im3PV.IdeaValues>();
                ideaValues.Add("GPO", publishIdea.GetIdeaValues(1));
                string newValue = "";
                foreach (KeyValuePair<string, im3PV.IdeaValues> ideaValue in ideaValues)
                {
                    //TODO: Action/Status

                    // -- Title
                    newValue = ideaValue.Value.Title.Text + " -- TC4826:" + ideaValue.Key + nameSuffix;
                    ideaValue.Value.Title.Type(newValue);
                    newValues.Add(ideaValue.Key + "Title", newValue);

                    // -- Category
                    newValue = ideaValue.Value.Category.Element.FindXPath(
                        "option[.!='" + ideaValue.Value.Category.Element.SelectedOption + "']",
                        Options.First).Text;
                    publishIdea.FillFormField(ideaValue.Value.Category, newValue);
                    newValues.Add(ideaValue.Key + "Category", newValue);

                    // -- Department
                    newValue = ideaValue.Value.Department.Element.FindXPath(
                        "option[.!='" + ideaValue.Value.Department.Element.SelectedOption + "']",
                        Options.First).Text;
                    publishIdea.FillFormField(ideaValue.Value.Department, newValue);
                    newValues.Add(ideaValue.Key + "Department", newValue);

                    // -- Impact
                    newValue = ideaValue.Value.Impact.Element.FindXPath(
                        "option[.!='" + ideaValue.Value.Impact.Element.SelectedOption + "']",
                        Options.First).Text;
                    publishIdea.FillFormField(ideaValue.Value.Impact, newValue);
                    newValues.Add(ideaValue.Key + "Impact", newValue);

                    // -- Effort
                    newValue = ideaValue.Value.Effort.Element.FindXPath(
                        "option[.!='" + ideaValue.Value.Effort.Element.SelectedOption + "']",
                        Options.First).Text;
                    publishIdea.FillFormField(ideaValue.Value.Effort, newValue);
                    newValues.Add(ideaValue.Key + "Effort", newValue);

                    // -- Description
                    newValue = ideaValue.Value.Description.Text + " -- TC4826:" + ideaValue.Key + nameSuffix;
                    ideaValue.Value.Description.Type(newValue);
                    newValues.Add(ideaValue.Key + "Description", newValue);

                    // -- Results
                    newValue = ideaValue.Value.Results.Text + " -- TC4826:" + ideaValue.Key + nameSuffix;
                    ideaValue.Value.Results.Type(newValue);
                    newValues.Add(ideaValue.Key + "Results", newValue);
                }
                WriteReport("New values entered, saving...");
                System.Threading.Thread.Sleep(3000);
                publishIdea.SaveAllButton.Click();
                System.Threading.Thread.Sleep(3000);
                publishIdea.SaveAllButton.Click();
                System.Threading.Thread.Sleep(3000);
                dbUtility.PublishQualifiedIdea(newIdeaID);
                //step 7 end

                //Step 8. Verify Published Idea detials are correct
                //Expected Result: Published Idea details are correct
                IMHome.GoToPublishedIdeaNumber(newIdeaID.ToString());
                page_objects.imPublishedIdea publishedIdea = new imPublishedIdea(IMHome.browser);
                HpgAssert.AreEqual(newValues["GPOTitle"].Trim(), publishedIdea.IdeaTitle.Text.Trim(), "Verify Title is correct");
                HpgAssert.AreEqual(newValues["GPOCategory"], publishedIdea.Category.Text, "Verify Category is correct");
                HpgAssert.AreEqual(newValues["GPODepartment"], publishedIdea.Department.Text, "Verify Department is correct");
                HpgAssert.AreEqual(newValues["GPOImpact"], publishedIdea.GetImpactLevel(), "Verify Impact is correct");
                HpgAssert.AreEqual(newValues["GPOEffort"], publishedIdea.GetEffortLevel(), "Verify Effort is correct");
                HpgAssert.AreEqual(newValues["GPODescription"].Trim(), publishedIdea.Description.Text.Trim(), "Verify Description is correct");
                //step 8 end

                //Clean-up
                //Delete idea completely
                dbUtility.DeleteIdeaByIdeaNumber(newIdeaID);
                //clean-up end

                //Step 9. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 9 end
            }
        }

        [Test]
        public void Test_TC4837()
        {
            //US6810: Verify Effort is visible and sortable on Published Ideas List, visible on Idea Details
            //Go to Published Ideas, Sort by Effort Verify sort order, Verify present on Idea Details

            foreach (KeyValuePair<string, Browser> testBrowser in ffChromeOnly)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();

                //Step 1. Navigate to webpage http://hcatest.xpxcloud.com/
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Standard User1
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Navigate to Published Ideas
                //Expected Result: Published Ideas page is displayed
                IMHome.GotoPublishedIdeas();
                page_objects.imPublishedIdeas IMPublishedIdeas = new page_objects.imPublishedIdeas(IMHome.browser);
                //step 3 end

                //Step 4. Go to Publish Idea
                //Expected Result: Publish Idea page is displayed
                //step 4 end

                //Step 5. Sort ascending and verify
                //Expected Result: Ideas are sorted ascending by Effort
                waitForBrowser((BrowserSession)CurrentBrowser);
                IMPublishedIdeas.SortIdeasBy("Effort");
                WriteReport("Gathering DataTable at " + DateTime.Now.ToString("yyyyMMddhhmmss"));
                DataTable allIdeas = IMPublishedIdeas.GetPublishedIdeasDT();
                WriteReport("DataTable gathered at " + DateTime.Now.ToString("yyyyMMddhhmmss"));
                DataView dv = allIdeas.DefaultView;
                DataView sDV = allIdeas.DefaultView;
                sDV.Sort = "Effort";
                for (int i = 0; i < dv.Table.Rows.Count; i++)
                {
                    if (!dv[i]["Effort"].Equals(sDV[i]["Effort"]))
                    {
                        HpgAssert.Fail("No match on line " + i + " of table (" + dv[i]["Effort"].ToString() + " != " + sDV[i]["Effort"].ToString() + ")");
                    }
                }
                WriteReport("Sorted table matches sorted results (Ascending)");
                //step 5 end

                //Step 6. Sort descending and verify
                //Expected Result: Ideas are sorted descending by Effort
                waitForBrowser((BrowserSession)CurrentBrowser);
                IMPublishedIdeas.SortIdeasBy("Effort", "DESCENDING");
                WriteReport("Gathering DataTable at " + DateTime.Now.ToString("yyyyMMddhhmmss"));
                allIdeas = IMPublishedIdeas.GetPublishedIdeasDT();
                WriteReport("DataTable gathered at " + DateTime.Now.ToString("yyyyMMddhhmmss"));
                dv = allIdeas.DefaultView;
                sDV = allIdeas.DefaultView;
                sDV.Sort = "Effort DESC";
                for (int i = 0; i < dv.Table.Rows.Count; i++)
                {
                    if (!dv[i]["Effort"].Equals(sDV[i]["Effort"]))
                    {
                        HpgAssert.Fail("No match on line " + i + " of table (" + dv[i]["Effort"].ToString() + " != " + sDV[i]["Effort"].ToString() + ")");
                    }
                }
                WriteReport("Sorted table matches sorted results (Descending)");
                //step 6 end

                //Step 7. Navigate to Published Idea
                //Expected Result: Verify presence of Idea Effort
                IMPublishedIdeas.browser.FindId("publishIdeasTable").FindXPath(".//td/a", Options.First).Click();
                page_objects.imPublishedIdea IMPublishedIdea = new imPublishedIdea(IMPublishedIdeas.browser);
                HpgAssert.True(IMPublishedIdea.EffortLevel.Element.Exists(), "Verify Effort Level is present on page");
                //step 7 end

                //Step 8. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achived via foreach browser loop
                //step 8 end
            }
        }

        [Test]
        public void Test_TC4838()
        {
            //US7120: Verify Updated Date is visible and sortable on Published Ideas List, visible on Idea Details
            //Go to Published Ideas, Sort by Updated Date Verify sort order, Verify present on Idea Details

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();

                //Step 1. Navigate to webpage http://hcatest.xpxcloud.com/
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Standard User1
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Navigate to Published Ideas
                //Expected Result: Published Ideas page is displayed
                IMHome.GotoPublishedIdeas();
                page_objects.imPublishedIdeas IMPublishedIdeas = new page_objects.imPublishedIdeas(IMHome.browser);
                //step 3 end

                //Step 4. Go to Publish Idea
                //Expected Result: Publish Idea page is displayed
                //step 4 end

                //Step 5. Sort ascending and verify
                //Expected Result: Ideas are sorted ascending by Updated Date
                waitForBrowser((BrowserSession)CurrentBrowser);
                IMPublishedIdeas.SortIdeasBy("Updated");
                DataTable allIdeas = IMPublishedIdeas.GetPublishedIdeasDT();
                DataView dv = allIdeas.DefaultView;
                DataView sDV = allIdeas.DefaultView;
                sDV.Sort = "UpdatedDate";
                for (int i = 0; i < dv.Table.Rows.Count; i++)
                {
                    if (!dv[i]["UpdatedDate"].Equals(sDV[i]["UpdatedDate"]))
                    {
                        HpgAssert.Fail("No match on line " + i + " of table (" + dv[i]["UpdatedDate"].ToString() + " != " + sDV[i]["UpdatedDate"].ToString() + ")");
                    }
                }
                WriteReport("Sorted table matches sorted results (Ascending)");
                //step 5 end

                //Step 6. Sort descending and verify
                //Expected Result: Ideas are sorted descending by Updated Date
                waitForBrowser((BrowserSession)CurrentBrowser);
                IMPublishedIdeas.SortIdeasBy("Updated", "DESCENDING");
                allIdeas = IMPublishedIdeas.GetPublishedIdeasDT();
                dv = allIdeas.DefaultView;
                sDV = allIdeas.DefaultView;
                sDV.Sort = "UpdatedDate DESC";
                for (int i = 0; i < dv.Table.Rows.Count; i++)
                {
                    if (!dv[i]["UpdatedDate"].Equals(sDV[i]["UpdatedDate"]))
                    {
                        HpgAssert.Fail("No match on line " + i + " of table (" + dv[i]["UpdatedDate"].ToString() + " != " + sDV[i]["UpdatedDate"].ToString() + ")");
                    }
                }
                WriteReport("Sorted table matches sorted results (Descending)");
                //step 6 end

                //Step 7. Navigate to Published Idea
                //Expected Result: Verify presence of Idea Updated Date
                //IMPublishedIdeas.browser.FindId("publishIdeasTable").FindXPath(".//td/a", Options.First).Hover();
                //IMPublishedIdeas.browser.FindId("publishIdeasTable").FindXPath(".//td/a", Options.First).SendKeys(OpenQA.Selenium.Keys.Space);
                IMPublishedIdeas.browser.FindId("publishIdeasTable").FindXPath(".//td/a", Options.First).Click();
                waitForBrowser((BrowserSession)CurrentBrowser);
                page_objects.imPublishedIdea IMPublishedIdea = new imPublishedIdea(IMPublishedIdeas.browser);
                HpgAssert.True(IMPublishedIdea.UpdatedDate.Element.Exists(), "Verify Updated Date is present on page");
                //step 7 end

                //Step 8. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achived via foreach browser loop
                //step 8 end
            }
        }

        [Test][Ignore]
        public void Test_TC4839()
        {
            //US7114: Verify Assigned User is visible and sortable on Published Ideas List, visible on Idea Details
            //Go to Published Ideas, Sort by Assigned User Verify sort order, Verify present on Idea Details

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();

                //Step 1. Navigate to webpage http://hcatest.xpxcloud.com/
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Admin
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Navigate to All Ideas
                //Expected Result: All Ideas page is displayed
                IMHome.GotoAllIdeas();
                page_objects.imMyIdeas IMMyIdeas = new page_objects.imMyIdeas(IMHome.browser);
                //step 3 end

                //Step 4. Sort ascending and verify
                //Expected Result: Ideas are sorted ascending by Assigned User
                IMMyIdeas.AllSortBy("Assigned");
                DataTable allIdeas = IMMyIdeas.GetAllIdeasDT();
                DataView dv = allIdeas.DefaultView;
                DataView sDV = allIdeas.DefaultView;
                sDV.Sort = "Assigned";
                for (int i = 0; i < dv.Table.Rows.Count; i++)
                {
                    if (!dv[i]["Assigned"].Equals(sDV[i]["Assigned"]))
                    {
                        HpgAssert.Fail("No match on line " + i + " of table (" + dv[i]["Assigned"].ToString() + " != " + sDV[i]["Assigned"].ToString() + ")");
                    }
                }
                WriteReport("Sorted table matches sorted results (Ascending)");
                //step 4 end

                //Step 5. Sort descending and verify
                //Expected Result: Ideas are sorted descending by Assigned User
                IMMyIdeas.AllSortBy("Assigned", "DESCENDING");
                allIdeas = IMMyIdeas.GetAllIdeasDT();
                dv = allIdeas.DefaultView;
                sDV = allIdeas.DefaultView;
                sDV.Sort = "Assigned DESC";
                for (int i = 0; i < dv.Table.Rows.Count; i++)
                {
                    if (!dv[i]["Assigned"].Equals(sDV[i]["Assigned"]))
                    {
                        HpgAssert.Fail("No match on line " + i + " of table (" + dv[i]["Assigned"].ToString() + " != " + sDV[i]["Assigned"].ToString() + ")");
                    }
                }
                WriteReport("Sorted table matches sorted results (Ascending)");
                //step 5 end

                //Step 6. Navigate to Idea Details
                //Expected Result: Verify presence of Assigned User
                // -- Excluded from test due to changes
                //step 6 end

                //Step 7. Repeat for My Ideas
                //Expected Result: Results are same
                IMHome.GotoMyIdeas();
                IMMyIdeas.MySortBy("Assigned");
                allIdeas = IMMyIdeas.GetMyIdeasDT();
                dv = allIdeas.DefaultView;
                sDV = allIdeas.DefaultView;
                sDV.Sort = "Assigned";
                for (int i = 0; i < dv.Table.Rows.Count; i++)
                {
                    if (!dv[i]["Assigned"].Equals(sDV[i]["Assigned"]))
                    {
                        HpgAssert.Fail("No match on line " + i + " of table (" + dv[i]["Assigned"].ToString() + " != " + sDV[i]["Assigned"].ToString() + ")");
                    }
                }
                WriteReport("Sorted table matches sorted results (Ascending)");
                IMMyIdeas.MySortBy("Assigned", "DESCENDING");
                allIdeas = IMMyIdeas.GetMyIdeasDT();
                dv = allIdeas.DefaultView;
                sDV = allIdeas.DefaultView;
                sDV.Sort = "Assigned DESC";
                for (int i = 0; i < dv.Table.Rows.Count; i++)
                {
                    if (!dv[i]["Assigned"].Equals(sDV[i]["Assigned"]))
                    {
                        HpgAssert.Fail("No match on line " + i + " of table (" + dv[i]["Assigned"].ToString() + " != " + sDV[i]["Assigned"].ToString() + ")");
                    }
                }
                WriteReport("Sorted table matches sorted results (Ascending)");
                //step 7 end

                //Step 8. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 8 end
            }
        }

        [Test][Ignore]
        public void Test_TC4847()
        {
            //US7134: Verify ability to associate idea directly from Idea Detail screen
            //Create idea, Submit, Approve, Associate from Idea Detail page

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");

                //Step 1. Navigate to webpage http://hcatest.xpxcloud.com/
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Standard User1
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Create Idea and Submit
                //Expected Result: Idea is created with all information
                IMLogin.GotoSubmitAnIdea();
                page_objects.imSubmitAnIdea IMSubmitIdea = new page_objects.imSubmitAnIdea(IMLogin.browser);
                IMSubmitIdea.FillFormField("IdeaName", "Automation TC4847 " + nameSuffix);
                IMSubmitIdea.FillFormField("Description", "Automation Description for TC4847 " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactName", "Automation Contact Name " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactEmail", "HPG.automation@HCAHealthcare.com");
                IMSubmitIdea.FillFormField("ContactPhone", "615-344-3000");
                IMSubmitIdea.SubmitSaveButton.Click();
                page_objects.imEditIdea IMEditIdea = new page_objects.imEditIdea(IMSubmitIdea.browser);
                string newItemID = IMEditIdea.IdeaID.Text.Trim();
                IMEditIdea.SubmitSubmitButton.Click();
                //step 3 end

                //Step 4. Login as Administrator
                //Expected Result: Administrator is logged in
                // -- Already logged in as Admin
                //step 4 end

                //Step 5. Accept Idea
                //Expected Result: Idea is marked as Accepted
                page_objects.imMyIdeas IMMyIdeas = new page_objects.imMyIdeas(IMHome.browser);
                IMMyIdeas.GoToIdeaNumber(newItemID);
                page_objects.imIdeaDetailsAdmin IMIdeaDetails = new page_objects.imIdeaDetailsAdmin(IMMyIdeas.browser);
                HpgAssert.Contains(IMIdeaDetails.pageHeader.Text, "Idea Details", "Verify Idea Details page is loaded");
                IMIdeaDetails.AcceptButton.Click();
                page_objects.imIdeaAccept IMIdeaAccept = new page_objects.imIdeaAccept(IMIdeaDetails.browser);
                string ideaComments = "Idea Accepted by Admin on " + DateTime.Now.ToString("F");
                IMIdeaAccept.AcceptComments.Type(ideaComments);
                IMIdeaAccept.ConfirmButton.Click();
                //step 5 end

                //Step 6. Verify ability to associate idea from Idea List page
                //Expected Result: Associate Idea function is available from Idea List page
                IMHome.GotoAllIdeas();
                System.Threading.Thread.Sleep(5000);
                IMMyIdeas.SearchFilterForm.Type(nameSuffix);
                IMMyIdeas.FilterButton.Click();
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4847')]/../../td[8]").FindLink("Action").Click();  //Click on Action
                HpgAssert.True(
                    IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4847')]/../../td[8]")
                             .FindLink("Associate Idea")
                             .Exists(), "Verify Associate Action is available");
                //step 6 end

                //Step 7. Verify ability to associate idea from Idea Detail page
                //Expected Result: Associate Idea function is available from Idea Detail page
                IMMyIdeas.browser.FindXPath("//table//td[2]/a[contains(.,'TC4847')]/../../td[8]").FindLink("Details").Click(); //Click on Details
                HpgAssert.Exists(IMIdeaDetails.AssociateIdeaButton, "Verify Associate Button exists on Idea Details page");
                //step 7 end

                //Step 8. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achived via foreach browser loop
                //step 8 end
            }
        }

        [Test]
        public void Test_TC4953()
        {
            //RallyTestID = "TC4953";
            //US7530: Edit List of Departments
            //Create department, edit, and delete

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");

                //Pre-Clean
                Utility.DBUtility dbUtility = new DBUtility();
                dbUtility.DeleteDepartmentByName("%TC4953%");

                //Step 1. Navigate to webpage http://hcatest.xpxcloud.com/
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Administrator
                //Expected Result: Administrator is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Navigate to Departments
                //Expected Result: Departments tab is displayed
                IMHome.GotoDepartments();
                //step 3 end

                //Step 4. Create Department
                //Expected Result: Department is created
                page_objects.imDepartments IMDepartments = new imDepartments(IMHome.browser);
                IMDepartments.newDepartmentName.Type("TC4953-" + nameSuffix);
                IMDepartments.newDepartmentCreateButton.Click();
                HpgElement departmentRow = IMDepartments.departmentRow("TC4953-" + nameSuffix);
                HpgAssert.True(departmentRow.Element.Exists(), "Verify Department was created and exists in the list");
                //step 4 end

                //Step 5. Edit Department and Save
                //Expected Result: Changes are saved
                departmentRow.Element.FindLink("Edit").Click();
                page_objects.imEditDepartment IMEditDepartment = new imEditDepartment(IMDepartments.browser);
                HpgAssert.Contains(IMEditDepartment.pageHeader.Text, "Edit Department", "Verify 'Edit Department' page is loaded");
                IMEditDepartment.DepartmentName.Type("CHANGE - TC4953-" + nameSuffix);
                IMEditDepartment.SaveButton.Click();
                departmentRow = IMDepartments.departmentRow("CHANGE - TC4953-" + nameSuffix);
                HpgAssert.True(departmentRow.Element.Exists(), "Verify Department was changed and exists in the list");
                //step 5 end

                //Step 6. Edit Department and Cancel
                //Expected Result: Changes are NOT saved
                departmentRow.Element.FindLink("Edit").Click();
                HpgAssert.Contains(IMEditDepartment.pageHeader.Text, "Edit Department", "Verify 'Edit Department' page is loaded");
                IMEditDepartment.DepartmentName.Type("NOCHANGE - TC4953-" + nameSuffix);
                IMEditDepartment.BackButton.Click();
                HpgAssert.True(departmentRow.Element.Exists(), "Verify Department was changed and exists in the list");
                //step 6 end

                //Step 7. Delete Department
                //Expected Result: Category is no longer listed
                departmentRow.Element.FindLink("Delete").Click();
                IMEditDepartment.browser.FindButton("Confirm Delete").Click();
                HpgAssert.True(departmentRow.Element.Missing(), "Verify Department is no longer displayed");
                //step 7 end

                //Step 8. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achived via foreach browser loop
                //step 8 end

                //Clean up
                HpgAssert.AreEqual("1", dbUtility.DeleteDepartmentByName("%TC4953%").ToString(), "Delete Test Department");
                //end Clean up
            }
        }

        [Test][Ignore]
        public void Test_TC4954()
        {
            //TODO: Determine if test is still valid
            //US7530: Edit Existing Department
            //Create Idea, Submit, Assign Department, Publish, Edit Department and verify change on Idea

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");

                //Step 1. Navigate to webpage http://hcatest.xpxcloud.com/
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Administrator
                //Expected Result: Administrator is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                IMHome.GotoDepartments();
                page_objects.imDepartments IMDepartments = new imDepartments(IMHome.browser);
                IMDepartments.newDepartmentName.Type("TC4954-" + nameSuffix);
                IMDepartments.newDepartmentCreateButton.Click();
                HpgElement departmentRow = IMDepartments.departmentRow("TC4954-" + nameSuffix);
                HpgAssert.Exists(departmentRow, "Verify Department was created and exists in the list");

                //Step 3. Create Idea and Submit
                //Expected Result: Idea is Submitted
                IMLogin.GotoSubmitAnIdea();
                page_objects.imSubmitAnIdea IMSubmitIdea = new page_objects.imSubmitAnIdea(IMLogin.browser);
                IMSubmitIdea.FillFormField("IdeaName", "Automation TC4954 " + nameSuffix);
                IMSubmitIdea.FillFormField("Description", "Automation Description for TC4954 " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactName", "Automation Contact Name " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactEmail", "HPG.automation@HCAHealthcare.com");
                IMSubmitIdea.FillFormField("ContactPhone", "615-344-3000");
                IMSubmitIdea.SubmitSaveButton.Click();
                page_objects.imEditIdea IMEditIdea = new page_objects.imEditIdea(IMSubmitIdea.browser);
                string newItemID = IMEditIdea.IdeaID.Text.Trim();
                IMEditIdea.SubmitSubmitButton.Click();
                //step 3 end

                //Step 4. Accept Idea
                //Expected Result: Idea is Accepted
                page_objects.imMyIdeas IMMyIdeas = new page_objects.imMyIdeas(IMHome.browser);
                IMMyIdeas.GoToIdeaNumber(newItemID);
                page_objects.imIdeaDetailsAdmin IMIdeaDetails = new page_objects.imIdeaDetailsAdmin(IMMyIdeas.browser);
                HpgAssert.Contains(IMIdeaDetails.pageHeader.Text, "Idea Details", "Verify Idea Details page is loaded");
                IMIdeaDetails.AcceptButton.Click();
                page_objects.imIdeaAccept IMIdeaAccept = new page_objects.imIdeaAccept(IMIdeaDetails.browser);
                string ideaComments = "Idea Accepted by Admin on " + DateTime.Now.ToString("F");
                IMIdeaAccept.AcceptComments.Type(ideaComments);
                IMIdeaAccept.ConfirmButton.Click();
                waitForBrowser((BrowserSession)CurrentBrowser);
                //step 4 end

                //Step 5. Assign Department to Idea and Publish
                //Expected Result: Idea is Published
                IMMyIdeas.GoToIdeaNumber(newItemID);
                for (int i = 0; i < 5; i++)
                {
                    if (IMIdeaDetails.PublishIdeaButton.Exists())
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(5000);
                    IMMyIdeas.GoToIdeaNumber(newItemID);
                    IMMyIdeas.pageHeader.Element.SendKeys(OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.F5);
                }
                HpgAssert.Exists(IMIdeaDetails.PublishIdeaButton, "Verify Publish Button is present");
                IMIdeaDetails.PublishIdeaButton.Click();
                page_objects.im3PV im3Pv = new page_objects.im3PV(IMIdeaDetails.browser);
                //im3PV.SelectLastOptionOnDropDown(im3PV.IdeaCategory);
                ////im3PV.IdeaDepartment.SelectListOptionByText("TC4954-" + nameSuffix);
                //im3PV.SelectOptionOnDropDown(im3PV.IdeaDepartment, "TC4954-" + nameSuffix);
                //im3PV.SelectLastOptionOnDropDown(im3PV.IdeaEffort);
                //im3PV.SelectLastOptionOnDropDown(im3PV.IdeaImpact);
                //im3PV.IdeaFirstName.Type("Robot");
                //im3PV.IdeaLastName.Type("Automation");
                //im3PV.IdeaPhone.Type("515-555-1212");
                //im3PV.IdeaEmail.Type("hpg.automation@hcahealthcare.com");
                //im3PV.longClickElement(im3PV.PublishButton.Element, 120, 10);
                waitForBrowser((BrowserSession)CurrentBrowser);
                HpgAssert.True(im3Pv.browser.FindXPath("//div[@class='validation-summary-errors']/span[contains(.,'fix the following errors')]").Missing(), "Verify no errors are present after publish");
                //step 5 end

                //Step 6. Edit Department
                //Expected Result: Department is changed
                IMHome.GotoDepartments();
                departmentRow.Element.FindLink("Edit").Click();
                page_objects.imEditDepartment IMEditDepartment = new imEditDepartment(IMDepartments.browser);
                HpgAssert.Contains(IMEditDepartment.pageHeader.Text, "Edit Department", "Verify 'Edit Department' page is loaded");
                IMEditDepartment.DepartmentName.Type("CHANGE - TC4954-" + nameSuffix);
                IMEditDepartment.SaveButton.Click();
                departmentRow = IMDepartments.departmentRow("CHANGE - TC4954-" + nameSuffix);
                HpgAssert.Exists(departmentRow, "Verify Department was changed and exists in the list");
                departmentRow.Element.FindLink("Delete").Click();
                IMEditDepartment.browser.FindButton("Confirm Delete").Click();
                HpgAssert.True(departmentRow.Element.Missing(), "Verify Department is no longer displayed");
                //step 6 end

                //Step 7. Verify change on Published Idea
                //Expected Result: Published Idea reflects change to Department
                IMMyIdeas.GoToPublishedIdeaNumber(newItemID);
                page_objects.imPublishedIdea IMPublishedIdea = new imPublishedIdea(IMMyIdeas.browser);
                HpgAssert.Contains(IMPublishedIdea.Department.Text, "CHANGE - TC4954-" + nameSuffix, "Verify Department change on Published Idea Details");
                //step 7 end

                //Step 8. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achived via foreach browser loop
                //step 8 end
            }
        }

        [Test][Ignore]
        public void Test_TC4955()
        {
            //TODO: Determine if test is still valid
            //US6652: Edit Existing Category
            //Create Idea, Submit, Assign Category, Publish, Edit Category and verify change on Idea

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");

                //Step 1. Navigate to webpage http://hcatest.xpxcloud.com/
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Administrator
                //Expected Result: Administrator is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Create Category
                IMHome.GotoCategories();
                page_objects.imCategories IMCategories = new page_objects.imCategories(IMHome.browser);
                IMCategories.newCategoryName.Type("TC4955-" + nameSuffix);
                string createOp = IMCategories.SelectLastOptionOnDropDown(IMCategories.newCategoryOpportunity);
                IMCategories.newCategoryCreateButton.Click();
                HpgElement categoryRow = IMCategories.categoryRow("TC4955-" + nameSuffix);
                HpgAssert.Exists(categoryRow, "Verify Category was created and exists in the list");
                //Create Category End

                //Step 3. Create Idea and Submit
                //Expected Result: Idea is Submitted
                IMLogin.GotoSubmitAnIdea();
                page_objects.imSubmitAnIdea IMSubmitIdea = new page_objects.imSubmitAnIdea(IMLogin.browser);
                IMSubmitIdea.FillFormField("IdeaName", "Automation TC4955 " + nameSuffix);
                IMSubmitIdea.FillFormField("Description", "Automation Description for TC4955 " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactName", "Automation Contact Name " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactEmail", "HPG.automation@HCAHealthcare.com");
                IMSubmitIdea.FillFormField("ContactPhone", "615-344-3000");
                IMSubmitIdea.SubmitSaveButton.Click();
                page_objects.imEditIdea IMEditIdea = new page_objects.imEditIdea(IMSubmitIdea.browser);
                string newItemID = IMEditIdea.IdeaID.Text.Trim();
                IMEditIdea.SubmitSubmitButton.Click();
                //step 3 end

                //Step 4. Accept Idea
                //Expected Result: Idea is Accepted
                page_objects.imMyIdeas IMMyIdeas = new page_objects.imMyIdeas(IMHome.browser);
                IMMyIdeas.GoToIdeaNumber(newItemID);
                page_objects.imIdeaDetailsAdmin IMIdeaDetails = new page_objects.imIdeaDetailsAdmin(IMMyIdeas.browser);
                HpgAssert.Contains(IMIdeaDetails.pageHeader.Text, "Idea Details", "Verify Idea Details page is loaded");
                IMIdeaDetails.AcceptButton.Click();
                page_objects.imIdeaAccept IMIdeaAccept = new page_objects.imIdeaAccept(IMIdeaDetails.browser);
                string ideaComments = "Idea Accepted by Admin on " + DateTime.Now.ToString("F");
                IMIdeaAccept.AcceptComments.Type(ideaComments);
                IMIdeaAccept.ConfirmButton.Click();
                waitForBrowser((BrowserSession)CurrentBrowser);
                //step 4 end

                //Step 5. Assign Category to Idea and Publish
                //Expected Result: Idea is Published
                IMMyIdeas.GoToIdeaNumber(newItemID);
                for (int i = 0; i < 5; i++)
                {
                    if (IMIdeaDetails.PublishIdeaButton.Exists())
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(5000);
                    IMMyIdeas.GoToIdeaNumber(newItemID);
                    IMMyIdeas.pageHeader.Element.SendKeys(OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.F5);
                }
                HpgAssert.Exists(IMIdeaDetails.PublishIdeaButton, "Verify Publish Button is present");
                IMIdeaDetails.PublishIdeaButton.Click();
                page_objects.im3PV im3Pv = new page_objects.im3PV(IMIdeaDetails.browser);
                //im3PV.SelectOptionOnDropDown(im3PV.IdeaCategory, "TC4955-" + nameSuffix);
                //im3PV.SelectLastOptionOnDropDown(im3PV.IdeaDepartment);
                //im3PV.SelectLastOptionOnDropDown(im3PV.IdeaEffort);
                //im3PV.SelectLastOptionOnDropDown(im3PV.IdeaImpact);
                //im3PV.IdeaFirstName.Type("Robot");
                //im3PV.IdeaLastName.Type("Automation");
                //im3PV.IdeaPhone.Type("515-555-1212");
                //im3PV.IdeaEmail.Type("hpg.automation@hcahealthcare.com");
                //im3PV.longClickElement(im3PV.PublishButton.Element, 120, 10);
                waitForBrowser((BrowserSession)CurrentBrowser);
                HpgAssert.True(im3Pv.browser.FindXPath("//div[@class='validation-summary-errors']/span[contains(.,'fix the following errors')]").Missing(), "Verify no errors are present after publish");
                //step 5 end

                //Step 6. Edit Category
                //Expected Result: Category is changed
                IMHome.GotoCategories();
                categoryRow.Element.FindLink("Edit").Click();
                page_objects.imEditCategory IMEditCategory = new page_objects.imEditCategory(IMCategories.browser);
                HpgAssert.Contains(IMEditCategory.pageHeader.Text, "Edit Category", "Verify 'Edit Category' page is loaded");
                IMEditCategory.CategoryName.Type("CHANGE - TC4955-" + nameSuffix);
                IMEditCategory.SaveButton.Click();
                categoryRow = IMCategories.categoryRow("CHANGE - TC4955-" + nameSuffix);
                HpgAssert.Exists(categoryRow, "Verify Category was changed and exists in the list");
                categoryRow.Element.FindLink("Delete").Click();
                IMEditCategory.browser.FindButton("Confirm Delete").Click();
                HpgAssert.True(categoryRow.Element.Missing(), "Verify category is no longer displayed");
                //step 6 end

                //Step 7. Verify change on Published Idea
                //Expected Result: Published Idea reflects change to Category
                waitForBrowser((BrowserSession)CurrentBrowser);
                IMMyIdeas.GoToPublishedIdeaNumber(newItemID);
                waitForBrowser((BrowserSession)CurrentBrowser);
                page_objects.imPublishedIdea IMPublishedIdea = new imPublishedIdea(IMMyIdeas.browser);
                HpgAssert.Contains(IMPublishedIdea.Category.Text, "CHANGE - TC4955-" + nameSuffix, "Verify Category change on Published Idea Details");
                //step 7 end

                //Step 8. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 8 end
            }
        }

        [Test]
        public void Test_TC4956()
        {
            //RallyTestID = "TC4956";
            //US7532: Verify sort order of Department
            //Go to Published Ideas, Sort by Department, Verify sort order

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();

                //Step 1. Navigate to webpage http://hcatest.xpxcloud.com/
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Standard User1
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Navigate to Published Ideas
                //Expected Result: Published Ideas page is displayed
                IMHome.GotoPublishedIdeas();
                page_objects.imPublishedIdeas IMPublishedIdeas = new page_objects.imPublishedIdeas(IMHome.browser);
                //step 3 end

                //Step 4. Go to Publish Idea
                //Expected Result: Publish Idea page is displayed
                //step 4 end

                //Step 5. Sort ascending and verify
                //Expected Result: Ideas are sorted ascending by Department
                IMPublishedIdeas.SortIdeasBy("Department");
                DataTable allIdeas = IMPublishedIdeas.GetPublishedIdeasDT();
                DataView dv = allIdeas.DefaultView;
                DataView sDV = allIdeas.DefaultView;
                dv.Sort = "Department";
                for (int i = 0; i < dv.Table.Rows.Count; i++)
                {
                    if (!dv[i]["Department"].Equals(sDV[i]["Department"]))
                    {
                        HpgAssert.Fail("No match on line " + i + " of table (" + dv[i]["Department"].ToString() + " != " + sDV[i]["Department"].ToString() + ")");
                    }
                }
                WriteReport("Sorted table matches sorted results (Ascending)");
                //step 5 end

                //Step 6. Sort descending and verify
                //Expected Result: Ideas are sorted descending by Department
                IMPublishedIdeas.SortIdeasBy("Department", "descending");
                allIdeas = IMPublishedIdeas.GetPublishedIdeasDT();
                dv = allIdeas.DefaultView;
                sDV = allIdeas.DefaultView;
                dv.Sort = "Department DESC";
                for (int i = 0; i < dv.Table.Rows.Count; i++)
                {
                    if (!dv[i]["Department"].Equals(sDV[i]["Department"]))
                    {
                        HpgAssert.Fail("No match on line " + i + " of table (" + dv[i]["Department"].ToString() + " != " + sDV[i]["Department"].ToString() + ")");
                    }
                }
                WriteReport("Sorted table matches sorted results (Descending)");
                //step 6 end

                //Step 7. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                //step 7 end
            }
        }

        [Test][Ignore]
        public void Test_TC4960()
        {
            //TODO: Determine if test is still valid
            //US7533: Verify Results Measurement
            //Create idea, submit, accept, assign Results Measurment, publish, verify Results Measurement is displayed to users on Published Idea Details Page only

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");
                string resultString = "TC4960:" + nameSuffix + "*" + nameSuffix;

                //Step 1. Navigate to webpage http://hcatest.xpxcloud.com/
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Standard User1
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Create an Idea and Submit
                WriteReport("3 - Create an Idea and Submit");
                //Expected Result: Idea is created with all information
                IMLogin.GotoSubmitAnIdea();
                page_objects.imSubmitAnIdea IMSubmitIdea = new page_objects.imSubmitAnIdea(IMLogin.browser);
                IMSubmitIdea.FillFormField("IdeaName", "Automation TC4611 " + nameSuffix);
                IMSubmitIdea.FillFormField("Description", "Automation Description for TC4611 " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactName", "Automation Contact Name " + nameSuffix);
                IMSubmitIdea.FillFormField("ContactEmail", "HPG.automation@HCAHealthcare.com");
                IMSubmitIdea.FillFormField("ContactPhone", "615-344-3000");
                IMSubmitIdea.SubmitSaveButton.Click();
                page_objects.imEditIdea IMEditIdea = new page_objects.imEditIdea(IMSubmitIdea.browser);
                string newItemID = IMEditIdea.IdeaID.Text.Trim();
                IMEditIdea.SubmitSubmitButton.Click();
                //step 3 end

                //Step 4. Login as Administrator
                //Expected Result: Administrator is logged in
                // -- already logged in as admin
                //step 4 end

                //Step 5. Approve the submitted idea
                WriteReport("5 - Approve the submitted idea");
                //Expected Result: Idea is marked as 'Approved'
                page_objects.imMyIdeas IMMyIdeas = new page_objects.imMyIdeas(IMHome.browser);
                IMMyIdeas.GoToIdeaNumber(newItemID);
                page_objects.imIdeaDetailsAdmin IMIdeaDetails = new page_objects.imIdeaDetailsAdmin(IMMyIdeas.browser);
                for (int i = 0; i < 5; i++)
                {
                    if (IMIdeaDetails.PublishIdeaButton.Element.Exists())
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(5000);
                    IMMyIdeas.GoToIdeaNumber(newItemID);
                    try
                    {
                        IMMyIdeas.pageHeader.Element.SendKeys(OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.F5);
                        ((OpenQA.Selenium.Remote.RemoteWebDriver)IMMyIdeas.browser.Native).Navigate().Refresh();
                    }
                    catch (Exception)
                    {
                    }
                }
                HpgAssert.Contains(IMIdeaDetails.pageHeader.Text, "Idea Details", "Verify Idea Details page is loaded");
                IMIdeaDetails.AcceptButton.Click();
                page_objects.imIdeaAccept IMIdeaAccept = new page_objects.imIdeaAccept(IMIdeaDetails.browser);
                string ideaComments = "Idea Accepted by Automation-Admin on " + DateTime.Now.ToString("F");
                IMIdeaAccept.AcceptComments.Type(ideaComments);
                IMIdeaAccept.ConfirmButton.Click();
                //step 5 end

                //Step 6. Input Results Measurement and Publish Idea
                WriteReport("6 - Publish Idea");
                //Expected Result: Idea is Published
                IMMyIdeas.GoToIdeaNumber(newItemID);
                for (int i = 0; i < 5; i++)
                {
                    if (IMIdeaDetails.PublishIdeaButton.Exists())
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(5000);
                    IMMyIdeas.GoToIdeaNumber(newItemID);
                    IMMyIdeas.pageHeader.Element.SendKeys(OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.F5);
                }
                HpgAssert.Exists(IMIdeaDetails.PublishIdeaButton, "Verify Publish Button is present");
                IMIdeaDetails.PublishIdeaButton.Click();
                page_objects.im3PV im3Pv = new page_objects.im3PV(IMIdeaDetails.browser);
                //string ideaCategory = im3PV.SelectLastOptionOnDropDown(im3PV.IdeaCategory);
                //string ideaDepartment = im3PV.SelectLastOptionOnDropDown(im3PV.IdeaDepartment);
                //string ideaEffort = im3PV.SelectLastOptionOnDropDown(im3PV.IdeaEffort);
                //string ideaImpact = im3PV.SelectLastOptionOnDropDown(im3PV.IdeaImpact);
                //im3PV.ResultsMeasurement.Type(resultString);
                //im3PV.IdeaFirstName.Type("Robot");
                //im3PV.IdeaLastName.Type("Automation");
                //im3PV.IdeaPhone.Type("515-555-1212");
                //im3PV.IdeaEmail.Type("hpg.automation@hcahealthcare.com");
                //try
                //{
                //    im3PV.PublishButton.Element.Click(new Options()
                //    {
                //        Timeout = TimeSpan.FromMinutes(5),
                //        RetryInterval = TimeSpan.FromSeconds(10)
                //    });
                //}
                //catch (Exception e)
                //{
                //    WriteReport("----Waiting on browser to publish (" + e.Message + ")----");
                //}
                //HpgAssert.True(im3PV.browser.FindXPath("//div[@class='validation-summary-errors']/span[contains(.,'fix the following errors')]").Missing(), "Verify no errors are present after publish");
                //SendKeys.SendWait("{ESC}");
                //SendKeys.SendWait("{ESC}");
                //im3PV.browser.Now();
                //step 6 end

                //Step 7. Login as Standard User1
                //Expected Result: User is logged in
                // -- Already logged in
                //step 7 end

                //Step 8. Navigate to Published Idea
                WriteReport("8 - Navigate to Published Idea");
                //Expected Result: Published Idea Details Page is displayed
                page_objects.imPublishedIdeas IMPublishedIdeas = new page_objects.imPublishedIdeas(IMHome.browser);
                IMPublishedIdeas.GoToPublishedIdeaNumber(newItemID);
                page_objects.imPublishedIdea IMPublishedIdea = new page_objects.imPublishedIdea(IMPublishedIdeas.browser);
                //step 8 end

                //Step 9. Verify Results Measurement
                WriteReport("9 - Verify Results Measurement of Published Idea");
                //Expected Result: Results Measurements is displayed correctly
                //--HpgAssert.AreEqual(resultString, IMPublishedIdea.Department.Text.Trim(), "Verify Results Measurement is correct on Published Idea");
                //step 9 end

                //Step 10. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                //step 10 end
            }
        }

        [Test]
        public void Test_TC7053()
        {
            RallyTestID = "TC7053";
            //US7748: The Published Idea List should only show Published Ideas
            //Login, view published ideas, compare list view to database query results

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();

                //Step 1. Navigate to webpage http://hcatest.xpxcloud.com/
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome =
                    new page_objects.imHome((BrowserSession) BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Standard User1
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"],
                                    Controller.fields["Password"]);
                //step 2 end

                //Step 3. Navigate to Published Ideas
                //Expected Result: Published Ideas page is displayed
                AdjustMaxTimeout(180);
                IMHome.GotoPublishedIdeas();
                waitForBrowser(IMHome.browser);
                page_objects.imPublishedIdeas IMPublishedIdeas = new page_objects.imPublishedIdeas(IMHome.browser);
                //step 3 end

                //Step 4. Verify only Published Ideas are listed
                //Expected Result: All ideas listed are "Published" status
                DBUtility dbUtility = new DBUtility();
                DataTable displayedIdeas = IMPublishedIdeas.GetPublishedIdeasDT();
                DataTable dbIdeas = dbUtility.GetPublishedIdeas();
                HpgAssert.AreEqual(dbIdeas.Rows.Count.ToString(), displayedIdeas.Rows.Count.ToString(), "Verify number of records are the same");
                DataView dbView = dbIdeas.DefaultView;
                DataView displayView = displayedIdeas.DefaultView;
                dbView.Sort = "IdeaId";
                displayView.Sort = "IdeaNumber";
                for (int i = 0; i < dbView.Table.Rows.Count; i++)
                {
                    if (!dbView[i]["IdeaId"].Equals(displayView[i]["IdeaNumber"]))
                    {
                        HpgAssert.Fail("No match on line " + i + " of table (" + dbView[i]["IdeaId"].ToString() + " != " + displayView[i]["IdeaNumber"].ToString() + ")!");
                    }
                }
                WriteReport("Published Ideas list view matches Database Query");
                //step 4 end

                //Step 5. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 5 end
            }
        }

        [Test]
        public void Test_TC7055()
        {
            RallyTestID = "TC7055";
            //US7849: Verify user shown throughout the site
            //Login, view every page and verify name at top of page

            IEnumerable<InputObject> testData = FileReader.getInputObjects("test_specific\\Test_TC7053.xls", "Main");
            string[] pages = (from p in testData where !string.IsNullOrEmpty(p.fields["URL"]) select p.fields["URL"]).ToArray();

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();

                //Step 1. Navigate to webpage http://hcatest.xpxcloud.com/
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome =
                    new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Standard User1
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.goHomeHCADev(testData.ElementAt(0).fields["Login"], testData.ElementAt(0).fields["Password"]);
                //step 2 end

                //Step 3. Visit Every Page
                //Expected Result: User Name is visible on every page
                foreach (string page in pages)
                {
                    IMHome.browser.Visit(page);
                    HpgAssert.True(IMHome.HeaderUserInfo.Text.Trim().EndsWith(testData.ElementAt(0).fields["User Name"]), "Verify User Info is visible in header");
                }
                //step 3 end

                //Step 4. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 4 end
            }
        }

        [Test]
        public void Test_TC7062()
        {
            RallyTestID = "TC7062";
            //US7762: Allow users to Sign-up
            //Verify "Register" link is present at top of page and takes user to sign-up page

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();

                //Step 1. Navigate to webpage http://sbx-im.healthtrustpg.com
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome =
                    new page_objects.imHome((BrowserSession) BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                CurrentBrowser.Visit("http://sbx-im.healthtrustpg.com");
                //step 1 end

                //Step 2. Verify "Register" link is at top of page
                //Expected Result: Link is present
                HpgAssert.True(IMHome.HeaderRegisterLink.Element.Exists(), "Verify 'Register' link is present");
                //step 2 end

                //Step 3. Click "Register" link
                //Expected Result: User is taken to PASS Sign-Up page
                IMHome.HeaderRegisterLink.Click();
                HpgAssert.AreEqual("Member Registration", IMHome.browser.Title, "Verify user is taken to PASS Sign-Up page");
                //step 3 end

                //Step 4. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 4 end
            }
        }

        [Test]
        public void Test_TC7081()
        {
            RallyTestID = "TC7081";
            //US7765: Verify Rich Text Areas are present and functional
            //Edit both Rich Text Areas, save, verify changes on Home Page

            string richText1 = "<STRONG>This is a rich text box</STRONG><BR><EM>Rich Text Box 1</EM>";
            string richText2 = "<STRONG>This is another rich text box</STRONG><BR><EM>Rich Text Box 2</EM>";
            string errorMessage = "";
            DBUtility dbUtility = new DBUtility();
            DataTable orgValues = new DataTable("OriginalValues");
            orgValues.Load(dbUtility.GetRecords("SELECT [PageModuleId], [Body] FROM [PageModules]"));

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();

                //Step 1. Navigate to webpage http://sbx-im.healthtrustpg.com
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome =
                    new page_objects.imHome((BrowserSession) BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Admin
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"],
                                    Controller.fields["Password"]);
                //step 2 end

                //Step 3. Navigate to Edit Home Page form
                //Expected Result: Edit Home Page form is displayed
                IMHome.GotoHomePageCMS();
                //step 3 end

                //Step 4. Edit both Rich Text Areas and save
                //Expected Result: Rich Text Areas are edited
                page_objects.imHomePageCMS cms = new page_objects.imHomePageCMS(IMHome.browser);
                HpgElement richTextRow1 = cms.GetCMSRow("Home Page Body Text 1");
                HpgElement richTextRow2 = cms.GetCMSRow("Home Page Body Text 2");
                HpgAssert.True(richTextRow1.Element.Exists(), "Verify Rich Text Area 1 is present");
                HpgAssert.True(richTextRow2.Element.Exists(), "Verify Rich Text Area 2 is present");
                
                try
                {
                    // -- Edit Rich Text Area 1
                    richTextRow1.Element.FindButton("Edit").Click();
                    cms.editBodyMenuTools.Click();
                    cms.editBodyMenuToolsSourceCode.Click();
                    cms.editSourceCodeTextarea.Type(richText1);
                    cms.editSourceCodeOKButton.Click();
                    cms.editSaveButton.Click();

                    // -- Edit Rich Text Area 2
                    richTextRow2.Element.FindButton("Edit").Click();
                    cms.editBodyMenuTools.Click();
                    cms.editBodyMenuToolsSourceCode.Click();
                    cms.editSourceCodeTextarea.Type(richText2);
                    cms.editSourceCodeOKButton.Click();
                    cms.editSaveButton.Click();
                    //step 4 end

                    //Step 5. Navigate to Home Page and verify changes
                    //Expected Result: Changes are displayed on Rich Text Areas
                    IMHome.GotoHomePage();
                    ((RemoteWebDriver)IMHome.browser.Native).Navigate().Refresh();
                    System.Threading.Thread.Sleep(10000);
                    ((RemoteWebDriver)IMHome.browser.Native).Navigate().Refresh();
                    HpgAssert.Contains(IMHome.browser.FindId("PageModuleId-1")["innerHTML"].ToLower(), richText1.ToLower(), "Verify Rich Text Area 1 is changed");
                    HpgAssert.Contains(IMHome.browser.FindId("PageModuleId-2")["innerHTML"].ToLower(), richText2.ToLower(), "Verify Rich Text Area 2 is changed");
                }
                catch (Exception e)
                {
                    WriteReport("Error during validation! " + e.Message);
                    errorMessage = e.Message + "\n" + e.StackTrace;
                }
                //step 5 end

                //Clean-up
                foreach (DataRow row in orgValues.Rows)
                {
                    string updateResult = dbUtility.UpdatePageModule(row["Body"].ToString(), row["PageModuleId"].ToString());
                    if (updateResult.StartsWith("ERROR"))
                    {
                        errorMessage = errorMessage + "\r\n" + updateResult;
                    }
                }
                //clean-up end

                //Step 6. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achived via foreach browser loop
                //step 6 end

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    HpgAssert.Fail(errorMessage);
                }
            }
        }

        [Test]
        public void Test_TC7082()
        {
            RallyTestID = "TC7082";
            //US8225: Verify Filtering (Amazon Style)
            //Navigate to Published Ideas List, filter by Department, Category, Effort, Impact, Updated Date, then filter by combinations.  Verify results for each filter application.

            foreach (KeyValuePair<string, Browser> testBrowser in ffChromeOnly)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();

                //Step 1. Navigate to webpage http://sbx-im.healthtrustpg.com
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession) BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Admin
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //IMHome.goHomeHCADev(false);
                //step 2 end

                //Step 3. Navigate to Published Ideas List
                //Expected Result: Published Ideas List page is dispalyed
                AdjustMaxTimeout(180);
                IMHome.GotoPublishedIdeas();
                page_objects.imPublishedIdeas IMPublishedIdeas = new page_objects.imPublishedIdeas(IMHome.browser);
                DataTable ideaList = IMPublishedIdeas.GetPublishedIdeasDT();
                //step 3 end

                //Step 4. Filter individually and verify results
                //Expected Result: Only Ideas meeting criteria are displayed
                // -- Filter by Max Department
                WriteReport("Filter by Department (max count)");
                IMPublishedIdeas.ShowAllDepartments();
                var filterCounts = from row in ideaList.AsEnumerable()
                            group row by row.Field<string>("Department")
                            into grp
                            select new {Name = grp.Key, Count = grp.Count()};
                string filterName = (from dep in filterCounts orderby dep.Count descending select dep.Name).First();
                int filterCount = (from dep in filterCounts orderby dep.Count descending select dep.Count).First();
                HpgElement filterElement = IMPublishedIdeas.GetDepartmentFilter(filterName);
                filterElement.Element.Hover();
                filterElement.Click();
                HpgAssert.True(IMPublishedIdeas.browser.FindXPath("//*[@id='publishIdeasTable']/tbody[1]/tr[1]").Exists(), "Verify at least 1 result is present");
                HpgAssert.AreEqual(filterCount.ToString(), IMPublishedIdeas.FilterRecordCount.Text.Trim(), "Verify number of filtered records is correct");
                ideaList = IMPublishedIdeas.GetPublishedIdeasDT();
                filterCounts = from row in ideaList.AsEnumerable()
                                   group row by row.Field<string>("Department")
                                       into grp
                                       select new { Name = grp.Key, Count = grp.Count() };
                HpgAssert.AreEqual("1", filterCounts.Count().ToString(), "Verify only 1 Department in results");
                HpgAssert.AreEqual(filterName, filterCounts.First().Name, "Verify only '" + filterName + "' Department is in results");

                // -- Filter by Max Category
                WriteReport("Filter by Category (Max Count)");
                IMHome.GotoPublishedIdeas();
                waitForBrowser(IMPublishedIdeas.browser);
                ideaList = IMPublishedIdeas.GetPublishedIdeasDT();
                IMPublishedIdeas.ShowAllCategories();
                filterCounts = from row in ideaList.AsEnumerable()
                               group row by row.Field<string>("Category")
                                   into grp
                                   select new { Name = grp.Key, Count = grp.Count() };
                filterName = (from dep in filterCounts orderby dep.Count descending select dep.Name).First();
                filterCount = (from dep in filterCounts orderby dep.Count descending select dep.Count).First();
                filterElement = IMPublishedIdeas.GetCategoryFilter(filterName);
                filterElement.Element.Hover();
                filterElement.Click();
                HpgAssert.True(IMPublishedIdeas.browser.FindXPath("//*[@id='publishIdeasTable']/tbody[1]/tr[1]").Exists(), "Verify at least 1 result is present");
                HpgAssert.AreEqual(filterCount.ToString(), IMPublishedIdeas.FilterRecordCount.Text.Trim(), "Verify number of filtered records is correct");
                ideaList = IMPublishedIdeas.GetPublishedIdeasDT();
                filterCounts = from row in ideaList.AsEnumerable()
                               group row by row.Field<string>("Category")
                                   into grp
                                   select new { Name = grp.Key, Count = grp.Count() };
                HpgAssert.AreEqual("1", filterCounts.Count().ToString(), "Verify only 1 Category in results");
                HpgAssert.AreEqual(filterName, filterCounts.First().Name, "Verify only '" + filterName + "' Category is in results");

                // -- Filter by Max Effort
                WriteReport("Filter by Effort (Max Count)");
                IMHome.GotoPublishedIdeas();
                waitForBrowser(IMPublishedIdeas.browser);
                ideaList = IMPublishedIdeas.GetPublishedIdeasDT();
                filterCounts = from row in ideaList.AsEnumerable()
                               group row by row.Field<int>("Effort")
                                   into grp
                                   select new { Name = grp.Key.ToString(), Count = grp.Count() };
                filterName = (from dep in filterCounts orderby dep.Count descending select dep.Name).First();
                filterCount = (from dep in filterCounts orderby dep.Count descending select dep.Count).First();
                filterElement = IMPublishedIdeas.GetEffortFilter(int.Parse(filterName));
                filterElement.Element.Hover();
                filterElement.Click();
                HpgAssert.True(IMPublishedIdeas.browser.FindXPath("//*[@id='publishIdeasTable']/tbody[1]/tr[1]").Exists(), "Verify at least 1 result is present");
                HpgAssert.AreEqual(filterCount.ToString(), IMPublishedIdeas.FilterRecordCount.Text.Trim(), "Verify number of filtered records is correct");
                ideaList = IMPublishedIdeas.GetPublishedIdeasDT();
                filterCounts = from row in ideaList.AsEnumerable()
                               group row by row.Field<int>("Effort")
                                   into grp
                                   select new { Name = grp.Key.ToString(), Count = grp.Count() };
                HpgAssert.AreEqual("1", filterCounts.Count().ToString(), "Verify only 1 Effort in results");
                HpgAssert.AreEqual(filterName, filterCounts.First().Name, "Verify only '" + filterName + "' Effort is in results");


                // -- Filter by Max Impact
                WriteReport("Filter by Impact (Max Count)");
                IMHome.GotoPublishedIdeas();
                waitForBrowser(IMPublishedIdeas.browser);
                ideaList = IMPublishedIdeas.GetPublishedIdeasDT();
                filterCounts = from row in ideaList.AsEnumerable()
                               group row by row.Field<int>("Impact")
                                   into grp
                                   select new { Name = grp.Key.ToString(), Count = grp.Count() };
                filterName = (from dep in filterCounts orderby dep.Count descending select dep.Name).First();
                filterCount = (from dep in filterCounts orderby dep.Count descending select dep.Count).First();
                filterElement = IMPublishedIdeas.GetImpactFilter(int.Parse(filterName));
                filterElement.Element.Hover();
                filterElement.Click();
                HpgAssert.True(IMPublishedIdeas.browser.FindXPath("//*[@id='publishIdeasTable']/tbody[1]/tr[1]").Exists(), "Verify at least 1 result is present");
                HpgAssert.AreEqual(filterCount.ToString(), IMPublishedIdeas.FilterRecordCount.Text.Trim(), "Verify number of filtered records is correct");
                ideaList = IMPublishedIdeas.GetPublishedIdeasDT();
                filterCounts = from row in ideaList.AsEnumerable()
                               group row by row.Field<int>("Impact")
                                   into grp
                                   select new { Name = grp.Key.ToString(), Count = grp.Count() };
                HpgAssert.AreEqual("1", filterCounts.Count().ToString(), "Verify only 1 Impact in results");
                HpgAssert.AreEqual(filterName, filterCounts.First().Name, "Verify only '" + filterName + "' Impact is in results");
                ResetMaxTimeout();
                WriteReport("--- Test for individual filters OK ---");
                //step 4 end

                //Step 5. Filter combinations and verify results
                //Expected Result: Only Ideas meeting ALL criteria are displayed
                AdjustMaxTimeout(120);
                WriteReport("Filter by Department (max count)");
                IMHome.GotoPublishedIdeas();
                waitForBrowser(IMPublishedIdeas.browser);
                ideaList = IMPublishedIdeas.GetPublishedIdeasDT();
                IMPublishedIdeas.ShowAllDepartments();
                filterCounts = from row in ideaList.AsEnumerable()
                                   group row by row.Field<string>("Department")
                                       into grp
                                       select new { Name = grp.Key, Count = grp.Count() };
                filterName = (from dep in filterCounts orderby dep.Count descending select dep.Name).First();
                filterCount = (from dep in filterCounts orderby dep.Count descending select dep.Count).First();
                filterElement = IMPublishedIdeas.GetDepartmentFilter(filterName);
                filterElement.Element.Hover();
                filterElement.Click();
                HpgAssert.True(IMPublishedIdeas.browser.FindXPath("//*[@id='publishIdeasTable']/tbody[1]/tr[1]").Exists(), "Verify at least 1 result is present");
                HpgAssert.AreEqual(filterCount.ToString(), IMPublishedIdeas.FilterRecordCount.Text.Trim(), "Verify number of filtered records is correct");
                ideaList = IMPublishedIdeas.GetPublishedIdeasDT();
                filterCounts = from row in ideaList.AsEnumerable()
                               group row by row.Field<string>("Department")
                                   into grp
                                   select new { Name = grp.Key, Count = grp.Count() };
                HpgAssert.AreEqual("1", filterCounts.Count().ToString(), "Verify only 1 Department in results");
                HpgAssert.AreEqual(filterName, filterCounts.First().Name, "Verify only '" + filterName + "' Department is in results");

                // -- Filter by Max Category
                WriteReport("Add Filter by Category (Max Count)");
                ideaList = IMPublishedIdeas.GetPublishedIdeasDT();
                IMPublishedIdeas.ShowAllCategories();
                filterCounts = from row in ideaList.AsEnumerable()
                               group row by row.Field<string>("Category")
                                   into grp
                                   select new { Name = grp.Key, Count = grp.Count() };
                filterName = (from dep in filterCounts orderby dep.Count descending select dep.Name).First();
                filterCount = (from dep in filterCounts orderby dep.Count descending select dep.Count).First();
                filterElement = IMPublishedIdeas.GetCategoryFilter(filterName);
                filterElement.Element.Hover();
                filterElement.Click();
                System.Threading.Thread.Sleep(10000);
                waitForBrowser(IMPublishedIdeas.browser);
                HpgAssert.True(IMPublishedIdeas.browser.FindXPath("//*[@id='publishIdeasTable']/tbody[1]/tr[1]").Exists(), "Verify at least 1 result is present");
                HpgAssert.AreEqual(filterCount.ToString(), IMPublishedIdeas.FilterRecordCount.Text.Trim(), "Verify number of filtered records is correct");
                ideaList = IMPublishedIdeas.GetPublishedIdeasDT();
                filterCounts = from row in ideaList.AsEnumerable()
                               group row by row.Field<string>("Category")
                                   into grp
                                   select new { Name = grp.Key, Count = grp.Count() };
                HpgAssert.AreEqual("1", filterCounts.Count().ToString(), "Verify only 1 Category in results");
                HpgAssert.AreEqual(filterName, filterCounts.First().Name, "Verify only '" + filterName + "' Category is in results");

                // -- Filter by Max Effort
                WriteReport("Filter by Effort (Max Count)");
                ideaList = IMPublishedIdeas.GetPublishedIdeasDT();
                filterCounts = from row in ideaList.AsEnumerable()
                               group row by row.Field<int>("Effort")
                                   into grp
                                   select new { Name = grp.Key.ToString(), Count = grp.Count() };
                filterName = (from dep in filterCounts orderby dep.Count descending select dep.Name).First();
                filterCount = (from dep in filterCounts orderby dep.Count descending select dep.Count).First();
                filterElement = IMPublishedIdeas.GetEffortFilter(int.Parse(filterName));
                filterElement.Element.Hover();
                filterElement.Click();
                System.Threading.Thread.Sleep(10000);
                waitForBrowser(IMPublishedIdeas.browser);
                HpgAssert.True(IMPublishedIdeas.browser.FindXPath("//*[@id='publishIdeasTable']/tbody[1]/tr[1]").Exists(), "Verify at least 1 result is present");
                HpgAssert.AreEqual(filterCount.ToString(), IMPublishedIdeas.FilterRecordCount.Text.Trim(), "Verify number of filtered records is correct");
                ideaList = IMPublishedIdeas.GetPublishedIdeasDT();
                filterCounts = from row in ideaList.AsEnumerable()
                               group row by row.Field<int>("Effort")
                                   into grp
                                   select new { Name = grp.Key.ToString(), Count = grp.Count() };
                HpgAssert.AreEqual("1", filterCounts.Count().ToString(), "Verify only 1 Effort in results");
                HpgAssert.AreEqual(filterName, filterCounts.First().Name, "Verify only '" + filterName + "' Effort is in results");


                // -- Filter by Max Impact
                WriteReport("Filter by Impact (Max Count)");
                ideaList = IMPublishedIdeas.GetPublishedIdeasDT();
                filterCounts = from row in ideaList.AsEnumerable()
                               group row by row.Field<int>("Impact")
                                   into grp
                                   select new { Name = grp.Key.ToString(), Count = grp.Count() };
                filterName = (from dep in filterCounts orderby dep.Count descending select dep.Name).First();
                filterCount = (from dep in filterCounts orderby dep.Count descending select dep.Count).First();
                filterElement = IMPublishedIdeas.GetImpactFilter(int.Parse(filterName));
                filterElement.Element.Hover();
                filterElement.Click();
                System.Threading.Thread.Sleep(10000);
                waitForBrowser(IMPublishedIdeas.browser);
                HpgAssert.True(IMPublishedIdeas.browser.FindXPath("//*[@id='publishIdeasTable']/tbody[1]/tr[1]").Exists(), "Verify at least 1 result is present");
                HpgAssert.AreEqual(filterCount.ToString(), IMPublishedIdeas.FilterRecordCount.Text.Trim(), "Verify number of filtered records is correct");
                ideaList = IMPublishedIdeas.GetPublishedIdeasDT();
                filterCounts = from row in ideaList.AsEnumerable()
                               group row by row.Field<int>("Impact")
                                   into grp
                                   select new { Name = grp.Key.ToString(), Count = grp.Count() };
                HpgAssert.AreEqual("1", filterCounts.Count().ToString(), "Verify only 1 Impact in results");
                HpgAssert.AreEqual(filterName, filterCounts.First().Name, "Verify only '" + filterName + "' Impact is in results");

                // -- Filter by last 6 months
                WriteReport("Filter by Impact (Max Count)");
                ideaList = IMPublishedIdeas.GetPublishedIdeasDT();
                filterElement = IMPublishedIdeas.GetUpdatedDateFilter(5);
                string filterDate = DateTime.Parse(filterElement.Element.Id.Replace("UpdatedDateSelected", "")).ToString("#MM/dd/yyyy#");
                filterElement.Element.Hover();
                filterElement.Click();
                System.Threading.Thread.Sleep(10000);
                waitForBrowser(IMPublishedIdeas.browser);
                HpgAssert.True(IMPublishedIdeas.browser.FindXPath("//*[@id='publishIdeasTable']/tbody[1]/tr[1]").Exists(), "Verify at least 1 result is present");
                HpgAssert.AreEqual(ideaList.Select("UpdatedDate > " + filterDate).Count().ToString(), IMPublishedIdeas.FilterRecordCount.Text.Trim(), "Verify number of filtered records is correct");
                ideaList = IMPublishedIdeas.GetPublishedIdeasDT();
                HpgAssert.AreEqual("0", ideaList.Select("UpdatedDate <= " + filterDate).Count().ToString(), "Verify there are no ideas with updated date before filter date");

                ResetMaxTimeout();
                WriteReport("--- Test for combined filters OK ---");
                //step 5 end

                //Step 6. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achived via foreach browser loop
                //step 6 end
            }
        }

        [Test]
        public void Test_TC7203()
        {
            //US8240: Excel export published idea details for a specific ideas.
            //Navigate to a Published Idea, click Excel Export link, verify details of Excel File match Published Idea
            RallyTestID = "TC7203";
            string saveFile = "TC7203ExcelExport.xls";

            foreach (KeyValuePair<string, Browser> testBrowser in ffChromeOnly)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                System.IO.File.Delete(Constants.CurrentDirectory + Constants.InputDataPath + saveFile);

                //Step 1. Navigate to webpage http://sbx-im.healthtrustpg.com
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as user
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Navigate to Published Ideas List
                //Expected Result: Published Ideas List page is displayed
                IMHome.GotoPublishedIdeas(false);
                page_objects.imPublishedIdeas IMPublishedIdeas = new page_objects.imPublishedIdeas(IMHome.browser);
                //step 3 end

                //Step 4. Navigate to a Published Idea
                //Expected Result: Published Idea Details Page is displayed
                // Going to the first published idea in the list
                string ideaNumber = IMPublishedIdeas.browser.FindXPath("//*[@id='publishIdeasTable']/tbody[1]/tr[1]//a", Options.First).Text.Trim();
                IMPublishedIdeas.GoToPublishedIdeaNumber(ideaNumber);
                page_objects.imPublishedIdea IMPublishedIdea = new page_objects.imPublishedIdea(IMPublishedIdeas.browser);
                HpgAssert.AreEqual(IMPublishedIdea.IdeaNumber.Text, ideaNumber, "Verify idea " + ideaNumber + " details are loaded");
                //step 4 end

                //Step 5. Click Excel Export button
                //Expected Result: Excel file is exported
                IMPublishedIdea.SaveExcel(Constants.CurrentDirectory + Constants.InputDataPath + saveFile);
                //step 5 end

                //Step 6. Verify details of Excel file with Published Idea Details
                //Expected Result: All details are correct
                HpgAssert.AreEqual("", IMPublishedIdea.CompareExcelFileToDetailsPage(saveFile));
                //step 6 end

                //Step 7. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 7 end
            }
        }

        [Test]
        public void Test_TC7265()
        {
            //US8241: PDF export published idea details for a specific ideas.
            //Navigate to a Published Idea, click PDF Export link, verify details of Excel File match Published Idea
            RallyTestID = "TC7265";
            string saveFile = "TC7265PDFExport.pdf";

            foreach (KeyValuePair<string, Browser> testBrowser in ffChromeOnly)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                System.IO.File.Delete(Constants.CurrentDirectory + Constants.InputDataPath + saveFile);

                //Step 1. Navigate to webpage http://sbx-im.healthtrustpg.com
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as user
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Navigate to Published Ideas List
                //Expected Result: Published Ideas List page is displayed
                IMHome.GotoPublishedIdeas();
                page_objects.imPublishedIdeas IMPublishedIdeas = new page_objects.imPublishedIdeas(IMHome.browser);
                //step 3 end

                //Step 4. Navigate to a Published Idea
                //Expected Result: Published Idea Details Page is displayed
                // Going to the first published idea in the list
                string ideaNumber = IMPublishedIdeas.browser.FindXPath("//*[@id='publishIdeasTable']/tbody[1]/tr[1]//a", Options.First).Text.Trim();
                IMPublishedIdeas.GoToPublishedIdeaNumber(ideaNumber);
                page_objects.imPublishedIdea IMPublishedIdea = new page_objects.imPublishedIdea(IMPublishedIdeas.browser);
                HpgAssert.AreEqual(IMPublishedIdea.IdeaNumber.Text, ideaNumber, "Verify idea " + ideaNumber + " details are loaded");
                //step 4 end

                //Step 5. Click PDF Export button
                //Expected Result: PDF file is exported
                IMPublishedIdea.SavePDF(Constants.CurrentDirectory + Constants.InputDataPath + saveFile);
                //step 5 end

                //Step 6. Verify PDF file was downloaded
                //Expected Result: PDF file is downloaded
                HpgAssert.True(System.IO.File.Exists(Constants.CurrentDirectory + Constants.InputDataPath + saveFile),
                               "Verify PDF file was downloaded");
                //step 6 end

                //Step 7. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 7 end
            }
        }

        [Test]
        public void Test_TC7271()
        {
            RallyTestID = "TC7271";
            //US8227: Excel export published idea details for a list of ideas
            //Navigate to a Published Ideas List Page, click Excel Export link, verify details of Excel File match Published Ideas
            string saveFile = "TC7271ExcelExport.xls";

            foreach (KeyValuePair<string, Browser> testBrowser in ffChromeOnly)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                System.IO.File.Delete(Constants.CurrentDirectory + Constants.InputDataPath + saveFile);

                //Step 1. Navigate to webpage http://sbx-im.healthtrustpg.com
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as user
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Navigate to Published Ideas List
                //Expected Result: Published Ideas List page is displayed
                IMHome.GotoPublishedIdeas();
                page_objects.imPublishedIdeas IMPublishedIdeas = new page_objects.imPublishedIdeas(IMHome.browser);
                //step 3 end

                //Step 4. Click Excel Export button
                //Expected Result: Excel file is exported
                IMPublishedIdeas.browser.SaveWebResource(IMPublishedIdeas.LinkExcel.Element["href"], Constants.CurrentDirectory + Constants.InputDataPath + saveFile);
                WriteReport("Excel export saved to " + Constants.CurrentDirectory + Constants.InputDataPath + saveFile);
                //step 4 end

                //Step 5. Verify details of Excel file with Published Idea Details
                //Expected Result: All details are correct
                IEnumerable<InputObject> ideaExport = FileReader.getInputObjects(saveFile, "Idea");
                string ideaNumbers = string.Join(", ", IMPublishedIdeas.GetPublishedIdeasDT().Rows.OfType<DataRow>().Select(x => x.Field<int>("IdeaNumber").ToString()));
                DBUtility dbUtility = new DBUtility();
                DataTable dbIdeas = dbUtility.GetPublishedIdeas("IdeaId IN (" + ideaNumbers + ")");
                HpgAssert.AreEqual(dbIdeas.Rows.Count.ToString(), ideaExport.Count().ToString(), "Verify number of exported ideas is correct");
                DataRow foundRow;
                DateTime rowDate;
                DateTime dtDate;
                foreach (InputObject exportIdea in ideaExport)
                {
                    WriteReport("--- Checking Idea #" + exportIdea.fields["Idea Number"] + "... ---");
                    foundRow = dbIdeas.Select("IdeaId = " + exportIdea.fields["Idea Number"]).First();
                    HpgAssert.AreEqual(foundRow["Title"].ToString().Trim(), exportIdea.fields["Idea Name"], "Verify Idea Name is correct");
                    HpgAssert.AreEqual(foundRow["Department"].ToString().Trim(), exportIdea.fields["Department"], "Verify Department is correct");
                    HpgAssert.AreEqual(foundRow["Category"].ToString().Trim(), exportIdea.fields["Category"], "Verify Category is correct");
                    HpgAssert.AreEqual(foundRow["EffortLevel"].ToString().Trim(), impactEffort[exportIdea.fields["Effort"]].ToString(), "Verify Effort is correct");
                    HpgAssert.AreEqual(foundRow["ImpactLevel"].ToString().Trim(), impactEffort[exportIdea.fields["Impact"]].ToString(), "Verify Impact is correct");
                    rowDate = string.IsNullOrEmpty(exportIdea.fields["Publish Date"]) ? DateTime.Parse("01/01/2000") : DateTime.FromOADate(double.Parse(exportIdea.fields["Publish Date"]));
                    dtDate = string.IsNullOrEmpty(foundRow["PublishedDate"].ToString()) ? DateTime.Parse("01/01/2000") : (DateTime)foundRow["PublishedDate"];
                    HpgAssert.AreEqual(dtDate.ToString("yyyyMMddhhmmss"), rowDate.ToString("yyyyMMddhhmmss"), "Verify Published Date is correct");
                    rowDate = string.IsNullOrEmpty(exportIdea.fields["Update Date"]) ? DateTime.Parse("01/01/2000") : DateTime.FromOADate(double.Parse(exportIdea.fields["Update Date"]));
                    dtDate = string.IsNullOrEmpty(foundRow["UpdatedDate"].ToString()) ? DateTime.Parse("01/01/2000") : (DateTime)foundRow["UpdatedDate"];
                    HpgAssert.AreEqual(dtDate.ToString("yyyyMMddhhmmss"), rowDate.ToString("yyyyMMddhhmmss"), "Verify Updated Date is correct");
                    HpgAssert.AreEqual(foundRow["Description"].ToString().Trim().ToLower().Replace(" ", "").Replace("\n", "").Replace("\r", ""), exportIdea.fields["Description"].Trim().ToLower().Replace(" ", "").Replace("\n", "").Replace("\r", ""), "Verify Description is correct");
                }
                //step 5 end

                //Step 6. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 6 end
            }
        }

        [Test]
        public void Test_TC7290()
        {
            RallyTestID = "TC7290";
            //US8228: PDF export idea details for a list of ideas.
            //Navigate to Published Ideas List Page, click PDF Export link, verify PDF file is downloaded
            string saveFile = "TC7290PDFExport.pdf";

            foreach (KeyValuePair<string, Browser> testBrowser in ffChromeOnly)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                System.IO.File.Delete(Constants.CurrentDirectory + Constants.InputDataPath + saveFile);

                //Step 1. Navigate to webpage http://sbx-im.healthtrustpg.com
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as user
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Navigate to Published Ideas List
                //Expected Result: Published Ideas List page is displayed
                IMHome.GotoPublishedIdeas();
                page_objects.imPublishedIdeas IMPublishedIdeas = new page_objects.imPublishedIdeas(IMHome.browser);
                //step 3 end

                //Step 4. Click PDF Export button
                //Expected Result: PDF file is exported
                IMPublishedIdeas.browser.SaveWebResource(IMPublishedIdeas.LinkPDF.Element["href"], Constants.CurrentDirectory + Constants.InputDataPath + saveFile);
                WriteReport("PDF export saved to " + Constants.CurrentDirectory + Constants.InputDataPath + saveFile);
                //step 4 end

                //Step 5. Verify PDF file was downloaded
                //Expected Result: PDF file is downloaded
                HpgAssert.True(System.IO.File.Exists(Constants.CurrentDirectory + Constants.InputDataPath + saveFile), "Verify PDF file was downloaded");
                //step 5 end

                //Step 6. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 6 end
            }
        }

        [Test]
        public void Test_TC7322()
        {
            RallyTestID = "TC7322";
            //US8530: email an idea from idea detail screen
            //Navigate to a Published Idea, click Email link, edit & send email, verify email sent and details are correct

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");
                AutomationCore.utility.ews ews = new AutomationCore.utility.ews();
                ews.FilterGreaterThan.Clear();
                ews.FilterGreaterThan.Add(new KeyValuePair<string, object>("sent", DateTime.Now));

                //Step 1. Navigate to webpage http://sbx-im.healthtrustpg.com
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as user
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Navigate to Published Ideas List
                //Expected Result: Published Ideas List page is displayed
                IMHome.GotoPublishedIdeas(false);
                page_objects.imPublishedIdeas IMPublishedIdeas = new page_objects.imPublishedIdeas(IMHome.browser);
                //step 3 end

                //Step 4. Navigate to a Published Idea
                //Expected Result: Published Idea Details Page is displayed
                // Going to the first published idea in the list
                string ideaNumber = IMPublishedIdeas.browser.FindXPath("//*[@id='publishIdeasTable']/tbody[1]/tr[1]//a", Options.First).Text.Trim();
                IMPublishedIdeas.GoToPublishedIdeaNumber(ideaNumber);
                page_objects.imPublishedIdea IMPublishedIdea = new page_objects.imPublishedIdea(IMPublishedIdeas.browser);
                HpgAssert.AreEqual(IMPublishedIdea.IdeaNumber.Text, ideaNumber, "Verify idea " + ideaNumber + " details are loaded");
                //step 4 end

                //Step 5. Click Email Idea button
                //Expected Result: Send Idea Email dialog is displayed
                page_objects.imPublishedIdeaEmail emailDialog = new page_objects.imPublishedIdeaEmail(IMPublishedIdea.browser);
                IMPublishedIdea.LinkEmail.Element.Hover();
                IMPublishedIdea.LinkEmail.Click();                    
                if (!emailDialog.EmailDialog.Element.Exists())
                {
                    IMPublishedIdea.browser.FindId("emailButton").Click();
                    //IMPublishedIdea.LinkEmail.Element.Click();
                    //IMPublishedIdea.LinkEmail.Element.SendKeys(OpenQA.Selenium.Keys.Space);
                }
                HpgAssert.True(emailDialog.EmailDialog.Element.Exists(), "Verify Email Dialog is Displayed");
                //step 5 end

                //Step 6. Edit & Send email
                //Expected Result: Email is sent and all details are correct
                ews.FilterGreaterThan.Clear();
                ews.FilterGreaterThan.Add(new KeyValuePair<string, object>("sent", DateTime.Now.AddSeconds(-30)));
                //Fill out the form
                string emailSubject = emailDialog.SubjectField.Text + " - " + nameSuffix;
                string emailBody = "TEST EMAIL IDEA " + nameSuffix;
                string emailFrom = "hpg.automation@hcahealthcare.com";
                emailDialog.FromField.Type(emailFrom);
                emailDialog.ToField.Type("hpg.automation@hcahealthcare.com, hpgautomation@gmail.com");
                emailDialog.SubjectField.Type(emailSubject);
                emailDialog.MessageField.Type(emailBody);
                //Submit
                emailDialog.SendButton.Click();
                //Check for email
                ews.FilterContains.Clear();
                ews.FilterContains.Add(new KeyValuePair<string, string>("subject", emailSubject));
                DataTable msg = ews.GetMessagesDTWait(true, 4, 120);
                if (msg.Rows.Count.Equals(0))
                {
                    msg = ews.GetMessagesDTWait(true, 4, 120);
                }
                HpgAssert.True(msg.Rows.Count >= 1, "Verify single email was received");
                HpgAssert.Contains(msg.Rows[0]["body"].ToString(), emailDialog.browser.Location.ToString(), "Verify link is present on email");
                HpgAssert.Contains(msg.Rows[0]["body"].ToString(), emailBody, "Verify Email Body is present on email");
                HpgAssert.AreEqual(msg.Rows[0]["FromAddress"].ToString(), "hpg.automation@hcahealthcare.com", "Verify From address is correct on email");
                ews.DeleteMessage(msg.Rows[0]["ID"].ToString());
                //step 6 end

                //Step 7. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 7 end
            }
        }

        [Test]
        public void Test_TC7381()
        {
            //US10105: Existing Streetwise Idea Migration Idea Edit
            //Navigate to an Accepted Idea, go to Publish Idea, Change each field, Save, Verify changes

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");

                //Step 1. Navigate to webpage http://sbx-im.healthtrustpg.com
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Admin
                //Expected Result: Admin is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Navigate to All Ideas Page
                //Expected Result: All Ideas Page is displayed
                IMHome.GotoAllIdeas();
                page_objects.imAllIdeas allIdeas = new page_objects.imAllIdeas(IMHome.browser);
                //step 3 end

                //Step 4. Choose Publish action on an accepted idea
                //Expected Result: Publish Idea page is displayed
                // -- Select the first idea with status "Accepted"
                List<imAllIdeas.AllIdea> ideaList = allIdeas.GetAllIdeas();
                //imAllIdeas.AllIdea acceptedIdea = (from allIdea in ideaList
                //                                   where allIdea.Status.Trim().ToLower().Equals("accepted")
                //                                   select allIdea).First();
                //string ideaNumber = acceptedIdea.IdeaNumber.Text;
                //acceptedIdea.Action.Element.Hover();
                //acceptedIdea.Action.Element.FindLink("Action").Click();
                //acceptedIdea.Action.Element.FindLink("Publish Idea").Click();
                page_objects.im3PV publishIdea = new page_objects.im3PV(allIdeas.browser);
                HpgAssert.AreEqual("Publish Idea", publishIdea.pageHeader.Text.Trim(), "Verify Publish Idea page is loaded");
                //step 4 end

                //Step 5. Edit each field for each view and save
                //Expected Result: Each field is editable
                string newValue = "";
                Dictionary<string, string> originalValues = new Dictionary<string, string>();
                Dictionary<string, string> newValues = new Dictionary<string, string>();
                Dictionary<string, im3PV.IdeaValues> ideaValues = new Dictionary<string, im3PV.IdeaValues>();
                ideaValues.Add("HCA", publishIdea.GetIdeaValues(0));
                ideaValues.Add("GPO", publishIdea.GetIdeaValues(1));
                ideaValues.Add("SOURCETRUST", publishIdea.GetIdeaValues(2));
                foreach (KeyValuePair<string, im3PV.IdeaValues> ideaValue in ideaValues)
                {
                    //TODO: Action/Status
                    
                    // -- Title
                    originalValues.Add(ideaValue.Value.Title.Element.Id, ideaValue.Value.Title.Text); //store original value
                    newValue = ideaValue.Value.Title.Text + " -- TC7381:" + ideaValue.Key + nameSuffix;
                    ideaValue.Value.Title.Type(newValue);
                    newValues.Add(ideaValue.Value.Title.Element.Id, newValue);
                    
                    // -- Category
                    originalValues.Add(ideaValue.Value.Category.Element.Id, ideaValue.Value.Category.Element.SelectedOption);
                    newValue = ideaValue.Value.Category.SelectFirstNotSelected();
                    newValues.Add(ideaValue.Value.Category.Element.Id, newValue);
                    
                    // -- Department
                    originalValues.Add(ideaValue.Value.Department.Element.Id, ideaValue.Value.Department.Element.SelectedOption);
                    newValue = ideaValue.Value.Department.SelectFirstNotSelected();
                    newValues.Add(ideaValue.Value.Department.Element.Id, newValue);
                    
                    // -- Impact
                    originalValues.Add(ideaValue.Value.Impact.Element.Id, ideaValue.Value.Impact.Element.SelectedOption);
                    newValue = ideaValue.Value.Impact.SelectFirstNotSelected();
                    newValues.Add(ideaValue.Value.Impact.Element.Id, newValue);
                    
                    // -- Effort
                    newValue = ideaValue.Value.Effort.SelectFirstNotSelected();
                    originalValues.Add(ideaValue.Value.Effort.Element.Id, ideaValue.Value.Effort.Element.SelectedOption);
                    newValues.Add(ideaValue.Value.Effort.Element.Id, newValue);
                    
                    // -- Description
                    newValue = ideaValue.Value.Description.Text + " -- TC7381:" + ideaValue.Key + nameSuffix;
                    originalValues.Add(ideaValue.Value.Description.Element.Id, ideaValue.Value.Description.Text); //store original value
                    ideaValue.Value.Description.Type(newValue);
                    newValues.Add(ideaValue.Value.Description.Element.Id, newValue);
                    
                    // -- Results
                    newValue = ideaValue.Value.Results.Text + " -- TC7381:" + ideaValue.Key + nameSuffix;
                    originalValues.Add(ideaValue.Value.Results.Element.Id, ideaValue.Value.Results.Text); //store original value
                    ideaValue.Value.Results.Type(newValue);
                    newValues.Add(ideaValue.Value.Results.Element.Id, newValue);
                }
                WriteReport("New values entered, saving...");
                System.Threading.Thread.Sleep(3000);
                publishIdea.SaveAllButton.Click();
                System.Threading.Thread.Sleep(3000);
                publishIdea.SaveAllButton.Click();
                //step 5 end

                //Step 6. Reload page and verify changes were saved
                //Expected Result: Changes are reflected in each field
                publishIdea.GotoAllIdeas();
                ideaList = allIdeas.GetAllIdeas();
                //acceptedIdea = (from i in ideaList where i.IdeaNumber.Text.Trim().Equals(ideaNumber) select i).First();
                //acceptedIdea.Action.Element.Hover();
                //acceptedIdea.Action.Element.FindLink("Action").Click();
                //acceptedIdea.Action.Element.FindLink("Publish Idea").Click();
                foreach (KeyValuePair<string, im3PV.IdeaValues> ideaValue in ideaValues)
                {
                    //TODO: Action/Status
                    // -- Title
                    HpgAssert.AreEqual(newValues[ideaValue.Value.Title.Element.Id].Trim(), ideaValue.Value.Title.Text.Trim());
                    // -- Category
                    HpgAssert.AreEqual(newValues[ideaValue.Value.Category.Element.Id].Trim(), ideaValue.Value.Category.Element.SelectedOption.Trim());
                    // -- Department
                    HpgAssert.AreEqual(newValues[ideaValue.Value.Department.Element.Id].Trim(), ideaValue.Value.Department.Element.SelectedOption.Trim());
                    // -- Impact
                    HpgAssert.AreEqual(newValues[ideaValue.Value.Impact.Element.Id].Trim(), ideaValue.Value.Impact.Element.SelectedOption.Trim());
                    // -- Effort
                    HpgAssert.AreEqual(newValues[ideaValue.Value.Effort.Element.Id].Trim(), ideaValue.Value.Effort.Element.SelectedOption.Trim());
                    // -- Description
                    HpgAssert.AreEqual(newValues[ideaValue.Value.Description.Element.Id].Trim(), ideaValue.Value.Description.Text.Trim());
                    // -- Results
                    HpgAssert.AreEqual(newValues[ideaValue.Value.Results.Element.Id].Trim(), ideaValue.Value.Results.Text.Trim());
                }
                WriteReport("New values verified.  Restoring original values...");
                //step 6 end

                //Restore original values...
                foreach (KeyValuePair<string, string> originalValue in originalValues)
                {
                    publishIdea.FillFormField(originalValue.Key, originalValue.Value);
                }
                System.Threading.Thread.Sleep(3000);
                publishIdea.SaveAllButton.Click();
                System.Threading.Thread.Sleep(3000);
                publishIdea.SaveAllButton.Click();
                WriteReport("Original values restored.  Test for browser passed.");
                //original values restored

                //Step 7. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 7 end
            }
        }

        [Test][Ignore]
        public void Test_TC7663()
        {
            //US8529: As a user I need the ability to click a print button on the Idea Detail page and be able to print the idea
            //Navigate to an idea, click print, verify print dialog is displayed

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                //Step 1. Navigate to the Published Idea Details page
                //Expected Result: The details of a published Idea are displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                IMHome.GotoPublishedIdeas(false);
                page_objects.imPublishedIdeas IMPublishedIdeas = new page_objects.imPublishedIdeas(IMHome.browser);
                string ideaNumber = IMPublishedIdeas.browser.FindXPath("//*[@id='publishIdeasTable']/tbody[1]/tr[1]//a", Options.First).Text.Trim();
                IMPublishedIdeas.GoToPublishedIdeaNumber(ideaNumber);
                page_objects.imPublishedIdea IMPublishedIdea = new page_objects.imPublishedIdea(IMPublishedIdeas.browser);
                HpgAssert.AreEqual(IMPublishedIdea.IdeaNumber.Text, ideaNumber, "Verify idea " + ideaNumber + " details are loaded");
                //step 1 end

                //Step 2. Click the Print icon
                //Expected Result: Print Preview version is displayed
                IMPublishedIdea.LinkPrint.Element.Hover();
                IMPublishedIdea.LinkPrint.Click();
                BrowserWindow pagePrint = IMHome.browser.FindWindow("Publish Idea Print Preview");

                page_objects.imPublishedIdeaPrint printPage = new imPublishedIdeaPrint(pagePrint);
                //step 2 end

                //Step 3. Verify Print option is on Print Preview page
                //Expected Result: Print button is available on Print Preview page
                //step 3 end

                //Step 4. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 4 end
            }
        }

        [Test]
        public void Test_TC7818()
        {
            RallyTestID = "TC7818";
            //Copy Text from Description one box to the next
            //Navigate to Publish Idea page for an Accepted Idea, copy text into empty text boxes, verify, copy text into non-empty text boxes, verify dialog.

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();

                //Step 1. Navigate to webpage http://sbx-im.healthtrustpg.com
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome =
                    new page_objects.imHome((BrowserSession) BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Admin
                //Expected Result: Admin is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], Controller.fields["Password"]);
                //step 2 end

                //Step 3. Navigate to All Ideas Page
                //Expected Result: All Ideas Page is displayed
                IMHome.GotoAllIdeas();
                page_objects.imAllIdeas allIdeas = new page_objects.imAllIdeas(IMHome.browser);
                //step 3 end

                //Step 4. Choose Publish action on an accepted idea
                //Expected Result: Publish Idea page is displayed
                // -- Select the first idea with status "Accepted"
                List<imAllIdeas.AllIdea> ideaList = allIdeas.GetAllIdeas();
                imAllIdeas.AllIdea acceptedIdea = (from allIdea in ideaList
                                                   where allIdea.Status.Trim().ToLower().Equals("accepted")
                                                   select allIdea).First();
                //string ideaNumber = acceptedIdea.IdeaNumber.Text;
                //acceptedIdea.Action.Element.Hover();
                //acceptedIdea.Action.Element.FindLink("Action").Click();
                //acceptedIdea.Action.Element.FindLink("Publish Idea").Click();
                page_objects.im3PV publishIdea = new page_objects.im3PV(allIdeas.browser);
                HpgAssert.AreEqual("Publish Idea", publishIdea.pageHeader.Text.Trim(),
                                   "Verify Publish Idea page is loaded");
                //step 4 end

                //Step 5. Clear the description of all versions of the Idea.
                //Expected Result: All version's Description field is blank.
                Dictionary<string, im3PV.IdeaValues> ideaValues = new Dictionary<string, im3PV.IdeaValues>();
                ideaValues.Add("HCA", publishIdea.GetIdeaValues(0));
                ideaValues.Add("GPO", publishIdea.GetIdeaValues(1));
                ideaValues.Add("SOURCETRUST", publishIdea.GetIdeaValues(2));
                foreach (KeyValuePair<string, im3PV.IdeaValues> ideaValue in ideaValues)
                {
                    ideaValue.Value.Description.Type("");
                }
                //step 5 end

                //Step 6. Modify the description text for the HCA version of the Idea. Click Copy
                //Expected Result: The modified HCA description text is copied to the GPO description field.
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");
                ideaValues["HCA"].Description.Type("HCA" + nameSuffix);
                publishIdea.CopyDescription1.Hover();
                publishIdea.CopyDescription1.Click();
                HpgAssert.AreEqual(ideaValues["HCA"].Description.Element.Value, ideaValues["GPO"].Description.Element.Value, "Verify GPO description matches HCA description");
                //step 6 end

                //Step 7. Modify the description text for the GPO version of the Idea. Click Copy
                //Expected Result: The modified GPO description text is copied to the Sourcetrust description field.
                ideaValues["GPO"].Description.Type("GPO" + nameSuffix);
                publishIdea.CopyDescription2.Hover();
                publishIdea.CopyDescription2.Click();
                HpgAssert.AreEqual(ideaValues["GPO"].Description.Element.Value, ideaValues["SOURCETRUST"].Description.Element.Value, "Verify SOURCETRUST description matches GPO description");
                //step 7 end

                //Step 8. Modify the description text for the HCA version of the Idea. Click Copy. Verify Dialog is present and choose Cancel
                //Expected Result: The modified HCA description is NOT copied to the GPO description field
                nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");
                ideaValues["HCA"].Description.Type("HCA" + nameSuffix);
                publishIdea.CopyDescription1.Hover();
                publishIdea.CopyDescription1.Click();
                cancelDialog(publishIdea.browser);
                HpgAssert.False(ideaValues["HCA"].Description.Element.Value.Equals(ideaValues["GPO"].Description.Element.Value), "Verify GPO description did not change");
                //step 8 end

                //Step 9. Modify the description text for the HCA version of the Idea. Click Copy. Verify Dialog is present and choose OK
                //Expected Result: The modified HCA description text is copied to the GPO description field.
                ideaValues["GPO"].Description.Type("GPO" + nameSuffix);
                publishIdea.CopyDescription2.Hover();
                publishIdea.CopyDescription2.Click();
                cancelDialog(publishIdea.browser);
                HpgAssert.False(ideaValues["GPO"].Description.Element.Value.Equals(ideaValues["SOURCETRUST"].Description.Element.Value), "Verify SOURCETRUST description did not change");
                //step 9 end

                //Step 10. Modify the description text for the GPO version of the Idea. Click Copy. Verify Dialog is present and choose Cancel
                //Expected Result: The modified GPO description is NOT copied to the SourceTrust description field
                nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");
                ideaValues["HCA"].Description.Type("HCA" + nameSuffix);
                publishIdea.CopyDescription1.Hover();
                publishIdea.CopyDescription1.Click();
                acceptDialog(publishIdea.browser, testBrowser.Key);
                System.Threading.Thread.Sleep(500);
                HpgAssert.True(ideaValues["HCA"].Description.Element.Value.Equals(ideaValues["GPO"].Description.Element.Value), "Verify GPO description matches HCA description");
                //step 10 end

                //Step 11. Modify the description text for the GPO version of the Idea. Click Copy. Verify Dialog is present and choose OK
                //Expected Result: The modified GPO description text is copied to the SourceTrust description field.
                ideaValues["GPO"].Description.Type("GPO" + nameSuffix);
                publishIdea.CopyDescription2.Hover();
                publishIdea.CopyDescription2.Click();
                acceptDialog(publishIdea.browser, testBrowser.Key);
                System.Threading.Thread.Sleep(500);
                HpgAssert.True(ideaValues["GPO"].Description.Element.Value.Equals(ideaValues["SOURCETRUST"].Description.Element.Value), "Verify SOURCETRUST description matches GPO description");
                //step 11 end
            }
        }

        [Test]
        public void Test_TC8029()
        {
            RallyTestID = "";
            //US9705: Reject & Approve new Streetwise Ideas using the Migration Maintenance Tool
            //Navigate to Tool page, reject a new idea, verify rejected idea, approve rejected idea, verify approved idea, approve new idea, verify approved idea.

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();

                //Step 1. Navigate to webpage http://sbx-im.healthtrustpg.com
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome =
                    new page_objects.imHome((BrowserSession) BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Admin
                //Expected Result: Admin is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"],
                                    Controller.fields["Password"]);
                //step 2 end

                //Step 3. Navigate to New Streetwise Ideas
                //Expected Result: New Streetwise Ideas page is displayed
                WriteReport("----- Step 3 -----");
                IMHome.GotoNewStreetwiseIdeas();
                imNewStreetwiseIdeas streetwiseIdeas = new imNewStreetwiseIdeas(IMHome.browser);
                streetwiseIdeas.FilterNewIdeas();
                List<imNewStreetwiseIdeas.NewIdea> ideaList = streetwiseIdeas.GetAllIdeas();
                HpgAssert.True(ideaList.Any(), "Verify atleast one idea is loaded on page");
                //step 3 end

                //Step 4. Reject a new idea
                //Expected Result: Idea is rejected and not visible in All Ideas
                // -- Grab the first idea that is not already rejected
                WriteReport("----- Step 4 -----");
                imNewStreetwiseIdeas.NewIdea testIdea = (from i in ideaList where i.CurrentAction.Equals("no action") select i).First();
                string rejectIdeaId = testIdea.IdeaID.Trim();
                WriteReport("Rejecting idea " + rejectIdeaId + "...");
                streetwiseIdeas.ScrollToBottom();
                testIdea.Action.Element.SendKeys(OpenQA.Selenium.Keys.Home);
                testIdea.Action.Click();
                testIdea.Action.SelectListOptionByText("Reject");
                testIdea.Action.Element.SendKeys(OpenQA.Selenium.Keys.Enter);
                streetwiseIdeas.SubmitButton.Element.Hover();
                streetwiseIdeas.SubmitButton.Click();
                HpgAssert.True(streetwiseIdeas.SubmitButton.Element.Missing(), "Submit changes on New Streetwise Ideas page");
                //TODO: Uncomment below code after change to Accept/Reject/Accepted/Update on New Streetwise Ideas page
                //IMHome.GotoAllIdeas();
                //imAllIdeas allIdeas = new imAllIdeas(IMHome.browser);
                //allIdeas.SearchFilterForm.Type(rejectIdeaId);
                //System.Threading.Thread.Sleep(2000);
                //List<imAllIdeas.AllIdea> allIdeasList = allIdeas.GetAllIdeas();
                //HpgAssert.False(allIdeasList.Select(i => i.IdeaNumber.Text.Equals(rejectIdeaId)).Any(), "Verify there are no results when searching All Ideas for rejected idea (" + rejectIdeaId + ")");
                IMHome.GotoNewStreetwiseIdeas();
                IMHome.Refresh();
                streetwiseIdeas.FilterNewIdeas();
                streetwiseIdeas.SearchBox.Type(rejectIdeaId);
                streetwiseIdeas.SearchButton.Click();
                ideaList = streetwiseIdeas.GetAllIdeas(true);
                HpgAssert.False((from i in ideaList where i.IdeaID.Trim().Equals(rejectIdeaId) select i).Any(), "Verify the rejected idea does not show up on New Streetwise Ideas page");
                streetwiseIdeas.FilterAcceptedIdeas();
                streetwiseIdeas.SearchBox.Type(rejectIdeaId);
                streetwiseIdeas.SearchButton.Click();
                ideaList = streetwiseIdeas.GetAllIdeas(true);
                HpgAssert.False((from i in ideaList where i.IdeaID.Trim().Equals(rejectIdeaId) select i).Any(), "Verify the rejected idea does not show up on Accepted Streetwise Ideas page");
                streetwiseIdeas.FilterRejectedIdeas();
                streetwiseIdeas.SearchBox.Type(rejectIdeaId);
                streetwiseIdeas.SearchButton.Click();
                ideaList = streetwiseIdeas.GetAllIdeas(true);
                testIdea = (from i in ideaList where i.IdeaID.Trim().Equals(rejectIdeaId) select i).First();
                HpgAssert.AreEqual(rejectIdeaId, testIdea.IdeaID, "Verify the rejected idea shows up on Rejected Streetwise Ideas page");
                //step 4 end

                //Step 5. Approve rejected idea
                //Expected Result: Idea is approved and is visible once in All Ideas
                WriteReport("----- Step 5 -----");
                WriteReport("Approving idea " + rejectIdeaId + "...");
                streetwiseIdeas.ScrollToBottom();
                testIdea.Action.Element.SendKeys(OpenQA.Selenium.Keys.Home);
                testIdea.Action.Click();
                testIdea.Action.SelectListOptionByText("Accept");
                streetwiseIdeas.SubmitButton.Element.Hover();
                streetwiseIdeas.SubmitButton.Click();
                HpgAssert.True(streetwiseIdeas.SubmitButton.Element.Missing(), "Submit changes on New Streetwise Ideas page");
                IMHome.GotoNewStreetwiseIdeas();
                IMHome.Refresh();
                //TODO: Verify idea is now in All Ideas
                streetwiseIdeas.FilterNewIdeas();
                streetwiseIdeas.SearchBox.Type(rejectIdeaId);
                streetwiseIdeas.SearchButton.Click();
                ideaList = streetwiseIdeas.GetAllIdeas(true);
                HpgAssert.False((from i in ideaList where i.IdeaID.Trim().Equals(rejectIdeaId) select i).Any(), "Verify the Accepted idea does not show up on New Streetwise Ideas page");
                streetwiseIdeas.FilterRejectedIdeas();
                streetwiseIdeas.SearchBox.Type(rejectIdeaId);
                streetwiseIdeas.SearchButton.Click();
                ideaList = streetwiseIdeas.GetAllIdeas(true);
                HpgAssert.False((from i in ideaList where i.IdeaID.Trim().Equals(rejectIdeaId) select i).Any(), "Verify the Accepted idea does not show up on Rejected Streetwise Ideas page");
                streetwiseIdeas.FilterAcceptedIdeas();
                streetwiseIdeas.SearchBox.Type(rejectIdeaId);
                streetwiseIdeas.SearchButton.Click();
                ideaList = streetwiseIdeas.GetAllIdeas(true);
                testIdea = (from i in ideaList where i.IdeaID.Trim().Equals(rejectIdeaId) select i).First();
                HpgAssert.AreEqual(rejectIdeaId, testIdea.IdeaID, "Verify the Accepted idea shows up on Accepted Streetwise Ideas page");
                //step 5 end

                //Step 6. Approve new idea
                //Expected Result: Idea is approved and is visible once in All Ideas
                WriteReport("----- Step 6 -----");
                IMHome.GotoNewStreetwiseIdeas();
                streetwiseIdeas.FilterNewIdeas();
                ideaList = streetwiseIdeas.GetAllIdeas();
                testIdea = (from i in ideaList where i.CurrentAction.Equals("no action") select i).First();
                rejectIdeaId = testIdea.IdeaID;
                WriteReport("Accepting idea " + rejectIdeaId + "...");
                streetwiseIdeas.ScrollToBottom();
                testIdea.Action.Element.SendKeys(OpenQA.Selenium.Keys.Home);
                testIdea.Action.Click();
                testIdea.Action.SelectListOptionByText("Accept");
                streetwiseIdeas.SubmitButton.Element.Hover();
                streetwiseIdeas.SubmitButton.Click();
                HpgAssert.True(streetwiseIdeas.SubmitButton.Element.Missing(), "Submit changes on New Streetwise Ideas page");
                IMHome.GotoNewStreetwiseIdeas();
                IMHome.Refresh();
                //TODO: Verify idea is now in All Ideas

                streetwiseIdeas.FilterNewIdeas();
                streetwiseIdeas.SearchBox.Type(rejectIdeaId);
                streetwiseIdeas.SearchButton.Click();
                ideaList = streetwiseIdeas.GetAllIdeas(true);
                HpgAssert.False((from i in ideaList where i.IdeaID.Trim().Equals(rejectIdeaId) select i).Any(), "Verify the Accepted idea does not show up on New Streetwise Ideas page");
                streetwiseIdeas.FilterRejectedIdeas();
                streetwiseIdeas.SearchBox.Type(rejectIdeaId);
                streetwiseIdeas.SearchButton.Click();
                ideaList = streetwiseIdeas.GetAllIdeas(true);
                HpgAssert.False((from i in ideaList where i.IdeaID.Trim().Equals(rejectIdeaId) select i).Any(), "Verify the Accepted idea does not show up on Rejected Streetwise Ideas page");
                streetwiseIdeas.FilterAcceptedIdeas();
                streetwiseIdeas.SearchBox.Type(rejectIdeaId);
                streetwiseIdeas.SearchButton.Click();
                ideaList = streetwiseIdeas.GetAllIdeas(true);
                testIdea = (from i in ideaList where i.IdeaID.Trim().Equals(rejectIdeaId) select i).First();
                HpgAssert.AreEqual(rejectIdeaId, testIdea.IdeaID, "Verify the Accepted idea shows up on Accepted Streetwise Ideas page");
                //step 6 end

                //Step 7. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                WriteReport("----- Step 7 -----");
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 7 end
            }
        }

        [Test][Ignore]
        public void Test_TC8030()
        {
            RallyTestID = "";
            //US11267: Filter / Sorting Imported Ideas
            //Navigate to Tool page, Apply New Filter, verify ideas displayed, Apply Approved Filter, verify ideas displayed, Apply Rejected Filter, verify ideas displayed

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();

                //Step 1. Navigate to webpage http://sbx-im.healthtrustpg.com
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome =
                    new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Admin
                //Expected Result: Admin is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"],
                                    Controller.fields["Password"]);
                //step 2 end

                //Step 3. Navigate to New Streetwise Ideas
                //Expected Result: New Streetwise Ideas page is displayed
                IMHome.GotoNewStreetwiseIdeas();
                imNewStreetwiseIdeas newStreetwiseIdeas = new imNewStreetwiseIdeas(IMHome.browser);
                //step 3 end

                //Step 4. Apply New Filter
                //Expected Result: Verify ideas displayed
                newStreetwiseIdeas.FilterNewIdeas();
                DataTable ideaList = new DataTable("IdeaList");
                ideaList = newStreetwiseIdeas.GetAllIdeasDT();
                HpgAssert.False(ideaList.Select("Action <> 'no action'").Any(), "Verify all ideas are marked as 'No Action'");
                HpgAssert.AreEqual(newStreetwiseIdeas.ResultCount.Text.Substring(newStreetwiseIdeas.ResultCount.Text.LastIndexOf(":") + 1).Trim(), ideaList.Rows.Count.ToString(), "Verify number of records is correct");
                //step 4 end

                //Step 5. Apply Approved Filter
                //Expected Result: Verify ideas displayed
                newStreetwiseIdeas.FilterAcceptedIdeas();
                ideaList = newStreetwiseIdeas.GetAllIdeasDT();
                HpgAssert.False(ideaList.Select("Action <> 'accepted'").Any(), "Verify all ideas are marked as 'Accepted'");
                HpgAssert.AreEqual(newStreetwiseIdeas.ResultCount.Text.Substring(newStreetwiseIdeas.ResultCount.Text.LastIndexOf(":") + 1).Trim(), ideaList.Rows.Count.ToString(), "Verify number of records is correct");
                //step 5 end

                //Step 6. Apply Rejected Filter
                //Expected Result: Verify ideas displayed
                newStreetwiseIdeas.FilterRejectedIdeas();
                ideaList = newStreetwiseIdeas.GetAllIdeasDT();
                HpgAssert.False(ideaList.Select("Action <> 'reject'").Any(), "Verify all ideas are marked as 'Rejected'");
                HpgAssert.AreEqual(newStreetwiseIdeas.ResultCount.Text.Substring(newStreetwiseIdeas.ResultCount.Text.LastIndexOf(":") + 1).Trim(), ideaList.Rows.Count.ToString(), "Verify number of records is correct");
                //step 6 end

                //Step 7. Apply All Filter
                //Expected Result: Verify ideas displayed
                newStreetwiseIdeas.FilterAllIdeas();
                ideaList = newStreetwiseIdeas.GetAllIdeasDT();
                HpgAssert.AreEqual(newStreetwiseIdeas.ResultCount.Text.Substring(newStreetwiseIdeas.ResultCount.Text.LastIndexOf(":") + 1).Trim(), ideaList.Rows.Count.ToString(), "Verify number of records is correct");
                //step 7 end

                //Step 8. Apply Compound Filters
                //Expected Result: Verify Ideas Displayed
                newStreetwiseIdeas.FilterNewIdeas();
                newStreetwiseIdeas.ScrollToBottom();
                ideaList = newStreetwiseIdeas.GetAllIdeasDT();
                var filterCounts = from row in ideaList.AsEnumerable()
                                   group row by row.Field<string>("Department")
                                       into grp
                                       select new { Name = grp.Key, Count = grp.Count() };
                string filterName = (from dep in filterCounts orderby dep.Count descending select dep.Name).First();
                int filterCount = (from dep in filterCounts orderby dep.Count descending select dep.Count).First();
                newStreetwiseIdeas.FilterShowMoreDepartments.Click();
                newStreetwiseIdeas.FilterDepartment(filterName).Check();
                ideaList = newStreetwiseIdeas.GetAllIdeasDT();
                HpgAssert.False(ideaList.Select("Department <> '" + filterName + "'").Any(), "Verify all ideas are in department '" + filterName + "'");
                HpgAssert.AreEqual(newStreetwiseIdeas.ResultCount.Text.Substring(newStreetwiseIdeas.ResultCount.Text.LastIndexOf(":") + 1).Trim(), ideaList.Rows.Count.ToString(), "Verify displayed number of records is correct");
                HpgAssert.AreEqual(filterCount.ToString(), ideaList.Rows.Count.ToString(), "Verify number of records is correct");
                //step 8 end

                //Step 9. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 9 end
            }
        }

        [Test][Ignore]
        public void Test_TC8353()
        {
            RallyTestID = "";
            //US11267: Search by ID, Title, Description
            //Search for and idea by ID, Title, then Description, apply filter(s) and search again.

            foreach (KeyValuePair<string, Browser> testBrowser in ffChromeOnly)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                

                //Step 1. Navigate to Idea Management Application
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome =
                    new page_objects.imHome((BrowserSession) BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Admin
                //Expected Result: Admin is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"],
                                           Controller.fields["Password"]);
                //step 2 end

                //Step 3. Navigate to New Streetwise Ideas
                //Expected Result: New Streetwise Ideas page is displayed
                IMHome.GotoNewStreetwiseIdeas();
                //Grab an idea to search for.
                imNewStreetwiseIdeas streetwiseIdeas = new imNewStreetwiseIdeas(IMHome.browser);
                List<imNewStreetwiseIdeas.NewIdea> ideaList = streetwiseIdeas.GetAllIdeas();
                HpgAssert.True(ideaList.Any(), "Verify atleast one idea is loaded on page");
                List<imNewStreetwiseIdeas.IdeaText> searchCriteria = new List<imNewStreetwiseIdeas.IdeaText>();
                foreach (imNewStreetwiseIdeas.NewIdea idea in ideaList.Take(3))
                {
                    imNewStreetwiseIdeas.IdeaText addIdea = new imNewStreetwiseIdeas.IdeaText();
                    addIdea.IdeaID = idea.IdeaID;
                    addIdea.IdeaName = idea.IdeaName.Text.Trim();
                    idea.IdeaName.Element.Hover();
                    idea.IdeaName.Element.FindXPath(".//a").Click();
                    addIdea.IdeaDescription = streetwiseIdeas.popupPublishedDescription.Text.Trim();
                    searchCriteria.Add(addIdea);
                    streetwiseIdeas.popupCloseButton.Click();
                }


                //var searchCriteria = (from i in ideaList.Take(3) select new {IdeaID = i.IdeaID, IdeaName = i.IdeaName.Text, IdeaDescription = ""}).ToList();
                //ideaList.ElementAt(2).IdeaName.Click();
                //searchCriteria.ElementAt(2).IdeaDescription = streetwiseIdeas.browser.FindId("detailsPublishedDescription").Text.Trim();
                //step 3 end

                //Step 4. Search for an Idea by ID
                //Expected Result: Verify ideas displayed
                streetwiseIdeas.SearchBox.Type(searchCriteria.ElementAt(0).IdeaID);
                streetwiseIdeas.SearchButton.Click();
                streetwiseIdeas.ScrollToBottom();
                DataTable ideaTable = streetwiseIdeas.GetAllIdeasDT();
                HpgAssert.True(ideaTable.Select("IdeaId = " + searchCriteria.ElementAt(0).IdeaID).Any(), "Verify idea '" + searchCriteria.ElementAt(0).IdeaID + "' is listed");
                //step 4 end

                //Step 5. Search for an Idea by Title
                //Expected Result: Verify ideas displayed
                streetwiseIdeas.SearchBox.Type(searchCriteria.ElementAt(1).IdeaName);
                streetwiseIdeas.SearchButton.Click();
                streetwiseIdeas.ScrollToBottom();
                ideaTable = streetwiseIdeas.GetAllIdeasDT();
                HpgAssert.True(ideaTable.Select("IdeaId = " + searchCriteria.ElementAt(1).IdeaID).Any(), "Verify idea '" + searchCriteria.ElementAt(1).IdeaID + "' is listed");
                //step 5 end

                //Step 6. Search for an Idea by Description
                //Expected Result: Verify ideas displayed
                string searchthis = searchCriteria.ElementAt(2).IdeaDescription;
                searchthis = searchthis.Substring(0, searchthis.Substring(0,40).LastIndexOf(' ')).Trim();
                streetwiseIdeas.SearchBox.Type(searchthis);
                streetwiseIdeas.SearchButton.Click();
                streetwiseIdeas.ScrollToBottom();
                ideaTable = streetwiseIdeas.GetAllIdeasDT();
                HpgAssert.True(ideaTable.Select("IdeaId = " + searchCriteria.ElementAt(2).IdeaID).Any(), "Verify idea '" + searchCriteria.ElementAt(2).IdeaID + "' is listed");
                //step 6 end
                
                //Step 7. Apply Filter(s)
                //Expected Result: Filter(s) are applied
                streetwiseIdeas.SearchReset.Click();
                streetwiseIdeas.FilterIdeaViewAccepted.Check();
                streetwiseIdeas.ScrollToBottom();
                ideaTable = streetwiseIdeas.GetAllIdeasDT();
                var filterCounts = from row in ideaTable.AsEnumerable()
                                   group row by row.Field<string>("Department")
                                       into grp
                                       select new { Name = grp.Key, Count = grp.Count() };
                string filterName = (from dep in filterCounts where dep.Count > 4 orderby dep.Count ascending select dep.Name).First();
                streetwiseIdeas.FilterShowMoreDepartments.Click();
                streetwiseIdeas.FilterDepartment(filterName).Check();
                ideaList = streetwiseIdeas.GetAllIdeas();
                HpgAssert.True(ideaList.Count > 0, "Verify atleast one idea is loaded on page");
                foreach (imNewStreetwiseIdeas.NewIdea idea in ideaList.Take(3))
                {
                    imNewStreetwiseIdeas.IdeaText addIdea = new imNewStreetwiseIdeas.IdeaText();
                    addIdea.IdeaID = idea.IdeaID;
                    addIdea.IdeaName = idea.IdeaName.Text.Trim();
                    idea.IdeaName.Click();
                    addIdea.IdeaDescription = streetwiseIdeas.popupPublishedDescription.Text.Trim();
                    streetwiseIdeas.popupCloseButton.Click();
                    searchCriteria.Add(addIdea);
                }
                //step 7 end

                //Step 8. Search for an Idea by ID
                //Expected Result: Verify ideas displayed
                streetwiseIdeas.SearchBox.Type(searchCriteria.ElementAt(0).IdeaID);
                streetwiseIdeas.SearchButton.Click();
                streetwiseIdeas.ScrollToBottom();
                ideaTable = streetwiseIdeas.GetAllIdeasDT();
                HpgAssert.True(ideaTable.Select("IdeaId = " + searchCriteria.ElementAt(0).IdeaID).Any(), "Verify idea '" + searchCriteria.ElementAt(0).IdeaID + "' is listed");
                //step 8 end

                //Step 9. Search for an Idea by Title
                //Expected Result: Verify ideas displayed
                streetwiseIdeas.SearchBox.Type(searchCriteria.ElementAt(1).IdeaName);
                streetwiseIdeas.SearchButton.Click();
                streetwiseIdeas.ScrollToBottom();
                ideaTable = streetwiseIdeas.GetAllIdeasDT();
                HpgAssert.True(ideaTable.Select("IdeaId = " + searchCriteria.ElementAt(1).IdeaID).Any(), "Verify idea '" + searchCriteria.ElementAt(1).IdeaID + "' is listed");
                //step 9 end

                //Step 10. Search for an Idea by Description
                //Expected Result: Verify ideas displayed
                searchthis = searchCriteria.ElementAt(2).IdeaDescription;
                searchthis = searchthis.Substring(0, searchthis.Substring(0, 40).LastIndexOf(' ')).Trim();
                streetwiseIdeas.SearchBox.Type(searchthis);
                streetwiseIdeas.SearchButton.Click();
                streetwiseIdeas.ScrollToBottom();
                ideaTable = streetwiseIdeas.GetAllIdeasDT();
                HpgAssert.True(ideaTable.Select("IdeaId = " + searchCriteria.ElementAt(2).IdeaID).Any(), "Verify idea '" + searchCriteria.ElementAt(2).IdeaID + "' is listed");
                //step 10 end

                //Step 11. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 11 end
            }
        }

        [Test][Ignore]
        public void Test_TC8381()
        {
            //US8233: Packaging Idea Page - Link Editing
            //Create an idea, add links, publish, verify links, edit links, verify edit
            RallyTestID = "TC8381";

            KeyValuePair<string, string> linkOne = new KeyValuePair<string, string>("Fox News", "http://www.foxnews.com");
            KeyValuePair<string, string> linkTwo = new KeyValuePair<string, string>("Xpanxion", "http://www.xpanxion.com");

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");
                DBUtility dbUtility = new DBUtility();

                //Step 1. Navigate to Idea Management Application
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession) BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Admin/SME
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"],
                                           Controller.fields["Password"]);
                //step 2 end

                //Step 3. Create and Submit an Idea and Submit
                //Expected Result: Idea is created with all information
                int newIdeaID = dbUtility.CreateSavedIdea("Automation TC8381 " + nameSuffix,
                                                          "Automation Description for TC8381-" + nameSuffix,
                                                          "",
                                                          "Automation Contact Name " + nameSuffix);
                dbUtility.AcceptIdea(newIdeaID);
                //step 3 end

                //Step 4. Navigate to Publish Idea page
                //Expected Result: Publish Idea Page is displayed
                IMHome.GoToIdeaNumber(newIdeaID.ToString());
                page_objects.imIdeaDetailsAdmin IMIdeaDetails = new page_objects.imIdeaDetailsAdmin(IMHome.browser);
                IMIdeaDetails.PublishIdeaButton.Click();
                page_objects.im3PV publishIdea = new page_objects.im3PV(IMIdeaDetails.browser);
                publishIdea.ExpandAllButton.Click();
                //step 4 end

                //Step 5. Add new link to idea and publish
                //Expected result: link is added to list
                publishIdea.EditLinks.Click();
                HpgAssert.True(publishIdea.EditLinkDialog.Element.Exists(), "Verify 'Edit Link' dialog is present");
                publishIdea.EditLinkAddNewLink.Click();
                publishIdea.EditLinksDialogGetDescriptions().Last().Type(linkOne.Key);
                publishIdea.EditLinksDialogGetURLs().Last().Type(linkOne.Value);
                publishIdea.EditLinkSave.Click();
                WriteReport("New values entered, saving...");
                System.Threading.Thread.Sleep(6000);
                publishIdea.SaveAllButton.Click();
                System.Threading.Thread.Sleep(3000);
                publishIdea.SaveAllButton.Click();
                System.Threading.Thread.Sleep(3000);
                dbUtility.PublishQualifiedIdea(newIdeaID);
                //step 5 end

                //Step 6. Navigate to Published Idea and verify link
                //Expected result: Link is present and correct
                IMHome.GoToPublishedIdeaNumber(newIdeaID.ToString());
                imPublishedIdea publishedIdea = new imPublishedIdea(IMHome.browser);
                publishedIdea.Refresh();
                //grab all the links and filter for the correct one (disregard any others)
                HpgElement publishedLink = (from l in publishedIdea.GetAllLinks() where l.Text.Trim().Equals(linkOne.Key) select l).First();
                //HpgAssert.True(publishedLink.Element.Exists(), "Verify link is displayed");
                HpgAssert.Contains(publishedLink.Element["href"], linkOne.Value, "Verify link is correct");
                //step 6 end

                //Step 7. Delete current link and add a different one
                //Expected result: Original link is deleted and new one is added
                IMHome.GoToIdeaNumber(newIdeaID.ToString());
                IMIdeaDetails = new page_objects.imIdeaDetailsAdmin(IMHome.browser);
                IMIdeaDetails.PublishIdeaButton.Click();
                publishIdea = new page_objects.im3PV(IMIdeaDetails.browser);
                publishIdea.ExpandAllButton.Click();
                publishIdea.EditLinks.Click();
                HpgAssert.True(publishIdea.EditLinkDialog.Element.Exists(), "Verify 'Edit Link' dialog is present");
                //Delete last link (assuming it's the one added in Step 5)
                publishIdea.EditLinksDialogGetDeleteLinks().Last().Click();
                //Add new link
                publishIdea.EditLinkAddNewLink.Click();
                publishIdea.EditLinksDialogGetDescriptions().Last().Type(linkTwo.Key);
                publishIdea.EditLinksDialogGetURLs().Last().Type(linkTwo.Value);
                publishIdea.EditLinkSave.Click();
                WriteReport("New values entered, saving...");
                System.Threading.Thread.Sleep(6000);
                publishIdea.SaveAllButton.Click();
                System.Threading.Thread.Sleep(3000);
                publishIdea.SaveAllButton.Click();
                System.Threading.Thread.Sleep(3000);
                dbUtility.PublishQualifiedIdea(newIdeaID);
                //step 7 end

                //Step 8. Verify deleted link is no longer present and new link is
                //Expected result: Deleted link is not present.  New link is present
                IMHome.GoToPublishedIdeaNumber(newIdeaID.ToString());
                publishedIdea.Refresh();
                //grab all the links and filter for the correct one (disregard any others)
                HpgAssert.False((from l in publishedIdea.GetAllLinks() where l.Text.Trim().Equals(linkOne.Key) select l).Any(), "Verify deleted link is not present");
                publishedLink = (from l in publishedIdea.GetAllLinks() where l.Text.Trim().Equals(linkTwo.Key) select l).First();
                //HpgAssert.True(publishedLink.Element.Exists(), "Verify new link is displayed");
                HpgAssert.Contains(publishedLink.Element["href"], linkTwo.Value, "Verify new link is correct");
                //step 8 end

                //Step 9. Disable added link on all views
                //Expected result: link checkboxes are unchecked
                IMHome.GoToIdeaNumber(newIdeaID.ToString());
                IMIdeaDetails.PublishIdeaButton.Click();
                publishIdea.ExpandAllButton.Click();
                //Loop through each view and uncheck box for linkTwo
                for (int view = 0; view < 3; view++)
                {
                    (from l in publishIdea.GetIdeaLinks(view) where l.Title.Text.Trim().Equals(linkTwo.Key) select l.Box).First().UnCheck();
                }
                WriteReport("New values entered, saving...");
                System.Threading.Thread.Sleep(6000);
                publishIdea.SaveAllButton.Click();
                System.Threading.Thread.Sleep(3000);
                publishIdea.SaveAllButton.Click();
                System.Threading.Thread.Sleep(3000);
                dbUtility.PublishQualifiedIdea(newIdeaID);
                //step 9 end

                //Step 10. Verify new link is not visible
                //Expected result: linkTwo is not visible on published idea details page
                IMHome.GoToPublishedIdeaNumber(newIdeaID.ToString());
                publishedIdea.Refresh();
                //grab all the links and filter for the correct one (disregard any others)
                HpgAssert.False((from l in publishedIdea.GetAllLinks() where l.Text.Trim().Equals(linkTwo.Key) select l).Any(), "Verify deleted link is not present");
                //step 10 end

                //Clean-up
                //Delete idea completely
                dbUtility.DeleteIdeaByIdeaNumber(newIdeaID);
                //clean-up end

                //Step 11. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 11 end
            }
        }

        [Test][Ignore]
        public void Test_TC9999()
        {
            //Streetwise Import test updated ideas
            //Verify updated idea compares changes, accept changes, verify changes

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();

                //Step 1. Navigate to Idea Management Application
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome =
                    new page_objects.imHome((BrowserSession) BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Admin/SME
                //Expected Result: Admin/SME is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"], BaseTest.GetPasswordForUser(Controller.fields["Domain"] + "\\" + Controller.fields["UserId"]));
                //step 2 end

                //Step 3. Navigate to New Streetwise Ideas
                //Expected Result: New Streetwise Ideas page is displayed
                WriteReport("----- Step 3 -----");
                IMHome.GotoNewStreetwiseIdeas();
                imNewStreetwiseIdeas streetwiseIdeas = new imNewStreetwiseIdeas(IMHome.browser);
                streetwiseIdeas.FilterAcceptedIdeas();
                List<imNewStreetwiseIdeas.NewIdea> ideaList = streetwiseIdeas.GetAllIdeas();
                HpgAssert.True(ideaList.Any(), "Verify atleast one idea is loaded on page");
                //step 3 end

                //Step 4. Verify comparison is present for updated idea
                //Expected Result: Popup comparison is present for any idea that has been updated
                // -- Grab the first idea that is updated
                WriteReport("----- Step 4 -----");
                imNewStreetwiseIdeas.NewIdea testIdea = (from i in ideaList where i.CurrentAction.Equals("pending") select i).First();
                string testIdeaId = testIdea.IdeaID.Trim();
                WriteReport("Verifying idea " + testIdeaId + "...");
                testIdea.IdeaName.Click();
                HpgAssert.True(streetwiseIdeas.popupCompareOriginalDescription.Element.Exists() && streetwiseIdeas.popupCompareUpdatedDescription.Element.Exists(), "Verify original and updated descriptions are present");
                string updatedDescription = streetwiseIdeas.popupCompareUpdatedDescription.Text.Trim();
                streetwiseIdeas.popupCloseButton.Click();

                streetwiseIdeas.ScrollToBottom();
                testIdea.Action.Element.SendKeys(OpenQA.Selenium.Keys.Home);
                testIdea.Action.Click();
                testIdea.Action.SelectListOptionByText("Import Changes");
                testIdea.Action.Element.SendKeys(OpenQA.Selenium.Keys.Enter);
                streetwiseIdeas.SubmitButton.Element.Hover();
                streetwiseIdeas.SubmitButton.Click();
                HpgAssert.True(streetwiseIdeas.SubmitButton.Element.Missing(),
                               "Submit changes on New Streetwise Ideas page");

                

                //Step 11. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 11 end
            }
        }

        [Test][Ignore]
        public void Test_TC8446()
        {
            //US8233: Packaging Idea Page - Attachment Editing
            //Create an idea, add attachments, publish, verify attachments, edit attachments, verify edit
            RallyTestID = "TC8446";

            string fileDir = Environment.CurrentDirectory + Constants.InputDataPath + @"global\Attachments\";

            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");
                DBUtility dbUtility = new DBUtility();

                //Step 1. Navigate to Idea Management Application
                //Expected Result: Homepage is displayed
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //step 1 end

                //Step 2. Login as Admin/SME
                //Expected Result: User is logged in
                page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                IMHome.loginIdeaManagement(BaseURL, Controller.fields["Domain"] + "\\" + Controller.fields["UserId"],
                                           Controller.fields["Password"]);
                IMHome.SelectRole(imHome.Role.Admin);
                //step 2 end

                //Step 3. Create and Submit an Idea and Submit
                //Expected Result: Idea is created with all information
                int newIdeaID = dbUtility.CreateSavedIdea("Automation TC8381 " + nameSuffix,
                                                          "Automation Description for TC8381-" + nameSuffix,
                                                          "",
                                                          "Automation Contact Name " + nameSuffix);
                dbUtility.AcceptIdea(newIdeaID);
                //step 3 end

                //Step 4. Navigate to Publish Idea page
                IMHome.GoToPackageIdea(newIdeaID);
                page_objects.im3PV publishIdea = new page_objects.im3PV(IMHome.browser);
                //step 4 end

                //Step 5. Add new attachments to idea and publish
                //Expected result: attachments are added to list
                publishIdea.ExpandAllButton.Click();
                publishIdea.EditAttachments.Click();
                HpgAssert.True(publishIdea.EditAttachmentsDialog.Element.Exists(), "Verify 'Edit Attachments' dialog is present");
                //Grab list of all files to test...
                Dictionary<string, string> filesToTest = GetGoodAttachments();
                
                //go thru each "good" file and test attachment
                publishIdea.AddAttachments(filesToTest);
                publishIdea.EditAttachmentsSave.Click();
                WriteReport("New values entered, saving...");
                System.Threading.Thread.Sleep(6000);
                publishIdea.SaveAllButton.Click();
                System.Threading.Thread.Sleep(3000);
                publishIdea.SaveAllButton.Click();
                System.Threading.Thread.Sleep(3000);
                dbUtility.PublishQualifiedIdea(newIdeaID);
                //step 5 end

                //Step 6. Navigate to Published Idea and verify Attachments
                //Expected result: Attachments are present and correct
                IMHome.GoToPublishedIdeaNumber(newIdeaID.ToString());
                imPublishedIdea publishedIdea = new imPublishedIdea(IMHome.browser);
                publishedIdea.Refresh();
                List<HpgElement> attachments = publishedIdea.GetAllAttachments();
                foreach (KeyValuePair<string, string> fileToTest in filesToTest)
                {
                    HpgAssert.True(attachments.Select(a => a.Text.Trim().Equals(fileToTest.Key)).Any(), "Verify attachment '" + fileToTest.Key + "' is present");
                    string expectedFilename = fileToTest.Value.Substring(fileToTest.Value.LastIndexOf(@"\")).Replace(@"\", "");
                    string actualFilename = (from a in attachments where a.Text.Equals(fileToTest.Key) select a.Element["href"]).First();
                    actualFilename = HttpUtility.UrlDecode(actualFilename);
                    HpgAssert.Contains(actualFilename, expectedFilename, "Verify attachment file is correct.");
                }
                //step 6 end

                //Step 7. Disable all attachments
                //Expected result: Attachments are no longer visible
                IMHome.GoToPackageIdea(newIdeaID);
                publishIdea.ExpandAllButton.Click();
                //Loop through each attachment on each view and uncheck box
                for (int view = 0; view < 3; view++)
                {
                    foreach (im3PV.IdeaLink attachment in publishIdea.GetIdeaAttachments(view))
                    {
                        attachment.Box.UnCheck();
                    }
                }
                WriteReport("All attachments disabled, saving...");
                System.Threading.Thread.Sleep(6000);
                publishIdea.SaveAllButton.Click();
                System.Threading.Thread.Sleep(3000);
                publishIdea.SaveAllButton.Click();
                System.Threading.Thread.Sleep(3000);
                dbUtility.PublishQualifiedIdea(newIdeaID);
                //step 7 end

                //Step 8. Verify no attachments are present
                //Expected result: No attachments visible on Published Idea Details page
                IMHome.GoToPublishedIdeaNumber(newIdeaID.ToString());
                publishedIdea.Refresh();
                for (int i = 20; i > 0; i--)
                {
                    if (!publishedIdea.GetAllAttachments().Any())
                    {
                        break;
                    }
                    WriteReport("Attachments still present, waiting and refreshing...");
                    System.Threading.Thread.Sleep(4000);
                    publishedIdea.Refresh();
                }
                HpgAssert.False(publishedIdea.GetAllAttachments().Any(), "Verify no attachments are present");
                //step 8 end

                //Step 9. Delete all attachments
                //Expected result: No attachments are listed
                IMHome.GoToPackageIdea(newIdeaID);
                publishIdea.ExpandAllButton.Click();
                publishIdea.EditAttachments.Click();
                HpgAssert.True(publishIdea.EditAttachmentsDialog.Element.Exists(), "Verify 'Edit Attachments' dialog is present");
                while (publishIdea.EditAttachmentsDialogGetDeleteLinks().Any())
                {
                    publishIdea.EditAttachmentsDialogGetDeleteLinks().Last().Click();
                }
                publishIdea.EditAttachmentsSave.Click();
                WriteReport("All attachments deleted, saving...");
                System.Threading.Thread.Sleep(6000);
                publishIdea.SaveAllButton.Click();
                System.Threading.Thread.Sleep(3000);
                publishIdea.SaveAllButton.Click();
                System.Threading.Thread.Sleep(3000);
                dbUtility.PublishQualifiedIdea(newIdeaID);
                //step 9 end

                //Step 10. Verify no attachments are present
                //Expected result: No attachments are visible on Published Idea Details page
                IMHome.GoToPublishedIdeaNumber(newIdeaID.ToString());
                publishedIdea.Refresh();
                if (publishedIdea.Attachments.Element.Exists())
                {
                    HpgAssert.False(publishedIdea.GetAllAttachments().Any(), "Verify no attachments are present");
                }
                else
                {
                    HpgAssert.True(publishedIdea.Attachments.Element.Missing(), "Verify no attachments are present");
                }
                //step 10 end

                //Step 11. Verify invalid file types are not allowed to attach
                //Expected Result: Files are rejected on Attach Dialog
                IMHome.GoToPackageIdea(newIdeaID);
                publishIdea.ExpandAllButton.Click();
                publishIdea.EditAttachments.Click();
                HpgAssert.True(publishIdea.EditAttachmentsDialog.Element.Exists(), "Verify 'Edit Attachments' dialog is present");
                //Grab list of all files to test...
                filesToTest.Clear();
                filesToTest = (from f in System.IO.Directory.GetFiles(fileDir + @"Bad\")
                                                          select
                                                              new
                                                              {
                                                                  k =
                                                          nameSuffix + "-" +
                                                          f.Substring(f.LastIndexOf(@"\")).Replace(@"\", ""),
                                                                  v = f
                                                              }).ToDictionary(p => p.k, p => p.v);
                foreach (KeyValuePair<string, string> fileToTest in filesToTest)
                {
                    //go thru each "bad" file and test attachment
                    publishIdea.EditAttachmentsAddNew.Click();
                    publishIdea.EditAttachmentsDialogGetDescriptions().Last().Type(fileToTest.Key);
                    publishIdea.EditAttachmentsDialogGetFileInputs().Last().Element.SendKeys(fileToTest.Value);
                }
                publishIdea.EditAttachmentsSave.Click();
                HpgAssert.True(publishIdea.EditAttachmentsDialog.Element.Exists(), "Verify invalid attachments creates error on dialog");
                foreach (HpgElement attachment in publishIdea.EditAttachmentsDialogGetDescriptions())
                {
                    HpgAssert.Contains(attachment.Element.FindXPath(@"../..")["class"].ToLower(), "error", "Verify invalid attachment has error");
                }
                publishIdea.EditAttachmentsCancel.Click();
                //step 11 end

                //Clean-up
                //Delete idea completely
                dbUtility.DeleteIdeaByIdeaNumber(newIdeaID);
                //clean-up end

                //Step 12. Verify the same in different browsers: IE, Firefox, Chrome and Safari.
                //Expected Result: Above behavior and functionality should remain the same.
                // -- Achieved via foreach browser loop
                //step 12 end
            }
        }



        

        [Test][Ignore]
        public void     x_sandbox()
        {
            DBUtility dbUtility = new DBUtility();
            //dbUtility.dbDatabase = "StreetwiseDeploy";

            foreach (KeyValuePair<string, Browser> testBrowser in ffChromeOnly)
            {
                #region setup
                WriteInfoReport("Setup");
                string saveFile = "TC7203ExcelExport.xls";
                WriteReport("\n~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");
                AutomationCore.utility.ews ews = new AutomationCore.utility.ews();
                ews.FilterContains.Clear();
                ews.FilterContains.Add(new KeyValuePair<string, string>("from", "HulkHoganStreetwise@gmail.com"));
                ews.FilterContains.Add(new KeyValuePair<string, string>("subject", nameSuffix));
                ews.FilterGreaterThan.Clear();
                ews.FilterGreaterThan.Add(new KeyValuePair<string, object>("sent", DateTime.Now));
                ews.UserDomain = "HCA";
                WriteInfoReport("Setup complete.");
                #endregion


                //page_objects.imHome tHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(testBrowser.Value));
                

                #region submitIdea
                WriteInfoReport("Submit as Standard");
                //page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(TestUserDomain + "\\" + StandardUser, "T3st5678", testBrowser.Value));
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(TestUserDomain + "\\" + StandardUser, testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //IMHome.loginIdeaManagement(BaseURL, TestUserDomain + "\\" + StandardUser);
                SessConfiguration.AppHost = BaseURL;
                IMHome.GotoDashboard();
                IMHome.SelectRole(imHome.Role.Standard);
                IMHome.GotoSubmitAnIdea();
                page_objects.imSubmitAnIdea submitIdea = new imSubmitAnIdea(IMHome.browser);
                submitIdea.IdeaName.Type("Test Idea - " + nameSuffix);
                submitIdea.IdeaDescription.Type("This is the description \r\n" + DateTime.Now.ToLongDateString());
                submitIdea.FillOutQuestions(" browser: " + testBrowser.Key);
                //adding attachments...
                submitIdea.AddAttachmentsButton.Click();
                imSubmitAnIdea.AddAttachmentsDialog attachmentsDialog = new imSubmitAnIdea.AddAttachmentsDialog(submitIdea.AttachmentsDialog);
                Dictionary<string, string> goodFiles = GetGoodAttachments();
                imSubmitAnIdea.AddAttachmentsDialog.AttachmentRow attachmentRow = attachmentsDialog.GetAttachmentRow(1);
                attachmentRow.Title.Type("Submit attachment");
                attachmentRow.LocalFile.Element.SendKeys(goodFiles.First().Value);
                attachmentsDialog.SaveButton.Click();
                //adding links...
                submitIdea.AddLinksButton.Click();
                imSubmitAnIdea.AddLinksDialog linksDialog = new imSubmitAnIdea.AddLinksDialog(submitIdea.LinksDialog);
                imSubmitAnIdea.AddLinksDialog.LinkRow linkRow = linksDialog.GetLinkRow(1);
                linkRow.Title.Type(linksList.First().Key);
                linkRow.Url.Type(linksList.First().Value);
                linksDialog.SaveButton.Click();
                //Submit idea
                submitIdea.SubmitIdea();
                WriteInfoReport("Submit as Standard complete.");
                #endregion

                #region SearchMyIdeas
                //Search for submitted idea in MyIdeas
                WriteInfoReport("Search for Submitted Idea");
                IMHome.GotoMyIdeas();
                page_objects.imMyIdeas myIdeas = new imMyIdeas(IMHome.browser);
                myIdeas.SearchFor(nameSuffix);

                List<imIdeaListMaster.Refinement> refinements = myIdeas.GetAllRefinements();
                HpgAssert.AreEqual(nameSuffix, refinements.First(r => r.Type.ToLower().Equals("search")).Value, "Verify search has been performed");

                List<imAllIdeas.AllIdea> displayedIdeas = myIdeas.GetAllIdeas();
                imAllIdeas.AllIdea newIdea = displayedIdeas.First(i => i.Title.Text.Contains(nameSuffix));
                int newIdeaNumber = newIdea.IdeaId;
                WriteInfoReport("Search for Submitted Idea complete.");
                #endregion

                #region EditInDCRD
                WriteInfoReport("Edit as DCRD");
                IMHome.browser.Dispose();
                IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(TestUserDomain + "\\" + DCRDUser, testBrowser.Value));
                CurrentBrowser = IMHome.browser;               
                //IMHome.loginIdeaManagement(BaseURL, TestUserDomain + "\\" + DCRDUser);
                IMHome.GotoDashboard();
                IMHome.SelectRole(imHome.Role.Boss);
                IMHome.GotoDashboard();
                Dashboard dashboard = new Dashboard(IMHome.browser);
                Dashboard.QueueIdea dcrdIdea = dashboard.GetQueueIdeas().First(i => i.IdeaId.Equals(newIdeaNumber));
                dcrdIdea.Click();
                //dcrdIdea.IdeaName.Hover();
                //dcrdIdea.IdeaName.Click();
                //if (dashboard.pageHeader.Text.Equals("My Dashboard"))
                //    dcrdIdea.IdeaName.Element.SendKeys(OpenQA.Selenium.Keys.Enter);
                DCRD dcrd = new DCRD(dashboard.browser);
                System.Threading.Thread.Sleep(2000);
                dcrd.SubmitButton.Click();
                System.Threading.Thread.Sleep(2000);
                dcrd.SMEDropDown.SelectListOptionByText("Auto Test");
                dcrd.AssignToSMESubmit.Click();
                WriteInfoReport("Edit as DCRD complete.");
                #endregion

                #region EditasSME
                WriteInfoReport("Edit as SME");
                IMHome.browser.Dispose();
                IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(TestUserDomain + "\\" + SMEUser, testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //IMHome.loginIdeaManagement(BaseURL, TestUserDomain + "\\" + SMEUser);
                IMHome.GotoDashboard();
                IMHome.SelectRole(imHome.Role.SME);
                dashboard = new Dashboard(IMHome.browser);
                Dashboard.QueueIdea smeIdea = dashboard.GetQueueIdeas().First(i => i.IdeaId.Equals(newIdeaNumber));
                smeIdea.Click();
                im3PV tPv = new im3PV(dashboard.browser);
                tPv.WaitForThrobber();
                tPv.UnlockEditIdea();
                tPv.ExpandAllButton.Click();
                Dictionary<string, im3PV.IdeaValues> ideaValues = new Dictionary<string, im3PV.IdeaValues>();
                ideaValues.Add("HCA", tPv.GetIdeaValues(0));
                ideaValues.Add("GPO", tPv.GetIdeaValues(1));
                ideaValues.Add("NONGPO", tPv.GetIdeaValues(2));
                ideaValues["HCA"].Description.Type(tPv.IdeaDescription.Text.Trim() + "\r\nEdited As SME");
                tPv.CopyDescription1.Click();
                tPv.CopyDescription2.Click();
                ideaValues["HCA"].Action.SelectListOptionByText("Not Applicable");
                System.Threading.Thread.Sleep(1000);
                ideaValues["GPO"].Action.SelectListOptionByText("Ready to Publish");
                System.Threading.Thread.Sleep(1000);
                ideaValues["NONGPO"].Action.SelectListOptionByText("Ready to Publish");
                System.Threading.Thread.Sleep(1000);
                //Add attachments on 3PV
                tPv.EditAttachments.Click();
                HpgAssert.True(tPv.EditAttachmentsDialog.Element.Exists(), "Verify 'Edit Attachments' dialog is present");
                tPv.AddAttachments(goodFiles.Skip(1).ToDictionary(p => p.Key, p => p.Value));
                tPv.EditAttachmentsSave.Click();
                System.Threading.Thread.Sleep(15000);  //Waiting for attachments to upload and attach
                //Add Links on 3PV
                tPv.EditLinks.Click();
                HpgAssert.True(tPv.EditLinkDialog.Element.Exists(), "Verify 'Edit Links' dialog is present");
                tPv.AddLinks(linksList.Skip(1).ToDictionary(p => p.Key, p => p.Value));
                tPv.EditLinkSave.Click();
                //Save 3PV
                tPv.ClickCommit();
                WriteInfoReport("Edit as SME complete.");
                #endregion

                #region EditAsAdmin
                WriteInfoReport("Edit as Admin");
                IMHome.browser.Dispose();
                IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(TestUserDomain + "\\" + AdminUser, testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //IMHome.loginIdeaManagement(BaseURL, TestUserDomain + "\\" + AdminUser);
                IMHome.GotoDashboard();
                IMHome.SelectRole(imHome.Role.Admin);
                System.Threading.Thread.Sleep(5000);
                dashboard = new Dashboard(IMHome.browser);
                Dashboard.QueueIdea adminIdea = dashboard.GetQueueIdeas().First(i => i.IdeaId.Equals(newIdeaNumber));
                adminIdea.IdeaName.Click();
                tPv = new im3PV(dashboard.browser);
                tPv.WaitForThrobber();
                tPv.UnlockEditIdea();
                tPv.ExpandAllButton.Click();
                ideaValues.Clear();
                ideaValues.Add("HCA", tPv.GetIdeaValues(0));
                ideaValues.Add("GPO", tPv.GetIdeaValues(1));
                ideaValues.Add("NONGPO", tPv.GetIdeaValues(2));
                foreach (KeyValuePair<string, im3PV.IdeaValues> ideaValue in ideaValues)
                {
                    ideaValue.Value.Description.Element.Click();
                    ideaValue.Value.Description.Element.SendKeys(OpenQA.Selenium.Keys.End);
                    ideaValue.Value.Description.Element.SendKeys("\r\n" + ideaValue.Key);
                    ideaValue.Value.Description.Element.SendKeys("\r\nEdited by Admin");
                    ideaValue.Value.ReportingType.SelectLastOption();
                    ideaValue.Value.ResultType.SelectFirstOption();
                }
                ideaValues["GPO"].Action.SelectListOptionByText("Retire");
                System.Threading.Thread.Sleep(1000);
                ideaValues["NONGPO"].Action.SelectListOptionByText("Publish");
                System.Threading.Thread.Sleep(1000);
                tPv.DisplaySoCDialog(2);
                im3PV.SoCDialog socDialog = new im3PV.SoCDialog(tPv.socDialog);
                socDialog.ApplySoC("HealthTrust/ABQ Health Partners/ABQ Health Partners/ABQ Health Partners/ABQ Health Partners/ABQ Health Partners (J2900)");
                socDialog.SaveButton.Click();
                tPv.ClickCommit();
                WriteInfoReport("Edit as Admin complete.");
                #endregion

                #region SearchAllIdeas
                WriteInfoReport("Search All Ideas");
                IMHome.GotoAllIdeas();
                page_objects.imAllIdeas allIdeas = new imAllIdeas(IMHome.browser);
                allIdeas.SearchFor(nameSuffix);
                System.Threading.Thread.Sleep(2000);

                refinements = allIdeas.GetAllRefinements();
                HpgAssert.AreEqual(nameSuffix, refinements.First(r => r.Type.ToLower().Equals("search")).Value, "Verify search has been performed");

                displayedIdeas = allIdeas.GetAllIdeas();

                newIdea = displayedIdeas.First(i => i.Title.Text.Contains(nameSuffix));
                HpgAssert.AreEqual("N R P", newIdea.Status, "Verify Status is correct on the card view");
                HpgAssert.AreEqual("P", newIdea.Level, "Verify level is correct on the card view");
                WriteInfoReport("Search All Ideas complete.");
                #endregion

                #region SearchPublishedIdeas
                WriteInfoReport("Search Published Ideas");
                IMHome.browser.Dispose();
                IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(TestUserDomain + "\\" + ABQUser, testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //IMHome.loginIdeaManagement(BaseURL, TestUserDomain + "\\" + ABQUser);
                IMHome.GotoDashboard();
                IMHome.SelectRole(imHome.Role.ReadOnly);
                System.Threading.Thread.Sleep(5000);
                IMHome.GotoPublishedIdeas();
                page_objects.imPublishedIdeas publishedIdeas = new imPublishedIdeas(IMHome.browser);
                publishedIdeas.SearchFor(nameSuffix);
                
                List<imPublishedIdeas.PublishedIdea> publishedIdeaList = publishedIdeas.GetPublishedIdeas();
                imPublishedIdeas.PublishedIdea newPublishedIdea = publishedIdeaList.First(i => i.IdeaNumber.Equals(newIdeaNumber));
                HpgAssert.Contains(newPublishedIdea.IdeaName.Text, nameSuffix, "Verify new idea is listed in Published Ideas list.");
                WriteInfoReport("Search Published Ideas complete.");
                #endregion

                #region ValidatePublishedIdea
                WriteInfoReport("Validate Published Idea Details");
                newPublishedIdea.IdeaName.Click();

                page_objects.imPublishedIdea publishedIdea = new imPublishedIdea(publishedIdeas.browser);
                HpgAssert.Contains(publishedIdea.IdeaTitle.Text, nameSuffix, "Verify published idea title");
                HpgAssert.AreEqual(newIdeaNumber.ToString(), publishedIdea.IdeaNumber.Text.Trim(), "Verify published idea number");
                List<HpgElement> attachments = publishedIdea.GetAllAttachments();
                HpgElement attachment = attachments.First(a => a.Text.Trim().ToLower().Equals("submit attachment"));
                HpgAssert.True(attachment.Element.Exists(), "Verify submitted attachment is present");
                HpgAssert.Contains(HttpUtility.UrlDecode(attachment.Element["href"]), goodFiles.First().Key, "Verify submitted attachment link");
                publishedIdea.VerifyAttachmentsArePresent(goodFiles.Skip(1).ToDictionary(a=>a.Key, a=>a.Value));
                publishedIdea.VerifyLinksArePresent(linksList);
                WriteInfoReport("Validate Published Idea Details complete.");
                #endregion

                #region VerifyEmail
                WriteInfoReport("Validate Published Idea Email function");
                publishedIdea.LinkEmail.Click();
                page_objects.imPublishedIdeaEmail ideaEmail = new imPublishedIdeaEmail(publishedIdea.browser);
                ideaEmail.ToField.Type("HPG.automation@HCAHealthcare.com");
                ideaEmail.MessageField.Type(nameSuffix + " BROWSER: " + testBrowser.Key);
                ideaEmail.SendButton.Click();
                DataTable msg = ews.GetMessagesDTWait(true, 2);
                if (msg.Rows.Count == 0)
                    msg = ews.GetMessagesDTWait(true, 2);
                HpgAssert.AreEqual("1", msg.Rows.Count.ToString(), "Verify single email was received");
                ews.DeleteMessage(msg.Rows[0]["ID"].ToString());
                WriteInfoReport("Validate Published Idea Email function complete.");
                #endregion

                #region VerifyExcel
                WriteInfoReport("Validate Published Idea Excel function");
                publishedIdea.SaveExcel(Constants.CurrentDirectory + Constants.InputDataPath + saveFile);
                HpgAssert.AreEqual("", publishedIdea.CompareExcelFileToDetailsPage(saveFile));
                WriteInfoReport("Validate Published Idea Excel function complete.");
                #endregion

                #region VerifyPDF
                WriteInfoReport("Validate Published Idea PDF function");
                saveFile = Constants.CurrentDirectory + Constants.InputDataPath + "TC7265PDFExport.pdf";
                publishedIdea.SavePDF(saveFile);
                HpgAssert.True(System.IO.File.Exists(saveFile), "Verify PDF file was downloaded");
                WriteInfoReport("Validate Published Idea PDF function complete.");
                #endregion


            }

        }

        [Test]
        public void x_adminTests()
        {
            DBUtility dbUtility = new DBUtility();
            //dbUtility.dbDatabase = "StreetwiseDeploy";

            #region staticData
            EditResultTypes.ResultType newResult = new EditResultTypes.ResultType()
                {
                    AppliesTo = EditResultTypes.AppliesTo.Both,
                    CaptureBaseline = true,
                    DenominatorName = DateTime.Now.ToString("MMMM"),
                    ExpressAs = EditResultTypes.ExpressAs.Rate,
                    IncreasingProgress = true,
                    Name = "New Result " + DateTime.Now.ToString("yyyyMMddhhmmss"),
                    NumeratorName = DateTime.Now.ToString("dddd"),
                    ReportingPeriod = EditResultTypes.ReportingPeriod.Yearly,
                    Summable = true,
                    UoM = "Cars"
                };
            EditResultTypes.ResultType changeResult = new EditResultTypes.ResultType()
                {
                    AppliesTo = EditResultTypes.AppliesTo.FacilityIdeas,
                    CaptureBaseline = false,
                    DenominatorName = DateTime.Now.ToString("dddd"),
                    ExpressAs = EditResultTypes.ExpressAs.RatePerUnit,
                    IncreasingProgress = false,
                    Name = "Change Result " + DateTime.Now.ToString("yyyyMMddhhmmss"),
                    NumeratorName = DateTime.Now.ToString("MMMM"),
                    ReportingPeriod = EditResultTypes.ReportingPeriod.Quarterly,
                    Summable = false,
                    UoM = "Baloons"
                };

            EditQuestions.Question newQuestion = new EditQuestions.Question()
                {
                    Name = "New Question " + DateTime.Now.ToString("yyyyMMddhhmmss"),
                    Required = false
                };
            EditQuestions.Question changeQuestion = new EditQuestions.Question()
                {
                    Name = "Update Question" + DateTime.Now.ToString("yyyyMMddhhmmss"),
                    Required = true
                };
#endregion

            foreach (KeyValuePair<string, Browser> testBrowser in ffChromeOnly)
            {
                #region setup
                WriteReport("\n~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");
                //page_objects.imHome IMHome = new page_objects.imHome((BrowserSession) BaseTest.OpenNewBrowser(testBrowser.Value));
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(TestUserDomain + "\\" + AdminUser, testBrowser.Value));
                BaseTest.SessConfiguration.AppHost = BaseURL;
                CurrentBrowser = IMHome.browser;
                #endregion

                #region login
                //IMHome.loginIdeaManagement(BaseURL, TestUserDomain + "\\" + AdminUser);
                IMHome.GotoDashboard();
                IMHome.SelectRole(imHome.Role.Admin);
                #endregion

                #region editCategories
                //IMHome.GotoCategories();
                #endregion

                #region editDepartments
                #endregion

                #region editQuestions
                if (false)
                {
                    WriteReport("----- Edit Questions -----");
                    IMHome.GotoQuestions();
                    EditQuestions editQuestions = new EditQuestions(IMHome.browser);
                    //Create new Question
                    editQuestions.SubmitNewQuestion(newQuestion);
                    List<EditQuestions.DisplayQuestion> displayedQuestions = editQuestions.GetQuestions();
                    EditQuestions.DisplayQuestion submittedQuestion = displayedQuestions.First(q => q.Name.Equals(newQuestion.Name));
                    HpgAssert.AreEqual("", editQuestions.CompareQuestion(submittedQuestion, newQuestion), "Verify Question data matches what was submitted");
                    //Edit newly created Question
                    submittedQuestion.EditButton.Click();
                    EditQuestions.EditForm editQuestion = new EditQuestions.EditForm(editQuestions.browser);
                    editQuestion.EditQuestion(changeQuestion);
                    System.Threading.Thread.Sleep(1000);
                    displayedQuestions = editQuestions.GetQuestions();
                    submittedQuestion = displayedQuestions.First(q => q.Name.Equals(changeQuestion.Name));
                    HpgAssert.AreEqual("", editQuestions.CompareQuestion(submittedQuestion, changeQuestion), "Verify Question data was changed correctly");
                    //Delete newly changed Question
                    submittedQuestion.DeleteButton.Click();
                    editQuestions.ConfirmDeleteDelete.Click();
                    editQuestions.Refresh();
                    displayedQuestions = editQuestions.GetQuestions(false);
                    HpgAssert.False(displayedQuestions.FindAll(q => q.Name.Equals(changeQuestion.Name)).Any(), "Verify Question no longer shows");
                    //Undo delete on newly changed Question
                    displayedQuestions = editQuestions.GetQuestions();
                    submittedQuestion = displayedQuestions.First(q => q.Name.Equals(changeQuestion.Name));
                    submittedQuestion.UndoDelete.Click();
                    editQuestions.ConfirmUndoDeleteUndo.Click();
                    editQuestions.Refresh();
                    displayedQuestions = editQuestions.GetQuestions(false);
                    HpgAssert.True(displayedQuestions.FindAll(q => q.Name.Equals(changeQuestion.Name)).Any(), "Verify undo-delted Question shows again");
                    //Verify requirement of newly created Question
                    IMHome.GotoSubmitAnIdea();
                    imSubmitAnIdea submitAnIdea = new imSubmitAnIdea(IMHome.browser);
                    HpgAssert.True(submitAnIdea.GetQuestion(changeQuestion.Name).IsRequired, "Verify newly created question is required on submit page");
                    //Cleanup
                    if (dbUtility.DeleteIdeaQuestion(changeQuestion.Name) != 1) WriteReport("\n** WARNING: Question not deleted from database! **");
                }
                #endregion

                #region editResultTypes
                if (false)
                {
                    WriteReport("----- Edit Result Types -----");
                    IMHome.GotoResultTypes();
                    EditResultTypes editResultTypes = new EditResultTypes(IMHome.browser);
                    //Create new Result type
                    editResultTypes.SubmitNewResultType(newResult);
                    List<EditResultTypes.DisplayResultType> displayedResultTypes = editResultTypes.GetDisplayedResultTypes();
                    EditResultTypes.DisplayResultType submittedResult = displayedResultTypes.First(r => r.Name.Equals(newResult.Name));
                    HpgAssert.AreEqual("", editResultTypes.CompareResultTypes(newResult, submittedResult), "Verify Result Type data matches what was submitted");
                    //Edit newly created Result Type
                    submittedResult.Edit.Click();
                    EditResultTypes.EditForm editResult = new EditResultTypes.EditForm(editResultTypes.browser);
                    editResult.EditResult(changeResult);
                    System.Threading.Thread.Sleep(1000);
                    displayedResultTypes = editResultTypes.GetDisplayedResultTypes();
                    submittedResult = displayedResultTypes.First(r => r.Name.Equals(changeResult.Name));
                    HpgAssert.AreEqual("", editResultTypes.CompareResultTypes(changeResult, submittedResult), "Verify Result Type data was changed correctly");
                    //Delete newly changed Result Type
                    submittedResult.Delete.Click();
                    editResultTypes.ConfirmDeleteDelete.Click();
                    editResultTypes.Refresh();
                    displayedResultTypes = editResultTypes.GetDisplayedResultTypes(false);
                    HpgAssert.False(displayedResultTypes.FindAll(r => r.Name.Equals(changeResult.Name)).Any(), "Verify result no longer shows");
                    //Undo delete on newly changed Result Type
                    displayedResultTypes = editResultTypes.GetDisplayedResultTypes();
                    submittedResult = displayedResultTypes.First(r => r.Name.Equals(changeResult.Name));
                    submittedResult.UndoDelete.Click();
                    editResultTypes.ConfirmUndoDeleteUndo.Click();
                    editResultTypes.Refresh();
                    displayedResultTypes = editResultTypes.GetDisplayedResultTypes(false);
                    HpgAssert.True(displayedResultTypes.FindAll(r => r.Name.Equals(changeResult.Name)).Any(), "Verify un-deleted result shows again");
                    //Cleanup
                    //HpgAssert.AreEqual("1", dbUtility.DeleteResultType(changeResult.Name).ToString(), "Delete the test Result Type");
                    if (!dbUtility.DeleteResultType(changeResult.Name).ToString().Equals("1")) WriteReport("\n** WARNING: Result type not deleted from database! **");
                }
                #endregion

                #region editStreetwise
                if (true)
                {
                    WriteReport("----- Edit Streetwise -----");
                    IMHome.GotoNewStreetwiseIdeas();
                    imNewStreetwiseIdeas streetwiseIdeas = new imNewStreetwiseIdeas(IMHome.browser);

                    #region RejectNewIdea
                    WriteReport("Reject New Idea");
                    streetwiseIdeas.FilterNewIdeas();
                    List<imNewStreetwiseIdeas.NewIdea> ideaList = streetwiseIdeas.GetAllIdeas(10);
                    HpgAssert.True(ideaList.Any(), "Verify atleast one idea is loaded on page");
                    imNewStreetwiseIdeas.NewIdea testIdea = ideaList.First(i => i.CurrentAction.Equals("pending"));
                    string rejectIdeaId = testIdea.IdeaID.Trim();
                    WriteReport("Rejecting idea " + rejectIdeaId + "...");
                    streetwiseIdeas.ScrollToBottom();
                    testIdea.Action.Element.SendKeys(OpenQA.Selenium.Keys.Home);
                    testIdea.Action.Click();
                    testIdea.Action.SelectListOptionByText("Exclude");
                    testIdea.Action.Element.SendKeys(OpenQA.Selenium.Keys.Enter);
                    streetwiseIdeas.Submit();
                    IMHome.GotoNewStreetwiseIdeas();
                    IMHome.Refresh();
                    streetwiseIdeas.FilterNewIdeas();
                    streetwiseIdeas.SearchFor(rejectIdeaId);
                    ideaList = streetwiseIdeas.GetAllIdeas(true);
                    HpgAssert.False(ideaList.Any(i => i.IdeaID.Trim().Equals(rejectIdeaId)), "Verify the rejected idea does not show up on New Streetwise Ideas page");
                    streetwiseIdeas.FilterAcceptedIdeas();
                    streetwiseIdeas.SearchFor(rejectIdeaId);
                    ideaList = streetwiseIdeas.GetAllIdeas(true);
                    HpgAssert.False(ideaList.Any(i => i.IdeaID.Trim().Equals(rejectIdeaId)), "Verify the rejected idea does not show up on Accepted Streetwise Ideas page");
                    streetwiseIdeas.FilterRejectedIdeas();
                    streetwiseIdeas.SearchFor(rejectIdeaId);
                    ideaList = streetwiseIdeas.GetAllIdeas(true);
                    testIdea = ideaList.First(i => i.IdeaID.Trim().Equals(rejectIdeaId));
                    HpgAssert.AreEqual(rejectIdeaId, testIdea.IdeaID, "Verify the rejected idea shows up on Rejected Streetwise Ideas page");
                    WriteReport("Reject New Idea - Idea (" + rejectIdeaId + ") is rejected");
                    #endregion

                    #region ImportNewIdea
                    WriteInfoReport("Approving idea " + rejectIdeaId + "...");
                    streetwiseIdeas.ScrollToBottom();
                    testIdea.Action.Element.SendKeys(OpenQA.Selenium.Keys.Home);
                    testIdea.Action.Click();
                    testIdea.Action.SelectListOptionByText("Import");
                    streetwiseIdeas.Submit();
                    IMHome.GotoNewStreetwiseIdeas();
                    IMHome.Refresh();
                    streetwiseIdeas.FilterNewIdeas();
                    streetwiseIdeas.SearchFor(rejectIdeaId);
                    ideaList = streetwiseIdeas.GetAllIdeas(true);
                    HpgAssert.False(ideaList.Any(i => i.IdeaID.Trim().Equals(rejectIdeaId)), "Verify the Accepted idea does not show up on New Streetwise Ideas page");
                    streetwiseIdeas.FilterRejectedIdeas();
                    streetwiseIdeas.SearchFor(rejectIdeaId);
                    HpgAssert.True(streetwiseIdeas.browser.HasContent("Search: " + rejectIdeaId));
                    ideaList = streetwiseIdeas.GetAllIdeas(true);
                    HpgAssert.False(ideaList.Any(i => i.IdeaID.Trim().Equals(rejectIdeaId)), "Verify the Accepted idea does not show up on Rejected Streetwise Ideas page");
                    streetwiseIdeas.FilterAcceptedIdeas();
                    streetwiseIdeas.SearchFor(rejectIdeaId);
                    ideaList = streetwiseIdeas.GetAllIdeas(true);
                    testIdea = ideaList.First(i => i.IdeaID.Trim().Equals(rejectIdeaId));
                    HpgAssert.AreEqual(rejectIdeaId, testIdea.IdeaID, "Verify the Accepted idea shows up on Accepted Streetwise Ideas page");
                    WriteInfoReport("Approving idea " + rejectIdeaId + "...");                    
                    testIdea.IdeaName.Click();
                    string importTitle = streetwiseIdeas.popupPublishedTitle.Text.Trim();
                    string importDescription = streetwiseIdeas.popupPublishedDescription.Text.Trim();
                    #endregion

                    #region PublishImportedIdea
                    #region EditasSME
                    WriteInfoReport("Edit as SME");
                    IMHome.browser.Dispose();
                    IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(TestUserDomain + "\\" + SMEUser, testBrowser.Value));
                    CurrentBrowser = IMHome.browser;
                    //IMHome.loginIdeaManagement(BaseURL, TestUserDomain + "\\" + SMEUser);
                    IMHome.GotoDashboard();
                    IMHome.SelectRole(imHome.Role.SME);
                    IMHome.GotoAllIdeas();
                    imAllIdeas allIdeas = new imAllIdeas(IMHome.browser);
                    allIdeas.GetFilter("Streetwise").Check();
                    allIdeas.SearchFor(rejectIdeaId);
                    allIdeas.GetAllIdeas().First(i => i.IdeaId.ToString().Equals(rejectIdeaId)).Title.Element.SendKeys(OpenQA.Selenium.Keys.Enter); //Click on the imported idea to go to it's 3PV
                    im3PV tPv = new im3PV(allIdeas.browser);
                    System.Threading.Thread.Sleep(5000);
                    tPv.EditButton.Click();
                    tPv.ExpandAllButton.Click();
                    System.Threading.Thread.Sleep(5000);
                    Dictionary<string, im3PV.IdeaValues> ideaValues = new Dictionary<string, im3PV.IdeaValues>();
                    ideaValues.Add("HCA", tPv.GetIdeaValues(0));
                    ideaValues.Add("GPO", tPv.GetIdeaValues(1));
                    ideaValues.Add("NONGPO", tPv.GetIdeaValues(2));
                    //ideaValues["HCA"].Description.Element.Click();
                    //ideaValues["HCA"].Description.Element.SendKeys(OpenQA.Selenium.Keys.End);
                    //ideaValues["HCA"].Description.Element.SendKeys("\r\nEdited by SME");
                    ideaValues["GPO"].Title.Element.Click();
                    ideaValues["GPO"].Title.Element.SendKeys(OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.End);
                    ideaValues["GPO"].Title.Element.SendKeys(" - GPO");
                    ideaValues["NONGPO"].Title.Element.Click();
                    ideaValues["NONGPO"].Title.Element.SendKeys(OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.End);
                    ideaValues["NONGPO"].Title.Element.SendKeys(" - NONGPO");
                    tPv.CopyDescription1.Click();
                    tPv.CopyDescription2.Click();
                    ideaValues["HCA"].Action.SelectListOptionByText("Ready to Publish");
                    System.Threading.Thread.Sleep(1000);
                    ideaValues["GPO"].Action.SelectListOptionByText("Ready to Publish");
                    System.Threading.Thread.Sleep(1000);
                    ideaValues["NONGPO"].Action.SelectListOptionByText("Ready to Publish");
                    System.Threading.Thread.Sleep(1000);
                    //Add attachments on 3PV
                    //tPv.EditAttachments.Click();
                    //HpgAssert.True(tPv.EditAttachmentsDialog.Element.Exists(), "Verify 'Edit Attachments' dialog is present");
                    //tPv.AddAttachments(GetGoodAttachments());
                    //tPv.EditAttachmentsSave.Click();
                    //System.Threading.Thread.Sleep(15000);  //Waiting for attachments to upload and attach
                    //Add Links on 3PV
                    //tPv.EditLinks.Click();
                    //HpgAssert.True(tPv.EditLinkDialog.Element.Exists(), "Verify 'Edit Links' dialog is present");
                    //tPv.AddLinks(linksList.Skip(1).ToDictionary(p => p.Key, p => p.Value));
                    //tPv.EditLinkSave.Click();
                    //Save 3PV
                    tPv.ClickCommit();
                    WriteInfoReport("Edit as SME complete.");
                    #endregion
                    #region EditAsAdmin
                    WriteInfoReport("Edit as Admin");
                    IMHome.browser.Dispose();
                    IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(TestUserDomain + "\\" + AdminUser, testBrowser.Value));
                    CurrentBrowser = IMHome.browser;
                    //IMHome.loginIdeaManagement(BaseURL, TestUserDomain + "\\" + AdminUser);
                    IMHome.GotoDashboard();
                    IMHome.SelectRole(imHome.Role.Admin);
                    System.Threading.Thread.Sleep(5000);
                    Dashboard dashboard = new Dashboard(IMHome.browser);
                    Dashboard.QueueIdea adminIdea = dashboard.GetQueueIdeas().First(i => i.IdeaId.ToString().Equals(rejectIdeaId));
                    adminIdea.IdeaName.Click();
                    tPv = new im3PV(dashboard.browser);
                    tPv.EditButton.Click();
                    tPv.ExpandAllButton.Click();
                    System.Threading.Thread.Sleep(5000);
                    ideaValues.Clear();
                    ideaValues.Add("HCA", tPv.GetIdeaValues(0));
                    ideaValues.Add("GPO", tPv.GetIdeaValues(1));
                    ideaValues.Add("NONGPO", tPv.GetIdeaValues(2));
                    //foreach (KeyValuePair<string, im3PV.IdeaValues> ideaValue in ideaValues)
                    //{
                    //    ideaValue.Value.Description.Element.Click();
                    //    ideaValue.Value.Description.Element.SendKeys(OpenQA.Selenium.Keys.End);
                    //    ideaValue.Value.Description.Element.SendKeys("\r\n" + ideaValue.Key);
                    //    ideaValue.Value.Description.Element.SendKeys("\r\nEdited by Admin");
                    //}
                    ideaValues["HCA"].Action.SelectListOptionByText("Publish");
                    System.Threading.Thread.Sleep(1000);
                    ideaValues["GPO"].Action.SelectListOptionByText("Publish");
                    System.Threading.Thread.Sleep(1000);
                    ideaValues["NONGPO"].Action.SelectListOptionByText("Publish");
                    System.Threading.Thread.Sleep(1000);
                    tPv.DisplaySoCDialog(0); //Edit HCA SoC
                    im3PV.SoCDialog socDialog = new im3PV.SoCDialog(tPv.socDialog);
                    socDialog.ApplySoC("HCA", "C");
                    socDialog.SaveButton.Click();
                    //tPv.DisplaySoCDialog(1); //Edit GPO SoC
                    //socDialog = new im3PV.SoCDialog(tPv.socDialog);
                    //socDialog.ApplySoC("HealthTrust/Acadia Healthcare Company/Acadia Healthcare Company/Acadia Healthcare Company/Acadia Healthcare Company/Acadia Corporate, Chicago Office (J6539)");
                    //socDialog.SaveButton.Click();
                    //tPv.DisplaySoCDialog(2); //Edit Non-GPO SoC
                    //socDialog = new im3PV.SoCDialog(tPv.socDialog);
                    //socDialog.ApplySoC("HealthTrust/ABQ Health Partners/ABQ Health Partners/ABQ Health Partners/ABQ Health Partners/ABQ Health Partners (J2900)");
                    //socDialog.SaveButton.Click();
                    tPv.ClickCommit();
                    WriteInfoReport("Edit as Admin complete.");
                    #endregion
                    #endregion

                    #region VerifyNewIdeaIsPublished
                    WriteInfoReport("Search Published Ideas");
                    IMHome.browser.Dispose();
                    IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(TestUserDomain + "\\" + CentennialUser, testBrowser.Value));
                    CurrentBrowser = IMHome.browser;
                    BaseTest.SessConfiguration.AppHost = BaseURL;
                    IMHome.GotoDashboard();
                    //IMHome.loginIdeaManagement(BaseURL, TestUserDomain + "\\" + CentennialUser); //Login as HCA user
                    IMHome.SelectRole(imHome.Role.ReadOnly);
                    IMHome.GotoPublishedIdeas();
                    page_objects.imPublishedIdeas publishedIdeas = new imPublishedIdeas(IMHome.browser);
                    publishedIdeas.SearchFor(rejectIdeaId);
                    imPublishedIdeas.PublishedIdea newPublishedIdea = publishedIdeas.GetPublishedIdeas().First(i => i.IdeaNumber.ToString().Equals(rejectIdeaId));
                    HpgAssert.True(newPublishedIdea.IdeaName.Element.Exists(), "Verify new idea is listed in Published Ideas list.");
                    WriteInfoReport("Search Published Ideas complete.");
                    WriteInfoReport("Validate Published Idea Details");
                    newPublishedIdea.IdeaName.Click();
                    page_objects.imPublishedIdea publishedIdea = new imPublishedIdea(publishedIdeas.browser);
                    HpgAssert.AreEqual(rejectIdeaId, publishedIdea.IdeaNumber.Text.Trim(), "Verify published idea Number");
                    HpgAssert.AreEqual(importTitle.ToLower(), publishedIdea.IdeaTitle.Text.Trim().ToLower(), "Verify published idea Title");
                    HpgAssert.AreEqual(importDescription.ToLower(), publishedIdea.Description.Text.Trim().ToLower(), "Verify published idea Description");
                    WriteInfoReport("Validate Published Idea Details");
                    #endregion

                    #region UpdateNewIdeaAndVerify
                    WriteInfoReport("Update Imported Idea on Streetwise Table and verify changes after importing changes");
                    IMHome.browser.Dispose();
                    dbUtility.StreetwiseImportUpdateIdea(int.Parse(rejectIdeaId), nameSuffix); //Make changes to idea in database
                    IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(TestUserDomain + "\\" + AdminUser, testBrowser.Value));
                    CurrentBrowser = IMHome.browser;
                    //IMHome.loginIdeaManagement(BaseURL, TestUserDomain + "\\" + AdminUser);
                    BaseTest.SessConfiguration.AppHost = BaseURL;
                    IMHome.GotoDashboard();
                    IMHome.SelectRole(imHome.Role.Admin);
                    IMHome.GotoNewStreetwiseIdeas();
                    streetwiseIdeas = new imNewStreetwiseIdeas(IMHome.browser);
                    try
                    {
                        streetwiseIdeas.browser.AcceptModalDialog();
                    }
                    catch (Exception)
                    {
                    }
                    streetwiseIdeas.FilterUpdatedIdeas();
                    streetwiseIdeas.SearchFor(rejectIdeaId);
                    ideaList = streetwiseIdeas.GetAllIdeas(true);
                    testIdea = ideaList.First(i => i.IdeaID.Equals(rejectIdeaId));
                    streetwiseIdeas.ScrollToBottom();
                    testIdea.Action.Element.SendKeys(OpenQA.Selenium.Keys.Home);
                    testIdea.Action.Click();
                    testIdea.Action.SelectListOptionByText("Import Changes");
                    streetwiseIdeas.Submit();
                    IMHome.GoToPackageIdea(int.Parse(rejectIdeaId));
                    tPv = new im3PV(streetwiseIdeas.browser);
                    tPv.EditButton.Click();
                    tPv.ClickCommit();
                    IMHome.browser.Dispose();
                    System.Threading.Thread.Sleep(5000);
                    IMHome = new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(TestUserDomain + "\\" + CentennialUser, testBrowser.Value));
                    CurrentBrowser = IMHome.browser;
                    //IMHome.loginIdeaManagement(BaseURL, TestUserDomain + "\\" + CentennialUser); //Login as HCA user
                    BaseTest.SessConfiguration.AppHost = BaseURL;
                    IMHome.GotoDashboard();
                    IMHome.SelectRole(imHome.Role.ReadOnly);
                    IMHome.GotoPublishedIdeas();
                    publishedIdeas = new imPublishedIdeas(IMHome.browser);
                    publishedIdeas.SearchFor(rejectIdeaId);
                    publishedIdeas.GetPublishedIdeas().First(i => i.IdeaNumber.ToString().Equals(rejectIdeaId)).IdeaName.Click(); //go to the published idea
                    publishedIdea = new imPublishedIdea(IMHome.browser);
                    HpgAssert.AreEqual(rejectIdeaId, publishedIdea.IdeaNumber.Text.Trim(), "Verify published idea Number");
                    HpgAssert.AreEqual(nameSuffix + importTitle.ToLower(), publishedIdea.IdeaTitle.Text.Trim().ToLower(), "Verify published idea Title");
                    HpgAssert.AreEqual(nameSuffix + importDescription.ToLower(), publishedIdea.Description.Text.Trim().ToLower(), "Verify published idea Description");
                    WriteInfoReport("Update Imported Idea on Streetwise Table and verify changes after importing changes");
                    #endregion
                }
                #endregion
            }
        }

        [Test][Ignore]
        public void x_FacilityTests()
        {
            foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
            {
                #region setup
                WriteInfoReport("Setup");
                WriteReport("\n~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");
                WriteInfoReport("Setup");
                #endregion

                page_objects.imHome IMHome =
                    new page_objects.imHome((BrowserSession)BaseTest.OpenNewBrowser(TestUserDomain + "\\" + ABQUser, testBrowser.Value));
                CurrentBrowser = IMHome.browser;

                #region login
                WriteInfoReport("Login");
                //page_objects.imLogin IMLogin = new page_objects.imLogin(IMHome.browser);
                //IMHome.loginIdeaManagement(BaseURL, TestUserDomain + "\\" + ABQUser);
                SessConfiguration.AppHost = BaseURL;
                IMHome.GotoDashboard();
                IMHome.SelectRole(imHome.Role.FacilityApprover);
                WriteInfoReport("Login");
                #endregion

                #region BulkEditFromPublishedList
                IMHome.GotoPublishedIdeas();
                imPublishedIdeas publishedIdeas = new imPublishedIdeas(IMHome.browser);
                //Change all ideas to 'Not Yet Reviewed'
                publishedIdeas.ShowBulkEdit();
                publishedIdeas.BulkEditSelectAllCheckbox.Check();
                publishedIdeas.BulkEditImplementDropdown.SelectListOptionByText(imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.NotYetReviewed)).Key);
                publishedIdeas.BulkEditApplyButton.Click();
                HpgAssert.Contains(publishedIdeas.BulkEditSuccessMessage.Text.Trim(), "Successfully updated all Implementated Statuses", "Verify Bulk Edit was successful");
                publishedIdeas.ShowBulkEdit();
                //System.Threading.Thread.Sleep(20000); //Let angular update the page (no page refresh)
                List<imPublishedIdeas.CFApPublishedIdea> pageIdeas = publishedIdeas.GetCFApPublishedIdeas();
                //Randomly select one to "bulk change" to 'Implemented'
                HpgAssert.True(pageIdeas.Count >= 6, "Verify there are at least 6 ideas to test");
                var rnd = new Random();
                imPublishedIdeas.CFApPublishedIdea updateIdea = pageIdeas[rnd.Next(6)];
                int r = updateIdea.IdeaNumber;
                WriteReport("Randomly selected idea " + r.ToString() + " to change...");
                publishedIdeas.BulkEditImplementDropdown.SelectListOptionByText(imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.Implemented)).Key);
                updateIdea.ImplementSelect.Element.Hover();
                System.Threading.Thread.Sleep(1000);
                publishedIdeas.CheckRetry(updateIdea.ImplementSelect);
                publishedIdeas.BulkEditApplyButton.Element.Hover();
                publishedIdeas.BulkEditApplyButton.Click();
                System.Threading.Thread.Sleep(5000);
                HpgAssert.Contains(publishedIdeas.BulkEditSuccessMessage.Text, "Successfully updated all Implementated Statuses", "Verify Bulk Edit was successful");
                publishedIdeas.Refresh();
                System.Threading.Thread.Sleep(10000);
                //Verify change to 'Implemented'...
                pageIdeas = publishedIdeas.GetCFApPublishedIdeas(); //get new updated list of ideas
                updateIdea = pageIdeas.First(i => i.IdeaNumber.Equals(r));
                HpgAssert.AreEqual(imPublishedIdeas.ImplementedStatus.Implemented.ToString(), updateIdea.ImplementedStatus.ToString(), "Verify Implemented Status was changed to 'Implemented'");
                //Change 'Implemented' to 'Discontinued'
                publishedIdeas.ShowBulkEdit();
                publishedIdeas.BulkEditImplementDropdown.SelectListOptionByText(imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.Discontinued)).Key);
                updateIdea.ImplementSelect.Element.Hover();
                System.Threading.Thread.Sleep(1000);
                publishedIdeas.CheckRetry(updateIdea.ImplementSelect);
                System.Threading.Thread.Sleep(10000); //Let angular update the page (no page refresh)
                publishedIdeas.BulkEditApplyButton.Element.Hover();
                publishedIdeas.BulkEditApplyButton.Click();
                HpgAssert.Contains(publishedIdeas.BulkEditSuccessMessage.Text, "Successfully updated all Implementated Statuses", "Verify Bulk Edit was successful");
                publishedIdeas.Refresh();
                System.Threading.Thread.Sleep(10000);
                //Verify change to 'Discontinued'...
                pageIdeas = publishedIdeas.GetCFApPublishedIdeas(); //get new updated list of ideas
                updateIdea = pageIdeas.First(i => i.IdeaNumber.Equals(r));
                HpgAssert.AreEqual(imPublishedIdeas.ImplementedStatus.Discontinued.ToString(), updateIdea.ImplementedStatus.ToString(), "Verify Implemented Status was changed to 'Implemented'");
                #endregion

                #region IndividualEditFromPublishedIdea
                updateIdea.IdeaName.Click();
                imPublishedIdea publishedIdea = new imPublishedIdea(publishedIdeas.browser);
                publishedIdea.ImplementationStatusDropDown.SelectListOptionByText(imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.NotYetReviewed)).Key);
                publishedIdea.Refresh();
                HpgAssert.AreEqual(publishedIdea.ImplementationStatusDropDown.Element.SelectedOption, imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.NotYetReviewed)).Key, "Verify selected status is 'Not Yet Reviewed");
                HpgAssert.False(publishedIdea.ImplementationStatusDropDown.OptionsAvailable.Contains(imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.Discontinued)).Key), "Verify 'Discontinue' is not available under 'Not Yet Reviewed'");
                publishedIdea.ImplementationStatusDropDown.SelectListOptionByText(imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.InProcess)).Key);
                publishedIdea.Refresh();
                HpgAssert.AreEqual(publishedIdea.ImplementationStatusDropDown.Element.SelectedOption, imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.InProcess)).Key, "Verify selected status is 'In Process");
                HpgAssert.False(publishedIdea.ImplementationStatusDropDown.OptionsAvailable.Contains(imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.Discontinued)).Key), "Verify 'Discontinue' is not available under 'In Process'");
                publishedIdea.ImplementationStatusDropDown.SelectListOptionByText(imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.Rejected)).Key);
                publishedIdea.Refresh();
                HpgAssert.AreEqual(publishedIdea.ImplementationStatusDropDown.Element.SelectedOption, imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.Rejected)).Key, "Verify selected status is 'Rejected");
                HpgAssert.False(publishedIdea.ImplementationStatusDropDown.OptionsAvailable.Contains(imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.Discontinued)).Key), "Verify 'Discontinue' is not available under 'Rejected'");
                publishedIdea.ImplementationStatusDropDown.SelectListOptionByText(imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.UnderReview)).Key);
                publishedIdea.Refresh();
                HpgAssert.AreEqual(publishedIdea.ImplementationStatusDropDown.Element.SelectedOption, imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.UnderReview)).Key, "Verify selected status is 'Under Review");
                HpgAssert.False(publishedIdea.ImplementationStatusDropDown.OptionsAvailable.Contains(imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.Discontinued)).Key), "Verify 'Discontinue' is not available under 'Under Review'");
                publishedIdea.ImplementationStatusDropDown.SelectListOptionByText(imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.NotApplicable)).Key);
                publishedIdea.Refresh();
                HpgAssert.AreEqual(publishedIdea.ImplementationStatusDropDown.Element.SelectedOption, imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.NotApplicable)).Key, "Verify selected status is 'Not Applicable");
                HpgAssert.False(publishedIdea.ImplementationStatusDropDown.OptionsAvailable.Contains(imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.Discontinued)).Key), "Verify 'Discontinue' is not available under 'Not Applicable'");
                publishedIdea.ImplementationStatusDropDown.SelectListOptionByText(imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.ImplementedNotReporting)).Key);
                publishedIdea.Refresh();
                HpgAssert.AreEqual(publishedIdea.ImplementationStatusDropDown.Element.SelectedOption, imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.ImplementedNotReporting)).Key, "Verify selected status is 'Implemented - Not Reporting");
                HpgAssert.False(publishedIdea.ImplementationStatusDropDown.OptionsAvailable.Contains(imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.Discontinued)).Key), "Verify 'Discontinue' is not available under 'Implemented - Not Reporting'");
                //Set to 'Implemented'
                publishedIdea.ImplementationStatusDropDown.SelectListOptionByText(imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.Implemented)).Key);
                publishedIdeas.Refresh();
                HpgAssert.AreEqual(publishedIdea.ImplementationStatusDropDown.Element.SelectedOption, imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.Implemented)).Key, "Verify selected status is 'Implemented");
                HpgAssert.True(publishedIdea.ImplementationStatusDropDown.OptionsAvailable.Contains(imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.Discontinued)).Key), "Verify 'Discontinue' is available under 'Implemented'");
                publishedIdea.ImplementationStatusDropDown.SelectListOptionByText(imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.Discontinued)).Key);
                publishedIdea.Refresh();
                HpgAssert.AreEqual(publishedIdea.ImplementationStatusDropDown.Element.SelectedOption, imPublishedIdeas.ImplementedStatusString.First(s => s.Value.Equals(imPublishedIdeas.ImplementedStatus.Discontinued)).Key, "Verify selected status is 'Discontinued");
                #endregion

            }

        }







        [Test][Ignore]
        public void X_BasicAuthTest()
        {
            foreach (KeyValuePair<string, Browser> testBrowser in ffOnly)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                
                page_objects.imHome IMHome = new page_objects.imHome((BrowserSession) BaseTest.OpenNewBrowser(
                    TestUserDomain + "\\" + SMEUser, "T3st5678", testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //IMHome.GotoHomePage();
                IMHome.GotoDashboard();
                //JS to clear authentication
                //document.execCommand('ClearAuthenticationCache', 'false');
                //along with clearing all cookies (FF/Chrome)
            }
        }



        [Test][Ignore]
        public void X_CreateIdeasInEachStatus()
        {
            DBUtility dbUtility = new DBUtility();
            List<int> createdIdeas = new List<int>();


            int numberOfIdeasEach = 5;
            List<DBUtility.IdeaStatus> statuses = new List<DBUtility.IdeaStatus>() { DBUtility.IdeaStatus.Submitted, DBUtility.IdeaStatus.Declined, DBUtility.IdeaStatus.UnderReview};
            List<DBUtility.WorkflowStep> workflowSteps = new List<DBUtility.WorkflowStep>() { DBUtility.WorkflowStep.User, DBUtility.WorkflowStep.Boss, DBUtility.WorkflowStep.SME, DBUtility.WorkflowStep.Admin};
            string userName = @"HCA\GFA8449";
            string assignedTo = @"HCA\IZL8181";
            
            string nameSuffix = DateTime.Now.ToString("yyyyMMddhhmmss");

            for (int i = 0; i < numberOfIdeasEach; i++)
            {
                foreach (DBUtility.IdeaStatus ideaStatus in statuses)
                {
                    foreach (DBUtility.WorkflowStep workflowStep in workflowSteps)
                    {
                        createdIdeas.Add(dbUtility.CreateNewIdea("Test Idea " + (i+1).ToString() + " - " + nameSuffix,
                                                                 "This idea was created for testing " +
                                                                 DateTime.Now.ToString(), ideaStatus, userName, workflowStep: workflowStep, assignedTo: assignedTo));
                    }
                }
            }
            HpgAssert.Fail(string.Join(",", createdIdeas.ToArray())); //call fail so it doesn't close all browsers
        }




        public class CookieAwareWebClient : WebClient
        {
            private CookieContainer cc = new CookieContainer();
            private string lastPage;

            protected override WebRequest GetWebRequest(System.Uri address)
            {
                WebRequest R = base.GetWebRequest(address);
                if (R is HttpWebRequest)
                {
                    HttpWebRequest WR = (HttpWebRequest)R;
                    WR.CookieContainer = cc;
                    if (lastPage != null)
                    {
                        WR.Referer = lastPage;
                    }
                }
                lastPage = address.ToString();
                return R;
            }
        }

        [Test][Ignore]
        public void XTestDomain()
        {
            CurrentBrowser.Visit(@"http://sbx-portal.healthtrustpg.com/SiteCoreAPI.asmx");
            OpenQA.Selenium.Remote.RemoteWebDriver rwd = ((RemoteWebDriver) ((BrowserSession) CurrentBrowser).Native);
            //rwd.Manage().Cookies.DeleteAllCookies();
            rwd.ExecuteScript("document.cookie='prelogin_odh=http://sbx-portal.healthtrustpg.com/';");
            //rwd.ExecuteScript("document.cookie='ASP.NET_SessionId=qfvo2j0mrw5le1rf4g4gwyge';");
            rwd.ExecuteScript(@"document.cookie='nocookie=NoCookie\r\n%0AAuthorization: Basic " + System.Convert.ToBase64String(System.Text.Encoding.UTF32.GetBytes("HCA\\gfa8449:WPpass16")) + "';");
            //rwd.Manage().Cookies.AddCookie(new OpenQA.Selenium.Cookie("prelogin_odh", @"http://sbx-portal.healthtrustpg.com/"));
            //rwd.Manage().Cookies.AddCookie(new OpenQA.Selenium.Cookie("nocookie", "NoCookie\nAuthorization: Basic " + System.Convert.ToBase64String(System.Text.Encoding.UTF32.GetBytes("HCA\\gfa8449:WPpass16"))));
            CurrentBrowser.Visit(@"http://sbx-portal.healthtrustpg.com/");
        }

        [Test][Ignore]
        public void UploadToRally()
        {
            RallyTestID = "TC4286";
            System.Threading.Thread.Sleep(10000);
            HpgAssert.True(true, "true is true");
            System.Threading.Thread.Sleep(10000);
            HpgAssert.AreEqual("match", "match", "Verify the strings match");
            System.Threading.Thread.Sleep(10000);
            HpgAssert.Contains("abcde", "bcd", "Verify string contains string");
            System.Threading.Thread.Sleep(10000);
            HpgAssert.False(false, "Verify false is false");
            System.Threading.Thread.Sleep(10000);
            HpgAssert.ValuesEqual(111.11m, 111.11m, 0.001m, "Verify tolerance");
            HpgAssert.Fail("something bad happend!");
        }

        public void cancelDialog(BrowserSession browser)
        {
            if (((RemoteWebDriver)browser.Native).Capabilities.BrowserName.ToLower().Contains("internet"))
            {
                SendKeys.SendWait("{ESC}");
            }
            else
            {
                try
                {
                    browser.CancelModalDialog();
                }
                catch (Exception e)
                {
                    SendKeys.SendWait("{ESC}");
                }
            }
            WriteReport("Dialog cancelled");
        }

        public void acceptDialog(BrowserSession browser, string browserName)
        {
                if(browserName.ToLower().Contains("internet"))
                {
                    SendKeys.SendWait("{ENTER}");
                }
                else if (browserName.ToLower().Contains("fire"))
                {
                    try
                    {
                        SendKeys.SendWait("{ENTER}");
                        browser.AcceptModalDialog();
                    }
                    catch (Exception)
                    {
                        SendKeys.SendWait("{ENTER}");
                    }
                }
                else
                {
                    browser.AcceptModalDialog();
                }
        }

        public void acceptDialog(BrowserSession browser)
        {
            acceptDialog(browser, ((RemoteWebDriver)browser.Native).Capabilities.BrowserName);
        }

        [SetUp]
        public void SetUp()
        {
            BaseTest.KillProcess("EXCEL");
            DisposeBrowsers();
            RallyBuild = DateTime.Now.ToString("yyyyMMddHHmmss");
            SessConfiguration.Match = Match.First;
        }

        [TearDown]
        public void TearDown()
        {
            if (!RallyUpload)
            {
                RallyTestID = "";
            }
            if (TestContext.CurrentContext.Result.Status.Equals(TestStatus.Passed))
            {
                DisposeBrowsers();
            }
            else
            {
                WriteReport("Browser Location: " + CurrentBrowser.Location);
                WriteReport("Error on step " + stepNumber.ToString());
            }
        }

        public void DisposeBrowsers()
        {
            try
            {
                ((BrowserSession)CurrentBrowser).Dispose();
            }
            catch (Exception)
            {
            }
            System.Threading.Thread.Sleep(1000);
            BaseTest.KillProcess("iexplore");
            BaseTest.KillProcess("firefox");
            BaseTest.KillProcess("chrome");
            BaseTest.KillProcess("IEDriverServer");
        }

        public Dictionary<string, string> GetGoodAttachments()
        {
            ScreenCapture screenCapture = new ScreenCapture();
            screenCapture.CaptureScreenToFile(Environment.CurrentDirectory + Constants.InputDataPath + @"global\Attachments\Good\CurrentScreenShot.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            return (from f in
                        System.IO.Directory.GetFiles(Environment.CurrentDirectory + Constants.InputDataPath +
                                                     @"global\Attachments\Good\")
                    select
                        new
                            {
                                k = f.Split('\\').Last(),
                                v = f
                            }).ToDictionary(p => p.k, p => p.v);
        }

        public bool waitForBrowser(BrowserSession wBrowser, int waitSeconds = 30, int retrySeconds = 5)
        {
            DateTime quitTime = DateTime.Now.AddSeconds(waitSeconds);
            while (DateTime.Now <= quitTime)
            {
                try
                {
                    wBrowser.Now();
                    if (wBrowser.FindAllXPath("*").Any())
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(retrySeconds * 1000);
                }
            }
            return false;
        }
    }
}



//-------CODE SMIPPITS-------
//CookieAwareWebClient Request = new CookieAwareWebClient();
//CookieContainer cookieContainer = new CookieContainer();
//var seleniumCookies = ((RemoteWebDriver) (IMPublishedIdea.browser).Native).Manage().Cookies.AllCookies;
//foreach (OpenQA.Selenium.Cookie cookie in seleniumCookies)
//{
//    cookieContainer.Add(new System.Net.Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain));
//}
//Request.DownloadFile(IMPublishedIdea.LinkExcel.Element["href"],saveFile);



