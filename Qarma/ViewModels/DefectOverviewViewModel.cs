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
		public string ThangHienThi { get; set; }
		public int Thang { get; set; }
		public int Nam { get; set; }
		public int Critical { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
    }

    public class DefectCategoryItem
    {
        public string TenLoiHienThi { get; set; }      // Ví dụ: "Fabric", "Packing"
        public int TongSoLoi { get; set; }
        public double PhanTram { get; set; } // Tính sẵn từ server cho chuẩn
    }

    public class ParetoItem
    {
        public string TenLoi { get; set; } // Ví dụ: "Incorrect Size Boxes"
        public int SoLuongLoi { get; set; }
        public double PhanTram { get; set; } // % Tích lũy (Đường line)
		public double PhanTramTichLuy { get; set; }
	}
}