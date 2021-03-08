using ctaCOMMON;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ctaWEB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BundleMobileConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.HttpMethod.ToUpper() == "POST")
            {
                string url = HttpContext.Current.Request.Url.ToString();
                if (url.Contains("/VcssTaskManager/InitializeSchedulers"))
                {
                    InitializeVcssTaskSchedulers();
                }
                else if (url.Contains("/VcssTaskManager/ScheduleTask") || url.Contains("/VcssTaskManager/ScheduleDailyTechnicalReportTask"))
                {
                    RunVcssTaskExecuters();                    
                }
                else if (url.Contains("/VcssTaskManager/ExecuteTask"))
                {
                    RunVcssTaskSchedulers();
                }
                else if (url.Contains("/VcssTaskManager/ScheduleHistoryTask"))
                {
                    RunVcssTaskExecuters(true);
                }                
            }            
        }  

        private void InitializeVcssTaskSchedulers()
        {
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(1), VcssTaskScheduleTrigger, VcssTaskInfoEnum.AuthTokenBCBA, "SCHE", "");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(1), VcssTaskScheduleTrigger, VcssTaskInfoEnum.IntradiaryBCBA, "SCHE", "");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(1), VcssTaskScheduleTrigger, VcssTaskInfoEnum.IntradiaryBCBAINDEX, "SCHE", "");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(1), VcssTaskScheduleTrigger, VcssTaskInfoEnum.IntradiaryNYSE, "SCHE", "");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(1), VcssTaskScheduleTrigger, VcssTaskInfoEnum.IntradiaryNASDAQ, "SCHE", "");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(1), VcssTaskScheduleTrigger, VcssTaskInfoEnum.ClearIntradiaryBCBA, "SCHE", "");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(3), VcssTaskScheduleTrigger, VcssTaskInfoEnum.ClearIntradiaryNASDAQ, "SCHE", "");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(4), VcssTaskScheduleTrigger, VcssTaskInfoEnum.ClearIntradiaryNYSE, "SCHE", "");

            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(5), VcssTaskScheduleTrigger, VcssTaskInfoEnum.DailyTechnicalReportARG, "SCHE", "Batch-1");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(6), VcssTaskScheduleTrigger, VcssTaskInfoEnum.DailyTechnicalReportARG, "SCHE", "Batch-2");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(7), VcssTaskScheduleTrigger, VcssTaskInfoEnum.DailyTechnicalReportARG, "SCHE", "Batch-3");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(8), VcssTaskScheduleTrigger, VcssTaskInfoEnum.DailyTechnicalReportARG, "SCHE", "Batch-4");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(9), VcssTaskScheduleTrigger, VcssTaskInfoEnum.DailyTechnicalReportARG, "SCHE", "Batch-5");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(10), VcssTaskScheduleTrigger, VcssTaskInfoEnum.DailyTechnicalReportARG, "SCHE", "Batch-6");
        }

        private void RunVcssTaskSchedulers()
        {
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(14), VcssTaskScheduleTrigger, VcssTaskInfoEnum.AuthTokenBCBA, "SCHE", "");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(14), VcssTaskScheduleTrigger, VcssTaskInfoEnum.IntradiaryBCBA, "SCHE", "");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(14), VcssTaskScheduleTrigger, VcssTaskInfoEnum.IntradiaryBCBAINDEX, "SCHE", "");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(14), VcssTaskScheduleTrigger, VcssTaskInfoEnum.IntradiaryNYSE, "SCHE", "");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(14), VcssTaskScheduleTrigger, VcssTaskInfoEnum.IntradiaryNASDAQ, "SCHE", "");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(29), VcssTaskScheduleTrigger, VcssTaskInfoEnum.ClearIntradiaryBCBA, "SCHE", "");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(29), VcssTaskScheduleTrigger, VcssTaskInfoEnum.ClearIntradiaryNASDAQ, "SCHE", "");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(29), VcssTaskScheduleTrigger, VcssTaskInfoEnum.ClearIntradiaryNYSE, "SCHE", "");

            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(59), VcssTaskScheduleTrigger, VcssTaskInfoEnum.DailyTechnicalReportARG, "SCHE", "Batch-1");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(59), VcssTaskScheduleTrigger, VcssTaskInfoEnum.DailyTechnicalReportARG, "SCHE", "Batch-2");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(59), VcssTaskScheduleTrigger, VcssTaskInfoEnum.DailyTechnicalReportARG, "SCHE", "Batch-3");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(59), VcssTaskScheduleTrigger, VcssTaskInfoEnum.DailyTechnicalReportARG, "SCHE", "Batch-4");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(59), VcssTaskScheduleTrigger, VcssTaskInfoEnum.DailyTechnicalReportARG, "SCHE", "Batch-5");
            SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(59), VcssTaskScheduleTrigger, VcssTaskInfoEnum.DailyTechnicalReportARG, "SCHE", "Batch-6");
        }

        private void RunVcssTaskExecuters(bool history = false)
        {
            if (!history)
            {
                SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(1), VcssTaskExecuteTrigger, VcssTaskInfoEnum.AuthTokenBCBA, "EXEC", "");
                SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(1), VcssTaskExecuteTrigger, VcssTaskInfoEnum.IntradiaryBCBA, "EXEC", "");
                SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(1), VcssTaskExecuteTrigger, VcssTaskInfoEnum.IntradiaryBCBAINDEX, "EXEC", "");
                SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(1), VcssTaskExecuteTrigger, VcssTaskInfoEnum.IntradiaryNYSE, "EXEC", "");
                SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(1), VcssTaskExecuteTrigger, VcssTaskInfoEnum.IntradiaryNASDAQ, "EXEC", "");
                SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(1), VcssTaskExecuteTrigger, VcssTaskInfoEnum.ClearIntradiaryBCBA, "EXEC", "");
                SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(1), VcssTaskExecuteTrigger, VcssTaskInfoEnum.ClearIntradiaryNASDAQ, "EXEC", "");
                SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(1), VcssTaskExecuteTrigger, VcssTaskInfoEnum.ClearIntradiaryNYSE, "EXEC", "");

                SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(1), VcssTaskExecuteTrigger, VcssTaskInfoEnum.DailyTechnicalReportARG, "EXEC", "Batch-1");
                SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(1), VcssTaskExecuteTrigger, VcssTaskInfoEnum.DailyTechnicalReportARG, "EXEC", "Batch-2");
                SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(1), VcssTaskExecuteTrigger, VcssTaskInfoEnum.DailyTechnicalReportARG, "EXEC", "Batch-3");
                SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(1), VcssTaskExecuteTrigger, VcssTaskInfoEnum.DailyTechnicalReportARG, "EXEC", "Batch-4");
                SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(1), VcssTaskExecuteTrigger, VcssTaskInfoEnum.DailyTechnicalReportARG, "EXEC", "Batch-5");
                SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(1), VcssTaskExecuteTrigger, VcssTaskInfoEnum.DailyTechnicalReportARG, "EXEC", "Batch-6");
            }
            else
            {
                SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(1), VcssTaskExecuteTrigger, VcssTaskInfoEnum.HistoryNYSE, "EXEC", "");
                SetupVcssTaskManagerTrigger(DateTime.Now.AddMinutes(1), VcssTaskExecuteTrigger, VcssTaskInfoEnum.HistoryNASDAQ, "EXEC", "");
            }            
        }

        private void SetupVcssTaskManagerTrigger(DateTime keyExpirationTime, Action<string, object, CacheItemRemovedReason> keyExpirationFunc, VcssTaskInfoEnum taskId, string action, string batch)
        {
            string cacheKey = Enum.GetName(typeof(VcssTaskInfoEnum), taskId) + action + batch;             
            string blockingCacheKey = Enum.GetName(typeof(VcssTaskInfoEnum), taskId) + ((action == "EXEC") ? "SCHE" : "EXEC") + batch;
            if (HttpContext.Current.Cache[blockingCacheKey] == null && HttpContext.Current.Cache[cacheKey] == null)
            {
                CookieExpirationValue cookieExpirationValue = new CookieExpirationValue() { TaskId = taskId, Action = action, Batch = batch };
                HttpContext.Current.Cache.Add(cacheKey, cookieExpirationValue, null, keyExpirationTime, Cache.NoSlidingExpiration, CacheItemPriority.Normal, new CacheItemRemovedCallback(keyExpirationFunc));
            }
        }

        private async void VcssTaskScheduleTrigger(string key, object value, CacheItemRemovedReason reason)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    CookieExpirationValue cookieExpirationValue = value as CookieExpirationValue;                    
                    var reqparm = new System.Collections.Specialized.NameValueCollection();
                    if (cookieExpirationValue.TaskId == VcssTaskInfoEnum.DailyTechnicalReportARG)
                    {
                        reqparm.Add("batch", cookieExpirationValue.Batch);
                        await client.UploadValuesTaskAsync(WebConfigurationManager.AppSettings["VcssScheduleDailyTechnicalReport"], reqparm);
                    }
                    else
                    {
                        reqparm.Add("vcssTaskInfoId", cookieExpirationValue.TaskId.ToString());
                        await client.UploadValuesTaskAsync(WebConfigurationManager.AppSettings["VcssScheduleTaskURL"], reqparm);
                    }
                    
                }
            }
            catch (Exception ex) { }
        }

        private async void VcssTaskExecuteTrigger(string key, object value, CacheItemRemovedReason reason)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    CookieExpirationValue cookieExpirationValue = value as CookieExpirationValue;
                    var reqparm = new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("vcssTaskInfoId", cookieExpirationValue.TaskId.ToString());                    
                    await client.UploadValuesTaskAsync(WebConfigurationManager.AppSettings["VcssExecuteTaskURL"], reqparm);                    
                }
            }
            catch (Exception ex) { }
        }
    }

    public class CookieExpirationValue
    {
        public VcssTaskInfoEnum TaskId { get; set; }
        public string Action { get; set; }
        public string Batch { get; set; }
    }
}
