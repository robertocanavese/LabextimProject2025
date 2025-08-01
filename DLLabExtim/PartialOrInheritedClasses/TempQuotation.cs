using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLLabExtim
{
    public partial class TempQuotation
    {

        public string NoteFromProduction
        {
            get
            {

                using (QuotationDataContext db = new QuotationDataContext())
                {
                    ProductionOrder po = db.ProductionOrders.OrderByDescending(p => p.ID).FirstOrDefault(s => s.Quotation.ID == this._ID_Quotation);
                    if (po != null)
                    {
                        //List<ProductionOrderTechSpec> tss = db.ProductionOrderTechSpecs.OrderByDescending(p => p.ID).Where(s => s.ProductionOrder.ID == po.ID && s.Status == 17).OrderBy(d => d.ID).ToList();
                        List<ProductionOrderTechSpec> tss = db.ProductionOrderTechSpecs.OrderByDescending(p => p.ID).Where(s => s.ProductionOrder.ID == po.ID).OrderBy(d => d.ID).ToList();
                        if (tss != null)
                        {
                            StringBuilder rs = new StringBuilder();
                            foreach (ProductionOrderTechSpec ts in tss)
                            {
                                rs.AppendLine(
                                          (string.Format("OdP: {0} ({1}) - {2} {3} {4}: ", po.Number, ts.ID_ProductionOrder, ts.ProductionDate.Value.ToString("dd/MM/yyyy"), ts.Employee.Surname, ts.Employee.Name)) +
                                          (ts.CodiceMarcaInchiostro != null ? "SERIGRAFIA SPECIALE: " : "") +
                                          (ts.CodiceMarcaInchiostro != null ? ("Codice marca inchiostro: " + ts.CodiceMarcaInchiostro + " ,") : "") +
                                          (ts.Ricetta != null ? ("Ricetta: " + (ts.Ricetta.GetValueOrDefault() ? "SI" : "NO") + " ,") : "") +
                                          (ts.TelaioNumeroFili != null ? ("Telaio numero fili: " + ts.TelaioNumeroFili.ToString() + " ,") : "") +
                                          (ts.GelatinaSpessore != null ? ("Gelatina spessore: " + ts.GelatinaSpessore.ToString() + " ,") : "") +
                                          (ts.RaclaInclinazione != null ? ("Racla inclinazione: " + ts.RaclaInclinazione.ToString() + " ,") : "") +
                                          (ts.RaclaDurezzaSpigolo != null ? ("Racla durezza spigolo: " + ts.RaclaDurezzaSpigolo.ToString() + " ,") : "") +
                                          (ts.CodiceMarcaFilm != null ? "FILM CALDO: " : "") +
                                          (ts.CodiceMarcaFilm != null ? ("Codice marca film: " + ts.CodiceMarcaFilm + " ,") : "") +
                                          (ts.ClicheReso != null ? ("Cliche: " + (ts.ClicheReso == "C" ? "Reso al Cliente" : "Ns archivio") + " ,") : "") +
                                          (ts.ClicheCondizioni != null ? ("Condizioni: " + (ts.ClicheCondizioni == "B" ? "Buone" : "Scadenti") + " ,") : "") +
                                          (ts.StampaTemperatura != null ? ("Temperatura stampa: " + ts.StampaTemperatura.ToString() + " ,") : "") +
                                          (ts.AltreInfo != null ? ("Info: " + ts.AltreInfo.ToString() + " ,") : "") +
                                          (ts.FustellaResa != null ? ("Fustella: " + (ts.FustellaResa == "C" ? "Resa al Cliente" : "Ns archivio") + " ,") : "") +
                                          (ts.FustellaCondizioni != null ? ("Condizioni: " + (ts.FustellaCondizioni == "B" ? "Buone" : "Scadenti") + " ,") : "") +
                                          (ts.AltreNoteDaProduzione != null ? ("ALTRE NOTE: " + ts.AltreNoteDaProduzione + " ,") : "")
                               );
                            }
                            return rs.ToString();
                        }
                        else
                            return string.Empty;
                    }
                    else
                        return string.Empty;
                }
            }
        }

    }
}
