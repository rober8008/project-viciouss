using ctaDATAMODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaSERVICES
{
    public static class ValidationService
    {
        public static string ValidateUserProtfolioCount(string usernameORemail)
        {
            string result_message = null;
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();
                Tenant tenant = entities.Tenants.Where(t => t.username == usernameORemail || t.email == usernameORemail).FirstOrDefault();

                if (tenant == null)
                {
                    result_message = "Inavlid Action (Add Portfolio), Username does not exists";
                }
                string config_value = entities.Configs.Where(c => c.ConfigName == tenant.Tenant_Type.Name + "-portfolio_count").Select(c => c.ConfigValue).FirstOrDefault();
                int portfolio_count = 0;
                if (int.TryParse(config_value, out portfolio_count))
                {
                    if (portfolio_count <= tenant.Portfolios.Count)
                    {
                        result_message = "Para disfrutar de todas las funcionalidades de forma ilimitada, debe ser usuario PRO. Conozca más ingresando en ";
                    }
                }                              

                entities.Database.Connection.Close();                
            }
            return result_message;
        }

        public static string ValidateUserSymbolCount(string usernameORemail, int portfolioID)
        {
            string result_message = null;
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();
                Tenant tenant = entities.Tenants.Where(t => t.username == usernameORemail || t.email == usernameORemail).FirstOrDefault();

                if (tenant == null)
                {
                    result_message = "Inavlid Action (Add Symbol), Username does not exists";
                }

                string config_value = entities.Configs.Where(c => c.ConfigName == tenant.Tenant_Type.Name + "-stock_count").Select(c => c.ConfigValue).FirstOrDefault();
                int stock_count = 0;
                if (int.TryParse(config_value, out stock_count))
                {
                    if (stock_count <= entities.Portfolio_Stock.Where(p => p.portfolio_id == portfolioID).Count())
                    {
                        result_message = "Para disfrutar de todas las funcionalidades de forma ilimitada, debe ser usuario PRO. Conozca más ingresando en ";
                    }
                }                

                entities.Database.Connection.Close();
            }
            return result_message;
        }

        public static string ValidateUserIndicatorCount(string usernameORemail, int portfolioID, int stockID)
        {
            string result_message = null;
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();
                Tenant tenant = entities.Tenants.Where(t => t.username == usernameORemail || t.email == usernameORemail).FirstOrDefault();

                if (tenant == null)
                {
                    result_message = "Inavlid Action (Add Indicator/Shape), Username does not exists";
                }

                string config_value = entities.Configs.Where(c => c.ConfigName == tenant.Tenant_Type.Name + "-indicator_count").Select(c => c.ConfigValue).FirstOrDefault();
                int stock_count = 0;
                if (int.TryParse(config_value, out stock_count))
                {
                    if (stock_count <= (entities.Portfolio_Stock_Indicator.Where(i => i.portfolio_id == portfolioID && i.stock_id == stockID).Count() + (entities.Portfolio_Stock_Shape.Where(i => i.portfolio_id == portfolioID && i.stock_id == stockID).Count())))
                    {
                        result_message = "Para disfrutar de todas las funcionalidades de forma ilimitada, debe ser usuario PRO. Conozca más ingresando en ";
                    }
                }

                entities.Database.Connection.Close();
            }
            return result_message;
        }            
    }
}
