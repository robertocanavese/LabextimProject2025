using System;
using System.Linq;

namespace DLLabExtim
{
    public partial class ProductionOrderDetail
    {
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

        //public decimal? CostCalc
        //{
        //    get
        //    {
        //        if (this != null)
        //        {

        //            if (this.DirectSupply == false)
        //            {
        //                decimal? _costCalc = 0;
        //                // calcolo costo di fase
        //                if (this.ProductionTime != null)
        //                {
        //                    if (this.ID_Phase != null)
        //                    {
        //                        QuotationDataContext _context = new QuotationDataContext();
        //                        PickingItem _curPickingItem = _context.PickingItems.FirstOrDefault(pi => pi.ID == Convert.ToInt32(this.ID_Phase));
        //                        if (_curPickingItem != null)
        //                        {
        //                            _costCalc += _curPickingItem.Cost *
        //                                (new TimeSpan(Convert.ToInt64(this.ProductionTime)).Hours +
        //                                 new TimeSpan(Convert.ToInt64(this.ProductionTime)).Minutes / 60m +
        //                                 new TimeSpan(Convert.ToInt64(this.ProductionTime)).Seconds / 3600m);
        //                        }
        //                    }
        //                }
        //                // calcolo costo materiale di base
        //                if (this.RawMaterialUArea != null && this.RawMaterialQuantity != null)
        //                {
        //                    if (this.ID_PickingItem != null)
        //                    {
        //                        QuotationDataContext _context = new QuotationDataContext();
        //                        PickingItem _curPickingItem = _context.PickingItems.FirstOrDefault(pi => pi.ID == Convert.ToInt32(this.ID_PickingItem));
        //                        if (_curPickingItem != null)
        //                        {
        //                            _costCalc += _curPickingItem.Cost * Convert.ToDecimal(this.RawMaterialUArea) * Convert.ToDecimal(this.RawMaterialQuantity);
        //                        }
        //                    }
        //                }
        //                // calcolo costo materiale supplementare (costo item al mq * battute (x) * y in cm * z in cm /10000)

        //                if (this.ID_PickingItemSup != null)
        //                {
        //                    QuotationDataContext _context = new QuotationDataContext();
        //                    PickingItem _curPickingItemSup = _context.PickingItems.FirstOrDefault(pi => pi.ID == Convert.ToInt32(this.ID_PickingItemSup));
        //                    if (_curPickingItemSup != null)
        //                    {
        //                        decimal _itemSupCost = 0m;
        //                        if (this.RawMaterialX != null && this.RawMaterialX != 0)
        //                            _itemSupCost = _curPickingItemSup.Cost * Convert.ToDecimal(this.RawMaterialX);
        //                        if (this.RawMaterialY != null && this.RawMaterialY != 0)
        //                            _itemSupCost = _itemSupCost * Convert.ToDecimal(this.RawMaterialY);
        //                        if (this.RawMaterialZ != null && this.RawMaterialZ != 0)
        //                            _itemSupCost = _itemSupCost * Convert.ToDecimal(this.RawMaterialZ);
        //                        _costCalc += _itemSupCost;
        //                    }

        //                }

        //                _costCalc += this.Cost == null ? decimal.Zero : this.Cost;
        //                return _costCalc;
        //            }
        //            return this.Cost;
        //        }
        //        return null;
        //    }
        //}

        //public decimal? CostCalc
        //{
        //    get
        //    {
        //        if (this != null)
        //        {

        //            if (this.DirectSupply == false)
        //            {
        //                decimal? _costCalc = 0;
        //                if (this.ProductionTime != null)
        //                {
        //                    if (this.ID_Phase != null)
        //                    {
        //                        QuotationDataContext _context = new QuotationDataContext();
        //                        PickingItem _curPickingItem = _context.PickingItems.FirstOrDefault(pi => pi.ID == Convert.ToInt32(this.ID_Phase));
        //                        if (_curPickingItem != null)
        //                        {
        //                            _costCalc += _curPickingItem.Cost *
        //                                (new TimeSpan(Convert.ToInt64(this.ProductionTime)).Hours +
        //                                 new TimeSpan(Convert.ToInt64(this.ProductionTime)).Minutes / 60m +
        //                                 new TimeSpan(Convert.ToInt64(this.ProductionTime)).Seconds / 3600m);
        //                        }
        //                    }
        //                }
        //                if (this.RawMaterialUArea != null && this.RawMaterialQuantity != null)
        //                {
        //                    if (this.ID_PickingItem != null)
        //                    {
        //                        QuotationDataContext _context = new QuotationDataContext();
        //                        PickingItem _curPickingItem = _context.PickingItems.FirstOrDefault(pi => pi.ID == Convert.ToInt32(this.ID_PickingItem));
        //                        if (_curPickingItem != null)
        //                        {
        //                            _costCalc += _curPickingItem.Cost * Convert.ToDecimal(this.RawMaterialUArea) * Convert.ToDecimal(this.RawMaterialQuantity);
        //                        }
        //                    }
        //                }
        //                if (this.RawMaterialX != null && this.RawMaterialY != null && this.RawMaterialZ != null)
        //                {
        //                    if (this.ID_PickingItemSup != null)
        //                    {
        //                        QuotationDataContext _context = new QuotationDataContext();
        //                        PickingItem _curPickingItemSup = _context.PickingItems.FirstOrDefault(pi => pi.ID == Convert.ToInt32(this.ID_PickingItemSup));
        //                        if (_curPickingItemSup != null)
        //                        {
        //                            _costCalc += _curPickingItemSup.Cost * Convert.ToDecimal(this.RawMaterialX) * Convert.ToDecimal(this.RawMaterialY) * Convert.ToDecimal(this.RawMaterialZ) / 10000m;
        //                        }
        //                    }
        //                }
        //                _costCalc += this.Cost == null ? decimal.Zero : this.Cost;
        //                return _costCalc;
        //            }
        //            return this.Cost;
        //        }
        //        return null;
        //    }
        //}

        public decimal? CostCalc
        {
            get { return CostCalcPhase + CostCalcRawM + CostCalcSupM; }
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
                                    _costCalc = _curPickingItem.Cost*
                                                (new TimeSpan(Convert.ToInt64(ProductionTime)).Hours +
                                                 new TimeSpan(Convert.ToInt64(ProductionTime)).Minutes/60m +
                                                 new TimeSpan(Convert.ToInt64(ProductionTime)).Seconds/3600m);
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
                                    _curPickingItem = new PickingItem {Cost = _curMacroItem.CostCalc};
                                }
                                else
                                {
                                    _curPickingItem =
                                        _context.PickingItems.FirstOrDefault(
                                            pi => pi.ID == Convert.ToInt32(ID_PickingItem));
                                }
                                if (_curPickingItem != null)
                                {
                                    _costCalc = _curPickingItem.Cost*Convert.ToDecimal(RawMaterialUArea)*
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
                                    _curPickingItemSup = new PickingItem {Cost = _curMacroItem.CostCalc};
                                }
                                else
                                {
                                    _curPickingItemSup =
                                        _context.PickingItems.FirstOrDefault(
                                            pi => pi.ID == Convert.ToInt32(ID_PickingItemSup));
                                }
                                if (_curPickingItemSup != null)
                                {
                                    _costCalc = _curPickingItemSup.Cost*
                                                Convert.ToDecimal(RawMaterialX == null ? 0f : RawMaterialX)*
                                                Convert.ToDecimal(RawMaterialY == null ? 1f : RawMaterialY)*
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

        public String PickingItemDesc
        {
            get
            {
                if (ID_PickingItem != null)
                    if (RFlag == null || RFlag == "P")
                        return
                            new QuotationDataContext().PickingItems.FirstOrDefault(
                                pi => pi.ID == Convert.ToInt32(ID_PickingItem)).ItemDescription;
                    else if (RFlag == "M")
                        return "[" +
                               new QuotationDataContext().MacroItems.FirstOrDefault(
                                   pi => pi.ID == Convert.ToInt32(ID_PickingItem)).MacroItemDescription + "]";
                    else
                        return null;
                return null;
            }
        }

        public String PickingItemSupDesc
        {
            get
            {
                if (ID_PickingItemSup != null)
                    if (SFlag == null || SFlag == "P")
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

        public decimal? HistoryCostCalc
        {
            get
            {
                return HistoricalCostPhase.GetValueOrDefault(0m) +
                       (RFlag == null ? HistoricalCostRawM.GetValueOrDefault(0m) : 0m) +
                       (SFlag == null ? HistoricalCostSupM.GetValueOrDefault(0m) : 0m);
            }
        }
    }
}