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
    public static class StockTypeService
    {
        public static List<StockTypeModel> GetStockTypes()
        {
            List<StockTypeModel> result = new List<StockTypeModel>();

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                result = entities.Stock_Type.Select(s => new StockTypeModel() { Id = s.Id, name = s.name }).ToList();

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }

            return result;
        }

        public static void CreateStockType(StockTypeModel stockTypeModel)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Stock_Type st = new Stock_Type() { name = stockTypeModel.name};
                entities.Stock_Type.Add(st);
                entities.SaveChanges();

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
        }

        public static void UpdateStockType(StockTypeModel stockTypeModel)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Stock_Type st = entities.Stock_Type.Where(s => s.Id == stockTypeModel.Id).FirstOrDefault();
                if (st != null)
                {
                    st.name = stockTypeModel.name;

                    entities.SaveChanges();
                }

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
        }

        public static void DeleteStockType(int stocktypeID)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Stock_Type st = entities.Stock_Type.Where(s => s.Id == stocktypeID).FirstOrDefault();
                if (st != null)
                {
                    entities.Stock_Type.Remove(st);
                    entities.SaveChanges();
                }

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
        }
    }
}
