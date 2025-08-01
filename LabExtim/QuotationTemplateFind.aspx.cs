using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using DLLabExtim;
using UILabExtim;

using System.Web.DynamicData;


namespace LabExtim
{
    public partial class QuotationTemplateFind : QuotationController //System.Web.UI.Page
    {
        //protected MetaTable table;
        protected int insertedKey;

        public OrderedDictionary SelectionData
        {
            get { return ViewState["SelectionData"] as OrderedDictionary; }
            set { ViewState["SelectionData"] = value; }
        }

        public void InitSelectionData()
        {
            SelectionData = ViewState["SelectionData"] as OrderedDictionary;
            if (SelectionData == null)
            {
                SelectionData = new OrderedDictionary();
                ViewState["SelectionData"] = SelectionData;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(DetailsView1);
            DynamicDataManager1.RegisterControl(grdDetails);
            InitSelectionData();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var _result = new QuotationService().DelLocks(WebUser.Member.ProviderUserKey.ToString(), SessionTimeStamp);
            //table = DetailsDataSource.GetTable();
            //Title = table.DisplayName;
            FillDependingControls();
        }

        //protected void FillDependingControls(DetailsViewInsertEventArgs e)
        protected void FillDependingControls()
        {
            //IEnumerable<Quotation> _currentQuotations =
            //    (IEnumerable<Quotation>)QuotationDataContext.Quotations.Where(Quotation =>
            //       e.Values["ID_Quotation"] != null ? Quotation.ID == Convert.ToInt32(e.Values["ID_Quotation"]) : Quotation.ID > 0 &&
            //       e.Values["CustomerCode"] != null ? Quotation.Customer.Code == e.Values["CustomerCode"].ToString() : Quotation.ID > 0 &&
            //       e.Values["Subject"] != null ? Quotation.Subject.Contains(e.Values["Subject"].ToString()) : Quotation.ID > 0 &&
            //       e.Values["StartDate"] != null ? Quotation.Date >= Convert.ToDateTime(e.Values["StartDate"]) : Quotation.ID > 0 &&
            //       e.Values["EndDate"] != null ? Quotation.Date <= Convert.ToDateTime(e.Values["EndDate"]) : Quotation.ID > 0);


            //IEnumerable<Quotation> _currentQuotations0 =
            //    (IEnumerable<Quotation>)QuotationDataContext.Quotations.Where(Quotation =>
            //       e.Values["ID_Quotation"] != null ? Quotation.ID == Convert.ToInt32(e.Values["ID_Quotation"]) : true);

            //IEnumerable<Quotation> _currentQuotations1 =
            //    (IEnumerable<Quotation>)_currentQuotations0.Where(Quotation =>
            //       e.Values["CustomerCode"] != null ? Quotation.Customer.Code == e.Values["CustomerCode"].ToString()  : true);

            //IEnumerable<Quotation> _currentQuotations2 =
            //    (IEnumerable<Quotation>)_currentQuotations1.Where(Quotation =>
            //       e.Values["Subject"] != null ? Quotation.Subject.Contains(e.Values["Subject"].ToString())  : true);

            //IEnumerable<Quotation> _currentQuotations3 =
            //    (IEnumerable<Quotation>)_currentQuotations2.Where(Quotation =>
            //       e.Values["StartDate"] != null ? Quotation.Date >= Convert.ToDateTime(e.Values["StartDate"]) : true);

            //IEnumerable<Quotation> _currentQuotations4 =
            //    (IEnumerable<Quotation>)_currentQuotations3.Where(Quotation =>
            //       e.Values["EndDate"] != null ? Quotation.Date >= Convert.ToDateTime(e.Values["EndDate"]) : true);

            //grdDetails.DataSource = _currentQuotations0;
            //grdDetails.DataBind();

            //string filterString = "0=0";
            //if (e.Values["ID_Quotation"] != null)
            //{
            //    filterString += " AND ID = @ID";
            //    GridDataSource.WhereParameters.Add("ID", DbType.Int32, e.Values["ID_Quotation"].ToString());
            //}
            //if (e.Values["CustomerCode"] != null)
            //{
            //    filterString += " AND CustomerCode = @CustomerCode";
            //    GridDataSource.WhereParameters.Add("CustomerCode", DbType.String, e.Values["CustomerCode"].ToString());
            //}
            //if (e.Values["Subject"] != null)
            //{
            //    filterString += " AND Subject LIKE @Subject";
            //    GridDataSource.WhereParameters.Add("Subject", DbType.String, e.Values["Subject"].ToString());
            //}
            //if (e.Values["StartDate"] != null)
            //{
            //    filterString += " AND Date >= @StartDate";
            //    GridDataSource.WhereParameters.Add("StartDate", DbType.DateTime, e.Values["StartDate"].ToString());
            //}
            //if (e.Values["EndDate"] != null)
            //{
            //    filterString += " AND Date <= @EndDate";
            //    GridDataSource.WhereParameters.Add("EndDate", DbType.DateTime, e.Values["EndDate"].ToString());
            //}

            if (SelectionData.Count > 0)
            {
                GridDataSource.WhereParameters.Clear();

                var filterString = "0=0";
                //if (SelectionData["ID_QuotationTemplate"] != null)
                //{
                //    filterString += " AND ID = @ID";
                //    GridDataSource.WhereParameters.Add("ID", DbType.Int32,
                //        SelectionData["ID_QuotationTemplate"].ToString());
                //}
                if (SelectionData["Description"] != null)
                {
                    filterString += " AND Description.Contains(@Description)";
                    GridDataSource.WhereParameters.Add("Description", DbType.String,
                        SelectionData["Description"].ToString());
                }
                if (SelectionData["TypeCode"] != null)
                {
                    filterString += " AND TypeCode = @TypeCode";
                    GridDataSource.WhereParameters.Add("TypeCode", DbType.Int32,
                            SelectionData["TypeCode"].ToString());
                }
                if (SelectionData["ItemTypeCode"] != null)
                {
                    filterString += " AND ItemTypeCode = @ItemTypeCode";
                    GridDataSource.WhereParameters.Add("ItemTypeCode", DbType.Int32,
                            SelectionData["ItemTypeCode"].ToString());
                }

                GridDataSource.Where = filterString;
                GridDataSource.AutoGenerateWhereClause = false;
            }
        }

        protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            if (e.CommandName == DataControlCommands.CancelCommandName)
            {
                SelectionData.Clear();
            }
        }

        protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            if (e.Exception == null || e.ExceptionHandled)
            {
                //QuotationHeader = new KeyValuePair<int,string>(insertedKey,e.Values["Name"].ToString());
                Response.Redirect(
                    string.Format("{2}?{0}={1}", QuotationTemplateKey, insertedKey.ToString().TrimEnd(),
                        QuotationTemplateConsolePage), true);
            }
        }

        //protected void DetailsDataSource_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        //{
        //    insertedKey = ((DLLabExtim.Quotation)e.Result).ID;
        //}


        protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            e.Cancel = true;
            //FillDependingControls(e);
            SelectionData = (OrderedDictionary) e.Values;
            FillDependingControls();
        }

        //protected void grdDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    grdDetails.PageIndex = e.NewPageIndex;
        //    grdDetails.DataBind();

        //}

        protected void OnFilterSelectedIndexChanged(object sender, EventArgs e)
        {
            grdDetails.PageIndex = 0;
        }

        protected void grdDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                var _quotationTemplateHeader = e.CommandArgument.ToString().Split('|');
                QuotationTemplateHeader = new KeyValuePair<int, string>(Convert.ToInt32(_quotationTemplateHeader[0]),
                    _quotationTemplateHeader[1]);
                Response.Redirect(
                    string.Format("{2}?{0}={1}", QuotationTemplateKey, _quotationTemplateHeader[0],
                        QuotationTemplateConsolePage), true);
            }

            if (e.CommandName == "Delete")
            {
                var _quotationTemplateHeader = e.CommandArgument.ToString().Split('|');
                QuotationHeader = new KeyValuePair<int, string>(Convert.ToInt32(_quotationTemplateHeader[0]),
                    _quotationTemplateHeader[1]);
                using (var _qc = new QuotationDataContext())
                {
                    var _toDeleteId = Convert.ToInt32(_quotationTemplateHeader[0]);
                    var _toDeleteQuotationTemplate =
                        _qc.QuotationTemplates.Single(QuotationTemplate => QuotationTemplate.ID == _toDeleteId);
                    _qc.QuotationTemplateDetails.DeleteAllOnSubmit(_toDeleteQuotationTemplate.QuotationTemplateDetails);
                    _qc.SubmitChanges();
                    _qc.QuotationTemplates.DeleteOnSubmit(_toDeleteQuotationTemplate);
                    _qc.SubmitChanges();
                    //grdDetails.DataBind();
                }
            }
        }

        protected void grdDetails_PreRender(object sender, EventArgs e)
        {
            //grdDetails.DataBind();
        }

        protected void grdDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var _row = e.Row;
                var _result = new QuotationService().GetLock(2, ((QuotationTemplate) _row.DataItem).ID,
                    WebUser.Member.ProviderUserKey.ToString(), SessionTimeStamp);

                switch (_result)
                {
                    case "|0|":
                        _row.ToolTip =
                            "Il record è in uso allo stesso utente nella stessa sessione di lavoro, accedere dalla pagina già attiva";
                        _row.ForeColor = Color.Blue;
                        var _lbtSelect0 = (LinkButton) e.Row.Cells[0].FindControl("lbtSelect");
                        _lbtSelect0.Visible = false;
                        var _lbtDelete0 = (LinkButton) e.Row.Cells[0].FindControl("lbtDelete");
                        _lbtDelete0.Visible = false;
                        break;
                    case "|1|":
                        _row.ToolTip =
                            "Il record è in uso allo stesso utente in un altra sessione di lavoro, accedere da quest'ultima";
                        _row.ForeColor = Color.DarkGreen;
                        var _lbtSelect1 = (LinkButton) e.Row.Cells[0].FindControl("lbtSelect");
                        _lbtSelect1.Visible = false;
                        var _lbtDelete1 = (LinkButton) e.Row.Cells[0].FindControl("lbtDelete");
                        _lbtDelete1.Visible = false;
                        break;
                    case "|2|":
                        break;
                    default:
                        _row.ToolTip = "Il record è in uso all'utente " +
                                       new MemberShipService().GetUser(_result).UserName;
                        _row.ForeColor = Color.Red;
                        var _lbtSelect2 = (LinkButton) e.Row.Cells[0].FindControl("lbtSelect");
                        _lbtSelect2.Visible = false;
                        var _lbtDelete2 = (LinkButton) e.Row.Cells[0].FindControl("lbtDelete");
                        _lbtDelete2.Visible = false;
                        break;
                }
            }
        }

       
        protected void GridDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            GridDataSource.OrderByParameters.Clear();
            GridDataSource.AutoGenerateOrderByClause = false;
            var table = GridDataSource.GetTable();
            var _qc = (QuotationDataContext)table.CreateContext();
            //e.Result = _qc.QuotationTemplates.OrderBy(qd => qd.Type.Order).ThenBy(qd => qd.ItemType.Order).ThenBy(qd => qd.Order);
            e.Result = _qc.QuotationTemplates.OrderBy(qd => qd.Order);
        }
    }
}