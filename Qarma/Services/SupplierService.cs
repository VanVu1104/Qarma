using Qarma.Models;
using Qarma.Models.SupplierPerformance;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Qarma.Services
{
    public class SupplierService
    {
        private string _connectionString;

        public SupplierService()
        {
            // Lấy chuỗi kết nối 1 lần duy nhất ở Constructor
            _connectionString = ConfigurationManager.ConnectionStrings["PMS_PHUONGDONG"].ConnectionString;
        }

        public List<KhachHang> GetDanhSachKhachHang()
        {
            List<KhachHang> result = new List<KhachHang>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Gọi Procedure
                using (SqlCommand cmd = new SqlCommand("sp_GetDanhSachKhachHang", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new KhachHang
                            {
                                MaKhachHang = reader["MaKhachHang"].ToString(),
                                TenKhachHang = reader["KhachHang"].ToString()
                            });
                        }
                    }
                }
            }
            return result;
        }

        public List<BaoCaoChatLuong> GetBaoCaoChatLuong(DateTime date, string customerId = null)
        {
            List<BaoCaoChatLuong> list = new List<BaoCaoChatLuong>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Tên Procedure vừa tạo
                using (SqlCommand cmd = new SqlCommand("sp_GetBaoCaoChatLuongChiTiet", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Truyền tham số ngày vào
                    cmd.Parameters.AddWithValue("@NgayBaoCao", date);

                    // Xử lý tham số Mã khách hàng
                    if (!string.IsNullOrEmpty(customerId))
                    {
                        // Nếu có mã -> Gửi mã xuống
                        cmd.Parameters.AddWithValue("@MaKhachHang", customerId);
                    }

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new BaoCaoChatLuong();

                                // Sử dụng an toàn (check null nếu cần, nhưng SP đã ISNULL rồi nên cứ ép kiểu)
                                item.MaKhachHang = reader["MaKhachHang"].ToString();
                                item.TenKhachHang = reader["TenKhachHang"].ToString();
                                item.SL_Dat = Convert.ToInt32(reader["SL_Dat"]);
                                item.SL_Loi = Convert.ToInt32(reader["SL_Loi"]);

                                // --- ĐỌC CÁC CỘT MỚI ---
                                item.Critical = Convert.ToInt32(reader["Critical"]);
                                item.Major = Convert.ToInt32(reader["Major"]);
                                item.Minor = Convert.ToInt32(reader["Minor"]);
                                // -----------------------

                                item.TongKiem = Convert.ToInt32(reader["TongKiem"]);
                                item.TiLeLoi_PhanTram = Convert.ToDouble(reader["TiLeLoi_PhanTram"]);

                                list.Add(item);
                            }
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            return list;
        }

        public List<BaoCaoLoiChiTiet> GetBaoCaoChatLuongChiTiet(DateTime date, string customerId = null)
        {
            List<BaoCaoLoiChiTiet> list = new List<BaoCaoLoiChiTiet>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Gọi đúng tên SP mới tạo
                using (SqlCommand cmd = new SqlCommand("sp_GetTyLeLoiChiTietTheoKhachHang", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Truyền tham số
                    cmd.Parameters.AddWithValue("@NgayBaoCao", date);

                    // Xử lý tham số Mã Khách Hàng (Bắt buộc phải có theo logic Pareto)
                    if (string.IsNullOrEmpty(customerId))
                    {
                        // Nếu không truyền mã -> Trả về list rỗng hoặc xử lý tùy ý
                        // Vì SP yêu cầu @MaKhachHang, ta không thể truyền NULL nếu SP không cho phép
                        return list;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@MaKhachHang", customerId);
                    }

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new BaoCaoLoiChiTiet
                                {
                                    TenLoi = reader["TenLoi"].ToString(),
                                    SoLuongLoi = Convert.ToInt32(reader["SoLuongLoi"]),
                                    // Kiểm tra DBNull cho cột Tỷ lệ (phòng trường hợp chia cho 0 ra NULL)
                                    TiLePhanTram = reader["TiLe_PhanTram"] != DBNull.Value
                                                   ? Convert.ToDouble(reader["TiLe_PhanTram"])
                                                   : 0.0,
                                    TongLoiChung = Convert.ToInt32(reader["TongLoiChung"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Ghi log lỗi tại đây nếu cần
                        throw new Exception("Lỗi khi lấy báo cáo chi tiết lỗi: " + ex.Message);
                    }
                }
            }
            return list;
        }

        public List<BaoCaoChatLuongTheoThang> GetBaoCaoChatLuongTheoThangs(string year = null, string customerId = null)
        {
            List<BaoCaoChatLuongTheoThang> list = new List<BaoCaoChatLuongTheoThang>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetBaoCaoChatLuong12Thang", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Truyền tham số Năm nếu có
                    //if (!string.IsNullOrEmpty(year))
                    //{
                    //    cmd.Parameters.AddWithValue("@Nam", year);
                    //}
                    // Truyền tham số Mã Khách Hàng nếu có
                    if (string.IsNullOrEmpty(customerId))
                    {
                        return list;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@MaKhachHang", customerId);
                    }
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new BaoCaoChatLuongTheoThang();

                                item.ThoiGian = reader["ThoiGian"].ToString();
                                item.SL_Dat = Convert.ToInt32(reader["SL_Dat"]);
                                item.SL_Loi = Convert.ToInt32(reader["SL_Loi"]);
                                item.TongKiem = Convert.ToInt32(reader["TongKiem"]);
                                item.TiLeLoi_PhanTram = Convert.ToDouble(reader["TiLeLoi_PhanTram"]);
                                
                                list.Add(item);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi lấy báo cáo chất lượng theo tháng: " + ex.Message);
                    }
                }
            }
            return list;
        }

        public SupplierDashboardViewModel GetSupplierData()
        {
            return new SupplierDashboardViewModel
            {
                ProductInfo = new ProductInfoViewModel
                {
                    Name = "Supplier 7",
                    TotalInspections = 10,
                    LocationRate = "90.00%"
                },
                Kpis = new KpisViewModel
                {
                    Rejection = new KpiItem { Target = 16.1, Current = 0.0 },
                    Reinspection = new KpiItem { Target = 2.3, Current = 0.0 },
                    Defect = new KpiItem { Target = 1.4, Current = 2.6 }, // Chú ý: Value cao (màu đỏ trong chart)
                    Measurement = new KpiItem { Target = 3.8, Current = 0.0 }
                },
                Distribution = new List<DistributionItem>
                {
                    new DistributionItem { Month = "Jun\n2\n2023", Count = 1, Rate = 0.1625 },
                    new DistributionItem { Month = "Sep\n3\n2023", Count = 1, Rate = 0.0625 },
                    new DistributionItem { Month = "Apr\n2024", Count = 1, Rate = 0.00 },
                    new DistributionItem { Month = "May\n2\n2024", Count = 2, Rate = 0.002 },
                    new DistributionItem { Month = "Jun\n2024", Count = 3, Rate = 0.0734 },
                    new DistributionItem { Month = "Jul\n2024", Count = 1, Rate = 0.008 },
                    new DistributionItem { Month = "Aug\n3\n2024", Count = 1, Rate = 0.10 }
                },
                MapLocations = new List<MapLocation>
                {
                    new MapLocation { Lat = 37.0902, Long = -95.7129, Name = "North America", Value = 1 },
                    new MapLocation { Lat = 48.8566, Long = 2.3522, Name = "Europe (France)", Value = 1 },
                    new MapLocation { Lat = 51.1657, Long = 10.4515, Name = "Europe (Germany)", Value = 2 },
                    new MapLocation { Lat = 21.0285, Long = 105.8542, Name = "Asia (Vietnam)", Value = 3 }
                },
                Severity = new List<SeverityItem>
                {
                    new SeverityItem { Type = "Major", Count = 28, Color = "#fccb00" },
                    new SeverityItem { Type = "Minor", Count = 12, Color = "#3182bd" },
                    new SeverityItem { Type = "Critical", Count = 0, Color = "#d32f2f" }
                },
                Categories = new List<CategoryItem>
                {
                    new CategoryItem { Name = "Fabric", Count = 15, Color = "#1f7a4d" },
                    new CategoryItem { Name = "Sewing & App...", Count = 6, Color = "#4ade80" },
                    new CategoryItem { Name = "Finishing", Count = 6, Color = "#86efac" },
                    new CategoryItem { Name = "Accessories", Count = 4, Color = "#166534" },
                    new CategoryItem { Name = "Packing & Pac...", Count = 23, Color = "#e5e7eb" }
                },
                Pareto = new List<ParetoItem>
                {
                    new ParetoItem { Type = "Carton defects", Count = 13, Percent = 0.325, IsLast = false },
                    new ParetoItem { Type = "Missing/check", Count = 9, Percent = 0.55, IsLast = false },
                    new ParetoItem { Type = "Stripes/check", Count = 6, Percent = 0.70, IsLast = false },
                    new ParetoItem { Type = "Thread trim", Count = 4, Percent = 0.80, IsLast = false },
                    new ParetoItem { Type = "Pick Missing", Count = 4, Percent = 0.90, IsLast = false },
                    new ParetoItem { Type = "Wrong polybag", Count = 1, Percent = 0.925, IsLast = false },
                    new ParetoItem { Type = "Snag Yarn", Count = 1, Percent = 0.95, IsLast = false },
                    new ParetoItem { Type = "Hole", Count = 1, Percent = 0.975, IsLast = false },
                    new ParetoItem { Type = "Buckle", Count = 1, Percent = 1.00, IsLast = true }
                }
            };
        }
    }
}