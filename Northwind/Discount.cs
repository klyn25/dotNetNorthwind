//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Northwind
{
    using System;
    using System.Collections.Generic;
    
    public partial class Discount
    {
        public int DiscountID { get; set; }
        public Nullable<int> Code { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<decimal> Discount1 { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    
        public virtual Product Product { get; set; }
    }
}
