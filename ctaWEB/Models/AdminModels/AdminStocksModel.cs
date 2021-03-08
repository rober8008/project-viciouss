using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ctaWEB.Models.AdminModels
{
    public class AdminStocksModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string symbol { get; set; }
        [Required]
        public string name { get; set; }
        [ForeignKey("AdminMarketsModel")]
        public int market_id { get; set; }
        [ForeignKey("AdminStockTypesModel")]
        public int type_id { get; set; }
        [AllowHtml]
        public string description { get; set; }
        public bool active { get; set; }
        public AdminMarketsModel AdminMarketsModel { get; set; }
        public AdminStockTypesModel AdminStockTypesModel { get; set; }
        public string symbol_market { get; set; }
    }
}