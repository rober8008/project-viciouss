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
    
    public partial class Market_Quote
    {
        public int Id { get; set; }
        public int market_id { get; set; }
    
        public virtual Market Market { get; set; }
    }
}
