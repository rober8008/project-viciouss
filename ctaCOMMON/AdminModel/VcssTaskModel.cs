using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.AdminModel
{
    public class VcssTaskModel
    {
        public int Id { get; set; }
        public int vcss_task_info_id { get; set; }
        public DateTime scheduled_time { get; set; }
        public string data { get; set; }

        public virtual VcssTaskInfoModel VcssTaskInfo { get; set; }
    }
}
