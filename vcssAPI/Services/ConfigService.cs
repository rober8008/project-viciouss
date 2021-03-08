using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vcssAPI.DBContext;
using vcssAPI.Models;

namespace vcssAPI.Services
{
    public class ConfigService
    {
        public static List<mdlConfig> Retrieve(int id = 0)
        {
            using (apiDBContext context = new apiDBContext())
            {
                return context.Config.Where(c => id == 0 || c.Id == id).Select(c => new mdlConfig() { Id = c.Id, ConfigName = c.ConfigName, ConfigValue = c.ConfigValue }).ToList<mdlConfig>();
            }
        }

        public static mdlConfig Create(mdlConfig config)
        {
            using (apiDBContext context = new apiDBContext())
            {
                Config dbConfig = context.Config.Add(new Config() { ConfigName = config.ConfigName, ConfigValue = config.ConfigValue }).Entity;
                context.SaveChanges();
                return new mdlConfig() { Id = dbConfig.Id, ConfigName = dbConfig.ConfigName, ConfigValue = dbConfig.ConfigValue };
            }
        }

        public static mdlConfig Update(mdlConfig config)
        {
            using (apiDBContext context = new apiDBContext())
            {
                Config dbConfig = context.Config.Where(c => c.Id == config.Id).FirstOrDefault();
                dbConfig.ConfigName = config.ConfigName;
                dbConfig.ConfigValue = config.ConfigValue;
                Config entityConfig = context.Config.Update(dbConfig).Entity;
                context.SaveChanges();
                return new mdlConfig() { Id = entityConfig.Id, ConfigName = entityConfig.ConfigName, ConfigValue = entityConfig.ConfigValue };
            }
        }

        public static mdlConfig Delete(int configId)
        {
            using (apiDBContext context = new apiDBContext())
            {
                Config dbConfig = context.Config.Where(c => c.Id == configId).FirstOrDefault();
                context.Config.Remove(dbConfig);
                context.SaveChanges();
                return new mdlConfig() { Id = configId, ConfigName = dbConfig.ConfigName, ConfigValue = dbConfig.ConfigValue };
            }
        }
    }
}
