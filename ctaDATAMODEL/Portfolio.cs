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
    
    public partial class Portfolio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Portfolio()
        {
            this.Portfolio_Stock = new HashSet<Portfolio_Stock>();
            this.Portfolio_Stock_Indicator = new HashSet<Portfolio_Stock_Indicator>();
            this.Portfolio_Stock_Shape = new HashSet<Portfolio_Stock_Shape>();
        }
    
        public int Id { get; set; }
        public string name { get; set; }
        public int user_id { get; set; }
    
        public virtual Tenant Tenant { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Portfolio_Stock> Portfolio_Stock { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Portfolio_Stock_Indicator> Portfolio_Stock_Indicator { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Portfolio_Stock_Shape> Portfolio_Stock_Shape { get; set; }
    }
}