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
    public class ConfigService
    {
        public static List<ConfigModel> GetConfigs()
        {
            List<ConfigModel> result = new List<ConfigModel>();

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                result = entities.Configs.Select(c => new ConfigModel() { Id =  c.Id, ConfigName = c.ConfigName, ConfigValue = c.ConfigValue }).ToList();

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }

            return result;
        }

        public static ConfigModel GetConfig(string configName)
        {
            ConfigModel result = null;

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                result = entities.Configs.Where(c => c.ConfigName == configName).Select(c => new ConfigModel() { Id = c.Id, ConfigName = c.ConfigName, ConfigValue = c.ConfigValue }).FirstOrDefault();

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }

            return result;
        }

        public static void CreateConfig(ConfigModel configModel)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Config cfg = new Config() { ConfigName = configModel.ConfigName, ConfigValue = configModel.ConfigValue };
                entities.Configs.Add(cfg);
                entities.SaveChanges();

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
        }

        public static void UpdateConfig(ConfigModel configModel)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Config cfg = entities.Configs.Where(s => s.Id == configModel.Id).FirstOrDefault();
                if (cfg != null)
                {
                    cfg.ConfigName = configModel.ConfigName;
                    cfg.ConfigValue = configModel.ConfigValue;
                    entities.SaveChanges();
                }

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
        }

        public static void DeleteConfig(ConfigModel configModel)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Config cfg = entities.Configs.Where(s => s.Id == configModel.Id).FirstOrDefault();
                if (cfg != null)
                {
                    entities.Configs.Remove(cfg);
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
