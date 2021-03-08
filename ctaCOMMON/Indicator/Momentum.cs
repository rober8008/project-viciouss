using ctaCOMMON.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Indicator
{
    public class Momentum : Chart_Indicator
    {
        public int Rounds { get; private set; }
        public string Serie_Color { get; private set; }
        public string Serie_Hundred_Color { get; private set; }
        public Momentum(List<Candel> data_source, int rounds, string serie_color, string serie_hundred_color): base(data_source)
        {
            this.Name = "Momentum";
            this.In_Main_Chart = false;
            this.Rounds = rounds;
            this.Serie_Color = serie_color;
            this.Serie_Hundred_Color = serie_hundred_color;

            ApplyFormula();
        }

        public Momentum()
        {
            // TODO: Complete member initialization
        } 
        public override void ApplyFormula()
        {
            this.Series = new List<Serie>();

            Serie serie = new Serie() { Color = this.Serie_Color, Serie_Type = SerieType.line, Column_Data_Label = "Momentum " + this.Rounds, Column_Serie_ID = "moms" };

            Serie originalDataSource = Candel.GetDataSerie(base.Data_Source, DataSourceFieldUsed.Close, false);
            double value = 0;
            for (int i = 0; i < originalDataSource.Data.Count; i++)
            {
                value = 100 + originalDataSource.Data[i].Value - originalDataSource.Data[Math.Max(0, i - this.Rounds)].Value;

                if(originalDataSource.Data[i].Visible)
                    serie.Data.Add(new SerieValue() { Date = originalDataSource.Data[i].Date, Value = value });
            }

            Serie hundred_serie = new Serie() { Color = this.Serie_Hundred_Color, Serie_Type = SerieType.line, Column_Data_Label = "Hundred", Column_Serie_ID = "momh" };
            foreach (var item in serie.Data)
            {
                hundred_serie.Data.Add(new SerieValue() { Date = item.Date, Value = 100 });
            }

            this.Series.Add(serie);            
            this.Series.Add(hundred_serie);
        }

        public override void InitializeFromDataBaseValues(List<Candel> quotes, string param1, string color1, string param2, string color2, string param3, string color3, CandelRange candelRange)
        {
            this.In_Main_Chart = false;
            int rounds = 0;
            int.TryParse(param1, out rounds);

            this.Name = "Momentum";
            this.Data_Source = quotes;
            this.Rounds = rounds / base.GetCandelRangeRoundNormalizerValue(candelRange);
            this.Serie_Color = (color1 != null) ? color1 : String.Empty;
            this.Serie_Hundred_Color = (color2 != null) ? color2 : String.Empty;

            this.Series = new List<Serie>();

            ApplyFormula();
        }
    }
}
