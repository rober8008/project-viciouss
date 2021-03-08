using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaWindowsService
{
    internal class Configs_QuoteDataProvider
    {
        public string Name { get; set; }
        public string APIToken { get; set; }
        public string RealTimeURL { get; set; }
        public string HistoricalURL { get; set; }
        public string Username { get; set; }
        public string LoginURL { get; set; }
        public string IndexURL { get; set; }
    }
}
