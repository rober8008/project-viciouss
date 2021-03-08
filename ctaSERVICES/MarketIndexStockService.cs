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
    public static class MarketIndexStockService
    {
        public static List<MarketIndexStockModel> GetMarketIndexStocks()
        {
            List<MarketIndexStockModel> result = new List<MarketIndexStockModel>();

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                result = entities.MarketIndex_Stock.OrderBy(x => x.marketindex_id).Select(s => new MarketIndexStockModel() { Id = s.Id, marketindex_id = s.marketindex_id, marketindex_name = s.MarketIndex.name, stock_id = s.stock_id, stock_symbol = s.Stock.symbol }).ToList();

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }

            return result;
        }

        public static void UpdateMarketIndexStock(MarketIndexStockModel marketIndexStockModel)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                MarketIndex_Stock mIndexS = entities.MarketIndex_Stock.Where(s => s.Id == marketIndexStockModel.Id).FirstOrDefault();
                if (mIndexS != null)
                {
                    mIndexS.marketindex_id = marketIndexStockModel.marketindex_id;
                    mIndexS.stock_id = marketIndexStockModel.stock_id;
                    entities.SaveChanges();
                }

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
        }

        public static void DeleteMarketIndexStock(int ID)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                MarketIndex_Stock mIndexS = entities.MarketIndex_Stock.Where(s => s.Id == ID).FirstOrDefault();
                if (mIndexS != null)
                {
                    entities.MarketIndex_Stock.Remove(mIndexS);
                    entities.SaveChanges();
                }

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
        }

        public static void CreateMarketIndexStock(MarketIndexStockModel marketIndexStockModel)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                MarketIndex_Stock mIndexs = new MarketIndex_Stock() { marketindex_id = marketIndexStockModel.marketindex_id, stock_id = marketIndexStockModel.stock_id };
                entities.MarketIndex_Stock.Add(mIndexs);
                entities.SaveChanges();

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
        }
    }
}
