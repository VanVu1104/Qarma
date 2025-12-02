using Qarma.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Qarma.Controllers
{
    public class DashboardController : Controller
    {
        private readonly string _connectionString;

        public DashboardController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["PMS_PHUONGDONG"].ConnectionString;
        }

        public ActionResult Index(string fromDate, string toDate)
        {
            // 1. Parse dates (Giữ nguyên logic của bạn)
            DateTime startDate;
            DateTime endDate;

            if (!DateTime.TryParseExact(fromDate, "M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
            {
                startDate = new DateTime(DateTime.Now.Year, 1, 1);
            }

            if (!DateTime.TryParseExact(toDate, "M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
            {
                endDate = DateTime.Today;
            }
            else
            {
                // Lấy ngày cuối cùng của tháng
                endDate = endDate.AddMonths(1).AddDays(-1);
            }

            Debug.WriteLine($"Lọc từ: {startDate} đến {endDate}");

            // 2. Lấy dữ liệu từ Database thông qua Procedure gộp
            var dbData = GetDashboardOverviewData(startDate, endDate);

            // 3. Map dữ liệu vào ViewModel
            var model = new DashboardViewModel
            {
                FromDate = startDate.ToString("MM/yyyy"),
                ToDate = endDate.ToString("MM/yyyy"),

                // Dữ liệu thật từ Result Set 1
                MonthlyInspections = dbData.Inspections,

                // Dữ liệu thật từ Result Set 2 (đã map màu)
                DefectStats = dbData.Defects,

                Kpis = new KpiMetrics
                {
                    // Tính tổng tự động dựa trên dữ liệu thật vừa lấy
                    TotalInspections = dbData.Inspections.Sum(x => x.Count),

                    // Lưu ý: Procedure hiện tại chưa trả về 3 chỉ số này, nên tạm thời giữ hard-code hoặc gán 0
                    ReInspections = 2,
                    Orders = 73,
                    Items = 78,
                },

                // Lưu ý: Procedure chưa có Result Set cho biểu đồ Stacked, giữ nguyên hard-code mẫu
                Conclusions = new List<InspectionConclusion>
                {
                    new InspectionConclusion { Type = "Final", Approved = 37, Pending = 10, Rejected = 12 },
                    new InspectionConclusion { Type = "Inline", Approved = 14, Pending = 0, Rejected = 0 },
                    new InspectionConclusion { Type = "Sample", Approved = 9, Pending = 4, Rejected = 1 }
                }
            };

            return View(model);
        }

        // Hàm xử lý gọi Procedure và map dữ liệu
        private (List<MonthlyInspection> Inspections, List<DefectStat> Defects) GetDashboardOverviewData(DateTime startDate, DateTime endDate)
        {
            var inspections = new List<MonthlyInspection>();
            var defects = new List<DefectStat>();

            using (var connection = new SqlConnection(_connectionString))
            {
                // Gọi Procedure mới: sp_GetDashboardOverview
                using (var command = new SqlCommand("sp_GetDashboardOverview", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FromDate", startDate);
                    command.Parameters.AddWithValue("@ToDate", endDate);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        // --- RESULT SET 1: BIỂU ĐỒ SẢN LƯỢNG ---
                        while (reader.Read())
                        {
                            inspections.Add(new MonthlyInspection
                            {
                                MonthYear = reader["MonthYear"].ToString(),
                                Count = reader["Count"] != DBNull.Value ? Convert.ToInt32(reader["Count"]) : 0
                            });
                        }

                        // --- CHUYỂN SANG RESULT SET 2 ---
                        if (reader.NextResult())
                        {
                            // --- RESULT SET 2: BIỂU ĐỒ LỖI ---
                            while (reader.Read())
                            {
                                string severityName = reader["TenLoaiLoi"].ToString();
                                int count = reader["TotalDefect"] != DBNull.Value ? Convert.ToInt32(reader["TotalDefect"]) : 0;

                                defects.Add(new DefectStat
                                {
                                    Severity = severityName,
                                    Count = count,
                                    // Hàm phụ trợ để lấy màu dựa trên tên lỗi
                                    Color = GetColorForSeverity(severityName)
                                });
                            }
                        }
                    }
                }
            }

            return (inspections, defects);
        }

        // Hàm helper để gán màu sắc (Vì SQL trả về Tên, nhưng View cần Mã Màu)
        private string GetColorForSeverity(string severity)
        {
            if (string.IsNullOrEmpty(severity)) return "#cccccc";

            // Chuẩn hóa chuỗi để so sánh (chữ thường, bỏ dấu cách nếu cần)
            var normalized = severity.Trim().ToLower();

            if (normalized.Contains("critical") || normalized.Contains("nghiêm trọng")) return "#dc3545"; // Đỏ
            if (normalized.Contains("major") || normalized.Contains("nặng")) return "#FFD700"; // Vàng
            if (normalized.Contains("minor") || normalized.Contains("nhẹ")) return "#2b908f"; // Xanh Teal

            return "#6c757d"; // Màu xám mặc định cho các loại khác
        }
    }
}