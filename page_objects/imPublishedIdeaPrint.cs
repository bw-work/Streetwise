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

namespace IdeaManagement.page_objects
{
    class imPublishedIdeaPrint : imPublishedIdea
    {
        public imPublishedIdeaPrint(BrowserWindow currentBrowser) : base((BrowserSession)currentBrowser)
        {
        }

        #region Objects
        #endregion
    }
}