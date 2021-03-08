using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.AdminModel
{
    public class TaskScheduleModel
    {
        public int TaskId { get; set; }
        public TaskScheduleType TaskType { get; set; }
        public string Data { get; set; }
        public TaskScheduleStatus Status { get; set; }

        public string Description { get; set; }
        public DateTime ExecTime { get; set; }
    }
}
