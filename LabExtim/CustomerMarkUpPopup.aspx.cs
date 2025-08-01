using System;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class CustomerMarkUpPopup : BaseController
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            //DynamicDataManager1.RegisterControl(GridView1, false /*setSelectionFromUrl*/);
            DynamicDataManager1.RegisterControl(dtvCustomerMarkUp);
            DynamicDataManager1.RegisterControl(dtvCustomerNickname);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblItemNo.Text = Int32.Parse(CustomerParameter) == -1 ? " [Nuovo]" : " No " + CustomerParameter;
            if (Int32.Parse(CustomerParameter) == -1)
            {
                lblItemNo.Text = " [Nuovo]";
                DetailsDataSource.WhereParameters.Clear();
                dtvCustomerMarkUp.ChangeMode(DetailsViewMode.Insert);
                ldsCustomerNicknames.WhereParameters.Clear();
                dtvCustomerNickname.ChangeMode(DetailsViewMode.Insert);
            }
            else
            {
                dtvCustomerMarkUp.AutoGenerateInsertButton = false;
                dtvCustomerNickname.AutoGenerateInsertButton = false;
            }
        }

        protected void OnDetailsViewItemDeleted(object sender, DetailsViewDeletedEventArgs e)
        {
        }

        protected void OnDetailsViewItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
        }

        protected void OnDetailsViewItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            dtvCustomerMarkUp.Visible = false;
            lblItemNo.Visible = false;
        }

        protected void OnDetailsViewModeChanging(object sender, DetailsViewModeEventArgs e)
        {
            if (e.NewMode == DetailsViewMode.Insert)
            {
                lblItemNo.Text = " [Nuovo]";
            }
            else
            {
                lblItemNo.Text = " No " + Request.QueryString[CustomerKey];
            }
        }

        protected void OnDetailsViewPreRender(object sender, EventArgs e)
        {
            if (dtvCustomerMarkUp.CurrentMode == DetailsViewMode.Insert)
            {
                var _dvr = dtvCustomerMarkUp.Rows[0];

                if (Int32.Parse(CustomerParameter) != -1)
                {
                    var _dyc = (DynamicControl) _dvr.FindControl("dycCustomer");
                    ((DropDownList) _dyc.Controls[0].Controls[0]).SelectedValue = CustomerParameter.Trim();
                }
            }
            else
            {
                dtvCustomerMarkUp.DataBind();
            }
        }

        private void SetDeleteConfirmation(TableRow row)
        {
            foreach (Control c in row.Cells[0].Controls)
            {
                if (c is LinkButton)
                {
                    var btn = (LinkButton) c;
                    if (btn.CommandName == DataControlCommands.DeleteCommandName)
                    {
                        btn.OnClientClick = "return confirm('Sei sicuro di voler eliminare questa voce?');";
                    }
                }
            }
        }

        protected void DetailsDataSource_Inserting(object sender, LinqDataSourceInsertEventArgs e)
        {
        }

        protected void dtvCustomerMarkUp_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
        }

        protected void dtvCustomerMarkUp_ItemCreated(object sender, EventArgs e)
        {
        }

        protected void DetailsDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (dtvCustomerMarkUp.CurrentMode != DetailsViewMode.Insert)
            {
                using (var _ctx = new QuotationDataContext())
                {
                    if (_ctx.CustomersMarkUps.SingleOrDefault(cmu => cmu.Code == Int32.Parse(CustomerParameter)) == null)
                    {
                        var _newItem = new CustomersMarkUp();
                        _newItem.Code = Int32.Parse(CustomerParameter);
                        _newItem.MarkUp = 140;
                        _newItem.Distance = 0;
                        _ctx.CustomersMarkUps.InsertOnSubmit(_newItem);
                        _ctx.SubmitChanges();
                        Response.Redirect("CustomerMarkUpPopup.aspx?" + CustomerKey + "=" + CustomerParameter, true);
                    }
                }

                var table = DetailsDataSource.GetTable();
                var _qc = (QuotationDataContext) table.CreateContext();
                e.Result = _qc.CustomersMarkUps.Where(po => po.Code == Int32.Parse(CustomerParameter));
            }
        }

        protected void lbtPrintCustomer_Click(object sender, EventArgs e)
        {
            var table = DetailsDataSource.GetTable();
            var _qc = (QuotationDataContext) table.CreateContext();
            //PrintProductionOrder(_qc);
        }

        protected void DetailsDataSource_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
            var _context = new QuotationDataContext();
            var _newQuotation =
                _context.Quotations.SingleOrDefault(q => q.ID == ((ProductionOrder) e.Result).ID_Quotation);
            //if (_newQuotation.Q3 == -1 && _newQuotation.Q4 == -1 && _newQuotation.Q5 == -1)
            if (((ProductionOrder) e.Result).Price != null)
            {
                _newQuotation.Subject = ((ProductionOrder) e.Result).Description + " (Automatico da OdP " +
                                        ((ProductionOrder) e.Result).Number + " [" +
                                        ((ProductionOrder) e.Result).ID + "])";
                _context.SubmitChanges();
            }

            Response.Redirect("CustomerMarkUpPopup.aspx?" + CustomerKey + "=" +
                              ((CustomersMarkUp) e.Result).Code);
        }

        protected void DetailsDataSource_Updating(object sender, LinqDataSourceUpdateEventArgs e)
        {
        }

        protected void dtvCustomerMarkUp_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            if (e.NewValues["MarkUp"] == null)
            {
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.QuantityIsMandatory);
                e.Cancel = true;
            }
        }

        protected void dtvCustomerMarkUp_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            if (e.Values["MarkUp"] == null)
            {
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.QuantityIsMandatory);
                e.Cancel = true;
            }
        }

        protected void ldsCustomerNicknames_Inserting(object sender, LinqDataSourceInsertEventArgs e)
        {

        }

        protected void ldsCustomerNicknames_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (dtvCustomerNickname.CurrentMode != DetailsViewMode.Insert)
            {
                using (var _ctx = new QuotationDataContext())
                {
                    if (_ctx.CustomerNicknames.SingleOrDefault(cmu => cmu.Code == Int32.Parse(CustomerParameter)) == null)
                    {
                        var _newItem = new CustomerNickname();
                        _newItem.Code = Int32.Parse(CustomerParameter);
                        _newItem.Nickname = _ctx.Customers.First(d => d.Code == _newItem.Code).Name;
                        _ctx.CustomerNicknames.InsertOnSubmit(_newItem);
                        _ctx.SubmitChanges();
                        Response.Redirect("CustomerMarkUpPopup.aspx?" + CustomerKey + "=" + CustomerParameter, true);
                    }
                }

                var table = ldsCustomerNicknames.GetTable();
                var _qc = (QuotationDataContext)table.CreateContext();
                e.Result = _qc.CustomerNicknames.Where(po => po.Code == Int32.Parse(CustomerParameter));
            }
        }

        protected void ldsCustomerNicknames_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {
        }

        protected void ldsCustomerNicknames_Updating(object sender, LinqDataSourceUpdateEventArgs e)
        {

        }

        protected void dtvCustomerNickname_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {
            if (e.NewMode == DetailsViewMode.Insert)
            {
                lblItemNo.Text = " [Nuovo]";
            }
            else
            {
                lblItemNo.Text = " No " + Request.QueryString[CustomerKey];
            }
        }

        protected void dtvCustomerNickname_PreRender(object sender, EventArgs e)
        {
            if (dtvCustomerNickname.CurrentMode == DetailsViewMode.Insert)
            {
                var _dvr = dtvCustomerNickname.Rows[0];

                if (Int32.Parse(CustomerParameter) != -1)
                {
                    var _dyc = (DynamicControl)_dvr.FindControl("dycCustomer");
                    ((DropDownList)_dyc.Controls[0].Controls[0]).SelectedValue = CustomerParameter.Trim();
                }
            }
            else
            {
                dtvCustomerNickname.DataBind();
            }
        }

        protected void dtvCustomerNickname_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {

        }

        protected void dtvCustomerNickname_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            dtvCustomerNickname.Visible = false;
            lblItemNo.Visible = false;
        }

        protected void dtvCustomerNickname_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {

        }

        protected void dtvCustomerNickname_ItemCreated(object sender, EventArgs e)
        {

        }

        protected void dtvCustomerNickname_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {

        }

        protected void dtvCustomerNickname_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {

        }
    }
}