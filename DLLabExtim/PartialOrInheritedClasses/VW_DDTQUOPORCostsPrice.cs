using System;

namespace DLLabExtim
{
    public partial class VW_DDTQUOPORCostsPrice
    {
        //public int? ID_Customer { get; set; }
        //public string CustomerName { get; set; }

        public decimal? Saving
        {
            get
            {
                if (_FATTotValue != null && PORPropCost != null)
                {
                    return (_FATTotValue - Convert.ToDecimal(PORPropHistoricalOrNotCost) - (ProvvTotValue ?? 0m));
                }
                return null;
            }
        }

        public decimal? PercentageSaving
        {
            get
            {
                if (_FATTotValue != null && PORPropCost != null)
                {
                    if (_FATTotValue > 0m)
                    {
                        return (_FATTotValue - Convert.ToDecimal(PORPropHistoricalOrNotCost) - (ProvvTotValue ?? 0m))/
                               _FATTotValue;
                    }
                    if (_FATTotValue < 0m) // fatturato negativo, in caso di danno al Cliente, sconto negativo inserito direttamente in fattura senza creare una voce libera di costo supplementare
                    {
                        return (_FATTotValue - Convert.ToDecimal(PORPropHistoricalOrNotCost) - (ProvvTotValue ?? 0m)) /
                               -_FATTotValue;
                    }
                    return 0;
                }
                return null;
            }
        }

        public decimal? TotCosts
        {
            get { return (PORPropHistoricalCost.GetValueOrDefault(0m) + ProvvTotValue.GetValueOrDefault(0m)); }
        }

        public decimal? PORPropHistoricalOrNotCost
        {
            get
            {
                return (PORPropHistoricalCost.GetValueOrDefault(0m) == 0m
                    ? Convert.ToDecimal(PORPropCost.GetValueOrDefault(0d))
                    : PORPropHistoricalCost.GetValueOrDefault(0m));
            }
        }
    }
}