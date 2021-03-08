using ctaCOMMON;
using ctaSERVICES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ctaWEB.Models
{
    public class QuotesCarouselModel
    {
        public List<SimplifiedSymbolQuotes> QuotesList { get ; set; }

        public QuotesCarouselModel(int market_id)
        {
            //this.QuotesList = new List<SimplifiedSymbolQuotes>();
            this.QuotesList = QuotesService.GetQuotesByMarketID(market_id);
        }
    }    
}