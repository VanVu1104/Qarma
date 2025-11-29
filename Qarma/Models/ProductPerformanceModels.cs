using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Qarma.Models
{
    public class DashboardViewModel
    {
        public ProductInfoViewModel ProductInfo { get; set; }
        public KpisViewModel Kpis { get; set; }
        public List<DistributionItem> Distribution { get; set; }
        public List<MapLocation> MapLocations { get; set; }
        public List<SeverityItem> Severity { get; set; }
        public List<CategoryItem> Categories { get; set; }
        public List<ParetoItem> Pareto { get; set; }
    }

    public class ProductInfoViewModel
    {
        public string Name { get; set; }
        public int TotalInspections { get; set; }
        public string LocationRate { get; set; }
    }

    public class KpisViewModel
    {
        public KpiItem Rejection { get; set; }
        public KpiItem Reinspection { get; set; }
        public KpiItem Defect { get; set; }
        public KpiItem Measurement { get; set; }
    }

    public class KpiItem
    {
        public double Target { get; set; }
        public double? Current { get; set; }
    }

    public class DistributionItem
    {
        public string Month { get; set; }
        public int Count { get; set; }
        public double? Rate { get; set; }
    }

    public class MapLocation
    {
        public double Lat { get; set; }
        public double Long { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class SeverityItem
    {
        public string Type { get; set; }
        public int Count { get; set; }
        public string Color { get; set; }
    }

    public class CategoryItem
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public string Color { get; set; }
    }

    public class ParetoItem
    {
        public string Type { get; set; }
        public int Count { get; set; }
        public double Percent { get; set; }
        public bool IsLast { get; set; }
    }
}