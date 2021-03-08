using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ctaCOMMON.Indicator;

namespace ctaCOMMON.Charts
{
    public class Candel
    {
        public DateTime Date { get; set; }
        public double Minimun { get; set; }
        public double Maximun { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }
        public bool Visible { get; set; }        

        public static Serie GetDataSerie(List<Candel> quotes, DataSourceFieldUsed data_name, bool filterCandels, int rounds = 0, List<Serie> series = null)
        {
            if(filterCandels)
                quotes = quotes.Where(q => q.Visible).ToList<Candel>();

            Serie serie = new Serie();
            switch (data_name)
            {
                case DataSourceFieldUsed.Close:
                    foreach (var quote in quotes.Where(q => q.Close > 0))
                    {
                        serie.Data.Add(new SerieValue() { Date = quote.Date, Value = quote.Close, Visible = quote.Visible });
                    }
                    break;
                case DataSourceFieldUsed.Open:
                    foreach (var quote in quotes)
                    {
                        serie.Data.Add(new SerieValue() { Date = quote.Date, Value = quote.Open });
                    }
                    break;
                case DataSourceFieldUsed.Minimun:
                    foreach (var quote in quotes)
                    {
                        serie.Data.Add(new SerieValue() { Date = quote.Date, Value = quote.Minimun });
                    }
                    break;
                case DataSourceFieldUsed.Maximun:
                    foreach (var quote in quotes)
                    {
                        serie.Data.Add(new SerieValue() { Date = quote.Date, Value = quote.Maximun });
                    }
                    break;
                case DataSourceFieldUsed.PriceVariation:
                    foreach (var quote in quotes)
                    {
                        serie.Data.Add(new SerieValue() { Date = quote.Date, Value = (quote.Close - quote.Open) });
                    }
                    break;
                case DataSourceFieldUsed.PercentVariation:
                    foreach (var quote in quotes)
                    {
                        serie.Data.Add(new SerieValue() { Date = quote.Date, Value = (100 * (quote.Close - quote.Open)) / quote.Open });
                    }
                    break;
                case DataSourceFieldUsed.Stochastic:
                    for(int i = 0; i < quotes.Count; i++)
                    {
                        int count = (i >= rounds) ? rounds : i + 1;
                        double min = quotes.GetRange(Math.Max(0, i - (rounds - 1)), count).Min(c => c.Minimun);
                        double max = quotes.GetRange(Math.Max(0, i - (rounds - 1)), count).Max(c => c.Maximun);

                        if (max != min) //this only happens with the first value
                        {
                            serie.Data.Add(new SerieValue() { Date = quotes[i].Date, Value = 100 * ((quotes[i].Close - min) / (max - min)), Visible = quotes[i].Visible });
                        }
                    }
                    break;
                case DataSourceFieldUsed.ToolTip:
                    foreach (var quote in quotes)
                    {
                        string datestr = quote.Date.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).Substring(0,3).ToUpper() + " " + quote.Date.Day + ", " + quote.Date.Year;
                        string border_color = "blue";
                        if(quote.Open > quote.Close)
                            border_color = "#a52714";
                        else if(quote.Open < quote.Close)
                            border_color = "#0f9d58";
                        string tooltip = "<div style='padding:5px;width:100%;border: 3px solid " + border_color + ";'>";
                        tooltip += "<strong>" + datestr + "</strong>";
                        tooltip += "<br/><span style='font-size:10px;'><strong>Ape: </strong>" + quote.Open.ToString("0.00") + "</span>  <span style='font-size:10px;'><strong>Cie: </strong>" + quote.Close.ToString("0.00") + "</span>";
                        tooltip += "<br/><span style='font-size:10px;'><strong>Mín: </strong>" + quote.Minimun.ToString("0.00") + "</span>  <span style='font-size:10px;'><strong>Máx: </strong>" + quote.Maximun.ToString("0.00") + "</span>";
                        tooltip += "<br/><span style='font-size:10px;'><strong>Vol: </strong>" + quote.Volume.ToString("0.##") + "</span>";
                        if (series != null)
                        {
                            foreach (var dserie in series)
                            {
                                SerieValue serie_val = dserie.Data.Where(s => s.Date.Year == quote.Date.Year && s.Date.Month == quote.Date.Month && s.Date.Day == quote.Date.Day).FirstOrDefault();
                                if (serie_val != default(SerieValue))
                                {
                                    tooltip += "<br/><span style='font-size:10px;color:" + dserie.Color + "';>" + dserie.Column_Data_Label  + ": " + serie_val.Value.ToString("0.00") + "</span>";
                                }
                            } 
                        }
                        tooltip += "</div>";
                        serie.Data.Add(new SerieValue() { Date = quote.Date, Tooltip = tooltip });                      
                    }
                    serie.IsTooltip = true;
                    break;
                case DataSourceFieldUsed.WilliansR:
                    for (int i = 0; i < quotes.Count; i++)
                    {
                        int count = (i >= rounds) ? rounds : i + 1;
                        double min = quotes.GetRange(Math.Max(0, i - (rounds - 1)), count).Min(c => c.Minimun);
                        double max = quotes.GetRange(Math.Max(0, i - (rounds - 1)), count).Max(c => c.Maximun);

                        if (max != min) //this only happens with the first value
                        {
                            serie.Data.Add(new SerieValue()
                            {
                                Date = quotes[i].Date,
                                Value = ((max - quotes[i].Close) / (max - min)) * -100,
                                Visible = quotes[i].Visible
                            });
                        }
                    }
                    break;
                case DataSourceFieldUsed.OBV:
                    serie.Data.Add(new SerieValue() { Date = quotes[0].Date, Value = quotes[0].Volume, Visible = quotes[0].Visible });
                    double anterior, hoy, value;
                    for (int i = 1; i < quotes.Count; i++)
                    {
                        anterior = quotes[i - 1].Close;
                        hoy = quotes[i].Close;

                        if (hoy < anterior)
                        {
                            value = serie.Data[i - 1].Value - quotes[i].Volume;
                        }
                        else if (hoy > anterior)
                        {
                            value = serie.Data[i - 1].Value + quotes[i].Volume;
                        }
                        else
                            value = serie.Data[i - 1].Value;

                        serie.Data.Add(new SerieValue()
                        {
                            Date = quotes[i].Date,
                            Value = value,
                            Visible = quotes[i].Visible
                        });
                    }
                    break;
                default:
                    foreach (var quote in quotes)
                    {
                        serie.Data.Add(new SerieValue() { Date = quote.Date, Value = quote.Volume, Visible = quote.Visible });
                    }
                    break;
            }
            serie.Data.Sort((x, y) => x.Date.CompareTo(y.Date));
            return serie;
        }

        public static Serie GetIndicatorTooltip(Chart_Indicator indicator)
        {
            Serie serie = new Serie();
            for (int j = 0; j < indicator.Series[0].Data.Count; j++)
            {
                string datestr = indicator.Series[0].Data[j].Date.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).Substring(0, 3).ToUpper() + " " + indicator.Series[0].Data[j].Date.Day + ", " + indicator.Series[0].Data[j].Date.Year;
                string border_color = "#797979";                
                string tooltip_content = "";
                string tooltip = "<div style='padding:5px;width:100%;border: 3px solid !!border_color!!;'><strong>!!datestr!!</strong>!!content!!</div>";
                
                for (int i = 0; i < indicator.Series.Count; i++)
                {
                    Serie current_serie = indicator.Series[i];
                    SerieValue current_serievalue = indicator.Series[i].Data[j];
                    tooltip_content += "<br/><span style='font-size:10px;color:" + current_serie.Color + ";'>" + current_serie.Column_Data_Label + ": " + "</span><span><strong>" + current_serievalue.Value.ToString("0.00") + "</strong></span>";
                }

                tooltip = tooltip.Replace("!!datestr!!", datestr).Replace("!!border_color!!", border_color).Replace("!!content!!", tooltip_content);
                serie.Data.Add(new SerieValue() { Date = indicator.Series[0].Data[j].Date, Tooltip = tooltip });
            }
            serie.IsTooltip = true;
            return serie;
        }

        public static List<Candel> GetRisingQuotes(List<Candel> quotes)
        {
            List<Candel> result = new List<Candel>();

            foreach (var quote in quotes)
            {
                if (quote.Close >= quote.Open)
                {
                    result.Add(quote);
                }
            }

            return result;
        }

        public static List<Candel> GetFallingQuotes(List<Candel> quotes)
        {
            List<Candel> result = new List<Candel>();

            foreach (var quote in quotes)
            {
                if (quote.Close < quote.Open)
                {
                    result.Add(quote);
                }
            }

            return result;
        }        
    }
}
