using ctaSERVICES;
using ctaWEB.Models;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace ctaWEB.Controllers
{
    public class UserController : BaseController
    {
        public ActionResult Login(string returnUrl = "")
        {
            return View();            
        }

        public ActionResult Welcome(string returnUrl = "")
        {
            return View();
        }

        public ActionResult PasswordSent(string returnUrl = "")
        {
            return View();
        }

        public ActionResult Activated(string returnUrl = "")
        {
            return View();
        }

        public ActionResult DoActivate(string token)
        {
            var userActivated = UserService.ActivateUser(token);

            if (userActivated)
                return RedirectToAction("Activated", "User");
            else
                return RedirectToAction("NotWelcome", "User");
        }

        public ActionResult Register(string returnUrl = "")
        {
            return View();
        }

        public ActionResult Logout()
        {
            UserService.LogoutUser();
            System.Web.HttpContext.Current.User = null;
            return View();
        }

        [HttpPost]        
        public ActionResult DoModalLogin(string username, string password)
        {
            object data = new { success = false, error = true, message = App_GlobalResources.Modals.LogIn_Failed };
            string usernameFromDB = UserService.ValidateUserAndPassword(username, password);
            if (!String.IsNullOrEmpty(username))
            {
                FormsAuthentication.SetAuthCookie(usernameFromDB, false);
                data = new { success = true, url = Url.Action("Index", "Dashboard") };                
            }   
            return Json(data, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult DoForgotPassword(string email)
        {
            if(string.IsNullOrEmpty(email))
                return Json(new { success = false, error = true, message = "Email no puede ser vacío" }, JsonRequestBehavior.DenyGet);

            object data;
            string psw = UserService.GetUserPass(email);

            switch (psw)
            {
                case "":
                    data = new { success = false, error = true, message = "No existe una cuenta con este e-mail" };
                    break;

                case "facebook":
                    data = new { success = false, error = true, message = "Email en uso, por favor ingrese usando facebook" };
                    break;

                default:
                    data = new { success = true, url = Url.Action("PasswordSent", "User") };
                    break;
            }

            return Json(data, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult DoFacebookLogin(string username, string email)
        {
            var validUsername = UserService.ValidateUser(email);

            if (string.IsNullOrEmpty(validUsername))
            {
                FormsAuthentication.SetAuthCookie(username, false);

                UserService.RegisterUser(email, username, "", Server.MapPath("../Images/"));
            }
            else
            {
                FormsAuthentication.SetAuthCookie(validUsername, false);

                UserService.ActivateFacebookUser(email);
            }

            object data = new { success = true, url = Url.Action("Index", "Dashboard") }; 

            return Json(data, JsonRequestBehavior.DenyGet);
        }


        [HttpPost]
        public ActionResult DoModalRegister(RegisterModel newUserToRegister)
        {
            if (!ModelState.IsValid)
            {
                object response = new { success = false, message = "Model not valid" };
                return Json(response, JsonRequestBehavior.DenyGet);
            }

            var validUsername = UserService.ValidateUser(newUserToRegister.email);
            object data;

            if (string.IsNullOrEmpty(validUsername))
            {
                UserService.RegisterUser(newUserToRegister.email, newUserToRegister.username, newUserToRegister.password, Server.MapPath("../Images/"));

                data = new { success = true, url = Url.Action("Welcome", "User") };
            }
            else
            {
                data = new { success = false, error = true, message = "Email en uso. Por favor usálo para entrar al sitio o introducí otro." };
            }
 
            return Json(data, JsonRequestBehavior.DenyGet);            
        }

        [HttpPost]        
        public ActionResult DoLogin(string username, string password, string returnUrl = "")
        {
            string usernameFromDB = UserService.ValidateUserAndPassword(username, password);
            if (!String.IsNullOrEmpty(usernameFromDB))
            {
                FormsAuthentication.SetAuthCookie(usernameFromDB, false);
                return RedirectToAction("Index", "Dashboard");
            }
            else
                return RedirectToAction("Login", "User");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoContact(ContactModel model)
        {
            try
            {
                ContactService.DoContact(model.Nombre, model.Email, /*model.Telefono*/ "No-Phone", model.Mensaje);
                return Json(
                    new
                    {
                        success = true
                    }, JsonRequestBehavior.AllowGet
                );
            }
            catch (Exception ex)
            {
                //TODO Log error
                return Json(
                        new
                        {
                            success = false,
                            message = "Hubo un problema al enviar el formulario, por favor reintente mas tarde.",
                        }, JsonRequestBehavior.AllowGet
                    );
            }
        }
    }
}