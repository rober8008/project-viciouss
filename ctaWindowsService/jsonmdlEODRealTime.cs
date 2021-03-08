using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaWindowsService
{
    internal class jsonmdlEODRealTime
    {
        [JsonProperty("code")]
        public string strCode { get; set; }

        [JsonProperty("timestamp")]
        //[JsonConverter(typeof(jsoncnvrtrNumericConverter))]
        public string strTimestamp { get; set; }

        [JsonProperty("gmtoffset")]
        //[JsonConverter(typeof(jsoncnvrtrNumericConverter))]
        public string strGmtoffset { get; set; }

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

        [JsonProperty("volume")]
        //[JsonConverter(typeof(jsoncnvrtrNumericConverter))]
        public string strVolume { get; set; }

        [JsonProperty("previousClose")]
        //[JsonConverter(typeof(jsoncnvrtrNumericConverter))]
        public string strPreviousClose { get; set; }

        [JsonProperty("change")]
        //[JsonConverter(typeof(jsoncnvrtrNumericConverter))]
        public string strChange { get; set; }


        public string Code
        {
            get
            {
                return this.strCode;
            }
        }
        public long Timestamp
        {
            get
            {
                long result = 0;
                long.TryParse(this.strTimestamp, out result);
                return result;
            }
        }
        public long Gmtoffset
        {
            get
            {
                long result = 0;
                long.TryParse(this.strGmtoffset, out result);
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
        public long Volume
        {
            get
            {
                long result = 0;
                long.TryParse(this.strVolume, out result);
                return result;
            }
        }
        public double PreviousClose
        {
            get
            {
                double result = 0;
                double.TryParse(this.strPreviousClose, out result);
                return result;
            }
        }
        public double Change
        {
            get
            {
                double result = 0;
                double.TryParse(this.strChange, out result);
                return result;
            }
        }
    }
}
