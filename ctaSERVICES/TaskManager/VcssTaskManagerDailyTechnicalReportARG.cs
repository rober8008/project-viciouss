using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ctaCOMMON;
using ctaCOMMON.AdminModel;
using ctaSERVICES.Reporting;

namespace ctaSERVICES.TaskManager
{
    public class VcssTaskManagerDailyTechnicalReportARG : VcssTaskManager
    {
        protected override VcssTaskCanExecuteResult CanExecute(VcssTaskModel vcssTask)
        {
            return VcssTaskCanExecuteResult.Execute;
        }

        protected override VcssTaskCanScheduleResult CanSchedule(VcssTaskInfoModel vcssTaskInfo)
        {
            DateTime marketTime = DateTime.UtcNow.AddHours(vcssTaskInfo.Market.utc_offset);
            if (marketTime.TimeOfDay.Hours == 8 && marketTime.DayOfWeek != DayOfWeek.Saturday && marketTime.DayOfWeek != DayOfWeek.Sunday && !HolidayService.IsHoliday(vcssTaskInfo.market_id.Value, marketTime))
            {
                return VcssTaskCanScheduleResult.Schedule;
            }
            return VcssTaskCanScheduleResult.InvalidTime;
        }

        protected override void ExecuteTask(VcssTaskModel vcssTask)
        {
            DateTime marketTime = DateTime.UtcNow.AddHours(vcssTask.VcssTaskInfo.Market.utc_offset);

            ReportData_Generator generator = new ReportData_Generator();
            generator.GenerateReportData(vcssTask.data);
        }

        protected override void ScheduleTask(VcssTaskInfoModel vcssTaskInfo, params string[] parameters)
        {
            DateTime marketTime = DateTime.UtcNow.AddHours(vcssTaskInfo.Market.utc_offset);
            VcssTaskManagerService.ScheduleTask(vcssTaskInfo.Id, parameters[0], marketTime);
        }
    }
}
