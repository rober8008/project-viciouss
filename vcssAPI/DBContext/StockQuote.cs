using System;
using System.Collections.Generic;

namespace vcssAPI.DBContext
{
    public partial class StockQuote
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public double Opening { get; set; }
        public double Closing { get; set; }
        public double Minimun { get; set; }
        public double Maximun { get; set; }
        public decimal Volume { get; set; }
        public DateTime DateRound { get; set; }
        public double AdjClose { get; set; }

        public Stock Stock { get; set; }
    }
}
