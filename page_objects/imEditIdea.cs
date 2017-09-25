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

namespace Streetwise.page_objects
{
    class imEditIdea : swMaster
    {
        public imEditIdea(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        #region Objects

        public HpgElement QuestionsTab
        {
            get
            {
                return new HpgElement(browser.FindId("tabQuestions"));
            }
        }

        public HpgElement CommentsTab
        {
            get
            {
                return new HpgElement(browser.FindId("tabComments"));
            }
        }

        public HpgElement AdditionalInfoTab
        {
            get
            {
                return new HpgElement(browser.FindId("tabAdditionalInfo"));
            }
        }

        public HpgElement SubmitSaveButton
        {
            get
            {
                return  new HpgElement(browser.FindButton("Save"));
            }
        }

        public  HpgElement SubmitSubmitButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Submit"));
            }
        }

        public HpgElement EditResubmit
        {
            get
            {
                return new HpgElement(browser.FindButton("Resubmit"));
            }
        }

        public HpgElement SubmitCancelButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Cancel"));
            }
        }

        public HpgElement SubmitAttachmentButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Attachment"));
            }
        }

        public HpgElement IdeaID
        {
            get
            {
                return new HpgElement(browser.FindId("Idea_IdeaId"));
            }
        }

        public HpgElement IdeaName
        {
            get
            {
                return new HpgElement(browser.FindId("Idea_Title"));
            }
        }

        public HpgElement IdeaDescription
        {
            get
            {
                return new HpgElement(browser.FindId("mainContent").FindXPath("p"));
            }
        }

        public HpgElement BackButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Back"));
            }
        }

        public HpgElement RMIDiv
        {
            get
            {
                return new HpgElement(browser.FindId("additionalInfoTab"));
            }
        }

        public HpgElement RMIRequestFromDropDown
        {
            get
            {
                return new HpgElement(RMIDiv.Element.FindXPath("//select[@name='responseUsers']"));
            }
        }

        public HpgElement RMIRequestTextArea
        {
            get
            {
                return new HpgElement(RMIDiv.Element.FindXPath("//textarea[@name='Request']"));
            }
        }

        public HpgElement RMIPostButton
        {
            get
            {
                return new HpgElement(RMIDiv.Element.FindButton("Post"));
            }
        }

        public HpgElement RMIForm
        {
            get
            {
                return new HpgElement(browser.FindXPath("//form[@name='responseForm']"));
            }
        }

        public HpgElement RMIResponseText
        {
            get
            {
                return new HpgElement(RMIForm.Element.FindField("Response"));
            }
        }

        public HpgElement RMIAddLinks
        {
            get
            {
                return new HpgElement(RMIForm.Element.FindButton("+Add Links"));
            }
        }

        public HpgElement RMIAddAttachments
        {
            get
            {
                return new HpgElement(RMIForm.Element.FindButton("+Add Attachments"));
            }
        }

        public HpgElement AddLinksDialog
        {
            get
            {
                return new HpgElement(browser.FindId("editLinksDialog2_RequestMoreInformationController"));
            }
        }

        public HpgElement AddLinksDialogAddLinkButton
        {
            get
            {
                return new HpgElement(AddLinksDialog.Element.FindButton("Add Link"));
            }
        }

        public HpgElement AddLinksDialogSaveButton
        {
            get
            {
                return new HpgElement(AddLinksDialog.Element.FindButton("Save"));
            }
        }

        public HpgElement AddLinksDialogCancelButton
        {
            get
            {
                return new HpgElement(AddLinksDialog.Element.FindButton("Cancel"));
            }
        }

        public List<HpgElement> AllLinksTitle
        {
            get
            {
                return
                    (from a in AddLinksDialog.Element.FindAllXPath(".//input[@ng-model='link.description']")
                     select new HpgElement(a)).ToList();
            }
        }

        public List<HpgElement> AllLinksURL
        {
            get
            {
                return
                    (from a in AddLinksDialog.Element.FindAllXPath(".//input[@ng-model='link.url']")
                     select new HpgElement(a)).ToList();
            }
        }

        public List<HpgElement> AllLinksDelete
        {
            get
            {
                return
                    (from a in AddLinksDialog.Element.FindAllXPath(".//a[@title='Delete Link']")
                     select new HpgElement(a)).ToList();
            }
        }

        public HpgElement AddAttachmentsDialog
        {
            get
            {
                return new HpgElement(browser.FindId("editAttachmentsDialog2_RequestMoreInformationController"));
            }
        }

        public HpgElement AddAttachmentsDialogAddAttachmentButton
        {
            get
            {
                return new HpgElement(AddAttachmentsDialog.Element.FindButton("Add Attachment"));
            }
        }

        public HpgElement AddAttachmentsDialogSaveButton
        {
            get
            {
                return new HpgElement(AddAttachmentsDialog.Element.FindButton("Save"));
            }
        }

        public HpgElement AddAttachmentsDialogCancelButton
        {
            get
            {
                return new HpgElement(AddAttachmentsDialog.Element.FindButton("Cancel"));
            }
        }

        public List<HpgElement> AllAttachmentsTitle
        {
            get
            {
                return
                    (from a in AddAttachmentsDialog.Element.FindAllXPath(".//input[@ng-model='attachment.description']")
                     select new HpgElement(a)).ToList();
            }
        }

        public List<HpgElement> AllAttachmentsFile
        {
            get
            {
                return
                    (from a in AddAttachmentsDialog.Element.FindAllXPath(".//input[@type='file']")
                     select new HpgElement(a)).ToList();
            }
        }

        public List<HpgElement> AllAttachmentsDelete
        {
            get
            {
                return
                    (from a in AddAttachmentsDialog.Element.FindAllXPath(".//a[@title='Delete Attachment']")
                     select new HpgElement(a)).ToList();
            }
        }

        public class RMIMessage
        {
            public ElementScope element;

            public RMIMessage(ElementScope rmiMessage)
            {
                element = rmiMessage;
            }

            public string user
            {
                get { return element.FindXPath(".//div/div[1]/h4").Text; }
            }

            public string message
            {
                get { return element.FindXPath(".//p").Text; }
            }

            public DateTime submittedDate
            {
                get { return DateTime.Parse(element.FindXPath(".//div[contains(@class, 'text-right')]").Text); }
            }

            public List<HpgElement> links
            {
                get
                {
                    element.FindXPath(".//span[contains(@ng-repeat, 'response.links')]",
                                      new Options() {Match = Match.First}).Now();
                    return
                        (from l in element.FindAllXPath(".//span[contains(@ng-repeat, 'response.links')]")
                         select new HpgElement(l)).ToList();
                }
            }

            public List<HpgElement> attachments
            {
                get
                {
                    element.FindXPath(".//span[contains(@ng-repeat, 'response.attachments')]",
                                      new Options() {Match = Match.First}).Now();
                    return
                        (from l in element.FindAllXPath(".//span[contains(@ng-repeat, 'response.attachments')]")
                         select new HpgElement(l)).ToList();
                }
            }
        }

        public List<RMIMessage> RMIMessages
        {
            get
            {
                return
                    (from m in browser.FindAllXPath("//div[contains(@ng-class, 'createdByMe')]")
                     select new RMIMessage(m)).ToList();
            }
        }

        #endregion

        #region Actions

        public void ShowAdditionalInfoTab()
        {
            for (int i = 0; i < 5; i++)
            {
                if (RMIDiv.Element.Exists(new Options() { Timeout = TimeSpan.FromSeconds(3) }))
                    break;
                AdditionalInfoTab.Click(2);
            }
            HpgAssert.True(RMIDiv.Element.Exists(), "Additional Information (RMI) tab is shown");
        }

        public void ShowRMIAddLinks()
        {
            for (int i = 0; i < 5; i++)
            {
                if (AddLinksDialog.Element.Exists(new Options() { Timeout = TimeSpan.FromSeconds(3) })) break;
                RMIAddLinks.Click(2);
            }
            HpgAssert.True(AddLinksDialog.Element.Exists(), "Request More Info - Add Links dialog is present");
        }

        public void ShowRMIAddAttachments()
        {
            for (int i = 0; i < 5; i++)
            {
                if (AddAttachmentsDialog.Element.Exists(new Options(){Timeout = TimeSpan.FromSeconds(3)})) break;
                RMIAddAttachments.Click(2);
            }
            HpgAssert.True(AddAttachmentsDialog.Element.Exists(), "Request More Info - Add Attachments dialog is present");
        }

        public void RMISaveAttachmentsAndClose()
        {
            AddAttachmentsDialogSaveButton.Click(2);
            for (int i = 0; i < 5; i++)
            {
                if (AddAttachmentsDialog.Element.Missing(new Options(){Timeout = TimeSpan.FromSeconds(3)})) break;
                AddAttachmentsDialogSaveButton.Click(2);
            }
            HpgAssert.True(AddAttachmentsDialog.Element.Missing(), "Request More Info - Attachments are saved and dialog is no longer present");
            
        }

        public void AddAttachment(string name, string filename)
        {
            AddAttachmentsDialogAddAttachmentButton.Click();
            AllAttachmentsTitle.Last().Type(name);
            AllAttachmentsFile.Last().Element.SendKeys(filename);
        }

        public void AddAttachments(Dictionary<string, string> attachments)
        {
            foreach (KeyValuePair<string, string> attachment in attachments)
            {
                AddAttachment(attachment.Key, attachment.Value);
            }
        }

        public void AddLink(string name, string url)
        {
            AddLinksDialogAddLinkButton.Click(2);
            AllLinksTitle.Last().Type(name);
            AllLinksURL.Last().Type(url);
        }

        public void AddLinks(Dictionary<string, string> links, bool openCloseDialog = false)
        {
            if(openCloseDialog) ShowRMIAddLinks();
            foreach (KeyValuePair<string, string> link in links)
            {
                AddLink(link.Key, link.Value);
            }
            if (openCloseDialog)
            {
                AddLinksDialogSaveButton.Click(2);
                for (int i = 0; i < 5; i++)
                {
                    if (AddLinksDialog.Element.Missing(new Options() {Timeout = TimeSpan.FromSeconds(3)})) break;
                    AddLinksDialogSaveButton.Click(2);
                }
                HpgAssert.True(AddLinksDialog.Element.Missing(), "RMI Add Links dialog is no longer present");
            }
        }

        #endregion
    }
}
