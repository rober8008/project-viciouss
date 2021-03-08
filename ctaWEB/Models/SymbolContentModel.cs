using ctaCOMMON;
using ctaCOMMON.Charts;
using ctaCOMMON.Dashboard;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

/*
DATAVIEW
     Line Chart   Candel Chart
 0 -    date         date
 1 -    close        minimun
 2 -    closing      open
 3 -    volumen      close
 4 -    tooltip      maximun 
 5 -                 closing
 6 -                 volumen
 7 -                 tooltip
 */
namespace ctaWEB.Models
{
    public class SymbolContentModel
    {
        public int portfolio_id;
        public int symbol_id;
        public DateTime max_date;
                
        public SymbolContentModel(int portfolio_id, int symbol_id)
        {
            // TODO: Complete member initialization
            this.portfolio_id = portfolio_id;
            this.symbol_id = symbol_id;            
        }

        public SymbolDashboard Symbol_Dashboard { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="main_chart_type">Possible values (candel,line)</param>
        /// <returns></returns>
        public List<Serie> Series(string main_chart_type)
        {
            List<Serie> dataSeries = new List<Serie>();

            Serie serie;
            List<Serie> main_chart_tooltip = new List<Serie>();

            if (main_chart_type == "candel")
            {
                serie = Candel.GetDataSerie(this.Symbol_Dashboard.Symbol.Quotes, DataSourceFieldUsed.Minimun, true);
                serie.Column_Data_Label = "Min";
                serie.Column_Serie_ID = "minimun";
                dataSeries.Add(serie);

                serie = Candel.GetDataSerie(this.Symbol_Dashboard.Symbol.Quotes, DataSourceFieldUsed.Open, true);
                serie.Column_Data_Label = "Apertura";
                serie.Column_Serie_ID = "open";
                dataSeries.Add(serie);
            }

            serie = Candel.GetDataSerie(this.Symbol_Dashboard.Symbol.Quotes, DataSourceFieldUsed.Close, true);
            serie.Column_Data_Label = "Cierre";
            serie.Column_Serie_ID = "close";
            dataSeries.Add(serie);

            if (main_chart_type == "candel")
            {
                serie = Candel.GetDataSerie(this.Symbol_Dashboard.Symbol.Quotes, DataSourceFieldUsed.Maximun, true);
                serie.Column_Data_Label = "Máximo";
                serie.Column_Serie_ID = "maximun";
                dataSeries.Add(serie);
            }

            serie = Candel.GetDataSerie(this.Symbol_Dashboard.Symbol.Quotes, DataSourceFieldUsed.Close, true);                
            serie.Column_Data_Label = "Cierres";
            serie.Column_Serie_ID = "closing";
            dataSeries.Add(serie);

            serie = Candel.GetDataSerie(this.Symbol_Dashboard.Symbol.Quotes, DataSourceFieldUsed.Volume, true);
            double max_vol = serie.Data.Max(v => v.Value);
            serie.Column_Data_Label = "Volumen";
            serie.Column_Serie_ID = "volume";
            serie.Color = "blue";    
            dataSeries.Add(serie);

            foreach (var indicator in this.Symbol_Dashboard.Indicators)
            {
                if (!indicator.In_Main_Chart)
                {
                    serie = Candel.GetIndicatorTooltip(indicator);
                    dataSeries.Add(serie);
                }
                foreach (var indicator_serie in indicator.Series)
                {
                    dataSeries.Add(indicator_serie);
                    if (indicator.In_Main_Chart)
                    {
                        main_chart_tooltip.Add(indicator_serie);
                    }
                }                
            }

            foreach (var shape in this.Symbol_Dashboard.Shapes.Where(s => s.In_Main_Chart))
            {
                foreach (var shape_serie in shape.Series)
                {
                    main_chart_tooltip.Add(shape_serie);
                }
            }

            serie = Candel.GetDataSerie(this.Symbol_Dashboard.Symbol.Quotes, DataSourceFieldUsed.ToolTip, true, 0, main_chart_tooltip);
            dataSeries.Insert(0,serie);            

            List<DateTime> shape_dates = new List<DateTime>();
            foreach (var shape in this.Symbol_Dashboard.Shapes)
            {
                foreach (var shape_serie in shape.Series)
                {
                    dataSeries.Add(shape_serie);

                    foreach (var serie_value in shape_serie.Data)
                    {
                        if (!shape_dates.Contains(serie_value.Date))
                        {
                            shape_dates.Add(serie_value.Date);
                        }
                    }
                }
            }

            this.max_date = Symbol_Dashboard.Symbol.Quotes.Select(q => q.Date).Max();

            foreach (var date in shape_dates)
            {
                Symbol_Dashboard.Symbol.Quotes.Add(new Candel() { Date = date });
            }

            return dataSeries;                        
        }        

        public object[] Columns(List<Serie> data)
        {
            object[] columns = new object[data.Count + 2]; //Adding DateTime column at the end for hAxis label
            columns[0] = new { id = "rowindex", label = "rowindex", type = "number" };                    

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].IsTooltip)
                {
                    columns[i + 1] = new { type = "string", role = "tooltip", p = new { html = true } };
                }
                else
                {
                    columns[i + 1] = new { id = data[i].Column_Serie_ID, label = data[i].Column_Data_Label, type = "number" };
                }
            }
            columns[data.Count + 1] = new { id = "rounddate", label = "RoundDate", type = "datetime" };//Adding DateTime column at the end for hAxis label

            return columns;
        }

        public object[] Rows(List<Serie> data)
        {
            List<Candel> quotes = this.Symbol_Dashboard.Symbol.Quotes.Where(q => q.Visible).ToList<Candel>();
            object[] rows_data = new object[quotes.Count+1];

            int index = 0;
            object empty = null;
            object[] data_item;
            
            foreach (DateTime date in quotes.Select(q => q.Date))
            {
                data_item = new object[data.Count + 2];

                data_item[0] = new { v = index + 1 };                
                
                for (int i = 0; i < data.Count; i++)
                {
                    SerieValue current_value = data[i].Data.Where(d => d.Date == date).FirstOrDefault();
                    if (current_value != default(SerieValue))
                    {                       
                        if (current_value.Tooltip != null)
                        {
                            data_item[i + 1] = new { v = current_value.Tooltip };
                        }
                        else
                        {
                            data_item[i + 1] = new { v = current_value.Value };
                        }                        
                    }
                    else 
                    {
                        data_item[i + 1] = new { v = empty };
                    }                                        
                }

                string date_str = String.Format("{0} {1}, {2}", date.Date.Month, date.Date.Day, date.Date.Year);
                string datestring = String.Format("Date({0},{1},{2})", date.Date.Year, date.Date.Month - 1, date.Date.Day);
                data_item[data.Count + 1] = new { v = datestring, f = date_str };//RoundDate

                object current_data_object = new { c = data_item };
                rows_data[index] = current_data_object;
                index++;
            }
            
            //Dummy Candel
            DateTime max_date = data.Max(l => l.Data.Max(d => d.Date)).AddDays(1);
            data_item = new object[data.Count + 2];
            data_item[0] = new { v = index + 1 };
            for (int i = 0; i < data.Count; i++)
            {
                data_item[i + 1] = new { v = empty };
            }
            data_item[data.Count + 1] = new { v = String.Format("Date({0},{1},{2})", max_date.Date.Year, max_date.Date.Month - 1, max_date.Date.Day), f = String.Format("{0} {1}, {2}", max_date.Date.Month, max_date.Date.Day, max_date.Date.Year) };
            rows_data[index] = new { c = data_item };
            return rows_data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="main_chart_type">Possible values (candel,line)</param>
        /// <returns></returns>
        public object GetDataInJSONFormat(string main_chart_type)
        {
            //retorna en el formato de google charts todos los datos a mostrar
            List<Serie> dataSeries = this.Series(main_chart_type);            
            var result = new { cols = this.Columns(dataSeries), rows = this.Rows(dataSeries) };
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="main_chart_tpe">Possible values (candel,line)</param>
        /// <returns></returns>
        public object GetDataViewInfoForMainChartData(string main_chart_type)
        {
            List<int> dataview = new List<int>();
            dataview.Add(0);
            dataview.Add(1);
            dataview.Add(2);
            int counter = 5;
            if (main_chart_type == "candel")
            {
                dataview.Add(3);
                dataview.Add(4);
                dataview.Add(5);
                counter = 8;
            }
            
            for (int i = 0; i < this.Symbol_Dashboard.Indicators.Count; i++)
            {
                if (this.Symbol_Dashboard.Indicators[i].In_Main_Chart)
                {
                    for (int j = 0; j < this.Symbol_Dashboard.Indicators[i].Series.Count; j++)
                    {
                        dataview.Add(counter);
                        counter++;
                    }
                }
                else
                {
                    counter += this.Symbol_Dashboard.Indicators[i].Series.Count + 1;//tooltip serie
                }
            }

            for (int i = 0; i < this.Symbol_Dashboard.Shapes.Count; i++)
            {                
                for (int j = 0; j < this.Symbol_Dashboard.Shapes[i].Series.Count; j++)
                {
                    dataview.Add(counter);
                    counter++;
                }                                
            }
            //retorna la lista view de los datos que corresponden con la vista main: [0,1,2,4,7,8]
            //siempre retorna el cero de primero porque es el index de la ronda
            return dataview;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="main_chart_type">Possible values (candel,line)</param>
        /// <returns></returns>
        public object GetDataViewInfoForDateRangeSelector(string main_chart_type)
        {
            List<int> dataview = new List<int>();
            dataview.Add(0);

            if (main_chart_type == "candel")
            {
                dataview.Add(6);
                dataview.Add(7);                
            }
            else if (main_chart_type == "line")
            {
                dataview.Add(3);
                dataview.Add(4);
            }
            
            return dataview; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="main_chart_type">Possible values (candel,line)</param>
        /// <returns></returns>
        public object GetDataViewInfoForVolumeChart(string main_chart_type)
        {
            List<int> dataview = new List<int>();
            dataview.Add(0);

            if (main_chart_type == "candel")
            {                
                dataview.Add(7);
            }
            else if (main_chart_type == "line")
            {                
                dataview.Add(4);
            }

            return dataview; 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="main_chart_type">Possible Values (candel,line)</param>
        /// <returns></returns>
        public object GetDataSeriesInfoForMainChart(string main_chart_type)
        {
            Dictionary<string, object> series = new Dictionary<string, object>();
             
            object serie_info = new { type = "candlesticks", color = "#FFF", visibleInLegend = false };
            if (main_chart_type == "line")
            {
                serie_info = new { type = "line", color = "#0000FF", visibleInLegend = false };
            }
            series.Add("0", serie_info);
            //serie_info = new { type = "candlesticks", color = "#a52714", visibleInLegend = false };
            //series.Add("1", serie_info);
            int counter = 1;// 2;
            for (int i = 0; i < this.Symbol_Dashboard.Indicators.Count; i++)
            {
                if (this.Symbol_Dashboard.Indicators[i].In_Main_Chart)
                {
                    for (int j = 0; j < this.Symbol_Dashboard.Indicators[i].Series.Count; j++)
                    {
                        series.Add(counter.ToString(), new { type = Symbol_Dashboard.Indicators[i].Series[j].Serie_Type, color = Symbol_Dashboard.Indicators[i].Series[j].Color, visibleInLegend = false/*(j == 0)*/, lineWidth = 1 });
                        counter++;
                    }
                }
            }

            for (int i = 0; i < this.Symbol_Dashboard.Shapes.Count; i++)
            {
                for (int j = 0; j < this.Symbol_Dashboard.Shapes[i].Series.Count; j++)
                {
                    if (Symbol_Dashboard.Shapes[i].Series[j].Serie_Type == SerieType.dashed)
                    {
                        series.Add(counter.ToString(), new { type = Symbol_Dashboard.Shapes[i].Series[j].Serie_Type, color = Symbol_Dashboard.Shapes[i].Series[j].Color, visibleInLegend = false/*(j == 0)*/, lineWidth = 1, lineDashStyle = new List<int>() { 14, 2, 2, 7 } });
                    }
                    else
                    {
                        series.Add(counter.ToString(), new { type = Symbol_Dashboard.Shapes[i].Series[j].Serie_Type, color = Symbol_Dashboard.Shapes[i].Series[j].Color, visibleInLegend = false/*(j == 0)*/, lineWidth = 1 }); 
                    }
                    counter++;
                }                
            }

            var dynamicObject = new ExpandoObject() as IDictionary<string, Object>;
            foreach (var property in series)
            {
                dynamicObject.Add(property.Key, property.Value);
            }
            
            return dynamicObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="main_chart_type">Possible values (candel,line)</param>
        /// <returns></returns>
        public object[] GetDataInfoForChartIndicators(string main_chart_type)
        {
            object[] result = new object[this.Symbol_Dashboard.Indicators.Where(i => !i.In_Main_Chart).Count()];

            int index = 0;
            foreach (var indicator in this.Symbol_Dashboard.Indicators)
            {
                if(!indicator.In_Main_Chart)
                {
                    object indicatordata = new { id = Symbol_Dashboard.Symbol.Symbol_Name.Trim() + indicator.ID, series = this.GetDataSeriesByIndicatorID(indicator.ID), view = this.GetDataViewInfoByIndicatorID(indicator.ID, main_chart_type)};
                    result[index] = indicatordata;
                    index++;
                }
            }
            return result;
        }

        private object GetDataSeriesByIndicatorID(int indicator_id)
        {
            Dictionary<string, object> series = new Dictionary<string, object>();

            for (int i = 0; i < this.Symbol_Dashboard.Indicators.Count; i++)
            {
                if (this.Symbol_Dashboard.Indicators[i].ID == indicator_id)
                {
                    for (int j = 0; j < this.Symbol_Dashboard.Indicators[i].Series.Count; j++)
                    {
                        object serie_info = new { type = Symbol_Dashboard.Indicators[i].Series[j].Serie_Type.ToString(), color = Symbol_Dashboard.Indicators[i].Series[j].Color, lineWidth = 2, visibleInLegend = false };
                        if(Symbol_Dashboard.Indicators[i].Series[j].Serie_Type == SerieType.dashed)
                        {
                            serie_info = new { type = Symbol_Dashboard.Indicators[i].Series[j].Serie_Type.ToString(), color = Symbol_Dashboard.Indicators[i].Series[j].Color, lineDashStyle = new List<int>() { 14, 2, 2, 7 }, visibleInLegend = false };
                        }
                        series.Add(j.ToString(), serie_info);
                    }
                    break;
                }                
            }
            
            //retorna las series que conforman al indicador correspondiente al id
            var dynamicObject = new ExpandoObject() as IDictionary<string, Object>;
            foreach (var property in series)
            {
                dynamicObject.Add(property.Key, property.Value);
            }
            return dynamicObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indicator_id"></param>
        /// <param name="main_chart_type">Possible values (candel,line)</param>
        /// <returns></returns>
        private object GetDataViewInfoByIndicatorID(int indicator_id, string main_chart_type)
        {
            List<int> dataview = new List<int>();
            dataview.Add(0);
            int counter = 0;
            if (main_chart_type == "line")
            {
                counter = 5;
            }
            else if (main_chart_type == "candel")
            {
                counter = 8;
            }
            for (int i = 0; i < this.Symbol_Dashboard.Indicators.Count; i++)
            {
                if (this.Symbol_Dashboard.Indicators[i].ID == indicator_id)
                {
                    dataview.Add(counter);//Adding Tooltip column
                    for (int j = 0; j < this.Symbol_Dashboard.Indicators[i].Series.Count; j++)
                    {
                        counter++;
                        dataview.Add(counter);                        
                    }                    
                    break;
                }
                else
                {
                    int count_tooltip = (Symbol_Dashboard.Indicators[i].In_Main_Chart) ? 0 : 1;//Adding Tooltip column
                    counter += Symbol_Dashboard.Indicators[i].Series.Count + count_tooltip;
                }
            }
            //retorna la lista view de los datos que corresponden con la vista main: [0,1,2,4,7,8]
            //siempre retorna el cero de primero porque es el index de la ronda
            return dataview;
        }        
    }
}