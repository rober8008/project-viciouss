using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ctaCOMMON;
using ctaCOMMON.AdminModel;

namespace ctaSERVICES.TaskManager
{
    public class VcssTaskManagerClearIntradiary : VcssTaskManager
    {
        protected override VcssTaskCanExecuteResult CanExecute(VcssTaskModel vcssTask)
        {
            DateTime marketTime = DateTime.UtcNow.AddHours(vcssTask.VcssTaskInfo.Market.utc_offset);
            if (marketTime.TimeOfDay.Hours == 5 && marketTime.DayOfWeek != DayOfWeek.Saturday && marketTime.DayOfWeek != DayOfWeek.Sunday && !HolidayService.IsHoliday(vcssTask.VcssTaskInfo.market_id.Value, marketTime))
            {
                return VcssTaskCanExecuteResult.Execute;
            }
            return VcssTaskCanExecuteResult.InvalidTime;
        }

        protected override VcssTaskCanScheduleResult CanSchedule(VcssTaskInfoModel vcssTaskInfo)
        {
            DateTime marketTime = DateTime.UtcNow.AddHours(vcssTaskInfo.Market.utc_offset);
            if(marketTime.TimeOfDay.Hours == 5 && marketTime.DayOfWeek != DayOfWeek.Saturday && marketTime.DayOfWeek != DayOfWeek.Sunday && !HolidayService.IsHoliday(vcssTaskInfo.market_id.Value, marketTime))
            {
                return VcssTaskCanScheduleResult.Schedule;
            }
            return VcssTaskCanScheduleResult.InvalidTime;
        }

        protected override void ExecuteTask(VcssTaskModel vcssTask)
        {
            QuotesService.ClearIntradiaryDataByMarketID(vcssTask.VcssTaskInfo.market_id.Value);
        }

        protected override void ScheduleTask(VcssTaskInfoModel vcssTaskInfo, params string[] parameters)
        {
            DateTime marketTime = DateTime.UtcNow.AddHours(vcssTaskInfo.Market.utc_offset);
            VcssTaskManagerService.ScheduleTask(vcssTaskInfo.Id, "", marketTime);
        }
    }
}
