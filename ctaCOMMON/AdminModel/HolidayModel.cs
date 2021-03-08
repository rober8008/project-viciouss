using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.AdminModel
{
    public class HolidayModel
    {
        public int Id { get; set; }
        public DateTime date { get; set; }
        public string duration { get; set; }
        public int market_id { get; set; }
        public string market_name { get; set; }
    }
}
