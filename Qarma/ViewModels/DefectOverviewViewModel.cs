using System.Collections.Generic;

namespace Qarma.ViewModels
{
    // Class bao gồm tất cả dữ liệu cho trang Defect Overview
    public class DefectOverviewViewModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        // Dữ liệu cho Chart 1 (Cột chồng theo thời gian)
        public List<DefectTrendItem> DefectTrends { get; set; }

        // Dữ liệu cho Chart 2 (Tròn - Danh mục lỗi)
        public List<DefectCategoryItem> DefectCategories { get; set; }

        // Dữ liệu cho Chart 3 (Pareto - Loại lỗi chi tiết)
        public List<ParetoItem> ParetoData { get; set; }
    }

    // --- CÁC CLASS CON ---

    public class DefectTrendItem
    {
        public string MonthYear { get; set; } // Ví dụ: "Jan-23"
        public int Critical { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
    }

    public class DefectCategoryItem
    {
        public string Name { get; set; }      // Ví dụ: "Fabric", "Packing"
        public int Count { get; set; }
        public double Percentage { get; set; } // Tính sẵn từ server cho chuẩn
    }

    public class ParetoItem
    {
        public string DefectName { get; set; } // Ví dụ: "Incorrect Size Boxes"
        public int Count { get; set; }
        public double CumulativePercentage { get; set; } // % Tích lũy (Đường line)
    }
}