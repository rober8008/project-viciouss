using System;
using System.Collections.Generic;

namespace vcssAPI.DBContext
{
    public partial class PortfolioStockIndicator
    {
        public int Id { get; set; }
        public int PortfolioId { get; set; }
        public int StockId { get; set; }
        public int IndicatorId { get; set; }
        public string Param1 { get; set; }
        public string Color1 { get; set; }
        public string Param2 { get; set; }
        public string Color2 { get; set; }
        public string Param3 { get; set; }
        public string Color3 { get; set; }

        public Indicator Indicator { get; set; }
        public Portfolio Portfolio { get; set; }
        public Stock Stock { get; set; }
    }
}
