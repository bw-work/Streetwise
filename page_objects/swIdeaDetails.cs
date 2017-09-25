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
    class swIdeaDetails : imEditIdea
    {
        public swIdeaDetails(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        #region Objects

        public HpgElement DetailsTab
        {
            get
            {
                return new HpgElement(browser.FindId("TabStandardDetails"));
            }
        }


        public HpgElement IdeaAuthor
        {
            get
            {
                return new HpgElement(browser.FindXPath("//div[@id='StandardDetails']/h3[.='Author']/following-sibling::p[1]"));
            }
        }

        public HpgElement IdeaStatus
        {
            get
            {
                //return new HpgElement(browser.FindXPath("//div[@id='StandardDetails']/h3[.='Status']/following-sibling::p[1]"));
                return new HpgElement(browser.FindId("status"));
            }
        }

        public HpgElement IdeaCreated
        {
            get
            {
                return new HpgElement(browser.FindXPath("//div[@id='StandardDetails']/h3[.='Created']/following-sibling::p[1]"));
            }
        }

        public HpgElement IdeaUpdated
        {
            get
            {
                return new HpgElement(browser.FindXPath("//div[@id='StandardDetails']/h3[.='Updated']/following-sibling::p[1]"));
            }
        }

        public HpgElement IdeaComment
        {
            get
            {
                return new HpgElement(browser.FindXPath("//div[@id='comments']/table/tbody/tr/td"));
            }
        }

       public HpgElement EditButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Edit"));
            }
        }

        public HpgElement DeleteButton
        {
            get
            {
                return new HpgElement(browser.FindButton("Delete"));
            }
        }


        #endregion

        #region Actions


        #endregion
    }
}
