using ctaCOMMON.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Indicator
{
    public class EMA : Chart_Indicator
    {
        public int Rounds { get; private set; }
        public string Serie_Color { get; private set; }
        public bool Filter { get; private set; }

        public EMA(List<Candel> data_source, int rounds, string serie_color, bool filter): base(data_source)
        {
            this.Name = "EMA";
            this.Rounds = rounds;
            this.Serie_Color = serie_color;
            this.In_Main_Chart = true;
            this.Filter = filter;

            ApplyFormula();
        }

        public EMA()
        {
            // TODO: Complete member initialization
        }

        public override void ApplyFormula()
        {
            this.Series = new List<Serie>();

            Serie originalDataSource = Candel.GetDataSerie(base.Data_Source, DataSourceFieldUsed.Close, false);

            //double K = 2 / (this.Rounds + 1);

            decimal Kdecimal = Decimal.Divide(2, this.Rounds + 1);
            double K = Double.Parse((Math.Truncate(100000000 * Kdecimal) / 100000000).ToString()); //set to 8 decimal places
            
            Serie serie = new Serie() { Color = this.Serie_Color, Serie_Type = SerieType.line, Column_Data_Label = "EMA " + this.Rounds, Column_Serie_ID = "emav" };
            double[] initialMAs = new double[this.Rounds];
            double value = 0;
            //double previousSerieValue = 0;
            for (int i = 0; i < originalDataSource.Data.Count; i++)
            {
                if (i < this.Rounds)
                {
                    initialMAs[i % this.Rounds] = originalDataSource.Data[i].Value;
                    serie.Data.Add(new SerieValue() { Date = originalDataSource.Data[i].Date, Value = initialMAs.Where(x => x > 0).Average() });
                }
                else
                {
                    value = (originalDataSource.Data[i].Value * K) + (serie.Data[i - 1].Value * (1 - K));
                    serie.Data.Add(new SerieValue() { Date = originalDataSource.Data[i].Date, Value = value, Visible = originalDataSource.Data[i].Visible });
                }
            }

            if(this.Filter)
                serie.Data = serie.Data.Where(x => x.Visible).ToList();

            this.Series.Add(serie);
        }        

        public override void InitializeFromDataBaseValues(List<Candel> quotes, string param1, string color1, string param2, string color2, string param3, string color3, CandelRange candelRange)
        {
            this.In_Main_Chart = true;
            int rounds = 0;
            int.TryParse(param1, out rounds);

            this.Name = "EMA";
            this.Data_Source = quotes;
            this.Rounds = rounds / base.GetCandelRangeRoundNormalizerValue(candelRange);
            this.Serie_Color = (color1 != null) ? color1 : String.Empty;

            this.Series = new List<Serie>();
            this.Filter = true;

            ApplyFormula();
        }
    }
}
