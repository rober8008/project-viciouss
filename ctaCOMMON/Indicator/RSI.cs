using ctaCOMMON.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Indicator
{
    public class RSI: Chart_Indicator
    {
        public int Rounds { get; private set; }
        public double Oversold { get; private set; }
        public double Overbought { get; private set; }
        public string Serie_Color { get; private set; }
        public string Serie_Bounded_Lines_Color { get; private set; }

        public RSI(List<Candel> data_source, int rounds, double oversold, double overbought, string serie_color, string serie_bounded_lines_color): base(data_source)
        {
            this.Name = "RSI";
            this.Rounds = rounds;
            this.Oversold = oversold;
            this.Overbought = overbought;
            this.Serie_Bounded_Lines_Color = serie_bounded_lines_color;
            this.Serie_Color = serie_color;
            this.In_Main_Chart = false;
            this.Series = new List<Serie>();

            ApplyFormula();
        }

        public RSI()
        {
            // TODO: Complete member initialization
        }

        public override void ApplyFormula()
        {
            this.Series.Clear();

            Serie oversold = new Serie() { Color = this.Serie_Bounded_Lines_Color, Serie_Type = SerieType.line, Column_Data_Label = "Sobre Venta", Column_Serie_ID = "rsid" };
            Serie serie = new Serie() { Color = this.Serie_Color, Serie_Type = SerieType.line, Column_Data_Label = "RSI " + this.Rounds, Column_Serie_ID = "rsis" };
            Serie overbought = new Serie() { Color = this.Serie_Bounded_Lines_Color, Serie_Type = SerieType.line, Column_Data_Label = "Sobre Compra", Column_Serie_ID = "rsib" };

            //Serie originalDataSource = Candel.GetDataSerie(base.Data_Source, DataSourceFieldUsed.PercentVariation);
            Serie originalDataSource = Candel.GetDataSerie(base.Data_Source, DataSourceFieldUsed.Close, false);

            double[] values = new double[this.Rounds];
            double last_ups_average = 0;
            double last_downs_average = 0;
            double value = 0, up = 0, down = 0;
            for (int i = 1; i < originalDataSource.Data.Count; i++)
            {
                values[i % this.Rounds] = originalDataSource.Data[i].Value - originalDataSource.Data[i - 1].Value;

                //last_ups_average = ((values.Where(x => x > 0).Count() > 0) ? values.Where(x => x > 0).Average() : 0);
                //last_downs_average = ((values.Where(x => x < 0).Count() > 0) ? Math.Abs(values.Where(x => x < 0).Average()) : 100);

                if (i < this.Rounds)
                {

                    last_ups_average = ((values.Where(x => x > 0).Count() > 0) ? values.Where(x => x > 0).Sum() / this.Rounds : 0);
                    last_downs_average = ((values.Where(x => x < 0).Count() > 0) ? Math.Abs(values.Where(x => x < 0).Sum() / this.Rounds) : 0);
                }
                else
                {
                    up = values[i % this.Rounds] > 0 ? values[i % this.Rounds] : 0;
                    down = values[i % this.Rounds] < 0 ? Math.Abs(values[i % this.Rounds]) : 0;

                    last_ups_average = (last_ups_average * (this.Rounds - 1) + up) / this.Rounds;
                    last_downs_average = (last_downs_average * (this.Rounds - 1) + down) / this.Rounds;
                }

                value = last_downs_average == 0 ? 100 : 100 - (100 / (1 + (last_ups_average/last_downs_average)));

                if(originalDataSource.Data[i].Visible)
                    serie.Data.Add(new SerieValue() { Date = originalDataSource.Data[i].Date, Value = value });
            }

            foreach (SerieValue item in serie.Data)
            {
                oversold.Data.Add(new SerieValue() { Date = item.Date, Value = this.Oversold });
                overbought.Data.Add(new SerieValue() { Date = item.Date, Value = this.Overbought });
            }

            this.Series.Add(serie);
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

            this.Name = "RSI";
            this.In_Main_Chart = false;
            this.Data_Source = quotes;
            this.Rounds = rounds / base.GetCandelRangeRoundNormalizerValue(candelRange);
            this.Oversold = overSold;
            this.Overbought = overBought;
            this.Serie_Color = (color1 != null) ? color1 : String.Empty;
            this.Serie_Bounded_Lines_Color = (color2 != null) ? color2 : String.Empty;

            this.Series = new List<Serie>();

            ApplyFormula();
        }
    }
}
