using ctaCOMMON.Charts;
using ctaCOMMON.Indicator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Dashboard
{
    public class SymbolDashboard
    {
        public SymbolDashboard()
        {
            
        }
        public int Portfolio_Id { get; set; }
        public int Symbol_Id { get; set; }
        public Symbol Symbol { get; set; }
        public List<Chart_Indicator> Indicators { get; set; }
        public List<Shape_Indicator> Shapes { get; set; }
        public object SymbolDashboardToJSON()
        {
            return null;
        }
    }
}
