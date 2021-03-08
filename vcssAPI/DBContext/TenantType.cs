using System;
using System.Collections.Generic;

namespace vcssAPI.DBContext
{
    public partial class TenantType
    {
        public TenantType()
        {
            Tenant = new HashSet<Tenant>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Tenant> Tenant { get; set; }
    }
}
