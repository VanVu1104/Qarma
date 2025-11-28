using Qarma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Qarma.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            // DỮ LIỆU ẢO (MOCK DATA) - Giống trong ảnh
            var mockData = new List<ProductMetric>
            {
                new ProductMetric { ItemNumber = "#123004", ItemName = "T-shirt - Black - Cotton", Minor = 16, Major = 0, Critical = 0, SampleQuantity = 200, AvailableQuantity = 9000, Inspections = 1, RejectionRate = 0 },
                new ProductMetric { ItemNumber = "#123257", ItemName = "Skirt - Navy - Cashmere", Minor = 7, Major = 0, Critical = 0, SampleQuantity = 50, AvailableQuantity = 300, Inspections = 1, RejectionRate = 0 },
                new ProductMetric { ItemNumber = "#123179", ItemName = "Coat - Blue - Fleece", Minor = 6, Major = 2, Critical = 0, SampleQuantity = 12089, AvailableQuantity = 12089, Inspections = 4, RejectionRate = 0 }, // Inspection cao như biểu đồ trái
                new ProductMetric { ItemNumber = "#123248", ItemName = "Pants - Red - Nylon", Minor = 6, Major = 0, Critical = 0, SampleQuantity = 100, AvailableQuantity = 500, Inspections = 1, RejectionRate = 100 }, // Fail 100%
                new ProductMetric { ItemNumber = "#123249", ItemName = "Pants - Green - Fleece", Minor = 6, Major = 0, Critical = 0, SampleQuantity = 100, AvailableQuantity = 500, Inspections = 1, RejectionRate = 100 },
                new ProductMetric { ItemNumber = "#123052", ItemName = "Dress - Green - Cotton", Minor = 5, Major = 0, Critical = 0, SampleQuantity = 80, AvailableQuantity = 988, Inspections = 4, RejectionRate = 0 },
                new ProductMetric { ItemNumber = "#123131", ItemName = "Dress - Black - Wool", Minor = 4, Major = 0, Critical = 0, SampleQuantity = 250, AvailableQuantity = 1000, Inspections = 1, RejectionRate = 0 },
                // Thêm vài dòng data nhỏ để biểu đồ dài ra
                new ProductMetric { ItemNumber = "#123001", ItemName = "Scarf - Silk", Minor = 1, Major = 0, Critical = 0, SampleQuantity = 200, Inspections = 2, RejectionRate = 0 },
                new ProductMetric { ItemNumber = "#123009", ItemName = "Gloves - Leather", Minor = 1, Major = 0, Critical = 0, SampleQuantity = 200, Inspections = 3, RejectionRate = 0 },
            };

            return View(mockData);
        }
    }
}