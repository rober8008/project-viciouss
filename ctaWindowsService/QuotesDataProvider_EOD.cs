using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ctaCOMMON.DataParser;
using Newtonsoft.Json;

namespace ctaWindowsService
{
    internal class QuotesDataProvider_EOD : QuotesDataProvider
    {
        public QuotesDataProvider_EOD(Configs_QuoteDataProvider dataproviderConfigs, Configs_Market marketConfigs) : base(dataproviderConfigs, marketConfigs)
        {
        }

        public override List<RealTimeQuoteDBModel> GetRealTimeQuotes(List<mdlStock> stocks, DateTime date, DBContext dbContext, out List<HistoricalQuoteDBModel> historical)
        {
            List<RealTimeQuoteDBModel> result = new List<RealTimeQuoteDBModel>();
            historical = new List<HistoricalQuoteDBModel>();

            Dictionary<string, int> symbols = stocks.ToDictionary(s => s.StockSymbol + ".US", s => s.StockId);
            string first = symbols.Keys.First();            
            List<jsonmdlEODRealTime> data;

            using (WebClient web = new WebClient())
            {
                string json_str = web.DownloadString($"{this.QuoteDataProviderConfigs.RealTimeURL}/{first}?api_token={this.QuoteDataProviderConfigs.APIToken}&fmt=json&s={String.Join(",", symbols.Keys.Where(k => k != first))}");
                data = JsonConvert.DeserializeObject<List<jsonmdlEODRealTime>>(json_str);

                result = data.Select(rt => new RealTimeQuoteDBModel()
                {
                    ask = 0,
                    ask_size = 0,
                    bid = 0,
                    bid_size = 0,
                    change = 0,
                    change_percent = rt.Change,
                    datetime = date,
                    last_trade_date = DateTime.Now,
                    last_trade_price = rt.Close,
                    last_trade_size = 0,
                    last_trade_time = "",
                    opening = rt.Close,
                    prev_closing = rt.PreviousClose,
                    stock_id = symbols[rt.Code]
                }).ToList<RealTimeQuoteDBModel>();

                //HISTORICAL DATA
                historical = data.Select(h => new HistoricalQuoteDBModel()
                {
                    adj_close = 0,
                    closing = h.Close,
                    date_round = date,
                    maximun = h.High,
                    minimun = h.Low,
                    opening = h.Open,
                    stock_id = symbols[h.Code],
                    volume = h.Volume
                }).ToList<HistoricalQuoteDBModel>();

            }

            return result;
        }

        public override List<HistoricalQuoteDBModel> GetHistoricalQuotes(List<mdlStock> stocks)
        {
            throw new NotImplementedException();
        }

        public bool AreSameEODRealTime(jsonmdlEODRealTime rt1, jsonmdlEODRealTime rt2)
        {
            return rt1.Code == rt2.Code;
        }
    }
}
