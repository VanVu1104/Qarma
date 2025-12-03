using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Qarma.ViewModels
{
	public class SupplierStatsViewModel
	{
		public string SupplierName { get; set; }

		// Các chỉ số lỗi
		public int Minor { get; set; }
		public int Major { get; set; }
		public int Critical { get; set; }
		public int SumDefects { get; set; }
		public double DefectRate { get; set; }
		public int SumPassed { get; set; }
		public double ParetoDefect { get; set; }      // Dùng để vẽ đường % Defect
		public double ParetoInspection { get; set; }
	}
}