using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ctaWEB.Models.AdminModels
{
    public class AdminMarketIndexStockModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("AdminMarketIndexModel")]
        public int marketindex_id { get; set; }
        [DisplayName("Market Index")]
        public AdminMarketIndexModel AdminMarketIndexModel { get; set; }
        public string marketindex_name { get; set; }
        [ForeignKey("AdminStocksModel")]
        public int stock_id { get; set; }
        [DisplayName("Stock")]
        public AdminStocksModel AdminStocksModel { get; set; }
        public string stock_symbol { get; set; }
    }
}