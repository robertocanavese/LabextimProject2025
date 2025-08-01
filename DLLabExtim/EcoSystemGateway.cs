using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMLabExtim;

namespace DLLabExtim
{
    public class EcoSystemGateway
    {
        public static void RefreshMachineSchedule(QuotationDataContext db)
        {
            db.prc_LAB_DelIns_LAB_ProductionExtMPS_GroupedByPhaseToEcoSystem(2);

        }

        public static void UpdateOdPPlasticCoatingData(QuotationDataContext db, int idProductionOrder)
        {
            db.prc_LAB_DelIns_LAB_PlasticCoatingMachineParameters(idProductionOrder);

        }

        public static OdPBag GetCurOdP(QuotationDataContext db)
        {
            prc_LAB_Get_LAB_PlasticCoatingMachineGetCurrentOdPDataResult found =
                db.prc_LAB_Get_LAB_PlasticCoatingMachineGetCurrentOdPData().FirstOrDefault();
            if (found != null)
                return new OdPBag { Id = found.OdP.Value, CopieRichieste = Convert.ToInt32(found.CopieInput), CopieLavorate = Convert.ToInt32(found.CopieOutput) };
            else
                return new OdPBag { Id = -1, CopieRichieste = 0, CopieLavorate = 0 };
        }

        public static OdPBag GetOdPHistoricalData(int poId, QuotationDataContext db)
        {

            OdPBag result = new OdPBag { Id = -1, CopieRichieste = 0, CopieLavorate = 0 };
            try
            {
                prc_LAB_Get_LAB_PlasticCoatingMachineOdPDataResult found = db.prc_LAB_Get_LAB_PlasticCoatingMachineOdPData(poId).FirstOrDefault();
                if (found != null)
                    result = new OdPBag { Id = found.OdP.Value, CopieRichieste = found.CopieInput.GetValueOrDefault(), CopieLavorate = found.CopieOutput.GetValueOrDefault(0) };
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.Message);
            }
            return result;

        }

    }
}
