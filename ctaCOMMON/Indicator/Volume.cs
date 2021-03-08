using ctaCOMMON.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Indicator
{
    public class Volume: Chart_Indicator
    {
        
        public string Serie_Color { get; private set; }
        public Volume(List<Candel> data_source, int rounds, string serie_color): base(data_source)
        {
            this.Serie_Color = serie_color;
            this.In_Main_Chart = false;            
            ApplyFormula();
        }

        public Volume()
        {
            // TODO: Complete member initialization
        }        
        public override void ApplyFormula()
        {
            this.Series = new List<Serie>();

            Serie originalDataSource = Candel.GetDataSerie(base.Data_Source, DataSourceFieldUsed.Volume, false);

            Serie serie = new Serie() { Color = this.Serie_Color, Serie_Type = SerieType.bars, Column_Data_Label = "Volumen", Column_Serie_ID = "volu", Orientation = "vertical" };
            
            for (int i = 0; i < originalDataSource.Data.Count; i++)
            {
                if(originalDataSource.Data[i].Visible)
                    serie.Data.Add(new SerieValue() { Date = originalDataSource.Data[i].Date, Value = originalDataSource.Data[i].Value });
            }
            this.Series.Add(serie);
        }

        public override void InitializeFromDataBaseValues(List<Candel> quotes, string param1, string color1, string param2, string color2, string param3, string color3, CandelRange candelRange)
        {
            this.In_Main_Chart = false;
                      
            this.Data_Source = quotes;
            this.Serie_Color = (color1 != null) ? color1 : String.Empty;
            this.Series = new List<Serie>();

            ApplyFormula();
        }
    }
}
