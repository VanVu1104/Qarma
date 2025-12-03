using Qarma.Models;
using Qarma.Models.ProductPerformance;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Qarma.Services
{
    public class ProductService
    {
        private string _connectionString;
        public ProductService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["PMS_PHUONGDONG"].ConnectionString;
        }

        public List<HangHoa> GetDanhSachProduct()
        {
            List<HangHoa> result = new List<HangHoa>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetDanhSachMaHangTongHop", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new HangHoa
                            {
                                MaHang = reader["MaHang"].ToString()
                            });
                        }
                    }
                }
            }
            return result;
        }

        public List<BaoCaoChatLuongHangHoa> GetBaoCaoChatLuongHangHoa(DateTime date, string maHang = null)
        {
            List<BaoCaoChatLuongHangHoa> list = new List<BaoCaoChatLuongHangHoa>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetBaoCaoChatLuongTheoMaHang", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NgayBaoCao", date);

                    if (!string.IsNullOrEmpty(maHang))
                    {
                        cmd.Parameters.AddWithValue("@MaHang", maHang);
                    }

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new BaoCaoChatLuongHangHoa();
                                
                                item.MaHang = reader["MaHang"].ToString();
                                item.TongLoi = Convert.ToInt32(reader["TongLoi"]);
                                item.Critical = Convert.ToInt32(reader["Critical"]);
                                item.Major = Convert.ToInt32(reader["Major"]);
                                item.Minor = Convert.ToInt32(reader["Minor"]);
                                item.SoLuongDat = Convert.ToInt32(reader["SoLuongDat"]);
                                item.TongKiem = Convert.ToInt32(reader["TongKiem"]);
                                item.TiLeLoi_PhanTram = Convert.ToInt32(reader["TiLeLoi_PhanTram"]);
                                
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

        public List<BaoCaoLoiChiTietHangHoa> GetBaoCaoChatLuongChiTietHangHoa(DateTime date, string maHang = null)
        {
            List<BaoCaoLoiChiTietHangHoa> list = new List<BaoCaoLoiChiTietHangHoa>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetTyLeLoiTheoMaHang", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NgayKiem", date);

                    if (string.IsNullOrEmpty(maHang))
                    {
                        return list;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@MaHang", maHang);
                    }

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new BaoCaoLoiChiTietHangHoa();

                                item.TenLoi = reader["TenLoi"].ToString();
                                item.SoLuongLoi = Convert.ToInt32(reader["SoLuongLoi"]);
                                item.TongLoi = Convert.ToInt32(reader["TongLoi"]);
                                item.TiLe_PhanTram = Convert.ToInt32(reader["TiLe_PhanTram"]);
                                
                                list.Add(item);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Lỗi khi lấy báo cáo chi tiết lỗi: " + ex.Message);
                    }
                }
            }
            return list;
        }

        public List<BaoCaoChatLuongHangHoaTheoThang> GetBaoCaoChatLuongHangHoaTheoThangs(string year = null, string maHang = null)
        {
            List<BaoCaoChatLuongHangHoaTheoThang> list = new List<BaoCaoChatLuongHangHoaTheoThang>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetBaoCaoChatLuong12ThangTheoMaHang", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //if (!string.IsNullOrEmpty(year))
                    //{
                    //    cmd.Parameters.AddWithValue("@Nam", year);
                    //}
                    if (string.IsNullOrEmpty(maHang))
                    {
                        return list;
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@MaHang", maHang);
                    }
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new BaoCaoChatLuongHangHoaTheoThang();
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
                        throw new Exception("Lỗi khi lấy báo cáo chất lượng hàng hóa theo tháng: " + ex.Message);
                    }
                }
            }
            return list;
        }

        // Hàm này giả lập việc lấy dữ liệu từ DB lên
        public DashboardViewModel GetDashboardData()
        {
            return new DashboardViewModel
            {
                ProductInfo = new ProductInfoViewModel
                {
                    Name = "#123179-Coat - Blue - Fleece",
                    TotalInspections = 0,
                    LocationRate = "66.67%"
                },
                Kpis = new KpisViewModel
                {
                    Rejection = new KpiItem { Target = 16.1, Current = 0.0 },
                    Reinspection = new KpiItem { Target = 2.3, Current = 0.0 },
                    Defect = new KpiItem { Target = 1.4, Current = 0.1 },
                    Measurement = new KpiItem { Target = 3.8, Current = null }
                },
                Distribution = new List<DistributionItem>
                {
                    new DistributionItem { Month = "", Count = 0, Rate = null },
                    new DistributionItem { Month = "Apr\n2\n2024", Count = 3, Rate = 0.0007 },
                    new DistributionItem { Month = "", Count = 0, Rate = null }
                },
                MapLocations = new List<MapLocation>
                {
                    new MapLocation { Lat = 16.0471, Long = 108.2068, Name = "Da Nang Factory", Value = 2 },
                    new MapLocation { Lat = 15.1205, Long = 108.7923, Name = "Quang Ngai Factory", Value = 1 }
                },
                Severity = new List<SeverityItem>
                {
                    new SeverityItem { Type = "Minor", Count = 6, Color = "#3182bd" },
                    new SeverityItem { Type = "Major", Count = 2, Color = "#fccb00" },
                    new SeverityItem { Type = "Critical", Count = 0, Color = "#d32f2f" }
                },
                Categories = new List<CategoryItem>
                {
                    new CategoryItem { Name = "Fabric", Count = 8, Color = "#3e4b47" }
                },
                Pareto = new List<ParetoItem>
                {
                    new ParetoItem { Type = "Yarn Contamination", Count = 2, Percent = 0.25, IsLast = false },
                    new ParetoItem { Type = "Thick Yarn", Count = 2, Percent = 0.50, IsLast = false },
                    new ParetoItem { Type = "Hole", Count = 2, Percent = 0.75, IsLast = false },
                    new ParetoItem { Type = "Fabric Crease Fold", Count = 2, Percent = 1.00, IsLast = true }
                }
            };
        }
    }
}