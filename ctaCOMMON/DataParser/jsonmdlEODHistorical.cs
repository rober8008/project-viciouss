using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.DataParser
{
    public class jsonmdlEODHistorical
    {
        [JsonProperty("date")]
        public string strDate { get; set; }

        [JsonProperty("open")]
        //[JsonConverter(typeof(jsoncnvrtrNumericConverter))]
        public string strOpen { get; set; }

        [JsonProperty("high")]
        //[JsonConverter(typeof(jsoncnvrtrNumericConverter))]
        public string strHigh { get; set; }

        [JsonProperty("low")]
        //[JsonConverter(typeof(jsoncnvrtrNumericConverter))]
        public string strLow { get; set; }

        [JsonProperty("close")]
        //[JsonConverter(typeof(jsoncnvrtrNumericConverter))]
        public string strClose { get; set; }

        [JsonProperty("adjusted_close")]
        //[JsonConverter(typeof(jsoncnvrtrNumericConverter))]
        public string strAdjustedClose { get; set; }

        [JsonProperty("volume")]
        //[JsonConverter(typeof(jsoncnvrtrNumericConverter))]
        public string strVolume { get; set; }
        
        public DateTime Date
        {
            get
            {
                DateTime result = DateTime.Now;
                DateTime.TryParse(this.strDate, out result);
                return result;
            }
        }        
        public double Open
        {
            get
            {
                double result = 0;
                double.TryParse(this.strOpen, out result);
                return result;
            }
        }
        public double High
        {
            get
            {
                double result = 0;
                double.TryParse(this.strHigh, out result);
                return result;
            }
        }
        public double Low
        {
            get
            {
                double result = 0;
                double.TryParse(this.strLow, out result);
                return result;
            }
        }
        public double Close
        {
            get
            {
                double result = 0;
                double.TryParse(this.strClose, out result);
                return result;
            }
        }
        public double AdjustedClose
        {
            get
            {
                double result = 0;
                double.TryParse(this.strAdjustedClose, out result);
                return result;
            }
        }
        public decimal Volume
        {
            get
            {
                decimal result = 0;
                decimal.TryParse(this.strVolume, out result);
                return result;
            }
        }
    }
}
