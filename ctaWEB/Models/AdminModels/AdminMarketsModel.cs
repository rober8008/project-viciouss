using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ctaWEB.Models.AdminModels
{
    public class AdminMarketsModel
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
        public string work_hours { get; set; }
    }
}