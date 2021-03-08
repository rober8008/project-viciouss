using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ctaWEB.Models
{
    public class AddShapeModel
    {
        public AddShapeModel() { }

        public AddShapeModel(int portfolio_id, int symbol_id, string username)
        {
            // TODO: Complete member initialization
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
        public int id_shape { get; set; }

        public DateTime date1 { get; set; }
        public DateTime date2 { get; set; }
        public double value1 { get; set; }
        public double value2 { get; set; }
        public string color { get; set; }
        public string name { get; set; }
    }
}