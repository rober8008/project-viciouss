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
    public class TenantsService
    {
        public static List<TenantModel> GetTenants()
        {
            List<TenantModel> result = new List<TenantModel>();

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                result = entities.Tenants.Select(t => new TenantModel() { Id = t.Id, email = t.email, username = t.username, type = t.type.Value, activationId = t.activationId, typeExpiration = t.typeExpiration.Value, typename = t.Tenant_Type.Name }).ToList();

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }

            return result;
        }

        public static List<TenantTypeModel> GetTenantsType()
        {
            List<TenantTypeModel> result = new List<TenantTypeModel>();

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                result = entities.Tenant_Type.Select(t => new TenantTypeModel() { Id = t.Id, name = t.Name }).ToList();

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }

            return result;
        }

        public static void UpdateTenant(TenantModel tenantModel)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Tenant tenant = entities.Tenants.Where(s => s.Id == tenantModel.Id).FirstOrDefault();
                if (tenant != null)
                {
                    tenant.activationId = String.IsNullOrEmpty(tenantModel.activationId) ? "" : tenantModel.activationId;
                    tenant.email = tenantModel.email;
                    tenant.type = tenantModel.type;
                    tenant.typeExpiration = tenantModel.typeExpiration;
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
