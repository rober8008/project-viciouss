using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ctaSERVICES;
using ctaSERVICES.Reporting;
using ctaCOMMON.AdminModel;
using ctaCOMMON;
using ctaSERVICES.QuotesProvider;
using ctaCOMMON.Interface;

namespace ctaWindowsService
{
    public partial class ctaQUOTESservice : ServiceBase
    {        
        Configs configs;
        DBContext dbcontext;
        bool starting = true;
        public ctaQUOTESservice()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            this.OnStart(null);
        }

        protected override void OnStart(string[] args)
        {            
            this.InitializeConfigs();
            LOG.Log("===> ServiceV1 is onSTART at " + DateTime.Now);
            this.OnElapsedTime(null, null);
        }

        protected override void OnStop()
        {
            LOG.Log("<=== Service is STOPPED at " + DateTime.Now);
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            LOG.Log(Environment.NewLine + "> Service RECALLED at " + DateTime.Now);
            if (!starting)
            {
                List<TaskScheduleModel> tasks = TaskManagerService.GetPendingTasks();
                LOG.Log($">> Pending Tasks:  {tasks.Count()} at " + DateTime.Now);
                
                foreach (TaskScheduleModel task in tasks)
                {
                    LOG.Log($">> Processing Task:  {task.Description} at " + DateTime.Now);
                    bool taskResult = RunTask(task);
                    LOG.Log($">> Processed Task:  {task.Description} at " + DateTime.Now);
                    if (taskResult)
                    {
                        TaskManagerService.RemoveTask(task.TaskId);
                        LOG.Log($">> Removed Task:  {task.Description} at " + DateTime.Now);
                    }                        
                    else
                    {
                        TaskManagerService.MarkAsPendingTask(task.TaskId, 555);
                        LOG.Log($">> Pending Task:  {task.Description} at " + DateTime.Now);
                    }
                }                
            }
            else
            {
                LOG.Log("===> Service STARTED at " + DateTime.Now);
                starting = false;
            }
            LOG.Log("< Service ENDED at " + DateTime.Now);
        }

        private bool RunTask(TaskScheduleModel task)
        {
            //if (task.TaskType == TaskScheduleType.UpdateIntradiaryBOLSAR)
            //{
            //    try
            //    {
            //        if (String.IsNullOrEmpty(task.Data))
            //            UpdateMarketRealTimeQuotes("BCBA");
            //        else
            //        {
            //            LOG.Log($"!!!BOLSAR(BCBA): Data => {task.Data}{Environment.NewLine}");
            //            UpdateMarketRealTimeQuotes("BOLSAR", "BCBA", task.Data);
            //        }                                           
            //    }
            //    catch (Exception ex)
            //    {
            //        LOG.Log($"???ERROR(BCBA): Message=>{ex.Message}{Environment.NewLine}");
            //        return false;
            //    }
            //}

            //if(task.TaskType == TaskScheduleType.UpdateIntradiaryBOLSARINDEX)
            //{
            //    try
            //    {
            //        if (!String.IsNullOrEmpty(task.Data))
            //        {
            //            LOG.Log($"!!!BOLSARINDEX(BCBA): Data => {task.Data}{Environment.NewLine}");
            //            UpdateMarketRealTimeQuotes("BOLSARINDEX", "BCBA", task.Data);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        LOG.Log($"???ERROR(BCBAINDEX): Message=>{ex.Message}{Environment.NewLine}");
            //        return false;
            //    }
            //}
            
            //if(task.TaskType == TaskScheduleType.UpdateIntradiaryEOD)
            //{
            //    try
            //    {
            //        if (String.IsNullOrEmpty(task.Data))
            //        {
            //            try { UpdateMarketRealTimeQuotes("NYSE"); } catch (Exception ex) { LOG.Log($"???ERROR(NYSE): Message=>{ex.Message}{Environment.NewLine}"); return false; }
            //            try { UpdateMarketRealTimeQuotes("NASDAQ"); } catch (Exception ex) { LOG.Log($"???ERROR(NASDAQ): Message=>{ex.Message}{Environment.NewLine}"); return false; }
            //        }
            //        else
            //            UpdateMarketRealTimeQuotes("EOD","NYSE", task.Data);
            //    }
            //    catch (Exception ex)
            //    {
            //        LOG.Log($"???ERROR(EOD): Message=>{ex.Message}{Environment.NewLine}");
            //        return false;
            //    }                
            //}
            
            if(task.TaskType == TaskScheduleType.DailyStockTechnicalReport)
            {
                try { UpdateReportData(); } catch (Exception ex) { LOG.Log($"???ERROR(REPORT): Message=>{ex.Message}{Environment.NewLine}"); return false; }
            }
            
            if(task.TaskType == TaskScheduleType.TenantTypeExpirationValidation)
            {
                try { VerifyUserTypeExpiration(); } catch (Exception ex) { LOG.Log($"???ERROR(VERIFY USER TYPE EXPIRATION): Message=>{ex.Message}{Environment.NewLine}"); return false; }
                try { UpdateUserTypeExpirationFromMercadoPago(); } catch (Exception ex) { LOG.Log($"???ERROR(MERCADO PAGO: UPDATE USER TYPE EXPIRATION): Message=>{ex.Message}{Environment.NewLine}"); return false; }
            }

            if(task.TaskType == TaskScheduleType.DeleteIntradiaryBOLSAR)
            {
                try { DeleteIntradiaryData("BCBA"); } catch (Exception ex) { LOG.Log($"???ERROR(DeleteIntradiaryData=BOLSAR): Message=>{ex.Message}{Environment.NewLine}"); return false; }
            }

            if (task.TaskType == TaskScheduleType.DeleteIntradiaryEOD)
            {
                try { DeleteIntradiaryData("NASDAQ"); } catch (Exception ex) { LOG.Log($"???ERROR(DeleteIntradiaryData=NASDAQ): Message=>{ex.Message}{Environment.NewLine}"); return false; }
                try { DeleteIntradiaryData("NYSE"); } catch (Exception ex) { LOG.Log($"???ERROR(DeleteIntradiaryData=NYSE): Message=>{ex.Message}{Environment.NewLine}"); return false; }
            }

            return true;
        }

        private void InitializeConfigs()
        {
            this.configs = new Configs();
            this.configs.ConnectionString = ConfigurationManager.AppSettings["connectionString"];

            LOG.Log($"ConnectionString: {this.configs.ConnectionString}");
            this.configs.Timer = new Timer();
            this.configs.Timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            this.configs.Timer.Interval = int.Parse(ConfigurationManager.AppSettings["timerInterval"]);
            this.configs.Timer.Enabled = true;

            this.dbcontext = new DBContext(this.configs);
        }

        private void UpdateMarketRealTimeQuotes(string marketName)
        {
            LOG.Log($"   >> {marketName} Starting - {DateTime.Now}");

            Configs_Market marketConfigs = this.dbcontext.GetMarketConfig(marketName);
            LOG.Log($"      MARKET: {marketConfigs.Name}|{marketConfigs.LastSync}|{marketConfigs.NextSync}|{marketConfigs.UTCOffset}|{marketConfigs.QuotesUpdatesActive}|");

            Configs_QuoteDataProvider dataproviderConfigs = this.dbcontext.GetQuoteDataProviderConfig(marketConfigs.QuoteProviderName);
            LOG.Log($"      QUOTES:{dataproviderConfigs.Name}|{dataproviderConfigs.APIToken}|{dataproviderConfigs.HistoricalURL}|{dataproviderConfigs.RealTimeURL}|");

            List<mdlStock> stocks = this.dbcontext.GetMarketActiveStocks(marketName);
            LOG.Log($"      STOCKS: Count={stocks.Count}|");

            DateTime date = DateTime.UtcNow.AddHours(marketConfigs.UTCOffset);
            if (Configs_Market.QuoteUpdateAvailabe(marketConfigs, this.dbcontext.IsHoliday, date))
            {
                QuotesDataProvider quotesDataProvider = QuotesDataProvider.GetQuoteDataProvider(dataproviderConfigs, marketConfigs);                
                List<HistoricalQuoteDBModel> historical;
                List<RealTimeQuoteDBModel> realtime = quotesDataProvider.GetRealTimeQuotes(stocks, date, this.dbcontext, out historical);
                LOG.Log($"      REALTIME: Count={realtime.Count}|");
                LOG.Log($"      HISTORICAL: Count={historical.Count}|");
                this.dbcontext.SaveRealTimeQuote(realtime);
                this.dbcontext.SaveHistoricalQuote(historical);
                this.dbcontext.UpdateConfig($"{marketName}_LastSync", date.ToString());                             
            }
            this.dbcontext.UpdateConfig($"{marketName}_NextSync", date.AddMinutes(15).ToString());

            if(date.Hour == 5)
            {
                string result = this.dbcontext.DeleteIntradiaryQuotes();
                LOG.Log($"      {result}");                
            }

            LOG.Log($"   << {marketName} End - {DateTime.Now}");
        }

        private void UpdateMarketRealTimeQuotes(string providerName, string marketName, string data)
        {
            Configs_Market marketConfigs = this.dbcontext.GetMarketConfig(marketName);
            LOG.Log($">>> MARKET Configs: {marketConfigs.Name}|{marketConfigs.LastSync}|{marketConfigs.NextSync}|{marketConfigs.UTCOffset}|{marketConfigs.QuotesUpdatesActive}|");

            DateTime date = DateTime.UtcNow.AddHours(marketConfigs.UTCOffset);
            if (Configs_Market.QuoteUpdateAvailabe(marketConfigs, this.dbcontext.IsHoliday, date))
            {
                QuotesProvider quotesProvider = QuotesProvider.GetQuotesProviderInstance(providerName);
                Tuple<List<IRealTimeQuote>, List<IHistoricalQuote>> quotes = quotesProvider.GetQuotesFromJson(data);
                this.dbcontext.SaveRealTimeQuoteV1(quotes.Item1);
                this.dbcontext.SaveHistoricalQuoteV1(quotes.Item2);
                this.dbcontext.UpdateConfig($"{marketName}_LastSync", date.ToString());
            }
            this.dbcontext.UpdateConfig($"{marketName}_NextSync", date.AddMinutes(15).ToString());
        }

        private void DeleteIntradiaryData(string marketName)
        {
            DateTime marketTime = MarketService.GetMarketCurrentLocalTime(marketName);
            if(marketTime.Hour == 5)
                QuotesService.ClearIntradiaryDataByMarketID(marketName);
        }

        private void UpdateReportData()
        {
            DateTime marketTime = MarketService.GetMarketCurrentLocalTime("BCBA");
            if (marketTime.Hour == 3)
            {
                LOG.Log($"   >> REPORT Starting - {DateTime.Now}");
                DateTime reportNextSync = DateTime.Parse(ConfigService.GetConfig("ReportDATA_NextSync").ConfigValue);
                LOG.Log($"   >> REPORT reportNextSync - {reportNextSync}");
                if (MarketService.GetMarketCurrentLocalTime("BCBA") > reportNextSync)
                {
                    ReportData_Generator generator = new ReportData_Generator();
                    generator.GenerateReportData(true);
                }
                LOG.Log($"   << REPORT End - {DateTime.Now}");
            }            
        }

        private void VerifyUserTypeExpiration()
        {
            LOG.Log($"   >> VERIFY USER TYPE EXPIRATION Starting - {DateTime.Now}");
            if (DateTime.Now.Hour == 1)
            {
                UserService.ValidateUserTypeExpiration(DateTime.Now.Date);
            }
            LOG.Log($"   << VERIFY USER TYPE EXPIRATION End - {DateTime.Now}");
        }

        private void UpdateUserTypeExpirationFromMercadoPago()
        {
            DateTime marketTime = MarketService.GetMarketCurrentLocalTime("BCBA");
            LOG.Log($"   >> MERCADO PAGO: UPDATE USER TYPE EXPIRATION Starting - {DateTime.Now}");
            if (marketTime.Hour == 1)
            {
                UserService.UpdateUserTypeExpirationFromMercadoPago(DateTime.Now.Date);
            }
            LOG.Log($"   << MERCADO PAGO: UPDATE USER TYPE EXPIRATION End - {DateTime.Now}");            
        }
    }
}
