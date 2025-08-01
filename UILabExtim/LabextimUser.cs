using System;
using System.Collections.Generic;

using System.Linq;
using System.Web.Security;

using CMLabExtim;




namespace DLLabExtim
{
    public class LabextimUser
    {
        public MembershipUser Member { get; set; }
        public Employee Employee { get; set; }

        public LabextimUser()
        {
        }

        public LabextimUser(MembershipUser user)
        {
            Member = user;
            if (Member != null)
                using (QuotationDataContext db = new QuotationDataContext())
                {
                    Guid guid = new Guid(Member.ProviderUserKey.ToString());
                    Employee = db.Employees.FirstOrDefault(d => d.UserGUID == guid);
                    // merge aziendale
                    if (Employee.ID_Company == 2)
                    {
                        Employee = db.Employees.FirstOrDefault(d => d.ID_Company == 1 && d.Surname == Employee.Surname && d.Name == Employee.Name);
                    }
                    Employee.Company = db.Companies.FirstOrDefault(d => d.ID == Employee.ID_Company);
                    Employee.UserRole = db.UserRoles.FirstOrDefault(d => d.ID == Employee.Role);
                    if (Employee.LeavingDate <= DateTime.Now.Date)
                    {
                        Member = null;
                    }
                }

        }

        public LabextimUser(string userName)
        {
            Member = Membership.GetUser(userName);
            if (Member != null)
                using (QuotationDataContext db = new QuotationDataContext())
                {
                    Guid guid = new Guid(Member.ProviderUserKey.ToString());
                    Employee = db.Employees.FirstOrDefault(d => d.UserGUID == guid);
                    // merge aziendale
                    if (Employee.ID_Company == 2)
                    {
                        Employee = db.Employees.FirstOrDefault(d => d.ID_Company == 1 && d.Surname == Employee.Surname && d.Name == Employee.Name);
                    }
                    Employee.Company = db.Companies.FirstOrDefault(d => d.ID == Employee.ID_Company);
                    Employee.UserRole = db.UserRoles.FirstOrDefault(d => d.ID == Employee.Role);
                    if (Employee.LeavingDate <= DateTime.Now.Date)
                    {
                        Member = null;
                    }
                }

        }

        public bool IsAdministrator
        {
            get { return this.Employee.Role.GetValueOrDefault(2) == 0; }
        }
        public bool IsOfficeUser
        {
            get { return this.Employee.Role.GetValueOrDefault(2) <= 1; }
        }
        public bool IsWorkshopUser
        {
            get { return this.Employee.Role.GetValueOrDefault(2) <= 2; }
        }


    }
}