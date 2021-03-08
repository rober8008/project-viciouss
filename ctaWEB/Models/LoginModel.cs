using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ctaWEB.Models
{
    public class LoginModel
    {   
        [Required]        
        public string username { get; set; }

        [Required]
        public string password { get; set; }

        public string returnUrl { get; set; }
    }
}