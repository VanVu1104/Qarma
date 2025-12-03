using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient; // Nếu bạn dùng ADO.NET

namespace Qarma.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Gọi hàm và lấy thông báo
            string ketQua = KetNoiDatabase();

            // Đẩy ra view để xem thử
            ViewBag.ThongBao = ketQua;

            return View();
        }

        public string KetNoiDatabase()
        {
            string strCon = ConfigurationManager.ConnectionStrings["PMS_PHUONGDONG"].ConnectionString;
            string thongBao = "";

            try
            {
                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    conn.Open();
                    // Kết nối thành công
                    thongBao = "Kết nối đến SQL Server thành công!";
                }
            }
            catch (Exception ex)
            {
                // Kết nối thất bại
                thongBao = "Lỗi kết nối: " + ex.Message;
            }

            return thongBao; // Trả về dòng thông báo
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}