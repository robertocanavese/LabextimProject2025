using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using DLLabExtim;

namespace UILabExtim
{
    public class TempQuotationController : QuotationController
    {
        protected int SaveQuotation(QuotationDataContext quotationDataContext, ProductionOrderService.SchedulingType type) //, string subject)
        {
            var _curTempQuotation =
                quotationDataContext.TempQuotations.First(
                    Quotation =>
                        Quotation.ID_Quotation == Convert.ToInt32(QuotationParameter) &&
                        Quotation.SessionUser == GetCurrentEmployee().ID);
            var _curQuotation =
                quotationDataContext.Quotations.FirstOrDefault(
                    Quotation => Quotation.ID == Convert.ToInt32(QuotationParameter));


            if (_curQuotation == null)
            {
                var _quotationToSave = new Quotation();
                //_quotationToSave.Subject = subject;
                _quotationToSave.ID_Company = CurrentCompanyId;
                _quotationToSave.ID_Manager = _curTempQuotation.ID_Manager;
                _quotationToSave.CustomerCode = _curTempQuotation.CustomerCode;
                if (GetCurrentEmployee() != null)
                    _quotationToSave.ID_Owner = GetCurrentEmployee().ID;
                _quotationToSave.Date = DateTime.Now;
                _quotationToSave.MarkUp = _curTempQuotation.MarkUp;
                _quotationToSave.Q1 = _curTempQuotation.Q1;
                _quotationToSave.Q2 = _curTempQuotation.Q2;
                _quotationToSave.Q3 = _curTempQuotation.Q3;
                _quotationToSave.Q4 = _curTempQuotation.Q4;
                _quotationToSave.Q5 = _curTempQuotation.Q5;
                _quotationToSave.Status = 9;
                _quotationToSave.Note = _curTempQuotation.Note;
                _quotationToSave.P1 = _curTempQuotation.P1.GetValueOrDefault(false);
                _quotationToSave.P2 = _curTempQuotation.P2.GetValueOrDefault(false);
                _quotationToSave.P3 = _curTempQuotation.P3.GetValueOrDefault(false);
                _quotationToSave.P4 = _curTempQuotation.P4.GetValueOrDefault(false);
                _quotationToSave.P5 = _curTempQuotation.P5.GetValueOrDefault(false);
                _quotationToSave.PriceCom = _curTempQuotation.PriceCom;
                _quotationToSave.PrintingMainText = _curTempQuotation.PrintingMainText;
                _quotationToSave.Draft = false;

                _quotationToSave.Note1 = _curQuotation.Note1;

                quotationDataContext.Quotations.InsertOnSubmit(_quotationToSave);

                foreach (var _quotationDetail in _curTempQuotation.TempQuotationDetails)
                {
                    var _quotationToSaveDetail = new QuotationDetail();

                    _quotationToSaveDetail.CommonKey = _quotationDetail.CommonKey;
                    _quotationToSaveDetail.MacroItemKey = _quotationDetail.MacroItemKey;
                    _quotationToSaveDetail.ID_Company = _quotationDetail.ID_Company;
                    _quotationToSaveDetail.Cost = _quotationDetail.Cost;
                    //_quotationToSaveDetail.ID_Quotation = _quotationDetail.ID_Quotation;
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
            //_curQuotation.Subject = subject;
            _curQuotation.ID_Company = CurrentCompanyId;
            _curQuotation.ID_Manager = _curTempQuotation.ID_Manager;
            _curQuotation.CustomerCode = _curTempQuotation.CustomerCode;
            if (_curQuotation.ID_Owner == null)
            {
                if (GetCurrentEmployee() != null)
                    _curQuotation.ID_Owner = GetCurrentEmployee().ID;
            }
            //_curQuotation.Date = DateTime.Now;
            _curQuotation.UpdateDate = DateTime.Now;
            _curQuotation.MarkUp = _curTempQuotation.MarkUp;
            _curQuotation.Q1 = _curTempQuotation.Q1;
            _curQuotation.Q2 = _curTempQuotation.Q2;
            _curQuotation.Q3 = _curTempQuotation.Q3;
            _curQuotation.Q4 = _curTempQuotation.Q4;
            _curQuotation.Q5 = _curTempQuotation.Q5;
            _curQuotation.Subject = _curTempQuotation.Subject;
            _curQuotation.Status = 9;
            _curQuotation.Note = _curTempQuotation.Note;
            _curQuotation.P1 = _curTempQuotation.P1.GetValueOrDefault(false);
            _curQuotation.P2 = _curTempQuotation.P2.GetValueOrDefault(false);
            _curQuotation.P3 = _curTempQuotation.P3.GetValueOrDefault(false);
            _curQuotation.P4 = _curTempQuotation.P4.GetValueOrDefault(false);
            _curQuotation.P5 = _curTempQuotation.P5.GetValueOrDefault(false);
            _curQuotation.PriceCom = _curTempQuotation.PriceCom;
            _curQuotation.PrintingMainText = _curTempQuotation.PrintingMainText;
            _curQuotation.Draft = false;

            _curQuotation.Note1 = _curTempQuotation.Note1;

            ProductionOrderService.SyncroniseProductionOrdersDescription(quotationDataContext, _curQuotation, _curTempQuotation.Subject, _curTempQuotation.Note, _curTempQuotation.Note1, _curTempQuotation.ID_Owner.GetValueOrDefault());

            ProductionOrderService.DeleteProductionOrderSchedulesOfAQuotation(quotationDataContext, _curQuotation);

            IEnumerable<QuotationDetail> _detailsToDelete =
                quotationDataContext.QuotationDetails.Where(qd => qd.ID_Quotation == Convert.ToInt32(QuotationParameter));
            quotationDataContext.QuotationDetails.DeleteAllOnSubmit(_detailsToDelete);



            foreach (var _quotationDetail in _curTempQuotation.TempQuotationDetails)
            {
                var _quotationToSaveDetail = new QuotationDetail();

                _quotationToSaveDetail.CommonKey = _quotationDetail.CommonKey;
                _quotationToSaveDetail.MacroItemKey = _quotationDetail.MacroItemKey;
                _quotationToSaveDetail.ID_Company = _quotationDetail.ID_Company;
                _quotationToSaveDetail.Cost = _quotationDetail.Cost;
                _quotationToSaveDetail.ID_Quotation = _quotationDetail.ID_Quotation;
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
                _curQuotation.QuotationDetails.Add(_quotationToSaveDetail);
            }

            quotationDataContext.SubmitChanges();
            ProductionOrderService.CreateProductionOrderSchedulesOfAQuotation(quotationDataContext, _curQuotation, type);
            return _curQuotation.ID;
        }

        protected int SaveTempAsNewQuotation(QuotationDataContext quotationDataContext, string newSubject)
        {
            var _curQuotation =
                quotationDataContext.TempQuotations.First(
                    Quotation =>
                        Quotation.ID_Quotation == Convert.ToInt32(QuotationParameter) &&
                        Quotation.SessionUser == GetCurrentEmployee().ID);

            var _quotationToSave = new Quotation();
            //_quotationToSave.Subject = "Copia di " + newSubject;

            _quotationToSave.Subject = newSubject;
            _quotationToSave.ID_Company = CurrentCompanyId;
            _quotationToSave.ID_Manager = _curQuotation.ID_Manager;
            _quotationToSave.CustomerCode = _curQuotation.CustomerCode;
            if (GetCurrentEmployee() != null)
                _quotationToSave.ID_Owner = GetCurrentEmployee().ID;
            _quotationToSave.Date = DateTime.Now;
            _quotationToSave.UpdateDate = _quotationToSave.Date;
            _quotationToSave.MarkUp = _curQuotation.MarkUp;
            _quotationToSave.Q1 = _curQuotation.Q1;
            _quotationToSave.Q2 = _curQuotation.Q2;
            _quotationToSave.Q3 = _curQuotation.Q3;
            _quotationToSave.Q4 = _curQuotation.Q4;
            _quotationToSave.Q5 = _curQuotation.Q5;
            _quotationToSave.Status = 9;
            _quotationToSave.Note = _curQuotation.Note;
            _quotationToSave.P1 = _curQuotation.P1.GetValueOrDefault(false);
            _quotationToSave.P2 = _curQuotation.P2.GetValueOrDefault(false);
            _quotationToSave.P3 = _curQuotation.P3.GetValueOrDefault(false);
            _quotationToSave.P4 = _curQuotation.P4.GetValueOrDefault(false);
            _quotationToSave.P5 = _curQuotation.P5.GetValueOrDefault(false);
            _quotationToSave.PriceCom = _curQuotation.PriceCom;
            _quotationToSave.PrintingMainText = _curQuotation.PrintingMainText;
            _quotationToSave.Draft = false;

            _quotationToSave.Note1 = _curQuotation.Note1;

            quotationDataContext.Quotations.InsertOnSubmit(_quotationToSave);

            foreach (var _quotationDetail in _curQuotation.TempQuotationDetails)
            {
                var _quotationToSaveDetail = new QuotationDetail();

                _quotationToSaveDetail.CommonKey = _quotationDetail.CommonKey;
                _quotationToSaveDetail.MacroItemKey = _quotationDetail.MacroItemKey;
                _quotationToSaveDetail.ID_Company = _quotationDetail.ID_Company;
                _quotationToSaveDetail.Cost = _quotationDetail.Cost;
                _quotationToSaveDetail.ID_Quotation = _curQuotation.ID_Quotation;
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

        protected int SaveTempAsNewUpdatedQuotation(QuotationDataContext quotationDataContext, string newSubject)
        {
            var _curQuotation =
                quotationDataContext.TempQuotations.First(
                    Quotation =>
                        Quotation.ID_Quotation == Convert.ToInt32(QuotationParameter) &&
                        Quotation.SessionUser == GetCurrentEmployee().ID);

            var _quotationToSave = new Quotation();
            _quotationToSave.Subject = "Copia di " + newSubject;

            _quotationToSave.Subject = newSubject;
            _quotationToSave.ID_Company = CurrentCompanyId;
            _quotationToSave.ID_Manager = _curQuotation.ID_Manager;
            _quotationToSave.CustomerCode = _curQuotation.CustomerCode;
            if (GetCurrentEmployee() != null)
                _quotationToSave.ID_Owner = GetCurrentEmployee().ID;
            _quotationToSave.Date = DateTime.Now;
            _quotationToSave.UpdateDate = _quotationToSave.Date;
            _quotationToSave.MarkUp = _curQuotation.MarkUp;
            _quotationToSave.Q1 = _curQuotation.Q1;
            _quotationToSave.Q2 = _curQuotation.Q2;
            _quotationToSave.Q3 = _curQuotation.Q3;
            _quotationToSave.Q4 = _curQuotation.Q4;
            _quotationToSave.Q5 = _curQuotation.Q5;
            _quotationToSave.Status = 9;
            _quotationToSave.Note = _curQuotation.Note;
            _quotationToSave.P1 = _curQuotation.P1.GetValueOrDefault(false);
            _quotationToSave.P2 = _curQuotation.P2.GetValueOrDefault(false);
            _quotationToSave.P3 = _curQuotation.P3.GetValueOrDefault(false);
            _quotationToSave.P4 = _curQuotation.P4.GetValueOrDefault(false);
            _quotationToSave.P5 = _curQuotation.P5.GetValueOrDefault(false);
            _quotationToSave.PriceCom = _curQuotation.PriceCom;
            _quotationToSave.PrintingMainText = _curQuotation.PrintingMainText;
            _quotationToSave.Draft = false;

            _quotationToSave.Note1 = _curQuotation.Note1;

            quotationDataContext.Quotations.InsertOnSubmit(_quotationToSave);

            foreach (var _quotationDetail in _curQuotation.TempQuotationDetails)
            {
                var _quotationToSaveDetail = new QuotationDetail();

                IEnumerable<PickingItem> _pickingItems = quotationDataContext.PickingItems;
                IEnumerable<MacroItem> _macroItems = quotationDataContext.MacroItems;

                if (_quotationDetail.CommonKey != null)
                {
                    var _toAddPickingItem = _pickingItems.First(pi => pi.ID == _quotationDetail.CommonKey);
                    //RecursiveDependenciesAdd(_curQuotation, _toAddPickingItem, _pickingItems);
                    var _quotationDetailToAdd = CreateDetail(_quotationToSave, _toAddPickingItem);
                    _quotationDetailToAdd.Quantity = _quotationDetail.Quantity;
                    _quotationToSave.QuotationDetails.Add(_quotationDetailToAdd);
                }
                else if (_quotationDetail.MacroItemKey != null)
                {
                    var _toAddPickingItem = _macroItems.First(pi => pi.ID == _quotationDetail.MacroItemKey);
                    //RecursiveDependenciesAdd(_curQuotation, _toAddPickingItem, _macroItems);
                    var _quotationDetailToAdd = CreateDetail(_quotationToSave, _toAddPickingItem);
                    _quotationDetailToAdd.Quantity = _quotationDetail.Quantity;
                    _quotationToSave.QuotationDetails.Add(_quotationDetailToAdd);
                }
                else
                {
                    var _quotationDetailToAdd = CreateDetail(_quotationToSave, _quotationDetail);
                    _quotationDetailToAdd.Quantity = _quotationDetail.Quantity;
                    _quotationToSave.QuotationDetails.Add(_quotationDetailToAdd);
                }
            }
            quotationDataContext.SubmitChanges();
            return _quotationToSave.ID;
        }

        protected void RecursiveDependenciesAdd(TempQuotation quotation, PickingItem toAddPickingItem,
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
                            if (quotation.TempQuotationDetails.FirstOrDefault(qd => qd.CommonKey == _item.ID) == null)
                            {
                                quotation.TempQuotationDetails.Add(CreateDetail(quotation, _item));
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

        protected void RecursiveDependenciesAdd(TempQuotation quotation, MacroItem toAddMacroItem,
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
                            if (quotation.TempQuotationDetails.FirstOrDefault(qd => qd.MacroItemKey == _item.ID) == null)
                            {
                                quotation.TempQuotationDetails.Add(CreateDetail(quotation, _item));
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

        protected void RecursiveDependenciesAdd(TempQuotation quotation, PickingItem toAddPickingItem,
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
                            if (quotation.TempQuotationDetails.FirstOrDefault(qd => qd.CommonKey == _item.ID) == null)
                            {
                                quotation.TempQuotationDetails.Add(CreateDetail(quotation, _item));
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
                        if (quotation.TempQuotationDetails.FirstOrDefault(qd => qd.MacroItemKey == _item.ID) == null)
                        {
                            quotation.TempQuotationDetails.Add(CreateDetail(quotation, _item));
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

        protected void RecursiveDependenciesAdd(TempQuotation quotation, MacroItem toAddMacroItem,
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
                            if (quotation.TempQuotationDetails.FirstOrDefault(qd => qd.MacroItemKey == _item.ID) == null)
                            {
                                quotation.TempQuotationDetails.Add(CreateDetail(quotation, _item));
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
                        if (quotation.TempQuotationDetails.FirstOrDefault(qd => qd.CommonKey == _item.ID) == null)
                        {
                            quotation.TempQuotationDetails.Add(CreateDetail(quotation, _item));
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

        protected TempQuotationDetail CreateDetail(TempQuotation quotation, PickingItem pickingItem)
        {
            var _toAddQuotationDetail = new TempQuotationDetail();
            _toAddQuotationDetail.ID_Quotation = quotation.ID_Quotation; //Convert.ToInt32(QuotationParameter);
            _toAddQuotationDetail.SessionUser = quotation.SessionUser;
            _toAddQuotationDetail.Cost = pickingItem.Cost;
            _toAddQuotationDetail.MarkUp = Convert.ToInt32(quotation.MarkUp);
            try
            {
                _toAddQuotationDetail.Price = pickingItem.Cost * Convert.ToDecimal(pickingItem.PercentageAuto / 100m) *
                                              Convert.ToDecimal(quotation.MarkUp / 100m);
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
            _toAddQuotationDetail.ID_Company = pickingItem.ID_Company;
            _toAddQuotationDetail.Position = pickingItem.Order;
            _toAddQuotationDetail.Inserted = pickingItem.Inserted;

            return _toAddQuotationDetail;
        }

        protected TempQuotationDetail CreateDetail(TempQuotation quotation, MacroItem pickingItem)
        {
            var _toAddQuotationDetail = new TempQuotationDetail();
            _toAddQuotationDetail.ID_Quotation = quotation.ID_Quotation; //Convert.ToInt32(QuotationParameter);
            _toAddQuotationDetail.SessionUser = quotation.SessionUser;
            _toAddQuotationDetail.Cost = pickingItem.CostCalc;
            _toAddQuotationDetail.MarkUp = Convert.ToInt32(quotation.MarkUp);
            try
            {
                _toAddQuotationDetail.Price = pickingItem.CostCalc * Convert.ToDecimal(pickingItem.PercentageCalc / 100m) *
                                              Convert.ToDecimal(quotation.MarkUp / 100m);
            }
            catch
            {
                _toAddQuotationDetail.Price = pickingItem.CostCalc * Convert.ToDecimal(pickingItem.PercentageCalc / 100m);
            }
            _toAddQuotationDetail.Multiply = pickingItem.Multiply;
            _toAddQuotationDetail.Percentage = pickingItem.PercentageCalc;
            _toAddQuotationDetail.SupplierCode = INTERNAL_SUPPLIERCODE;
            _toAddQuotationDetail.TypeCode = pickingItem.TypeCode; //Convert.ToInt32(TypeParameter);
            _toAddQuotationDetail.UM = pickingItem.UM;
            _toAddQuotationDetail.ItemTypeDescription = "[" + pickingItem.MacroItemDescription + "]";
            _toAddQuotationDetail.ItemTypeCode = pickingItem.ItemTypeCode;
            _toAddQuotationDetail.MacroItemKey = pickingItem.ID;
            _toAddQuotationDetail.ID_Company = pickingItem.ID_Company;
            _toAddQuotationDetail.Position = pickingItem.Order;
            _toAddQuotationDetail.Inserted = pickingItem.Inserted;
            _toAddQuotationDetail.Save = true; // il campo Save identifica se la riga è una macrovoce!

            return _toAddQuotationDetail;
        }

        protected QuotationDetail CreateDetail(Quotation quotation, TempQuotationDetail quotationDetail)
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

        protected TempQuotationDetail CreateDetail(TempQuotation quotation, QuotationDetail quotationDetail)
        {
            var _toAddQuotationDetail = new TempQuotationDetail();
            _toAddQuotationDetail.ID_Quotation = quotation.ID_Quotation; //Convert.ToInt32(QuotationParameter);
            _toAddQuotationDetail.SessionUser = quotation.SessionUser;
            _toAddQuotationDetail.ID_QuotationDetail = quotationDetail.ID;
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

        protected bool ActivateSavedQuotation(int idQuotation)
        {

            bool canModify = true;
            if (Convert.ToInt32(QuotationParameter) > 0)
            {
                canModify = (new QuotationDataContext().ProductionOrders.Where(po => po.ID_Quotation == Convert.ToInt32(QuotationParameter) && (po.Status == 3)).Count() == 0);
            }

            var _conn =
                new SqlConnection(ConfigurationManager.ConnectionStrings["LabExtimConnectionString"].ConnectionString);
            SqlTransaction _trans = null;
            var _qc = new QuotationDataContext(_conn);
            var _error = false;

            try
            {
                _conn.Open();
                _trans = _conn.BeginTransaction();
                _qc.Transaction = _trans;

                //TempQuotation _curTempQuotation = _qc.TempQuotations.FirstOrDefault(Quotation => Quotation.ID_Quotation == idQuotation && Quotation.SessionUser == GetCurrentEmployee().ID);
                //if (_curTempQuotation != null)
                //{
                //    TempQuotation _toDeleteTempQuotation = _qc.TempQuotations.Single(Quotation => Quotation.ID_Quotation == idQuotation);
                //    _qc.TempQuotationDetails.DeleteAllOnSubmit(_toDeleteTempQuotation.TempQuotationDetails);
                //    _qc.SubmitChanges();
                //    _qc.TempQuotations.DeleteOnSubmit(_toDeleteTempQuotation);
                //    _qc.SubmitChanges();
                //}

                foreach (
                    var _toDeleteTempQuotation in
                        _qc.TempQuotations.Where(
                            Quotation =>
                                Quotation.ID_Quotation == idQuotation &&
                                Quotation.SessionUser == GetCurrentEmployee().ID))
                {
                    _qc.TempQuotationDetails.DeleteAllOnSubmit(_toDeleteTempQuotation.TempQuotationDetails);
                    _qc.SubmitChanges();
                    _qc.TempQuotations.DeleteOnSubmit(_toDeleteTempQuotation);
                    _qc.SubmitChanges();
                }

                var _curQuotation = _qc.Quotations.First(Quotation => Quotation.ID == idQuotation);

                var _quotationToActivate = new TempQuotation();
                _quotationToActivate.ID_Quotation = _curQuotation.ID;
                _quotationToActivate.Number = _curQuotation.Number;
                _quotationToActivate.Subject = _curQuotation.Subject;
                _quotationToActivate.ID_Company = CurrentCompanyId;
                _quotationToActivate.ID_Manager = _curQuotation.ID_Manager;
                _quotationToActivate.CustomerCode = _curQuotation.CustomerCode;
                _quotationToActivate.SessionUser = GetCurrentEmployee().ID;
                if (_curQuotation.ID_Owner == null)
                {
                    _quotationToActivate.ID_Owner = GetCurrentEmployee().ID;
                }
                else
                {
                    _quotationToActivate.ID_Owner = _curQuotation.ID_Owner;
                }



                //_quotationToActivate.Date = (canModify ? DateTime.Now : _curQuotation.Date);
                _quotationToActivate.Date = _curQuotation.Date;
                _quotationToActivate.UpdateDate = _curQuotation.UpdateDate;
                _quotationToActivate.MarkUp = _curQuotation.MarkUp;
                _quotationToActivate.Q1 = _curQuotation.Q1;
                _quotationToActivate.Q2 = _curQuotation.Q2;
                _quotationToActivate.Q3 = _curQuotation.Q3;
                _quotationToActivate.Q4 = _curQuotation.Q4;
                _quotationToActivate.Q5 = _curQuotation.Q5;
                _quotationToActivate.Status = 9;
                _quotationToActivate.Note = _curQuotation.Note;
                _quotationToActivate.P1 = _curQuotation.P1.GetValueOrDefault(false);
                _quotationToActivate.P2 = _curQuotation.P2.GetValueOrDefault(false);
                _quotationToActivate.P3 = _curQuotation.P3.GetValueOrDefault(false);
                _quotationToActivate.P4 = _curQuotation.P4.GetValueOrDefault(false);
                _quotationToActivate.P5 = _curQuotation.P5.GetValueOrDefault(false);
                _quotationToActivate.PriceCom = _curQuotation.PriceCom;
                _quotationToActivate.PrintingMainText = _curQuotation.PrintingMainText;
                _quotationToActivate.Draft = false;

                _quotationToActivate.Note1 = _curQuotation.Note1;

                _qc.TempQuotations.InsertOnSubmit(_quotationToActivate);

                foreach (var _quotationDetail in _curQuotation.QuotationDetails)
                {
                    var _quotationToSaveDetail = new TempQuotationDetail();

                    IEnumerable<PickingItem> _pickingItems = _qc.PickingItems;
                    IEnumerable<MacroItem> _macroItems = _qc.MacroItems;

                    if (_quotationDetail.CommonKey != null)
                    {
                        var _toAddPickingItem = _pickingItems.First(pi => pi.ID == _quotationDetail.CommonKey);
                        //RecursiveDependenciesAdd(_curQuotation, _toAddPickingItem, _pickingItems);
                        var _quotationDetailToAdd = CreateDetail(_quotationToActivate, _toAddPickingItem);
                        _quotationDetailToAdd.ID_QuotationDetail = _quotationDetail.ID;
                        _quotationDetailToAdd.Quantity = _quotationDetail.Quantity;
                        _quotationToActivate.TempQuotationDetails.Add(_quotationDetailToAdd);
                    }
                    else if (_quotationDetail.MacroItemKey != null)
                    {
                        var _toAddPickingItem = _macroItems.First(pi => pi.ID == _quotationDetail.MacroItemKey);
                        //RecursiveDependenciesAdd(_curQuotation, _toAddPickingItem, _macroItems);
                        var _quotationDetailToAdd = CreateDetail(_quotationToActivate, _toAddPickingItem);
                        _quotationDetailToAdd.ID_QuotationDetail = _quotationDetail.ID;
                        _quotationDetailToAdd.Quantity = _quotationDetail.Quantity;
                        _quotationToActivate.TempQuotationDetails.Add(_quotationDetailToAdd);
                    }
                    else
                    {
                        var _quotationDetailToAdd = CreateDetail(_quotationToActivate, _quotationDetail);
                        _quotationDetailToAdd.ID_QuotationDetail = _quotationDetail.ID;
                        _quotationDetailToAdd.Quantity = _quotationDetail.Quantity;
                        _quotationToActivate.TempQuotationDetails.Add(_quotationDetailToAdd);
                    }
                }
                _qc.SubmitChanges();
                _trans.Commit();
            }
            catch
            {
                _trans.Rollback();
                _error = true;
            }
            finally
            {
                _qc.Dispose();
                _conn.Close();
            }
            return _error;
        }

        protected static bool IsSyncronized(string quotationParameter, int sessionUser)
        {
            var _isSyncronized = true;
            using (var QuotationDataContext = new QuotationDataContext())
            {
                try
                {
                    var _curTempQuotation =
                        QuotationDataContext.TempQuotations.First(
                            Quotation =>
                                Quotation.ID_Quotation == Convert.ToInt32(quotationParameter) &&
                                Quotation.SessionUser == sessionUser);
                    var _curQuotation =
                        QuotationDataContext.Quotations.FirstOrDefault(
                            Quotation => Quotation.ID == Convert.ToInt32(quotationParameter));
                    if (_curQuotation != null)
                    {
                        if (_curTempQuotation.Customer.Code != _curQuotation.Customer.Code) _isSyncronized = false;
                        if (_curTempQuotation.Subject != _curQuotation.Subject) _isSyncronized = false;
                        if (_curTempQuotation.ID_Company != _curQuotation.ID_Company) _isSyncronized = false;
                        if (_curTempQuotation.ID_Manager != _curQuotation.ID_Manager) _isSyncronized = false;
                        if (_curTempQuotation.CustomerCode != _curQuotation.CustomerCode) _isSyncronized = false;
                        if (_curTempQuotation.MarkUp != _curQuotation.MarkUp) _isSyncronized = false;
                        if (_curTempQuotation.Q1 != _curQuotation.Q1) _isSyncronized = false;
                        if (_curTempQuotation.Q2 != _curQuotation.Q2) _isSyncronized = false;
                        if (_curTempQuotation.Q3 != _curQuotation.Q3) _isSyncronized = false;
                        if (_curTempQuotation.Q4 != _curQuotation.Q4) _isSyncronized = false;
                        if (_curTempQuotation.Q5 != _curQuotation.Q5) _isSyncronized = false;
                        if (_curTempQuotation.Status != _curQuotation.Status) _isSyncronized = false;
                        if (_curTempQuotation.Note != _curQuotation.Note) _isSyncronized = false;
                        if (_curTempQuotation.P1.GetValueOrDefault(false) != _curQuotation.P1.GetValueOrDefault(false)) _isSyncronized = false;
                        if (_curTempQuotation.P2.GetValueOrDefault(false) != _curQuotation.P2.GetValueOrDefault(false)) _isSyncronized = false;
                        if (_curTempQuotation.P3.GetValueOrDefault(false) != _curQuotation.P3.GetValueOrDefault(false)) _isSyncronized = false;
                        if (_curTempQuotation.P4.GetValueOrDefault(false) != _curQuotation.P4.GetValueOrDefault(false)) _isSyncronized = false;
                        if (_curTempQuotation.P5.GetValueOrDefault(false) != _curQuotation.P5.GetValueOrDefault(false)) _isSyncronized = false;
                        if (_curTempQuotation.PriceCom != _curQuotation.PriceCom) _isSyncronized = false;
                        if (_curTempQuotation.PrintingMainText != _curQuotation.PrintingMainText) _isSyncronized = false;
                        if (_curTempQuotation.Draft != _curQuotation.Draft) _isSyncronized = false;

                        if (_curTempQuotation.Note1 != _curQuotation.Note1) _isSyncronized = false;

                        if (_curTempQuotation.TempQuotationDetails.Count != _curQuotation.QuotationDetails.Count)
                        {
                            _isSyncronized = false;
                        }
                        else
                        {
                            foreach (var _tempDetail in _curTempQuotation.TempQuotationDetails)
                            {
                                var _detail =
                                    QuotationDataContext.QuotationDetails.FirstOrDefault(
                                        QuotationDetail => QuotationDetail.ID == _tempDetail.ID_QuotationDetail);

                                if (_detail == null)
                                {
                                    _isSyncronized = false;
                                }
                                else
                                {
                                    if (_tempDetail.Cost != _detail.Cost) _isSyncronized = false;
                                    if (_tempDetail.MarkUp != _detail.MarkUp) _isSyncronized = false;
                                    if (_tempDetail.Price != _detail.Price) _isSyncronized = false;
                                    if (_tempDetail.Multiply != _detail.Multiply) _isSyncronized = false;
                                    if (_tempDetail.Percentage != _detail.Percentage) _isSyncronized = false;
                                    if (_tempDetail.SupplierCode != _detail.SupplierCode) _isSyncronized = false;
                                    if (_tempDetail.TypeCode != _detail.TypeCode) _isSyncronized = false;
                                    if (_tempDetail.UM != _detail.UM) _isSyncronized = false;
                                    if (_tempDetail.ItemTypeDescription != _detail.ItemTypeDescription)
                                        _isSyncronized = false;
                                    if (_tempDetail.ItemTypeCode != _detail.ItemTypeCode) _isSyncronized = false;
                                    if (_tempDetail.Position != _detail.Position) _isSyncronized = false;
                                    if (_tempDetail.Inserted != _detail.Inserted) _isSyncronized = false;
                                    if (_tempDetail.ID_Company != _detail.ID_Company) _isSyncronized = false;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    return false;
                }

                return _isSyncronized;
            }
        }





    }
}