using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Dashboard
{
    public class DashboardReport
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public bool active { get; set; }
        public string typename { get; set; }        
    }
}
