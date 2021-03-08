using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ctaCOMMON.Interface;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using ctaCOMMON.DataParser;
using ctaCOMMON.Quotes;
using ctaCOMMON.AdminModel;

namespace ctaSERVICES.QuotesProvider
{
    public class BOLSARINDEXQuotesProvider : QuotesProvider
    {
        public override async Task<string> GetQuotesFromExternalInJsonFormatAsync()
        {
            this.UpdateAuthToken();
            return await this.GetQuotesFromExternalInJsonAsync();            
        }

        public override Tuple<List<IRealTimeQuote>, List<IHistoricalQuote>> GetQuotesFromJson(string json)
        {
            try
            {
                int utc_offset = ConfigService.GetConfigs().Where(c => c.ConfigName == "ARG_UTCOffset").Select(c => int.Parse(c.ConfigValue)).FirstOrDefault();
                DateTime date = DateTime.UtcNow.AddHours(utc_offset);
                Dictionary<string, int> symbols = StockService.GetStocks().Where(s => s.market_id == 1 && s.active).ToDictionary(s => s.symbol, s => s.Id);
                List<jsonmdlBOLSARIndex> data = JsonConvert.DeserializeObject<List<jsonmdlBOLSARIndex>>(json).Where(x => symbols.ContainsKey(x.Symbol)).ToList();

                //INTRADIARY DATA
                List<IRealTimeQuote> realTimeQuotes = data.Select(rt => new RealTimeQuoteBOLSAR()
                {
                    ask = 0,
                    ask_size = 0,
                    bid = 0,
                    bid_size = 0,
                    change = rt.Tendencia,
                    change_percent = rt.Variacion,
                    datetime = date,
                    last_trade_date = DateTime.Now,
                    last_trade_price = rt.Ultimo,
                    last_trade_size = 0,
                    last_trade_time = "",
                    opening = rt.Ultimo,
                    prev_closing = rt.Apertura,
                    stock_id = symbols[rt.Symbol]
                }).ToList<IRealTimeQuote>();

                //HISTORICAL DATA
                List<IHistoricalQuote> historicalQuotes = data.Select(h => new HistoricalQuoteBOLSAR()
                {
                    adj_close = 0,
                    closing = h.Ultimo,
                    date_round = date,
                    maximun = h.Maximo_Valor,
                    minimun = h.Minimo_Valor,
                    opening = h.Apertura,
                    stock_id = symbols[h.Symbol],
                    volume = 0
                }).ToList<IHistoricalQuote>();

                return new Tuple<List<IRealTimeQuote>, List<IHistoricalQuote>>(realTimeQuotes, historicalQuotes);
            }
            catch(Exception ex)
            {
                //TODO - Log error
                TaskManagerService.LogTaskError("BOLSARINDEXQuotesProvider - GetQuotesFromJson", ex.Message);
                return null;
            }
        }

        public override async void UpdateAuthToken()
        {
            try
            {
                /*BOLSAR_Quotes_LoginURL
                 homologacion => https://hs-wss-bolsar-bcba.sba.com.ar/Seguridad.svc/sg?us={0}&tk={1}
                 Production   => https://wss-bolsar.bcba.sba.com.ar/Seguridad.svc/sg?us={0}&tk={1}
                 */
                string loginURL = ConfigService.GetConfig("BOLSAR_Quotes_LoginURL").ConfigValue;

                /*BOLSAR_Quotes_Username
                 homologacion => grau
                 Production   => grauwss
                 */
                string username = ConfigService.GetConfig("BOLSAR_Quotes_Username").ConfigValue;

                /*BOLSAR_Quotes_Password
                 homologacion => 53bpueDi
                 Production   => i76sljJs
                 */
                string password = ConfigService.GetConfig("BOLSAR_Quotes_Password").ConfigValue;

                string requestURL = String.Format(loginURL, username, ComputeHash(password));

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestURL);
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        var strResponse = reader.ReadToEnd();
                        dynamic result = JsonConvert.DeserializeObject<dynamic>(strResponse);

                        ConfigModel current_auth_token = ConfigService.GetConfig("BOLSAR_Auth_Token");
                        current_auth_token.ConfigValue = result.Resultado;
                        ConfigService.UpdateConfig(current_auth_token);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO - Log error
                TaskManagerService.LogTaskError("ERROR => BOLSARQuotesProvider - UpdateAuthToken", ex.Message);
            }
        }

        public override Task UpdateHistoricalQuote(int stockID, DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        private async Task<string> GetQuotesFromExternalInJsonAsync()
        {
            string BOLSAR_Auth_Token = "";
            try
            {
                /*BOLSAR_Quotes_URL
                homologacion => https://hs-wss-bolsar-bcba.sba.com.ar/Cotizaciones.svc/ri
                Production   => https://wss-bolsar.bcba.sba.com.ar/Cotizaciones.svc/ri
                */
                string quotesURL = ConfigService.GetConfig("BOLSARINDEX_Quotes_URL").ConfigValue;

                BOLSAR_Auth_Token = ConfigService.GetConfig("BOLSAR_Auth_Token").ConfigValue;

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(quotesURL);
                request.Headers["Authorization"] = BOLSAR_Auth_Token;

                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                using (Stream stream2 = response.GetResponseStream())
                {
                    using (StreamReader reader2 = new StreamReader(stream2))
                    {
                        return reader2.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("(401)"))
                {
                    //TODO - Log error
                    TaskManagerService.LogTaskError("ERROR => BOLSARINDEXQuotesProvider - Private Method1: GetQuotesFromExternalInJsonAsync", $"Error: {ex.Message} === Token: {BOLSAR_Auth_Token}");
                    return null;
                }
                else
                {
                    //TODO - Log error
                    TaskManagerService.LogTaskError("ERROR => BOLSARINDEXQuotesProvider - Private Method2: GetQuotesFromExternalInJsonAsync", $"Error: {ex.Message} === Token: {BOLSAR_Auth_Token}");
                    return "";
                }
            }
        }
    }
}
