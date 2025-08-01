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
    public class ZechiniGateway
    {

        private SharedConfiguration sharedConfiguration;

        public ZechiniGateway()
        {
            sharedConfiguration = new SharedConfiguration();
        }



        public void SendNewDataset()
        {

            try
            {

                List<RowValues> firstFives;
                using (QuotationDataContext db = new QuotationDataContext())
                {
                    int[] processed = db.ZechiniDatas.Where(d => d.Stato == 1).Select(d => Convert.ToInt32(d.Commessa.Substring(0, 6).Trim())).ToArray();

                    firstFives = db.VW_ProductionExtMPS_GroupedByPhases
                        .Where(d => d.IDProductionMachine == d.curMachineId && d.IDProductionMachine == 107 && d.poStatus == 1 && (d.Status == 11 || d.Status == 15) && !processed.Contains(d.IDProductionOrder.Value))
                            .OrderBy(d => d.DeliveryDate).ToList().Select(d =>
                                new RowValues
                                {
                                    Commessa = d.IDProductionOrder.ToString() + " " + d.cuName.SubstringWithMaxLen(8),
                                    PzRichiesti = Convert.ToInt32(d.Quantity)
                                }).Take(5).ToList();
                }
                if (firstFives != null)
                    if (firstFives.Count > 0)
                        SendDataFileToFtp(firstFives);

            }
            catch (Exception _exception)
            {
                Log.Write("ZechiniGateway - SendNewDataset", _exception);
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

                    foreach (RowValues row in values)
                    {
                        ZechiniData zd = db.ZechiniDatas.FirstOrDefault(d => d.Commessa == row.Commessa);
                        if (zd == null)
                        {
                            VW_ProductionExtMPS_GroupedByPhase labextimFound = db.VW_ProductionExtMPS_GroupedByPhases.FirstOrDefault(d => d.IDProductionOrder == Convert.ToInt32(row.Commessa.Substring(0, row.Commessa.IndexOf(" "))) && d.IDProductionMachine == 107);

                            zd = new ZechiniData();
                            zd.Commessa = row.Commessa;
                            zd.DataFile = file.CreationTime;
                            zd.DatVar = DateTime.Now;
                            zd.Fine = row.Fine;
                            zd.Inizio = row.Inizio;
                            zd.NomeFile = fileName;
                            zd.PzFatti = row.PzFatti;
                            zd.PzRichiesti = (labextimFound != null ? Convert.ToInt32(labextimFound.Quantity) : -1); // row.PzRichiesti;
                            zd.Stato = 1;
                            zd.tMacchina = row.Fine.GetValueOrDefault().Subtract(row.Inizio.GetValueOrDefault()); // row.TMacchina
                            db.ZechiniDatas.InsertOnSubmit(zd);
                        }
                        else
                        {
                            zd.DataFile = file.CreationTime;
                            zd.DatVar = DateTime.Now;
                            zd.Fine = row.Fine;
                            zd.Inizio = row.Inizio;
                            zd.NomeFile = fileName;
                            zd.PzFatti = row.PzFatti;
                            zd.PzRichiesti = row.PzRichiesti;
                            zd.Stato = 1;
                            zd.tMacchina = row.Fine.GetValueOrDefault().Subtract(row.Inizio.GetValueOrDefault());  // row.TMacchina
                            zd.pzMephisto = row.PzMephisto;
                        }
                    }

                    db.SubmitChanges();
                }

            }
            catch (Exception _exception)
            {
                Log.Write("ZechiniGateway - RetrieveCurrentDataset", _exception);
            }

        }


        public class RowValues
        {
            public string Commessa { get; set; }
            public int? PzRichiesti { get; set; }
            public int? PzFatti { get; set; }
            public DateTime? Inizio { get; set; }
            public DateTime? Fine { get; set; }
            public TimeSpan? TMacchina { get; set; }
            public int? PzMephisto { get; set; }

            public static RowValues FromCsv(string csvLine)
            {
                string[] values = csvLine.Split(';');
                RowValues rowValues = new RowValues();
                rowValues.Commessa = (string.IsNullOrEmpty(values[0]) ? null : values[0]);
                rowValues.PzRichiesti = (string.IsNullOrEmpty(values[1]) ? null : (int?)Convert.ToInt32(values[1]));
                rowValues.PzFatti = (string.IsNullOrEmpty(values[2]) ? null : (int?)Convert.ToInt32(values[2]));
                rowValues.Inizio = (string.IsNullOrEmpty(values[3]) ? null : (DateTime?)DateTime.ParseExact(values[3], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture));
                rowValues.Fine = (string.IsNullOrEmpty(values[4]) ? null : (DateTime?)DateTime.ParseExact(values[4], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture));
                rowValues.TMacchina = (string.IsNullOrEmpty(values[5]) ? null : (TimeSpan?)TimeSpan.ParseExact(values[5], "hh\\:mm", CultureInfo.InvariantCulture));
                rowValues.PzMephisto = (string.IsNullOrEmpty(values[6]) ? null : (int?)Convert.ToInt32(values[6]));
                return rowValues;
            }
        }

        private void SendDataFileToFtp(List<RowValues> rows)
        {

            try
            {

                FtpService ftp = new FtpService(sharedConfiguration);
                LocalService loc = new LocalService(sharedConfiguration);

                StringBuilder sb = new StringBuilder();


                foreach (RowValues row in rows)
                {
                    sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6}", row.Commessa, row.PzRichiesti, 0, 0, 0, 0, ""));
                }

                try
                {

                    string outFile = "ESB_input.csv";
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
                    Log.WriteMessage(string.Format("ZechiniGateway - Il metodo {0} dell'attività {1} ha generato il seguente errore: {2}", MethodBase.GetCurrentMethod().Name, this.GetType().Name, _ex.Message));
                }

            }
            catch (Exception _ex)
            {
                Log.WriteMessage(string.Format("ZechiniGateway - Il metodo {0} dell'attività {1} ha generato il seguente errore: {2}", MethodBase.GetCurrentMethod().Name, this.GetType().Name, _ex.Message));
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
                            if ((lastReceivedFile == null) || (!loc.FileCompare(lastReceivedFile, Path.Combine(loc.InputDir, file))))
                            {
                                string uniquefile = string.Format("{0}_{1}.csv", file.Substring(0, file.IndexOf('.')), DateTime.Now.ToString("yyyyMMdd_HHmmss"));
                                loc.RenameAndMoveFileToDir(file, uniquefile, loc.InputDir, loc.ArchInDir);
                            }
                            else
                            {
                                loc.InputDirDeleteFile(file);
                            }
                        }
                    }
                    catch (Exception _ex)
                    {
                        Log.WriteMessage(string.Format("ZechiniGateway - Il metodo {0} dell'attività {1} ha generato il seguente errore: {2}", MethodBase.GetCurrentMethod().Name, this.GetType().Name, _ex.Message));
                    }
                }
            }
            catch (Exception _ex)
            {
                Log.WriteMessage(string.Format("ZechiniGateway - Il metodo {0} dell'attività {1} ha generato il seguente errore: {2}", MethodBase.GetCurrentMethod().Name, this.GetType().Name, _ex.Message));
            }
        }

        private static List<ZechiniData> GetDataFromFtp()
        {

            List<ZechiniData> result = new List<ZechiniData>();



            return result;

        }

        private void WriteDataToFtp()
        {



        }


        public static OdPBag GetCurOdP(QuotationDataContext db)
        {
            // La Zechini non è al momento in grado di indicare quale odP sta lavorando
                return new OdPBag { Id = -1, CopieRichieste = 0, CopieLavorate = 0 };
        }

        public static OdPBag GetOdPHistoricalData(int poId, QuotationDataContext db)
        {

            OdPBag result = new OdPBag { Id = -1, CopieRichieste = 0, CopieLavorate = 0 };
            try
            {
                List<ZechiniData> found = db.ZechiniDatas.Where(d => Convert.ToInt32(d.Commessa.Substring(0, 6).Trim()) == poId).ToList();
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
