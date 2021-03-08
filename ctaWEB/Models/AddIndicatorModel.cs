using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ctaWEB.Models
{
    public class AddIndicatorModel
    {
        public AddIndicatorModel() { }

        public AddIndicatorModel(int portfolio_id, int symbol_id, string username)
        {
            this.id_portfolio = portfolio_id;
            this.id_symbol = symbol_id;
            this.username = username;
        }

        public string username { get; set; }

        [Required]
        public int id_portfolio { get; set; }

        [Required]
        public int id_symbol { get; set; }

        [Required]
        public int id_indicator { get; set; }

        [Required]
        public string param1 { get; set; }

        [Required]        
        public string color1 { get; set; }

        [Required]
        public string param2 { get; set; }

        [Required]
        public string color2 { get; set; }

        [Required]
        public string param3 { get; set; }

        [Required]
        public string color3 { get; set; }
    }
}