//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBOperation
{
    using System;
    using System.Collections.Generic;
    
    public partial class SaleActivity
    {
        public int Id { get; set; }
        public System.DateTime SaleDate { get; set; }
        public int Status { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhoneNo { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string ProductName { get; set; }
        public string Capacity { get; set; }
        public string Unit { get; set; }
        public System.DateTime RecentCallDate { get; set; }
        public Nullable<System.DateTime> AnticipatedClosingDate { get; set; }
        public Nullable<int> NoOfFollowUps { get; set; }
        public string Remarks { get; set; }
        public int SalesRepresentativeId { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<decimal> InvoiceAmount { get; set; }
        public Nullable<System.DateTime> DateOfClosing { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual SalesStatu SalesStatu { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }
    }
}
