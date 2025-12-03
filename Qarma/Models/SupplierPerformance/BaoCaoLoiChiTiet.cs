using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Qarma.Models.SupplierPerformance
{
    public class BaoCaoLoiChiTiet
    {
        public string TenLoi { get; set; }      // Tên lỗi cụ thể (VD: Đứt chỉ)
        public int SoLuongLoi { get; set; }     // Số lượng
        public double TiLePhanTram { get; set; } // Tỷ lệ %
        public int TongLoiChung { get; set; }   // Tổng lỗi của KH (để tham khảo nếu cần)
    }
}