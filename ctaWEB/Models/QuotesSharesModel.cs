using ctaCOMMON;
using ctaSERVICES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ctaWEB.Models
{
    public class QuotesSharesModel
    {
        public int Market_ID { get; set; }

        public int? MarketIndex_ID { get; set; }

        public int StockType_ID { get; set; }

        public List<SimplifiedSymbolQuotes> GetQuotesValues
        {
            get { return QuotesService.GetQuotesValues(this.StockType_ID, this.MarketIndex_ID); }
        }

        public QuotesSharesModel(int stockType_id, int? marketIndex_id)
        {
            this.StockType_ID = stockType_id;
            this.MarketIndex_ID = marketIndex_id;
        }
    }
}