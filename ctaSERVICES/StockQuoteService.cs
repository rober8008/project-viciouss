using ctaCOMMON.AdminModel;
using ctaDATAMODEL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaSERVICES
{
    public static class StockQuoteService
    {
        public static List<StockQuoteModel> GetStockQuotes()
        {
            List<StockQuoteModel> result = new List<StockQuoteModel>();

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                result = entities.Stock_Quote.Select(s => new StockQuoteModel() { Id = s.Id, adj_close = s.adj_close, closing = s.closing, date_round = s.date_round, maximun = s.maximun, minimun = s.minimun, opening = s.opening, stock_id = s.stock_id, volume = s.volume, stock = new StockModel() { Id = s.Stock.Id, symbol = s.Stock.symbol} }).ToList();

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }

            return result;
        }
    }
}
