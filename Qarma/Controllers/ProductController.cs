using Qarma.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Qarma.Models.ProductMetric;

namespace Qarma.Controllers
{
    public class ProductController : Controller
    {
        private readonly string connectionString;
        public ProductController()
        {
            // Lấy connection string từ Web.config
            connectionString = ConfigurationManager.ConnectionStrings["PMS_PHUONGDONG"].ConnectionString;
        }
        public ActionResult Index(DateTime? fromDate = null, DateTime? toDate = null)
        {
            // Nếu không truyền tham số, lấy mặc định tháng hiện tại
            if (!fromDate.HasValue)
                fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            if (!toDate.HasValue)
                toDate = DateTime.Now;
            Debug.WriteLine("From Date: " + fromDate.Value.Date);
            Debug.WriteLine("From Date: " + toDate.Value.Date);
            var productMetrics = GetProductQualityData(fromDate.Value.Date, toDate.Value.Date);

            // Truyền date range vào ViewBag để hiển thị trên view
            ViewBag.FromDate = fromDate.Value.ToString("dd/MM/yyyy");
            ViewBag.ToDate = toDate.Value.ToString("dd/MM/yyyy");
            ViewBag.FromDateInput = fromDate.Value.ToString("yyyy-MM-dd");
            ViewBag.ToDateInput = toDate.Value.ToString("yyyy-MM-dd");
            return View(productMetrics);
        }
        private List<ProductMetric> GetProductQualityData(DateTime fromDate, DateTime toDate)
        {
            var productMetrics = new List<ProductMetric>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("sp_GetProductQualitySummary", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FromDate", fromDate);
                    command.Parameters.AddWithValue("@ToDate", toDate);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        // ========================================
                        // Đọc ResultSet 1: Bảng Tổng Hợp
                        // ========================================
                        var summaryDict = new Dictionary<string, ProductSummaryResult>();

                        while (reader.Read())
                        {
                            var summary = new ProductSummaryResult
                            {
                                MaHang = reader["MaHang"].ToString(),
                                SpDat = Convert.ToInt32(reader["SpDat"]),
                                SpLoi = Convert.ToInt32(reader["SpLoi"]),
                                DefectRate = Convert.ToDecimal(reader["DefectRate"])
                            };
                            summaryDict[summary.MaHang] = summary;
                        }

                        // ========================================
                        // Đọc ResultSet 2: Chi Tiết Lỗi Theo Loại
                        // ========================================
                        if (reader.NextResult())
                        {
                            var defectDetails = new Dictionary<string, Dictionary<string, int>>();

                            while (reader.Read())
                            {
                                string maHang = reader["MaHang"].ToString();
                                string loaiLoi = reader["TenLoaiLoi"].ToString();
                                int total = Convert.ToInt32(reader["Total_TheoLoai"]);

                                if (!defectDetails.ContainsKey(maHang))
                                {
                                    defectDetails[maHang] = new Dictionary<string, int>();
                                }

                                defectDetails[maHang][loaiLoi] = total;
                            }

                            // ========================================
                            // Kết hợp dữ liệu vào ProductMetric
                            // ========================================
                            foreach (var summary in summaryDict.Values)
                            {
                                var metric = new ProductMetric
                                {
                                    ItemNumber = summary.MaHang,
                                    DefectRate = summary.DefectRate,
                                    DefectSum = summary.SpLoi,
                                    InspectSum = summary.SpDat
                                };

                                // Map loại lỗi
                                if (defectDetails.ContainsKey(summary.MaHang))
                                {
                                    var details = defectDetails[summary.MaHang];

                                    metric.Minor = details.ContainsKey("Lỗi nhẹ") ? details["Lỗi nhẹ"] : 0;
                                    metric.Major = details.ContainsKey("Lỗi nặng") ? details["Lỗi nặng"] : 0;
                                    metric.Critical = details.ContainsKey("Lỗi nghiêm trọng") ? details["Lỗi nghiêm trọng"] : 0;
                                }

                                productMetrics.Add(metric);
                            }
                        }
                    }
                }
            }

            return productMetrics.ToList();
        }
    }
}