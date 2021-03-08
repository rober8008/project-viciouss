using ctaCOMMON;
using ctaSERVICES;
using ctaSERVICES.Reporting;
using ctaWEB.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ctaWEB.Controllers
{
    public class HomeController : Controller
    {
        //[Authorize/*(Roles = "Administrator")*/]
        public ActionResult Index()
        {
            return View();
            //return View("~/Views/Home/Home2.cshtml"); 
        }

        public ActionResult Welcome()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Empty()
        {            
            return View();
        }

        public ActionResult IntradiaryQuotes()
        {
            return View();
        }

        public ActionResult Rates()
        {
            return View();
        }

        public ActionResult Informes()
        {
            ViewBag.Informes = DashboardService.GetReports("Informes Pagos");
            return View();
        }

        public ActionResult DailyReportInfo()
        {
            return View();
        }

        public ActionResult Recomendations()
        {
            return View();
        }

        public FileResult CreatePdf(string username)
        {
            if (System.Web.HttpContext.Current.User.Identity.Name == username && UserService.GetTenant_Type(username) != 1)
            {
                ctaSERVICES.Reporting.ReportService service = new ctaSERVICES.Reporting.ReportService();

                return File(service.CreatePdf(Server.MapPath("../Images/logo/Viciouss_3.0.png"), Server.MapPath("../Images/logo/reporte_reglas.png")), "application/pdf", "Reporte Diario de Indicadores de Análisis Técnico.pdf");
            }

            return null;
            
        }
    }
}