using ctaCOMMON.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Indicator
{
    public class MACD : Chart_Indicator
    {
        public int Short_Term { get; private set; }
        public int Long_Term { get; private set; }
        public int Average_Term { get; private set; }
        public string Serie_Color { get; private set; }
        public string Zero_Serie_Color { get; private set; }
        public MACD(List<Candel> data_source, int short_term, int long_term, int average_term, string serie_color, string zero_serie_color): base(data_source)
        {
            this.Name = "MACD";
            this.In_Main_Chart = false;
            this.Short_Term = short_term;
            this.Long_Term = long_term;
            this.Average_Term = average_term;
            this.Serie_Color = serie_color;
            this.Zero_Serie_Color = zero_serie_color;

            ApplyFormula();
        }

        public MACD()
        {
            // TODO: Complete member initialization
        }

        public override void ApplyFormula()
        {
            this.Series = new List<Serie>();

            EMA emax = new EMA(base.Data_Source, this.Short_Term, "", false);
            EMA emay = new EMA(base.Data_Source, this.Long_Term, "", false);
            
            Serie serie = new Serie() { Color = this.Serie_Color, Serie_Type = SerieType.line, Column_Data_Label = "MACD", Column_Serie_ID = "macs" };
            Serie originalDataSource = Candel.GetDataSerie(base.Data_Source, DataSourceFieldUsed.Close, false);
            for (int i = 0; i < originalDataSource.Data.Count; i++)
            {
                serie.Data.Add(new SerieValue()
                {
                    Date = originalDataSource.Data[i].Date,
                    Value = (emax.Series[0].Data[i].Value - emay.Series[0].Data[i].Value),
                    Visible = originalDataSource.Data[i].Visible
                });
            }

            Serie average = MA.ApplyFormula(serie, this.Average_Term, this.Average_Serie_Color, SerieType.dashed);
            average.Column_Data_Label = "MA(" + this.Average_Term + ")";
            average.Column_Serie_ID = "maca";

            serie.Data = serie.Data.Where(x => x.Visible).ToList();
            
            Serie zero_serie = new Serie() { Color = this.Zero_Serie_Color, Serie_Type = SerieType.line, Column_Data_Label = "Zero", Column_Serie_ID = "macz" };
            foreach (var item in serie.Data)
            {
                zero_serie.Data.Add(new SerieValue() { Date = item.Date, Value = 0 });
            }
            
            this.Series.Add(serie);
            this.Series.Add(average);
            this.Series.Add(zero_serie);
        }

        public override void InitializeFromDataBaseValues(List<Candel> quotes, string param1, string color1, string param2, string color2, string param3, string color3, CandelRange candelRange)
        {
            int shortTerm = 0;
            int.TryParse(param1, out shortTerm);
            int longTerm = 0;
            int.TryParse(param2, out longTerm);
            int averageTerm = 0;
            int.TryParse(param3, out averageTerm);

            this.Name = "MACD";
            this.In_Main_Chart = false;
            this.Data_Source = quotes;
            this.Short_Term = shortTerm / base.GetCandelRangeRoundNormalizerValue(candelRange);
            this.Long_Term = longTerm / base.GetCandelRangeRoundNormalizerValue(candelRange);
            this.Average_Term = averageTerm / base.GetCandelRangeRoundNormalizerValue(candelRange);
            this.Serie_Color = (color1 != null) ? color1 : String.Empty;
            this.Zero_Serie_Color = (color2 != null) ? color2 : String.Empty;
            this.Average_Serie_Color = (color3 != null) ? color3 : String.Empty; 

            this.Series = new List<Serie>();

            ApplyFormula();
        }

        public string Average_Serie_Color { get; set; }
    }
}
