using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ctaWEB.Models
{
    public class DashboardModel
    {
        public DashboardModel() { }

        public DashboardModel(string username, int activePortfolio_Id)
        {
            this.Username = username;
            this.dashboard_content = new DashboardContentModel(username, activePortfolio_Id);            
        }

        public string Username { get; private set; }

        [Required]
        public string Portfolio_Name { get; set; }

        public DashboardContentModel dashboard_content { get; set; }        
    }
}