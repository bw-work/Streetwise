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
using System.Data;
using HtmlAgilityPack;
using Streetwise.Utility;


namespace Streetwise.page_objects
{
    class swPublishedIdeas : swMaster
    {
        public swPublishedIdeas(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        public class PublishedIdea
        {
            public HpgElement Bookmark;
            public int IdeaNumber;
            public HpgElement IdeaName;
            public string Department;
            public string Category;
            public string EffortLevel;
            public int Effort;
            public string ImpactLevel;
            public int Impact;
            public DateTime Updated;
        }

        public class CFApPublishedIdea : PublishedIdea
        {
            public HpgElement ImplementSelect;
            public Enums.ImplementedStatus ImplementedStatus;
        }

        #region Objects
        
        public HpgElement ClearRefinementsTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#PublishedIdeasListController > div.span12 > div > div:nth-child(2) > div > button.btn-link.clearSearchLink"));
            }
        }

        public HpgElement SearchField
        {
            get
            {
                return new HpgElement(browser.FindCss("#PublishedIdeasListController > div.span12 > div > div:nth-child(2) > div > input"));
            }
        }

        public HpgElement SearchButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Search"));
            }
        }

        public HpgElement SortByTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#PublishedIdeasListController > div.span9 > div.fixed-scroll > div.resultHeaderBackground > div > div.span5 > div > strong"));
            }
        }

        public HpgElement SortByDropdown
        {
            get
            {
                return new HpgElement(browser.FindCss("#PublishedIdeasListController > div.span9 > div.fixed-scroll > div.resultHeaderBackground > div > div.span5 > div > select"));
            }
        }



        public HpgElement LinkExcel
        {
            get
            {
                return new HpgElement(browser.FindCss("#PublishedIdeasListController > div.span3 > ul > li:nth-child(2) > form > input[type=\"image\"]"));
            }
        }

        public HpgElement LinkPDF
        {
            get
            {
                return new HpgElement(browser.FindCss("#PublishedIdeasListController > div.span3 > ul > li:nth-child(1) > form > input[type=\"image\"]"));
            }
        }

        public HpgElement RecordCountTxt
        {
            get
            {
                return new HpgElement(browser.FindId("Recordcount"));
            }
        }

        public HpgElement RefineResultsTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#PublishedIdeasListController > div.span3 > div > h2"));
            }
        }

        public HpgElement MyBookmarksChkBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#PublishedIdeasListController > div.span3 > div > label > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement MyBookmarksTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#PublishedIdeasListController > div.span3 > div > label"));
            }
        }

        public HpgElement PublishedDateRangeTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#IdeaDateRangeDiv > h5"));
            }
        }

        public HpgElement IdeaDateRangeStart
        {
            get
            {
                return new HpgElement(browser.FindId("ideaDateRangeStart"));
            }
        }

        public HpgElement IdeaDataRangeEnd
        {
            get
            {
                return new HpgElement(browser.FindId("ideaDateRangeEnd"));
            }
        }

        public HpgElement ReloadIdeasBasedOnRangeButton
        {
            get
            {
                return new HpgElement(browser.FindCss("#IdeaDateRangeDiv > button"));
            }
        }

        public HpgElement UpdatedDateTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#PublishedIdeasListController > div.span3 > div > h5"));
            }
        }

        public HpgElement UpdateDateDropdown
        {
            get
            {
                return new HpgElement(browser.FindId("updateDateSelect"));
            }
        }

        public HpgElement DepartmentTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#filter > h5:nth-child(1)"));
            }
        }

        public HpgElement ShowMoreDepartmentsButton
        {
            get
            {
                return new HpgElement(browser.FindId("showMoreDepartments"));
            }
        }

        public HpgElement ShowLessDepartmentsButton
        {
            get
            {
                return new HpgElement(browser.FindId("showLessDepartments"));
            }
        }

        public HpgElement ShowMoreCategoriesButton
        {
            get
            {
                return new HpgElement(browser.FindId("showMoreCategories"));
            }
        }

        public HpgElement ShowLessCategoriesButton
        {
            get
            {
                return new HpgElement(browser.FindId("showLessCategories"));
            }
        }

        public HpgElement AnyDepartmentCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(1) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement AnyDepartmentTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(1)"));
            }
        }

        public HpgElement BusinessCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(2) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement BusinessTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(2)"));
            }
        }

        public HpgElement CardiovascularServicesCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(3) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement CardiovascularServicesTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(3)"));
            }
        }

        public HpgElement CentralSterileProcessingCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(4) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement CentralSterileProcessingTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(4)"));
            }
        }

        public HpgElement ContractConversionCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(5) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement ContractConversionTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(5)"));
            }
        }

        public HpgElement EmergencyServicesCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(6) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement EmergencyServicesTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(6)"));
            }
        }

        public HpgElement EndoscopyCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(7) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement EndoscopyTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(7)"));
            }
        }

        public HpgElement EnvironmentalServicesCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(8) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement EnvironmentalServicesTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(8)"));
            }
        }

        public HpgElement FacilitySafetyCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(9) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement FacilitySafetyTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(9)"));
            }
        }

        public HpgElement FoodNutritionServicesCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(10) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement FoodNutritionServicesTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(10)"));
            }
        }

        public HpgElement ImagingCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(11) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement ImagingTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(11)"));
            }
        }

        public HpgElement LaboratoryCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(12) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement LaboratoryTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(12)"));
            }
        }

        public HpgElement MaterialManagementCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(13) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement MaterialManagementTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(13)"));
            }
        }

        public HpgElement NICUPediatricsCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(14) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement NICUPediatricsTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(14)"));
            }
        }

        public HpgElement NursingServicesCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(15) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement NursingServicesTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(15)"));
            }
        }

        public HpgElement ObstetricsPerinatalCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(16) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement ObstetricsPerinatalTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(16)"));
            }
        }

        public HpgElement OtherClinicalServiceCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(17) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement OtherClinicalServiceTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(17)"));
            }
        }

        public HpgElement OtherSupportServiceCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(18) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement OtherSupportServiceTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(18)"));
            }
        }

        public HpgElement PharmacyCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(19) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement PharmacyTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(19)"));
            }
        }

        public HpgElement PhysicianReferenceContractsCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(20) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement PhysicianReferenceContractsTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(20)"));
            }
        }

        public HpgElement DepartmentProductFormularyCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(21) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement DepartmentProductFormularyTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(21)"));
            }
        }

        public HpgElement RadiologyCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(22) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement RadiologyTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(22)"));
            }
        }

        public HpgElement RespiratoryTherapyCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(23) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement RespiratoryTherapyTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(23)"));
            }
        }

        public HpgElement SurgicalServicesCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(24) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement SurgicalServicesTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#departments > label:nth-child(24)"));
            }
        }

        public HpgElement CategoryTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#filter > h5:nth-child(6)"));
            }
        }

        public HpgElement APInfrastructureCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(1) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement APInfrastructureTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(1)"));
            }
        }

        public HpgElement BloodCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(2) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement BloodTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(2)"));
            }
        }

        public HpgElement CapitalAcquisitionCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(3) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement CapitalAcquisitionTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(3)"));
            }
        }

        public HpgElement CommoditiesCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(4) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement CommoditiesTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(4)"));
            }
        }

        public HpgElement CommodityPharmacyCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(5) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement CommodityPharmacyTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(5)"));
            }
        }

        public HpgElement CVATCostAvoidanceCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(6) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement CVATCostAvoidanceTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(6)"));
            }
        }

        public HpgElement CVATCostSavingsCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(7) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement CVATCostSavingsTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(7)"));
            }
        }

        public HpgElement DivManagedDisintermediationCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(8) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement DivManagedDisintermediationTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(8)"));
            }
        }

        public HpgElement DivisionBasedContractingCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(9) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement DivisionBasedContractingTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(9)"));
            }
        }

        public HpgElement HCAPharmacyOperationsProjectCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(10) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement HCAPharmacyOperationsProjectTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(10)"));
            }
        }

        public HpgElement HPGSIPCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(11) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement HPGSIPTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(11)"));
            }
        }

        public HpgElement HTRoadmapCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(12) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement HTRoadmapTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(12)"));
            }
        }

        public HpgElement NonSupplyExpenseSavingsCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(13) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement NonSupplyExpenseSavingsTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(13)"));
            }
        }

        public HpgElement ORCostPerCaseCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(14) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement ORCostPerCaseTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(14)"));
            }
        }

        public HpgElement CategoryProductFormularyCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(15) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement CategoryProductFormularyTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(15)"));
            }
        }

        public HpgElement RoadMapPharmacy2017CheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(16) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement RoadMapPharmacy2017Txt
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(16)"));
            }
        }

        public HpgElement RoadMapReprocessing2017CheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(17) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement RoadMapReprocessing2017Txt
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(17)"));
            }
        }

        public HpgElement RoadMap2015CheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(18) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement RoadMap2015Txt
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(18)"));
            }
        }

        public HpgElement SMATCostAvoidanceCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(19) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement SMATCostAvoidanceTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(19)"));
            }
        }

        public HpgElement SupportServicesEVSCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(20) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement SupportServicesEVSTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(20)"));
            }
        }

        public HpgElement SupportServicesFoodCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(21) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement SupportServicesFoodTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(21)"));
            }
        }

        public HpgElement TechnologyCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(22) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement TechnologyTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(22)"));
            }
        }

        public HpgElement ThirdPartyFreightManagementCheckBox
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(23) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement ThirdPartyFreightManagementTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#categories > label:nth-child(23)"));
            }
        }

        public HpgElement ImpactTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#filter > h5:nth-child(11)"));
            }
        }

        public HpgElement ImpactCheckBox1
        {
            get
            {
                return new HpgElement(browser.FindCss("#filter > label:nth-child(12) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement ImpactCheckBox2
        {
            get
            {
                return new HpgElement(browser.FindCss("#filter > label:nth-child(13) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement ImpactCheckBox3
        {
            get
            {
                return new HpgElement(browser.FindCss("#filter > label:nth-child(14) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement EffortTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#filter > h5:nth-child(11)"));
            }
        }

        public HpgElement EffortCheckBox1
        {
            get
            {
                return new HpgElement(browser.FindCss("#filter > label:nth-child(17) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement EffortCheckBox2
        {
            get
            {
                return new HpgElement(browser.FindCss("#filter > label:nth-child(18) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement EffortCheckBox3
        {
            get
            {
                return new HpgElement(browser.FindCss("#filter > label:nth-child(19) > input[type=\"checkbox\"]"));
            }
        }

        public HpgElement MoreIdeasButton
        {
            get
            {
                return new HpgElement(browser.FindId("infiniteLinkText"));
            }
        }

        public HpgElement RefinementsTxt
        {
            get
            {
                return new HpgElement(browser.FindCss("#PublishedIdeasListController > div.span9 > div.fixed-scroll > div.row > div > div > h4"));
            }
        }



        #endregion

        #region Actions

        //public void BulkEditApplyCuttonClick()
        //{
        //    BulkEditApplyButton.Element.Hover();
        //    BulkEditApplyButton.Click();
        //    System.Threading.Thread.Sleep(10000);
        //}

        //public void ShowBulkEdit()
        //{
        //    if(!BulkEditImplementDropdown.Element.Exists(new Options(){Timeout = TimeSpan.FromSeconds(1)})) BulkEditLink.Click();
        //    HpgAssert.True(BulkEditImplementDropdown.Element.Exists(), "Bulk Change Drop Down is visible");
        //}

        //public void SaveExcel(string saveFileName)
        //{
        //    browser.SaveWebResource(LinkExcel.Element["href"], saveFileName);
        //}

        //public void ShowAllDepartments()
        //{
        //    for (int i = 0; i < 5; i++)
        //    {
        //        FilterMoreDepartments.Element.Hover();
        //        FilterMoreDepartments.Click();
        //        if (FilterMoreDepartments.Element.Missing())
        //        {
        //            break;
        //        }
        //    }
        //    HpgAssert.True(FilterMoreDepartments.Element.Missing(), "Show More Departments selected");
        //}

        //public void ShowAllCategories()
        //{
        //    for (int i = 0; i < 5; i++)
        //    {
        //        FilterMoreCategories.Element.Hover();
        //        FilterMoreCategories.Click();
        //        if (FilterMoreCategories.Element.Missing())
        //        {
        //            break;
        //        }
        //    }
        //    HpgAssert.True(FilterMoreCategories.Element.Missing(), "Show More Categories selected");
        //}

        //public HpgElement GetDepartmentFilter(string department)
        //{
        //    return new HpgElement(browser.FindXPath("//ul[@id='departments']//a[.='" + department + "']"));
        //}

        //public HpgElement GetCategoryFilter(string category)
        //{
        //    return new HpgElement(browser.FindXPath("//ul[@id='categories']//a[.='" + category + "']"));
        //}

        //public HpgElement GetEffortFilter(int level)
        //{
        //    return new HpgElement(browser.FindXPath("//li[count(span[@id='level'])=" + level.ToString() + "]/a[contains(@id, 'Effort')]"));
        //}

        //public HpgElement GetImpactFilter(int level)
        //{
        //    return new HpgElement(browser.FindXPath("//li[count(span[@id='level'])=" + level.ToString() + "]/a[contains(@id, 'Impact')]"));
        //}

        //public HpgElement GetUpdatedDateFilter(int filter)
        //{
        //    return new HpgElement(browser.FindXPath("//ul[@id='updateDiate']/li[" + filter.ToString() + "]/a"));
        //}

        public DataTable GetPublishedIdeasDT()
        {
            DataTable returnTable = new DataTable("PublishedIdeas");

            returnTable.Columns.Add("IdeaNumber", typeof(int));
            returnTable.Columns["IdeaNumber"].Unique = true;
            returnTable.Columns.Add("IdeaName", typeof(string));
            returnTable.Columns.Add("Assigned", typeof(string));
            returnTable.Columns.Add("Effort", typeof(int));
            returnTable.Columns.Add("Impact", typeof(int));
            returnTable.Columns.Add("UpdatedDate", typeof(DateTime));
            //returnTable.Columns.Add("PublishedDate", typeof(DateTime));
            returnTable.Columns.Add("Category", typeof(string));
            returnTable.Columns.Add("Department", typeof(string));

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(browser.FindId("publishIdeasTable")["outerHTML"]);
            foreach (HtmlNode ideaRow in doc.DocumentNode.SelectNodes("//tr[td]"))
            {
                DataRow idea = returnTable.NewRow();
                string[] ideaColumns = (from c in ideaRow.SelectNodes("./td") select c.InnerText).ToArray();
                idea["IdeaNumber"] = int.Parse(ideaColumns[0]);
                idea["IdeaName"] = ideaColumns[1];
                idea["Department"] = ideaColumns[2];
                idea["Category"] = ideaColumns[3];
                idea["Effort"] = ideaRow.SelectNodes("./td")[4].SelectNodes(".//i").Count;
                idea["Impact"] = ideaRow.SelectNodes("./td")[5].SelectNodes(".//i").Count;
                //idea["PublishedDate"] = DateTime.Parse(ideaColumns[6]);
                idea["UpdatedDate"] = DateTime.Parse(string.IsNullOrEmpty(ideaColumns[6]) ? "01/01/1900" : ideaColumns[6]);
                returnTable.Rows.Add(idea);
            }
            return returnTable;
        }

        //Used to grab only the first card for assertion purposes
        //public PublishedIdea GetPublishedIdeas()
        //{
        //    browser.FindCss("#IdeasCardRowsDiv > div > div:nth-child(1) > div > div:nth-child(1) > div", new Options() { Match = Match.First, Timeout = TimeSpan.FromSeconds(90) }).Exists();
        //    PublishedIdea returnIdea = new PublishedIdea();

        //    foreach (SnapshotElementScope row in browser.FindCss("#IdeasCardRowsDiv > div > div:nth-child(1) > div > div:nth-child(1) > div"))
        //    {
        //        returnIdea.Bookmark = new HpgElement(row.FindCss("a[ng-tooltip='Bookmark Idea']"));
        //        returnIdea.IdeaName = new HpgElement(row.FindCss("a[class='toggle ng-binding']"));
        //        returnIdea.IdeaNumber = int.Parse(returnIdea.IdeaName.Text.Trim().Split('-')[0].Trim());
        //        returnIdea.Updated = DateTime.Parse(row.FindCss("p[class='pull-right autoUpdated']").Text.Replace("Updated:", "").Trim());
        //        returnIdea.Category = row.FindCss("p[class='autoCategory ng-binding']").Text.Replace("Category:", "").Trim();
        //        returnIdea.Department = row.FindCss("p[class='autoDepartment ng-binding']").Text.Replace("Department:", "").Trim();
        //        returnIdea.Impact = row.FindAllCss("i[class='icon-usd']").Count();
        //        returnIdea.ImpactLevel = Enums.levels[returnIdea.Impact];
        //        returnIdea.Effort = row.FindAllCss("i[class='icon-wrench']").Count();
        //        returnIdea.EffortLevel = Enums.levels[returnIdea.Effort];
        //    }
            
        //    return returnIdea;
        //}

        public List<PublishedIdea> GetPublishedIdeas(bool ScrollToBottom = true)
        {
            if(ScrollToBottom) base.ScrollToBottom();

            browser.FindXPath("//div[@class='cardResult']", new Options() { Match = Match.First, Timeout = TimeSpan.FromSeconds(90) }).Exists();
            List<PublishedIdea> returnList = new List<PublishedIdea>();
            foreach (SnapshotElementScope row in browser.FindAllXPath("//div[@class='cardResult']"))
            {
                PublishedIdea addIdea = new PublishedIdea();
                addIdea.Bookmark = new HpgElement(row.FindCss("a[ng-tooltip='Bookmark Idea']"));
                addIdea.IdeaName = new HpgElement(row.FindCss("a[class='toggle ng-binding']"));
                addIdea.IdeaNumber = int.Parse(addIdea.IdeaName.Text.Trim().Split('-')[0].Trim());
                addIdea.Updated = DateTime.Parse(row.FindCss("p[class='pull-right autoUpdated']").Text.Replace("Updated:", "").Trim());
                addIdea.Category = row.FindCss("p[class='autoCategory ng-binding']").Text.Replace("Category:", "").Trim();
                addIdea.Department = row.FindCss("p[class='autoDepartment ng-binding']").Text.Replace("Department:", "").Trim();
                addIdea.Impact = row.FindAllCss("i[class='icon-usd']").Count();
                addIdea.ImpactLevel = Enums.levels[addIdea.Impact];
                addIdea.Effort = row.FindAllCss("i[class='icon-wrench']").Count();
                addIdea.EffortLevel = Enums.levels[addIdea.Effort];
                returnList.Add(addIdea);
            }
            return returnList;
        }

        public int[] GetPublishedIdeasIds()
        {
            ScrollToBottom();
            browser.FindXPath("//div[@class='cardResult']", new Options() { Match = Match.First, Timeout = TimeSpan.FromSeconds(90) }).Exists();
            HtmlAgilityPack.HtmlDocument reader = new HtmlAgilityPack.HtmlDocument();
            reader.LoadHtml(browser.FindXPath("//div[@ng-repeat='cardRow in cardRows']/..").OuterHTML);
            return (from t in reader.DocumentNode.SelectNodes("//h3[contains(@class, 'autoTitle')]")
                    select int.Parse(t.InnerText.Split('-').First().Trim())).ToArray();
        }

        public List<CFApPublishedIdea> GetCFApPublishedIdeas(bool ScrollToBottom = true)
        {
            if (ScrollToBottom) base.ScrollToBottom();

            browser.FindXPath("//div[@class='cardResult']", new Options() { Match = Match.First, Timeout = TimeSpan.FromSeconds(90) }).Exists();
            List<CFApPublishedIdea> returnList = new List<CFApPublishedIdea>();
            foreach (SnapshotElementScope row in browser.FindAllXPath("//div[@class='cardResult']"))
            {
                returnList.Add(ToPublishedIdea(new HpgElement(row)));
            }
            return returnList;
        }

        public List<CFApPublishedIdea> GetCFApPublishedIdeas(int start, int count, bool ScrollToBottom = true)
        {
            if (ScrollToBottom) base.ScrollToBottom();
            System.Threading.Thread.Sleep(5000);
            browser.FindXPath("//div[@class='cardResult']", new Options() { Match = Match.First, Timeout = TimeSpan.FromSeconds(90) }).Exists();
            List<CFApPublishedIdea> returnList = new List<CFApPublishedIdea>();
            foreach (SnapshotElementScope row in browser.FindAllXPath("//div[@class='cardResult']").Skip(start).Take(count))
            {
                returnList.Add(ToPublishedIdea(new HpgElement(row)));
            }
            return returnList;
        }

        private CFApPublishedIdea ToPublishedIdea(HpgElement card)
        {
            CFApPublishedIdea addIdea = new CFApPublishedIdea();
            addIdea.Bookmark = new HpgElement(card.Element.FindCss("a[ng-tooltip='Bookmark Idea']"));
            addIdea.IdeaName = new HpgElement(card.Element.FindCss("a[class='toggle ng-binding']"));
            addIdea.IdeaNumber = int.Parse(addIdea.IdeaName.Text.Trim().Split('-')[0].Trim());
            addIdea.Updated = DateTime.Parse(card.Element.FindCss("p[class='pull-right autoUpdated']").Text.Replace("Updated:", "").Trim());
            addIdea.Category = card.Element.FindCss("p[class='autoCategory ng-binding']").Text.Replace("Category:", "").Trim();
            addIdea.Department = card.Element.FindCss("p[class='autoDepartment ng-binding']").Text.Replace("Department:", "").Trim();
            addIdea.Impact = card.Element.FindAllCss("i[class='icon-usd']").Count();
            addIdea.ImpactLevel = Enums.levels[addIdea.Impact];
            addIdea.Effort = card.Element.FindAllCss("i[class='icon-wrench']").Count();
            addIdea.EffortLevel = Enums.levels[addIdea.Effort];
            addIdea.ImplementSelect = new HpgElement(card.Element.FindXPath(".//input[@ng-model='idea.updateImplementationStatus']"));
            addIdea.ImplementedStatus = Enums.ImplementedStatusString[card.Element.FindXPath(".//p[@class='autoImplementedStatus']/span").Text.Trim()];
            return addIdea;
        }
        
        public void SortIdeasBy(string columnName, string order = "ASCENDING")
        {
            SuperTest.WriteReport("Sorting by " + columnName + " " + order);
            AutomationCore.base_tests.BaseTest.AdjustMaxTimeout(240);
            var headerLink = browser.FindXPath("//a[contains(@id,'sortOrder') and .='" + columnName + "'][1]", new Options(){Match = Match.First});
            //HpgElement headerLink = new HpgElement(browser.FindId("publishIdeasTable").FindLink(columnName));
            //HpgAssert.True(headerLink.Element.Exists(), "Verify header link exists");
            headerLink.SendKeys(OpenQA.Selenium.Keys.Home);
            headerLink.Hover();
            headerLink.Hover();
            System.Threading.Thread.Sleep(2000);
            headerLink.SendKeys(OpenQA.Selenium.Keys.Enter);
            //headerLink.Click();
            System.Threading.Thread.Sleep(20000);
            if (order.ToLower().Contains("desc"))
            {
                //Sort Descending
                while (!headerLink.FindXPath("./..").Text.Contains("▼"))
                {
                    headerLink.Hover();
                    headerLink.Hover();
                    System.Threading.Thread.Sleep(2000);
                    headerLink.SendKeys(OpenQA.Selenium.Keys.Enter);
                    //headerLink.Click();
                    System.Threading.Thread.Sleep(5000);
                }
            }
            else
            {
                //Sort Ascending
                while (!headerLink.FindXPath("./..").Text.Contains("▲"))
                {
                    headerLink.Hover();
                    headerLink.Hover();
                    System.Threading.Thread.Sleep(2000);
                    headerLink.SendKeys(OpenQA.Selenium.Keys.Enter);
                    //headerLink.Click();
                    System.Threading.Thread.Sleep(5000);
                }
            }
            AutomationCore.base_tests.BaseTest.ResetMaxTimeout();
        }

        #endregion
    }
}
