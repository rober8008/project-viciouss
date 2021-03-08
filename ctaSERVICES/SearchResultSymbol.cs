using ctaCOMMON;
using ctaCOMMON.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ctaSERVICES
{
    public class SearchResultSymbol : ISimplifiedSearchSymbolResult
    {
        public int Symbol_Id { get; set; }
        public string Symbol_Name { get; set; }
        public int Market_Id { get; set; }
        public string Market_Name { get; set; }
    }
}
