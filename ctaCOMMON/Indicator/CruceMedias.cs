using ctaCOMMON.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Indicator
{
    public class CruceMedias : Chart_Indicator
    {
        public int ShortRounds { get; private set; }
        public int LongRounds { get; private set; }
        public string Serie_Color1 { get; private set; }
        public string Serie_Color2 { get; private set; }

        public CruceMedias(List<Candel> data_source, int shortRounds, int longRounds, string serie_color1, string serie_color2) : base(data_source)
        {
            this.Name = "CruceMedias";
            this.ShortRounds = shortRounds;
            this.LongRounds = longRounds;
            this.Serie_Color1 = serie_color1;
            this.Serie_Color2 = serie_color2;
            this.In_Main_Chart = true;

            ApplyFormula();
        }

        public CruceMedias()
        {
            // TODO: Complete member initialization
        }

        public override void InitializeFromDataBaseValues(List<Candel> quotes, string param1, string color1, string param2, string color2, string param3, string color3, CandelRange candelRange)
        {
            int shortTerm = 0;
            int.TryParse(param1, out shortTerm);
            int longTerm = 0;
            int.TryParse(param2, out longTerm);

            this.Name = "CMAS";
            this.In_Main_Chart = true;
            this.Data_Source = quotes;
            this.ShortRounds = shortTerm / base.GetCandelRangeRoundNormalizerValue(candelRange);
            this.LongRounds = longTerm / base.GetCandelRangeRoundNormalizerValue(candelRange);
            this.Serie_Color1 = (color1 != null) ? color1 : String.Empty;
            this.Serie_Color2 = (color2 != null) ? color2 : String.Empty;

            this.Series = new List<Serie>();

            ApplyFormula();
        }

        public override void ApplyFormula()
        {
            this.Series = new List<Serie>();

            Serie closeData = Candel.GetDataSerie(base.Data_Source, DataSourceFieldUsed.Close, false);

            Serie serieMA1 = MA.ApplyFormula(closeData, this.ShortRounds, this.Serie_Color1, SerieType.line);
            serieMA1.Column_Data_Label = string.Format("CM {0}", this.ShortRounds);
            serieMA1.Column_Serie_ID = "cma1";

            Serie serieMA2 = MA.ApplyFormula(closeData, this.LongRounds, this.Serie_Color2, SerieType.line);
            serieMA2.Column_Data_Label = string.Format("CM {0}", this.LongRounds);
            serieMA2.Column_Serie_ID = "cma2";

            this.Series.Add(serieMA1);
            this.Series.Add(serieMA2);
        }
    }
}
