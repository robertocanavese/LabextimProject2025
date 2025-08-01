//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LabExtimOperator.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Quotations
    {
        public Quotations()
        {
            this.CustomerOrders = new HashSet<CustomerOrders>();
            this.QuotationDetails = new HashSet<QuotationDetails>();
            this.ProductionOrders = new HashSet<ProductionOrders>();
        }
    
        public int ID { get; set; }
        public Nullable<int> CustomerCode { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Subject { get; set; }
        public Nullable<int> Q1 { get; set; }
        public Nullable<int> Q2 { get; set; }
        public Nullable<int> Q3 { get; set; }
        public Nullable<int> Q4 { get; set; }
        public Nullable<int> Q5 { get; set; }
        public Nullable<int> MarkUp { get; set; }
        public Nullable<int> ID_Owner { get; set; }
        public Nullable<int> ID_Approver { get; set; }
        public Nullable<bool> Draft { get; set; }
        public Nullable<int> Status { get; set; }
        public string Note { get; set; }
        public Nullable<bool> P1 { get; set; }
        public Nullable<bool> P2 { get; set; }
        public Nullable<bool> P3 { get; set; }
        public Nullable<bool> P4 { get; set; }
        public Nullable<bool> P5 { get; set; }
        public string PriceCom { get; set; }
        public string PrintingMainText { get; set; }
        public Nullable<int> ID_Company { get; set; }
    
        public virtual ICollection<CustomerOrders> CustomerOrders { get; set; }
        public virtual Customers Customers { get; set; }
        public virtual Employees Employees { get; set; }
        public virtual Employees Employees1 { get; set; }
        public virtual ICollection<QuotationDetails> QuotationDetails { get; set; }
        public virtual Statuses Statuses { get; set; }
        public virtual ICollection<ProductionOrders> ProductionOrders { get; set; }
    }
}
