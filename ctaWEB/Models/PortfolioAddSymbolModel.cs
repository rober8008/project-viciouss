using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace ctaWEB.Models
{
    public class PortfolioAddSymbolModel
    {
        public PortfolioAddSymbolModel() { }

        [Required]        
        public int Portfolio_Id { get; set; }

        [Required]
        public int Symbol_Id { get; set; }
        public string Username { get; set; }
    }
}