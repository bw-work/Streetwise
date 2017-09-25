using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    class imPublishedIdea : swMaster
    {
        public imPublishedIdea(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        #region Objects

        public HpgElement EffortLevel
        {
            get
            {
                return new HpgElement(browser.FindId("effortlevel"));
            }
        }

        public HpgElement ImpactLevel
        {
            get
            {
                return new HpgElement(browser.FindId("impactlevel"));
            }
        }

        public HpgElement Department
        {
            get
            {
                return new HpgElement(browser.FindId("QualifiedIdea_Department_Name"));
            }
        }

        public HpgElement PublishedDate
        {
            get
            {
                return new HpgElement(browser.FindXPath("//div[b[.='Published:']]"));
            }
        }

        public HpgElement UpdatedDate
        {
            get
            {
                return new HpgElement(browser.FindXPath("//div[b[.='Updated Date:']]"));
            }
        }

        public HpgElement Category
        {
            get
            {
                return new HpgElement(browser.FindId("QualifiedIdea_Category_Name"));
            }
        }

        public HpgElement IdeaTitle
        {
            get
            {
                return new HpgElement(browser.FindId("QualifiedIdea_Title"));
            }
        }

        public HpgElement Attachments
        {
            get
            {
                return new HpgElement(browser.FindId("editIdeaAttachments"));
            }
        }

        public HpgElement Links
        {
            get
            {
                return new HpgElement(browser.FindId("editIdeaLinks"));
            }
        }

        public HpgElement BackButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Back"));
            }
        }

        public HpgElement Description
        {
            get
            {
                //return new HpgElement(browser.FindId("publishedIdeaDescription"));
                return new HpgElement(browser.FindXPath("//*[@id='mainContent']/p[4]"));
            }
        }

        public HpgElement LinkFavorite
        {
            get
            {
                return new HpgElement(browser.FindXPath("//a[img[@alt='Favorite']]"));
            }
        }

        public HpgElement LinkPDF
        {
            get
            {
                return new HpgElement(browser.FindXPath("//a[img[@alt='Download PDF']]"));
            }
        }

        public HpgElement LinkExcel
        {
            get
            {
                return new HpgElement(browser.FindXPath("//a[img[@alt='Download Excel']]"));
            }
        }

        public HpgElement LinkPrint
        {
            get
            {
                return new HpgElement(browser.FindId("printButton"));
            }
        }

        public HpgElement LinkEmail
        {
            get
            {
                return new HpgElement(browser.FindId("emailButton"));
            }
        }

        public HpgElement IdeaNumber
        {
            get
            {
                return new HpgElement(browser.FindId("QualifiedIdea_IdeaId"));
            }
        }

        public HpgElement ImplementationStatusDropDown
        {
            get
            {
                return new HpgElement(browser.FindId("ImplementationStatus"));
            }
        }

        #endregion

        #region validations

        /// <summary>
        /// Verifies all attachments supplied in fileList dictionary parameter are present and href links are correct
        /// </summary>
        /// <param name="fileList">Dictionary containing linkname / filepath</param>
        public void VerifyAttachmentsArePresent(Dictionary<string, string> fileList)
        {
            List<HpgElement> attachments = GetAllAttachments();
            foreach (KeyValuePair<string, string> fileToTest in fileList)
            {
                HpgElement a = attachments.First(b => b.Text.Trim().Equals(fileToTest.Key));
                HpgAssert.True(a.Element.Exists(), "Verify attachment '" + fileToTest.Key + "' is present");
                HpgAssert.Contains(System.Web.HttpUtility.UrlDecode(a.Element["href"]), fileToTest.Value.Split('\\').Last(), "Verify attachment file is correct");
            }
        }

        /// <summary>
        /// Compares details from an Excel export to the details on the Published Idea Details page
        /// </summary>
        /// <param name="excelFileName">Exported Excel file name in the InputObjects directory</param>
        /// <returns>Returns string list of all fields that did not match</returns>
        public string CompareExcelFileToDetailsPage(string excelFileName)
        {
            IEnumerable<AutomationCore.input_objects.InputObject> ideaExport = FileReader.getInputObjects(excelFileName, "Idea");
            string errorOut = "";
            if(IdeaNumber.Text.Trim() != ideaExport.ElementAt(0).fields["Idea Number"]) errorOut = "Idea Number did not match!\n";
            if (IdeaTitle.Text.Trim() != ideaExport.ElementAt(0).fields["Idea Name"]) errorOut += "Idea Name did not match!\n";
            if(Department.Text.Trim() != ideaExport.ElementAt(0).fields["Department"]) errorOut += "Department did not match!\n";
            if (Category.Text.Trim() != ideaExport.ElementAt(0).fields["Category"]) errorOut += "Category did not match!\n";
            if (GetEffortLevel() != ideaExport.ElementAt(0).fields["Effort"]) errorOut += "Effort did not match!\n";
            if (GetImpactLevel() != ideaExport.ElementAt(0).fields["Impact"]) errorOut += "Impact did not match!\n";
            //BUG: DE2023 Verify publish/update date after defect is fixed
            //HpgAssert.AreEqual(IMPublishedIdea.PublishedDate.Text.Trim(), ideaExport.ElementAt(0).fields["Publish Date"], "Verify Idea Name is correct");
            //HpgAssert.AreEqual(IMPublishedIdea.UpdatedDate.Text.Trim(), ideaExport.ElementAt(0).fields["Update Date"], "Verify Idea Name is correct");

            if (AZCharOnly(Description.Text.ToLower()) !=
                AZCharOnly(ideaExport.ElementAt(0).fields["Description"].ToLower()))
                errorOut += "Description did not match!";
            return errorOut;
        }

        public void VerifyLinksArePresent(Dictionary<string, string> linksList)
        {
            List<HpgElement> links = GetAllLinks();
            foreach (KeyValuePair<string, string> linkToTest in linksList)
            {
                HpgElement a = links.First(b => b.Text.Trim().Equals(linkToTest.Key));
                HpgAssert.True(a.Element.Exists(), "Verify link '" + linkToTest.Key + "' is present");
                HpgAssert.Contains(System.Web.HttpUtility.UrlDecode(a.Element["href"]), linkToTest.Value, "Verify link URL is correct");
            }
        }

        #endregion

        #region actions

        public void SaveExcel(string saveFileName)
        {
            System.IO.File.Delete(saveFileName);
            if (remoteWebDriver.Capabilities.BrowserName.ToLower().Contains("internet"))
            {
                IESaveResource(LinkExcel.Element["href"], saveFileName);
            }
            else
            {
                browser.SaveWebResource(LinkExcel.Element["href"], saveFileName);
            }
            AutomationCore.base_tests.BaseTest.WriteReport("Excel export saved to " + saveFileName);
        }


        public void SavePDF(string saveFileName)
        {
            System.IO.File.Delete(saveFileName);
            if (remoteWebDriver.Capabilities.BrowserName.ToLower().Contains("internet"))
            {
                IESaveResource(LinkPDF.Element["href"], saveFileName);
            }
            else
            {
                browser.SaveWebResource(LinkPDF.Element["href"], saveFileName);
            }
            AutomationCore.base_tests.BaseTest.WriteReport("PDF export saved to " + saveFileName);
        }

        public void OpenEmailDialog()
        {
            LinkEmail.Click();
            WaitForThrobber();
            if(!browser.FindId("emailDialog").Exists())
                LinkEmail.Click(2);
            HpgAssert.True(browser.FindId("emailDialog").Exists(), "Email Dialog is open");
        }

        public string GetEffortLevel()
        {
            switch (EffortLevel.Element.FindAllXPath(".//i").Count())
            {
                case 1:
                    return "Low";
                    break;
                case 2:
                    return "Medium";
                    break;
                case 3:
                    return "High";
                    break;
            }
            return "";
        }

        public string GetImpactLevel()
        {
            switch (ImpactLevel.Element.FindAllXPath(".//i").Count())
            {
                case 1:
                    return "Low";
                    break;
                case 2:
                    return "Medium";
                    break;
                case 3:
                    return "High";
                    break;
            }
            return "";
        }

        public List<HpgElement> GetAllLinks()
        {
            if(Links.Element.Missing()) return new List<HpgElement>();
            return (from l in Links.Element.FindAllXPath(".//a") select new HpgElement(l)).ToList();
        }

        public List<HpgElement> GetAllAttachments()
        {
            if(Attachments.Element.Missing()) return new List<HpgElement>();
            return (from l in Attachments.Element.FindAllXPath(".//a") select new HpgElement(l)).ToList();
        }

        #endregion
    }
}
