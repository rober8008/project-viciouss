using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ctaCOMMON.Charts;

namespace ctaCOMMON.Indicator
{
    public class VolumeOscillator : Chart_Indicator
    {
        public int LongRounds { get; private set; }
        public int ShortRounds { get; private set; }
        public string Serie_Color { get; private set; }

        public VolumeOscillator(List<Candel> data_source, int srounds, int lrounds, string serie_color): base(data_source)
        {
            this.Name = "Volume Oscillator";
            this.In_Main_Chart = false;
            this.ShortRounds = srounds;
            this.LongRounds = lrounds;
            this.Serie_Color = serie_color;

            this.Series = new List<Serie>();

            ApplyFormula();
        }

        public VolumeOscillator()
        {
            // TODO: Complete member initialization
        }

        public override void ApplyFormula()
        {
            this.Series = new List<Serie>();

            Serie volumeData = Candel.GetDataSerie(base.Data_Source, DataSourceFieldUsed.Volume, false);
            volumeData.Data = volumeData.Data.Where(x => x.Visible).ToList();
            Serie shortVolAverage = MA.ApplyFormula(volumeData, this.ShortRounds, "", SerieType.dashed);
            Serie longVolAverage = MA.ApplyFormula(volumeData, this.LongRounds, "", SerieType.dashed);

            Serie serie = new Serie() { Color = this.Serie_Color, Serie_Type = SerieType.line, Column_Serie_ID = "vosc" };
            serie.Column_Data_Label = string.Format("VO {0}-{1}", this.ShortRounds, this.LongRounds);

            for (int i = 0; i < volumeData.Data.Count; i++)
            {
                serie.Data.Add(new SerieValue()
                {
                    Date = volumeData.Data[i].Date,
                    Value = longVolAverage.Data[i].Value - shortVolAverage.Data[i].Value
                });
            }

            this.Series.Add(serie);
        }

        public override void InitializeFromDataBaseValues(List<Candel> quotes, string param1, string color1, string param2, string color2, string param3, string color3, CandelRange candelRange)
        {
            int shortTerm = 0;
            int.TryParse(param1, out shortTerm);
            int longTerm = 0;
            int.TryParse(param2, out longTerm);

            this.Name = "VOSC";
            this.In_Main_Chart = false;
            this.Data_Source = quotes;
            this.ShortRounds = shortTerm / base.GetCandelRangeRoundNormalizerValue(candelRange);
            this.LongRounds = longTerm / base.GetCandelRangeRoundNormalizerValue(candelRange);
            this.Serie_Color = (color1 != null) ? color1 : String.Empty;

            this.Series = new List<Serie>();

            ApplyFormula();
        }
    }
}
