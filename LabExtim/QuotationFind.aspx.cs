using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class QuotationFind : QuotationController //System.Web.UI.Page
    {
        //protected MetaTable table;
        protected int insertedKey;

        public OrderedDictionary SelectionData
        {
            get { return Session["SelectionData"] as OrderedDictionary; }
            set { Session["SelectionData"] = value; }
        }

        public void InitSelectionData()
        {
            SelectionData = Session["SelectionData"] as OrderedDictionary;
            if (SelectionData == null)
            {
                SelectionData = new OrderedDictionary();
                Session["SelectionData"] = SelectionData;
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
            if (!IsPostBack)
            {
                var _result = new QuotationService().DelLocks(WebUser.Member.ProviderUserKey.ToString(), SessionTimeStamp);
                //table = DetailsDataSource.GetTable();
                //Title = table.DisplayName;
                FillControls();
                //FillDependingControls();
            }
            //FillDependingControls();
        }

        private void FillControls()
        {
            ddlOrderBy.Items.Add(new ListItem("Data", "Date DESC"));
            ddlOrderBy.Items.Add(new ListItem("Cliente, Descrizione", "Customer, Subject"));
            ddlOrderBy.Items.Add(new ListItem("Cliente, Data", "Customer, Date"));
            ddlOrderBy.Items.Add(new ListItem("Numero", "ID DESC"));
            ddlOrderBy.Items.Add(new ListItem("Utente", "Owner"));
            ddlOrderBy.DataBind();
        }

        //protected void FillDependingControls(DetailsViewInsertEventArgs e)
        //protected void FillDependingControls()
        //{

        //    //IEnumerable<Quotation> _currentQuotations =
        //    //    (IEnumerable<Quotation>)QuotationDataContext.Quotations.Where(Quotation =>
        //    //       e.Values["ID_Quotation"] != null ? Quotation.ID == Convert.ToInt32(e.Values["ID_Quotation"]) : Quotation.ID > 0 &&
        //    //       e.Values["CustomerCode"] != null ? Quotation.Customer.Code == e.Values["CustomerCode"].ToString() : Quotation.ID > 0 &&
        //    //       e.Values["Subject"] != null ? Quotation.Subject.Contains(e.Values["Subject"].ToString()) : Quotation.ID > 0 &&
        //    //       e.Values["StartDate"] != null ? Quotation.Date >= Convert.ToDateTime(e.Values["StartDate"]) : Quotation.ID > 0 &&
        //    //       e.Values["EndDate"] != null ? Quotation.Date <= Convert.ToDateTime(e.Values["EndDate"]) : Quotation.ID > 0);


        //    //IEnumerable<Quotation> _currentQuotations0 =
        //    //    (IEnumerable<Quotation>)QuotationDataContext.Quotations.Where(Quotation =>
        //    //       e.Values["ID_Quotation"] != null ? Quotation.ID == Convert.ToInt32(e.Values["ID_Quotation"]) : true);

        //    //IEnumerable<Quotation> _currentQuotations1 =
        //    //    (IEnumerable<Quotation>)_currentQuotations0.Where(Quotation =>
        //    //       e.Values["CustomerCode"] != null ? Quotation.Customer.Code == e.Values["CustomerCode"].ToString()  : true);

        //    //IEnumerable<Quotation> _currentQuotations2 =
        //    //    (IEnumerable<Quotation>)_currentQuotations1.Where(Quotation =>
        //    //       e.Values["Subject"] != null ? Quotation.Subject.Contains(e.Values["Subject"].ToString())  : true);

        //    //IEnumerable<Quotation> _currentQuotations3 =
        //    //    (IEnumerable<Quotation>)_currentQuotations2.Where(Quotation =>
        //    //       e.Values["StartDate"] != null ? Quotation.Date >= Convert.ToDateTime(e.Values["StartDate"]) : true);

        //    //IEnumerable<Quotation> _currentQuotations4 =
        //    //    (IEnumerable<Quotation>)_currentQuotations3.Where(Quotation =>
        //    //       e.Values["EndDate"] != null ? Quotation.Date >= Convert.ToDateTime(e.Values["EndDate"]) : true);

        //    //grdDetails.DataSource = _currentQuotations0;
        //    //grdDetails.DataBind();

        //    //string filterString = "0=0";
        //    //if (e.Values["ID_Quotation"] != null)
        //    //{
        //    //    filterString += " AND ID = @ID";
        //    //    GridDataSource.WhereParameters.Add("ID", DbType.Int32, e.Values["ID_Quotation"].ToString());
        //    //}
        //    //if (e.Values["CustomerCode"] != null)
        //    //{
        //    //    filterString += " AND CustomerCode = @CustomerCode";
        //    //    GridDataSource.WhereParameters.Add("CustomerCode", DbType.String, e.Values["CustomerCode"].ToString());
        //    //}
        //    //if (e.Values["Subject"] != null)
        //    //{
        //    //    filterString += " AND Subject LIKE @Subject";
        //    //    GridDataSource.WhereParameters.Add("Subject", DbType.String, e.Values["Subject"].ToString());
        //    //}
        //    //if (e.Values["StartDate"] != null)
        //    //{
        //    //    filterString += " AND Date >= @StartDate";
        //    //    GridDataSource.WhereParameters.Add("StartDate", DbType.DateTime, e.Values["StartDate"].ToString());
        //    //}
        //    //if (e.Values["EndDate"] != null)
        //    //{
        //    //    filterString += " AND Date <= @EndDate";
        //    //    GridDataSource.WhereParameters.Add("EndDate", DbType.DateTime, e.Values["EndDate"].ToString());
        //    //}

        //    //if (SelectionData.Count > 0)
        //    //{


        //    GridDataSource.WhereParameters.Clear();

        //    string filterString = "0=0";
        //    if (SelectionData["ID_Quotation"] != null)
        //    {
        //        filterString += " AND ID = @ID";
        //        GridDataSource.WhereParameters.Add("ID", DbType.Int32, SelectionData["ID_Quotation"].ToString());
        //    }
        //    if (SelectionData["CustomerCode"] != null)
        //    {
        //        filterString += " AND CustomerCode = @CustomerCode";
        //        GridDataSource.WhereParameters.Add("CustomerCode", DbType.Int32, SelectionData["CustomerCode"].ToString());
        //    }
        //    if (SelectionData["Subject"] != null)
        //    {
        //        filterString += " AND Subject.Contains(@Subject)";
        //        GridDataSource.WhereParameters.Add("Subject", DbType.String, SelectionData["Subject"].ToString());
        //    }
        //    if (SelectionData["StartDate"] != null)
        //    {
        //        filterString += " AND Date >= @StartDate";
        //        GridDataSource.WhereParameters.Add("StartDate", DbType.DateTime, SelectionData["StartDate"].ToString());
        //    }
        //    if (SelectionData["EndDate"] != null)
        //    {
        //        filterString += " AND Date <= @EndDate";
        //        GridDataSource.WhereParameters.Add("EndDate", DbType.DateTime, SelectionData["EndDate"].ToString());
        //    }
        //    if (SelectionData["ID_Owner"] != null)
        //    {
        //        filterString += " AND ID_Owner = @ID_Owner";
        //        GridDataSource.WhereParameters.Add("ID_Owner", DbType.Int32, SelectionData["ID_Owner"].ToString());
        //    }
        //    if (rdlQuotationType.SelectedValue == "Bozze")
        //    {
        //        filterString += " AND Draft = @Draft";
        //        GridDataSource.WhereParameters.Add("Draft", DbType.Boolean, true.ToString());
        //    }
        //    else if (rdlQuotationType.SelectedValue == "Preventivi")
        //    {
        //        //filterString += " AND (Draft = @Draft OR Draft == NULL)";
        //        filterString += " AND (Draft = @Draft)";
        //        GridDataSource.WhereParameters.Add("Draft", DbType.Boolean, false.ToString());
        //    }

        //    GridDataSource.Where = filterString;
        //    GridDataSource.AutoGenerateWhereClause = false;
        //    grdDetails.DataBind();
        //    //}

        //}


        protected void ddlOrderBy_DataBound(object sender, EventArgs e)
        {
            if (Session["QuotationsOrderBySelector"] != null)
            {
                ddlOrderBy.Items.FindByValue(Session["QuotationsOrderBySelector"].ToString()).Selected = true;
            }
        }

        protected void PersistSelection(object sender, EventArgs e)
        {
            Session["QuotationsOrderBySelector"] = ddlOrderBy.SelectedValue;
        }

        protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            if (e.CommandName == DataControlCommands.CancelCommandName)
            {
                SelectionData.Clear();
                //Response.Redirect(HomePage);
            }
        }

        protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            if (e.Exception == null || e.ExceptionHandled)
            {
                //QuotationHeader = new KeyValuePair<int,string>(insertedKey,e.Values["Name"].ToString());
                Response.Redirect(
                    string.Format("{2}?{0}={1}", QuotationKey, insertedKey.ToString().TrimEnd(), QuotationConsolePage),
                    true);
            }

            OnFilterSelectedIndexChanged(null, null);
        }

        //protected void DetailsDataSource_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        //{
        //    insertedKey = ((DLLabExtim.Quotation)e.Result).ID;
        //}


        protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            e.Cancel = true;
            //FillDependingControls(e);
            SelectionData = (OrderedDictionary)e.Values;
            SelectionData.Add("QuotationType", rdlQuotationType.SelectedValue);
            if (!string.IsNullOrEmpty(((HiddenField)DetailsView1.Rows[0].FindControl("hidSearchCli")).Value))
            {
                SelectionData.Add("CustomerName", ((TextBox)DetailsView1.Rows[0].FindControl("txtSearchCli")).Text);
                SelectionData.Add("CustomerCode", ((HiddenField)DetailsView1.Rows[0].FindControl("hidSearchCli")).Value);
            }

            //FillDependingControls();
            grdDetails.DataBind();
        }

        protected void grdDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdDetails.PageIndex = e.NewPageIndex;
            grdDetails.DataBind();
        }

        protected void OnFilterSelectedIndexChanged(object sender, EventArgs e)
        {
            grdDetails.PageIndex = 0;
        }

        protected void grdDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                var _quotationHeader = e.CommandArgument.ToString().Split('|');
                QuotationHeader = new KeyValuePair<int, string>(Convert.ToInt32(_quotationHeader[0]),
                    _quotationHeader[1]);

                //char? _result = new QuotationService().SetLock(1, Convert.ToInt32(_quotationHeader[0]), WebUser.ProviderUserKey.ToString(), Session.SessionID);

                Response.Redirect(
                    string.Format("{2}?{0}={1}", QuotationKey, _quotationHeader[0], QuotationConsolePage), true);
            }

            if (e.CommandName == "SelectTemp")
            {
                var _quotationHeader = e.CommandArgument.ToString().Split('|');
                QuotationHeader = new KeyValuePair<int, string>(Convert.ToInt32(_quotationHeader[0]),
                    _quotationHeader[1]);

                //char? _result = new QuotationService().SetLock(1, Convert.ToInt32(_quotationHeader[0]), WebUser.ProviderUserKey.ToString(), Session.SessionID);
                LoadPersistedQuotation = true;
                Response.Redirect(
                    string.Format("{2}?{0}={1}", QuotationKey, _quotationHeader[0], TempQuotationConsolePage), true);
            }


            if (e.CommandName == "Delete")
            {
                var _quotationHeader = e.CommandArgument.ToString().Split('|');
                QuotationHeader = new KeyValuePair<int, string>(Convert.ToInt32(_quotationHeader[0]),
                    _quotationHeader[1]);
                using (var _qc = new QuotationDataContext())
                {
                    var _toDeleteId = Convert.ToInt32(_quotationHeader[0]);

                    var _toDeleteTempQuotation =
                        _qc.TempQuotations.Single(Quotation => Quotation.ID_Quotation == _toDeleteId && Quotation.ID_Owner == GetCurrentEmployee().ID);
                    _qc.TempQuotationDetails.DeleteAllOnSubmit(_toDeleteTempQuotation.TempQuotationDetails);
                    _qc.SubmitChanges();
                    _qc.TempQuotations.DeleteOnSubmit(_toDeleteTempQuotation);
                    _qc.SubmitChanges();

                    if (_toDeleteId > 0)
                    {
                        var _toDeleteQuotation = _qc.Quotations.Single(Quotation => Quotation.ID == _toDeleteId);
                        _qc.QuotationDetails.DeleteAllOnSubmit(_toDeleteQuotation.QuotationDetails);
                        _qc.SubmitChanges();
                        _qc.Quotations.DeleteOnSubmit(_toDeleteQuotation);
                    }

                    _qc.SubmitChanges();
                    //grdDetails.DataBind();
                }
            }
        }

        protected void grdDetails_PreRender(object sender, EventArgs e)
        {
            grdDetails.DataBind();
        }

        protected void grdDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var _row = e.Row;
                var _result = new QuotationService().GetLock(1, ((VW_AllQuotations)_row.DataItem).ID,
                    WebUser.Member.ProviderUserKey.ToString(), SessionTimeStamp);

                try
                {
                    switch (_result)
                    {
                        case "|0|":
                            _row.ToolTip =
                                "Il record è in uso allo stesso utente nella stessa sessione di lavoro, si consiglia di accedere dalla pagina già attiva";
                            _row.ForeColor = Color.Blue;
                            //LinkButton _lbtSelect0 = (LinkButton)e.Row.Cells[0].FindControl("lbtSelect");
                            //_lbtSelect0.Visible = false;
                            //var _lbtSelectTemp0 = (LinkButton)e.Row.Cells[0].FindControl("lbtSelectTemp");
                            //_lbtSelectTemp0.Visible = false;
                            //var _lbtDelete0 = (LinkButton)e.Row.Cells[0].FindControl("lbtDelete");
                            //_lbtDelete0.Visible = false;
                            break;
                        case "|1|":
                            _row.ToolTip =
                                "Il record è in uso allo stesso utente in un altra sessione di lavoro, si consiglia di accedere da quest'ultima";
                            _row.ForeColor = Color.DarkGreen;
                            //LinkButton _lbtSelect1 = (LinkButton)e.Row.Cells[0].FindControl("lbtSelect");
                            //_lbtSelect1.Visible = false;
                            //var _lbtSelectTemp1 = (LinkButton)e.Row.Cells[0].FindControl("lbtSelectTemp");
                            //_lbtSelectTemp1.Visible = false;
                            //var _lbtDelete1 = (LinkButton)e.Row.Cells[0].FindControl("lbtDelete");
                            //_lbtDelete1.Visible = false;
                            break;
                        case "|2|":
                            break;
                        default:
                            _row.ToolTip = "Il record è in uso all'utente " +
                                           new MemberShipService().GetUser(_result).UserName;
                            _row.ForeColor = Color.Red;
                            //LinkButton _lbtSelect2 = (LinkButton)e.Row.Cells[0].FindControl("lbtSelect");
                            //_lbtSelect2.Visible = false;
                            var _lbtSelectTemp2 = (LinkButton)e.Row.Cells[0].FindControl("lbtSelectTemp");
                            _lbtSelectTemp2.Visible = false;
                            var _lbtDelete2 = (LinkButton)e.Row.Cells[0].FindControl("lbtDelete");
                            _lbtDelete2.Visible = false;
                            break;
                    }
                }
                catch
                {
                }
            }
        }

        protected void GridDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            //IQueryable<Quotation> table = GridDataSource.GetTable().GetQuery().Cast<Quotation>();

            //if (SelectionData["ID_ProductionOrder"] != null)
            //{
            //    ProductionOrder _toFind =
            //        new QuotationDataContext().ProductionOrders.SingleOrDefault(po => po.ID == Convert.ToInt32(SelectionData["ID_ProductionOrder"]));
            //    if (_toFind != null)
            //        table = table.Where(q => q.ID == _toFind.ID_Quotation);
            //    else
            //        table = table.Where(q => false);

            //}

            ////QuotationDataContext _qc = (QuotationDataContext)table.CreateContext();
            //switch (ddlOrderBy.SelectedValue)
            //{
            //    case ("Customer, Subject"):
            //        GridDataSource.OrderByParameters.Clear();
            //        GridDataSource.AutoGenerateOrderByClause = false;
            //        e.Result = table.OrderBy(qt => qt.Customer.Name).ThenBy(qt => qt.Subject);
            //        break;
            //    case ("Customer, Date"):
            //        GridDataSource.OrderByParameters.Clear();
            //        GridDataSource.AutoGenerateOrderByClause = false;
            //        e.Result = table.OrderBy(qt => qt.Customer.Name).ThenByDescending(qt => qt.Date);
            //        break;
            //    case ("Owner"):
            //        GridDataSource.OrderByParameters.Clear();
            //        GridDataSource.AutoGenerateOrderByClause = false;
            //        e.Result = table.OrderBy(qt => qt.Employee.Name).ThenByDescending(qt => qt.Date);
            //        break;
            //    default:
            //        break;

            //}
            e.Result = GetQuotationsFiltered(new QuotationDataContext().VW_AllQuotations, SelectionData,
                rdlQuotationType, ddlOrderBy);
        }

        protected void rdlQuotationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdDetails.PageIndex = 0;
            //FillDependingControls();
            grdDetails.DataBind();
            SelectionData["QuotationType"] = rdlQuotationType.SelectedValue;
        }

        protected IQueryable<VW_AllQuotations> GetQuotationsFiltered(IQueryable<VW_AllQuotations> data,
            OrderedDictionary selectionData, RadioButtonList rdlQuotationType, DropDownList ddlOrderBy)
        {
            //merge aziendale
            //data = data.Where(d => d.ID_Company == CurrentCompanyId);

            if (selectionData["ID_Quotation"] != null)
            {
                data = data.Where(d => d.ID == Convert.ToInt32(selectionData["ID_Quotation"]));
            }
            if (selectionData["Number"] != null)
            {
                string number = selectionData["Number"].ToString();
                if (number.StartsWith("/"))
                {
                    data = data.Where(d => d.Number.Contains(number.Substring(number.IndexOf('/') + 1)));
                }
                else if (number.Length > 4)
                {
                    data = data.Where(d => d.Number.Substring(2) == number);
                }
                else
                {
                    data = data.Where(d => d.Number.Substring(2, 2) == number.Substring(2));
                }
            }
            if (selectionData["CustomerCode"] != null)
            {
                data = data.Where(d => d.CustomerCode == Convert.ToInt32(selectionData["CustomerCode"]));
                selectionData["CustomerName"] = new QuotationDataContext().Customers.FirstOrDefault(c => c.Code == Convert.ToInt32(selectionData["CustomerCode"])).Name;
            }
            if (selectionData["Subject"] != null)
            {
                data = data.Where(d => d.Subject.Contains(selectionData["Subject"].ToString()));
            }
            if (selectionData["StartDate"] != null)
            {
                data = data.Where(d => d.Date >= Convert.ToDateTime(selectionData["StartDate"]));
            }
            if (selectionData["EndDate"] != null)
            {
                data = data.Where(d => d.Date <= Convert.ToDateTime(selectionData["EndDate"]));
            }
            if (selectionData["ID_Manager"] != null)
            {
                data = data.Where(d => d.ID_Manager == Convert.ToInt32(selectionData["ID_Manager"]));
            }
            if (selectionData["ID_Owner"] != null)
            {
                data = data.Where(d => d.ID_Owner == Convert.ToInt32(selectionData["ID_Owner"]));
            }
            if (selectionData["ID_ProductionOrder"] != null)
            {
                data = data.Where(d => d.ProductionOrder.Any(p => p.ID == Convert.ToInt32(selectionData["ID_ProductionOrder"])));
            }
            if (selectionData["Number_ProductionOrder"] != null)
            {
                string number = selectionData["Number_ProductionOrder"].ToString();
                if (number.StartsWith("/"))
                {
                    data = data.Where(d => d.ProductionOrder.Any(p => p.Number.Contains(number.Substring(number.IndexOf('/') + 1))));
                }
                else if (number.Length > 4)
                {
                    data = data.Where(d => d.ProductionOrder.Any(p => p.Number.Substring(2) == number));
                }
                else
                {
                    data = data.Where(d => d.ProductionOrder.Any(p => p.Number.Substring(2, 2) == number.Substring(2)));
                }
            }
            if (rdlQuotationType.SelectedValue == "Bozze")
            {
                data = data.Where(d => d.Draft == true);
            }
            else if (rdlQuotationType.SelectedValue == "Preventivi")
            {
                data = data.Where(d => d.Draft == false);
            }

            switch (ddlOrderBy.SelectedValue)
            {
                case ("Customer, Subject"):
                    data = data.OrderBy(qt => qt.Customer.Name).ThenBy(qt => qt.Subject);
                    break;
                case ("Date DESC"):
                    data = data.OrderByDescending(qt => qt.Date);
                    break;
                case ("Customer, Date"):
                    data = data.OrderBy(qt => qt.Customer.Name).ThenByDescending(qt => qt.Date);
                    break;
                case ("Owner"):
                    data =
                        data.OrderBy(qt => qt.Employee1.Surname)
                            .ThenBy(qt => qt.Employee1.Name)
                            .ThenByDescending(qt => qt.Date);
                    break;
                default:
                    break;
            }

            return data;
        }

        protected void DetailsView1_DataBound(object sender, EventArgs e)
        {
            try
            {
                if (SelectionData != null)
                {
                    if (SelectionData["ID_Quotation"] != null)
                    {
                        ((TextBox)DetailsView1.Rows[0].FindControl("dycPicker_ID").Controls[0].Controls[0]).Text =
                            SelectionData["ID_Quotation"].ToString();
                    }
                    if (SelectionData["CustomerCode"] != null)
                    {
                        //((LabExtim.Autocomplete_EditField)DetailsView1.Rows[0].FindControl("dycPicker_CustomerCode").Controls[0])
                        //    .AutoCompleteValuePublic.Value = SelectionData["CustomerCode"].ToString();
                        //((LabExtim.Autocomplete_EditField)DetailsView1.Rows[0].FindControl("dycPicker_CustomerCode").Controls[0])
                        //    .AutoCompleteTextPublic.Text = SelectionData["CustomerName"].ToString();
                        ((TextBox)DetailsView1.Rows[0].FindControl("txtSearchCli")).Text =
                            SelectionData["CustomerName"].ToString();
                        ((HiddenField)DetailsView1.Rows[0].FindControl("hidSearchCli")).Value =
                            SelectionData["CustomerCode"].ToString();
                    }
                    if (SelectionData["Subject"] != null)
                    {
                        ((TextBox)DetailsView1.Rows[0].FindControl("dycPicker_Subject").Controls[0].Controls[0]).Text =
                            SelectionData["Subject"].ToString();
                    }
                    if (SelectionData["StartDate"] != null)
                    {
                        ((TextBox)DetailsView1.Rows[0].FindControl("dycPicker_StartDate").Controls[0].Controls[0]).Text
                            = SelectionData["StartDate"].ToString();
                    }
                    if (SelectionData["EndDate"] != null)
                    {
                        ((TextBox)DetailsView1.Rows[0].FindControl("dycPicker_EndDate").Controls[0].Controls[0]).Text =
                            SelectionData["EndDate"].ToString();
                    }
                    if (SelectionData["ID_Manager"] != null)
                    {
                        ((DropDownList)DetailsView1.Rows[0].FindControl("dycPicker_Manager").Controls[0].Controls[0])
                            .SelectedValue = SelectionData["ID_Manager"].ToString();
                    }
                    if (SelectionData["ID_Owner"] != null)
                    {
                        ((DropDownList)DetailsView1.Rows[0].FindControl("dycPicker_Owner").Controls[0].Controls[0])
                            .SelectedValue = SelectionData["ID_Owner"].ToString();
                    }
                    if (SelectionData["ID_ProductionOrder"] != null)
                    {
                        ((TextBox)
                            DetailsView1.Rows[0].FindControl("dycPicker_ID_ProductionOrder").Controls[0].Controls[0])
                            .Text = SelectionData["ID_ProductionOrder"].ToString();
                    }
                    if (SelectionData["QuotationType"] != null)
                    {
                        rdlQuotationType.SelectedValue = SelectionData["QuotationType"].ToString();
                    }
                }
            }
            catch
            {
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
                        return new JavaScriptSerializer().Serialize(ctx.Customers.Where(c => !c.Name.StartsWith("**") && c.Name.Contains(q.Replace("%27", "'")) && c.Code >= 1 && c.Code <= 199999999).Select(c => new { Code = c.Code, Name = c.Name }).ToList());
                    else if (System.Web.HttpContext.Current.Session["CurrentCompanyId"].ToString() == "2")
                        return new JavaScriptSerializer().Serialize(ctx.Customers.Where(c => !c.Name.StartsWith("**") && c.Name.Contains(q.Replace("%27", "'")) && c.Code >= 200000000 && c.Code <= 299999999).Select(c => new { Code = c.Code, Name = c.Name }).ToList());
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