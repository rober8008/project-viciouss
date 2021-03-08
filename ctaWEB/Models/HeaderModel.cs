using ctaCOMMON.Dashboard;
using ctaSERVICES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ctaWEB.Models
{
    public class HeaderModel
    {
        public int? TenantType { get; private set; }

        public string CurrentUsername { get; private set; }

        public bool CurrentUserIsLoged { get; private set; }

        public HeaderModel(string username)
        {
            if (!String.IsNullOrEmpty(username))
            {
                this.CurrentUsername = username;
                this.TenantType = UserService.GetTenant_Type(username);
            }
            this.CurrentUserIsLoged = System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
        }
    }
}