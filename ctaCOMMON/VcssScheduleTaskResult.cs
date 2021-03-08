using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON
{
    public class VcssScheduleTaskResult
    {
        public bool Success { get; set; }        
        public int Next { get; set; }
        public VcssTaskInfoEnum TaskInfoId { get; set; }

        public string Message { get; set; }
    }
}
