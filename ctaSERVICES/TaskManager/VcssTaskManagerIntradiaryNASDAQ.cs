using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ctaCOMMON.AdminModel;
using ctaCOMMON.DataParser;
using ctaCOMMON.Interface;
using ctaCOMMON.Quotes;
using Newtonsoft.Json;

namespace ctaSERVICES.TaskManager
{
    public class VcssTaskManagerIntradiaryNASDAQ : VcssTaskManagerIntradiary
    {
        protected override void ExecuteTask(VcssTaskModel vcssTask)
        {            
            DateTime marketDate = DateTime.UtcNow.AddHours(vcssTask.VcssTaskInfo.Market.utc_offset).Date;
            Dictionary<string, int> symbols = StockService.GetStocks().Where(s => s.market_id == 3 && s.active).ToDictionary(s => s.symbol + ".US", s => s.Id);
            IEnumerable<jsonmdlEODRealTime> data = JsonConvert.DeserializeObject<List<jsonmdlEODRealTime>>(vcssTask.data).Where(rt => symbols.ContainsKey(rt.Code));

            //INTRADIARY DATA
            List<IRealTimeQuote> realTimeQuotes = data.Select(rt => new RealTimeQuoteEOD()
            {
                ask = 0,
                ask_size = 0,
                bid = 0,
                bid_size = 0,
                change = 0,
                change_percent = rt.Change,
                datetime = this.GetIntradiaryDateTime(vcssTask.VcssTaskInfo.Market.utc_offset, rt.Timestamp),
                last_trade_date = DateTime.Now,
                last_trade_price = rt.Close,
                last_trade_size = 0,
                last_trade_time = "",
                opening = rt.Close,
                prev_closing = rt.PreviousClose,
                stock_id = symbols[rt.Code]
            }).ToList<IRealTimeQuote>();

            //HISTORICAL DATA
            List<IHistoricalQuote> historicalQuotes = data.Select(h => new HistoricalQuoteEOD()
            {
                adj_close = 0,
                closing = h.Close,
                date_round = marketDate,
                maximun = h.High,
                minimun = h.Low,
                opening = h.Open,
                stock_id = symbols[h.Code],
                volume = h.Volume
            }).ToList<IHistoricalQuote>();

            QuotesService.SaveRealTimeQuote(realTimeQuotes);
            QuotesService.SaveHistoricalQuote(historicalQuotes);            
        }

        protected override void ScheduleTask(VcssTaskInfoModel vcssTaskInfo, params string[] parameters)
        {
            Dictionary<string, int> symbols = StockService.GetStocks().Where(s => s.market_id == 3 && s.active).ToDictionary(s => s.symbol + ".US", s => s.Id);
            using (WebClient web = new WebClient())
            {
                string quotesData = web.DownloadString(String.Format(vcssTaskInfo.url, symbols.Keys.First(), vcssTaskInfo.auth_token, String.Join(",", symbols.Keys)));
                DateTime marketTime = DateTime.UtcNow.AddHours(vcssTaskInfo.Market.utc_offset);
                VcssTaskManagerService.ScheduleTask(vcssTaskInfo.Id, quotesData, marketTime);
            }
        }

        private DateTime GetIntradiaryDateTime(int utcOffset, long timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(timestamp).AddHours(utcOffset);
        }
    }
}
