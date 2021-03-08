using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ctaWEB.Models.AdminModels
{
    public class AdminMarketIndexModel
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
        [ForeignKey("AdminMarketsModel")]
        public int market_id { get; set; }
        [DisplayName("Market")]
        public AdminMarketsModel AdminMarketsModel { get; set; }
        public string market_name { get; set; }
    }
}