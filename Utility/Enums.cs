using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdeaManagement.Utility
{
    public static class Enums
    {
        public static Dictionary<int, string> levels = new Dictionary<int, string>()
                {
                    {1,"Low"},
                    {2, "Medium"},
                    {3, "High"}
                };

        public enum ImplementedStatus
        {
            NotYetReviewed,
            UnderReview,
            InProcess,
            Implemented,
            ImplementedNotReporting,
            Rejected,
            NotApplicable,
            Discontinued
        }

        public static Dictionary<string, ImplementedStatus> ImplementedStatusString = new Dictionary<string, ImplementedStatus>()
            {
                {"Not Yet Reviewed", ImplementedStatus.NotYetReviewed},
                {"Under Review", ImplementedStatus.UnderReview},
                {"In Process", ImplementedStatus.InProcess},
                {"Implemented", ImplementedStatus.Implemented},
                {"Implemented - Not Reporting", ImplementedStatus.ImplementedNotReporting},
                {"Rejected", ImplementedStatus.Rejected},
                {"Not Applicable", ImplementedStatus.NotApplicable},
                {"Discontinued", ImplementedStatus.Discontinued}
            };

        public class SavingsIdea
        {
            public int IdeaId;
            public DateTime? Implemented;
            public DateTime? Discontinued;
            public decimal? Savings;
            public bool FacilitySavings;
            public int QualifiedIdeaId;
            public bool ShowNegative;
        }

    }
}
