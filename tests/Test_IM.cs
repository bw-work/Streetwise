using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using AutomationCore;
using AutomationCore.input_objects;
using AutomationCore.utility;
using Coypu.Drivers;
using Streetwise.Utility;
using Streetwise.page_objects;
using NUnit.Framework;
using Coypu;
using System.Data;
using AutomationCore.base_tests;
using System.Windows.Forms;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium.Remote;
using Match = Coypu.Match;

namespace Streetwise.tests
{
    [TestFixture]
    public class Test_IM : SuperTest
    {
        private string BaseURL = "https://qa-streetwise.healthtrustpg.com";
        //private string BaseURL = "http://sbx-im.healthtrustpg.com";
        //private string BaseURL = "http://automation-im.healthtrustpg.com";
        //private string BaseURL = "http://deploy-streetwise.healthtrustpg.com";
        //private string BaseURL = "http://dev-streetwise.healthtrustpg.com";
        //private string BaseURL = "http://jeremy-im.healthtrustpg.com";
        //private string BaseURL = "http://robert-im.healthtrustpg.com";
        public bool RallyUpload = false;
        private IEnumerable<InputObject> testUsers;

        private Dictionary<string, Browser> ffChromeOnly = new Dictionary<string, Browser>()
            {
                {"Chrome", Coypu.Drivers.Browser.Chrome},
                {"Firefox", Coypu.Drivers.Browser.Firefox}
            };

        private Dictionary<string, Browser> ffOnly = new Dictionary<string, Browser>()
            {
                {"Firefox", Coypu.Drivers.Browser.Firefox}
            };


        public int stepNumber = 0;


        //[Test]
        //[Ignore("reason")]
        //public void zzzz_Sandbox()
        //{
        //    string saveFile = "SaveFromIE.xls";
        //    DisposeBrowsers();
        //    Parallel.ForEach(browsersToBeTested, testBrowser =>
        //        {
        //            var rnd = new Random();
        //            var readOnlyUsers = testUsers.Where(u => u.fields["Role"].Equals("ReadOnly") && !u.fields["Facility"].Contains("HCA/"));
        //            InputObject randomReadOnlyUser = readOnlyUsers.ElementAt(rnd.Next(readOnlyUsers.Count()));

        //            swHome IMHome =
        //                new page_objects.swHome(
        //                    (BrowserSession)
        //                    BaseTest.OpenNewBrowser(
        //                        randomReadOnlyUser.fields["Domain"] + "\\" + randomReadOnlyUser.fields["UserID"],
        //                        randomReadOnlyUser.fields["Password"], testBrowser.Value));
        //            CurrentBrowser = IMHome.browser;
        //            IMHome.GotoDashboard();
        //            IMHome.SelectRole(swHome.Role.ReadOnly);
        //            System.Threading.Thread.Sleep(5000);
        //            IMHome.GotoPublishedIdeas();
        //            swPublishedIdeas publishedIdeas = new swPublishedIdeas(IMHome.browser);
        //            publishedIdeas.WaitForThrobber();
        //            var publishedList = publishedIdeas.GetPublishedIdeas();
        //            var randomIdea = publishedList.ElementAt(rnd.Next(publishedList.Count - 1));
        //            randomIdea.IdeaName.Element.SendKeys(OpenQA.Selenium.Keys.Home);
        //            randomIdea.Bookmark.Hover();
        //            //randomIdea.IdeaName.Hover();
        //            randomIdea.IdeaName.Element.SendKeys(OpenQA.Selenium.Keys.ArrowUp);
        //            randomIdea.IdeaName.Element.SendKeys(OpenQA.Selenium.Keys.ArrowUp);
        //            randomIdea.IdeaName.Element.SendKeys(OpenQA.Selenium.Keys.ArrowUp);
        //            randomIdea.IdeaName.Click(10);
        //            page_objects.swPublishedIdeaDetails publishedIdea = new swPublishedIdeaDetails(publishedIdeas.browser);
        //            publishedIdea.LinkExcel.Element.Now();

        //            #region VerifyExcel
        //            WriteInfoReport("Validate Published Idea Excel function");
        //            publishedIdea.SaveExcel(Constants.CurrentDirectory + Constants.InputDataPath + saveFile);
        //            HpgAssert.AreEqual("", publishedIdea.CompareExcelFileToDetailsPage(saveFile));
        //            WriteInfoReport("Validate Published Idea Excel function complete.");
        //            #endregion

        //            #region VerifyPDF
        //            WriteInfoReport("Validate Published Idea PDF function");
        //            saveFile = Constants.CurrentDirectory + Constants.InputDataPath + "TC7265PDFExport.pdf";
        //            publishedIdea.SavePDF(saveFile);
        //            HpgAssert.True(System.IO.File.Exists(saveFile), "Verify PDF file was downloaded");
        //            WriteInfoReport("Validate Published Idea PDF function complete.");
        //            #endregion
        //        });
        //}

        //[Test]
        //[Ignore("reason")]
        //public void TCxxxx_IdeaCreationJourney()
        //{
        //    string errorStrings = "";
        //    Dictionary<string, Exception> errors = new Dictionary<string, Exception>();
        //    DisposeBrowsers();
        //    Parallel.ForEach(browsersToBeTested.Skip(1).Take(1), testBrowser =>
        //        {
        //            try
        //            {
        //                //TODO: Verify Associated Ideas (both in SoC and out of SoC of random user), Reassign @ SME, Reassign @ Admin, Retire (and un-retire)
        //                DBUtility dbUtility = new DBUtility();
        //                //dbUtility.dbDatabase = "StreetwiseDeploy";
        //                var rnd = new Random();
        //                WriteReport("\n~*~*~ Begin test with " + testBrowser.Key + " at " + DateTime.Now.ToString("yyyyMMddHHmmss") + " ~*~*~");
        //                IEnumerable<InputObject> declineIdeas = FileReader.getInputObjects("test_specific\\IdeaCreationJourney.xls", "DeclineIdeas").Where(i => !string.IsNullOrEmpty(i.fields["Title"].Trim()));
        //                IEnumerable<InputObject> badIdeas = FileReader.getInputObjects("test_specific\\IdeaCreationJourney.xls", "BadIdeas");
        //                IEnumerable<InputObject> linkslist = FileReader.getInputObjects("test_specific\\IdeaCreationJourney.xls", "LinksList");
        //                InputObject standardUser = testUsers.First(u => u.fields["Role"].Contains("Standard"));
        //                InputObject DCRDUser = testUsers.First(u => u.fields["Role"].Equals("DCRD"));
        //                InputObject SMEUser = testUsers.First(u => u.fields["Role"].Equals("SME"));
        //                InputObject AdminUser = testUsers.First(u => u.fields["Role"].Equals("Admin"));
        //                var readOnlyUsers = testUsers.Where(u => u.fields["Role"].Equals("ReadOnly") && !u.fields["Facility"].Contains("HCA/"));
        //                InputObject randomReadOnlyUser = readOnlyUsers.ElementAt(rnd.Next(readOnlyUsers.Count()));


        //                #region setup
        //                WriteInfoReport("Setup");
        //                page_objects.swHome IMHome;
        //                Dictionary<string, string> RMIAttachments = new Dictionary<string, string>();
        //                string nameSuffix = string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), testBrowser.Key);
        //                string saveFile = string.Format("{0}.xls", nameSuffix);
        //                Dictionary<int, bool> associatedIdeasList = new Dictionary<int, bool>();
        //                AutomationCore.utility.ews ews = new AutomationCore.utility.ews();
        //                ews.FilterContains.Clear();
        //                ews.FilterContains.Add(new KeyValuePair<string, string>("subject", nameSuffix));
        //                ews.FilterGreaterThan.Clear();
        //                ews.FilterGreaterThan.Add(new KeyValuePair<string, object>("sent", DateTime.Now));
        //                ews.UserDomain = "HCA";
        //                page_objects.swPublishedIdeas publishedIdeas;
        //                WriteInfoReport("Setup complete.");
        //                #endregion

        //                #region GetIdeasToAssociate
        //                foreach (InputObject user in readOnlyUsers)
        //                {
        //                    //true = ONLY the random read only user
        //                    WriteReport(string.Format("Gathering ideas for user {0}...", user.fields["FullName"]));
        //                    IMHome = new page_objects.swHome((BrowserSession)BaseTest.OpenNewBrowser(user.fields["Domain"] + "\\" + user.fields["UserID"], user.fields["Password"], testBrowser.Value));
        //                    CurrentBrowser = IMHome.browser;
        //                    IMHome.GotoDashboard();
        //                    IMHome.SelectRole(swHome.Role.ReadOnly);
        //                    System.Threading.Thread.Sleep(3000);
        //                    IMHome.GotoPublishedIdeas();
        //                    publishedIdeas = new swPublishedIdeas(IMHome.browser);
        //                    publishedIdeas.WaitForThrobber();
        //                    int[] uids = publishedIdeas.GetPublishedIdeasIds();
        //                    foreach (int i in uids)
        //                    {
        //                        if (user.fields["UserID"].Equals(randomReadOnlyUser.fields["UserID"]))
        //                        {
        //                            if (associatedIdeasList.ContainsKey(i))
        //                            {
        //                                associatedIdeasList[i] = true;
        //                            }
        //                            else
        //                            {
        //                                associatedIdeasList.Add(i, true);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (!associatedIdeasList.ContainsKey(i))
        //                            {
        //                                associatedIdeasList.Add(i, false);
        //                            }
        //                        }
        //                    }
        //                    IMHome.browser.Dispose();
        //                }
        //                //Get 3 of each ideas
        //                #endregion

        //                #region submitIdeas
        //                IMHome = new page_objects.swHome((BrowserSession)BaseTest.OpenNewBrowser(standardUser.fields["Domain"] + "\\" + standardUser.fields["UserID"], standardUser.fields["Password"], testBrowser.Value));
        //                CurrentBrowser = IMHome.browser;
        //                SessConfiguration.AppHost = BaseURL;
        //                IMHome.GotoDashboard();
        //                IMHome.SelectRole(swHome.Role.Standard);
        //                #region saveIdea
        //                WriteInfoReport("Submit as Standard");
        //                IMHome.GotoSubmitAnIdea();
        //                page_objects.swSubmitAnIdea submitIdea = new swSubmitAnIdea(IMHome.browser);
        //                submitIdea.IdeaName.Type("Test Idea - " + nameSuffix);
        //                submitIdea.IdeaDescription.Type("This is the description \r\n" + DateTime.Now.ToLongDateString());
        //                submitIdea.FillOutQuestions(" browser: " + testBrowser.Key);
        //                //adding attachments...
        //                submitIdea.AddAttachmentsButton.Click();
        //                swSubmitAnIdea.AddAttachmentsDialog attachmentsDialog = new swSubmitAnIdea.AddAttachmentsDialog(submitIdea.AttachmentsDialog);
        //                Dictionary<string, string> goodFiles = GetGoodAttachments();
        //                attachmentsDialog.AddAttachmentButton.Click();
        //                swSubmitAnIdea.AddAttachmentsDialog.AttachmentRow attachmentRow = attachmentsDialog.GetAttachmentRow(1);
        //                attachmentRow.Title.Type("Submit attachment");
        //                attachmentRow.LocalFile.Element.SendKeys(goodFiles.First().Value);
        //                attachmentsDialog.SaveButton.Click();
        //                submitIdea.SubmitSaveButton.Click(3);
        //                //HpgAssert.True(submitIdea.browser.HasContent("Your Idea was saved successfully."), "Idea was saved successfully.");
        //                swIdeaDetails detailsPage = new swIdeaDetails(submitIdea.browser);
        //                HpgAssert.AreEqual("Saved", detailsPage.IdeaStatus.Text, "Verify idea is saved successfully");
        //                #endregion
        //                #region IncompleteIdeas
        //                WriteInfoReport("Attempt to submit incomplete ideas");
        //                submitIdea = new swSubmitAnIdea(IMHome.browser);
        //                foreach (InputObject badIdea in badIdeas)
        //                {
        //                    IMHome.GotoSubmitAnIdea();
        //                    submitIdea = new swSubmitAnIdea(IMHome.browser);
        //                    submitIdea.IdeaName.Type(badIdea.fields["Title"]);
        //                    submitIdea.IdeaDescription.Type(badIdea.fields["Description"]);
        //                    if (!string.IsNullOrEmpty(badIdea.fields["RequiredAnswers"])) submitIdea.FillOutQuestions(badIdea.fields["RequiredAnswers"]);
        //                    submitIdea.SubmitSubmitButton.Click();
        //                    HpgAssert.True(submitIdea.browser.HasNoContent("Your Idea was submitted successfully.", new Options() { Timeout = TimeSpan.FromSeconds(2) }), "Verify success message is NOT present for imcomplete form");
        //                }
        //                WriteInfoReport("Attempt to submit incomplete ideas end");
        //                #endregion
        //                #region SubmitIdeasForDecline
        //                WriteInfoReport("Submit Ideas for Decline");
        //                foreach (InputObject declineIdea in declineIdeas)
        //                {
        //                    IMHome.GotoSubmitAnIdea();
        //                    submitIdea = new swSubmitAnIdea(IMHome.browser);
        //                    submitIdea.IdeaName.Type(declineIdea.fields["Title"] + " - " + nameSuffix);
        //                    submitIdea.IdeaDescription.Type(declineIdea.fields["Description"] + "\r\nSubmitted " + DateTime.Now.ToLongDateString());
        //                    if (!string.IsNullOrEmpty(declineIdea.fields["RequiredAnswers"])) submitIdea.FillOutQuestions("\r\n" + declineIdea.fields["RequiredAnswers"] + "\r\nBrowser: " + testBrowser.Key);
        //                    submitIdea.SubmitIdea();
        //                }
        //                WriteInfoReport("Submit Ideas for Decline");
        //                #endregion
        //                #region SubmitIdeaForPublish
        //                Dashboard dashboard;
        //                //TODO: once saved ideas widget is added to standard user dashboard, use the below
        //                ////Grab idea from the dashboard
        //                //IMHome.GotoDashboard();
        //                //Dashboard dashboard = new Dashboard(IMHome.browser);
        //                //dashboard.GetSavedIdeas().First(i => i.IdeaName.Text.EndsWith("Test Idea - " + nameSuffix)).Click();
        //                IMHome.GotoMyIdeas();
        //                swMyIdeas myIdeas = new swMyIdeas(IMHome.browser);
        //                myIdeas.SearchFor(nameSuffix);
        //                myIdeas.GetAllIdeas().First(i => i.Title.Text.EndsWith("Test Idea - " + nameSuffix)).Title.Click(20);
        //                //((HpgElement)IMHome.browser.FindButton("Edit")).Click(5);
        //                IMHome.browser.ClickButton("Edit");
        //                //adding links...
        //                submitIdea.AddLinksButton.Click();
        //                swSubmitAnIdea.AddLinksDialog linksDialog = new swSubmitAnIdea.AddLinksDialog(submitIdea.LinksDialog);
        //                linksDialog.AddLinkButton.Click();
        //                swSubmitAnIdea.AddLinksDialog.LinkRow linkRow = linksDialog.GetLinkRow(1);
        //                linkRow.Title.Type(linkslist.First(l => l.fields["Type"].Equals("Link")).fields["Name"]);
        //                linkRow.Url.Type(linkslist.First(l => l.fields["Type"].Equals("Link")).fields["URL"]);
        //                linksDialog.SaveButton.Click();
        //                //Submit idea
        //                submitIdea.SubmitIdea();
        //                WriteInfoReport("Submit as Standard complete.");
        //                #endregion
        //                #endregion

        //                #region SearchMyIdeas
        //                //Search for submitted idea in MyIdeas
        //                WriteInfoReport("Search for Submitted Idea");
        //                IMHome.GotoMyIdeas();
        //                myIdeas = new swMyIdeas(IMHome.browser);
        //                myIdeas.SearchFor(nameSuffix);

        //                List<swIdeaListMaster.Refinement> refinements = myIdeas.GetAllRefinements();
        //                HpgAssert.AreEqual(nameSuffix, refinements.First(r => r.Type.ToLower().Equals("search")).Value, "Verify search has been performed");

        //                List<swAllIdeas.AllIdea> displayedIdeas = myIdeas.GetAllIdeas();
        //                int[] submittedIdeaIDs = displayedIdeas.Select(i => i.IdeaId).ToArray();
        //                swAllIdeas.AllIdea newIdea = displayedIdeas.First(i => i.Title.Text.EndsWith("Test Idea - " + nameSuffix));
        //                int newIdeaNumber = newIdea.IdeaId;
        //                IMHome.browser.Dispose();
        //                WriteInfoReport("Search for Submitted Idea complete.");
        //                #endregion

        //                ////ASSIGN THE IDEA TO THE CORRECT DCRD
        //                //foreach (int submittedIdeaID in submittedIdeaIDs)
        //                //{
        //                //    dbUtility.AssignIdeaTo(submittedIdeaID, DCRDUser.fields["Domain"] + "\\" + DCRDUser.fields["UserID"]);
        //                //}

        //                #region EditInDCRD
        //                WriteInfoReport("Edit as DCRD");
        //                IMHome = new page_objects.swHome((BrowserSession)BaseTest.OpenNewBrowser(DCRDUser.fields["Domain"] + "\\" + DCRDUser.fields["UserID"], DCRDUser.fields["Password"], testBrowser.Value));
        //                CurrentBrowser = IMHome.browser;
        //                IMHome.GotoDashboard();
        //                IMHome.SelectRole(swHome.Role.Boss);
        //                IMHome.GotoDashboard();
        //                dashboard = new Dashboard(IMHome.browser);
        //                DCRD dcrd = new DCRD(dashboard.browser);
        //                int[] submittedIdeaNumbers = dashboard.GetQueueIdeas().Where(i => i.IdeaName.Text.EndsWith(nameSuffix)).Select(i => i.IdeaId).ToArray();
        //                foreach (int ideaID in submittedIdeaNumbers)
        //                {
        //                    IMHome.GotoDashboard();
        //                    dashboard = new Dashboard(IMHome.browser);

        //                    if (!dashboard.GetQueueIdeas(true, nameSuffix).Any(i => i.IdeaId == ideaID))
        //                    {
        //                        System.Threading.Thread.Sleep(10000);
        //                        IMHome.GotoDashboard();
        //                        if (!dashboard.GetQueueIdeas(true, nameSuffix).Any(i => i.IdeaId == ideaID))
        //                        {
        //                            System.Threading.Thread.Sleep(60000);
        //                            System.Threading.Thread.Sleep(60000);
        //                        }
        //                    }
        //                    //var idea = dashboard.GetQueueIdeas(IdeaId:ideaID).First(i => i.IdeaId == ideaID);
        //                    var idea = dashboard.GetQueueIdeas(true, nameSuffix).First(i => i.IdeaId == ideaID);
        //                    if (idea.IdeaName.Text.Contains("DCRD Decline"))
        //                    {
        //                        //Decline the idea
        //                        idea.IdeaName.Hover();
        //                        idea.IdeaName.Click(2);
        //                        dcrd = new DCRD(dashboard.browser);
        //                        dcrd.DeclineButton.Click(2);
        //                        dcrd.DeclineComment.Type("Declined by DCRD during automation test at " + DateTime.Now.ToString("F"));
        //                        System.Threading.Thread.Sleep(1000);
        //                        dcrd.DeclineCommentSaveButton.Click(5);
        //                    }
        //                    else
        //                    {
        //                        idea.IdeaName.Hover();
        //                        idea.IdeaName.Click(8);
        //                        dcrd = new DCRD(dashboard.browser);
        //                        if (dcrd.IdeaID.Text.Equals(newIdeaNumber.ToString()))
        //                        {
        //                            //Request more info for the idea from the submitter
        //                            WriteReport("Requesting more info for idea " + dcrd.IdeaID.Text);
        //                            dcrd.ShowAdditionalInfoTab();
        //                            dcrd.RMIRequestFromDropDown.SelectListOptionByText("User -- " + standardUser.fields["FullName"]);
        //                            dcrd.RMIRequestTextArea.Type("Request to Submitter from DCRD " + DateTime.Now.ToString("F"));
        //                            dcrd.RMIPostButton.Click(5);
        //                        }
        //                        else
        //                        {
        //                            //Submit ideas that are for decline later in the workflow
        //                            WriteReport("Submitting to SME for decline idea " + dcrd.IdeaID.Text);
        //                            dcrd.SubmitToSME(SMEUser.fields["FullName"]);
        //                        }
        //                    }
        //                }
        //                WriteInfoReport("Edit as DCRD complete.");
        //                IMHome.browser.Dispose();
        //                #endregion

        //                #region RespondRMISubmitter
        //                IMHome = new page_objects.swHome((BrowserSession)BaseTest.OpenNewBrowser(standardUser.fields["Domain"] + "\\" + standardUser.fields["UserID"], standardUser.fields["Password"], testBrowser.Value));
        //                CurrentBrowser = IMHome.browser;
        //                SessConfiguration.AppHost = BaseURL;
        //                IMHome.GotoDashboard();
        //                IMHome.SelectRole(swHome.Role.Standard);
        //                dashboard = new Dashboard(IMHome.browser);
        //                dashboard.GetRMIIdeas(true, " - " + nameSuffix).First(i => i.IdeaId.Equals(newIdeaNumber)).IdeaName.Click();
        //                swIdeaDetails detailspage = new swIdeaDetails(dashboard.browser);
        //                detailspage.ShowAdditionalInfoTab();
        //                detailspage.RMIResponseText.Type("Response to DCRD from Submitter " + DateTime.Now.ToString("F"));
        //                detailspage.AddLinks(linkslist.Where(l => l.fields["Type"].Equals("RMISubmitter")).ToDictionary(l => l.fields["Name"], l => l.fields["URL"]), true);
        //                detailspage.ShowRMIAddAttachments();
        //                var rmiAttachment = GetRMIAttachments("Submitter1");
        //                foreach (KeyValuePair<string, string> attach in rmiAttachment)
        //                {
        //                    RMIAttachments.Add(attach.Key, attach.Value);
        //                    detailspage.AddAttachment(attach.Key, attach.Value);
        //                }
        //                detailspage.RMISaveAttachmentsAndClose();
        //                detailspage.RMIPostButton.Click(8);
        //                HpgAssert.True(detailspage.RMIMessages.First(m => m.user.Trim().Equals(standardUser.fields["FullName"])).links.Any(), "Verify Attachments are present");
        //                HpgAssert.True(detailspage.RMIMessages.First(m => m.user.Trim().Equals(standardUser.fields["FullName"])).attachments.Any(), "Verify Attachments are present");
        //                #endregion

        //                #region ApproveAtDCRD
        //                WriteInfoReport("Approve at DCRD");
        //                IMHome.browser.Dispose();
        //                IMHome = new page_objects.swHome((BrowserSession)BaseTest.OpenNewBrowser(DCRDUser.fields["Domain"] + "\\" + DCRDUser.fields["UserID"], DCRDUser.fields["Password"], testBrowser.Value));
        //                CurrentBrowser = IMHome.browser;
        //                IMHome.GotoDashboard();
        //                IMHome.SelectRole(swHome.Role.Boss);
        //                IMHome.GotoDashboard();
        //                dashboard = new Dashboard(IMHome.browser);
        //                dashboard.GetQueueIdeas(true, " - " + nameSuffix).First(i => i.IdeaId.Equals(newIdeaNumber)).IdeaName.Click();
        //                dcrd = new DCRD(dashboard.browser);
        //                System.Threading.Thread.Sleep(3000);
        //                dcrd.SubmitButton.Click();
        //                System.Threading.Thread.Sleep(2000);
        //                dcrd.SMEDropDown.SelectListOptionByText(SMEUser.fields["FullName"]);
        //                dcrd.AssignToSMESubmit.Click();
        //                IMHome.GotoDashboard();
        //                HpgAssert.False(dashboard.GetQueueIdeas(true, " - " + nameSuffix).Any(), "Verify no test ideas are left in DCRD Queue");
        //                WriteInfoReport("Approve at DCRD");
        //                #endregion

        //                #region EditAsSME
        //                WriteInfoReport("Edit as SME");
        //                IMHome.browser.Dispose();
        //                IMHome = new page_objects.swHome((BrowserSession)BaseTest.OpenNewBrowser(SMEUser.fields["Domain"] + "\\" + SMEUser.fields["UserID"], SMEUser.fields["Password"], testBrowser.Value));
        //                CurrentBrowser = IMHome.browser;
        //                IMHome.GotoDashboard();
        //                IMHome.SelectRole(swHome.Role.SME);
        //                dashboard = new Dashboard(IMHome.browser);
        //                submittedIdeaNumbers = dashboard.GetQueueIdeas(true, " - " + nameSuffix).Where(i => i.IdeaName.Text.EndsWith(" - " + nameSuffix)).Select(i => i.IdeaId).ToArray();
        //                WriteReport(submittedIdeaNumbers.Count().ToString() + " ideas found on dashboard");
        //                im3PV tPv = new im3PV(dashboard.browser);
        //                swAllIdeas allIdeas = new swAllIdeas(IMHome.browser);
        //                Dictionary<string, im3PV.IdeaValues> ideaValues = new Dictionary<string, im3PV.IdeaValues>();
        //                foreach (int ideaID in submittedIdeaNumbers)
        //                {
        //                    IMHome.GotoDashboard();
        //                    tPv.WaitForThrobber();
        //                    var idea = dashboard.GetQueueIdeas(true, " - " + nameSuffix).First(i => i.IdeaId == ideaID);
        //                    if (idea.IdeaName.Text.Contains("SME Decline"))
        //                    {
        //                        //Decline the idea
        //                        idea.IdeaName.Hover();
        //                        idea.IdeaName.Click();
        //                        tPv.WaitForThrobber();
        //                        tPv.UnlockEditIdea();
        //                        tPv.ClickDecline();
        //                        tPv.DeclineComment.Type("Declined by SME during automation test at " + DateTime.Now.ToString("F"));
        //                        System.Threading.Thread.Sleep(1000);
        //                        tPv.DeclineCommentSaveButton.Click(5);
        //                        IMHome.GotoAllIdeas();
        //                        allIdeas.WaitForThrobber();
        //                        allIdeas.SearchFor(ideaID.ToString());
        //                        HpgAssert.AreEqual("DECLINED", allIdeas.GetAllIdeas().First(i => i.IdeaId.Equals(ideaID)).Status.ToUpper(), "Verify declined idea is Declined on All Ideas");
        //                    }
        //                    else
        //                    {
        //                        idea.IdeaName.Hover();
        //                        idea.IdeaName.Click();
        //                        tPv.WaitForThrobber();
        //                        tPv.UnlockEditIdea();
        //                        tPv.ExpandAllButton.Click(1);
        //                        tPv.ExpandAllButton.Click();
        //                        if (tPv.IdeaID.Text.Equals(newIdeaNumber.ToString()))
        //                        {
        //                            //Request more info from DCRD and Submitter
        //                            HpgAssert.AreEqual(tPv.RMIRequestFrom.OptionsAvailable.Count().ToString(),
        //                                               tPv.RMIRequestSelectAll().Count().ToString(),
        //                                               "Select all user on Request More Info");
        //                            tPv.RMIRequestTextArea.Type("Request to Submitter, DCRD from SME " + DateTime.Now.ToString("F"));
        //                            tPv.RMIPostButton.Click();
        //                        }
        //                        else
        //                        {
        //                            //Approve the idea that will later be declined
        //                            WriteReport("Approve the idea that will later be declined");
        //                            ideaValues = new Dictionary<string, im3PV.IdeaValues>();
        //                            ideaValues.Add("HCA", tPv.GetIdeaValues(0));
        //                            ideaValues.Add("GPO", tPv.GetIdeaValues(1));
        //                            ideaValues.Add("NONGPO", tPv.GetIdeaValues(2));
        //                            ideaValues["HCA"].Description.Type(tPv.IdeaDescription.Text.Trim() + "\r\nEdited As SME");
        //                            tPv.CopyFields(0);
        //                            tPv.CopyFields(1);
        //                            ideaValues["HCA"].Action.SelectListOptionByText("Not Applicable");
        //                            System.Threading.Thread.Sleep(1000);
        //                            ideaValues["GPO"].Action.SelectListOptionByText("Ready to Publish");
        //                            System.Threading.Thread.Sleep(1000);
        //                            ideaValues["NONGPO"].Action.SelectListOptionByText("Ready to Publish");
        //                            System.Threading.Thread.Sleep(5000);
        //                            ////Add attachments on 3PV
        //                            //tPv.EditAttachments.Click();
        //                            //HpgAssert.True(tPv.EditAttachmentsDialog.Element.Exists(), "Verify 'Edit Attachments' dialog is present");
        //                            //tPv.AddAttachments(goodFiles.Skip(1).ToDictionary(p => p.Key, p => p.Value));
        //                            //tPv.EditAttachmentsSave.Click();
        //                            //System.Threading.Thread.Sleep(15000);  //Waiting for attachments to upload and attach
        //                            ////Add Links on 3PV
        //                            //tPv.EditLinks.Click();
        //                            //HpgAssert.True(tPv.EditLinkDialog.Element.Exists(), "Verify 'Edit Links' dialog is present");
        //                            //tPv.AddLinks(linkslist.Where(l => l.fields["Type"].Equals("Link")).Skip(1).ToDictionary(l => l.fields["Name"], l => l.fields["URL"]));
        //                            //tPv.EditLinkSave.Click();
        //                            //Save 3PV
        //                            tPv.ClickCommit();
        //                            IMHome.GotoAllIdeas();
        //                            allIdeas.SearchFor(ideaID.ToString());
        //                            HpgAssert.False(allIdeas.GetAllIdeas().Any(i => i.IdeaId.Equals(ideaID)), "Verify idea submitted to Admin no longer shows up in All Ideas");
        //                        }
        //                    }

        //                }
        //                WriteInfoReport("Edit as SME complete.");
        //                #endregion

        //                #region RespondRMISubmitter2
        //                WriteInfoReport("Respond to SME RMI as Submitter");
        //                IMHome.browser.Dispose();
        //                IMHome = new page_objects.swHome((BrowserSession)BaseTest.OpenNewBrowser(standardUser.fields["Domain"] + "\\" + standardUser.fields["UserID"], standardUser.fields["Password"], testBrowser.Value));
        //                CurrentBrowser = IMHome.browser;
        //                SessConfiguration.AppHost = BaseURL;
        //                IMHome.GotoDashboard();
        //                IMHome.SelectRole(swHome.Role.Standard);
        //                dashboard = new Dashboard(IMHome.browser);
        //                dashboard.GetRMIIdeas(true, " - " + nameSuffix).First(i => i.IdeaId.Equals(newIdeaNumber)).IdeaName.Click();
        //                detailspage = new swIdeaDetails(dashboard.browser);
        //                detailspage.ShowAdditionalInfoTab();
        //                detailspage.RMIResponseText.Type("Response to SME from Submitter " + DateTime.Now.ToString("F"));
        //                detailspage.AddLinks(linkslist.Where(l => l.fields["Type"].Equals("RMISubmitter2")).ToDictionary(l => l.fields["Name"], l => l.fields["URL"]), true);
        //                detailspage.ShowRMIAddAttachments();
        //                rmiAttachment = GetRMIAttachments("Submitter2");
        //                foreach (KeyValuePair<string, string> attach in rmiAttachment)
        //                {
        //                    RMIAttachments.Add(attach.Key, attach.Value);
        //                    detailspage.AddAttachment(attach.Key, attach.Value);
        //                }
        //                detailspage.RMISaveAttachmentsAndClose();
        //                detailspage.RMIPostButton.Click(8);
        //                HpgAssert.True(detailspage.RMIMessages.First(m => m.user.Trim().Equals(standardUser.fields["FullName"])).links.Any(), "Verify Attachments are present");
        //                HpgAssert.True(detailspage.RMIMessages.First(m => m.user.Trim().Equals(standardUser.fields["FullName"])).attachments.Any(), "Verify Attachments are present");
        //                WriteInfoReport("Respond to SME RMI as Submitter");
        //                #endregion

        //                #region RespondRMIDCRD
        //                WriteInfoReport("Respond to SME RMI as DCRD");
        //                IMHome.browser.Dispose();
        //                IMHome = new page_objects.swHome((BrowserSession)BaseTest.OpenNewBrowser(DCRDUser.fields["Domain"] + "\\" + DCRDUser.fields["UserID"], DCRDUser.fields["Password"], testBrowser.Value));
        //                CurrentBrowser = IMHome.browser;
        //                IMHome.GotoDashboard();
        //                IMHome.SelectRole(swHome.Role.Boss);
        //                IMHome.GotoDashboard();
        //                dashboard = new Dashboard(IMHome.browser);
        //                dashboard.GetRMIIdeas(true, " - " + nameSuffix).First(i => i.IdeaId.Equals(newIdeaNumber)).IdeaName.Click();
        //                detailspage = new swIdeaDetails(dashboard.browser);
        //                detailspage.ShowAdditionalInfoTab();
        //                detailspage.AddLinks(linkslist.Where(l => l.fields["Type"].Equals("RMIDCRD")).ToDictionary(l => l.fields["Name"], l => l.fields["URL"]), true);
        //                detailspage.ShowRMIAddAttachments();
        //                rmiAttachment = GetRMIAttachments("DCRD1");
        //                foreach (KeyValuePair<string, string> attach in rmiAttachment)
        //                {
        //                    RMIAttachments.Add(attach.Key, attach.Value);
        //                    detailspage.AddAttachment(attach.Key, attach.Value);
        //                }
        //                detailspage.RMISaveAttachmentsAndClose();
        //                detailspage.RMIResponseText.Type("Response to SME from DCRD " + DateTime.Now.ToString("F"));
        //                detailspage.RMIPostButton.Click(8);
        //                HpgAssert.True(detailspage.RMIMessages.First(m => m.user.Trim().Equals(standardUser.fields["FullName"])).links.Any(), "Verify Attachments are present");
        //                HpgAssert.True(detailspage.RMIMessages.First(m => m.user.Trim().Equals(standardUser.fields["FullName"])).attachments.Any(), "Verify Attachments are present");
        //                WriteInfoReport("Respond to SME RMI as DCRD");
        //                #endregion

        //                #region ApproveAtSME
        //                WriteInfoReport("Approve idea at SME");
        //                IMHome.browser.Dispose();
        //                IMHome = new page_objects.swHome((BrowserSession)BaseTest.OpenNewBrowser(SMEUser.fields["Domain"] + "\\" + SMEUser.fields["UserID"], SMEUser.fields["Password"], testBrowser.Value));
        //                CurrentBrowser = IMHome.browser;
        //                IMHome.GotoDashboard();
        //                IMHome.SelectRole(swHome.Role.SME);
        //                dashboard = new Dashboard(IMHome.browser);
        //                dashboard.GetQueueIdeas(true, " - " + nameSuffix).First(i => i.IdeaId.Equals(newIdeaNumber)).IdeaName.Click();
        //                tPv = new im3PV(dashboard.browser);
        //                tPv.WaitForThrobber();
        //                tPv.UnlockEditIdea();
        //                tPv.ExpandAllButton.Click(1);
        //                tPv.ExpandAllButton.Click();
        //                ideaValues = new Dictionary<string, im3PV.IdeaValues>();
        //                ideaValues.Add("HCA", tPv.GetIdeaValues(0));
        //                ideaValues.Add("GPO", tPv.GetIdeaValues(1));
        //                ideaValues.Add("NONGPO", tPv.GetIdeaValues(2));
        //                ideaValues["HCA"].Description.Type(tPv.IdeaDescription.Text.Trim() + "\r\nEdited As SME");
        //                WriteReport("Selected Impact: " + ideaValues["HCA"].Impact.SelectLastOption());
        //                WriteReport("Selected Effort: " + ideaValues["HCA"].Effort.SelectLastOption());
        //                WriteReport("Selected Cetegory: " + ideaValues["HCA"].Category.SelectLastOption());
        //                WriteReport("Selected Department: " + ideaValues["HCA"].Department.SelectLastOption());
        //                ideaValues["HCA"].Results.Type("HCA Results = d/dx [f(g(x))] = f^0 (g(x))g^0 (x)\r\nCorrect?");
        //                ideaValues["GPO"].Results.Type("GPO Results = d/dx [f(g(x))] = f^0 (g(x))g^0 (x)\r\nCorrect?");
        //                ideaValues["NONGPO"].Results.Type("Non-GPO Results = d/dx [f(g(x))] = f^0 (g(x))g^0 (x)\r\nCorrect?");
        //                System.Threading.Thread.Sleep(3000);
        //                tPv.CopyFields(0);
        //                System.Threading.Thread.Sleep(1000);
        //                tPv.CopyFields(1);
        //                System.Threading.Thread.Sleep(1000);
        //                ideaValues["HCA"].Action.SelectListOptionByText("Not Applicable");
        //                System.Threading.Thread.Sleep(1000);
        //                ideaValues["GPO"].Action.SelectListOptionByText("Ready to Publish");
        //                System.Threading.Thread.Sleep(1000);
        //                ideaValues["NONGPO"].Action.SelectListOptionByText("Ready to Publish");
        //                System.Threading.Thread.Sleep(1000);
        //                //Add attachments on 3PV
        //                tPv.EditAttachments.Click(2);
        //                HpgAssert.True(tPv.EditAttachmentsDialog.Element.Exists(), "Verify 'Edit Attachments' dialog is present");
        //                tPv.AddAttachments(goodFiles.Skip(1).ToDictionary(p => p.Key, p => p.Value));
        //                tPv.EditAttachmentsSave.Click(15); //Waiting for attachments to upload and attach
        //                //Add Links on 3PV
        //                tPv.EditLinks.Click(2);
        //                HpgAssert.True(tPv.EditLinkDialog.Element.Exists(), "Verify 'Edit Links' dialog is present");
        //                tPv.AddLinks(linkslist.Where(l => l.fields["Type"].Equals("Link")).Skip(1).ToDictionary(l => l.fields["Name"], l => l.fields["URL"]));
        //                tPv.EditLinkSave.Click(10);
        //                //Save 3PV
        //                tPv.ClickCommit();
        //                IMHome.GotoAllIdeas();
        //                allIdeas = new swAllIdeas(IMHome.browser);
        //                allIdeas.SearchFor(newIdeaNumber.ToString());
        //                HpgAssert.False(allIdeas.GetAllIdeas().Any(i => i.IdeaId.Equals(newIdeaNumber)), "Verify idea submitted to Admin no longer shows up in All Ideas");
        //                WriteInfoReport("Approve idea at SME");
        //                #endregion

        //                #region EditAsAdmin

        //                #region DeclineAdminDeclineIdea
        //                WriteInfoReport("Decline idea by Admin");
        //                IMHome.browser.Dispose();
        //                IMHome = new page_objects.swHome((BrowserSession)BaseTest.OpenNewBrowser(AdminUser.fields["Domain"] + "\\" + AdminUser.fields["UserID"], AdminUser.fields["Password"], testBrowser.Value));
        //                CurrentBrowser = IMHome.browser;
        //                IMHome.GotoDashboard();
        //                IMHome.SelectRole(swHome.Role.Admin);
        //                System.Threading.Thread.Sleep(5000);
        //                dashboard = new Dashboard(IMHome.browser);
        //                tPv = new im3PV(dashboard.browser);
        //                allIdeas = new swAllIdeas(IMHome.browser);
        //                submittedIdeaNumbers = dashboard.GetQueueIdeas(true, " - " + nameSuffix).Where(i => i.IdeaName.Text.EndsWith(" - " + nameSuffix)).Select(i => i.IdeaId).ToArray();
        //                foreach (int ideaID in submittedIdeaNumbers)
        //                {
        //                    IMHome.GotoDashboard();
        //                    tPv.WaitForThrobber();
        //                    var idea = dashboard.GetQueueIdeas(true, " - " + nameSuffix).First(i => i.IdeaId == ideaID);
        //                    if (idea.IdeaName.Text.Contains("Admin Decline"))
        //                    {
        //                        //Decline the idea
        //                        idea.IdeaName.Hover();
        //                        idea.IdeaName.Click(5);
        //                        tPv.WaitForThrobber();
        //                        tPv.UnlockEditIdea();
        //                        tPv.ClickDecline();
        //                        tPv.DeclineComment.Type("Declined by SME during automation test at " + DateTime.Now.ToString("F"));
        //                        System.Threading.Thread.Sleep(1000);
        //                        tPv.DeclineCommentSaveButton.Click(5);
        //                        IMHome.GotoAllIdeas();
        //                        allIdeas.WaitForThrobber();
        //                        allIdeas.SearchFor(ideaID.ToString());
        //                        HpgAssert.AreEqual("DECLINED", allIdeas.GetAllIdeas().First(i => i.IdeaId.Equals(ideaID)).Status.ToUpper(), "Verify declined idea is Declined on All Ideas");
        //                    }
        //                }
        //                WriteInfoReport("Decline idea by Admin");
        //                #endregion

        //                #region RequestMoreInfoFromAdmin
        //                WriteInfoReport("Request More Info by Admin");
        //                IMHome.GotoDashboard();
        //                dashboard.WaitForThrobber();
        //                Dashboard.QueueIdea adminIdea = dashboard.GetQueueIdeas(true, " - " + nameSuffix).First(i => i.IdeaId.Equals(newIdeaNumber));
        //                adminIdea.IdeaName.Click();
        //                tPv.WaitForThrobber();
        //                //tPv.UnlockEditIdea();
        //                tPv.ExpandAllButton.Click(1);
        //                tPv.ExpandAllButton.Click();
        //                //Request more info from DCRD, SME, and Submitter
        //                HpgAssert.AreEqual(tPv.RMIRequestFrom.OptionsAvailable.Count().ToString(),
        //                                   tPv.RMIRequestSelectAll().Count().ToString(),
        //                                   "Select all user on Request More Info");
        //                tPv.RMIRequestTextArea.Type("Request to Submitter, DCRD, SME from Admin " + DateTime.Now.ToString("F"));
        //                tPv.RMIPostButton.Click(4);
        //                //tPv.SaveAllButton.Click(4);
        //                WriteInfoReport("Request More Info by Admin");
        //                #endregion

        //                #region RespondBySubmitterDCRDSME
        //                #region RespondRMISubmitter3
        //                WriteInfoReport("Respond to Admin RMI as Submitter");
        //                IMHome.browser.Dispose();
        //                IMHome = new page_objects.swHome((BrowserSession)BaseTest.OpenNewBrowser(standardUser.fields["Domain"] + "\\" + standardUser.fields["UserID"], standardUser.fields["Password"], testBrowser.Value));
        //                CurrentBrowser = IMHome.browser;
        //                SessConfiguration.AppHost = BaseURL;
        //                IMHome.GotoDashboard();
        //                IMHome.SelectRole(swHome.Role.Standard);
        //                dashboard = new Dashboard(IMHome.browser);
        //                dashboard.GetRMIIdeas(true, " - " + nameSuffix).First(i => i.IdeaId.Equals(newIdeaNumber)).IdeaName.Click();
        //                detailspage = new swIdeaDetails(dashboard.browser);
        //                detailspage.ShowAdditionalInfoTab();
        //                detailspage.RMIResponseText.Type("Response to Admin from Submitter " + DateTime.Now.ToString("F"));
        //                detailspage.AddLinks(linkslist.Where(l => l.fields["Type"].Equals("RMISubmitter3")).ToDictionary(l => l.fields["Name"], l => l.fields["URL"]), true);
        //                detailspage.ShowRMIAddAttachments();
        //                rmiAttachment = GetRMIAttachments("Submitter3");
        //                foreach (KeyValuePair<string, string> attach in rmiAttachment)
        //                {
        //                    RMIAttachments.Add(attach.Key, attach.Value);
        //                    detailspage.AddAttachment(attach.Key, attach.Value);
        //                }
        //                detailspage.RMISaveAttachmentsAndClose();
        //                detailspage.RMIPostButton.Click(8);
        //                HpgAssert.True(detailspage.RMIMessages.First(m => m.user.Trim().Equals(standardUser.fields["FullName"])).links.Any(), "Verify Attachments are present");
        //                HpgAssert.True(detailspage.RMIMessages.First(m => m.user.Trim().Equals(standardUser.fields["FullName"])).attachments.Any(), "Verify Attachments are present");
        //                WriteInfoReport("Respond to Admin RMI as Submitter");
        //                #endregion
        //                #region RespondRMIDCRD2
        //                WriteInfoReport("Respond to Admin RMI as DCRD");
        //                IMHome.browser.Dispose();
        //                IMHome = new page_objects.swHome((BrowserSession)BaseTest.OpenNewBrowser(DCRDUser.fields["Domain"] + "\\" + DCRDUser.fields["UserID"], DCRDUser.fields["Password"], testBrowser.Value));
        //                CurrentBrowser = IMHome.browser;
        //                IMHome.GotoDashboard();
        //                IMHome.SelectRole(swHome.Role.Boss);
        //                IMHome.GotoDashboard();
        //                dashboard = new Dashboard(IMHome.browser);
        //                dashboard.GetRMIIdeas(true, " - " + nameSuffix).First(i => i.IdeaId.Equals(newIdeaNumber)).IdeaName.Click();
        //                detailspage = new swIdeaDetails(dashboard.browser);
        //                detailspage.ShowAdditionalInfoTab();
        //                detailspage.RMIResponseText.Type("Response to Admin from DCRD " + DateTime.Now.ToString("F"));
        //                detailspage.AddLinks(linkslist.Where(l => l.fields["Type"].Equals("RMIDCRD2")).ToDictionary(l => l.fields["Name"], l => l.fields["URL"]), true);
        //                detailspage.ShowRMIAddAttachments();
        //                rmiAttachment = GetRMIAttachments("DCRD2");
        //                foreach (KeyValuePair<string, string> attach in rmiAttachment)
        //                {
        //                    RMIAttachments.Add(attach.Key, attach.Value);
        //                    detailspage.AddAttachment(attach.Key, attach.Value);
        //                }
        //                detailspage.RMISaveAttachmentsAndClose();
        //                detailspage.RMIPostButton.Click(8);
        //                HpgAssert.True(detailspage.RMIMessages.First(m => m.user.Trim().Equals(standardUser.fields["FullName"])).links.Any(), "Verify Attachments are present");
        //                HpgAssert.True(detailspage.RMIMessages.First(m => m.user.Trim().Equals(standardUser.fields["FullName"])).attachments.Any(), "Verify Attachments are present");
        //                WriteInfoReport("Respond to Admin RMI as DCRD");
        //                #endregion
        //                #region RespondRMISME
        //                WriteInfoReport("Respond to Admin RMI as SME");
        //                IMHome.browser.Dispose();
        //                IMHome = new page_objects.swHome((BrowserSession)BaseTest.OpenNewBrowser(SMEUser.fields["Domain"] + "\\" + SMEUser.fields["UserID"], SMEUser.fields["Password"], testBrowser.Value));
        //                CurrentBrowser = IMHome.browser;
        //                IMHome.GotoDashboard();
        //                IMHome.SelectRole(swHome.Role.SME);
        //                dashboard = new Dashboard(IMHome.browser);
        //                dashboard.GetRMIIdeas(true, " - " + nameSuffix).First(i => i.IdeaId.Equals(newIdeaNumber)).IdeaName.Click();
        //                tPv = new im3PV(dashboard.browser);
        //                tPv.WaitForThrobber();
        //                tPv.ExpandAllButton.Click(1);
        //                tPv.ExpandAllButton.Click();
        //                tPv.RMIResponseTextArea.Type("Response to Admin from SME " + DateTime.Now.ToString("F"));
        //                tPv.RMIAddLinks(linkslist.Where(l => l.fields["Type"].Equals("RMIDSME")).ToDictionary(l => l.fields["Name"], l => l.fields["URL"]), true);
        //                tPv.RMIResponseAddAttachments.Click(1);
        //                rmiAttachment = GetRMIAttachments("SME");
        //                foreach (KeyValuePair<string, string> attach in rmiAttachment)
        //                {
        //                    RMIAttachments.Add(attach.Key, attach.Value);
        //                    tPv.RMIAddAttachment(attach.Key, attach.Value);
        //                }
        //                tPv.AddAttachmentsDialogSaveButton.Click(8);
        //                tPv.RMIPostButton.Click(8);
        //                HpgAssert.True(tPv.RMIMessages.First(m => m.user.Trim().Equals(standardUser.fields["FullName"])).links.Any(), "Verify Attachments are present");
        //                HpgAssert.True(tPv.RMIMessages.First(m => m.user.Trim().Equals(standardUser.fields["FullName"])).attachments.Any(), "Verify Attachments are present");
        //                WriteInfoReport("Respond to Admin RMI as SME");
        //                #endregion
        //                #endregion

        //                #region ApproveAtAdmin
        //                WriteInfoReport("Approve as Admin");
        //                IMHome.browser.Dispose();
        //                IMHome = new page_objects.swHome((BrowserSession)BaseTest.OpenNewBrowser(AdminUser.fields["Domain"] + "\\" + AdminUser.fields["UserID"], AdminUser.fields["Password"], testBrowser.Value));
        //                CurrentBrowser = IMHome.browser;
        //                IMHome.GotoDashboard();
        //                IMHome.SelectRole(swHome.Role.Admin);
        //                dashboard = new Dashboard(IMHome.browser);
        //                dashboard.GetQueueIdeas(true, " - " + nameSuffix).First(i => i.IdeaId.Equals(newIdeaNumber)).IdeaName.Click();
        //                tPv = new im3PV(dashboard.browser);
        //                tPv.WaitForThrobber();
        //                tPv.UnlockEditIdea();
        //                tPv.ExpandAllButton.Click(1);
        //                tPv.ExpandAllButton.Click(1);
                        
        //                tPv.SelectAssociatedIdeas(
        //                        associatedIdeasList.Where(i => i.Value.Equals(false)).Take(3).Select(i => i.Key).ToArray()); //Add some associated ideas that should NOT show up for random user
        //                tPv.SelectAssociatedIdeas(
        //                        associatedIdeasList.Where(i => i.Value.Equals(true)).Take(3).Select(i => i.Key).ToArray()); //Add some associated ideas that SHOULD show up for random user
        //                ideaValues = new Dictionary<string, im3PV.IdeaValues>();
        //                ideaValues.Add("HCA", tPv.GetIdeaValues(0));
        //                ideaValues.Add("GPO", tPv.GetIdeaValues(1));
        //                ideaValues.Add("NONGPO", tPv.GetIdeaValues(2));
        //                foreach (KeyValuePair<string, im3PV.IdeaValues> ideaValue in ideaValues)
        //                {
        //                    ideaValue.Value.Description.Element.Click();
        //                    ideaValue.Value.Description.Element.SendKeys(OpenQA.Selenium.Keys.End);
        //                    ideaValue.Value.Description.Element.SendKeys("\r\n" + ideaValue.Key);
        //                    ideaValue.Value.Description.Element.SendKeys("\r\nEdited by Admin");
        //                    ideaValue.Value.ReportingType.SelectLastOption();
        //                    ideaValue.Value.ResultType.SelectFirstOption();
        //                }
        //                ideaValues["GPO"].Action.SelectListOptionByText("Retire");
        //                System.Threading.Thread.Sleep(1000);
        //                ideaValues["NONGPO"].Action.SelectListOptionByText("Publish");
        //                System.Threading.Thread.Sleep(1000);
        //                tPv.DisplaySoCDialog(2);
        //                im3PV.SoCDialog socDialog = new im3PV.SoCDialog(tPv.socDialog);
        //                socDialog.ApplySoC(randomReadOnlyUser.fields["Facility"].Trim()); //Apply the facility of the randomly-selected ReadOnly user
        //                socDialog.SaveButton.Click(4);
        //                tPv.ClickCommit();
        //                WriteInfoReport("Approve as Admin");
        //                #endregion
        //                #endregion

        //                #region SearchAllIdeas
        //                WriteInfoReport("Search All Ideas");
        //                IMHome.GotoAllIdeas();
        //                allIdeas = new swAllIdeas(IMHome.browser);
        //                allIdeas.SearchFor(nameSuffix);
        //                System.Threading.Thread.Sleep(2000);

        //                refinements = allIdeas.GetAllRefinements();
        //                HpgAssert.AreEqual(nameSuffix, refinements.First(r => r.Type.ToLower().Equals("search")).Value, "Verify search has been performed");

        //                displayedIdeas = allIdeas.GetAllIdeas();

        //                newIdea = displayedIdeas.First(i => i.IdeaId.Equals(newIdeaNumber));
        //                HpgAssert.AreEqual("N R P", newIdea.Status, "Verify Status is correct on the card view");
        //                HpgAssert.AreEqual("P", newIdea.Level, "Verify level is correct on the card view");
        //                WriteInfoReport("Search All Ideas complete.");
        //                #endregion

        //                #region SearchPublishedIdeas
        //                WriteInfoReport("Search Published Ideas");
        //                publishedIdeas = new swPublishedIdeas(IMHome.browser);
        //                List<swPublishedIdeas.PublishedIdea> publishedIdeaList = new List<swPublishedIdeas.PublishedIdea>();
        //                #region TryOutOfSoCUsers
        //                foreach (InputObject user in readOnlyUsers.Where(u => !u.fields["UserID"].Equals(randomReadOnlyUser.fields["UserID"])))
        //                {
        //                    IMHome.browser.Dispose();
        //                    IMHome = new page_objects.swHome((BrowserSession)BaseTest.OpenNewBrowser(user.fields["Domain"] + "\\" + user.fields["UserID"], user.fields["Password"], testBrowser.Value));
        //                    CurrentBrowser = IMHome.browser;
        //                    IMHome.GotoDashboard();
        //                    IMHome.SelectRole(swHome.Role.ReadOnly);
        //                    System.Threading.Thread.Sleep(3000);
        //                    IMHome.GotoPublishedIdeas();
        //                    publishedIdeas = new swPublishedIdeas(IMHome.browser);
        //                    publishedIdeas.WaitForThrobber();
        //                    publishedIdeas.SearchFor(nameSuffix);
        //                    publishedIdeaList = publishedIdeas.GetPublishedIdeas();
        //                    HpgAssert.False(publishedIdeaList.Any(i => i.IdeaNumber.Equals(newIdeaNumber)), "Verify published idea is NOT displayed for user who's facility is outside of Idea's SoC");
        //                }
        //                #endregion

        //                #region InSoCUser
        //                IMHome.browser.Dispose();
        //                IMHome = new page_objects.swHome((BrowserSession)BaseTest.OpenNewBrowser(randomReadOnlyUser.fields["Domain"] + "\\" + randomReadOnlyUser.fields["UserID"], randomReadOnlyUser.fields["Password"], testBrowser.Value));
        //                CurrentBrowser = IMHome.browser;
        //                IMHome.GotoDashboard();
        //                IMHome.SelectRole(swHome.Role.ReadOnly);
        //                System.Threading.Thread.Sleep(5000);
        //                IMHome.GotoPublishedIdeas();
        //                publishedIdeas = new swPublishedIdeas(IMHome.browser);
        //                publishedIdeas.WaitForThrobber();
        //                publishedIdeas.SearchFor(nameSuffix);
        //                publishedIdeaList = publishedIdeas.GetPublishedIdeas();

        //                #region SearchForDeclinedIdeas
        //                foreach (int ideaID in submittedIdeaIDs)
        //                {
        //                    if (ideaID != newIdeaNumber)
        //                    {
        //                        HpgAssert.False(publishedIdeaList.Any(i => i.IdeaNumber.Equals(ideaID)), "Verify previously declined idea (" + ideaID.ToString() + ") is not on published list");
        //                    }
        //                }
        //                #endregion

        //                swPublishedIdeas.PublishedIdea newPublishedIdea = publishedIdeaList.First(i => i.IdeaNumber.Equals(newIdeaNumber));
        //                HpgAssert.Contains(newPublishedIdea.IdeaName.Text, nameSuffix, "Verify new idea is listed in Published Ideas list.");
        //                WriteInfoReport("Search Published Ideas complete.");
        //                #endregion
        //                #endregion

        //                #region ValidateAssociatedIdeas
        //                //TODO: Validate Associated Ideas when they are shown on the Published Idea Details Page
        //                foreach (KeyValuePair<int, bool> associatedIdea in associatedIdeasList.Where(i => i.Value.Equals(false)).Take(3))
        //                {
        //                    //Check each idea DOES NOT display for user
        //                }
        //                foreach (KeyValuePair<int, bool> associatedIdea in associatedIdeasList.Where(i => i.Value.Equals(true)).Take(3))
        //                {
        //                    //Check each idea DOES display for user
        //                }
        //                #endregion

        //                #region ValidatePublishedIdeaDetails
        //                WriteInfoReport("Validate Published Idea Details");
        //                newPublishedIdea.IdeaName.Click();

        //                page_objects.swPublishedIdeaDetails publishedIdea = new swPublishedIdeaDetails(publishedIdeas.browser);
        //                HpgAssert.Contains(publishedIdea.IdeaTitle.Text, nameSuffix, "Verify published idea title");
        //                HpgAssert.AreEqual(newIdeaNumber.ToString(), publishedIdea.IdeaNumber.Text.Trim(), "Verify published idea number");
        //                List<HpgElement> attachments = publishedIdea.GetAllAttachments();
        //                HpgElement attachment = attachments.First(a => a.Text.Trim().ToLower().Equals("submit attachment"));
        //                HpgAssert.True(attachment.Element.Exists(), "Verify submitted attachment is present");
        //                HpgAssert.Contains(HttpUtility.UrlDecode(attachment.Element["href"]), goodFiles.First().Key, "Verify submitted attachment link");
        //                publishedIdea.VerifyAttachmentsArePresent(goodFiles.Skip(1).ToDictionary(a => a.Key, a => a.Value));
        //                publishedIdea.VerifyLinksArePresent(linkslist.Where(l => l.fields["Type"].Equals("Link")).ToDictionary(l => l.fields["Name"], l => l.fields["URL"]));
        //                WriteInfoReport("Validate Published Idea Details complete.");
        //                #endregion

        //                #region VerifyExcel
        //                WriteInfoReport("Validate Published Idea Excel function");
        //                publishedIdea.SaveExcel(Constants.CurrentDirectory + Constants.InputDataPath + saveFile);
        //                HpgAssert.AreEqual("", publishedIdea.CompareExcelFileToDetailsPage(saveFile));
        //                WriteInfoReport("Validate Published Idea Excel function complete.");
        //                #endregion

        //                #region VerifyPDF
        //                WriteInfoReport("Validate Published Idea PDF function");
        //                saveFile = string.Format("{2}{3}{0}-{1}.pdf", nameSuffix, testBrowser.Key, Constants.CurrentDirectory, Constants.InputDataPath);
        //                publishedIdea.SavePDF(saveFile);
        //                HpgAssert.True(System.IO.File.Exists(saveFile), "Verify PDF file was downloaded");
        //                WriteInfoReport("Validate Published Idea PDF function complete.");
        //                #endregion

        //                #region Cleanup
        //                if (false)
        //                {
        //                    //soft delete ideas from the test
        //                    dbUtility.SoftDeleteIdeas(submittedIdeaIDs.Where(i => i != newIdeaNumber).ToArray()); //delete all the declined ideas
        //                }
        //                #endregion

        //                #region VerifyEmail
        //                WriteInfoReport("Validate Published Idea Email function");
        //                publishedIdea.OpenEmailDialog();
        //                System.Threading.Thread.Sleep(5000);
        //                page_objects.imPublishedIdeaEmail ideaEmail = new imPublishedIdeaEmail(publishedIdea.browser);
        //                ideaEmail.ToField.Type("HPG.automation@HCAHealthcare.com");
        //                ideaEmail.MessageField.Type(nameSuffix + " BROWSER: " + testBrowser.Key);
        //                ideaEmail.SendButton.Click();
        //                ews.FilterContains.Add(new KeyValuePair<string, string>("from", randomReadOnlyUser.fields["Email"]));
        //                WriteReport("Looking for email from " + randomReadOnlyUser.fields["Email"] + " ...");
        //                DataTable msg = ews.GetMessagesDTWait(true, 2);
        //                if (msg.Rows.Count == 0) msg = ews.GetMessagesDTWait(true, 2);
        //                HpgAssert.AreEqual("1", msg.Rows.Count.ToString(), "Verify single email was received");
        //                ews.DeleteMessage(msg.Rows[0]["ID"].ToString());
        //                WriteInfoReport("Validate Published Idea Email function complete.");
        //                #endregion

        //                WriteReport("Browser " + testBrowser.Key + " ended at " + DateTime.Now.ToString("yyyyMMddHHmmss"));
        //            }
        //            catch (Exception e)
        //            {
        //                errors.Add(string.Format("Browser: {0} at {1}", testBrowser.Key, DateTime.Now.ToString("G")), e);
        //                WriteReport(string.Format("\n!! ERROR IN {0} at {2} !!: {1}", testBrowser.Key, e.Message, DateTime.Now.ToString("yyyyMMddHHmmss")));
        //                errorStrings += string.Format("\nERROR IN {0}: {1}\n{2}", testBrowser.Key, e.Message, e.StackTrace);
        //            }
        //        });
        //    System.IO.File.WriteAllText(Constants.CurrentDirectory + @"\" + TestContext.CurrentContext.Test.Name + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt", reportOut);
        //    if (errors.Any()) 
        //        HpgAssert.Fail(errorStrings);
        //}



        //[Test]
        //public void TCxxxx_AdminJourney()
        //{
        //    //TODO: Facility Implementation (multiple/single)
        //    DBUtility dbUtility = new DBUtility();
        //    InputObject AdminUser = testUsers.First(u => u.fields["Role"].Equals("Admin"));
        //    InputObject SMEUser = testUsers.First(u => u.fields["Role"].Equals("SME"));
        //    var rnd = new Random();
        //    var readOnlyUsers = testUsers.Where(u => u.fields["Role"].Equals("ReadOnly") && u.fields["Facility"].Contains("HCA/"));
        //    InputObject randomReadOnlyUser = readOnlyUsers.ElementAt(rnd.Next(readOnlyUsers.Count()));
        //    Dictionary<string, Exception> errors = new Dictionary<string, Exception>();
        //    //dbUtility.dbDatabase = "StreetwiseDeploy";


        //    //foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested)
        //    Parallel.ForEach(browsersToBeTested.Skip(1).Take(1), testBrowser =>
        //        {
        //            try
        //            {
        //                #region setup

        //                WriteReport("\n~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
        //                DisposeBrowsers();
        //                string nameSuffix = DateTime.Now.ToString("yyyyMMddHHmmss") + "." + testBrowser.Key;
        //                page_objects.swHome IMHome =
        //                    new page_objects.swHome(
        //                        (BrowserSession)
        //                        BaseTest.OpenNewBrowser(AdminUser.fields["Domain"] + "\\" + AdminUser.fields["UserID"],
        //                                                AdminUser.fields["Password"], testBrowser.Value));
        //                BaseTest.SessConfiguration.AppHost = BaseURL;
        //                CurrentBrowser = IMHome.browser;

        //                #endregion
        //                #region staticData
        //                EditResultTypes.ResultType newResult = new EditResultTypes.ResultType()
        //                {
        //                    Name = "New Result " + nameSuffix,
        //                    ReportingPeriod = EditResultTypes.ReportingPeriod.Yearly,
        //                    UoM = "Cars"
        //                };
        //                EditResultTypes.ResultType changeResult = new EditResultTypes.ResultType()
        //                {
        //                    Name = "Change Result " + nameSuffix,
        //                    ReportingPeriod = EditResultTypes.ReportingPeriod.Quarterly,
        //                    UoM = "Baloons"
        //                };

        //                EditQuestions.Question newQuestion = new EditQuestions.Question()
        //                {
        //                    Name = "New Question " + nameSuffix,
        //                    Required = false
        //                };
        //                EditQuestions.Question changeQuestion = new EditQuestions.Question()
        //                {
        //                    Name = "Update Question" + nameSuffix,
        //                    Required = true
        //                };
        //                #endregion

        //                #region login

        //                //IMHome.loginIdeaManagement(BaseURL, TestUserDomain + "\\" + AdminUser);
        //                IMHome.GotoDashboard();
        //                IMHome.SelectRole(swHome.Role.Admin);

        //                #endregion

        //                #region editCategories
        //                if (false)
        //                {
        //                    //Edit Categories
        //                    string catName = nameSuffix;
        //                    Dictionary<string, string> categoryInfo = new Dictionary<string, string>();
        //                    IMHome.GotoCategories();
        //                    Categories categories = new Categories(IMHome.browser);
        //                    categories.Create.Name.Type(catName);
        //                    categoryInfo.Add("HCAOpportunity",
        //                                     categories.Create.HCAOpportunity.OptionsAvailable[
        //                                         rnd.Next(1, categories.Create.HCAOpportunity.OptionsAvailable.Count() - 1)]);
        //                    categoryInfo.Add("GPOOpportunity",
        //                                     categories.Create.GPOOpportunity.OptionsAvailable[
        //                                         rnd.Next(1, categories.Create.GPOOpportunity.OptionsAvailable.Count() - 1)]);
        //                    categoryInfo.Add("NonGPOOpportunity",
        //                                     categories.Create.NonGPOOpportunity.OptionsAvailable[
        //                                         rnd.Next(1, categories.Create.NonGPOOpportunity.OptionsAvailable.Count() - 1)]);
        //                    categories.Create.HCAOpportunity.SelectListOptionByText(categoryInfo["HCAOpportunity"]);
        //                    categories.Create.GPOOpportunity.SelectListOptionByText(categoryInfo["GPOOpportunity"]);
        //                    categories.Create.NonGPOOpportunity.SelectListOptionByText(categoryInfo["NonGPOOpportunity"]);
        //                    categories.Create.CreateButton.Click(5);
        //                    //New category created
        //                    //Now verify details of newly created category
        //                    Categories.Category newCategory = categories.CategoryList.First(c => c.Name.Equals(catName));
        //                    HpgAssert.AreEqual(categoryInfo["HCAOpportunity"], newCategory.HCAOpportunity, "Verify HCA Opportunity is correct");
        //                    HpgAssert.AreEqual(categoryInfo["GPOOpportunity"], newCategory.GPOOpportunity, "Verify GPO Opportunity is correct");
        //                    HpgAssert.AreEqual(categoryInfo["NonGPOOpportunity"], newCategory.NonGPOOpportunity, "Verify Non-GPO Opportunity is correct");
        //                    HpgAssert.True((newCategory.CreatedDate - DateTime.Now) <= TimeSpan.FromDays(1) && (newCategory.CreatedDate - DateTime.Now) >= TimeSpan.FromDays(-1), "Verify Created Date is correct");
        //                    //Newly created category verified correct
        //                    //Now edit newly created category
        //                    categoryInfo["HCAOpportunity"] = categories.Create.HCAOpportunity.OptionsAvailable[rnd.Next(1, categories.Create.HCAOpportunity.OptionsAvailable.Count() -1)];
        //                    categoryInfo["GPOOpportunity"] = categories.Create.GPOOpportunity.OptionsAvailable[rnd.Next(1, categories.Create.GPOOpportunity.OptionsAvailable.Count() -1)];
        //                    categoryInfo["NonGPOOpportunity"] = categories.Create.NonGPOOpportunity.OptionsAvailable[rnd.Next(1, categories.Create.NonGPOOpportunity.OptionsAvailable.Count() -1)];
        //                    newCategory.EditButton.Click(3);
        //                    catName += "EDIT";
        //                    categories.EditCategory.Name.Type(catName);
        //                    categories.EditCategory.HCAOpportunity.SelectListOptionByText(categoryInfo["HCAOpportunity"]);
        //                    categories.EditCategory.GPOOpportunity.SelectListOptionByText(categoryInfo["GPOOpportunity"]);
        //                    categories.EditCategory.NonGPOOpportunity.SelectListOptionByText(categoryInfo["NonGPOOpportunity"]);
        //                    categories.EditCategory.SaveButton.Click(5);
        //                    //Changes saved to newly created category
        //                    //Now verify changes to changed category
        //                    newCategory = categories.CategoryList.First(c => c.Name.Equals(catName));
        //                    HpgAssert.AreEqual(categoryInfo["HCAOpportunity"], newCategory.HCAOpportunity, "Verify HCA Opportunity is correct");
        //                    HpgAssert.AreEqual(categoryInfo["GPOOpportunity"], newCategory.GPOOpportunity, "Verify GPO Opportunity is correct");
        //                    HpgAssert.AreEqual(categoryInfo["NonGPOOpportunity"], newCategory.NonGPOOpportunity, "Verify Non-GPO Opportunity is correct");
        //                    HpgAssert.True((newCategory.CreatedDate - DateTime.Now) <= TimeSpan.FromDays(1) && (newCategory.CreatedDate - DateTime.Now) >= TimeSpan.FromDays(-1), "Verify Created Date is correct");
        //                    //Newly changed category changes verified correct
        //                    //Now delete newly changed category
        //                    newCategory.DeleteButton.Click(5);
        //                    categories.ConfirmDelete.DeleteButton.Click(5);
        //                    swMaster.SetCheckBox(categories.ShowDeletedCheckbox, false);
        //                    HpgAssert.False(categories.CategoryList.Any(c => c.Name.Equals(catName)), "Verify category no longer shows up in list (not showing all)");
        //                    swMaster.SetCheckBox(categories.ShowDeletedCheckbox, true);
        //                    newCategory = categories.CategoryList.First(c => c.Name.Equals(catName));
        //                    newCategory.UndoDeleteButton.Click(5);
        //                    categories.UnDelete.UndoDeleteButton.Click(5);
        //                    swMaster.SetCheckBox(categories.ShowDeletedCheckbox, false);
        //                    HpgAssert.True(categories.CategoryList.Any(c => c.Name.Equals(catName)), "Verify category is un-deleted (not showing all)");
        //                    //TODO: Delete category from DB
        //                }

        //                #endregion

        //                #region editDepartments
        //                if (false)
        //                {
        //                    string depName = nameSuffix;
        //                    IMHome.GotoDepartments();
        //                    Departments departments = new Departments(IMHome.browser);
        //                    //Try to create a department with the same name
        //                    List<Departments.Department> Departments = departments.DepartmentsList;
        //                    departments.CreateDepartment.Name.Type(Departments.ElementAt(rnd.Next(Departments.Count - 1)).Name);
        //                    departments.CreateDepartment.CreateButton.Click(2);
        //                    HpgAssert.True(departments.CreateForm.Element.HasContent("The Department already exists. If it was deleted, you will need to undo the delete to use it."), "Verify error message when creating an existing department");
        //                    //Create a new department
        //                    departments.CreateDepartment.Name.Type(depName);
        //                    departments.CreateDepartment.CreateButton.Click(5);
        //                    Departments.Department newDepartment = departments.DepartmentsList.First(d => d.Name.Equals(depName));
        //                    //Verify details of newly created department
        //                    HpgAssert.True(newDepartment.CreatedDate <= DateTime.Now.AddDays(1) && newDepartment.CreatedDate >= DateTime.Now.AddDays(-1), "Verify Created Date is correct");
        //                    //Edit newly created department
        //                    newDepartment.EditButton.Click(2);
        //                    depName += "EDIT";
        //                    departments.EditDepartment.Name.Type(depName);
        //                    departments.EditDepartment.SaveButton.Click(2);
        //                    //Verify details of newly edited department
        //                    newDepartment = departments.DepartmentsList.First(d => d.Name.Equals(depName));
        //                    HpgAssert.True(newDepartment.CreatedDate <= DateTime.Now.AddDays(1) && newDepartment.CreatedDate >= DateTime.Now.AddDays(-1), "Verify Created Date is correct");
        //                    //Delete newly edited department
        //                    newDepartment.DeleteButton.Click(3);
        //                    departments.ConfirmDelete.DeleteButton.Click(3);
        //                    swMaster.SetCheckBox(departments.ShowDeletedCheckbox, false);
        //                    HpgAssert.False(departments.DepartmentsList.Any(d => d.Name.Equals(depName)), "Verify newly deleted department no longer shows (not showing deleted)");
        //                    //Un-Delete newly deleted department
        //                    swMaster.SetCheckBox(departments.ShowDeletedCheckbox, true);
        //                    departments.DepartmentsList.First(d => d.Name.Equals(depName)).UnDeleteButton.Click(3);
        //                    departments.UnDelete.UndoDeleteButton.Click(3);
        //                    swMaster.SetCheckBox(departments.ShowDeletedCheckbox, false);
        //                    HpgAssert.True(departments.DepartmentsList.Any(d => d.Name.Equals(depName)), "Verify newly Un-deleted department shows in list (not showing deleted)");

        //                    //TODO: Delete Department from DB
        //                }
        //                #endregion

        //                #region editQuestions

        //                if (false)
        //                {
        //                    WriteReport("----- Edit Questions -----");
        //                    IMHome.GotoQuestions();
        //                    EditQuestions editQuestions = new EditQuestions(IMHome.browser);
        //                    //Create new Question
        //                    editQuestions.SubmitNewQuestion(newQuestion);
        //                    List<EditQuestions.DisplayQuestion> displayedQuestions = editQuestions.GetQuestions();
        //                    EditQuestions.DisplayQuestion submittedQuestion =
        //                        displayedQuestions.First(q => q.Name.Equals(newQuestion.Name));
        //                    HpgAssert.AreEqual("", editQuestions.CompareQuestion(submittedQuestion, newQuestion),
        //                                       "Verify Question data matches what was submitted");
        //                    //Edit newly created Question
        //                    submittedQuestion.EditButton.Click();
        //                    EditQuestions.EditForm editQuestion = new EditQuestions.EditForm(editQuestions.browser);
        //                    editQuestion.EditQuestion(changeQuestion);
        //                    System.Threading.Thread.Sleep(1000);
        //                    displayedQuestions = editQuestions.GetQuestions();
        //                    submittedQuestion = displayedQuestions.First(q => q.Name.Equals(changeQuestion.Name));
        //                    HpgAssert.AreEqual("", editQuestions.CompareQuestion(submittedQuestion, changeQuestion),
        //                                       "Verify Question data was changed correctly");
        //                    //Delete newly changed Question
        //                    submittedQuestion.DeleteButton.Click();
        //                    editQuestions.ConfirmDeleteDelete.Click();
        //                    editQuestions.Refresh();
        //                    displayedQuestions = editQuestions.GetQuestions(false);
        //                    HpgAssert.False(displayedQuestions.FindAll(q => q.Name.Equals(changeQuestion.Name)).Any(),
        //                                    "Verify Question no longer shows");
        //                    //Undo delete on newly changed Question
        //                    displayedQuestions = editQuestions.GetQuestions();
        //                    submittedQuestion = displayedQuestions.First(q => q.Name.Equals(changeQuestion.Name));
        //                    submittedQuestion.UndoDelete.Click();
        //                    editQuestions.ConfirmUndoDeleteUndo.Click();
        //                    editQuestions.Refresh();
        //                    displayedQuestions = editQuestions.GetQuestions(false);
        //                    HpgAssert.True(displayedQuestions.FindAll(q => q.Name.Equals(changeQuestion.Name)).Any(),
        //                                   "Verify undo-delted Question shows again");
        //                    //Verify requirement of newly created Question
        //                    IMHome.GotoSubmitAnIdea();
        //                    swSubmitAnIdea submitAnIdea = new swSubmitAnIdea(IMHome.browser);
        //                    HpgAssert.True(submitAnIdea.GetQuestion(changeQuestion.Name).IsRequired,
        //                                   "Verify newly created question is required on submit page");
        //                    //Cleanup
        //                    if (dbUtility.DeleteIdeaQuestion(changeQuestion.Name) != 1)
        //                        WriteReport("\n** WARNING: Question not deleted from database! **");
        //                }

        //                #endregion

        //                #region editResultTypes
        //                if (true)
        //                {
        //                    WriteReport("----- Edit Result Types -----");
        //                    IMHome.GotoResultTypes();
        //                    EditResultTypes editResultTypes = new EditResultTypes(IMHome.browser);
        //                    //Create new Result type
        //                    editResultTypes.SubmitNewResultType(newResult);
        //                    List<EditResultTypes.DisplayResultType> displayedResultTypes =
        //                        editResultTypes.GetDisplayedResultTypes();
        //                    EditResultTypes.DisplayResultType submittedResult =
        //                        displayedResultTypes.First(r => r.Name.Equals(newResult.Name));
        //                    HpgAssert.AreEqual("", editResultTypes.CompareResultTypes(newResult, submittedResult),
        //                                       "Verify Result Type data matches what was submitted");
        //                    //Edit newly created Result Type
        //                    submittedResult.Edit.Click();
        //                    EditResultTypes.EditForm editResult = new EditResultTypes.EditForm(editResultTypes.browser);
        //                    editResult.EditResult(changeResult);
        //                    System.Threading.Thread.Sleep(1000);
        //                    displayedResultTypes = editResultTypes.GetDisplayedResultTypes();
        //                    submittedResult = displayedResultTypes.First(r => r.Name.Equals(changeResult.Name));
        //                    HpgAssert.AreEqual("", editResultTypes.CompareResultTypes(changeResult, submittedResult),
        //                                       "Verify Result Type data was changed correctly");
        //                    //Delete newly changed Result Type
        //                    submittedResult.Delete.Click();
        //                    editResultTypes.ConfirmDeleteDelete.Click();
        //                    editResultTypes.Refresh();
        //                    displayedResultTypes = editResultTypes.GetDisplayedResultTypes(false);
        //                    HpgAssert.False(displayedResultTypes.FindAll(r => r.Name.Equals(changeResult.Name)).Any(),
        //                                    "Verify result no longer shows");
        //                    //Undo delete on newly changed Result Type
        //                    displayedResultTypes = editResultTypes.GetDisplayedResultTypes();
        //                    submittedResult = displayedResultTypes.First(r => r.Name.Equals(changeResult.Name));
        //                    submittedResult.UndoDelete.Click();
        //                    editResultTypes.ConfirmUndoDeleteUndo.Click();
        //                    editResultTypes.Refresh();
        //                    displayedResultTypes = editResultTypes.GetDisplayedResultTypes(false);
        //                    HpgAssert.True(displayedResultTypes.FindAll(r => r.Name.Equals(changeResult.Name)).Any(),
        //                                   "Verify un-deleted result shows again");
        //                    //Cleanup
        //                    if (!dbUtility.DeleteResultType(changeResult.Name).ToString().Equals("1"))
        //                        WriteReport("\n** WARNING: Result type not deleted from database! **");
        //                }

        //                #endregion

        //                #region editStreetwise
        //                if (false)
        //                {
        //                    WriteReport("----- Edit Streetwise -----");
        //                    IMHome.GotoNewStreetwiseIdeas();
        //                    imNewStreetwiseIdeas streetwiseIdeas = new imNewStreetwiseIdeas(IMHome.browser);

        //                    #region RejectNewIdea

        //                    WriteReport("Reject New Idea");
        //                    streetwiseIdeas.FilterNewIdeas();
        //                    List<imNewStreetwiseIdeas.NewIdea> ideaList =
        //                        streetwiseIdeas.GetAllIdeas(10).Where(i => i.CurrentAction.Equals("pending")).ToList();
        //                    HpgAssert.True(ideaList.Any(), "Verify atleast one idea is loaded on page");
        //                    imNewStreetwiseIdeas.NewIdea testIdea = ideaList.ElementAt(rnd.Next(ideaList.Count));
        //                    string rejectIdeaId = testIdea.IdeaID.Trim();
        //                    WriteReport("Rejecting idea " + rejectIdeaId + "...");
        //                    streetwiseIdeas.ScrollToBottom();
        //                    testIdea.Action.Element.SendKeys(OpenQA.Selenium.Keys.Home);
        //                    testIdea.Action.Click();
        //                    testIdea.Action.SelectListOptionByText("Exclude");
        //                    testIdea.Action.Element.SendKeys(OpenQA.Selenium.Keys.Enter);
        //                    streetwiseIdeas.Submit();
        //                    IMHome.GotoNewStreetwiseIdeas();
        //                    IMHome.Refresh();
        //                    streetwiseIdeas.FilterNewIdeas();
        //                    streetwiseIdeas.SearchFor(rejectIdeaId);
        //                    ideaList = streetwiseIdeas.GetAllIdeas(true);
        //                    HpgAssert.False(ideaList.Any(i => i.IdeaID.Trim().Equals(rejectIdeaId)),
        //                                    "Verify the rejected idea does not show up on New Streetwise Ideas page");
        //                    streetwiseIdeas.FilterAcceptedIdeas();
        //                    streetwiseIdeas.SearchFor(rejectIdeaId);
        //                    ideaList = streetwiseIdeas.GetAllIdeas(true);
        //                    HpgAssert.False(ideaList.Any(i => i.IdeaID.Trim().Equals(rejectIdeaId)),
        //                                    "Verify the rejected idea does not show up on Accepted Streetwise Ideas page");
        //                    streetwiseIdeas.FilterRejectedIdeas();
        //                    streetwiseIdeas.SearchFor(rejectIdeaId);
        //                    ideaList = streetwiseIdeas.GetAllIdeas(true);
        //                    testIdea = ideaList.First(i => i.IdeaID.Trim().Equals(rejectIdeaId));
        //                    HpgAssert.AreEqual(rejectIdeaId, testIdea.IdeaID,
        //                                       "Verify the rejected idea shows up on Rejected Streetwise Ideas page");
        //                    WriteReport("Reject New Idea - Idea (" + rejectIdeaId + ") is rejected");

        //                    #endregion

        //                    #region ImportNewIdea

        //                    WriteInfoReport("Approving idea " + rejectIdeaId + "...");
        //                    streetwiseIdeas.ScrollToBottom();
        //                    testIdea.Action.Element.SendKeys(OpenQA.Selenium.Keys.Home);
        //                    testIdea.Action.Click();
        //                    testIdea.Action.SelectListOptionByText("Import");
        //                    streetwiseIdeas.Submit();
        //                    IMHome.GotoNewStreetwiseIdeas();
        //                    IMHome.Refresh();
        //                    streetwiseIdeas.FilterNewIdeas();
        //                    streetwiseIdeas.SearchFor(rejectIdeaId);
        //                    ideaList = streetwiseIdeas.GetAllIdeas(true);
        //                    HpgAssert.False(ideaList.Any(i => i.IdeaID.Trim().Equals(rejectIdeaId)),
        //                                    "Verify the Accepted idea does not show up on New Streetwise Ideas page");
        //                    streetwiseIdeas.FilterRejectedIdeas();
        //                    streetwiseIdeas.SearchFor(rejectIdeaId);
        //                    HpgAssert.True(streetwiseIdeas.browser.HasContent("Search: " + rejectIdeaId));
        //                    ideaList = streetwiseIdeas.GetAllIdeas(true);
        //                    HpgAssert.False(ideaList.Any(i => i.IdeaID.Trim().Equals(rejectIdeaId)),
        //                                    "Verify the Accepted idea does not show up on Rejected Streetwise Ideas page");
        //                    streetwiseIdeas.FilterAcceptedIdeas();
        //                    streetwiseIdeas.SearchFor(rejectIdeaId);
        //                    ideaList = streetwiseIdeas.GetAllIdeas(true);
        //                    testIdea = ideaList.First(i => i.IdeaID.Trim().Equals(rejectIdeaId));
        //                    HpgAssert.AreEqual(rejectIdeaId, testIdea.IdeaID,
        //                                       "Verify the Accepted idea shows up on Accepted Streetwise Ideas page");
        //                    WriteInfoReport("Approving idea " + rejectIdeaId + "...");
        //                    testIdea.IdeaName.Click();
        //                    string importTitle = streetwiseIdeas.popupPublishedTitle.Text.Trim();
        //                    string importDescription = streetwiseIdeas.popupPublishedDescription.Text.Trim();

        //                    #endregion

        //                    #region PublishImportedIdea

        //                    #region EditasSME

        //                    WriteInfoReport("Edit as SME");
        //                    IMHome.browser.Dispose();
        //                    IMHome =
        //                        new page_objects.swHome(
        //                            (BrowserSession)
        //                            BaseTest.OpenNewBrowser(SMEUser.fields["Domain"] + "\\" + SMEUser.fields["UserID"],
        //                                                    SMEUser.fields["Password"], testBrowser.Value));
        //                    CurrentBrowser = IMHome.browser;
        //                    IMHome.GotoDashboard();
        //                    IMHome.SelectRole(swHome.Role.SME);
        //                    IMHome.GotoAllIdeas();
        //                    swAllIdeas allIdeas = new swAllIdeas(IMHome.browser);
        //                    allIdeas.GetFilter("Streetwise").Check();
        //                    allIdeas.SearchFor(rejectIdeaId);
        //                    allIdeas.GetAllIdeas()
        //                            .First(i => i.IdeaId.ToString().Equals(rejectIdeaId))
        //                            .Title.Element.SendKeys(OpenQA.Selenium.Keys.Enter);
        //                        //Click on the imported idea to go to it's 3PV
        //                    im3PV tPv = new im3PV(allIdeas.browser);
        //                    System.Threading.Thread.Sleep(5000);
        //                    tPv.UnlockEditIdea();
        //                    tPv.ExpandAllButton.Click(1);
        //                    tPv.ExpandAllButton.Click();
        //                    System.Threading.Thread.Sleep(5000);
        //                    Dictionary<string, im3PV.IdeaValues> ideaValues = new Dictionary<string, im3PV.IdeaValues>();
        //                    ideaValues.Add("HCA", tPv.GetIdeaValues(0));
        //                    ideaValues.Add("GPO", tPv.GetIdeaValues(1));
        //                    ideaValues.Add("NONGPO", tPv.GetIdeaValues(2));
        //                    //ideaValues["HCA"].Description.Element.Click();
        //                    //ideaValues["HCA"].Description.Element.SendKeys(OpenQA.Selenium.Keys.End);
        //                    //ideaValues["HCA"].Description.Element.SendKeys("\r\nEdited by SME");
        //                    ideaValues["GPO"].Title.Element.Click();
        //                    ideaValues["GPO"].Title.Element.SendKeys(OpenQA.Selenium.Keys.Control +
        //                                                             OpenQA.Selenium.Keys.End);
        //                    ideaValues["GPO"].Title.Element.SendKeys(" - GPO");
        //                    ideaValues["NONGPO"].Title.Element.Click();
        //                    ideaValues["NONGPO"].Title.Element.SendKeys(OpenQA.Selenium.Keys.Control +
        //                                                                OpenQA.Selenium.Keys.End);
        //                    ideaValues["NONGPO"].Title.Element.SendKeys(" - NONGPO");
        //                    tPv.CopyFields(0);
        //                    tPv.CopyFields(1);
        //                    ideaValues["HCA"].Action.SelectListOptionByText("Ready to Publish");
        //                    System.Threading.Thread.Sleep(1000);
        //                    ideaValues["GPO"].Action.SelectListOptionByText("Ready to Publish");
        //                    System.Threading.Thread.Sleep(1000);
        //                    ideaValues["NONGPO"].Action.SelectListOptionByText("Ready to Publish");
        //                    System.Threading.Thread.Sleep(1000);
        //                    //Add attachments on 3PV
        //                    //tPv.EditAttachments.Click();
        //                    //HpgAssert.True(tPv.EditAttachmentsDialog.Element.Exists(), "Verify 'Edit Attachments' dialog is present");
        //                    //tPv.AddAttachments(GetGoodAttachments());
        //                    //tPv.EditAttachmentsSave.Click();
        //                    //System.Threading.Thread.Sleep(15000);  //Waiting for attachments to upload and attach
        //                    //Add Links on 3PV
        //                    //tPv.EditLinks.Click();
        //                    //HpgAssert.True(tPv.EditLinkDialog.Element.Exists(), "Verify 'Edit Links' dialog is present");
        //                    //tPv.AddLinks(linksList.Skip(1).ToDictionary(p => p.Key, p => p.Value));
        //                    //tPv.EditLinkSave.Click();
        //                    //Save 3PV
        //                    tPv.ClickCommit();
        //                    System.Threading.Thread.Sleep(5000);
        //                    //TODO: Remove below code when streetwise commit issue is fixed
        //                    //tPv.browser.GoBack();
        //                    //tPv.UnlockEditIdea();
        //                    //tPv.ClickCommit();
        //                    //Should now be back at AllIdeas page
        //                    allIdeas.WaitForThrobber();
        //                    allIdeas.ClearRefinements();
        //                    allIdeas.SearchFor(rejectIdeaId);
        //                    HpgAssert.True(!allIdeas.GetAllIdeas().Any(i => i.IdeaId.Equals(int.Parse(rejectIdeaId))),
        //                                   "Idea is LEVEL 3 and no longer available to SME");
        //                    //HpgAssert.AreEqual("level 3", allIdeas.GetAllIdeas().First(i => i.IdeaId.ToString().Equals(rejectIdeaId)).Level.Trim().ToLower(), "Idea is now in 'LEVEL 3' workflow step");
        //                    //HpgAssert.AreEqual("level 3", tPv.WorkflowStep.Text.Trim().ToLower(), "Idea is now in 'LEVEL 3' workflow step");
        //                    WriteInfoReport("Edit as SME complete.");

        //                    #endregion

        //                    #region EditAsAdmin

        //                    WriteInfoReport("Edit as Admin");
        //                    IMHome.browser.Dispose();
        //                    IMHome =
        //                        new page_objects.swHome(
        //                            (BrowserSession)
        //                            BaseTest.OpenNewBrowser(
        //                                AdminUser.fields["Domain"] + "\\" + AdminUser.fields["UserID"],
        //                                AdminUser.fields["Password"], testBrowser.Value));
        //                    CurrentBrowser = IMHome.browser;
        //                    IMHome.GotoDashboard();
        //                    Dashboard dashboard = new Dashboard(IMHome.browser);
        //                    IMHome.SelectRole(swHome.Role.Admin);
        //                    System.Threading.Thread.Sleep(5000);
        //                    Dashboard.QueueIdea adminIdea =
        //                        dashboard.GetQueueIdeas(IdeaId: int.Parse(rejectIdeaId))
        //                                 .First(i => i.IdeaId.ToString().Equals(rejectIdeaId));
        //                    adminIdea.IdeaName.Click();
        //                    tPv = new im3PV(dashboard.browser);
        //                    tPv.UnlockEditIdea();
        //                    tPv.ExpandAllButton.Click(1);
        //                    tPv.ExpandAllButton.Click();
        //                    System.Threading.Thread.Sleep(5000);
        //                    ideaValues.Clear();
        //                    ideaValues.Add("HCA", tPv.GetIdeaValues(0));
        //                    ideaValues.Add("GPO", tPv.GetIdeaValues(1));
        //                    ideaValues.Add("NONGPO", tPv.GetIdeaValues(2));
        //                    //foreach (KeyValuePair<string, im3PV.IdeaValues> ideaValue in ideaValues)
        //                    //{
        //                    //    ideaValue.Value.Description.Element.Click();
        //                    //    ideaValue.Value.Description.Element.SendKeys(OpenQA.Selenium.Keys.End);
        //                    //    ideaValue.Value.Description.Element.SendKeys("\r\n" + ideaValue.Key);
        //                    //    ideaValue.Value.Description.Element.SendKeys("\r\nEdited by Admin");
        //                    //}
        //                    ideaValues["HCA"].Action.SelectListOptionByText("Publish");
        //                    System.Threading.Thread.Sleep(1000);
        //                    ideaValues["GPO"].Action.SelectListOptionByText("Publish");
        //                    System.Threading.Thread.Sleep(1000);
        //                    ideaValues["NONGPO"].Action.SelectListOptionByText("Publish");
        //                    System.Threading.Thread.Sleep(1000);
        //                    //tPv.DisplaySoCDialog(0); //Edit HCA SoC
        //                    //im3PV.SoCDialog socDialog = new im3PV.SoCDialog(tPv.socDialog);
        //                    //socDialog.ApplySoC("HCA", "C");
        //                    //socDialog.SaveButton.Click();
        //                    //tPv.DisplaySoCDialog(1); //Edit GPO SoC
        //                    //socDialog = new im3PV.SoCDialog(tPv.socDialog);
        //                    //socDialog.ApplySoC("HealthTrust/Acadia Healthcare Company/Acadia Healthcare Company/Acadia Healthcare Company/Acadia Healthcare Company/Acadia Corporate, Chicago Office (J6539)");
        //                    //socDialog.SaveButton.Click();
        //                    //tPv.DisplaySoCDialog(2); //Edit Non-GPO SoC
        //                    //socDialog = new im3PV.SoCDialog(tPv.socDialog);
        //                    //socDialog.ApplySoC("HealthTrust/ABQ Health Partners/ABQ Health Partners/ABQ Health Partners/ABQ Health Partners/ABQ Health Partners (J2900)");
        //                    //socDialog.SaveButton.Click();
        //                    tPv.ClickCommit();
        //                    WriteInfoReport("Edit as Admin complete.");

        //                    #endregion

        //                    #endregion

        //                    #region VerifyNewIdeaIsPublished

        //                    WriteInfoReport("Search Published Ideas");
        //                    IMHome.browser.Dispose();
        //                    IMHome =
        //                        new page_objects.swHome(
        //                            (BrowserSession)
        //                            BaseTest.OpenNewBrowser(
        //                                randomReadOnlyUser.fields["Domain"] + "\\" + randomReadOnlyUser.fields["UserID"],
        //                                randomReadOnlyUser.fields["Password"], testBrowser.Value));
        //                    CurrentBrowser = IMHome.browser;
        //                    BaseTest.SessConfiguration.AppHost = BaseURL;
        //                    IMHome.GotoDashboard();
        //                    //IMHome.loginIdeaManagement(BaseURL, TestUserDomain + "\\" + CentennialUser); //Login as HCA user
        //                    IMHome.SelectRole(swHome.Role.ReadOnly);
        //                    IMHome.GotoPublishedIdeas();
        //                    page_objects.swPublishedIdeas publishedIdeas = new swPublishedIdeas(IMHome.browser);
        //                    publishedIdeas.SearchFor(rejectIdeaId);
        //                    swPublishedIdeas.PublishedIdea newPublishedIdea =
        //                        publishedIdeas.GetPublishedIdeas()
        //                                      .First(i => i.IdeaNumber.ToString().Equals(rejectIdeaId));
        //                    HpgAssert.True(newPublishedIdea.IdeaName.Element.Exists(),
        //                                   "Verify new idea is listed in Published Ideas list.");
        //                    WriteInfoReport("Search Published Ideas complete.");
        //                    WriteInfoReport("Validate Published Idea Details");
        //                    newPublishedIdea.IdeaName.Click();
        //                    page_objects.swPublishedIdeaDetails publishedIdea = new swPublishedIdeaDetails(publishedIdeas.browser);
        //                    HpgAssert.AreEqual(rejectIdeaId, publishedIdea.IdeaNumber.Text.Trim(),
        //                                       "Verify published idea Number");
        //                    HpgAssert.AreEqual(importTitle.ToLower(), publishedIdea.IdeaTitle.Text.Trim().ToLower(),
        //                                       "Verify published idea Title");
        //                    HpgAssert.AreEqual(importDescription.ToLower(),
        //                                       publishedIdea.Description.Text.Trim().ToLower(),
        //                                       "Verify published idea Description");
        //                    WriteInfoReport("Validate Published Idea Details");

        //                    #endregion

        //                    #region UpdateNewIdeaAndVerify

        //                    WriteInfoReport(
        //                        "Update Imported Idea on Streetwise Table and verify changes after importing changes");
        //                    IMHome.browser.Dispose();
        //                    dbUtility.StreetwiseImportUpdateIdea(int.Parse(rejectIdeaId), nameSuffix);
        //                        //Make changes to idea in database
        //                    IMHome =
        //                        new page_objects.swHome(
        //                            (BrowserSession)
        //                            BaseTest.OpenNewBrowser(
        //                                AdminUser.fields["Domain"] + "\\" + AdminUser.fields["UserID"],
        //                                AdminUser.fields["Password"], testBrowser.Value));
        //                    CurrentBrowser = IMHome.browser;
        //                    //IMHome.loginIdeaManagement(BaseURL, TestUserDomain + "\\" + AdminUser);
        //                    BaseTest.SessConfiguration.AppHost = BaseURL;
        //                    IMHome.GotoDashboard();
        //                    IMHome.SelectRole(swHome.Role.Admin);
        //                    IMHome.GotoNewStreetwiseIdeas();
        //                    streetwiseIdeas = new imNewStreetwiseIdeas(IMHome.browser);
        //                    try
        //                    {
        //                        streetwiseIdeas.browser.AcceptModalDialog();
        //                    }
        //                    catch (Exception)
        //                    {
        //                    }
        //                    streetwiseIdeas.FilterUpdatedIdeas();
        //                    streetwiseIdeas.SearchFor(rejectIdeaId);
        //                    ideaList = streetwiseIdeas.GetAllIdeas(true);
        //                    testIdea = ideaList.First(i => i.IdeaID.Equals(rejectIdeaId));
        //                    streetwiseIdeas.ScrollToBottom();
        //                    testIdea.Action.Element.SendKeys(OpenQA.Selenium.Keys.Home);
        //                    testIdea.Action.Click();
        //                    testIdea.Action.SelectListOptionByText("Import Changes");
        //                    streetwiseIdeas.Submit();
        //                    IMHome.GoToPackageIdea(int.Parse(rejectIdeaId));
        //                    tPv = new im3PV(streetwiseIdeas.browser);
        //                    tPv.UnlockEditIdea();
        //                    tPv.ClickCommit();
        //                    //TODO: When Streetwise commit issue is fixed remove the following
        //                    //IMHome.browser.GoBack();
        //                    //IMHome.WaitForThrobber();
        //                    //tPv.UnlockEditIdea();
        //                    //tPv.ClickCommit();
        //                    //End of remove
        //                    System.Threading.Thread.Sleep(5000);
        //                    IMHome.browser.Dispose();
        //                    IMHome =
        //                        new page_objects.swHome(
        //                            (BrowserSession)
        //                            BaseTest.OpenNewBrowser(
        //                                randomReadOnlyUser.fields["Domain"] + "\\" + randomReadOnlyUser.fields["UserID"],
        //                                randomReadOnlyUser.fields["Password"], testBrowser.Value));
        //                    CurrentBrowser = IMHome.browser;
        //                    BaseTest.SessConfiguration.AppHost = BaseURL;
        //                    IMHome.GotoDashboard();
        //                    IMHome.SelectRole(swHome.Role.ReadOnly);
        //                    IMHome.GotoPublishedIdeas();
        //                    publishedIdeas = new swPublishedIdeas(IMHome.browser);
        //                    publishedIdeas.SearchFor(rejectIdeaId);
        //                    publishedIdeas.GetPublishedIdeas()
        //                                  .First(i => i.IdeaNumber.ToString().Equals(rejectIdeaId))
        //                                  .IdeaName.Click(); //go to the published idea
        //                    publishedIdea = new swPublishedIdeaDetails(IMHome.browser);
        //                    HpgAssert.AreEqual(rejectIdeaId, publishedIdea.IdeaNumber.Text.Trim(),
        //                                       "Verify published idea Number");
        //                    HpgAssert.AreEqual(nameSuffix + importTitle.ToLower(),
        //                                       publishedIdea.IdeaTitle.Text.Trim().ToLower(),
        //                                       "Verify published idea Title");
        //                    HpgAssert.AreEqual(nameSuffix + importDescription.ToLower(),
        //                                       publishedIdea.Description.Text.Trim().ToLower(),
        //                                       "Verify published idea Description");
        //                    WriteInfoReport(
        //                        "Update Imported Idea on Streetwise Table and verify changes after importing changes");

        //                    #endregion
        //                }

        //                #endregion
        //            }
        //            catch (Exception e)
        //            {
        //                errors.Add(string.Format("Browser: {0} at {1}", testBrowser.Key, DateTime.Now.ToString("G")), e);
        //                WriteReport(string.Format("\n!! ERROR IN {0} at {2} !!: {1}", testBrowser.Key, e.Message,
        //                                          DateTime.Now.ToString("yyyyMMddHHmmss")));
        //            }
        //            string error = "";
        //            foreach (KeyValuePair<string, Exception> exception in errors)
        //            {
        //                error += string.Format("\n------{0}------:\n{1}\n{2}\n", exception.Key, exception.Value.Message,
        //                                       exception.Value.StackTrace);
        //            }
        //            System.IO.File.WriteAllText(
        //                Constants.CurrentDirectory + @"\" + TestContext.CurrentContext.Test.Name +
        //                DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt", reportOut + error);
        //            if (errors.Any())
        //            {
        //                HpgAssert.Fail(error);
        //            }
        //        });
        //}

        //[Test][Ignore("reason")]
        //public void TCxxxx_FacilityApproverJourney()
        //{
        //    //TODO: Upload Results, Manual Input Results, Manual Edit Results, View Goals
        //    var rnd = new Random();
        //    var readOnlyUsers = testUsers.Where(u => u.fields["Role"].Equals("ReadOnly"));
        //    InputObject randomReadOnlyUser = readOnlyUsers.ElementAt(rnd.Next(readOnlyUsers.Count()));
        //    InputObject adminUser = testUsers.First(u => u.fields["Role"].Equals("Admin"));
        //    foreach (KeyValuePair<string, Browser> testBrowser in browsersToBeTested.Skip(1))
        //    {
        //        #region setup
        //        WriteInfoReport("Setup");
        //        WriteReport("\n~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
        //        DisposeBrowsers();
        //        string nameSuffix = DateTime.Now.ToString("yyyyMMddHHmmss");
        //        WriteInfoReport("Setup");
        //        #endregion

        //        #region login
        //        WriteInfoReport("Login as " + randomReadOnlyUser.fields["FullName"]);
        //        page_objects.swHome IMHome = new page_objects.swHome((BrowserSession)BaseTest.OpenNewBrowser(randomReadOnlyUser.fields["Domain"] + "\\" + randomReadOnlyUser.fields["UserID"], randomReadOnlyUser.fields["Password"], testBrowser.Value));
        //        CurrentBrowser = IMHome.browser;
        //        //page_objects.swLogin IMLogin = new page_objects.swLogin(IMHome.browser);
        //        //IMHome.loginIdeaManagement(BaseURL, TestUserDomain + "\\" + ABQUser);
        //        SessConfiguration.AppHost = BaseURL;
        //        IMHome.GotoDashboard();
        //        IMHome.SelectRole(swHome.Role.FacilityApprover);
        //        WriteInfoReport("Login");
        //        #endregion

        //        IMHome.GotoPublishedIdeas();
        //        swPublishedIdeas publishedIdeas = new swPublishedIdeas(IMHome.browser);
        //        List<swPublishedIdeas.CFApPublishedIdea> pageIdeas = new List<swPublishedIdeas.CFApPublishedIdea>();
        //        swPublishedIdeas.CFApPublishedIdea updateIdea = new swPublishedIdeas.CFApPublishedIdea();
        //        DataTable savingsTable = new DataTable("SavingsTable");
        //        savingsTable.Columns.Add(new DataColumn("IdeaId", typeof (int)));
        //        savingsTable.Columns.Add(new DataColumn("Year", typeof(int)));
        //        savingsTable.Columns.Add(new DataColumn("Month", typeof(string)));
        //        savingsTable.Columns.Add(new DataColumn("Value", typeof(decimal)));

        //        #region BulkEditFromPublishedList
        //        if (false)
        //        {
        //            //Change all ideas to 'Not Yet Reviewed'
        //            publishedIdeas.ShowBulkEdit();
        //            swMaster.SetCheckBox(publishedIdeas.BulkEditSelectAllCheckbox, true);
        //            publishedIdeas.BulkEditImplementDropdown.SelectListOptionByText(Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.NotYetReviewed)).Key);
        //            publishedIdeas.BulkEditApplyCuttonClick();
        //            HpgAssert.Contains(publishedIdeas.BulkEditSuccessMessage.Text.Trim(), "Successfully updated all Implementated Statuses", "Verify Bulk Edit was successful");
        //            IMHome.GotoPublishedIdeas();
        //            //Randomly select one to for "bulk changes"
        //            pageIdeas = publishedIdeas.GetCFApPublishedIdeas();
        //            //TODO: Refactor to improve speed for large number of ideas displayed
        //            HpgAssert.True(pageIdeas.Count > 1, "Verify there are at least 2 ideas to test");
        //            HpgAssert.False(pageIdeas.Any(i => i.ImplementedStatus != Enums.ImplementedStatus.NotYetReviewed), "All ideas are marked 'Not Yet Reviewed'");
        //            updateIdea = pageIdeas[rnd.Next(pageIdeas.Take(6).Count())]; //Randomly grab one of the first 6 ideas
        //            int r = updateIdea.IdeaNumber;
        //            WriteReport("Randomly selected idea " + r.ToString() + " to change...");
        //            //Change Attempt to change a non-Implemented idea to 'Discontinued' and check for failure
        //            publishedIdeas.ShowBulkEdit();
        //            publishedIdeas.BulkEditImplementDropdown.SelectListOptionByText(Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.Discontinued)).Key);
        //            updateIdea.ImplementSelect.Element.Hover();
        //            System.Threading.Thread.Sleep(1000);
        //            publishedIdeas.CheckRetry(updateIdea.ImplementSelect);
        //            WriteReport(string.Format("Idea {0} Implemented Status is currently '{1}', attempting to change to 'Discontinued'...", r.ToString(), updateIdea.ImplementedStatus));
        //            publishedIdeas.BulkEditApplyCuttonClick();
        //            HpgAssert.Contains(publishedIdeas.BulkEditSuccessMessage.Text, "Successfully updated all Implementated Statuses", "Verify Bulk Edit was successful");
        //            //Verify idea wasn NOT changed to 'Discontinued'...
        //            IMHome.GotoPublishedIdeas();
        //            pageIdeas = publishedIdeas.GetCFApPublishedIdeas(); //get new updated list of ideas
        //            updateIdea = pageIdeas.First(i => i.IdeaNumber.Equals(r));
        //            HpgAssert.False(Enums.ImplementedStatus.Discontinued.ToString() == updateIdea.ImplementedStatus.ToString(), "Verify non-Implemented Status was NOT changed to 'Implemented'");
        //            //Set idea to 'Implemented' and verify
        //            publishedIdeas.ShowBulkEdit();
        //            publishedIdeas.BulkEditImplementDropdown.SelectListOptionByText(Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.Implemented)).Key);
        //            updateIdea.ImplementSelect.Element.Hover();
        //            System.Threading.Thread.Sleep(1000);
        //            publishedIdeas.CheckRetry(updateIdea.ImplementSelect);
        //            publishedIdeas.BulkEditApplyCuttonClick();
        //            HpgAssert.Contains(publishedIdeas.BulkEditSuccessMessage.Text, "Successfully updated all Implementated Statuses", "Verify Bulk Edit was successful");
        //            //Verify change to 'Implemented'...
        //            IMHome.GotoPublishedIdeas();
        //            pageIdeas = publishedIdeas.GetCFApPublishedIdeas(); //get new updated list of ideas
        //            updateIdea = pageIdeas.First(i => i.IdeaNumber.Equals(r));
        //            HpgAssert.AreEqual(Enums.ImplementedStatus.Implemented.ToString(), updateIdea.ImplementedStatus.ToString(), "Verify Implemented Status was changed to 'Implemented'");
        //            //Change 'Implemented' to 'Discontinued'
        //            publishedIdeas.ShowBulkEdit();
        //            publishedIdeas.BulkEditImplementDropdown.SelectListOptionByText(Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.Discontinued)).Key);
        //            updateIdea.ImplementSelect.Element.Hover();
        //            System.Threading.Thread.Sleep(1000);
        //            publishedIdeas.CheckRetry(updateIdea.ImplementSelect);
        //            publishedIdeas.BulkEditApplyCuttonClick();
        //            HpgAssert.Contains(publishedIdeas.BulkEditSuccessMessage.Text, "Successfully updated all Implementated Statuses", "Verify Bulk Edit was successful");
        //            System.Threading.Thread.Sleep(20000);
        //            //Verify change to 'Discontinued'...
        //            IMHome.GotoPublishedIdeas();
        //            pageIdeas = publishedIdeas.GetCFApPublishedIdeas(); //get new updated list of ideas
        //            updateIdea = pageIdeas.First(i => i.IdeaNumber.Equals(r));
        //            HpgAssert.AreEqual(Enums.ImplementedStatus.Discontinued.ToString(), updateIdea.ImplementedStatus.ToString(), "Verify Implemented Status was changed to 'Discontinued'");
        //        }
        //        #endregion

        //        #region IndividualEditFromPublishedIdea
        //        if (false)
        //        {
        //            updateIdea.IdeaName.Click();
        //            swPublishedIdeaDetails publishedIdea = new swPublishedIdeaDetails(publishedIdeas.browser);
        //            publishedIdea.ImplementationStatusDropDown.SelectListOptionByText(Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.NotYetReviewed)).Key);
        //            publishedIdea.Refresh();
        //            HpgAssert.AreEqual(publishedIdea.ImplementationStatusDropDown.Element.SelectedOption, Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.NotYetReviewed)).Key, "Verify selected status is 'Not Yet Reviewed");
        //            HpgAssert.False(publishedIdea.ImplementationStatusDropDown.OptionsAvailable.Contains(Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.Discontinued)).Key), "Verify 'Discontinue' is not available under 'Not Yet Reviewed'");
        //            publishedIdea.ImplementationStatusDropDown.SelectListOptionByText(Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.InProcess)).Key);
        //            publishedIdea.Refresh();
        //            HpgAssert.AreEqual(publishedIdea.ImplementationStatusDropDown.Element.SelectedOption, Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.InProcess)).Key, "Verify selected status is 'In Process");
        //            HpgAssert.False(publishedIdea.ImplementationStatusDropDown.OptionsAvailable.Contains(Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.Discontinued)).Key), "Verify 'Discontinue' is not available under 'In Process'");
        //            publishedIdea.ImplementationStatusDropDown.SelectListOptionByText(Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.Rejected)).Key);
        //            publishedIdea.Refresh();
        //            HpgAssert.AreEqual(publishedIdea.ImplementationStatusDropDown.Element.SelectedOption, Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.Rejected)).Key, "Verify selected status is 'Rejected");
        //            HpgAssert.False(publishedIdea.ImplementationStatusDropDown.OptionsAvailable.Contains(Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.Discontinued)).Key), "Verify 'Discontinue' is not available under 'Rejected'");
        //            publishedIdea.ImplementationStatusDropDown.SelectListOptionByText(Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.UnderReview)).Key);
        //            publishedIdea.Refresh();
        //            HpgAssert.AreEqual(publishedIdea.ImplementationStatusDropDown.Element.SelectedOption, Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.UnderReview)).Key, "Verify selected status is 'Under Review");
        //            HpgAssert.False(publishedIdea.ImplementationStatusDropDown.OptionsAvailable.Contains(Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.Discontinued)).Key), "Verify 'Discontinue' is not available under 'Under Review'");
        //            publishedIdea.ImplementationStatusDropDown.SelectListOptionByText(Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.NotApplicable)).Key);
        //            publishedIdea.Refresh();
        //            HpgAssert.AreEqual(publishedIdea.ImplementationStatusDropDown.Element.SelectedOption, Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.NotApplicable)).Key, "Verify selected status is 'Not Applicable");
        //            HpgAssert.False(publishedIdea.ImplementationStatusDropDown.OptionsAvailable.Contains(Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.Discontinued)).Key), "Verify 'Discontinue' is not available under 'Not Applicable'");
        //            publishedIdea.ImplementationStatusDropDown.SelectListOptionByText(Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.ImplementedNotReporting)).Key);
        //            publishedIdea.Refresh();
        //            HpgAssert.AreEqual(publishedIdea.ImplementationStatusDropDown.Element.SelectedOption, Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.ImplementedNotReporting)).Key, "Verify selected status is 'Implemented - Not Reporting");
        //            HpgAssert.False(publishedIdea.ImplementationStatusDropDown.OptionsAvailable.Contains(Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.Discontinued)).Key), "Verify 'Discontinue' is not available under 'Implemented - Not Reporting'");
        //            //Set to 'Implemented'
        //            publishedIdea.ImplementationStatusDropDown.SelectListOptionByText(Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.Implemented)).Key);
        //            publishedIdeas.Refresh();
        //            HpgAssert.AreEqual(publishedIdea.ImplementationStatusDropDown.Element.SelectedOption, Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.Implemented)).Key, "Verify selected status is 'Implemented");
        //            HpgAssert.True(publishedIdea.ImplementationStatusDropDown.OptionsAvailable.Contains(Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.Discontinued)).Key), "Verify 'Discontinue' is available under 'Implemented'");
        //            publishedIdea.ImplementationStatusDropDown.SelectListOptionByText(Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.Discontinued)).Key);
        //            publishedIdea.Refresh();
        //            HpgAssert.AreEqual(publishedIdea.ImplementationStatusDropDown.Element.SelectedOption, Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.Discontinued)).Key, "Verify selected status is 'Discontinued");
        //        }
        //        #endregion

        //        #region FacilitySavings
        //        #region BulkImplementRandom9Ideas
        //        IMHome.GotoPublishedIdeas();
        //        publishedIdeas = new swPublishedIdeas(IMHome.browser);
        //        //Randomly select 9 to implement
        //        HpgAssert.True(publishedIdeas.FilterRecordCountInt > 8, "Verify there are at least 9 ideas to test");
        //        System.Threading.Thread.Sleep(5000);
        //        pageIdeas = publishedIdeas.GetCFApPublishedIdeas(publishedIdeas.FilterRecordCountInt > 9 ? rnd.Next(publishedIdeas.FilterRecordCountInt - 9) : 0, 9); //Grab a random group of 9 ideas
        //        List<Enums.SavingsIdea> savingsIdeas = new List<Enums.SavingsIdea>();
        //        for (int id = 0; id < 9; id++)
        //        {
        //            int addmonth = id < 3 ? -3 : 0; //Ideas 0-2 will be -3mon, 3-5 currentmon, 6-8 +3mon
        //            if (id > 5) addmonth = 3;
        //            savingsIdeas.Add(new Enums.SavingsIdea()
        //                {
        //                    IdeaId = pageIdeas[id].IdeaNumber,
        //                    QualifiedIdeaId = int.Parse(pageIdeas[id].IdeaName.Element["href"].Split('/').Last()),
        //                    Implemented = DateTime.Now.AddMonths(addmonth),
        //                    Discontinued = DateTime.Now.AddMonths(addmonth + 1),
        //                    FacilitySavings = ((id % 2) == 0),
        //                    Savings = (decimal)rnd.Next(10, 10000),
        //                    ShowNegative = rnd.NextDouble() > .5
        //                });
        //        }
        //        foreach (IGrouping<string, Enums.SavingsIdea> grouping in savingsIdeas.GroupBy(g => g.Implemented.Value.ToString("y")))
        //        {
        //            //Implement ideas for specified month
        //            publishedIdeas.ShowBulkEdit();
        //            WriteReport(string.Format("Setting Implemented date for ideas {0} to {1}", string.Join(", ", grouping.Select(b => b.IdeaId.ToString())), grouping.Key));
        //            foreach (var savingsIdea in grouping)
        //            {
        //                var selectIdea = pageIdeas.First(b => b.IdeaNumber == savingsIdea.IdeaId);
        //                selectIdea.IdeaName.Element.Hover();
        //                selectIdea.ImplementSelect.Element.Hover();
        //                swHome.SetCheckBox(selectIdea.ImplementSelect, true);
        //            }
        //            publishedIdeas.BulkEditImplementDropdown.SelectListOptionByText(Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.Implemented)).Key); //Select 'Implement"
        //            publishedIdeas.BulkEditImplementDateDropdown.SelectListOptionByText(grouping.Key);
        //            publishedIdeas.BulkEditApplyCuttonClick(); //Apply implementation
        //            HpgAssert.Contains(publishedIdeas.BulkEditSuccessMessage.Text, "Successfully", "Verify Bulk Edit was successful");
        //            //Discontinue ideas for next month
        //            publishedIdeas.ShowBulkEdit();
        //            foreach (var savingsIdea in grouping)
        //            {
        //                var selectIdea = pageIdeas.First(b => b.IdeaNumber == savingsIdea.IdeaId);
        //                selectIdea.IdeaName.Element.Hover();
        //                selectIdea.ImplementSelect.Element.Hover();
        //                swHome.SetCheckBox(selectIdea.ImplementSelect, true);
        //            }
        //            publishedIdeas.BulkEditImplementDropdown.SelectListOptionByText(Enums.ImplementedStatusString.First(s => s.Value.Equals(Enums.ImplementedStatus.Discontinued)).Key); //Select 'Discontinue"
        //            publishedIdeas.BulkEditImplementDateDropdown.SelectListOptionByText(grouping.First().Discontinued.Value.ToString("y"));
        //            publishedIdeas.BulkEditApplyCuttonClick(); //Apply implementation
        //            HpgAssert.Contains(publishedIdeas.BulkEditSuccessMessage.Text, "Successfully", "Verify Bulk Edit was successful");
        //        }
        //        #endregion
        //        #region CheckIdeasVia3PV
        //        //Login as Admin
        //        IMHome.browser.Dispose();
        //        IMHome = new page_objects.swHome((BrowserSession)BaseTest.OpenNewBrowser(adminUser.fields["Domain"] + "\\" + adminUser.fields["UserID"], adminUser.fields["Password"], testBrowser.Value));
        //        CurrentBrowser = IMHome.browser;
        //        SessConfiguration.AppHost = BaseURL;
        //        IMHome.GotoDashboard();
        //        IMHome.SelectRole(swHome.Role.Admin);
        //        DBUtility dbUtility = new DBUtility();
        //        string sICOID = randomReadOnlyUser.fields["Facility"].Split('(').Last().Split(')').First();
        //        //Navigate to each of the 9 ideas
        //        foreach (var savingsIdea in savingsIdeas)
        //        {
        //            //TODO: When 3PV implementation allows for any date, remove the below code and use 3PV instead
        //            for (int mon = 1; mon < 13; mon++)
        //            {
        //                //implement the idea for each month of previous year
        //                dbUtility.ImplementQualifiedIdea(savingsIdea.QualifiedIdeaId, sICOID, new DateTime(DateTime.Now.Year - 1, mon, 1), Enums.ImplementedStatus.Implemented, randomReadOnlyUser.fields["Domain"] + "\\" + randomReadOnlyUser.fields["UserID"]);
        //            }
        //            IMHome.GoToPackageIdea(savingsIdea.IdeaId);
        //            im3PV tPv = new im3PV(IMHome.browser);
        //            tPv.UnlockEditIdea();
        //            tPv.ExpandAllButton.Click(1);
        //            tPv.ExpandAllButton.Click();
        //            Dictionary<string, im3PV.IdeaValues> ideaValues = new Dictionary<string, im3PV.IdeaValues>();
        //            ideaValues.Add("HCA", tPv.GetIdeaValues(0));
        //            ideaValues.Add("GPO", tPv.GetIdeaValues(1));
        //            ideaValues.Add("NONGPO", tPv.GetIdeaValues(2));
        //            foreach (KeyValuePair<string, im3PV.IdeaValues> ideaValue in ideaValues)
        //            {
        //                ideaValue.Value.ReportingType.SelectListOptionByText(savingsIdea.FacilitySavings ? "Facility Reporting" : "Corporate Reporting");
        //                swHome.SetCheckBox(ideaValue.Value.IncrementalErosion, savingsIdea.ShowNegative);
        //            }
        //            //Click on facility tab
        //            //Verify correct facility is marked as "Implemented"
        //            tPv.ClickCommit();
        //            System.Threading.Thread.Sleep(2000);
        //        }
        //        #endregion
        //        #region EnterSavingsAsAdmin
        //        IMHome.GotoFacilitySavings();
        //        //go thru last year's months checking monthly totals, and calculate for current year baselines
        //        //Enter savings for same months last year
        //        FacilitySavings.SavingsTab savingsTab = new FacilitySavings.SavingsTab(IMHome.browser);
        //        savingsTab.CoidTextBox.Type(sICOID); //Select readonly user's facility
        //        System.Threading.Thread.Sleep(2000);
        //        savingsTab.CoidTextBox.Element.SendKeys(OpenQA.Selenium.Keys.Enter);
        //        //Previous Year...
        //        foreach (string savingsMonth in savingsTab.SavingsMonthDropDown.OptionsAvailable.Skip(1))
        //        {
        //            savingsTab.SavingsMonthDropDown.SelectListOptionByText(savingsMonth);
        //            savingsTab.SavingsYearDropDown.SelectListOptionByText(DateTime.Now.AddYears(-1).ToString("yyyy"));
        //            savingsTab.SearchSavings.Click();
        //            savingsTab.WaitForThrobber();
        //            foreach (int facilitySavingsId in savingsIdeas.Where(sI => !sI.FacilitySavings).Select(sI => sI.IdeaId).ToArray())
        //            {
        //                savingsTab.WaitForThrobber();
        //                //Idea should be implemented, show on list, and be editable by FacilitySavings role
        //                WriteReport(string.Format("Entering savings for {0}, {1} for idea {2}", savingsMonth, DateTime.Now.AddYears(-1).ToString("yyyy"), facilitySavingsId.ToString()));
        //                decimal savingsValue = (rnd.Next(100, 5000) / 100);
        //                var rowList = savingsTab.SavingsRows;
        //                savingsTab.EnterSavings(rowList.First(sR => sR.IdeaId == facilitySavingsId), savingsValue);
        //                //TODO: Verify tooltip for savings (user, date)
        //            }
        //            List<FacilitySavings.SavingsRow> savingsRows = savingsTab.SavingsRows;
        //            foreach (int facilitySavingsId in savingsIdeas.Where(sI => sI.FacilitySavings).Select(sI => sI.IdeaId).ToArray())
        //            {
        //                //Idea should be implemented, show on list, and be 'Facility Reporting'
        //                HpgAssert.AreEqual(savingsRows.First(sR => sR.IdeaId == facilitySavingsId).Actions.Element.FindXPath(".//img")["ng-tooltip"], "Facility Reported Savings", string.Format("Verify idea {0} is not editable by facility", facilitySavingsId.ToString()));
        //            }
        //            HpgAssert.AreEqual(savingsRows.Where(sR => sR.Gross != null).Select(sR => sR.Gross).Sum().GetValueOrDefault().ToString("C"), savingsTab.TableTotal.Text.Trim(), "Verify table total is correct");
        //        }
        //        //Current Year...
        //        foreach (string savingsMonth in savingsTab.SavingsMonthDropDown.OptionsAvailable.Skip(1))
        //        {
        //            savingsTab.SavingsMonthDropDown.SelectListOptionByText(savingsMonth);
        //            savingsTab.SavingsYearDropDown.SelectListOptionByText(DateTime.Now.ToString("yyyy"));
        //            savingsTab.SearchSavings.Click();
        //            savingsTab.WaitForThrobber();
        //            foreach (Enums.SavingsIdea savingsIdea in savingsIdeas.Where(sI => sI.Implemented.Value.ToString("MMMM").Equals(savingsMonth) && !sI.FacilitySavings && sI.Savings != null))
        //            {
        //                WriteReport(string.Format("Entering corporate savings for {0}, {1} for idea {2}", savingsMonth, DateTime.Now.AddYears(-1).ToString("yyyy"), savingsIdea.IdeaId.ToString()));
        //                savingsTab.EnterSavings(savingsTab.SavingsRows.First(sR => sR.IdeaId == savingsIdea.IdeaId), savingsIdea.Savings.GetValueOrDefault());
        //                //TODO: Verify tooltip for savings (user, date)
        //            }
        //            List<FacilitySavings.SavingsRow> savingsRows = savingsTab.SavingsRows;
        //            if (savingsTab.browser.HasNoContent("No results found. Please modify your search criteria."))
        //            {
        //                //Verify that any ideas that are displayed are not ones marked as discontinued for selected month
        //                foreach (Enums.SavingsIdea savingsIdea in savingsIdeas.Where(sI => sI.Discontinued.Value.ToString("MMMM").Equals(savingsMonth)))
        //                {
        //                    HpgAssert.False(savingsRows.Any(sR => sR.IdeaId.Equals(savingsIdea.IdeaId)), string.Format("Verify 'Discontinued' idea {0} is not in list", savingsIdea.IdeaId.ToString()));
        //                }
        //                HpgAssert.AreEqual(savingsRows.Where(sR => sR.Gross != null).Select(sR => sR.Gross).Sum().GetValueOrDefault().ToString("C"), savingsTab.TableTotal.Text.Trim(), "Verify table total is correct");
        //            }
        //        }
        //        #endregion

        //        #region EnterSavingsAsUser
        //        //Login as previous user
        //        IMHome.browser.Dispose();
        //        IMHome = new page_objects.swHome((BrowserSession)BaseTest.OpenNewBrowser(randomReadOnlyUser.fields["Domain"] + "\\" + randomReadOnlyUser.fields["UserID"], randomReadOnlyUser.fields["Password"], testBrowser.Value));
        //        CurrentBrowser = IMHome.browser;
        //        SessConfiguration.AppHost = BaseURL;
        //        IMHome.GotoDashboard();
        //        IMHome.SelectRole(swHome.Role.FacilitySavings);
        //        IMHome.GotoFacilitySavings();
        //        //go thru last year's months checking monthly totals, and calculate for current year baselines
        //        //Enter savings for same months last year
        //        savingsTab = new FacilitySavings.SavingsTab(IMHome.browser);
        //        //Previous Year...
        //        foreach (string savingsMonth in savingsTab.SavingsMonthDropDown.OptionsAvailable.Skip(1))
        //        {
        //            savingsTab.SavingsMonthDropDown.SelectListOptionByText(savingsMonth);
        //            savingsTab.SavingsYearDropDown.SelectListOptionByText(DateTime.Now.AddYears(-1).ToString("yyyy"));
        //            savingsTab.SearchSavings.Click();
        //            savingsTab.WaitForThrobber();
        //            foreach (int facilitySavingsId in savingsIdeas.Where(sI => sI.FacilitySavings).Select(sI => sI.IdeaId).ToArray())
        //            {
        //                //Idea should be implemented, show on list, and be editable by FacilitySavings role
        //                WriteReport(string.Format("Entering savings for {0}, {1} for idea {2}", savingsMonth, DateTime.Now.AddYears(-1).ToString("yyyy"), facilitySavingsId.ToString()));
        //                decimal savingsValue = (rnd.Next(100, 5000) / 100);
        //                savingsTab.EnterSavings(savingsTab.SavingsRows.First(sR => sR.IdeaId == facilitySavingsId), savingsValue);
        //                //TODO: Verify tooltip for savings (user, date)
        //            }
        //            List<FacilitySavings.SavingsRow>  savingsRows = savingsTab.SavingsRows;
        //            foreach (int facilitySavingsId in savingsIdeas.Where(sI => !sI.FacilitySavings).Select(sI => sI.IdeaId).ToArray())
        //            {
        //                //Idea should be implemented, show on list, and be 'Corporate/Admin Reporting'
        //                HpgAssert.AreEqual(savingsRows.First(sR => sR.IdeaId == facilitySavingsId).Actions.Element.FindXPath(".//img")["ng-tooltip"], "Admin Reported Savings", string.Format("Verify idea {0} is not editable by facility", facilitySavingsId.ToString()));
        //            }
        //            foreach (var savingsRow in savingsRows.Where(sR => sR.Gross != null))
        //            {
        //                DataRow dataRow = savingsTable.NewRow();
        //                dataRow["IdeaId"] = savingsRow.IdeaId;
        //                dataRow["Year"] = DateTime.Now.Year - 1;
        //                dataRow["Month"] = savingsMonth;
        //                dataRow["Value"] = savingsRow.Gross;
        //                savingsTable.Rows.Add(dataRow);
        //            }
        //            HpgAssert.AreEqual(savingsRows.Where(sR => sR.Gross != null).Select(sR => sR.Gross).Sum().GetValueOrDefault().ToString("C"), savingsTab.TableTotal.Text.Trim(), "Verify table total is correct");
        //        }
        //        //TODO: When previous year(s) YTD calculations are fixed, uncomment below code to verify
        //        //HpgAssert.AreEqual(
        //        //    savingsTable.Compute("SUM(Value)", "Year = " + (DateTime.Now.Year - 1).ToString()).ToString(),
        //        //    savingsTab.TotalYearToDate.Text.Replace("$", "").Replace(",", ""), "Verify YTD total is correct");
        //        //Current Year...
        //        foreach (string savingsMonth in savingsTab.SavingsMonthDropDown.OptionsAvailable.Skip(1))
        //        {
        //            savingsTab.SavingsMonthDropDown.SelectListOptionByText(savingsMonth);
        //            savingsTab.SavingsYearDropDown.SelectListOptionByText(DateTime.Now.ToString("yyyy"));
        //            savingsTab.SearchSavings.Click();
        //            savingsTab.WaitForThrobber();
        //            List<FacilitySavings.SavingsRow> adminRows = savingsTab.SavingsRows;
        //            foreach (Enums.SavingsIdea savingsIdea in savingsIdeas.Where(sI => sI.Implemented.Value.ToString("MMMM").Equals(savingsMonth) && !sI.FacilitySavings))
        //            {
        //                //verify all the admin reporting ideas are marked as such
        //                HpgAssert.AreEqual(savingsTab.SavingsRows.First(sR => sR.IdeaId == savingsIdea.IdeaId).Actions.Element.FindXPath(".//img")["ng-tooltip"], "Admin Reported Savings", string.Format("Verify idea {0} is not editable by facility", savingsIdea.IdeaId.ToString()));
        //            }
        //            foreach (Enums.SavingsIdea savingsIdea in savingsIdeas.Where(sI => sI.Implemented.Value.ToString("MMMM").Equals(savingsMonth) && sI.FacilitySavings && sI.Savings != null))
        //            {
        //                WriteReport(string.Format("Entering savings for {0}, {1} for idea {2}", savingsMonth, DateTime.Now.AddYears(-1).ToString("yyyy"), savingsIdea.IdeaId.ToString()));
        //                //TODO: Verify tooltip for savings (user, date)
        //            }
        //            List<FacilitySavings.SavingsRowBase> savingsRows = savingsTab.SavingsRowBases;
        //            if (savingsTab.browser.HasNoContent("No results found. Please modify your search criteria."))
        //            {
        //                //Verify that any ideas that are displayed are not ones marked as discontinued for selected month
        //                foreach (Enums.SavingsIdea savingsIdea in savingsIdeas.Where(sI => sI.Discontinued.Value.ToString("MMMM").Equals(savingsMonth)))
        //                {
        //                    HpgAssert.False(savingsRows.Any(sR => sR.IdeaId.Equals(savingsIdea.IdeaId)), string.Format("Verify 'Discontinued' idea {0} is not in list", savingsIdea.IdeaId.ToString()));
        //                }
        //            }
        //            if (savingsRows.Any())
        //            {
        //                foreach (var savingsRow in savingsRows.Where(sR => sR.Gross != null))
        //                {
        //                    DataRow dataRow = savingsTable.NewRow();
        //                    dataRow["IdeaId"] = savingsRow.IdeaId;
        //                    dataRow["Year"] = DateTime.Now.Year;
        //                    dataRow["Month"] = savingsMonth;
        //                    dataRow["Value"] = savingsRow.Gross;
        //                    savingsTable.Rows.Add(dataRow);
        //                }
        //                HpgAssert.AreEqual(savingsRows.Where(sR => sR.Gross != null).Select(sR => sR.Gross).Sum().GetValueOrDefault().ToString("C"), savingsTab.TableTotal.Text.Trim(), "Verify table total is correct");
        //            }
        //        }
        //        //TODO: When previous year(s) YTD calculations are fixed, uncomment below code to verify
        //        //HpgAssert.AreEqual(
        //        //    savingsTable.Compute("SUM(Value)", "Year = " + DateTime.Now.Year.ToString()).ToString(),
        //        //    savingsTab.TotalYearToDate.Text.Replace("$", "").Replace(",", ""), "Verify YTD total is correct");
        //        #endregion

        //        #region BaselineCalculations
        //        FacilitySavings.BaselineTab baselineTab = new FacilitySavings.BaselineTab(IMHome.browser);
        //        baselineTab.SavingsYearDropDown.SelectListOptionByText(DateTime.Now.Year.ToString());
        //        baselineTab.SearchSavings.Click(3);
        //        List<FacilitySavings.BaselineRow> baselines = baselineTab.BaselineRows;
        //        foreach (FacilitySavings.BaselineRow row in baselines)
        //        {
        //            //var bla = savingsTable.AsEnumerable().Where(r => r.Field<int>("Year") == DateTime.Now.AddYears(-1) && r.Field<int>("IdeaId") == row.IdeaId).Average(r => r.Field<double>("Value"));
        //            HpgAssert.AreEqual(
        //                savingsTable.Compute("AVG(Value)", "Year = " + DateTime.Now.AddYears(-1).Year.ToString() + " AND IdeaId = " + row.IdeaId.ToString()).ToString(),
        //                row.Baseline.ToString(),
        //                string.Format("Verify baseline for current year for {0} is correct", row.IdeaId.ToString()));
        //        }
        //        #endregion

        //        #region IncrementalSavingsCalculations
        //        FacilitySavings.IncrementalTab incrementalTab = new FacilitySavings.IncrementalTab(IMHome.browser);
        //        foreach (string savingsMonth in incrementalTab.SavingsMonthDropDown.OptionsAvailable.Skip(1))
        //        {
        //            incrementalTab.SavingsMonthDropDown.SelectListOptionByText(savingsMonth);
        //            incrementalTab.SearchSavings.Click(3);
        //            List<FacilitySavings.IncrementalRow> incrementalRows = incrementalTab.IncrementalRows;
        //            foreach (Enums.SavingsIdea savingsIdea in savingsIdeas)
        //            {
        //                FacilitySavings.IncrementalRow incrementalRow = incrementalRows.First(iR => iR.IdeaId == savingsIdea.IdeaId);
        //                FacilitySavings.BaselineRow baselineRow = baselines.First(bR => bR.IdeaId == savingsIdea.IdeaId);
        //                HpgAssert.AreEqual(baselineRow.Baseline.ToString(), incrementalRow.Baseline.ToString(), "Verify baseline for idea " + savingsIdea.IdeaId.ToString());
        //                if (savingsIdea.ShowNegative)
        //                {
        //                    HpgAssert.AreEqual((savingsIdea.Savings - incrementalRow.Incremental).ToString(), incrementalRow.Incremental.ToString(), "Verify incremental for idea (displays negative) " + savingsIdea.IdeaId.ToString());
        //                }
        //                else
        //                {
        //                    if ((savingsIdea.Savings - incrementalRow.Incremental) >= 0)
        //                    {
        //                        HpgAssert.AreEqual((savingsIdea.Savings - incrementalRow.Incremental).ToString(), incrementalRow.Incremental.ToString(), "Verify incremental for idea (does not display negative) " + savingsIdea.IdeaId.ToString());
        //                    }
        //                    else
        //                    {
        //                        HpgAssert.AreEqual("0", incrementalRow.Incremental.ToString(), "Verify incremental for idea (does not display negative) " + savingsIdea.IdeaId.ToString());
        //                    }
        //                }
        //            }
        //        }
        //        #endregion

        //        #endregion

        //    }
        //}





        #region extraFunctionsTests
        //--------------------------------------------------------------------------------------------------------------------------------------
        //---------------- ALL BELOW THIS LINE ARE FOR FUNTIONAL TESTING OF NON-APPLICATION FEATURES SUCH AS BASIC AUTH, ETC... ----------------
        //--------------------------------------------------------------------------------------------------------------------------------------


        [Test]
        [Ignore("reason")]
        public void X_BasicAuthTest()
        {
            foreach (KeyValuePair<string, Browser> testBrowser in ffOnly)
            {
                WriteReport("~*~*~ Begin test with " + testBrowser.Key + " ~*~*~");
                DisposeBrowsers();
                
                page_objects.swHome IMHome = new page_objects.swHome((BrowserSession) BaseTest.OpenNewBrowser("DOMAIN\\USERNAME", "T3st5678", testBrowser.Value));
                CurrentBrowser = IMHome.browser;
                //IMHome.GotoHomePage();
                IMHome.GotoDashboard();
                //JS to clear authentication
                //document.execCommand('ClearAuthenticationCache', 'false');
                //along with clearing all cookies (FF/Chrome)
            }
        }



        [Test][Ignore("reason")]
        public void X_CreateIdeasInEachStatus()
        {
            DBUtility dbUtility = new DBUtility();
            List<int> createdIdeas = new List<int>();


            int numberOfIdeasEach = 5;
            List<DBUtility.IdeaStatus> statuses = new List<DBUtility.IdeaStatus>() { DBUtility.IdeaStatus.Submitted, DBUtility.IdeaStatus.Declined, DBUtility.IdeaStatus.UnderReview};
            List<DBUtility.WorkflowStep> workflowSteps = new List<DBUtility.WorkflowStep>() { DBUtility.WorkflowStep.User, DBUtility.WorkflowStep.Boss, DBUtility.WorkflowStep.SME, DBUtility.WorkflowStep.Admin};
            string userName = @"HCADEV\FSA9091";
            string assignedTo = @"HCADEV\FSA9091";
            
            string nameSuffix = DateTime.Now.ToString("yyyyMMddHHmmss");

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

        [Test]
        //[Ignore("reason")]
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
        #endregion



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
            SessConfiguration.Browser = ffChromeOnly["Chrome"];
            SessConfiguration.AppHost = BaseURL;
            BaseTest.KillProcess("EXCEL");
            DisposeBrowsers();
            RallyBuild = DateTime.Now.ToString("yyyyMMddHHmmss");
            SessConfiguration.Match = Match.First;
            testUsers = FileReader.getInputObjects("global\\Main.xls", "TestUsers").Where(u => !string.IsNullOrEmpty(u.fields["UserID"].Trim()));
        }

        [TearDown]
        public void TearDown()
        {
            if (!RallyUpload)
            {
                RallyTestID = "";
            }
            if (TestContext.CurrentContext.Result.Outcome.Status.Equals(TestStatus.Passed))
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

        public Dictionary<string, string> GetRMIAttachments(string prefix = "")
        {
            return GetAttachmentsAtPath(@"global\Attachments\RMI\", prefix);
        }

        public Dictionary<string, string> GetAttachmentsAtPath(string path, string prefix = "")
        {
            ScreenCapture screenCapture = new ScreenCapture();
            screenCapture.CaptureScreenToFile(Environment.CurrentDirectory + Constants.InputDataPath + path + "CurrentScreenShot.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            return (from f in
                        System.IO.Directory.GetFiles(Environment.CurrentDirectory + Constants.InputDataPath +
                                                     path)
                    select
                        new
                            {
                                k = prefix + f.Split('\\').Last(),
                                v = f
                            }).ToDictionary(p => p.k, p => p.v);
            
        }

        public Dictionary<string, string> GetGoodAttachments()
        {
            return GetAttachmentsAtPath(@"global\Attachments\Good\");
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