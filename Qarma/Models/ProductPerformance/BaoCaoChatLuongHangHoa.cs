using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Qarma.Models.ProductPerformance
{
    public class BaoCaoChatLuongHangHoa
    {
        public string MaHang { get; set; }
        public int TongLoi { get; set; }
        public int Critical { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
        public int SoLuongDat { get; set; }
        public int TongKiem { get; set; }
        public int TiLeLoi_PhanTram { get; set; }
    }
}