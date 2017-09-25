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
    class ConfigMaster : swMaster
    {
        public ConfigMaster(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        #region CustomClasses
        public class ConfirmDel
        {
            public HpgElement DeleteButton;
            public HpgElement CancelButton;
        }

        public class UnDel
        {
            public HpgElement UndoDeleteButton;
            public HpgElement CancelButton;
        }

        public class BaseEdit
        {
            public HpgElement SaveButton;
            public HpgElement CancelButton;
        }
        #endregion

        public HpgElement ConfirmDeleteDialog
        {
            get
            {
                return new HpgElement(browser.FindId("deleteDialog"));
            }
        }

        public ConfirmDel ConfirmDelete
        {
            get
            {
                return new ConfirmDel()
                {
                    DeleteButton = new HpgElement(ConfirmDeleteDialog.Element.FindButton("Delete")),
                    CancelButton = new HpgElement(ConfirmDeleteDialog.Element.FindButton("Cancel"))
                };
            }
        }

        public HpgElement EditDialog
        {
            get
            {
                return new HpgElement(browser.FindId("editDialog"));
            }
        }

        public HpgElement ShowDeletedCheckbox
        {
            get
            {
                return new HpgElement(browser.FindXPath("//input[@ng-model='filterDeleted']"));
            }
        }
        
        public HpgElement UnDeleteDialog
        {
            get
            {
                return new HpgElement(browser.FindId("undoDeleteDialog"));
            }
        }

        public UnDel UnDelete
        {
            get
            {
                return new UnDel()
                {
                    UndoDeleteButton = new HpgElement(UnDeleteDialog.Element.FindButton("Undo Delete")),
                    CancelButton = new HpgElement(UnDeleteDialog.Element.FindButton("Cancel"))
                };
            }
        }

    }
}
