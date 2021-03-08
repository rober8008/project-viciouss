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
    public static class MarketIndexService
    {
        public static List<MarketIndexModel> GetMarketIndexes()
        {
            List<MarketIndexModel> result = new List<MarketIndexModel>();

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                result = entities.MarketIndexes.OrderBy(x => x.market_id).Select(s => new MarketIndexModel() { Id = s.Id, name = s.name, market_id = s.market_id, market_name = s.Market.name }).ToList();

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }

            return result;
        }

        public static void UpdateMarketIndex(MarketIndexModel marketIndexModel)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                MarketIndex mIndex = entities.MarketIndexes.Where(s => s.Id == marketIndexModel.Id).FirstOrDefault();
                if (mIndex != null)
                {
                    mIndex.name = marketIndexModel.name;
                    mIndex.market_id = marketIndexModel.market_id;
                    entities.SaveChanges();
                }

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
        }

        public static void DeleteMarketIndex(int ID)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                MarketIndex mIndex = entities.MarketIndexes.Where(s => s.Id == ID).FirstOrDefault();
                if (mIndex != null)
                {
                    entities.MarketIndexes.Remove(mIndex);
                    entities.SaveChanges();
                }

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
        }

        public static void CreateMarketIndex(MarketIndexModel marketIndexModel)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                MarketIndex mIndex = new MarketIndex() { name = marketIndexModel.name, market_id = marketIndexModel.market_id };
                entities.MarketIndexes.Add(mIndex);
                entities.SaveChanges();

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
        }
    }
}
