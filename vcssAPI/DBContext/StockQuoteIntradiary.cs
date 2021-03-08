using System;
using System.Collections.Generic;

namespace vcssAPI.DBContext
{
    public partial class StockQuoteIntradiary
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public double Opening { get; set; }
        public double PrevClosing { get; set; }
        public double Ask { get; set; }
        public double AskSize { get; set; }
        public double Bid { get; set; }
        public double BidSize { get; set; }
        public double Change { get; set; }
        public double ChangePercent { get; set; }
        public string LastTradeTime { get; set; }
        public double LastTradePrice { get; set; }
        public decimal LastTradeSize { get; set; }
        public DateTime LastTradeDate { get; set; }
        public DateTime Datetime { get; set; }

        public Stock Stock { get; set; }
    }
}
