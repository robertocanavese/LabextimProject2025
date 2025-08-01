using System;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class LocationPopup : BaseController
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            //DynamicDataManager1.RegisterControl(GridView1, false /*setSelectionFromUrl*/);
            DynamicDataManager1.RegisterControl(dtvLocation);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblItemNo.Text = Int32.Parse(LocationParameter) == -1 ? " [Nuovo]" : " No " + LocationParameter;
            if (Int32.Parse(LocationParameter) == -1)
            {
                lblItemNo.Text = " [Nuovo]";
                DetailsDataSource.WhereParameters.Clear();
                dtvLocation.ChangeMode(DetailsViewMode.Insert);
            }
            else
            {
                dtvLocation.AutoGenerateInsertButton = false;
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
            dtvLocation.Visible = false;
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
                lblItemNo.Text = " No " + Request.QueryString[LocationKey];
            }
        }

        protected void OnDetailsViewPreRender(object sender, EventArgs e)
        {
             

            if (dtvLocation.CurrentMode == DetailsViewMode.Insert || dtvLocation.CurrentMode == DetailsViewMode.Edit)
            {
                var _dvr = dtvLocation.Rows[0];

                var _dycCompany = (DynamicControl)_dvr.FindControl("dycCompany");
                ((DropDownList)_dycCompany.Controls[0].Controls[0]).SelectedValue = CurrentCompanyId.ToString();
                ((DropDownList)_dycCompany.Controls[0].Controls[0]).Enabled = false;

                //if (Int32.Parse(LocationParameter) != -1)
                //{
                //    var _dyc = (DynamicControl) _dvr.FindControl("dycLocation");
                //    ((DropDownList) _dyc.Controls[0].Controls[0]).SelectedValue = LocationParameter.Trim();
                //}
            }
            else
            {
                dtvLocation.DataBind();
            }
        }

        private void SetDeleteConfirmation(TableRow row)
        {
            foreach (Control c in row.Cells[0].Controls)
            {
                if (c is LinkButton)
                {
                    var btn = (LinkButton)c;
                    if (btn.CommandName == DataControlCommands.DeleteCommandName)
                    {
                        btn.OnClientClick = "return confirm('Sei sicuro di voler eliminare questa voce?');";
                    }
                }
            }
        }

        protected void DetailsDataSource_Inserting(object sender, LinqDataSourceInsertEventArgs e)
        {
            if (((Location)e.NewObject).Type == null)
            {
                ((Location)e.NewObject).Type = "D";
            }

        }

        protected void dtvLocation_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
        }

        protected void dtvLocation_ItemCreated(object sender, EventArgs e)
        {
        }

        protected void DetailsDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (dtvLocation.CurrentMode != DetailsViewMode.Insert)
            {
                using (var _ctx = new QuotationDataContext())
                {
                    if (_ctx.Locations.SingleOrDefault(cmu => cmu.Code == Int32.Parse(LocationParameter)) == null)
                    {
                        var _newItem = new Location();
                        _newItem.Code = Int32.Parse(LocationParameter);
                        _newItem.Distance = 0;
                        _ctx.Locations.InsertOnSubmit(_newItem);
                        _ctx.SubmitChanges();
                        Response.Redirect("LocationPopup.aspx?" + LocationKey + "=" + LocationParameter, true);
                    }
                }

                var table = DetailsDataSource.GetTable();
                var _qc = (QuotationDataContext)table.CreateContext();
                e.Result = _qc.Locations.Where(po => po.Code == Int32.Parse(LocationParameter));
            }
        }

        protected void lbtPrintLocation_Click(object sender, EventArgs e)
        {
            var table = DetailsDataSource.GetTable();
            var _qc = (QuotationDataContext)table.CreateContext();
            //PrintProductionOrder(_qc);
        }

        protected void DetailsDataSource_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        {

            Response.Redirect("LocationPopup.aspx?" + LocationKey + "=" +
                              ((Location)e.Result).Code);
        }

        protected void DetailsDataSource_Updating(object sender, LinqDataSourceUpdateEventArgs e)
        {
        }

        protected void dtvLocation_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            //if (e.NewValues["Distance"] == null)
            //{
            //    ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.QuantityIsMandatory);
            //    e.Cancel = true;
            //}
        }

        protected void dtvLocation_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            //if (e.Values["Distance"] == null)
            //{
            //    ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.QuantityIsMandatory);
            //    e.Cancel = true;
            //}
        }
    }
}