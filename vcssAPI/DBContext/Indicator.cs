using System;
using System.Collections.Generic;

namespace vcssAPI.DBContext
{
    public partial class Indicator
    {
        public Indicator()
        {
            PortfolioStockIndicator = new HashSet<PortfolioStockIndicator>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        public string Param1 { get; set; }
        public string Color1 { get; set; }
        public string Param2 { get; set; }
        public string Color2 { get; set; }
        public string Param3 { get; set; }
        public string Color3 { get; set; }

        public ICollection<PortfolioStockIndicator> PortfolioStockIndicator { get; set; }
    }
}
