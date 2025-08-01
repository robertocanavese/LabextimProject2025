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
    public partial class LocationConsole : BaseController
    {


        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(grdLocations);
            senMain.SearchClick += senMain_SearchClick;
        }

        public void senMain_SearchClick(object sender, EventArgs e)
        {
            ldsLocations.AutoGenerateWhereClause = false;

            ldsLocations.WhereParameters.Clear();
            var _filter = "";

            ldsLocations.WhereParameters.Add("ID_Company", DbType.Int32, CurrentCompanyId.ToString());
            _filter += "ID_Company == @ID_Company ";

            if (senMain.ItbNo.ReturnValue != 0)
            {
                ldsLocations.WhereParameters.Add("Code", DbType.Int32, senMain.ItbNo.ReturnValue.ToString());
                _filter += " AND Code == @Code";
            }

            if (senMain.TextField1Text != string.Empty)
            {
                ldsLocations.WhereParameters.Add("Name", DbType.String, senMain.TextField1Text);
                _filter += " AND Name.Contains(@Name)";
            }
            if (senMain.TextField2Text != string.Empty)
            {
                ldsLocations.WhereParameters.Add("CAP", DbType.String, senMain.TextField2Text);
                _filter += " AND CAP.Contains(@CAP)";
            }

            if (senMain.DropDownList1.SelectedValue != string.Empty)
            {
                ldsLocations.WhereParameters.Add("Province", DbType.String, senMain.DropDownList1.SelectedValue);
                _filter += " AND Province == @Province";
            }
            if (senMain.DropDownList2.SelectedValue != string.Empty)
            {
                ldsLocations.WhereParameters.Add("City", DbType.String, senMain.DropDownList2.SelectedValue);
                _filter += " AND City == @City";
            }

            if (_filter != "TRUE ")
                ldsLocations.Where = _filter;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                FillControls();
                PopulateSearchEngine();
                SwitchDependingControls();


            }
            senMain_SearchClick(null, null);
        }

        private void PopulateSearchEngine()
        {
            senMain.LblTextField1Text = "Ragione sociale contiene...";
            senMain.LblTextField2Text = "CAP contiene...";

            using (var _qc = new QuotationDataContext())
            {
                senMain.LblDropDownList1Text = "Provincia";
                var _items1 =
                    _qc.Locations.Select(s => new ListItem { Text = s.Province, Value = s.Province }).Distinct().ToArray();
                senMain.DropDownList1.Items.AddRange(_items1);
                senMain.DropDownList1.Items.Insert(0, new ListItem("Tutti", ""));

                senMain.LblDropDownList2Text = "Località";
                var _items2 =
                    _qc.Locations.Select(s => new ListItem { Text = s.City, Value = s.City }).Distinct().ToArray();
                senMain.DropDownList2.Items.AddRange(_items2);
                senMain.DropDownList2.Items.Insert(0, new ListItem("Tutti", ""));

            }

            senMain.DdlOrderBy.Items.Add(new ListItem("Ragione Sociale", "LocationName"));
            senMain.DdlOrderBy.Items.Add(new ListItem("Località", "City"));
        }

        private void FillControls()
        {
            lbtNewItem.Attributes.Add("onclick", "javascript:OpenItem('LocationPopup.aspx?" + LocationKey + "=-1')");
        }


        protected void SwitchDependingControls()
        {
            ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.Empty);
        }

        protected void grdLocations_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdLocations.PageIndex = e.NewPageIndex;
        }

        protected void lbtPrintLocations_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("{2}?{0}={1}", GenericReportKey, "Locations", GenericPrintPage), true);
        }

        protected void OnFilterSelectedIndexChanged(object sender, EventArgs e)
        {
            grdLocations.EditIndex = -1;
            grdLocations.PageIndex = 0;
        }

        protected void ldsLocations_Selected(object sender, LinqDataSourceStatusEventArgs e)
        {
            //if (e.TotalRowCount == 0 && grdLocations.PageIndex == 0)
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

        protected void grdLocations_DataBound(object sender, EventArgs e)
        {
            //if (grdLocations.Rows.Count == 0 && grdLocations.PageIndex == 0)
            //{
            //    grdLocations.ChangeMode(DetailsViewMode.Insert);
            //}
        }

        protected void grdLocations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";

                //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.grdPickingItems, "Select$" + e.Row.RowIndex);

                //GridViewRow _row = (GridViewRow)e.Row;
                //if (((Location)e.Row.DataItem).Inserted == false)
                //{ _row.ForeColor = System.Drawing.Color.Red; }

                var _hypEdit = (HyperLink)e.Row.Cells[0].FindControl("hypEdit");
                _hypEdit.Attributes.Add("onclick",
                    "javascript:OpenItem('LocationPopup.aspx?" + LocationKey + "=" +
                    ((Location)e.Row.DataItem).Code + "')");
                //ImageButton _ibtUpdate = (ImageButton)e.Row.Cells[0].FindControl("ibtUpdate");
            }
        }

        protected void ldsLocations_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var table = ldsLocations.GetTable();
            var _qc = (QuotationDataContext)table.CreateContext();
            switch (senMain.DdlOrderBy.SelectedValue)
            {
                //case (""):
                //    ldsLocations.OrderByParameters.Clear();
                //    ldsLocations.AutoGenerateOrderByClause = false;
                //    e.Result = _qc.Locations.OrderBy(pi => pi.Type.Order).ThenBy(pi => pi.ItemType.Order).ThenBy(pi => pi.Order);
                //    break;
                case ("City"):
                    ldsLocations.OrderByParameters.Clear();
                    ldsLocations.AutoGenerateOrderByClause = false;
                    e.Result = _qc.Locations.OrderBy(qt => qt.City);
                    break;
                case ("LocationName"):
                    ldsLocations.OrderByParameters.Clear();
                    ldsLocations.AutoGenerateOrderByClause = false;
                    e.Result = _qc.Locations.OrderBy(qt => qt.Name);
                    break;

                default:
                    break;
            }
        }

        protected void grdLocations_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.CannotDelete);
                e.ExceptionHandled = true;
            }
        }

        protected void PersistSelection(object sender, EventArgs e)
        {
            //Session["LocationsTypesSelector"] = ddlTypes.SelectedValue;
            //Session["LocationsStatusesSelector"] = ddlStatuses.SelectedValue;
            //Session["LocationsSuppliersSelector"] = ddlLocations.SelectedValue;
            //Session["LocationsOrderBySelector"] = ddlOrderBy.SelectedValue;
        }

        protected void grdLocations_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //int _destinationId = Convert.ToInt32(e.CommandArgument.ToString() == string.Empty ? 0.ToString() : e.CommandArgument.ToString());
            if (e.CommandName == "Reload")
            {
                //grdLocations.DataBind();
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

        protected void grdLocations_PreRender(object sender, EventArgs e)
        {
            grdLocations.DataBind();
        }

    }
}