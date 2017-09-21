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

namespace IdeaManagement.page_objects
{
    class DCRD : imEditIdea
    {
        public DCRD(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        #region Objects


        public HpgElement DeclineButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Decline"));
            }
        }

        public HpgElement SubmitButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Submit"));
            }
        }

        public HpgElement DeclineComment
        {
            get
            {
                return new HpgElement(browser.FindId("editComment").FindId("UpdatedComment_Comment"));
            }
        }

        public HpgElement DeclineCommentSaveButton
        {
            get
            {
                //return new HpgElement(browser.FindId("editCommentDialog").FindButton("Save"));
                return new HpgElement(browser.FindXPath("//*[@id='editCommentDialog']//button[.='Decline']"));
            }
        }

        public HpgElement AssignToSMESubmit
        {
            get
            {
                return new HpgElement(AssignToSMEDialog.Element.FindButton("Submit"));
            }
        }

        public HpgElement AssignToSMECancel
        {
            get
            {
                return new HpgElement(AssignToSMEDialog.Element.FindButton("Cancel"));
            }
        }

        public  HpgElement SMEDropDown
        {
            get
            {
                return new HpgElement(AssignToSMEDialog.Element.FindId("AssignedSME"));
            }
        }

        public HpgElement AssignToSMEDialog
        {
            get
            {
                return new HpgElement(browser.FindId("assignSmeDialog"));
            }
        }

        #endregion

        public void SubmitToSME(string smeName)
        {
            for (int i = 0; i < 5; i++)
            {
                if (AssignToSMEDialog.Element.Exists(new Options() {Timeout = TimeSpan.FromSeconds(3)})) break;
                SubmitButton.Click(2);
            }
            HpgAssert.True(AssignToSMEDialog.Element.Exists(), "Assign To SME dialog is present");
            SMEDropDown.SelectListOptionByText(smeName);
            AssignToSMESubmit.Click(3);
            for (int i = 0; i < 5; i++)
            {
                if (AssignToSMEDialog.Element.Missing(new Options() { Timeout = TimeSpan.FromSeconds(3) })) break;
                AssignToSMESubmit.Click(3);
            }
            HpgAssert.True(AssignToSMEDialog.Element.Missing(new Options() { Timeout = TimeSpan.FromSeconds(3) }), "Assign to SME dialog is no longer present");
        }
    }
}
