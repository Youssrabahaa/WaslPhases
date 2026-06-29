using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phase_1.BLL.DTOs
{
        // ملخص إحصائي شامل للداشبورد - بيتجمع من جداول متعددة (مش جدول مستقل)
        public class DashboardStatsDTO
        {
            public int TotalCases { get; set; }
            public int TotalActiveMatches { get; set; }
            public int TotalCompletedMatches { get; set; }
            public int TotalCancelledMatches { get; set; }

            public int TotalSessions { get; set; }
            public int TotalCompletedSessions { get; set; }

            public int OpenReportsCount { get; set; }
            public int UnderReviewReportsCount { get; set; }
            public int ResolvedReportsCount { get; set; }

            public double AverageRating { get; set; }
            public int TotalReviews { get; set; }

            public int TotalNoShowStrikes { get; set; }
        }

        // عنصر بسيط لرسم Chart (مثلاً عدد البلاغات لكل شهر)
        public class ChartPointDTO
        {
            public string Label { get; set; } = null!;
            public int Value { get; set; }
        }

}
