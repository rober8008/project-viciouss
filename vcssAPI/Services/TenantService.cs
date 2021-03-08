using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vcssAPI.Models;

namespace vcssAPI.Services
{
    public class TenantService
    {
        public static mdlTenant Retrieve(int id)
        {
            using (apiDBContext context = new apiDBContext())
            {
                return context.Tenant.Where(t => t.Id == id).Select(t => new mdlTenant() { Id = t.Id, Email = t.Email, Username = t.Username, Type = t.TypeNavigation.Name }).FirstOrDefault();
            }
        }        
    }
}
