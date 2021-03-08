using System;
using System.Collections.Generic;

namespace vcssAPI.DBContext
{
    public partial class Holidays
    {
        public int Id { get; set; }
        public int MarketId { get; set; }
        public DateTime Date { get; set; }
        public string Duration { get; set; }

        public Market Market { get; set; }
    }
}
