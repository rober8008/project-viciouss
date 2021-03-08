using ctaCOMMON;
using ctaCOMMON.AdminModel;
using ctaSERVICES.TaskManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ctaWEB.Controllers
{
    public class VcssTaskManagerController : Controller
    {
        // GET: VcssTaskManager
        public ActionResult Index()
        {
            return View();
        }

        public Task<string> InitializeSchedulers()
        {
            return Task.Run(() => { return "Schedullers Initialized!";  });
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ScheduleTask(VcssTaskInfoEnum vcssTaskInfoId)
        {
            try
            {
                VcssTaskManager taskManager = VcssTaskManager.GetTaskSchedulerInstance(vcssTaskInfoId);
                VcssTaskInfoModel taskInfo = VcssTaskManagerService.GetVcssTask(vcssTaskInfoId);
                bool scheduled = taskManager.CreateTask(taskInfo);
                return Json(new VcssScheduleTaskResult() { Success = scheduled, Next = taskInfo.step, TaskInfoId = vcssTaskInfoId }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                VcssTaskManagerService.Log(vcssTaskInfoId.ToString(), "ServerTime", DateTime.Now, "Scheduling", ex.Message, DateTime.Now);
                return Json(new VcssScheduleTaskResult() { Success = false, Next = 5, TaskInfoId = vcssTaskInfoId }, JsonRequestBehavior.AllowGet);
            }            
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ScheduleDailyTechnicalReportTask(string batch)
        {
            VcssTaskInfoEnum vcssTaskInfoId = VcssTaskInfoEnum.DailyTechnicalReportARG;
            try
            {
                VcssTaskManager taskManager = VcssTaskManager.GetTaskSchedulerInstance(vcssTaskInfoId);
                VcssTaskInfoModel taskInfo = VcssTaskManagerService.GetVcssTask(vcssTaskInfoId);
                bool scheduled = taskManager.CreateTask(taskInfo, batch);
                return Json(new VcssScheduleTaskResult() { Success = scheduled, TaskInfoId = vcssTaskInfoId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                VcssTaskManagerService.Log(vcssTaskInfoId.ToString(), "ServerTime", DateTime.Now, "Scheduling", ex.Message, DateTime.Now);
                return Json(new VcssScheduleTaskResult() { Success = false, TaskInfoId = vcssTaskInfoId, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ScheduleHistoryTask(int stockid, DateTime startdate, DateTime enddate)
        {
            VcssTaskInfoEnum vcssTaskInfoId = VcssTaskInfoEnum.HistoryNASDAQ;
            try
            {                
                VcssTaskManager taskManager = VcssTaskManager.GetTaskSchedulerInstance(vcssTaskInfoId);
                VcssTaskInfoModel taskInfo = VcssTaskManagerService.GetVcssTask(vcssTaskInfoId);
                bool scheduled = taskManager.CreateTask(taskInfo, stockid.ToString(), startdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"));
                return Json(new VcssScheduleTaskResult() { Success = scheduled, TaskInfoId = vcssTaskInfoId }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                VcssTaskManagerService.Log(vcssTaskInfoId.ToString(), "ServerTime", DateTime.Now, "Scheduling", ex.Message, DateTime.Now);
                return Json(new VcssScheduleTaskResult() { Success = false, TaskInfoId = vcssTaskInfoId, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public string ExecuteTask(VcssTaskInfoEnum vcssTaskInfoId)
        {
            try
            {
                List<VcssTaskModel> tasks = VcssTaskManagerService.GetVcssTasksToExecute(vcssTaskInfoId);
                foreach (VcssTaskModel task in tasks)
                {
                    try
                    {
                        VcssTaskManager taskManager = VcssTaskManager.GetTaskSchedulerInstance((VcssTaskInfoEnum)task.vcss_task_info_id);
                        bool executed = taskManager.RunTask(task);                         
                        if(vcssTaskInfoId == VcssTaskInfoEnum.DailyTechnicalReportARG)
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        VcssTaskManagerService.Log(task.VcssTaskInfo.name, task.data, task.scheduled_time, "Running", ex.Message, DateTime.Now);
                    }
                }
                return "Task Executed";
            }
            catch(Exception ex)
            {
                VcssTaskManagerService.Log(vcssTaskInfoId.ToString(), "ServerTime", DateTime.Now, "Scheduling", ex.Message, DateTime.Now);
                return "Task Errored";
            }            
        }
    }
}