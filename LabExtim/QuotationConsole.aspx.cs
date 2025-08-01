using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.DynamicData;
using System.Web.UI.WebControls;
using CMLabExtim.CustomClasses;
using DLLabExtim;
using UILabExtim;

namespace LabExtim
{
    public partial class QuotationConsole : QuotationController
    {
        protected List<SPQDetailTotal> m_toolsTotals;
        //protected List<SPQDetailCalculation> m_calculatedItems;
        //protected List<SPQDetailResult> m_calculatedResults;
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
                return (Mode) Session["CurQuotationConsoleMode"];
            }
            set { Session["CurQuotationConsoleMode"] = value; }
        }

        protected int? GetQuotationCustomerID()
        {
            return new QuotationDataContext().Quotations.SingleOrDefault(q => q.ID ==
                                                                              Convert.ToInt32(
                                                                                  dtvQuotation.DataKey.Values[0]
                                                                                      .ToString())).CustomerCode;

            //if (dtvQuotation.CurrentMode == DetailsViewMode.ReadOnly)
            //{
            //    DynamicControl _dycCustomer = (DynamicControl)dtvQuotation.FindControl("dycCustomer");
            //    int _customerCode =
            //        new QuotationDataContext().Customers.SingleOrDefault(
            //        c => c.Name ==
            //            ((HyperLink)_dycCustomer.Controls[0].Controls[0]).Text).Code;
            //    return _customerCode;
            //}
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(dtvQuotation);
            DynamicDataManager1.RegisterControl(grdQuotationDetails);
            //DynamicDataManager1.RegisterControl(grdQuotationCalculatedDetails);
            //DynamicDataManager1.RegisterControl(grdQuotationResults);
            m_voicePosition = 1000;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //InitSelectionData();
            //CustomValidator1.Enabled = true;
            //RequiredFieldValidator1.Enabled = false;
            //RegularExpressionValidator1.Enabled = false;
            Page.Form.DefaultButton = btnRecalc.UniqueID;
            var _result = new QuotationService().SetLock(1, Convert.ToInt32(QuotationParameter),
                WebUser.Member.ProviderUserKey.ToString(), SessionTimeStamp);

            if (!IsPostBack)
            {
                LoadHeaderMenu(mnuOperations1, MenuType.MenuOperations, 1);
                LoadHeaderMenu(mnuOperations2, MenuType.MenuOperations, 2);
                FillControls();
                //SwitchDependingControls(CurQuotationConsoleMode);
            }
            FillDependingControls(CurQuotationConsoleMode);
            SetAsReadOnly(UsageParameter);
            //this.DataBind();
        }

        //private void ReadQuantities()
        //{

        //    using (QuotationDataContext _qc = new QuotationDataContext())
        //    {

        //        for (int i = 0; i < grdQuotationDetails.Rows.Count; i++)
        //        {
        //            TextBox _txtQuantity = (TextBox)grdQuotationDetails.Rows[i].Cells[7].FindControl("txtQuantity");
        //            Literal _litID = (Literal)grdQuotationDetails.Rows[i].Cells[1].FindFieldTemplate("ID").Controls[0];
        //            int _toModifyId = Convert.ToInt32(_litID.Text);
        //            QuotationDetail _toUpdateQuotationDetail = _qc.QuotationDetails.Single(QuotationDetail => QuotationDetail.ID == _toModifyId);
        //            _txtQuantity.Text = _toUpdateQuotationDetail.Quantity.ToString();

        //        }
        //    }

        //}

        private void WriteQuantities(QuotationDataContext qc)
        {
            //using (QuotationDataContext qc = new QuotationDataContext())
            //{

            for (var i = 0; i < grdQuotationDetails.Rows.Count; i++)
            {
                var _txtQuantity = (TextBox) grdQuotationDetails.Rows[i].Cells[8].FindControl("txtQuantity");
                var _litID = (Literal) grdQuotationDetails.Rows[i].Cells[1].FindFieldTemplate("ID").Controls[0];
                var _toModifyId = Convert.ToInt32(_litID.Text);
                var _toUpdateQuotationDetail =
                    qc.QuotationDetails.Single(QuotationDetail => QuotationDetail.ID == _toModifyId);
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
                        _digitToDivide = _digitToDivide/_digitDivisor;
                    }
                    _quantity = _digitToDivide;
                }
                else
                {
                    float.TryParse(_txtQuantity.Text, out _quantity);
                }
                _toUpdateQuotationDetail.Quantity = _quantity;
            }
            //qc.SubmitChanges();

            //}
        }

        private void WriteQuotationHeaderData(QuotationDataContext qc)
        {
            //using (QuotationDataContext qc = new QuotationDataContext())
            //{
            if (dtvQuotation.CurrentMode == DetailsViewMode.ReadOnly)
            {
                var _toUpdateQuotation =
                    qc.Quotations.Single(Quotation => Quotation.ID == Convert.ToInt32(QuotationParameter));

                var _txtQuantity1 = (TextBox) dtvQuotation.FindControl("txtQ1");
                var _quantity1 = 0;
                int.TryParse(_txtQuantity1.Text, out _quantity1);
                _toUpdateQuotation.Q1 = _quantity1;

                var _txtQuantity2 = (TextBox) dtvQuotation.FindControl("txtQ2");
                var _quantity2 = 0;
                int.TryParse(_txtQuantity2.Text, out _quantity2);
                _toUpdateQuotation.Q2 = _quantity2;

                var _txtQuantity3 = (TextBox) dtvQuotation.FindControl("txtQ3");
                var _quantity3 = 0;
                int.TryParse(_txtQuantity3.Text, out _quantity3);
                _toUpdateQuotation.Q3 = _quantity3;

                var _txtQuantity4 = (TextBox) dtvQuotation.FindControl("txtQ4");
                var _quantity4 = 0;
                int.TryParse(_txtQuantity4.Text, out _quantity4);
                _toUpdateQuotation.Q4 = _quantity4;

                var _txtQuantity5 = (TextBox) dtvQuotation.FindControl("txtQ5");
                var _quantity5 = 0;
                int.TryParse(_txtQuantity5.Text, out _quantity5);
                _toUpdateQuotation.Q5 = _quantity5;

                var _txtMarkUp = (TextBox) dtvQuotation.FindControl("txtMarkUp");
                var _markUp = 0;
                int.TryParse(_txtMarkUp.Text, out _markUp);
                _toUpdateQuotation.MarkUp = _markUp;

                _toUpdateQuotation.Note = txtDescription.Text;

                foreach (var _quotationDetail in _toUpdateQuotation.QuotationDetails)
                {
                    _quotationDetail.MarkUp = _markUp;
                    _quotationDetail.Price =
                        Convert.ToDecimal(_quotationDetail.Cost*(_quotationDetail.Percentage/100m)*(_markUp/100m));
                }
                //qc.SubmitChanges();
            }
            //}
        }

        private void FillControls()
        {
            using (var QuotationDataContext =
                new QuotationDataContext())
            {
                IEnumerable<QuotationTemplate> _quotationTemplates =
                    QuotationDataContext.QuotationTemplates;
                ddlTemplates.DataSource = _quotationTemplates;
                ddlTemplates.DataTextField = "Description";
                ddlTemplates.DataValueField = "ID";
                ddlTemplates.DataBind();
            }
            hypListProductionOrders.Attributes.Add("onclick",
                "javascript:OpenBigItem('ProductionOrdersList.aspx?" + "POquo=" + QuotationParameter + "')");
            hypListProductionOrders.Attributes["onmouseover"] =
                "this.style.cursor='hand';this.style.textDecoration='underline';";
            hypListProductionOrders.Attributes["onmouseout"] = "this.style.textDecoration='none';";

            //IEnumerable<Quotation> _currentQuotations =
            // (IEnumerable<Quotation>)QuotationDataContext.Quotations.Where(Quotation => Quotation.ID == Convert.ToInt32(QuotationParameter));
            //Quotation _currentQuotation = _currentQuotations.First();
            //quotationLabel = new KeyValuePair<int, string>(_currentQuotation.ID, _currentQuotation.Subject);

            //foreach (HyperLink _selector in GlobalVariables.GetPickingSelectors())
            //{
            //    TableCell _cell = new TableCell();
            //    _cell.Controls.Add(_selector);
            //    TableRow _row = new TableRow();
            //    _row.Cells.Add(_cell);
            //    tblSelectors.Controls.Add(_row);
            //}
        }

        protected void FillDependingControls(Mode mode)
        {
            var filterString = "ID_Quotation = @ID";

            GridDataSource.WhereParameters.Clear();
            //GridDataSource.WhereParameters.Add("ID", DbType.Int32, SelectionData.ToString());
            GridDataSource.WhereParameters.Add("ID", DbType.Int32, QuotationParameter);
            GridDataSource.Where = filterString;
            GridDataSource.AutoGenerateWhereClause = false;
            //GridDataSource.DataBind();
            //WriteQuantities();

            using (var _qc = new QuotationDataContext())
            {
                WriteQuantities(_qc);
                WriteQuotationHeaderData(_qc);
                _qc.SubmitChanges();

                //m_calculatedItems = _qc.prc_LAB_MGet_LAB_CalculatedDetailsByQuotationID(Convert.ToInt32(QuotationParameter)).ToList<SPQDetailCalculation>();
                //m_calculatedResults = _qc.prc_LAB_MGet_LAB_CalculatedResultsByQuotationID(Convert.ToInt32(QuotationParameter)).ToList<SPQDetailResult>();
                m_totals =
                    _qc.prc_LAB_MGet_LAB_TotalsByQuotationID(Convert.ToInt32(QuotationParameter))
                        .ToList();
                m_toolsTotals =
                    _qc.prc_LAB_MGet_LAB_ToolsTotalsByQuotationID(Convert.ToInt32(QuotationParameter))
                        .ToList();
                UpdatePivot(m_totals, m_toolsTotals);

                var _currentQuotation =
                    _qc.Quotations.SingleOrDefault(Quotation => Quotation.ID == Convert.ToInt32(QuotationParameter));
                foreach (var _quotationDetail in _currentQuotation.QuotationDetails)
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

                if (_currentQuotation.Q3 == -1 && _currentQuotation.Q4 == -1 && _currentQuotation.Q5 == -1)
                {
                    lbtCreateProductionOrder.Visible = false;
                    lbtLoadTemplate.Enabled = false;
                    lbtPrintQuotation.Enabled = false;
                    lbtPrintQuotationTechnical.Enabled = false;
                    lbtSave.Enabled = false;
                    lbtSaveAsTemplate.Enabled = false;
                    //lbtViewInputItems.Enabled = false;
                    btnEmpty.Enabled = false;
                }

                lbtCreateProductionOrder.Visible =
                    !(_currentQuotation.Draft == true || _currentQuotation.ProductionOrders.Count > 0);

                //if (grdQuotationDetails.Rows.Count > 0)
                //{
                //    btnRecalc_Click(null, null);
                //    using (QuotationDataContext _qc = new QuotationDataContext())
                //    {
                //        Quotation _currentQuotation = _qc.Quotations.First(Quotation => Quotation.ID == Convert.ToInt32(QuotationParameter));

                //        lbtCreateProductionOrder.Attributes.Add("onclick", "javascript:OpenItem('ProductionOrderPopup.aspx?" +
                //        ProductionOrderController.POQuotationIdKey + "=" + _currentQuotation.ID + "&" +
                //        ProductionOrderController.POQuantityKey + "=" + GetSelectedQuantity() +
                //        "')");
                //    }

                //}

                if (grdQuotationDetails.Rows.Count > 0)
                {
                    //lbtCreateProductionOrder.Attributes.Remove("onclick");
                    //lbtCreateProductionOrder.Attributes.Add("onclick", "javascript:OpenItem('ProductionOrderPopup.aspx?" +
                    //ProductionOrderController.POQuotationIdKey + "=" + _currentQuotation.ID + "&" +
                    //ProductionOrderController.POQuantityKey + "=" + GetSelectedQuantity().ToString() + 
                    //"')");
                }
            }


            //switch (mode)
            //{
            //    case Mode.InputItems:

            //string filterString = "ID_Quotation = @ID";

            //GridDataSource.WhereParameters.Clear();
            ////GridDataSource.WhereParameters.Add("ID", DbType.Int32, SelectionData.ToString());
            //GridDataSource.WhereParameters.Add("ID", DbType.Int32, QuotationParameter);
            //GridDataSource.Where = filterString;
            //GridDataSource.AutoGenerateWhereClause = false;
            //GridDataSource.DataBind();
            //    break;

            //case Mode.Calculation:

            //    using (QuotationDataContext _qc = new QuotationDataContext())
            //    {
            //        grdQuotationCalculatedDetails.DataSource = null;
            //        m_calculatedItems = _qc.prc_LAB_MGet_LAB_CalculatedDetailsByQuotationID(Convert.ToInt32(QuotationParameter));
            //        grdQuotationCalculatedDetails.DataSource = m_calculatedItems;
            //        grdQuotationCalculatedDetails.DataBind();
            //    //}
            ////    break;
            ////case Mode.Results:

            //    //using (QuotationDataContext _qc = new QuotationDataContext())
            //    //{
            //        grdQuotationCalculatedDetails.DataSource = null;
            //        m_calculatedResults = _qc.prc_LAB_MGet_LAB_CalculatedResultsByQuotationID(Convert.ToInt32(QuotationParameter));
            //        grdQuotationResults.DataSource = m_calculatedResults;
            //        grdQuotationResults.DataBind();
            //    }
            //    break;

            //default:
            //    break;

            //}

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
                        _total.PrcUni = Convert.ToDecimal(m_totals[0].PrcUni1);
                        _total.CstTot = Convert.ToDecimal(m_totals[0].CstTot1);
                        _total.PrcTot = Convert.ToDecimal(m_totals[0].PrcTot1);

                        _total.CstToolsUni = Convert.ToDecimal(m_toolsTotals[0].CstUni1);
                        _total.PrcToolsUni = Convert.ToDecimal(m_toolsTotals[0].PrcUni1);
                        _total.CstToolsTot = Convert.ToDecimal(m_toolsTotals[0].CstTot1);
                        _total.PrcToolsTot = Convert.ToDecimal(m_toolsTotals[0].PrcTot1);

                        break;
                    case 1:
                        _total.Quantity = m_totals[0].Q2;
                        _total.CstUni = Convert.ToDecimal(m_totals[0].CstUni2);
                        _total.PrcUni = Convert.ToDecimal(m_totals[0].PrcUni2);
                        _total.CstTot = Convert.ToDecimal(m_totals[0].CstTot2);
                        _total.PrcTot = Convert.ToDecimal(m_totals[0].PrcTot2);

                        _total.CstToolsUni = Convert.ToDecimal(m_toolsTotals[0].CstUni2);
                        _total.PrcToolsUni = Convert.ToDecimal(m_toolsTotals[0].PrcUni2);
                        _total.CstToolsTot = Convert.ToDecimal(m_toolsTotals[0].CstTot2);
                        _total.PrcToolsTot = Convert.ToDecimal(m_toolsTotals[0].PrcTot2);

                        break;
                    case 2:
                        _total.Quantity = m_totals[0].Q3;
                        _total.CstUni = Convert.ToDecimal(m_totals[0].CstUni3);
                        _total.PrcUni = Convert.ToDecimal(m_totals[0].PrcUni3);
                        _total.CstTot = Convert.ToDecimal(m_totals[0].CstTot3);
                        _total.PrcTot = Convert.ToDecimal(m_totals[0].PrcTot3);

                        _total.CstToolsUni = Convert.ToDecimal(m_toolsTotals[0].CstUni3);
                        _total.PrcToolsUni = Convert.ToDecimal(m_toolsTotals[0].PrcUni3);
                        _total.CstToolsTot = Convert.ToDecimal(m_toolsTotals[0].CstTot3);
                        _total.PrcToolsTot = Convert.ToDecimal(m_toolsTotals[0].PrcTot3);

                        break;
                    case 3:
                        _total.Quantity = m_totals[0].Q4;
                        _total.CstUni = Convert.ToDecimal(m_totals[0].CstUni4);
                        _total.PrcUni = Convert.ToDecimal(m_totals[0].PrcUni4);
                        _total.CstTot = Convert.ToDecimal(m_totals[0].CstTot4);
                        _total.PrcTot = Convert.ToDecimal(m_totals[0].PrcTot4);

                        _total.CstToolsUni = Convert.ToDecimal(m_toolsTotals[0].CstUni4);
                        _total.PrcToolsUni = Convert.ToDecimal(m_toolsTotals[0].PrcUni4);
                        _total.CstToolsTot = Convert.ToDecimal(m_toolsTotals[0].CstTot4);
                        _total.PrcToolsTot = Convert.ToDecimal(m_toolsTotals[0].PrcTot4);

                        break;
                    case 4:
                        _total.Quantity = m_totals[0].Q5;
                        _total.CstUni = Convert.ToDecimal(m_totals[0].CstUni5);
                        _total.PrcUni = Convert.ToDecimal(m_totals[0].PrcUni5);
                        _total.CstTot = Convert.ToDecimal(m_totals[0].CstTot5);
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
            //grdQuotationCalculatedDetails.DataBind();

            //grdQuotationCalculatedDetails.Visible = mode == Mode.Calculation;
            //grdQuotationCalculatedDetails.DataBind();

            //grdQuotationResults.Visible = mode == Mode.Results;
            //grdQuotationResults.DataBind();

            //fvwTotals.DataBind();
        }

        protected void SetAsReadOnly(int? usageParameter)
        {
            if (usageParameter == 1)
            {
                ((Site) Master).FindControl("tblHeader").Visible = false;
                mnuOperations1.Enabled = false;
                mnuOperations2.Enabled = false;
                dtvQuotation.Enabled = false;
                txtDescription.Enabled = false;
                grdQuotationDetails.Enabled = false;

                lbtCreateProductionOrder.Enabled = false;
                lbtLoadTemplate.Enabled = false;
                lbtPrintQuotation.Enabled = false;
                lbtPrintQuotationTechnical.Enabled = false;
                lbtSave.Enabled = false;
                lbtSaveAsTemplate.Enabled = false;
                btnEmpty.Enabled = false;
                lbtAddUnmanagedItem.Enabled = false;
                lbtLoadTemplate.Enabled = false;
                hypListProductionOrders.Visible = false;
            }
        }

        protected void lbtSaveAsTemplate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                using (var QuotationDataContext = new QuotationDataContext())
                {
                    var _quotationTemplate = new QuotationTemplate();
                    _quotationTemplate.Description = txtTemplateDescription.Text;
                    QuotationDataContext.QuotationTemplates.InsertOnSubmit(_quotationTemplate);

                    var _curQuotation =
                        QuotationDataContext.Quotations.First(
                            Quotation => Quotation.ID == Convert.ToInt32(QuotationParameter));

                    foreach (var _quotationDetail in _curQuotation.QuotationDetails)
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

                        _quotationTemplate.QuotationTemplateDetails.Add(_quotationTemplateDetail);
                    }
                    QuotationDataContext.SubmitChanges();
                }
                txtTemplateDescription.Text = string.Empty;
            }
        }

        protected void dtvQuotation_ItemCreated(object sender, EventArgs e)
        {
            Quotation _currentQuotation = null;
            if (QuotationHeader.Key == 0)
            {
                _currentQuotation = (Quotation) dtvQuotation.DataItem;
            }
            else
            {
                using (var QuotationDataContext = new QuotationDataContext())
                {
                    _currentQuotation =
                        QuotationDataContext.Quotations.First(
                            Quotation => Quotation.ID == Convert.ToInt32(QuotationParameter));
                }
            }
            QuotationHeader = new KeyValuePair<int, string>(_currentQuotation.ID, _currentQuotation.Subject);
            lblQuotationHeader.Text = "Gestione preventivo No. " + QuotationHeader.Key + " (" +
                                      QuotationHeader.Value + ")";
            txtDescription.Text = _currentQuotation.Note;
        }

        protected void lbtLoadTemplate_Click(object sender, EventArgs e)
        {
            using (var _qc = new QuotationDataContext())
            {
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

                //Quotation _curQuotation = (Quotation)dtvQuotation.DataItem;
                var _curQuotation =
                    _qc.Quotations.First(Quotation => Quotation.ID == Convert.ToInt32(QuotationParameter));
                _qc.QuotationDetails.DeleteAllOnSubmit(_curQuotation.QuotationDetails);
                var _curQuotationTemplate =
                    _qc.QuotationTemplates.First(
                        QuotationTemplate => QuotationTemplate.ID == Convert.ToInt32(ddlTemplates.SelectedValue));

                try
                {
                    foreach (
                        var _quotationTemplateDetail in
                            _curQuotationTemplate.QuotationTemplateDetails)
                    {
                        var _quotationDetail = new QuotationDetail();

                        if (_quotationTemplateDetail.Save)
                        {
                            _quotationDetail.CommonKey = _quotationTemplateDetail.CommonKey;
                            _quotationDetail.MacroItemKey = _quotationTemplateDetail.MacroItemKey;
                            _quotationDetail.Cost =
                                Convert.ToDecimal(
                                    _macroItemTypes[_quotationTemplateDetail.MacroItemKey.ToString()].CostCalc);
                            _quotationDetail.ID_Quotation = Convert.ToInt32(QuotationParameter);
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
                                Convert.ToDecimal(_quotationDetail.Cost*(_quotationDetail.Percentage/100m)*
                                                  (_curQuotation.MarkUp/100m));
                            _quotationDetail.Quantity = _quotationTemplateDetail.Quantity;
                            _quotationDetail.Save = _quotationTemplateDetail.Save;
                            _quotationDetail.SelectPhase = _quotationTemplateDetail.SelectPhase;
                            _quotationDetail.SupplierCode =
                                _macroItemTypes[_quotationTemplateDetail.MacroItemKey.ToString()].INTERNAL_SUPPLIERCODE;
                            _quotationDetail.TypeCode = _quotationTemplateDetail.TypeCode;
                            _quotationDetail.UM = _quotationTemplateDetail.UM;
                        }
                        else
                        {
                            _quotationDetail.CommonKey = _quotationTemplateDetail.CommonKey;
                            _quotationDetail.MacroItemKey = _quotationTemplateDetail.MacroItemKey;
                            _quotationDetail.Cost =
                                Convert.ToDecimal(_pickingItemTypes[_quotationTemplateDetail.CommonKey.ToString()].Cost);
                            _quotationDetail.ID_Quotation = Convert.ToInt32(QuotationParameter);
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
                                Convert.ToDecimal(_quotationDetail.Cost*(_quotationDetail.Percentage/100m)*
                                                  (_curQuotation.MarkUp/100m));
                            _quotationDetail.Quantity = _quotationTemplateDetail.Quantity;
                            _quotationDetail.Save = _quotationTemplateDetail.Save;
                            _quotationDetail.SelectPhase = _quotationTemplateDetail.SelectPhase;
                            _quotationDetail.SupplierCode =
                                _pickingItemTypes[_quotationTemplateDetail.CommonKey.ToString()].SupplierCode;
                            _quotationDetail.TypeCode = _quotationTemplateDetail.TypeCode;
                            _quotationDetail.UM = _quotationTemplateDetail.UM;
                        }

                        _curQuotation.QuotationDetails.Add(_quotationDetail);
                    }
                    _qc.SubmitChanges();
                    Response.Redirect(
                        string.Format("{2}?{0}={1}", QuotationKey, QuotationParameter, QuotationConsolePage), true);
                }
                catch
                {
                    ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.ModelLoadingFailed);
                }
            }
        }

        protected void ddlTemplates_DataBound(object sender, EventArgs e)
        {
            ddlTemplates.Items.Insert(0, "Seleziona un modello...");
        }

        protected void lbtViewInputItems_Click(object sender, EventArgs e)
        {
            SwitchDependingControls(Mode.InputItems);
        }

        //protected void lbtViewCalculation_Click(object sender, EventArgs e)
        //{
        //    SwitchDependingControls(Mode.Calculation);
        //}

        //protected void lbtResultsCalculation_Click(object sender, EventArgs e)
        //{
        //    SwitchDependingControls(Mode.Results);
        //}

        //protected void grdQuotationCalculatedDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    if (e.NewPageIndex >= 0 && e.NewPageIndex < grdQuotationCalculatedDetails.PageCount)
        //    {
        //        grdQuotationCalculatedDetails.PageIndex = e.NewPageIndex;
        //        SwitchDependingControls(Mode.Calculation);
        //    }

        //}

        //protected void grdQuotationResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    if (e.NewPageIndex >= 0 && e.NewPageIndex < grdQuotationResults.PageCount)
        //    {
        //        grdQuotationResults.PageIndex = e.NewPageIndex;
        //        SwitchDependingControls(Mode.Results);
        //    }
        //}

        protected void grdQuotationDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex >= 0 && e.NewPageIndex < grdQuotationDetails.PageCount)
            {
                grdQuotationDetails.PageIndex = e.NewPageIndex;
                SwitchDependingControls(Mode.InputItems);
            }
        }

        //protected void ResultDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        //{
        //    e.Result = m_calculatedResults;
        //}

        //protected void CalculatedDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        //{
        //    e.Result = m_calculatedItems;
        //}

        protected void lbtPrintQuotation_Click(object sender, EventArgs e)
        {
            //Response.Redirect(string.Format("{2}?{0}={1}", QuotationKey, QuotationParameter, QuotationPrintPage), true);
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
            if (((RadioButton) dtvQuotation.FindControl("rbtQ1")).Checked)
            {
                _selectedQuantity = ((TextBox) dtvQuotation.FindControl("txtQ1")).Text;
            }
            if (((RadioButton) dtvQuotation.FindControl("rbtQ2")).Checked)
            {
                _selectedQuantity = ((TextBox) dtvQuotation.FindControl("txtQ2")).Text;
            }
            if (((RadioButton) dtvQuotation.FindControl("rbtQ3")).Checked)
            {
                _selectedQuantity = ((TextBox) dtvQuotation.FindControl("txtQ3")).Text;
            }
            if (((RadioButton) dtvQuotation.FindControl("rbtQ4")).Checked)
            {
                _selectedQuantity = ((TextBox) dtvQuotation.FindControl("txtQ4")).Text;
            }
            if (((RadioButton) dtvQuotation.FindControl("rbtQ5")).Checked)
            {
                _selectedQuantity = ((TextBox) dtvQuotation.FindControl("txtQ5")).Text;
            }
            return _selectedQuantity;
        }

        protected void grdQuotationDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var _txtQuantity = (TextBox) e.Row.Cells[8].FindControl("txtQuantity");
                _txtQuantity.Text = ((QuotationDetail) e.Row.DataItem).Quantity.ToString();

                var _row = e.Row;
                if (((QuotationDetail) e.Row.DataItem).PickingItem != null)
                {
                    if ((((QuotationDetail) e.Row.DataItem).Inserted == false ||
                         ((QuotationDetail) e.Row.DataItem).PickingItem.Inserted == false) &&
                        ((QuotationDetail) e.Row.DataItem).Save == false)
                    {
                        _row.ForeColor = Color.Red;
                    }

                    if (((QuotationDetail) e.Row.DataItem).PickingItem.IsObsolete(GlobalConfiguration))
                    {
                        var _dycCost = (DataControlFieldCell) e.Row.Cells[8];
                        _dycCost.ForeColor = Color.Red;
                        _dycCost.ToolTip = string.Format("Il costo di questa voce non è aggiornato da almeno {0} mesi",
                            GlobalConfiguration["PIMU"]);
                    }
                }
                if (((QuotationDetail) e.Row.DataItem).MacroItem != null)
                {
                    if ((((QuotationDetail) e.Row.DataItem).Inserted == false ||
                         ((QuotationDetail) e.Row.DataItem).MacroItem.Inserted == false) &&
                        ((QuotationDetail) e.Row.DataItem).Save)
                    {
                        _row.ForeColor = Color.Red;
                    }
                }
                m_voicePosition += 1;
                _txtQuantity.TabIndex = m_voicePosition;
                _txtQuantity.Attributes.Add("onchange",
                    "javascript:document.getElementById('ctl00_ContentPlaceHolder1_btnRecalc').disabled = false;this.value = this.value.replace('.',',');");

                if (((QuotationDetail) e.Row.DataItem).PickingItem == null &&
                    ((QuotationDetail) e.Row.DataItem).MacroItem == null)
                {
                    var _ibtEdit = (ImageButton) e.Row.Cells[8].FindControl("ibtEdit");
                    _ibtEdit.Visible = true;
                    _ibtEdit.Attributes.Add("onclick", "");
                }
            }
        }

        //protected void grdQuotationCalculatedDetails_PreRender(object sender, EventArgs e)
        //{
        //    grdQuotationCalculatedDetails.DataBind();
        //}

        //protected void grdQuotationResults_PreRender(object sender, EventArgs e)
        //{
        //    grdQuotationResults.DataBind();
        //}

        protected void ldsTotals_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            //e.Result = m_totals;
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
                    _qc.prc_LAB_MGet_LAB_TotalsByQuotationID(Convert.ToInt32(QuotationParameter))
                        .ToList();
                m_toolsTotals =
                    _qc.prc_LAB_MGet_LAB_ToolsTotalsByQuotationID(Convert.ToInt32(QuotationParameter))
                        .ToList();
                UpdatePivot(m_totals, m_toolsTotals);
                grdTotals.DataBind();
                grdToolsTotals.DataBind();
            }
        }

        protected void GridDataSource_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            e.ExceptionHandled = true;
        }

        protected void dtvQuotation_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            using (var QuotationDataContext = new QuotationDataContext())
            {
                var _curQuotation =
                    QuotationDataContext.Quotations.First(
                        Quotation => Quotation.ID == Convert.ToInt32(QuotationParameter));

                foreach (var _quotationDetail in _curQuotation.QuotationDetails)
                {
                    try
                    {
                        _quotationDetail.MarkUp = Convert.ToInt32(e.NewValues["MarkUp"]);
                        _quotationDetail.Price = _quotationDetail.Cost*
                                                 (Convert.ToDecimal(_quotationDetail.Percentage)/100m)*
                                                 (Convert.ToDecimal(_curQuotation.MarkUp)/100m);
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
                    QuotationDataContext.Quotations.First(
                        Quotation => Quotation.ID == Convert.ToInt32(QuotationParameter));

                var _cbk = new ComboKey(_menuPath);

                if (_cbk.Prefix == PICKINGITEM_PREFIX)
                {
                    var _toAddPickingItem = _pickingItems.First(pi => pi.ID == _cbk.CommonKey);
                    //RecursiveDependenciesAdd(_curQuotation, _toAddPickingItem, _pickingItems);
                    RecursiveDependenciesAdd(_curQuotation, _toAddPickingItem, _pickingItems, _macroItems);
                    _curQuotation.QuotationDetails.Add(CreateDetail(_curQuotation, _toAddPickingItem));
                }

                if (_cbk.Prefix == MACROITEM_PREFIX)
                {
                    var _toAddPickingItem = _macroItems.First(pi => pi.ID == _cbk.CommonKey);
                    //RecursiveDependenciesAdd(_curQuotation, _toAddPickingItem, _macroItems);
                    RecursiveDependenciesAdd(_curQuotation, _toAddPickingItem, _macroItems, _pickingItems);
                    _curQuotation.QuotationDetails.Add(CreateDetail(_curQuotation, _toAddPickingItem));
                }

                QuotationDataContext.SubmitChanges();
            }
        }

        //protected void RecursiveDependenciesAdd(Quotation quotation, PickingItem toAddPickingItem, IEnumerable<PickingItem> pickingItems)
        //{
        //    foreach (PickingItem _item in pickingItems)
        //    {
        //        if (toAddPickingItem.Link != null)
        //        {
        //            try
        //            {
        //                if (_item.ID == Convert.ToInt32(toAddPickingItem.Link))
        //                {
        //                    if (quotation.QuotationDetails.FirstOrDefault(qd => qd.CommonKey == _item.ID) == null)
        //                    {
        //                        quotation.QuotationDetails.Add(CreateDetail(quotation, _item));
        //                        RecursiveDependenciesAdd(quotation, _item, pickingItems);
        //                    }
        //                }
        //            }
        //            catch { }
        //        }
        //    }

        //}

        //protected void RecursiveDependenciesAdd(Quotation quotation, MacroItem toAddPickingItem, IEnumerable<MacroItem> pickingItems)
        //{
        //    foreach (MacroItem _item in pickingItems)
        //    {
        //        if (toAddPickingItem.Link != null)
        //        {
        //            try
        //            {
        //                if (_item.ID == Convert.ToInt32(toAddPickingItem.Link))
        //                {
        //                    if (quotation.QuotationDetails.FirstOrDefault(qd => qd.MacroItemKey == _item.ID) == null)
        //                    {
        //                        quotation.QuotationDetails.Add(CreateDetail(quotation, _item));
        //                        RecursiveDependenciesAdd(quotation, _item, pickingItems);
        //                    }
        //                }
        //            }
        //            catch { }
        //        }
        //    }

        //}

        //protected QuotationDetail CreateDetail(Quotation quotation, PickingItem pickingItem)
        //{

        //    QuotationDetail _toAddQuotationDetail = new QuotationDetail();
        //    _toAddQuotationDetail.ID_Quotation = quotation.ID; //Convert.ToInt32(QuotationParameter);
        //    _toAddQuotationDetail.Cost = pickingItem.Cost;
        //    _toAddQuotationDetail.MarkUp = Convert.ToInt32(quotation.MarkUp);
        //    try
        //    { _toAddQuotationDetail.Price = pickingItem.Cost * Convert.ToDecimal(pickingItem.PercentageAuto / 100m) * Convert.ToDecimal(quotation.MarkUp / 100m); }
        //    catch
        //    { _toAddQuotationDetail.Price = pickingItem.Cost * Convert.ToDecimal(pickingItem.PercentageAuto / 100m); }
        //    _toAddQuotationDetail.Multiply = pickingItem.Multiply;
        //    _toAddQuotationDetail.Percentage = pickingItem.PercentageAuto;
        //    _toAddQuotationDetail.SupplierCode = pickingItem.SupplierCode;
        //    _toAddQuotationDetail.TypeCode = pickingItem.TypeCode; //Convert.ToInt32(TypeParameter);
        //    _toAddQuotationDetail.UM = pickingItem.UM;
        //    _toAddQuotationDetail.ItemTypeDescription = pickingItem.ItemDescription;
        //    _toAddQuotationDetail.ItemTypeCode = pickingItem.ItemTypeCode;
        //    _toAddQuotationDetail.CommonKey = pickingItem.ID;
        //    _toAddQuotationDetail.Position = pickingItem.Order;
        //    _toAddQuotationDetail.Inserted = pickingItem.Inserted;

        //    return _toAddQuotationDetail;

        //}

        //protected QuotationDetail CreateDetail(Quotation quotation, MacroItem pickingItem)
        //{

        //    QuotationDetail _toAddQuotationDetail = new QuotationDetail();
        //    _toAddQuotationDetail.ID_Quotation = quotation.ID; //Convert.ToInt32(QuotationParameter);
        //    _toAddQuotationDetail.Cost = pickingItem.CostCalc;
        //    _toAddQuotationDetail.MarkUp = Convert.ToInt32(quotation.MarkUp);
        //    try
        //    { _toAddQuotationDetail.Price = pickingItem.CostCalc * Convert.ToDecimal(pickingItem.PercentageCalc / 100m) * Convert.ToDecimal(quotation.MarkUp / 100m); }
        //    catch
        //    { _toAddQuotationDetail.Price = pickingItem.CostCalc * Convert.ToDecimal(pickingItem.PercentageCalc / 100m); }
        //    _toAddQuotationDetail.Multiply = pickingItem.Multiply;
        //    _toAddQuotationDetail.Percentage = pickingItem.PercentageCalc;
        //    _toAddQuotationDetail.SupplierCode = INTERNAL_SUPPLIERCODE;
        //    _toAddQuotationDetail.TypeCode = pickingItem.TypeCode; //Convert.ToInt32(TypeParameter);
        //    _toAddQuotationDetail.UM = pickingItem.UM;
        //    _toAddQuotationDetail.ItemTypeDescription = "[" + pickingItem.MacroItemDescription + "]";
        //    _toAddQuotationDetail.ItemTypeCode = pickingItem.ItemTypeCode;
        //    _toAddQuotationDetail.MacroItemKey = pickingItem.ID;
        //    _toAddQuotationDetail.Position = pickingItem.Order;
        //    _toAddQuotationDetail.Inserted = pickingItem.Inserted;
        //    _toAddQuotationDetail.Save = true; // il campo Save identifica se la riga è una macrovoce!

        //    return _toAddQuotationDetail;

        //}

        protected void grdQuotationDetails_DataBound(object sender, EventArgs e)
        {
            //int rowCount = grdQuotationDetails.Rows.Count;
            //int i = 0;
            //for (i = 0; i < rowCount; i++)
            //{
            //    TextBox _tabStop = (TextBox)grdQuotationDetails.Rows[i].Cells[6].Controls[1];
            //    _tabStop.TabIndex = (short)(i + 1);
            //}
            if (grdQuotationDetails.Rows.Count > 0)
            {
                var _tabStart = (TextBox) grdQuotationDetails.Rows[0].Cells[7].Controls[1];
                _tabStart.Attributes.Add("onfocus",
                    "this.select();"
                    );
                var _tabStop =
                    (TextBox) grdQuotationDetails.Rows[grdQuotationDetails.Rows.Count - 1].Cells[7].Controls[1];
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
            var _qc = (QuotationDataContext) table.CreateContext();
            //e.Result = _qc.QuotationDetails.OrderBy(qd => qd.Type.Order).ThenBy(qd => qd.ItemType.Order).ThenBy(qd => qd.Position);
            e.Result = _qc.QuotationDetails.OrderBy(qd => qd.Position);
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            //using (QuotationDataContext _qc = new QuotationDataContext())
            //{
            //    args.IsValid = _qc.QuotationTemplates.FirstOrDefault(t => t.Description.Replace(" ", "").Contains(args.Value.Replace(" ", ""))) == null;
            //}
        }

        protected void CustomValidator3_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            //using (QuotationDataContext _qc = new QuotationDataContext())
            //{
            //    args.IsValid = _qc.Quotations.Where(q => q.CustomerCode ==
            //        _qc.Quotations.Single(qt => qt.ID == Convert.ToInt32(QuotationParameter)).CustomerCode)
            //        .FirstOrDefault(t => t.Subject.Replace(" ", "").Contains(args.Value.Replace(" ", ""))) == null;
            //}
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
                        _qc.Quotations.First(Quotation => Quotation.ID == Convert.ToInt32(QuotationParameter));
                    _qc.QuotationDetails.DeleteAllOnSubmit(_curQuotation.QuotationDetails);
                    _qc.SubmitChanges();
                    Response.Redirect(
                        string.Format("{2}?{0}={1}", QuotationKey, QuotationParameter, QuotationConsolePage), true);
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
                        _destQuotation = SaveAsNewUpdatedQuotation(QuotationDataContext, txtQuotationDescription.Text);
                    else
                        _destQuotation = SaveAsNewQuotation(QuotationDataContext, txtQuotationDescription.Text);
                }
                txtQuotationDescription.Text = string.Empty;
                Response.Redirect(string.Format("~/QuotationConsole.aspx?{0}={1}", QuotationKey,
                    _destQuotation));
            }
        }

        protected void dtvQuotation_DataBound(object sender, EventArgs e)
        {
            var _txtQ1 = (TextBox) dtvQuotation.FindControl("txtQ1");
            var _txtQ2 = (TextBox) dtvQuotation.FindControl("txtQ2");
            var _txtQ3 = (TextBox) dtvQuotation.FindControl("txtQ3");
            var _txtQ4 = (TextBox) dtvQuotation.FindControl("txtQ4");
            var _txtQ5 = (TextBox) dtvQuotation.FindControl("txtQ5");
            var _txtMarkUp = (TextBox) dtvQuotation.FindControl("txtMarkUp");


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

                if (Convert.ToInt32(_txtQ3.Text) == -1 && Convert.ToInt32(_txtQ4.Text) == -1 &&
                    Convert.ToInt32(_txtQ5.Text) == -1)
                {
                    _txtQ3.Enabled = _txtQ4.Enabled = _txtQ5.Enabled = false;
                    var _CustomValidator2 = (CustomValidator) dtvQuotation.FindControl("CustomValidator2");
                    _CustomValidator2.Enabled = false;
                }
            }
            if (txtDescription != null)
            {
                txtDescription.Attributes.Add("onchange",
                    "javascript:document.getElementById('ctl00_ContentPlaceHolder1_btnRecalc').disabled = false;");
            }
        }

        protected void lbtAddUnmanagedItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (var QuotationDataContext = new QuotationDataContext())
                {
                    var _curQuotation =
                        QuotationDataContext.Quotations.First(
                            Quotation => Quotation.ID == Convert.ToInt32(QuotationParameter));
                    var _unmanagedQuotationDetail = new QuotationDetail();
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
                    _curQuotation.QuotationDetails.Add(_unmanagedQuotationDetail);
                    QuotationDataContext.SubmitChanges();
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
                    //if (_markUp < 120) { throw new Exception(); }
                }
            }
            catch
            {
                _markUp = 120;
            }
            e.NewValues["MarkUp"] = _markUp;
            e.NewValues["Note"] = txtDescription.Text;
        }

        protected void lbtNewQuotation_Click(object sender, EventArgs e)
        {
            var _result = new QuotationService().DelLocks(WebUser.Member.ProviderUserKey.ToString(), SessionTimeStamp);
            Response.Redirect("~/QuotationInsert.aspx");
        }

        protected enum Mode
        {
            InputItems,
            Calculation,
            Results
        }
    }
}