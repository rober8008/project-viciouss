using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Charts
{
    public class SymbolIntradiaryInfo
    {
        public double Ask {get; set;}
        public double AskSize {get; set;}
        public double Bid {get; set;}
        public double BidSize {get; set;}
        public double Change {get; set;}
        public double ChangePercent {get; set;}
        public double LastTradeSize {get; set;}
        public DateTime LastTradeDate {get; set;}
        public double LastTradePrice {get; set;}
        public string LastTradeTime {get; set;}
        public double Opening {get; set;}        
        public double PreviousClosing {get; set;}
        public int SymbolId {get; set;}
        public DateTime Date {get; set;}
        public double Volume { get; set; }
        public string DayRange 
        {
            get
            {
                return this.PreviousClosing + " - " + this.LastTradePrice;
            }
        }
        public double Maximun { get; set; }
        public double Minimun { get; set; }
    }
}
