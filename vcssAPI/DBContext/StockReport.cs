using System;
using System.Collections.Generic;

namespace vcssAPI.DBContext
{
    public partial class StockReport
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public double Price { get; set; }
        public DateTime DateRound { get; set; }
        public double? Ma20 { get; set; }
        public double? Ma50 { get; set; }
        public double? Ma200 { get; set; }
        public double? Rsi14 { get; set; }
        public double? Momentum12 { get; set; }
        public double? SofastK14 { get; set; }
        public double? SofastD3 { get; set; }
        public double? Macd2612 { get; set; }
        public double? Ma9 { get; set; }
        public double? Ema20 { get; set; }
        public double? Ema12 { get; set; }
        public double? Ema26 { get; set; }
        public double? BoolUp { get; set; }
        public double? BoolLow { get; set; }
        public double? WilliansR { get; set; }

        public Stock Stock { get; set; }
    }
}
