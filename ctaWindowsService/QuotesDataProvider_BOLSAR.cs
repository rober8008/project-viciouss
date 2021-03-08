using ctaCOMMON.DataParser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ctaWindowsService
{
    internal class QuotesDataProvider_BOLSAR : QuotesDataProvider
    {
        public QuotesDataProvider_BOLSAR(Configs_QuoteDataProvider dataproviderConfigs, Configs_Market marketConfigs) : base(dataproviderConfigs, marketConfigs)
        {
        }

        public override List<RealTimeQuoteDBModel> GetRealTimeQuotes(List<mdlStock> stocks, DateTime date, DBContext dbContext, out List<HistoricalQuoteDBModel> historical)
        {
            List<RealTimeQuoteDBModel> result = new List<RealTimeQuoteDBModel>();
            historical = new List<HistoricalQuoteDBModel>();            

            using (WebClient web = new WebClient())
            {
               
                //homologacion
                //string url = String.Format("https://hs-wss-bolsar-bcba.sba.com.ar/Seguridad.svc/sg?us={0}&tk={1}", "grau", ComputeHash("53bpueDi"));
                //Login prod
                string url = $"{this.QuoteDataProviderConfigs.LoginURL}?us={this.QuoteDataProviderConfigs.Username}&tk={this.ComputeHash(this.QuoteDataProviderConfigs.APIToken)}";

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        var strResponse = reader.ReadToEnd();
                        LOG.Log($"          ===>Login Response: {strResponse}");
                        dynamic loginResult = JsonConvert.DeserializeObject<dynamic>(strResponse);

                        //homologacion
                        //request = (HttpWebRequest)HttpWebRequest.Create("https://hs-wss-bolsar-bcba.sba.com.ar/Cotizaciones.svc/ssEspecies");
                        //prod
                        request = (HttpWebRequest)HttpWebRequest.Create(this.QuoteDataProviderConfigs.RealTimeURL);
                        request.Headers["Authorization"] = loginResult.Resultado;

                        response = (HttpWebResponse)request.GetResponse();
                        using (Stream stream2 = response.GetResponseStream())
                        {
                            using (StreamReader reader2 = new StreamReader(stream2))
                            {
                                strResponse = reader2.ReadToEnd();

                                //parse strResponse
                                List< jsonmdlBOLSARRealTime> data = JsonConvert.DeserializeObject<List<jsonmdlBOLSARRealTime>>(strResponse);

                                List<jsonmdlBOLSARRealTime> data48hrs = data.Where(x => x.Vencimiento == "48hs" && stocks.Any(s => s.StockSymbol == x.Simbolo)).ToList<jsonmdlBOLSARRealTime>();
                                     
                                //INTRADIARY DATA
                                result = data48hrs.Select(rt => new RealTimeQuoteDBModel()
                                {
                                    ask = rt.PrecioVenta,
                                    ask_size = rt.CantidadNominalVenta,
                                    bid = rt.PrecioCompra,
                                    bid_size = rt.CantidadNominalCompra,
                                    change = rt.Tendencia,
                                    change_percent = rt.Variacion,
                                    datetime = date,
                                    last_trade_date = DateTime.Now,
                                    last_trade_price = rt.Ultimo,
                                    last_trade_size = 0,
                                    last_trade_time = "",
                                    opening = rt.Ultimo,
                                    prev_closing = rt.CierreAnterior,
                                    stock_id = stocks.Where(s => s.StockSymbol == rt.Simbolo).First().StockId
                                }).ToList<RealTimeQuoteDBModel>();

                                //HISTORICAL DATA
                                historical = data48hrs.Select(h => new HistoricalQuoteDBModel()
                                {
                                    adj_close = 0,
                                    closing = h.Ultimo,
                                    date_round = date,
                                    maximun = h.Maximo,
                                    minimun = h.Minimo,
                                    opening = h.Apertura,
                                    stock_id = stocks.Where(s => s.StockSymbol == h.Simbolo).First().StockId,
                                    volume = h.VolumenNominal
                                }).ToList<HistoricalQuoteDBModel>();
                            }
                        }
                            
                        //INDICES
                        request = (HttpWebRequest)HttpWebRequest.Create(this.QuoteDataProviderConfigs.IndexURL);
                        request.Headers["Authorization"] = loginResult.Resultado;
                                
                        response = (HttpWebResponse)request.GetResponse();
                        using (Stream stream3 = response.GetResponseStream())
                        {
                            using (StreamReader reader3 = new StreamReader(stream3))
                            {
                                strResponse = reader3.ReadToEnd();
                                //parse strResponse
                                List<jsonmdlBOLSARIndex> indexData = JsonConvert.DeserializeObject<List<jsonmdlBOLSARIndex>>(strResponse).Where(d => stocks.Any(s => s.StockSymbol == d.Symbol)).ToList< jsonmdlBOLSARIndex>();

                                //INTRADIARY DATA
                                result.AddRange(indexData.Select(rt => new RealTimeQuoteDBModel()
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
                                    stock_id = stocks.Where(s => s.StockSymbol == rt.Symbol).First().StockId
                                }).ToList<RealTimeQuoteDBModel>());

                                //HISTORICAL DATA
                                historical.AddRange(indexData.Select(h => new HistoricalQuoteDBModel()
                                {
                                    adj_close = 0,
                                    closing = h.Ultimo,
                                    date_round = date,
                                    maximun = h.Maximo_Valor,
                                    minimun = h.Minimo_Valor,
                                    opening = h.Apertura,
                                    stock_id = stocks.Where(s => s.StockSymbol == h.Symbol).First().StockId,
                                    volume = 0
                                }).ToList<HistoricalQuoteDBModel>());
                            }
                        }                            
                    }
                }                
            }
            return result;
        }

        public override List<HistoricalQuoteDBModel> GetHistoricalQuotes(List<mdlStock> stocks)
        {
            throw new NotImplementedException();
        }

        private string ComputeHash(string plainText)
        {
            Encoding en = Encoding.GetEncoding(1252);
            // Get the md5 provider 
            MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
            // Compute the local hash 
            Byte[] md5HashLocal = md5Provider.ComputeHash(en.GetBytes(plainText));
            StringBuilder sbHexOutput = new StringBuilder("");
            foreach (Byte _eachChar in md5HashLocal)
            {
                sbHexOutput.AppendFormat("{0:X2}", _eachChar);
            }

            return sbHexOutput.ToString();
        }
    }
}
