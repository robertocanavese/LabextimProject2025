using System;
using System.Collections.Generic;
using System.Linq;
using CMLabExtim;
using DLLabExtim;

namespace UILabExtim
{
    public class OptionsController : ProductionOrderDetailsInsertController
    {
        public bool RecalcProductionOrderDetails(DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                var _owners = new List<Employee>();

                var _dates = new List<DateTime>();
                var _curDate = dateFrom;
                while (_curDate <= dateTo)
                {
                    _dates.Add(_curDate);
                    _curDate = _curDate.AddDays(1);
                }

                using (var _db = new QuotationDataContext())
                {
                    _owners = _db.Employees.ToList();
                }

                foreach (var _owner in _owners)
                {
                    foreach (var _date in _dates)
                    {
                        TempProductionOrderDetails.Clear();
                        TempProductionOrderDetails.AddRange(GetProductionOrderDetailsOfAnOwner(_owner.ID, _date, _owner.Company.ID));
                        if (TempProductionOrderDetails.Count > 0)
                        {
                            for (var i = 0; i < TempProductionOrderDetails.Count; i++)
                            {
                                SubmitRow(i, _date.ToString(), _owner.ID.ToString());
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception _ex)
            {
                Log.Write("Errore nell'elaborazione - ", _ex);
                return false;
            }
        }
    }
}