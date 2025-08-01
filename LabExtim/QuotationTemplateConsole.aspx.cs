using System;
using System.Collections.Generic;
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
    public partial class QuotationTemplateConsole : QuotationController
    {
        //protected List<SPQDetailCalculation> m_calculatedItems;
        //protected List<SPQDetailResult> m_calculatedResults;
        //protected List<SPQDetailTotal> m_totals;
        //protected List<QuotationTotal> m_totalsPivot;

        protected Mode CurQuotationConsoleMode
        {
            get
            {
                if (Session["CurQuotationTemplateConsoleMode"] == null)
                {
                    return Mode.InputItems;
                }
                return (Mode)Session["CurQuotationTemplateConsoleMode"];
            }
            set { Session["CurQuotationTemplateConsoleMode"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicDataManager1.RegisterControl(dtvQuotation);
            DynamicDataManager1.RegisterControl(grdQuotationDetails);
            //DynamicDataManager1.RegisterControl(grdQuotationCalculatedDetails);
            //DynamicDataManager1.RegisterControl(grdQuotationResults);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //InitSelectionData();

            var _result = new QuotationService().SetLock(2, Convert.ToInt32(QuotationTemplateParameter),
                WebUser.Member.ProviderUserKey.ToString(), SessionTimeStamp);

            if (!IsPostBack)
            {
                LoadHeaderMenu(mnuOperations1, MenuType.MenuOperations,1);
                LoadHeaderMenu(mnuOperations2, MenuType.MenuOperations, 2);
                LoadHeaderMenu(mnuQuotationTemplates, MenuType.MenuQuotationTemplates, -1);
                LoadHeaderMenu(mnuQuotationTemplatesAdd, MenuType.MenuQuotationTemplates, -1);
                FillControls();
                //SwitchDependingControls(CurQuotationConsoleMode);
            }
            FillDependingControls(CurQuotationConsoleMode);
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
            //using (QuotationDataContext _qc = new QuotationDataContext())
            //{

            for (var i = 0; i < grdQuotationDetails.Rows.Count; i++)
            {
                try
                {
                    var _txtQuantity = (TextBox)grdQuotationDetails.Rows[i].Cells[7].FindControl("txtQuantity");
                    var _litID = (Literal)grdQuotationDetails.Rows[i].Cells[1].FindFieldTemplate("ID").Controls[0];
                    var _toModifyId = Convert.ToInt32(_litID.Text);
                    var _toUpdateQuotationTemplateDetail =
                        qc.QuotationTemplateDetails.Single(
                            QuotationTemplateDetail => QuotationTemplateDetail.ID == _toModifyId);
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
                    _toUpdateQuotationTemplateDetail.Quantity = _quantity;
                }
                catch
                {
                }
            }
            qc.SubmitChanges();

            //}
        }

        private void FillControls()
        {
            //using (var QuotationDataContext =
            //    new QuotationDataContext())
            //{
            //    IEnumerable<QuotationTemplate> _quotationTemplates =
            //        QuotationDataContext.QuotationTemplates;
            //    ddlTemplates.DataSource = _quotationTemplates;
            //    ddlTemplates.DataTextField = "Description";
            //    ddlTemplates.DataValueField = "ID";
            //    ddlTemplates.DataBind();
            //}
        }

        protected void FillDependingControls(Mode mode)
        {
            var filterString = "ID_QuotationTemplate = @ID";

            GridDataSource.WhereParameters.Clear();
            //GridDataSource.WhereParameters.Add("ID", DbType.Int32, SelectionData.ToString());
            GridDataSource.WhereParameters.Add("ID", DbType.Int32, QuotationTemplateParameter);
            GridDataSource.Where = filterString;
            GridDataSource.AutoGenerateWhereClause = false;
            //GridDataSource.DataBind();
            //WriteQuantities();

            using (var _qc = new QuotationDataContext())
            {
                WriteQuantities(_qc);
                //m_calculatedItems = _qc.prc_LAB_MGet_LAB_CalculatedDetailsByQuotationID(Convert.ToInt32(QuotationParameter)).ToList<SPQDetailCalculation>();
                //m_calculatedResults = _qc.prc_LAB_MGet_LAB_CalculatedResultsByQuotationID(Convert.ToInt32(QuotationParameter)).ToList<SPQDetailResult>();
                //m_totals = _qc.prc_LAB_MGet_LAB_TotalsByQuotationID(Convert.ToInt32(QuotationParameter)).ToList<SPQDetailTotal>();
                //UpdatePivot(m_totals);


                try
                {
                    var _curQuotationTemplate =
                        _qc.QuotationTemplates.First(
                            QuotationTemplate => QuotationTemplate.ID == Convert.ToInt32(QuotationTemplateParameter));

                    foreach (
                        var _quotationTemplateDetail in
                            _curQuotationTemplate.QuotationTemplateDetails)
                    {
                        //PickingItem _calledPickingItem = _qc.PickingItems.SingleOrDefault(pi => pi.ID == _quotationTemplateDetail.CommonKey);
                        //if (_calledPickingItem.Inserted == false)
                        //{
                        //    ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.ModelObsoleteItem);
                        //    break;
                        //}
                        if (_quotationTemplateDetail.Save == false)
                        {
                            if (_quotationTemplateDetail.Inserted == false ||
                                _quotationTemplateDetail.PickingItem.Inserted == false)
                            {
                                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.ObsoleteItem);
                                break;
                            }
                        }
                        else
                        {
                            if (_quotationTemplateDetail.Inserted == false ||
                                _quotationTemplateDetail.MacroItem.Inserted == false)
                            {
                                ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.ObsoleteItem);
                                break;
                            }
                        }
                    }
                }
                catch
                {
                    ToggleSuccessMessage(false, lblSuccess, LabExtimErrorType.ItemLoadingFailed);
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

        protected void dtvQuotation_ItemCreated(object sender, EventArgs e)
        {
            QuotationTemplate _currentQuotationTemplate = null;
            if (QuotationTemplateHeader.Key == 0)
            {
                _currentQuotationTemplate = (QuotationTemplate)dtvQuotation.DataItem;
            }
            else
            {
                using (var QuotationDataContext = new QuotationDataContext())
                {
                    _currentQuotationTemplate =
                        QuotationDataContext.QuotationTemplates.First(
                            QuotationTemplate => QuotationTemplate.ID == Convert.ToInt32(QuotationTemplateParameter));
                }
            }
            QuotationTemplateHeader = new KeyValuePair<int, string>(_currentQuotationTemplate.ID,
                _currentQuotationTemplate.Description);
            lblQuotationHeader.Text = "Gestione modello di preventivo No. " + QuotationTemplateHeader.Key +
                                      " (" + QuotationTemplateHeader.Value + ")";

            Cache.Remove(MenuType.MenuQuotationTemplates.ToString());
            LoadHeaderMenu(mnuQuotationTemplates, MenuType.MenuQuotationTemplates, -1);

        }

        //protected void lbtLoadTemplate_Click(object sender, EventArgs e)
        //{
        //    using (var QuotationDataContext = new QuotationDataContext())
        //    {
        //        var _pickingItemTypes =
        //            QuotationDataContext.PickingItems.Select(
        //                pi =>
        //                    new
        //                    {
        //                        pi.ID,
        //                        pi.Cost,
        //                        pi.PercentageAuto,
        //                        pi.Inserted,
        //                        pi.Multiply,
        //                        pi.SupplierCode,
        //                        pi.Order,
        //                        pi.ItemDescription
        //                    }).ToDictionary(pi => pi.ID.ToString());
        //        var _macroItemTypes =
        //            QuotationDataContext.MacroItems.Select(
        //                pi =>
        //                    new
        //                    {
        //                        pi.ID,
        //                        pi.CostCalc,
        //                        pi.PercentageCalc,
        //                        pi.Inserted,
        //                        pi.Multiply,
        //                        INTERNAL_SUPPLIERCODE,
        //                        pi.Order,
        //                        pi.MacroItemDescription
        //                    }).ToDictionary(pi => pi.ID.ToString());

        //        //Quotation _curQuotation = (Quotation)dtvQuotation.DataItem;
        //        var _curQuotationTemplate =
        //            QuotationDataContext.QuotationTemplates.First(
        //                QuotationTemplate => QuotationTemplate.ID == Convert.ToInt32(QuotationTemplateParameter));
        //        var _curQuotationTemplateGet =
        //            QuotationDataContext.QuotationTemplates.First(
        //                QuotationTemplate => QuotationTemplate.ID == Convert.ToInt32(ddlTemplates.SelectedValue));

        //        if (_curQuotationTemplate.ID != _curQuotationTemplateGet.ID)
        //        {
        //            QuotationDataContext.QuotationTemplateDetails.DeleteAllOnSubmit(
        //                _curQuotationTemplate.QuotationTemplateDetails);
        //            QuotationDataContext.SubmitChanges();

        //            foreach (
        //                var _quotationTemplateDetailGet in
        //                    _curQuotationTemplateGet.QuotationTemplateDetails)
        //            {
        //                var _quotationTemplateDetail = new QuotationTemplateDetail();

        //                //_quotationTemplateDetail.CommonKey = _quotationTemplateDetailGet.CommonKey;
        //                //_quotationTemplateDetail.MacroItemKey = _quotationTemplateDetailGet.MacroItemKey;
        //                //_quotationTemplateDetail.Cost = _quotationTemplateDetailGet.Cost;
        //                //_quotationTemplateDetail.ID_QuotationTemplate = Convert.ToInt32(QuotationTemplateParameter);
        //                //_quotationTemplateDetail.Inserted = _quotationTemplateDetailGet.Inserted;
        //                //_quotationTemplateDetail.ItemTypeCode = _quotationTemplateDetailGet.ItemTypeCode;
        //                //_quotationTemplateDetail.ItemTypeDescription = _quotationTemplateDetailGet.ItemTypeDescription;
        //                //_quotationTemplateDetail.MarkUp = _quotationTemplateDetailGet.MarkUp;
        //                //_quotationTemplateDetail.Multiply = _quotationTemplateDetailGet.Multiply;
        //                //_quotationTemplateDetail.Percentage = _quotationTemplateDetailGet.Percentage;
        //                //_quotationTemplateDetail.Position = _quotationTemplateDetailGet.Position;
        //                //_quotationTemplateDetail.Price = _quotationTemplateDetailGet.Price;
        //                //_quotationTemplateDetail.Quantity = _quotationTemplateDetailGet.Quantity;
        //                //_quotationTemplateDetail.Save = _quotationTemplateDetailGet.Save;
        //                //_quotationTemplateDetail.SelectPhase = _quotationTemplateDetailGet.SelectPhase;
        //                //_quotationTemplateDetail.SupplierCode = _quotationTemplateDetailGet.SupplierCode;
        //                //_quotationTemplateDetail.TypeCode = _quotationTemplateDetailGet.TypeCode;
        //                //_quotationTemplateDetail.UM = _quotationTemplateDetailGet.UM;

        //                if (_quotationTemplateDetailGet.Save)
        //                {
        //                    _quotationTemplateDetail.CommonKey = _quotationTemplateDetailGet.CommonKey;
        //                    _quotationTemplateDetail.MacroItemKey = _quotationTemplateDetailGet.MacroItemKey;
        //                    _quotationTemplateDetail.Cost =
        //                        Convert.ToDecimal(
        //                            _macroItemTypes[_quotationTemplateDetailGet.MacroItemKey.ToString()].CostCalc);
        //                    _quotationTemplateDetail.ID_QuotationTemplate = Convert.ToInt32(QuotationTemplateParameter);
        //                    _quotationTemplateDetail.Inserted =
        //                        _macroItemTypes[_quotationTemplateDetailGet.MacroItemKey.ToString()].Inserted;
        //                    _quotationTemplateDetail.ItemTypeCode = _quotationTemplateDetailGet.ItemTypeCode;
        //                    _quotationTemplateDetail.ItemTypeDescription = "[" +
        //                                                                   _macroItemTypes[
        //                                                                       _quotationTemplateDetailGet.MacroItemKey
        //                                                                           .ToString()].MacroItemDescription +
        //                                                                   "]";
        //                    _quotationTemplateDetail.MarkUp = Convert.ToInt32(_quotationTemplateDetailGet.MarkUp);
        //                    _quotationTemplateDetail.Multiply =
        //                        _macroItemTypes[_quotationTemplateDetailGet.MacroItemKey.ToString()].Multiply;
        //                    _quotationTemplateDetail.Percentage =
        //                        Convert.ToInt32(
        //                            _macroItemTypes[_quotationTemplateDetailGet.MacroItemKey.ToString()].PercentageCalc);
        //                    _quotationTemplateDetail.Position =
        //                        _macroItemTypes[_quotationTemplateDetailGet.MacroItemKey.ToString()].Order;
        //                    _quotationTemplateDetail.Price =
        //                        Convert.ToDecimal(_quotationTemplateDetailGet.Cost *
        //                                          (_quotationTemplateDetailGet.Percentage / 100m) *
        //                                          (_quotationTemplateDetailGet.MarkUp / 100m));
        //                    _quotationTemplateDetail.Quantity = _quotationTemplateDetailGet.Quantity;
        //                    _quotationTemplateDetail.Save = _quotationTemplateDetailGet.Save;
        //                    _quotationTemplateDetail.SelectPhase = _quotationTemplateDetailGet.SelectPhase;
        //                    _quotationTemplateDetail.SupplierCode =
        //                        _macroItemTypes[_quotationTemplateDetailGet.MacroItemKey.ToString()]
        //                            .INTERNAL_SUPPLIERCODE;
        //                    _quotationTemplateDetail.TypeCode = _quotationTemplateDetailGet.TypeCode;
        //                    _quotationTemplateDetail.UM = _quotationTemplateDetailGet.UM;
        //                }
        //                else
        //                {
        //                    _quotationTemplateDetail.CommonKey = _quotationTemplateDetailGet.CommonKey;
        //                    _quotationTemplateDetail.MacroItemKey = _quotationTemplateDetailGet.MacroItemKey;
        //                    _quotationTemplateDetail.Cost =
        //                        Convert.ToDecimal(
        //                            _pickingItemTypes[_quotationTemplateDetailGet.CommonKey.ToString()].Cost);
        //                    _quotationTemplateDetail.ID_QuotationTemplate = Convert.ToInt32(QuotationTemplateParameter);
        //                    _quotationTemplateDetail.Inserted =
        //                        _pickingItemTypes[_quotationTemplateDetailGet.CommonKey.ToString()].Inserted;
        //                    _quotationTemplateDetail.ItemTypeCode = _quotationTemplateDetailGet.ItemTypeCode;
        //                    _quotationTemplateDetail.ItemTypeDescription =
        //                        _pickingItemTypes[_quotationTemplateDetailGet.CommonKey.ToString()].ItemDescription;
        //                    _quotationTemplateDetail.MarkUp = Convert.ToInt32(_quotationTemplateDetailGet.MarkUp);
        //                    _quotationTemplateDetail.Multiply =
        //                        _pickingItemTypes[_quotationTemplateDetailGet.CommonKey.ToString()].Multiply;
        //                    _quotationTemplateDetail.Percentage =
        //                        Convert.ToInt32(
        //                            _pickingItemTypes[_quotationTemplateDetailGet.CommonKey.ToString()].PercentageAuto);
        //                    _quotationTemplateDetail.Position =
        //                        _pickingItemTypes[_quotationTemplateDetailGet.CommonKey.ToString()].Order;
        //                    _quotationTemplateDetail.Price =
        //                        Convert.ToDecimal(_quotationTemplateDetailGet.Cost *
        //                                          (_quotationTemplateDetailGet.Percentage / 100m) *
        //                                          (_quotationTemplateDetailGet.MarkUp / 100m));
        //                    _quotationTemplateDetail.Quantity = _quotationTemplateDetailGet.Quantity;
        //                    _quotationTemplateDetail.Save = _quotationTemplateDetailGet.Save;
        //                    _quotationTemplateDetail.SelectPhase = _quotationTemplateDetailGet.SelectPhase;
        //                    _quotationTemplateDetail.SupplierCode =
        //                        _pickingItemTypes[_quotationTemplateDetailGet.CommonKey.ToString()].SupplierCode;
        //                    _quotationTemplateDetail.TypeCode = _quotationTemplateDetailGet.TypeCode;
        //                    _quotationTemplateDetail.UM = _quotationTemplateDetailGet.UM;
        //                }

        //                _curQuotationTemplate.QuotationTemplateDetails.Add(_quotationTemplateDetail);
        //            }
        //            QuotationDataContext.SubmitChanges();
        //            Response.Redirect(
        //                string.Format("{2}?{0}={1}", QuotationTemplateKey, _curQuotationTemplateGet.ID,
        //                    QuotationTemplateConsolePage), true);
        //        }
        //    }
        //}

        //protected void ddlTemplates_DataBound(object sender, EventArgs e)
        //{
        //    ddlTemplates.Items.Insert(0, "Seleziona un modello...");
        //}

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

        protected void ResultDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            //e.Result = m_calculatedResults;
        }

        protected void CalculatedDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            //e.Result = m_calculatedItems;
        }

        protected void lbtPrintQuotationTechnical_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                string.Format("{2}?{0}={1}&{3}={4}", QuotationTemplateKey, QuotationTemplateParameter,
                    QuotationTemplatePrintPage, ReportTypeKey, 1), true);
        }

        protected void grdQuotationDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var _txtQuantity = (TextBox)e.Row.Cells[7].FindControl("txtQuantity");
                _txtQuantity.Text = ((QuotationTemplateDetail)e.Row.DataItem).Quantity.ToString();

                var _row = e.Row;
                using (var _qc = new QuotationDataContext())
                {
                    try
                    {
                        //PickingItem _calledPickingItem = _qc.PickingItems.SingleOrDefault(pi => pi.ID == ((QuotationTemplateDetail)e.Row.DataItem).CommonKey);

                        //if (_calledPickingItem.Inserted == false)
                        //{
                        //    _row.ForeColor = System.Drawing.Color.Red;
                        //}
                        if (((QuotationTemplateDetail)e.Row.DataItem).PickingItem != null)
                        {
                            if ((((QuotationTemplateDetail)e.Row.DataItem).Inserted == false ||
                                 ((QuotationTemplateDetail)e.Row.DataItem).PickingItem.Inserted == false) &&
                                ((QuotationTemplateDetail)e.Row.DataItem).Save == false)
                            {
                                _row.ForeColor = Color.Red;
                            }
                        }

                        if (((QuotationTemplateDetail)e.Row.DataItem).MacroItem != null)
                        {
                            if ((((QuotationTemplateDetail)e.Row.DataItem).Inserted == false ||
                                 ((QuotationTemplateDetail)e.Row.DataItem).MacroItem.Inserted == false) &&
                                ((QuotationTemplateDetail)e.Row.DataItem).Save)
                            {
                                _row.ForeColor = Color.Red;
                            }
                        }

                        if (((QuotationTemplateDetail)e.Row.DataItem).PickingItem == null && ((QuotationTemplateDetail)e.Row.DataItem).MacroItem == null)
                        {
                            throw new Exception("");
                        }
                    }
                    catch
                    {
                        _row.ForeColor = Color.Blue;
                    }
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
            //e.Result = m_totalsPivot;
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
            //using (QuotationDataContext _qc = new QuotationDataContext())
            //{
            //    m_totals = _qc.prc_LAB_MGet_LAB_TotalsByQuotationID(Convert.ToInt32(QuotationParameter)).ToList<SPQDetailTotal>();
            //    UpdatePivot(m_totals);
            //    grdTotals.DataBind();
            //}
        }

        protected void GridDataSource_Deleted(object sender, LinqDataSourceStatusEventArgs e)
        {
            e.ExceptionHandled = true;
        }

        protected void dtvQuotation_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            //using (QuotationDataContext QuotationDataContext = new QuotationDataContext())
            //{
            //    Quotation _curQuotation = QuotationDataContext.Quotations.First(Quotation => Quotation.ID == Convert.ToInt32(QuotationParameter));

            //    foreach (QuotationDetail _quotationDetail in _curQuotation.QuotationDetails)
            //    {
            //        try
            //        {
            //            _quotationDetail.MarkUp = Convert.ToInt32(e.NewValues["MarkUp"]);
            //            _quotationDetail.Price = _quotationDetail.Cost *
            //                (Convert.ToDecimal(_quotationDetail.Percentage) / 100m) *
            //                (Convert.ToDecimal(_curQuotation.MarkUp) / 100m);
            //        }
            //        catch
            //        { _quotationDetail.MarkUp = 100; }
            //    }
            //    QuotationDataContext.SubmitChanges();
            //}
            Cache.Remove(MenuType.MenuQuotationTemplates.ToString());
            LoadHeaderMenu(mnuQuotationTemplates, MenuType.MenuQuotationTemplates, -1);
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
                    QuotationDataContext.QuotationTemplates.First(
                        QuotationTemplate => QuotationTemplate.ID == Convert.ToInt32(QuotationTemplateParameter));

                var _cbk = new ComboKey(_menuPath);

                if (_cbk.Prefix == PICKINGITEM_PREFIX)
                {
                    var _toAddPickingItem = _pickingItems.First(pi => pi.ID == _cbk.CommonKey);
                    //RecursiveDependenciesAdd(_curQuotation, _toAddPickingItem, _pickingItems);
                    RecursiveDependenciesAdd(_curQuotation, _toAddPickingItem, _pickingItems, _macroItems);
                    _curQuotation.QuotationTemplateDetails.Add(CreateDetail(_curQuotation, _toAddPickingItem));
                }

                if (_cbk.Prefix == MACROITEM_PREFIX)
                {
                    var _toAddPickingItem = _macroItems.First(pi => pi.ID == _cbk.CommonKey);
                    //RecursiveDependenciesAdd(_curQuotation, _toAddPickingItem, _macroItems);
                    RecursiveDependenciesAdd(_curQuotation, _toAddPickingItem, _macroItems, _pickingItems);
                    _curQuotation.QuotationTemplateDetails.Add(CreateDetail(_curQuotation, _toAddPickingItem));
                }

                //PickingItem _toAddPickingItem = _pickingItems.First(pi => pi.ID == Convert.ToInt32(_menuPath[_menuPath.Length - 1]));
                //foreach (PickingItem _item in _pickingItems)
                //{
                //    if (_toAddPickingItem.Link != null)
                //    {
                //        try
                //        {
                //            if (_item.ID == Convert.ToInt32(_toAddPickingItem.Link))
                //            {
                //                _curQuotationTemplate.QuotationTemplateDetails.Add(CreateDetail(_curQuotationTemplate, _item));
                //            }
                //        }
                //        catch { }
                //    }
                //}
                //_curQuotationTemplate.QuotationTemplateDetails.Add(CreateDetail(_curQuotationTemplate, _toAddPickingItem));

                QuotationDataContext.SubmitChanges();
            }
        }

        //protected QuotationTemplateDetail CreateDetail(QuotationTemplate quotationTemplate, PickingItem pickingItem)
        //{

        //    QuotationTemplateDetail _toAddQuotationTemplateDetail = new QuotationTemplateDetail();
        //    _toAddQuotationTemplateDetail.ID_QuotationTemplate = quotationTemplate.ID; //Convert.ToInt32(QuotationParameter);
        //    _toAddQuotationTemplateDetail.Cost = pickingItem.Cost;
        //    _toAddQuotationTemplateDetail.MarkUp = 100;
        //    try
        //    { _toAddQuotationTemplateDetail.Price = Convert.ToDecimal(pickingItem.Cost * (pickingItem.PercentageAuto / 100m)); }
        //    catch
        //    { _toAddQuotationTemplateDetail.Price = Convert.ToDecimal(pickingItem.Cost * (pickingItem.PercentageAuto / 100m)); }
        //    _toAddQuotationTemplateDetail.Multiply = pickingItem.Multiply;
        //    _toAddQuotationTemplateDetail.Percentage = pickingItem.PercentageAuto;
        //    _toAddQuotationTemplateDetail.SupplierCode = pickingItem.SupplierCode;
        //    _toAddQuotationTemplateDetail.TypeCode = pickingItem.TypeCode; //Convert.ToInt32(TypeParameter);
        //    _toAddQuotationTemplateDetail.UM = pickingItem.UM;
        //    _toAddQuotationTemplateDetail.ItemTypeDescription = pickingItem.ItemDescription;
        //    _toAddQuotationTemplateDetail.ItemTypeCode = pickingItem.ItemTypeCode;
        //    _toAddQuotationTemplateDetail.CommonKey = pickingItem.ID;
        //    _toAddQuotationTemplateDetail.Position = pickingItem.Order;
        //    _toAddQuotationTemplateDetail.Inserted = pickingItem.Inserted;

        //    return _toAddQuotationTemplateDetail;

        //}


        protected void RecursiveDependenciesAdd(QuotationTemplate quotation, PickingItem toAddPickingItem,
           IEnumerable<PickingItem> pickingItems, IEnumerable<MacroItem> macroItems)
        {
            foreach (var _item in pickingItems)
            {
                if (toAddPickingItem.Link != null)
                {
                    try
                    {
                        if (_item.ID == Convert.ToInt32(toAddPickingItem.Link))
                        {
                            if (quotation.QuotationTemplateDetails.FirstOrDefault(qd => qd.CommonKey == _item.ID) == null)
                            {
                                quotation.QuotationTemplateDetails.Add(CreateDetail(quotation, _item));
                                //RecursiveDependenciesAdd(quotation, _item, pickingItems); 20160612
                                RecursiveDependenciesAdd(quotation, _item, pickingItems, macroItems);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
            foreach (var _item in macroItems)
            {
                try
                {
                    //if (Convert.ToInt32(_item.PILink) == toAddPickingItem.ID)
                    if (_item.ID == Convert.ToInt32(toAddPickingItem.MILink))
                    {
                        if (quotation.QuotationTemplateDetails.FirstOrDefault(qd => qd.MacroItemKey == _item.ID) == null)
                        {
                            quotation.QuotationTemplateDetails.Add(CreateDetail(quotation, _item));
                            //RecursiveDependenciesAdd(quotation, _item, macroItems); 20160612
                            RecursiveDependenciesAdd(quotation, _item, macroItems, pickingItems);
                        }
                    }
                }
                catch
                {
                }
            }
        }

        protected void RecursiveDependenciesAdd(QuotationTemplate quotation, MacroItem toAddMacroItem,
            IEnumerable<MacroItem> macroItems, IEnumerable<PickingItem> pickingItems)
        {
            foreach (var _item in macroItems)
            {
                if (toAddMacroItem.Link != null)
                {
                    try
                    {
                        if (_item.ID == Convert.ToInt32(toAddMacroItem.Link))
                        {
                            if (quotation.QuotationTemplateDetails.FirstOrDefault(qd => qd.MacroItemKey == _item.ID) == null)
                            {
                                quotation.QuotationTemplateDetails.Add(CreateDetail(quotation, _item));
                                //RecursiveDependenciesAdd(quotation, _item, macroItems); 20160612
                                RecursiveDependenciesAdd(quotation, _item, macroItems, pickingItems);

                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
            foreach (var _item in pickingItems)
            {
                try
                {
                    //if (Convert.ToInt32(_item.MILink) == toAddMacroItem.ID)
                    if (_item.ID == Convert.ToInt32(toAddMacroItem.PILink))
                    {
                        if (quotation.QuotationTemplateDetails.FirstOrDefault(qd => qd.CommonKey == _item.ID) == null)
                        {
                            quotation.QuotationTemplateDetails.Add(CreateDetail(quotation, _item));
                            //RecursiveDependenciesAdd(quotation, _item, pickingItems); 20160612
                            RecursiveDependenciesAdd(quotation, _item, pickingItems, macroItems);

                        }
                    }
                }
                catch
                {
                }
            }
        }


        protected void RecursiveDependenciesAdd(QuotationTemplate quotation, PickingItem toAddPickingItem,
            IEnumerable<PickingItem> pickingItems)
        {
            foreach (var _item in pickingItems)
            {
                if (toAddPickingItem.Link != null)
                {
                    try
                    {
                        if (_item.ID == Convert.ToInt32(toAddPickingItem.Link))
                        {
                            if (quotation.QuotationTemplateDetails.FirstOrDefault(qd => qd.CommonKey == _item.ID) ==
                                null)
                            {
                                quotation.QuotationTemplateDetails.Add(CreateDetail(quotation, _item));
                                RecursiveDependenciesAdd(quotation, _item, pickingItems);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        protected void RecursiveDependenciesAdd(QuotationTemplate quotation, MacroItem toAddPickingItem,
            IEnumerable<MacroItem> pickingItems)
        {
            foreach (var _item in pickingItems)
            {
                if (toAddPickingItem.Link != null)
                {
                    try
                    {
                        if (_item.ID == Convert.ToInt32(toAddPickingItem.Link))
                        {
                            if (quotation.QuotationTemplateDetails.FirstOrDefault(qd => qd.MacroItemKey == _item.ID) ==
                                null)
                            {
                                quotation.QuotationTemplateDetails.Add(CreateDetail(quotation, _item));
                                RecursiveDependenciesAdd(quotation, _item, pickingItems);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        protected QuotationTemplateDetail CreateDetail(QuotationTemplate quotation, PickingItem pickingItem)
        {
            var _toAddQuotationDetail = new QuotationTemplateDetail();
            _toAddQuotationDetail.ID_QuotationTemplate = quotation.ID; //Convert.ToInt32(QuotationParameter);
            _toAddQuotationDetail.Cost = pickingItem.Cost;
            _toAddQuotationDetail.MarkUp = 100;
            try
            {
                _toAddQuotationDetail.Price = pickingItem.Cost * Convert.ToDecimal(pickingItem.PercentageAuto / 100m) *
                                              Convert.ToDecimal(100m / 100m);
            }
            catch
            {
                _toAddQuotationDetail.Price = pickingItem.Cost * Convert.ToDecimal(pickingItem.PercentageAuto / 100m);
            }
            _toAddQuotationDetail.Multiply = pickingItem.Multiply;
            _toAddQuotationDetail.Percentage = pickingItem.PercentageAuto;
            _toAddQuotationDetail.SupplierCode = pickingItem.SupplierCode;
            _toAddQuotationDetail.TypeCode = pickingItem.TypeCode; //Convert.ToInt32(TypeParameter);
            _toAddQuotationDetail.UM = pickingItem.UM;
            _toAddQuotationDetail.ItemTypeDescription = pickingItem.ItemDescription;
            _toAddQuotationDetail.ItemTypeCode = pickingItem.ItemTypeCode;
            _toAddQuotationDetail.CommonKey = pickingItem.ID;
            _toAddQuotationDetail.Position = pickingItem.Order;
            _toAddQuotationDetail.Inserted = pickingItem.Inserted;
            _toAddQuotationDetail.ID_Company = pickingItem.ID_Company;

            return _toAddQuotationDetail;
        }

        protected QuotationTemplateDetail CreateDetail(QuotationTemplate quotation, MacroItem pickingItem)
        {
            var _toAddQuotationDetail = new QuotationTemplateDetail();
            _toAddQuotationDetail.ID_QuotationTemplate = quotation.ID; //Convert.ToInt32(QuotationParameter);
            _toAddQuotationDetail.Cost = pickingItem.CostCalc;
            _toAddQuotationDetail.MarkUp = 100;
            try
            {
                _toAddQuotationDetail.Price = pickingItem.CostCalc * Convert.ToDecimal(pickingItem.PercentageCalc / 100m) *
                                              Convert.ToDecimal(100m / 100m);
            }
            catch
            {
                _toAddQuotationDetail.Price = pickingItem.CostCalc * Convert.ToDecimal(pickingItem.PercentageCalc / 100m);
            }
            _toAddQuotationDetail.Multiply = pickingItem.Multiply;
            _toAddQuotationDetail.Percentage = pickingItem.PercentageCalc;
            _toAddQuotationDetail.SupplierCode = 11010481;
            _toAddQuotationDetail.TypeCode = pickingItem.TypeCode; //Convert.ToInt32(TypeParameter);
            _toAddQuotationDetail.UM = pickingItem.UM;
            _toAddQuotationDetail.ItemTypeDescription = "[" + pickingItem.MacroItemDescription + "]";
            _toAddQuotationDetail.ItemTypeCode = pickingItem.ItemTypeCode;
            _toAddQuotationDetail.MacroItemKey = pickingItem.ID;
            _toAddQuotationDetail.Position = pickingItem.Order;
            _toAddQuotationDetail.Inserted = pickingItem.Inserted;
            _toAddQuotationDetail.Save = true; // il campo Save identifica se la riga è una macrovoce!
            _toAddQuotationDetail.ID_Company = pickingItem.ID_Company;

            return _toAddQuotationDetail;
        }

        protected void GridDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            GridDataSource.OrderByParameters.Clear();
            GridDataSource.AutoGenerateOrderByClause = false;
            var table = GridDataSource.GetTable();
            var _qc = (QuotationDataContext)table.CreateContext();
            e.Result =
                _qc.QuotationTemplateDetails.OrderBy(qd => qd.Type.Order)
                    .ThenBy(qd => qd.ItemType.Order)
                    .ThenBy(qd => qd.Position);
        }

        protected enum Mode
        {
            InputItems,
            Calculation,
            Results
        }


        //protected void mnuQuotationTemplates_MenuItemClick(object sender, MenuEventArgs e)
        //{

        //    using (var QuotationDataContext = new QuotationDataContext())
        //    {
        //        var _pickingItemTypes =
        //            QuotationDataContext.PickingItems.Select(
        //                pi =>
        //                    new
        //                    {
        //                        pi.ID,
        //                        pi.Cost,
        //                        pi.PercentageAuto,
        //                        pi.Inserted,
        //                        pi.Multiply,
        //                        pi.SupplierCode,
        //                        pi.Order,
        //                        pi.ItemDescription
        //                    }).ToDictionary(pi => pi.ID.ToString());
        //        var _macroItemTypes =
        //            QuotationDataContext.MacroItems.Select(
        //                pi =>
        //                    new
        //                    {
        //                        pi.ID,
        //                        pi.CostCalc,
        //                        pi.PercentageCalc,
        //                        pi.Inserted,
        //                        pi.Multiply,
        //                        INTERNAL_SUPPLIERCODE,
        //                        pi.Order,
        //                        pi.MacroItemDescription
        //                    }).ToDictionary(pi => pi.ID.ToString());

        //        //Quotation _curQuotation = (Quotation)dtvQuotation.DataItem;
        //        var _curQuotationTemplate =
        //            QuotationDataContext.QuotationTemplates.First(
        //                QuotationTemplate => QuotationTemplate.ID == Convert.ToInt32(QuotationTemplateParameter));

        //        //var _curQuotationTemplateGet =
        //        //    QuotationDataContext.QuotationTemplates.First(
        //        //        QuotationTemplate => QuotationTemplate.ID == Convert.ToInt32(ddlTemplates.SelectedValue));

        //        var _menuPath = e.Item.Value.Split('.');
        //        var _cbk = new ComboKey(_menuPath);


        //        var _curQuotationTemplateGet = QuotationDataContext.QuotationTemplates.First(qt => qt.ID == _cbk.CommonKey);


        //        if (_curQuotationTemplate.ID != _curQuotationTemplateGet.ID)
        //        {
        //            QuotationDataContext.QuotationTemplateDetails.DeleteAllOnSubmit(
        //                _curQuotationTemplate.QuotationTemplateDetails);
        //            QuotationDataContext.SubmitChanges();

        //            foreach (
        //                var _quotationTemplateDetailGet in
        //                    _curQuotationTemplateGet.QuotationTemplateDetails)
        //            {
        //                var _quotationTemplateDetail = new QuotationTemplateDetail();

        //                //_quotationTemplateDetail.CommonKey = _quotationTemplateDetailGet.CommonKey;
        //                //_quotationTemplateDetail.MacroItemKey = _quotationTemplateDetailGet.MacroItemKey;
        //                //_quotationTemplateDetail.Cost = _quotationTemplateDetailGet.Cost;
        //                //_quotationTemplateDetail.ID_QuotationTemplate = Convert.ToInt32(QuotationTemplateParameter);
        //                //_quotationTemplateDetail.Inserted = _quotationTemplateDetailGet.Inserted;
        //                //_quotationTemplateDetail.ItemTypeCode = _quotationTemplateDetailGet.ItemTypeCode;
        //                //_quotationTemplateDetail.ItemTypeDescription = _quotationTemplateDetailGet.ItemTypeDescription;
        //                //_quotationTemplateDetail.MarkUp = _quotationTemplateDetailGet.MarkUp;
        //                //_quotationTemplateDetail.Multiply = _quotationTemplateDetailGet.Multiply;
        //                //_quotationTemplateDetail.Percentage = _quotationTemplateDetailGet.Percentage;
        //                //_quotationTemplateDetail.Position = _quotationTemplateDetailGet.Position;
        //                //_quotationTemplateDetail.Price = _quotationTemplateDetailGet.Price;
        //                //_quotationTemplateDetail.Quantity = _quotationTemplateDetailGet.Quantity;
        //                //_quotationTemplateDetail.Save = _quotationTemplateDetailGet.Save;
        //                //_quotationTemplateDetail.SelectPhase = _quotationTemplateDetailGet.SelectPhase;
        //                //_quotationTemplateDetail.SupplierCode = _quotationTemplateDetailGet.SupplierCode;
        //                //_quotationTemplateDetail.TypeCode = _quotationTemplateDetailGet.TypeCode;
        //                //_quotationTemplateDetail.UM = _quotationTemplateDetailGet.UM;

        //                if (_quotationTemplateDetailGet.Save)
        //                {
        //                    _quotationTemplateDetail.CommonKey = _quotationTemplateDetailGet.CommonKey;
        //                    _quotationTemplateDetail.MacroItemKey = _quotationTemplateDetailGet.MacroItemKey;
        //                    _quotationTemplateDetail.Cost =
        //                        Convert.ToDecimal(
        //                            _macroItemTypes[_quotationTemplateDetailGet.MacroItemKey.ToString()].CostCalc);
        //                    _quotationTemplateDetail.ID_QuotationTemplate = Convert.ToInt32(QuotationTemplateParameter);
        //                    _quotationTemplateDetail.Inserted =
        //                        _macroItemTypes[_quotationTemplateDetailGet.MacroItemKey.ToString()].Inserted;
        //                    _quotationTemplateDetail.ItemTypeCode = _quotationTemplateDetailGet.ItemTypeCode;
        //                    _quotationTemplateDetail.ItemTypeDescription = "[" +
        //                                                                   _macroItemTypes[
        //                                                                       _quotationTemplateDetailGet.MacroItemKey
        //                                                                           .ToString()].MacroItemDescription +
        //                                                                   "]";
        //                    _quotationTemplateDetail.MarkUp = Convert.ToInt32(_quotationTemplateDetailGet.MarkUp);
        //                    _quotationTemplateDetail.Multiply =
        //                        _macroItemTypes[_quotationTemplateDetailGet.MacroItemKey.ToString()].Multiply;
        //                    _quotationTemplateDetail.Percentage =
        //                        Convert.ToInt32(
        //                            _macroItemTypes[_quotationTemplateDetailGet.MacroItemKey.ToString()].PercentageCalc);
        //                    _quotationTemplateDetail.Position =
        //                        _macroItemTypes[_quotationTemplateDetailGet.MacroItemKey.ToString()].Order;
        //                    _quotationTemplateDetail.Price =
        //                        Convert.ToDecimal(_quotationTemplateDetailGet.Cost *
        //                                          (_quotationTemplateDetailGet.Percentage / 100m) *
        //                                          (_quotationTemplateDetailGet.MarkUp / 100m));
        //                    _quotationTemplateDetail.Quantity = _quotationTemplateDetailGet.Quantity;
        //                    _quotationTemplateDetail.Save = _quotationTemplateDetailGet.Save;
        //                    _quotationTemplateDetail.SelectPhase = _quotationTemplateDetailGet.SelectPhase;
        //                    _quotationTemplateDetail.SupplierCode =
        //                        _macroItemTypes[_quotationTemplateDetailGet.MacroItemKey.ToString()]
        //                            .INTERNAL_SUPPLIERCODE;
        //                    _quotationTemplateDetail.TypeCode = _quotationTemplateDetailGet.TypeCode;
        //                    _quotationTemplateDetail.UM = _quotationTemplateDetailGet.UM;
        //                }
        //                else
        //                {
        //                    _quotationTemplateDetail.CommonKey = _quotationTemplateDetailGet.CommonKey;
        //                    _quotationTemplateDetail.MacroItemKey = _quotationTemplateDetailGet.MacroItemKey;
        //                    _quotationTemplateDetail.Cost =
        //                        Convert.ToDecimal(
        //                            _pickingItemTypes[_quotationTemplateDetailGet.CommonKey.ToString()].Cost);
        //                    _quotationTemplateDetail.ID_QuotationTemplate = Convert.ToInt32(QuotationTemplateParameter);
        //                    _quotationTemplateDetail.Inserted =
        //                        _pickingItemTypes[_quotationTemplateDetailGet.CommonKey.ToString()].Inserted;
        //                    _quotationTemplateDetail.ItemTypeCode = _quotationTemplateDetailGet.ItemTypeCode;
        //                    _quotationTemplateDetail.ItemTypeDescription =
        //                        _pickingItemTypes[_quotationTemplateDetailGet.CommonKey.ToString()].ItemDescription;
        //                    _quotationTemplateDetail.MarkUp = Convert.ToInt32(_quotationTemplateDetailGet.MarkUp);
        //                    _quotationTemplateDetail.Multiply =
        //                        _pickingItemTypes[_quotationTemplateDetailGet.CommonKey.ToString()].Multiply;
        //                    _quotationTemplateDetail.Percentage =
        //                        Convert.ToInt32(
        //                            _pickingItemTypes[_quotationTemplateDetailGet.CommonKey.ToString()].PercentageAuto);
        //                    _quotationTemplateDetail.Position =
        //                        _pickingItemTypes[_quotationTemplateDetailGet.CommonKey.ToString()].Order;
        //                    _quotationTemplateDetail.Price =
        //                        Convert.ToDecimal(_quotationTemplateDetailGet.Cost *
        //                                          (_quotationTemplateDetailGet.Percentage / 100m) *
        //                                          (_quotationTemplateDetailGet.MarkUp / 100m));
        //                    _quotationTemplateDetail.Quantity = _quotationTemplateDetailGet.Quantity;
        //                    _quotationTemplateDetail.Save = _quotationTemplateDetailGet.Save;
        //                    _quotationTemplateDetail.SelectPhase = _quotationTemplateDetailGet.SelectPhase;
        //                    _quotationTemplateDetail.SupplierCode =
        //                        _pickingItemTypes[_quotationTemplateDetailGet.CommonKey.ToString()].SupplierCode;
        //                    _quotationTemplateDetail.TypeCode = _quotationTemplateDetailGet.TypeCode;
        //                    _quotationTemplateDetail.UM = _quotationTemplateDetailGet.UM;
        //                }

        //                _curQuotationTemplate.QuotationTemplateDetails.Add(_quotationTemplateDetail);
        //            }
        //            QuotationDataContext.SubmitChanges();
        //            Response.Redirect(
        //                string.Format("{2}?{0}={1}", QuotationTemplateKey, _curQuotationTemplateGet.ID,
        //                    QuotationTemplateConsolePage), true);
        //        }


        //    }

        //}

        protected void mnuQuotationTemplates_MenuItemClick(object sender, MenuEventArgs e)
        {

            using (var QuotationDataContext = new QuotationDataContext())
            {

                var _menuPath = e.Item.Value.Split('.');
                var _cbk = new ComboKey(_menuPath);


                var _curQuotationTemplateGet = QuotationDataContext.QuotationTemplates.First(qt => qt.ID == _cbk.CommonKey);

                Response.Redirect(
                    string.Format("{2}?{0}={1}", QuotationTemplateKey, _curQuotationTemplateGet.ID,
                        QuotationTemplateConsolePage), true);

            }

        }


        protected void mnuQuotationTemplatesAdd_MenuItemClick(object sender, MenuEventArgs e)
        {

            using (var QuotationDataContext = new QuotationDataContext())
            {
                var _pickingItemTypes =
                    QuotationDataContext.PickingItems.Select(
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
                    QuotationDataContext.MacroItems.Select(
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


                var _curQuotationTemplate =
                    QuotationDataContext.QuotationTemplates.First(
                        QuotationTemplate => QuotationTemplate.ID == Convert.ToInt32(QuotationTemplateParameter));

                var _menuPath = e.Item.Value.Split('.');
                var _cbk = new ComboKey(_menuPath);


                var _curQuotationTemplateGet = QuotationDataContext.QuotationTemplates.First(qt => qt.ID == _cbk.CommonKey);


                if (_curQuotationTemplate.ID != _curQuotationTemplateGet.ID)
                {

                    foreach (
                        var _quotationTemplateDetailGet in
                            _curQuotationTemplateGet.QuotationTemplateDetails)
                    {
                        var _quotationTemplateDetail = new QuotationTemplateDetail();

                        if (_quotationTemplateDetailGet.Save)
                        {
                            _quotationTemplateDetail.CommonKey = _quotationTemplateDetailGet.CommonKey;
                            _quotationTemplateDetail.MacroItemKey = _quotationTemplateDetailGet.MacroItemKey;
                            _quotationTemplateDetail.Cost =
                                Convert.ToDecimal(
                                    _macroItemTypes[_quotationTemplateDetailGet.MacroItemKey.ToString()].CostCalc);
                            _quotationTemplateDetail.ID_QuotationTemplate = Convert.ToInt32(QuotationTemplateParameter);
                            _quotationTemplateDetail.Inserted =
                                _macroItemTypes[_quotationTemplateDetailGet.MacroItemKey.ToString()].Inserted;
                            _quotationTemplateDetail.ItemTypeCode = _quotationTemplateDetailGet.ItemTypeCode;
                            _quotationTemplateDetail.ItemTypeDescription = "[" +
                                                                           _macroItemTypes[
                                                                               _quotationTemplateDetailGet.MacroItemKey
                                                                                   .ToString()].MacroItemDescription +
                                                                           "]";
                            _quotationTemplateDetail.MarkUp = Convert.ToInt32(_quotationTemplateDetailGet.MarkUp);
                            _quotationTemplateDetail.Multiply =
                                _macroItemTypes[_quotationTemplateDetailGet.MacroItemKey.ToString()].Multiply;
                            _quotationTemplateDetail.Percentage =
                                Convert.ToInt32(
                                    _macroItemTypes[_quotationTemplateDetailGet.MacroItemKey.ToString()].PercentageCalc);
                            _quotationTemplateDetail.Position =
                                _macroItemTypes[_quotationTemplateDetailGet.MacroItemKey.ToString()].Order;
                            _quotationTemplateDetail.Price =
                                Convert.ToDecimal(_quotationTemplateDetailGet.Cost *
                                                  (_quotationTemplateDetailGet.Percentage / 100m) *
                                                  (_quotationTemplateDetailGet.MarkUp / 100m));
                            _quotationTemplateDetail.Quantity = _quotationTemplateDetailGet.Quantity;
                            _quotationTemplateDetail.Save = _quotationTemplateDetailGet.Save;
                            _quotationTemplateDetail.SelectPhase = _quotationTemplateDetailGet.SelectPhase;
                            _quotationTemplateDetail.SupplierCode =
                                _macroItemTypes[_quotationTemplateDetailGet.MacroItemKey.ToString()]
                                    .INTERNAL_SUPPLIERCODE;
                            _quotationTemplateDetail.TypeCode = _quotationTemplateDetailGet.TypeCode;
                            _quotationTemplateDetail.UM = _quotationTemplateDetailGet.UM;
                            _quotationTemplateDetail.ID_Company = _quotationTemplateDetailGet.ID_Company;
                        }
                        else
                        {
                            _quotationTemplateDetail.CommonKey = _quotationTemplateDetailGet.CommonKey;
                            _quotationTemplateDetail.MacroItemKey = _quotationTemplateDetailGet.MacroItemKey;
                            _quotationTemplateDetail.Cost =
                                Convert.ToDecimal(
                                    _pickingItemTypes[_quotationTemplateDetailGet.CommonKey.ToString()].Cost);
                            _quotationTemplateDetail.ID_QuotationTemplate = Convert.ToInt32(QuotationTemplateParameter);
                            _quotationTemplateDetail.Inserted =
                                _pickingItemTypes[_quotationTemplateDetailGet.CommonKey.ToString()].Inserted;
                            _quotationTemplateDetail.ItemTypeCode = _quotationTemplateDetailGet.ItemTypeCode;
                            _quotationTemplateDetail.ItemTypeDescription =
                                _pickingItemTypes[_quotationTemplateDetailGet.CommonKey.ToString()].ItemDescription;
                            _quotationTemplateDetail.MarkUp = Convert.ToInt32(_quotationTemplateDetailGet.MarkUp);
                            _quotationTemplateDetail.Multiply =
                                _pickingItemTypes[_quotationTemplateDetailGet.CommonKey.ToString()].Multiply;
                            _quotationTemplateDetail.Percentage =
                                Convert.ToInt32(
                                    _pickingItemTypes[_quotationTemplateDetailGet.CommonKey.ToString()].PercentageAuto);
                            _quotationTemplateDetail.Position =
                                _pickingItemTypes[_quotationTemplateDetailGet.CommonKey.ToString()].Order;
                            _quotationTemplateDetail.Price =
                                Convert.ToDecimal(_quotationTemplateDetailGet.Cost *
                                                  (_quotationTemplateDetailGet.Percentage / 100m) *
                                                  (_quotationTemplateDetailGet.MarkUp / 100m));
                            _quotationTemplateDetail.Quantity = _quotationTemplateDetailGet.Quantity;
                            _quotationTemplateDetail.Save = _quotationTemplateDetailGet.Save;
                            _quotationTemplateDetail.SelectPhase = _quotationTemplateDetailGet.SelectPhase;
                            _quotationTemplateDetail.SupplierCode =
                                _pickingItemTypes[_quotationTemplateDetailGet.CommonKey.ToString()].SupplierCode;
                            _quotationTemplateDetail.TypeCode = _quotationTemplateDetailGet.TypeCode;
                            _quotationTemplateDetail.UM = _quotationTemplateDetailGet.UM;
                            _quotationTemplateDetail.ID_Company = _quotationTemplateDetailGet.ID_Company;
                        }

                        _curQuotationTemplate.QuotationTemplateDetails.Add(_quotationTemplateDetail);
                    }
                    QuotationDataContext.SubmitChanges();
                    Response.Redirect(
                        string.Format("{2}?{0}={1}", QuotationTemplateKey, Convert.ToInt32(QuotationTemplateParameter),
                            QuotationTemplateConsolePage), true);
                }


            }

        }
    }
}