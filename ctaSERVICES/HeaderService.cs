using ctaCOMMON.Charts;
using ctaCOMMON.Dashboard;
using ctaDATAMODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaSERVICES
{
    public static class HeaderService
    {
        public static List<DashboardItem> GetMenuItems(string username)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                List<DashboardItem> result = new List<DashboardItem>();

                entities.Database.Connection.Open();

                int user_id = UserService.GetUserId(username);

                var user_portfolios = entities.Portfolios.Where(ptfolio => ptfolio.user_id == user_id);

                foreach (var portfolio in user_portfolios)
                {
                    DashboardItem dashboard_item = new DashboardItem() { Portfolio = portfolio.name, Symbols = new List<ctaCOMMON.Charts.Symbol>(), Portfolio_Id = portfolio.Id };

                    foreach (var stock in portfolio.Portfolio_Stock)
                    {
                        Symbol symbol = new ctaCOMMON.Charts.Symbol() { Symbol_ID = stock.stock_id, Symbol_Name = stock.Stock.symbol };
                        dashboard_item.Symbols.Add(symbol);
                    }

                    result.Add(dashboard_item);
                }

                entities.Database.Connection.Close();

                return result;
            }
        }
    }
}
