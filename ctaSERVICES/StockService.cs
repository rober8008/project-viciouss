using ctaCOMMON.AdminModel;
using ctaDATAMODEL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaSERVICES
{
    public static class StockService
    {
        public static Stock GetStockSymbol(int stock_id)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Stock result = entities.Stocks.Where(s => s.Id == stock_id).FirstOrDefault();

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
                return result;
            } 
        }

        public static List<StockModel> GetStocks()
        {
            List<StockModel> result = new List<StockModel>();

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                result = entities.Stocks.Select(s => new StockModel() { active = s.active.Value, description = s.description, Id = s.Id, market_id = s.market_id, name = s.name, symbol = s.symbol, type_id = s.type_id, market_name = s.Market.name}).ToList();                

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }

            return result;
        }        

        public static void CreateStock(StockModel stockModel)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Stock st = new Stock() { active = stockModel.active, description = stockModel.description, market_id = stockModel.market_id, name = stockModel.name, symbol = stockModel.symbol, type_id = stockModel.type_id };
                entities.Stocks.Add(st);
                entities.SaveChanges();
                
                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
        }

        public static void UpdateStock(StockModel stockModel)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Stock st = entities.Stocks.Where(s => s.Id == stockModel.Id).FirstOrDefault();
                if (st != null)
                {
                    st.active = stockModel.active;
                    st.description = stockModel.description;
                    st.market_id = stockModel.market_id;
                    st.name = stockModel.name;
                    st.symbol = stockModel.symbol;
                    st.type_id = stockModel.type_id;
                    
                    entities.SaveChanges();
                }

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
        }

        public static void DeleteStock(int stockID)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Stock st = entities.Stocks.Where(s => s.Id == stockID).FirstOrDefault();
                if (st != null)
                {
                    entities.Stocks.Remove(st);
                    entities.SaveChanges();
                }

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
        }
        
        public static string GetLastTimeSynchronized(int stockID)
        {
            string result = "-";
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                if (entities.Stock_Quote_Intradiary.Any(s => s.stock_id == stockID))
                {
                    DateTime dt = entities.Stock_Quote_Intradiary.Where(s => s.stock_id == stockID).Select(s => s.datetime).Max();
                    result = dt.Date.Day + "/" + dt.Date.Month/*dt.Date.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")).Substring(0, 3).ToUpper()*/ + "/" + dt.Date.Year + " " + dt.ToShortTimeString().Replace(" ","");
                }                 

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
            return result;
        }
    }
}
