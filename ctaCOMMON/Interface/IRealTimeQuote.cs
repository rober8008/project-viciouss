using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Interface
{
    public interface IRealTimeQuote
    {
        int Id { get; set; }
        int stock_id { get; set; }
        double opening { get; set; }
        double prev_closing { get; set; }
        double ask { get; set; }
        double ask_size { get; set; }
        double bid { get; set; }
        double bid_size { get; set; }
        double change { get; set; }
        double change_percent { get; set; }
        string last_trade_time { get; set; }
        double last_trade_price { get; set; }
        decimal last_trade_size { get; set; }
        DateTime last_trade_date { get; set; }
        DateTime datetime { get; set; }
    }
}
