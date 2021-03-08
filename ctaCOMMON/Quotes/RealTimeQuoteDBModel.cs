using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaWindowsService
{
    public class RealTimeQuoteDBModel
    {
        public int Id { get; set; }
        public int stock_id { get; set; }
        public double opening { get; set; }
        public double prev_closing { get; set; }
        public double ask { get; set; }
        public double ask_size { get; set; }
        public double bid { get; set; }
        public double bid_size { get; set; }
        public double change { get; set; }
        public double change_percent { get; set; }
        public string last_trade_time { get; set; }
        public double last_trade_price { get; set; }
        public decimal last_trade_size { get; set; }
        public DateTime last_trade_date { get; set; }
        public DateTime datetime { get; set; }
    }
}
