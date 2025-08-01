using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMLabExtim;

namespace DLLabExtim
{
    public class EuroProgettiGateway
    {

        public static bool Close_EuroProgetti_DB_Ordine(QuotationDataContext dbLoc, Sql_EpDataContext dbRem, int labextimID_OdP)
        {
            var _success = false;
            try
            {

                foreach (EuroProgetti_DB_Ordini odp in dbLoc.EuroProgetti_DB_Ordinis.Where(d => d.LabextimID_OdP == labextimID_OdP))
                {
                    odp.LabextimStatus = 2;
                    //dbRem.DB_Ordinis.DeleteAllOnSubmit(dbRem.DB_Ordinis.Where(d => d.i_ID == odp.i_ID));
                    //dbRem.DB_Ordinis.DeleteAllOnSubmit(dbRem.DB_Ordinis.Where(d => d.i_ID == odp.i_ID && d.i_o_Sospensione == "LAVORO CONCLUSO"));
                }
                dbRem.SubmitChanges();

                _success = true;
            }
            catch (Exception _exception)
            {
                Log.Write("Importazione dati", _exception);
            }
            return _success;
        }

        public static OdPBag  GetCurOdP(QuotationDataContext db)
        {
            EuroProgetti_DB_Ordini found = db.EuroProgetti_DB_Ordinis.FirstOrDefault(d => d.i_o_Sospensione == "LAVORO IN CORSO");
            if (found != null)
                return new OdPBag { Id = found.LabextimID_OdP.Value, CopieRichieste = found.i_Quantita.GetValueOrDefault(0), CopieLavorate = found.o_Pezzi.GetValueOrDefault(0) };
            else
                return new OdPBag { Id= -1, CopieRichieste = 0, CopieLavorate = 0 };
        }

        public static OdPBag GetOdPHistoricalData(int poId, QuotationDataContext db)
        {

            OdPBag result = new OdPBag { Id = -1, CopieRichieste = 0, CopieLavorate = 0 };
            try
            {
                List<EuroProgetti_DB_Ordini> found = db.EuroProgetti_DB_Ordinis.Where(d => d.LabextimID_OdP == poId).ToList();
                result.CopieRichieste = found[0].i_Quantita.GetValueOrDefault();
                result.CopieLavorate = found.Max(d => d.o_Pezzi).GetValueOrDefault();
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.Message);
            }
            return result;

        }


    }
}
