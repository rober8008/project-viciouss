using ctaCOMMON;
using ctaDATAMODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ctaSERVICES
{
    public class YahooStockQuoteAPI
    {
        public YahooStockQuoteAPI()
        {
            //HISTORICAL - http://ichart.finance.yahoo.com a = From Month b = From Day c = From Year d = To Month e = To Day f = To Year Eg: 6 months data for Taylor Wimpey: http://ichart.finance.yahoo.com/table.csv?s=TW.L&a=03&b=07&c=2013&d=09&e=04&f=2013
            //INTRADIARY - http://finance.yahoo.com/d/quotes.csv?s=AAPL+GOOG+MSFT&f=snbaopl1
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="market_name">BCAB;NYSE;NASDAQ</param>
        /// <param name="market_id">[BCAB,1];[NYSE,2];[NASDAQ,3]</param>
        /// <param name="utc_time_offset">[BCBA,-3];[NYSE,];[NASDAQ,]</param>
        /// <param name="market_yahoo_api_id">[BCBA,".BA"];[NYSE,""];[NASDAQ,""]</param>
        public void ReadMarketIntradiaryData(string market_name, int market_id, DateTime date, string market_yahoo_api_id)
        {
            string csvData = "";
            List<IntradiaryData> prices = null;
            
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();

                Dictionary<string, int> stocks_id = new Dictionary<string, int>();

                string symbols = string.Empty;
                try
                {
                    foreach (Stock smb in entities.Stocks.Where(s => s.market_id == market_id && s.active.HasValue && s.active.Value))
                    {
                        symbols += ((symbols == string.Empty) ? "" : "+") + smb.symbol.Trim() + ((smb.Id == 671) ? "" : market_yahoo_api_id);
                        stocks_id[smb.symbol.Trim() + ((smb.Id == 671) ? "" : market_yahoo_api_id)] = smb.Id;
                    }
                }
                catch (Exception ex)
                {
                    EmailSender.SendErrorGettingSymbols("Message: " + ex.Message + Environment.NewLine + "Source: " + ex.Source + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + ((ex.InnerException != null) ? "Inner Message: " + ex.InnerException.Message + Environment.NewLine + "Inner Source: " + ex.InnerException.Source + Environment.NewLine + "Inner StackTrace: " + ex.InnerException.StackTrace : ""), market_name, date);
                }

                using (WebClient web = new WebClient())
                {
                    try
                    {
                        csvData = web.DownloadString("http://finance.yahoo.com/d/quotes.csv?s=" + symbols + "&f=sopaa5bb6c1p2d1t1l1k3vm");

                        prices = this.Parse(csvData);

                        foreach (IntradiaryData intradiary in prices)
                        {
                            try
                            {
                                Stock_Quote_Intradiary sqi = new Stock_Quote_Intradiary();

                                sqi.ask = intradiary.Ask;
                                sqi.ask_size = intradiary.AskSize;
                                sqi.bid = intradiary.Bid;
                                sqi.bid_size = intradiary.BidSize;
                                sqi.change = intradiary.Change;
                                sqi.change_percent = intradiary.ChangePercent;
                                sqi.last_trade_size = intradiary.LastTradeSize;
                                sqi.last_trade_date = (intradiary.LastTradeDate) == DateTime.MinValue ? date.Date : intradiary.LastTradeDate;
                                sqi.last_trade_price = intradiary.LastTradePrice;
                                sqi.last_trade_time = intradiary.LastTradeTime;
                                sqi.opening = intradiary.Open;
                                sqi.prev_closing = intradiary.PreviousClose;
                                sqi.stock_id = stocks_id[intradiary.Symbol.Trim()];
                                sqi.datetime = date;

                                entities.Stock_Quote_Intradiary.Add(sqi);
                            }
                            catch (Exception ex)
                            {
                                EmailSender.SendErrorGettingSymbolsStocks("StockName: " + intradiary.Symbol + " Message: " + ex.Message + Environment.NewLine + "Source: " + ex.Source + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + ((ex.InnerException != null) ? "Inner Message: " + ex.InnerException.Message + Environment.NewLine + "Inner Source: " + ex.InnerException.Source + Environment.NewLine + "Inner StackTrace: " + ex.InnerException.StackTrace : ""), market_name, date);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EmailSender.SendErrorGettingSymbolsStocks("Message: " + ex.Message + Environment.NewLine + "Source: " + ex.Source + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + ((ex.InnerException != null) ? "Inner Message: " + ex.InnerException.Message + Environment.NewLine + "Inner Source: " + ex.InnerException.Source + Environment.NewLine + "Inner StackTrace: " + ex.InnerException.StackTrace : ""), market_name, date);
                    }
                }
                try
                {
                    entities.SaveChanges();
                    //EmailSender.SendIntradiarySaved(market_name, date);
                }
                catch (Exception ex)
                {
                    EmailSender.SendErrorSavingChangesInDataBase("Message: " + ex.Message + Environment.NewLine + "Source: " + ex.Source + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + ((ex.InnerException != null) ? "Inner Message: " + ex.InnerException.Message + Environment.NewLine + "Inner Source: " + ex.InnerException.Source + Environment.NewLine + "Inner StackTrace: " + ex.InnerException.StackTrace : ""), market_name, date);
                }

                try
                {
                    //Update Historical Data
                    foreach (IntradiaryData intradiary in prices)
                    {
                        try
                        {
                            int currentStockId = stocks_id[intradiary.Symbol.Trim()];
                            Stock_Quote quote = entities.Stock_Quote.Where(sq => sq.stock_id == currentStockId && sq.date_round.Year == date.Year && sq.date_round.Month == date.Month && sq.date_round.Day == date.Day).FirstOrDefault();
                            if (quote != null)
                            {
                                if (intradiary.LastTradePrice > 0)
                                {
                                    quote.minimun = Math.Min(intradiary.LastTradePrice, quote.minimun);
                                    quote.maximun = Math.Max(intradiary.LastTradePrice, quote.maximun);
                                    quote.closing = intradiary.LastTradePrice;
                                }
                                if (intradiary.Volume > 0)
                                {
                                    quote.volume = intradiary.Volume;
                                }
                            }
                            else
                            {
                                if (intradiary.LastTradePrice > 0 && intradiary.Open > 0)
                                {
                                    quote = new Stock_Quote();
                                    quote.date_round = date;
                                    quote.opening = intradiary.Open;
                                    quote.minimun = Math.Min(intradiary.LastTradePrice, intradiary.Open);
                                    quote.maximun = Math.Max(intradiary.LastTradePrice, intradiary.Open);
                                    quote.closing = intradiary.Open;
                                    quote.stock_id = stocks_id[intradiary.Symbol.Trim()];
                                    quote.volume = intradiary.Volume;
                                    entities.Stock_Quote.Add(quote);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            EmailSender.SendErrorUpdatingHistorical("StockName: " + intradiary.Symbol + " Message: " + ex.Message + Environment.NewLine + "Source: " + ex.Source + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + ((ex.InnerException != null) ? "Inner Message: " + ex.InnerException.Message + Environment.NewLine + "Inner Source: " + ex.InnerException.Source + Environment.NewLine + "Inner StackTrace: " + ex.InnerException.StackTrace : ""), market_name, date);
                        }
                    }

                    entities.SaveChanges();
                    //EmailSender.SendHistoricalUpdated(market_name, date);
                }
                catch (Exception ex)
                {
                    EmailSender.SendErrorUpdatingHistorical("Message: " + ex.Message + Environment.NewLine + "Source: " + ex.Source + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + ((ex.InnerException != null) ? "Inner Message: " + ex.InnerException.Message + Environment.NewLine + "Inner Source: " + ex.InnerException.Source + Environment.NewLine + "Inner StackTrace: " + ex.InnerException.StackTrace : ""), market_name, date);
                }

                if (entities.Database.Connection.State != System.Data.ConnectionState.Closed)
                    entities.Database.Connection.Close();
            }            
        }

        /// <summary>
        /// Update Historical Data
        /// </summary>
        /// <param name="startdate">Date with format DD/MM/YYYY</param>
        /// <param name="enddate">Date with format DD/MM/YYYY</param>
        public void ReadHistoricalData(DateTime startdate, DateTime enddate, string stock_symbol, int stock_id, string market_identifier_in_yahoo)
        {
            string csvHistoricalData;
            if (stock_id == 671)
                market_identifier_in_yahoo = "";

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();

                string start_month = (startdate.Month - 1) < 10 ? "0" + (startdate.Month - 1).ToString() : (startdate.Month - 1).ToString();
                string start_day = startdate.Day < 10 ? "0" + startdate.Day.ToString() : startdate.Day.ToString();
                string start_year = startdate.Year.ToString();

                string end_month = (enddate.Month - 1) < 10 ? "0" + (enddate.Month - 1).ToString() : (enddate.Month - 1).ToString();
                string end_day = enddate.Day < 10 ? "0" + enddate.Day.ToString() : enddate.Day.ToString();
                string end_year = enddate.Year.ToString();

                using (WebClient web = new WebClient())
                {
                    try
                    {
                        csvHistoricalData = web.DownloadString("http://ichart.finance.yahoo.com/table.csv?s=" + stock_symbol.Trim() + market_identifier_in_yahoo + "&a=" + start_month + "&b=" + start_day + "&c=" + start_year + "&d=" + end_month + "&e=" + end_day + "&f=" + end_year);

                        string[] rows = csvHistoricalData.Replace("\r", "").Split('\n');

                        string row;
                        for (int i = 1; i < rows.Length; i++)
                        {
                            row = rows[i];
                            if (string.IsNullOrEmpty(row)) continue;

                            try
                            {

                                string[] cols = row.Split(',');
                                DateTime date = DateTime.Parse(cols[0]);
                                double open = double.Parse(cols[1]);
                                double high = double.Parse(cols[2]);
                                double low = double.Parse(cols[3]);
                                double close = double.Parse(cols[4]);
                                decimal volu = decimal.Parse(cols[5]);
                                double adjclo = double.Parse(cols[6]);

                                bool delete = false;
                                var sqs = entities.Stock_Quote.Where(s => s.stock_id == stock_id && s.date_round.Year == date.Year && s.date_round.Month == date.Month && s.date_round.Day == date.Day);
                                foreach (Stock_Quote sq1 in sqs)
                                {
                                    entities.Stock_Quote.Remove(sq1);
                                    delete = true;
                                }
                                if (delete)
                                {
                                    entities.SaveChanges();
                                }

                                Stock_Quote sq = new Stock_Quote();
                                sq.stock_id = stock_id;
                                sq.date_round = date;
                                sq.opening = open;
                                sq.adj_close = adjclo;
                                sq.closing = close;
                                sq.maximun = high;
                                sq.minimun = low;
                                sq.volume = volu;

                                entities.Stock_Quote.Add(sq);
                                entities.SaveChanges();
                            }
                            catch (Exception ex) { }
                        }
                    }
                    catch (Exception ex) { }
                }

                if (entities.Database.Connection.State != System.Data.ConnectionState.Closed)
                    entities.Database.Connection.Close();
            }
        }

        private List<IntradiaryData> Parse(string csvData)
        {
            List<IntradiaryData> prices = new List<IntradiaryData>();

            string[] rows = csvData.Replace("\r", "").Split('\n');

            foreach (string row in rows)
            {
                if (string.IsNullOrEmpty(row)) continue;

                string[] cols = row.Split(',');
                prices.Add(new IntradiaryData(cols[0], cols[1], cols[2], cols[3], cols[4], cols[5], cols[6], cols[7], cols[8], cols[9], cols[10], cols[11], cols[12], cols[13], cols[14]));
            }

            return prices;
        }
    }
}
