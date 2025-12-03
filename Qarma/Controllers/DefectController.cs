using Qarma.Models;
using Qarma.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Qarma.Controllers
{
    public class DefectController : Controller
    {
		private QarmaContext db = new QarmaContext();

		public ActionResult Index(DateTime? start, DateTime? end)
		{
			// --- 1. Xử lý ngày tháng (Giữ nguyên) ---
			DateTime fromDate = start ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
			DateTime toDateRaw = end ?? DateTime.Now;
			DateTime toDate = new DateTime(toDateRaw.Year, toDateRaw.Month, toDateRaw.Day, 23, 59, 59);

			ViewBag.StartDate = fromDate.ToString("yyyy-MM-dd");
			ViewBag.EndDate = (end ?? toDate).ToString("yyyy-MM-dd");

			// --- 2. Khởi tạo Model Rỗng ---
			var model = new DefectOverviewViewModel
			{
				FromDate = fromDate.ToString("dd/MM/yyyy"),
				ToDate = toDate.ToString("dd/MM/yyyy"),
				ParetoData = new List<ViewModels.ParetoItem>(),
				DefectTrends = new List<ViewModels.DefectTrendItem>(),
				DefectCategories = new List<ViewModels.DefectCategoryItem>()
			};

			// --- 3. GỌI DATABASE (1 Lần Duy Nhất - 1 Proc Duy Nhất) ---
			var connection = db.Database.Connection;
			try
			{
				if (connection.State != ConnectionState.Open) connection.Open();

				using (var cmd = connection.CreateCommand())
				{
					// Tên Proc gộp bạn đã viết
					cmd.CommandText = "sp_GetDefectDashboard";
					cmd.CommandType = CommandType.StoredProcedure;

					// Truyền tham số
					cmd.Parameters.Add(new SqlParameter("@FromDate", fromDate));
					cmd.Parameters.Add(new SqlParameter("@ToDate", toDate));

					using (var reader = cmd.ExecuteReader())
					{
						// --- ĐỌC BẢNG 1: PARETO ---
						while (reader.Read())
						{
							model.ParetoData.Add(new ViewModels.ParetoItem
							{
								TenLoi = reader["TenLoi"].ToString(),
								SoLuongLoi = Convert.ToInt32(reader["SoLuongLoi"]),
								// Xử lý null an toàn
								PhanTram = reader["PhanTram"] != DBNull.Value ? Convert.ToDouble(reader["PhanTram"]) : 0,
								PhanTramTichLuy = reader["PhanTramTichLuy"] != DBNull.Value ? Convert.ToDouble(reader["PhanTramTichLuy"]) : 0
							});
						}

						// --- ĐỌC BẢNG 2: CỘT CHỒNG (Nhảy sang bảng tiếp theo) ---
						if (reader.NextResult())
						{
							while (reader.Read())
							{
								model.DefectTrends.Add(new ViewModels.DefectTrendItem
								{
									ThangHienThi = reader["ThangHienThi"].ToString(),
									Thang = Convert.ToInt32(reader["Thang"]),
									Nam = Convert.ToInt32(reader["Nam"]),
									Critical = reader["Critical"] != DBNull.Value ? Convert.ToInt32(reader["Critical"]) : 0,
									Major = reader["Major"] != DBNull.Value ? Convert.ToInt32(reader["Major"]) : 0,
									Minor = reader["Minor"] != DBNull.Value ? Convert.ToInt32(reader["Minor"]) : 0
								});
							}
						}

						// --- ĐỌC BẢNG 3: TRÒN (Nhảy sang bảng cuối) ---
						if (reader.NextResult())
						{
							while (reader.Read())
							{
								model.DefectCategories.Add(new ViewModels.DefectCategoryItem
								{
									TenLoiHienThi = reader["TenLoiHienThi"].ToString(),
									TongSoLoi = Convert.ToInt32(reader["TongSoLoi"]),
									PhanTram = reader["PhanTram"] != DBNull.Value ? Convert.ToDouble(reader["PhanTram"]) : 0
								});
							}
						}
					}
				}
			}
			finally
			{
				if (connection.State == ConnectionState.Open) connection.Close();
			}

			// --- 4. TRẢ VỀ VIEW ---
			return View(model);
		}
	}
}