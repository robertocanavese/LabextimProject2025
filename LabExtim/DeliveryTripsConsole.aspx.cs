using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Linq;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using DLLabExtim;
using UILabExtim;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;


namespace LabExtim
{
    public partial class DeliveryTripsConsole : BaseController
    {


        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(grdDeliveryTrips);
            senMain.SearchClick += senMain_SearchClick;
        }

        public void senMain_SearchClick(object sender, EventArgs e)
        {
            ldsDeliveryTrips.AutoGenerateWhereClause = false;

            ldsDeliveryTrips.WhereParameters.Clear();
            var _filter = "";

            // merge aziendale
            //ldsDeliveryTrips.WhereParameters.Add("ID_Company", DbType.Int32, CurrentCompanyId.ToString());
            //_filter += "ID_Company == @ID_Company ";

            if (senMain.ItbNo.ReturnValue != 0)
            {
                ldsDeliveryTrips.WhereParameters.Add("ID", DbType.Int32, senMain.ItbNo.ReturnValue.ToString());
                _filter += " AND ID == @ID";
            }

            if (senMain.ValueHidField1Text != string.Empty)
            {
                ldsDeliveryTrips.WhereParameters.Add("CustomerCode", DbType.Int32, senMain.ValueHidField1Text);
                _filter += " AND CustomerCode == @CustomerCode";
            }
            if (senMain.DropDownList1.SelectedValue != string.Empty)
            {
                ldsDeliveryTrips.WhereParameters.Add("LocationCode", DbType.String, senMain.DropDownList1.SelectedValue);
                _filter += " AND LocationCode == @LocationCode";
            }

            if (_filter != "TRUE ")
                ldsDeliveryTrips.Where = _filter;
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
            senMain.LblTextField1Text = "Cliente";

            using (var _qc = new QuotationDataContext())
            {

                senMain.LblDropDownList1Text = "Altra destinazione";
                var _items1 =
                    _qc.Locations.Select(s => new ListItem { Text = s.Name, Value = s.Code.ToString() }).ToArray();
                senMain.DropDownList1.Items.AddRange(_items1);
                senMain.DropDownList1.Items.Insert(0, new ListItem("Tutti", ""));

            }

            senMain.DdlOrderBy.Items.Add(new ListItem("Cliente", "CustomerName"));
            senMain.DdlOrderBy.Items.Add(new ListItem("Altra destinazione", "LocationName"));
        }

        private void FillControls()
        {
            lbtNewItem.Attributes.Add("onclick", "javascript:OpenItem('DeliveryTripPopup.aspx?" + DeliveryTripKey + "=-1')");
        }


        protected void SwitchDependingControls()
        {
            ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.Empty);
        }

        protected void grdDeliveryTrips_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdDeliveryTrips.PageIndex = e.NewPageIndex;
        }

        protected void lbtPrintDeliveryTrips_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("{2}?{0}={1}", GenericReportKey, "DeliveryTrips", GenericPrintPage), true);
        }

        protected void OnFilterSelectedIndexChanged(object sender, EventArgs e)
        {
            grdDeliveryTrips.EditIndex = -1;
            grdDeliveryTrips.PageIndex = 0;
        }

        protected void ldsDeliveryTrips_Selected(object sender, LinqDataSourceStatusEventArgs e)
        {
            //if (e.TotalRowCount == 0 && grdDeliveryTrips.PageIndex == 0)
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

        protected void grdDeliveryTrips_DataBound(object sender, EventArgs e)
        {
            //if (grdDeliveryTrips.Rows.Count == 0 && grdDeliveryTrips.PageIndex == 0)
            //{
            //    grdDeliveryTrips.ChangeMode(DetailsViewMode.Insert);
            //}
        }

        protected void grdDeliveryTrips_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";

                DeliveryTrip item = e.Row.DataItem as DeliveryTrip;

                var _hypClose = (LinkButton)e.Row.Cells[0].FindControl("ibtClose");
                _hypClose.Visible = (item.Status == 0);
                var _hypReopen = (LinkButton)e.Row.Cells[1].FindControl("ibtReopen");
                _hypReopen.Visible = (item.Status == 1);

                var _hypEdit = (HyperLink)e.Row.Cells[2].FindControl("hypEdit");
                _hypEdit.Visible = (item.Status == 0);
                _hypEdit.Attributes.Add("onclick",
                    "javascript:OpenItem('DeliveryTripPopup.aspx?" + DeliveryTripKey + "=" +
                    ((DeliveryTrip)e.Row.DataItem).ID + "')");

            }
        }

        protected void ldsDeliveryTrips_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var table = ldsDeliveryTrips.GetTable();
            var _qc = (QuotationDataContext)table.CreateContext();
            switch (senMain.DdlOrderBy.SelectedValue)
            {
                //case (""):
                //    ldsDeliveryTrips.OrderByParameters.Clear();
                //    ldsDeliveryTrips.AutoGenerateOrderByClause = false;
                //    e.Result = _qc.DeliveryTrips.OrderBy(pi => pi.Type.Order).ThenBy(pi => pi.ItemType.Order).ThenBy(pi => pi.Order);
                //    break;
                case ("CustomerName"):
                    ldsDeliveryTrips.OrderByParameters.Clear();
                    ldsDeliveryTrips.AutoGenerateOrderByClause = false;
                    e.Result = _qc.DeliveryTrips.OrderBy(qt => qt.Customer.Name);
                    break;
                case ("LocationName"):
                    ldsDeliveryTrips.OrderByParameters.Clear();
                    ldsDeliveryTrips.AutoGenerateOrderByClause = false;
                    e.Result = _qc.DeliveryTrips.OrderBy(qt => qt.Location.Name);
                    break;

                default:
                    break;
            }
        }

        protected void grdDeliveryTrips_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.CannotDelete);
                e.ExceptionHandled = true;
            }
        }

        protected void PersistSelection(object sender, EventArgs e)
        {
            //Session["DeliveryTripsTypesSelector"] = ddlTypes.SelectedValue;
            //Session["DeliveryTripsStatusesSelector"] = ddlStatuses.SelectedValue;
            //Session["DeliveryTripsSuppliersSelector"] = ddlDeliveryTrips.SelectedValue;
            //Session["DeliveryTripsOrderBySelector"] = ddlOrderBy.SelectedValue;
        }

        protected void grdDeliveryTrips_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Reopen")
            {
                using (QuotationDataContext ctx = new QuotationDataContext())
                {

                    DeliveryTrip trip = ctx.DeliveryTrips.FirstOrDefault(d => d.ID == Convert.ToInt32(e.CommandArgument));
                    trip.Status = 0;
                    trip.EndDate = null;

                    ctx.SubmitChanges();
                }
            }
            
            if (e.CommandName == "Closed")
            {
                using (QuotationDataContext ctx = new QuotationDataContext())
                {
                    var dataOptions = new DataLoadOptions();
                    dataOptions.LoadWith<DeliveryTrip>(c => c.DeliveryTripDetails);
                    dataOptions.LoadWith<DeliveryTripDetail>(c => c.ProductionOrder);
                    ctx.LoadOptions = dataOptions;

                    DeliveryTrip trip = ctx.DeliveryTrips.FirstOrDefault(d => d.ID == Convert.ToInt32(e.CommandArgument));
                    trip.Status = 1;
                    trip.EndDate = DateTime.Now;

                    new ProductionOrderDetailsInsertController().RecalcDeliveryCostDistribution(ctx, trip);
                    ctx.SubmitChanges();
                }
            }
        }

        protected void grdDeliveryTrips_PreRender(object sender, EventArgs e)
        {
            grdDeliveryTrips.DataBind();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetCustomers(string q)
        {
            try
            {
                using (var ctx = new QuotationDataContext())
                {
                    if (System.Web.HttpContext.Current.Session["CurrentCompanyId"].ToString() == "1")
                        return new JavaScriptSerializer().Serialize(ctx.Customers.Where(c => !c.Name.StartsWith("**") && c.Name.Contains(q.Replace("%27", "'")) && c.Code >= 1 && c.Code <= 199999999).Select(c => new { Code = c.Code, Name = c.Name, MarkUp = c.CustomersMarkUp.MarkUp }).ToList());
                    else if (System.Web.HttpContext.Current.Session["CurrentCompanyId"].ToString() == "2")
                        return new JavaScriptSerializer().Serialize(ctx.Customers.Where(c => !c.Name.StartsWith("**") && c.Name.Contains(q.Replace("%27", "'")) && c.Code >= 200000000 && c.Code <= 299999999).Select(c => new { Code = c.Code, Name = c.Name, MarkUp = c.CustomersMarkUp.MarkUp }).ToList());
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                return String.Format("Errore: {0}", ex.Message);
            }

        }
    }
}