using System;
using System.Collections.Generic;

namespace vcssAPI.DBContext
{
    public partial class MarketIndex
    {
        public MarketIndex()
        {
            MarketIndexQuote = new HashSet<MarketIndexQuote>();
            MarketIndexStock = new HashSet<MarketIndexStock>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int MarketId { get; set; }

        public Market Market { get; set; }
        public ICollection<MarketIndexQuote> MarketIndexQuote { get; set; }
        public ICollection<MarketIndexStock> MarketIndexStock { get; set; }
    }
}
