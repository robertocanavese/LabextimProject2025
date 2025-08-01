using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class ProductionOrdersOnDelivery : ProductionOrderController
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(grdProductionMPS);
            senMain.SearchClick += senMain_SearchClick;
            senMain.EmptyClick += senMain_EmptyClick;
        }

        public void SetFilter()
        {
            ldsProductionMPS.AutoGenerateWhereClause = false;

            ldsProductionMPS.WhereParameters.Clear();
            var _filter = "TRUE ";

            if (senMain.ItbNo.ReturnValue != 0)
            {
                ldsProductionMPS.WhereParameters.Add("ID", DbType.Int32, senMain.ItbNo.ReturnValue.ToString());
                _filter += " AND ID = @ID";
            }
            
            if (senMain.TextField1Text != string.Empty)
            {
                ldsProductionMPS.WhereParameters.Add("poDescription", DbType.String, senMain.TextField1Text);
                _filter += " AND poDescription.Contains(@poDescription)";
            }

            if (senMain.ValueHidField2Text != string.Empty)
            {
                ldsProductionMPS.WhereParameters.Add("ID_Customer", DbType.Int32, senMain.ValueHidField2Text);
                _filter += " AND ID_Customer = @ID_Customer";
            }
            //if (senMain.TextDateFromText != string.Empty)
            //{
            //    ldsProductionMPS.WhereParameters.Add("ProdStart", DbType.DateTime,
            //        DateTime.Parse(senMain.TextDateFromText).ToString());
            //    _filter += " AND ProdStart >= @ProdStart";
            //}
            //if (senMain.TextDateToText != string.Empty)
            //{
            //    ldsProductionMPS.WhereParameters.Add("ProdEnd", DbType.DateTime,
            //        DateTime.Parse(senMain.TextDateToText).ToString());
            //    _filter += " AND ProdEnd <= @ProdEnd";
            //}
            //if (senMain.DropDownList1.SelectedValue != string.Empty)
            //{
            //ldsProductionMPS.WhereParameters.Add("Status", DbType.Int32, senMain.DropDownList1.SelectedValue);
            //_filter += " AND Status == 11";
            //}
            //if (senMain.DropDownList1.SelectedValue != string.Empty)
            //{
            //    ldsProductionMPS.WhereParameters.Add("ID_Customer", DbType.Int32, senMain.DropDownList1.SelectedValue);
            //    _filter += " AND ID_Customer == @ID_Customer";
            //}
            //if (senMain.DropDownList2.SelectedValue != string.Empty)
            //{
            //    ldsProductionMPS.WhereParameters.Add("IDProductionMachine", DbType.Int32, senMain.DropDownList2.SelectedValue);
            //    _filter += " AND IDProductionMachine == @IDProductionMachine";
            //}
            //if (senMain.DropDownList3.SelectedValue != string.Empty)
            //{
            //    ldsProductionMPS.WhereParameters.Add("NumProductionMachine", DbType.Int32, senMain.DropDownList3.SelectedValue);
            //    _filter += " AND NumProductionMachine == @NumProductionMachine";
            //}
            _filter += " AND OpenPhases == 0 AND poStatus = 1";
            if (_filter != "TRUE ")
                ldsProductionMPS.Where = _filter;
            else
            {
                //ldsProductionMPS.WhereParameters.Add("StartDate", DbType.DateTime, DateTime.Today.AddDays(-14).ToString());
                ldsProductionMPS.WhereParameters.Add("DeliveryDate", DbType.DateTime,
                    DateTime.Today.AddMonths(-12).ToString());
                ldsProductionMPS.Where = _filter + " AND DeliveryDate >= @DeliveryDate";
                //ldsProductionMPS.Where = _filter + " AND FALSE";
            }
        }

        public void senMain_SearchClick(object sender, EventArgs e)
        {
            grdProductionMPS.PageIndex = 0;
            SetFilter();
        }

        public void senMain_EmptyClick(object sender, EventArgs e)
        {
            grdProductionMPS.PageIndex = 0;
            SetFilter();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillControls();
                PopulateSearchEngine();
            }
            SetFilter();
        }

        private void PopulateSearchEngine()
        {
            //senMain.LblYearCounterText = "Numero";
            senMain.LblTextField1Text = "Descrizione OdP contiene...";
            senMain.LblTextField2Text = "Cliente";
            senMain.LblDateFromText = "Data consegna da";
            senMain.LblDateToText = "Data consegna a";

            using (var _qc = new QuotationDataContext())
            {
                //senMain.LblDropDownList1Text = "Stato";
                //var _statusItems =
                //    _qc.Statuses.Where(s => s.StatusType == 3)
                //        .Select(s => new ListItem {Text = s.Description, Value = s.ID.ToString()})
                //        .ToArray();
                //senMain.DropDownList1.Items.AddRange(_statusItems);
                //senMain.DropDownList1.Items.Insert(0, new ListItem("Tutti", ""));
                //senMain.DropDownList1.SelectedIndex = 1;

                //senMain.LblDropDownList1Text = "Cliente";
                //var _customerItems =
                //    _qc.Customers.Select(s => new ListItem {Text = s.Name, Value = s.Code.ToString()}).ToArray();
                //senMain.DropDownList1.Items.AddRange(_customerItems);
                //senMain.DropDownList1.Items.Insert(0, new ListItem("Tutti", ""));

                //senMain.LblDropDownList2Text = "Macchina";
                //var _machineItems =
                //    _qc.ProductionMachines.Select(s => new ListItem { Text = s.Description, Value = s.ID.ToString() }).ToArray();
                //senMain.DropDownList2.Items.AddRange(_machineItems);
                //senMain.DropDownList2.Items.Insert(0, new ListItem("Tutte", ""));

                //senMain.LblDropDownList3Text = "Numero macchina";
                //senMain.DropDownList3.Items.Insert(0, new ListItem("Tutte", ""));
                //senMain.DropDownList3.Items.Insert(1, new ListItem("0", "0"));
                //senMain.DropDownList3.Items.Insert(2, new ListItem("1", "1"));
                //senMain.DropDownList3.Items.Insert(3, new ListItem("2", "2"));

            }

            senMain.DdlOrderBy.Items.Add(new ListItem("Sequenza produttiva", "StartDate"));
 
        }

        private void FillControls()
        {
            lbtNewItem.Attributes.Add("onclick", "javascript:OpenItem('ProductionOrderPopup.aspx?" + POIdKey + "=-1')");

            
        }

        protected void grdProductionMPS_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdProductionMPS.PageIndex = e.NewPageIndex;
            //SetFilter();
            grdProductionMPS.DataBind();
        }

        protected void lbtPrintProductionMPS_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("{2}?{0}={1}", GenericReportKey, "VW_ProductionMPSSnapshots", GenericPrintPage), true);
        }

        protected void ldsProductionMPS_Selected(object sender, LinqDataSourceStatusEventArgs e)
        {
            //if (e.TotalRowCount == 0 && grdProductionMPS.PageIndex == 0)
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

        protected void grdProductionMPS_DataBound(object sender, EventArgs e)
        {
            //if (grdProductionMPS.Rows.Count == 0 && grdProductionMPS.PageIndex == 0)
            //{
            //    grdProductionMPS.ChangeMode(DetailsViewMode.Insert);
            //}
        }

        protected void grdProductionMPS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";

                var _hypEdit = (HyperLink) e.Row.Cells[0].FindControl("hypEdit");
                _hypEdit.Attributes.Add("onclick",
                    "javascript:OpenBigItem('ProductionOrderPopup.aspx?" + POIdKey + "=" +
                    ((VW_ProductionMPSSnapshot) e.Row.DataItem).id + "')");

            }
        }

        protected void ldsProductionMPS_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var table = ldsProductionMPS.GetTable();
            var _qc = (QuotationDataContext) table.CreateContext();

            ldsProductionMPS.OrderByParameters.Clear();
            ldsProductionMPS.AutoGenerateOrderByClause = false;
            e.Result = _qc.VW_ProductionMPSSnapshots.OrderBy(qt => qt.DeliveryDate);

            //switch (senMain.DdlOrderBy.SelectedValue)
            //{

            //    case ("StatusName"):
            //        ldsProductionMPS.OrderByParameters.Clear();
            //        ldsProductionMPS.AutoGenerateOrderByClause = false;
            //        e.Result = _qc.VW_ProductionExtMPs.OrderBy(qt => qt.mpstDescription);
            //        break;
            //    case ("CustomerName"):
            //        ldsProductionMPS.OrderByParameters.Clear();
            //        ldsProductionMPS.AutoGenerateOrderByClause = false;
            //        e.Result = _qc.VW_ProductionExtMPs.OrderBy(qt => qt.cuName);
            //        break;
            //    case ("StartDate"):
            //        ldsProductionMPS.OrderByParameters.Clear();
            //        ldsProductionMPS.AutoGenerateOrderByClause = false;
            //        e.Result = _qc.VW_ProductionExtMPs.OrderByDescending(qt => qt.ProdStart).ThenByDescending(qt => qt.ID);
            //        break;
                
            //    default:
                   
            //        break;
            //}
        }

        protected void grdProductionMPS_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.CannotDelete);
                e.ExceptionHandled = true;
            }
        }

        protected void grdProductionMPS_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var _destinationId = -1;
            if (e.CommandName != "Page")
            {
                _destinationId =
                    Convert.ToInt32(e.CommandArgument.ToString().Split('|')[0] == string.Empty
                        ? 0.ToString()
                        : e.CommandArgument.ToString().Split('|')[0]);
            }
            if (e.CommandName == "Reload")
            {
                UpdateList(false);
                //grdProductionMPS.DataBind();
            }
            //if (e.CommandName == "Delete")
            //{
            //    using (var _qc = new QuotationDataContext())
            //    {
            //        //int _toDeleteId = Convert.ToInt32(e.CommandArgument.ToString());
            //        var _toDeleteProductionOrder = _qc.ProductionMPs.Single(po => po.ID == _destinationId);
            //        _qc.ProductionMPs.DeleteAllOnSubmit(_toDeleteProductionOrder.ProductionMPs);
            //        _qc.SubmitChanges();
            //        _qc.ProductionMPs.DeleteOnSubmit(_toDeleteProductionOrder);
            //        _qc.SubmitChanges();
            //    }
            //    //grdProductionMPS.DataBind();
            //}
            if (e.CommandName == "Close")
            {
                var table = ldsProductionMPS.GetTable();
                using (var _qc = (QuotationDataContext) table.CreateContext())
                {
                    //int _toCloseId = Convert.ToInt32(e.CommandArgument.ToString());
                    var _toDeleteProductionOrder = _qc.ProductionMPs.FirstOrDefault(po => po.IDProductionOrder == _destinationId);
                    _toDeleteProductionOrder.ProductionOrder.Status = 3;
                    ProductionOrderService.CloseProductionOrderSchedule(_qc, _toDeleteProductionOrder.ID);
                    _qc.SubmitChanges();
                }
                SetFilter();
                grdProductionMPS.DataBind();
            }

            if (e.CommandName == "GoToQuotation")
            {
                using (var _qc = new QuotationDataContext())
                {
                    try
                    {
                        LoadPersistedQuotation = true;
                        Response.Redirect(
                            string.Format("{2}?{0}={1}", QuotationKey, _destinationId, TempQuotationConsolePage), true);
                    }
                    catch
                    {
                        lblSuccess.Text = "Il preventivo No " + _destinationId +
                                          " non può avere quantità zero: modificare la quantità dell'Odp per permetterne il caricamento.";
                    }
                }
            }

        }

        protected void grdProductionMPS_PreRender(object sender, EventArgs e)
        {
            //grdProductionMPS.DataBind();
        }

        protected void lbtUpdateGrid_Click(object sender, EventArgs e)
        {
            UpdateList(true);
        }

        protected void UpdateList(bool resetCurrentPage)
        {
            if (resetCurrentPage) grdProductionMPS.PageIndex = 0;
            grdProductionMPS.DataBind();
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetCustomers(string q)
        {
            try
            {
                using (var ctx = new QuotationDataContext())
                {
                    return new JavaScriptSerializer().Serialize(ctx.Customers.Where(c => !c.Name.StartsWith("**") && c.Name.Contains(q.Replace("%27", "'"))).Select(c => new { Code = c.Code, Name = c.Name, MarkUp = c.CustomersMarkUp.MarkUp }).ToList());
                }
            }
            catch (Exception ex)
            {
                return String.Format("Errore: {0}", ex.Message);
            }

        }

    }

    public class CaseInsensitiveComparer11 : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return string.Compare(x, y, true);
        }
    }
}