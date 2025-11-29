using Qarma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Qarma.Services
{
    public class ProductService
    {
        // Hàm này giả lập việc lấy dữ liệu từ DB lên
        public DashboardViewModel GetDashboardData()
        {
            return new DashboardViewModel
            {
                ProductInfo = new ProductInfoViewModel
                {
                    Name = "#123179-Coat - Blue - Fleece",
                    TotalInspections = 0,
                    LocationRate = "66.67%"
                },
                Kpis = new KpisViewModel
                {
                    Rejection = new KpiItem { Target = 16.1, Current = 0.0 },
                    Reinspection = new KpiItem { Target = 2.3, Current = 0.0 },
                    Defect = new KpiItem { Target = 1.4, Current = 0.1 },
                    Measurement = new KpiItem { Target = 3.8, Current = null }
                },
                Distribution = new List<DistributionItem>
                {
                    new DistributionItem { Month = "", Count = 0, Rate = null },
                    new DistributionItem { Month = "Apr\n2\n2024", Count = 3, Rate = 0.0007 },
                    new DistributionItem { Month = "", Count = 0, Rate = null }
                },
                MapLocations = new List<MapLocation>
                {
                    new MapLocation { Lat = 16.0471, Long = 108.2068, Name = "Da Nang Factory", Value = 2 },
                    new MapLocation { Lat = 15.1205, Long = 108.7923, Name = "Quang Ngai Factory", Value = 1 }
                },
                Severity = new List<SeverityItem>
                {
                    new SeverityItem { Type = "Minor", Count = 6, Color = "#3182bd" },
                    new SeverityItem { Type = "Major", Count = 2, Color = "#fccb00" },
                    new SeverityItem { Type = "Critical", Count = 0, Color = "#d32f2f" }
                },
                Categories = new List<CategoryItem>
                {
                    new CategoryItem { Name = "Fabric", Count = 8, Color = "#3e4b47" }
                },
                Pareto = new List<ParetoItem>
                {
                    new ParetoItem { Type = "Yarn Contamination", Count = 2, Percent = 0.25, IsLast = false },
                    new ParetoItem { Type = "Thick Yarn", Count = 2, Percent = 0.50, IsLast = false },
                    new ParetoItem { Type = "Hole", Count = 2, Percent = 0.75, IsLast = false },
                    new ParetoItem { Type = "Fabric Crease Fold", Count = 2, Percent = 1.00, IsLast = true }
                }
            };
        }
    }
}