using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coypu;
using AutomationCore;
using AutomationCore.utility;
using System.Windows.Forms;
using Streetwise.Utility;
using OpenQA.Selenium.IE;
using System.Xml;
using System.Xml.Linq;

namespace Streetwise.page_objects
{
    class swSubmitAnIdea : swMaster
    {
        public swSubmitAnIdea(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        public class AddAttachmentsDialog
        {
            private ElementScope _dialog;
            public AddAttachmentsDialog(ElementScope dialog)
            {
                _dialog = dialog;
            }

            public class AttachmentRow
            {
                public HpgElement Title;
                public HpgElement LocalFile;
                public HpgElement Delete;
            }

            public HpgElement SaveButton
            {
                get
                {
                    return new HpgElement(_dialog.FindButton("Save"));
                }
            }

            public HpgElement CancelButton
            {
                get
                {
                    return new HpgElement(_dialog.FindButton("Cancel"));
                }
            }

            public HpgElement AddAttachmentButton
            {
                get
                {
                    return new HpgElement(_dialog.FindButton("Add Attachment"));
                }
            }

            /// <summary>
            /// Gets a single attachment row as an object
            /// </summary>
            /// <param name="rowNumber">1-based row number on the dialog</param>
            /// <returns>Custom class containing Title text box, LocalFile Input, and Delete link</returns>
            public AttachmentRow GetAttachmentRow(int rowNumber)
            {
                return GetAllAttachmentRow().ElementAt(rowNumber - 1);
            }

            /// <summary>
            /// Returns a list of attachment rows
            /// </summary>
            /// <returns>List of custom class containing Title text box, LocalFile Input, and Delete link</returns>
            public List<AttachmentRow> GetAllAttachmentRow()
            {
                _dialog.FindXPath(".//tbody/tr").Now();
                return (from row in _dialog.FindAllXPath(".//tbody/tr")
                        select new AttachmentRow()
                            {
                                Title = new HpgElement(row.FindXPath(".//input[@ng-model='attachment.description']")),
                                LocalFile = new HpgElement(row.FindXPath(".//input[@type='file']")),
                                Delete = new HpgElement(row.FindXPath(".//a[@title='Delete Attachment']"))
                            }).ToList();
            }
        }

        public class AddLinksDialog
        {
            private ElementScope _dialog;
            public AddLinksDialog(ElementScope dialog)
            {
                _dialog = dialog;
            }

            public class LinkRow
            {
                public HpgElement Title;
                public HpgElement Url;
                public HpgElement Delete;
            }

            public HpgElement SaveButton
            {
                get
                {
                    return new HpgElement(_dialog.FindButton("Save"));
                }
            }

            public HpgElement CancelButton
            {
                get
                {
                    return new HpgElement(_dialog.FindButton("Cancel"));
                }
            }

            public HpgElement AddLinkButton
            {
                get
                {
                    return new HpgElement(_dialog.FindButton("Add Link"));
                }
            }

            /// <summary>
            /// Gets a single link row as an object
            /// </summary>
            /// <param name="rowNumber">1-based row number on the dialog</param>
            /// <returns>Custom class containing Title text box, URL Input, and Delete link</returns>
            public LinkRow GetLinkRow(int rowNumber)
            {
                return GetAllAttachmentRow().ElementAt(rowNumber - 1);
            }

            /// <summary>
            /// Returns a list of link rows
            /// </summary>
            /// <returns>List of custom class containing Title text box, URL Input, and Delete link</returns>
            public List<LinkRow> GetAllAttachmentRow()
            {
                _dialog.FindXPath(".//tbody/tr").Now();
                return (from row in _dialog.FindAllXPath(".//tbody/tr")
                        select new LinkRow()
                            {
                                Title = new HpgElement(row.FindXPath(".//input[@name='description']")),
                                Url = new HpgElement(row.FindXPath(".//input[@name='url']")),
                                Delete = new HpgElement(row.FindXPath("//a[@title='Delete Link']"))
                            }).ToList();
            }
        }

        #region Objects

        public ElementScope AttachmentsDialog
        {
            get { return browser.FindId("editAttachmentsDialog2"); }
        }

        public ElementScope LinksDialog
        {
            get { return browser.FindId("editLinksDialog2"); }
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
                return new HpgElement(browser.FindId("Idea_Description"));
            }
        }

        public HpgElement AddLinksButton
        {
            get
            {
                return new HpgElement(browser.FindCss("#ideaSubmit > div:nth-child(1) > div:nth-child(1) > div.row > div:nth-child(1) > button"));
            }
        }

        public HpgElement AddAttachmentsButton
        {
            get
            {
                return new HpgElement(browser.FindCss("#ideaSubmit > div:nth-child(1) > div:nth-child(1) > div.row > div:nth-child(2) > button"));
            }
        }

        public HpgElement IdeaQuestionAndResponses1
        {
            get
            {
                return new HpgElement(browser.FindId("IdeaQuestionAndResponses_1"));
            }
        }

        public HpgElement IdeaQuestionAndResponses2
        {
            get
            {
                return new HpgElement(browser.FindId("IdeaQuestionAndResponses_2"));
            }
        }

        public HpgElement IdeaQuestionsAndResponses3
        {
            get
            {
                return new HpgElement(browser.FindId("IdeaQuestionAndResponses_3"));
            }
        }

        public HpgElement IdeaQuestionsAndResponses4
        {
            get
            {
                return new HpgElement(browser.FindId("IdeaQuestionAndResponses_4"));
            }
        }

        #endregion

        #region Actions

        /// <summary>
        /// Fills out all questions with text
        /// </summary>
        /// <param name="suffix">suffix text added to the end of the question answers</param>
        public void FillOutQuestions(string suffix = "")
        {
            foreach (AngularElements.TextBox question in GetAllQuestions())
            {
                if (question.IsRequired)
                {
                    question.Type("This question is a required question so here's an answer!" + suffix);
                }
                else
                {
                    question.Type("This question is not required, so I'm not gonna answer it!" + suffix);
                }
            }
        }

        /// <summary>
        /// Clicks submit, verifys success dialog, accepts success dialog.
        /// </summary>
        public void SubmitIdea()
        {
            SubmitSubmitButton.Click();
            //HpgAssert.True(browser.HasContent("Your Idea was submitted successfully.", new Options() { Timeout = TimeSpan.FromMinutes(2) }), "Verify success message is present");
            HpgAssert.AreEqual("submitted", browser.FindId("status").Text.Trim().ToLower(), "Verify idea was submitted successfully");
            //browser.AcceptModalDialog();
        }

        /// <summary>
        /// Returns Utility.AngularElements.TextBox for the text box of the question who's text matches supplied string
        /// </summary>
        /// <param name="question">The text in the question</param>
        /// <returns>Utility.AngularElements.TextBox</returns>
        public Utility.AngularElements.TextBox GetQuestion(string question)
        {
            return new Utility.AngularElements.TextBox(browser.FindXPath(string.Format("//div[label[contains(normalize-space(), '{0}') and substring-after(normalize-space(), '{0}') = '']]//textarea", question)));
        }

        /// <summary>
        /// Returns Utility.AngularElements.TextBox for the text box of the question who's index is supplied int
        /// </summary> 
        /// <param name="question">The 0-based index of the question</param>
        /// <returns>Utility.AngularElements.TextBox</returns>
        public Utility.AngularElements.TextBox GetQuestion(int question)
        {
            return new Utility.AngularElements.TextBox(browser.FindId(string.Format("IdeaQuestionAndResponses_{0}", question.ToString())));
        }

        /// <summary>
        /// Returns list of Utility.AngularElements.TextBox for all questions on the page
        /// </summary>
        /// <returns>List of Utility.AngularElements.TextBox</returns>
        public List<Utility.AngularElements.TextBox> GetAllQuestions()
        {
            browser.FindXPath("//div[@ng-form='questionForm']/textarea").Now();
            return (from q in browser.FindAllXPath("//div[@ng-form='questionForm']/textarea") select new Utility.AngularElements.TextBox(q)).ToList();
        }

        #endregion
    }
}
