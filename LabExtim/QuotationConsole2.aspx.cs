using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.DynamicData;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using CMLabExtim.CustomClasses;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class QuotationConsole2 : TempQuotationController
    {
        protected List<SPQDetailTotal> m_toolsTotals;
        protected List<SPQDetailTotal> m_totals;
        protected List<QuotationTotal> m_totalsPivot;
        protected short m_voicePosition;

        protected Mode CurQuotationConsoleMode
        {
            get
            {
                if (Session["CurQuotationConsoleMode"] == null)
                {
                    return Mode.InputItems;
                }
                return (Mode)Session["CurQuotationConsoleMode"];
            }
            set { Session["CurQuotationConsoleMode"] = value; }
        }

        protected int? GetQuotationCustomerID()
        {
            return new QuotationDataContext().Quotations.SingleOrDefault(q => q.ID ==
                                                                              Convert.ToInt32(
                                                                                  dtvQuotation.DataKey.Values[0]
                                                                                      .ToString())).CustomerCode;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(dtvQuotation);
            DynamicDataManager1.RegisterControl(grdQuotationDetails);
            m_voicePosition = 1000;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultButton = btnRecalc.UniqueID;
            var _result = new QuotationService().SetLock(1, Convert.ToInt32(QuotationParameter),
                WebUser.Member.ProviderUserKey.ToString(), SessionTimeStamp);

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["hm"]))
                {
                    Site master = Page.Master as Site;
                    master.MenuBarVisible = false;
                    lbtNewQuotation.Visible = false;
                }

                if (Convert.ToInt32(QuotationParameter) > 0)
                {
                    if (LoadPersistedQuotation)
                    {
                        ActivateSavedQuotation(Convert.ToInt32(QuotationParameter));
                        LoadPersistedQuotation = false;
                    }
                }
                LoadHeaderMenu(mnuOperations1, MenuType.MenuOperations, 1);
                LoadHeaderMenu(mnuOperations2, MenuType.MenuOperations, 2);
                LoadHeaderMenu(mnuQuotationTemplates, MenuType.MenuQuotationTemplates, -1);
                FillControls();
            }
            FillDependingControls(CurQuotationConsoleMode);
            SetAsReadOnly(UsageParameter);
        }

        private void WriteQuantities(QuotationDataContext qc)
        {
            for (var i = 0; i < grdQuotationDetails.Rows.Count; i++)
            {
                var _txtQuantity = (TextBox)grdQuotationDetails.Rows[i].Cells[8 + 2].FindControl("txtQuantity");
                var _litID = (Literal)grdQuotationDetails.Rows[i].Cells[1].FindFieldTemplate("ID").Controls[0];
                //Literal _litID = (Literal)grdQuotationDetails.Rows[i].Cells[2].FindFieldTemplate("ID_QuotationDetail").Controls[0];
                var _toModifyId = Convert.ToInt32(_litID.Text);
                var _toUpdateQuotationDetail = qc.TempQuotationDetails.Single(
                    QuotationDetail =>
                        QuotationDetail.ID == _toModifyId
                        //QuotationDetail.ID_QuotationDetail == _toModifyId
                        && QuotationDetail.ID_Quotation == Convert.ToInt32(QuotationParameter)
                        && QuotationDetail.SessionUser == GetCurrentEmployee().ID);
                var _quantity = 0f;
                _txtQuantity.Text = _txtQuantity.Text.Replace('.', ',');
                if (_txtQuantity.Text.Contains('/'))
                {
                    var _digits = _txtQuantity.Text.Split('/');
                    var _digitToDivide = 0f;
                    float.TryParse(_digits[0], out _digitToDivide);
                    for (var j = 1; j < _digits.Length; j++)
                    {
                        var _digitDivisor = 0f;
                        float.TryParse(_digits[j], out _digitDivisor);
                        _digitToDivide = _digitToDivide / _digitDivisor;
                    }
                    _quantity = _digitToDivide;
                }
                else
                {
                    float.TryParse(_txtQuantity.Text, out _quantity);
                }
                _toUpdateQuotationDetail.Quantity = _quantity;
            }
        }

        private void WriteQuotationHeaderData(QuotationDataContext qc)
        {
            if (dtvQuotation.CurrentMode == DetailsViewMode.ReadOnly)
            {
                var _toUpdateQuotation =
                    qc.TempQuotations.Single(
                        Quotation =>
                            Quotation.ID_Quotation == Convert.ToInt32(QuotationParameter) &&
                            Quotation.SessionUser == GetCurrentEmployee().ID);


                var _ddlManagers = (DropDownList)dtvQuotation.FindControl("ddlManagers");
                var _IdManager = 1;
                int.TryParse(_ddlManagers.SelectedValue, out _IdManager);
                _toUpdateQuotation.ID_Manager = _IdManager;

                var _txtQuantity1 = (TextBox)dtvQuotation.FindControl("txtQ1");
                var _quantity1 = 0;
                int.TryParse(_txtQuantity1.Text, out _quantity1);
                _toUpdateQuotation.Q1 = _quantity1;

                var _txtQuantity2 = (TextBox)dtvQuotation.FindControl("txtQ2");
                var _quantity2 = 0;
                int.TryParse(_txtQuantity2.Text, out _quantity2);
                _toUpdateQuotation.Q2 = _quantity2;

                var _txtQuantity3 = (TextBox)dtvQuotation.FindControl("txtQ3");
                var _quantity3 = 0;
                int.TryParse(_txtQuantity3.Text, out _quantity3);
                _toUpdateQuotation.Q3 = _quantity3;

                var _txtQuantity4 = (TextBox)dtvQuotation.FindControl("txtQ4");
                var _quantity4 = 0;
                int.TryParse(_txtQuantity4.Text, out _quantity4);
                _toUpdateQuotation.Q4 = _quantity4;

                var _txtQuantity5 = (TextBox)dtvQuotation.FindControl("txtQ5");
                var _quantity5 = 0;
                int.TryParse(_txtQuantity5.Text, out _quantity5);
                _toUpdateQuotation.Q5 = _quantity5;

                bool isAuto = (_quantity3 == -1 && _quantity4 == -1 && _quantity5 == -1);
                if (!isAuto)
                {
                    try
                    {
                        int startPos = _toUpdateQuotation.Subject.IndexOf(" (Automatico da OdP");
                        if (startPos > -1)
                        {
                            _toUpdateQuotation.Subject = _toUpdateQuotation.Subject.Substring(0, startPos);
                        }
                    }
                    catch
                    {
                    }
                }


                var _chkPrint1 = (CheckBox)dtvQuotation.FindControl("chkP1");
                _toUpdateQuotation.P1 = _chkPrint1.Checked;

                var _chkPrint2 = (CheckBox)dtvQuotation.FindControl("chkP2");
                _toUpdateQuotation.P2 = _chkPrint2.Checked;

                var _chkPrint3 = (CheckBox)dtvQuotation.FindControl("chkP3");
                _toUpdateQuotation.P3 = _chkPrint3.Checked;

                var _chkPrint4 = (CheckBox)dtvQuotation.FindControl("chkP4");
                _toUpdateQuotation.P4 = _chkPrint4.Checked;

                var _chkPrint5 = (CheckBox)dtvQuotation.FindControl("chkP5");
                _toUpdateQuotation.P5 = _chkPrint5.Checked;


                var _txtMarkUp = (TextBox)dtvQuotation.FindControl("txtMarkUp");
                var _markUp = 0;
                int.TryParse(_txtMarkUp.Text, out _markUp);
                _toUpdateQuotation.MarkUp = _markUp;

                _toUpdateQuotation.Note = txtDescription.Text;
                _toUpdateQuotation.PriceCom = txtPriceCom.Text;
                _toUpdateQuotation.PrintingMainText = txtPrintingMainText.Text;

                _toUpdateQuotation.Note1 = txtAppunti1.Text;

                var hidSearchCli = (HiddenField)dtvQuotation.FindControl("hidSearchCli");
                int _code = 0;
                int.TryParse(hidSearchCli.Value, out _code);
                if (_code != 0)
                {
                    _toUpdateQuotation.CustomerCode = _code;
                    ProductionOrder found = qc.ProductionOrders.FirstOrDefault(po => po.ID_Quotation == _toUpdateQuotation.ID_Quotation);
                    if (found != null)
                    {
                        found.ID_Customer = _code;
                    }

                }

                foreach (var _quotationDetail in _toUpdateQuotation.TempQuotationDetails)
                {
                    _quotationDetail.MarkUp = _markUp;
                    _quotationDetail.Price =
                        Convert.ToDecimal(_quotationDetail.Cost * (_quotationDetail.Percentage / 100m) * (_markUp / 100m));
                }
            }
        }

        private void FillControls()
        {
            using (var QuotationDataContext =
                new QuotationDataContext())
            {
                //IEnumerable<QuotationTemplate> _quotationTemplates =
                //    QuotationDataContext.QuotationTemplates;
                //ddlTemplates.DataSource = _quotationTemplates;
                //ddlTemplates.DataTextField = "Description";
                //ddlTemplates.DataValueField = "ID";
                //ddlTemplates.DataBind();

                IEnumerable<DLLabExtim.Type> _templateTypes =
                    QuotationDataContext.Types.Where(t => t.Category == "T").OrderBy(t => t.Order);
                ddlTypes.DataSource = _templateTypes;
                ddlTypes.DataTextField = "Description";
                ddlTypes.DataValueField = "Code";
                ddlTypes.DataBind();

                IEnumerable<ItemType> _templateItemTypes =
                    QuotationDataContext.ItemTypes.Where(t => t.Category == "T").OrderBy(t => t.Order);
                ddlItemTypes.DataSource = _templateItemTypes;
                ddlItemTypes.DataTextField = "Description";
                ddlItemTypes.DataValueField = "Code";
                ddlItemTypes.DataBind();
            }
            hypListProductionOrders.Attributes.Add("onclick",
                "javascript:OpenBigItem('ProductionOrdersList.aspx?" + "POquo=" + QuotationParameter + "');return false;");
            hypListProductionOrders.Attributes["onmouseover"] =
                "this.style.cursor='hand';this.style.textDecoration='underline';";
            hypListProductionOrders.Attributes["onmouseout"] = "this.style.textDecoration='none';";
        }

        protected void FillDependingControls(Mode mode)
        {
            var filterString = "ID_Quotation = @ID && SessionUser = @SessionUser";

            DetailsDataSource.WhereParameters.Clear();
            DetailsDataSource.WhereParameters.Add("ID", DbType.Int32, QuotationParameter);
            DetailsDataSource.WhereParameters.Add("SessionUser", DbType.Int32, GetCurrentEmployee().ID.ToString());
            DetailsDataSource.Where = filterString;
            DetailsDataSource.AutoGenerateWhereClause = false;

            GridDataSource.WhereParameters.Clear();
            GridDataSource.WhereParameters.Add("ID", DbType.Int32, QuotationParameter);
            GridDataSource.WhereParameters.Add("SessionUser", DbType.Int32, GetCurrentEmployee().ID.ToString());
            GridDataSource.Where = filterString;
            GridDataSource.AutoGenerateWhereClause = false;

            using (var _qc = new QuotationDataContext())
            {
                WriteQuantities(_qc);
                WriteQuotationHeaderData(_qc);
                _qc.SubmitChanges();

                m_totals =
                    _qc.prc_LAB_MGet_LAB_TotalsByQuotationID(Convert.ToInt32(QuotationParameter))
                        .ToList();
                m_toolsTotals =
                    _qc.prc_LAB_MGet_LAB_ToolsTotalsByQuotationID(Convert.ToInt32(QuotationParameter))
                        .ToList();
                UpdatePivot(m_totals, m_toolsTotals);

                var _currentQuotation =
                    _qc.TempQuotations.SingleOrDefault(
                        Quotation =>
                            Quotation.ID_Quotation == Convert.ToInt32(QuotationParameter) &&
                            Quotation.SessionUser == GetCurrentEmployee().ID);
                foreach (var _quotationDetail in _currentQuotation.TempQuotationDetails)
                {
                    if (_quotationDetail.PickingItem != null || _quotationDetail.MacroItem != null)
                    {
                        if (_quotationDetail.Save == false)
                        {
                            if (_quotationDetail.Inserted == false || _quotationDetail.PickingItem.Inserted == false)
                            {
                                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.ObsoleteItem);
                                break;
                            }
                        }
                        else
                        {
                            if (_quotationDetail.Inserted == false || _quotationDetail.MacroItem.Inserted == false)
                            {
                                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.ObsoleteItem);
                                break;
                            }
                        }
                    }
                }

                bool isAuto = (_currentQuotation.Q3 == -1 && _currentQuotation.Q4 == -1 && _currentQuotation.Q5 == -1);
                lbtCreateProductionOrder.Visible = !isAuto;
                lbtPrintQuotation.Enabled = !isAuto;
                lbtPrintQuotationTechnical.Enabled = !isAuto;
                lbtSave.Enabled = !isAuto;
                lbtSaveAsTemplate.Enabled = !isAuto;
                btnEmpty.Enabled = !isAuto;

                if (Convert.ToInt32(QuotationParameter) < 0)
                {
                    lbtSave.Enabled = false;
                    lbtPrintQuotation.Enabled = false;
                    lbtPrintQuotationTechnical.Enabled = false;
                }

                var _canLaunchOdP = !(
                    _currentQuotation.Draft == true ||
                    _qc.ProductionOrders.Where(po => po.ID_Quotation == _currentQuotation.ID_Quotation && po.ID_Company == CurrentCompanyId).Count() > 0 ||
                    (Convert.ToInt32(QuotationParameter) < 0));
                if (_canLaunchOdP)
                {
                    _canLaunchOdP = IsSyncronized(QuotationParameter, GetCurrentEmployee().ID);
                }
                lbtCreateProductionOrder.Visible = _canLaunchOdP;
            }

            SwitchDependingControls(mode);
        }

        protected void UpdatePivot(List<SPQDetailTotal> inputs, List<SPQDetailTotal> toolsInputs)
        {
            m_totalsPivot = new List<QuotationTotal>();
            for (var i = 0; i < 5; i++)
            {
                var _total = new QuotationTotal();

                switch (i)
                {
                    case 0:
                        _total.Quantity = m_totals[0].Q1;
                        _total.CstUni = Convert.ToDecimal(m_totals[0].CstUni1);
                        _total.CstPUni = Convert.ToDecimal(m_totals[0].CstUniP1);
                        _total.PrcUni = Convert.ToDecimal(m_totals[0].PrcUni1);
                        _total.CstTot = Convert.ToDecimal(m_totals[0].CstTot1);
                        _total.CstPTot = Convert.ToDecimal(m_totals[0].CstTotP1);
                        _total.PrcTot = Convert.ToDecimal(m_totals[0].PrcTot1);

                        _total.CstToolsUni = Convert.ToDecimal(m_toolsTotals[0].CstUni1);
                        _total.PrcToolsUni = Convert.ToDecimal(m_toolsTotals[0].PrcUni1);
                        _total.CstToolsTot = Convert.ToDecimal(m_toolsTotals[0].CstTot1);
                        _total.PrcToolsTot = Convert.ToDecimal(m_toolsTotals[0].PrcTot1);

                        break;
                    case 1:
                        _total.Quantity = m_totals[0].Q2;
                        _total.CstUni = Convert.ToDecimal(m_totals[0].CstUni2);
                        _total.CstPUni = Convert.ToDecimal(m_totals[0].CstUniP2);
                        _total.PrcUni = Convert.ToDecimal(m_totals[0].PrcUni2);
                        _total.CstTot = Convert.ToDecimal(m_totals[0].CstTot2);
                        _total.CstPTot = Convert.ToDecimal(m_totals[0].CstTotP2);
                        _total.PrcTot = Convert.ToDecimal(m_totals[0].PrcTot2);

                        _total.CstToolsUni = Convert.ToDecimal(m_toolsTotals[0].CstUni2);
                        _total.PrcToolsUni = Convert.ToDecimal(m_toolsTotals[0].PrcUni2);
                        _total.CstToolsTot = Convert.ToDecimal(m_toolsTotals[0].CstTot2);
                        _total.PrcToolsTot = Convert.ToDecimal(m_toolsTotals[0].PrcTot2);

                        break;
                    case 2:
                        _total.Quantity = m_totals[0].Q3;
                        _total.CstUni = Convert.ToDecimal(m_totals[0].CstUni3);
                        _total.CstPUni = Convert.ToDecimal(m_totals[0].CstUniP3);
                        _total.PrcUni = Convert.ToDecimal(m_totals[0].PrcUni3);
                        _total.CstTot = Convert.ToDecimal(m_totals[0].CstTot3);
                        _total.CstPTot = Convert.ToDecimal(m_totals[0].CstTotP3);
                        _total.PrcTot = Convert.ToDecimal(m_totals[0].PrcTot3);

                        _total.CstToolsUni = Convert.ToDecimal(m_toolsTotals[0].CstUni3);
                        _total.PrcToolsUni = Convert.ToDecimal(m_toolsTotals[0].PrcUni3);
                        _total.CstToolsTot = Convert.ToDecimal(m_toolsTotals[0].CstTot3);
                        _total.PrcToolsTot = Convert.ToDecimal(m_toolsTotals[0].PrcTot3);

                        break;
                    case 3:
                        _total.Quantity = m_totals[0].Q4;
                        _total.CstUni = Convert.ToDecimal(m_totals[0].CstUni4);
                        _total.CstPUni = Convert.ToDecimal(m_totals[0].CstUniP4);
                        _total.PrcUni = Convert.ToDecimal(m_totals[0].PrcUni4);
                        _total.CstTot = Convert.ToDecimal(m_totals[0].CstTot4);
                        _total.CstPTot = Convert.ToDecimal(m_totals[0].CstTotP4);
                        _total.PrcTot = Convert.ToDecimal(m_totals[0].PrcTot4);

                        _total.CstToolsUni = Convert.ToDecimal(m_toolsTotals[0].CstUni4);
                        _total.PrcToolsUni = Convert.ToDecimal(m_toolsTotals[0].PrcUni4);
                        _total.CstToolsTot = Convert.ToDecimal(m_toolsTotals[0].CstTot4);
                        _total.PrcToolsTot = Convert.ToDecimal(m_toolsTotals[0].PrcTot4);

                        break;
                    case 4:
                        _total.Quantity = m_totals[0].Q5;
                        _total.CstUni = Convert.ToDecimal(m_totals[0].CstUni5);
                        _total.CstPUni = Convert.ToDecimal(m_totals[0].CstUniP5);
                        _total.PrcUni = Convert.ToDecimal(m_totals[0].PrcUni5);
                        _total.CstTot = Convert.ToDecimal(m_totals[0].CstTot5);
                        _total.CstPTot = Convert.ToDecimal(m_totals[0].CstTotP5);
                        _total.PrcTot = Convert.ToDecimal(m_totals[0].PrcTot5);

                        _total.CstToolsUni = Convert.ToDecimal(m_toolsTotals[0].CstUni5);
                        _total.PrcToolsUni = Convert.ToDecimal(m_toolsTotals[0].PrcUni5);
                        _total.CstToolsTot = Convert.ToDecimal(m_toolsTotals[0].CstTot5);
                        _total.PrcToolsTot = Convert.ToDecimal(m_toolsTotals[0].PrcTot5);

                        break;
                    default:
                        break;
                }
                m_totalsPivot.Add(_total);
            }
        }

        protected void SwitchDependingControls(Mode mode)
        {
            CurQuotationConsoleMode = mode;

            grdQuotationDetails.Visible = mode == Mode.InputItems;
        }

        protected void SetAsReadOnly(int? usageParameter)
        {


            if (usageParameter == 1)
            {
                ((Site)Master).FindControl("tblHeader").Visible = false;
                mnuOperations1.Enabled = false;
                mnuOperations2.Enabled = false;
                mnuQuotationTemplates.Enabled = false;
                dtvQuotation.Enabled = false;
                txtDescription.Enabled = false;
                txtPriceCom.Enabled = false;
                txtPrintingMainText.Enabled = false;
                grdQuotationDetails.Enabled = false;

                lbtCreateProductionOrder.Visible = false;
                //lbtLoadTemplate.Enabled = false;
                lbtPrintQuotation.Enabled = false;
                lbtPrintQuotationTechnical.Enabled = false;
                lbtSave.Enabled = false;
                lbtSaveAsTemplate.Enabled = false;
                btnEmpty.Enabled = false;
                lbtAddUnmanagedItem.Enabled = false;
                //lbtLoadTemplate.Enabled = false;
                //hypListProductionOrders.Visible = false;
            }

            bool canModify = true;
            if (Convert.ToInt32(QuotationParameter) > 0)
            {
                canModify = (new QuotationDataContext().ProductionOrders.Where(po => po.ID_Quotation == Convert.ToInt32(QuotationParameter) && (po.Status == 3)).Count() == 0);
            }
            if (!canModify)
            {
                //((Site)Master).FindControl("tblHeader").Visible = false;
                mnuOperations1.Enabled = false;
                mnuOperations2.Enabled = false;
                mnuQuotationTemplates.Enabled = false;
                dtvQuotation.Enabled = false;
                txtDescription.Enabled = false;
                txtPriceCom.Enabled = false;
                txtPrintingMainText.Enabled = false;
                grdQuotationDetails.Enabled = false;

                lbtCreateProductionOrder.Visible = false;
                //lbtLoadTemplate.Enabled = false;
                lbtPrintQuotation.Enabled = false;
                lbtPrintQuotationTechnical.Enabled = false;
                lbtSave.Enabled = false;
                lbtSaveAsTemplate.Enabled = false;
                btnEmpty.Enabled = false;
                lbtAddUnmanagedItem.Enabled = false;
                //lbtLoadTemplate.Enabled = false;
                //hypListProductionOrders.Visible = false;

            }
        }


        protected void dtvQuotation_ItemCreated(object sender, EventArgs e)
        {
            TempQuotation _currentQuotation = null;
            if (QuotationHeader.Key == 0)
            {
                _currentQuotation = (TempQuotation)dtvQuotation.DataItem;
            }
            else
            {
                using (var QuotationDataContext = new QuotationDataContext())
                {
                    _currentQuotation =
                        QuotationDataContext.TempQuotations.First(
                            Quotation =>
                                Quotation.ID_Quotation == Convert.ToInt32(QuotationParameter) &&
                                Quotation.SessionUser == GetCurrentEmployee().ID);
                }
            }
            QuotationHeader = new KeyValuePair<int, string>(_currentQuotation.ID_Quotation, _currentQuotation.Subject);
            lblQuotationHeader.Text = "Gestione preventivo No. " + QuotationHeader.Key + " (" +
                                      QuotationHeader.Value + ")";
            txtDescription.Text = _currentQuotation.Note;

            //txtPriceCom.Text = _currentQuotation.PriceCom;
            //txtPriceCom.Text = (string.IsNullOrEmpty(_currentQuotation.NoteFromProduction) ? _currentQuotation.PriceCom : _currentQuotation.NoteFromProduction);
            txtPriceCom.Text = ((_currentQuotation.PriceCom ?? "") + _currentQuotation.NoteFromProduction);
            txtPrintingMainText.Text = _currentQuotation.PrintingMainText;

            txtAppunti1.Text = _currentQuotation.Note1;
        }


        protected void ddlTypes_DataBound(object sender, EventArgs e)
        {
            //ddlTypes.Items.Insert(0, "Seleziona un tipo...");
        }

        protected void ddlItemTypes_DataBound(object sender, EventArgs e)
        {
            //ddlItemTypes.Items.Insert(0, "Seleziona un tipo voce...");
        }

        protected void lbtViewInputItems_Click(object sender, EventArgs e)
        {
            SwitchDependingControls(Mode.InputItems);
        }

        protected void grdQuotationDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0 && e.NewPageIndex < grdQuotationDetails.PageCount)
            {
                grdQuotationDetails.PageIndex = e.NewPageIndex;
                SwitchDependingControls(Mode.InputItems);
            }
        }

        protected void lbtPrintQuotation_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                string.Format("{2}?{0}={1}&{3}={4}", QuotationKey, QuotationParameter, QuotationPrintPage, ReportTypeKey,
                    0), true);
        }

        protected void lbtPrintQuotationTechnical_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                string.Format("{2}?{0}={1}&{3}={4}&{5}={6}", QuotationKey, QuotationParameter, QuotationPrintPage,
                    ReportTypeKey, 1, ReportOnProductionQuantityKey, GetSelectedQuantity()), true);
        }

        protected string GetSelectedQuantity()
        {
            var _selectedQuantity = "0";
            if (((RadioButton)dtvQuotation.FindControl("rbtQ1")).Checked)
            {
                _selectedQuantity = ((TextBox)dtvQuotation.FindControl("txtQ1")).Text;
            }
            if (((RadioButton)dtvQuotation.FindControl("rbtQ2")).Checked)
            {
                _selectedQuantity = ((TextBox)dtvQuotation.FindControl("txtQ2")).Text;
            }
            if (((RadioButton)dtvQuotation.FindControl("rbtQ3")).Checked)
            {
                _selectedQuantity = ((TextBox)dtvQuotation.FindControl("txtQ3")).Text;
            }
            if (((RadioButton)dtvQuotation.FindControl("rbtQ4")).Checked)
            {
                _selectedQuantity = ((TextBox)dtvQuotation.FindControl("txtQ4")).Text;
            }
            if (((RadioButton)dtvQuotation.FindControl("rbtQ5")).Checked)
            {
                _selectedQuantity = ((TextBox)dtvQuotation.FindControl("txtQ5")).Text;
            }
            return _selectedQuantity;
        }

        protected void grdQuotationDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var _txtQuantity = (TextBox)e.Row.Cells[8 + 3].FindControl("txtQuantity");
                _txtQuantity.Text = ((TempQuotationDetail)e.Row.DataItem).Quantity.ToString();
                _txtQuantity.Attributes.Add("autocomplete", "something-new");

                var _row = e.Row;
                if (((TempQuotationDetail)e.Row.DataItem).PickingItem != null)
                {
                    //if ((((TempQuotationDetail)e.Row.DataItem).Inserted == false ||
                    //     ((TempQuotationDetail)e.Row.DataItem).PickingItem.Inserted == false) &&
                    //    ((TempQuotationDetail)e.Row.DataItem).Save == false)
                    if ((((TempQuotationDetail)e.Row.DataItem).PickingItem.Inserted == false) && ((TempQuotationDetail)e.Row.DataItem).Save == false)
                    {
                        _row.ForeColor = Color.Red;
                    }

                    if (((TempQuotationDetail)e.Row.DataItem).PickingItem.IsObsolete(GlobalConfiguration))
                    {
                        var _dycCost = (DataControlFieldCell)e.Row.Cells[10 + 3];
                        _dycCost.ControlStyle.ForeColor = Color.Red;
                        _dycCost.ToolTip = string.Format("Il costo di questa voce non è aggiornato da almeno {0} mesi",
                            GlobalConfiguration["PIMU"]);
                    }
                }
                if (((TempQuotationDetail)e.Row.DataItem).MacroItem != null)
                {

                    //if ((((TempQuotationDetail)e.Row.DataItem).Inserted == false ||
                    //    ((TempQuotationDetail)e.Row.DataItem).MacroItem.HasInactivePickingItems()) &&
                    //    ((TempQuotationDetail)e.Row.DataItem).Save)
                    if ((((TempQuotationDetail)e.Row.DataItem).MacroItem.HasInactivePickingItems()) && ((TempQuotationDetail)e.Row.DataItem).Save)
                    {
                        _row.ForeColor = Color.Red;
                    }
                    if (((TempQuotationDetail)e.Row.DataItem).MacroItem.IsObsolete(GlobalConfiguration))
                    {
                        var _dycCost = (DataControlFieldCell)e.Row.Cells[10 + 3];
                        _dycCost.ControlStyle.ForeColor = Color.Red;
                        _dycCost.ToolTip = string.Format("Il costo di una o più voci di dettaglio di questa macrovoce non è aggiornato da almeno {0} mesi",
                            GlobalConfiguration["PIMU"]);
                    }
                }
                m_voicePosition += 1;
                _txtQuantity.TabIndex = m_voicePosition;
                _txtQuantity.Attributes.Add("onchange",
                    "javascript:document.getElementById('ctl00_ContentPlaceHolder1_btnRecalc').disabled = false;this.value = this.value.replace('.',',');");

                if (((TempQuotationDetail)e.Row.DataItem).PickingItem == null &&
                    ((TempQuotationDetail)e.Row.DataItem).MacroItem == null)
                {
                    var _ibtEdit = (ImageButton)e.Row.Cells[0].FindControl("ibtEdit");
                    _ibtEdit.Visible = true;
                    _ibtEdit.Attributes.Add("onclick", "");
                }
            }
        }

        protected void ldsTotals_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            e.Result = m_totalsPivot;
        }

        protected void grdQuotationDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                //SwitchDependingControls(Mode.InputItems);
            }
        }

        protected void grdQuotationDetails_PreRender(object sender, EventArgs e)
        {
            grdQuotationDetails.DataBind();
            using (var _qc = new QuotationDataContext())
            {
                m_totals =
                    _qc.prc_LAB_MGet_LAB_TotalsByTempQuotationID(Convert.ToInt32(QuotationParameter),
                        GetCurrentEmployee().ID).ToList();
                m_toolsTotals =
                    _qc.prc_LAB_MGet_LAB_ToolsTotalsByTempQuotationID(Convert.ToInt32(QuotationParameter),
                        GetCurrentEmployee().ID).ToList();
                UpdatePivot(m_totals, m_toolsTotals);
                grdTotals.DataBind();
                grdToolsTotals.DataBind();
            }
        }

        protected void GridDataSource_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            e.ExceptionHandled = true;
            lbtCreateProductionOrder.Visible = false;
        }

        protected void dtvQuotation_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            using (var QuotationDataContext = new QuotationDataContext())
            {
                TempQuotation _curQuotation =
                    QuotationDataContext.TempQuotations.First(
                        Quotation =>
                            Quotation.ID_Quotation == Convert.ToInt32(QuotationParameter) &&
                            Quotation.SessionUser == GetCurrentEmployee().ID);

                foreach (var _quotationDetail in _curQuotation.TempQuotationDetails)
                {
                    try
                    {
                        _quotationDetail.MarkUp = Convert.ToInt32(e.NewValues["MarkUp"]);
                        _quotationDetail.Price = _quotationDetail.Cost *
                                                 (Convert.ToDecimal(_quotationDetail.Percentage) / 100m) *
                                                 (Convert.ToDecimal(_curQuotation.MarkUp) / 100m);
                    }
                    catch
                    {
                        _quotationDetail.MarkUp = 100;
                    }
                }
                QuotationDataContext.SubmitChanges();
            }
        }

        protected void grdQuotationDetails_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow &&
                (e.Row.RowState == DataControlRowState.Normal ||
                 e.Row.RowState == DataControlRowState.Alternate))
            {
                //e.Row.TabIndex = -1;
                //e.Row.Attributes["onclick"] =
                //  string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex);
                //e.Row.Attributes["onkeydown"] = "javascript:return SelectSibling(event);";
                //e.Row.Attributes["onselectstart"] = "javascript:return false;";
            }
        }

        protected void mnuOperations_MenuItemClick(object sender, MenuEventArgs e)
        {
            using (var QuotationDataContext = new QuotationDataContext())
            {
                IEnumerable<PickingItem> _pickingItems = QuotationDataContext.PickingItems;
                IEnumerable<MacroItem> _macroItems = QuotationDataContext.MacroItems;
                var _menuPath = e.Item.Value.Split('.');
                var _curQuotation =
                    QuotationDataContext.TempQuotations.First(
                        Quotation =>
                            Quotation.ID_Quotation == Convert.ToInt32(QuotationParameter) &&
                            Quotation.SessionUser == GetCurrentEmployee().ID);

                var _cbk = new ComboKey(_menuPath);

                if (_cbk.Prefix == PICKINGITEM_PREFIX)
                {
                    var _toAddPickingItem = _pickingItems.First(pi => pi.ID == _cbk.CommonKey);
                    RecursiveDependenciesAdd(_curQuotation, _toAddPickingItem, _pickingItems, _macroItems);
                    _curQuotation.TempQuotationDetails.Add(CreateDetail(_curQuotation, _toAddPickingItem));
                }

                if (_cbk.Prefix == MACROITEM_PREFIX)
                {
                    var _toAddPickingItem = _macroItems.First(pi => pi.ID == _cbk.CommonKey);
                    RecursiveDependenciesAdd(_curQuotation, _toAddPickingItem, _macroItems, _pickingItems);
                    _curQuotation.TempQuotationDetails.Add(CreateDetail(_curQuotation, _toAddPickingItem));
                }

                QuotationDataContext.SubmitChanges();
                lbtCreateProductionOrder.Visible = false;
            }
        }

        protected void grdQuotationDetails_DataBound(object sender, EventArgs e)
        {
            if (grdQuotationDetails.Rows.Count > 0)
            {
                var _tabStart = (TextBox)grdQuotationDetails.Rows[0].Cells[8 + 3].Controls[1];
                _tabStart.Attributes.Add("onfocus",
                    "this.select();"
                    );
                var _tabStop =
                    (TextBox)grdQuotationDetails.Rows[grdQuotationDetails.Rows.Count - 1].Cells[8 + 3].Controls[1];
                _tabStop.Attributes.Add("onblur",
                    "javascript:document.getElementById('ctl00_ContentPlaceHolder1_grdQuotationDetails_ctl02_txtQuantity').focus();" +
                    "javascript:document.getElementById('ctl00_ContentPlaceHolder1_grdQuotationDetails_ctl02_txtQuantity').select();"
                    );
            }
        }

        protected void GridDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            GridDataSource.OrderByParameters.Clear();
            GridDataSource.AutoGenerateOrderByClause = false;
            var table = GridDataSource.GetTable();
            var _qc = (QuotationDataContext)table.CreateContext();
            e.Result = _qc.TempQuotationDetails.OrderBy(qd => qd.Position);
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }

        protected void CustomValidator3_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
        }

        protected void CustomValidator2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = Convert.ToInt32(args.Value) >= 100;
        }

        protected void btnRecalc_Click(object sender, EventArgs e)
        {
        }

        protected void ldsToolsTotals_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            e.Result = m_totalsPivot[0];
        }

        protected void btnEmpty_Click(object sender, EventArgs e)
        {
            using (var _qc = new QuotationDataContext())
            {
                try
                {
                    var _curQuotation =
                        _qc.TempQuotations.First(
                            Quotation =>
                                Quotation.ID_Quotation == Convert.ToInt32(QuotationParameter) &&
                                Quotation.SessionUser == GetCurrentEmployee().ID);
                    _qc.TempQuotationDetails.DeleteAllOnSubmit(_curQuotation.TempQuotationDetails);
                    _qc.SubmitChanges();
                    Response.Redirect(
                        string.Format("{2}?{0}={1}", QuotationKey, QuotationParameter, TempQuotationConsolePage), true);
                }
                catch
                {
                    ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.ModelLoadingFailed);
                }
            }
        }

        protected void lbtSaveAs_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                int _destQuotation;
                using (var QuotationDataContext = new QuotationDataContext())
                {
                    WriteQuantities(QuotationDataContext);
                    WriteQuotationHeaderData(QuotationDataContext);

                    if (chkUpdateNewQuotation.Checked)
                    {
                        _destQuotation = SaveTempAsNewUpdatedQuotation(QuotationDataContext,
                            txtQuotationDescription.Text);
                        if (Convert.ToInt32(QuotationParameter) < 0)
                        {
                            using (var _qc = new QuotationDataContext())
                            {
                                var _toDeleteId = Convert.ToInt32(QuotationParameter);

                                var _toDeleteTempQuotation =
                                    _qc.TempQuotations.Single(
                                        Quotation =>
                                            Quotation.ID_Quotation == _toDeleteId &&
                                            Quotation.SessionUser == GetCurrentEmployee().ID);
                                _qc.TempQuotationDetails.DeleteAllOnSubmit(_toDeleteTempQuotation.TempQuotationDetails);
                                _qc.SubmitChanges();
                                _qc.TempQuotations.DeleteOnSubmit(_toDeleteTempQuotation);
                                _qc.SubmitChanges();
                            }
                        }
                    }
                    else
                        _destQuotation = SaveTempAsNewQuotation(QuotationDataContext, txtQuotationDescription.Text);
                }
                txtQuotationDescription.Text = string.Empty;
                LoadPersistedQuotation = true;
                Response.Redirect(string.Format("~/QuotationConsole2.aspx?{0}={1}", QuotationKey,
                    _destQuotation));
            }
        }

        protected void lbtSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                int _destQuotation;
                using (var QuotationDataContext = new QuotationDataContext())
                {
                    WriteQuantities(QuotationDataContext);
                    WriteQuotationHeaderData(QuotationDataContext);

                    _destQuotation = SaveQuotation(QuotationDataContext, Global.CurrentSchedulingType); //, txtQuotationDescription.Text);
                }
                //txtQuotationDescription.Text = string.Empty;
                LoadPersistedQuotation = true;
                Response.Redirect(string.Format("~/QuotationConsole2.aspx?{0}={1}", QuotationKey,
                    _destQuotation));
            }
        }

        protected void dtvQuotation_DataBound(object sender, EventArgs e)
        {

            var _ddlManagers = (DropDownList)dtvQuotation.FindControl("ddlManagers");
            if (_ddlManagers != null)
            {
                _ddlManagers.SelectedValue = ((TempQuotation)dtvQuotation.DataItem).ID_Manager.ToString();
                _ddlManagers.Attributes.Add("onchange",
                       "javascript:document.getElementById('ctl00_ContentPlaceHolder1_btnRecalc').disabled = false;");
            }

            var _txtQ1 = (TextBox)dtvQuotation.FindControl("txtQ1");
            var _txtQ2 = (TextBox)dtvQuotation.FindControl("txtQ2");
            var _txtQ3 = (TextBox)dtvQuotation.FindControl("txtQ3");
            var _txtQ4 = (TextBox)dtvQuotation.FindControl("txtQ4");
            var _txtQ5 = (TextBox)dtvQuotation.FindControl("txtQ5");
            var _txtMarkUp = (TextBox)dtvQuotation.FindControl("txtMarkUp");


            if (_txtQ1 != null && _txtQ2 != null && _txtQ3 != null && _txtQ4 != null && _txtQ5 != null &&
                _txtMarkUp != null)
            {
                _txtQ1.Attributes.Add("onchange",
                    "javascript:document.getElementById('ctl00_ContentPlaceHolder1_btnRecalc').disabled = false;");
                _txtQ2.Attributes.Add("onchange",
                    "javascript:document.getElementById('ctl00_ContentPlaceHolder1_btnRecalc').disabled = false;");
                _txtQ3.Attributes.Add("onchange",
                    "javascript:document.getElementById('ctl00_ContentPlaceHolder1_btnRecalc').disabled = false;");
                _txtQ4.Attributes.Add("onchange",
                    "javascript:document.getElementById('ctl00_ContentPlaceHolder1_btnRecalc').disabled = false;");
                _txtQ5.Attributes.Add("onchange",
                    "javascript:document.getElementById('ctl00_ContentPlaceHolder1_btnRecalc').disabled = false;");
                _txtMarkUp.Attributes.Add("onchange",
                    "javascript:document.getElementById('ctl00_ContentPlaceHolder1_btnRecalc').disabled = false;");

                //if (Convert.ToInt32(_txtQ3.Text) == -1 && Convert.ToInt32(_txtQ4.Text) == -1 &&
                //    Convert.ToInt32(_txtQ5.Text) == -1)
                //{
                //    _txtQ3.Enabled = _txtQ4.Enabled = _txtQ5.Enabled = false;
                //    var _CustomValidator2 = (CustomValidator)dtvQuotation.FindControl("CustomValidator2");
                //    _CustomValidator2.Enabled = false;
                //}
            }
            if (txtDescription != null)
            {
                txtDescription.Attributes.Add("onchange",
                    "javascript:document.getElementById('ctl00_ContentPlaceHolder1_btnRecalc').disabled = false;");
            }
            if (txtPriceCom != null)
            {
                txtPriceCom.Attributes.Add("onchange",
                    "javascript:document.getElementById('ctl00_ContentPlaceHolder1_btnRecalc').disabled = false;");
            }
            if (txtPrintingMainText != null)
            {
                txtPrintingMainText.Attributes.Add("onchange",
                    "javascript:document.getElementById('ctl00_ContentPlaceHolder1_btnRecalc').disabled = false;");
            }



            var hidSearchCli = (HiddenField)dtvQuotation.FindControl("hidSearchCli");
            var txtSearchCli = (TextBox)dtvQuotation.FindControl("txtSearchCli");
            if (hidSearchCli != null)
            {

                if (((TempQuotation)dtvQuotation.DataItem).CustomerCode != null)
                {
                    hidSearchCli.Value = ((TempQuotation)dtvQuotation.DataItem).CustomerCode.ToString();
                    using (var QuotationDataContext = new QuotationDataContext())
                    {
                        Customer found = QuotationDataContext.Customers.FirstOrDefault(c => !c.Name.StartsWith("**") && c.Code == Convert.ToInt32(hidSearchCli.Value));
                        if (found != null)
                        {
                            txtSearchCli.Text = found.Name;
                        }
                    }
                }

            }



        }

        protected void lbtAddUnmanagedItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (var QuotationDataContext = new QuotationDataContext())
                {
                    var _curQuotation =
                        QuotationDataContext.TempQuotations.First(
                            Quotation =>
                                Quotation.ID_Quotation == Convert.ToInt32(QuotationParameter) &&
                                Quotation.SessionUser == GetCurrentEmployee().ID);
                    var _unmanagedQuotationDetail = new TempQuotationDetail();
                    _unmanagedQuotationDetail.TypeCode =
                        Convert.ToInt32(ConfigurationManager.AppSettings["UnmanagedQuotationDetailTypeCode"]);
                    _unmanagedQuotationDetail.ItemTypeCode =
                        Convert.ToInt32(ConfigurationManager.AppSettings["UnmanagedQuotationDetailItemTypeCode"]);
                    _unmanagedQuotationDetail.ItemTypeDescription = "(VOCE LIBERA)";
                    _unmanagedQuotationDetail.MarkUp = Convert.ToInt32(_curQuotation.MarkUp);
                    _unmanagedQuotationDetail.Percentage = 100;
                    _unmanagedQuotationDetail.UM = 7;
                    _unmanagedQuotationDetail.Inserted = true;
                    _unmanagedQuotationDetail.SupplierCode = INTERNAL_SUPPLIERCODE;
                    _unmanagedQuotationDetail.Position = "ZZZZZZ";
                    _unmanagedQuotationDetail.ID_Company = CurrentCompanyId;
                    _curQuotation.TempQuotationDetails.Add(_unmanagedQuotationDetail);
                    QuotationDataContext.SubmitChanges();
                    lbtCreateProductionOrder.Visible = false;
                }
            }
            catch
            {
            }
        }

        protected void dtvQuotation_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            int? _markUp = 0;

            try
            {
                using (var QuotationDataContext = new QuotationDataContext())
                {
                    var _curCustomersMarkUp =
                        QuotationDataContext.CustomersMarkUps.First(
                            CustomersMarkUp => CustomersMarkUp.Code == Convert.ToInt32(e.NewValues["CustomerCode"]));
                    _markUp = _curCustomersMarkUp.MarkUp;
                    if (_markUp < 110)
                    {
                        throw new Exception();
                    }
                }
            }
            catch
            {
                _markUp = 110;
            }
            e.NewValues["MarkUp"] = _markUp;
            e.NewValues["Note"] = txtDescription.Text;

            e.NewValues["PriceCom"] = txtPriceCom.Text;

            e.NewValues["PrintingMainText"] = txtPrintingMainText.Text;


        }

        protected void lbtNewQuotation_Click(object sender, EventArgs e)
        {
            var _result = new QuotationService().DelLocks(WebUser.Member.ProviderUserKey.ToString(), SessionTimeStamp);
            Response.Redirect("~/QuotationInsert2.aspx");
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetIsSyncronized(string quotationParameter, string sessionUserId)
        {
            return
                (IsSyncronized(quotationParameter, Convert.ToInt32(sessionUserId)) &&
                 Convert.ToInt32(quotationParameter) > 0).ToString();
        }

        protected enum Mode
        {
            InputItems,
            Calculation,
            Results
        }

        protected void mnuQuotationTemplates_MenuItemClick(object sender, MenuEventArgs e)
        {

            using (var _qc = new QuotationDataContext())
            {

                var _menuPath = e.Item.Value.Split('.');
                var _cbk = new ComboKey(_menuPath);
                var _curQuotation =
                    _qc.TempQuotations.First(
                        Quotation =>
                            Quotation.ID_Quotation == Convert.ToInt32(QuotationParameter) &&
                            Quotation.SessionUser == GetCurrentEmployee().ID);

                var _curQuotationTemplate = _qc.QuotationTemplates.First(qt => qt.ID == _cbk.CommonKey);

                var _pickingItemTypes =
                    _qc.PickingItems.Select(
                        pi =>
                            new
                            {
                                pi.ID,
                                pi.Cost,
                                pi.PercentageAuto,
                                pi.Inserted,
                                pi.Multiply,
                                pi.SupplierCode,
                                pi.Order,
                                pi.ItemDescription
                            }).ToDictionary(pi => pi.ID.ToString());
                var _macroItemTypes =
                    _qc.MacroItems.Select(
                        pi =>
                            new
                            {
                                pi.ID,
                                pi.CostCalc,
                                pi.PercentageCalc,
                                pi.Inserted,
                                pi.Multiply,
                                INTERNAL_SUPPLIERCODE,
                                pi.Order,
                                pi.MacroItemDescription
                            }).ToDictionary(pi => pi.ID.ToString());

                try
                {
                    _curQuotation.Q1 = _curQuotationTemplate.Q1;
                    _curQuotation.Q2 = _curQuotationTemplate.Q2;
                    _curQuotation.Q3 = _curQuotationTemplate.Q3;
                    _curQuotation.Q4 = _curQuotationTemplate.Q4;
                    _curQuotation.Q5 = _curQuotationTemplate.Q5;
                    ((TextBox)dtvQuotation.FindControl("txtQ1")).Text = _curQuotationTemplate.Q1.ToString();
                    ((TextBox)dtvQuotation.FindControl("txtQ2")).Text = _curQuotationTemplate.Q2.ToString();
                    ((TextBox)dtvQuotation.FindControl("txtQ3")).Text = _curQuotationTemplate.Q3.ToString();
                    ((TextBox)dtvQuotation.FindControl("txtQ4")).Text = _curQuotationTemplate.Q4.ToString();
                    ((TextBox)dtvQuotation.FindControl("txtQ5")).Text = _curQuotationTemplate.Q5.ToString();

                    foreach (
                        var _quotationTemplateDetail in
                            _curQuotationTemplate.QuotationTemplateDetails)
                    {
                        var _quotationDetail = new TempQuotationDetail();

                        if (_quotationTemplateDetail.Save)
                        {
                            _quotationDetail.CommonKey = _quotationTemplateDetail.CommonKey;
                            _quotationDetail.MacroItemKey = _quotationTemplateDetail.MacroItemKey;
                            _quotationDetail.Cost =
                                Convert.ToDecimal(
                                    _macroItemTypes[_quotationTemplateDetail.MacroItemKey.ToString()].CostCalc);
                            _quotationDetail.ID_Quotation = Convert.ToInt32(QuotationParameter);
                            _quotationDetail.SessionUser = GetCurrentEmployee().ID;
                            _quotationDetail.Inserted =
                                _macroItemTypes[_quotationTemplateDetail.MacroItemKey.ToString()].Inserted;
                            _quotationDetail.ItemTypeCode = _quotationTemplateDetail.ItemTypeCode;
                            _quotationDetail.ItemTypeDescription = "[" +
                                                                   _macroItemTypes[
                                                                       _quotationTemplateDetail.MacroItemKey.ToString()]
                                                                       .MacroItemDescription + "]";
                            _quotationDetail.MarkUp = Convert.ToInt32(_curQuotation.MarkUp);
                            _quotationDetail.Multiply =
                                _macroItemTypes[_quotationTemplateDetail.MacroItemKey.ToString()].Multiply;
                            _quotationDetail.Percentage =
                                Convert.ToInt32(
                                    _macroItemTypes[_quotationTemplateDetail.MacroItemKey.ToString()].PercentageCalc);
                            _quotationDetail.Position =
                                _macroItemTypes[_quotationTemplateDetail.MacroItemKey.ToString()].Order;
                            _quotationDetail.Price =
                                Convert.ToDecimal(_quotationDetail.Cost * (_quotationDetail.Percentage / 100m) *
                                                  (_curQuotation.MarkUp / 100m));
                            _quotationDetail.Quantity = _quotationTemplateDetail.Quantity;
                            _quotationDetail.Save = _quotationTemplateDetail.Save;
                            _quotationDetail.SelectPhase = _quotationTemplateDetail.SelectPhase;
                            _quotationDetail.SupplierCode =
                                _macroItemTypes[_quotationTemplateDetail.MacroItemKey.ToString()].INTERNAL_SUPPLIERCODE;
                            _quotationDetail.TypeCode = _quotationTemplateDetail.TypeCode;
                            _quotationDetail.UM = _quotationTemplateDetail.UM;
                            _quotationDetail.ID_Company = _quotationTemplateDetail.ID_Company;
                        }
                        else
                        {
                            _quotationDetail.CommonKey = _quotationTemplateDetail.CommonKey;
                            _quotationDetail.MacroItemKey = _quotationTemplateDetail.MacroItemKey;
                            _quotationDetail.Cost =
                                Convert.ToDecimal(_pickingItemTypes[_quotationTemplateDetail.CommonKey.ToString()].Cost);
                            _quotationDetail.ID_Quotation = Convert.ToInt32(QuotationParameter);
                            _quotationDetail.SessionUser = GetCurrentEmployee().ID;
                            _quotationDetail.Inserted =
                                _pickingItemTypes[_quotationTemplateDetail.CommonKey.ToString()].Inserted;
                            _quotationDetail.ItemTypeCode = _quotationTemplateDetail.ItemTypeCode;
                            _quotationDetail.ItemTypeDescription =
                                _pickingItemTypes[_quotationTemplateDetail.CommonKey.ToString()].ItemDescription;
                            _quotationDetail.MarkUp = Convert.ToInt32(_curQuotation.MarkUp);
                            _quotationDetail.Multiply =
                                _pickingItemTypes[_quotationTemplateDetail.CommonKey.ToString()].Multiply;
                            _quotationDetail.Percentage =
                                Convert.ToInt32(
                                    _pickingItemTypes[_quotationTemplateDetail.CommonKey.ToString()].PercentageAuto);
                            _quotationDetail.Position =
                                _pickingItemTypes[_quotationTemplateDetail.CommonKey.ToString()].Order;
                            _quotationDetail.Price =
                                Convert.ToDecimal(_quotationDetail.Cost * (_quotationDetail.Percentage / 100m) *
                                                  (_curQuotation.MarkUp / 100m));
                            _quotationDetail.Quantity = _quotationTemplateDetail.Quantity;
                            _quotationDetail.Save = _quotationTemplateDetail.Save;
                            _quotationDetail.SelectPhase = _quotationTemplateDetail.SelectPhase;
                            _quotationDetail.SupplierCode =
                                _pickingItemTypes[_quotationTemplateDetail.CommonKey.ToString()].SupplierCode;
                            _quotationDetail.TypeCode = _quotationTemplateDetail.TypeCode;
                            _quotationDetail.UM = _quotationTemplateDetail.UM;
                            _quotationDetail.ID_Company = _quotationTemplateDetail.ID_Company;
                        }

                        _curQuotation.TempQuotationDetails.Add(_quotationDetail);
                    }
                    _qc.SubmitChanges();


                    bool isAuto = (_curQuotation.Q3 == -1 && _curQuotation.Q4 == -1 && _curQuotation.Q5 == -1);
                    lbtCreateProductionOrder.Visible = !isAuto;
                    lbtPrintQuotation.Enabled = !isAuto;
                    lbtPrintQuotationTechnical.Enabled = !isAuto;
                    lbtSave.Enabled = !isAuto;
                    lbtSaveAsTemplate.Enabled = !isAuto;
                    btnEmpty.Enabled = !isAuto;

                    lbtCreateProductionOrder.Visible = false;

                }
                catch
                {
                    ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.ModelLoadingFailed);
                }

            }

        }

        protected void lbtSaveAsTemplate_Click1(object sender, EventArgs e)
        {

            if (Page.IsValid)
            {
                using (var QuotationDataContext = new QuotationDataContext())
                {
                    var _quotationTemplate = new QuotationTemplate();
                    _quotationTemplate.Description = txtTemplateDescription.Text;
                    //_quotationTemplate.TypeCode = 41;
                    //_quotationTemplate.ItemTypeCode = 92;
                    _quotationTemplate.TypeCode = Convert.ToInt32(ddlTypes.SelectedValue);
                    _quotationTemplate.ItemTypeCode = Convert.ToInt32(ddlItemTypes.SelectedValue);
                    _quotationTemplate.UM = 3;
                    _quotationTemplate.Q1 = Convert.ToInt32(((TextBox)dtvQuotation.FindControl("txtQ1")).Text);
                    _quotationTemplate.Q2 = Convert.ToInt32(((TextBox)dtvQuotation.FindControl("txtQ2")).Text);
                    _quotationTemplate.Q3 = Convert.ToInt32(((TextBox)dtvQuotation.FindControl("txtQ3")).Text);
                    _quotationTemplate.Q4 = Convert.ToInt32(((TextBox)dtvQuotation.FindControl("txtQ4")).Text);
                    _quotationTemplate.Q5 = Convert.ToInt32(((TextBox)dtvQuotation.FindControl("txtQ5")).Text);
                    _quotationTemplate.Inserted = true;
                    //_quotationTemplate.Order = "ZZZZZZZZZZ";
                    _quotationTemplate.Order = txtOrder.Text;
                    QuotationDataContext.QuotationTemplates.InsertOnSubmit(_quotationTemplate);

                    var _curQuotation =
                        QuotationDataContext.TempQuotations.First(
                            Quotation =>
                                Quotation.ID_Quotation == Convert.ToInt32(QuotationParameter) &&
                                Quotation.SessionUser == GetCurrentEmployee().ID);

                    foreach (var _quotationDetail in _curQuotation.TempQuotationDetails)
                    {
                        var _quotationTemplateDetail = new QuotationTemplateDetail();

                        _quotationTemplateDetail.CommonKey = _quotationDetail.CommonKey;
                        _quotationTemplateDetail.MacroItemKey = _quotationDetail.MacroItemKey;
                        _quotationTemplateDetail.Cost = _quotationDetail.Cost;
                        _quotationTemplateDetail.ID_QuotationTemplate = _quotationTemplate.ID;
                        _quotationTemplateDetail.Inserted = _quotationDetail.Inserted;
                        _quotationTemplateDetail.ItemTypeCode = _quotationDetail.ItemTypeCode;
                        _quotationTemplateDetail.ItemTypeDescription = _quotationDetail.ItemTypeDescription;
                        _quotationTemplateDetail.MarkUp = _quotationDetail.MarkUp;
                        _quotationTemplateDetail.Multiply = _quotationDetail.Multiply;
                        _quotationTemplateDetail.Percentage = _quotationDetail.Percentage;
                        _quotationTemplateDetail.Position = _quotationDetail.Position;
                        _quotationTemplateDetail.Price = _quotationDetail.Price;
                        _quotationTemplateDetail.Quantity = _quotationDetail.Quantity;
                        _quotationTemplateDetail.Save = _quotationDetail.Save;
                        _quotationTemplateDetail.SelectPhase = _quotationDetail.SelectPhase;
                        _quotationTemplateDetail.SupplierCode = _quotationDetail.SupplierCode;
                        _quotationTemplateDetail.TypeCode = _quotationDetail.TypeCode;
                        _quotationTemplateDetail.UM = _quotationDetail.UM;
                        _quotationTemplateDetail.ID_Company = _quotationDetail.ID_Company;

                        _quotationTemplate.QuotationTemplateDetails.Add(_quotationTemplateDetail);
                    }
                    QuotationDataContext.SubmitChanges();
                }
                txtTemplateDescription.Text = string.Empty;
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