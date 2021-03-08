using System;
using System.Collections.Generic;

namespace vcssAPI.DBContext
{
    public partial class Shape
    {
        public Shape()
        {
            PortfolioStockShape = new HashSet<PortfolioStockShape>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<PortfolioStockShape> PortfolioStockShape { get; set; }
    }
}
