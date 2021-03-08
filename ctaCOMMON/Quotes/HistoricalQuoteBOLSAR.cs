using ctaCOMMON.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Quotes
{
    public class HistoricalQuoteBOLSAR : IHistoricalQuote
    {
        public int Id { get; set; }
        public int stock_id { get; set; }
        public double opening { get; set; }
        public double closing { get; set; }
        public double minimun { get; set; }
        public double maximun { get; set; }
        public decimal volume { get; set; }
        public System.DateTime date_round { get; set; }
        public double adj_close { get; set; }
    }
}
