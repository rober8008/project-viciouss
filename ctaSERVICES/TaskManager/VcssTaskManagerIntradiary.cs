using ctaCOMMON;
using ctaCOMMON.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaSERVICES.TaskManager
{
    public abstract class VcssTaskManagerIntradiary : VcssTaskManager
    {
        protected override VcssTaskCanExecuteResult CanExecute(VcssTaskModel vcssTask)
        {            
            DateTime marketTime = DateTime.UtcNow.AddHours(vcssTask.VcssTaskInfo.Market.utc_offset);
            if (marketTime.DayOfYear != vcssTask.scheduled_time.DayOfYear)
                return VcssTaskCanExecuteResult.InvalidTime;
            if (String.IsNullOrEmpty(vcssTask.data))
                return VcssTaskCanExecuteResult.InvalidData;
            return VcssTaskCanExecuteResult.Execute;
        }

        protected override VcssTaskCanScheduleResult CanSchedule(VcssTaskInfoModel vcssTaskInfo)
        {            
            DateTime marketTime = DateTime.UtcNow.AddHours(vcssTaskInfo.Market.utc_offset);
            DateTime lastScheduled = VcssTaskManagerService.GetLastVcssTaskScheduledTime(vcssTaskInfo).AddMinutes(vcssTaskInfo.step);
            if (marketTime >= lastScheduled)
            {
                if(MarketService.IsMarketOpen(vcssTaskInfo.Market.Id))
                {
                    return VcssTaskCanScheduleResult.Schedule;
                }
                else
                {
                    return VcssTaskCanScheduleResult.MarketClosed;
                }                        
            }                    
            return VcssTaskCanScheduleResult.InvalidTime;            
        }
    }
}
