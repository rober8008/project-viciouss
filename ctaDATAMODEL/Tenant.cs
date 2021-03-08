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
    
    public partial class Tenant
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tenant()
        {
            this.Portfolios = new HashSet<Portfolio>();
        }
    
        public int Id { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string secretpass { get; set; }
        public string activationId { get; set; }
        public Nullable<int> type { get; set; }
        public Nullable<System.DateTime> typeExpiration { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Portfolio> Portfolios { get; set; }
        public virtual Tenant_Type Tenant_Type { get; set; }
    }
}