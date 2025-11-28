using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Qarma.Models
{
    public class ProductMetric
    {
        public string ItemNumber { get; set; }
        public string ItemName { get; set; }

        // Số liệu lỗi
        public int Minor { get; set; }
        public int Major { get; set; }
        public int Critical { get; set; }

        // Tổng lỗi (Tính toán)
        public int DefectSum => Minor + Major + Critical;

        // Số lượng mẫu & tồn kho
        public int SampleQuantity { get; set; }
        public int AvailableQuantity { get; set; }

        // Tỷ lệ lỗi (Tính toán)
        // Lưu ý: Trong ảnh, có item tỷ lệ lỗi là Infinity do Sample = 0, cần handle
        public double DefectRate => SampleQuantity > 0 ? Math.Round((double)DefectSum / SampleQuantity * 100, 2) : 0;

        // Số liệu kiểm hàng
        public int Inspections { get; set; }

        // Tỷ lệ từ chối
        public double RejectionRate { get; set; }
    }
}