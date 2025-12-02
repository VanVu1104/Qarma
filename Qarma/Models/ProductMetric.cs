using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Qarma.Models
{
    public class ProductMetric
    {
        public string ItemNumber { get; set; }
        public int Minor { get; set; }
        public int Major { get; set; }
        public int Critical { get; set; }
        public int InspectSum { get; set; }
        public int DefectSum { get; set; }
        public decimal DefectRate { get; set; }

        // Model để map với kết quả từ stored procedure
        public class ProductSummaryResult
        {
            public string MaHang { get; set; }
            public int SpDat { get; set; }
            public int SpLoi { get; set; }
            public decimal DefectRate { get; set; }
        }

        public class DefectDetailResult
        {
            public string MaHang { get; set; }
            public string TenLoaiLoi { get; set; }
            public int Total_TheoLoai { get; set; }
        }
    }
}