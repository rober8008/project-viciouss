using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ctaWEB.Models;
using ctaWEB.Models.AdminModels;
using ctaSERVICES;
using ctaCOMMON.AdminModel;

namespace ctaWEB.Controllers
{
    [Authorize(Users = "amed,jgrau,boby")]
    public class AdminTenantsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetTenants()
        {
            return Json(TenantsService.GetTenants().ToList<TenantModel>(), JsonRequestBehavior.AllowGet);
        }   
        
        public JsonResult GetTenantsType()
        {
            return Json(TenantsService.GetTenantsType().ToList<TenantTypeModel>(), JsonRequestBehavior.AllowGet);
        }        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(TenantModel tenant)
        {
            TenantsService.UpdateTenant(tenant);
            return Json(new { Status = "OK" }, JsonRequestBehavior.AllowGet);
        }          
    }
}
