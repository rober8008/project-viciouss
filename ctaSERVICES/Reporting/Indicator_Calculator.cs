using ctaDATAMODEL;
using ctaCOMMON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaSERVICES.Reporting
{
    public class Indicator_Calculator
    {
        public int Rounds { get; set; }
        public IEnumerable<Stock_Quote> Data_Source { get; set; }

        public double GetMA
        {
            get
            {
                return Data_Source.TakeLast(this.Rounds).Average(x => x.closing);
            }
        }

        public Tuple<double, double, double> GetRSI
        {
            get
            {
                var data = Data_Source.TakeLast(this.Rounds).OrderBy(x => x.date_round).ToList();

                double[] values = new double[this.Rounds];                

                for (int i = 1; i < data.Count(); i++)
                {
                    values[i % this.Rounds] = data[i].closing - data[i - 1].closing;
                }

                double ups_average = ((values.Where(x => x > 0).Count() > 0) ? values.Where(x => x > 0).Sum() / this.Rounds : 0);
                double downs_average = ((values.Where(x => x < 0).Count() > 0) ? Math.Abs(values.Where(x => x < 0).Sum() / this.Rounds) : 0);

                return new Tuple<double, double, double>(
                    downs_average == 0 ? 100 : 100 - (100 / (1 + (ups_average / downs_average))),
                    ups_average,
                    downs_average);
            }            
        }

        public double? GetMomentum
        {
            get
            {
                var data = Data_Source.TakeLast(this.Rounds + 1).OrderBy(x => x.date_round).ToList();
                return 100 + (data.Last().closing - data[0].closing);
            }
        }

        public double GetSOFastK(int offset)
        {            
            var data = Data_Source.TakeLast(this.Rounds + offset).Take(this.Rounds).OrderBy(x => x.date_round).ToList();
            var min = data.Min(x => x.minimun);
            var max = data.Max(x => x.maximun);

            if (min == max)
                return 0;

            return 100 * ((data.Last().closing - min) / (max - min));            
        }

        public double GetSOFast_d3
        {
            get
            {
                var values = new List<double>();
                values.Add(this.GetSOFastK(0));
                values.Add(this.GetSOFastK(1));
                values.Add(this.GetSOFastK(2));
                return values.Average();
            }
        }

        public double GetEMA(int rounds, int offset)
        {            
            double result = Data_Source.TakeLast((rounds * 2) + offset).Take(rounds).Average(x => x.closing);
            var data = Data_Source.TakeLast(rounds + offset).Take(rounds);                 
            foreach (Stock_Quote quote in data)
            {
                decimal Kdecimal = Decimal.Divide(2, rounds + 1);
                double K = Double.Parse((Math.Truncate(100000000 * Kdecimal) / 100000000).ToString()); //set to 8 decimal places

                result = (quote.closing * K) + (result * (1 - K));
            }

            return result;                        
        }

        public double GetMA9
        {
            get
            {
                var values = new List<double>();
                values.Add(this.GetEMA(12, 0) - this.GetEMA(26, 0));
                values.Add(this.GetEMA(12, 1) - this.GetEMA(26, 1));
                values.Add(this.GetEMA(12, 2) - this.GetEMA(26, 2));
                values.Add(this.GetEMA(12, 3) - this.GetEMA(26, 3));
                values.Add(this.GetEMA(12, 4) - this.GetEMA(26, 4));
                values.Add(this.GetEMA(12, 5) - this.GetEMA(26, 5));
                values.Add(this.GetEMA(12, 6) - this.GetEMA(26, 6));
                values.Add(this.GetEMA(12, 7) - this.GetEMA(26, 7));
                values.Add(this.GetEMA(12, 8) - this.GetEMA(26, 8));
                return values.Average();                
            }
        }

        public Tuple<double, double> GetBoolinger
        {
            get
            {
                var data = Data_Source.TakeLast(this.Rounds);

                double[] values = data.Select(x => x.closing).ToArray();
                double values_average = values.Average(); ;

                //desvest
                var pows = values.Select(x => Math.Pow(x - values_average, 2));
                var sum = pows.Sum() / (this.Rounds - 1);
                var desvest = Math.Sqrt(sum);

                // upper, lower
                return new Tuple<double, double>(values_average + (2 * desvest), values_average - (2 * desvest));
            }
        }

        public double? GetWilliansR
        {
            get
            {
                var data = Data_Source.TakeLast(this.Rounds)
                                      .OrderBy(x => x.date_round)
                                      .ToList();

                var min = data.Min(x => x.minimun);
                var max = data.Max(x => x.maximun);

                if (min == max)
                    return 0;

                if (data.Count() < this.Rounds)
                {
                    return null;
                }

                return ((max - data.Last().closing) / (max - min)) * -100;
            }
        }
    }
}
