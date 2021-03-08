using ctaCOMMON.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ctaCOMMON.Dashboard
{
    public class DashboardItem
    {
        public string Portfolio { get; set; }
        public int Portfolio_Id { get; set; }
        public List<Symbol> Symbols { get; set; }
        public Chart Chart { get; set; } 
    }
}
