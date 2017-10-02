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
    class MyDashboardAssertions : BaseAssertions
    {
        private Dashboard dashboard;

        public MyDashboardAssertions(BrowserSession browser, Dashboard dashboard) : base(browser)
        {
            this.dashboard = dashboard;
        }

        public void AssertPageElements()
        {
            AssertBaseElements();
            HpgAssert.AreEqual(dashboard.pageHeader.Text, "My Dashboard", "The page header is missing or not displayed properly.");
            HpgAssert.Exists(dashboard.NewsFeedWidget, "The News Feed widget is missing or not displayed properly.");
            HpgAssert.AreEqual(dashboard.NewsFeedTxt.Text, "NEWS FEED", "The News Feed text is missing or not displayed properly.");
            HpgAssert.Exists(dashboard.NewsFeedBadge, "The News Feed badge is missing or not displayed properly.");
            HpgAssert.Exists(dashboard.RMIWidget, "The Required Information Needed widget is missing or not displayed properly.");
            HpgAssert.Exists(dashboard.RequiredInformationNeededFlag, "The Required Information Needed flag is missing or not displayed properly.");
            HpgAssert.AreEqual(dashboard.RequiredInformationNeededTxt.Text, "REQUIRED INFORMATION NEEDED", "The Required Information Needed text is missing or not displayed properly.");
            HpgAssert.Exists(dashboard.RequiredInformationNeededBadge, "The Required Information Needed badge is missing or not displayed properly.");
            HpgAssert.Exists(dashboard.QueueWidget, "The Ideas Ready for Review widget is missing or not displayed properly.");
            HpgAssert.AreEqual(dashboard.IdeasReadyForReviewTxt.Text, "IDEAS READY FOR REVIEW", "The Ideas Ready for Review text is missing or not displayed properly.");
            HpgAssert.Exists(dashboard.IdeasReadyForReviewBadge, "The Ideas Ready for Review badge is missing or not displayed properly.");
            HpgAssert.Exists(dashboard.MyBookmarkedIdeasWidget, "The My Bookmarked Ideas widget is missing or not displayed properly");
            HpgAssert.AreEqual(dashboard.MyBookmarkedIdeasTxt.Text, "MY BOOKMARKED IDEAS", "The My Bookmarked Ideas text is missing or not displayed properly.");
            HpgAssert.Exists(dashboard.MyBookmarkedIdeasBadge, "The My Bookmarked Ideas badge is missing or not displayed correctly.");
            HpgAssert.Exists(dashboard.SavedIdeasWidget, "The My Saved Ideas widget is missing or not displayed properly.");
            HpgAssert.AreEqual(dashboard.MySavedIdeasTxt.Text, "MY SAVED IDEAS", "The My Saved Ideas text is missing or not displayed properly.");
            HpgAssert.Exists(dashboard.MySavedIdeasBadge, "The My Saved Ideas badge is missing or not displayed properly.");
        }
    }
}