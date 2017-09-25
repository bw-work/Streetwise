using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coypu;
using AutomationCore;
using AutomationCore.utility;
using System.Windows.Forms;
using HtmlAgilityPack;
using OpenQA.Selenium.IE;
using System.Xml;
using System.Xml.Linq;

namespace Streetwise.page_objects
{
    class swAllIdeas : swIdeaListMaster
    {
        public swAllIdeas(BrowserSession currentBrowser) : base(currentBrowser)
        {
        }



        #region Objects
        #endregion

        #region Actions



        //public List<AllIdea> OLDGetAllIdeas()
        //{
        //    List<AllIdea> returnList = new List<AllIdea>();
        //    if (!browser.FindXPath("//*[@id='allIdeasTable']//tr[td[8]]", Options.First).Exists())
        //    {
        //        return returnList;
        //    }
        //    foreach (SnapshotElementScope row in browser.FindAllXPath("//*[@id='allIdeasTable']//tr[td[8]]"))
        //    {
        //        AllIdea addIdea = new AllIdea();
        //        addIdea.IdeaNumber = new HpgElement(row.FindXPath("td[1]"));
        //        addIdea.IdeaName = new HpgElement(row.FindXPath("td[2]"));
        //        addIdea.Author = row.FindXPath("td[3]").Text.Trim();
        //        addIdea.Assigned = row.FindXPath("td[4]").Text.Trim();
        //        addIdea.Status = row.FindXPath("td[5]").Text.Trim();
        //        addIdea.Created = DateTime.Parse(string.IsNullOrEmpty(row.FindXPath("td[6]").Text.Trim()) ? "01/01/1950" : row.FindXPath("td[6]").Text.Trim());
        //        addIdea.Updated = DateTime.Parse(string.IsNullOrEmpty(row.FindXPath("td[7]").Text.Trim()) ? "01/01/1950" : row.FindXPath("td[6]").Text.Trim());
        //        addIdea.Action = new HpgElement(row.FindXPath("td[8]"));
        //        returnList.Add(addIdea);
        //    }
        //    return returnList;
        //}



        #endregion
    }
}
