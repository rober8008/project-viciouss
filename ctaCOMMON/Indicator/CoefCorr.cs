using ctaCOMMON.Charts;
using ctaDATAMODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Indicator
{
    public class CoefCorr : Chart_Indicator
    {
        public int Rounds { get; private set; }

        public string Serie_Color { get; private set; }

        public string SecondaryIndex { get; private set; }

        public override void ApplyFormula()
        {
            this.Series = new List<Serie>();

            Serie DataSource = Candel.GetDataSerie(base.Data_Source, DataSourceFieldUsed.Close, false);

            //get the other quotes
            Serie indexDataSource = Candel.GetDataSerie(this.GetCandelsBySymbol(this.SecondaryIndex), DataSourceFieldUsed.Close, false);

            //get al the values that had data for the index for the same date
            var filterDataSource = DataSource.Data.Where(x => indexDataSource.Data.Select(y => y.Date).Contains(x.Date)).ToList();
            var filterIndexDataSource = indexDataSource.Data.Where(x => DataSource.Data.Select(y => y.Date).Contains(x.Date)).ToList();

            Serie corr = new Serie() { Color = this.Serie_Color, Serie_Type = SerieType.line, Column_Serie_ID = "corr" };
            corr.Column_Data_Label = "Coeficiente Correlacion " + this.SecondaryIndex;

            double[] values1 = new double[this.Rounds];
            double[] values2 = new double[this.Rounds];
            double desvest1 = 0, desvest2 = 0;
            double values_average1 = 0, values_average2 = 0;
            SerieValue current = null;

            for (int i = 0; i < filterDataSource.Count(); i++)
            {
                current = filterDataSource[i];
                values1[i % this.Rounds] = current.Value;
                values2[i % this.Rounds] = filterIndexDataSource[i].Value;

                if (current.Date != filterIndexDataSource[i].Date) throw new InvalidOperationException();

                values_average1 = values1.Where(x => x > 0).Average();
                values_average2 = values2.Where(x => x > 0).Average();

                if (i >= this.Rounds - 1)
                {
                    //desvest 1
                    var pows = values1.Select(x => Math.Pow(x - values_average1, 2));
                    var sum = pows.Sum() / (this.Rounds - 1);
                    desvest1 = Math.Sqrt(sum);

                    //desvest 2
                    var pows2 = values2.Select(x => Math.Pow(x - values_average2, 2));
                    var sum2 = pows2.Sum() / (this.Rounds - 1);
                    desvest2 = Math.Sqrt(sum2);

                    //cov
                    var cov1 = values1.Select(x => x - values_average1);
                    var cov2 = values2.Select(x => x - values_average2);
                    var prodList = cov1.Zip(cov2, (x, y) => x * y).ToList();
                    var cov = prodList.Sum() / this.Rounds;

                    //corr
                    if(filterDataSource[i].Visible)
                        corr.Data.Add(new SerieValue() { Date = current.Date, Value = cov / (desvest1 * desvest2) });
                }
            }

            this.Series.Add(corr);
        }

        private List<Candel> GetCandelsBySymbol(string symbol)
        {
            var result = new List<Candel>();

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();

                var stock_entity = entities.Stocks.Where(s => s.symbol == symbol).First();

                result = stock_entity.Stock_Quote.Select(x => new Candel()
                {
                    Date = x.date_round,
                    Open = x.opening,
                    Close = x.closing,
                    Minimun = x.minimun,
                    Maximun = x.maximun,
                    Volume = (double)x.volume
                })
                .OrderBy(y => y.Date).ToList();

                entities.Database.Connection.Close();
            }

            return result;
        }

        public override void InitializeFromDataBaseValues(List<Candel> quotes, string param1, string color1, string param2, string color2, string param3, string color3, CandelRange candelRange)
        {
            this.Name = "Coeficiente Correlacion";
            this.In_Main_Chart = false;
            int rounds = 0;
            int.TryParse(param1, out rounds);

            this.Data_Source = quotes;
            this.Rounds = rounds / base.GetCandelRangeRoundNormalizerValue(candelRange);
            this.Serie_Color = (color1 != null) ? color1 : String.Empty;

            this.SecondaryIndex = param2;

            this.Series = new List<Serie>();

            ApplyFormula();
        }
    }
}
