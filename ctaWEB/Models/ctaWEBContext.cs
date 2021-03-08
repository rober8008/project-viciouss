using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ctaWEB.Models
{
    public class ctaWEBContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public ctaWEBContext() : base("name=ctaWEBContext")
        {
        }

        public System.Data.Entity.DbSet<ctaWEB.Models.AdminModels.AdminMarketsModel> AdminMarketsModels { get; set; }

        public System.Data.Entity.DbSet<ctaWEB.Models.AdminModels.AdminStockTypesModel> AdminStockTypesModels { get; set; }

        public System.Data.Entity.DbSet<ctaWEB.Models.AdminModels.AdminStocksModel> AdminStocksModel { get; set; }

        public System.Data.Entity.DbSet<ctaWEB.Models.AdminModels.AdminStockQuotesModel> AdminStockQuotesModels { get; set; }

        public System.Data.Entity.DbSet<ctaWEB.Models.AdminModels.AdminHolidaysModel> AdminHolidaysModels { get; set; }

        public System.Data.Entity.DbSet<ctaWEB.Models.AdminModels.AdminConfigsModel> AdminConfigsModels { get; set; }

        public System.Data.Entity.DbSet<ctaWEB.Models.AdminModels.AdminTenantsModel> AdminTenantsModels { get; set; }
    }
}
