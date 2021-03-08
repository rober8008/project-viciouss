using System;
using System.Collections.Generic;

namespace vcssAPI.DBContext
{
    public partial class Tenant
    {
        public Tenant()
        {
            Portfolio = new HashSet<Portfolio>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Secretpass { get; set; }
        public string ActivationId { get; set; }
        public int? Type { get; set; }

        public TenantType TypeNavigation { get; set; }
        public ICollection<Portfolio> Portfolio { get; set; }
    }
}
