using System;
using System.Collections.Generic;

namespace vcssAPI.DBContext
{
    public partial class MarketIndexStock
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public int MarketindexId { get; set; }

        public MarketIndex Marketindex { get; set; }
        public Stock Stock { get; set; }
    }
}
