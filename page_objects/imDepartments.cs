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
    class imDepartments : imIdeaDetails
    {
        public imDepartments(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        #region Objects

        public HpgElement FilterButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Filter"));
            }
        }

        public HpgElement ClearFilterButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Clear Filter"));
            }
        }

        public HpgElement searchDepartment
        {
            get
            {
                return new HpgElement(browser.FindId("searchString"));
            }
        }

        public HpgElement newDepartmentName
        {
            get
            {
                return new HpgElement(browser.FindXPath("//div[@id='content']//input[@name='departmentName']"));
            }
        }

        public HpgElement newDepartmentCreateButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Create Department"));
            }
        }

        public HpgElement departmentRow(string departmentName)
        {
            return new HpgElement(browser.FindXPath("//form[@action='/Admin/Department/Create']/table//tr[td[.='" + departmentName + "']]"));
        }

        #endregion
    }
}
