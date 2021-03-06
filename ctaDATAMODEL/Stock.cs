//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ctaDATAMODEL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Stock
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Stock()
        {
            this.MarketIndex_Stock = new HashSet<MarketIndex_Stock>();
            this.Portfolio_Stock = new HashSet<Portfolio_Stock>();
            this.Portfolio_Stock_Indicator = new HashSet<Portfolio_Stock_Indicator>();
            this.Portfolio_Stock_Shape = new HashSet<Portfolio_Stock_Shape>();
            this.Stock_Quote = new HashSet<Stock_Quote>();
            this.Stock_Report = new HashSet<Stock_Report>();
            this.Stock_Quote_Intradiary = new HashSet<Stock_Quote_Intradiary>();
        }
    
        public int Id { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public int market_id { get; set; }
        public int type_id { get; set; }
        public string description { get; set; }
        public Nullable<bool> active { get; set; }
        public string technical_report_batch { get; set; }
    
        public virtual Market Market { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MarketIndex_Stock> MarketIndex_Stock { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Portfolio_Stock> Portfolio_Stock { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Portfolio_Stock_Indicator> Portfolio_Stock_Indicator { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Portfolio_Stock_Shape> Portfolio_Stock_Shape { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stock_Quote> Stock_Quote { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stock_Report> Stock_Report { get; set; }
        public virtual Stock_Type Stock_Type { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stock_Quote_Intradiary> Stock_Quote_Intradiary { get; set; }
    }
}
