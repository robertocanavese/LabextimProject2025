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
    
    public partial class CustomersMarkUps
    {
        public int Code { get; set; }
        public Nullable<int> MarkUp { get; set; }
    
        public virtual Customers Customers { get; set; }
    }
}
