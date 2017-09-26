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
    class Dashboard : swMaster
    {
        public Dashboard(BrowserSession currentBrowser)
            : base(currentBrowser)
        {
        }

        #region CustomObjects

        public class QueueIdea
        {
            public int IdeaId;
            public HpgElement IdeaName;
            public string SubmittedBy;
            public DateTime UpdatedDate;
            public void Click()
            {
                IdeaName.Hover();
                IdeaName.Click();
                if (!IdeaName.Element.Missing(new Options(){Timeout = TimeSpan.FromSeconds(1)})) IdeaName.Element.SendKeys(OpenQA.Selenium.Keys.Enter);
            }
        }

        #endregion

        #region Objects

        public HpgElement QueueWidget
        {
            get
            {
                return new HpgElement(browser.FindId("queueWidget"));
            }
        }

        public HpgElement SavedIdeasWidget
        {
            get
            {
                return new HpgElement(browser.FindId("draftedIdeasWidget"));
            }
        }

        public List<HpgElement> QueueIdeas
        {
            get
            {
                QueueWidget.Element.FindXPath(".//a", new Options(){Match = Match.First}).SendKeys(OpenQA.Selenium.Keys.End);
                QueueWidget.Element.FindXPath(".//a", new Options() { Match = Match.First }).SendKeys(OpenQA.Selenium.Keys.Home);
                return (from i in QueueWidget.Element.FindAllXPath(".//div[@ng-repeat='idea in ideaQueue']") select new HpgElement(i)).ToList();
            }
        }

        public HpgElement RMIWidget
        {
            get
            {
                return new HpgElement(browser.FindId("reqInfoWidget"));
            }
        }

        public List<HpgElement> RMIIdeas
        {
            get
            {
                RMIWidget.Element.FindXPath(".//a", new Options() { Match = Match.First }).SendKeys(OpenQA.Selenium.Keys.End);
                RMIWidget.Element.FindXPath(".//a", new Options() { Match = Match.First }).SendKeys(OpenQA.Selenium.Keys.Home);
                return (from i in RMIWidget.Element.FindAllXPath(".//div[@ng-repeat='idea in ideaMoreInformation']") select new HpgElement(i)).ToList();
            }
        }

        public List<HpgElement> SavedIdeas
        {
            get
            {
                RMIWidget.Element.FindXPath(".//a", new Options() { Match = Match.First }).SendKeys(OpenQA.Selenium.Keys.End);
                RMIWidget.Element.FindXPath(".//a", new Options() { Match = Match.First }).SendKeys(OpenQA.Selenium.Keys.Home);
                return (from i in RMIWidget.Element.FindAllXPath(".//div[@ng-repeat='idea in myDraftedIdeas']") select new HpgElement(i)).ToList();
            }
        }

        public HpgElement NewsFeedWidget
        {
            get
            {
                return new HpgElement(browser.FindId("newsFeedWidget"));
            }
        }

        #endregion

        #region Actions

        public List<QueueIdea> GetQueueIdeas(List<HpgElement> ideas, int limit = 0, string IdeaName = "", int IdeaId = 0)
        {
            List<QueueIdea> returList = new List<QueueIdea>();
            foreach (HpgElement idea in ideas.Take(limit > 0 ? limit : ideas.Count))
            {
                try
                {
                    //idea.Hover();
                    //idea.Element.FindXPath(".//a", new Options(){Match = Match.First}).Hover();
                    idea.Element.FindXPath(".//a", new Options() {Match = Match.First}).SendKeys(OpenQA.Selenium.Keys.PageDown);
                }
                catch (Exception)
                {
                }
                //idea.Element.SendKeys(OpenQA.Selenium.Keys.Space);
                returList.Add(ParseIdea(idea));
                if (!string.IsNullOrEmpty(IdeaName))
                    if (returList.Any(i => i.IdeaName.Text.EndsWith(IdeaName))) break;
                if (IdeaId > 0)
                    if (returList.Any(i => i.IdeaId.Equals(IdeaId))) break;
            }
            return returList;
        }

        public QueueIdea ParseIdea(HpgElement idea)
        {
            QueueIdea addIdea = new QueueIdea();
            addIdea.IdeaName = new HpgElement(idea.Element.FindCss("#newsFeedWidget > div > div.scrollbar-outer.ng-isolate-scope.scroll-content > div.widgetBodyContent.ng-scope > div > div.span11 > div > p.marginTop2 > a"));
            addIdea.IdeaId = int.Parse(addIdea.IdeaName.Text.Split('-').First().Trim());
            addIdea.SubmittedBy = idea.Element.FindCss("#newsFeedWidget > div > div.scrollbar-outer.ng-isolate-scope.scroll-content > div.widgetBodyContent.ng-scope > div > div.span11 > div > p.ng-binding").Text.Replace("Submitted By:", "").Trim();
            string ud = string.Join(" ", (from d in idea.Element.FindAllXPath(".//div[@class='widgetDate']/span") select d.Text.Trim()));
            addIdea.UpdatedDate = DateTime.Parse(ud);
            return addIdea;
        }

        public List<QueueIdea> GetQueueIdeas(string IdeaName = "", int IdeaId = 0)
        {
            return GetQueueIdeas(QueueIdeas, IdeaName:IdeaName, IdeaId:IdeaId);
        }

        public List<QueueIdea> GetQueueIdeas(bool MatchTitle, string matchThisTitle)
        {
            if (!MatchTitle) return GetQueueIdeas();
            return GetQueueIdeas(QueueIdeas.Where(q => q.Text.Contains(matchThisTitle)).ToList());
        }

        public List<QueueIdea> GetQueueIdeas(int limit)
        {
            return GetQueueIdeas(QueueIdeas, limit);
        }

        public List<QueueIdea> GetRMIIdeas()
        {
            return GetQueueIdeas(RMIIdeas);
        }

        public List<QueueIdea> GetRMIIdeas(bool MatchTitle, string matchThisTitle)
        {
            if (!MatchTitle) return GetRMIIdeas();
            return GetQueueIdeas(RMIIdeas.Where(q => q.Text.Contains(matchThisTitle)).ToList());
        }

        public List<QueueIdea> GetRMIIdeas(int limit)
        {
            return GetQueueIdeas(RMIIdeas, limit);
        }

        public List<QueueIdea> GetSavedIdeas()
        {
            return GetQueueIdeas(SavedIdeas);
        }

        #endregion
    }
}
