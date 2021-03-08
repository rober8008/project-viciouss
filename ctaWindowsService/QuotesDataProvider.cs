using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaWindowsService
{
    internal abstract class QuotesDataProvider
    {
        public Configs_QuoteDataProvider QuoteDataProviderConfigs  { get; set; }
        public Configs_Market MarketConfigs { get; set; }
        protected QuotesDataProvider(Configs_QuoteDataProvider dataproviderConfigs, Configs_Market marketConfigs)
        {
            this.MarketConfigs = marketConfigs;
            this.QuoteDataProviderConfigs = dataproviderConfigs;
        }

        public abstract List<HistoricalQuoteDBModel> GetHistoricalQuotes(List<mdlStock> stocks);
        public abstract List<RealTimeQuoteDBModel> GetRealTimeQuotes(List<mdlStock> stocks, DateTime date, DBContext dbContext, out List<HistoricalQuoteDBModel> historical);

        public static QuotesDataProvider GetQuoteDataProvider(Configs_QuoteDataProvider dataproviderConfigs, Configs_Market marketConfigs)
        {
            if(marketConfigs.QuoteProviderName == "EOD") return new QuotesDataProvider_EOD(dataproviderConfigs, marketConfigs);
            if (marketConfigs.QuoteProviderName == "BOLSAR") return new QuotesDataProvider_BOLSAR(dataproviderConfigs, marketConfigs);
            return null;
        }
    }
}
