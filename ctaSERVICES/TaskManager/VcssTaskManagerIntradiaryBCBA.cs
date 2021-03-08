using System;
using System.Collections.Generic;
using System.IO;
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
    public class VcssTaskManagerIntradiaryBCBA : VcssTaskManagerIntradiary
    {        
        protected override void ExecuteTask(VcssTaskModel vcssTask)
        {            
            DateTime marketDate = DateTime.UtcNow.AddHours(vcssTask.VcssTaskInfo.Market.utc_offset).Date;
            Dictionary<string, int> symbols = StockService.GetStocks().Where(s => s.market_id == 1 && s.active).ToDictionary(s => s.symbol, s => s.Id);
            List<jsonmdlBOLSARRealTime> data = JsonConvert.DeserializeObject<List<jsonmdlBOLSARRealTime>>(vcssTask.data).Where(x => x.Vencimiento == "48hs" && symbols.ContainsKey(x.Simbolo)).ToList();

            //INTRADIARY DATA
            List<IRealTimeQuote> realTimeQuotes = data.Select(rt => new RealTimeQuoteBOLSAR()
            {
                ask = rt.PrecioVenta,
                ask_size = rt.CantidadNominalVenta,
                bid = rt.PrecioCompra,
                bid_size = rt.CantidadNominalCompra,
                change = rt.Tendencia,
                change_percent = rt.Variacion,
                datetime = this.GetIntradiaryDateTime(marketDate, rt.Hora_Cotizacion),
                last_trade_date = DateTime.Now,
                last_trade_price = rt.Ultimo,
                last_trade_size = 0,
                last_trade_time = "",
                opening = rt.Ultimo,
                prev_closing = rt.CierreAnterior,
                stock_id = symbols[rt.Simbolo]
            }).ToList<IRealTimeQuote>();

            //HISTORICAL DATA
            List<IHistoricalQuote> historicalQuotes = data.Select(h => new HistoricalQuoteBOLSAR()
            {
                adj_close = 0,
                closing = h.Ultimo,
                date_round = marketDate,
                maximun = h.Maximo,
                minimun = h.Minimo,
                opening = h.Apertura,
                stock_id = symbols[h.Simbolo],
                volume = h.VolumenNominal
            }).ToList<IHistoricalQuote>();

            QuotesService.SaveRealTimeQuote(realTimeQuotes);
            QuotesService.SaveHistoricalQuote(historicalQuotes);            
        }        

        protected override void ScheduleTask(VcssTaskInfoModel vcssTaskInfo, params string[] parameters)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(vcssTaskInfo.url);
            request.Headers["Authorization"] = vcssTaskInfo.auth_token;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream stream2 = response.GetResponseStream())
            {
                using (StreamReader reader2 = new StreamReader(stream2))
                {
                    string data = reader2.ReadToEnd();
                    DateTime marketTime = DateTime.UtcNow.AddHours(vcssTaskInfo.Market.utc_offset);
                    VcssTaskManagerService.ScheduleTask(vcssTaskInfo.Id, data, marketTime);
                }
            }
        }

        private DateTime GetIntradiaryDateTime(DateTime date, string hora_Cotizacion)
        {
            string[] timeInfo = hora_Cotizacion.Split(':');
            int hours = -1;
            int minutes = -1;
            int seconds = -1;
            if (timeInfo.Length > 0 && int.TryParse(timeInfo[0], out hours))
            {
                if (timeInfo.Length > 1 && int.TryParse(timeInfo[1], out minutes))
                {
                    if (timeInfo.Length > 2 && int.TryParse(timeInfo[2], out seconds))
                    {
                        return date.Date.AddHours(hours).AddMinutes(minutes).AddSeconds(seconds);
                    }
                }
            }
            return date;
        }
    }
}
