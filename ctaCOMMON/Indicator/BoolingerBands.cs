using ctaCOMMON.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Indicator
{
    public class BoolingerBands : Chart_Indicator
    {
        public int Rounds { get; private set; }
        public string Serie_Color { get; private set; }
        public BoolingerBands(List<Candel> data_source, int rounds, string serie_color): base(data_source)
        {
            this.Name = "Boolinger Bands";
            this.In_Main_Chart = true;
            this.Rounds = rounds;
            this.Serie_Color = serie_color;

            ApplyFormula();
        }

        public BoolingerBands()
        {
            // TODO: Complete member initialization
        } 
        public override void ApplyFormula()
        {
            this.Series = new List<Serie>();

            Serie originalDataSource = Candel.GetDataSerie(base.Data_Source, DataSourceFieldUsed.Close, false);

            Serie middle_bound = new Serie() { Color = this.Serie_Color, Serie_Type = SerieType.line, Column_Data_Label = "Boolinger " + this.Rounds, Column_Serie_ID = "bbmi" };
            Serie lower_serie = new Serie() { Color = this.Serie_Color, Serie_Type = SerieType.line, Column_Data_Label = "BoolingerLO ", Column_Serie_ID = "bblo" };
            Serie upper_serie = new Serie() { Color = this.Serie_Color, Serie_Type = SerieType.line, Column_Data_Label = "BoolingerUP ", Column_Serie_ID = "bbup" };
            double[] values = base.Data_Source.GetRange(Math.Max(0, (base.Data_Source.Count - originalDataSource.Data.Count) - this.Rounds), Math.Min(this.Rounds,originalDataSource.Data.Count)).Select(d => d.Close).ToArray<double>();

            double desvest = 0;
            double values_average = 0;
            for (int i = 0; i < originalDataSource.Data.Count; i++)
            {
                values[i % this.Rounds] = originalDataSource.Data[i].Value;
                values_average = values.Where(x => x > 0).Average();
               
                //desvest
                var pows = values.Select(x => Math.Pow(x - values_average, 2));
                var sum = pows.Sum() / (this.Rounds - 1);
                desvest = Math.Sqrt(sum);

                if (originalDataSource.Data[i].Visible)
                {
                    middle_bound.Data.Add(new SerieValue() { Date = originalDataSource.Data[i].Date, Value = values_average });
                    upper_serie.Data.Add(new SerieValue() { Date = originalDataSource.Data[i].Date, Value = values_average + (2 * desvest) });
                    lower_serie.Data.Add(new SerieValue() { Date = originalDataSource.Data[i].Date, Value = values_average - (2 * desvest) });
                }
            }

            this.Series.Add(middle_bound);
            this.Series.Add(upper_serie);
            this.Series.Add(lower_serie);
        }

        public override void InitializeFromDataBaseValues(List<Candel> quotes, string param1, string color1, string param2, string color2, string param3, string color3, CandelRange candelRange)
        {
            this.Name = "Boolinger Bands";
            this.In_Main_Chart = true;
            int rounds = 0;
            int.TryParse(param1, out rounds);

            this.Data_Source = quotes;
            this.Rounds = Math.Max(2, rounds / base.GetCandelRangeRoundNormalizerValue(candelRange));
            this.Serie_Color = (color1 != null) ? color1 : String.Empty;

            this.Series = new List<Serie>();

            ApplyFormula();
        }
    }
}
