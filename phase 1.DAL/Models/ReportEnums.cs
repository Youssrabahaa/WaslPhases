using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phase_1.Models
{
    // حالة البلاغ
    public enum ReportStatus
    {
        Open = 1,       // بلاغ جديد لسه متعمل
        UnderReview = 2,// الأدمن شايفه دلوقتي
        Resolved = 3,   // الأدمن اتخذ إجراء وقفل البلاغ
        Rejected = 4    // الأدمن شافه ورفضه (بلاغ غير صحيح)
    }

    // نوع البلاغ (بيساعد الأدمن يفلتر ويفهم بسرعة)
    public enum ReportType
    {
        NoShow = 1,           // غياب عن الجلسة
        Misconduct = 2,       // سوء تصرف
        QualityIssue = 3,     // مشكلة في جودة الجلسة/الخدمة
        Other = 4
    }
}

