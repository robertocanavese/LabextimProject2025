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
    public partial class QuotationFreeItems : ProductionOrderController
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(grdProductionOrders);
            senMain.SearchClick += senMain_SearchClick;
            senMain.EmptyClick += senMain_EmptyClick;
            //senMain.DropDownList1.SelectedIndexChanged += DropDownList_SelectedIndexChanged;
        }

        //protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    grdProductionOrders.PageIndex = 0;
        //    SetFilter();
        //}

        public void SetFilter()
        {
            ldsProductionOrders.AutoGenerateWhereClause = false;

            ldsProductionOrders.WhereParameters.Clear();
            var _filter = "TRUE ";


            //if (CurrentCompanyId != -1)
            //{
            //    ldsProductionOrders.WhereParameters.Add("ID_Company", DbType.Int16, CurrentCompanyId.ToString());
            //    _filter += " AND ID_Company = @ID_Company";
            //}

            if (senMain.ItbNo.ReturnValue != 0)
            {
                ldsProductionOrders.WhereParameters.Add("ID_Quotation", DbType.Int32, senMain.ItbNo.ReturnValue.ToString());
                _filter += " AND ID_Quotation = @ID_Quotation";
            }

            if (senMain.TextField1Text != string.Empty)
            {
                ldsProductionOrders.WhereParameters.Add("Subject", DbType.String, senMain.TextField1Text);
                _filter += " AND Subject.Contains(@Subject)";
            }
            if (senMain.TextField2Text != string.Empty)
            {
                ldsProductionOrders.WhereParameters.Add("FreeTypeDescription", DbType.String, senMain.TextField2Text);
                _filter += " AND FreeTypeDescription.Contains(@FreeTypeDescription)";
            }


            //if (senMain.ValueHidField2Text != string.Empty)
            //{
            //    ldsProductionOrders.WhereParameters.Add("CustomerCode", DbType.Int32, senMain.ValueHidField2Text);
            //    _filter += " AND CustomerCode = @CustomerCode";
            //}
            if (senMain.ValueHidField3Text != string.Empty)
            {
                ldsProductionOrders.WhereParameters.Add("SupplierCode", DbType.Int32, senMain.ValueHidField3Text);
                _filter += " AND SupplierCode = @SupplierCode";
            }

            if (senMain.DropDownList1.SelectedValue != string.Empty)
            {
                ldsProductionOrders.WhereParameters.Add("ID_Manager", DbType.Int32, senMain.DropDownList1.SelectedValue);
                _filter += " AND ID_Manager == @ID_Manager";
            }

            if (senMain.TextDateFromText != string.Empty)
            {
                ldsProductionOrders.WhereParameters.Add("DataDa", DbType.DateTime,
                    DateTime.Parse(senMain.TextDateFromText).ToString());
                _filter += " AND CreationDate >= @DataDa";
            }
            if (senMain.TextDateToText != string.Empty)
            {
                ldsProductionOrders.WhereParameters.Add("DataA", DbType.DateTime,
                    DateTime.Parse(senMain.TextDateToText).ToString());
                _filter += " AND CreationDate <= @DataA";
            }


            if (_filter != "TRUE ")
                ldsProductionOrders.Where = _filter;
            else
            {
                ldsProductionOrders.WhereParameters.Add("CreationDate", DbType.DateTime, DateTime.Today.AddMonths(-12).ToString());
                ldsProductionOrders.Where = _filter + " AND CreationDate >= @CreationDate";
            }

            Session["QuotationFreeItemsCurrentSelection"] = senMain.GetCurrentSelection();

        }

        public void senMain_SearchClick(object sender, EventArgs e)
        {
            grdProductionOrders.PageIndex = 0;
            SetFilter();
        }

        public void senMain_EmptyClick(object sender, EventArgs e)
        {
            grdProductionOrders.PageIndex = 0;
            Session["QuotationFreeItemsCurrentSelection"] = null;
            SetFilter();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateSearchEngine();
                if (Session["QuotationFreeItemsCurrentSelection"] != null)
                    senMain.SetCurrentSelection(Session["QuotationFreeItemsCurrentSelection"].ToString());
            }
            SetFilter();

        }

        private void PopulateSearchEngine()
        {

            senMain.LblTextField1Text = "Titolo preventivo contiene...";
            senMain.LblTextField2Text = "Voce libera contiene...";
            senMain.LblTextField3Text = "Fornitore";
            senMain.LblDateFromText = "Data da";
            senMain.LblDateToText = "Data a";

            using (var _qc = new QuotationDataContext())
            {
                senMain.LblDropDownList1Text = "Gestione";
                var _managerItems = _qc.Managers.Select(s => new ListItem { Text = s.Description, Value = s.ID.ToString() }).ToArray();
                senMain.DropDownList1.Items.AddRange(_managerItems);
                senMain.DropDownList1.Items.Insert(0, new ListItem("Tutti", ""));
            }

            senMain.DdlOrderBy.Items.Add(new ListItem("Cliente", "CustomerName"));
            senMain.DdlOrderBy.Items.Add(new ListItem("Data", "CreationDate"));

            senMain.DropDownList1.SelectedValue = "false";
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
        }

        protected void grdProductionOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";

                VW_QuotationFreeItem item = ((VW_QuotationFreeItem)e.Row.DataItem);

                if (item.ID_ProductionOrder != null)
                {
                    var _hypEdit = (HyperLink)e.Row.Cells[0].FindControl("hypEdit");
                    _hypEdit.Attributes.Add("onclick",
                        "javascript:OpenBigItemNarrow('ProductionOrderPopup.aspx?" + POIdKey + "=" +
                        ((VW_QuotationFreeItem)e.Row.DataItem).ID_ProductionOrder + "')");

                    var _hypDetails = (HyperLink)e.Row.Cells[1].FindControl("hypDetails");
                    _hypDetails.Attributes.Add("onclick",
                        "javascript:OpenBigItem2('ProductionOrderQuotationStats.aspx?" + POIdKey + "=" +
                        ((VW_QuotationFreeItem)e.Row.DataItem).ID_ProductionOrder + "')");
                }

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
                case ("CustomerName"):
                    ldsProductionOrders.OrderByParameters.Clear();
                    ldsProductionOrders.AutoGenerateOrderByClause = false;
                    e.Result = _qc.VW_QuotationFreeItems.OrderBy(qt => qt.CustomerName);
                    break;
                case ("Date"):
                    ldsProductionOrders.OrderByParameters.Clear();
                    ldsProductionOrders.AutoGenerateOrderByClause = false;
                    e.Result = _qc.VW_QuotationFreeItems.OrderByDescending(qt => qt.CreationDate).ThenByDescending(qt => qt.ID_Quotation);
                    break;
                default:
                    break;
            }
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


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetCustomers(string q)
        {
            try
            {
                using (var ctx = new QuotationDataContext())
                {
                    if (System.Web.HttpContext.Current.Session["CurrentCompanyId"].ToString() == "1")
                        //return new JavaScriptSerializer().Serialize(ctx.Customers.Where(c => !c.Name.StartsWith("**") && c.Name.Contains(q.Replace("%27", "'")) && c.Code >= 1 && c.Code <= 199999999).Select(c => new { Code = c.Code, Name = c.Name, MarkUp = c.CustomersMarkUp.MarkUp }).ToList());
                        return new JavaScriptSerializer().Serialize(ctx.Customers.Where(c => !c.Name.StartsWith("**") && c.Name.Contains(q.Replace("%27", "'"))).Select(c => new { Code = c.Code, Name = c.Name, MarkUp = c.CustomersMarkUp.MarkUp }).ToList());
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

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetSuppliers(string q)
        {
            try
            {
                using (var ctx = new QuotationDataContext())
                {
                    if (System.Web.HttpContext.Current.Session["CurrentCompanyId"].ToString() == "1")
                        //return new JavaScriptSerializer().Serialize(ctx.Suppliers.Where(c => !c.Name.StartsWith("**") && c.Name.Contains(q.Replace("%27", "'")) && c.Code >= 1 && c.Code <= 199999999).Select(c => new { Code = c.Code, Name = c.Name }).ToList());
                        return new JavaScriptSerializer().Serialize(ctx.Suppliers.Where(c => !c.Name.StartsWith("**") && c.Name.Contains(q.Replace("%27", "'"))).Select(c => new { Code = c.Code, Name = c.Name }).ToList());
                    else if (System.Web.HttpContext.Current.Session["CurrentCompanyId"].ToString() == "2")
                        return new JavaScriptSerializer().Serialize(ctx.Suppliers.Where(c => !c.Name.StartsWith("**") && c.Name.Contains(q.Replace("%27", "'")) && c.Code >= 200000000 && c.Code <= 299999999).Select(c => new { Code = c.Code, Name = c.Name }).ToList());
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                return String.Format("Errore: {0}", ex.Message);
            }

        }

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static string FreeTypeDescription(string q)
        //{
        //    try
        //    {
        //        using (var ctx = new QuotationDataContext())
        //        {
        //                return new JavaScriptSerializer().Serialize(ctx.VW_QuotationFreeItems.Where(c => c.FreeTypeDescription.Contains(q.Replace("%27", "'"))).Select(c => new { Code = c.ID, Name = c.FreeTypeDescription }).ToList());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return String.Format("Errore: {0}", ex.Message);
        //    }

        //}

        protected void grdProductionOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "GoToQuotation")
            {
                LoadPersistedQuotation = true;
                Response.Redirect(QuotationConsolePage + "?" + QuotationKey + "=" + e.CommandArgument + "&Usage=0", true);
            }
        }


    }


}