//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectShopManagment.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Account
    {
        public int ID { get; set; }
        public Nullable<int> Stall_Id { get; set; }
        public Nullable<decimal> Total_Rent { get; set; }
        public Nullable<decimal> Total_Recive { get; set; }
        public Nullable<decimal> Total_Due { get; set; }
    }
}
