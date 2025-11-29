using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Qarma.Models
{
    // ViewModel chính cho trang Supplier
    public class SupplierDashboardViewModel
    {
        public ProductInfoViewModel ProductInfo { get; set; }
        public KpisViewModel Kpis { get; set; }
        public List<DistributionItem> Distribution { get; set; }
        public List<MapLocation> MapLocations { get; set; }
        public List<SeverityItem> Severity { get; set; }
        public List<CategoryItem> Categories { get; set; }
        public List<ParetoItem> Pareto { get; set; }
    }

    // Tận dụng lại các class con (nếu bạn đã khai báo ở bài trước thì có thể bỏ qua bước khai báo lại này, 
    // hoặc giữ nguyên nếu muốn file này độc lập)
}