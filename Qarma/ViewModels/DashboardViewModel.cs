using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Qarma.ViewModels
{
    public class DashboardViewModel
    {
        public KpiMetrics Kpis { get; set; }
        public List<MonthlyInspection> MonthlyInspections { get; set; } // Biểu đồ to bên trái
        public List<DefectStat> DefectStats { get; set; }               // Biểu đồ lỗi (Vàng/Xanh/Đỏ)
        public List<InspectionConclusion> Conclusions { get; set; }     // Biểu đồ chồng (Stacked)
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    // Các class con chi tiết
    public class KpiMetrics
    {
        public int TotalInspections { get; set; }
        public int ReInspections { get; set; }
        public int Orders { get; set; }
        public int Items { get; set; }
        public int SampleQuantity { get; set; }
    }

    public class MonthlyInspection
    {
        public string MonthYear { get; set; } // Ví dụ: "Jan 2023"
        public int Count { get; set; }
    }

    public class DefectStat
    {
        public string Severity { get; set; } // Major, Minor, Critical
        public int Count { get; set; }
        public string Color { get; set; }    // Mã màu Hex
    }

    public class InspectionConclusion
    {
        public string Type { get; set; }     // Final, Inline, Sample
        public int Approved { get; set; }
        public int Pending { get; set; }
        public int Rejected { get; set; }
    }
}