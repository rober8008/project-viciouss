using ctaCOMMON;
using ctaCOMMON.AdminModel;
using ctaSERVICES;
using ctaSERVICES.QuotesProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ctaWEB.Controllers
{
    public class TaskController : Controller
    {
        // GET: Task
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public string InitializeServices()
        {
            return "Services Initialized";
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<string> ScheduleIntradiaryUpdate(string taskType, DateTime nextTime)
        {
            TaskManagerService.LogTaskError($"{taskType}_RunIntradiaryUpdate", DateTime.Now.ToString());
            
            QuotesProvider quotesProvider = null;
            TaskScheduleType taskScheduleType = (TaskScheduleType)Enum.Parse(typeof(TaskScheduleType), taskType);            
            switch (taskScheduleType)
            {
                case TaskScheduleType.UpdateIntradiaryBOLSAR:
                    quotesProvider = new BOLSARQuotesProvider();                    
                    break;
                case TaskScheduleType.UpdateIntradiaryBOLSARINDEX:
                    quotesProvider = new BOLSARINDEXQuotesProvider();                    
                    break;
                case TaskScheduleType.UpdateIntradiaryEOD:
                    quotesProvider = new EODQuotesProvider();                    
                    break;
            }

            if(quotesProvider != null)
            {
                string jsonQuotes = await quotesProvider.GetQuotesFromExternalInJsonFormatAsync();
                TaskManagerService.ScheduleTask(taskScheduleType, jsonQuotes, nextTime);
            }

            return taskType + " Schedulled";           
        }

        [AllowAnonymous]
        [HttpPost]
        public string ScheduleDailyReportUpdate(string taskType)
        {  
            TaskScheduleType task = (TaskScheduleType)Enum.Parse(typeof(TaskScheduleType), taskType);
            TaskManagerService.ScheduleTask(task, "", DateTime.Now);           

            return taskType + " Schedulled";
        }

        [AllowAnonymous]
        [HttpPost]
        public string ScheduleDeleteIntradiary(string taskType)
        {
            TaskScheduleType task = (TaskScheduleType)Enum.Parse(typeof(TaskScheduleType), taskType);
            TaskManagerService.ScheduleTask(task, "", DateTime.Now);

            return taskType + " Schedulled";
        }

        [AllowAnonymous]
        [HttpPost]
        public string ScheduleTenantTypeExpirationValidation(string taskType)
        {
            TaskScheduleType task = (TaskScheduleType)Enum.Parse(typeof(TaskScheduleType), taskType);
            TaskManagerService.ScheduleTask(task, "", DateTime.Now);

            return taskType + " Schedulled";
        }
    }
}