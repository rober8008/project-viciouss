using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Dashboard
{
    public class UserDashboard
    {
        public List<DashboardItem> DashboardItems { get; private set; }
        public string Username { get; private set; }

        public int ActivePortfolio_Id { get; set; }

        public UserDashboard(string username)
        {
            this.Username = username;
            this.DashboardItems = new List<DashboardItem>();
        }   
   
        public void AddDashboardItem(DashboardItem item)
        {
            this.DashboardItems.Add(item);            
        }

        public object UserDashboardToJSON()
        {
            return null;
        }
    }
}
