using ctaDATAMODEL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ctaCOMMON.AdminModel;
using ctaCOMMON;

namespace ctaSERVICES
{
    public class TaskManagerService
    {
        public static void LogTaskError(string error, string message)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                TashScheduleErrorLog errorlog = new TashScheduleErrorLog() { Error = error, Message = message, Date = DateTime.Now };
                entities.TashScheduleErrorLogs.Add(errorlog);
                entities.SaveChanges();
            }                        
        }

        public static void RemoveTask(int taskId)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                TaskSchedule task = entities.TaskSchedules.Where(t => t.Id == taskId).FirstOrDefault();
                if (task != null)
                {
                    entities.TaskSchedules.Remove(task);
                    entities.SaveChanges();
                }
            }
        }

        public static void ScheduleTask(TaskScheduleType taskType, string data, DateTime nextTime)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();
                TaskSchedule task = new TaskSchedule() { Type = (int)taskType, Data = data, Status = (int)TaskScheduleStatus.Pending, Description = Enum.GetName(typeof(TaskScheduleType), taskType), ExecTime = nextTime };
                entities.TaskSchedules.Add(task);
                entities.SaveChanges();
            }            
        }

        public static List<TaskScheduleModel> GetPendingTasks()
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                return entities.TaskSchedules.Where(ts => ts.Status == (int)TaskScheduleStatus.Pending).OrderBy(t => t.Id).Select(t => new TaskScheduleModel() { TaskId = t.Id, Status = (TaskScheduleStatus)t.Status, TaskType = (TaskScheduleType)t.Type, Description = t.Description, Data = t.Data, ExecTime = t.ExecTime }).ToList();       
            }            
        }

        public static void MarkAsPendingTask(int taskId, int aux = 0)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                TaskSchedule task = entities.TaskSchedules.Where(t => t.Id == taskId).FirstOrDefault();
                if (task != null)
                {
                    task.Status = aux == 0 ? (int)TaskScheduleStatus.InProcess : aux;
                    entities.SaveChanges();
                }
            }
        }
    }
}
