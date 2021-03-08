using ctaCOMMON.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaWindowsService
{
    internal class DBContext
    {
        Configs configs;
        public DBContext(Configs configs)
        {
            this.configs = configs;
        }

        public Configs_Market GetMarketConfig(string name)
        {
            Configs_Market market = new Configs_Market();
            market.Name = name;

            using (SqlConnection connection = new SqlConnection(this.configs.ConnectionString))
            {
                connection.Open();
                string queryString = $"SELECT ConfigName, ConfigValue FROM Config WHERE ConfigName like '{name}_%'";
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    
                    if (reader["ConfigName"].ToString().EndsWith("_LastSync"))
                    {
                        market.LastSync = DateTime.Parse(reader["ConfigValue"].ToString());
                    }
                    else if (reader["ConfigName"].ToString().EndsWith("_NextSync"))
                    {
                        market.NextSync = DateTime.Parse(reader["ConfigValue"].ToString());
                    }
                    else if (reader["ConfigName"].ToString().EndsWith("_UTCOffset"))
                    {
                        market.UTCOffset = int.Parse(reader["ConfigValue"].ToString());
                    }
                    else if (reader["ConfigName"].ToString().EndsWith("_QuotesUpdateActive"))
                    {
                        market.QuotesUpdatesActive = bool.Parse(reader["ConfigValue"].ToString());
                    }
                    else if (reader["ConfigName"].ToString().EndsWith("_WorkHours"))
                    {
                        market.WorkHours = reader["ConfigValue"].ToString();
                    }
                    else if (reader["ConfigName"].ToString().EndsWith("_QuoteProviderName"))
                    {
                        market.QuoteProviderName = reader["ConfigValue"].ToString();
                    }
                }
                reader.Close();
                connection.Close();
            }
            return market;
        }

        public List<mdlStock> GetMarketActiveStocks(string marketName)
        {
            List<mdlStock> result = new List<mdlStock>();
            using (SqlConnection connection = new SqlConnection(this.configs.ConnectionString))
            {
                connection.Open();
                string queryString = $"SELECT s.Id, symbol, m.Id from Stock s INNER JOIN Market m on s.market_id = m.Id WHERE m.name = '{marketName}' AND s.active = 1";
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new mdlStock() { StockId = int.Parse(reader[0].ToString()), StockSymbol = reader[1].ToString(), MarketId = int.Parse(reader[2].ToString()) });                    
                }
                reader.Close();
                connection.Close();
            }
            return result;
        }

        public bool IsHoliday(DateTime date, string marketName)
        {
            using (SqlConnection connection = new SqlConnection(this.configs.ConnectionString))
            {
                try
                {
                    connection.Open();

                    string queryString = $"SELECT 1 FROM Holidays h INNER JOIN Market m ON h.market_id = m.Id WHERE h.date = '{date.ToString("yyyy-MM-dd")}' AND m.name = '{marketName}'";
                    SqlCommand command = new SqlCommand(queryString, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    return reader.HasRows;
                }
                catch (Exception ex)
                {
                    LOG.Log($"Error (IsHoliday): {ex.Message}{Environment.NewLine} date={date}|marketName={marketName}|");
                }
            }
            return false;            
        }

        public Configs_QuoteDataProvider GetQuoteDataProviderConfig(string name)
        {
            Configs_QuoteDataProvider dataprovider = new Configs_QuoteDataProvider();
            dataprovider.Name = name;

            using (SqlConnection connection = new SqlConnection(this.configs.ConnectionString))
            {
                connection.Open();
                string queryString = $"SELECT ConfigName, ConfigValue FROM Config WHERE ConfigName like '{name}_%'";
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["ConfigName"].ToString().EndsWith("_APIToken"))
                    {
                        dataprovider.APIToken = reader["ConfigValue"].ToString();
                    }
                    else if (reader["ConfigName"].ToString().EndsWith("_RealTimeURL"))
                    {
                        dataprovider.RealTimeURL = reader["ConfigValue"].ToString();
                    }
                    else if (reader["ConfigName"].ToString().EndsWith("_HistoricalURL"))
                    {
                        dataprovider.HistoricalURL = reader["ConfigValue"].ToString();
                    }
                    else if (reader["ConfigName"].ToString().EndsWith("_Username"))
                    {
                        dataprovider.Username = reader["ConfigValue"].ToString();
                    }
                    else if (reader["ConfigName"].ToString().EndsWith("_LoginURL"))
                    {
                        dataprovider.LoginURL = reader["ConfigValue"].ToString();
                    }
                    else if (reader["ConfigName"].ToString().EndsWith("_IndexURL"))
                    {
                        dataprovider.IndexURL = reader["ConfigValue"].ToString();
                    }
                }
                reader.Close();
                connection.Close();
            }
            return dataprovider;
        }

        public string DeleteIntradiaryQuotes()
        {
            using (SqlConnection connection = new SqlConnection(this.configs.ConnectionString))
            {
                try
                {
                    connection.Open();
                    string queryString = $"DELETE [dbo].[Stock_Quote_Intradiary]";
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    return $"Error Deleting Intradiary: {ex.Message}";
                }
            }
            return "!!!OK - Intradiary Deleted";
        }

        public string SaveRealTimeQuote(List<RealTimeQuoteDBModel> quotes)
        {
            string connectionString = this.configs.ConnectionString;            

            SqlConnection connection = new SqlConnection(connectionString);
            List<SqlCommand> commands = new List<SqlCommand>();
            string queryString = $@"INSERT INTO [dbo].[Stock_Quote_Intradiary] ([stock_id],[opening],[prev_closing],[ask],[ask_size],[bid],[bid_size],[change],[change_percent],[last_trade_time],[last_trade_price],[last_trade_size],[last_trade_date],[datetime]) VALUES(@stock_id, @opening, @prev_closing, @ask, @ask_size, @bid, @bid_size, @change, @change_percent, @last_trade_time, @last_trade_price, @last_trade_size, @last_trade_date, @datetime);";
            string errorProcesingData = string.Empty;
            string errorSavingData = string.Empty;

            foreach (RealTimeQuoteDBModel quote in quotes)
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
            
            return (string.IsNullOrEmpty(errorProcesingData + errorSavingData)) ? "!!!OK - SaveRealTimeQuote" : $"???{Environment.NewLine}ERRORS SaveRealTimeQuote {Environment.NewLine} Processing: {errorProcesingData}{Environment.NewLine} Saving: {errorSavingData}{Environment.NewLine}???";
        }

        public void SaveRealTimeQuoteV1(List<IRealTimeQuote> quotes)
        {
            string connectionString = this.configs.ConnectionString;

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
                LOG.Log("ERROR-ProcesingData (SaveRealTimeQuoteV1):" + errorProcesingData);                
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
                LOG.Log("ERROR-SavingData (SaveRealTimeQuoteV1):" + errorSavingData);
            }
        }

        public string SaveHistoricalQuote(List<HistoricalQuoteDBModel> quotes)
        {
            string connectionString = this.configs.ConnectionString;            

            SqlConnection connection = new SqlConnection(connectionString);
            List<SqlCommand> commands = new List<SqlCommand>();
            string queryString = $@"IF NOT EXISTS(SELECT 1 FROM Stock_Quote WHERE stock_id = @stock_id AND date_round = CONVERT(date,@date_round)) BEGIN INSERT INTO [dbo].[Stock_Quote] ([stock_id],[opening],[closing],[minimun],[maximun],[volume],[date_round],[adj_close])VALUES(@stock_id, @opening, @closing, @minimun, @maximun, @volume, @date_round, @adj_close); END ELSE BEGIN UPDATE [dbo].[Stock_Quote] SET [opening] = @opening, [closing] = @closing, [minimun] = @minimun, [maximun] = @maximun, [volume] = @volume, [date_round] = @date_round, [adj_close] = @adj_close WHERE [stock_id] = @stock_id AND date_round = CONVERT(date, @date_round); END";
            string errorProcesingData = string.Empty;
            string errorSavingData = string.Empty;

            foreach (HistoricalQuoteDBModel quote in quotes)
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

            return (string.IsNullOrEmpty(errorProcesingData + errorSavingData)) ? "!!!OK - SaveHistoricalQuote" : $"???{Environment.NewLine}ERRORS SaveHistoricalQuote {Environment.NewLine} Processing: {errorProcesingData}{Environment.NewLine} Saving: {errorSavingData}{Environment.NewLine}???";
        }

        public void SaveHistoricalQuoteV1(List<IHistoricalQuote> quotes)
        {
            string connectionString = this.configs.ConnectionString;

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
                LOG.Log("ERROR-ProcesingData (SaveHistoricalQuoteV1):" + errorProcesingData);
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
                LOG.Log("ERROR-SavingData (SaveHistoricalQuoteV1):" + errorSavingData);
            }
        }

        public string UpdateConfig(string configName, string configValue)
        {
            using (SqlConnection connection = new SqlConnection(this.configs.ConnectionString))
            {
                try
                {
                    connection.Open();

                    string queryString = $"UPDATE [dbo].[Config] SET [ConfigValue]='{configValue}' WHERE[ConfigName] = '{configName}'";
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    return $"Error: {ex.Message}{Environment.NewLine} configName={configName}|configValue={configValue}|";
                }
            }
            return "";
        }

        public float GetIntradiaryMaximum(int stockId)
        {
            using (SqlConnection connection = new SqlConnection(this.configs.ConnectionString))
            {
                connection.Open();
                string queryString = $"SELECT Max(opening) FROM Stock_Quote_Intradiary WHERE stock_id = {stockId}";
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    return float.Parse(reader[0].ToString());                    
                }
                reader.Close();
                connection.Close();
            }
            return float.MinValue;
        }

        public float GetIntradiaryMinimum(int stockId)
        {
            using (SqlConnection connection = new SqlConnection(this.configs.ConnectionString))
            {
                connection.Open();
                string queryString = $"SELECT Min(opening) FROM Stock_Quote_Intradiary WHERE stock_id = {stockId}";
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    return float.Parse(reader[0].ToString());
                }
                reader.Close();
                connection.Close();
            }
            return float.MaxValue;
        }
    }
}
