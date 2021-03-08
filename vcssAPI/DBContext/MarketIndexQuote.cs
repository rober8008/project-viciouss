using System;
using System.Collections.Generic;

namespace vcssAPI.DBContext
{
    public partial class MarketIndexQuote
    {
        public int Id { get; set; }
        public int MarketindexId { get; set; }

        public MarketIndex Marketindex { get; set; }
    }
}
