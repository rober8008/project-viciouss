using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Interface
{
    public interface ISimplifiedSearchSymbolResult
    {
        int Symbol_Id { get; set; }
        string Symbol_Name { get; set; }
        int Market_Id { get; set; }
        string Market_Name { get; set; }
    }
}
