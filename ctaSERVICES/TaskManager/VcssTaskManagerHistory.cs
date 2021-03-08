using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ctaCOMMON;
using ctaCOMMON.AdminModel;
using ctaCOMMON.DataParser;
using ctaCOMMON.Interface;
using ctaCOMMON.Quotes;
using Newtonsoft.Json;

namespace ctaSERVICES.TaskManager
{
    public class VcssTaskManagerHistory : VcssTaskManager
    {
        protected override VcssTaskCanExecuteResult CanExecute(VcssTaskModel vcssTask)
        {
            return VcssTaskCanExecuteResult.Execute;
        }

        protected override VcssTaskCanScheduleResult CanSchedule(VcssTaskInfoModel vcssTaskInfo)
        {
            return VcssTaskCanScheduleResult.Schedule;
        }

        protected override void ExecuteTask(VcssTaskModel vcssTask)
        {
            string[] jsonData = vcssTask.data.Split('#');
            int stockId = int.Parse(jsonData[1]);
            List<jsonmdlEODHistorical> data = JsonConvert.DeserializeObject<List<jsonmdlEODHistorical>>(jsonData[0]);

            List<IHistoricalQuote> historicalQuotes = data.Select(h => new HistoricalQuoteEOD()
            {
                adj_close = h.AdjustedClose,
                closing = h.Close,
                date_round = h.Date,
                maximun = h.High,
                minimun = h.Low,
                opening = h.Open,
                stock_id = stockId,
                volume = h.Volume
            }).ToList<IHistoricalQuote>();
            QuotesService.SaveHistoricalQuote(historicalQuotes);
        }

        protected override void ScheduleTask(VcssTaskInfoModel vcssTaskInfo, params string[] parameters)
        {
            int symbolId = int.Parse(parameters[0]);
            DateTime from = DateTime.Parse(parameters[1]);
            DateTime to = DateTime.Parse(parameters[2]);
            string symbol = StockService.GetStockSymbol(symbolId).symbol;            

            using (WebClient web = new WebClient())
            {
                string data = web.DownloadString(String.Format(vcssTaskInfo.url, symbol, from.ToString("yyyy-MM-dd"), to.ToString("yyyy-MM-dd"), vcssTaskInfo.auth_token));
                data += "#" + symbolId;
                DateTime marketTime = DateTime.UtcNow.AddHours(vcssTaskInfo.Market.utc_offset);
                VcssTaskManagerService.ScheduleTask(vcssTaskInfo.Id, data, marketTime);
            }
        }
    }
}
