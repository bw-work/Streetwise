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
    class EditQuestions : swMaster
    {
        public EditQuestions(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        #region customClasses

        public class Question
        {
            public string Name;
            public bool Required;
        }

        public class DisplayQuestion : Question
        {
            public DateTime CreatedDate;
            public HpgElement EditButton;
            public HpgElement DeleteButton;
            public HpgElement UndoDelete;
        }

        #endregion

        #region objects

        public HpgElement NewQuestionText
        {
            get
            {
                return new HpgElement(browser.FindCss("#ng-app > div > div > div:nth-child(3) > div.span3 > div > div > form > div:nth-child(3) > div > textarea"));
            }
        }

        public HpgElement NewQuestionRequiredCheckbox
        {
            get
            {
                return new HpgElement(browser.FindId("isRequired"));
            }
        }

        public HpgElement NewQuestionSubmitButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Create"));
            }
        }

        public HpgElement ShowDeleted
        {
            get
            {
                return new HpgElement(browser.FindCss("#ng-app > div > div > div:nth-child(2) > div > div > label > input"));
            }
        }

        public HpgElement ConfirmDeleteDelete
        {
            get
            {
                return new HpgElement(browser.FindId("deleteDialog").FindButton("Delete"));
            }
        }

        public HpgElement ConfirmDeleteCancel
        {
            get
            {
                return new HpgElement(browser.FindId("deleteDialog").FindButton("Cancel"));
            }
        }

        public HpgElement ConfirmUndoDeleteUndo
        {
            get
            {
                return new HpgElement(browser.FindId("undoDeleteDialog").FindButton("Undo Delete"));
            }
        }

        public HpgElement ConfirmUndoDeleteCancel
        {
            get
            {
                return new HpgElement(browser.FindId("undoDeleteDialog").FindButton("Cancel"));
            }
        }

        public class EditForm
        {
            public HpgElement form;
            public EditForm(BrowserSession browser)
            {
                form = new HpgElement(browser.FindCss("#editDialog > form"));
            }

            public HpgElement Question
            {
                get
                {
                    return new HpgElement(form.Element.FindField("question"));
                }
            }

            public HpgElement QuestionRequiredCheckbox
            {
                get
                {
                    return new HpgElement(form.Element.FindField("isRequired"));
                }
            }

            public HpgElement SaveButton
            {
                get
                {
                    return new HpgElement(form.Element.FindButton("Save"));
                }
            }

            public HpgElement CancelButton
            {
                get
                {
                    return new HpgElement(form.Element.FindButton("Cancel"));
                }
            }

            public void EditQuestion(Question question)
            {
                Question.Type(question.Name);
                SetCheckBox(QuestionRequiredCheckbox, question.Required);
                System.Threading.Thread.Sleep(1000);
                SaveButton.Click();
            }
        }

        

        #endregion

        #region Actions

        public List<DisplayQuestion> GetQuestions(bool showDeleted = true)
        {
            browser.FindXPath("//tr[contains(@ng-repeat, 'question')]").Now();
            SetCheckBox(ShowDeleted, showDeleted);
            ScrollToBottom();
            return (from q in browser.FindAllXPath("//tr[contains(@ng-repeat, 'question')]")
                    select new DisplayQuestion()
                        {
                            Name = q.FindXPath("td[1]").Text.Trim(),
                            Required = q.FindXPath("td[2]/input[@ng-checked='question.isRequired']").Selected,
                            CreatedDate = DateTime.Parse(q.FindXPath("td[3]").Text.Trim()),
                            EditButton = new HpgElement(q.FindXPath(".//a[@ng-click='edit(question, questionToFormData)']")),
                            DeleteButton = new HpgElement(q.FindXPath(".//a[@ng-click='doDelete(question)']")),
                            UndoDelete = new HpgElement(q.FindXPath("td[4]").FindLink("Undo Delete"))
                        }).ToList();
        }

        public void SubmitNewQuestion(Question question)
        {
            NewQuestionText.Type(question.Name);
            SetCheckBox(NewQuestionRequiredCheckbox, question.Required);
            System.Threading.Thread.Sleep(1000);
            NewQuestionSubmitButton.Click();
        }

        public string CompareQuestion(Question one, Question two)
        {
            string returnString = "";
            if (one.Name != two.Name) returnString += "\nName does not match (" + one.Name + " != " + two.Name + ")";
            if (one.Required != two.Required) returnString += "\nRequired does not match (" + one.Required.ToString() + " != " + two.Required.ToString() + ")";
            return returnString;
        }

        #endregion
    }
}
