using System;
using System.Collections.Generic;

namespace vcssAPI.DBContext
{
    public partial class Stock
    {
        public Stock()
        {
            MarketIndexStock = new HashSet<MarketIndexStock>();
            PortfolioStock = new HashSet<PortfolioStock>();
            PortfolioStockIndicator = new HashSet<PortfolioStockIndicator>();
            PortfolioStockShape = new HashSet<PortfolioStockShape>();
            StockQuote = new HashSet<StockQuote>();
            StockQuoteIntradiary = new HashSet<StockQuoteIntradiary>();
            StockReport = new HashSet<StockReport>();
        }

        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public int MarketId { get; set; }
        public int TypeId { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }

        public Market Market { get; set; }
        public StockType Type { get; set; }
        public ICollection<MarketIndexStock> MarketIndexStock { get; set; }
        public ICollection<PortfolioStock> PortfolioStock { get; set; }
        public ICollection<PortfolioStockIndicator> PortfolioStockIndicator { get; set; }
        public ICollection<PortfolioStockShape> PortfolioStockShape { get; set; }
        public ICollection<StockQuote> StockQuote { get; set; }
        public ICollection<StockQuoteIntradiary> StockQuoteIntradiary { get; set; }
        public ICollection<StockReport> StockReport { get; set; }
    }
}
