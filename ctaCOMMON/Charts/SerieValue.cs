using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ctaCOMMON.Charts
{
    public class SerieValue
    {
        public DateTime Date { get; set; }        
        public double Value { get; set; }
        public string Tooltip { get; set; }      
        public bool Visible { get; set; }
    }
}
