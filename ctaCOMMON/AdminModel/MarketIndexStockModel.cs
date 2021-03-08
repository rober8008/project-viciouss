using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.AdminModel
{
    public class MarketIndexStockModel
    {
        public int Id { get; set; }
        public int marketindex_id { get; set; }
        public string marketindex_name { get; set; }
        public int stock_id { get; set; }
        public string stock_symbol { get; set; }
    }
}
