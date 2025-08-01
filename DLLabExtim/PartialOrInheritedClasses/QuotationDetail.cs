using System;

namespace DLLabExtim
{
    public partial class QuotationDetail
    {
 
        public decimal TotPrice
        {
            get
            {
                //if (this.MacroItem != null)
                //{
                //    decimal _tempTotMultiplyablePrice = 0;
                //    decimal _tempTotNotMultiplyablePrice = 0;
                //    foreach (MacroItemDetail _macroItemDetail in this.MacroItem.MacroItemDetails)
                //    {
                //        if (_macroItemDetail.Multiply == true)
                //            _tempTotMultiplyablePrice +=
                //                (_macroItemDetail.PickingItem.Cost * Convert.ToDecimal(_macroItemDetail.Quantity) * Convert.ToDecimal(Quantity) *
                //                (Convert.ToDecimal(_macroItemDetail.PickingItem.PercentageAuto) / 100m) *
                //                (Convert.ToDecimal(this.MarkUp) / 100m));
                //        else
                //            _tempTotNotMultiplyablePrice +=
                //                (_macroItemDetail.PickingItem.Cost * Convert.ToDecimal(_macroItemDetail.Quantity) *
                //                (Convert.ToDecimal(_macroItemDetail.PickingItem.PercentageAuto) / 100m) *
                //                (Convert.ToDecimal(this.MarkUp) / 100m));
                //    }
                //    return _tempTotMultiplyablePrice + _tempTotNotMultiplyablePrice;
                //}
                //else
                //{
                return Convert.ToDecimal(_Quantity)*_Price;
                //}
            }
        }
    }
}