using ctaCOMMON.Interface;
using ctaDATAMODEL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ctaSERVICES.QuotesProvider
{
    public abstract class QuotesProvider
    {
        public abstract Tuple<List<IRealTimeQuote>, List<IHistoricalQuote>> GetQuotesFromJson(string json);        
        
        public abstract Task<string> GetQuotesFromExternalInJsonFormatAsync();

        public abstract Task UpdateHistoricalQuote(int stockID, DateTime from, DateTime to);

        public abstract void UpdateAuthToken();

        public string GetQuotesFromExternalInJsonFormat()
        {
            return GetQuotesFromExternalInJsonFormatAsync().Result;
        }

        public async Task<Tuple<List<IRealTimeQuote>, List<IHistoricalQuote>>> GetQuotesFromJsonAsync(string json)
        {
            return await Task.FromResult<Tuple<List<IRealTimeQuote>, List<IHistoricalQuote>>>(GetQuotesFromJson(json));
        }

        public void SaveRealTimeQuotes(List<IRealTimeQuote> quotes)
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
                catch (Exception ex)
                {
                    errorProcesingData += $"Error: {ex.Message}{Environment.NewLine}StockID:{quote.stock_id}{Environment.NewLine}opening={quote.opening}|prev_closing={quote.prev_closing}|ask={quote.ask}|ask_size={quote.ask_size}|bid={quote.bid}|bid_size={quote.bid_size}|change={quote.change}|change_percent={quote.change_percent}|last_trade_time={quote.last_trade_time}|last_trade_price={quote.last_trade_price}|last_trade_size={quote.last_trade_size}|last_trade_date={quote.last_trade_date}|datetime={quote.datetime}|";
                }
            }
            if (!string.IsNullOrEmpty(errorProcesingData))
            {
                //EmailSender.SendErrorUpdatingIntradiary(errorProcesingData, "Error Processing Intradiary Data", DateTime.Now);
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
                //EmailSender.SendErrorUpdatingIntradiary(errorProcesingData, "Error Saving Intradiary Data", DateTime.Now);
            }
        }
        public async Task SaveRealTimeQuotesAsync(List<IRealTimeQuote> quotes)
        {
            await Task.Run(() => SaveRealTimeQuotes(quotes));            
        }
        public void SaveHistoricalQuotes(List<IHistoricalQuote> quotes)
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
                catch (Exception ex)
                {
                    errorProcesingData += $"Error: {ex.Message}{Environment.NewLine}StockID:{quote.stock_id}{Environment.NewLine}opening={quote.opening}|closing={quote.closing}|minimun={quote.minimun}|maximun={quote.maximun}|volume={quote.volume}|date_round={quote.date_round}|adj_close={quote.adj_close}|";
                }
            }
            if (!string.IsNullOrEmpty(errorProcesingData))
            {
                //EmailSender.SendErrorUpdatingIntradiary(errorProcesingData, "Error Processing Historical Data", DateTime.Now);
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
                //EmailSender.SendErrorUpdatingIntradiary(errorProcesingData, "Error Saving Historical Data", DateTime.Now);
            }
        }

        public static QuotesProvider GetQuotesProviderInstance(string providerName)
        {
            if (providerName == "BOLSAR") return new BOLSARQuotesProvider();
            if (providerName == "BOLSARINDEX") return new BOLSARINDEXQuotesProvider();
            if (providerName == "EOD") return new EODQuotesProvider();
            return null;
        }

        public async Task SaveHistoricalQuotesAsync(List<IHistoricalQuote> quotes)
        {
            await Task.Run(() => SaveHistoricalQuotes(quotes));
        }

        public string ComputeHash(string plainText)
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
