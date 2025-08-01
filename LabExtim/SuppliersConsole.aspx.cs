using System;
using System.Data;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class SuppliersConsole : BaseController
    {
        //protected List<SPCustomerCalculated> m_calculated;

        protected Mode CurSuppliersConsoleMode
        {
            get
            {
                if (ViewState["CurSuppliersConsoleMode"] == null)
                {
                    return Mode.InputItems;
                }
                return (Mode) ViewState["CurSuppliersConsoleMode"];
            }
            set { ViewState["CurSuppliersConsoleMode"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(grdSuppliers);
            senMain.SearchClick += senMain_SearchClick;
        }

        public void senMain_SearchClick(object sender, EventArgs e)
        {
            ldsSuppliers.AutoGenerateWhereClause = false;

            ldsSuppliers.WhereParameters.Clear();
            //var _filter = "TRUE ";
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
                ldsSuppliers.WhereParameters.Add("Code", DbType.Int32, senMain.ItbNo.ReturnValue.ToString());
                _filter += " AND Code = @Code";
            }

            if (senMain.TextField1Text != string.Empty)
            {
                ldsSuppliers.WhereParameters.Add("Name", DbType.String, senMain.TextField1Text);
                _filter += " AND Name.Contains(@Name)";
            }
            //if (senMain.TextField2Text != string.Empty)
            //{
            //    ldsSuppliers.WhereParameters.Add("Contact", DbType.String, senMain.TextField2Text);
            //    _filter += " AND Contact.Contains(@Contact)";
            //}

            if (senMain.DropDownList1.SelectedValue != string.Empty)
            {
                ldsSuppliers.WhereParameters.Add("Province", DbType.String, senMain.DropDownList1.SelectedValue);
                _filter += " AND Province == @Province";
            }
            if (senMain.DropDownList2.SelectedValue != string.Empty)
            {
                ldsSuppliers.WhereParameters.Add("City", DbType.String, senMain.DropDownList2.SelectedValue);
                _filter += " AND City == @City";
            }
            if (_filter != "TRUE ")
                ldsSuppliers.Where = _filter;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //MetaTable table = ldsSuppliers.GetTable();
            //InitSelectionData();

            if (!IsPostBack)
            {
                FillControls();
                PopulateSearchEngine();
                SwitchDependingControls(CurSuppliersConsoleMode);
            }
            FillDependingControls(CurSuppliersConsoleMode);
            senMain_SearchClick(null, null);
        }

        private void PopulateSearchEngine()
        {
            senMain.LblTextField1Text = "Ragione sociale contiene...";
            //senMain.LblTextField2Text = "Contatto contiene...";
            //senMain.LblDateFromText = "Data lancio da";
            //senMain.LblDateToText = "Data lancio a";

            using (var _qc = new QuotationDataContext())
            {
                senMain.LblDropDownList1Text = "Provincia";
                var _items1 =
                    _qc.Suppliers.Select(s => new ListItem {Text = s.Province, Value = s.Province}).Distinct().ToArray();
                senMain.DropDownList1.Items.AddRange(_items1);
                senMain.DropDownList1.Items.Insert(0, new ListItem("Tutti", ""));

                senMain.LblDropDownList2Text = "Località";
                var _items2 =
                    _qc.Suppliers.Select(s => new ListItem {Text = s.City, Value = s.City}).Distinct().ToArray();
                senMain.DropDownList2.Items.AddRange(_items2);
                senMain.DropDownList2.Items.Insert(0, new ListItem("Tutti", ""));
            }

            senMain.DdlOrderBy.Items.Add(new ListItem("Ragione Sociale", "SupplierName"));
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
            CurSuppliersConsoleMode = mode;
        }

        protected void grdCalculated_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //grdCalculated.PageIndex = e.NewPageIndex;
            SwitchDependingControls(Mode.Calculation);
        }

        protected void grdSuppliers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdSuppliers.PageIndex = e.NewPageIndex;
            //SwitchDependingControls(Mode.InputItems);
            SwitchDependingControls(CurSuppliersConsoleMode);
        }

        protected void lbtPrintSuppliers_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("{2}?{0}={1}", GenericReportKey, "Suppliers", GenericPrintPage), true);
        }

        protected void OnFilterSelectedIndexChanged(object sender, EventArgs e)
        {
            grdSuppliers.EditIndex = -1;
            grdSuppliers.PageIndex = 0;
        }

        protected void ldsSuppliers_Selected(object sender, LinqDataSourceStatusEventArgs e)
        {
            //if (e.TotalRowCount == 0 && grdSuppliers.PageIndex == 0)
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

        protected void grdSuppliers_DataBound(object sender, EventArgs e)
        {
            //if (grdSuppliers.Rows.Count == 0 && grdSuppliers.PageIndex == 0)
            //{
            //    grdSuppliers.ChangeMode(DetailsViewMode.Insert);
            //}
        }

        protected void grdSuppliers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";

                //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.grdPickingItems, "Select$" + e.Row.RowIndex);

                //GridViewRow _row = (GridViewRow)e.Row;
                //if (((Customer)e.Row.DataItem).Inserted == false)
                //{ _row.ForeColor = System.Drawing.Color.Red; }

                //HyperLink _hypEdit = (HyperLink)e.Row.Cells[0].FindControl("hypEdit");
                //_hypEdit.Attributes.Add("onclick", "javascript:OpenItem('CustomerMarkUpPopup.aspx?" + CustomerKey + "=" + ((Customer)e.Row.DataItem).Code.ToString() + "')");
                //ImageButton _ibtUpdate = (ImageButton)e.Row.Cells[0].FindControl("ibtUpdate");
            }
        }

        protected void ldsSuppliers_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var table = ldsSuppliers.GetTable();
            var _qc = (QuotationDataContext) table.CreateContext();
            switch (senMain.DdlOrderBy.SelectedValue)
            {
                //case (""):
                //    ldsSuppliers.OrderByParameters.Clear();
                //    ldsSuppliers.AutoGenerateOrderByClause = false;
                //    e.Result = _qc.Suppliers.OrderBy(pi => pi.Type.Order).ThenBy(pi => pi.ItemType.Order).ThenBy(pi => pi.Order);
                //    break;
                case ("City"):
                    ldsSuppliers.OrderByParameters.Clear();
                    ldsSuppliers.AutoGenerateOrderByClause = false;
                    e.Result = _qc.Suppliers.OrderBy(qt => qt.City);
                    break;
                case ("SupplierName"):
                    ldsSuppliers.OrderByParameters.Clear();
                    ldsSuppliers.AutoGenerateOrderByClause = false;
                    e.Result = _qc.Suppliers.OrderBy(qt => qt.Name);
                    break;

                default:
                    break;
            }
        }

        protected void grdSuppliers_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.CannotDelete);
                e.ExceptionHandled = true;
            }
        }

        protected void PersistSelection(object sender, EventArgs e)
        {
            //Session["SuppliersTypesSelector"] = ddlTypes.SelectedValue;
            //Session["SuppliersStatusesSelector"] = ddlStatuses.SelectedValue;
            //Session["SuppliersSuppliersSelector"] = ddlSuppliers.SelectedValue;
            //Session["SuppliersOrderBySelector"] = ddlOrderBy.SelectedValue;
        }

        protected void grdSuppliers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //int _destinationId = Convert.ToInt32(e.CommandArgument.ToString() == string.Empty ? 0.ToString() : e.CommandArgument.ToString());
            if (e.CommandName == "Reload")
            {
                //grdSuppliers.DataBind();
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

        protected void grdSuppliers_PreRender(object sender, EventArgs e)
        {
            grdSuppliers.DataBind();
        }

        protected enum Mode
        {
            InputItems,
            Calculation,
            DeactivatedItems
        }
    }

    //public class CaseInsensitiveComparer11 : IComparer<string> { public int Compare(string x, string y) { return string.Compare(x, y, true); } }
}