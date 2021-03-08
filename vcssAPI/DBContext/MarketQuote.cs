using System;
using System.Collections.Generic;

namespace vcssAPI.DBContext
{
    public partial class MarketQuote
    {
        public int Id { get; set; }
        public int MarketId { get; set; }

        public Market Market { get; set; }
    }
}
