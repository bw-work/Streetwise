﻿using System;
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
    class Categories : ConfigMaster
    {
        public Categories(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        #region CustomClasses
        public class Category
        {
            public string Name;
            public string HCAOpportunity;
            public string GPOOpportunity;
            public string NonGPOOpportunity;
            public DateTime CreatedDate;
            public HpgElement EditButton;
            public HpgElement DeleteButton;
            public HpgElement UndoDeleteButton;
        }

        public class CreateCategory
        {
            public HpgElement Name;
            public HpgElement HCAOpportunity;
            public HpgElement GPOOpportunity;
            public HpgElement NonGPOOpportunity;
            public HpgElement CreateButton;
        }

        public class EditCat : BaseEdit
        {
            public HpgElement Name;
            public HpgElement HCAOpportunity;
            public HpgElement GPOOpportunity;
            public HpgElement NonGPOOpportunity;
        }

        #endregion

        #region Objects

        public List<Category> CategoryList
        {
            get
            {
                ScrollToBottom();
                browser.FindXPath("//tr[@ng-repeat='category in viewData']", new Options() {Match = Match.First}).Now();
                return (from r in browser.FindAllXPath("//tr[@ng-repeat='category in viewData']")
                        select GetCategory(r)).ToList();
            }
        }

        public CreateCategory Create
        {
            get
            {
                return new CreateCategory()
                    {
                        Name = new HpgElement(browser.FindXPath("//input[@name='categoryName']")),
                        HCAOpportunity = new HpgElement(browser.FindXPath("//select[@ng-model='createData.HCAOpportunityId']")),
                        GPOOpportunity = new HpgElement(browser.FindXPath("//select[@ng-model='createData.GPOOpportunityId']")),
                        NonGPOOpportunity = new HpgElement(browser.FindXPath("//select[@ng-model='createData.NonGPOOpportunityId']")),
                        CreateButton = new HpgElement(browser.FindButton("Create"))
                    };
            }
        }

        public EditCat EditCategory
        {
            get
            {
                return new EditCat()
                    {
                        Name = new HpgElement(EditDialog.Element.FindXPath(".//input[@name='categoryName']")),
                        HCAOpportunity = new HpgElement(EditDialog.Element.FindXPath(".//select[@ng-model='editData.HCAOpportunityId']")),
                        GPOOpportunity = new HpgElement(EditDialog.Element.FindXPath(".//select[@ng-model='editData.GPOOpportunityId']")),
                        NonGPOOpportunity = new HpgElement(EditDialog.Element.FindXPath(".//select[@ng-model='editData.NonGPOOpportunityId']")),
                        SaveButton = new HpgElement(EditDialog.Element.FindButton("Save")),
                        CancelButton = new HpgElement(EditDialog.Element.FindButton("Cancel"))
                    };
            }
        }

        
        #endregion

        #region Methods

        public Category GetCategory(ElementScope row)
        {
            return new Category()
                {
                    Name = row.FindXPath(".//td[1]").Text.Trim(),
                    HCAOpportunity = row.FindXPath(".//td[2]").Text.Trim(),
                    GPOOpportunity = row.FindXPath(".//td[3]").Text.Trim(),
                    NonGPOOpportunity = row.FindXPath(".//td[4]").Text.Trim(),
                    CreatedDate = DateTime.Parse(row.FindXPath(".//td[5]").Text.Trim()),
                    EditButton = new HpgElement(row.FindXPath(".//a[@ng-click='edit(category, categoryToFormData)']")),
                    DeleteButton = new HpgElement(row.FindXPath(".//a[@ng-click='doDelete(category)']")),
                    UndoDeleteButton = new HpgElement(row.FindLink("Undo Delete"))
                };
        }

        #endregion
    }
}
