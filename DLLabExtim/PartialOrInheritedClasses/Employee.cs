using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Data.Linq.Mapping;


namespace DLLabExtim
{
    [MetadataType(typeof(EmployeeMetadata))]
    [DisplayColumn("UniqueName", "UniqueName", false)]
    public partial class Employee
    {
    }


    public partial class EmployeeMetadata
    {

        [Editable(false)]
        public int ID;
        [Editable(false)]
        public System.Nullable<int> CompanyCode;
        [Editable(false)]
        public string Name;
        [Editable(false)]
        public string Surname;
        [Editable(false)]
        public System.Nullable<System.DateTime> HireDate;
        [Editable(false)]
        public System.Nullable<System.DateTime> LeavingDate;
        [Editable(false)]
        public System.Nullable<int> ID_Manager;
        [Editable(false)]
        public System.Nullable<int> ID_Dept;
        [Editable(false)]
        public System.Nullable<int> ID_Machine;
        [Editable(false)]
        public string UniqueName;
        [Editable(false)]
        public System.Nullable<System.Guid> UserGUID;
        [Editable(false)]
        public System.Nullable<short> Role;
        [Editable(false)]
        public System.Nullable<int> ID_Company;


        [Display(AutoGenerateField = false)]
        public EntitySet<Find_Quotation> Find_Quotations;
        [Display(AutoGenerateField = false)]
        public EntitySet<Quotation> Quotations;
        [Display(AutoGenerateField = false)]
        public EntitySet<Quotation> Quotations1;
        [Display(AutoGenerateField = false)]
        public EntitySet<Employee> Employees;
        [Display(AutoGenerateField = false)]
        public EntitySet<VW_AllQuotations> VW_AllQuotations;
        [Display(AutoGenerateField = false)]
        public EntitySet<VW_AllQuotations> VW_AllQuotations1;
        [Display(AutoGenerateField = false)]
        public EntitySet<ProductionOrderDetail> ProductionOrderDetails;
        [Display(AutoGenerateField = false)]
        public EntitySet<ProductionOrderTechSpec> ProductionOrderTechSpecs;
        [Display(AutoGenerateField = false)]
        public EntityRef<Employee> Employee1;
        [Display(AutoGenerateField = false)]
        public EntityRef<UserRole> UserRole;
        [Display(AutoGenerateField = false)]
        public EntityRef<Company> Company;

    }
}