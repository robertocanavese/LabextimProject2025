using System;
using System.Collections.Generic;
using System.Linq;

namespace DLLabExtim
{
    public partial class PickingItem
    {
        public int PercentageAuto
        {
            get
            {
                if (StandardPercentage == null)
                {
                    return _Percentage;
                }
                return _StandardPercentage.Entity.Percentage;
            }
        }

        public bool BelongsToMacroItem
        {
            get { return _MacroItemDetails.FirstOrDefault() != null; }
        }

        public string DependantMacroItems
        {
            get {

                return "Macrovoci dipendenti: " +  string.Join(" | ", _MacroItemDetails.Select(d => d.MacroItem.MacroItemDescription));
            }
        }

        public bool IsObsolete(Dictionary<string, string> currentConfiguration)
        {
            return
                (Date.GetValueOrDefault(new DateTime(2000, 1, 1))
                    .AddMonths(Convert.ToInt32(currentConfiguration["PIMU"])) < DateTime.Now);
        }


    }
}