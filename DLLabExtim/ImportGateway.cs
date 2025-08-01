using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CMLabExtim;

namespace DLLabExtim
{
    public class ImportGateway
    {
        public static bool BulkCopyFromExternalSource(string originConnectionString, string originCommand, string destinationTable, string deleteFilterExpression,
            List<string> pipedPairs)
        {


            var _originConnection =
                new SqlConnection(originConnectionString);

            var _destinationConnection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["LabExtimConnectionString"].ConnectionString);
            SqlTransaction _transaction = null;

            try
            {
                _originConnection.Open();
                _destinationConnection.Open();
                _transaction = _destinationConnection.BeginTransaction();

                var _readerCommand = new SqlCommand(originCommand, _originConnection);
                var _reader = _readerCommand.ExecuteReader();

                var _destUncheckAllConstraint =
                    new SqlCommand("exec sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'",
                        _destinationConnection);
                _destUncheckAllConstraint.Transaction = _transaction;
                var _unchecked = _destUncheckAllConstraint.ExecuteNonQuery(); 

                var _destDeleteAllCommand = new SqlCommand("delete from " + destinationTable + " where 1=1 and " + deleteFilterExpression, _destinationConnection);
                _destDeleteAllCommand.Transaction = _transaction;
                var _deleted = _destDeleteAllCommand.ExecuteNonQuery();

                var _bulkCopy =
                    new SqlBulkCopy(_destinationConnection, SqlBulkCopyOptions.Default, _transaction);

                if (pipedPairs != null)
                {
                    foreach (var _pipedPair in pipedPairs)
                    {
                        var _pair = _pipedPair.Split('|');
                        _bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(_pair[0], _pair[1]));
                    }
                }

                _bulkCopy.BatchSize = 10000;
                _bulkCopy.BulkCopyTimeout = 3600;
                _bulkCopy.DestinationTableName = destinationTable;
                // occhio, il mapping è case sensitive!!!!!!!!!!!
                _bulkCopy.WriteToServer(_reader);

                var _destCheckAllConstraint =
                    new SqlCommand("exec sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL'", _destinationConnection);
                _destCheckAllConstraint.Transaction = _transaction;
                var _checked = _destCheckAllConstraint.ExecuteNonQuery();

                _transaction.Commit();

                return true;
            }
            catch (Exception _exception)
            {
                _transaction.Rollback();

                Log.Write("Bulk Copy", _exception);
                return false;
            }
            finally
            {
                _originConnection.Close();
                _destinationConnection.Close();
            }
        }

        public static List<ImportFieldMappingInfo> GetMappingInfos(string destTableName)
        {
            try
            {
                var _mappingInfos = new List<ImportFieldMappingInfo>();

                using (var _context = new GeneralDataContext())
                {
                    var _importItems =
                        from m in _context.ImportFieldMappingInfos
                        where m.DestTableName == destTableName
                        select new { m.OriginTableName, m.DestTableName, m.OriginFieldName, m.DestFieldName };

                    foreach (var _mappinInfoItem in _importItems)
                    {
                        _mappingInfos.Add(new ImportFieldMappingInfo
                        {
                            OriginTableName = _mappinInfoItem.OriginTableName,
                            DestTableName = _mappinInfoItem.DestTableName,
                            OriginFieldName = _mappinInfoItem.OriginFieldName,
                            DestFieldName = _mappinInfoItem.DestFieldName
                        });
                    }
                }
                return _mappingInfos;
            }
            catch (Exception _exception)
            {
                Log.Write("GetMappingInfos", _exception);
                return null;
            }
        }

        public static bool ExecuteBulkCopy(int idCompany, string originConnectionString, string originTableName, string originFilterExpression, string destTableName, string deleteFilterExpression)
        {
            var _pipedPairs = new List<string>();
            var _originFieldsSQLList = "select ";

            try
            {
                foreach (var _ifmi in GetMappingInfos(destTableName))
                {
                    _pipedPairs.Add("[" + _ifmi.OriginFieldName.TrimEnd() + "]" + "|" + "[" +
                        _ifmi.DestFieldName.TrimEnd() + "]");

                    if (_ifmi.OriginFieldName.TrimEnd() == "an_conto")
                    {
                        if (idCompany == 1)
                        {
                            _originFieldsSQLList += "[an_conto]" + ",";
                        }
                        if (idCompany == 2)
                        {
                            _originFieldsSQLList += "200000000 + [an_conto] as [an_conto]" + ",";
                        }
                    }
                    else
                    {
                        _originFieldsSQLList += "[" + _ifmi.OriginFieldName.TrimEnd() + "]" + ",";
                    }
                }

                _originFieldsSQLList = _originFieldsSQLList.Substring(0, _originFieldsSQLList.Length - 1);
                _originFieldsSQLList += " from " + originTableName;
                _originFieldsSQLList += " where " + originFilterExpression;
                BulkCopyFromExternalSource(originConnectionString, _originFieldsSQLList, destTableName, deleteFilterExpression, _pipedPairs);

                return true;
            }
            catch (Exception _exception)
            {
                Log.Write("ExecuteBulkCopy", _exception);
                return false;
            }
        }

        public static bool UpdateAgentCustomer()
        {
            try
            {
                var originConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["GestConnectionString"].ConnectionString);
                string sqlQuery = @"UPDATE [LabExtim].[dbo].[Customers] SET DescrizioneAgente1 = (SELECT tb_descage FROM Labe.dbo.tabcage where tabcage.tb_codcage = Customers.IDAgente1) where code between 1 and 199999999";
                var sqlCommand = new SqlCommand(sqlQuery, originConnection);
                originConnection.Open();
                sqlCommand.ExecuteNonQuery();
                originConnection.Close();

                originConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["GestConnectionString"].ConnectionString);
                sqlQuery = @"UPDATE [LabExtim].[dbo].[Customers] SET DescrizioneAgente1 = (SELECT tb_descage FROM Cartolabe.dbo.tabcage where tabcage.tb_codcage = Customers.IDAgente1) where code between 200000000 and 299999999";
                sqlCommand = new SqlCommand(sqlQuery, originConnection);
                originConnection.Open();
                sqlCommand.ExecuteNonQuery();
                originConnection.Close();

                return true;
            }
            catch (Exception ex)
            {
                Log.Write("UpdateAgentCustomer", ex);
                return false;
            }


        }

        public static bool Import(int tablesGroup)
        {
            var _success = false;
            try
            {
                switch (tablesGroup)
                {
                    case 0:

                        ExecuteBulkCopy(1, ConfigurationManager.ConnectionStrings["GestConnectionString"].ConnectionString, "anagra", "an_tipo in ('C')", "Customers", "code between 1 and 199999999");
                        ExecuteBulkCopy(1, ConfigurationManager.ConnectionStrings["GestConnectionString"].ConnectionString, "anagra", "an_tipo in ('F')", "Suppliers", "code between 1 and 199999999");

                        ExecuteBulkCopy(2, ConfigurationManager.ConnectionStrings["GestCARTOLABEConnectionString"].ConnectionString, "anagra", "an_tipo in ('C')", "Customers", "code between 200000000 and 299999999");
                        ExecuteBulkCopy(2, ConfigurationManager.ConnectionStrings["GestCARTOLABEConnectionString"].ConnectionString, "anagra", "an_tipo in ('F')", "Suppliers", "code between 200000000 and 299999999");

                        UpdateAgentCustomer();
                        ComposeLongCompanyName();
                        break;

                    default:
                        break;
                }
                _success = true;
            }
            catch (Exception _exception)
            {
                Log.Write("Importazione dati", _exception);
            }
            return _success;
        }

        public static bool Sync_EuroProgetti_DB_Ordini(QuotationDataContext dbLoc, Sql_EpDataContext dbRem)
        {
            var _success = false;
            try
            {

                dbLoc.prc_LAB_DelIns_LAB_ProductionExtMPS_GroupedByPhaseToEuroProgetti(null);

                foreach (EuroProgetti_DB_Ordini odp in dbLoc.EuroProgetti_DB_Ordinis.Where(d => d.LabextimStatus == 0))
                {
                    DB_Ordini newOdp = new DB_Ordini();
                    newOdp.i_ID = odp.i_ID;
                    newOdp.i_Cliente = odp.i_Cliente.ControlCharsCleaned();
                    newOdp.i_Codice = odp.i_Codice.ControlCharsCleaned();
                    newOdp.i_Quantita = odp.i_Quantita;

                    // dati al momento non forniti
                    //newOdp.i_o_Ricetta = odp.i_o_Ricetta;
                    //newOdp.i_Passaggi = odp.i_Passaggi;
                    //newOdp.i_Larghezza = odp.i_Larghezza;
                    //newOdp.i_Lunghezza = odp.i_Lunghezza;


                    newOdp.i_o_Ricetta = "";
                    newOdp.i_Passaggi = 0;
                    newOdp.i_Larghezza = 0;
                    newOdp.i_Lunghezza = 0;
                    newOdp.i_o_Sospensione = "LAVORO DA INIZIARE";

                    newOdp.o_Pezzi = 0;
                    newOdp.o_FogliIncollati = 0;
                    newOdp.o_Cartoni = 0;
                    newOdp.o_Ribordati = 0;
                    newOdp.o_Scarti = 0;
                    newOdp.o_TimeEmergenza = "";
                    newOdp.o_ProdPezzi = "";
                    newOdp.o_TimeArresto = "";
                    newOdp.o_TimeAvviamento = "";
                    newOdp.o_TimeProduzione = "";
                    newOdp.o_TimeRegistrazione = "";
                    newOdp.o_BitEmergenza = false;
                    newOdp.o_BitArresto = false;
                    newOdp.o_BitAvviamento = false;
                    newOdp.o_BitProduzione = false;
                    newOdp.o_DataInizio = "";
                    newOdp.o_OraInizio = "";
                    newOdp.o_DataFine = "";
                    newOdp.o_OraFine = "";

                    newOdp.o_pezzi_2 = 0;
                    newOdp.o_FogliIncollati_2 = 0;
                    newOdp.o_Cartoni_2 = 0;
                    newOdp.o_Ribordati_2 = 0;
                    newOdp.o_Scarti_2 = 0;
                    newOdp.o_TimeEmergenza_2 = "";
                    newOdp.o_ProdPezzi_2 = "";
                    newOdp.o_TimeArresto_2 = "";
                    newOdp.o_TimeAvviamento_2 = "";
                    newOdp.o_TimeProduzione_2 = "";
                    newOdp.o_TimeRegistrazione_2 = "";
                    newOdp.o_BitEmergenza_2 = false;
                    newOdp.o_BitArresto_2 = false;
                    newOdp.o_BitAvviamento_2 = false;
                    newOdp.o_BitProduzione_2 = false;
                    newOdp.o_DataInizio_2 = "";
                    newOdp.o_OraInizio_2 = "";
                    newOdp.o_DataFine_2 = "";
                    newOdp.o_OraFine_2 = "";
                    newOdp.i_Larghezza_2 = 0;
                    newOdp.i_Lunghezza_2 = 0;

                    dbRem.DB_Ordinis.InsertOnSubmit(newOdp);

                    odp.LabextimStatus = 1;
                }

                dbRem.SubmitChanges();
                dbLoc.SubmitChanges();

                foreach (EuroProgetti_DB_Ordini odp in dbLoc.EuroProgetti_DB_Ordinis.Where(d => d.LabextimStatus == 1))
                {
                    DB_Ordini foundOnMachine = dbRem.DB_Ordinis.FirstOrDefault(d => d.i_ID == odp.i_ID);
                    if (foundOnMachine != null)
                    {
                        odp.i_o_Ricetta = foundOnMachine.i_o_Ricetta;
                        odp.o_Pezzi = foundOnMachine.o_Pezzi;
                        odp.o_FogliIncollati = foundOnMachine.o_FogliIncollati;
                        odp.o_Cartoni = foundOnMachine.o_Cartoni;
                        odp.o_Ribordati = foundOnMachine.o_Ribordati;
                        odp.o_Scarti = foundOnMachine.o_Scarti;
                        odp.o_TimeEmergenza = foundOnMachine.o_TimeEmergenza;
                        odp.o_Prodpezzi = foundOnMachine.o_ProdPezzi;
                        odp.o_TimeArresto = foundOnMachine.o_TimeArresto;
                        odp.o_TimeAvviamento = foundOnMachine.o_TimeAvviamento;
                        odp.o_TimeProduzione = foundOnMachine.o_TimeProduzione;
                        odp.o_TimeRegistrazione = foundOnMachine.o_TimeRegistrazione;
                        odp.o_BitEmergenza = foundOnMachine.o_BitEmergenza;
                        odp.o_BitArresto = foundOnMachine.o_BitArresto;
                        odp.o_BitAvviamento = foundOnMachine.o_BitAvviamento;
                        odp.o_BitProduzione = foundOnMachine.o_BitProduzione;
                        odp.o_DataInizio = foundOnMachine.o_DataInizio;
                        odp.o_OraInizio = foundOnMachine.o_OraInizio;
                        odp.o_DataFine = foundOnMachine.o_DataFine;
                        odp.o_OraFine = foundOnMachine.o_OraFine;
                        odp.i_o_Sospensione = foundOnMachine.i_o_Sospensione;

                        odp.i_Passaggi = foundOnMachine.i_Passaggi;
                        odp.i_Larghezza = foundOnMachine.i_Larghezza;
                        odp.i_Lunghezza = foundOnMachine.i_Lunghezza;

                        odp.o_pezzi_2 = foundOnMachine.o_pezzi_2;
                        odp.o_FogliIncollati_2 = foundOnMachine.o_FogliIncollati_2;
                        odp.o_Cartoni_2 = foundOnMachine.o_Cartoni_2;
                        odp.o_Ribordati_2 = foundOnMachine.o_Ribordati_2;
                        odp.o_Scarti_2 = foundOnMachine.o_Scarti_2;
                        odp.o_TimeEmergenza_2 = foundOnMachine.o_TimeEmergenza_2;
                        odp.o_ProdPezzi_2 = foundOnMachine.o_ProdPezzi_2;
                        odp.o_TimeArresto_2 = foundOnMachine.o_TimeArresto_2;
                        odp.o_TimeAvviamento_2 = foundOnMachine.o_TimeAvviamento_2;
                        odp.o_TimeProduzione_2 = foundOnMachine.o_TimeProduzione_2;
                        odp.o_TimeRegistrazione_2 = foundOnMachine.o_TimeRegistrazione_2;
                        odp.o_BitEmergenza_2 = foundOnMachine.o_BitEmergenza_2;
                        odp.o_BitArresto_2 = foundOnMachine.o_BitArresto_2;
                        odp.o_BitAvviamento_2 = foundOnMachine.o_BitAvviamento_2;
                        odp.o_BitProduzione_2 = foundOnMachine.o_BitProduzione_2;
                        odp.o_DataInizio_2 = foundOnMachine.o_DataInizio_2;
                        odp.o_OraInizio_2 = foundOnMachine.o_OraInizio_2;
                        odp.o_DataFine_2 = foundOnMachine.o_DataFine_2;
                        odp.o_OraFine_2 = foundOnMachine.o_OraFine_2;
                        odp.i_Larghezza_2 = foundOnMachine.i_Larghezza_2;
                        odp.i_Lunghezza_2 = foundOnMachine.i_Lunghezza_2;

                    }
                }


                //foreach (DB_Ordini remPo in dbRem.DB_Ordinis.Where(d =>
                //    d.i_o_Sospensione == "LAVORO CONCLUSO"
                //    &&
                //    (
                //    (d.i_Passaggi == 1 && d.o_DataFine != null && d.o_DataFine != "")
                //    ||
                //    (d.i_Passaggi == 2 && d.o_DataFine_2 != null && d.o_DataFine_2 != "")
                //    )))

                foreach (DB_Ordini remPo in dbRem.DB_Ordinis.Where(d => d.i_o_Sospensione == "LAVORO CONCLUSO" && d.o_DataFine != ""))
                {
                    EuroProgetti_DB_Ordini current = dbLoc.EuroProgetti_DB_Ordinis.FirstOrDefault(d => d.i_ID == remPo.i_ID);
                    if (current != null)
                    {
                        ProductionOrder currentPo = dbLoc.ProductionOrders.FirstOrDefault(d => d.ID == current.LabextimID_OdP);
                        if (currentPo.Status == 3 || currentPo.Status == 7)
                        {
                            dbRem.DB_Ordinis.DeleteOnSubmit(remPo);
                        }
                    }
                }

                dbLoc.SubmitChanges();
                dbRem.SubmitChanges();
                _success = true;
            }
            catch (Exception _exception)
            {
                Log.Write("Importazione dati", _exception);
            }
            return _success;
        }

        public static bool AutoOpen_EcoSystem_ORDINE(QuotationDataContext db)
        {
            var _success = false;
            try
            {

                OdPBag current = EcoSystemGateway.GetCurOdP(db);
                if (current.Id != -1)
                {
                    VW_ProductionExtMP wmp = db.VW_ProductionExtMPs.FirstOrDefault(d => d.IDProductionOrder == current.Id && d.Status != 12 && d.IDProductionMachine == 19);
                    var toManageTimeStamp = db.ProductionTimeStamps.OrderByDescending(po => po.ID).FirstOrDefault(po => po.IDProductionOrder == current.Id && po.MinIDQuotationDetail == wmp.IDQuotationDetail);

                    if (toManageTimeStamp == null)
                    {
                        toManageTimeStamp = new ProductionTimeStamp();
                        toManageTimeStamp.IDProductionOrder = current.Id;
                        toManageTimeStamp.MinIDQuotationDetail = wmp.IDQuotationDetail;
                        toManageTimeStamp.ProdStart = DateTime.Now;
                        toManageTimeStamp.IdUser = -1;
                        db.ProductionTimeStamps.InsertOnSubmit(toManageTimeStamp);
                    }
                    else if (toManageTimeStamp != null && toManageTimeStamp.ProdEnd != null)
                    {
                        toManageTimeStamp = new ProductionTimeStamp();
                        toManageTimeStamp.IDProductionOrder = current.Id;
                        toManageTimeStamp.MinIDQuotationDetail = wmp.IDQuotationDetail;
                        toManageTimeStamp.ProdStart = DateTime.Now;
                        toManageTimeStamp.IdUser = -1;
                        db.ProductionTimeStamps.InsertOnSubmit(toManageTimeStamp);
                    }
                    else
                    {

                    }
                    db.SubmitChanges();
                }
                _success = true;
            }
            catch (Exception _exception)
            {
                Log.Write("Importazione dati - AutoOpen_EcoSystem_ORDINE ", _exception);
            }
            return _success;
        }

        public static bool Sync_Zechini_Ordini(QuotationDataContext dbLoc)
        {
            var _success = false;
            try
            {

                ZechiniGateway gw = new ZechiniGateway();
                gw.ReceiveDataFromFtp();
                gw.SendNewDataset();
                _success = true;
            }
            catch (Exception _exception)
            {
                Log.Write("Importazione dati - Sync_Zechini_Ordini", _exception);
            }
            return _success;
        }

        public static bool ComposeLongCompanyName()
        {
            var _success = false;
            try
            {
                var _ctx = new QuotationDataContext();
                var _affected = _ctx.prc_LAB_Upd_LAB_LongCompanyName();
                _success = true;
            }
            catch (Exception _exception)
            {
                Log.Write("Trasformazione dati", _exception);
            }
            return _success;
        }

        public static bool BackUp(string connectionString, string databaseName, string destinationPath,
            string networkDestinationPath)
        {
            var _connection = new SqlConnection(connectionString);
            try
            {
                _connection.Open();
                var _backupCommand =
                    new SqlCommand(
                        "BACKUP DATABASE " + databaseName + " TO DISK='" + destinationPath + @"\" + databaseName +
                        ".bak' WITH INIT", _connection);
                _backupCommand.CommandTimeout = 1200;
                var _done = _backupCommand.ExecuteNonQuery();
                _connection.Close();

                Process _compressAction;
                var _timeStamp = DateTime.Now;
                _compressAction = Process.Start(
                    @"C:\CFBDeployment\Utilities\7za.exe", "a -tzip " +
                                                           '"' + destinationPath + @"\" + databaseName +
                                                           _timeStamp.ToString("yyyyMMdd-HHmmss") + ".zip" + '"' + " " +
                                                           '"' + destinationPath + @"\" + databaseName + ".bak" + '"');
                while (!_compressAction.HasExited)
                {
                }
                if (networkDestinationPath != null)
                {
                    CopyToNetworkDrive(databaseName, destinationPath, networkDestinationPath, null);
                    CopyToNetworkDrive(databaseName, destinationPath, networkDestinationPath, _timeStamp);
                }
                return true;
            }
            catch (Exception _exception)
            {
                Log.Write("BackUp " + databaseName, _exception);
                throw;
                //return false;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public static void CopyToNetworkDrive(string databaseName, string originPath, string destinationPath,
            DateTime? zipFileTimeStamp)
        {
            try
            {
                var _impersonation = new Impersonation();

                if (_impersonation.ImpersonateValidUser(
                    ConfigurationManager.AppSettings["BackupNetworkPathUsername"],
                    ConfigurationManager.AppSettings["BackupNetworkPathDomain"],
                    ConfigurationManager.AppSettings["BackupNetworkPathPassword"]))
                {
                    if (zipFileTimeStamp == null)
                        File.Copy(originPath + @"\" + databaseName + ".bak",
                            destinationPath + @"\" + databaseName + ".bak", true);
                    else
                        File.Copy(
                            originPath + @"\" + databaseName + zipFileTimeStamp.Value.ToString("yyyyMMdd-HHmmss") +
                            ".zip",
                            destinationPath + @"\" + databaseName + zipFileTimeStamp.Value.ToString("yyyyMMdd-HHmmss") +
                            ".zip", true);
                    _impersonation.UndoImpersonation();
                }
            }
            catch (Exception _exception)
            {
                Log.Write("Copia " + databaseName, _exception);
                throw;
            }
        }
    }
}