using Qarma.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Qarma.Controllers
{
    public class SupplierController : Controller
    {
		public ActionResult Index()
		{
			var mockData = new List<SupplierStatsViewModel>
	{
        // 1. Supplier 12
        new SupplierStatsViewModel {
			SupplierName = "Supplier 12",
			Minor = 27, Major = 37, Critical = 6, GeneralDefects = 1,
			DefectRate = 0.55, Inspections = 12, SampleQuantity = 12811, AvailableQuantity = 26789,
			RejectionRate = 8.33, ReInspectionRate = 0, LocationRate = 100
		},
        // 2. Supplier 7
        new SupplierStatsViewModel {
			SupplierName = "Supplier 7",
			Minor = 12, Major = 28, Critical = 0, GeneralDefects = 1,
			DefectRate = 2.63, Inspections = 10, SampleQuantity = 1519, AvailableQuantity = 114269,
			RejectionRate = 0, ReInspectionRate = 0, LocationRate = 90
		},
        // 3. Supplier 10
        new SupplierStatsViewModel {
			SupplierName = "Supplier 10",
			Minor = 8, Major = 19, Critical = 0, GeneralDefects = 0,
			DefectRate = 2.14, Inspections = 11, SampleQuantity = 1262, AvailableQuantity = 13413,
			RejectionRate = 0, ReInspectionRate = 9.09, LocationRate = 72.73
		},
        // 4. Supplier 13
        new SupplierStatsViewModel {
			SupplierName = "Supplier 13",
			Minor = 0, Major = 24, Critical = 0, GeneralDefects = 0,
			DefectRate = 4.61, Inspections = 8, SampleQuantity = 521, AvailableQuantity = 6030,
			RejectionRate = 25.00, ReInspectionRate = 0, LocationRate = 100
		},
        // 5. Supplier 5
        new SupplierStatsViewModel {
			SupplierName = "Supplier 5",
			Minor = 0, Major = 18, Critical = 0, GeneralDefects = 0,
			DefectRate = 12.59, Inspections = 3, SampleQuantity = 143, AvailableQuantity = 2500,
			RejectionRate = 33.33, ReInspectionRate = 0, LocationRate = 100
		},
        // 6. Supplier 8
        new SupplierStatsViewModel {
			SupplierName = "Supplier 8",
			Minor = 3, Major = 11, Critical = 4, GeneralDefects = 2,
			DefectRate = 17.65, Inspections = 3, SampleQuantity = 102, AvailableQuantity = 450,
			RejectionRate = 0, ReInspectionRate = 0, LocationRate = 100
		},
        // 7. Supplier 26
        new SupplierStatsViewModel {
			SupplierName = "Supplier 26",
			Minor = 2, Major = 13, Critical = 0, GeneralDefects = 0,
			DefectRate = 2.05, Inspections = 3, SampleQuantity = 730, AvailableQuantity = 1402,
			RejectionRate = 33.33, ReInspectionRate = 0, LocationRate = 100
		},

        // --- CÁC DÒNG DỮ LIỆU ĐUÔI (Để biểu đồ dài ra giống hình) ---
        new SupplierStatsViewModel { SupplierName = "Supplier 1",  Minor = 3, Major = 10, Critical = 0, Inspections = 9, DefectRate = 1.2 },
		new SupplierStatsViewModel { SupplierName = "Supplier 15", Minor = 2, Major = 10, Critical = 0, Inspections = 4, DefectRate = 0.8 },
		new SupplierStatsViewModel { SupplierName = "Supplier 9",  Minor = 2, Major = 10, Critical = 0, Inspections = 5, DefectRate = 0.5 },
		new SupplierStatsViewModel { SupplierName = "Supplier 16", Minor = 1, Major = 8,  Critical = 0, Inspections = 2, DefectRate = 0.1 },
		new SupplierStatsViewModel { SupplierName = "Supplier 20", Minor = 0, Major = 7,  Critical = 0, Inspections = 1, DefectRate = 0 },
		new SupplierStatsViewModel { SupplierName = "Supplier 27", Minor = 1, Major = 5,  Critical = 0, Inspections = 3, DefectRate = 0 },
		new SupplierStatsViewModel { SupplierName = "Supplier 2",  Minor = 0, Major = 5,  Critical = 0, Inspections = 3, DefectRate = 0 },
		new SupplierStatsViewModel { SupplierName = "Supplier 23", Minor = 1, Major = 3,  Critical = 0, Inspections = 1, DefectRate = 0 },
		new SupplierStatsViewModel { SupplierName = "Supplier 19", Minor = 0, Major = 4,  Critical = 0, Inspections = 3, DefectRate = 0 },
		new SupplierStatsViewModel { SupplierName = "Supplier 4",  Minor = 0, Major = 3,  Critical = 0, Inspections = 1, DefectRate = 0 },
		new SupplierStatsViewModel { SupplierName = "Supplier 24", Minor = 0, Major = 1,  Critical = 0, Inspections = 3, DefectRate = 0 },
	};

			// --- LOGIC TÍNH % PARETO TRONG CONTROLLER (Để hết lỗi code cũ) ---
			// Mặc dù Javascript ở View sẽ tính lại cho chính xác khi vẽ, nhưng ta cứ tính ở đây để object đầy đủ dữ liệu.

			// 1. Tính Pareto cho Defect (Sắp xếp theo Lỗi)
			var sortedByDefect = mockData.OrderByDescending(x => x.SumDefects).ToList();
			double totalDefects = sortedByDefect.Sum(x => x.SumDefects);
			double currentSumDefect = 0;
			foreach (var item in sortedByDefect)
			{
				currentSumDefect += item.SumDefects;
				item.ParetoDefect = totalDefects == 0 ? 0 : Math.Round((currentSumDefect / totalDefects) * 100, 1);
			}

			// 2. Tính Pareto cho Inspection (Sắp xếp theo Inspection)
			var sortedByInsp = mockData.OrderByDescending(x => x.Inspections).ToList();
			double totalInsp = sortedByInsp.Sum(x => x.Inspections);
			double currentSumInsp = 0;
			foreach (var item in sortedByInsp)
			{
				currentSumInsp += item.Inspections;
				item.ParetoInspection = totalInsp == 0 ? 0 : Math.Round((currentSumInsp / totalInsp) * 100, 1);
			}

			// Trả về danh sách gốc (View sẽ tự sort lại khi cần)
			return View(mockData);
		}
	}
}