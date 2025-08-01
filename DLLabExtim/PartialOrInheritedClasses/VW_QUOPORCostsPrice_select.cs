using System;

namespace DLLabExtim
{
    public partial class VW_QUOPORCostsPrices_select
    {
        //public int? ID_Customer { get; set; }
        //public string CustomerName { get; set; }

        public decimal? Saving
        {
            get
            {
                if (_FATTotValue != null && PORTotCost != null)
                {
                    return (_FATTotValue - Convert.ToDecimal(PORTotHistoricalOrNotCost) - (ProvvTotValue ?? 0m));
                }
                return null;
            }
        }

        public decimal? PercentageSaving
        {
            get
            {
                if (_FATTotValue != null && PORTotCost != null)
                {
                    if (_FATTotValue > 0m)
                    {
                        return (_FATTotValue - Convert.ToDecimal(PORTotHistoricalOrNotCost) - (ProvvTotValue ?? 0m))/
                               _FATTotValue;
                    }
                    if (_FATTotValue < 0m) // fatturato negativo, in caso di danno al Cliente, sconto negativo inserito direttamente in fattura senza creare una voce libera di costo supplementare
                    {
                        return (_FATTotValue - Convert.ToDecimal(PORTotHistoricalOrNotCost) - (ProvvTotValue ?? 0m)) /
                               -_FATTotValue;
                    }
                    return 0;
                }
                return null;
            }
        }

        public decimal? TotCosts
        {
            get { return (PORTotHistoricalCost.GetValueOrDefault(0m) + ProvvTotValue.GetValueOrDefault(0m)); }
        }

        public decimal? PORTotHistoricalOrNotCost
        {
            get
            {
                return (PORTotHistoricalCost.GetValueOrDefault(0m) == 0m
                    ? Convert.ToDecimal(PORTotCost.GetValueOrDefault(0d))
                    : PORTotHistoricalCost.GetValueOrDefault(0m));
            }
        }
    }
}