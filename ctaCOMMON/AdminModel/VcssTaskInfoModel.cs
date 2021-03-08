using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.AdminModel
{
    public class VcssTaskInfoModel
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int? market_id { get; set; }
        public string url { get; set; }
        public string auth_token { get; set; }       
        public DateTime last_updated { get; set; }
        public int step { get; set; }
        public MarketModel Market { get; set; }
    }
}
