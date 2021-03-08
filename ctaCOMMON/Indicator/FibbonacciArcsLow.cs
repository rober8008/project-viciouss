using ctaCOMMON.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Indicator
{
    public class FibbonacciArcsLow : Shape_Indicator
    {
        public FibbonacciArcsLow() { this.In_Main_Chart = true; }
        public FibbonacciArcsLow(List<Candel> Data_Source, DateTime start_date, DateTime end_date, double start_value, double end_value, string color, string name)
            : base(Data_Source, start_date, end_date, start_value, end_value, color, name)
        {
            this.In_Main_Chart = true;
            ApplyFormula();
        }
        public override void ApplyFormula()
        {
            List<Candel> quotesRange = this.Data_Source.Where(c => c.Date >= this.Start_Date && c.Date <= this.End_Date).ToList();
            double maximun = quotesRange.Select(q => q.Maximun).Max();
            double minimun = quotesRange.Select(q => q.Minimun).Min();

            double C = maximun - minimun;
            Line rectMax = new Line(quotesRange, this.Start_Date, this.End_Date, maximun, maximun, this.Color, this.Name + "(Max)");
            Line rect0618 = new Line(quotesRange, this.Start_Date, this.End_Date, maximun, (minimun + (0.618 * C)), this.Color, this.Name + "(0.618)", SerieType.dashed);
            Line rect05 = new Line(quotesRange, this.Start_Date, this.End_Date, maximun, (minimun + (0.5 * C)), this.Color, this.Name + "(0.5)", SerieType.dashed);
            Line rect0382 = new Line(quotesRange, this.Start_Date, this.End_Date, maximun, (minimun + (0.382 * C)), this.Color, this.Name + "(0.382)", SerieType.dashed);
            Line rectMin = new Line(quotesRange, this.Start_Date, this.End_Date, minimun, minimun, this.Color, this.Name + "(Min)");
            Line rectTrend = new Line(quotesRange, this.Start_Date, this.End_Date, maximun, minimun, this.Color, this.Name);

            this.Series = new List<Serie>();
            this.Series.Add(rectTrend.Series[0]);
            this.Series.Add(rectMax.Series[0]);
            this.Series.Add(rect0618.Series[0]);
            this.Series.Add(rect05.Series[0]);
            this.Series.Add(rect0382.Series[0]);
            this.Series.Add(rectMin.Series[0]);            
        }
    }
}
