using Qarma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Qarma.Content
{
    public class InspectorController : Controller
    {
        // GET: Inspector
        public ActionResult Index()
        {
            var mockData = new List<InspectorMetric>
            {
                new InspectorMetric { Name = "QC Inspector 31", Email = "inspector31@qarmainspect.com", Minor = 12, Major = 63, Critical = 9, DefectRate = 0.53, SampleQuantity = 15893, Inspections = 27, RejectionRate = 7.41, ReInspections = 0, ConclusionAgreementRate = 59.26, LocationRate = 88.89, AvgDuration = 0.17 },
                new InspectorMetric { Name = "QC Inspector 19", Email = "inspector19@qarmainspect.com", Minor = 23, Major = 9, Critical = 0, DefectRate = 3.19, SampleQuantity = 1003, Inspections = 7, RejectionRate = 0.00, ReInspections = 1, ConclusionAgreementRate = 71.43, LocationRate = 57.14, AvgDuration = 0.26 },
                // Dòng có chấm đỏ Defect Rate & Rejection Rate
                new InspectorMetric { Name = "QC Inspector 17", Email = "inspector17@qarmainspect.com", Minor = 7, Major = 12, Critical = 3, DefectRate = 11.58, SampleQuantity = 190, Inspections = 5, RejectionRate = 80.00, ReInspections = 0, ConclusionAgreementRate = 60.00, LocationRate = 100.00, AvgDuration = 0.27 },
                new InspectorMetric { Name = "QC Inspector 5", Email = "inspector5@qarmainspect.com", Minor = 7, Major = 11, Critical = 4, DefectRate = 20.56, SampleQuantity = 107, Inspections = 5, RejectionRate = 20.00, ReInspections = 0, ConclusionAgreementRate = 100.00, LocationRate = 100.00, AvgDuration = 657.93 }, // Số liệu ảo ma trong ảnh gốc
                new InspectorMetric { Name = "QC Inspector 4", Email = "inspector4@qarmainspect.com", Minor = 0, Major = 20, Critical = 0, DefectRate = 2.47, SampleQuantity = 810, Inspections = 3, RejectionRate = 33.33, ReInspections = 0, ConclusionAgreementRate = 33.33, LocationRate = 100.00, AvgDuration = 0.33 },
                new InspectorMetric { Name = "QC Inspector 4", Email = "inspector4@qarmainspect.com", Minor = 0, Major = 20, Critical = 0, DefectRate = 2.47, SampleQuantity = 810, Inspections = 3, RejectionRate = 33.33, ReInspections = 0, ConclusionAgreementRate = 33.33, LocationRate = 100.00, AvgDuration = 0.33 },
                new InspectorMetric { Name = "QC Inspector 4", Email = "inspector4@qarmainspect.com", Minor = 0, Major = 20, Critical = 0, DefectRate = 2.47, SampleQuantity = 810, Inspections = 3, RejectionRate = 33.33, ReInspections = 0, ConclusionAgreementRate = 33.33, LocationRate = 100.00, AvgDuration = 0.33 },
                new InspectorMetric { Name = "QC Inspector 4", Email = "inspector4@qarmainspect.com", Minor = 0, Major = 20, Critical = 0, DefectRate = 2.47, SampleQuantity = 810, Inspections = 3, RejectionRate = 33.33, ReInspections = 0, ConclusionAgreementRate = 33.33, LocationRate = 100.00, AvgDuration = 0.33 },
                new InspectorMetric { Name = "QC Inspector 4", Email = "inspector4@qarmainspect.com", Minor = 0, Major = 20, Critical = 0, DefectRate = 2.47, SampleQuantity = 810, Inspections = 3, RejectionRate = 33.33, ReInspections = 0, ConclusionAgreementRate = 33.33, LocationRate = 100.00, AvgDuration = 0.33 },
                new InspectorMetric { Name = "QC Inspector 4", Email = "inspector4@qarmainspect.com", Minor = 0, Major = 20, Critical = 0, DefectRate = 2.47, SampleQuantity = 810, Inspections = 3, RejectionRate = 33.33, ReInspections = 0, ConclusionAgreementRate = 33.33, LocationRate = 100.00, AvgDuration = 0.33 },
                new InspectorMetric { Name = "QC Inspector 4", Email = "inspector4@qarmainspect.com", Minor = 0, Major = 20, Critical = 0, DefectRate = 2.47, SampleQuantity = 810, Inspections = 3, RejectionRate = 33.33, ReInspections = 0, ConclusionAgreementRate = 33.33, LocationRate = 100.00, AvgDuration = 0.33 },
                new InspectorMetric { Name = "QC Inspector 4", Email = "inspector4@qarmainspect.com", Minor = 0, Major = 20, Critical = 0, DefectRate = 2.47, SampleQuantity = 810, Inspections = 3, RejectionRate = 33.33, ReInspections = 0, ConclusionAgreementRate = 33.33, LocationRate = 100.00, AvgDuration = 0.33 },
                // Thêm vài dòng thường
                new InspectorMetric { Name = "QC Inspector 28", Email = "inspector28@qarmainspect.com", Minor = 0, Major = 12, Critical = 0, DefectRate = 1.02, SampleQuantity = 1175, Inspections = 4, RejectionRate = 0.00, ReInspections = 0, ConclusionAgreementRate = 75.00, LocationRate = 75.00, AvgDuration = 1.50 },
            };

            return View(mockData);
        }
    }
}