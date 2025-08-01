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
    public partial class CustomerOrdersConsole : QuotationController
    {
        //protected List<SPCustomerOrderCalculated> m_calculated;

        protected Mode CurCustomerOrdersConsoleMode
        {
            get
            {
                if (ViewState["CurCustomerOrdersConsoleMode"] == null)
                {
                    return Mode.InputItems;
                }
                return (Mode) ViewState["CurCustomerOrdersConsoleMode"];
            }
            set { ViewState["CurCustomerOrdersConsoleMode"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(grdCustomerOrders);
            //DynamicDataManager1.RegisterControl(grdQuotationResults);
            itbNo.SearchClick += StartSearch;
        }

        public void StartSearch(object sender, EventArgs e)
        {
            if (itbNo.ReturnValue != 0)
            {
                GridDataSource.AutoGenerateWhereClause = false;
                GridDataSource.WhereParameters.Clear();
                GridDataSource.WhereParameters.Add("ID", DbType.Int32, itbNo.ReturnValue.ToString());
                GridDataSource.Where = "ID = @ID";
                grdCustomerOrders.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //MetaTable table = GridDataSource.GetTable();
            //InitSelectionData();

            if (!IsPostBack)
            {
                FillControls();
                SwitchDependingControls(CurCustomerOrdersConsoleMode);
            }
            FillDependingControls(CurCustomerOrdersConsoleMode);
        }

        private void FillControls()
        {
            ddlOrderBy.Items.Add(new ListItem("Cliente", "CustomerName"));
            ddlOrderBy.Items.Add(new ListItem("Stato", "StatusName"));
            ddlOrderBy.Items.Add(new ListItem("Riferimento Cliente", "CustomerOrderCode"));
            ddlOrderBy.Items.Add(new ListItem("Data creazione", "OrderDate"));
            ddlOrderBy.DataBind();

            lbtNewItem.Attributes.Add("onclick", "javascript:OpenItem('CustomerOrderPopup.aspx?ID=-1')");
        }

        protected void FillDependingControls(Mode mode)
        {
            //GridDataSource.WhereParameters.Clear();
            //GridDataSource.WhereParameters.Add("ID", DbType.Int32, SelectionData.ToString());
            //GridDataSource.WhereParameters.Add("ID", DbType.Int32, QuotationParameter);
            //GridDataSource.Where = filterString;
            //GridDataSource.AutoGenerateWhereClause = false;

            //GridDataSource.DataBind();

            //using (QuotationDataContext _qc = new QuotationDataContext())
            //{
            //m_calculated = _qc.prc_LAB_MGet_LAB_CustomerOrders().ToList<SPCustomerOrderCalculated>();
            //}

            //switch (mode)
            //{
            //    case Mode.InputItems:

            //string filterString = "ID_Quotation = @ID";

            //GridDataSource.WhereParameters.Clear();
            ////GridDataSource.WhereParameters.Add("ID", DbType.Int32, SelectionData.ToString());
            //GridDataSource.WhereParameters.Add("ID", DbType.Int32, QuotationParameter);
            //GridDataSource.Where = filterString;
            //GridDataSource.AutoGenerateWhereClause = false;
            //GridDataSource.DataBind();
            //    break;

            //case Mode.Calculation:

            //    using (QuotationDataContext _qc = new QuotationDataContext())
            //    {
            //        grdQuotationCalculatedDetails.DataSource = null;
            //        m_calculatedItems = _qc.prc_LAB_MGet_LAB_CalculatedDetailsByQuotationID(Convert.ToInt32(QuotationParameter));
            //        grdQuotationCalculatedDetails.DataSource = m_calculatedItems;
            //        grdQuotationCalculatedDetails.DataBind();
            //    //}
            ////    break;
            ////case Mode.Results:

            //    //using (QuotationDataContext _qc = new QuotationDataContext())
            //    //{
            //        grdQuotationCalculatedDetails.DataSource = null;
            //        m_calculatedResults = _qc.prc_LAB_MGet_LAB_CalculatedResultsByQuotationID(Convert.ToInt32(QuotationParameter));
            //        grdQuotationResults.DataSource = m_calculatedResults;
            //        grdQuotationResults.DataBind();
            //    }
            //    break;

            //default:
            //    break;

            //}

            //SwitchDependingControls(mode);
        }

        protected void GetModeDescription(Mode mode)
        {
            switch (mode)
            {
                case Mode.InputItems:
                    lblModeDescription.Text = "(gestione voci attive)";
                    break;
                case Mode.Calculation:
                    lblModeDescription.Text = "(prospetto calcolato)";
                    break;
                case Mode.DeactivatedItems:
                    lblModeDescription.Text = "(gestione voci disattivate)";
                    break;
                default:
                    lblModeDescription.Text = "";
                    break;
            }
        }

        protected void SwitchDependingControls(Mode mode)
        {
            ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.Empty);
            grdCustomerOrders.Visible = mode == Mode.InputItems || mode == Mode.DeactivatedItems;
            grdCalculated.Visible = mode == Mode.Calculation;
            lblddlOrderBy.Visible = (mode != Mode.Calculation);
            ddlOrderBy.Visible = (mode != Mode.Calculation);
            CurCustomerOrdersConsoleMode = mode;
            GetModeDescription(CurCustomerOrdersConsoleMode);
        }

        protected void lbtViewInputItems_Click(object sender, EventArgs e)
        {
            //GridDataSource.WhereParameters["Inserted"].DefaultValue = true.ToString();
            grdCustomerOrders.AutoGenerateDeleteButton = false;
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
            grdCustomerOrders.AutoGenerateDeleteButton = true;
            SwitchDependingControls(Mode.DeactivatedItems);
            OnFilterSelectedIndexChanged(null, null);
        }

        protected void grdCalculated_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCalculated.PageIndex = e.NewPageIndex;
            SwitchDependingControls(Mode.Calculation);
        }

        protected void grdCustomerOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCustomerOrders.PageIndex = e.NewPageIndex;
            //SwitchDependingControls(Mode.InputItems);
            SwitchDependingControls(CurCustomerOrdersConsoleMode);
        }

        protected void CalculatedDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            //e.Result = m_calculated;
            if (ddlOrderBy.SelectedValue == "")
            {
                CalculatedDataSource.OrderByParameters.Clear();
                CalculatedDataSource.AutoGenerateOrderByClause = false;
                var table = CalculatedDataSource.GetTable();
                var _qc = (QuotationDataContext) table.CreateContext();
                //e.Result = m_calculated.OrderBy(pi => pi.TypeCode).ThenBy(pi => pi.ItemTypeCode).ThenBy(pi => pi.ItemDescription);

                //.Join<PickingItem, DLLabExtim.Type, int, PickingItem>(_qc.Types, pi => pi.TypeCode, o => o.Code, (o, e2) => o)
                //.Join<PickingItem, DLLabExtim.ItemType, int, PickingItem>(_qc.ItemTypes, pi => pi.ItemTypeCode, o => o.Code, (o, e2) => o);
            }
        }

        protected void lbtPrintCustomerOrders_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("{2}?{0}={1}", GenericReportKey, "CustomerOrders", GenericPrintPage), true);
        }

        protected void OnFilterSelectedIndexChanged(object sender, EventArgs e)
        {
            grdCustomerOrders.EditIndex = -1;
            grdCustomerOrders.PageIndex = 0;
        }

        //protected void ddlTypes_DataBound(object sender, EventArgs e)
        //{
        //    ddlTypes.Items.Insert(0, new ListItem("Tutti", ""));
        //    //using (QuotationDataContext _qc = new QuotationDataContext())
        //    //{
        //    //    DLLabExtim.Type _firstType = _qc.Types.OrderBy(t => t.Order).FirstOrDefault();
        //    //    ddlTypes.Items.FindByValue(_firstType.Code.ToString()).Selected = true;
        //    //}
        //    if (Session["CustomerOrdersTypesSelector"] != null)
        //    {
        //        ddlTypes.Items.FindByValue(Session["CustomerOrdersTypesSelector"].ToString()).Selected = true;
        //    }
        //    else
        //    {
        //        using (QuotationDataContext _qc = new QuotationDataContext())
        //        {
        //            DLLabExtim.Type _firstType = _qc.Types.OrderBy(t => t.Order).FirstOrDefault();
        //            ddlTypes.Items.FindByValue(_firstType.Code.ToString()).Selected = true;
        //        }
        //    }


        //}

        protected void ddlStatuses_DataBound(object sender, EventArgs e)
        {
            ddlStatuses.Items.Insert(0, new ListItem("Tutti", ""));
            if (Session["CustomerOrdersStatusesSelector"] != null)
            {
                ddlStatuses.Items.FindByValue(Session["CustomerOrdersStatusesSelector"].ToString()).Selected = true;
            }
        }

        protected void ddlCustomers_DataBound(object sender, EventArgs e)
        {
            ddlCustomers.Items.Insert(0, new ListItem("Tutti", ""));
            if (Session["CustomerOrdersSuppliersSelector"] != null)
            {
                ddlCustomers.Items.FindByValue(Session["CustomerOrdersSuppliersSelector"].ToString()).Selected = true;
            }
        }

        protected void ddlOrderBy_DataBound(object sender, EventArgs e)
        {
            if (Session["CustomerOrdersOrderBySelector"] != null)
            {
                ddlOrderBy.Items.FindByValue(Session["CustomerOrdersOrderBySelector"].ToString()).Selected = true;
            }
        }

        protected void GridDataSource_Selected(object sender, LinqDataSourceStatusEventArgs e)
        {
            //if (e.TotalRowCount == 0 && grdCustomerOrders.PageIndex == 0)
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

        protected void OnGridViewDataBound(object sender, EventArgs e)
        {
            //if (grdCustomerOrders.Rows.Count == 0 && grdCustomerOrders.PageIndex == 0)
            //{
            //    grdCustomerOrders.ChangeMode(DetailsViewMode.Insert);
            //}
        }

        protected void grdCustomerOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";

                //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.grdPickingItems, "Select$" + e.Row.RowIndex);

                //GridViewRow _row = (GridViewRow)e.Row;
                //if (((CustomerOrder)e.Row.DataItem).Inserted == false)
                //{ _row.ForeColor = System.Drawing.Color.Red; }

                var _hypEdit = (HyperLink) e.Row.Cells[0].FindControl("hypEdit");
                _hypEdit.Attributes.Add("onclick",
                    "javascript:OpenItem('CustomerOrderPopup.aspx?ID=" + ((CustomerOrder) e.Row.DataItem).ID + "')");
            }
        }

        protected void GridDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var table = GridDataSource.GetTable();
            var _qc = (QuotationDataContext) table.CreateContext();
            switch (ddlOrderBy.SelectedValue)
            {
                //case (""):
                //    GridDataSource.OrderByParameters.Clear();
                //    GridDataSource.AutoGenerateOrderByClause = false;
                //    e.Result = _qc.CustomerOrders.OrderBy(pi => pi.Type.Order).ThenBy(pi => pi.ItemType.Order).ThenBy(pi => pi.Order);
                //    break;
                case ("StatusName"):
                    GridDataSource.OrderByParameters.Clear();
                    GridDataSource.AutoGenerateOrderByClause = false;
                    e.Result = _qc.CustomerOrders.OrderBy(qt => qt.Statuse.Description);
                    break;
                case ("CustomerName"):
                    GridDataSource.OrderByParameters.Clear();
                    GridDataSource.AutoGenerateOrderByClause = false;
                    e.Result = _qc.CustomerOrders.OrderBy(qt => qt.Customer.Name);
                    break;
                default:
                    break;
            }
        }

        protected void grdCustomerOrders_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.CannotDelete);
                e.ExceptionHandled = true;
            }
        }

        protected void PersistSelection(object sender, EventArgs e)
        {
            //Session["CustomerOrdersTypesSelector"] = ddlTypes.SelectedValue;
            Session["CustomerOrdersStatusesSelector"] = ddlStatuses.SelectedValue;
            Session["CustomerOrdersSuppliersSelector"] = ddlCustomers.SelectedValue;
            Session["CustomerOrdersOrderBySelector"] = ddlOrderBy.SelectedValue;
        }

        protected void grdCustomerOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Reload")
            {
                grdCustomerOrders.DataBind();
            }
        }

        protected enum Mode
        {
            InputItems,
            Calculation,
            DeactivatedItems
        }
    }

    public class CaseInsensitiveComparer3 : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return string.Compare(x, y, true);
        }
    }
}