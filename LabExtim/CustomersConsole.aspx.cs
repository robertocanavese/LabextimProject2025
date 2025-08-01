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
    public partial class CustomerConsole : BaseController
    {
        //protected List<SPCustomerCalculated> m_calculated;

        protected Mode CurCustomersConsoleMode
        {
            get
            {
                if (ViewState["CurCustomersConsoleMode"] == null)
                {
                    return Mode.InputItems;
                }
                return (Mode) ViewState["CurCustomersConsoleMode"];
            }
            set { ViewState["CurCustomersConsoleMode"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(grdCustomers);
            senMain.SearchClick += senMain_SearchClick;
        }

        public void senMain_SearchClick(object sender, EventArgs e)
        {
            ldsCustomers.AutoGenerateWhereClause = false;

            ldsCustomers.WhereParameters.Clear();
            var _filter = "";

            if (CurrentCompanyId == 1)
            {
                _filter += "Code >= 1 && Code <= 199999999 ";
            }
            if (CurrentCompanyId == 2)
            {
                _filter += "Code >= 200000000 && Code <= 299999999 ";
            }


            if (senMain.ItbNo.ReturnValue != 0)
            {
                ldsCustomers.WhereParameters.Add("Code", DbType.Int32, senMain.ItbNo.ReturnValue.ToString());
                _filter += " AND Code = @Code";
            }

            if (senMain.TextField1Text != string.Empty)
            {
                ldsCustomers.WhereParameters.Add("Name", DbType.String, senMain.TextField1Text);
                _filter += " AND Name.Contains(@Name)";
            }
            if (senMain.TextField2Text != string.Empty)
            {
                ldsCustomers.WhereParameters.Add("CAP", DbType.String, senMain.TextField2Text);
                _filter += " AND CAP.Contains(@CAP)";
            }

            if (senMain.DropDownList1.SelectedValue != string.Empty)
            {
                ldsCustomers.WhereParameters.Add("Province", DbType.String, senMain.DropDownList1.SelectedValue);
                _filter += " AND Province == @Province";
            }
            if (senMain.DropDownList2.SelectedValue != string.Empty)
            {
                ldsCustomers.WhereParameters.Add("City", DbType.String, senMain.DropDownList2.SelectedValue);
                _filter += " AND City == @City";
            }

            if (senMain.DropDownAgenti.SelectedValue != string.Empty)
            {
                ldsCustomers.WhereParameters.Add("IDAgente1", DbType.Int32, senMain.DropDownAgenti.SelectedValue);
                _filter += " AND IDAgente1 == @IDAgente1";
            }

            if (_filter != "TRUE ")
                ldsCustomers.Where = _filter;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //MetaTable table = ldsCustomers.GetTable();
            //InitSelectionData();

            if (!IsPostBack)
            {
                FillControls();
                PopulateSearchEngine();
                SwitchDependingControls(CurCustomersConsoleMode);
            }
            FillDependingControls(CurCustomersConsoleMode);
            senMain_SearchClick(null, null);
        }

        private void PopulateSearchEngine()
        {
            senMain.LblTextField1Text = "Ragione sociale contiene...";
            senMain.LblTextField2Text = "CAP contiene...";
            

            //senMain.LblDateFromText = "Data lancio da";
            //senMain.LblDateToText = "Data lancio a";

            using (var _qc = new QuotationDataContext())
            {
                senMain.LblDropDownList1Text = "Provincia";
                var _items1 =
                    _qc.Customers.Select(s => new ListItem {Text = s.Province, Value = s.Province}).Distinct().ToArray();
                senMain.DropDownList1.Items.AddRange(_items1);
                senMain.DropDownList1.Items.Insert(0, new ListItem("Tutti", ""));

                senMain.LblDropDownList2Text = "Località";
                var _items2 =
                    _qc.Customers.Select(s => new ListItem {Text = s.City, Value = s.City}).Distinct().ToArray();
                senMain.DropDownList2.Items.AddRange(_items2);
                senMain.DropDownList2.Items.Insert(0, new ListItem("Tutti", ""));

                senMain.LblAgente = "Agenti";
                var _items3 =
                    _qc.Customers.Where(x=>x.IDAgente1 != 0).Select(s => new ListItem { Text = s.DescrizioneAgente1, Value = s.IDAgente1.ToString() }).Distinct().ToArray();
                senMain.DropDownAgenti.Items.AddRange(_items3);
                senMain.DropDownAgenti.Items.Insert(0, new ListItem("Tutti", ""));
            }

            senMain.DdlOrderBy.Items.Add(new ListItem("Ragione Sociale", "CustomerName"));
            senMain.DdlOrderBy.Items.Add(new ListItem("Località", "City"));
        }

        private void FillControls()
        {
            //lbtNewItem.Attributes.Add("onclick", "javascript:OpenItem('CustomerMarkUpPopup.aspx?" + POIdKey + "=-1')");
        }

        protected void FillDependingControls(Mode mode)
        {
        }

        protected void SwitchDependingControls(Mode mode)
        {
            ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.Empty);
            CurCustomersConsoleMode = mode;
        }

        protected void grdCalculated_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //grdCalculated.PageIndex = e.NewPageIndex;
            SwitchDependingControls(Mode.Calculation);
        }

        protected void grdCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCustomers.PageIndex = e.NewPageIndex;
            //SwitchDependingControls(Mode.InputItems);
            SwitchDependingControls(CurCustomersConsoleMode);
        }

        protected void lbtPrintCustomers_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("{2}?{0}={1}", GenericReportKey, "Customers", GenericPrintPage), true);
        }

        protected void OnFilterSelectedIndexChanged(object sender, EventArgs e)
        {
            grdCustomers.EditIndex = -1;
            grdCustomers.PageIndex = 0;
        }

        protected void ldsCustomers_Selected(object sender, LinqDataSourceStatusEventArgs e)
        {
            //if (e.TotalRowCount == 0 && grdCustomers.PageIndex == 0)
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

        protected void grdCustomers_DataBound(object sender, EventArgs e)
        {
            //if (grdCustomers.Rows.Count == 0 && grdCustomers.PageIndex == 0)
            //{
            //    grdCustomers.ChangeMode(DetailsViewMode.Insert);
            //}
        }

        protected void grdCustomers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";

                //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.grdPickingItems, "Select$" + e.Row.RowIndex);

                //GridViewRow _row = (GridViewRow)e.Row;
                //if (((Customer)e.Row.DataItem).Inserted == false)
                //{ _row.ForeColor = System.Drawing.Color.Red; }

                var _hypEdit = (HyperLink) e.Row.Cells[0].FindControl("hypEdit");
                _hypEdit.Attributes.Add("onclick",
                    "javascript:OpenItem('CustomerMarkUpPopup.aspx?" + CustomerKey + "=" +
                    ((Customer) e.Row.DataItem).Code + "')");
                //ImageButton _ibtUpdate = (ImageButton)e.Row.Cells[0].FindControl("ibtUpdate");
            }
        }

        protected void ldsCustomers_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var table = ldsCustomers.GetTable();
            var _qc = (QuotationDataContext) table.CreateContext();
            switch (senMain.DdlOrderBy.SelectedValue)
            {
                //case (""):
                //    ldsCustomers.OrderByParameters.Clear();
                //    ldsCustomers.AutoGenerateOrderByClause = false;
                //    e.Result = _qc.Customers.OrderBy(pi => pi.Type.Order).ThenBy(pi => pi.ItemType.Order).ThenBy(pi => pi.Order);
                //    break;
                case ("City"):
                    ldsCustomers.OrderByParameters.Clear();
                    ldsCustomers.AutoGenerateOrderByClause = false;
                    e.Result = _qc.Customers.OrderBy(qt => qt.City);
                    break;
                case ("CustomerName"):
                    ldsCustomers.OrderByParameters.Clear();
                    ldsCustomers.AutoGenerateOrderByClause = false;
                    e.Result = _qc.Customers.OrderBy(qt => qt.Name);
                    break;

                default:
                    break;
            }
        }

        protected void grdCustomers_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.CannotDelete);
                e.ExceptionHandled = true;
            }
        }

        protected void PersistSelection(object sender, EventArgs e)
        {
            //Session["CustomersTypesSelector"] = ddlTypes.SelectedValue;
            //Session["CustomersStatusesSelector"] = ddlStatuses.SelectedValue;
            //Session["CustomersSuppliersSelector"] = ddlCustomers.SelectedValue;
            //Session["CustomersOrderBySelector"] = ddlOrderBy.SelectedValue;
        }

        protected void grdCustomers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //int _destinationId = Convert.ToInt32(e.CommandArgument.ToString() == string.Empty ? 0.ToString() : e.CommandArgument.ToString());
            if (e.CommandName == "Reload")
            {
                //grdCustomers.DataBind();
            }

            //if (e.CommandName == "GoToQuotation")
            //{
            //    using (QuotationDataContext _qc = new QuotationDataContext())
            //    {
            //        try
            //        {
            //            List<SPQDetailTotal> m_totals = _qc.prc_LAB_MGet_LAB_TotalsByQuotationID(Convert.ToInt32(_destinationId)).ToList<SPQDetailTotal>();
            //            Response.Redirect(string.Format("{2}?{0}={1}", QuotationKey, _destinationId, QuotationConsolePage), true);
            //        }
            //        catch
            //        {
            //            lblSuccess.Text = "Il preventivo No " + _destinationId + " è inutilizzabile: se ne consiglia l'eliminazione.";
            //        }

            //    }

            //}
        }

        protected void grdCustomers_PreRender(object sender, EventArgs e)
        {
            grdCustomers.DataBind();
        }

        protected enum Mode
        {
            InputItems,
            Calculation,
            DeactivatedItems
        }
    }

    public class CaseInsensitiveComparer10 : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return string.Compare(x, y, true);
        }
    }
}