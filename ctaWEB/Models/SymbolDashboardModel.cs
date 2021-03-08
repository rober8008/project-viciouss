using ctaCOMMON;
using ctaSERVICES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ctaWEB.Models
{
    public class SymbolDashboardModel
    {
        public int symbol_id { get; set; }
        public int portfolio_id { get; set; }
        public SymbolContentModel symbol_content { get; set; }
        public string Username { get; set; }
        public List<ctaCOMMON.Indicator.IndicatorParameter> Indicators { get; set; }
        public string LastTimeSync { get; set; }

        public SymbolDashboardModel(int portfolio_id, int symbol_id, string username, ChartRange chartRange, CandelRange candelRange, bool withQuotes)
        {
            // TODO: Complete member initialization
            this.portfolio_id = portfolio_id;
            this.symbol_id = symbol_id;
            this.Username = username;
            this.symbol_content = new SymbolContentModel(portfolio_id, symbol_id);
            this.symbol_content.Symbol_Dashboard = DashboardService.GetSymbolDashboard(portfolio_id, symbol_id, chartRange, candelRange, withQuotes);
            this.Indicators = DashboardService.GetIndicatorsDetails();
            this.LastTimeSync = StockService.GetLastTimeSynchronized(symbol_id);
        }
    }
}