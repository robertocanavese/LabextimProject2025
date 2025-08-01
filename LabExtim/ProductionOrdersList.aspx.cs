using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class ProductionOrdersList : ProductionOrderController
    {
        protected Mode CurProductionOrdersConsoleMode
        {
            get
            {
                if (ViewState["CurProductionOrdersListMode"] == null)
                {
                    return Mode.InputItems;
                }
                return (Mode) ViewState["CurProductionOrdersListMode"];
            }
            set { ViewState["CurProductionOrdersListMode"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(grdProductionOrders);
        }

        private void setupClientScript()
        {
            //if (Context.User.Identity.IsAuthenticated)
            //{
            var csm = Page.ClientScript;
            if (!csm.IsClientScriptIncludeRegistered("Site"))
            {
                csm.RegisterClientScriptInclude(Page.GetType(), "Site", "Site.js");
            }
            //}
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            setupClientScript();
            //MetaTable table = GridDataSource.GetTable();
            //InitSelectionData();

            if (!IsPostBack)
            {
                SwitchDependingControls(CurProductionOrdersConsoleMode);
            }
            FillDependingControls(CurProductionOrdersConsoleMode);
        }

        protected void FillDependingControls(Mode mode)
        {
        }

        protected void GetModeDescription(Mode mode)
        {
        }

        protected void SwitchDependingControls(Mode mode)
        {
            ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.Empty);
            grdProductionOrders.Visible = mode == Mode.InputItems || mode == Mode.DeactivatedItems;
            CurProductionOrdersConsoleMode = mode;
            GetModeDescription(CurProductionOrdersConsoleMode);
        }

        protected void grdCalculated_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //grdCalculated.PageIndex = e.NewPageIndex;
            SwitchDependingControls(Mode.Calculation);
        }

        protected void grdProductionOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdProductionOrders.PageIndex = e.NewPageIndex;
            //SwitchDependingControls(Mode.InputItems);
            SwitchDependingControls(CurProductionOrdersConsoleMode);
        }

        protected void lbtPrintProductionOrders_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("{2}?{0}={1}", GenericReportKey, "ProductionOrders", GenericPrintPage), true);
        }

        protected void OnFilterSelectedIndexChanged(object sender, EventArgs e)
        {
            grdProductionOrders.EditIndex = -1;
            grdProductionOrders.PageIndex = 0;
        }

        protected void ldsProductionOrders_Selected(object sender, LinqDataSourceStatusEventArgs e)
        {
        }

        protected void grdProductionOrders_DataBound(object sender, EventArgs e)
        {
            //if (grdProductionOrders.Rows.Count == 0 && grdProductionOrders.PageIndex == 0)
            //{
            //    grdProductionOrders.ChangeMode(DetailsViewMode.Insert);
            //}
        }

        protected void grdProductionOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";

                //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.grdPickingItems, "Select$" + e.Row.RowIndex);

                //GridViewRow _row = (GridViewRow)e.Row;
                //if (((ProductionOrder)e.Row.DataItem).Inserted == false)
                //{ _row.ForeColor = System.Drawing.Color.Red; }

                var _hypEdit = (HyperLink) e.Row.Cells[0].FindControl("hypEdit");
                _hypEdit.Attributes.Add("onclick",
                    "javascript:OpenBigItem('ProductionOrderPopup.aspx?" + POIdKey + "=" +
                    ((ProductionOrder) e.Row.DataItem).ID + "')");
                //ImageButton _ibtUpdate = (ImageButton)e.Row.Cells[0].FindControl("ibtUpdate");
            }
        }

        protected void ldsProductionOrders_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
        }

        protected void grdProductionOrders_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.CannotDelete);
                e.ExceptionHandled = true;
            }
        }

        protected void grdProductionOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var _destinationId =
                Convert.ToInt32(e.CommandArgument.ToString() == string.Empty
                    ? 0.ToString()
                    : e.CommandArgument.ToString());
            if (e.CommandName == "Reload")
            {
                //grdProductionOrders.DataBind();
            }
            if (e.CommandName == "Delete")
            {
                using (var _qc = new QuotationDataContext())
                {
                    var _toDeleteProductionOrder = _qc.ProductionOrders.Single(po => po.ID == _destinationId);
                    _qc.ProductionOrderDetails.DeleteAllOnSubmit(_toDeleteProductionOrder.ProductionOrderDetails);
                    _qc.ProductionMPs.DeleteAllOnSubmit(_toDeleteProductionOrder.ProductionMPs);
                    _qc.SubmitChanges();
                    _qc.ProductionOrders.DeleteOnSubmit(_toDeleteProductionOrder);
                    _qc.SubmitChanges();
                }
                //grdProductionOrders.DataBind();
            }
            if (e.CommandName == "Close")
            {
                using (var db = new QuotationDataContext())
                {
                    var toClose = db.ProductionOrders.Single(po => po.ID == _destinationId);
                    toClose.Status = 3;
                    ProductionOrderService.CloseProductionOrderSchedule(db, toClose.ID);
                    db.SubmitChanges();
                }
                //grdProductionOrders.DataBind();
            }
        }

        protected void grdProductionOrders_PreRender(object sender, EventArgs e)
        {
            grdProductionOrders.DataBind();
        }

        protected enum Mode
        {
            InputItems,
            Calculation,
            DeactivatedItems
        }
    }

    public class CaseInsensitiveComparer8 : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return string.Compare(x, y, true);
        }
    }
}