using ctaCOMMON;
using ctaCOMMON.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaSERVICES
{
    public class SearchResultPortfolioUser : ISimplifiedSearchPortfolioUserResult
    {
        public int Portfolio_Id { get; set; }

        public string Portfolio_Name { get; set; }
    }
}
