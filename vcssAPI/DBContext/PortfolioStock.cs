using System;
using System.Collections.Generic;

namespace vcssAPI.DBContext
{
    public partial class PortfolioStock
    {
        public int Id { get; set; }
        public int PortfolioId { get; set; }
        public int StockId { get; set; }

        public Portfolio Portfolio { get; set; }
        public Stock Stock { get; set; }
    }
}
