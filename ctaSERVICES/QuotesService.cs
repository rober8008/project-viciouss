using ctaCOMMON;
using ctaCOMMON.Charts;
using ctaDATAMODEL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ctaCOMMON.AdminModel;
using ctaCOMMON.DataParser;
using System.IO;
using System.Data.Entity;
using ctaCOMMON.Interface;
using System.Data.SqlClient;
using ctaSERVICES.QuotesProvider;

namespace ctaSERVICES
{
    public static class QuotesService
    {
        public static void ClearIntradiaryDataByMarketID(int marketId)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                var range = entities.Stock_Quote_Intradiary.Where(sqi => sqi.Stock.market_id == marketId);

                entities.Stock_Quote_Intradiary.RemoveRange(range);
                entities.SaveChanges();
            }
        }

        public static void ClearIntradiaryDataByMarketID(string market_name)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                var range = entities.Stock_Quote_Intradiary.Where(sqi => sqi.Stock.Market.name == market_name || market_name.ToUpper() == "ALL" );

                entities.Stock_Quote_Intradiary.RemoveRange(range);
                entities.SaveChanges();                
            }
        }        

        public static List<SimplifiedSymbolQuotes> GetQuotesByMarketID(int market_id)
        {
            List<SimplifiedSymbolQuotes> list_result = new List<SimplifiedSymbolQuotes>();

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                List<GetCarouselQuotesByMarketID_Result> carouselInfo = entities.GetCarouselQuotesByMarketID(market_id).ToList();

                foreach (GetCarouselQuotesByMarketID_Result carouselItem in carouselInfo)
                {                    
                    list_result.Add(new SimplifiedSymbolQuotes
                    {
                        Symbol_ID = carouselItem.StockID,
                        Symbol = carouselItem.Symbol,
                        PercentVariation = carouselItem.PercentVariation,
                        Variation = carouselItem.Variation > 0 ? 1 : carouselItem.Variation < 0 ? -1 : 0,
                        LastTradePrize = carouselItem.LastTradePrize,
                        CurrentAskPrice = carouselItem.Ask
                    });                    
                }
            }
            return list_result.Distinct(SimplifiedSymbolQuotes.ComparerBySymbolId).ToList();
        }

        public static SymbolIntradiaryInfo GetSymbolIntradiaryInfo(int stock_id)
        {
            SymbolIntradiaryInfo result = new SymbolIntradiaryInfo();
            result.SymbolId = stock_id;

            using (ctaDBEntities entities = new ctaDBEntities())
            {                
                entities.Database.Connection.Open();
                Stock_Quote_Intradiary info = entities.Stock_Quote_Intradiary.Where(sqi => sqi.stock_id == stock_id).OrderByDescending(sqi => sqi.datetime).FirstOrDefault();
               
                if (info != null)
                {                    
                    result.Ask = info.ask;
                    result.AskSize = info.ask_size;
                    result.Bid = info.bid;
                    result.BidSize = info.bid_size;
                    result.LastTradeDate = info.last_trade_date;
                    result.LastTradePrice = info.last_trade_price;
                    result.LastTradeTime = info.last_trade_time;
                    result.LastTradeSize = (double)info.last_trade_size;                    
                }

                Stock_Quote historic = entities.Stock_Quote.Where(sq => sq.stock_id == stock_id).OrderByDescending(h => h.date_round).FirstOrDefault();                
                if (historic != null)
                {
                    double previousClosing = entities.Stock_Quote.Where(sq => sq.stock_id == stock_id && sq.date_round < historic.date_round).OrderByDescending(h => h.date_round).Select(sq => sq.closing).FirstOrDefault();
                    if (previousClosing > 0)
                    {
                        result.Change = historic.closing - previousClosing;
                        result.ChangePercent = ((historic.closing / previousClosing) - 1) * 100;
                        result.PreviousClosing = previousClosing;
                    }
                    result.Date = historic.date_round;
                    result.Opening = historic.opening;
                    result.Minimun = historic.minimun;
                    result.Maximun = historic.maximun;
                    result.LastTradePrice = (result.LastTradePrice > 0) ? result.LastTradePrice : ((historic.closing > 0) ? historic.closing : previousClosing );
                    result.Volume = (double)historic.volume;
                }                

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
            return result ?? new SymbolIntradiaryInfo();
        }

        public static void SaveRealTimeQuote(List<IRealTimeQuote> quotes)
        {
            string connectionString = "";
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                connectionString = entities.Configs.Where(c => c.ConfigName == "UpdateQuotesConectionString").Select(c => c.ConfigValue).First();                
            }

            SqlConnection connection = new SqlConnection(connectionString);
            List<SqlCommand> commands = new List<SqlCommand>(); 
            string queryString = $@"INSERT INTO [dbo].[Stock_Quote_Intradiary] ([stock_id],[opening],[prev_closing],[ask],[ask_size],[bid],[bid_size],[change],[change_percent],[last_trade_time],[last_trade_price],[last_trade_size],[last_trade_date],[datetime]) VALUES(@stock_id, @opening, @prev_closing, @ask, @ask_size, @bid, @bid_size, @change, @change_percent, @last_trade_time, @last_trade_price, @last_trade_size, @last_trade_date, @datetime);";
            string errorProcesingData = string.Empty;
            string errorSavingData = string.Empty;

            foreach (IRealTimeQuote quote in quotes)
            {
                try
                {
                    if (quote.opening > 0)
                    {
                        SqlCommand command = new SqlCommand(queryString, connection);

                        SqlParameter sqlParameter = new SqlParameter("@stock_id", SqlDbType.Int);
                        sqlParameter.Value = quote.stock_id;
                        command.Parameters.Add(sqlParameter);

                        sqlParameter = new SqlParameter("@opening", SqlDbType.Float);
                        sqlParameter.Value = quote.opening;
                        command.Parameters.Add(sqlParameter);

                        sqlParameter = new SqlParameter("@prev_closing", SqlDbType.Float);
                        sqlParameter.Value = quote.prev_closing;
                        command.Parameters.Add(sqlParameter);

                        sqlParameter = new SqlParameter("@ask", SqlDbType.Float);
                        sqlParameter.Value = quote.ask;
                        command.Parameters.Add(sqlParameter);

                        sqlParameter = new SqlParameter("@ask_size", SqlDbType.Float);
                        sqlParameter.Value = quote.ask_size;
                        command.Parameters.Add(sqlParameter);

                        sqlParameter = new SqlParameter("@bid", SqlDbType.Float);
                        sqlParameter.Value = quote.bid;
                        command.Parameters.Add(sqlParameter);

                        sqlParameter = new SqlParameter("@bid_size", SqlDbType.Float);
                        sqlParameter.Value = quote.bid_size;
                        command.Parameters.Add(sqlParameter);

                        sqlParameter = new SqlParameter("@change", SqlDbType.Float);
                        sqlParameter.Value = quote.change;
                        command.Parameters.Add(sqlParameter);

                        sqlParameter = new SqlParameter("@change_percent", SqlDbType.Float);
                        sqlParameter.Value = quote.change_percent;
                        command.Parameters.Add(sqlParameter);

                        sqlParameter = new SqlParameter("@last_trade_time", SqlDbType.NVarChar, 20);
                        sqlParameter.Value = quote.last_trade_time;
                        command.Parameters.Add(sqlParameter);

                        sqlParameter = new SqlParameter("@last_trade_price", SqlDbType.Float);
                        sqlParameter.Value = quote.last_trade_price;
                        command.Parameters.Add(sqlParameter);

                        sqlParameter = new SqlParameter("@last_trade_size", SqlDbType.Decimal);
                        sqlParameter.Value = quote.last_trade_size;
                        command.Parameters.Add(sqlParameter);

                        sqlParameter = new SqlParameter("@last_trade_date", SqlDbType.DateTime);
                        sqlParameter.Value = quote.last_trade_date;
                        command.Parameters.Add(sqlParameter);

                        sqlParameter = new SqlParameter("@datetime", SqlDbType.DateTime);
                        sqlParameter.Value = quote.datetime;
                        command.Parameters.Add(sqlParameter);

                        commands.Add(command);
                    }
                }
                catch(Exception ex)
                {
                    errorProcesingData += $"Error: {ex.Message}{Environment.NewLine}StockID:{quote.stock_id}{Environment.NewLine}opening={quote.opening}|prev_closing={quote.prev_closing}|ask={quote.ask}|ask_size={quote.ask_size}|bid={quote.bid}|bid_size={quote.bid_size}|change={quote.change}|change_percent={quote.change_percent}|last_trade_time={quote.last_trade_time}|last_trade_price={quote.last_trade_price}|last_trade_size={quote.last_trade_size}|last_trade_date={quote.last_trade_date}|datetime={quote.datetime}|";
                }                
            }
            if (!string.IsNullOrEmpty(errorProcesingData))
            {
                EmailSender.SendErrorUpdatingIntradiary(errorProcesingData, "Error Processing Intradiary Data", DateTime.Now);
            }

            using (connection)
            {
                connection.Open();
                foreach (SqlCommand command in commands)
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        errorSavingData += $"Error:{ex.Message}{Environment.NewLine}";
                        foreach (SqlParameter param in command.Parameters)
                        {
                            errorSavingData += $"{param.ParameterName}={param.Value}|";
                        }
                    }
                }
                connection.Close();
            }
            if (!string.IsNullOrEmpty(errorSavingData))
            {
                EmailSender.SendErrorUpdatingIntradiary(errorProcesingData, "Error Saving Intradiary Data", DateTime.Now);
            }
        }

        public static void SaveHistoricalQuote(List<IHistoricalQuote> quotes)
        {
            string connectionString = "";
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                connectionString = entities.Configs.Where(c => c.ConfigName == "UpdateQuotesConectionString").Select(c => c.ConfigValue).First();                
            }

            SqlConnection connection = new SqlConnection(connectionString);
            List<SqlCommand> commands = new List<SqlCommand>();
            string queryString = $@"IF NOT EXISTS(SELECT 1 FROM Stock_Quote WHERE stock_id = @stock_id AND date_round = CONVERT(date,@date_round)) BEGIN INSERT INTO [dbo].[Stock_Quote] ([stock_id],[opening],[closing],[minimun],[maximun],[volume],[date_round],[adj_close])VALUES(@stock_id, @opening, @closing, @minimun, @maximun, @volume, @date_round, @adj_close); END ELSE BEGIN UPDATE [dbo].[Stock_Quote] SET [opening] = @opening, [closing] = @closing, [minimun] = @minimun, [maximun] = @maximun, [volume] = @volume, [date_round] = @date_round, [adj_close] = @adj_close WHERE [stock_id] = @stock_id AND date_round = CONVERT(date, @date_round); END";
            string errorProcesingData = string.Empty;
            string errorSavingData = string.Empty;

            foreach (IHistoricalQuote quote in quotes)
            {
                try
                {
                    if (quote.opening > 0 && quote.closing > 0 && quote.maximun > 0 && quote.minimun > 0)
                    {
                        SqlCommand command = new SqlCommand(queryString, connection);

                        SqlParameter sqlParameter = new SqlParameter("@stock_id", SqlDbType.Int);
                        sqlParameter.Value = quote.stock_id;
                        command.Parameters.Add(sqlParameter);

                        sqlParameter = new SqlParameter("@opening", SqlDbType.Float);
                        sqlParameter.Value = quote.opening;
                        command.Parameters.Add(sqlParameter);

                        sqlParameter = new SqlParameter("@closing", SqlDbType.Float);
                        sqlParameter.Value = quote.closing;
                        command.Parameters.Add(sqlParameter);

                        sqlParameter = new SqlParameter("@minimun", SqlDbType.Float);
                        sqlParameter.Value = quote.minimun;
                        command.Parameters.Add(sqlParameter);

                        sqlParameter = new SqlParameter("@maximun", SqlDbType.Float);
                        sqlParameter.Value = quote.maximun;
                        command.Parameters.Add(sqlParameter);

                        sqlParameter = new SqlParameter("@volume", SqlDbType.Decimal);
                        sqlParameter.Value = quote.volume;
                        command.Parameters.Add(sqlParameter);

                        sqlParameter = new SqlParameter("@date_round", SqlDbType.DateTime);
                        sqlParameter.Value = quote.date_round;
                        command.Parameters.Add(sqlParameter);

                        sqlParameter = new SqlParameter("@adj_close", SqlDbType.Float);
                        sqlParameter.Value = quote.adj_close;
                        command.Parameters.Add(sqlParameter);

                        commands.Add(command);
                    }
                }
                catch (Exception ex)
                {
                    errorProcesingData += $"Error: {ex.Message}{Environment.NewLine}StockID:{quote.stock_id}{Environment.NewLine}opening={quote.opening}|closing={quote.closing}|minimun={quote.minimun}|maximun={quote.maximun}|volume={quote.volume}|date_round={quote.date_round}|adj_close={quote.adj_close}|";
                }
            }
            if (!string.IsNullOrEmpty(errorProcesingData))
            {
                EmailSender.SendErrorUpdatingIntradiary(errorProcesingData, "Error Processing Historical Data", DateTime.Now);
            }

            using (connection)
            {
                connection.Open();
                foreach (SqlCommand command in commands)
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        errorSavingData += $"Error:{ex.Message}{Environment.NewLine}";
                        foreach (SqlParameter param in command.Parameters)
                        {
                            errorSavingData += $"{param.ParameterName}={param.Value}|";
                        }
                    }
                }
                connection.Close();
            }
            if (!string.IsNullOrEmpty(errorSavingData))
            {
                EmailSender.SendErrorUpdatingIntradiary(errorProcesingData, "Error Saving Historical Data", DateTime.Now);
            }
        }

        public static void UpdateQuotesSyncNextTime(string market_name, DateTime currentDate, bool updateOnlyNextSynctime = false)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                string syncStep = ConfigService.GetConfigs().Where(c => c.ConfigName == market_name + "_SyncStep").Select(c => c.ConfigValue).FirstOrDefault();
                Config NextSyncTime = entities.Configs.Where(c => c.ConfigName == market_name + "_NextSync").FirstOrDefault();
                Config lastSyncTime = entities.Configs.Where(c => c.ConfigName == market_name + "_LastSync").FirstOrDefault();

                if (!updateOnlyNextSynctime)
                {
                    lastSyncTime.ConfigValue = currentDate.ToString("yyyy-MM-dd hh:mm:ss tt"); ;
                }
                NextSyncTime.ConfigValue = currentDate.AddMinutes(int.Parse(syncStep)).ToString("yyyy-MM-dd hh:mm:ss tt");

                entities.SaveChanges();

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
        }

        public static List<SimplifiedSymbolQuotes> GetQuotesValues(int stockType_id, int? marketIndex_id)
        {
            List<SimplifiedSymbolQuotes> list_result = new List<SimplifiedSymbolQuotes>();

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();

                var stocksSet = entities.Stocks.Where(x => x.Stock_Type.Id == stockType_id && x.active.Value);

                if(marketIndex_id.HasValue)
                    stocksSet = stocksSet.Where(x => x.MarketIndex_Stock.Any(y => y.marketindex_id == marketIndex_id.Value));

                foreach (var stock in stocksSet)
                {
                    var intradiaryInfo = QuotesService.GetSymbolIntradiaryInfo(stock.Id);

                    var lastQuote = stock.Stock_Quote.OrderByDescending(itm => itm.date_round).FirstOrDefault();

                    var simplifiedQuote = new SimplifiedSymbolQuotes
                        {
                            Symbol_ID = stock.Id,
                            Symbol = stock.symbol,
                            PercentVariation = intradiaryInfo.ChangePercent,
                            Variation = intradiaryInfo.Change > 0 ? 1 : intradiaryInfo.Change < 0 ? -1 : 0,
                            VariationValue = Math.Round(intradiaryInfo.Change, 2),
                            CurrentAskPrice = intradiaryInfo.LastTradePrice,
                            Opening = intradiaryInfo.Opening
                        };

                    if (lastQuote != null)
                    {
                        simplifiedQuote.Minimum = lastQuote.minimun;
                        simplifiedQuote.Maximum = lastQuote.maximun;
                    }

                    list_result.Add(simplifiedQuote);
                }

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }

            return list_result;
        }

        public static Symbol GetSymbolInfo(int symbol_ID)
        {
            Symbol result = new Symbol();

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                var stock = entities.Stocks.Where(x => x.Id == symbol_ID).FirstOrDefault();

                result.Symbol_ID = symbol_ID;
                result.Symbol_Name = stock.symbol;
                result.Symbol_Company_Name = stock.name;
                result.Description = stock.description;
                result.Symbol_Market_ID = stock.market_id;
                result.Symbol_Market = stock.Market.name;

                result.Intradiary_Info = QuotesService.GetSymbolIntradiaryInfo(symbol_ID);

                var lastQuote = stock.Stock_Quote.OrderByDescending(itm => itm.date_round).FirstOrDefault();

                if (lastQuote != null)
                {
                    result.Intradiary_Info.Minimun = lastQuote.minimun;
                    result.Intradiary_Info.Maximun = lastQuote.maximun;
                    result.Intradiary_Info.Volume = (double)lastQuote.volume;
                }

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }

            return result;
        }

        public static List<MarketSyncInfo> GetMarketsSyncTimeInfo()
        {
            List<MarketSyncInfo> result = new List<MarketSyncInfo>();

            List<string> markets = MarketService.GetMarkets().Select(m => m.name).ToList();
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Config cfg;
                string lastSync = "";
                string nextSync = "";
                foreach (string marketName in markets)
                {
                    cfg = entities.Configs.Where(c => c.ConfigName == marketName + "_LastSync").FirstOrDefault();
                    if (cfg != null)
                    {
                        lastSync = cfg.ConfigValue;
                    }
                    cfg = entities.Configs.Where(c => c.ConfigName == marketName + "_NextSync").FirstOrDefault();
                    if (cfg != null)
                    {
                        nextSync = cfg.ConfigValue;
                    }
                    result.Add(new MarketSyncInfo() { Market = marketName, LastSync = lastSync, NextSync = nextSync });
                }

                //adding the report sync
                var reportConfig_LastSync = entities.Configs.Where(c => c.ConfigName == "ReportDATA_LastSync").FirstOrDefault();
                var reportConfig_NextSync = entities.Configs.Where(c => c.ConfigName == "ReportDATA_NextSync").FirstOrDefault();

                result.Add(new MarketSyncInfo()
                {
                    Market = "ReportDATA",
                    LastSync = reportConfig_LastSync.ConfigValue,
                    NextSync = reportConfig_NextSync.ConfigValue
                });
            }

            return result;
        }

        public static string ReadMarketIntradiaryData(string market_name, int market_id, int utc_time_offset, out bool error, params string[] otherParams)
        {
            error = false;
            DateTime date = DateTime.UtcNow.AddHours(utc_time_offset);

            string status = "O";
            bool marketOpen = MarketService.IsMarketOpen(market_id, date, out status);            

            if (marketOpen)
            {
                try
                {
                    //switch (market_name)
                    //{
                    //    case "NYSE":
                    //        QuotesFromEOD quotesFromEODny = new QuotesFromEOD();
                    //        quotesFromEODny.SaveQuotesToDB(market_name, market_id, utc_time_offset);                            
                    //        break;
                    //    case "NASDAQ":
                    //        QuotesFromEOD quotesFromEODna = new QuotesFromEOD();
                    //        quotesFromEODna.SaveQuotesToDB(market_name, market_id, utc_time_offset);                           
                    //        break;
                    //    case "BCBA":
                    //        QuotesFromBOLSAR quotesFromBolsar = new QuotesFromBOLSAR();
                    //        quotesFromBolsar.SaveQuotesToDB(market_name, market_id, utc_time_offset);
                    //        break;
                    //    default:
                    //        return "Quotes for other markets than NYSE, NASDAQ or BCBA are not implemented";
                    //}
                    return "Quotes Updated";
                }
                catch (Exception ex)
                {
                    error = true;
                    EmailSender.SendErrorUpdatingIntradiary("Message: " + ex.Message + Environment.NewLine + "Source: " + ex.Source + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + ((ex.InnerException != null) ? "Inner Message: " + ex.InnerException.Message + Environment.NewLine + "Inner Source: " + ex.InnerException.Source + Environment.NewLine + "Inner StackTrace: " + ex.InnerException.StackTrace : ""), market_name, date);
                    return "Error Updating Intradiry: " + ex.Message;
                }
            }
            else
            {
                error = true;
                if (date.Hour == 5 && status == "C")
                {
                    try
                    {
                        QuotesService.ClearIntradiaryDataByMarketID(market_name);
                        //EmailSender.DeletingIntradiaryData("Intradiry Deleted", market_name, date);
                        return "Market Close --> Intradiry Deleted";
                    }
                    catch (Exception ex)
                    {
                        EmailSender.SendErrorClearIntradiaryData("Message: " + ex.Message + Environment.NewLine + "Source: " + ex.Source + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + ((ex.InnerException != null) ? "Inner Message: " + ex.InnerException.Message + Environment.NewLine + "Inner Source: " + ex.InnerException.Source + Environment.NewLine + "Inner StackTrace: " + ex.InnerException.StackTrace : ""), market_name, date);
                        return "Error deleting Intradiry: " + ex.Message;
                    }
                }
                else
                {
                    //EmailSender.SendMarketCloseEmail(market_name, date, status);
                    return "Market Close";
                }
            }            
        }

        public static void ReadHistoricalData(DateTime startdate, DateTime enddate, string stock_symbol, int stock_id, params string[] otherParams)
        {
            string market = StockService.GetStocks().Where(s => s.Id == stock_id).Select(s => s.market_name).FirstOrDefault();
            switch (market)
            {
                //case "NYSE":
                //    QuotesFromEOD quotesFromEODny = new QuotesFromEOD();
                //    quotesFromEODny.UpdateHistoricalData(startdate, enddate, stock_symbol, stock_id);
                //    break;
                //case "NASDAQ":
                //    QuotesFromEOD quotesFromEODna = new QuotesFromEOD();
                //    quotesFromEODna.UpdateHistoricalData(startdate, enddate, stock_symbol, stock_id);
                //    break;
                default:
                    throw new NotImplementedException("Quotes for other markets than NYSE or NASDAQ are not implemented");
            }
        }

        public static void ReadHistoricalDataFromCSV(Stream inputStream, string fileName)
        {
            CSVQuotesProvider csvfile = new CSVQuotesProvider();
            csvfile.ReadData(inputStream, fileName);
        }

        public static List<Candel> GetSymbolQuotes(int symbol_id, ChartRange chartRange, CandelRange candelRange, string user_type)
        {
            DateTime chartMinDate = DateTime.Now;
            DateTime today = DateTime.Now.Date;
            switch (chartRange)
            {
                case ChartRange.Month:
                    chartMinDate = chartMinDate.AddMonths(-1);                                            
                    break;
                case ChartRange.ThreeMonths:
                    chartMinDate = chartMinDate.AddMonths(-3);                    
                    break;
                case ChartRange.SixMonths:
                    chartMinDate = chartMinDate.AddMonths(-6);                    
                    break;
                case ChartRange.Year:
                    chartMinDate = chartMinDate.AddYears(-1);                    
                    break;
                case ChartRange.ThreeYears:
                    chartMinDate = chartMinDate.AddYears(-3);                    
                    break;
                default:
                    chartMinDate = DateTime.MinValue.AddDays(200);
                    break;
            }

            List<Candel> candels = new List<Candel>();
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();
                bool createCandel = false;
                Candel currentCandel = null;
                DateTime startDate = chartMinDate.AddDays(-200);
                List<Stock_Quote> quotes = entities.Stock_Quote.Where(s => s.stock_id == symbol_id && (s.date_round > startDate && (user_type != "FREE" || s.date_round < today))).OrderBy(sq => sq.date_round).ToList();
                Stock_Quote quote = null;
                for(int i = 0; i < quotes.Count; i++)
                {
                    quote = quotes[i];
                    currentCandel = (currentCandel != null) ? currentCandel : new Candel() { Date = quote.date_round, Open = quote.opening, Close = quote.closing, Minimun = quote.minimun, Maximun = quote.maximun, Volume = (double)quote.volume };                    

                    if (candelRange == CandelRange.Daily)
                    {
                        createCandel = true;
                    }
                    else if(candelRange == CandelRange.Weekly)
                    {
                        currentCandel.Minimun = (currentCandel.Minimun > quote.minimun) ? quote.minimun : currentCandel.Minimun;
                        currentCandel.Maximun = (currentCandel.Maximun < quote.maximun) ? quote.maximun : currentCandel.Maximun;                       
                        if (quote.date_round.DayOfWeek == DayOfWeek.Friday)
                        {
                            currentCandel.Close = quote.closing;
                            currentCandel.Date = quote.date_round;
                            createCandel = true;
                        }
                    }
                    else
                    {
                        currentCandel.Minimun = (currentCandel.Minimun > quote.minimun) ? quote.minimun : currentCandel.Minimun;
                        currentCandel.Maximun = (currentCandel.Maximun < quote.maximun) ? quote.maximun : currentCandel.Maximun;
                        if(currentCandel.Date.Month != quote.date_round.Month)
                        {                            
                            createCandel = true;
                            i--;
                        }
                        else
                        {
                            currentCandel.Date = quote.date_round;
                        }
                    }

                    if (createCandel || i+1 == quotes.Count)
                    {
                        currentCandel.Close = quote.closing;
                        currentCandel.Visible = (currentCandel.Date >= chartMinDate.Date);
                        candels.Add(currentCandel);
                        currentCandel = null;
                        createCandel = false;
                    }
                }

                entities.Database.Connection.Close();
            }
            return candels;
        }       
    }

    public class MarketSyncInfo
    {
        public string Market;
        public string LastSync;
        public string NextSync;
    }
}
