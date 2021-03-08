using ctaCOMMON.AdminModel;
using ctaCOMMON.DataParser;
using ctaCOMMON.Interface;
using ctaCOMMON.Quotes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ctaSERVICES.QuotesProvider
{
    public class EODQuotesProvider : QuotesProvider
    {
        public EODQuotesProvider()
        {
            //HISTORICAL - https://eodhistoricaldata.com/api/eod/AAPL.US?from=YYYY-MM-DD&to=YYYY-MM-DD&api_token=5a32d75e4ebab&period=d&fmt=json
            //REAL TIME - https://eodhistoricaldata.com/api/real-time/AAPL.US?api_token=5a32d75e4ebab&fmt=json&s=VTI,EUR.FX
        }

        public override async Task<string> GetQuotesFromExternalInJsonFormatAsync()
        {
            try
            {
                string EODapiToken = ConfigService.GetConfig("EOD_APIToken").ConfigValue;
                string quotesURL = ConfigService.GetConfig("EOD_Quotes_URL").ConfigValue;

                Dictionary<string, int> symbols = StockService.GetStocks().Where(s => (s.market_id == 2 || s.market_id == 3) && s.active).ToDictionary(s => s.symbol + ".US", s => s.Id);
                using (WebClient web = new WebClient())
                {
                    return await web.DownloadStringTaskAsync(String.Format(quotesURL, symbols.Keys.First(), EODapiToken, String.Join(",", symbols.Keys)));
                }
            }
            catch(Exception ex)
            {
                //TODO - Log error
                TaskManagerService.LogTaskError("EODQuotesProvider - GetQuotesFromExternalInJsonFormatAsync", ex.Message);
                return "";
            }
        }

        public override Tuple<List<IRealTimeQuote>, List<IHistoricalQuote>> GetQuotesFromJson(string json)
        {
            try
            {
                int utc_offset = ConfigService.GetConfigs().Where(c => c.ConfigName == "USA_UTCOffset").Select(c => int.Parse(c.ConfigValue)).FirstOrDefault();
                DateTime date = DateTime.UtcNow.AddHours(utc_offset);
                Dictionary<string, int> symbols = StockService.GetStocks().Where(s => (s.market_id == 2 || s.market_id == 3) && s.active).ToDictionary(s => s.symbol + ".US", s => s.Id);
                IEnumerable<jsonmdlEODRealTime> data = JsonConvert.DeserializeObject<List<jsonmdlEODRealTime>>(json).Where(rt => symbols.ContainsKey(rt.Code));                 

                //INTRADIARY DATA
                List<IRealTimeQuote>  realTimeQuotes = data.Select(rt => new RealTimeQuoteEOD()
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
                }).ToList<IRealTimeQuote>();

                //HISTORICAL DATA
                List<IHistoricalQuote>  historicalQuotes = data.Select(h => new HistoricalQuoteEOD()
                {
                    adj_close = 0,
                    closing = h.Close,
                    date_round = date,
                    maximun = Math.Max(h.Close, h.Open),
                    minimun = Math.Min(h.Close, h.Open),
                    opening = h.Open,
                    stock_id = symbols[h.Code],
                    volume = h.Volume
                }).ToList<IHistoricalQuote>();

                return new Tuple<List<IRealTimeQuote>, List<IHistoricalQuote>>(realTimeQuotes, historicalQuotes);
            }
            catch (Exception ex)
            {
                //TODO - Log error
                TaskManagerService.LogTaskError("EODQuotesProvider - GetQuotesFromJson", ex.Message);
                return null;
            }
        }

        public override void UpdateAuthToken()
        {
            throw new NotImplementedException();
        }

        public override async Task UpdateHistoricalQuote(int stockID, DateTime from, DateTime to)
        {
            try
            {
                string EODapiToken = ConfigService.GetConfigs().Where(c => c.ConfigName == "EOD_APIToken").Select(c => c.ConfigValue).FirstOrDefault();
                string quotesURL = ConfigService.GetConfigs().Where(c => c.ConfigName == "EOD_HistoricalQuotes_URL").Select(c => c.ConfigValue).FirstOrDefault();
                StockModel stock = StockService.GetStocks().Where(s => s.Id == stockID).FirstOrDefault();

                using (WebClient web = new WebClient())
                {
                    string jsonHistoricalData = await web.DownloadStringTaskAsync(String.Format(quotesURL, stock.symbol, from.ToString("yyyy-MM-dd"), to.ToString("yyyy-MM-dd"), EODapiToken));

                    List<jsonmdlEODHistorical> data = JsonConvert.DeserializeObject<List<jsonmdlEODHistorical>>(jsonHistoricalData);
                    List<HistoricalQuoteEOD> historicalQuotes = data.Select(h => new HistoricalQuoteEOD()
                    {
                        adj_close = h.AdjustedClose,
                        closing = h.Close,
                        date_round = h.Date,
                        maximun = h.High,
                        minimun = h.Low,
                        opening = h.Open,
                        stock_id = stockID,
                        volume = h.Volume
                    }).ToList<HistoricalQuoteEOD>();
                    QuotesService.SaveHistoricalQuote(historicalQuotes.ToList<IHistoricalQuote>());
                }
            }
            catch(Exception ex)
            {
                //TODO - Log error
                TaskManagerService.LogTaskError("EODQuotesProvider - UpdateHistoricalQuote", ex.Message);                
            }
        }     
    }
}
