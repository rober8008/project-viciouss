using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.AdminModel
{
    public class MarketModel
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string work_hours { get; set; }
        public int utc_offset { get; set; }
    }
}
