using System;
using System.Collections.Generic;

namespace vcssAPI.DBContext
{
    public partial class Portfolio
    {
        public Portfolio()
        {
            PortfolioStock = new HashSet<PortfolioStock>();
            PortfolioStockIndicator = new HashSet<PortfolioStockIndicator>();
            PortfolioStockShape = new HashSet<PortfolioStockShape>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }

        public Tenant User { get; set; }
        public ICollection<PortfolioStock> PortfolioStock { get; set; }
        public ICollection<PortfolioStockIndicator> PortfolioStockIndicator { get; set; }
        public ICollection<PortfolioStockShape> PortfolioStockShape { get; set; }
    }
}
