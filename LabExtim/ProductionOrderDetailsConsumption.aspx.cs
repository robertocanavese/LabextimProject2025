using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using CMLabExtim.CustomClasses;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class ProductionOrderDetailsConsumption : QuotationController
    {
        private HtmlTextWriter m_HtmlTextWriter;
        private StringWriter m_StringWriter;

        private string GridRender
        {
            get { return Session["ProductionOrderDetailsConsumptionGridRender"].ToString(); }
            set { Session["ProductionOrderDetailsConsumptionGridRender"] = value; }
        }

        public List<int?> SelectedPickingItems
        {
            get { return ViewState["SelectedPickingItems"] as List<int?>; }
            set { ViewState["SelectedPickingItems"] = value; }
        }

        public int? SelectedMacroItem
        {
            get { return ViewState["SelectedMacroItem"] as int?; }
            set { ViewState["SelectedMacroItem"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            //DynamicDataManager1.RegisterControl(grdProductionOrderConsumptions);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //MetaTable table = GridDataSource.GetTable();
            //InitSelectionData();

            if (!IsPostBack)
            {
                SelectedPickingItems = new List<int?>();
                LoadHeaderMenu(mnuOperations1, MenuType.MenuOperations, 1);
                LoadHeaderMenu(mnuOperations2, MenuType.MenuOperations, 2);
                FillControls();
                //SwitchDependingControls(CurEmployeesWorkingDaysHoursConsoleMode);
            }

            FillDependingControls();
        }

        private void FillControls()
        {
            btpPeriod.DataPartenzaOnLoad = DateTime.Now.Date;
            btpPeriod.AnnoIniziale = DateTime.Now.AddYears(-1).Year;
            btpPeriod.AnnoFinale = DateTime.Now.AddYears(+1).Year;

            ddlGroupBy.Items.Add(new ListItem("Non raggruppare (max 256 voci)", "All"));
            ddlGroupBy.Items.Add(new ListItem("Mese", "MonthPickingItem"));
            ddlGroupBy.Items.Add(new ListItem("Anno", "YearPickingItem"));

            ddlGroupBy.DataBind();
            //ddlGroupBy.Items.FindByValue("All").Selected = true;

        }

        protected void FillDependingControls()
        {
            //foreach (DataControlField _column in grdProductionOrderConsumptions.Columns)
            //{
            //    _column.Visible = false;
            //}
        }

        protected void SwitchDependingControls()
        {
        }

        protected void grdProductionOrderConsumptions_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdProductionOrderConsumptions.PageIndex = e.NewPageIndex;
            //SwitchDependingControls(Mode.InputItems);
            SwitchDependingControls();
        }

        //protected void lbtPrintProductionOrderConsumptions_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect(
        //        string.Format("{2}?{0}={1}", GenericReportKey, "ProductionOrderConsumptionsStats", GenericPrintPage), true);
        //}

        protected void OnFilterSelectedIndexChanged(object sender, EventArgs e)
        {
            grdProductionOrderConsumptions.EditIndex = -1;
            grdProductionOrderConsumptions.PageIndex = 0;
            grdProductionOrderConsumptionMCount.EditIndex = -1;
            grdProductionOrderConsumptionMCount.PageIndex = 0;
        }

        protected void ddlCompanies_DataBound(object sender, EventArgs e)
        {
            ddlCompanies.Items.Insert(0, new ListItem("Tutte", ""));
            if (Session["ProductionOrderConsumptionsStatsCompanyIDsSelector"] != null)
            {
                ddlCompanies.Items.FindByValue(Session["ProductionOrderConsumptionsStatsCompanyIDsSelector"].ToString()).Selected = true;
            }
            else
            {
                ddlCompanies.Items.FindByValue(CurrentCompanyId.ToString()).Selected = true;
            }
        }

        protected void ddlTypes_DataBound(object sender, EventArgs e)
        {
            ddlTypes.Items.Insert(0, new ListItem("Tutti", ""));
            if (Session["ProductionOrderConsumptionsStatsTypesSelector"] != null)
            {
                ddlTypes.Items.FindByValue(Session["ProductionOrderConsumptionsStatsTypesSelector"].ToString())
                    .Selected = true;
            }
        }

        protected void ddlItemTypes_DataBound(object sender, EventArgs e)
        {
            ddlItemTypes.Items.Insert(0, new ListItem("Tutti", ""));
            if (Session["ProductionOrderConsumptionsStatsItemTypesSelector"] != null)
            {
                ddlItemTypes.Items.FindByValue(Session["ProductionOrderConsumptionsStatsItemTypesSelector"].ToString())
                    .Selected = true;
            }
        }

        protected void ddlGroupBy_DataBound(object sender, EventArgs e)
        {
            if (Session["ProductionOrderConsumptionsStatsGroupBySelector"] != null)
            {
                try
                {
                    ddlGroupBy.Items.FindByValue(Session["ProductionOrderConsumptionsStatsGroupBySelector"].ToString())
                        .Selected = true;
                }
                catch
                {
                }
            }
        }

        protected void ldsEmployeesWorkingDaysHours_Selected(object sender, LinqDataSourceStatusEventArgs e)
        {
            //if (e.TotalRowCount == 0 && grdProductionOrderConsumptions.PageIndex == 0)
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

        protected void grdProductionOrderConsumptions_DataBound(object sender, EventArgs e)
        {
            if (grdProductionOrderConsumptions.Rows.Count > 0)
            {
                m_StringWriter = new StringWriter();
                m_HtmlTextWriter = new HtmlTextWriter(m_StringWriter);
                grdProductionOrderConsumptions.RenderControl(m_HtmlTextWriter);
                GridRender = m_StringWriter.ToString();
                m_StringWriter.Dispose();
                m_HtmlTextWriter.Dispose();
            }
        }

        protected void grdProductionOrderConsumptions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";

                //var _row = e.Row;
                //if (((VW_EmployeesWorkingDayHour)e.Row.DataItem).ProductionTime > (36000000000m * 8m) &&
                //    ddlGroupBy.SelectedValue == "All")
                //{
                //    _row.ForeColor = Color.Red;
                //}


            }
        }

        public VW_ProductionOrderDetailsConsumption[] GetDataSource()
        {

            using (QuotationDataContext _qc = new QuotationDataContext())
            {
                IQueryable<VW_ProductionOrderDetailsConsumption> _selection = _qc.VW_ProductionOrderDetailsConsumptions.Where(d => d.ProductionDate.HasValue);

                switch (itbNoOdP.Text)
                {
                    case (""):
                        break;
                    default:
                        _selection = _selection.Where(
                            ew => ew.ID_ProductionOrder == Convert.ToInt32(itbNoOdP.Text));
                        break;
                }

                switch (yctNumber.ReturnValue)
                {
                    case (""):
                        break;
                    default:
                    if (yctNumber.ReturnValue.StartsWith("/"))
                    {
                        _selection = _selection.Where(po => yctNumber.ReturnValue.Substring(yctNumber.ReturnValue.IndexOf('/') + 1) == po.Number.Substring(po.Number.IndexOf('/') + 1));
                    }
                    else if (yctNumber.ReturnValue.Length > 4)
                    {
                        _selection = _selection.Where(po => po.Number.Substring(2) == yctNumber.ReturnValue);
                    }
                    else
                    {
                        _selection = _selection.Where(po => po.Number.Substring(2, 2) == yctNumber.ReturnValue.Substring(2));
                    }
                    break;
                }

                _selection = _selection.Where(
                    qt => qt.ProductionDate >= btpPeriod.DataInizio &&
                            qt.ProductionDate <= btpPeriod.DataFine);

                switch (ddlCompanies.SelectedValue)
                {
                    case (""):
                        break;
                    default:
                        _selection = _selection.Where(
                            ew => ew.ID_Company == Convert.ToInt32(ddlCompanies.SelectedValue));
                        break;
                }

                switch (ddlTypes.SelectedValue)
                {
                    case (""):
                        break;
                    default:
                        _selection = _selection.Where(
                            ew => ew.TypeCode == Convert.ToInt32(ddlTypes.SelectedValue));
                        break;
                }

                switch (ddlItemTypes.SelectedValue)
                {
                    case (""):
                        break;
                    default:
                        _selection = _selection.Where(
                          ew => ew.ItemTypeCode == Convert.ToInt32(ddlItemTypes.SelectedValue));
                        break;
                }

                switch (hidPickingItem.Value)
                {
                    case (""):
                        break;
                    default:
                        _selection = _selection.Where(
                          ew => ew.ID_PickingItem == Convert.ToInt32(hidPickingItem.Value));
                        break;
                }

                switch (SelectedPickingItems.Count)
                {
                    case 0:
                        break;
                    default:
                        _selection = _selection.Where(
                          ew => SelectedPickingItems.Contains(ew.ID_PickingItem));
                        break;
                }

                switch (ddlGroupBy.SelectedValue)
                {
                    case ("YearPickingItem"):
                        _selection = _selection.GroupBy(
                            ew => new { ew.YearProductionDate, ew.ID_PickingItemDesc, ew.TypeDescription, ew.ItemTypeDescription, ew.Name, ew.Order, ew.UMDescription }).Select(
                                ewg =>
                                    new VW_ProductionOrderDetailsConsumption
                                    {
                                        ID = 0,
                                        YearProductionDate = ewg.Key.YearProductionDate,
                                        ID_PickingItemDesc = ewg.Key.ID_PickingItemDesc,
                                        TypeDescription = ewg.Key.TypeDescription,
                                        ItemTypeDescription = ewg.Key.ItemTypeDescription,
                                        Name = ewg.Key.Name,
                                        Order = ewg.Key.Order,
                                        UMDescription = ewg.Key.UMDescription,
                                        RawMaterialQuantity = ewg.Sum(ewgg => ewgg.RawMaterialQuantity),
                                        CurrentCost = ewg.Sum(ewgg => ewgg.CurrentCost),
                                        HistoricalCost = ewg.Sum(ewgg => ewgg.HistoricalCost)
                                    });
                        grdProductionOrderConsumptions.Columns[0].Visible = false;
                        grdProductionOrderConsumptions.Columns[2].Visible = false;
                        grdProductionOrderConsumptions.Columns[3].Visible = false;
                        break;

                    case ("MonthPickingItem"):
                        _selection = _selection.GroupBy(
                            ew => new { ew.YearProductionDate, ew.MonthProductionDate, ew.ID_PickingItemDesc, ew.TypeDescription, ew.ItemTypeDescription, ew.Name, ew.Order, ew.UMDescription }).Select(
                                ewg =>
                                    new VW_ProductionOrderDetailsConsumption
                                    {
                                        ID = 0,
                                        YearProductionDate = ewg.Key.YearProductionDate,
                                        MonthProductionDate = ewg.Key.MonthProductionDate,
                                        ID_PickingItemDesc = ewg.Key.ID_PickingItemDesc,
                                        TypeDescription = ewg.Key.TypeDescription,
                                        ItemTypeDescription = ewg.Key.ItemTypeDescription,
                                        Name = ewg.Key.Name,
                                        Order = ewg.Key.Order,
                                        UMDescription = ewg.Key.UMDescription,
                                        RawMaterialQuantity = ewg.Sum(ewgg => ewgg.RawMaterialQuantity),
                                        CurrentCost = ewg.Sum(ewgg => ewgg.CurrentCost),
                                        HistoricalCost = ewg.Sum(ewgg => ewgg.HistoricalCost)
                                    });
                        grdProductionOrderConsumptions.Columns[0].Visible = false;
                        grdProductionOrderConsumptions.Columns[3].Visible = false;

                        break;


                    default:

                        //foreach (DataControlField _column in grdProductionOrderConsumptions.Columns)
                        //{
                        //    _column.Visible = true;
                        //}
                        _selection = _selection.Take(256);
                        break;
                }
                _selection = _selection
                    .OrderBy(qt => qt.TypeDescription)
                    .ThenBy(qt => qt.ItemTypeDescription)
                    .ThenBy(qt => qt.Order)
                    .ThenBy(qt => qt.ID_PickingItemDesc);


                return _selection.ToArray();

            }
        }

        public VW_ProductionOrderDetailsConsumptionMCount[] GetDataSourceM()
        {

            using (QuotationDataContext _qc = new QuotationDataContext())
            {
                IQueryable<VW_ProductionOrderDetailsConsumptionMCount> _selection = _qc.VW_ProductionOrderDetailsConsumptionMCounts.Where(d => d.ProductionDate.HasValue);

                switch (itbNoOdP.Text)
                {
                    case (""):
                        break;
                    default:
                        _selection = _selection.Where(
                            ew => ew.ID_ProductionOrder == Convert.ToInt32(itbNoOdP.Text));
                        break;
                }

                switch (yctNumber.ReturnValue)
                {
                    case (""):
                        break;
                    default:
                        if (yctNumber.ReturnValue.StartsWith("/"))
                        {
                            _selection = _selection.Where(po => yctNumber.ReturnValue.Substring(yctNumber.ReturnValue.IndexOf('/') + 1) == po.Number.Substring(po.Number.IndexOf('/') + 1));
                        }
                        else if (yctNumber.ReturnValue.Length > 4)
                        {
                            _selection = _selection.Where(po => po.Number.Substring(2) == yctNumber.ReturnValue);
                        }
                        else
                        {
                            _selection = _selection.Where(po => po.Number.Substring(2, 2) == yctNumber.ReturnValue.Substring(2));
                        }
                        break;
                }

                switch (ddlCompanies.SelectedValue)
                {
                    case (""):
                        break;
                    default:
                        _selection = _selection.Where(
                            ew => ew.ID_Company == Convert.ToInt32(ddlCompanies.SelectedValue));
                        break;
                }

                switch (ddlTypes.SelectedValue)
                {
                    case (""):
                        break;
                    default:
                        _selection = _selection.Where(
                            ew => ew.TypeCode == Convert.ToInt32(ddlTypes.SelectedValue));
                        break;
                }

                switch (ddlItemTypes.SelectedValue)
                {
                    case (""):
                        break;
                    default:
                        _selection = _selection.Where(
                          ew => ew.ItemTypeCode == Convert.ToInt32(ddlItemTypes.SelectedValue));
                        break;
                }

                switch (SelectedMacroItem)
                {
                    case (null):
                        break;
                    default:
                        _selection = _selection.Where(
                          ew => ew.Macrovoce == SelectedMacroItem);
                        break;
                }


                _selection = _selection.Where(
                    qt => qt.ProductionDate >= btpPeriod.DataInizio &&
                            qt.ProductionDate <= btpPeriod.DataFine);

                switch (ddlGroupBy.SelectedValue)
                {
                    case ("YearPickingItem"):
                        _selection = _selection.GroupBy(
                            ew => new { ew.YearProductionDate, ew.descrizione, ew.TypeDescription, ew.ItemTypeDescription }).Select(
                                ewg =>
                                    new VW_ProductionOrderDetailsConsumptionMCount
                                    {
                                        ID_ProductionOrder = 0,
                                        Number = "",
                                        YearProductionDate = ewg.Key.YearProductionDate,
                                        descrizione = ewg.Key.descrizione,
                                        TypeDescription = ewg.Key.TypeDescription,
                                        ItemTypeDescription = ewg.Key.ItemTypeDescription,
                                        Count = ewg.Count(),
                                        RawMaterialQuantity = ewg.Sum(s => s.RawMaterialQuantity)
                                    });
                        grdProductionOrderConsumptionMCount.Columns[0].Visible = false;
                        grdProductionOrderConsumptionMCount.Columns[1].Visible = false;
                        grdProductionOrderConsumptionMCount.Columns[2].Visible = false;
                        grdProductionOrderConsumptionMCount.Columns[3].Visible = false;
                        break;

                    case ("MonthPickingItem"):
                        _selection = _selection.GroupBy(
                            ew => new { ew.YearProductionDate, ew.MonthProductionDate, ew.descrizione, ew.TypeDescription, ew.ItemTypeDescription }).Select(
                                ewg =>
                                    new VW_ProductionOrderDetailsConsumptionMCount
                                    {
                                        ID_ProductionOrder = 0,
                                        Number = "",
                                        YearProductionDate = ewg.Key.YearProductionDate,
                                        MonthProductionDate = ewg.Key.MonthProductionDate,
                                        descrizione = ewg.Key.descrizione,
                                        TypeDescription = ewg.Key.TypeDescription,
                                        ItemTypeDescription = ewg.Key.ItemTypeDescription,
                                        Count = ewg.Count(),
                                        RawMaterialQuantity = ewg.Sum(s => s.RawMaterialQuantity)
                                    });
                        grdProductionOrderConsumptionMCount.Columns[0].Visible = false;
                        grdProductionOrderConsumptionMCount.Columns[1].Visible = false;
                        grdProductionOrderConsumptionMCount.Columns[3].Visible = false;

                        break;


                    default:

                        _selection = _selection.Take(0);
                        break;
                }
                _selection = _selection
                    .OrderBy(qt => qt.TypeDescription)
                    .ThenBy(qt => qt.ItemTypeDescription)
                    .ThenBy(qt => qt.descrizione);


                return _selection.ToArray();

            }
        }



        protected void grdProductionOrderConsumptions_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.CannotDelete);
                e.ExceptionHandled = true;
            }
        }


        public void BindData()
        {
            grdProductionOrderConsumptions.DataSource = null;
            grdProductionOrderConsumptions.DataSource = GetDataSource();
            grdProductionOrderConsumptions.DataBind();

            grdProductionOrderConsumptionMCount.DataSource = null;
            grdProductionOrderConsumptionMCount.DataSource = GetDataSourceM();
            grdProductionOrderConsumptionMCount.DataBind();

            SelectedPickingItems.Clear();
            SelectedMacroItem = null;

        }

        protected void PersistSelection(object sender, EventArgs e)
        {
            Session["ProductionOrderConsumptionsStatsCompanyIDsSelector"] = ddlCompanies.SelectedValue;
            Session["ProductionOrderConsumptionsStatsItemTypesSelector"] = ddlItemTypes.SelectedValue;
            Session["ProductionOrderConsumptionsStatsTypesSelector"] = ddlTypes.SelectedValue;
            Session["ProductionOrderConsumptionsStatsGroupBySelector"] = ddlGroupBy.SelectedValue;

            BindData();


        }

        protected void grdProductionOrderConsumptions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Reload")
            {
                grdProductionOrderConsumptions.DataBind();
            }
        }


        protected void grdProductionOrderConsumptions_PreRender(object sender, EventArgs e)
        {
            //grdProductionOrderConsumptions.DataBind();
        }

        protected void lbtExportToExcel_Click(object sender, EventArgs e)
        {

            ExportToExcel(GridRender);
        }



        public override void VerifyRenderingInServerForm(Control control)
        {
            // base.VerifyRenderingInServerForm(control);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetPickingItems(string q)
        {
            try
            {
                using (var ctx = new QuotationDataContext())
                {
                    return new JavaScriptSerializer().Serialize(ctx.PickingItems.Where(c => !c.ItemDescription.StartsWith("**") && c.ItemDescription.Contains(q.Replace("%27", "'"))).Select(c => new { Code = c.ID, Name = c.ItemDescription }).ToList());
                }
            }
            catch (Exception ex)
            {
                return String.Format("Errore: {0}", ex.Message);
            }

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetMacroItems(string q)
        {
            try
            {
                using (var ctx = new QuotationDataContext())
                {
                    return new JavaScriptSerializer().Serialize(ctx.MacroItems.Where(c => !c.MacroItemDescription.StartsWith("**") && c.MacroItemDescription.Contains(q.Replace("%27", "'"))).Select(c => new { Code = c.ID, Name = c.MacroItemDescription }).ToList());
                }
            }
            catch (Exception ex)
            {
                return String.Format("Errore: {0}", ex.Message);
            }

        }

        protected void lbtReset_Click(object sender, EventArgs e)
        {
            ddlCompanies.SelectedIndex = 0;
            ddlTypes.SelectedIndex = 0;
            ddlItemTypes.SelectedIndex = 0;
            ddlGroupBy.SelectedIndex = 0;
            txtPickingItem.Text = "";
            hidPickingItem.Value = "";
            itbNoOdP.Text = "";
            yctNumber.Text = string.Empty;
            SelectedPickingItems = new List<int?>();
            SelectedMacroItem = null;

            Session["ProductionOrderConsumptionsStatsCompanyIDsSelector"] = ddlCompanies.SelectedValue;
            Session["ProductionOrderConsumptionsStatsItemTypesSelector"] = ddlItemTypes.SelectedValue;
            Session["ProductionOrderConsumptionsStatsTypesSelector"] = ddlTypes.SelectedValue;
            Session["ProductionOrderConsumptionsStatsGroupBySelector"] = ddlGroupBy.SelectedValue;

        }

        protected void grdProductionOrderConsumptionMCount_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdProductionOrderConsumptionMCount.PageIndex = e.NewPageIndex;
            //SwitchDependingControls(Mode.InputItems);
            SwitchDependingControls();
        }

        protected void mnuOperations_MenuItemClick(object sender, MenuEventArgs e)
        {

            using (var QuotationDataContext = new QuotationDataContext())
            {
                IEnumerable<PickingItem> _pickingItems = QuotationDataContext.PickingItems;
                IEnumerable<MacroItem> _macroItems = QuotationDataContext.MacroItems;
                var _menuPath = e.Item.Value.Split('.');
                var _cbk = new ComboKey(_menuPath);

                if (_cbk.Prefix == PICKINGITEM_PREFIX)
                {
                    SelectedPickingItems.Add(_pickingItems.First(pi => pi.ID == _cbk.CommonKey).ID);
                    SelectedMacroItem = null;
                }

                if (_cbk.Prefix == MACROITEM_PREFIX)
                {
                    var macroItem = _macroItems.First(pi => pi.ID == _cbk.CommonKey);
                    SelectedMacroItem = macroItem.ID;
                    foreach (var macroItemDetail in macroItem.MacroItemDetails)
                    {
                        SelectedPickingItems.Add(Convert.ToInt32(macroItemDetail.CommonKey));
                    }
                }

            }
            BindData();
        }



    }


}