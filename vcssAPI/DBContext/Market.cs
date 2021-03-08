using System;
using System.Collections.Generic;

namespace vcssAPI.DBContext
{
    public partial class Market
    {
        public Market()
        {
            Holidays = new HashSet<Holidays>();
            MarketIndex = new HashSet<MarketIndex>();
            MarketQuote = new HashSet<MarketQuote>();
            Stock = new HashSet<Stock>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string WorkHours { get; set; }

        public ICollection<Holidays> Holidays { get; set; }
        public ICollection<MarketIndex> MarketIndex { get; set; }
        public ICollection<MarketQuote> MarketQuote { get; set; }
        public ICollection<Stock> Stock { get; set; }
    }
}
