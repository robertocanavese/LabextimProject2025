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
    public partial class LeaveRequestsConsole : BaseController
    {


        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(grdLeaveRequests);
            senMain.SearchClick += senMain_SearchClick;
        }

        public void senMain_SearchClick(object sender, EventArgs e)
        {
            ldsLeaveRequests.AutoGenerateWhereClause = false;

            ldsLeaveRequests.WhereParameters.Clear();
            var _filter = "";

            // merge aziendale
            //ldsLeaveRequests.WhereParameters.Add("ID_Company", DbType.Int32, CurrentCompanyId.ToString());
            //_filter += "ID_Company == @ID_Company ";

            if (senMain.ItbNo.ReturnValue != 0)
            {
                ldsLeaveRequests.WhereParameters.Add("ID", DbType.Int32, senMain.ItbNo.ReturnValue.ToString());
                _filter += " AND ID == @ID";
            }
            if (senMain.ValueHidField1Text != string.Empty)
            {
                ldsLeaveRequests.WhereParameters.Add("ID_Applicant", DbType.Int32, senMain.ValueHidField1Text);
                _filter += " AND ID_Applicant == @ID_Applicant";
            }
            if (senMain.DropDownList1.SelectedValue != string.Empty)
            {
                ldsLeaveRequests.WhereParameters.Add("Status", DbType.String, senMain.DropDownList1.SelectedValue);
                _filter += " AND Status == @Status";
            }

            if (_filter != "TRUE ")
                ldsLeaveRequests.Where = _filter;
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

                //senMain.LblDropDownList1Text = "Altra destinazione";
                //var _items1 =
                //    _qc.Locations.Select(s => new ListItem { Text = s.Name, Value = s.Code.ToString() }).ToArray();
                //senMain.DropDownList1.Items.AddRange(_items1);
                //senMain.DropDownList1.Items.Insert(0, new ListItem("Tutti", ""));

            }

            senMain.DdlOrderBy.Items.Add(new ListItem("Più recente", "RequestDate"));
        }

        private void FillControls()
        {
            lbtNewItem.Attributes.Add("onclick", "javascript:OpenItem('LeaveRequestPopup.aspx?" + LeaveRequestKey + "=-1')");
        }


        protected void SwitchDependingControls()
        {
            ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.Empty);
        }

        protected void grdLeaveRequests_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdLeaveRequests.PageIndex = e.NewPageIndex;
        }

        protected void lbtPrintLeaveRequests_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("{2}?{0}={1}", GenericReportKey, "LeaveRequests", GenericPrintPage), true);
        }

        protected void OnFilterSelectedIndexChanged(object sender, EventArgs e)
        {
            grdLeaveRequests.EditIndex = -1;
            grdLeaveRequests.PageIndex = 0;
        }

        protected void ldsLeaveRequests_Selected(object sender, LinqDataSourceStatusEventArgs e)
        {
            //if (e.TotalRowCount == 0 && grdLeaveRequests.PageIndex == 0)
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

        protected void grdLeaveRequests_DataBound(object sender, EventArgs e)
        {
            //if (grdLeaveRequests.Rows.Count == 0 && grdLeaveRequests.PageIndex == 0)
            //{
            //    grdLeaveRequests.ChangeMode(DetailsViewMode.Insert);
            //}
        }

        protected void grdLeaveRequests_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";

                LeaveRequest item = e.Row.DataItem as LeaveRequest;

                var _hypApprove = (LinkButton)e.Row.Cells[0].FindControl("ibtApprove");
                _hypApprove.Visible = (item.Status == 19);
                var _hypDeny = (LinkButton)e.Row.Cells[1].FindControl("ibtDeny");
                _hypDeny.Visible = (item.Status == 19);

                var _hypEdit = (HyperLink)e.Row.Cells[2].FindControl("hypEdit");
                _hypEdit.Visible = (item.Status == 19);
                _hypEdit.Attributes.Add("onclick",
                    "javascript:OpenItem('LeaveRequestPopup.aspx?" + LeaveRequestKey + "=" +
                    ((LeaveRequest)e.Row.DataItem).ID + "')");

            }
        }

        protected void ldsLeaveRequests_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var table = ldsLeaveRequests.GetTable();
            var _qc = (QuotationDataContext)table.CreateContext();
            switch (senMain.DdlOrderBy.SelectedValue)
            {
                case (""):
                    ldsLeaveRequests.OrderByParameters.Clear();
                    ldsLeaveRequests.AutoGenerateOrderByClause = false;
                    e.Result = __qc.LeaveRequests.OrderByDescending(qt => qt.RequestDate);
                    break;
                case ("RequestDate"):
                    ldsLeaveRequests.OrderByParameters.Clear();
                    ldsLeaveRequests.AutoGenerateOrderByClause = false;
                    e.Result = _qc.LeaveRequests.OrderByDescending(qt => qt.RequestDate);
                    break;

                default:
                    break;
            }
        }

        protected void grdLeaveRequests_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.CannotDelete);
                e.ExceptionHandled = true;
            }
        }

        protected void PersistSelection(object sender, EventArgs e)
        {
            //Session["LeaveRequestsTypesSelector"] = ddlTypes.SelectedValue;
            //Session["LeaveRequestsStatusesSelector"] = ddlStatuses.SelectedValue;
            //Session["LeaveRequestsSuppliersSelector"] = ddlLeaveRequests.SelectedValue;
            //Session["LeaveRequestsOrderBySelector"] = ddlOrderBy.SelectedValue;
        }

        protected void grdLeaveRequests_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Approve")
            {
                new ProductionOrderDetailsInsertController().ChangeLeaveRequestStatus(20, Convert.ToInt32(e.CommandArgument), WebUser.Employee.ID);
            }
            
            if (e.CommandName == "Deny")
            {
                new ProductionOrderDetailsInsertController().ChangeLeaveRequestStatus(21, Convert.ToInt32(e.CommandArgument), WebUser.Employee.ID);
            }
        }

        protected void grdLeaveRequests_PreRender(object sender, EventArgs e)
        {
            grdLeaveRequests.DataBind();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetUsers(string q)
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