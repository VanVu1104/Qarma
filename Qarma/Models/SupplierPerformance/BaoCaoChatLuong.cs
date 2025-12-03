using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Qarma.Models.SupplierPerformance
{
    public class BaoCaoChatLuong
    {
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public int SL_Dat { get; set; }
        public int SL_Loi { get; set; }
        public int Critical { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
        public int TongKiem { get; set; }
        public double TiLeLoi_PhanTram { get; set; }
    }
}