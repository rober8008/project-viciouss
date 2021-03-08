using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON
{
    public class IntradiaryData
    {
        /// <summary>
        /// 
        /// </summary>
        /// /// <param name="s">Symbol</param>
        /// <param name="o">Open</param>
        /// <param name="p">Previous Closure</param>
        /// <param name="a">Ask</param>
        /// <param name="a5">Ask Size</param>
        /// <param name="b">Bid</param>
        /// <param name="b6">Bid Size</param>
        /// <param name="c1">Change</param>
        /// <param name="p2">Change Percent</param>
        /// <param name="d1">Last Trade Date</param>
        /// <param name="t1">Last Trade Time</param>
        /// <param name="l1">Last Trade Price</param>
        /// <param name="k3">Last Trade Size</param>
        public IntradiaryData(string s, string o, string p, string a, string a5, string b, string b6, string c1, string p2, string d1, string t1, string l1, string k3, string v, string m)
        {
            this.Symbol = s.Replace("\"","");

            double value;

            double.TryParse(o, out value);
            this.Open = value;

            double.TryParse(p, out value);
            this.PreviousClose = value;

            double.TryParse(a, out value);
            this.Ask = value;

            double.TryParse(a5, out value);
            this.AskSize = value;

            double.TryParse(b, out value);
            this.Bid = value;

            double.TryParse(b6, out value);
            this.BidSize = value;

            double.TryParse(c1, out value);
            this.Change = value;

            double.TryParse(p2.Replace("\"","").Replace("%",""), out value);
            this.ChangePercent = value;

            DateTime dt;
            DateTime.TryParse(d1, out dt);
            this.LastTradeDate = dt;

            this.LastTradeTime = t1.Replace("\"",""); 

            double.TryParse(l1, out value);
            this.LastTradePrice = value;

            double.TryParse(k3, out value);
            this.LastTradeSize = (decimal)value;

            double.TryParse(v, out value);
            this.Volume = (decimal)value;

            double.TryParse(m, out value);
            this.DayRange = (decimal)value;
        }
        public string Symbol { get; set; }
        public double Open { get; set; }
        public double PreviousClose { get; set; }
        public double Ask { get; set; }
        public double AskSize { get; set; }
        public double Bid { get; set; }
        public double BidSize { get; set; }
        public double Change { get; set; }
        public double ChangePercent { get; set; }
        public DateTime LastTradeDate { get; set; }
        public string LastTradeTime { get; set; }
        public double LastTradePrice { get; set; }
        public decimal LastTradeSize { get; set; }
        public decimal Volume { get; set; }
        public decimal DayRange { get; set; }
    }
}
