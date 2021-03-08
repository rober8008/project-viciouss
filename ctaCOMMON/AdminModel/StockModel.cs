using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.AdminModel
{
    public class StockModel
    {
        public int Id { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public int market_id { get; set; }
        public int type_id { get; set; }
        public string description { get; set; }
        public bool active { get; set; }
        public string market_name { get; set; }
    }
}
