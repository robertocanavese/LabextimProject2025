using System.Collections.Generic;
using CMLabExtim.CustomClasses;
using DLLabExtim;

namespace LabExtimOperator.Models
{
    public class ModelloComplesso
    {
        public List<TempProductionOrderDetail> ModelloDati { get; set; }
        public List<ComboListItem> ModelloPrimo { get; set; }

        public ModelloComplesso()
        {
            ModelloDati = new List<TempProductionOrderDetail>();
            ModelloPrimo = new List<ComboListItem>();
        }
    }
}