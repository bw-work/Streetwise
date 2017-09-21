using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Coypu;
using AutomationCore;
using AutomationCore.utility;
using System.Windows.Forms;
using HtmlAgilityPack;
using OpenQA.Selenium.IE;
using System.Xml;
using System.Xml.Linq;
using Match = Coypu.Match;

namespace IdeaManagement.page_objects
{
    class FacilitySavings : imMaster
    {
        public FacilitySavings(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        #region CustomObjects

        public class SavingsRowBase
        {
            public int IdeaId;
            public string IdeaName;
            public string Opportunity;
            public string Category;
            public decimal? Gross;
        }

        public class SavingsRow : SavingsRowBase
        {
            public HpgElement Actions;
        }

        public class BaselineRow
        {
            public int IdeaId;
            public string IdeaName;
            public string View;
            public decimal? Baseline;
        }

        public class IncrementalRow
        {
            public int IdeaId;
            public string IdeaName;
            public string View;
            public string Category;
            public string Opportunity;
            public decimal? Baseline;
            public decimal? Incremental;
        }

        #endregion

        #region objects

        public HpgElement GrossSavingsTab
        {
            get
            {
                return new HpgElement(browser.FindId("tabSavings"));
            }
        }

        public HpgElement BaselinesTab
        {
            get
            {
                return new HpgElement(browser.FindId("baselinesTab"));
            }
        }

        public HpgElement IncrementalSavingsTab
        {
            get
            {
                return new HpgElement(browser.FindId("tabIncrementalSavings"));
            }
        }

        private HpgElement TabSavings
        {
            get
            {
                return new HpgElement(browser.FindId("savingsTab"));
            }
        }

        private HpgElement TabBaselines
        {
            get
            {
                return new HpgElement(browser.FindId("tabBaselines"));
            }
        }

        private HpgElement TabIncremental
        {
            get { return new HpgElement(browser.FindId("incrementalSavingsTab")); }
        }

        private HpgElement TabUpload
        {
            get
            {
                return new HpgElement(browser.FindId("uploadTab"));
            }
        }

        public HpgElement EditDialog
        {
            get
            {
                return new HpgElement(browser.FindId("editDialog"));
            }
        }

        #endregion

        #region GrossSavingsTab
        public class SavingsTab : FacilitySavings
        {
            public ElementScope element;
            public SavingsTab(BrowserSession currentBrowser)
                : base(currentBrowser)
            {
                base.TabSavings.Click(1);
                element = TabSavings.Element;
            }

            public HpgElement CoidTextBox
            {
                get
                {
                    return new HpgElement(element.FindId("coIdAndName"));
                }
            }

            public HpgElement TableTotal
            {
                get
                {
                    return new HpgElement(element.FindId("autoTotal"));
                }
            }

            public HpgElement SavingsMonthDropDown
            {
                get
                {
                    return new HpgElement(element.FindId("FacilitySavingsMonth"));
                }
            }

            public HpgElement SavingsYearDropDown
            {
                get
                {
                    return new HpgElement(element.FindId("FacilitySavingsYear"));
                }
            }

            public HpgElement SearchSavings
            {
                get
                {
                    return new HpgElement(element.FindId("btnSearchSavings"));
                }
            }

            public HpgElement EnterSavingsTextBox
            {
                get
                {
                    return new HpgElement(EditDialog.Element.FindField("correctedSavings"));
                }
            }

            public HpgElement EnterSavingsSaveButton
            {
                get
                {
                    return new HpgElement(EditDialog.Element.FindButton("Save"));
                }
            }

            public HpgElement EnterSavingsCancelButton
            {
                get
                {
                    return new HpgElement(EditDialog.Element.FindButton("Cancel"));
                }
            }

            public HpgElement TotalYearToDate
            {
                get
                {
                    return new HpgElement(element.FindXPath(".//div[@ng-show='ytd']//h3"));
                }
            }

            public void EnterSavings(SavingsRow row, decimal value)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (EditDialog.Element.Exists(new Options() {Timeout = TimeSpan.FromSeconds(3)})) break;
                    new HpgElement(row.Actions.Element.FindXPath(".//a")).Click();
                }
                HpgAssert.True(EditDialog.Element.Exists(new Options() { Timeout = TimeSpan.FromSeconds(3) }), "Verify Edit Savings dialog is open");
                EnterSavingsTextBox.Type(value.ToString());
                for (int i = 0; i < 5; i++)
                {
                    if (EditDialog.Element.Missing(new Options() { Timeout = TimeSpan.FromSeconds(3) })) break;
                    EnterSavingsSaveButton.Click(2);
                }
                HpgAssert.True(EditDialog.Element.Missing(new Options() { Timeout = TimeSpan.FromSeconds(3) }), "Verify Edit Savings dialog is no longer open");
            }

            public List<SavingsRowBase> SavingsRowBases
            {
                get
                {
                    if (element.HasContent("No results found. Please modify your search criteria.",
                                           new Options() {Timeout = TimeSpan.FromSeconds(5)})) return null;
                    try
                    {
                        element.FindXPath("//tr[@ng-repeat='savings in viewData']", new Options() {Match = Match.First}).Now();
                        element.FindXPath("//tr[@ng-repeat='savings in viewData']", new Options() {Match = Match.First}).SendKeys(OpenQA.Selenium.Keys.End);
                    }
                    catch (Exception)
                    {
                    }
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(element.FindXPath(".//table[tbody/tr[@ng-repeat='savings in viewData']]").OuterHTML);
                    return (from r in doc.DocumentNode.SelectNodes("//tr[@ng-repeat='savings in viewData']")
                            select new SavingsRowBase()
                                {
                                    IdeaId = int.Parse(r.SelectSingleNode("./td[1]").InnerText.Split(' ').First()),
                                    IdeaName = r.SelectSingleNode("./td[1]").InnerText.Trim(),
                                    Opportunity = r.SelectSingleNode("./td[2]").InnerText.Trim(),
                                    Category = r.SelectSingleNode("./td[3]").InnerText.Trim(),
                                    Gross = string.IsNullOrEmpty(Regex.Replace(r.SelectSingleNode("./td[4]").InnerText, "[^0-9]+", "")) ? (decimal?)null : decimal.Parse(Regex.Replace(r.SelectSingleNode("./td[4]").InnerText, "[^0-9.]+", ""))
                                }).ToList();
                }
            }

            public List<SavingsRow>  SavingsRows
            {
                get
                {
                    try
                    {
                        element.FindXPath("//tr[@ng-repeat='savings in viewData']",
                                          new Options() {Timeout = TimeSpan.FromSeconds(5)}).Now();
                    }
                    catch (Exception)
                    {
                    }
                    return (from r in element.FindAllXPath("//tr[@ng-repeat='savings in viewData']")
                            select new SavingsRow()
                                {
                                    IdeaId = int.Parse(r.FindXPath("./td[1]").Text.Split(' ').First()),
                                    IdeaName = r.FindXPath("./td[1]").Text.Trim(),
                                    Opportunity = r.FindXPath("./td[2]").Text.Trim(),
                                    Category = r.FindXPath("./td[3]").Text.Trim(),
                                    Gross =
                                        r.FindXPath("./td[4]").Text.Trim().Equals("N/A")
                                            ? (decimal?)null
                                            : decimal.Parse(r.FindXPath("./td[4]").Text.Trim().Replace("$", "").Replace(",","")),
                                    Actions = new HpgElement(r.FindXPath("./td[5]"))
                                }).ToList();
                }
            }

        }
        #endregion

        #region BaselineTab
        public class BaselineTab : FacilitySavings
        {
            public ElementScope element;
            public BaselineTab(BrowserSession currentBrowser)
                : base(currentBrowser)
            {
                //browser.FindId("baselinesTab").Click();
                TabBaselines.Click(1);
                element = BaselinesTab.Element;
            }

            public HpgElement CoidTextBox
            {
                get
                {
                    return new HpgElement(element.FindId("coIdAndName"));
                }
            }

            public HpgElement SavingsYearDropDown
            {
                get
                {
                    return new HpgElement(element.FindId("FacilitySavingsYear"));
                }
            }

            public HpgElement SearchSavings
            {
                get
                {
                    return new HpgElement(element.FindId("btnSearchBaselines"));
                }
            }

            public List<BaselineRow> BaselineRows
            {
                get
                {
                    try
                    {
                        element.FindXPath("//tr[@ng-repeat='idea in viewData']", new Options() {Match = Match.First}).Now();
                        element.FindXPath("//tr[@ng-repeat='idea in viewData']", new Options() {Match = Match.First}).SendKeys(OpenQA.Selenium.Keys.End);
                    }
                    catch (Exception)
                    {}
                    if (element.HasContent("No results found. Please modify your search criteria.")) return null;
                    ScrollToBottom();
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(element.FindXPath(".//table[tbody/tr[@ng-repeat='idea in viewData']]").OuterHTML);
                    return (from r in doc.DocumentNode.SelectNodes("//tr[@ng-repeat='idea in viewData']")
                            select new BaselineRow()
                                {
                                    Baseline = r.SelectSingleNode("./td[3]").InnerText.Trim().Equals("N/A")
                                                   ? (decimal?) null
                                                   : decimal.Parse(r.SelectSingleNode("./td[3]").InnerText.Trim(), NumberStyles.Currency),
                                    IdeaId = int.Parse(r.SelectSingleNode("./td[1]").InnerText.Split(' ').First().Trim()),
                                    IdeaName = r.SelectSingleNode("./td[1]").InnerText,
                                    View = r.SelectSingleNode("./td[2]").InnerText
                                }).ToList();
                }
            }

        }
        #endregion

        #region IncrementalTab

        public class IncrementalTab : FacilitySavings
        {
            public ElementScope element;

            public IncrementalTab(BrowserSession currentBrowser)
                : base(currentBrowser)
            {
                
                TabIncremental.Click(1);
                element = IncrementalSavingsTab.Element;
            }

            public HpgElement CoidTextBox
            {
                get
                {
                    return new HpgElement(element.FindId("coIdAndName"));
                }
            }

            public HpgElement TableTotal
            {
                get
                {
                    return new HpgElement(element.FindId("autoTotal"));
                }
            }

            public HpgElement SavingsMonthDropDown
            {
                get
                {
                    return new HpgElement(element.FindId("FacilitySavingsMonth"));
                }
            }

            public HpgElement SearchSavings
            {
                get
                {
                    return new HpgElement(element.FindId("btnSearchIncrementalSavings"));
                }
            }

            public List<IncrementalRow> IncrementalRows
            {
                get
                {
                    try
                    {
                        element.FindXPath("//tr[@ng-repeat='savings in viewData']", new Options() {Match = Match.First}).Now();
                        element.FindXPath("//tr[@ng-repeat='savings in viewData']", new Options() {Match = Match.First}).SendKeys(OpenQA.Selenium.Keys.End);
                    }
                    catch (Exception)
                    {
                    }
                    if (element.HasContent("No results found. Please modify your search criteria.")) return null;
                    ScrollToBottom();
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(element.FindXPath(".//table[tbody/tr[@ng-repeat='savings in viewData']]").OuterHTML);
                    return (from r in doc.DocumentNode.SelectNodes("//tr[@ng-repeat='savings in viewData']")
                            select new IncrementalRow()
                                {
                                    IdeaId = int.Parse(r.SelectSingleNode("./td[1]").InnerText.Split(' ').First()),
                                    IdeaName = r.SelectSingleNode("./td[1]").InnerText.Trim(),
                                    Opportunity = r.SelectSingleNode("./td[3]").InnerText.Trim(),
                                    Category = r.SelectSingleNode("./td[2]").InnerText.Trim(),
                                    Baseline =
                                        r.SelectSingleNode("./td[4]").InnerText.Trim().Equals("N/A")
                                            ? (decimal?)null
                                            : decimal.Parse(r.SelectSingleNode("./td[4]").InnerText.Trim(), NumberStyles.Currency),
                                    Incremental =
                                        r.SelectSingleNode("./td[5]").InnerText.Trim().Equals("N/A")
                                            ? (decimal?)null
                                            : decimal.Parse(r.SelectSingleNode("./td[5]").InnerText.Trim(), NumberStyles.Currency),
                                }).ToList();
                }
            }

        }

        #endregion

    }
}
