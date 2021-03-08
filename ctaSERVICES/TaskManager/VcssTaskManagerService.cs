using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ctaCOMMON.AdminModel;
using ctaDATAMODEL;
using System.Data.Entity;
using ctaCOMMON;

namespace ctaSERVICES.TaskManager
{
    public class VcssTaskManagerService
    {
        public static DateTime GetLastVcssTaskScheduledTime(VcssTaskInfoModel vcssTaskInfo)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                return entities.VcssTasks.Where(t => t.vcss_task_info_id == vcssTaskInfo.Id).OrderByDescending(t => t.Id).Select(t => t.scheduled_time).FirstOrDefault();
            }
        }

        public static void UpdateExecutedTime(VcssTaskInfoModel vcssTaskInfo)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                VcssTaskInfo taskInfo = entities.VcssTaskInfoes.Where(t => t.Id == vcssTaskInfo.Id).FirstOrDefault();
                if(taskInfo != null)
                {
                    DateTime marketTime = DateTime.UtcNow.AddHours(vcssTaskInfo.Market.utc_offset);
                    taskInfo.last_updated = marketTime;
                    entities.SaveChanges();
                }
            }
        }

        public static VcssTaskInfoModel GetVcssTask(VcssTaskInfoEnum vcssTaskInfoId)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                return entities.VcssTaskInfoes
                    .Include(t => t.Market)
                    .Where(t => t.Id == (int)vcssTaskInfoId)
                    .Select(t => new VcssTaskInfoModel() {
                        auth_token = t.auth_token,
                        last_updated = t.last_updated,
                        market_id = t.market_id,
                        name = t.name,
                        step = t.step,
                        url = t.url,
                        Id = t.Id,
                        Market = new MarketModel() {
                            Id = t.Market.Id,
                            name = t.Market.name,
                            utc_offset = t.Market.utc_offset,
                            work_hours = t.Market.work_hours
                        }
                    }).FirstOrDefault();                
            }
        }

        public static void DeleteTask(VcssTaskModel vcssTask)
        {
            using(ctaDBEntities entities = new ctaDBEntities())
            {
                VcssTask task = entities.VcssTasks.Where(t => t.Id == vcssTask.Id).FirstOrDefault();
                if(task != null)
                {
                    entities.VcssTasks.Remove(task);
                    entities.SaveChanges();
                }                
            }
        }

        public static List<VcssTaskModel> GetVcssTasksToExecute(VcssTaskInfoEnum vcssTaskInfoId)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                return entities.VcssTasks
                    .Include(t => t.VcssTaskInfo)
                    .Include(t => t.VcssTaskInfo.Market) 
                    .Where(t => t.vcss_task_info_id == (int)vcssTaskInfoId)
                    .Select(t => new VcssTaskModel() {
                        Id = t.Id,
                        data = t.data,
                        scheduled_time = t.scheduled_time,
                        vcss_task_info_id = t.vcss_task_info_id,
                        VcssTaskInfo = new VcssTaskInfoModel() {
                            auth_token = t.VcssTaskInfo.auth_token,
                            last_updated = t.VcssTaskInfo.last_updated,
                            market_id = t.VcssTaskInfo.market_id,
                            name = t.VcssTaskInfo.name,
                            step = t.VcssTaskInfo.step,
                            url = t.VcssTaskInfo.url,
                            Id = t.VcssTaskInfo.Id,
                            Market = new MarketModel()
                            {
                                Id = t.VcssTaskInfo.Market.Id,
                                name = t.VcssTaskInfo.Market.name,
                                utc_offset = t.VcssTaskInfo.Market.utc_offset,
                                work_hours = t.VcssTaskInfo.Market.work_hours
                            }
                        }
                    })
                    .ToList<VcssTaskModel>();
            }
        }

        public static void ScheduleTask(int vcssTaskInfoId, string data, DateTime scheduledTime)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                VcssTask task = new VcssTask() { vcss_task_info_id = vcssTaskInfoId, data = data, scheduled_time = scheduledTime };
                entities.VcssTasks.Add(task);
                entities.SaveChanges();
            }
        }

        public static void UpdateBOLSARAuthToken(string authToken)
        {
            using(ctaDBEntities entities = new ctaDBEntities())
            {
                List<VcssTaskInfo> tasksInfo = entities.VcssTaskInfoes.Where(t => t.name == "IntradiaryBCBAINDEX" || t.name == "IntradiaryBCBA").ToList();
                for (int i = 0; i < tasksInfo.Count; i++)
                {
                    tasksInfo[i].auth_token = authToken;
                }
                entities.SaveChanges();
            }
        }

        public static void Log(string taskName, string taskData, DateTime taskScheduledTime, string action, string errorMessage, DateTime errorTime)
        {
            using(ctaDBEntities entities = new ctaDBEntities())
            {
                VcssTaskLog log = new VcssTaskLog() { action = action, error_msg = errorMessage, error_time = errorTime, task_data = taskData, task_name = taskName, task_scheduled_time = taskScheduledTime  };
                entities.VcssTaskLogs.Add(log);
                entities.SaveChanges();
            }
        }
    }
}
