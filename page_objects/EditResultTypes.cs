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
    class EditResultTypes : swMaster
    {
        public EditResultTypes(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        public class ResultType
        {
            public string Name;
            public string UoM;
            public ReportingPeriod ReportingPeriod;
            public bool IsDefault;
        }

        public class DisplayResultType : ResultType
        {
            public HpgElement Edit;
            public HpgElement Delete;
            public HpgElement UndoDelete;
        }

        public enum ReportingPeriod
        {
            None,
            Monthly,
            Quarterly,
            Yearly
        }

        public enum ExpressAs
        {
            None,
            Percentage,
            Rate,
            RatePerUnit
        }

        public enum AppliesTo
        {
            None,
            CommunityIdeas,
            FacilityIdeas,
            Both
        }

        #region Objects

        public HpgElement ShowDeleted
        {
            get
            {
                return new HpgElement(browser.FindCss("#ng-app > div > div > div:nth-child(2) > div > div > label > input"));
            }
        }

        public HpgElement NewName
        {
            get
            {
                return new HpgElement(browser.FindField("name"));
            }
        }

        public HpgElement NewUoM
        {
            get
            {
                return new HpgElement(browser.FindField("unitOfMeasure"));
            }
        }

        public HpgElement NewSummable
        {
            get
            {
                return new HpgElement(browser.FindField("summable"));
            }
        }

        public HpgElement NewDefault
        {
            get
            {
                return new HpgElement(browser.FindId("isDefault"));
            }
        }

        public HpgElement NewIncreasingProgress
        {
            get
            {
                return new HpgElement(browser.FindField("increasing"));
            }
        }

        public HpgElement NewNumerator
        {
            get
            {
                return new HpgElement(browser.FindField("numeratorName"));
            }
        }

        public HpgElement NewDenominator
        {
            get
            {
                return new HpgElement(browser.FindField("denominatorName"));
            }
        }

        public HpgElement NewCaptureBaseline
        {
            get
            {
                return new HpgElement(browser.FindField("baseline"));
            }
        }

        public HpgElement NewReportingPeriod
        {
            get
            {
                return new HpgElement(browser.FindCss("#ng-app > div > div > div:nth-child(3) > div.span3 > div > div > form > div:nth-child(5) > label > select"));
            }
        }

        public HpgElement NewCreateButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Create"));
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

            public HpgElement Name
            {
                get
                {
                    return new HpgElement(form.Element.FindField("name"));
                }
            }

            public HpgElement IsDefault
            {
                get
                {
                    return new HpgElement(form.Element.FindId("isDefault"));
                }
            }

            public HpgElement UoM
            {
                get
                {
                    return new HpgElement(form.Element.FindField("unitOfMeasure"));
                }
            }

            public HpgElement Summable
            {
                get
                {
                    return new HpgElement(form.Element.FindField("summable"));
                }
            }

            public HpgElement IncreasingProgress
            {
                get
                {
                    return new HpgElement(form.Element.FindField("increasing"));
                }
            }

            public HpgElement Numerator
            {
                get
                {
                    return new HpgElement(form.Element.FindField("numeratorName"));
                }
            }

            public HpgElement Denominator
            {
                get
                {
                    return new HpgElement(form.Element.FindField("denominatorName"));
                }
            }

            public void SetExpressAs(ExpressAs expressAs)
            {
                form.Element.Choose(expressAs.ToString());
            }

            public HpgElement CaptureBaseline
            {
                get
                {
                    return new HpgElement(form.Element.FindField("baseline"));
                }
            }

            public HpgElement ReportingPeriod
            {
                get
                {
                    return new HpgElement(form.Element.FindCss("#editDialog > form > div.modal-body > div:nth-child(4) > label > select"));
                }
            }

            public void SetAppliesTo(AppliesTo appliesTo)
            {
                form.Element.Choose(appliesTo.ToString());
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

            /// <summary>
            /// Changes the Edit Result Type fields to the specified ResultType
            /// </summary>
            /// <param name="resultType"></param>
            public void EditResult(ResultType resultType)
            {
                Name.Type(resultType.Name);
                UoM.Type(resultType.UoM);
                SetCheckBox(IsDefault, resultType.IsDefault);
                ReportingPeriod.SelectListOptionByText(resultType.ReportingPeriod.ToString());
                System.Threading.Thread.Sleep(1000);  //give it a couple seconds to catch up
                SaveButton.Click();
                System.Threading.Thread.Sleep(1000);  //give it a couple seconds to save
            }
        }

        #endregion

        #region Actions

        /// <summary>
        /// Sets the New Result Type ExpressAs field to the specified value
        /// </summary>
        /// <param name="expressAs"></param>
        public void SetNewExpressAs(ExpressAs expressAs)
        {
            browser.Choose(expressAs.ToString());
        }

        /// <summary>
        /// Sets the New Result Type AppliesTo field to the specified value
        /// </summary>
        /// <param name="appliesTo"></param>
        public void SetNewAppliesTo(AppliesTo appliesTo)
        {
            browser.Choose(appliesTo.ToString());
        }

        /// <summary>
        /// Submits a supplied ResultType as a new Result Type
        /// </summary>
        /// <param name="result">Single ResultType</param>
        public void SubmitNewResultType(ResultType result)
        {
            ScrollToBottom();
            NewName.Type(result.Name);
            NewUoM.Type(result.UoM);
            SetCheckBox(NewDefault, result.IsDefault);
            NewReportingPeriod.SelectListOptionByText(result.ReportingPeriod.ToString());
            System.Threading.Thread.Sleep(1000);  //give it a couple seconds to catch up
            NewCreateButton.Click();
            System.Threading.Thread.Sleep(1000);  //give it a couple seconds to catch up
        }

        /// <summary>
        /// Gets a list of the currently displayed Result Types
        /// </summary>
        /// <param name="showDeleted">Default to show deleted Result Types as well</param>
        /// <returns>List of DisplayResultTypes</returns>
        public List<DisplayResultType> GetDisplayedResultTypes(bool showDeleted = true)
        {
            List<DisplayResultType> returnList = new List<DisplayResultType>();
            SetCheckBox(ShowDeleted, showDeleted);
            ScrollToBottom();
            foreach (SnapshotElementScope row in browser.FindAllXPath("//tr[contains(@ng-repeat, 'resultTypes')]"))
            {
                DisplayResultType addItem = new DisplayResultType()
                    {
                        Name = row.FindXPath("td[1]").Text.Trim(),
                        UoM = row.FindXPath("td[2]").Text.Trim(),
                        IsDefault = row.FindXPath("td[4]/input").Selected,
                        Edit = new HpgElement(row.FindXPath("td[5]").FindButton("Edit")),
                        Delete = new HpgElement(row.FindXPath("td[5]").FindButton("Delete")),
                        UndoDelete = new HpgElement(row.FindXPath("td[5]").FindButton("Undo Delete"))
                    };

                string cellText = row.FindXPath("td[3]").Text.Trim(); //Reporting Period
                addItem.ReportingPeriod = ReportingPeriod.None;
                if (!string.IsNullOrEmpty(cellText))
                {
                    ReportingPeriod pOut = new ReportingPeriod();
                    if (ReportingPeriod.TryParse(cellText, true, out pOut))
                        addItem.ReportingPeriod = pOut;
                }

                returnList.Add(addItem);
            }


            return returnList;
        }

        public string CompareResultTypes(ResultType one, ResultType two)
        {
            string returnString = "";
            if (!one.Name.Equals(two.Name))
                returnString += "\nName does not match (" + one.Name + " != " + two.Name + ")";
            if (!one.UoM.Equals(two.UoM))
                returnString += "\nUoM does not match (" + one.UoM + " != " + two.UoM + ")";
            if (!one.ReportingPeriod.Equals(two.ReportingPeriod))
                returnString += "\nReportingPeriod does not match (" + one.ReportingPeriod + " != " + two.ReportingPeriod + ")";
            if(!one.IsDefault.Equals(two.IsDefault))
                returnString += "\nIsDefault does not match (" + one.IsDefault + " != " + two.IsDefault + ")";
            return returnString;
        }

        #endregion
    }
}
