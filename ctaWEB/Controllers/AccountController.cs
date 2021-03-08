using ctaSERVICES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ctaWEB.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        /*public JsonResult VerifyNewUserEmail(string username)
        {
            bool exist_email = UserService.VerifyIfEmailExist(username);
        }*/
    }
}