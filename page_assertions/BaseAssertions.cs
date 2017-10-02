using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coypu;
using AutomationCore;
using AutomationCore.utility;
using Streetwise.page_objects;


namespace Streetwise.page_assertions
{
    class BaseAssertions : swMaster
    {
        public BaseAssertions(BrowserSession browser) : base(browser) { }
        
        public void AssertBaseElements()
        {
            HpgAssert.Exists(HealthtrustLogo, "The Healthtrust logo could not be found.");
            HpgAssert.Exists(HomeButton, "The home button could not be found.");
            HpgAssert.AreEqual(TabPublishedIdeas.Text, "PUBLISHED IDEAS", "The 'Published Ideas' tab is missing or not displayed correctly.");
            HpgAssert.AreEqual(TabAllIdeas.Text, "ALL IDEAS", "The 'All Ideas' tab is missing or not displayed correctly.");
            HpgAssert.AreEqual(TabMyIdeas.Text, "MY IDEAS", "The 'My Ideas' tab is missing or not displayed correctly.");
            HpgAssert.AreEqual(TabSubmitAnIdea.Text, "SUBMIT AN IDEA", "The 'Submit an Idea' tab is missing or not displayed correrctly.");
            HpgAssert.Exists(pageFooter, "The footer is missing or not displayed properly.");
        }
    }
}