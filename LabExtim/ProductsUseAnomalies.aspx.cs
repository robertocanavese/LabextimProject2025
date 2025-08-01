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
    public partial class ProductsUseAnomalies : ProductionOrderController
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
                ldsProductionOrders.WhereParameters.Add("OdP", DbType.Int32, senMain.ItbNo.ReturnValue.ToString());
                _filter += " AND ID_ProductionOrder = @OdP";
            }

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
                ldsProductionOrders.WhereParameters.Add("Subject", DbType.String, senMain.TextField1Text);
                _filter += " AND Subject.Contains(@Subject)";
            }

            if (senMain.DropDownList1.SelectedValue != string.Empty)
            {
                ldsProductionOrders.WhereParameters.Add("UnusedProductsCheck", DbType.Boolean, senMain.DropDownList1.SelectedValue);
                _filter += " AND UnusedProductsCheck = @UnusedProductsCheck";
            }

            if (senMain.DropDownList2.SelectedValue != string.Empty)
            {
                ldsProductionOrders.WhereParameters.Add("ID_Manager", DbType.Int32, senMain.DropDownList2.SelectedValue);
                _filter += " AND ID_Manager == @ID_Manager";
            }

            if (senMain.TextDateFromText != string.Empty)
            {
                ldsProductionOrders.WhereParameters.Add("StartDate", DbType.DateTime,
                    DateTime.Parse(senMain.TextDateFromText).ToString());
                _filter += " AND StartDate >= @StartDate";
            }
            if (senMain.TextDateToText != string.Empty)
            {
                ldsProductionOrders.WhereParameters.Add("DeliveryDate", DbType.DateTime,
                    DateTime.Parse(senMain.TextDateToText).ToString());
                _filter += " AND DeliveryDate <= @DeliveryDate";
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
#if DEBUG
                ldsProductionOrders.WhereParameters.Add("StartDate", DbType.DateTime, DateTime.Today.AddMonths(-12).ToString());
#else
                ldsProductionOrders.WhereParameters.Add("StartDate", DbType.DateTime, DateTime.Today.AddMonths(-3).ToString());
#endif

                ldsProductionOrders.Where = _filter + " AND StartDate >= @StartDate";
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
                PopulateSearchEngine();
            }
            SetFilter();

        }

        private void PopulateSearchEngine()
        {


            senMain.LblYearCounterText = "Anno/Numero";
            senMain.LblTextField1Text = "Titolo preventivo contiene...";
            senMain.LblTextField2Text = "Cliente";
            senMain.LblDateFromText = "Data lancio";
            senMain.LblDateToText = "Data consegna";
            senMain.LblDropDownList1Text = "Stato";
            senMain.DropDownList1.Items.Add(new ListItem("Tutti", ""));
            senMain.DropDownList1.Items.Add(new ListItem("Da controllare", "false"));
            senMain.DropDownList1.Items.Add(new ListItem("Controllati", "true"));

            using (var _qc = new QuotationDataContext())
            {
                senMain.LblDropDownList2Text = "Gestione";
                var _managerItems = _qc.Managers.Select(s => new ListItem { Text = s.Description, Value = s.ID.ToString() }).ToArray();
                senMain.DropDownList2.Items.AddRange(_managerItems);
                senMain.DropDownList2.Items.Insert(0, new ListItem("Tutti", ""));
            }

            senMain.DdlOrderBy.Items.Add(new ListItem("Più recenti", "StartDate"));
            senMain.DdlOrderBy.Items.Add(new ListItem("Cliente", "DescCustomer"));
            senMain.DdlOrderBy.Items.Add(new ListItem("Data consegna", "DeliveryDate"));

            senMain.DropDownList1.SelectedValue="false";
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

                var _hypEdit = (HyperLink)e.Row.Cells[0].FindControl("hypEdit");
                _hypEdit.Attributes.Add("onclick",
                    "javascript:OpenBigItemNarrow('ProductionOrderPopup.aspx?" + POIdKey + "=" +
                    ((VW_QuotationDetailsUnusedOnProduction)e.Row.DataItem).ID_ProductionOrder + "')");

                var _hypDetails = (HyperLink)e.Row.Cells[1].FindControl("hypDetails");
                _hypDetails.Attributes.Add("onclick",
                    "javascript:OpenBigItem2('ProductionOrderQuotationStats.aspx?" + POIdKey + "=" +
                    ((VW_QuotationDetailsUnusedOnProduction)e.Row.DataItem).ID_ProductionOrder + "')");

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
                case ("DescCustomer"):
                    ldsProductionOrders.OrderByParameters.Clear();
                    ldsProductionOrders.AutoGenerateOrderByClause = false;
                    e.Result = _qc.VW_QuotationDetailsUnusedOnProductions.OrderBy(qt => qt.CustomerName);
                    break;
                case ("StartDate"):
                    ldsProductionOrders.OrderByParameters.Clear();
                    ldsProductionOrders.AutoGenerateOrderByClause = false;
                    e.Result = _qc.VW_QuotationDetailsUnusedOnProductions.OrderByDescending(qt => qt.StartDate).ThenByDescending(qt => qt.ID_ProductionOrder);
                    break;
                case ("DeliveryDate"):
                    ldsProductionOrders.OrderByParameters.Clear();
                    ldsProductionOrders.AutoGenerateOrderByClause = false;
                    e.Result = _qc.VW_QuotationDetailsUnusedOnProductions.OrderByDescending(qt => qt.DeliveryDate).ThenByDescending(qt => qt.ID_ProductionOrder);
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


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string CheckUnCheck(string pipedParams)
        {

            int idpo = Convert.ToInt32(pipedParams);

            try
            {
                using (var ctx = new QuotationDataContext())
                {
                    ProductionOrder po = ctx.ProductionOrders.First(d => d.ID == idpo);
                    po.UnusedProductsCheck = !po.UnusedProductsCheck.GetValueOrDefault(false);
                    ctx.SubmitChanges();
                    return po.UnusedProductsCheck.ToString();
                }
                
            }
            catch (Exception ex)
            {
                return String.Format("Errore: {0}", ex.Message);
            }

        }

    }


}