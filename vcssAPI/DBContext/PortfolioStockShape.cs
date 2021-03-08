using System;
using System.Collections.Generic;

namespace vcssAPI.DBContext
{
    public partial class PortfolioStockShape
    {
        public int Id { get; set; }
        public int PortfolioId { get; set; }
        public int StockId { get; set; }
        public int ShapeId { get; set; }
        public string Color { get; set; }
        public DateTime Date1 { get; set; }
        public double Value1 { get; set; }
        public DateTime? Date2 { get; set; }
        public double? Value2 { get; set; }
        public DateTime? Date3 { get; set; }
        public double? Value3 { get; set; }
        public string Name { get; set; }

        public Portfolio Portfolio { get; set; }
        public Shape Shape { get; set; }
        public Stock Stock { get; set; }
    }
}
