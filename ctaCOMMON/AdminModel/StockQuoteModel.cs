using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.AdminModel
{
    public class StockQuoteModel
    {
        public int Id { get; set; }
        public int stock_id { get; set; }
        public double opening { get; set; }
        public double closing { get; set; }
        public double minimun { get; set; }
        public double maximun { get; set; }
        public decimal volume { get; set; }
        public DateTime date_round { get; set; }
        public double adj_close { get; set; }
        public StockModel stock { get; set; }
    }
}
