using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Qarma.ViewModels
{
	public class SupplierStatsViewModel
	{
		public string SupplierName { get; set; }
		public int Minor { get; set; }
		public int Major { get; set; }
		public int Critical { get; set; }
		public int SumDefects => Minor + Major + Critical; // Tự động tính
		public double DefectRate { get; set; }
		public int Inspections { get; set; }
		public double ParetoInspection { get; set; }
		public double ParetoDefect { get; set; }
	}
}