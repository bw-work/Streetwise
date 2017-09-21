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
    internal class Departments : ConfigMaster
    {
        public Departments(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        #region CustomClasses

        public class Create
        {
            public HpgElement Name;
            public HpgElement CreateButton;
        }

        public class Department
        {
            public string Name;
            public DateTime CreatedDate;
            public HpgElement EditButton;
            public HpgElement DeleteButton;
            public HpgElement UnDeleteButton;
        }

        public class EditDep : BaseEdit
        {
            public HpgElement Name;
        }
        #endregion

        #region Objects

        public EditDep EditDepartment
        {
            get
            {
                return new EditDep()
                    {
                        Name = new HpgElement(EditDialog.Element.FindXPath(".//input[@name='deptName']")),
                        SaveButton = new HpgElement(EditDialog.Element.FindButton("Save")),
                        CancelButton = new HpgElement(EditDialog.Element.FindButton("Canel"))
                    };
            }
        }

        public HpgElement CreateForm
        {
            get { return new HpgElement(browser.FindXPath("//form[@name='createForm']")); }
        }

        public Create CreateDepartment
        {
            get
            {
                return new Create()
                    {
                        CreateButton = new HpgElement(CreateForm.Element.FindButton("Create")),
                        Name = new HpgElement(CreateForm.Element.FindXPath(".//input[@name='deptName']"))
                    };
            }
        }

        public List<Department> DepartmentsList
        {
            get
            {
                ScrollToBottom();
                browser.FindXPath("//tr[@ng-repeat='department in viewData']", new Options() {Match = Match.First})
                       .Now();
                return (from row in browser.FindAllXPath("//tr[@ng-repeat='department in viewData']")
                        select new Department()
                            {
                                Name = row.FindXPath(".//td[1]").Text.Trim(),
                                CreatedDate = DateTime.Parse(row.FindXPath(".//td[2]").Text.Trim()),
                                EditButton =
                                    new HpgElement(
                            row.FindXPath(".//a[@ng-click='edit(department, departmentToFormData)']")),
                                DeleteButton = new HpgElement(row.FindXPath(".//a[@ng-click='doDelete(department)']")),
                                UnDeleteButton = new HpgElement(row.FindButton("Undo Delete"))
                            }).ToList();
            }
        }
        
        #endregion

        #region Methods
        #endregion
    }
}
