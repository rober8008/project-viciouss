using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ctaWEB.Models
{
    public class DashhboardAddPortfolioModel
    {
        [Required]
        [Display(Name="Nombre del Portafolio:")]
        public string Portfolio_Name { get; set; }
        public string username { get; set; }

        public string Action { get; set; }
    }
}