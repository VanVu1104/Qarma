using Qarma.Models;
using Qarma.ViewModels;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Qarma.Controllers
{
    public class SupplierController : Controller
    {
		private QarmaContext db = new QarmaContext();
		public ActionResult Index(DateTime? start, DateTime? end)
		{
			// 1. Xử lý ngày bắt đầu (Nếu null thì lấy ngày 1 tháng hiện tại)
			DateTime fromDate = start ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

			// 2. Xử lý ngày kết thúc
			// Lấy ngày user chọn, nếu không chọn thì lấy ngày hiện tại
			DateTime toDateRaw = end ?? DateTime.Now;

			// 3. Quan trọng: Đẩy giờ về cuối ngày (23:59:59) để lấy hết dữ liệu trong ngày đó
			DateTime toDate = new DateTime(toDateRaw.Year, toDateRaw.Month, toDateRaw.Day, 23, 59, 59);

			ViewBag.StartDate = fromDate.ToString("yyyy-MM-dd");
			ViewBag.EndDate = (end ?? toDate).ToString("yyyy-MM-dd");
			object[] sqlParams = {
				new SqlParameter("@FromDate", fromDate),
				new SqlParameter("@ToDate", toDate)
			};

			var rawData = db.Database.SqlQuery<SupplierStatsViewModel>("EXEC sp_GetSupplierOverview @FromDate, @ToDate", sqlParams).ToList();

			return View(rawData);
		}
	}
}