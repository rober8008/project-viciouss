using ctaCOMMON.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Indicator
{
    public class OBV : Chart_Indicator
    {
        public string Serie_Color { get; private set; }
        public OBV(List<Candel> data_source, string serie_color): base(data_source)
        {
            this.Name = "OBV";
            this.Serie_Color = serie_color;
            this.In_Main_Chart = false;

            ApplyFormula();
        }

        public OBV()
        {
            // TODO: Complete member initialization
        }

        public override void ApplyFormula()
        {
            this.Series = new List<Serie>();

            Serie serie = Candel.GetDataSerie(base.Data_Source, DataSourceFieldUsed.OBV, false);

            serie.Color = this.Serie_Color;
            serie.Serie_Type = SerieType.line;
            serie.Column_Data_Label = "On Balance Volume";
            serie.Column_Serie_ID = "obv";

            serie.Data = serie.Data.Where(x => x.Visible).ToList();
            this.Series.Add(serie);
        }

        public override void InitializeFromDataBaseValues(List<Candel> quotes, string param1, string color1, string param2, string color2, string param3, string color3, CandelRange candelRange)
        {
            this.In_Main_Chart = false;

            this.Name = "OBV";
            this.Data_Source = quotes;
            this.Serie_Color = (color1 != null) ? color1 : String.Empty;

            this.Series = new List<Serie>();

            ApplyFormula();
        }
    }
}
