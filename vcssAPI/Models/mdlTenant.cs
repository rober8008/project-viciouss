using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vcssAPI.Models
{
    public class mdlTenant
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Secretpass { get; set; }
        public string ActivationId { get; set; }
        public string Type { get; set; }
    }
}
