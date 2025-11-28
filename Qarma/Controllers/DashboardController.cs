using Qarma.ViewModels;
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
}