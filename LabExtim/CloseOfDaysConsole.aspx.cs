using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.DynamicData;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using DLLabExtim;
using UILabExtim;


namespace LabExtim
{
    public partial class CloseOfDaysConsole : BaseController
    {

        private HtmlTextWriter m_HtmlTextWriter;
        private StringWriter m_StringWriter;

        private string GridRender
        {
            get { return Session["CloseOfDaysGridRender"] == null ? string.Empty : Session["CloseOfDaysGridRender"].ToString(); }
            set { Session["CloseOfDaysGridRender"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(grdCloseOfDays);
            senMain.SearchClick += senMain_SearchClick;
            senMain.EmptyClick += senMain_EmptyClick;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // pagina riservata agli amministratori
            if (WebUser == null || !WebUser.IsAdministrator)
            {
                Response.Redirect("~/Home.aspx", true);
                return;
            }

            if (!IsPostBack)
            {
                // ordinamento iniziale: data discendente
                ViewState["SortExpression"] = "ProductionDate";
                ViewState["SortDirection"] = "DESC";
                PopulateSearchEngine();
                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.Empty);
            }
            senMain_SearchClick(null, null);
        }

        private void PopulateSearchEngine()
        {
            senMain.ShowIdField = false;
            senMain.LblTextField1Text = "Operatore";
            senMain.LblTextField2Text = "Cerca testo nelle note...";
            senMain.LblDateFromText = "Data da";
            senMain.LblDateToText = "Data a";

            senMain.DdlOrderBy.Items.Add(new ListItem("Più recente", "ProductionDate"));
            senMain.DdlOrderBy.Items.Add(new ListItem("Più recente, Operatore", "ProductionDateOperator"));
        }

        public void senMain_SearchClick(object sender, EventArgs e)
        {
            ldsCloseOfDays.AutoGenerateWhereClause = false;
            ldsCloseOfDays.WhereParameters.Clear();
            var _filter = "TRUE ";

            var _operator = senMain.TextField1Text.Trim();
            if (_operator != string.Empty)
            {
                ldsCloseOfDays.WhereParameters.Add("Operator", DbType.String, _operator);
                _filter += " AND Employee.UniqueName.Contains(@Operator)";
            }

            var _note = senMain.TextField2Text.Trim();
            if (_note != string.Empty)
            {
                ldsCloseOfDays.WhereParameters.Add("Note", DbType.String, _note);
                _filter += " AND Note.Contains(@Note)";
            }

            DateTime _dateFrom;
            if (DateTime.TryParse(senMain.TextDateFromText, out _dateFrom))
            {
                ldsCloseOfDays.WhereParameters.Add("DateFrom", DbType.DateTime, _dateFrom.Date.ToString());
                _filter += " AND ProductionDate >= @DateFrom";
            }

            DateTime _dateTo;
            if (DateTime.TryParse(senMain.TextDateToText, out _dateTo))
            {
                // estremo superiore incluso: si filtra sul giorno successivo
                ldsCloseOfDays.WhereParameters.Add("DateTo", DbType.DateTime, _dateTo.Date.AddDays(1).ToString());
                _filter += " AND ProductionDate < @DateTo";
            }

            ldsCloseOfDays.Where = _filter != "TRUE " ? _filter : string.Empty;

            grdCloseOfDays.PageSize = 30;
            Session["CloseOfDays_PagerSize"] = "30";
            grdCloseOfDays.PageIndex = 0;
        }

        public void senMain_EmptyClick(object sender, EventArgs e)
        {
            // dopo il reset si torna all'ordinamento della tendina (data discendente)
            ViewState["SortExpression"] = "";
            senMain_SearchClick(null, null);
        }

        protected void ldsCloseOfDays_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var table = ldsCloseOfDays.GetTable();
            var _qc = (QuotationDataContext)table.CreateContext();

            ldsCloseOfDays.OrderByParameters.Clear();
            ldsCloseOfDays.AutoGenerateOrderByClause = false;

            if (ViewState["SortExpression"] != null && !string.IsNullOrEmpty(ViewState["SortExpression"].ToString()))
            {
                e.Result = _qc.CloseOfDays.OrderBy(string.Format("{0} {1}", ViewState["SortExpression"], ViewState["SortDirection"]));
            }
            else
            {
                switch (senMain.DdlOrderBy.SelectedValue)
                {
                    case ("ProductionDateOperator"):
                        e.Result = _qc.CloseOfDays.OrderByDescending(c => c.ProductionDate).ThenBy(c => c.Employee.Surname);
                        break;
                    default: // "" e "ProductionDate"
                        e.Result = _qc.CloseOfDays.OrderByDescending(c => c.ProductionDate);
                        break;
                }
            }
        }

        protected void grdCloseOfDays_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCloseOfDays.PageIndex = Math.Max(0, e.NewPageIndex);
        }

        protected void grdCloseOfDays_Sorting(object sender, GridViewSortEventArgs e)
        {
            ViewState["SortExpression"] = e.SortExpression;
            ViewState["SortDirection"] = e.SortDirection;
        }

        protected void grdCloseOfDays_DataBound(object sender, EventArgs e)
        {
            if (grdCloseOfDays.Rows.Count > 0)
            {
                try
                {
                    m_StringWriter = new StringWriter();
                    m_HtmlTextWriter = new HtmlTextWriter(m_StringWriter);
                    grdCloseOfDays.RenderControl(m_HtmlTextWriter);
                    GridRender = m_StringWriter.ToString().Replace("<textarea", "<span").Replace("</textarea", "</span").Replace("<a", "<span").Replace("</a", "</span").Replace("€", "Euro");
                    m_StringWriter.Dispose();
                    m_HtmlTextWriter.Dispose();
                }
                catch
                {
                }
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }

        protected void lbtExportToExcel_Click(object sender, EventArgs e)
        {
            grdCloseOfDays.PagerSettings.Visible = false;

            int oldPageSize = grdCloseOfDays.PageSize;
            grdCloseOfDays.PageSize = Int16.MaxValue;
            grdCloseOfDays.SetPageIndex(0);
            grdCloseOfDays.DataBind();
            ExportToExcel(GridRender);

            grdCloseOfDays.PageSize = oldPageSize;
            grdCloseOfDays.SetPageIndex(0);
            grdCloseOfDays.PagerSettings.Visible = true;
        }

        protected void lbtUpdateGrid_Click(object sender, EventArgs e)
        {
            senMain_SearchClick(null, null);
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
                        return new JavaScriptSerializer().Serialize(ctx.Employees.Where(c => c.ID_Company == 1 && c.UniqueName.ToLower().Contains(q.ToLower())).Select(c => new { Id = c.ID, UniqueName = c.UniqueName }).ToList());
                    else if (System.Web.HttpContext.Current.Session["CurrentCompanyId"].ToString() == "2")
                        return new JavaScriptSerializer().Serialize(ctx.Employees.Where(c => c.ID_Company == 2 && c.UniqueName.ToLower().Contains(q.ToLower())).Select(c => new { Id = c.ID, UniqueName = c.UniqueName }).ToList());
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
