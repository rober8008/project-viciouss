using ctaCOMMON;
using ctaCOMMON.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaSERVICES.TaskManager
{
    public abstract class VcssTaskManager
    {
        protected abstract VcssTaskCanScheduleResult CanSchedule(VcssTaskInfoModel vcssTaskInfo);
        protected abstract void ScheduleTask(VcssTaskInfoModel vcssTaskInfo, params string[] parameters);
        protected abstract VcssTaskCanExecuteResult CanExecute(VcssTaskModel vcssTask);
        protected abstract void ExecuteTask(VcssTaskModel vcssTask);
        
        protected void UpdateTaskExecutedTime(VcssTaskModel vcssTask)
        {
            VcssTaskManagerService.UpdateExecutedTime(vcssTask.VcssTaskInfo);            
        }

        public bool CreateTask(VcssTaskInfoModel vcssTaskInfo, params string[] parameters)
        {
            VcssTaskCanScheduleResult canSchedule = VcssTaskCanScheduleResult.EMPTY;
            string parametersStr = "";
            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    parametersStr += item + ";";
                }
            }

            try
            {
                VcssTaskManagerService.Log(vcssTaskInfo.name, "Last Updated->", vcssTaskInfo.last_updated, "Create", "", DateTime.Now);
                canSchedule = this.CanSchedule(vcssTaskInfo);
                if (canSchedule == VcssTaskCanScheduleResult.Schedule)
                {
                    this.ScheduleTask(vcssTaskInfo, parameters);
                    return true;
                }
                else
                {                    
                    VcssTaskManagerService.Log(vcssTaskInfo.name, "Last Updated-> Params:" + parametersStr, vcssTaskInfo.last_updated, "Creating", canSchedule.ToString(), DateTime.Now);
                    return false;
                }
            }
            catch(Exception ex)
            {
                VcssTaskManagerService.Log(vcssTaskInfo.name, "Last Updated-> Params:" + parametersStr + " -> CanSchedule: " + canSchedule.ToString(), vcssTaskInfo.last_updated, "Create ERROR", ex.Message, DateTime.Now);
                return false;
            }                       
        }

        public bool RunTask(VcssTaskModel vcssTask)
        {
            VcssTaskCanExecuteResult canExecute = VcssTaskCanExecuteResult.EMPTY;
            try
            {
                VcssTaskManagerService.Log(vcssTask.VcssTaskInfo.name, vcssTask.data, vcssTask.scheduled_time, "Run", "", DateTime.Now);
                canExecute = this.CanExecute(vcssTask);
                if (canExecute == VcssTaskCanExecuteResult.Execute)
                {
                    this.ExecuteTask(vcssTask);
                    this.UpdateTaskExecutedTime(vcssTask);
                    VcssTaskManagerService.DeleteTask(vcssTask);
                    return true;
                }
                else
                {
                    VcssTaskManagerService.Log(vcssTask.VcssTaskInfo.name, vcssTask.data, vcssTask.scheduled_time, "Running", canExecute.ToString(), DateTime.Now);
                    return false;
                }
            }
            catch(Exception ex)
            {
                VcssTaskManagerService.Log(vcssTask.VcssTaskInfo.name, "Info -> CanExecute: " + canExecute.ToString() + " -> Data: " + vcssTask.data, vcssTask.scheduled_time, "Run ERROR", ex.Message, DateTime.Now);
                return false;
            }        
        }        

        public static VcssTaskManager GetTaskSchedulerInstance(VcssTaskInfoEnum vcssTaskInfoId)
        {
            switch (vcssTaskInfoId)
            {
                case VcssTaskInfoEnum.IntradiaryBCBA:
                    return new VcssTaskManagerIntradiaryBCBA();
                case VcssTaskInfoEnum.AuthTokenBCBA:
                    return new VcssTaskManagerAuthTokenBCBA();
                case VcssTaskInfoEnum.IntradiaryBCBAINDEX:
                    return new VcssTaskManagerIntradiaryBCBAINDEX();
                case VcssTaskInfoEnum.IntradiaryNYSE:
                    return new VcssTaskManagerIntradiaryNYSE();
                case VcssTaskInfoEnum.IntradiaryNASDAQ:
                    return new VcssTaskManagerIntradiaryNASDAQ();
                case VcssTaskInfoEnum.HistoryNASDAQ:
                    return new VcssTaskManagerHistory();
                case VcssTaskInfoEnum.HistoryNYSE:
                    return new VcssTaskManagerHistory();
                case VcssTaskInfoEnum.ClearIntradiaryBCBA:
                    return new VcssTaskManagerClearIntradiary();
                case VcssTaskInfoEnum.ClearIntradiaryNASDAQ:
                    return new VcssTaskManagerClearIntradiary();
                case VcssTaskInfoEnum.ClearIntradiaryNYSE:
                    return new VcssTaskManagerClearIntradiary();
                case VcssTaskInfoEnum.DailyTechnicalReportARG:
                    return new VcssTaskManagerDailyTechnicalReportARG();
            }
            return null;
        }
    }
}
