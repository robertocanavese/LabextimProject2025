using System;
using System.Collections.Generic;
using System.Linq;
using DLLabExtim;

namespace UILabExtim
{
    public class QuotationController : BaseController
    {
        protected const string MACROITEM_PREFIX = "M";
        protected const string PICKINGITEM_PREFIX = "P";
        //private QuotationDataContext m_QuotationDataContext;

        //public QuotationDataContext QuotationDataContext
        //{
        //    get { return m_QuotationDataContext; }
        //}

        //protected readonly static string QuotationKey = "P0";
        //public string QuotationParameter
        //{
        //    get
        //    {
        //        object temp = Request.QueryString[QuotationKey];
        //        return temp == null ? string.Empty : temp.ToString().PadRight(6, ' ').Substring(0, 6);
        //    }
        //}

        //protected readonly static string ReportTypeKey = "P1";
        //public int ReportTypeParameter
        //{
        //    get
        //    {
        //        object temp = Request.QueryString[ReportTypeKey];
        //        return temp == null ? 0 : Convert.ToInt32(temp);
        //    }
        //}

        protected static readonly string QuotationTemplateKey = "P2"; 
        protected static readonly string MacroItemKey = "P3";

        public string QuotationTemplateParameter  
        {
            get
            {
                object temp = Request.QueryString[QuotationTemplateKey];
                return temp == null ? string.Empty : temp.ToString().PadRight(6, ' ').Substring(0, 6);
            }
        }

        public string MacroItemParameter
        {
            get
            {
                object temp = Request.QueryString[MacroItemKey];
                return temp == null ? string.Empty : temp.ToString().PadRight(6, ' ').Substring(0, 6);
            }
        }

        //protected readonly static string ReportOnProductionQuantityKey = "P4";
        //public int ReportOnProductionQuantityParameter
        //{
        //    get
        //    {
        //        object temp = Request.QueryString[ReportOnProductionQuantityKey];
        //        return temp == null ? 0 : Convert.ToInt32(temp);
        //    }
        //}

        //protected readonly static string CustomerOrderKey = "P5";
        //public string CustomerOrderParameter
        //{
        //    get
        //    {
        //        object temp = Request.QueryString[CustomerOrderKey];
        //        return temp == null ? string.Empty : temp.ToString().PadRight(6, ' ').Substring(0, 6);
        //    }
        //}

        //protected readonly static string ProductionOrderKey = "P6";
        //public string ProductionOrderParameter
        //{
        //    get
        //    {
        //        object temp = Request.QueryString[ProductionOrderKey];
        //        return temp == null ? string.Empty : temp.ToString().PadRight(6, ' ').Substring(0, 6);
        //    }
        //}

        //public enum ReportType { Customer, Technical }

        public KeyValuePair<int, string> QuotationTemplateHeader
        {
            get
            {
                if (Session["QuotationTemplateHeader"] == null)
                {
                    return
                        new KeyValuePair<int, string>(
                            QuotationParameter != string.Empty ? Convert.ToInt32(QuotationTemplateParameter) : -1,
                            "Senza nome");
                }
                return (KeyValuePair<int, string>) Session["QuotationTemplateHeader"];
            }
            set { Session["QuotationTemplateHeader"] = value; }
        }

        public KeyValuePair<int, string> MacroItemHeader
        {
            get
            {
                if (Session["MacroItemHeader"] == null)
                {
                    return
                        new KeyValuePair<int, string>(
                            MacroItemParameter != string.Empty ? Convert.ToInt32(MacroItemParameter) : -1, "Senza nome");
                }
                return (KeyValuePair<int, string>) Session["MacroItemHeader"];
            }
            set { Session["MacroItemHeader"] = value; }
        }

        public List<ReportText> CurrentReportTexts
        {
            get
            {
                {
                    return (List<ReportText>) Session["CurrentReportTexts"];
                }
            }
            set { Session["CurrentReportTexts"] = value; }
        }

        //public void InitSelectionData()
        //{
        //    SelectionData = QuotationParameter;
        //    ViewState["SelectionData"] = SelectionData;
        //}


        protected override void OnInit(EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                InitQuotationDataContext();
            }
            base.OnInit(e);
        }

        public void InitQuotationDataContext()
        {
            //m_QuotationDataContext = Session["QuotationDataContext"] as QuotationDataContext;
            //if (m_QuotationDataContext == null)
            //{
            //    m_QuotationDataContext = new QuotationDataContext();
            //    Session["QuotationDataContext"] = m_QuotationDataContext;
            //}
        }

        protected override void OnUnload(EventArgs e)
        {
            //Session["QuotationDataContext"] = m_QuotationDataContext;
            base.OnUnload(e);
        }

        protected int SaveAsNewQuotation(QuotationDataContext quotationDataContext, string newSubject)
        {
            var _curQuotation =
                quotationDataContext.Quotations.First(Quotation => Quotation.ID == Convert.ToInt32(QuotationParameter));

            var _quotationToSave = new Quotation();
            //_quotationToSave.Subject = "Copia di " + newSubject;

            _quotationToSave.Subject = newSubject;
            _quotationToSave.CustomerCode = _curQuotation.CustomerCode;
            if (GetCurrentEmployee() != null)
                _quotationToSave.ID_Owner = GetCurrentEmployee().ID;
            _quotationToSave.Date = DateTime.Now;
            _quotationToSave.MarkUp = _curQuotation.MarkUp;
            _quotationToSave.Q1 = _curQuotation.Q1;
            _quotationToSave.Q2 = _curQuotation.Q2;
            _quotationToSave.Q3 = _curQuotation.Q3;
            _quotationToSave.Q4 = _curQuotation.Q4;
            _quotationToSave.Q5 = _curQuotation.Q5;
            _quotationToSave.Status = 9;
            _quotationToSave.Note = _curQuotation.Note;
            _quotationToSave.Draft = false;

            quotationDataContext.Quotations.InsertOnSubmit(_quotationToSave);

            foreach (var _quotationDetail in _curQuotation.QuotationDetails)
            {
                var _quotationToSaveDetail = new QuotationDetail();

                _quotationToSaveDetail.CommonKey = _quotationDetail.CommonKey;
                _quotationToSaveDetail.MacroItemKey = _quotationDetail.MacroItemKey;
                _quotationToSaveDetail.Cost = _quotationDetail.Cost;
                _quotationToSaveDetail.ID_Quotation = _curQuotation.ID;
                _quotationToSaveDetail.Inserted = _quotationDetail.Inserted;
                _quotationToSaveDetail.ItemTypeCode = _quotationDetail.ItemTypeCode;
                _quotationToSaveDetail.ItemTypeDescription = _quotationDetail.ItemTypeDescription;
                _quotationToSaveDetail.MarkUp = _quotationDetail.MarkUp;
                _quotationToSaveDetail.Multiply = _quotationDetail.Multiply;
                _quotationToSaveDetail.Percentage = _quotationDetail.Percentage;
                _quotationToSaveDetail.Position = _quotationDetail.Position;
                _quotationToSaveDetail.Price = _quotationDetail.Price;
                _quotationToSaveDetail.Quantity = _quotationDetail.Quantity;
                _quotationToSaveDetail.Save = _quotationDetail.Save;
                _quotationToSaveDetail.SelectPhase = _quotationDetail.SelectPhase;
                _quotationToSaveDetail.SupplierCode = _quotationDetail.SupplierCode;
                _quotationToSaveDetail.TypeCode = _quotationDetail.TypeCode;
                _quotationToSaveDetail.UM = _quotationDetail.UM;
                _quotationToSave.QuotationDetails.Add(_quotationToSaveDetail);
            }
            quotationDataContext.SubmitChanges();
            return _quotationToSave.ID;
        }

        protected int SaveAsNewUpdatedQuotation(QuotationDataContext quotationDataContext, string newSubject)
        {
            var _curQuotation =
                quotationDataContext.Quotations.First(Quotation => Quotation.ID == Convert.ToInt32(QuotationParameter));

            var _quotationToSave = new Quotation();
            _quotationToSave.Subject = "Copia di " + newSubject;

            _quotationToSave.Subject = newSubject;
            _quotationToSave.CustomerCode = _curQuotation.CustomerCode;
            if (GetCurrentEmployee() != null)
                _quotationToSave.ID_Owner = GetCurrentEmployee().ID;
            _quotationToSave.Date = DateTime.Now;
            _quotationToSave.MarkUp = _curQuotation.MarkUp;
            _quotationToSave.Q1 = _curQuotation.Q1;
            _quotationToSave.Q2 = _curQuotation.Q2;
            _quotationToSave.Q3 = _curQuotation.Q3;
            _quotationToSave.Q4 = _curQuotation.Q4;
            _quotationToSave.Q5 = _curQuotation.Q5;
            _quotationToSave.Status = 9;
            _quotationToSave.Note = _curQuotation.Note;
            _quotationToSave.Draft = false;

            quotationDataContext.Quotations.InsertOnSubmit(_quotationToSave);

            foreach (var _quotationDetail in _curQuotation.QuotationDetails)
            {
                var _quotationToSaveDetail = new QuotationDetail();

                IEnumerable<PickingItem> _pickingItems = quotationDataContext.PickingItems;
                IEnumerable<MacroItem> _macroItems = quotationDataContext.MacroItems;

                if (_quotationDetail.CommonKey != null)
                {
                    var _toAddPickingItem = _pickingItems.First(pi => pi.ID == _quotationDetail.CommonKey);
                    //RecursiveDependenciesAdd(_curQuotation, _toAddPickingItem, _pickingItems);
                    var _quotationDetailToAdd = CreateDetail(_curQuotation, _toAddPickingItem);
                    _quotationDetailToAdd.Quantity = _quotationDetail.Quantity;
                    _quotationToSave.QuotationDetails.Add(_quotationDetailToAdd);
                }
                else if (_quotationDetail.MacroItemKey != null)
                {
                    var _toAddPickingItem = _macroItems.First(pi => pi.ID == _quotationDetail.MacroItemKey);
                    //RecursiveDependenciesAdd(_curQuotation, _toAddPickingItem, _macroItems);
                    var _quotationDetailToAdd = CreateDetail(_curQuotation, _toAddPickingItem);
                    _quotationDetailToAdd.Quantity = _quotationDetail.Quantity;
                    _quotationToSave.QuotationDetails.Add(_quotationDetailToAdd);
                }
                else
                {
                    var _quotationDetailToAdd = CreateDetail(_curQuotation, _quotationDetail);
                    _quotationDetailToAdd.Quantity = _quotationDetail.Quantity;
                    _quotationToSave.QuotationDetails.Add(_quotationDetailToAdd);
                }
            }
            quotationDataContext.SubmitChanges();
            return _quotationToSave.ID;
        }

        protected void RecursiveDependenciesAdd(Quotation quotation, PickingItem toAddPickingItem,
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
                            if (quotation.QuotationDetails.FirstOrDefault(qd => qd.CommonKey == _item.ID) == null)
                            {
                                quotation.QuotationDetails.Add(CreateDetail(quotation, _item));
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

        protected void RecursiveDependenciesAdd(Quotation quotation, MacroItem toAddMacroItem,
            IEnumerable<MacroItem> macroItems)
        {
            foreach (var _item in macroItems)
            {
                if (toAddMacroItem.Link != null)
                {
                    try
                    {
                        if (_item.ID == Convert.ToInt32(toAddMacroItem.Link))
                        {
                            if (quotation.QuotationDetails.FirstOrDefault(qd => qd.MacroItemKey == _item.ID) == null)
                            {
                                quotation.QuotationDetails.Add(CreateDetail(quotation, _item));
                                RecursiveDependenciesAdd(quotation, _item, macroItems);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        protected void RecursiveDependenciesAdd(Quotation quotation, PickingItem toAddPickingItem,
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
                            if (quotation.QuotationDetails.FirstOrDefault(qd => qd.CommonKey == _item.ID) == null)
                            {
                                quotation.QuotationDetails.Add(CreateDetail(quotation, _item));
                                RecursiveDependenciesAdd(quotation, _item, pickingItems);
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
                        if (quotation.QuotationDetails.FirstOrDefault(qd => qd.MacroItemKey == _item.ID) == null)
                        {
                            quotation.QuotationDetails.Add(CreateDetail(quotation, _item));
                            RecursiveDependenciesAdd(quotation, _item, macroItems);
                        }
                    }
                }
                catch
                {
                }
            }
        }

        protected void RecursiveDependenciesAdd(Quotation quotation, MacroItem toAddMacroItem,
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
                            if (quotation.QuotationDetails.FirstOrDefault(qd => qd.MacroItemKey == _item.ID) == null)
                            {
                                quotation.QuotationDetails.Add(CreateDetail(quotation, _item));
                                RecursiveDependenciesAdd(quotation, _item, macroItems);
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
                        if (quotation.QuotationDetails.FirstOrDefault(qd => qd.CommonKey == _item.ID) == null)
                        {
                            quotation.QuotationDetails.Add(CreateDetail(quotation, _item));
                            RecursiveDependenciesAdd(quotation, _item, pickingItems);
                        }
                    }
                }
                catch
                {
                }
            }
        }

        protected QuotationDetail CreateDetail(Quotation quotation, PickingItem pickingItem)
        {
            var _toAddQuotationDetail = new QuotationDetail();
            _toAddQuotationDetail.ID_Quotation = quotation.ID; //Convert.ToInt32(QuotationParameter);
            _toAddQuotationDetail.Cost = pickingItem.Cost;
            _toAddQuotationDetail.MarkUp = Convert.ToInt32(quotation.MarkUp);
            try
            {
                _toAddQuotationDetail.Price = pickingItem.Cost*Convert.ToDecimal(pickingItem.PercentageAuto/100m)*
                                              Convert.ToDecimal(quotation.MarkUp/100m);
            }
            catch
            {
                _toAddQuotationDetail.Price = pickingItem.Cost*Convert.ToDecimal(pickingItem.PercentageAuto/100m);
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

        protected QuotationDetail CreateDetail(Quotation quotation, MacroItem pickingItem)
        {
            var _toAddQuotationDetail = new QuotationDetail();
            _toAddQuotationDetail.ID_Quotation = quotation.ID; //Convert.ToInt32(QuotationParameter);
            _toAddQuotationDetail.Cost = pickingItem.CostCalc;
            _toAddQuotationDetail.MarkUp = Convert.ToInt32(quotation.MarkUp);
            try
            {
                _toAddQuotationDetail.Price = pickingItem.CostCalc*Convert.ToDecimal(pickingItem.PercentageCalc/100m)*
                                              Convert.ToDecimal(quotation.MarkUp/100m);
            }
            catch
            {
                _toAddQuotationDetail.Price = pickingItem.CostCalc*Convert.ToDecimal(pickingItem.PercentageCalc/100m);
            }
            _toAddQuotationDetail.Multiply = pickingItem.Multiply;
            _toAddQuotationDetail.Percentage = pickingItem.PercentageCalc;
            _toAddQuotationDetail.SupplierCode = INTERNAL_SUPPLIERCODE;
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

        protected QuotationDetail CreateDetail(Quotation quotation, QuotationDetail quotationDetail)
        {
            var _toAddQuotationDetail = new QuotationDetail();
            _toAddQuotationDetail.ID_Quotation = quotation.ID; //Convert.ToInt32(QuotationParameter);
            _toAddQuotationDetail.Cost = quotationDetail.Cost;
            _toAddQuotationDetail.MarkUp = Convert.ToInt32(quotation.MarkUp);
            _toAddQuotationDetail.Price = quotationDetail.Price;
            _toAddQuotationDetail.Multiply = quotationDetail.Multiply;
            _toAddQuotationDetail.Percentage = quotationDetail.Percentage;
            _toAddQuotationDetail.SupplierCode = quotationDetail.SupplierCode;
            _toAddQuotationDetail.TypeCode = quotationDetail.TypeCode; //Convert.ToInt32(TypeParameter);
            _toAddQuotationDetail.UM = quotationDetail.UM;
            _toAddQuotationDetail.ItemTypeDescription = quotationDetail.ItemTypeDescription;
            _toAddQuotationDetail.ItemTypeCode = quotationDetail.ItemTypeCode;
            _toAddQuotationDetail.Position = quotationDetail.Position;
            _toAddQuotationDetail.Inserted = quotationDetail.Inserted;
            _toAddQuotationDetail.ID_Company = quotationDetail.ID_Company;

            return _toAddQuotationDetail;
        }

        protected string GetBestReportText(int reportTypeCode, int textTypeCode, int? referenceId)
        {
            var _foundText = string.Empty;
            var _foundReportText =
                CurrentReportTexts.SingleOrDefault(
                    rt =>
                        rt.ReportTypeCode == reportTypeCode && rt.TextTypeCode == textTypeCode &&
                        rt.ID_Ref01 == referenceId);
            if (_foundReportText != null)
            {
                _foundText = _foundReportText.Text;
            }
            else
            {
                _foundReportText =
                    new ReportDataContext().ReportTexts.SingleOrDefault(
                        rt =>
                            rt.ReportTypeCode == reportTypeCode && rt.TextTypeCode == textTypeCode &&
                            rt.ID_Ref01 == referenceId);
                if (_foundReportText != null)
                {
                    _foundText = _foundReportText.Text;
                }
                else
                {
                    if (reportTypeCode == 1 && textTypeCode == 1)
                    {
                        var _customerContact =
                            new QuotationDataContext().Customers.SingleOrDefault(rt => rt.Code == referenceId).Contact;
                        if (_customerContact != null)
                        {
                            _foundText = _customerContact;
                        }
                        else
                        {
                            _foundReportText =
                                new ReportDataContext().ReportTexts.SingleOrDefault(
                                    rt =>
                                        rt.ReportTypeCode == reportTypeCode && rt.TextTypeCode == textTypeCode &&
                                        rt.Standard);
                            if (_foundReportText != null)
                            {
                                _foundText = _foundReportText.Text;
                            }
                        }
                    }
                    else
                    {
                        _foundReportText =
                            new ReportDataContext().ReportTexts.SingleOrDefault(
                                rt =>
                                    rt.ReportTypeCode == reportTypeCode && rt.TextTypeCode == textTypeCode &&
                                    rt.Standard);
                        if (_foundReportText != null)
                        {
                            _foundText = _foundReportText.Text;
                        }
                    }
                }
            }
            return _foundText;
        }

        public int GetTempQuotationUniqueId()
        {
            var _newValue = 0;
            while (true)
            {
                var _uniqueId = new Random(DateTime.Now.Millisecond).Next().ToString();

                _newValue = Convert.ToInt32("-" + _uniqueId.Substring(_uniqueId.Length - 5));
                using (var _qc = new QuotationDataContext())
                {
                    var _found =
                        _qc.TempQuotations.FirstOrDefault(
                            tq => tq.ID_Quotation == _newValue && tq.SessionUser == GetCurrentEmployee().ID);
                    if (_found == null)
                        break;
                    ;
                }
            }
            return _newValue;
        }
    }
}