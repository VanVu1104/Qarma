
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