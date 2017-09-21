using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coypu;
using AutomationCore;
using AutomationCore.utility;
using HtmlAgilityPack;
using IdeaManagement.Utility;
using System.Windows.Forms;
using OpenQA.Selenium.IE;
using System.Xml;
using System.Xml.Linq;

namespace IdeaManagement.page_objects
{
    class im3PV : imMaster
    {
        public im3PV(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        #region CustomClasses

        public class IdeaValues
        {
            public HpgElement Action;
            public string Status;
            public HpgElement Title;
            public HpgElement Category;
            public HpgElement Department;
            public HpgElement Impact;
            public HpgElement Effort;
            public HpgElement Description;
            public HpgElement Results;
            public HpgElement Baseline;
            public HpgElement ResultType;
            public HpgElement ReportingType;
            public HpgElement IncrementalErosion;
        }

        public class FacilityStatusRow
        {
            public Enums.ImplementedStatus ImplementedStatus;
            public HpgElement ChangeImplementation;
            public string COID;
            public string Facility;
            public DateTime? Date;
        }

        public class IdeaLink
        {
            public HpgElement Title;
            public AngularElements.CheckBox Box;
        }

        public class SoC
        {
            public HpgElement Name;
            public HpgElement Checkbox;
            public string Hierarchy;
            public SoC Parent;
            public HpgElement row;
        }

        #endregion

        #region dialogs
        
        public class SoCDialog
        {
            private ElementScope _dialog;
            public SoCDialog(ElementScope dialog)
            {
                _dialog = dialog;
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

            public List<SoC> SelectedSoCs()
            {
                List<SoC> returnList = new List<SoC>();
                foreach (SnapshotElementScope soc in _dialog.FindAllXPath(".//ul[@class='unstyled']//li"))
                {
                    returnList.Add(new SoC()
                        {
                            Checkbox = new HpgElement(soc.FindXPath(".//input", new Options(){Match = Match.First})),
                            Name = new HpgElement(soc.FindXPath(".//span[@ng-show='!node.isTreeViewJurisdiction']", new Options() { Match = Match.First })),
                            Hierarchy = soc.FindXPath(".//span[@ng-class='formatter.getJurisdictionTypeClass(node.hierarchy)']", new Options() { Match = Match.First }).Text.Trim()
                        });
                }
                return returnList;
            }

            public List<SoC> AvailableSoCs()
            {
                return AvailableSoCs(new HpgElement(_dialog));
            }

            public List<SoC> AvailableSoCs(SoC parent)
            {
                return AvailableSoCs(parent.row);
            }

            public List<SoC> AvailableSoCs(HpgElement parent, string first = "")
            {
                List<SoC> returnList = new List<SoC>();
                if (!parent.Element.FindXPath(".//ul/li").Exists(new Options() { Timeout = TimeSpan.FromSeconds(60) })) return returnList;
                foreach (SnapshotElementScope soc in parent.Element.FindAllXPath(".//ul/li"))
                {
                    returnList.Add(new SoC()
                    {
                        Checkbox = new HpgElement(soc.FindXPath(".//input", new Options() { Match = Match.First })),
                        Name = new HpgElement(soc.FindXPath(".//span[@ng-click='toggleChildren(node)']", new Options() { Match = Match.First })),
                        Hierarchy = soc.FindXPath(".//span[@ng-class='formatter.getJurisdictionTypeClass(node.hierarchy)']", new Options() { Match = Match.First }).Text.Trim(),
                        row = new HpgElement(soc)
                    });
                    if (!string.IsNullOrEmpty(first))
                    {
                        if (returnList.Any(s => RegExRemove(s.Name.Text).Equals(RegExRemove(first))))
                        {
                            return returnList;
                        }
                    }
                }
                return returnList;
            }

            public string RegExRemove(string input, string RegExpression = "[^a-zA-Z0-9]+")
            {
                return System.Text.RegularExpressions.Regex.Replace(input, RegExpression, "");
            }


            /// <summary>
            /// Checks the last SoC specified in socPath
            /// </summary>
            /// <param name="socPath">string '/' seperated path to the desired SoC</param>
            public void ApplySoC(string socPath, string hierarchy = "OCGDMF")
            {
                //hierarchy = Orginazation, Company, Group, Division, Market, Facility
                var paths = socPath.Split('/');
                SoC node = new SoC();
                HpgElement socParent = new HpgElement(_dialog);
                for (int i = 0; i < paths.Count() - 1; i++)
                {
                    node = AvailableSoCs(socParent, paths[i].Trim()).Last(s => RegExRemove(s.Name.Text).ToLower().Equals(RegExRemove(paths[i]).ToLower()) && s.Hierarchy.ToUpper().Equals(hierarchy[i].ToString()));
                    socParent = node.row;
                    node.Name.Click();
                }
                node = AvailableSoCs(socParent, paths.Last()).Last(s => RegExRemove(s.Name.Text).ToLower().Equals(RegExRemove(paths.Last()).ToLower()) && s.Hierarchy.ToUpper().Equals(hierarchy[paths.Count() - 1].ToString()));
                node.Name.Click();
                node.Checkbox.Check();

                HpgAssert.True(SelectedSoCs().FindAll(s => RegExRemove(s.Name.Text).ToLower().Equals(RegExRemove(paths.Last()).ToLower()) && s.Hierarchy.ToUpper().Equals(hierarchy[paths.Count() - 1].ToString())).Any(), "Verify specified SoC has been selected");
            }
        }

        #endregion

        #region objects

        #region FacilityStatusTab

        /// <summary>
        /// Returns a list of Facility Implementation Statuses
        /// </summary>
        /// <param name="interactive">default = true (slow), if false (much faster) does not return HpgElement for interaction</param>
        /// <returns>List of FacilityStatusRow for each facility currently displayed</returns>
        public List<FacilityStatusRow> FacilityStatusRows(bool interactive = true)
        {
            ScrollToBottom();
            try
            {
                FacilityStatusTable.Element.FindXPath(".//tr", new Options() {Match = Match.First}).Now();
            }
            catch (Exception)
            {
            }
            if(interactive)
                return (from r in FacilityStatusTable.Element.FindAllXPath(".//tr")
                    select ParseFacilityStatusRow(new HpgElement(r))).ToList();
            //else
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(FacilityStatusTable.Element.InnerHTML);
            return (from row in doc.DocumentNode.SelectNodes("//tr")
                    select new FacilityStatusRow()
                        {
                            ImplementedStatus = Enums.ImplementedStatusString[row.SelectSingleNode(".//td[1]").InnerText.Trim()],
                            COID = row.SelectSingleNode(".//td[2]").InnerText.Trim().Split('(', ')')[1],
                            Facility = row.SelectSingleNode(".//td[2]").InnerText.Trim().Split(")".ToCharArray(), 2)[1].Trim(),
                            Date = string.IsNullOrEmpty(row.SelectSingleNode(".//td[3]").InnerText.Trim()) ? (DateTime?)null : DateTime.Parse(row.SelectSingleNode(".//td[3]").InnerText.Trim())
                        }).ToList();
        }

        public FacilityStatusRow ParseFacilityStatusRow(HpgElement row)
        {
            return new FacilityStatusRow()
                {
                    ImplementedStatus = Enums.ImplementedStatusString[row.Element.FindXPath(".//td[1]").Text.Trim()],
                    ChangeImplementation = new HpgElement(row.Element.FindXPath(".//td[1]")),
                    COID = row.Element.FindXPath(".//td[2]").Text.Trim().Split('(', ')')[1],
                    Facility = row.Element.FindXPath(".//td[2]").Text.Trim().Split(")".ToCharArray(), 2)[1].Trim(),
                    Date = string.IsNullOrEmpty(row.Element.FindXPath(".//td[3]").Text.Trim()) ? (DateTime?)null : DateTime.Parse(row.Element.FindXPath(".//td[3]").Text.Trim())
                };
        }

        public List<HpgElement> FacilityStatusViews
        {
            get
            {
                return
                    (from t in browser.FindId("FacilityStatusApp").FindAllXPath(".//ul[contains(@class, 'pillNav')]//a")
                     select new HpgElement(t)).ToList();
            }
        }

        #endregion

        public HpgElement EditSoCButton(int view)
        {
            return new HpgElement(browser.FindId("editSpanOfControl" + view.ToString()));
        }

        public ElementScope socDialog
        {
            get { return browser.FindId("editSpanOfControlDialog"); }
        }

        public HpgElement IdeaLinksSection
        {
            get
            {
                return new HpgElement(browser.FindId("panelLinks"));
            }
        }

        public HpgElement IdeaAttachmentsSection
        {
            get
            {
                return new HpgElement(browser.FindId("panelAttachments"));
            }
        }

        public HpgElement IdeaDescription
        {
            get
            {
                return new HpgElement(browser.FindId("Idea_Description"));
            }
        }

        public HpgElement IdeaID
        {
            get
            {
                return new HpgElement(browser.FindId("Idea_IdeaId"));
            }
        }

        public HpgElement IdeaTitle
        {
            get
            {
                return new HpgElement(browser.FindId("Idea_Title"));
            }
        }

        public HpgElement AssignedTo
        {
            get
            {
                return new HpgElement(browser.FindXPath("//h5[contains(@class, 'autoAssigned')]"));
            }
        }

        public HpgElement AssociatedIdeasPanel
        {
            get
            {
                return new HpgElement(browser.FindId("panelAssociatedIdeas"));
            }
        }

        public HpgElement AddAssociatedIdeasButton
        {
            get
            {
                return new HpgElement(AssociatedIdeasPanel.Element.FindButton("Add Associated Ideas"));
            }
        }

        public class AssociatedIdeasDialog
        {
            public HpgElement Dialog;
            public AssociatedIdeasDialog(BrowserSession browser)
            {
                Dialog = new HpgElement(browser.FindId("editAssociationsDialog"));
            }

            public HpgElement SearchBox
            {
                get
                {
                    return new HpgElement(Dialog.Element.FindId("txtSearchTerms"));
                }
            }

            public HpgElement SearchButton
            {
                get
                { 
                    return new HpgElement(Dialog.Element.FindId("btnSearchIdeas"));
                }
            }

            public HpgElement SaveButton
            {
                get
                {
                    return new HpgElement(Dialog.Element.FindButton("Save"));
                }
            }

            public HpgElement CancelButton
            {
                get
                {
                    return new HpgElement(Dialog.Element.FindButton("Cancel"));
                }
            }

            public void SearchFor(string q)
            {
                SearchBox.Type(q);
                SearchButton.Click(5);
            }

            public List<searchResult> SearchResults
            {
                get
                {
                    try
                    {
                        Dialog.Element.FindXPath(".//table/tbody").SendKeys(OpenQA.Selenium.Keys.End);
                    }
                    catch (Exception)
                    {
                    }
                    return (from i in Dialog.Element.FindAllXPath(".//tr[@ng-repeat='result in searchResults']")
                     select new searchResult()
                         {
                             checkbox = new HpgElement(i.FindXPath(".//input")),
                             IdeaId = int.Parse(i.FindXPath(".//td[2]").Text.Trim()),
                             IdeaName = i.FindXPath(".//td[3]").Text.Trim()
                         }).ToList();
                }
            }

            public class searchResult
            {
                public HpgElement checkbox;
                public int IdeaId;
                public string IdeaName;
            }

            public bool SelectIdea(int ideaId)
            {
                SearchFor(ideaId.ToString());
                var idea = SearchResults.First(i => i.IdeaId.Equals(ideaId));
                SetCheckBox(idea.checkbox, true);
                bool selected = idea.checkbox.Element.Selected;
                SaveButton.Click(2);
                return selected;
            }
             
        }

        public int SelectAssociatedIdeas(int[] ideaIds)
        {
            int selectcount = 0;
            foreach (int ideaId in ideaIds)
            {
                AddAssociatedIdeasButton.Click(2);
                var dialog = new AssociatedIdeasDialog(browser);
                if (dialog.SelectIdea(ideaId)) selectcount++;
            }
            return selectcount;
        }

        public HpgElement tabIdea
        {
            get
            {
                return new HpgElement(browser.FindId("ideaTabs").FindLink("Idea"));
            }
        }

        public HpgElement tabFacilityStatus
        {
            get
            {
                return new HpgElement(browser.FindId("ideaTabs").FindLink("Facility Status"));
            }
        }

        public HpgElement tabHistory
        {
            get
            {
                return new HpgElement(browser.FindId("ideaTabs").FindLink("Change History"));
            }
        }

        public HpgElement FacilityStatusTable
        {
            get
            {
                return new HpgElement(browser.FindId("ideaResults"));
            }
        }

        public HpgElement WorkflowStep
        {
            get
            {
                return new HpgElement(browser.FindId("workflowStepAbbrev"));
            }
        }

        public HpgElement Status
        {
            get
            {
                return new HpgElement(browser.FindId("status"));
            }
        }

        public HpgElement UpdatedDate
        {
            get
            {
                return new HpgElement(browser.FindId("Idea_UpdatedDate"));
            }
        }

        public HpgElement EditAttachments
        {
            get
            {
                return new HpgElement(IdeaAttachmentsSection.Element.FindButton("Edit Attachments"));
            }
        }

        public HpgElement EditLinks
        {
            get
            {
                return new HpgElement(IdeaLinksSection.Element.FindButton("Edit Links"));
            }
        }

        public HpgElement EditButton
        {
            get
            {
                return new HpgElement(browser.FindId("bottomButton"));
            }
        }

        public HpgElement ButtonGroup
        {
            get
            {
                return new HpgElement(browser.FindId("ideaTab"));
            }
        }

        public HpgElement ButtonGroupSave
        {
            get
            {
                return new HpgElement(ButtonGroup.Element.FindButton("Save"));
            }
        }

        public HpgElement ButtonGroupDropDown
        {
            get
            {
                    return new HpgElement(ButtonGroup.Element.FindXPath("button"));
            }
        }

        public HpgElement ButtonGroupCommit
        {
            get
            {
                if (ButtonGroup.Element.FindButton("Commit").Exists(new Options() { Timeout = TimeSpan.FromSeconds(2) }))
                    return new HpgElement(ButtonGroup.Element.FindButton("Commit"));
                return new HpgElement(ButtonGroup.Element.FindButton("Submit"));
            }
        }

        public HpgElement ButtonGroupDecline
        {
            get
            {
                return new HpgElement(ButtonGroup.Element.FindButton("Decline"));
            }
        }

        public HpgElement EditAttachmentsDialog
        {
            get
            {
                return new HpgElement(browser.FindId("editAttachmentsDialog2_QualifiedIdeaAttachmentsController"));
            }
        }

        public HpgElement EditAttachmentsAddNew
        {
            get
            {
                return new HpgElement(EditAttachmentsDialog.Element.FindButton("Add Attachment"));
            }
        }

        public HpgElement EditLinkDialog
        {
            get
            {
                return new HpgElement(browser.FindId("editLinksDialog2_QualifiedIdeaLinksController"));
            }
        }

        public HpgElement DeclineComment
        {
            get
            {
                return new HpgElement(browser.FindId("editCommentDialog").FindId("ideaComment"));
            }
        }

        public HpgElement DeclineCommentSaveButton
        {
            get
            {
                return new HpgElement(browser.FindId("editCommentDialog").FindButton("Decline"));
                //return new HpgElement(browser.FindXPath("//*[@id='editCommentDialog']//button[.='Decline']"));
            }
        }

        public HpgElement EditLinkAddNewLink
        {
            get
            {
                return new HpgElement(EditLinkDialog.Element.FindButton("Add Link"));
            }
        }

        public HpgElement EditLinkCancel
        {
            get
            {
                return new HpgElement(EditLinkDialog.Element.FindButton("Cancel"));
            }
        }

        public HpgElement EditLinkSave
        {
            get
            {
                return new HpgElement(EditLinkDialog.Element.FindButton("Save"));
            }
        }

        public HpgElement EditAttachmentsSave
        {
            get
            {
                return new HpgElement(EditAttachmentsDialog.Element.FindButton("Save"));
            }
        }

        public HpgElement EditAttachmentsCancel
        {
            get
            {
                return new HpgElement(EditAttachmentsDialog.Element.FindButton("Cancel"));
            }
        }

        public HpgElement ExpandAllButton
        {
            get
            {
                return new HpgElement(browser.FindId("expand"));
            }
        }

        public HpgElement SaveComboButton
        {
            get
            {
                return new HpgElement(browser.FindXPath("//div[contains(@class, 'divActionButtonGroup')]", new Options(){Match = Match.First}));
            }
        }

        public HpgElement SaveAllButton
        {
            get
            {
                return new HpgElement(SaveComboButton.Element.FindLink("Save"));
            }
        }

        public HpgElement SaveDropDown
        {
            get
            {
                return new HpgElement(SaveComboButton.Element.FindXPath("button"));
            }
        }

        public HpgElement SaveDecline 
        {
            get
            {
                return new HpgElement(SaveComboButton.Element.FindLink("Decline"));
            }
        }

        public HpgElement SaveCommit
        {
            get
            {
                return new HpgElement(SaveComboButton.Element.FindLink("Commit"));
            }
        }

        public HpgElement BackButton
        {
            get
            {
                return new HpgElement(browser.FindLink("Cancel"));
            }
        }


        public HpgElement CopyDescription1
        {
            get
            {
                return new HpgElement(browser.FindId("copyToGPO"));
            }
        }

        public HpgElement CopyDescription2
        {
            get
            {
                return new HpgElement(browser.FindId("copyToNonGPO"));
            }
        }
        
        #region RMIAccordianPanel
        public HpgElement RMIPanel
        {
            get
            {
                return new HpgElement(browser.FindXPath("//div[@ng-app='RequestMoreInformationApp']"));
            }
        }

        public HpgElement RMIRequestFrom
        {
            get
            {
                return new HpgElement(RMIPanel.Element.FindXPath(".//select"));
            }
        }

        public HpgElement RMIRequestTextArea
        {
            get
            {
                return new HpgElement(RMIPanel.Element.FindXPath(".//textarea[@name='Request']"));
            }
        }

        public HpgElement RMIResponseTextArea
        {
            get
            {
                return new HpgElement(RMIPanel.Element.FindXPath("//textarea[@name='Response']"));
            }
        }

        public HpgElement RMIResponseAddLinks
        {
            get
            {
                return new HpgElement(RMIPanel.Element.FindButton("+Add Links"));
            }
        }

        public HpgElement RMIResponseAddAttachments
        {
            get
            {
                return new HpgElement(RMIPanel.Element.FindButton("+Add Attachments"));
            }
        }

        public HpgElement RMIPostButton
        {
            get
            {
                return new HpgElement(RMIPanel.Element.FindButton("Post"));
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
                get
                {
                    return element.FindXPath(".//div/div[1]/h4").Text;
                }
            }
            public string message
            {
                get
                {
                    return element.FindXPath(".//p").Text;
                }
            }

            public DateTime submittedDate
            {
                get
                {
                    return DateTime.Parse(element.FindXPath(".//div[contains(@class, 'text-right')]").Text);
                }
            }
            public List<HpgElement> links
            {
                get
                {
                    element.FindXPath(".//span[contains(@ng-repeat, 'response.links')]", new Options() { Match = Match.First }).Now(); 
                    return
                        (from l in element.FindAllXPath(".//span[contains(@ng-repeat, 'response.links')]")
                         select new HpgElement(l)).ToList();
                }
            }
            public List<HpgElement> attachments
            {
                get
                {
                    element.FindXPath(".//span[contains(@ng-repeat, 'response.attachments')]", new Options() {Match = Match.First}).Now();
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

        #region RMIAddLinks

        public void RMIShowAddLinks()
        {
            for (int i = 0; i < 5; i++)
            {
                if (AddLinksDialog.Element.Exists(new Options() {Timeout = TimeSpan.FromSeconds(3)})) break;
                RMIResponseAddLinks.Click(2);
            }
            HpgAssert.True(AddLinksDialog.Element.Exists(), "RMI Add Links dialog is present");
        }

        public void RMIAddLinks(Dictionary<string, string> links, bool openCloseDialog = false)
        {
            if(openCloseDialog) RMIShowAddLinks();
            foreach (KeyValuePair<string, string> link in links)
            {
                RMIAddLink(link.Key, link.Value);
            }
            if(openCloseDialog) AddLinksDialogSaveButton.Click(2);
        }

        public void RMIAddLink(string title, string url)
        {
            AllLinksTitle.Last().Type(title);
            AllLinksURL.Last().Type(url);
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
        #endregion

        #region RMIAddAttachments
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
        #endregion
        #endregion

        #region actions

        /// <summary>
        /// Clicks the 'Copy All' button for the specified 0-based view to copy all fields from one view to the other
        /// </summary>
        /// <param name="fromView">0-based integer of what view to copy from</param>
        public void CopyFields(int fromView = 0)
        {
            AutomationCore.base_tests.BaseTest.WriteReport(string.Format("Copy all fields from view {0} to {1}...", fromView.ToString(), (fromView + 1).ToString()));
            browser.FindButton("Copy All", new Options() { Match = Match.First }).Now();
            var copyButton = browser.FindAllXPath("//button[.='Copy All']").ElementAt(fromView);
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    copyButton.SendKeys(OpenQA.Selenium.Keys.ArrowUp);
                    copyButton.Hover();
                    copyButton.Click();
                }
                catch (Exception)
                {
                }
                try
                {
                    browser.FindId("copyAlertDialog").FindButton("Continue").Click();
                    browser.FindId("copyAlertDialog").FindButton("Continue").SendKeys(OpenQA.Selenium.Keys.Enter);
                    break;
                }
                catch (Exception)
                {
                }
            }
        }

        public void UnlockEditIdea()
        {
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    if (EditButton.Element.Exists(new Options() { Timeout = TimeSpan.FromSeconds(3) }))
                    {
                        EditButton.Element.SendKeys(OpenQA.Selenium.Keys.ArrowUp);
                        EditButton.Hover();
                        EditButton.Click();
                        while (browser.FindId("alertDialog").FindButton("OK" ,new Options() {Timeout = TimeSpan.FromSeconds(5)}).Exists())
                        {
                            browser.FindId("alertDialog").FindButton("OK", new Options() { Timeout = TimeSpan.FromSeconds(5) }).Click();
                        }
                        if (EditButton.Element.Missing(new Options() {Timeout = TimeSpan.FromSeconds(3)})) break;
                    }
                }
                catch (Exception)
                {
                }
                try
                {
                    if (browser.FindId("unpublishAlertDialog").Exists(new Options() {Timeout = TimeSpan.FromSeconds(3)}))
                    {
                        browser.FindId("unpublishAlertDialog").ClickButton("Continue");
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public string[] RMIRequestSelectAll()
        {
            RMIRequestFrom.Click();
            RMIRequestFrom.Element.FindXPath(".//option", new Options() {Match = Match.First}).Now();
            DateTime quit = DateTime.Now.AddSeconds(60);
            for (int o = 1; o <= RMIRequestFrom.Element.FindAllXPath(".//option").Count(); o++)
            {
                var selectMe = RMIRequestFrom.Element.FindXPath(".//option[" + o.ToString() + "]");
                if (string.IsNullOrEmpty(selectMe["selected"]))
                {
                    AutomationCore.base_tests.BaseTest.WriteReport("Selecting '" + selectMe.Text + "'...");
                    RMIRequestFrom.Element.SelectOption(selectMe.Text);
                }
            }
            string[] returnItems = RMIRequestFrom.Element.FindAllXPath(".//option").Where(o => !string.IsNullOrEmpty(o["selected"]))
                              .Select(o => o.Text)
                              .ToArray();
            return returnItems;
        }

        public void RMIAddAttachments(Dictionary<string, string> attachments)
        {
            foreach (KeyValuePair<string, string> attachment in attachments)
            {
                RMIAddAttachment(attachment.Key, attachment.Value);
            }
        }

        public void RMIAddAttachment(string name, string path)
        {
            AddAttachmentsDialogAddAttachmentButton.Click();
            AllAttachmentsTitle.Last().Type(name);
            AllAttachmentsFile.Last().Element.SendKeys(path);
        }

        //public void EditPage()
        //{
        //    for (int i = 0; i < 5; i++)
        //    {
        //        if (EditButton.Element.Exists(new Options() {Timeout = TimeSpan.FromSeconds(2)}))
        //        {
        //            EditButton.Click();
        //        }
        //        if (SaveDropDown.Element.Exists(new Options() { Timeout = TimeSpan.FromSeconds(2) }))
        //        {
        //            break;
        //        }
        //        System.Threading.Thread.Sleep(3000);
        //    }
        //    HpgAssert.True(EditButton.Element.Missing(), "Verify Edit Button was pressed and is no longer present.");
        //    HpgAssert.True(SaveDropDown.Element.Exists(), "Verify Multifunction Save button is now present");
        //}

        public void DisplaySoCDialog(int view)
        {
            for (int i = 0; i < 3; i++)
            {
                if (socDialog.Exists())
                {
                    break;
                }
                EditSoCButton(view).Click();
                System.Threading.Thread.Sleep(1000);
            }
            HpgAssert.True(socDialog.Exists(), "Verify SoC dialog is present");
        }

        public void AddAttachments(Dictionary<string, string> files)
        {
            foreach (KeyValuePair<string, string> fileToTest in files)
            {
                AddAttachment(fileToTest.Key, fileToTest.Value);
            }
        }

        public void AddAttachment(string name, string path)
        {
            EditAttachmentsAddNew.Click();
            EditAttachmentsDialogGetDescriptions().Last().Type(name);
            EditAttachmentsDialogGetFileInputs().Last().Element.SendKeys(path);
        }

        public void ShowAddLinksDialog()
        {
            for (int i = 0; i < 5; i++)
            {
                if (RMIResponseAddLinks.Element.Exists(new Options() {Timeout = TimeSpan.FromSeconds(3)})) break;
                RMIResponseAddLinks.Click(2);
            }
            HpgAssert.True(RMIResponseAddLinks.Element.Exists(), "RMI Add Links dialog is present");
        }

        public void AddLinks(Dictionary<string, string> links, bool OpenCloseDialog = false)
        {
            if(OpenCloseDialog) ShowAddLinksDialog();
            foreach (KeyValuePair<string, string> link in links)
            {
                //go thru each link and add
                EditLinkAddNewLink.Element.SendKeys(OpenQA.Selenium.Keys.PageDown);
                EditLinkAddNewLink.Element.Hover();
                EditLinkAddNewLink.Click();
                EditLinksDialogGetDescriptions().Last().Type(link.Key);
                EditLinksDialogGetURLs().Last().Type(link.Value);
            }
            if(OpenCloseDialog) AddLinksDialogSaveButton.Click(2);
        }

        public void ClickDecline()
        {
            //if(ButtonGroupDecline.Element.Missing())
            //    ButtonGroupDropDown.Click(2);
            ButtonGroupDecline.Element.Hover();
            ButtonGroupDecline.Click(5);
            System.Threading.Thread.Sleep(10000);
        }

        public void DeclineIdea(string declineComment)
        {
            ClickDecline();
            //type the comment into the decline dialog
            //click ok on the decline dialog
        }

        public void ClickCommit()
        {
            //for (int i = 0; i < 5; i++)
            //{
            //    if (ButtonGroupCommit.Element.Exists(new Options() {Timeout = TimeSpan.FromSeconds(1)}))
            //    {
            //        break;
            //    }
            //    ButtonGroupDropDown.Click(2);
            //}
            ButtonGroupCommit.Element.Hover();
            ButtonGroupCommit.Click(10);
            WaitForThrobber();
        }

        #region EditLinks
        public List<HpgElement> EditLinksDialogGetDescriptions()
        {
            return
                (from l in EditLinkDialog.Element.FindAllXPath(".//input[@name='description']")
                 select new HpgElement(l)).ToList();
        }

        public List<HpgElement> EditLinksDialogGetURLs()
        {
            return
                (from l in EditLinkDialog.Element.FindAllXPath(".//input[@name='url']")
                 select new HpgElement(l)).ToList();
        }

        public List<HpgElement> EditLinksDialogGetDeleteLinks()
        {
            return
                (from l in EditLinkDialog.Element.FindAllXPath(".//a[@title='Delete Link']")
                 select new HpgElement(l)).ToList();
        }
        #endregion

        #region EditAttachments
        public List<HpgElement> EditAttachmentsDialogGetDescriptions()
        {
            return
                (from a in EditAttachmentsDialog.Element.FindAllXPath(".//input[contains(@id, '__Description')]")
                 select new HpgElement(a)).ToList();
        }

        public List<HpgElement> EditAttachmentsDialogGetFileInputs()
        {
            return
                (from a in EditAttachmentsDialog.Element.FindAllXPath(".//input[contains(@id, '__UploadedFile')]")
                 select new HpgElement(a)).ToList();
        }

        public List<HpgElement> EditAttachmentsDialogGetDeleteLinks()
        {
            return
                (from a in EditAttachmentsDialog.Element.FindAllXPath(".//a[@title='Delete Attachment']")
                 select new HpgElement(a)).ToList();
        }
        #endregion

        public IdeaValues GetIdeaValues(int view)
        {
            IdeaValues returnValues = new IdeaValues();
            returnValues.Action = new HpgElement(browser.FindId("qIdeas_" + view.ToString() + "__StatusId"));
            returnValues.Status = browser.FindId("qIdeas_" + view.ToString() + "__StatusId").SelectedOption;
            returnValues.Title = new HpgElement(browser.FindId("qIdeas_" + view.ToString() + "__Title"));
            returnValues.Category = new HpgElement(browser.FindId("qIdeas_" + view.ToString() + "__CategoryId"));
            returnValues.Department = new HpgElement(browser.FindId("qIdeas_" + view.ToString() + "__DepartmentId"));
            returnValues.Impact = new HpgElement(browser.FindId("qIdeas_" + view.ToString() + "__ImpactLevel"));
            returnValues.Effort = new HpgElement(browser.FindId("qIdeas_" + view.ToString() + "__EffortLevel"));
            returnValues.Description = new HpgElement(browser.FindId("qIdeas_" + view.ToString() + "__Description"));
            returnValues.Results = new HpgElement(browser.FindId("qIdeas_" + view.ToString() + "__ResultsMeasurement"));
            returnValues.Baseline = new HpgElement(browser.FindId("qIdeas_" + view.ToString() + "__IsBaseline"));
            returnValues.ResultType = new HpgElement(browser.FindId("qIdeas_" + view.ToString() + "__ResultTypeId"));
            returnValues.ReportingType = new HpgElement(browser.FindId("qIdeas_" + view.ToString() + "__ReportingType"));
            returnValues.IncrementalErosion = new HpgElement(browser.FindId("qIdeas_" + view.ToString() + "__IncrementalValueErosion"));
            return returnValues;
        }

        public List<IdeaLink> GetIdeaLinks(int view)
        {
            List<IdeaLink> returnList = new List<IdeaLink>();
            foreach (SnapshotElementScope row in IdeaLinksSection.Element.FindAllXPath(".//tr[td[2]]"))
            {
                returnList.Add(new IdeaLink()
                {
                    Title = new HpgElement(row.FindXPath(".//td[1]//a")),
                    Box = new AngularElements.CheckBox(row.FindXPath(".//td[" + (view + 2).ToString() + "]//a"))
                });
            }
            return returnList;
        }

        public List<IdeaLink> GetIdeaAttachments(int view)
        {
            List<IdeaLink> returnList = new List<IdeaLink>();
            foreach (SnapshotElementScope row in IdeaAttachmentsSection.Element.FindAllXPath(".//tr[td[2]]"))
            {
                returnList.Add(new IdeaLink()
                {
                    Title = new HpgElement(row.FindXPath(".//td[1]//a")),
                    Box = new AngularElements.CheckBox(row.FindXPath(".//td[" + (view + 2).ToString() + "]//a"))
                });
            }
            return returnList;
        }

        #endregion

    }
}
