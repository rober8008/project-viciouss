using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON
{
    public class SimplifiedSymbolQuotes
    {
        public string Symbol { get; set; }
        public double PercentVariation { get; set; }
        public int Variation { get; set; }
        public double LastTradePrize { get; set; }
        public double CurrentAskPrice { get; set; }

        public double Minimum { get; set; }
        public double Maximum { get; set; }
        public double Opening { get; set; }
        public double VariationValue { get; set; }
        public int Symbol_ID { get; set; }

        public static SimplifiedSymbolQuotesEqualityComparer ComparerBySymbolId = new SimplifiedSymbolQuotesEqualityComparer();
    }

    public class SimplifiedSymbolQuotesEqualityComparer : IEqualityComparer<SimplifiedSymbolQuotes>
    {
        public bool Equals(SimplifiedSymbolQuotes b1, SimplifiedSymbolQuotes b2)
        {
            if (b2 == null && b1 == null)
                return true;
            else if (b1 == null || b2 == null)
                return false;
            return (b1.Symbol_ID == b2.Symbol_ID);                 
        }

        public int GetHashCode(SimplifiedSymbolQuotes bx)
        {
            return base.GetHashCode();
        }
    }
}
