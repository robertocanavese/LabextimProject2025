using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Security;

using DLLabExtim;
using CMLabExtim;


namespace LabExtim.Restricted
{
    public partial class ManageUser : Page
    {

        public LabextimUser CurrentUser
        {
            get
            {
                return Session["CurrentUserToManage"] as LabextimUser;
            }

            set
            {
                Session["CurrentUserToManage"] = value;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (QuotationDataContext db = new QuotationDataContext())
                {
                    ddlCompanies.DataSource = db.Companies.ToList();
                    ddlCompanies.DataTextField = "Description";
                    ddlCompanies.DataValueField = "ID";
                    ddlCompanies.DataBind();

                    ddlRoles.DataSource = db.UserRoles.ToList();
                    ddlRoles.DataTextField = "Description";
                    ddlRoles.DataValueField = "ID";
                    ddlRoles.DataBind();

                    ddlEditCompanies.DataSource = db.Companies.ToList();
                    ddlEditCompanies.DataTextField = "Description";
                    ddlEditCompanies.DataValueField = "ID";
                    ddlEditCompanies.DataBind();

                    ddlEditRoles.DataSource = db.UserRoles.ToList();
                    ddlEditRoles.DataTextField = "Description";
                    ddlEditRoles.DataValueField = "ID";
                    ddlEditRoles.DataBind();
                }
            }

            lblMessage.Text = "";

        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            var curUser = Membership.FindUsersByName(txtUserName.Text).Cast<MembershipUser>().FirstOrDefault();
            if (curUser != null)
            {
                lblMessage.Text = "Username già in uso!";
                return;
            }
            int n;
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                !int.TryParse(txtCompanyCode.Text, out n) ||
                string.IsNullOrWhiteSpace(txtUserName.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtMail.Text) ||
                !Utilities.IsValidEmail(txtMail.Text) ||
                string.IsNullOrWhiteSpace(ddlCompanies.SelectedValue) ||
                string.IsNullOrWhiteSpace(ddlRoles.SelectedValue) ||
                txtPassword.Text != txtConfirmPassword.Text)
            {
                lblMessage.Text = "Uno o più campi non è stato compilato correttamente!";
                return;
            }

            else
            {
                try
                {
                    curUser = Membership.CreateUser(txtUserName.Text, txtPassword.Text, txtMail.Text);
                    using (QuotationDataContext db = new QuotationDataContext())
                    {
                        Employee newEmployee = new Employee();

                        newEmployee.CompanyCode = int.Parse(txtCompanyCode.Text);
                        newEmployee.Name = txtFirstName.Text;
                        newEmployee.Surname = txtLastName.Text;
                        newEmployee.HireDate = DateTime.Now;
                        newEmployee.LeavingDate = DateTime.Now.AddYears(50);
                        newEmployee.UserGUID = (Guid)curUser.ProviderUserKey;
                        newEmployee.Role = Int16.Parse(ddlRoles.SelectedValue);
                        newEmployee.ID_Company = Int32.Parse(ddlCompanies.SelectedValue);

                        db.Employees.InsertOnSubmit(newEmployee);

                        db.SubmitChanges();
                        grdEmployees.DataBind();
                        lblMessage.Text = "Utente creato con successo!";
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = string.Format("Errore nella creazione dell'utente ({0}) , riprovare!", ex.Message);
                }

            }
        }

        protected void btnSearchName_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtEditName.Text))
            {
                CurrentUser = new LabextimUser(txtEditName.Text);
                if (CurrentUser.Employee != null)
                {
                    txtEditCompanyCode.Text = CurrentUser.Employee.CompanyCode.ToString();
                    ddlEditCompanies.SelectedValue = CurrentUser.Employee.ID_Company.ToString();
                    ddlEditRoles.SelectedValue = CurrentUser.Employee.Role.ToString();
                    lblUniqueName.Text = CurrentUser.Employee.UniqueName;
                }
                else
                {
                    lblMessage.Text = "Username non trovato!";
                }
            }
        }

        protected void btnEditUser_Click(object sender, EventArgs e)
        {

            if (CurrentUser.Employee != null)
            {
                int n;
                if (
                    !int.TryParse(txtEditCompanyCode.Text, out n) ||
                    string.IsNullOrWhiteSpace(ddlEditCompanies.SelectedValue) ||
                    string.IsNullOrWhiteSpace(ddlEditRoles.SelectedValue))
                {
                    lblMessage.Text = "Uno o più campi non è stato compilato correttamente!";
                    return;
                }

                else
                {
                    try
                    {

                        using (QuotationDataContext db = new QuotationDataContext())
                        {
                            Employee modEmployee = db.Employees.FirstOrDefault(d => d.UserGUID == CurrentUser.Employee.UserGUID);

                            modEmployee.CompanyCode = int.Parse(txtEditCompanyCode.Text);
                            modEmployee.Role = Int16.Parse(ddlEditRoles.SelectedValue);
                            modEmployee.ID_Company = Int32.Parse(ddlEditCompanies.SelectedValue);

                            db.SubmitChanges();
                            grdEmployees.DataBind();
                            lblMessage.Text = "Utente modificato con successo!";
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = string.Format("Errore nella modifica dell'utente ({0}) , riprovare!", ex.Message);
                    }

                }
            }

        }

        protected void btnDeactivateUser_Click(object sender, System.EventArgs e)
        {
            if (CurrentUser.Employee != null)
            {
                try
                {

                    using (QuotationDataContext db = new QuotationDataContext())
                    {
                        Employee modEmployee = db.Employees.FirstOrDefault(d => d.UserGUID == CurrentUser.Employee.UserGUID);
                        modEmployee.LeavingDate = DateTime.Today;
                        db.SubmitChanges();
                        grdEmployees.DataBind();
                        lblMessage.Text = "Utente disattivato con successo!";
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = string.Format("Errore nella modifica dell'utente ({0}) , riprovare!", ex.Message);
                }
            }
        }

        protected void btnActivateUser_Click(object sender, EventArgs e)
        {
            if (CurrentUser.Employee != null)
            {
                try
                {

                    using (QuotationDataContext db = new QuotationDataContext())
                    {
                        Employee modEmployee = db.Employees.FirstOrDefault(d => d.UserGUID == CurrentUser.Employee.UserGUID);
                        modEmployee.HireDate = DateTime.Today;
                        modEmployee.LeavingDate = DateTime.Now.AddYears(50);
                        db.SubmitChanges();
                        grdEmployees.DataBind();
                        lblMessage.Text = "Utente riattivato con successo!";
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = string.Format("Errore nella modifica dell'utente ({0}) , riprovare!", ex.Message);
                }
            }
        }

        protected void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (CurrentUser.Employee != null)
            {
                try
                {

                    using (QuotationDataContext db = new QuotationDataContext())
                    {
                        Membership.DeleteUser(CurrentUser.Member.UserName);

                        bool passed = true;
                        if (db.Quotations.FirstOrDefault(d => d.ID_Owner == CurrentUser.Employee.ID) != null)
                            passed = false;
                        else if (db.Quotations.FirstOrDefault(d => d.ID_Approver == CurrentUser.Employee.ID) != null)
                            passed = false;
                        else if (db.ProductionOrders.FirstOrDefault(d => d.ID_Contractor == CurrentUser.Employee.ID) != null)
                            passed = false;
                        else if (db.ProductionOrderDetails.FirstOrDefault(d => d.ID_Owner == CurrentUser.Employee.ID) != null)
                            passed = false;
                        else if (db.TempQuotations.FirstOrDefault(d => d.ID_Owner == CurrentUser.Employee.ID) != null)
                            passed = false;

                        if (passed)
                        {
                            db.Employees.DeleteOnSubmit(db.Employees.FirstOrDefault(d => d.UserGUID == CurrentUser.Employee.UserGUID));
                            db.SubmitChanges();
                        }
                        grdEmployees.DataBind();
                        lblMessage.Text = "Utente eliminato con successo!";
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = string.Format("Errore nella modifica dell'utente ({0}) , riprovare!", ex.Message);
                }
            }

        }

        protected void grdEmployees_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            if (e.SortDirection == System.Web.UI.WebControls.SortDirection.Ascending)
                ldsVW_Employees.OrderBy = e.SortExpression + " ASC";
            else
                ldsVW_Employees.OrderBy = e.SortExpression + " DESC";

            grdEmployees.DataBind();
        }

        protected void grdEmployees_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                VW_Employee rowData = e.Row.DataItem as VW_Employee;
                if (rowData.LeavingDate <= DateTime.Now.Date)
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                    e.Row.ToolTip = "Utente disattivato";
                }
                else
                {
                    e.Row.ToolTip = "Utente attivo";
                }
            }
        }

    }
}