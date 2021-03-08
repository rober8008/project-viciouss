using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.AdminModel
{
    public class TenantModel
    {
        public int Id { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public int type { get; set; }
        public string typename { get; set; }
        public string activationId { get; set; }
        public DateTime typeExpiration { get; set; }
    }
}
