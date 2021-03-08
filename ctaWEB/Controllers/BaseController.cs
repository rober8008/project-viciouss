using ctaWEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ctaWEB.Controllers
{
    public class BaseController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            //retrieve data
            string username = System.Web.HttpContext.Current.User.Identity.Name;

            if (!String.IsNullOrEmpty(username))
            {
                DashboardModel dashboard = new DashboardModel(username, 0);
                ViewBag.DashboardData = dashboard;
            }
            else
            {
                DashboardModel dashboard = new DashboardModel();
                ViewBag.DashboardData = dashboard;
            }
        }
    }
}