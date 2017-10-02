using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Streetwise.page_objects;
using Coypu;
using AutomationCore.utility;

namespace Streetwise.page_assertions
{
    class PublishedIdeasAssertions : BaseAssertions
    {
        private swPublishedIdeas publishedIdeas;

        public PublishedIdeasAssertions(BrowserSession browser, swPublishedIdeas publishedIdeas) : base(browser)
        {
            this.publishedIdeas = publishedIdeas;
        }

        public void AssertPageElements()
        {
            AssertBaseElements();
            HpgAssert.AreEqual(publishedIdeas.pageHeader.Text, "Published Ideas");
            HpgAssert.AreEqual(publishedIdeas.ClearRefinementsTxt.Text, "Clear Refinements");
            HpgAssert.Exists(publishedIdeas.SearchField);
            HpgAssert.Exists(publishedIdeas.SearchButton);
            HpgAssert.AreEqual(publishedIdeas.SortByTxt.Text, "Sort By");
            HpgAssert.Exists(publishedIdeas.SortByDropdown);
            HpgAssert.Exists(publishedIdeas.LinkPDF);
            HpgAssert.Exists(publishedIdeas.LinkExcel);
            HpgAssert.Exists(publishedIdeas.RecordCountTxt);
            HpgAssert.AreEqual(publishedIdeas.RefinementsTxt.Text, "Refinements");
            HpgAssert.AreEqual(publishedIdeas.RefineResultsTxt.Text, "Refine Results");
            HpgAssert.Exists(publishedIdeas.MyBookmarksChkBox);
            HpgAssert.AreEqual(publishedIdeas.MyBookmarksTxt.Text, "My Bookmarks");
            HpgAssert.AreEqual(publishedIdeas.PublishedDateRangeTxt.Text, "Published Date Range");
            HpgAssert.Exists(publishedIdeas.IdeaDateRangeStart);
            HpgAssert.Exists(publishedIdeas.IdeaDataRangeEnd);
            HpgAssert.Exists(publishedIdeas.ReloadIdeasBasedOnRangeButton);
            HpgAssert.AreEqual(publishedIdeas.UpdatedDateTxt.Text, "Updated Date");
            HpgAssert.Exists(publishedIdeas.UpdateDateDropdown);
            HpgAssert.AreEqual(publishedIdeas.DepartmentTxt.Text, "Department");
            HpgAssert.Exists(publishedIdeas.AnyDepartmentCheckBox);
            HpgAssert.AreEqual(publishedIdeas.AnyDepartmentTxt.Text, "Any Department");
            HpgAssert.Exists(publishedIdeas.BusinessCheckBox);
            HpgAssert.AreEqual(publishedIdeas.BusinessTxt.Text, "Business Products/Services/DMS");
            HpgAssert.Exists(publishedIdeas.CardiovascularServicesCheckBox);
            HpgAssert.AreEqual(publishedIdeas.CardiovascularServicesTxt.Text, "Cardiovascular Services");
            HpgAssert.Exists(publishedIdeas.CentralSterileProcessingCheckBox);
            HpgAssert.AreEqual(publishedIdeas.CentralSterileProcessingTxt.Text, "Central Sterile Processing");
            HpgAssert.Exists(publishedIdeas.ContractConversionCheckBox);
            HpgAssert.AreEqual(publishedIdeas.ContractConversionTxt.Text, "Contract Conversion");
            HpgAssert.Exists(publishedIdeas.EmergencyServicesCheckBox);
            HpgAssert.AreEqual(publishedIdeas.EmergencyServicesTxt.Text, "Emergency Services");
            HpgAssert.Exists(publishedIdeas.EndoscopyCheckBox);
            HpgAssert.AreEqual(publishedIdeas.EndoscopyTxt.Text, "Endoscopy");
            HpgAssert.Exists(publishedIdeas.EnvironmentalServicesCheckBox);
            HpgAssert.AreEqual(publishedIdeas.EnvironmentalServicesTxt.Text, "Environmental Services");
            HpgAssert.Exists(publishedIdeas.FacilitySafetyCheckBox);
            HpgAssert.AreEqual(publishedIdeas.FacilitySafetyTxt.Text, "Facility Safety");
            HpgAssert.Exists(publishedIdeas.FoodNutritionServicesCheckBox);
            HpgAssert.AreEqual(publishedIdeas.FoodNutritionServicesTxt.Text, "Food/Nutrition Services");
            HpgAssert.Exists(publishedIdeas.ImagingCheckBox);
            HpgAssert.AreEqual(publishedIdeas.ImagingTxt.Text, "Imaging");
            HpgAssert.Exists(publishedIdeas.LaboratoryCheckBox);
            HpgAssert.AreEqual(publishedIdeas.LaboratoryTxt.Text, "Laboratory");
            HpgAssert.Exists(publishedIdeas.MaterialManagementCheckBox);
            HpgAssert.AreEqual(publishedIdeas.MaterialManagementTxt.Text, "Material Management");
            HpgAssert.Exists(publishedIdeas.NICUPediatricsCheckBox);
            HpgAssert.AreEqual(publishedIdeas.NICUPediatricsTxt.Text, "NICU/Pediatrics");
            HpgAssert.Exists(publishedIdeas.NursingServicesCheckBox);
            HpgAssert.AreEqual(publishedIdeas.NursingServicesTxt.Text, "Nursing Services");
            HpgAssert.Exists(publishedIdeas.ObstetricsPerinatalCheckBox);
            HpgAssert.AreEqual(publishedIdeas.ObstetricsPerinatalTxt.Text, "Obstetrics/Perinatal");
            HpgAssert.Exists(publishedIdeas.OtherClinicalServiceCheckBox);
            HpgAssert.AreEqual(publishedIdeas.OtherClinicalServiceTxt.Text, "Other Clinical Service");
            HpgAssert.Exists(publishedIdeas.OtherSupportServiceCheckBox);
            HpgAssert.AreEqual(publishedIdeas.OtherSupportServiceTxt.Text, "Other Support Service");
            HpgAssert.Exists(publishedIdeas.PharmacyCheckBox);
            HpgAssert.AreEqual(publishedIdeas.PharmacyTxt.Text, "Pharmacy");
            HpgAssert.Exists(publishedIdeas.PhysicianReferenceContractsCheckBox);
            HpgAssert.AreEqual(publishedIdeas.PhysicianReferenceContractsTxt.Text, "Physician Preference Contracts");
            HpgAssert.Exists(publishedIdeas.DepartmentProductFormularyCheckBox);
            HpgAssert.AreEqual(publishedIdeas.DepartmentProductFormularyTxt.Text, "Product Formulary");
            HpgAssert.Exists(publishedIdeas.RadiologyCheckBox);
            HpgAssert.AreEqual(publishedIdeas.RadiologyTxt.Text, "Radiology");
            HpgAssert.Exists(publishedIdeas.RespiratoryTherapyCheckBox);
            HpgAssert.AreEqual(publishedIdeas.RespiratoryTherapyTxt.Text, "Respiratory Therapy");
            HpgAssert.Exists(publishedIdeas.SurgicalServicesCheckBox);
            HpgAssert.AreEqual(publishedIdeas.SurgicalServicesTxt.Text, "Surgical Services");
            HpgAssert.AreEqual(publishedIdeas.ShowLessDepartmentsButton.Text, "Show Less");
            HpgAssert.AreEqual(publishedIdeas.ShowMoreDepartmentsButton.Text, "Show More");

            HpgAssert.AreEqual(publishedIdeas.CategoryTxt.Text, "Category");
            HpgAssert.Exists(publishedIdeas.APInfrastructureCheckBox);
            HpgAssert.AreEqual(publishedIdeas.APInfrastructureTxt.Text, "A/P Infrastructure");
            HpgAssert.Exists(publishedIdeas.BloodCheckBox);
            HpgAssert.AreEqual(publishedIdeas.BloodTxt.Text, "Blood/Blood Products");
            HpgAssert.Exists(publishedIdeas.CapitalAcquisitionCheckBox);
            HpgAssert.AreEqual(publishedIdeas.CapitalAcquisitionTxt.Text, "Capital Acquisition");
            HpgAssert.Exists(publishedIdeas.CommoditiesCheckBox);
            HpgAssert.AreEqual(publishedIdeas.CommoditiesTxt.Text, "Commodities");
            HpgAssert.Exists(publishedIdeas.CommodityPharmacyCheckBox);
            HpgAssert.AreEqual(publishedIdeas.CommodityPharmacyTxt.Text, "Commodity - Pharmacy");
            HpgAssert.Exists(publishedIdeas.CVATCostAvoidanceCheckBox);
            HpgAssert.AreEqual(publishedIdeas.CVATCostAvoidanceTxt.Text, "CVAT - Cost Avoidance");
            HpgAssert.Exists(publishedIdeas.CVATCostSavingsCheckBox);
            HpgAssert.AreEqual(publishedIdeas.CVATCostSavingsTxt.Text, "CVAT - Cost Savings");
            HpgAssert.Exists(publishedIdeas.DivManagedDisintermediationCheckBox);
            HpgAssert.AreEqual(publishedIdeas.DivManagedDisintermediationTxt.Text, "Div Managed Disintermediation");
            HpgAssert.Exists(publishedIdeas.DivisionBasedContractingCheckBox);
            HpgAssert.AreEqual(publishedIdeas.DivisionBasedContractingTxt.Text, "Division Based Contracting");
            HpgAssert.Exists(publishedIdeas.HCAPharmacyOperationsProjectCheckBox);
            HpgAssert.AreEqual(publishedIdeas.HCAPharmacyOperationsProjectTxt.Text, "HCA Pharmacy Operations Project");
            HpgAssert.Exists(publishedIdeas.HPGSIPCheckBox);
            HpgAssert.AreEqual(publishedIdeas.HPGSIPTxt.Text, "HPG SIP");
            HpgAssert.Exists(publishedIdeas.HTRoadmapCheckBox);
            HpgAssert.AreEqual(publishedIdeas.HTRoadmapTxt.Text, "HT Roadmap");
            HpgAssert.Exists(publishedIdeas.NonSupplyExpenseSavingsCheckBox);
            HpgAssert.AreEqual(publishedIdeas.NonSupplyExpenseSavingsTxt.Text, "Non-Supply Expense Savings");
            HpgAssert.Exists(publishedIdeas.ORCostPerCaseCheckBox);
            HpgAssert.AreEqual(publishedIdeas.ORCostPerCaseTxt.Text, "OR Cost Per Case");
            HpgAssert.Exists(publishedIdeas.CategoryProductFormularyCheckBox);
            HpgAssert.AreEqual(publishedIdeas.CategoryProductFormularyTxt.Text, "Product Formulary");
            HpgAssert.Exists(publishedIdeas.RoadMapPharmacy2017CheckBox);
            HpgAssert.AreEqual(publishedIdeas.RoadMapPharmacy2017Txt.Text, "RoadMap - Pharmacy - 2017");
            HpgAssert.Exists(publishedIdeas.RoadMapReprocessing2017CheckBox);
            HpgAssert.AreEqual(publishedIdeas.RoadMapReprocessing2017Txt.Text, "RoadMap - Reprocessing - 2017");
            HpgAssert.Exists(publishedIdeas.RoadMap2015CheckBox);
            HpgAssert.AreEqual(publishedIdeas.RoadMap2015Txt.Text, "RoadMap-2015");
            HpgAssert.Exists(publishedIdeas.SMATCostAvoidanceCheckBox);
            HpgAssert.AreEqual(publishedIdeas.SMATCostAvoidanceTxt.Text, "SMAT - Cost Avoidance");
            HpgAssert.Exists(publishedIdeas.SupportServicesEVSCheckBox);
            HpgAssert.AreEqual(publishedIdeas.SupportServicesEVSTxt.Text, "Support Services - EVS");
            HpgAssert.Exists(publishedIdeas.SupportServicesFoodCheckBox);
            HpgAssert.AreEqual(publishedIdeas.SupportServicesFoodTxt.Text, "Support Services - Food");
            HpgAssert.Exists(publishedIdeas.TechnologyCheckBox);
            HpgAssert.AreEqual(publishedIdeas.TechnologyTxt.Text, "Technology");
            HpgAssert.Exists(publishedIdeas.ThirdPartyFreightManagementCheckBox);
            HpgAssert.AreEqual(publishedIdeas.ThirdPartyFreightManagementTxt.Text, "Third Party Freight Management");
            HpgAssert.Exists(publishedIdeas.ShowLessCategoriesButton);
            HpgAssert.Exists(publishedIdeas.ShowMoreCategoriesButton);

            HpgAssert.AreEqual(publishedIdeas.ImpactTxt.Text, "Impact");
            HpgAssert.Exists(publishedIdeas.ImpactCheckBox1);
            HpgAssert.Exists(publishedIdeas.ImpactCheckBox2);
            HpgAssert.Exists(publishedIdeas.ImpactCheckBox3);
            HpgAssert.AreEqual(publishedIdeas.EffortTxt.Text, "Effort");
            HpgAssert.Exists(publishedIdeas.EffortCheckBox1);
            HpgAssert.Exists(publishedIdeas.EffortCheckBox2);
            HpgAssert.Exists(publishedIdeas.EffortCheckBox3);
        }

        public void AssertIdeaCardElements()
        {

        }
    }
}