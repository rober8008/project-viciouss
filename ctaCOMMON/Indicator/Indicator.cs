using ctaCOMMON.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Indicator
{
    public abstract class Indicator
    {
        public int ID { get; set; }        
        public bool In_Main_Chart { get; protected set; }
        public abstract void ApplyFormula();
        public List<Serie> Series { get; protected set; }
        public string Name { get; set; }

        public string DisplayValue
        {
            get
            {
                switch (this.Name)
                {
                    case "RSI":
                        return this.GetDisplayValueSingle("rsis");
                    case "MACD":
                        return this.GetDisplayValueWithAvg("macs", "maca");
                    case "Stochastic Oscillator Fast":
                        return this.GetDisplayValueWithAvg("osse", "osav");
                    case "Stochastic Oscillator Slow":
                        return this.GetDisplayValueWithAvg("osse", "osav");
                    case "Momentum":
                        return this.GetDisplayValueSingle("moms");
                    case "ROC":
                        return this.GetDisplayValueSingle("rocs");
                    case "Williams R":
                        return this.GetDisplayValueSingle("wrse");
                    case "OBV":
                        return this.GetDisplayValueSingle("obv");
                    case "VOSC":
                        return this.GetDisplayValueSingle("vosc");
                    case "Coeficiente Correlacion":
                        return this.GetDisplayValueSingle("corr");
                    default:
                        return "";
                }
            }
        }

        private string GetDisplayValueSingle(string columnSerieId)
        {
            var serie = Series.Where(x => x.Column_Serie_ID == columnSerieId).First();
            var element = serie.Data[serie.Data.Count - 1];
            return serie.Column_Data_Label + ": " + Math.Round(element.Value, 3);
        }

        private string GetDisplayValueWithAvg(string columnSerieId, string columnAvgId)
        {
            var serie = Series.Where(x => x.Column_Serie_ID == columnSerieId).First();
            var serieAV = Series.Where(x => x.Column_Serie_ID == columnAvgId).First();
            var element = serie.Data[serie.Data.Count - 1];
            var elementAV = serieAV.Data[serieAV.Data.Count - 1];
            return serie.Column_Data_Label + ": " + Math.Round(element.Value, 3) + " " +
                   serieAV.Column_Data_Label + ": " + Math.Round(elementAV.Value, 3);
        }
    }
}
