using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Data.Linq;
using System.Web.UI.WebControls;
using CMLabExtim;
using DLLabExtim;

namespace UILabExtim
{
    public class ProductionOrderDetailsInsertController : QuotationController
    {

        public List<TempProductionOrderDetail> TempProductionOrderDetails
        {
            get
            {
                if (Session["TempProductionOrderDetails"] == null)
                {
                    Session["TempProductionOrderDetails"] = new List<TempProductionOrderDetail>();
                }
                return (List<TempProductionOrderDetail>)Session["TempProductionOrderDetails"];
            }
            set { Session["TempProductionOrderDetails"] = value; }
        }

        protected List<QuotationDetailForProduction> GetQuotationDetailsForProduction(Quotation quotation)
        {
            var _results = new List<QuotationDetailForProduction>();

            foreach (var _quotationDetail in quotation.QuotationDetails)
            {
                if (_quotationDetail.PickingItem != null)
                {
                    if (_quotationDetail.PickingItem.ItemDisplayMode.ID == 1 ||
                        _quotationDetail.PickingItem.ItemDisplayMode.ID == 2)
                    {
                        var _temp = new QuotationDetailForProduction(_quotationDetail, _quotationDetail.PickingItem);
                        _temp.Quantity = 0;
                        _results.Add(_temp);
                    }
                }
                if (_quotationDetail.MacroItem != null)
                {
                    foreach (var _macroItemDetail in _quotationDetail.MacroItem.MacroItemDetails)
                    {
                        if (_macroItemDetail.PickingItem.ItemDisplayMode.ID == 1 ||
                            _macroItemDetail.PickingItem.ItemDisplayMode.ID == 2)
                        {
                            var _temp = new QuotationDetailForProduction(_quotationDetail, _macroItemDetail.PickingItem);
                            _temp.Quantity = 0;
                            _results.Add(_temp);
                        }
                    }
                }
            }
            return _results;
        }

        protected PickingItem GetPickingItem(int? id)
        {
            var _context = new QuotationDataContext();
            return _context.PickingItems.FirstOrDefault(pi => pi.ID == id);
        }

        public static PickingItem GetPickingItemFromMacroOrPickingItem(int? id, string prefix)
        {
            if (id != null)
            {
                PickingItem _pickingItem;
                using (var _context = new QuotationDataContext())
                {
                    if (prefix == "M")
                    {
                        var _macroItem = _context.MacroItems.FirstOrDefault(pi => pi.ID == id);
                        _pickingItem = new PickingItem
                        {
                            Cost = _macroItem.Cost.GetValueOrDefault(0m),
                            Date = _macroItem.Date,
                            ID = _macroItem.ID,
                            Inserted = _macroItem.Inserted,
                            ItemDescription = _macroItem.MacroItemDescription,
                            ItemManufacturing =
                                (_macroItem.ItemManufacturing.HasValue
                                    ? Convert.ToInt32(_macroItem.ItemManufacturing)
                                    : 0),
                            //ItemType = _macroItem.ItemType,
                            ItemTypeCode = _macroItem.ItemTypeCode,
                            Link = _macroItem.Link,
                            Multiply = _macroItem.Multiply,
                            Order = _macroItem.Order,
                            Percentage = _macroItem.Percentage,
                            //Type = _macroItem.Type,
                            TypeCode = _macroItem.TypeCode,
                            UM = _macroItem.UM
                        };
                    }
                    else //if (prefix == "P")
                    {
                        _pickingItem = _context.PickingItems.FirstOrDefault(pi => pi.ID == id);
                    }
                    //else return null;
                    return _pickingItem;
                }
            }
            return null;
        }

        public static ProductionOrderDetail GetProductionOrderDetail(QuotationDataContext context, int id)
        {
            return context.ProductionOrderDetails.FirstOrDefault(pi => pi.ID == id);
        }

        public static DeliveryTripDetail GetDeliveryTripDetail(QuotationDataContext context, int id)
        {
            return context.DeliveryTripDetails.FirstOrDefault(pi => pi.ID == id);
        }

        public static DeliveryTrip GetDeliveryTrip(QuotationDataContext context, int id)
        {
            return context.DeliveryTrips.FirstOrDefault(pi => pi.ID == id);
        }

        public static List<DeliveryTrip> GetDeliveryTrips(QuotationDataContext context, int? customerCode, int? status)
        {

            var dataOptions = new DataLoadOptions();
            dataOptions.LoadWith<DeliveryTrip>(c => c.Employee);
            dataOptions.LoadWith<DeliveryTrip>(c => c.Customer);
            dataOptions.LoadWith<DeliveryTrip>(c => c.Location);
            context.LoadOptions = dataOptions;
            var data = context.DeliveryTrips.AsQueryable();

            if (customerCode != null)
            {
                data = data.Where(d => d.CustomerCode == customerCode.Value);
            }

            if (status != null)
            {
                data = data.Where(d => d.Status == status.Value);
            }
            return data.ToList();
        }

        /// <summary>
        /// Restituisce l'oggetto tempproductionorderdetails immpostato con l'ordine di produzione impostato
        /// </summary>
        /// <param name="productionOrderDetail">dettaglio ordine di produzione da trasformare in temp</param>
        /// <param name="freeType">Indica se impostare anche gli elementi freetype (mantenuto per uniformità - bisogna capire cosa fanno sti freetype, erano presenti solo nella fuznione che li recupera per data.</param>
        /// <returns></returns>
        public static TempProductionOrderDetail GetTempFromProductionOrderDetail(ProductionOrderDetail productionOrderDetail, bool freeType)
        {
            var _tempProductionOrderDetail = new TempProductionOrderDetail();
            _tempProductionOrderDetail.ID = productionOrderDetail.ID;
            _tempProductionOrderDetail.Cost = productionOrderDetail.Cost;
            _tempProductionOrderDetail.DirectSupply = productionOrderDetail.DirectSupply;
            _tempProductionOrderDetail.QuantityOver = productionOrderDetail.QuantityOver;
            _tempProductionOrderDetail.ID_Owner = productionOrderDetail.ID_Owner;
            _tempProductionOrderDetail.ID_Company = productionOrderDetail.ID_Company;
            _tempProductionOrderDetail.ID_Phase = productionOrderDetail.ID_Phase;
            _tempProductionOrderDetail.ID_PickingItem = productionOrderDetail.ID_PickingItem;
            _tempProductionOrderDetail.RFlag = productionOrderDetail.RFlag;
            _tempProductionOrderDetail.ID_ProductionOrder = productionOrderDetail.ID_ProductionOrder;
            _tempProductionOrderDetail.Note = productionOrderDetail.Note;
            _tempProductionOrderDetail.ProducedQuantity = productionOrderDetail.ProducedQuantity;
            _tempProductionOrderDetail.ProductionDate = productionOrderDetail.ProductionDate;
            _tempProductionOrderDetail.ProductionTime = productionOrderDetail.ProductionTime;
            _tempProductionOrderDetail.RawMaterialQuantity = productionOrderDetail.RawMaterialQuantity;
            _tempProductionOrderDetail.ID_QuotationDetail = productionOrderDetail.ID_QuotationDetail;
            _tempProductionOrderDetail.ID_PickingItemSup = productionOrderDetail.ID_PickingItemSup;
            _tempProductionOrderDetail.SFlag = productionOrderDetail.SFlag;
            _tempProductionOrderDetail.UMUser = productionOrderDetail.UMUser;
            _tempProductionOrderDetail.RawMaterialX = productionOrderDetail.RawMaterialX;
            _tempProductionOrderDetail.RawMaterialY = productionOrderDetail.RawMaterialY;
            _tempProductionOrderDetail.RawMaterialZ = productionOrderDetail.RawMaterialZ;
            _tempProductionOrderDetail.SupplierCode = productionOrderDetail.SupplierCode;
            _tempProductionOrderDetail.SupplierCodeSup = productionOrderDetail.SupplierCodeSup;
            _tempProductionOrderDetail.UMProduct = productionOrderDetail.UMProduct;
            _tempProductionOrderDetail.UMRawMaterial = productionOrderDetail.UMRawMaterial;

            if (freeType)
            {
                _tempProductionOrderDetail.FreeTypeCode = productionOrderDetail.FreeTypeCode;
                _tempProductionOrderDetail.FreeItemTypeCode = productionOrderDetail.FreeItemTypeCode;
                _tempProductionOrderDetail.FreeItemDescription = productionOrderDetail.FreeItemDescription;
            }

            _tempProductionOrderDetail.OkCopiesCount = productionOrderDetail.OkCopiesCount;
            _tempProductionOrderDetail.KoCopiesCount = productionOrderDetail.KoCopiesCount;
            _tempProductionOrderDetail.Special = productionOrderDetail.Special;


            using (QuotationDataContext db = new QuotationDataContext())
            {
                ProductionOrderTechSpec pts = db.ProductionOrderTechSpecs.FirstOrDefault(d => d.ID_ProductionOrderDetail == productionOrderDetail.ID);
                if (pts != null)
                {
                    _tempProductionOrderDetail.CodiceMarcaInchiostro = pts.CodiceMarcaInchiostro;
                    _tempProductionOrderDetail.Ricetta = pts.Ricetta;
                    _tempProductionOrderDetail.TelaioNumeroFili = pts.TelaioNumeroFili;
                    _tempProductionOrderDetail.GelatinaSpessore = pts.GelatinaSpessore;
                    _tempProductionOrderDetail.RaclaInclinazione = pts.RaclaInclinazione;
                    _tempProductionOrderDetail.RaclaDurezzaSpigolo = pts.RaclaDurezzaSpigolo;

                    _tempProductionOrderDetail.CodiceMarcaFilm = pts.CodiceMarcaFilm;
                    _tempProductionOrderDetail.ClicheReso = pts.ClicheReso;
                    _tempProductionOrderDetail.ClicheCondizioni = pts.ClicheCondizioni;
                    _tempProductionOrderDetail.StampaTemperatura = pts.StampaTemperatura;
                    _tempProductionOrderDetail.AltreInfo = pts.AltreInfo;

                    _tempProductionOrderDetail.FustellaResa = pts.FustellaResa;
                    _tempProductionOrderDetail.FustellaCondizioni = pts.FustellaCondizioni;
                    _tempProductionOrderDetail.ControCordonatori = pts.ControCordonatori;

                    _tempProductionOrderDetail.AltreNoteDaProduzione = pts.AltreNoteDaProduzione;

                }
                _tempProductionOrderDetail.ProductionOrderNumber = db.ProductionOrders.First(d => d.ID == productionOrderDetail.ID_ProductionOrder).Number;
            }

            _tempProductionOrderDetail.State = TempProductionOrderDetail.ItemState.Passed;
            _tempProductionOrderDetail.FFlag = productionOrderDetail.FFlag;

            return _tempProductionOrderDetail;
        }

        protected List<TempProductionOrderDetail> GetProductionOrderDetailsOfADate(DateTime date, int idCompany)
        {

            var _listTempProductionOrderDetails = new List<TempProductionOrderDetail>();
            var _listProductionOrderDetails = new List<ProductionOrderDetail>();
            var _context = new QuotationDataContext();
            _listProductionOrderDetails =
                _context.ProductionOrderDetails.Where(pod => pod.ProductionDate == date && pod.FreeTypeCode != null && (idCompany == -1 || pod.ProductionOrder.ID_Company == idCompany))
                    .ToList();
            foreach (var _productionOrderDetail in _listProductionOrderDetails)
            {
                _listTempProductionOrderDetails.Add(GetTempFromProductionOrderDetail(_productionOrderDetail, true));
            }
            return _listTempProductionOrderDetails;
        }

        public List<TempProductionOrderDetail> GetProductionOrderDetailsOfAnOwner(int? owner, DateTime? date, int idCompany, int idProductionOrder = 0)
        {



            var _listTempProductionOrderDetails = new List<TempProductionOrderDetail>();


            var _listProductionOrderDetails = new List<ProductionOrderDetail>();
            var _context = new QuotationDataContext();
            IQueryable<ProductionOrderDetail> data = _context.ProductionOrderDetails.AsQueryable();

            if (date.GetValueOrDefault() == DateTime.MinValue) { date = null; }
            if (date != null)
                data = data.Where(d => d.ProductionDate == date);
            if (owner.GetValueOrDefault() != 0)
                data = data.Where(d => d.ID_Owner == owner);
            if (idProductionOrder != 0)
                data = data.Where(d => d.ID_ProductionOrder == idProductionOrder);
            if (idCompany != 0)
                data = data.Where(d => d.ID_Company == idCompany);
            if (owner == null && date == null && idProductionOrder != 0)
                data = data.Where(d => d.MacroRef == null);
            else
                data = data.Where(d => d.FreeTypeCode == null && d.MacroRef == null);

            if (owner.GetValueOrDefault() == 0 && date == null && idProductionOrder == 0)
                data = data.Take(0);
            if (owner.GetValueOrDefault() != 0 && date == null && idProductionOrder == 0)
                data = data.Take(0);

            _listProductionOrderDetails = data.OrderBy(d => d.ProductionDate).ThenBy(d => d.Employee.UniqueName).ToList();

            //if (owner != null && owner != 0 && date != null && idProductionOrder != 0)
            //{
            //    _listProductionOrderDetails =
            //        _context.ProductionOrderDetails.Where(
            //            pod =>
            //                pod.ID_ProductionOrder == idProductionOrder && pod.ProductionDate == date && pod.ID_Owner == owner && pod.FreeTypeCode == null &&
            //                pod.MacroRef == null).ToList();
            //}
            //else if (owner != null && owner != 0 && date == null && idProductionOrder != 0)
            //{
            //    _listProductionOrderDetails =
            //        _context.ProductionOrderDetails.Where(
            //            pod =>
            //                pod.ID_ProductionOrder == idProductionOrder && pod.ID_Owner == owner && pod.FreeTypeCode == null &&
            //                pod.MacroRef == null && (idCompany == -1 || pod.ProductionOrder.ID_Company == idCompany)).ToList();
            //}
            //else if (owner != null && owner != 0)
            //{
            //    _listProductionOrderDetails =
            //        _context.ProductionOrderDetails.Where(
            //            pod =>
            //                pod.ProductionDate == date && pod.ID_Owner == owner && pod.FreeTypeCode == null &&
            //                pod.MacroRef == null).ToList();
            //}
            //else if (owner == 0 && date == null && idProductionOrder != 0)
            //{
            //    _listProductionOrderDetails =
            //        _context.ProductionOrderDetails.Where(
            //            pod =>
            //                pod.ID_ProductionOrder == idProductionOrder &&
            //                pod.MacroRef == null).ToList();
            //}
            //else
            //{
            //    _listProductionOrderDetails =
            //        _context.ProductionOrderDetails.Where(
            //            pod => pod.ProductionDate == date && pod.FreeTypeCode == null && pod.MacroRef == null && (idCompany == -1 || pod.ProductionOrder.ID_Company == idCompany))
            //            .OrderBy(pod => pod.Employee.UniqueName)
            //            .ToList();
            //}




            foreach (var _productionOrderDetail in _listProductionOrderDetails)
            {
                _listTempProductionOrderDetails.Add(GetTempFromProductionOrderDetail(_productionOrderDetail, false));
            }

            return _listTempProductionOrderDetails;
        }


        public List<DeliveryTrip> GetDeliveryTripsOfAnOwner(int? owner, DateTime? date, int idCompany)
        {
            var _listTempDeliveryTrips = new List<DeliveryTrip>();
            var _listDeliveryTrips = new List<DeliveryTrip>();

            using (QuotationDataContext _context = new QuotationDataContext())
            {

                var dataOptions = new DataLoadOptions();
                dataOptions.LoadWith<DeliveryTrip>(c => c.Employee);
                dataOptions.LoadWith<DeliveryTrip>(c => c.Customer);
                dataOptions.LoadWith<DeliveryTrip>(c => c.Location);
                _context.LoadOptions = dataOptions;

                if (owner != null && owner != 0 && date != null)
                {
                    _listDeliveryTrips =
                        _context.DeliveryTrips.Where(
                            pod =>
                                pod.StartDate == date && pod.ID_Owner == owner && pod.Status == 0).ToList();
                }
                else if (owner != null && owner != 0 && date == null)
                {
                    _listDeliveryTrips =
                        _context.DeliveryTrips.Where(
                            pod =>
                                pod.ID_Owner == owner && pod.Status == 0 && (idCompany == -1 || pod.ID_Company == idCompany)).ToList();
                }
                else if (owner != null && owner != 0)
                {
                    _listDeliveryTrips =
                        _context.DeliveryTrips.Where(
                            pod =>
                                pod.ID_Owner == owner && pod.Status == 0).ToList();
                }
                else
                {
                    _listDeliveryTrips =
                        _context.DeliveryTrips.Where(
                            pod => pod.Status == 0 && (idCompany == -1 || pod.ID_Company == idCompany))
                            .ToList();
                }
                foreach (var _deliveryTrip in _listDeliveryTrips)
                {
                    _listTempDeliveryTrips.Add(_deliveryTrip);
                }
            }
            return _listTempDeliveryTrips;
        }

        public List<DeliveryTripDetail> GetDeliveryTripDetailsOfAnOwner(int? owner, DateTime? date, int idCompany, int idDeliveryTrip = 0)
        {
            var _listTempDeliveryTripDetails = new List<DeliveryTripDetail>();
            var _listDeliveryTripDetails = new List<DeliveryTripDetail>();

            using (QuotationDataContext _context = new QuotationDataContext())
            {

                var dataOptions = new DataLoadOptions();
                dataOptions.LoadWith<DeliveryTripDetail>(c => c.DeliveryTrip);
                dataOptions.LoadWith<DeliveryTrip>(c => c.Employee);
                dataOptions.LoadWith<DeliveryTrip>(c => c.Customer);
                dataOptions.LoadWith<DeliveryTrip>(c => c.Location);
                _context.LoadOptions = dataOptions;

                if (owner != null && owner != 0 && date != null && idDeliveryTrip != 0)
                {
                    _listDeliveryTripDetails =
                        _context.DeliveryTripDetails.Where(
                            pod =>
                                pod.ID_DeliveryTrip == idDeliveryTrip && pod.DeliveryTrip.StartDate == date && pod.DeliveryTrip.ID_Owner == owner && pod.DeliveryTrip.Status == 0).ToList();
                }
                else if (owner != null && owner != 0 && date == null && idDeliveryTrip != 0)
                {
                    _listDeliveryTripDetails =
                        _context.DeliveryTripDetails.Where(
                            pod =>
                                pod.ID_DeliveryTrip == idDeliveryTrip && pod.DeliveryTrip.ID_Owner == owner && pod.DeliveryTrip.Status == 0 && (idCompany == -1 || pod.ProductionOrder.ID_Company == idCompany)).ToList();
                }
                else if (owner != null && owner != 0)
                {
                    _listDeliveryTripDetails =
                        _context.DeliveryTripDetails.Where(
                            pod =>
                                pod.DeliveryTrip.ID_Owner == owner && pod.DeliveryTrip.Status == 0).ToList();
                }
                else
                {
                    _listDeliveryTripDetails =
                        _context.DeliveryTripDetails.Where(
                            pod => pod.DeliveryTrip.Status == 0 && (idCompany == -1 || pod.ProductionOrder.ID_Company == idCompany))
                            .ToList();
                }
                foreach (var _deliveryTripDetail in _listDeliveryTripDetails)
                {
                    _listTempDeliveryTripDetails.Add(_deliveryTripDetail);
                }
            }
            return _listTempDeliveryTripDetails;
        }


        public List<DeliveryTripDetail> GetDeliveryTripDetailsOfAProductionOrder(int IdProductionOrder)
        {

            using (QuotationDataContext ctx = new QuotationDataContext())
            {
                return ctx.DeliveryTripDetails.Where(d => d.ID_ProductionOrder == IdProductionOrder).ToList();
            }

        }


        public List<TempProductionOrderDetail> GetProductionOrderDetailsOfAnOdPId(int? productionOrderId)
        {
            var _listTempProductionOrderDetails = new List<TempProductionOrderDetail>();


            var _listProductionOrderDetails = new List<ProductionOrderDetail>();
            var _context = new QuotationDataContext();

            _listProductionOrderDetails =
                _context.ProductionOrderDetails.Where(
                    pod =>
                        pod.ID_ProductionOrder == productionOrderId && pod.FreeTypeCode == null && pod.MacroRef == null)
                    .ToList();

            foreach (var _productionOrderDetail in _listProductionOrderDetails)
            {
                _listTempProductionOrderDetails.Add(GetTempFromProductionOrderDetail(_productionOrderDetail, false));
            }

            return _listTempProductionOrderDetails;
        }

        protected void SubmitFreeTypeRow(int i, string date, int currentCompanyId)
        {
            if (TempProductionOrderDetails[i].ID_ProductionOrder != 0)
            {

                using (var _quotationDataContext = new QuotationDataContext())
                {

                    //if (_quotationDataContext.ProductionOrders.FirstOrDefault(d => d.ID == TempProductionOrderDetails[i].ID_ProductionOrder).ID_Company != CurrentCompanyId)
                    //{
                    //    throw new Exception("Impossibile procedere: l'azienda dell'OdP è diversa da quella corrente!");
                    //}

                    ProductionOrderDetail _productionOrderDetail = null;
                    var _isNew = false;
                    _productionOrderDetail = GetProductionOrderDetail(_quotationDataContext,
                        TempProductionOrderDetails[i].ID);
                    if (_productionOrderDetail == null)
                    {
                        _productionOrderDetail = new ProductionOrderDetail();
                        _isNew = true;
                    }
                    else
                    {
                        _productionOrderDetail.ID = TempProductionOrderDetails[i].ID;
                    }
                    _productionOrderDetail.ID_Company = TempProductionOrderDetails[i].ID_Company;
                    _productionOrderDetail.ID_ProductionOrder = TempProductionOrderDetails[i].ID_ProductionOrder;
                    _productionOrderDetail.ID_PickingItem = TempProductionOrderDetails[i].ID_PickingItem;
                    _productionOrderDetail.SupplierCode = TempProductionOrderDetails[i].SupplierCode;
                    _productionOrderDetail.UMRawMaterial = TempProductionOrderDetails[i].UMRawMaterial;
                    _productionOrderDetail.RawMaterialQuantity = TempProductionOrderDetails[i].RawMaterialQuantity;
                    _productionOrderDetail.SupplierCodeSup = TempProductionOrderDetails[i].SupplierCodeSup;
                    _productionOrderDetail.Cost = TempProductionOrderDetails[i].Cost;
                    _productionOrderDetail.Note = TempProductionOrderDetails[i].Note;
                    _productionOrderDetail.ProductionDate = Convert.ToDateTime(date);
                    _productionOrderDetail.FreeTypeCode = TempProductionOrderDetails[i].FreeTypeCode;
                    _productionOrderDetail.FreeItemTypeCode = TempProductionOrderDetails[i].FreeItemTypeCode;
                    _productionOrderDetail.FreeItemDescription = TempProductionOrderDetails[i].FreeItemDescription;

                    if (_isNew)
                    {
                        _quotationDataContext.ProductionOrderDetails.InsertOnSubmit(_productionOrderDetail);
                    }
                    try
                    {
                        _quotationDataContext.SubmitChanges();
                    }
                    catch
                    {
                        throw;
                    }
                    if (_isNew)
                    {
                        TempProductionOrderDetails[i].ID = _productionOrderDetail.ID;
                    }
                    TempProductionOrderDetails[i].State = TempProductionOrderDetail.ItemState.Passed;

                }

            }
        }


        // versione in produzione
        //protected void SubmitRow(int i, string date, string idOwner)
        //{
        //    if (TempProductionOrderDetails[i].ID_ProductionOrder != 0)
        //    {
        //        //if (!IsOverProductionOrder(TempProductionOrderDetails[i].ID_ProductionOrder))
        //        //{


        //        using (var _quotationDataContext = new QuotationDataContext())
        //        {

        //            //if (_quotationDataContext.ProductionOrders.FirstOrDefault(d => d.ID == TempProductionOrderDetails[i].ID_ProductionOrder).ID_Company != CurrentCompanyId)
        //            //{
        //            //    throw new Exception("Impossibile procedere: l'azienda dell'OdP è diversa da quella corrente!");
        //            //}


        //            ProductionOrderDetail _productionOrderDetail = null;
        //            var _isNew = false;
        //            _productionOrderDetail = GetProductionOrderDetail(_quotationDataContext,
        //                TempProductionOrderDetails[i].ID);
        //            if (_productionOrderDetail == null)
        //            {
        //                _productionOrderDetail = new ProductionOrderDetail();
        //                _isNew = true;
        //            }
        //            else
        //            {
        //                _productionOrderDetail.ID = TempProductionOrderDetails[i].ID;
        //            }
        //            _productionOrderDetail.ID_ProductionOrder = TempProductionOrderDetails[i].ID_ProductionOrder;

        //            if (TempProductionOrderDetails[i].ID_Owner == 0)
        //                TempProductionOrderDetails[i].ID_Owner = null;
        //            //throw new Exception("ddlOwner", new Exception("3"));
        //            _productionOrderDetail.ID_Owner = idOwner != null
        //                ? Convert.ToInt32(idOwner)
        //                : TempProductionOrderDetails[i].ID_Owner;

        //            // merge aziendale
        //            //if (TempProductionOrderDetails[i].ID_Company == null && TempProductionOrderDetails[i].ID_Owner != null)
        //            //{
        //            //    TempProductionOrderDetails[i].ID_Company = _quotationDataContext.Employees.First(d => d.ID == _productionOrderDetail.ID_Owner).ID_Company;
        //            //}
        //            //if (TempProductionOrderDetails[i].ID_Company == null && TempProductionOrderDetails[i].ID_Owner == null)
        //            //{
        //            //    TempProductionOrderDetails[i].ID_Company = CurrentCompanyId;
        //            //}
        //            //_productionOrderDetail.ID_Company = TempProductionOrderDetails[i].ID_Company;

        //            if (TempProductionOrderDetails[i].ID_Phase == 0)
        //                TempProductionOrderDetails[i].ID_Phase = null;
        //            _productionOrderDetail.ID_Phase = TempProductionOrderDetails[i].ID_Phase;

        //            if (TempProductionOrderDetails[i].ProductionTime == null)
        //                TempProductionOrderDetails[i].ProductionTime = 0;
        //            _productionOrderDetail.ProductionTime = TempProductionOrderDetails[i].ProductionTime;

        //            if (TempProductionOrderDetails[i].ID_PickingItem == 0)
        //            {
        //                TempProductionOrderDetails[i].ID_PickingItem = null;
        //                TempProductionOrderDetails[i].RFlag = null;
        //            }
        //            _productionOrderDetail.ID_PickingItem = TempProductionOrderDetails[i].ID_PickingItem;
        //            _productionOrderDetail.RFlag = (TempProductionOrderDetails[i].RFlag == "M"
        //                ? TempProductionOrderDetails[i].RFlag
        //                : null);

        //            if (TempProductionOrderDetails[i].SupplierCode == 0)
        //                TempProductionOrderDetails[i].SupplierCode = null;
        //            _productionOrderDetail.SupplierCode = TempProductionOrderDetails[i].SupplierCode;

        //            if (TempProductionOrderDetails[i].UMRawMaterial == 0)
        //                TempProductionOrderDetails[i].UMRawMaterial = null;
        //            _productionOrderDetail.UMRawMaterial = TempProductionOrderDetails[i].UMRawMaterial;

        //            _productionOrderDetail.RawMaterialQuantity = TempProductionOrderDetails[i].RawMaterialQuantity;

        //            if (TempProductionOrderDetails[i].UMUser == 0)
        //                TempProductionOrderDetails[i].UMUser = null;
        //            _productionOrderDetail.UMUser = TempProductionOrderDetails[i].UMUser;

        //            if (TempProductionOrderDetails[i].ID_PickingItemSup == 0)
        //            {
        //                TempProductionOrderDetails[i].ID_PickingItemSup = null;
        //                TempProductionOrderDetails[i].SFlag = null;
        //            }
        //            _productionOrderDetail.ID_PickingItemSup = TempProductionOrderDetails[i].ID_PickingItemSup;
        //            _productionOrderDetail.SFlag = (TempProductionOrderDetails[i].SFlag == "M"
        //                ? TempProductionOrderDetails[i].SFlag
        //                : null);

        //            if (TempProductionOrderDetails[i].SupplierCodeSup == 0)
        //                TempProductionOrderDetails[i].SupplierCodeSup = null;
        //            _productionOrderDetail.SupplierCodeSup = TempProductionOrderDetails[i].SupplierCodeSup;

        //            _productionOrderDetail.RawMaterialX = TempProductionOrderDetails[i].RawMaterialX;
        //            _productionOrderDetail.RawMaterialY = TempProductionOrderDetails[i].RawMaterialY;
        //            _productionOrderDetail.RawMaterialZ = TempProductionOrderDetails[i].RawMaterialZ;

        //            if (TempProductionOrderDetails[i].UMProduct == 0)
        //                TempProductionOrderDetails[i].UMProduct = null;
        //            _productionOrderDetail.UMProduct = TempProductionOrderDetails[i].UMProduct;

        //            _productionOrderDetail.ProducedQuantity = TempProductionOrderDetails[i].ProducedQuantity;
        //            _productionOrderDetail.QuantityOver = TempProductionOrderDetails[i].QuantityOver;
        //            _productionOrderDetail.DirectSupply = TempProductionOrderDetails[i].DirectSupply;
        //            _productionOrderDetail.Cost = TempProductionOrderDetails[i].Cost;
        //            _productionOrderDetail.HistoricalCostPhase = TempProductionOrderDetails[i].CostCalcPhase;
        //            _productionOrderDetail.HistoricalCostRawM = TempProductionOrderDetails[i].CostCalcRawM;
        //            _productionOrderDetail.HistoricalCostSupM = TempProductionOrderDetails[i].CostCalcSupM;
        //            _productionOrderDetail.Note = TempProductionOrderDetails[i].Note;
        //            _productionOrderDetail.ProductionDate = Convert.ToDateTime(date);

        //            _productionOrderDetail.FreeTypeCode = TempProductionOrderDetails[i].FreeTypeCode;
        //            if (TempProductionOrderDetails[i].FreeTypeCode != null)
        //            {
        //                _productionOrderDetail.ID_Phase = null;
        //                _productionOrderDetail.ProductionTime = 0;
        //            }
        //            _productionOrderDetail.FreeItemTypeCode = TempProductionOrderDetails[i].FreeItemTypeCode;
        //            _productionOrderDetail.FreeItemDescription = TempProductionOrderDetails[i].FreeItemDescription;

        //            //merge aziendale
        //            //if (TempProductionOrderDetails[i].ID_Company == null)
        //            //{
        //            TempProductionOrderDetails[i].ID_Company = GetCompanyIdFromTempProductionOrderDetail(_quotationDataContext, TempProductionOrderDetails[i]);
        //            //}
        //            _productionOrderDetail.ID_Company = TempProductionOrderDetails[i].ID_Company;

        //            if (_isNew)
        //            {
        //                _quotationDataContext.ProductionOrderDetails.InsertOnSubmit(_productionOrderDetail);
        //            }
        //            try
        //            {
        //                _quotationDataContext.SubmitChanges();
        //            }
        //            catch
        //            {
        //                throw;
        //            }
        //            if (_isNew)
        //            {
        //                TempProductionOrderDetails[i].ID = _productionOrderDetail.ID;
        //            }

        //            TempProductionOrderDetails[i].State = TempProductionOrderDetail.ItemState.Passed;
        //            SubmitUnderlyingDetails(TempProductionOrderDetails[i], Convert.ToDateTime(date), false);
        //        }
        //        //}
        //    }
        //}


        // versione da testare per ferraro (cambio al volo numero Odp su dettagglio della denuncia operazioni per OPERATORE)
        protected void SubmitRow(int i, string date, string idOwner)
        {
            if (TempProductionOrderDetails[i].ID_ProductionOrder != 0)
            {
                //if (!IsOverProductionOrder(TempProductionOrderDetails[i].ID_ProductionOrder))
                //{


                using (var _quotationDataContext = new QuotationDataContext())
                {

                    //if (_quotationDataContext.ProductionOrders.FirstOrDefault(d => d.ID == TempProductionOrderDetails[i].ID_ProductionOrder).ID_Company != CurrentCompanyId)
                    //{
                    //    throw new Exception("Impossibile procedere: l'azienda dell'OdP è diversa da quella corrente!");
                    //}


                    ProductionOrderDetail _productionOrderDetail = null;
                    var _isNew = false;
                    var _mustChangeOdp = false;
                    _productionOrderDetail = GetProductionOrderDetail(_quotationDataContext,
                        TempProductionOrderDetails[i].ID);
                    if (_productionOrderDetail == null)
                    {
                        _productionOrderDetail = new ProductionOrderDetail();
                        _isNew = true;
                    }
                    else if (_productionOrderDetail.ID_ProductionOrder != TempProductionOrderDetails[i].ID_ProductionOrder && string.IsNullOrEmpty(date))
                    {
                        _productionOrderDetail = new ProductionOrderDetail();
                        _mustChangeOdp = true;
                    }
                    else
                    {
                        _productionOrderDetail.ID = TempProductionOrderDetails[i].ID;
                    }
                    _productionOrderDetail.ID_ProductionOrder = TempProductionOrderDetails[i].ID_ProductionOrder;

                    if (TempProductionOrderDetails[i].ID_Owner == 0)
                        TempProductionOrderDetails[i].ID_Owner = null;
                    //throw new Exception("ddlOwner", new Exception("3"));
                    _productionOrderDetail.ID_Owner = idOwner != null
                        ? Convert.ToInt32(idOwner)
                        : TempProductionOrderDetails[i].ID_Owner;

                    // merge aziendale
                    //if (TempProductionOrderDetails[i].ID_Company == null && TempProductionOrderDetails[i].ID_Owner != null)
                    //{
                    //    TempProductionOrderDetails[i].ID_Company = _quotationDataContext.Employees.First(d => d.ID == _productionOrderDetail.ID_Owner).ID_Company;
                    //}
                    //if (TempProductionOrderDetails[i].ID_Company == null && TempProductionOrderDetails[i].ID_Owner == null)
                    //{
                    //    TempProductionOrderDetails[i].ID_Company = CurrentCompanyId;
                    //}
                    //_productionOrderDetail.ID_Company = TempProductionOrderDetails[i].ID_Company;

                    if (TempProductionOrderDetails[i].ID_Phase == 0)
                        TempProductionOrderDetails[i].ID_Phase = null;
                    _productionOrderDetail.ID_Phase = TempProductionOrderDetails[i].ID_Phase;

                    if (TempProductionOrderDetails[i].ProductionTime == null)
                        TempProductionOrderDetails[i].ProductionTime = 0;
                    _productionOrderDetail.ProductionTime = TempProductionOrderDetails[i].ProductionTime;

                    if (TempProductionOrderDetails[i].ID_PickingItem == 0)
                    {
                        TempProductionOrderDetails[i].ID_PickingItem = null;
                        TempProductionOrderDetails[i].RFlag = null;
                    }
                    _productionOrderDetail.ID_PickingItem = TempProductionOrderDetails[i].ID_PickingItem;
                    _productionOrderDetail.RFlag = (TempProductionOrderDetails[i].RFlag == "M"
                        ? TempProductionOrderDetails[i].RFlag
                        : null);

                    if (TempProductionOrderDetails[i].SupplierCode == 0)
                        TempProductionOrderDetails[i].SupplierCode = null;
                    _productionOrderDetail.SupplierCode = TempProductionOrderDetails[i].SupplierCode;

                    if (TempProductionOrderDetails[i].UMRawMaterial == 0)
                        TempProductionOrderDetails[i].UMRawMaterial = null;
                    _productionOrderDetail.UMRawMaterial = TempProductionOrderDetails[i].UMRawMaterial;

                    _productionOrderDetail.RawMaterialQuantity = TempProductionOrderDetails[i].RawMaterialQuantity;

                    if (TempProductionOrderDetails[i].UMUser == 0)
                        TempProductionOrderDetails[i].UMUser = null;
                    _productionOrderDetail.UMUser = TempProductionOrderDetails[i].UMUser;

                    if (TempProductionOrderDetails[i].ID_PickingItemSup == 0)
                    {
                        TempProductionOrderDetails[i].ID_PickingItemSup = null;
                        TempProductionOrderDetails[i].SFlag = null;
                    }
                    _productionOrderDetail.ID_PickingItemSup = TempProductionOrderDetails[i].ID_PickingItemSup;
                    _productionOrderDetail.SFlag = (TempProductionOrderDetails[i].SFlag == "M"
                        ? TempProductionOrderDetails[i].SFlag
                        : null);

                    if (TempProductionOrderDetails[i].SupplierCodeSup == 0)
                        TempProductionOrderDetails[i].SupplierCodeSup = null;
                    _productionOrderDetail.SupplierCodeSup = TempProductionOrderDetails[i].SupplierCodeSup;

                    _productionOrderDetail.RawMaterialX = TempProductionOrderDetails[i].RawMaterialX;
                    _productionOrderDetail.RawMaterialY = TempProductionOrderDetails[i].RawMaterialY;
                    _productionOrderDetail.RawMaterialZ = TempProductionOrderDetails[i].RawMaterialZ;

                    if (TempProductionOrderDetails[i].UMProduct == 0)
                        TempProductionOrderDetails[i].UMProduct = null;
                    _productionOrderDetail.UMProduct = TempProductionOrderDetails[i].UMProduct;

                    _productionOrderDetail.ProducedQuantity = TempProductionOrderDetails[i].ProducedQuantity;
                    _productionOrderDetail.QuantityOver = TempProductionOrderDetails[i].QuantityOver;
                    _productionOrderDetail.DirectSupply = TempProductionOrderDetails[i].DirectSupply;
                    _productionOrderDetail.Cost = TempProductionOrderDetails[i].Cost;
                    _productionOrderDetail.HistoricalCostPhase = TempProductionOrderDetails[i].CostCalcPhase;
                    _productionOrderDetail.HistoricalCostRawM = TempProductionOrderDetails[i].CostCalcRawM;
                    _productionOrderDetail.HistoricalCostSupM = TempProductionOrderDetails[i].CostCalcSupM;
                    _productionOrderDetail.Note = TempProductionOrderDetails[i].Note;


                    _productionOrderDetail.ProductionDate = (_mustChangeOdp ? TempProductionOrderDetails[i].ProductionDate : Convert.ToDateTime(date));

                    _productionOrderDetail.FreeTypeCode = TempProductionOrderDetails[i].FreeTypeCode;
                    if (TempProductionOrderDetails[i].FreeTypeCode != null)
                    {
                        _productionOrderDetail.ID_Phase = null;
                        _productionOrderDetail.ProductionTime = 0;
                    }
                    _productionOrderDetail.FreeItemTypeCode = TempProductionOrderDetails[i].FreeItemTypeCode;
                    _productionOrderDetail.FreeItemDescription = TempProductionOrderDetails[i].FreeItemDescription;

                    //merge aziendale
                    //if (TempProductionOrderDetails[i].ID_Company == null)
                    //{
                    TempProductionOrderDetails[i].ID_Company = GetCompanyIdFromTempProductionOrderDetail(_quotationDataContext, TempProductionOrderDetails[i]);
                    //}
                    _productionOrderDetail.ID_Company = TempProductionOrderDetails[i].ID_Company;

                    if (_isNew)
                    {
                        _quotationDataContext.ProductionOrderDetails.InsertOnSubmit(_productionOrderDetail);
                    }
                    if (_mustChangeOdp)
                    {
                        _quotationDataContext.ProductionOrderDetails.InsertOnSubmit(_productionOrderDetail);
                        _quotationDataContext.ProductionOrderDetails.DeleteOnSubmit(_quotationDataContext.ProductionOrderDetails.FirstOrDefault(d => d.ID == TempProductionOrderDetails[i].ID));
                    }
                    try
                    {
                        _quotationDataContext.SubmitChanges();
                    }
                    catch
                    {
                        throw;
                    }
                    if (_isNew)
                    {
                        TempProductionOrderDetails[i].ID = _productionOrderDetail.ID;
                    }

                    TempProductionOrderDetails[i].State = TempProductionOrderDetail.ItemState.Passed;
                    SubmitUnderlyingDetails(TempProductionOrderDetails[i], _mustChangeOdp ? _productionOrderDetail.ProductionDate.GetValueOrDefault() : Convert.ToDateTime(date), false);
                }
                //}
            }
        }


        protected void SubmitRow(int i)
        {
            if (TempProductionOrderDetails[i].ID_ProductionOrder != 0
                &&
                TempProductionOrderDetails[i].ProductionDate.GetValueOrDefault(DateTime.MinValue) != DateTime.MinValue
                && TempProductionOrderDetails[i].ID_Owner.GetValueOrDefault(0) != 0)
            {
                //if (!IsOverProductionOrder(TempProductionOrderDetails[i].ID_ProductionOrder))
                //{

                using (var _quotationDataContext = new QuotationDataContext())
                {
                    ProductionOrderDetail _productionOrderDetail = null;
                    var _isNew = false;
                    _productionOrderDetail = GetProductionOrderDetail(_quotationDataContext,
                        TempProductionOrderDetails[i].ID);
                    if (_productionOrderDetail == null)
                    {
                        _productionOrderDetail = new ProductionOrderDetail();
                        _isNew = true;
                    }
                    else
                    {
                        _productionOrderDetail.ID = TempProductionOrderDetails[i].ID;
                    }
                    _productionOrderDetail.ID_ProductionOrder = TempProductionOrderDetails[i].ID_ProductionOrder;

                    if (TempProductionOrderDetails[i].ID_Owner == 0)
                        TempProductionOrderDetails[i].ID_Owner = null;
                    _productionOrderDetail.ID_Owner = TempProductionOrderDetails[i].ID_Owner;

                    // merge aziendale
                    //if (TempProductionOrderDetails[i].ID_Company == null && TempProductionOrderDetails[i].ID_Owner != null)
                    //{
                    //    TempProductionOrderDetails[i].ID_Company = _quotationDataContext.Employees.First(d => d.ID == _productionOrderDetail.ID_Owner).ID_Company;
                    //}
                    //if (TempProductionOrderDetails[i].ID_Company == null && TempProductionOrderDetails[i].ID_Owner == null)
                    //{
                    //    TempProductionOrderDetails[i].ID_Company = CurrentCompanyId;
                    //}
                    //_productionOrderDetail.ID_Company = TempProductionOrderDetails[i].ID_Company;

                    if (TempProductionOrderDetails[i].ID_Phase == 0)
                        TempProductionOrderDetails[i].ID_Phase = null;
                    _productionOrderDetail.ID_Phase = TempProductionOrderDetails[i].ID_Phase;

                    if (TempProductionOrderDetails[i].ProductionTime == null)
                        TempProductionOrderDetails[i].ProductionTime = 0;
                    _productionOrderDetail.ProductionTime = TempProductionOrderDetails[i].ProductionTime;

                    if (TempProductionOrderDetails[i].ID_PickingItem == 0)
                    {
                        TempProductionOrderDetails[i].ID_PickingItem = null;
                        TempProductionOrderDetails[i].RFlag = null;
                    }
                    _productionOrderDetail.ID_PickingItem = TempProductionOrderDetails[i].ID_PickingItem;
                    _productionOrderDetail.RFlag = (TempProductionOrderDetails[i].RFlag == "M"
                        ? TempProductionOrderDetails[i].RFlag
                        : null);

                    if (TempProductionOrderDetails[i].SupplierCode == 0)
                        TempProductionOrderDetails[i].SupplierCode = null;
                    _productionOrderDetail.SupplierCode = TempProductionOrderDetails[i].SupplierCode;

                    if (TempProductionOrderDetails[i].UMRawMaterial == 0)
                        TempProductionOrderDetails[i].UMRawMaterial = null;
                    _productionOrderDetail.UMRawMaterial = TempProductionOrderDetails[i].UMRawMaterial;

                    _productionOrderDetail.RawMaterialQuantity = TempProductionOrderDetails[i].RawMaterialQuantity;

                    if (TempProductionOrderDetails[i].UMUser == 0)
                        TempProductionOrderDetails[i].UMUser = null;
                    _productionOrderDetail.UMUser = TempProductionOrderDetails[i].UMUser;

                    if (TempProductionOrderDetails[i].ID_PickingItemSup == 0)
                    {
                        TempProductionOrderDetails[i].ID_PickingItemSup = null;
                        TempProductionOrderDetails[i].SFlag = null;
                    }
                    _productionOrderDetail.ID_PickingItemSup = TempProductionOrderDetails[i].ID_PickingItemSup;
                    _productionOrderDetail.SFlag = (TempProductionOrderDetails[i].SFlag == "M"
                        ? TempProductionOrderDetails[i].SFlag
                        : null);

                    if (TempProductionOrderDetails[i].SupplierCodeSup == 0)
                        TempProductionOrderDetails[i].SupplierCodeSup = null;
                    _productionOrderDetail.SupplierCodeSup = TempProductionOrderDetails[i].SupplierCodeSup;

                    _productionOrderDetail.RawMaterialX = TempProductionOrderDetails[i].RawMaterialX;
                    _productionOrderDetail.RawMaterialY = TempProductionOrderDetails[i].RawMaterialY;
                    _productionOrderDetail.RawMaterialZ = TempProductionOrderDetails[i].RawMaterialZ;

                    if (TempProductionOrderDetails[i].UMProduct == 0)
                        TempProductionOrderDetails[i].UMProduct = null;
                    _productionOrderDetail.UMProduct = TempProductionOrderDetails[i].UMProduct;

                    _productionOrderDetail.ProducedQuantity = TempProductionOrderDetails[i].ProducedQuantity;
                    _productionOrderDetail.QuantityOver = TempProductionOrderDetails[i].QuantityOver;
                    _productionOrderDetail.DirectSupply = TempProductionOrderDetails[i].DirectSupply;
                    _productionOrderDetail.Cost = TempProductionOrderDetails[i].Cost;
                    _productionOrderDetail.HistoricalCostPhase = TempProductionOrderDetails[i].CostCalcPhase;
                    _productionOrderDetail.HistoricalCostRawM = TempProductionOrderDetails[i].CostCalcRawM;
                    _productionOrderDetail.HistoricalCostSupM = TempProductionOrderDetails[i].CostCalcSupM;
                    _productionOrderDetail.Note = TempProductionOrderDetails[i].Note;
                    _productionOrderDetail.ProductionDate = TempProductionOrderDetails[i].ProductionDate;

                    _productionOrderDetail.FreeTypeCode = TempProductionOrderDetails[i].FreeTypeCode;
                    _productionOrderDetail.FreeItemTypeCode = TempProductionOrderDetails[i].FreeItemTypeCode;
                    _productionOrderDetail.FreeItemDescription = TempProductionOrderDetails[i].FreeItemDescription;


                    //merge aziendale
                    //if (TempProductionOrderDetails[i].ID_Company == null )
                    //{
                    TempProductionOrderDetails[i].ID_Company = GetCompanyIdFromTempProductionOrderDetail(_quotationDataContext, TempProductionOrderDetails[i]);
                    //}
                    _productionOrderDetail.ID_Company = TempProductionOrderDetails[i].ID_Company;

                    if (_isNew)
                    {
                        _quotationDataContext.ProductionOrderDetails.InsertOnSubmit(_productionOrderDetail);
                    }
                    try
                    {
                        _quotationDataContext.SubmitChanges();
                    }
                    catch
                    {
                        throw;
                    }
                    if (_isNew)
                    {
                        TempProductionOrderDetails[i].ID = _productionOrderDetail.ID;
                    }



                    TempProductionOrderDetails[i].State = TempProductionOrderDetail.ItemState.Passed;
                    SubmitUnderlyingDetails(TempProductionOrderDetails[i],
                        TempProductionOrderDetails[i].ProductionDate.Value, false);
                }
                //}
            }
        }

        public void SubmitUnderlyingDetails(TempProductionOrderDetail tempProductionOrderDetail, DateTime date,
            bool onlyDelete)
        {
            using (var quotationDataContext = new QuotationDataContext())
            {
                quotationDataContext.ProductionOrderDetails.DeleteAllOnSubmit(
                    quotationDataContext.ProductionOrderDetails.Where(
                        pod => pod.MacroRef == tempProductionOrderDetail.ID && pod.FreeTypeCode == null
                               && pod.ID_PickingItem != null));
                //try
                //{
                //    quotationDataContext.SubmitChanges();
                //}
                //catch
                //{ throw; }

                if (tempProductionOrderDetail.RFlag == "M")
                {
                    if (!onlyDelete)
                    {
                        var _macroItem =
                            quotationDataContext.MacroItems.SingleOrDefault(
                                m => m.ID == tempProductionOrderDetail.ID_PickingItem.Value);
                        foreach (var _macroItemDetail in _macroItem.MacroItemDetails)
                        {
                            var _autoProductionOrderDetail = new ProductionOrderDetail();
                            var _pickingItem =
                                quotationDataContext.PickingItems.SingleOrDefault(
                                    p => p.ID == _macroItemDetail.CommonKey);
                            _autoProductionOrderDetail.ID_Owner = tempProductionOrderDetail.ID_Owner;
                            _autoProductionOrderDetail.ID_Company = tempProductionOrderDetail.ID_Company;
                            _autoProductionOrderDetail.ProductionDate = date;

                            if (_pickingItem.TypeCode == 31)
                            {
                                _autoProductionOrderDetail.ID_Phase = _pickingItem.ID;

                                _autoProductionOrderDetail.ProductionTime =
                                    Utilities.DecimalHoursToTicks(Convert.ToDecimal(_macroItemDetail.Quantity) *
                                                                  Convert.ToDecimal(
                                                                      tempProductionOrderDetail.RawMaterialQuantity));
                                _autoProductionOrderDetail.HistoricalCostPhase = _pickingItem.Cost *
                                                                                 Convert.ToDecimal(
                                                                                     _macroItemDetail.Quantity) *
                                                                                 Convert.ToDecimal(
                                                                                     tempProductionOrderDetail
                                                                                         .RawMaterialQuantity);
                                _autoProductionOrderDetail.ProductionDate = tempProductionOrderDetail.ProductionDate;
                                _autoProductionOrderDetail.ID_ProductionOrder =
                                    tempProductionOrderDetail.ID_ProductionOrder;
                                _autoProductionOrderDetail.MacroRef = tempProductionOrderDetail.ID;
                                _autoProductionOrderDetail.Note = tempProductionOrderDetail.Note;
                                _autoProductionOrderDetail.ID_PickingItem = _pickingItem.ID;
                                // serve per capire che è una fase creata a partire da un macroitem di base
                            }
                            else
                            {
                                _autoProductionOrderDetail.ID_PickingItem = _pickingItem.ID;
                                _autoProductionOrderDetail.RawMaterialQuantity = _macroItemDetail.Quantity *
                                                                                 tempProductionOrderDetail
                                                                                     .RawMaterialQuantity;
                                _autoProductionOrderDetail.SupplierCode = _pickingItem.SupplierCode;
                                _autoProductionOrderDetail.UMRawMaterial = _pickingItem.UM;
                                _autoProductionOrderDetail.HistoricalCostRawM = _pickingItem.Cost *
                                                                                Convert.ToDecimal(
                                                                                    _macroItemDetail.Quantity) *
                                                                                Convert.ToDecimal(
                                                                                    tempProductionOrderDetail
                                                                                        .RawMaterialQuantity);
                                _autoProductionOrderDetail.ProductionDate = tempProductionOrderDetail.ProductionDate;
                                _autoProductionOrderDetail.ID_ProductionOrder =
                                    tempProductionOrderDetail.ID_ProductionOrder;
                                _autoProductionOrderDetail.MacroRef = tempProductionOrderDetail.ID;
                                _autoProductionOrderDetail.Note = tempProductionOrderDetail.Note;
                                _autoProductionOrderDetail.ID_DeliveryTrip = tempProductionOrderDetail.ID_DeliveryTrip;
                            }
                            if (_pickingItem.TypeCode != 31) // la componente fase delle sottovoci deve essere esclusa
                            {
                                quotationDataContext.ProductionOrderDetails.InsertOnSubmit(_autoProductionOrderDetail);
                            }
                            //try
                            //{
                            //    quotationDataContext.SubmitChanges();
                            //}
                            //catch
                            //{ throw; }
                        }
                    }
                }

                quotationDataContext.ProductionOrderDetails.DeleteAllOnSubmit(
                    quotationDataContext.ProductionOrderDetails.Where(
                        pod => pod.MacroRef == tempProductionOrderDetail.ID
                               && pod.ID_PickingItemSup != null && pod.FreeTypeCode == null));
                //try
                //{
                //    quotationDataContext.SubmitChanges();
                //}
                //catch
                //{ throw; }
                if (tempProductionOrderDetail.SFlag == "M")
                {
                    if (!onlyDelete)
                    {
                        var _macroItem =
                            quotationDataContext.MacroItems.SingleOrDefault(
                                m => m.ID == tempProductionOrderDetail.ID_PickingItemSup.Value);
                        foreach (var _macroItemDetail in _macroItem.MacroItemDetails)
                        {
                            var _autoProductionOrderDetail = new ProductionOrderDetail();
                            var _pickingItem =
                                quotationDataContext.PickingItems.SingleOrDefault(
                                    p => p.ID == _macroItemDetail.CommonKey);
                            _autoProductionOrderDetail.ID_Owner = tempProductionOrderDetail.ID_Owner;
                            _autoProductionOrderDetail.ID_Company = tempProductionOrderDetail.ID_Company;
                            _autoProductionOrderDetail.ProductionDate = date;

                            if (_pickingItem.TypeCode == 31)
                            {
                                _autoProductionOrderDetail.ID_Phase = _pickingItem.ID;

                                _autoProductionOrderDetail.ProductionTime =
                                    Utilities.DecimalHoursToTicks(Convert.ToDecimal(_macroItemDetail.Quantity)
                                                                  *
                                                                  (tempProductionOrderDetail.RawMaterialX == null
                                                                      ? 1
                                                                      : Convert.ToDecimal(
                                                                          tempProductionOrderDetail.RawMaterialX))
                                                                  *
                                                                  (tempProductionOrderDetail.RawMaterialY == null
                                                                      ? 1
                                                                      : Convert.ToDecimal(
                                                                          tempProductionOrderDetail.RawMaterialY))
                                                                  *
                                                                  (tempProductionOrderDetail.RawMaterialZ == null
                                                                      ? 1
                                                                      : Convert.ToDecimal(
                                                                          tempProductionOrderDetail.RawMaterialZ))
                                        );

                                _autoProductionOrderDetail.HistoricalCostPhase = _pickingItem.Cost *
                                                                                 Convert.ToDecimal(
                                                                                     _macroItemDetail.Quantity)
                                                                                 *
                                                                                 (tempProductionOrderDetail.RawMaterialX ==
                                                                                  null
                                                                                     ? 1
                                                                                     : Convert.ToDecimal(
                                                                                         tempProductionOrderDetail
                                                                                             .RawMaterialX))
                                                                                 *
                                                                                 (tempProductionOrderDetail.RawMaterialY ==
                                                                                  null
                                                                                     ? 1
                                                                                     : Convert.ToDecimal(
                                                                                         tempProductionOrderDetail
                                                                                             .RawMaterialY))
                                                                                 *
                                                                                 (tempProductionOrderDetail.RawMaterialZ ==
                                                                                  null
                                                                                     ? 1
                                                                                     : Convert.ToDecimal(
                                                                                         tempProductionOrderDetail
                                                                                             .RawMaterialZ));

                                _autoProductionOrderDetail.ProductionDate = tempProductionOrderDetail.ProductionDate;
                                _autoProductionOrderDetail.ID_ProductionOrder =
                                    tempProductionOrderDetail.ID_ProductionOrder;
                                _autoProductionOrderDetail.MacroRef = tempProductionOrderDetail.ID;
                                _autoProductionOrderDetail.Note = tempProductionOrderDetail.Note;
                                _autoProductionOrderDetail.ID_PickingItemSup = _pickingItem.ID;
                                // serve per capire che è una fase creata a partire da un macroitem supplementare
                            }
                            else
                            {
                                _autoProductionOrderDetail.ID_PickingItemSup = _pickingItem.ID;
                                _autoProductionOrderDetail.RawMaterialX = _macroItemDetail.Quantity *
                                                                          tempProductionOrderDetail.RawMaterialX;
                                _autoProductionOrderDetail.RawMaterialY = tempProductionOrderDetail.RawMaterialY;
                                _autoProductionOrderDetail.RawMaterialZ = tempProductionOrderDetail.RawMaterialZ;
                                _autoProductionOrderDetail.SupplierCodeSup = _pickingItem.SupplierCode;
                                _autoProductionOrderDetail.UMUser = _pickingItem.UM;
                                _autoProductionOrderDetail.HistoricalCostSupM = _pickingItem.Cost *
                                                                                Convert.ToDecimal(
                                                                                    _macroItemDetail.Quantity)
                                                                                *
                                                                                (tempProductionOrderDetail.RawMaterialX ==
                                                                                 null
                                                                                    ? 1
                                                                                    : Convert.ToDecimal(
                                                                                        tempProductionOrderDetail
                                                                                            .RawMaterialX))
                                                                                *
                                                                                (tempProductionOrderDetail.RawMaterialY ==
                                                                                 null
                                                                                    ? 1
                                                                                    : Convert.ToDecimal(
                                                                                        tempProductionOrderDetail
                                                                                            .RawMaterialY))
                                                                                *
                                                                                (tempProductionOrderDetail.RawMaterialZ ==
                                                                                 null
                                                                                    ? 1
                                                                                    : Convert.ToDecimal(
                                                                                        tempProductionOrderDetail
                                                                                            .RawMaterialZ));
                                _autoProductionOrderDetail.ProductionDate = tempProductionOrderDetail.ProductionDate;
                                _autoProductionOrderDetail.ID_ProductionOrder =
                                    tempProductionOrderDetail.ID_ProductionOrder;
                                _autoProductionOrderDetail.MacroRef = tempProductionOrderDetail.ID;
                                _autoProductionOrderDetail.Note = tempProductionOrderDetail.Note;
                                _autoProductionOrderDetail.ID_DeliveryTrip = tempProductionOrderDetail.ID_DeliveryTrip;
                            }
                            if (_pickingItem.TypeCode != 31) // la componente fase delle sottovoci deve essere esclusa
                            {
                                quotationDataContext.ProductionOrderDetails.InsertOnSubmit(_autoProductionOrderDetail);
                            }
                            //try
                            //{
                            //    quotationDataContext.SubmitChanges();
                            //}
                            //catch
                            //{ throw; }
                        }
                    }
                }

                try
                {
                    quotationDataContext.SubmitChanges();
                }
                catch
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// RICHIEDE:
        /// var dataOptions = new DataLoadOptions();
        /// dataOptions.LoadWith<DeliveryTrip>(c => c.DeliveryTripDetails);
        /// dataOptions.LoadWith<DeliveryTripDetail>(c => c.ProductionOrder);
        /// ctx.LoadOptions = dataOptions;
        /// </summary>
        /// <param name="db"></param>
        /// <param name="deliveryTrip"></param>
        public void RecalcDeliveryCostDistribution(QuotationDataContext db, DeliveryTrip deliveryTrip)
        {
            decimal realDistance = 0m;
            decimal totalDistance = 0m;
            decimal OdPsCount = 0m;
            Dictionary<int?, decimal> distances = new Dictionary<int?, decimal>();

            if (deliveryTrip.LocationCode == null) // viaggio verso Cliente
            {
                realDistance = db.CustomersMarkUps.FirstOrDefault(d => d.Code == deliveryTrip.CustomerCode.Value).Distance.GetValueOrDefault() * 2m;
                foreach (DeliveryTripDetail detail in deliveryTrip.DeliveryTripDetails)
                {
                    decimal km = db.CustomersMarkUps.FirstOrDefault(d => d.Code == detail.ProductionOrder.ID_Customer.Value).Distance.GetValueOrDefault() * 2m;
                    totalDistance += km;
                }
                foreach (DeliveryTripDetail detail in deliveryTrip.DeliveryTripDetails)
                {
                    decimal km = db.CustomersMarkUps.FirstOrDefault(d => d.Code == detail.ProductionOrder.ID_Customer.Value).Distance.GetValueOrDefault() * 2m;
                    distances.Add(detail.ID_ProductionOrder, (realDistance / totalDistance) * km);
                }
            }
            else // viaggio verso altra destinazione
            {
                totalDistance = db.Locations.FirstOrDefault(d => d.Code == deliveryTrip.LocationCode).Distance.GetValueOrDefault() * 2m; ;
                OdPsCount = deliveryTrip.DeliveryTripDetails.Count();
                foreach (DeliveryTripDetail detail in deliveryTrip.DeliveryTripDetails)
                {
                    distances.Add(detail.ID_ProductionOrder, totalDistance / OdPsCount);
                }
            }

            db.ProductionOrderDetails.DeleteAllOnSubmit(db.ProductionOrderDetails.Where(d => d.ID_DeliveryTrip == deliveryTrip.ID));


            db.SubmitChanges();

            foreach (DeliveryTripDetail detail in deliveryTrip.DeliveryTripDetails)
            {

                TempProductionOrderDetail tpod = new TempProductionOrderDetail();
                tpod.ID_ProductionOrder = detail.ID_ProductionOrder.Value;
                tpod.ID_Owner = deliveryTrip.ID_Owner;
                tpod.ID_Company = deliveryTrip.ID_Company;
                tpod.ID_Phase = null;
                tpod.ProductionTime = 0m;
                //macrovoce viaggio
                MacroItem m = db.MacroItems.SingleOrDefault(x => x.ID == deliveryTrip.MacroRef);
                tpod.ID_PickingItem = deliveryTrip.MacroRef;
                tpod.RFlag = "M";
                tpod.SupplierCode = null;
                tpod.UMRawMaterial = m.UM;
                tpod.RawMaterialQuantity = (float)distances[detail.ID_ProductionOrder.Value];
                tpod.ProductionDate = DateTime.Parse(deliveryTrip.StartDate.Value.ToShortDateString());
                tpod.ID_DeliveryTrip = deliveryTrip.ID;


                ProductionOrderDetail pod = new ProductionOrderDetail();
                pod.ID_ProductionOrder = tpod.ID_ProductionOrder;
                pod.ID_Owner = tpod.ID_Owner;
                pod.ID_Company = tpod.ID_Company;
                pod.ID_Phase = tpod.ID_Phase;
                pod.ProductionTime = tpod.ProductionTime;
                pod.ID_PickingItem = tpod.ID_PickingItem;
                pod.RFlag = tpod.RFlag;
                pod.SupplierCode = tpod.SupplierCode;
                pod.UMRawMaterial = tpod.UMRawMaterial;
                pod.RawMaterialQuantity = tpod.RawMaterialQuantity;
                pod.ProductionDate = tpod.ProductionDate;
                pod.ID_DeliveryTrip = tpod.ID_DeliveryTrip;
                pod.HistoricalCostRawM = tpod.CostCalcRawM;

                db.ProductionOrderDetails.InsertOnSubmit(pod);

                db.SubmitChanges();

                tpod.ID = pod.ID;

                SubmitUnderlyingDetails(tpod, tpod.ProductionDate.Value, false);

            }

        }



        public void ChangeProductionOrderScheduleStatus(QuotationDataContext db, int idProductionOrder, int? idPhase, int? idQuotationDetail, int newStatus)
        {
            if (idPhase.HasValue)
            {
                if (idQuotationDetail.HasValue)
                {
                    ProductionMP currentSlot = db.ProductionMPs.Where(p => p.IDProductionOrder == idProductionOrder && p.IDPickingItem == idPhase && p.IDQuotationDetail == idQuotationDetail).FirstOrDefault();
                    if (currentSlot != null)
                    {
                        currentSlot.Status = newStatus;
                    }
                }
                else
                {
                    if (newStatus == 12)
                    {
                        List<ProductionMP> currentSlots = db.ProductionMPs.Where(p => p.Status == 11 && p.IDProductionOrder == idProductionOrder).OrderBy(p => p.ProdStart).ToList();
                        bool working = false;
                        foreach (ProductionMP currentSlot in currentSlots)
                        {
                            if (working)
                            {
                                if (currentSlot.IDPickingItem != idPhase)
                                {
                                    break;
                                }
                            }
                            if (currentSlot.IDPickingItem == idPhase)
                            {
                                currentSlot.Status = newStatus;
                                working = true;
                            }
                        }
                    }
                    if (newStatus == 11)
                    {
                        List<ProductionMP> currentSlots = db.ProductionMPs.Where(p => p.Status == 12 && p.IDProductionOrder == idProductionOrder).OrderByDescending(p => p.ProdStart).ToList();
                        bool working = false;
                        foreach (ProductionMP currentSlot in currentSlots)
                        {
                            if (working)
                            {
                                if (currentSlot.IDPickingItem != idPhase)
                                {
                                    break;
                                }
                            }
                            if (currentSlot.IDPickingItem == idPhase)
                            {
                                currentSlot.Status = newStatus;
                                working = true;
                            }
                        }
                    }
                }
            }

        }



        protected void InsertRow(DateTime date, int currentCompanyId, int? curOwner, int curProductionOrder = 0)
        {
            var _newTempProductionOrderDetail = new TempProductionOrderDetail();
            _newTempProductionOrderDetail.ID = 0;
            _newTempProductionOrderDetail.ProductionDate = date;
            _newTempProductionOrderDetail.ID_Company = currentCompanyId;
            if (curOwner != null)
            {
                _newTempProductionOrderDetail.ID_Owner = curOwner;
            }
            if (curProductionOrder != 0)
            {
                _newTempProductionOrderDetail.ID_ProductionOrder = curProductionOrder;
            }
            TempProductionOrderDetails.Add(_newTempProductionOrderDetail);
        }

        protected void InsertRow(int productionOrderId, int currentCompanyId)
        {
            var _newTempProductionOrderDetail = new TempProductionOrderDetail();
            _newTempProductionOrderDetail.ID = 0;

            _newTempProductionOrderDetail.ID_ProductionOrder = productionOrderId;
            _newTempProductionOrderDetail.ID_Company = currentCompanyId;

            TempProductionOrderDetails.Add(_newTempProductionOrderDetail);
        }

        protected void DeleteRow(int i)
        {
            if (TempProductionOrderDetails[i].State == TempProductionOrderDetail.ItemState.Passed)
            {
                DeleteProductionOrderDetail(TempProductionOrderDetails[i]);
            }
            TempProductionOrderDetails.RemoveAt(i);
        }

        public void DeleteProductionOrderDetail(TempProductionOrderDetail detail)
        {
            var _context = new QuotationDataContext();
            var _productionOrderDetail = GetProductionOrderDetail(_context, detail.ID);
            if (_productionOrderDetail != null)
            {
                _context.ProductionOrderDetails.DeleteOnSubmit(_productionOrderDetail);

                _context.ProductionOrderTechSpecs.DeleteAllOnSubmit(
                    _context.ProductionOrderTechSpecs.Where(
                        pot => pot.ID_ProductionOrderDetail == _productionOrderDetail.ID));

                _context.SubmitChanges();
                SubmitUnderlyingDetails(detail, DateTime.Now.Date, true);
            }
        }

        public void DeleteDeliveryTripDetail(DeliveryTripDetail detail)
        {
            using (QuotationDataContext _context = new QuotationDataContext())
            {
                var _deliveryTripDetail = GetDeliveryTripDetail(_context, detail.ID);
                if (_deliveryTripDetail != null)
                {
                    _context.DeliveryTripDetails.DeleteOnSubmit(_deliveryTripDetail);

                    _context.SubmitChanges();

                }
            }
        }

        public void DeleteDeliveryTrip(int id)
        {
            using (QuotationDataContext _context = new QuotationDataContext())
            {
                var _deliveryTrip = GetDeliveryTrip(_context, id);
                if (_deliveryTrip != null)
                {
                    _context.ProductionOrderDetails.DeleteAllOnSubmit(_context.ProductionOrderDetails.Where(d => d.ID_DeliveryTrip == id));
                    _context.DeliveryTripDetails.DeleteAllOnSubmit(_context.DeliveryTripDetails.Where(d => d.ID_DeliveryTrip == id));
                    _context.DeliveryTrips.DeleteOnSubmit(_deliveryTrip);

                    _context.SubmitChanges();

                }
            }
        }

        protected void PresetForConversion(DropDownList selected, TempProductionOrderDetail tempProductionOrderDetail)
        {
            var _context = new QuotationDataContext();

            IEnumerable<KeyValuePair<string, string>> _allUnits =
                _context.Units.Select(u => new KeyValuePair<string, string>(u.ID.ToString(), u.Description));

            var _convertibleUnits =
                _context.UnitConverters.Where(u => u.ID_FinalUnit == tempProductionOrderDetail.UMRawMaterial)
                    .Select(u => new KeyValuePair<string, string>(u.Unit.ID.ToString(), u.Unit.Description))
                    .ToList();

            //IEnumerable<KeyValuePair<string, string>> _filteredUnits = _allUnits.Intersect(_convertibleUnits);

            if (_convertibleUnits.Count > 0)
            {
                selected.Items.Clear();

                selected.DataTextField = "Value";
                selected.DataValueField = "Key";
                selected.DataSourceID = "";
                selected.DataSource = _convertibleUnits;
                selected.DataBind();
                selected.SelectedIndex = 0;
            }
        }

        protected void SetWorkDayDurationAlert(Label lblOverTimeMessage)
        {
            var _workDayDuration =
                TempProductionOrderDetails.Where(t => t.ProductionTime != null).Sum(t => t.ProductionTime);
            var _elapsed = new TimeSpan(Convert.ToInt64(_workDayDuration));
            lblOverTimeMessage.Text = "Attività giornaliera operatore (hh.mm): " + _elapsed.Hours.ToString("00") + "." +
                                      _elapsed.Minutes.ToString("00");
            if (_elapsed.TotalHours > 8d)
            {
                lblOverTimeMessage.ForeColor = Color.Red;
                lblOverTimeMessage.Font.Underline = true;
            }
            else
            {
                lblOverTimeMessage.ForeColor = Color.Olive;
                lblOverTimeMessage.Font.Underline = false;
            }
        }

        protected string GetProductionOrderToolTip(string productionOrderID, out bool found, int idCompany = -1)
        {
            var _message = string.Empty;
            found = false;

            using (var _quotationDataContext = new QuotationDataContext())
            {
                try
                {
                    var _curProductionOrder =
                        _quotationDataContext.ProductionOrders.SingleOrDefault(
                            po => po.ID == Int32.Parse(productionOrderID) && (idCompany == -1 || po.ID_Company == idCompany));
                    if (_curProductionOrder == null) _message = "Ordine di produzione inesistente o azienda errata! ";
                    //else if (_curProductionOrder.Status == 3) _message = "Ordine di produzione già evaso!";
                    else
                    {
                        _message = "Lanciato il " + _curProductionOrder.StartDate.Value.ToString("dd/MM/yyyy") +
                                   " - Cliente: " +
                                   (_curProductionOrder.Customer == null
                                       ? "Nessun Cliente"
                                       : _curProductionOrder.Customer.Name) + " - " + _curProductionOrder.Description;
                        found = true;
                    }
                }
                catch
                {
                    _message = "Impossibile determinare l'ordine di produzione";
                }
            }
            return _message;
        }

        protected bool IsOverProductionOrder(int productionOrderID)
        {
            using (var _quotationDataContext = new QuotationDataContext())
            {
                var _productionOrder =
                    _quotationDataContext.ProductionOrders.SingleOrDefault(po => po.ID == productionOrderID);
                if (_productionOrder == null)
                    return false;
                return _productionOrder.Status == 3;
            }
        }


        public static int GetCompanyIdFromTempProductionOrderDetail(QuotationDataContext db, TempProductionOrderDetail item)
        {

            if (item.ID_Phase.GetValueOrDefault() != 0)
            {
                return db.PickingItems.FirstOrDefault(d => d.ID == item.ID_Phase).ID_Company.Value;
            }
            else if (item.ID_PickingItem.GetValueOrDefault() != 0)
            {
                if (item.RFlag == "M")
                    return db.MacroItems.FirstOrDefault(d => d.ID == item.ID_PickingItem).ID_Company.Value;
                else
                    return db.PickingItems.FirstOrDefault(d => d.ID == item.ID_PickingItem).ID_Company.Value;
            }
            else if (item.ID_PickingItemSup.GetValueOrDefault() != 0)
            {
                if (item.SFlag == "M")
                    return db.MacroItems.FirstOrDefault(d => d.ID == item.ID_PickingItemSup).ID_Company.Value;
                else
                    return db.PickingItems.FirstOrDefault(d => d.ID == item.ID_PickingItemSup).ID_Company.Value;
            }
            else if (item.FreeItemTypeCode.GetValueOrDefault() != 0)
            {
                if (item.FFlag == "M")
                    return db.MacroItems.FirstOrDefault(d => d.ID == item.FreeItemTypeCode).ID_Company.Value;
                else
                    return db.PickingItems.FirstOrDefault(d => d.ID == item.FreeItemTypeCode).ID_Company.Value;
            }
            return 1;

        }

    }
}