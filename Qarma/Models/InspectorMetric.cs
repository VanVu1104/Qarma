using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Qarma.Models
{
    public class InspectorMetric
    {
        public string Name { get; set; }
        public string Email { get; set; }

        // Số liệu lỗi
        public int Minor { get; set; }
        public int Major { get; set; }
        public int Critical { get; set; }
        public int SumDefects => Minor + Major + Critical;

        // Chỉ số hiệu suất
        public double DefectRate { get; set; } // Dữ liệu ảnh có sẵn %, nên ta nhập thẳng hoặc tính toán
        public int SampleQuantity { get; set; }
        public int Inspections { get; set; }

        public double RejectionRate { get; set; }
        public int ReInspections { get; set; }

        public double ConclusionAgreementRate { get; set; } // Tỷ lệ đồng thuận
        public double LocationRate { get; set; }
        public double AvgDuration { get; set; } // Giờ
    }
}