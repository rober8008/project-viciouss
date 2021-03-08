using ctaSERVICES;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ctaWEB.Models.AdminModels
{
    public class AdminStockQuotesModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("AdminStocksModel")]
        public int stock_id { get; set; }
        public double opening { get; set; }
        public double closing { get; set; }
        public double minimun { get; set; }
        public double maximun { get; set; }
        public decimal volume { get; set; }
        public DateTime date_round { get; set; }
        public double adj_close { get; set; }
        public AdminStocksModel AdminStocksModel { get; set; }               
    }
}