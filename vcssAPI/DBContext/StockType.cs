using System;
using System.Collections.Generic;

namespace vcssAPI.DBContext
{
    public partial class StockType
    {
        public StockType()
        {
            Stock = new HashSet<Stock>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Stock> Stock { get; set; }
    }
}
