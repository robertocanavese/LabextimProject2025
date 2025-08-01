using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CMLabExtim;
using DLLabExtim;


namespace DailyManager
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var daysToSkip = Convert.ToInt32(ConfigurationManager.AppSettings["DeleteQuotationDraftsDaysToSkip"]);
                if (args.Length > 1)
                {
                    Console.Write(
                        "Errore: Digitare un solo argomento alla volta.\nDigitare ? per ottenere l'elenco degli argomenti.");
                }
                if (args[0] == ("?"))
                {
                    Console.Write(
                        "Argomenti: \nLabExtimBackup \nLabeBackup \nAll \nAspNetDB \nPintonBackUp \nArcProcBackUp \nMasterBackUp \nDelOldDrafts \nImport");
                }
                if (args[0] == ("MasterBackUp"))
                {
                    BackUpMaster();
                }
                if (args[0] == ("LabExtimBackup"))
                {
                    BackUpLabExtim(); 
                }
                if (args[0] == ("AspNetDB"))
                {
                    BackUpAspNetDB();
                }
                if (args[0] == ("LabeBackup"))
                {
                    BackUpLabe();
                }
                if (args[0] == ("ArcProcBackUp"))
                {
                    BackUpLabeArcProc();
                }
                if (args[0] == ("PintonBackUp"))
                {
                    BackUpLabePinton();
                }
                if (args[0] == ("DelOldDrafts"))
                {
                    DeleteOldQuotationDrafts(daysToSkip);
                }
                if (args[0] == ("DelOldTempQuotations"))
                {
                    DeleteOldTempQuotations(daysToSkip);
                }
                if (args[0] == ("Import"))
                {
                    ImportCustomersAndSuppliers();
                }
                if (args[0] == ("ReindexLabExtim"))
                {
                    ReindexLabextim();
                }
                if (args[0] == ("ReindexLabe"))
                {
                    ReindexLabe();
                }
                if (args[0] == ("RecalcIBCMPS"))
                {
                    RecalcIBCMPS();
                }
                if (args[0] == ("RecalcFFCMPS"))
                {
                    RecalcFFCMPS();
                }
                if (args[0] == ("RecalcVW_QUOPORCostsPrices"))
                {
                    RecalcVW_QUOPORCostsPrices();
                }
                if (args[0] == ("Sync_EuroProgetti_DB_Ordini"))
                {
                    Sync_EuroProgetti_DB_Ordini();
                }
                if (args[0] == ("Sync_Zechini_Ordini"))
                {
                    Sync_Zechini_Ordini();
                }
                
                if (args[0] == ("All"))
                {
                    BackUpMaster();
                    BackUpLabExtim();
                    BackUpAspNetDB();
                    BackUpLabe();
                    BackUpLabeArcProc();
                    BackUpLabePinton();
                    DeleteOldQuotationDrafts(daysToSkip);
                    DeleteOldTempQuotations(daysToSkip);
                    ImportCustomersAndSuppliers();
                    ReindexLabextim();
                }
                if (args[0] == ("Check_MCCG"))
                {
                    Check_MCCG();
                }

                if (args[0] == ("test"))
                {
                    using (QuotationDataContext dbLoc = new QuotationDataContext())
                    {
                        foreach (EuroProgetti_DB_Ordini odp in dbLoc.EuroProgetti_DB_Ordinis)
                        {
                            odp.i_Cliente = odp.i_Cliente.ControlCharsCleaned();
                            odp.i_Codice = odp.i_Codice.ControlCharsCleaned();
                            dbLoc.SubmitChanges();
                        }
                    }
                }

                

            }
            catch (Exception ex)
            {
                Console.Write("ERRORE: " + ex.Message);
                Log.Write("LabExtim Daily Manager: " + ex.Message, ex);
            }
        }

        private static void BackUpLabExtim()
        {
            try
            {
                ImportGateway.BackUp(
                    ConfigurationManager.ConnectionStrings["LabExtimConnectionString"].ConnectionString,
                    "LabExtim",
                    ConfigurationManager.AppSettings["BackupLocalPath"],
                    ConfigurationManager.AppSettings["BackupNetworkPath"]);
                //ImportGateway.BackUp(
                //    ConfigurationManager.ConnectionStrings["LabExtimConnectionString"].ConnectionString,
                //    "LabExtim",
                //    ConfigurationManager.AppSettings["BackupNetworkPath"]);
                Log.Write("Backup LabExtim terminato con successo", null);
            }
            catch (Exception ex)
            {
                Console.Write("ERRORE: " + ex.Message);
                Log.Write("LabExtim Daily Manager: " + ex.Message, ex);
            }
        }

        private static void BackUpLabe()
        {
            try
            {
                ImportGateway.BackUp(
                    ConfigurationManager.ConnectionStrings["GestConnectionString"].ConnectionString,
                    "Labe",
                    ConfigurationManager.AppSettings["BackupLocalPath"],
                    ConfigurationManager.AppSettings["BackupNetworkPath"]);
                //ImportGateway.BackUp(
                //    ConfigurationManager.ConnectionStrings["GestConnectionString"].ConnectionString,
                //    "Labe",
                //    ConfigurationManager.AppSettings["BackupNetworkPath"]);
                Log.Write("Backup Labe terminato con successo", null);
            }
            catch (Exception ex)
            {
                Console.Write("ERRORE: " + ex.Message);
                Log.Write("LabExtim Daily Manager: " + ex.Message, ex);
            }
        }

        private static void BackUpLabeArcProc()
        {
            try
            {
                ImportGateway.BackUp(
                    ConfigurationManager.ConnectionStrings["GestConnectionString"].ConnectionString,
                    "ArcProc",
                    ConfigurationManager.AppSettings["BackupLocalPath"],
                    ConfigurationManager.AppSettings["BackupNetworkPath"]);
                //ImportGateway.BackUp(
                //    ConfigurationManager.ConnectionStrings["GestConnectionString"].ConnectionString,
                //    "ArcProc",
                //    ConfigurationManager.AppSettings["BackupNetworkPath"]);
                Log.Write("Backup ArcProc terminato con successo", null);
            }
            catch (Exception ex)
            {
                Console.Write("ERRORE: " + ex.Message);
                Log.Write("LabExtim Daily Manager: " + ex.Message, ex);
            }
        }

        private static void BackUpLabePinton()
        {
            try
            {
                ImportGateway.BackUp(
                    ConfigurationManager.ConnectionStrings["GestConnectionString"].ConnectionString,
                    "Pinton",
                    ConfigurationManager.AppSettings["BackupLocalPath"],
                    ConfigurationManager.AppSettings["BackupNetworkPath"]);
                //ImportGateway.BackUp(
                //    ConfigurationManager.ConnectionStrings["GestConnectionString"].ConnectionString,
                //    "Pinton",
                //    ConfigurationManager.AppSettings["BackupNetworkPath"]);
                Log.Write("Backup Pinton terminato con successo", null);
            }
            catch (Exception ex)
            {
                Console.Write("ERRORE: " + ex.Message);
                Log.Write("LabExtim Daily Manager: " + ex.Message, ex);
            }
        }

        private static void BackUpMaster()
        {
            try
            {
                ImportGateway.BackUp(
                    ConfigurationManager.ConnectionStrings["GestConnectionString"].ConnectionString,
                    "Master",
                    ConfigurationManager.AppSettings["BackupLocalPath"],
                    ConfigurationManager.AppSettings["BackupNetworkPath"]);
                //ImportGateway.BackUp(
                //    ConfigurationManager.ConnectionStrings["GestConnectionString"].ConnectionString,
                //    "Master",
                //    ConfigurationManager.AppSettings["BackupNetworkPath"]);
                Log.Write("Backup Master terminato con successo", null);
            }
            catch (Exception ex)
            {
                Console.Write("ERRORE: " + ex.Message);
                Log.Write("LabExtim Daily Manager: " + ex.Message, ex);
            }
        }

        private static void BackUpAspNetDB()
        {
            try
            {
                ImportGateway.BackUp(
                    ConfigurationManager.ConnectionStrings["MemberShipConnString"].ConnectionString,
                    "AspNetDB",
                    ConfigurationManager.AppSettings["BackupLocalPath"],
                    ConfigurationManager.AppSettings["BackupNetworkPath"]);
                //ImportGateway.BackUp(
                //    ConfigurationManager.ConnectionStrings["MemberShipConnString"].ConnectionString,
                //    "AspNetDB",
                //    ConfigurationManager.AppSettings["BackupNetworkPath"]);
                Log.Write("Backup AspNetDB terminato con successo", null);
            }
            catch (Exception ex)
            {
                Console.Write("ERRORE: " + ex.Message);
                Log.Write("LabExtim Daily Manager: " + ex.Message, ex);
            }
        }

        private static void DeleteOldQuotationDrafts(int daysToSkip)
        {
            try
            {
                var _ctx = new QuotationDataContext();
                IEnumerable<Quotation> _quotationsToMark = _ctx.Quotations.Where(q =>
                    q.Draft == true && q.Date.Value <
                    Utilities.GetNextWorkDate(-daysToSkip, DateTime.Now.Date));
                foreach (var _quotationToMark in _quotationsToMark)
                    _quotationToMark.Draft = null;
                _ctx.SubmitChanges();
                Log.Write(
                    "Cancellazione bozze preventivi anteriori al " +
                    Utilities.GetNextWorkDate(-daysToSkip, DateTime.Now.Date).ToString("dd/MM/yyyy") +
                    " eseguita con successo", null);
            }
            catch (Exception ex)
            {
                Console.Write("ERRORE: " + ex.Message);
                Log.Write("LabExtim Daily Manager: " + ex.Message, ex);
            }
        }

        private static void DeleteOldTempQuotations(int daysToSkip)
        {
            try
            {
                var _ctx = new QuotationDataContext();
                IEnumerable<TempQuotation> _quotationsToMark = _ctx.TempQuotations.Where(q =>
                    q.ID_Quotation < 0 && q.Date.Value <
                    Utilities.GetNextWorkDate(-daysToSkip, DateTime.Now.Date));

                foreach (var _quotationToMark in _quotationsToMark)
                {
                    IEnumerable<TempQuotationDetail> _details = _quotationToMark.TempQuotationDetails;
                    _ctx.TempQuotationDetails.DeleteAllOnSubmit(_details);
                    _ctx.TempQuotations.DeleteOnSubmit(_quotationToMark);
                }
                _ctx.SubmitChanges();
                Log.Write(
                    "Cancellazione preventivi temporanei anteriori al " +
                    Utilities.GetNextWorkDate(-daysToSkip, DateTime.Now.Date).ToString("dd/MM/yyyy") +
                    " eseguita con successo", null);
            }
            catch (Exception ex)
            {
                Console.Write("ERRORE: " + ex.Message);
                Log.Write("LabExtim Daily Manager: " + ex.Message, ex);
            }
        }

        private static void ImportCustomersAndSuppliers()
        {
            if (ImportGateway.Import(0))
                Log.Write("Importazione dati gestionale eseguita con successo", null);
            else
                Log.Write("Importazione dati gestionale eseguita con errori", null);
        }


        private static void ReindexLabextim()
        {
            DBReindex.DbReindex("LabExtimConnectionString");
            DBReindex.DbReindex("MemberShipConnString");
            DBReindex.DBReindex_mm_lotto("GestConnectionString");
            DBReindex.DBReindex_mm_lotto("GestCARTOLABEConnectionString");
         
        }

        private static void ReindexLabe()
        {
            DBReindex.DbReindex("GestConnectionString");

        }

        private static void RecalcIBCMPS()
        {
            using (QuotationDataContext db = new QuotationDataContext())
            {
                ProductionOrderService.RecalcMPS(db, DLLabExtim.ProductionOrderService.SchedulingType.InfiniteBackwardCapacity);
            }

        }

        private static void RecalcFFCMPS()
        {
            using (QuotationDataContext db = new QuotationDataContext())
            {
                ProductionOrderService.RecalcMPS(db, DLLabExtim.ProductionOrderService.SchedulingType.FiniteForwardCapacity);
            }

        }

        private static void RecalcVW_QUOPORCostsPrices()
        {
            using (QuotationDataContext db = new QuotationDataContext())
            {
                ProductionOrderService.RecalcVW_QUOPORCostsPrices(db, DateTime.Now.AddYears(-2).ToString("yyyyMMdd"));
            }

        }

        private static void Check_MCCG()
        {
            using (QuotationDataContext db = new QuotationDataContext())
            using (GeneralDataContext dbg = new GeneralDataContext())
            {
                ProductionOrderService.Check_MCCG(db, dbg);
            }

        }

        private static void Sync_EuroProgetti_DB_Ordini()
        {
            try
            {
                using (Sql_EpDataContext dbRem = new Sql_EpDataContext())
                using (QuotationDataContext dbLoc = new QuotationDataContext())
                {
                    ImportGateway.Sync_EuroProgetti_DB_Ordini(dbLoc, dbRem);
                    ImportGateway.AutoOpen_EcoSystem_ORDINE(dbLoc);
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(string.Format("Impossibile comunicare con la linea di accoppiatura EUROPROGETTI - {0}", ex.Message));
            }

        }

        private static void Sync_Zechini_Ordini()
        {
            try
            {
                using (QuotationDataContext dbLoc = new QuotationDataContext())
                {
                    ImportGateway.Sync_Zechini_Ordini(dbLoc);
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(string.Format("Impossibile comunicare con la macchina ACCOPPIATRICE MANUALE ZECHINI - {0}", ex.Message));
            }

        }




    }
}