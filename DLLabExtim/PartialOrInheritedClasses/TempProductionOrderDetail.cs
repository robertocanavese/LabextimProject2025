using System;
using System.Linq;

namespace DLLabExtim
{
    public class NoDBProductionOrderDetail
    {
        private String _RFlag;
        private String _SFlag;
        public int ID { get; set; }
        public int ID_ProductionOrder { get; set; }
        public int? ID_QuotationDetail { get; set; }
        public int? ID_Owner { get; set; }
        public int? ID_Company { get; set; }
        public int? ID_PickingItem { get; set; }
        public int? SupplierCode { get; set; }
        public int? UMRawMaterial { get; set; }
        public int? UMUser { get; set; }
        public int? ID_PickingItemSup { get; set; }
        public int? SupplierCodeSup { get; set; }
        public float? RawMaterialX { get; set; }
        public string RawMaterialXWithUm
        {
            get
            {
                var ctx = new QuotationDataContext();
                if (UMUser != null)
                    return string.Format("{0} ({1})", RawMaterialX, ctx.Units.Single(x => x.ID == UMUser).Description.Trim());
                else
                    return string.Empty;

            }
            //set { RawMaterialQuantity = (float?) Convert.ToDouble(value.Substring(0, value.IndexOf('('))); }
        }
        public float? RawMaterialY { get; set; }
        public float? RawMaterialYcm { get { return RawMaterialY * 100; } set { RawMaterialY = value / 100; } }
        public float? RawMaterialZ { get; set; }
        public float? RawMaterialZcm { get { return RawMaterialZ * 100; } set { RawMaterialZ = value / 100; } }
        public float? RawMaterialQuantity { get; set; }

        public string RawMaterialQuantityWithUm
        {
            get
            {
                if (RawMaterialQuantity.HasValue && UMRawMaterial.HasValue)
                {
                    var ctx = new QuotationDataContext();

                    return string.Format("{0} ({1})", RawMaterialQuantity, ctx.Units.Single(x => x.ID == UMRawMaterial).Description.Trim());
                }
                else
                {
                    return "";
                }
            }
            //set { RawMaterialQuantity = (float?) Convert.ToDouble(value.Substring(0, value.IndexOf('('))); }
        }
        public int? ID_Phase { get; set; }
        public int? UMProduct { get; set; }
        public float? ProducedQuantity { get; set; }
        public decimal? ProductionTime { get; set; }
        public DateTime? ProductionDate { get; set; }
        public DateTime? ProductionDateDefault { get { return DateTime.Now; } set { ProductionDate = value; } }
        public bool QuantityOver { get; set; }
        public decimal? Cost { get; set; }
        public bool DirectSupply { get; set; }
        public string Note { get; set; }
        public int? FreeTypeCode { get; set; }
        public int? FreeItemTypeCode { get; set; }
        public string FreeItemDescription { get; set; }
        public int? OkCopiesCount { get; set; }
        public int? KoCopiesCount { get; set; }
        public bool? Special { get; set; }

        public DateTime ProductionDateTime
        {
            get
            {
                return DateTime.MinValue.AddMinutes(_productionDateTimeInt64 == 0 ? ProductionDateTimeInt64 : _productionDateTimeInt64);
            }
            set { ProductionTime = Convert.ToInt64((value.Hour * 60 + value.Minute) * 60) * Convert.ToInt64(10000000); }
        }

        private Int64 _productionDateTimeInt64;

        public Int64 ProductionDateTimeInt64
        {
            get { return Convert.ToInt64(ProductionTime / (60 * 10000000)); }
            set
            {
                ProductionTime = value * (60 * 10000000);
                _productionDateTimeInt64 = value;
            }
        }

        public String RFlag
        {
            get { return _RFlag == null ? "P" : _RFlag; }
            //get { return _RFlag; }
            set { _RFlag = value; }
        }

        public String SFlag
        {
            get { return _SFlag == null ? "P" : _SFlag; }
            //get { return _SFlag; }
            set { _SFlag = value; }
        }

        public String FFlag { get; set; }
        public decimal? HistoricalCost { get; set; }
        public int? MacroRef { get; set; }
        public int? ID_DeliveryTrip { get; set; }
    }

    public class TempProductionOrderDetail : NoDBProductionOrderDetail
    {
        public enum ItemState
        {
            Passed,
            Error
        }

        public TempProductionOrderDetail()
        {
            State = ItemState.Error;
        }

        public ItemState State { get; set; }
        public string Data { get; set; }

        public float? RawMaterialUArea
        {
            get
            {
                return 1;
                //if (this != null)
                //{
                //    if (this.RawMaterialX == null)
                //    {
                //        return null;
                //    }
                //    else
                //    {
                //        if (this.RawMaterialY == null)
                //        {
                //            return this.RawMaterialX;
                //        }
                //        else
                //        {
                //            if (this.RawMaterialZ == null)
                //            {
                //                return this.RawMaterialX * this.RawMaterialY;
                //            }
                //            else
                //            {
                //                return this.RawMaterialX * this.RawMaterialY * this.RawMaterialZ;
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    return null;
                //}
            }
        }

        public decimal? CostCalc
        {
            get
            {
                if (this != null)
                {
                    if (DirectSupply == false)
                    {
                        decimal? _costCalc = 0;
                        if (ProductionTime != null)
                        {
                            if (ID_Phase != null)
                            {
                                var _context = new QuotationDataContext();
                                var _curPickingItem =
                                    _context.PickingItems.FirstOrDefault(pi => pi.ID == Convert.ToInt32(ID_Phase));
                                if (_curPickingItem != null)
                                {
                                    _costCalc += _curPickingItem.Cost *
                                                 (new TimeSpan(Convert.ToInt64(ProductionTime)).Hours +
                                                  new TimeSpan(Convert.ToInt64(ProductionTime)).Minutes / 60m +
                                                  new TimeSpan(Convert.ToInt64(ProductionTime)).Seconds / 3600m);
                                }
                            }
                        }
                        if (RawMaterialUArea != null && RawMaterialQuantity != null)
                        {
                            if (ID_PickingItem != null)
                            {
                                var _context = new QuotationDataContext();
                                var _curPickingItem =
                                    _context.PickingItems.FirstOrDefault(pi => pi.ID == Convert.ToInt32(ID_PickingItem));
                                if (_curPickingItem != null)
                                {
                                    _costCalc += _curPickingItem.Cost * Convert.ToDecimal(RawMaterialUArea) *
                                                 Convert.ToDecimal(RawMaterialQuantity);
                                }
                            }
                        }
                        if (RawMaterialX != null && RawMaterialY != null && RawMaterialZ != null)
                        {
                            if (ID_PickingItemSup != null)
                            {
                                var _context = new QuotationDataContext();
                                var _curPickingItemSup =
                                    _context.PickingItems.FirstOrDefault(
                                        pi => pi.ID == Convert.ToInt32(ID_PickingItemSup));
                                if (_curPickingItemSup != null)
                                {
                                    _costCalc += _curPickingItemSup.Cost * Convert.ToDecimal(RawMaterialX) *
                                                 Convert.ToDecimal(RawMaterialY) * Convert.ToDecimal(RawMaterialZ) / 10000m;
                                }
                            }
                        }
                        _costCalc += Cost == null ? decimal.Zero : Cost;
                        return _costCalc;
                    }
                    return Cost;
                }
                return null;
            }
        }

        public decimal? CostCalcPhase
        {
            get
            {
                if (this != null)
                {
                    if (DirectSupply == false)
                    {
                        decimal? _costCalc = 0;
                        if (ProductionTime != null)
                        {
                            if (ID_Phase != null)
                            {
                                var _context = new QuotationDataContext();
                                var _curPickingItem =
                                    _context.PickingItems.FirstOrDefault(pi => pi.ID == Convert.ToInt32(ID_Phase));
                                if (_curPickingItem != null)
                                {
                                    _costCalc = _curPickingItem.Cost *
                                                (new TimeSpan(Convert.ToInt64(ProductionTime)).Hours +
                                                 new TimeSpan(Convert.ToInt64(ProductionTime)).Minutes / 60m +
                                                 new TimeSpan(Convert.ToInt64(ProductionTime)).Seconds / 3600m);
                                }
                            }
                        }
                        return _costCalc;
                    }
                    return 0m;
                }
                return null;
            }
        }

        public decimal? CostCalcRawM
        {
            get
            {
                if (this != null)
                {
                    if (DirectSupply == false)
                    {
                        decimal? _costCalc = 0;
                        if (RawMaterialUArea != null && RawMaterialQuantity != null)
                        {
                            if (ID_PickingItem != null)
                            {
                                PickingItem _curPickingItem;
                                var _context = new QuotationDataContext();

                                if (RFlag == "M")
                                {
                                    var _curMacroItem =
                                        _context.MacroItems.FirstOrDefault(
                                            pi => pi.ID == Convert.ToInt32(ID_PickingItem));
                                    _curPickingItem = new PickingItem { Cost = _curMacroItem.CostCalc };
                                }
                                else
                                {
                                    _curPickingItem =
                                        _context.PickingItems.FirstOrDefault(
                                            pi => pi.ID == Convert.ToInt32(ID_PickingItem));
                                }
                                if (_curPickingItem != null)
                                {
                                    _costCalc = _curPickingItem.Cost * Convert.ToDecimal(RawMaterialUArea) *
                                                Convert.ToDecimal(RawMaterialQuantity);
                                }
                            }
                        }
                        return _costCalc;
                    }
                    return 0m;
                }
                return null;
            }
        }

        public decimal? CostCalcSupM
        {
            get
            {
                if (this != null)
                {
                    if (DirectSupply == false)
                    {
                        decimal? _costCalc = 0;
                        if (RawMaterialX != null) // && this.RawMaterialY != null && this.RawMaterialZ != null)
                        {
                            if (ID_PickingItemSup != null)
                            {
                                PickingItem _curPickingItemSup;
                                var _context = new QuotationDataContext();

                                if (SFlag == "M")
                                {
                                    var _curMacroItem =
                                        _context.MacroItems.FirstOrDefault(
                                            pi => pi.ID == Convert.ToInt32(ID_PickingItemSup));
                                    _curPickingItemSup = new PickingItem { Cost = _curMacroItem.CostCalc };
                                }
                                else
                                {
                                    _curPickingItemSup =
                                        _context.PickingItems.FirstOrDefault(
                                            pi => pi.ID == Convert.ToInt32(ID_PickingItemSup));
                                }
                                if (_curPickingItemSup != null)
                                {
                                    _costCalc = _curPickingItemSup.Cost *
                                                Convert.ToDecimal(RawMaterialX == null ? 0f : RawMaterialX) *
                                                Convert.ToDecimal(RawMaterialY == null ? 1f : RawMaterialY) *
                                                Convert.ToDecimal(RawMaterialZ == null ? 1f : RawMaterialZ);
                                    /// 10000m;
                                }
                            }
                        }
                        return _costCalc;
                    }
                    return 0m;
                }
                return null;
            }
        }

        //public String UMRawMaterialDesc { get; set; }

        //public String UMUserDesc { get; set; }

        //public String PickingItemDesc { get; set; }

        //public String PickingItemSupDesc { get; set; }

        //public String PhaseDesc { get; set; }

        public String UMRawMaterialDesc
        {
            get
            {
                if (UMRawMaterial != null)
                    return
                        new QuotationDataContext().Units.FirstOrDefault(pi => pi.ID == Convert.ToInt32(UMRawMaterial))
                            .Description;
                return null;
            }
        }

        public String UMUserDesc
        {
            get
            {
                if (UMUser != null)
                    return
                        new QuotationDataContext().Units.FirstOrDefault(pi => pi.ID == Convert.ToInt32(UMUser))
                            .Description;
                return null;
            }
        }

        public string PickingItemDescId
        {
            get
            {
                if (ID_PickingItem != null)
                    if (RFlag == "P")
                        return
                            string.Format("P{0}", ID_PickingItem);
                    else if (RFlag == "M")
                        return string.Format("M{0}", ID_PickingItem);
                    else
                        return null;
                return null;
            }
            set
            {
                if (value != null)
                {
                    RFlag = value[0].ToString();
                    ID_PickingItem = Convert.ToInt32(value.Substring(1));
                }
            }
        }

        public String PickingItemDesc
        {
            get
            {
                if (ID_PickingItem != null)
                    if (RFlag == "P")
                    {
                        PickingItem pkit = new QuotationDataContext().PickingItems.FirstOrDefault(pi => pi.ID == Convert.ToInt32(ID_PickingItem));
                        if (pkit != null)
                            return pkit.ItemDescription;
                        else
                            return "Materiale non trovato";
                    }
                    else if (RFlag == "M")
                    {
                        MacroItem mi = new QuotationDataContext().MacroItems.FirstOrDefault(pi => pi.ID == Convert.ToInt32(ID_PickingItem));
                        if (mi != null)
                            return "[" + mi.MacroItemDescription + "]";
                        else
                            return "[Macrovoce non trovata]";
                    }
                    else
                        return null;
                return null;
            }
        }

        public string PickingItemSupId
        {
            get
            {
                if (ID_PickingItemSup != null)
                    if (SFlag == "P")
                        return
                            string.Format("P{0}", ID_PickingItemSup);
                    else if (SFlag == "M")
                        return string.Format("M{0}", ID_PickingItemSup);
                    else
                        return null;
                return null;
            }
            set
            {
                if (value != null)
                {
                    SFlag = value[0].ToString();
                    ID_PickingItemSup = Convert.ToInt32(value.Substring(1));
                }
            }
        }

        public String PickingItemSupDesc
        {
            get
            {
                if (ID_PickingItemSup != null)
                    if (SFlag == "P")
                        return
                            new QuotationDataContext().PickingItems.FirstOrDefault(
                                pi => pi.ID == Convert.ToInt32(ID_PickingItemSup)).ItemDescription;
                    else if (SFlag == "M")
                        return "[" +
                               new QuotationDataContext().MacroItems.FirstOrDefault(
                                   pi => pi.ID == Convert.ToInt32(ID_PickingItemSup)).MacroItemDescription + "]";
                    else
                        return null;
                return null;
            }
        }

        public String PhaseDesc
        {
            get
            {
                if (ID_Phase != null)
                    return
                        new QuotationDataContext().PickingItems.FirstOrDefault(pi => pi.ID == Convert.ToInt32(ID_Phase))
                            .ItemDescription;
                return null;
            }
        }

        public String EmployeeDesc
        {
            get
            {
                if (ID_Owner != null)
                    return
                        new QuotationDataContext().Employees.FirstOrDefault(pi => pi.ID == Convert.ToInt32(ID_Owner))
                            .UniqueName;
                return null;
            }
        }

        public String CustomerDesc
        {
            get
            {
                if (ID_ProductionOrder != 0)
                {
                    QuotationDataContext context = new QuotationDataContext();
                    return context.ProductionOrders.Join(context.Customers, p => p.ID_Customer, c => c.Code, (p, c) => new { ProductionOrders = p, Customers = c }).FirstOrDefault(c => c.ProductionOrders.ID == Convert.ToInt32(ID_ProductionOrder)).Customers.Name;

                }
                return null;
            }
        }

        public string CodiceMarcaFilm { get; set; }
        public string ClicheReso { get; set; }
        public string ClicheCondizioni { get; set; }
        public string StampaTemperatura { get; set; }
        public string AltreInfo { get; set; }
        public string CodiceMarcaInchiostro { get; set; }
        public Nullable<bool> Ricetta { get; set; }
        public string TelaioNumeroFili { get; set; }
        public string GelatinaSpessore { get; set; }
        public string RaclaInclinazione { get; set; }
        public string RaclaDurezzaSpigolo { get; set; }
        public string FustellaResa { get; set; }
        public string FustellaCondizioni { get; set; }
        public string ControCordonatori { get; set; }
        public string AltreNoteDaProduzione { get; set; }

        public string ProductionOrderNumber { get; set; }


    }
}