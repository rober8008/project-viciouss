using ctaCOMMON.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Indicator
{
    public class StochasticOscillatorSlow : Chart_Indicator
    {
        public int Rounds { get; private set; }
        public int Oversold { get; private set; }
        public int Overbought { get; private set; }
        public string Serie_Color { get; private set; }
        public string Serie_Bounded_Lines_Color { get; private set; }
        public string Serie_Average_Color { get; private set; }
        
        public StochasticOscillatorSlow(List<Candel> data_source, int rounds, int oversold, int overbought, string serie_color, string serie_bounded_lines_color, string serie_average_color): base(data_source)
        {
            this.Name = "Stochastic Oscillator Slow";
            this.In_Main_Chart = false;
            this.Rounds = rounds;
            this.Oversold = oversold;
            this.Overbought = overbought;
            this.Serie_Bounded_Lines_Color = serie_bounded_lines_color;
            this.Serie_Color = serie_color;
            this.Serie_Average_Color = serie_average_color;

            this.Series = new List<Serie>();

            ApplyFormula();
        }

        public StochasticOscillatorSlow()
        {
            // TODO: Complete member initialization
        }
        public override void ApplyFormula()
        {
            this.Series = new List<Serie>();

            Serie serie = MA.ApplyFormula(Candel.GetDataSerie(base.Data_Source, DataSourceFieldUsed.Stochastic, false, this.Rounds), 3, this.Serie_Color, SerieType.line, false);
            serie.Column_Data_Label = "SO Slow";
            serie.Column_Serie_ID = "osse";

            Serie average = MA.ApplyFormula(serie, 3, this.Serie_Average_Color, SerieType.dashed);
            average.Column_Data_Label = "MA(" + this.Rounds + ")";
            average.Column_Serie_ID = "osav";

            serie.Data = serie.Data.Where(x => x.Visible).ToList();
           
            DateTime min_date = base.Data_Source.Select(x => x.Date).Min();
            DateTime max_date = base.Data_Source.Select(x => x.Date).Max();
            Serie oversold = new Serie() { Color = this.Serie_Bounded_Lines_Color, Serie_Type = SerieType.line, Column_Data_Label = "Sobre Venta", Column_Serie_ID = "osos" };
            Serie overbought = new Serie() { Color = this.Serie_Bounded_Lines_Color, Serie_Type = SerieType.line, Column_Data_Label = "Sobre Compra", Column_Serie_ID = "osob" };
            foreach (SerieValue serie_item in serie.Data)
            {
                oversold.Data.Add(new SerieValue() { Date = serie_item.Date, Value = this.Oversold });
                overbought.Data.Add(new SerieValue() { Date = serie_item.Date, Value = this.Overbought });
            }

            this.Series.Add(serie);
            this.Series.Add(average);
            this.Series.Add(oversold);
            this.Series.Add(overbought);
        }

        public override void InitializeFromDataBaseValues(List<Candel> quotes, string param1, string color1, string param2, string color2, string param3, string color3, CandelRange candelRange)
        {
            int rounds = 0;
            int.TryParse(param1, out rounds);
            int overSold = 0;
            int.TryParse(param2, out overSold);
            int overBought = 0;
            int.TryParse(param3, out overBought);

            this.Name = "Stochastic Oscillator Slow";
            this.In_Main_Chart = false;
            this.Data_Source = quotes;
            this.Rounds = rounds / base.GetCandelRangeRoundNormalizerValue(candelRange);
            this.Oversold = overSold;
            this.Overbought = overBought;
            this.Serie_Color = (color1 != null) ? color1 : String.Empty;
            this.Serie_Bounded_Lines_Color = (color2 != null) ? color2 : String.Empty;
            this.Serie_Average_Color = (color3 != null) ? color3 : String.Empty;

            ApplyFormula();
        }
    }
}
