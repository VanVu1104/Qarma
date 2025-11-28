
﻿using Qarma.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Qarma.Controllers
{
    public class DashboardController : Controller
    {
		public ActionResult SupplierOverview()
		{
			var mockData = new List<SupplierStatsViewModel>
			{
				// Các cột màu xanh đậm (Top cao nhất)
				new SupplierStatsViewModel { SupplierName = "Supplier 12", Inspections = 12, Minor = 10, Major = 5, Critical = 1, DefectRate = 0.55 },
				new SupplierStatsViewModel { SupplierName = "Supplier 10", Inspections = 11, Minor = 8, Major = 4, Critical = 0, DefectRate = 2.14 },
				new SupplierStatsViewModel { SupplierName = "Supplier 7",  Inspections = 10, Minor = 12, Major = 8, Critical = 0, DefectRate = 2.63 },
				new SupplierStatsViewModel { SupplierName = "Supplier 1",  Inspections = 9,  Minor = 5, Major = 3, Critical = 1, DefectRate = 1.20 },
				new SupplierStatsViewModel { SupplierName = "Supplier 13", Inspections = 8,  Minor = 4, Major = 6, Critical = 0, DefectRate = 4.61 },
				new SupplierStatsViewModel { SupplierName = "Supplier 9",  Inspections = 5,  Minor = 2, Major = 2, Critical = 0, DefectRate = 1.50 },
				new SupplierStatsViewModel { SupplierName = "Supplier 15", Inspections = 4,  Minor = 1, Major = 1, Critical = 0, DefectRate = 0.80 },

				// Các cột thấp hơn (Nhóm giữa)
				new SupplierStatsViewModel { SupplierName = "Supplier 8",  Inspections = 3,  Minor = 3, Major = 1, Critical = 0, DefectRate = 17.65 },
				new SupplierStatsViewModel { SupplierName = "Supplier 5",  Inspections = 3,  Minor = 0, Major = 5, Critical = 0, DefectRate = 12.59 },
				new SupplierStatsViewModel { SupplierName = "Supplier 27", Inspections = 3,  Minor = 2, Major = 1, Critical = 0, DefectRate = 1.10 },
				new SupplierStatsViewModel { SupplierName = "Supplier 26", Inspections = 3,  Minor = 1, Major = 0, Critical = 0, DefectRate = 2.05 },
				new SupplierStatsViewModel { SupplierName = "Supplier 24", Inspections = 3,  Minor = 2, Major = 2, Critical = 0, DefectRate = 3.40 },
				new SupplierStatsViewModel { SupplierName = "Supplier 2",  Inspections = 3,  Minor = 1, Major = 1, Critical = 0, DefectRate = 1.00 },
				new SupplierStatsViewModel { SupplierName = "Supplier 19", Inspections = 3,  Minor = 0, Major = 2, Critical = 0, DefectRate = 5.20 },

				// Các cột thấp nhất (Đuôi biểu đồ)
				new SupplierStatsViewModel { SupplierName = "Supplier 25", Inspections = 2,  Minor = 1, Major = 0, Critical = 0, DefectRate = 0.50 },
				new SupplierStatsViewModel { SupplierName = "Supplier 16", Inspections = 2,  Minor = 0, Major = 1, Critical = 0, DefectRate = 1.10 },
				new SupplierStatsViewModel { SupplierName = "Supplier 4",  Inspections = 1,  Minor = 0, Major = 0, Critical = 0, DefectRate = 0.00 },
				new SupplierStatsViewModel { SupplierName = "Supplier 23", Inspections = 1,  Minor = 1, Major = 0, Critical = 0, DefectRate = 2.00 },
				new SupplierStatsViewModel { SupplierName = "Supplier 20", Inspections = 1,  Minor = 0, Major = 0, Critical = 0, DefectRate = 0.00 },
			};

			// 2. LOGIC TÍNH % PARETO (Tính dựa trên SumDefects đã cộng tự động ở trên)

			// Sắp xếp giảm dần theo SumDefects (Quan trọng để vẽ biểu đồ đẹp)
			mockData = mockData.OrderByDescending(x => x.SumDefects).ToList();

			double totalDefects = mockData.Sum(x => x.SumDefects);
			double currentSum = 0;

			foreach (var item in mockData)
			{
				currentSum += item.SumDefects;

				// Tính % tích lũy
				item.ParetoDefect = totalDefects == 0 ? 0 : Math.Round((currentSum / totalDefects) * 100, 1);

				// Tính % tích lũy cho Inspection (nếu cần vẽ biểu đồ kia)
				// Lưu ý: Logic này đang chạy theo loop của Defects, nếu muốn chuẩn Inspection Pareto phải sort lại list khác
				item.ParetoInspection = 0; // Tạm để 0 hoặc tính logic riêng nếu cần
			}

			return View(mockData);
		}
	}
        public ActionResult Index(string fromDate, string toDate)
        {
            // 1. Dữ liệu biểu đồ cột (Inspections)
        var allInspections = new List<MonthlyInspection>
        {
        // Năm 2023
       new MonthlyInspection { MonthYear = "Jan-23", Count = 1 },
        new MonthlyInspection { MonthYear = "Feb-23", Count = 1 },
        new MonthlyInspection { MonthYear = "Mar-23", Count = 5 },
        new MonthlyInspection { MonthYear = "Apr-23", Count = 2 },
        new MonthlyInspection { MonthYear = "May-23", Count = 3 },
        new MonthlyInspection { MonthYear = "Jun-23", Count = 6 },
        new MonthlyInspection { MonthYear = "Jul-23", Count = 1 },
        new MonthlyInspection { MonthYear = "Aug-23", Count = 1 },
        new MonthlyInspection { MonthYear = "Sep-23", Count = 7 },
        new MonthlyInspection { MonthYear = "Oct-23", Count = 2 },
        new MonthlyInspection { MonthYear = "Nov-23", Count = 8 },
        new MonthlyInspection { MonthYear = "Dec-23", Count = 4 },
        
        // --- NĂM 2024 (Thêm đuôi -24) ---
        new MonthlyInspection { MonthYear = "Jan-24", Count = 5 },
        new MonthlyInspection { MonthYear = "Feb-24", Count = 4 },
        new MonthlyInspection { MonthYear = "Mar-24", Count = 3 },
        new MonthlyInspection { MonthYear = "Apr-24", Count = 14 },
        new MonthlyInspection { MonthYear = "May-24", Count = 7 },
        new MonthlyInspection { MonthYear = "Jun-24", Count = 3 },
        new MonthlyInspection { MonthYear = "Jul-24", Count = 3 },
        new MonthlyInspection { MonthYear = "Aug-24", Count = 4 },
        new MonthlyInspection { MonthYear = "Sep-24", Count = 3 }
    };
            // 2. Logic LỌC (Giả lập)
            var filteredInspections = allInspections;
            if (!string.IsNullOrEmpty(fromDate) || !string.IsNullOrEmpty(toDate))
            {
                // Demo: Nếu có lọc thì cắt bớt dữ liệu để thấy sự thay đổi
                filteredInspections = allInspections.Skip(8).ToList();
            }

            // 3. Đưa dữ liệu vào Model
            var model = new DashboardViewModel
            {
                // Gán giá trị ngày (Giờ đã hết lỗi vì Bước 1 đã khai báo rồi)
                FromDate = fromDate ?? "01/01/2023",
                ToDate = toDate ?? "26/09/2024",

                MonthlyInspections = filteredInspections,

                Kpis = new KpiMetrics
                {
                    TotalInspections = filteredInspections.Sum(x => x.Count), // Tự cộng tổng theo dữ liệu lọc
                    ReInspections = 2,
                    Orders = 73,
                    Items = 78,
                    SampleQuantity = 21436
                },

                // Dữ liệu biểu đồ Lỗi (Phải điền vào, không được để trống)
                DefectStats = new List<DefectStat>
                {
                    new DefectStat { Severity = "Major", Count = 200, Color = "#FFD700" },
                    new DefectStat { Severity = "Minor", Count = 82, Color = "#2b908f" },
                    new DefectStat { Severity = "Critical", Count = 17, Color = "#dc3545" }
                },

                // Dữ liệu biểu đồ Kết luận (Phải điền vào)
                Conclusions = new List<InspectionConclusion>
                {
                    new InspectionConclusion { Type = "Final", Approved = 37, Pending = 10, Rejected = 12 },
                    new InspectionConclusion { Type = "Inline", Approved = 14, Pending = 0, Rejected = 0 },
                    new InspectionConclusion { Type = "Sample", Approved = 9, Pending = 4, Rejected = 1 }
                }
            };

            return View(model);
        }
    }
}