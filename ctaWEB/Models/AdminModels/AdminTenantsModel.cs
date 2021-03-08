using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ctaWEB.Models.AdminModels
{
    public class AdminTenantsModel
    {
        [Key]
        public int Id { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public int type { get; set; }
        public string activationId { get; set; }
        public DateTime typeExpiration { get; set; }
    }
}