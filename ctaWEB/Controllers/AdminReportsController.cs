using ctaCOMMON.AdminModel;
using ctaSERVICES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ctaWEB.Controllers
{
    [Authorize(Users = "amed,jgrau,boby")]
    public class AdminReportsController : Controller
    {
        // GET: AdminReports
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetReports()
        {
            return Json(ReportService.GetReports().ToList<ReportModel>(), JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(ReportModel report)
        {
            try
            {
                if (report.Id > 0)
                    ReportService.UpdateReport(report);
                else
                    ReportService.CreateReport(ref report);
                return Json(new { Status = "OK", ItemId = report.Id }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { Status = "ERROR", ItemId = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}