using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Charts
{
    public class Symbol
    {
        public List<Candel> Quotes { get; set; }        
        public int Portfolio_ID { get; set; }
        public int Symbol_ID { get; set; }
        public string Symbol_Name { get; set; }
        public int Symbol_Market_ID { get; set; }
        public string Symbol_Market { get; set; }        
        public SymbolIntradiaryInfo Intradiary_Info { get; set; }
        public string Symbol_Company_Name { get; set; }
        public string Description { get; set; }        
    }    
}
