using Qarma.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;

namespace Qarma.Controllers
{
    public class SupplierPerformanceController : Controller
    {
        private readonly SupplierService _supplierService;

        public SupplierPerformanceController()
        {
            _supplierService = new SupplierService();
        }

        // GET: SupplierPerformance
        public ActionResult Index()
        {
            var dataKhachHang = _supplierService.GetDanhSachKhachHang();
            ViewBag.DsKhachHang = new SelectList(dataKhachHang, "MaKhachHang", "TenKhachHang");

            return View();
        }

        [HttpGet]
        public JsonResult GetReportData(string customerId)
        {
            DateTime ngayBaoCao = new DateTime(2025, 12, 01);

            // Gọi Service lấy danh sách
            var data = _supplierService.GetBaoCaoChatLuong(ngayBaoCao, customerId);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetParetoData(string customerId)
        {
            // Ngày cố định theo yêu cầu của bạn (hoặc DateTime.Now)
            DateTime ngayBaoCao = new DateTime(2025, 12, 01);

            // Nếu không có CustomerId thì không lấy được Pareto (trả về rỗng)
            if (string.IsNullOrEmpty(customerId))
            {
                return Json(new List<object>(), JsonRequestBehavior.AllowGet);
            }

            // Gọi Service
            var data = _supplierService.GetBaoCaoChatLuongChiTiet(ngayBaoCao, customerId);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetReportByMonths(string customerId)
        {
            string year = "2025";

            if(string.IsNullOrEmpty(customerId))
            {
                return Json(new List<object>(), JsonRequestBehavior.AllowGet);
            }

            var data = _supplierService.GetBaoCaoChatLuongTheoThangs(year, customerId);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ContentResult GetDashboardData()
        {
            var data = _supplierService.GetSupplierData();

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