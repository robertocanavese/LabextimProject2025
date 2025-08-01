using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;

using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class ProductionOrdersConsole : ProductionOrderController
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(grdProductionOrders);
            senMain.SearchClick += senMain_SearchClick;
            senMain.EmptyClick += senMain_EmptyClick;
        }

        public void SetFilter()
        {
            ldsProductionOrders.AutoGenerateWhereClause = false;

            ldsProductionOrders.WhereParameters.Clear();
            var _filter = "TRUE ";

            // merge aziendale
            //if (CurrentCompanyId != -1)
            //{
            //    ldsProductionOrders.WhereParameters.Add("ID_Company", DbType.Int16, CurrentCompanyId.ToString());
            //    _filter += " AND ID_Company = @ID_Company";
            //}

            if (senMain.ItbNo.ReturnValue != 0)
            {
                ldsProductionOrders.WhereParameters.Add("ID", DbType.Int32, senMain.ItbNo.ReturnValue.ToString());
                _filter += " AND ID = @ID";
            }

            //if (senMain.YctNumber.ReturnValue.Length > 4)
            //{
            //    ldsProductionOrders.WhereParameters.Add("Number", DbType.String, senMain.YctNumber.ReturnValue);
            //    _filter += " AND Number == @Number";
            //}

            if (!string.IsNullOrEmpty(senMain.YctNumber.ReturnValue))
            {
                if (senMain.YctNumber.ReturnValue.StartsWith("/"))
                {
                    ldsProductionOrders.WhereParameters.Add("Number", DbType.String, senMain.YctNumber.ReturnValue.Substring(senMain.YctNumber.ReturnValue.IndexOf('/') + 1));
                    _filter += " AND Number.Contains(@Number)";
                }
                else if (senMain.YctNumber.ReturnValue.Length > 4)
                {
                    ldsProductionOrders.WhereParameters.Add("Number", DbType.String, senMain.YctNumber.ReturnValue);
                    _filter += " AND Number.Substring(2) == @Number";
                }
                else
                {
                    ldsProductionOrders.WhereParameters.Add("Number", DbType.String, senMain.YctNumber.ReturnValue.Substring(2));
                    //_filter += " AND Year(StartDate) == @Number";
                    _filter += " AND Number.Substring(2,2) == @Number";
                }
            }

            if (senMain.TextField1Text != string.Empty)
            {
                ldsProductionOrders.WhereParameters.Add("Description", DbType.String, senMain.TextField1Text);
                _filter += " AND Description.Contains(@Description)";
            }
            //if (senMain.TextField2Text != string.Empty)
            //{
            //    ldsProductionOrders.WhereParameters.Add("QuotationSubject", DbType.String, senMain.TextField2Text);
            //    _filter += " AND Quotation.Subject.Contains(@QuotationSubject)";
            //}
            if (senMain.TextDateFromText != string.Empty)
            {
                ldsProductionOrders.WhereParameters.Add("StartDateFrom", DbType.DateTime,
                    DateTime.Parse(senMain.TextDateFromText).ToString());
                _filter += " AND StartDate >= @StartDateFrom";
            }
            if (senMain.TextDateToText != string.Empty)
            {
                ldsProductionOrders.WhereParameters.Add("StartDateTo", DbType.DateTime,
                    DateTime.Parse(senMain.TextDateToText).ToString());
                _filter += " AND StartDate <= @StartDateTo";
            }
            if (senMain.DropDownList1.SelectedValue != string.Empty)
            {
                ldsProductionOrders.WhereParameters.Add("Status", DbType.Int32, senMain.DropDownList1.SelectedValue);
                _filter += " AND Status == @Status";
            }
            if (senMain.DropDownList2.SelectedValue != string.Empty)
            {
                ldsProductionOrders.WhereParameters.Add("ID_Manager", DbType.Int32, senMain.DropDownList2.SelectedValue);
                _filter += " AND ID_Manager == @ID_Manager";
            }
            if (senMain.ValueHidField2Text != string.Empty)
            {
                ldsProductionOrders.WhereParameters.Add("ID_Customer", DbType.Int32, senMain.ValueHidField2Text);
                _filter += " AND ID_Customer = @ID_Customer";
            }
            if (_filter != "TRUE ")
                ldsProductionOrders.Where = _filter;
            else
            {
                //ldsProductionOrders.WhereParameters.Add("StartDate", DbType.DateTime, DateTime.Today.AddDays(-14).ToString());
                ldsProductionOrders.WhereParameters.Add("StartDate", DbType.DateTime,
                    DateTime.Today.AddMonths(-12).ToString());
                ldsProductionOrders.Where = _filter + " AND StartDate >= @StartDate";
                //ldsProductionOrders.Where = _filter + " AND FALSE";
            }
        }

        public void senMain_SearchClick(object sender, EventArgs e)
        {
            grdProductionOrders.PageIndex = 0;
            SetFilter();
        }

        public void senMain_EmptyClick(object sender, EventArgs e)
        {
            grdProductionOrders.PageIndex = 0;
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
            senMain.LblYearCounterText = "Anno/Numero";
            senMain.LblTextField1Text = "Titolo OdP contiene...";
            //senMain.LblTextField2Text = "Descrizione preventivo contiene...";
            senMain.LblTextField2Text = "Cliente";
            senMain.LblDateFromText = "Data lancio da";
            senMain.LblDateToText = "Data lancio a";

            using (var _qc = new QuotationDataContext())
            {
                senMain.LblDropDownList1Text = "Stato";
                var _statusItems =
                    _qc.Statuses.Where(s => s.StatusType == 1)
                        .Select(s => new ListItem { Text = s.Description, Value = s.ID.ToString() })
                        .ToArray();
                senMain.DropDownList1.Items.AddRange(_statusItems);
                senMain.DropDownList1.Items.Insert(0, new ListItem("Tutti", ""));

                senMain.LblDropDownList2Text = "Gestione";
                var _managerItems =
                    _qc.Managers.Select(s => new ListItem { Text = s.Description, Value = s.ID.ToString() }).ToArray();
                senMain.DropDownList2.Items.AddRange(_managerItems);
                senMain.DropDownList2.Items.Insert(0, new ListItem("Tutti", ""));
            }

            senMain.DdlOrderBy.Items.Add(new ListItem("Più recenti", "StartDate"));
            senMain.DdlOrderBy.Items.Add(new ListItem("Cliente", "CustomerName"));
            senMain.DdlOrderBy.Items.Add(new ListItem("Stato", "StatusName"));
            senMain.DdlOrderBy.Items.Add(new ListItem("Data conferma", "DeliveryDate"));
        }

        private void FillControls()
        {
            lbtNewItem.Attributes.Add("onclick", "javascript:OpenItem('ProductionOrderPopup.aspx?" + POIdKey + "=-1')");

            if (UsageParameter == 1)
            {
                lbtNewItem.Enabled = false;
                lbtPrintProductionOrders.Enabled = false;
                grdProductionOrders.Columns[0].Visible = false;
                grdProductionOrders.Columns[1].Visible = false;
                grdProductionOrders.Columns[2].Visible = false;
                grdProductionOrders.Columns[3].Visible = false;
                grdProductionOrders.Columns[4].Visible = false;
                grdProductionOrders.Columns[5].Visible = false;
            }
        }

        protected void grdProductionOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdProductionOrders.PageIndex = e.NewPageIndex;
            //SetFilter();
            grdProductionOrders.DataBind();
        }

        protected void lbtPrintProductionOrders_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("{2}?{0}={1}", GenericReportKey, "ProductionOrders", GenericPrintPage), true);
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

                var _hypEdit = (HyperLink)e.Row.Cells[0].FindControl("hypEdit");
                _hypEdit.Attributes.Add("onclick",
                    "javascript:OpenBigItemNarrow('ProductionOrderPopup.aspx?" + POIdKey + "=" +
                    ((ProductionOrder)e.Row.DataItem).ID + "')");
                //ImageButton _ibtUpdate = (ImageButton)e.Row.Cells[0].FindControl("ibtUpdate");

                var _hypDetails = (HyperLink)e.Row.Cells[2 + 1].FindControl("hypDetails");
                _hypDetails.Attributes.Add("onclick",
                    "javascript:OpenBigItem2('ProductionOrderQuotationStats.aspx?" + POIdKey + "=" +
                    ((ProductionOrder)e.Row.DataItem).ID + "' , " + ((ProductionOrder)e.Row.DataItem).ID +
                    " ) ");


                //if (POUsageParameter == 1)
                //{
                //    _hypEdit.Visible = false;
                //    _ibtUpdate.Visible = false;
                //}
                var _CloseLinkButton = (LinkButton)e.Row.Cells[3 + 1].FindControl("CloseLinkButton");
                //_CloseLinkButton.Visible = (((ProductionOrder)e.Row.DataItem).Statuse.ID != 3);
                _CloseLinkButton.Visible = (!new int[] { 3, 19 }.Contains((((ProductionOrder)e.Row.DataItem).Statuse.ID)));

                //var _DeliveredLinkButton = (LinkButton)e.Row.Cells[3 + 1].FindControl("DeliveredLinkButton");
                //_DeliveredLinkButton.Visible = (new int[] { 3 }.Contains((((ProductionOrder)e.Row.DataItem).Statuse.ID)));

                var _ToWaitLinkButton = (LinkButton)e.Row.Cells[4 + 1].FindControl("ToWaitLinkButton");
                _ToWaitLinkButton.Visible = (!new int[] { 3, 19, 9 }.Contains((((ProductionOrder)e.Row.DataItem).Statuse.ID)));


            }
        }

        protected void ldsProductionOrders_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var table = ldsProductionOrders.GetTable();
            var _qc = (QuotationDataContext)table.CreateContext();
            switch (senMain.DdlOrderBy.SelectedValue)
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
                                            .Sum(pod => pod.ProducedQuantity)) / qt.Quantity
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

        protected void grdProductionOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var _destinationId = -1;
            if (e.CommandName != "Page")
            {
                _destinationId =
                    Convert.ToInt32(e.CommandArgument.ToString() == string.Empty
                        ? 0.ToString()
                        : e.CommandArgument.ToString());
            }
            if (e.CommandName == "Reload")
            {
                UpdateList(false);
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
                var table = ldsProductionOrders.GetTable();
                using (var _qc = (QuotationDataContext)table.CreateContext())
                {
                    var _toDeleteProductionOrder = _qc.ProductionOrders.Single(po => po.ID == _destinationId);
                    _toDeleteProductionOrder.Status = 3;
                    ProductionOrderService.CloseProductionOrderSchedule(_qc, _toDeleteProductionOrder.ID);
                    _qc.SubmitChanges();
                }
                SetFilter();
                grdProductionOrders.DataBind();
            }

            //if (e.CommandName == "Delivered")
            //{
            //    var table = ldsProductionOrders.GetTable();
            //    using (var _qc = (QuotationDataContext)table.CreateContext())
            //    {
            //        var _toDeleteProductionOrder = _qc.ProductionOrders.Single(po => po.ID == _destinationId);
            //        _toDeleteProductionOrder.Status = 19;
            //        _qc.SubmitChanges();
            //    }
            //    SetFilter();
            //    grdProductionOrders.DataBind();
            //}

            if (e.CommandName == "ToWait")
            {
                var table = ldsProductionOrders.GetTable();
                using (var _qc = (QuotationDataContext)table.CreateContext())
                {
                    var _toDeleteProductionOrder = _qc.ProductionOrders.Single(po => po.ID == _destinationId);
                    _toDeleteProductionOrder.Status = 9;
                    //ProductionOrderService.CloseProductionOrderSchedule(_qc, _toDeleteProductionOrder);
                    ProductionOrderService.DeleteProductionOrderSchedule(_qc, _toDeleteProductionOrder, false);
                    ProductionOrderService.CreateProductionOrderSchedule(_qc, _toDeleteProductionOrder, Global.CurrentSchedulingType);

                    _qc.SubmitChanges();
                }
                SetFilter();
                grdProductionOrders.DataBind();
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
                        LoadPersistedQuotation = true;

                        var _result = new QuotationService().GetLock(1, Convert.ToInt32(_destinationId), WebUser.Member.ProviderUserKey.ToString(), SessionTimeStamp);

                        try
                        {
                            switch (_result)
                            {
                                case "|0|":

                                    lblSuccess.Text = string.Format("Il preventivo {0} {1}", _destinationId, "è in uso allo stesso utente nella stessa sessione di lavoro, accedere dalla pagina già attiva");
                                    break;
                                case "|1|":
                                    lblSuccess.Text = string.Format("Il preventivo {0} {1}", _destinationId, "è in uso allo stesso utente in un altra sessione di lavoro, accedere da quest'ultima");
                                    break;
                                case "|2|":
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "toq", "OpenBigItem2('" + string.Format("{2}?{0}={1}&hm=1", QuotationKey, _destinationId, "QuotationConsole2.aspx") + "' )", true);
                                    break;
                                default:
                                    lblSuccess.Text = string.Format("Il preventivo {0} {1}", _destinationId, "è in uso all'utente " + new MemberShipService().GetUser(_result).UserName);
                                    break;
                            }
                        }
                        catch
                        {
                        }

                        //Response.Redirect(
                        //    string.Format("{2}?{0}={1}", QuotationKey, _destinationId, TempQuotationConsolePage), true);
                        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "toq", "OpenBigItem2('" + string.Format("{2}?{0}={1}&hm=1", QuotationKey, _destinationId, "QuotationConsole2.aspx") + "' )", true);
                    }
                    catch
                    {
                        lblSuccess.Text = "Il preventivo No " + _destinationId +
                                          " non può avere quantità zero: modificare la quantità dell'Odp per permetterne il caricamento.";
                    }
                }
            }

            if (e.CommandName == "RecalcHistoricalCost")
            {
                try
                {
                    using (var _ctx = new QuotationDataContext())
                    {
                        foreach (
                            var _curItem in
                                _ctx.ProductionOrderDetails.Where(
                                    pod => pod.ID_ProductionOrder == Convert.ToInt32(e.CommandArgument)))
                        {
                            _curItem.HistoricalCostPhase = _curItem.CostCalcPhase;
                            if (_curItem.RFlag == null)
                                _curItem.HistoricalCostRawM = _curItem.CostCalcRawM;
                            if (_curItem.SFlag == null)
                                _curItem.HistoricalCostSupM = _curItem.CostCalcSupM;
                        }
                        _ctx.SubmitChanges();
                    }
                }
                catch
                {
                    lblSuccess.Text = "Il ricalcolo del costo storico non è stato possibile.";
                }
            }
        }

        protected void grdProductionOrders_PreRender(object sender, EventArgs e)
        {
            //grdProductionOrders.DataBind();
        }

        protected void lbtUpdateGrid_Click(object sender, EventArgs e)
        {
            UpdateList(true);
        }

        protected void UpdateList(bool resetCurrentPage)
        {
            if (resetCurrentPage) grdProductionOrders.PageIndex = 0;
            grdProductionOrders.DataBind();
        }

        protected void grdProductionOrders_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            var table = ldsProductionOrders.GetTable();
            using (QuotationDataContext db = (QuotationDataContext)table.CreateContext())
            {
                var po = db.ProductionOrders.Single(p => p.ID == Convert.ToInt32(e.Keys["ID"]));
                //po.DeliveryDate = newObject.DeliveryDate;
                ProductionOrderService.DeleteProductionOrderSchedule(db, po, false);
                ProductionOrderService.CreateProductionOrderSchedule(db, po, Global.CurrentSchedulingType);
                db.SubmitChanges();
            }
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