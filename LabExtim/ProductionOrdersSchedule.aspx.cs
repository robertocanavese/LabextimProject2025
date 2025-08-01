using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class ProductionOrdersSchedule : ProductionOrderController
    {
        //protected List<SPProductionOrderCalculated> m_calculated;

        protected Mode CurProductionOrdersConsoleMode
        {
            get
            {
                if (ViewState["CurProductionOrdersConsoleMode"] == null)
                {
                    return Mode.InputItems;
                }
                return (Mode) ViewState["CurProductionOrdersConsoleMode"];
            }
            set { ViewState["CurProductionOrdersConsoleMode"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(grdProductionOrders);
            //DynamicDataManager1.RegisterControl(grdQuotationResults);
            itbNo.SearchClick += StartSearch;
            yctNumber.SearchClick += StartSearch;
        }

        public void StartSearch(object sender, EventArgs e)
        {
            if (itbNo.ReturnValue != 0)
            {
                ldsProductionOrders.AutoGenerateWhereClause = false;
                ldsProductionOrders.WhereParameters.Clear();
                ldsProductionOrders.WhereParameters.Add("ID", DbType.Int32, itbNo.ReturnValue.ToString());
                ldsProductionOrders.Where = "ID = @ID";
                grdProductionOrders.DataBind();
            }
            if (yctNumber.ReturnValue != string.Empty)
            {
                ldsProductionOrders.AutoGenerateWhereClause = false;
                ldsProductionOrders.WhereParameters.Clear();
                ldsProductionOrders.WhereParameters.Add("Number", DbType.String, yctNumber.ReturnValue);
                ldsProductionOrders.Where = "Number.Contains(@Number)";
                grdProductionOrders.DataBind();
            }
            if (CurrentCompanyId != -1)
            {
                ldsProductionOrders.WhereParameters.Add("ID_Company", DbType.Int16, CurrentCompanyId.ToString());
                ldsProductionOrders.Where += " TRUE AND ID_Company = @ID_Company";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //MetaTable table = GridDataSource.GetTable();
            //InitSelectionData();

            if (!IsPostBack)
            {
                FillControls();
                SwitchDependingControls(CurProductionOrdersConsoleMode);
            }
            FillDependingControls(CurProductionOrdersConsoleMode);
        }

        private void FillControls()
        {
            //lbtNewItem.Attributes.Add("onclick", "javascript:OpenItem('ProductionOrderPopup.aspx?" + POIdKey + "=-1')");

            //if (UsageParameter == 1)
            //{
            itbNo.Enabled = false;
            yctNumber.Enabled = false;
            ddlStatuses.Enabled = false;
            ddlOrderBy.Items.Add(new ListItem("Priorità avanzamanto", "DeliveryDateForOutstanding"));
            lbtNewItem.Enabled = false;
            lbtPrintProductionOrders.Enabled = false;
            lbtViewInputItems.Enabled = false;
            lbtViewCalculated.Enabled = false;
            lbtViewDeactivatedItems.Enabled = false;
            grdProductionOrders.Columns[0].Visible = false;
            grdProductionOrders.Columns[1].Visible = false;
            grdProductionOrders.Columns[2].Visible = false;
            grdProductionOrders.Columns[3].Visible = false;
            //}
            //else
            //{
            //    ddlOrderBy.Items.Add(new ListItem("Data conferma", "DeliveryDate"));
            //    ddlOrderBy.Items.Add(new ListItem("Cliente", "CustomerName"));
            //    ddlOrderBy.Items.Add(new ListItem("Stato", "StatusName"));
            //    ddlOrderBy.Items.Add(new ListItem("Più recenti", "StartDate"));
            //}
            ddlOrderBy.DataBind();
        }

        protected void FillDependingControls(Mode mode)
        {
        }

        protected void GetModeDescription(Mode mode)
        {
            //if (UsageParameter == 1)
            //{
            lblModeDescription.Text = "(priorità avanzamento)";
            //}
            //else
            //{
            //    switch (mode)
            //    {
            //        case Mode.InputItems:
            //            lblModeDescription.Text = "(gestione voci attive)";
            //            break;
            //        case Mode.Calculation:
            //            lblModeDescription.Text = "(prospetto calcolato)";
            //            break;
            //        case Mode.DeactivatedItems:
            //            lblModeDescription.Text = "(gestione voci disattivate)";
            //            break;
            //        default:
            //            lblModeDescription.Text = "";
            //            break;
            //    }
            //}
        }

        protected void SwitchDependingControls(Mode mode)
        {
            ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.Empty);
            grdProductionOrders.Visible = mode == Mode.InputItems || mode == Mode.DeactivatedItems;
            //grdCalculated.Visible = mode == Mode.Calculation;
            lblddlOrderBy.Visible = (mode != Mode.Calculation);
            ddlOrderBy.Visible = (mode != Mode.Calculation);
            CurProductionOrdersConsoleMode = mode;
            GetModeDescription(CurProductionOrdersConsoleMode);
        }

        protected void lbtViewInputItems_Click(object sender, EventArgs e)
        {
            //GridDataSource.WhereParameters["Inserted"].DefaultValue = true.ToString();
            grdProductionOrders.AutoGenerateDeleteButton = false;
            SwitchDependingControls(Mode.InputItems);
            OnFilterSelectedIndexChanged(null, null);
        }

        protected void lbtViewCalculated_Click(object sender, EventArgs e)
        {
            //GridDataSource.WhereParameters["Inserted"].DefaultValue = "";
            SwitchDependingControls(Mode.Calculation);
            OnFilterSelectedIndexChanged(null, null);
        }

        protected void lbtViewDeactivatedItems_Click(object sender, EventArgs e)
        {
            //GridDataSource.WhereParameters["Inserted"].DefaultValue = false.ToString();
            grdProductionOrders.AutoGenerateDeleteButton = true;
            SwitchDependingControls(Mode.DeactivatedItems);
            OnFilterSelectedIndexChanged(null, null);
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

        protected void ddlStatuses_DataBound(object sender, EventArgs e)
        {
            ddlStatuses.Items.Insert(0, new ListItem("Tutti", ""));
            if (Session["SchedProductionOrdersStatusesSelector"] != null)
            {
                ddlStatuses.Items.FindByValue(Session["SchedProductionOrdersStatusesSelector"].ToString()).Selected =
                    true;
            }
        }

        protected void ddlCustomers_DataBound(object sender, EventArgs e)
        {
            ddlCustomers.Items.Insert(0, new ListItem("Tutti", ""));
            if (Session["SchedProductionOrdersSuppliersSelector"] != null)
            {
                ddlCustomers.Items.FindByValue(Session["SchedProductionOrdersSuppliersSelector"].ToString()).Selected =
                    true;
            }
        }

        protected void ddlOrderBy_DataBound(object sender, EventArgs e)
        {
            if (Session["SchedProductionOrdersOrderBySelector"] != null)
            {
                try
                {
                    ddlOrderBy.Items.FindByValue(Session["SchedProductionOrdersOrderBySelector"].ToString()).Selected =
                        true;
                }
                catch
                {
                }
            }
        }

        protected void ldsProductionOrders_Selected(object sender, LinqDataSourceStatusEventArgs e)
        {
            //if (e.TotalRowCount == 0 && grdProductionOrders.PageIndex == 0)
            //{
            //    System.Type typeList = e.Result.GetType(); //List<T> for a select statement
            //    System.Type typeObj = e.Result.GetType().GetGenericArguments()[0]; //<T>
            //    object ojb = Activator.CreateInstance(typeObj);  //new T
            //    // insert the new T into the list by using InvokeMember on the List<T>
            //    object result = null;
            //    object[] arguments = { 0, ojb };
            //    result = typeList.InvokeMember("Insert", BindingFlags.InvokeMethod, null, e.Result, arguments);
            //}
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

                //if (POUsageParameter == 1)
                //{
                //    _hypEdit.Visible = false;
                //    _ibtUpdate.Visible = false;
                //}
            }
        }

        protected void ldsProductionOrders_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var table = ldsProductionOrders.GetTable();
            var _qc = (QuotationDataContext) table.CreateContext();
            switch (ddlOrderBy.SelectedValue)
            {
                //case (""):
                //    GridDataSource.OrderByParameters.Clear();
                //    GridDataSource.AutoGenerateOrderByClause = false;
                //    e.Result = _qc.ProductionOrders.OrderBy(pi => pi.Type.Order).ThenBy(pi => pi.ItemType.Order).ThenBy(pi => pi.Order);
                //    break;
                case ("StatusName"):
                    ldsProductionOrders.OrderByParameters.Clear();
                    ldsProductionOrders.AutoGenerateOrderByClause = false;
                    e.Result = _qc.ProductionOrders.OrderBy(qt => qt.Statuse.Description);
                    break;
                case ("CustomerName"):
                    ldsProductionOrders.OrderByParameters.Clear();
                    ldsProductionOrders.AutoGenerateOrderByClause = false;
                    e.Result = _qc.ProductionOrders.OrderBy(qt => qt.Customer.Name);
                    break;
                case ("StartDate"):
                    ldsProductionOrders.OrderByParameters.Clear();
                    ldsProductionOrders.AutoGenerateOrderByClause = false;
                    e.Result = _qc.ProductionOrders.OrderByDescending(qt => qt.StartDate).ThenByDescending(qt => qt.ID);
                    break;
                case ("DeliveryDateForOutstanding"):
                    ldsProductionOrders.OrderByParameters.Clear();
                    ldsProductionOrders.AutoGenerateOrderByClause = false;
                    e.Result =
                        _qc.ProductionOrders.
                            OrderBy(qt => qt.DeliveryDate).
                            ThenBy(
                                qt =>
                                    qt.Quantity != null
                                        ? (qt.ProductionOrderDetails.Where(pod => pod.QuantityOver)
                                            .Sum(pod => pod.ProducedQuantity))/qt.Quantity
                                        : 1f)
                            .Where(qt => qt.Status == 1);
                    break;
                default:
                    break;
            }
        }

        protected void grdProductionOrders_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.CannotDelete);
                e.ExceptionHandled = true;
            }
        }

        protected void PersistSelection(object sender, EventArgs e)
        {
            //Session["SchedProductionOrdersTypesSelector"] = ddlTypes.SelectedValue;
            Session["SchedProductionOrdersStatusesSelector"] = ddlStatuses.SelectedValue;
            Session["SchedProductionOrdersSuppliersSelector"] = ddlCustomers.SelectedValue;
            Session["SchedProductionOrdersOrderBySelector"] = ddlOrderBy.SelectedValue;
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
                    ProductionOrderService.CloseProductionOrderSchedule(db, _destinationId);
                    db.SubmitChanges();
                }
                //grdProductionOrders.DataBind();
            }

            if (e.CommandName == "GoToQuotation")
            {
  
                using (var _qc = new QuotationDataContext())
                {
                    try
                    {
                        var m_totals =
                            _qc.prc_LAB_MGet_LAB_TotalsByQuotationID(Convert.ToInt32(_destinationId))
                                .ToList();
                        Response.Redirect(
                            string.Format("{2}?{0}={1}", QuotationKey, _destinationId, QuotationConsolePage), true);
                    }
                    catch
                    {
                        lblSuccess.Text = "Il preventivo No " + _destinationId +
                                          " è inutilizzabile: se ne consiglia l'eliminazione.";
                    }
                }
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

}