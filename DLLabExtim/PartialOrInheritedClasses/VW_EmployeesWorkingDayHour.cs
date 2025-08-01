using System;
using CMLabExtim;

namespace DLLabExtim
{
    public partial class VW_EmployeesWorkingDayHour
    {
        public decimal? HourlyProductivity
        {
            get
            {
                return Convert.ToDecimal(RawMaterialX)/
                       (Utilities.ConvertToCentiHours(ProductionTime) == 0
                           ? decimal.MaxValue
                           : Utilities.ConvertToCentiHours(ProductionTime));
            }
        }
    }
}