using ctaCOMMON.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Indicator
{
    public abstract class Shape_Indicator : Indicator
    {
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public double Start_Value { get; set; }
        public double End_Value { get; set; }
        public string Color { get; set; }
        //public string Name { get; set; }
        public List<Candel> Data_Source { get; set; }

        public Shape_Indicator() { }

        public Shape_Indicator(List<Candel> Data_Source, DateTime start_date, DateTime end_date, double start_value, double end_value, string color, string name)
        {
            this.Data_Source = Data_Source;
            this.Start_Date = start_date;
            this.End_Date = end_date;
            this.Start_Value = start_value;
            this.End_Value = end_value;
            this.Color = color;
            this.Name = name;
        }
    }
}
