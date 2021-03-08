using ctaCOMMON;
using ctaCOMMON.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ctaWEB.Models
{
    public class UserDashboardModel
    {
        public UserDashboard DashboardContent { get; set; }

        public string Username { get; private set; }

        public UserDashboardModel(string username)
        {
            this.Username = username;
            this.DashboardContent = new UserDashboard(username);
        }
    }
}