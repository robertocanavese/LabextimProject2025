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
using DLLabExtim;
using UILabExtim;
using CMLabExtim;

namespace LabExtim
{
    public partial class EmployeesWorkingDayHoursStats : ProductionOrderController
    {
        private HtmlTextWriter m_HtmlTextWriter;
        private StringWriter m_StringWriter;
        private decimal[] totalsStatistiche0;
        private int[] _totalColumns;

        private string GridRender
        {
            get { return Session["EmployeesWorkingDayHoursStatsGridRender"].ToString(); }
            set { Session["EmployeesWorkingDayHoursStatsGridRender"] = value; }
        }

        private string PivotGridRender
        {
            get { return Session["EmployeesWorkingDaysHoursPivotRender"].ToString(); }
            set { Session["EmployeesWorkingDaysHoursPivotRender"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            //DynamicDataManager1.RegisterControl(grdEmployeesWorkingDaysHoursStats);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //MetaTable table = GridDataSource.GetTable();
            //InitSelectionData();

            if (!IsPostBack)
            {
                FillControls();
                //SwitchDependingControls(CurEmployeesWorkingDaysHoursConsoleMode);
            }

            FillDependingControls();
        }

        private void FillControls()
        {
            ldsOwners.AutoGenerateWhereClause = false;
            ldsOwners.Where = "ID_Company = " + CurrentCompanyId.ToString();

            btpPeriod.DataPartenzaOnLoad = DateTime.Now.Date;
            btpPeriod.AnnoIniziale = DateTime.Now.AddYears(-10).Year;
            btpPeriod.AnnoFinale = DateTime.Now.AddYears(+1).Year;

            ddlOrderBy.Items.Add(new ListItem("Operatore", "UniqueName"));
            ddlOrderBy.Items.Add(new ListItem("Data operazione", "ProductionDate"));
            ddlOrderBy.Items.Add(new ListItem("Fase", "Phase"));
            ddlOrderBy.Items.Add(new ListItem("Tipo voce", "ItemTypeDescription"));
            ddlOrderBy.Items.Add(new ListItem("Ore produzione", "ProductionTime"));

            ddlOrderBy.DataBind();

            //ddlExtractBy.Items.Add(new ListItem("Tutte le date", "All"));
            //ddlExtractBy.Items.Add(new ListItem("Giorno", "Day"));
            //ddlExtractBy.Items.Add(new ListItem("Mese", "Month"));
            //ddlExtractBy.DataBind();

            ddlGroupBy.Items.Add(new ListItem("Non raggruppare (max 256 voci)", "All"));
            ddlGroupBy.Items.Add(new ListItem("Operatore", "Owner"));
            ddlGroupBy.Items.Add(new ListItem("Operatore/Copie", "OwnerCopies"));
            ddlGroupBy.Items.Add(new ListItem("Mese", "Month"));
            ddlGroupBy.Items.Add(new ListItem("Mese/Copie", "MonthCopies"));
            ddlGroupBy.Items.Add(new ListItem("Operatore/Giorno", "OwnerDate"));
            ddlGroupBy.Items.Add(new ListItem("Operatore/Mese", "OwnerMonth"));
            ddlGroupBy.Items.Add(new ListItem("Operatore/Mese/Copie", "OwnerMonthCopies"));
            ddlGroupBy.Items.Add(new ListItem("Fase", "Phase"));
            ddlGroupBy.Items.Add(new ListItem("Fase/Copie", "PhaseCopies"));
            ddlGroupBy.Items.Add(new ListItem("Fase/Mese", "PhaseMonth"));
            ddlGroupBy.Items.Add(new ListItem("Fase/Mese/Copie", "PhaseMonthCopies"));
            ddlGroupBy.Items.Add(new ListItem("Operatore/Fase", "OwnerPhase"));
            ddlGroupBy.Items.Add(new ListItem("Operatore/Fase/Copie", "OwnerPhaseCopies"));
            ddlGroupBy.Items.Add(new ListItem("Operatore/Fase/Mese", "OwnerPhaseMonth"));
            ddlGroupBy.Items.Add(new ListItem("Operatore/Fase/Mese/Copie", "OwnerPhaseMonthCopies"));
            ddlGroupBy.Items.Add(new ListItem("Operatore/OdP/Fase/Mese/Copie", "OwnerOdPPhaseMonthCopies"));

            ddlGroupBy.Items.Add(new ListItem("OdP", "OdP"));
            ddlGroupBy.Items.Add(new ListItem("OdP/Fase", "OdPPhase"));

            ddlGroupBy.DataBind();

            ddlGroupBy.Items.FindByValue("Phase").Selected = true;
        }

        protected void FillDependingControls()
        {
            foreach (DataControlField _column in grdEmployeesWorkingDaysHoursStats.Columns)
            {
                _column.Visible = false;
            }
        }

        protected void SwitchDependingControls()
        {
        }

        protected void grdEmployeesWorkingDaysHours_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdEmployeesWorkingDaysHoursStats.PageIndex = e.NewPageIndex;
            //SwitchDependingControls(Mode.InputItems);
            SwitchDependingControls();
        }

        protected void lbtPrintEmployeesWorkingDaysHours_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                string.Format("{2}?{0}={1}", GenericReportKey, "EmployeesWorkingDaysHoursStats", GenericPrintPage), true);
        }

        protected void OnFilterSelectedIndexChanged(object sender, EventArgs e)
        {
            grdEmployeesWorkingDaysHoursStats.EditIndex = -1;
            grdEmployeesWorkingDaysHoursStats.PageIndex = 0;
        }

        protected void ddlOwners_DataBound(object sender, EventArgs e)
        {
            ddlOwners.Items.Insert(0, new ListItem("Tutti", ""));
            if (Session["EmployeesWorkingDaysHoursStatsOwnersSelector"] != null)
            {
                ddlOwners.Items.FindByValue(Session["EmployeesWorkingDaysHoursStatsOwnersSelector"].ToString()).Selected
                    = true;
            }
        }

        protected void ddlItemTypes_DataBound(object sender, EventArgs e)
        {
            ddlItemTypes.Items.Insert(0, new ListItem("Tutti", ""));
            if (Session["EmployeesWorkingDaysHoursStatsItemTypesSelector"] != null)
            {
                ddlItemTypes.Items.FindByValue(Session["EmployeesWorkingDaysHoursStatsItemTypesSelector"].ToString())
                    .Selected = true;
            }
        }

        protected void ddlPhases_DataBound(object sender, EventArgs e)
        {
            ddlPhases.Items.Insert(0, new ListItem("Tutte", ""));
            if (Session["EmployeesWorkingDaysHoursStatsPhasesBySelector"] != null)
            {
                ddlPhases.Items.FindByValue(Session["EmployeesWorkingDaysHoursStatsPhasesBySelector"].ToString())
                    .Selected = true;
            }
        }

        protected void ddlGroupBy_DataBound(object sender, EventArgs e)
        {
            if (Session["EmployeesWorkingDaysHoursStatsGroupBySelector"] != null)
            {
                try
                {
                    ddlGroupBy.Items.FindByValue(Session["EmployeesWorkingDaysHoursStatsGroupBySelector"].ToString())
                        .Selected = true;
                }
                catch
                {
                }
            }
        }

        protected void ddlOrderBy_DataBound(object sender, EventArgs e)
        {
            if (Session["EmployeesWorkingDaysHoursStatsOrderBySelector"] != null)
            {
                try
                {
                    ddlOrderBy.Items.FindByValue(Session["EmployeesWorkingDaysHoursStatsOrderBySelector"].ToString())
                        .Selected = true;
                }
                catch
                {
                }
            }
        }

        protected void ldsEmployeesWorkingDaysHours_Selected(object sender, LinqDataSourceStatusEventArgs e)
        {
            //if (e.TotalRowCount == 0 && grdEmployeesWorkingDaysHours.PageIndex == 0)
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

        protected void grdEmployeesWorkingDaysHours_DataBound(object sender, EventArgs e)
        {
            if (grdEmployeesWorkingDaysHoursStats.Rows.Count > 0)
            {
                m_StringWriter = new StringWriter();
                m_HtmlTextWriter = new HtmlTextWriter(m_StringWriter);
                grdEmployeesWorkingDaysHoursStats.RenderControl(m_HtmlTextWriter);
                GridRender = m_StringWriter.ToString();
                m_StringWriter.Dispose();
                m_HtmlTextWriter.Dispose();
            }
        }

        protected void grdEmployeesWorkingDaysHours_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='hand';this.style.textDecoration='underline';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";

                //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.grdPickingItems, "Select$" + e.Row.RowIndex);

                var _row = e.Row;
                if (((VW_EmployeesWorkingDayHour)e.Row.DataItem).ProductionTime > (36000000000m * 8m) &&
                    ddlGroupBy.SelectedValue == "All")
                {
                    _row.ForeColor = Color.Red;
                }

                //HyperLink _hypEdit = (HyperLink)e.Row.Cells[0].FindControl("hypEdit");
                //_hypEdit.Attributes.Add("onclick", "javascript:OpenBigItem('ProductionOrderPopup.aspx?" + POIdKey + "=" + ((ProductionOrder)e.Row.DataItem).ID + "')");
                //ImageButton _ibtUpdate = (ImageButton)e.Row.Cells[0].FindControl("ibtUpdate");

                //if (POUsageParameter == 1)
                //{
                //    _hypEdit.Visible = false;
                //    _ibtUpdate.Visible = false;
                //}
            }
        }


        public VW_EmployeesWorkingDayHour[] GetDataSource()
        {

            using (QuotationDataContext _qc = new QuotationDataContext())
            {
                _qc.CommandTimeout = 360;
                IQueryable<VW_EmployeesWorkingDayHour> _selection = _qc.VW_EmployeesWorkingDayHours.Where(d => d.ProductionTime > 0);

                // merge aziendale
                //_selection = _selection.Where(po => (CurrentCompanyId == -1 || po.ID_Company == CurrentCompanyId));

                switch (itbNoOdP.Text)
                {
                    case (""):
                        break;
                    default:
                        _selection = _selection.Where(
                            ew => ew.ID_ProductionOrder == Convert.ToInt32(itbNoOdP.Text));
                        break;
                }

                if (!string.IsNullOrEmpty(yctNumber.ReturnValue))
                {
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
                }

                if (!string.IsNullOrEmpty(hidCustomer.Value))
                {
                    int _temp = 0;
                    int.TryParse(hidCustomer.Value, out _temp);
                    _selection = _selection.Where(ew => ew.ID_Customer == _temp);
                }

                switch (ddlOwners.SelectedValue)
                {
                    case (""):
                        break;
                    default:
                        _selection = _selection.Where(
                            ew => ew.UniqueName == ddlOwners.SelectedItem.Text);
                        break;
                }
                switch (ddlPhases.SelectedValue)
                {
                    case (""):
                        break;
                    default:
                        _selection = _selection.Where(
                            ew => ew.ID_Phase == Convert.ToInt32(ddlPhases.SelectedValue));
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

                if (string.IsNullOrEmpty(itbNoOdP.Text))
                {
                    _selection = _selection.Where(
                        qt => qt.ProductionDate >= btpPeriod.DataInizio &&
                              qt.ProductionDate <= btpPeriod.DataFine);
                }

                switch (ddlGroupBy.SelectedValue)
                {
                    case ("Owner"):
                        _selection = _selection.GroupBy(ew => ew.UniqueName).Select(
                            ewg =>
                                new VW_EmployeesWorkingDayHour
                                {
                                    ID = 0,
                                    UniqueName = ewg.Key,
                                    YearProductionDate = ewg.Min(ew => ew.YearProductionDate),
                                    MonthProductionDate = ewg.Min(ew => ew.MonthProductionDate),
                                    ProductionDate = DateTime.MaxValue,
                                    ItemDescription = ewg.Min(ew => ew.ItemDescription),
                                    ItemTypeDescription = ewg.Min(ew => ew.ItemTypeDescription),
                                    TypeDescription = ewg.Min(ew => ew.TypeDescription),
                                    ProductionTime = ewg.Sum(ewgg => ewgg.ProductionTime)
                                });

                        grdEmployeesWorkingDaysHoursStats.Columns[0 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[7 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[8 + 1].Visible = true;

                        break;
                    case ("Month"):
                        _selection = _selection.GroupBy(
                            ew => new DateTime(ew.ProductionDate.Value.Year, ew.ProductionDate.Value.Month, 1)).Select(
                                ewg =>
                                    new VW_EmployeesWorkingDayHour
                                    {
                                        ID = 0,
                                        UniqueName = "Tutti",
                                        YearProductionDate = ewg.Min(ew => ew.YearProductionDate),
                                        MonthProductionDate = ewg.Min(ew => ew.MonthProductionDate),
                                        ProductionDate = ewg.Key,
                                        ItemDescription = ewg.Min(ew => ew.ItemDescription),
                                        ItemTypeDescription = ewg.Min(ew => ew.ItemTypeDescription),
                                        TypeDescription = ewg.Min(ew => ew.TypeDescription),
                                        ProductionTime = ewg.Sum(ewgg => ewgg.ProductionTime)
                                    });
                        grdEmployeesWorkingDaysHoursStats.Columns[1 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[2 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[7 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[8 + 1].Visible = true;
                        break;

                    case ("OwnerDate"):
                        _selection = _selection.GroupBy(
                            ew => new { ew.UniqueName, ew.ProductionDate.Value.Date }).Select(
                                ewg =>
                                    new VW_EmployeesWorkingDayHour
                                    {
                                        ID = 0,
                                        UniqueName = ewg.Key.UniqueName,
                                        YearProductionDate = ewg.Min(ew => ew.YearProductionDate),
                                        MonthProductionDate = ewg.Min(ew => ew.MonthProductionDate),
                                        ProductionDate = ewg.Key.Date,
                                        ItemDescription = ewg.Min(ew => ew.ItemDescription),
                                        ItemTypeDescription = ewg.Min(ew => ew.ItemTypeDescription),
                                        TypeDescription = ewg.Min(ew => ew.TypeDescription),
                                        ProductionTime = ewg.Sum(ew => ew.ProductionTime)
                                    });
                        grdEmployeesWorkingDaysHoursStats.Columns[0 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[3 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[7 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[8 + 1].Visible = true;
                        break;

                    case ("OwnerMonth"):
                        _selection = _selection.GroupBy(
                            ew => new { ew.UniqueName, ew.ProductionDate.Value.Year, ew.ProductionDate.Value.Month }).Select(
                                ewg =>
                                    new VW_EmployeesWorkingDayHour
                                    {
                                        ID = 0,
                                        UniqueName = ewg.Key.UniqueName,
                                        YearProductionDate = ewg.Min(ew => ew.YearProductionDate),
                                        MonthProductionDate = ewg.Min(ew => ew.MonthProductionDate),
                                        ProductionDate = new DateTime(ewg.Key.Year, ewg.Key.Month, 1),
                                        ItemDescription = ewg.Min(ew => ew.ItemDescription),
                                        ItemTypeDescription = ewg.Min(ew => ew.ItemTypeDescription),
                                        TypeDescription = ewg.Min(ew => ew.TypeDescription),
                                        ProductionTime = ewg.Sum(ew => ew.ProductionTime)
                                    });
                        grdEmployeesWorkingDaysHoursStats.Columns[0 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[1 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[2 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[7 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[8 + 1].Visible = true;
                        break;

                    case ("Phase"):
                        _selection = _selection.GroupBy(ew => ew.ItemDescription).Select(
                            ewg =>
                                new VW_EmployeesWorkingDayHour
                                {
                                    ID = 0,
                                    UniqueName = ewg.Key,
                                    YearProductionDate = ewg.Min(ew => ew.YearProductionDate),
                                    MonthProductionDate = ewg.Min(ew => ew.MonthProductionDate),
                                    ProductionDate = DateTime.MaxValue,
                                    ItemDescription = ewg.Min(ew => ew.ItemDescription),
                                    ItemTypeDescription = ewg.Min(ew => ew.ItemTypeDescription),
                                    TypeDescription = ewg.Min(ew => ew.TypeDescription),
                                    ProductionTime = ewg.Sum(ewgg => ewgg.ProductionTime)
                                });
                        grdEmployeesWorkingDaysHoursStats.Columns[4 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[7 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[8 + 1].Visible = true;
                        break;

                    case ("PhaseMonth"):
                        _selection = _selection.GroupBy(
                            ew => new { ew.ItemDescription, ew.ProductionDate.Value.Year, ew.ProductionDate.Value.Month })
                            .Select(
                                ewg =>
                                    new VW_EmployeesWorkingDayHour
                                    {
                                        ID = 0,
                                        UniqueName = ewg.Key.ItemDescription,
                                        YearProductionDate = ewg.Min(ew => ew.YearProductionDate),
                                        MonthProductionDate = ewg.Min(ew => ew.MonthProductionDate),
                                        ProductionDate = new DateTime(ewg.Key.Year, ewg.Key.Month, 1),
                                        ItemDescription = ewg.Min(ew => ew.ItemDescription),
                                        ItemTypeDescription = ewg.Min(ew => ew.ItemTypeDescription),
                                        TypeDescription = ewg.Min(ew => ew.TypeDescription),
                                        ProductionTime = ewg.Sum(ew => ew.ProductionTime)
                                    });
                        grdEmployeesWorkingDaysHoursStats.Columns[1 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[2 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[4 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[7 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[8 + 1].Visible = true;
                        break;

                    case ("OwnerPhase"):
                        _selection = _selection.GroupBy(ew => new { ew.UniqueName, ew.ItemDescription }).Select(
                            ewg =>
                                new VW_EmployeesWorkingDayHour
                                {
                                    ID = 0,
                                    UniqueName = ewg.Key.UniqueName,
                                    YearProductionDate = ewg.Min(ew => ew.YearProductionDate),
                                    MonthProductionDate = ewg.Min(ew => ew.MonthProductionDate),
                                    ProductionDate = DateTime.MaxValue,
                                    ItemDescription = ewg.Key.ItemDescription,
                                    ItemTypeDescription = ewg.Min(ew => ew.ItemTypeDescription),
                                    TypeDescription = ewg.Min(ew => ew.TypeDescription),
                                    ProductionTime = ewg.Sum(ew => ew.ProductionTime)
                                });
                        grdEmployeesWorkingDaysHoursStats.Columns[0 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[4 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[7 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[8 + 1].Visible = true;
                        break;

                    case ("OwnerPhaseMonth"):
                        _selection = _selection.GroupBy(
                            ew =>
                                new
                                {
                                    ew.UniqueName,
                                    ew.ItemDescription,
                                    ew.ProductionDate.Value.Year,
                                    ew.ProductionDate.Value.Month
                                }).Select(
                                    ewg =>
                                        new VW_EmployeesWorkingDayHour
                                        {
                                            ID = 0,
                                            UniqueName = ewg.Key.UniqueName,
                                            YearProductionDate = ewg.Min(ew => ew.YearProductionDate),
                                            MonthProductionDate = ewg.Min(ew => ew.MonthProductionDate),
                                            ProductionDate = DateTime.MaxValue,
                                            ItemDescription = ewg.Key.ItemDescription,
                                            ItemTypeDescription = ewg.Min(ew => ew.ItemTypeDescription),
                                            TypeDescription = ewg.Min(ew => ew.TypeDescription),
                                            ProductionTime = ewg.Sum(ew => ew.ProductionTime)
                                        });
                        grdEmployeesWorkingDaysHoursStats.Columns[0 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[1 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[2 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[4 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[7 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[8 + 1].Visible = true;
                        break;


                    case ("OwnerCopies"):
                        _selection = _selection.GroupBy(ew => new { ew.UniqueName, ew.RawMaterialY, ew.RawMaterialZ }).Select(
                            ewg =>
                                new VW_EmployeesWorkingDayHour
                                {
                                    ID = 0,
                                    UniqueName = ewg.Key.UniqueName,
                                    YearProductionDate = ewg.Min(ew => ew.YearProductionDate),
                                    MonthProductionDate = ewg.Min(ew => ew.MonthProductionDate),
                                    ProductionDate = DateTime.MaxValue,
                                    ItemDescription = ewg.Min(ew => ew.ItemDescription),
                                    ItemTypeDescription = ewg.Min(ew => ew.ItemTypeDescription),
                                    TypeDescription = ewg.Min(ew => ew.TypeDescription),
                                    ProductionTime = ewg.Sum(ewgg => ewgg.ProductionTime),
                                    RawMaterialX = ewg.Sum(ew => ew.RawMaterialX),
                                    RawMaterialY = ewg.Key.RawMaterialY,
                                    RawMaterialZ = ewg.Key.RawMaterialZ
                                });

                        grdEmployeesWorkingDaysHoursStats.Columns[0 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[7 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[8 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[9 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[10 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[11 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[12 + 1].Visible = true;

                        break;
                    case ("MonthCopies"):
                        _selection = _selection.GroupBy(
                            ew =>
                                new
                                {
                                    FirstDayOfMonth =
                                        new DateTime(ew.ProductionDate.Value.Year, ew.ProductionDate.Value.Month, 1),
                                    ew.RawMaterialY,
                                    ew.RawMaterialZ
                                }).Select(
                                    ewg =>
                                        new VW_EmployeesWorkingDayHour
                                        {
                                            ID = 0,
                                            UniqueName = "Tutti",
                                            YearProductionDate = ewg.Min(ew => ew.YearProductionDate),
                                            MonthProductionDate = ewg.Min(ew => ew.MonthProductionDate),
                                            ProductionDate = ewg.Key.FirstDayOfMonth,
                                            ItemDescription = ewg.Min(ew => ew.ItemDescription),
                                            ItemTypeDescription = ewg.Min(ew => ew.ItemTypeDescription),
                                            TypeDescription = ewg.Min(ew => ew.TypeDescription),
                                            ProductionTime = ewg.Sum(ewgg => ewgg.ProductionTime),
                                            RawMaterialX = ewg.Sum(ew => ew.RawMaterialX),
                                            RawMaterialY = ewg.Key.RawMaterialY,
                                            RawMaterialZ = ewg.Key.RawMaterialZ
                                        });
                        grdEmployeesWorkingDaysHoursStats.Columns[1 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[2 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[7 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[8 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[9 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[10 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[11 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[12 + 1].Visible = true;
                        break;

                    case ("OwnerMonthCopies"):
                        _selection = _selection.GroupBy(
                            ew =>
                                new
                                {
                                    ew.UniqueName,
                                    ew.ProductionDate.Value.Year,
                                    ew.ProductionDate.Value.Month,
                                    ew.RawMaterialY,
                                    ew.RawMaterialZ
                                }).Select(
                                    ewg =>
                                        new VW_EmployeesWorkingDayHour
                                        {
                                            ID = 0,
                                            UniqueName = ewg.Key.UniqueName,
                                            YearProductionDate = ewg.Min(ew => ew.YearProductionDate),
                                            MonthProductionDate = ewg.Min(ew => ew.MonthProductionDate),
                                            ProductionDate = new DateTime(ewg.Key.Year, ewg.Key.Month, 1),
                                            ItemDescription = ewg.Min(ew => ew.ItemDescription),
                                            ItemTypeDescription = ewg.Min(ew => ew.ItemTypeDescription),
                                            TypeDescription = ewg.Min(ew => ew.TypeDescription),
                                            ProductionTime = ewg.Sum(ew => ew.ProductionTime),
                                            RawMaterialX = ewg.Sum(ew => ew.RawMaterialX),
                                            RawMaterialY = ewg.Key.RawMaterialY,
                                            RawMaterialZ = ewg.Key.RawMaterialZ
                                        });
                        grdEmployeesWorkingDaysHoursStats.Columns[0 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[1 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[2 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[7 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[8 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[9 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[10 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[11 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[12 + 1].Visible = true;
                        break;

                    case ("PhaseCopies"):
                        _selection = _selection.GroupBy(ew => new { ew.ItemDescription, ew.RawMaterialY, ew.RawMaterialZ })
                            .Select(
                                ewg =>
                                    new VW_EmployeesWorkingDayHour
                                    {
                                        ID = 0,
                                        UniqueName = ewg.Key.ItemDescription,
                                        YearProductionDate = ewg.Min(ew => ew.YearProductionDate),
                                        MonthProductionDate = ewg.Min(ew => ew.MonthProductionDate),
                                        ProductionDate = DateTime.MaxValue,
                                        ItemDescription = ewg.Min(ew => ew.ItemDescription),
                                        ItemTypeDescription = ewg.Min(ew => ew.ItemTypeDescription),
                                        TypeDescription = ewg.Min(ew => ew.TypeDescription),
                                        ProductionTime = ewg.Sum(ewgg => ewgg.ProductionTime),
                                        RawMaterialX = ewg.Sum(ew => ew.RawMaterialX),
                                        RawMaterialY = ewg.Key.RawMaterialY,
                                        RawMaterialZ = ewg.Key.RawMaterialZ
                                    });
                        grdEmployeesWorkingDaysHoursStats.Columns[4 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[7 + 1].Visible = true;

                        grdEmployeesWorkingDaysHoursStats.Columns[9 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[10 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[11 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[12 + 1].Visible = true;
                        break;

                    case ("PhaseMonthCopies"):
                        _selection = _selection.GroupBy(
                            ew =>
                                new
                                {
                                    ew.ItemDescription,
                                    ew.ProductionDate.Value.Year,
                                    ew.ProductionDate.Value.Month,
                                    ew.RawMaterialY,
                                    ew.RawMaterialZ
                                }).Select(
                                    ewg =>
                                        new VW_EmployeesWorkingDayHour
                                        {
                                            ID = 0,
                                            UniqueName = ewg.Key.ItemDescription,
                                            YearProductionDate = ewg.Min(ew => ew.YearProductionDate),
                                            MonthProductionDate = ewg.Min(ew => ew.MonthProductionDate),
                                            ProductionDate = new DateTime(ewg.Key.Year, ewg.Key.Month, 1),
                                            ItemDescription = ewg.Min(ew => ew.ItemDescription),
                                            ItemTypeDescription = ewg.Min(ew => ew.ItemTypeDescription),
                                            TypeDescription = ewg.Min(ew => ew.TypeDescription),
                                            ProductionTime = ewg.Sum(ew => ew.ProductionTime),
                                            RawMaterialX = ewg.Sum(ew => ew.RawMaterialX),
                                            RawMaterialY = ewg.Key.RawMaterialY,
                                            RawMaterialZ = ewg.Key.RawMaterialZ
                                        });
                        grdEmployeesWorkingDaysHoursStats.Columns[1 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[2 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[4 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[7 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[8 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[9 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[10 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[11 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[12 + 1].Visible = true;
                        break;

                    case ("OwnerPhaseCopies"):
                        _selection = _selection.GroupBy(
                            ew => new { ew.UniqueName, ew.ItemDescription, ew.RawMaterialY, ew.RawMaterialZ }).Select(
                                ewg =>
                                    new VW_EmployeesWorkingDayHour
                                    {
                                        ID = 0,
                                        UniqueName = ewg.Key.UniqueName,
                                        YearProductionDate = ewg.Min(ew => ew.YearProductionDate),
                                        MonthProductionDate = ewg.Min(ew => ew.MonthProductionDate),
                                        ProductionDate = DateTime.MaxValue,
                                        ItemDescription = ewg.Key.ItemDescription,
                                        ItemTypeDescription = ewg.Min(ew => ew.ItemTypeDescription),
                                        TypeDescription = ewg.Min(ew => ew.TypeDescription),
                                        ProductionTime = ewg.Sum(ew => ew.ProductionTime),
                                        RawMaterialX = ewg.Sum(ew => ew.RawMaterialX),
                                        RawMaterialY = ewg.Key.RawMaterialY,
                                        RawMaterialZ = ewg.Key.RawMaterialZ
                                    });
                        grdEmployeesWorkingDaysHoursStats.Columns[0 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[4 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[7 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[8 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[9 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[10 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[11 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[12 + 1].Visible = true;
                        break;

                    case ("OwnerPhaseMonthCopies"):
                        _selection = _selection.GroupBy(
                            ew =>
                                new
                                {
                                    ew.UniqueName,
                                    ew.ItemDescription,
                                    ew.ProductionDate.Value.Year,
                                    ew.ProductionDate.Value.Month,
                                    ew.RawMaterialY,
                                    ew.RawMaterialZ
                                }).Select(
                                    ewg =>
                                        new VW_EmployeesWorkingDayHour
                                        {
                                            ID = 0,
                                            UniqueName = ewg.Key.UniqueName,
                                            YearProductionDate = ewg.Min(ew => ew.YearProductionDate),
                                            MonthProductionDate = ewg.Min(ew => ew.MonthProductionDate),
                                            ProductionDate = DateTime.MaxValue,
                                            ItemDescription = ewg.Key.ItemDescription,
                                            ItemTypeDescription = ewg.Min(ew => ew.ItemTypeDescription),
                                            TypeDescription = ewg.Min(ew => ew.TypeDescription),
                                            ProductionTime = ewg.Sum(ew => ew.ProductionTime),
                                            RawMaterialX = ewg.Sum(ew => ew.RawMaterialX),
                                            RawMaterialY = ewg.Key.RawMaterialY,
                                            RawMaterialZ = ewg.Key.RawMaterialZ
                                        });
                        grdEmployeesWorkingDaysHoursStats.Columns[0 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[1 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[2 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[4 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[7 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[8 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[9 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[10 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[11 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[12 + 1].Visible = true;
                        break;

                    case ("OwnerOdPPhaseMonthCopies"):
                        _selection = _selection.GroupBy(
                            ew =>
                                new
                                {
                                    ew.UniqueName,
                                    ew.ID_ProductionOrder,
                                    ew.ItemDescription,
                                    ew.ProductionDate.Value.Year,
                                    ew.ProductionDate.Value.Month,
                                    ew.RawMaterialY,
                                    ew.RawMaterialZ
                                }).Select(
                                    ewg =>
                                        new VW_EmployeesWorkingDayHour
                                        {
                                            ID = 0,
                                            ID_ProductionOrder = ewg.Key.ID_ProductionOrder,
                                            UniqueName = ewg.Key.UniqueName,
                                            YearProductionDate = ewg.Min(ew => ew.YearProductionDate),
                                            MonthProductionDate = ewg.Min(ew => ew.MonthProductionDate),
                                            ProductionDate = DateTime.MaxValue,
                                            ItemDescription = ewg.Key.ItemDescription,
                                            ItemTypeDescription = ewg.Min(ew => ew.ItemTypeDescription),
                                            TypeDescription = ewg.Min(ew => ew.TypeDescription),
                                            ProductionTime = ewg.Sum(ew => ew.ProductionTime),
                                            RawMaterialX = ewg.Sum(ew => ew.RawMaterialX),
                                            RawMaterialY = ewg.Key.RawMaterialY,
                                            RawMaterialZ = ewg.Key.RawMaterialZ
                                        });
                        grdEmployeesWorkingDaysHoursStats.Columns[0].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[0 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[1 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[2 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[4 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[7 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[8 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[9 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[10 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[11 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[12 + 1].Visible = true;
                        break;


                    case ("OdP"):
                        _selection = _selection.GroupBy(ew => ew.ID_ProductionOrder).Select(
                            ewg =>
                                new VW_EmployeesWorkingDayHour
                                {
                                    ID = 0,
                                    ID_ProductionOrder = ewg.Key,
                                    UniqueName = "Tutti",
                                    YearProductionDate = ewg.Min(ew => ew.YearProductionDate),
                                    MonthProductionDate = ewg.Min(ew => ew.MonthProductionDate),
                                    ProductionDate = DateTime.MaxValue,
                                    ItemDescription = ewg.Min(ew => ew.ItemDescription),
                                    ItemTypeDescription = ewg.Min(ew => ew.ItemTypeDescription),
                                    TypeDescription = ewg.Min(ew => ew.TypeDescription),
                                    ProductionTime = ewg.Sum(ewgg => ewgg.ProductionTime)
                                });
                        grdEmployeesWorkingDaysHoursStats.Columns[0].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[4 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[7 + 1].Visible = true;
                        break;

                    case ("OdPPhase"):
                        _selection = _selection.GroupBy(ew => new { ew.ID_ProductionOrder, ew.ID_Phase, ew.RawMaterialY, ew.RawMaterialZ  }).Select(
                            ewg =>
                                new VW_EmployeesWorkingDayHour
                                {
                                    ID = 0,
                                    ID_ProductionOrder = ewg.Key.ID_ProductionOrder,
                                    UniqueName = "Tutti",
                                    ID_Phase = ewg.Key.ID_Phase,
                                    YearProductionDate = ewg.Min(ew => ew.YearProductionDate),
                                    MonthProductionDate = ewg.Min(ew => ew.MonthProductionDate),
                                    ProductionDate = DateTime.MaxValue,
                                    ItemDescription = ewg.Min(ew => ew.ItemDescription),
                                    ItemTypeDescription = ewg.Min(ew => ew.ItemTypeDescription),
                                    TypeDescription = ewg.Min(ew => ew.TypeDescription),
                                    ProductionTime = ewg.Sum(ew => ew.ProductionTime),

                                    RawMaterialX = ewg.Sum(ew => ew.RawMaterialX),
                                    RawMaterialY = ewg.Key.RawMaterialY,
                                    RawMaterialZ = ewg.Key.RawMaterialZ
                                });
                        grdEmployeesWorkingDaysHoursStats.Columns[0].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[1 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[2 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[4 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[7 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[8 + 1].Visible = true;

                        grdEmployeesWorkingDaysHoursStats.Columns[9 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[10 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[11 + 1].Visible = true;
                        grdEmployeesWorkingDaysHoursStats.Columns[12 + 1].Visible = true;

                        break;


                    //default:


                    //    foreach (DataControlField _column in grdEmployeesWorkingDaysHoursStats.Columns)
                    //    {
                    //        _column.Visible = true;
                    //    }
                    //    _selection = _selection.Take(256);
                    //    break;


                    default:

                        _selection = _selection.GroupBy(
                            ew =>
                                new
                                {
                                    ew.ID_ProductionOrder,
                                    ew.UniqueName,
                                    ew.ItemDescription,
                                    ew.ProductionDate,
                                    ew.RawMaterialY,
                                    ew.RawMaterialZ
                                }).Select(
                                    ewg =>
                                        new VW_EmployeesWorkingDayHour
                                        {
                                            ID = 0,
                                            ID_ProductionOrder = ewg.Key.ID_ProductionOrder,
                                            UniqueName = ewg.Key.UniqueName,
                                            //YearProductionDate = ewg.Min(ew => ew.YearProductionDate),
                                            // MonthProductionDate = ewg.Min(ew => ew.MonthProductionDate),
                                            ProductionDate = ewg.Key.ProductionDate,
                                            ItemDescription = ewg.Key.ItemDescription,
                                            ItemTypeDescription = ewg.Min(ew => ew.ItemTypeDescription),
                                            TypeDescription = ewg.Min(ew => ew.TypeDescription),
                                            ProductionTime = ewg.Sum(ew => ew.ProductionTime),
                                            RawMaterialX = ewg.Sum(ew => ew.RawMaterialX),
                                            RawMaterialY = ewg.Key.RawMaterialY,
                                            RawMaterialZ = ewg.Key.RawMaterialZ
                                        });

                        foreach (DataControlField _column in grdEmployeesWorkingDaysHoursStats.Columns)
                        {
                            _column.Visible = true;
                        }
                        _selection = _selection.Take(256);
                        break;
                }

                switch (ddlOrderBy.SelectedValue)
                {
                    case ("UniqueName"):
                        _selection = _selection.OrderBy(qt => qt.UniqueName);
                        break;
                    case ("ProductionDate"):
                        _selection = _selection.OrderBy(qt => qt.ProductionDate);
                        break;
                    case ("Phase"):
                        _selection = _selection.OrderBy(qt => qt.ItemDescription);
                        break;
                    case ("ItemType"):
                        _selection = _selection.OrderBy(qt => qt.ItemTypeDescription);
                        break;
                    case ("ProductionTime"):
                        _selection = _selection.OrderBy(qt => qt.ProductionTime);
                        break;


                    default:
                        break;
                }
                return _selection.ToArray();

            }
        }



        protected void grdEmployeesWorkingDaysHours_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if (e.Exception != null)
            {
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.CannotDelete);
                e.ExceptionHandled = true;
            }
        }


        public void BindData()
        {

            if (pnlDefault.Visible == true)
            {
                grdEmployeesWorkingDaysHoursStats.DataSource = null;
                grdEmployeesWorkingDaysHoursStats.DataSource = GetDataSource();
                grdEmployeesWorkingDaysHoursStats.DataBind();

            }
            if (pnlPivot.Visible == true)
            {
                BindPivotData();
            }

        }

        protected void PersistSelection(object sender, EventArgs e)
        {
            Session["EmployeesWorkingDaysHoursStatsItemTypesSelector"] = ddlItemTypes.SelectedValue;
            Session["EmployeesWorkingDaysHoursStatsOwnersSelector"] = ddlOwners.SelectedValue;
            Session["EmployeesWorkingDaysHoursStatsPhasesBySelector"] = ddlPhases.SelectedValue;
            Session["EmployeesWorkingDaysHoursStatsGroupBySelector"] = ddlGroupBy.SelectedValue;
            Session["EmployeesWorkingDaysHoursStatsOrderBySelector"] = ddlOrderBy.SelectedValue;
            if (pnlDefault.Visible == true)
            {
                BindData();
            }
            if (pnlPivot.Visible == true)
            {
                BindPivotData();
            }


        }

        protected void grdEmployeesWorkingDaysHours_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Reload")
            {
                grdEmployeesWorkingDaysHoursStats.DataBind();
            }
        }

        //protected void txtDateStart_TextChanged(object sender, EventArgs e)
        //{
        //    DateTime _curDate = DateTime.Now.Date;
        //    DateTime.TryParse(txtDateStart.Text, out _curDate);
        //    txtDateStart_CalendarExtender.SelectedDate = _curDate;

        //}

        //protected void txtDateEnd_TextChanged(object sender, EventArgs e)
        //{
        //    DateTime _curDate = DateTime.Now.Date;
        //    DateTime.TryParse(txtDateEnd.Text, out _curDate);
        //    txtDateEnd_CalendarExtender.SelectedDate = _curDate;

        //}

        protected void grdEmployeesWorkingDaysHoursStats_PreRender(object sender, EventArgs e)
        {
            //grdEmployeesWorkingDaysHoursStats.DataBind();
        }

        protected void lbtExportToExcel_Click(object sender, EventArgs e)
        {

            ExportToExcel(GridRender);
        }



        public override void VerifyRenderingInServerForm(Control control)
        {
            // base.VerifyRenderingInServerForm(control);
        }

        protected void BindPivotData()
        {
            GridView curGrid = grdEmployeesWorkingDaysHoursPivot;
            lblSuccess.Text = "";

            try
            {
                int customerId = -1;
                if (!string.IsNullOrEmpty(hidCustomer.Value))
                {

                    int.TryParse(hidCustomer.Value, out customerId);
                }

                List<object[]> _data = PivotDataAdapters.GetEmployeesWorkingDayHoursPivot(CurrentCompanyId, customerId, btpPeriod.DataInizio.ToString("yyyyMMdd"), btpPeriod.DataFine.ToString("yyyyMMdd"));
                if (_data.Count > 0)
                {
                    object[] _data0 = _data[0];
                    curGrid.Columns.Clear();


                    _totalColumns = new int[_data[0].Length];


                    for (int col = 0; col <= _data0.Length - 1; col++)
                    {
                        TemplateField _templateField = new TemplateField();
                        // _templateField.HeaderText = _data0(col).ToString()
                        _templateField.HeaderText = "<span title='Ore operatori del mese' >" + _data0[col].ToString() + "</span>";
                        curGrid.Columns.Add(_templateField);
                        _totalColumns[col] = col;
                    }
                    TemplateField _templateFieldT = new TemplateField();
                    _templateFieldT.HeaderText = "&nbsp;&nbsp;&nbsp;<span title='Totale del periodo' >Totale</span>&nbsp;&nbsp;&nbsp;";
                    curGrid.Columns.Add(_templateFieldT);
                    //_totalColumns[_data[0].Length] = _data[0].Length;


                    decimal[] totalsStatistiche0 = new decimal[_data[0].Length];
                    _data.RemoveAt(0);
                    curGrid.DataSource = _data;
                    curGrid.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblSuccess.Text = "Selezionare un mese completo per generare la tabella Pivot";
            }
        }


        protected void lbtExportPivotToExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel(PivotGridRender);
        }

        protected void lbtGoToPivot_Click(object sender, EventArgs e)
        {
            pnlDefault.Visible = false;
            pnlPivot.Visible = true;
            BindPivotData();
        }

        protected void lbtGoToDefault_Click(object sender, EventArgs e)
        {
            pnlDefault.Visible = true;
            pnlPivot.Visible = false;
        }

        protected void grdEmployeesWorkingDaysHoursPivot_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                object[] _item = (object[])e.Row.DataItem;
                Int64 _rowTotal = 0;
                Int64 _value;
                for (int col = 0; col <= _item.Length - 1; col++)
                {
                    if (!Int64.TryParse(_item[col].ToString(), out  _value))
                    {
                        e.Row.Cells[col].Text = _item[col].ToString();
                        e.Row.Cells[col].HorizontalAlign = HorizontalAlign.Left;
                    }
                    else
                    {
                        e.Row.Cells[col].Text = (_item[col] == DBNull.Value ? "0" : Utilities.DisplayAsCentiHours(Convert.ToInt64(_item[col])));
                        _rowTotal += _value;
                    }
                }
                e.Row.Cells[_item.Length].Text = Utilities.DisplayAsCentiHours(Convert.ToInt64(_rowTotal));
                e.Row.Cells[_item.Length].Font.Bold = true;
            }

            BuildGridTotals(ref totalsStatistiche0, _totalColumns, new int[] { -1, -1, -1 }, e);

        }

        protected void grdEmployeesWorkingDaysHoursPivot_DataBound(object sender, EventArgs e)
        {
            if (grdEmployeesWorkingDaysHoursPivot.Rows.Count > 0)
            {
                m_StringWriter = new StringWriter();
                m_HtmlTextWriter = new HtmlTextWriter(m_StringWriter);
                grdEmployeesWorkingDaysHoursPivot.RenderControl(m_HtmlTextWriter);
                PivotGridRender = m_StringWriter.ToString();
                m_StringWriter.Dispose();
                m_HtmlTextWriter.Dispose();
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

        protected void lbtResetFilter_Click(object sender, EventArgs e)
        {
            itbNoOdP.Text = string.Empty;
            txtCustomer.Text = "";
            hidCustomer.Value = "";
            btpPeriod.SetData(DateTime.Now);

            ddlOwners.SelectedIndex = 0;
            ddlPhases.SelectedIndex = 0;
            ddlItemTypes.SelectedIndex = 0;
            ddlGroupBy.Items.FindByValue("Phase").Selected = true;
            ddlOrderBy.SelectedIndex = 0;

            PersistSelection(null, null);


        }
    }


}