using System.Linq;
using AutomationCore;
using Coypu;
using System.Windows.Forms;
using Coypu.Drivers;
using AutomationCore.utility;

namespace IdeaManagement.Utility
{
    class AngularElements
    {
        public class CheckBox : HpgElement
        {
            public CheckBox(ElementScope element)
                : base(element)
            { }

            public new void Check()
            {
                if (!Element.FindXPath("i")["class"].ToLower().Trim().Equals("icon-check")) Element.Click();
                HpgAssert.True(Element.FindXPath("i")["class"].ToLower().Trim().Equals("icon-check"), string.Format("Checked a CheckBox({0})", Element.Text));
                SuperTest.WriteReport(string.Format("Checked a CheckBox({0})", Element.Text));
            }

            public new void UnCheck()
            {
                if (!Element.FindXPath("i")["class"].ToLower().Trim().Equals("icon-check-empty")) Element.Click();
                HpgAssert.True(Element.FindXPath("i")["class"].ToLower().Trim().Equals("icon-check-empty"), string.Format("Checked a CheckBox({0})", Element.Text));
                SuperTest.WriteReport(string.Format("Unchecked a CheckBox({0})", Element.Text));
            }
        }

        public class TextBox : HpgElement
        {
            public TextBox(ElementScope element) : base(element)
            {}

            public bool IsRequired
            {
                get
                {
                    //return bool.Parse(Element.FindXPath("following-sibling::input[contains(@id, 'IsRequired')]", new Options() { ConsiderInvisibleElements = true })["Value"]);
                    return !string.IsNullOrEmpty(Element["required"]);
                }
            }
        }
    }
}
