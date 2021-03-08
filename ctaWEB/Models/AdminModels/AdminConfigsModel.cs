using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ctaCOMMON.AdminModel;

namespace ctaWEB.Models.AdminModels
{
    public class AdminConfigsModel
    {
        [Key]
        public int Id { get; set; }
        public string ConfigName { get; set; }
        public string ConfigValue { get; set; }        
    }
}