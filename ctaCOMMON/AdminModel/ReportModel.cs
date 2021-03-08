using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.AdminModel
{
    public class ReportModel
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public int type { get; set; }        
        public bool active { get; set; }        
    }
}
