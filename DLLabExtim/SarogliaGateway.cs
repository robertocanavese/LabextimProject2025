using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Globalization;

using CMLabExtim;



namespace DLLabExtim
{
    public class SarogliaGateway
    {

        private SharedConfiguration sharedConfiguration;

        public SarogliaGateway()
        {
            sharedConfiguration = new SharedConfiguration(2);
        }



        public void SendNewDataset()
        {

            try
            {

                SendExistingDataFileToFtp();

                List<RowValues> toSend;
                using (QuotationDataContext db = new QuotationDataContext())
                {
                    int[] processed = db.SarogliaDatas.Where(d => d.Stato == 1).Select(d => Convert.ToInt32(d.Commessa.Substring(0, 6).Trim())).ToArray();

                    toSend = db.VW_ProductionExtMPS_GroupedByPhases
                        .Where(d => d.IDProductionMachine == d.curMachineId && (d.IDProductionMachine == 15 || d.IDProductionMachine == 101) && d.poStatus == 1 && (d.Status == 11 || d.Status == 15) && !processed.Contains(d.IDProductionOrder.Value))
                            .OrderBy(d => d.DeliveryDate).ToList().Select(d =>
                                new RowValues
                                {
                                    Commessa = d.IDProductionOrder.ToString(),
                                    Descrizione = d.cuName.SubstringWithMaxLen(50),
                                    PzRichiesti = Convert.ToInt32(d.Quantity),
                                    StampaACaldo = (d.IDProductionMachine == 101),
                                    Fustellatura = (d.IDProductionMachine == 15)
                                }).Take(100).ToList();
                }
                if (toSend != null)
                    if (toSend.Count > 0)
                        SendDataFileToFtp(toSend);

            }
            catch (Exception _exception)
            {
                Log.Write("SarogliaGateway - SendNewDataset", _exception);
            }

        }

        private void RetrieveCurrentDataset(LocalService loc, string fileName)
        {

            try
            {
                FileInfo file = new FileInfo(Path.Combine(loc.InputDir, fileName));
                List<RowValues> values = File.ReadAllLines(file.FullName)
                                           .Skip(1)
                                           .Select(v => RowValues.FromCsv(v))
                                           .ToList();

                using (QuotationDataContext db = new QuotationDataContext())
                {

                    foreach (RowValues row in values.Where(d => d.Fustellatura == true))
                    {
                        SarogliaData sd = db.SarogliaDatas.FirstOrDefault(d => d.Commessa == row.Commessa);
                        if (sd == null)
                        {
                            VW_ProductionExtMPS_GroupedByPhase labextimFound = db.VW_ProductionExtMPS_GroupedByPhases.FirstOrDefault(d => d.IDProductionOrder == Convert.ToInt32(row.Commessa) && (d.IDProductionMachine == 15));

                            sd = new SarogliaData();
                            sd.Commessa = row.Commessa;
                            sd.DataFile = file.CreationTime;
                            sd.DatVar = DateTime.Now;
                            sd.StampaACaldo = false;
                            sd.Fustellatura = true;
                            sd.Fine = row.Fine;
                            sd.Inizio = row.Inizio;
                            sd.NomeFile = fileName;
                            sd.PzFatti = row.PzFatti;
                            sd.PzRichiesti = (labextimFound != null ? Convert.ToInt32(labextimFound.Quantity) : -1); // row.PzRichiesti;
                            sd.PzScarto = row.PzScarto;
                            sd.Completato = row.Completato;
                            sd.Stato = 1;
                            sd.tMacchina = (row.Fine == null ? new TimeSpan() : row.Fine.GetValueOrDefault().Subtract(row.Inizio.GetValueOrDefault())); 
                            db.SarogliaDatas.InsertOnSubmit(sd);
                        }
                        else
                        {
                            sd.DataFile = file.CreationTime;
                            sd.DatVar = DateTime.Now;
                            sd.Fine = row.Fine;
                            sd.Inizio = row.Inizio;
                            sd.NomeFile = fileName;
                            sd.PzFatti = row.PzFatti;
                            sd.PzRichiesti = row.PzRichiesti;
                            sd.PzScarto = row.PzScarto;
                            sd.Completato = row.Completato;
                            sd.Stato = 1;
                            sd.tMacchina = (row.Fine == null ? new TimeSpan() : row.Fine.GetValueOrDefault().Subtract(row.Inizio.GetValueOrDefault())); 
                        }
                    }

                    foreach (RowValues row in values.Where(d => d.StampaACaldo == true))
                    {
                        SarogliaData sd = db.SarogliaDatas.FirstOrDefault(d => d.Commessa == row.Commessa);
                        if (sd == null)
                        {
                            VW_ProductionExtMPS_GroupedByPhase labextimFound = db.VW_ProductionExtMPS_GroupedByPhases.FirstOrDefault(d => d.IDProductionOrder == Convert.ToInt32(row.Commessa) && (d.IDProductionMachine == 101));

                            sd = new SarogliaData();
                            sd.Commessa = row.Commessa;
                            sd.DataFile = file.CreationTime;
                            sd.DatVar = DateTime.Now;
                            sd.StampaACaldo = true;
                            sd.Fustellatura = false;
                            sd.Fine = row.Fine;
                            sd.Inizio = row.Inizio;
                            sd.NomeFile = fileName;
                            sd.PzFatti = row.PzFatti;
                            sd.PzRichiesti = (labextimFound != null ? Convert.ToInt32(labextimFound.Quantity) : -1); // row.PzRichiesti;
                            sd.PzScarto = row.PzScarto;
                            sd.Completato = row.Completato;
                            sd.Stato = 1;
                            sd.tMacchina = (row.Fine == null ? new TimeSpan() : row.Fine.GetValueOrDefault().Subtract(row.Inizio.GetValueOrDefault())); 
                            db.SarogliaDatas.InsertOnSubmit(sd);
                        }
                        else
                        {
                            sd.DataFile = file.CreationTime;
                            sd.DatVar = DateTime.Now;
                            sd.Fine = row.Fine;
                            sd.Inizio = row.Inizio;
                            sd.NomeFile = fileName;
                            sd.PzFatti = row.PzFatti;
                            sd.PzRichiesti = row.PzRichiesti;
                            sd.PzScarto = row.PzScarto;
                            sd.Completato = row.Completato;
                            sd.Stato = 1;
                            sd.tMacchina = (row.Fine == null ? new TimeSpan() : row.Fine.GetValueOrDefault().Subtract(row.Inizio.GetValueOrDefault())); 
                        }
                    }


                    db.SubmitChanges();
                }

            }
            catch (Exception _exception)
            {
                Log.Write("SarogliaGateway - RetrieveCurrentDataset", _exception);
            }

        }


        public class RowValues
        {
            public string Commessa { get; set; }
            public string Descrizione { get; set; }
            public Boolean StampaACaldo { get; set; }
            public Boolean Fustellatura { get; set; }
            public int? PzRichiesti { get; set; }
            public int? PzFatti { get; set; }
            public int? PzScarto { get; set; }
            public DateTime? Inizio { get; set; }
            public DateTime? Fine { get; set; }
            public Boolean Completato { get; set; }
            public TimeSpan? TMacchina { get; set; }

            public static RowValues FromCsv(string csvLine)
            {
                string[] values = csvLine.Replace("\"","").Split(';');
                RowValues rowValues = new RowValues();
                rowValues.Commessa = (string.IsNullOrEmpty(values[0]) ? null : values[0]);
                rowValues.Descrizione = (string.IsNullOrEmpty(values[1]) ? null : values[1]);
                rowValues.StampaACaldo = (string.IsNullOrEmpty(values[2]) ? false : values[2] == "1" ? true : false);
                rowValues.Fustellatura = (string.IsNullOrEmpty(values[3]) ? false : values[3] == "1" ? true : false);
                rowValues.PzRichiesti = (string.IsNullOrEmpty(values[4]) ? null : (int?)Convert.ToInt32(values[4]));
                rowValues.PzFatti = (string.IsNullOrEmpty(values[5]) ? null : (int?)Convert.ToInt32(values[5]));
                rowValues.PzScarto = (string.IsNullOrEmpty(values[6]) ? null : (int?)Convert.ToInt32(values[6]));
                rowValues.Inizio = (string.IsNullOrEmpty(values[7]) ? null : (DateTime?)DateTime.ParseExact(values[7], "yyyyMMdd HHmmss", CultureInfo.InvariantCulture));
                rowValues.Fine = (string.IsNullOrEmpty(values[8]) ? null : (DateTime?)DateTime.ParseExact(values[8], "yyyyMMdd HHmmss", CultureInfo.InvariantCulture));
                rowValues.Completato = (string.IsNullOrEmpty(values[9]) ? false : values[9] == "1" ? true : false);
                //rowValues.TMacchina = (string.IsNullOrEmpty(values[10]) ? null : (TimeSpan?)TimeSpan.ParseExact(values[10], "hh\\:mm", CultureInfo.InvariantCulture));
                return rowValues;
            }
        }


        private void SendExistingDataFileToFtp()
        {

            try
            {

                FtpService ftp = new FtpService(sharedConfiguration);
                LocalService loc = new LocalService(sharedConfiguration);

                try
                {

                    string outFile = "ToMachine.csv";
                    if (File.Exists(Path.Combine(loc.OutputDir, outFile)))
                    {
                        ftp.Upload(loc.OutputDir, outFile);
                        if (ftp.OutputDirFileExists(outFile))
                        {
                            string uniquefile = string.Format("{0}_{1}.csv", outFile.Substring(0, outFile.IndexOf('.')), DateTime.Now.ToString("yyyyMMdd_HHmmss"));
                            loc.RenameAndMoveFileToDir(outFile, uniquefile, loc.OutputDir, loc.ArchOutDir);
                        }
                    }

                }
                catch (Exception _ex)
                {
                    Log.WriteMessage(string.Format("SarogliaGateway - Il metodo {0} dell'attività {1} ha generato il seguente errore: {2}", MethodBase.GetCurrentMethod().Name, this.GetType().Name, _ex.Message));
                }

            }
            catch (Exception _ex)
            {
                Log.WriteMessage(string.Format("SarogliaGateway - Il metodo {0} dell'attività {1} ha generato il seguente errore: {2}", MethodBase.GetCurrentMethod().Name, this.GetType().Name, _ex.Message));
            }
        }



        private void SendDataFileToFtp(List<RowValues> rows)
        {

            try
            {

                FtpService ftp = new FtpService(sharedConfiguration);
                LocalService loc = new LocalService(sharedConfiguration);

                StringBuilder sb = new StringBuilder();

                sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}", "Commessa", "Descrizione", "StampaACaldo", "Fustellatura", "PzRichiesti", "PzFatti", "PzScarto", "Inizio", "Fine", "Completato"));
                foreach (RowValues row in rows)
                {
                    sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}", row.Commessa, row.Descrizione, row.StampaACaldo ? "1":"0", row.Fustellatura ? "1":"0", row.PzRichiesti, 0, 0, "", "", 0));
                }

                try
                {

                    string outFile = "ToMachine.csv";
                    File.WriteAllText(Path.Combine(loc.OutputDir, outFile), sb.ToString());

                    string lastSentFile = loc.ArchivedOutputDirGetLastFile("csv");
                    // invio il file solo se è diverso dall'ultimo inviato
                    if ((lastSentFile == null) || (!loc.FileCompare(lastSentFile, Path.Combine(loc.OutputDir, outFile))))
                    {
                        ftp.Upload(loc.OutputDir, outFile);
                        if (ftp.OutputDirFileExists(outFile))
                        {
                            string uniquefile = string.Format("{0}_{1}.csv", outFile.Substring(0, outFile.IndexOf('.')), DateTime.Now.ToString("yyyyMMdd_HHmmss"));
                            loc.RenameAndMoveFileToDir(outFile, uniquefile, loc.OutputDir, loc.ArchOutDir);
                        }
                    }

                }
                catch (Exception _ex)
                {
                    Log.WriteMessage(string.Format("SarogliaGateway - Il metodo {0} dell'attività {1} ha generato il seguente errore: {2}", MethodBase.GetCurrentMethod().Name, this.GetType().Name, _ex.Message));
                }

            }
            catch (Exception _ex)
            {
                Log.WriteMessage(string.Format("SarogliaGateway - Il metodo {0} dell'attività {1} ha generato il seguente errore: {2}", MethodBase.GetCurrentMethod().Name, this.GetType().Name, _ex.Message));
            }
        }


        public void ReceiveDataFromFtp()
        {

            try
            {

                FtpService ftp = new FtpService(sharedConfiguration);
                LocalService loc = new LocalService(sharedConfiguration);
                List<string> incomingFiles = ftp.GetListOfFiles(ftp.InputDir).Where(d => d.ToLower().Contains("output")).ToList();
                foreach (string file in incomingFiles.OrderBy(z => z))
                {
                    try
                    {
                        if (!loc.InputDirFileExists(file))
                        {
                            ftp.Download(file, loc.InputDir);
                            if (loc.InputDirFileExists(file))
                            {
                                ftp.Delete(Path.Combine(ftp.InputDir, file));
                            }
                            RetrieveCurrentDataset(loc, file);

                            string lastReceivedFile = loc.ArchivedInputDirGetLastFile("csv");

                            // archivio il file solo se è diverso dall'ultimo ricevuto
                            // archivio sempre il file !!!!
                            //if ((lastReceivedFile == null) || (!loc.FileCompare(lastReceivedFile, Path.Combine(loc.InputDir, file))))
                            //{
                                string uniquefile = string.Format("{0}_{1}.csv", file.Substring(0, file.IndexOf('.')), DateTime.Now.ToString("yyyyMMdd_HHmmss"));
                                loc.RenameAndMoveFileToDir(file, uniquefile, loc.InputDir, loc.ArchInDir);
                            //}
                            //else
                            //{
                            //    loc.InputDirDeleteFile(file);
                            //}
                        }
                    }
                    catch (Exception _ex)
                    {
                        Log.WriteMessage(string.Format("SarogliaGateway - Il metodo {0} dell'attività {1} ha generato il seguente errore: {2}", MethodBase.GetCurrentMethod().Name, this.GetType().Name, _ex.Message));
                    }
                }
            }
            catch (Exception _ex)
            {
                Log.WriteMessage(string.Format("SarogliaGateway - Il metodo {0} dell'attività {1} ha generato il seguente errore: {2}", MethodBase.GetCurrentMethod().Name, this.GetType().Name, _ex.Message));
            }
        }

        private static List<SarogliaData> GetDataFromFtp()
        {

            List<SarogliaData> result = new List<SarogliaData>();



            return result;

        }

        private void WriteDataToFtp()
        {



        }


        public static OdPBag GetCurOdP(QuotationDataContext db, Boolean stampaACaldo, Boolean fustellatura )
        {

            //return new OdPBag { Id = -1, CopieRichieste = 0, CopieLavorate = 0 };
            OdPBag result = new OdPBag { Id = -1, CopieRichieste = 0, CopieLavorate = 0 };
            try
            {
                List<SarogliaData> found = db.SarogliaDatas.Where(d => d.Completato == false && d.StampaACaldo == stampaACaldo && d.Fustellatura == fustellatura).ToList();
                result.Id = Convert.ToInt32(found[0].Commessa);
                result.CopieRichieste = found[0].PzRichiesti.GetValueOrDefault();
                result.CopieLavorate = found.Max(d => d.PzFatti).GetValueOrDefault();
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.Message);
            }
            return result;
        }

        public static OdPBag GetOdPHistoricalData(int poId, QuotationDataContext db, Boolean stampaACaldo, Boolean fustellatura)
        {

            OdPBag result = new OdPBag { Id = -1, CopieRichieste = 0, CopieLavorate = 0 };
            try
            {
                List<SarogliaData> found = db.SarogliaDatas.Where(d => Convert.ToInt32(d.Commessa.Substring(0, 6).Trim()) == poId && d.StampaACaldo == stampaACaldo && d.Fustellatura == fustellatura).ToList();
                result.Id = Convert.ToInt32(found[0].Commessa);
                result.CopieRichieste = found[0].PzRichiesti.GetValueOrDefault();
                result.CopieLavorate = found.Max(d => d.PzFatti).GetValueOrDefault();
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.Message);
            }
            return result;

        }


    }
}
