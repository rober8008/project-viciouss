using ctaCOMMON.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ctaCOMMON.Indicator
{
    public abstract class Chart_Indicator : Indicator
    {
        public List<Candel> Data_Source { get; set; }
        public Chart_Indicator(List<Candel> data_source)
        {
            this.Data_Source = data_source;                       
        }

        public Chart_Indicator() { }

        public abstract void InitializeFromDataBaseValues(List<Candel> quotes, string param1, string color1, string param2, string color2, string param3, string color3, CandelRange candelRange); 
        
        public int GetCandelRangeRoundNormalizerValue(CandelRange candelRange)
        {
            switch(candelRange)
            {
                case CandelRange.Daily:
                    return 1;
                case CandelRange.Weekly:
                    return 5;
                case CandelRange.Monthly:
                    return 20;
                default:
                    return 0;
            }
        }
    }
}
