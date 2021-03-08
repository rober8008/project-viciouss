using ctaCOMMON;
using ctaDATAMODEL;
using ctaMEMBERSHIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace ctaSERVICES
{
    public static class UserService
    {
        /// <summary>
        /// Returns UserName in case the login is done by email
        /// </summary>
        /// <param name="usernameORemail"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string ValidateUserAndPassword(string usernameORemail, string password)
        {
            ctaMembershipProvider member = new ctaMembershipProvider();
            if(member.ValidateUser(usernameORemail, password))
            {
                return UserService.GetUserName(usernameORemail);
            }
            else 
                return null;            
        }        

        public static void LogoutUser()
        {
            FormsAuthentication.SignOut();
        }

        public static int GetUserId(string usernameORemail)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();
                int user_id = entities.Tenants.Where(u => (u.username == usernameORemail || u.email == usernameORemail)).Select(u => u.Id).FirstOrDefault();
                entities.Database.Connection.Close();
                return user_id;
            }
        }

        public static string GetUserName(string usernameORemail)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();
                string user_name = entities.Tenants.Where(u => (u.username == usernameORemail || u.email == usernameORemail)).Select(u => u.username).FirstOrDefault();
                entities.Database.Connection.Close();
                return user_name;
            }
        }

        public static string GetUserPass(string email)
        {
            Tenant current = null;
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();

                current = entities.Tenants.Where(x => x.email == email).FirstOrDefault();

                entities.Database.Connection.Close();
            }

            return current != null ? string.IsNullOrEmpty(current.secretpass) ? "facebook" : EmailSender.SendForgotPSWMail(current.email, current.secretpass, current.username) : "";
        }

        public static void RegisterUser(string email, string username, string password, string imgPath)
        {
            Tenant inserted_user = null;
            string activationId = string.IsNullOrEmpty(password) ? string.Empty : Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 16);

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();
                Tenant u = new Tenant() { secretpass = password, username = username, email = email, activationId = activationId, type = 1, typeExpiration = DateTime.Now };

                inserted_user = entities.Tenants.Add(u);
                entities.SaveChanges();               
                
                entities.Database.Connection.Close();                
            }
            
            //send confirmation mail
            EmailSender.SendConfirmationMail(email, activationId, imgPath);
        }

        public static string ValidateUser(string email)
        {
            Tenant tenant = null;

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();

                tenant = entities.Tenants.Where(x => x.email == email).FirstOrDefault();

                entities.Database.Connection.Close();
            }

            return tenant != null ? tenant.username : string.Empty;
        }

        public static int? GetTenant_Type(string usernameORemail)
        {
            Tenant_Type type = null;

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();

                var tenant = entities.Tenants.Where(x => (x.username == usernameORemail || x.email == usernameORemail)).FirstOrDefault();
                type = tenant.Tenant_Type;

                entities.Database.Connection.Close();
            }

            return type != null ? type.Id : (int?)null;
        }        

        public static bool VerifyIfEmailExist(string username)
        {
            throw new NotImplementedException();
        }

        public static bool ActivateUser(string token)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();

                var tenant = entities.Tenants.Where(u => u.activationId == token).FirstOrDefault();

                if (tenant != null)
                {
                    tenant.activationId = "";
                    tenant.type = 2;
                    tenant.typeExpiration = DateTime.Now.AddMonths(1);

                    entities.SaveChanges();

                    //Send email to contact@viciouss.com to inform new activated user
                    string mensaje = "Se ha registrado y validado un nuevo usuario:<br/><br/><hr/>" +
                        "Datos de usuario:<br/>" +
                                        "Username: " + tenant.username + "<br/>" +
                                        "Email: " + tenant.email;
                    EmailSender.SendHTMLEmail("contact@viciouss.com", "postmaster@viciouss.com", "[Viciouss] Nuevo usuario registrado", mensaje);

                    entities.Database.Connection.Close();

                    return true;
                }
                else
                {
                    entities.Database.Connection.Close();
                    return false; 
                }
            }
        }

        public static void ActivateFacebookUser(string email)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();

                var tenant = entities.Tenants.Where(u => u.email == email).FirstOrDefault();

                tenant.activationId = "";
                tenant.type = 2;
                tenant.typeExpiration = DateTime.Now.AddMonths(1);

                entities.SaveChanges();

                entities.Database.Connection.Close();
            }
        }

        public static void ValidateUserTypeExpiration(DateTime date)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();

                var tenants = entities.Tenants.Where(u => u.type == 2 && u.typeExpiration < date);
                foreach (var tenant in tenants)
                {
                    tenant.type = 1;
                }                

                entities.SaveChanges();
                entities.Database.Connection.Close();
            }
        }

        public static void UpdateUserTypeExpirationFromMercadoPago(DateTime date)
        {
            //MercadoPago.SDK.AccessToken = "TEST-4874303055372851-012200-0d71136e75dbb5ea167424aa680c54c1-327289499";
            //MercadoPago.Resources.MerchantOrder mo = new MercadoPago.Resources.MerchantOrder();
            //MercadoPago.Customer c = new MercadoPago.Customer();
            //MercadoPago.Resources.Subscription s = new MercadoPago.Resources.Subscription();
            //s.

            //using (ctaDBEntities entities = new ctaDBEntities())
            //{
            //    entities.Database.Connection.Open();

                
            //    entities.Database.Connection.Close();
            //}
        }
    }
}
