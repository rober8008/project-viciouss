using ctaCOMMON.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Indicator
{
    public class MA : Chart_Indicator
    {
        public int Rounds { get; private set; }
        public string Serie_Color { get; private set; }
        public MA(List<Candel> data_source, int rounds, string serie_color): base(data_source)
        {
            this.Name = "MA";
            this.Rounds = rounds;
            this.Serie_Color = serie_color;
            this.In_Main_Chart = true;
            ApplyFormula();
        }

        public MA()
        {
            // TODO: Complete member initialization
        }        
        public override void ApplyFormula()
        {
            this.Series = new List<Serie>();

            Serie originalDataSource = Candel.GetDataSerie(base.Data_Source, DataSourceFieldUsed.Close, false);

            string label = "MA " + this.Rounds.ToString();
            Serie serie = new Serie() { Color = this.Serie_Color, Serie_Type = SerieType.line, Column_Data_Label = label, Column_Serie_ID = "mave" };
            
            double[] values = new double[this.Rounds];
            for (int i = 0; i < originalDataSource.Data.Count; i++)
            {
                values[i % this.Rounds] = originalDataSource.Data[i].Value;

                if(originalDataSource.Data[i].Visible)
                    serie.Data.Add(new SerieValue() { Date = originalDataSource.Data[i].Date, Value = values.Where(x => x > 0).Average() });
            }
            this.Series.Add(serie);
        }

        public override void InitializeFromDataBaseValues(List<Candel> quotes, string param1, string color1, string param2, string color2, string param3, string color3, CandelRange candelRange)
        {
            this.In_Main_Chart = true;
            int rounds = 0;
            int.TryParse(param1, out rounds);

            this.Name = "MA";
            this.Data_Source = quotes;
            this.Rounds = rounds / base.GetCandelRangeRoundNormalizerValue(candelRange);
            this.Serie_Color = (color1 != null) ? color1 : String.Empty;

            this.Series = new List<Serie>();

            ApplyFormula();
        }

        internal static Serie ApplyFormula(Serie serie, int rounds, string serieColor, SerieType serieType, bool filtering = true)
        {
            Serie result = new Serie() { Color = serieColor, Serie_Type = serieType };

            double[] values = new double[rounds];
            
            for (int i = 0; i < serie.Data.Count; i++)
            {
                values[i % rounds] = serie.Data[i].Value;

                result.Data.Add(new SerieValue() { Date = serie.Data[i].Date, Value = values.Average(), Visible = serie.Data[i].Visible});
            }

            if (filtering)
                result.Data = result.Data.Where(x => x.Visible).ToList();

            return result; 
        }

        internal static Serie ApplySmoothFormula(Serie serie, int rounds, string serieColor, SerieType serieType)
        {
            Serie result = new Serie() { Color = serieColor, Serie_Type = serieType };

            double value = 0;
            for (int i = 0; i < serie.Data.Count; i++)
            {
                value = ((value * (Math.Min(rounds, i + 1) - 1)) + serie.Data[i].Value) / (Math.Min(rounds, i + 1));
                result.Data.Add(new SerieValue() { Date = serie.Data[i].Date, Value = value });
            }

            return result;
        }
    }
}
