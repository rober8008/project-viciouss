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
    public class AdminReportTypesController : Controller
    {
        // GET: AdminReportTypes
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetReportTypes()
        {
            return Json(ReportTypeService.GetReportTypes().ToList<ReportTypeModel>(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(ReportTypeModel reporttype)
        {
            try
            {
                if (reporttype.Id > 0)
                    ReportTypeService.UpdateReportType(reporttype);
                else
                    ReportTypeService.CreateReportType(ref reporttype);
                return Json(new { Status = "OK", ItemId = reporttype.Id }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { Status = "ERROR", ItemId = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}