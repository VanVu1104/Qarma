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
            return View();
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