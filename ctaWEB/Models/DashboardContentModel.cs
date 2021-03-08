using ctaCOMMON;
using ctaCOMMON.Dashboard;
using ctaSERVICES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ctaWEB.Models
{
    public class DashboardContentModel
    {
        public string Username { get; private set; }
        public string MorningBrief { get; set; }
        public string DailyReportDescription { get; set; }

        public UserDashboard Dashboard {get; private set;}
        public Dictionary<string, List<DashboardReport>> Reports { get; private set; }

        public DashboardContentModel(string username, int activePortfolio_Id)
        {
            this.Username = username;
            this.Dashboard = DashboardService.GetDashboard(username);
            this.Reports = DashboardService.GetReports();
            this.Dashboard.ActivePortfolio_Id = activePortfolio_Id;
            this.MorningBrief = ConfigService.GetConfig("DashboardMorningBrief").ConfigValue;
            this.DailyReportDescription = ConfigService.GetConfig("DashboardDailyReportDescription").ConfigValue;
        }
    }
}