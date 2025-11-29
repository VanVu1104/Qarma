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
		public int SumDefects => Minor + Major + Critical + GeneralDefects;
		public int GeneralDefects { get; set; }
		public int SampleQuantity { get; set; }
		public int AvailableQuantity { get; set; }
		public double DefectRate { get; set; }
		public int Inspections { get; set; }
		public double RejectionRate { get; set; }
		public double ReInspectionRate { get; set; }
		public double LocationRate { get; set; }
		public double ParetoDefect { get; set; }      // Dùng để vẽ đường % Defect
		public double ParetoInspection { get; set; }
	}
}