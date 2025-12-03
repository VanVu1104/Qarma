using Qarma.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Qarma.Controllers
{
    public class ProductPerformanceController : Controller
    {
        private readonly ProductService _productService;

        public ProductPerformanceController()
        {
            // Khởi tạo Service
            _productService = new ProductService();
        }

        // GET: ProductPerformance
        public ActionResult Index()
        {
            var dataHangHoa = _productService.GetDanhSachProduct();
            ViewBag.DsHangHoa = new SelectList(dataHangHoa, "MaHang", "MaHang");

            return View();
        }

        [HttpGet]
        public JsonResult GetReportData(string maHang)
        {
            DateTime ngayKiem = new DateTime(2025, 12, 01);

            var data = _productService.GetBaoCaoChatLuongHangHoa(ngayKiem, maHang);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetParetoData(string maHang)
        {
            DateTime ngayKiem = new DateTime(2025, 12, 01);

            if(string.IsNullOrEmpty(maHang))
            {
                return Json(new List<object>(), JsonRequestBehavior.AllowGet);
            }

            var data = _productService.GetBaoCaoChatLuongChiTietHangHoa(ngayKiem, maHang);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetReportByMonths(string maHang)
        {
            string year = "2025";

            if(string.IsNullOrEmpty(maHang))
            {
                return Json(new List<object>(), JsonRequestBehavior.AllowGet);
            }

            var data = _productService.GetBaoCaoChatLuongHangHoaTheoThangs(year, maHang);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ContentResult GetDashboardData()
        {
            var data = _productService.GetDashboardData();

            var jsonSettings = new Newtonsoft.Json.JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(data, jsonSettings);

            // Trả về Content thay vì Json để tự quản lý chuỗi JSON
            return Content(jsonString, "application/json");
        }
    }
}