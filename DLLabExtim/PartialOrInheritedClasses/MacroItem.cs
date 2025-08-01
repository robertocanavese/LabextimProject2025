using System;
using System.Linq;
using System.Collections.Generic;

namespace DLLabExtim
{
    public partial class MacroItem
    {
        public decimal CostCalc
        {
            get
            {
                if (_Cost == 0)
                {
                    if (this != null)
                    {
                        decimal _tempTotMultiplyableCost = 0;
                        decimal _tempTotNotMultiplyableCost = 0;
                        foreach (var _macroItemDetail in MacroItemDetails)
                        {
                            if (_macroItemDetail.Multiply)
                                _tempTotMultiplyableCost +=
                                    (_macroItemDetail.PickingItem.Cost * Convert.ToDecimal(_macroItemDetail.Quantity));
                            else
                                _tempTotNotMultiplyableCost +=
                                    (_macroItemDetail.PickingItem.Cost);
                        }
                        return _tempTotMultiplyableCost + _tempTotNotMultiplyableCost;
                    }

                    //return Convert.ToDecimal(this._MacroItemDetails.Sum<MacroItemDetail>(mid => mid.PickingItem.Cost * Convert.ToDecimal(mid.Quantity)));
                }
                return _Cost.GetValueOrDefault(0m);
            }
        }

        public int PercentageCalc
        {
            get
            {
                //if (this._Percentage == 0 )
                //{
                var _totalCost =
                    Convert.ToDecimal(_MacroItemDetails.Sum(mid => mid.PickingItem.Cost * Convert.ToDecimal(mid.Quantity)));
                var _totalPrice =
                    Convert.ToDecimal(
                        _MacroItemDetails.Sum(
                            mid =>
                                mid.PickingItem.Cost * (Convert.ToDecimal(mid.PickingItem.PercentageAuto) / 100m) *
                                Convert.ToDecimal(mid.Quantity)));
                return _totalCost != 0 ? Convert.ToInt32((_totalPrice / _totalCost) * 100m) : 0;

                //}
                //return this._Percentage;
            }
        }

        public bool IsObsolete(Dictionary<string, string> currentConfiguration)
        {
            foreach (MacroItemDetail _macroItemDetail in MacroItemDetails)
            {

                if (_macroItemDetail.PickingItem.Date.GetValueOrDefault(new DateTime(2000, 1, 1)).AddMonths(Convert.ToInt32(currentConfiguration["PIMU"])) < DateTime.Now)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasInactivePickingItems()
        {
            foreach (var _macroItemDetail in MacroItemDetails)
            {

                if (_macroItemDetail.PickingItem.Inserted == false)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
