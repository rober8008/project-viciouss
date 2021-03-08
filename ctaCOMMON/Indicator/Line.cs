using ctaCOMMON.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Indicator
{
    public class Line : Shape_Indicator
    {
        public SerieType LineSerieType { get; set; }
        public Line(List<Candel> Data_Source, DateTime start_date, DateTime end_date, double start_value, double end_value, string color, string name, SerieType lineSerieType = SerieType.line) : base(Data_Source, start_date, end_date, start_value, end_value, color, name)
        {
            this.LineSerieType = lineSerieType;
            ApplyFormula();
        }

        public Line()
        {
            // TODO: Complete member initialization
        }

        public override void ApplyFormula()
        {
            this.Series = new List<Serie>();
            Serie serie = new Serie() { Color = this.Color, Column_Data_Label = this.Name, Serie_Type = this.LineSerieType, Column_Serie_ID = this.Name + this.ID };
            double slope = CalculateSlop(GetDateIndexInDataSource(this.Start_Date), this.Start_Value, GetDateIndexInDataSource(this.End_Date), this.End_Value);
            double independentTerm = CalculateIndependentTerm(slope, GetDateIndexInDataSource(this.Start_Date), this.Start_Value);

            serie.Data.Add(new SerieValue() { Date = this.Start_Date, Value = this.Start_Value });
            DateTime date = this.Start_Date.AddDays(1);
            while(date < this.End_Date)
            {
                serie.Data.Add(new SerieValue() { Date = date, Value = slope * GetDateIndexInDataSource(date) + independentTerm });
                date = date.AddDays(1);
            }
            serie.Data.Add(new SerieValue() { Date = this.End_Date, Value = this.End_Value });
            this.Series.Add(serie);
        }

        private double CalculateSlop(double pointA_X, double pointA_Y, double pointB_X, double pointB_Y) 
        {
            return (pointB_Y - pointA_Y) / (pointB_X - pointA_X);
        }

        private double CalculateIndependentTerm(double slope, double point_X, double point_Y) 
        {
            return point_Y - (slope * point_X);
        }

        private int GetDateIndexInDataSource(DateTime date)
        {
            DateTime lastDataSourceDate = DateTime.MinValue;
            var result = 0;
            foreach (Candel candel in this.Data_Source)
            {
                if (candel.Date == date)
                    return result;
                result++;                
                lastDataSourceDate = candel.Date;
            }

            return result + (date - lastDataSourceDate).Days;            
        }
    }
}
