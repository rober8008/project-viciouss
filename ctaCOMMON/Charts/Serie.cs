using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ctaCOMMON.Charts
{
    public class Serie
    {       
        public string Color { get; set; }
        public List<SerieValue> Data { get; set; }
        public SerieType Serie_Type { get; set; }
        public string Column_Serie_ID { get; set; }
        public string Column_Data_Label { get; set; }
        public string Orientation { get; set; }
        public bool IsTooltip { get; set; }
        public Serie(string column_serie_id = "value", string column_data_label = "Value")
        {
            this.Data = new List<SerieValue>();
            this.Column_Serie_ID = column_serie_id;
            this.Column_Data_Label = column_data_label;
        }

        public static object GetChartLineFormatedData(Serie data)
        {
            var result = new { cols = new Object[2], rows = new Object[data.Data.Count] };

            SetUpColumns(result.cols, data.Column_Serie_ID, data.Column_Data_Label);
            FillRowsData(result.rows, data.Data);

            return result;
        }

        private static void SetUpColumns(object[] cols, string column_serie_id, string column_data_label)
        {
            cols[0] = new { id = "round", label = "Round", type = "datetime" };
            cols[1] = new { id = column_serie_id, label = column_data_label, type = "number" };            
        }

        private static void FillRowsData(object[] rows, List<SerieValue> all_data)
        {
            object[] currentData = null;

            for (int i = 0; i < rows.Length; i++)
            {
                DateTime datetime = all_data[i].Date;
                string date = String.Format("{0} {1}, {2}", datetime.ToString("MMM", CultureInfo.InvariantCulture), datetime.Day, datetime.Year);

                string datestring = String.Format("Date({0},{1},{2},{3},{4})", datetime.Year, datetime.Month - 1, datetime.Day, datetime.Hour, datetime.Minute);

                currentData = new object[2];
                currentData[0] = new { v = datestring, f = date };
                currentData[1] = new { v = all_data[i].Value };
                rows[i] = new { c = currentData };
            }
        }        
    }
}
