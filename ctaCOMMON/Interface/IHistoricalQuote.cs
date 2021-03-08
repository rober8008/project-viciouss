using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Interface
{
    public interface IHistoricalQuote
    {
        int Id { get; set; }
        int stock_id { get; set; }
        double opening { get; set; }
        double closing { get; set; }
        double minimun { get; set; }
        double maximun { get; set; }
        decimal volume { get; set; }
        DateTime date_round { get; set; }
        double adj_close { get; set; }
    }
}
