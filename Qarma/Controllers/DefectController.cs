using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Qarma.ViewModels;

namespace Qarma.Controllers
{
    public class DefectController : Controller
    {
        public ActionResult Index()
        {
            var model = new DefectOverviewViewModel
            {
                FromDate = "01/01/2023",
                ToDate = "26/09/2024",

                // 1. DỮ LIỆU CHART STACKED (Giả lập theo ảnh)
                DefectTrends = new List<DefectTrendItem>
                {
                    new DefectTrendItem { MonthYear = "Jan-23", Critical = 0, Major = 7, Minor = 0 },
                    new DefectTrendItem { MonthYear = "Feb-23", Critical = 4, Major = 4, Minor = 3 },
                    new DefectTrendItem { MonthYear = "Mar-23", Critical = 0, Major = 19, Minor = 11 },
                    // ... Thêm các tháng khác nếu cần ...
                    new DefectTrendItem { MonthYear = "Jan-24", Critical = 4, Major = 3, Minor = 1 },
                    new DefectTrendItem { MonthYear = "Feb-24", Critical = 0, Major = 2, Minor = 2 },
                    new DefectTrendItem { MonthYear = "Mar-24", Critical = 0, Major = 9, Minor = 29 }, // Cột cao nhất
                    new DefectTrendItem { MonthYear = "Apr-24", Critical = 0, Major = 21, Minor = 17 },
                },

                // 2. DỮ LIỆU CHART TRÒN (Giả lập theo ảnh)
                DefectCategories = new List<DefectCategoryItem>
                {
                    new DefectCategoryItem { Name = "Packing & Package", Count = 160 },
                    new DefectCategoryItem { Name = "Fabric", Count = 75 },
                    new DefectCategoryItem { Name = "Accessories", Count = 31 },
                    new DefectCategoryItem { Name = "Sewing & Appearance", Count = 19 },
                    new DefectCategoryItem { Name = "Finishing", Count = 14 }
                }
            };

            // 3. XỬ LÝ DỮ LIỆU PARETO (Tự động tính toán)
            // Giả lập dữ liệu thô từ SQL
            var rawParetoData = new List<ParetoItem>
            {
                new ParetoItem { DefectName = "Incorrect Size Boxes", Count = 42 },
                new ParetoItem { DefectName = "Carton defects / over / under", Count = 38 },
                new ParetoItem { DefectName = "Missing / extra quantity", Count = 32 },
                new ParetoItem { DefectName = "Hole", Count = 29 },
                new ParetoItem { DefectName = "Wrong / missing hangtag", Count = 25 },
                new ParetoItem { DefectName = "Stud / poppers / buttons", Count = 12 },
                new ParetoItem { DefectName = "Pick Missing", Count = 11 },
                new ParetoItem { DefectName = "Others", Count = 11 },
                new ParetoItem { DefectName = "Wrong polybag size", Count = 10 },
                new ParetoItem { DefectName = "Stain / dirt", Count = 10 }
            };

            // Logic tính % Tích lũy (Cumulative Percentage)
            // Sau này có SQL thật bạn vẫn giữ nguyên đoạn logic này
            double totalDefects = rawParetoData.Sum(x => x.Count);
            double runningTotal = 0;

            foreach (var item in rawParetoData)
            {
                runningTotal += item.Count;
                item.CumulativePercentage = Math.Round((runningTotal / totalDefects) * 100, 1);
            }

            model.ParetoData = rawParetoData;

            return View(model);
        }
    }
}