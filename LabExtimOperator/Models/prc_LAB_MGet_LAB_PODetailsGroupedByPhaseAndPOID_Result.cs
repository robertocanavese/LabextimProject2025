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
    
    public partial class prc_LAB_MGet_LAB_PODetailsGroupedByPhaseAndPOID_Result
    {
        public int ID { get; set; }
        public Nullable<float> POQuantity { get; set; }
        public Nullable<int> ID_PickingItem { get; set; }
        public string TypeDescription { get; set; }
        public string ItemTypeDescription { get; set; }
        public string ItemDescription { get; set; }
        public Nullable<int> UnitID { get; set; }
        public string UnitDescription { get; set; }
        public Nullable<int> SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public Nullable<System.DateTime> ProductionDate { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> Cost { get; set; }
        public Nullable<double> ProducedQuantity { get; set; }
        public Nullable<decimal> HistoricalCost { get; set; }
        public string Order { get; set; }
    }
}
