using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Configuration;
using System.Xml.Serialization;
using System.IO;
using System.Net.Cache;

using Snap7;
using CMLabExtim;
using CMLabExtim.S7Classes;
//using Sharp7;


namespace DLLabExtim
{
    public class Snap7Gateway
    {

        private S7Client Client;
        private byte[] Buffer = new byte[65536];

        private string ipAddress;
        private int rack;
        private int slot;
        private int dbNumber;
        private int size;
        private string dump;



        public Snap7Gateway()
        {

            string[] connData = ConfigurationManager.AppSettings["SilkFoil1_IPAddressAndRackAndSlotAndDBAndSize"].ToString().Split('|');
            ipAddress = connData[0];
            rack = System.Convert.ToInt32(connData[1]);
            slot = System.Convert.ToInt32(connData[2]);
            dbNumber = System.Convert.ToInt32(connData[3]);
            size = System.Convert.ToInt32(connData[4]);

            Client = new S7Client();
            Connect();

        }

        /// <summary>
        /// 0 per connessione OK
        /// </summary>
        /// <returns></returns>
        private int Connect()
        {
            int result = Client.ConnectTo(ipAddress, rack, slot);
            string msg = Client.ErrorText(result) + " (" + Client.ExecTime().ToString() + " ms)";
            Log.WriteMessage(msg);
            if (result != 0)
            {
                throw new Exception(msg);
            }
            return result;
        }

        public void Disconnect()
        {
            Client.Disconnect();
        }

        private void PlcDBRead()
        {
            int result = Client.DBRead(dbNumber, 0, size, Buffer);
            if (result == 0)
                HexDump(Buffer, size);
        }

        private void PlcDBWrite()
        {
            int result = Client.DBWrite(dbNumber, 0, size, Buffer);
            if (result != 0)
            {
                string msg = Client.ErrorText(result);
                Log.WriteMessage(msg);
            }

        }

        private void HexDump(byte[] bytes, int Size)
        {
            if (bytes == null)
                return;
            int bytesLength = Size;
            int bytesPerLine = 16;

            char[] HexChars = "0123456789ABCDEF".ToCharArray();

            int firstHexColumn =
                  8                   // 8 characters for the address
                + 3;                  // 3 spaces

            int firstCharColumn = firstHexColumn
                + bytesPerLine * 3       // - 2 digit for the hexadecimal value and 1 space
                + (bytesPerLine - 1) / 8 // - 1 extra space every 8 characters from the 9th
                + 2;                  // 2 spaces 

            int lineLength = firstCharColumn
                + bytesPerLine           // - characters to show the ascii value
                + Environment.NewLine.Length; // Carriage return and line feed (should normally be 2)

            char[] line = (new String(' ', lineLength - 2) + Environment.NewLine).ToCharArray();
            int expectedLines = (bytesLength + bytesPerLine - 1) / bytesPerLine;
            StringBuilder result = new StringBuilder(expectedLines * lineLength);

            for (int i = 0; i < bytesLength; i += bytesPerLine)
            {
                line[0] = HexChars[(i >> 28) & 0xF];
                line[1] = HexChars[(i >> 24) & 0xF];
                line[2] = HexChars[(i >> 20) & 0xF];
                line[3] = HexChars[(i >> 16) & 0xF];
                line[4] = HexChars[(i >> 12) & 0xF];
                line[5] = HexChars[(i >> 8) & 0xF];
                line[6] = HexChars[(i >> 4) & 0xF];
                line[7] = HexChars[(i >> 0) & 0xF];

                int hexColumn = firstHexColumn;
                int charColumn = firstCharColumn;

                for (int j = 0; j < bytesPerLine; j++)
                {
                    if (j > 0 && (j & 7) == 0) hexColumn++;
                    if (i + j >= bytesLength)
                    {
                        line[hexColumn] = ' ';
                        line[hexColumn + 1] = ' ';
                        line[charColumn] = ' ';
                    }
                    else
                    {
                        byte b = bytes[i + j];
                        line[hexColumn] = HexChars[(b >> 4) & 0xF];
                        line[hexColumn + 1] = HexChars[b & 0xF];
                        line[charColumn] = (b < 32 ? '·' : (char)b);
                    }
                    hexColumn += 3;
                    charColumn++;
                }
                result.Append(line);
            }
            dump = result.ToString();
        }

        public void SetOdPDataToSilkFoil1(QuotationDataContext db, int labextimOdp, int copieDaProdurre, string titoloOdp)
        {
            try
            {

                S7Data s7data = db.S7Datas.OrderByDescending(d => d.ID).FirstOrDefault(d => d.ID_Odp == labextimOdp);
                S7Data new_s7data = null;
                if (s7data == null)
                {
                    new_s7data = new S7Data();
                    new_s7data.ID_Odp = labextimOdp;
                    new_s7data.OdpMacchina = labextimOdp;
                    new_s7data.Titolo = (titoloOdp.Length > 30 ? titoloOdp.Substring(0, 30) : titoloOdp);
                    new_s7data.CopieDaProdurre = copieDaProdurre;
                    new_s7data.DataOraInizio = DateTime.Now;
                    new_s7data.Stato = 0;
                    db.S7Datas.InsertOnSubmit(new_s7data);
                }
                if (s7data != null && s7data.Stato == 1)
                {
                    new_s7data = new S7Data();
                    new_s7data.ID_Odp = labextimOdp;
                    new_s7data.OdpMacchina = labextimOdp;
                    new_s7data.Titolo = (titoloOdp.Length > 30 ? titoloOdp.Substring(0, 30) : titoloOdp);
                    new_s7data.CopieDaProdurre = copieDaProdurre;
                    new_s7data.DataOraInizio = DateTime.Now;
                    new_s7data.Stato = 0;
                    db.S7Datas.InsertOnSubmit(new_s7data);
                }

                PlcDBRead();

                if (S7.GetDWordAt(Buffer, 0) != labextimOdp)
                {
                    S7.SetUDIntAt(Buffer, 0, Convert.ToUInt32(labextimOdp));
                    S7.SetUDIntAt(Buffer, 4, Convert.ToUInt32(copieDaProdurre));
                    S7.SetStringAt(Buffer, 8, 30, titoloOdp);
                }

                PlcDBWrite();
                Disconnect();


                // se l'odp era già stato lavorato parzialmente in precedenza scrivo sulla macchina gli ultimi dati
                if (s7data != null && s7data.Stato == 1)
                {
                    Connect();

                    S7.SetUDIntAt(Buffer, 40, Convert.ToUInt32(s7data.CopieProdotte));
                    S7.SetUDIntAt(Buffer, 44, Convert.ToUInt32(s7data.MetriLineariLavorati));

                    S7.SetUDIntAt(Buffer, 48, Convert.ToUInt32(s7data.MinutiMacchinaAccesa / 60));
                    S7.SetUDIntAt(Buffer, 52, Convert.ToUInt32(s7data.MinutiMacchinaInPassaggio / 60));
                    S7.SetUDIntAt(Buffer, 56, Convert.ToUInt32(s7data.MinutiMacchinaInPressa / 60));

                    S7.SetUDIntAt(Buffer, 106, Convert.ToUInt32(s7data.MinutiMacchinaAccesa % 60));
                    S7.SetUDIntAt(Buffer, 114, Convert.ToUInt32(s7data.MinutiMacchinaInPassaggio % 60));
                    S7.SetUDIntAt(Buffer, 122, Convert.ToUInt32(s7data.MinutiMacchinaInPressa % 60));

                    S7.SetUDIntAt(Buffer, 126, Convert.ToUInt32(s7data.MinutiMacchinaAccesa));
                    S7.SetUDIntAt(Buffer, 130, Convert.ToUInt32(s7data.MinutiMacchinaInPassaggio));
                    S7.SetUDIntAt(Buffer, 134, Convert.ToUInt32(s7data.MinutiMacchinaInPressa));

                    PlcDBWrite();
                    Disconnect();
                    
                }

            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.Message);
            }

            //#endif

        }

        public void GetCurrentDataFromSilkFoil1(QuotationDataContext db, int labextimOdp, int copieDaProdurre, string titoloOdp)
        {

            try
            {
                //#if !DEBUG
                PlcDBRead();
                S7_DATI result = new S7_DATI();
                result.OdP = S7.GetDWordAt(Buffer, 0);
                result.CopieDaProdurre = S7.GetDWordAt(Buffer, 4);
                result.TitoloOdp = Convert.ToString(S7.GetStringAt(Buffer, 8));

                // campi azzerati ad inizio lavorazione OdP
                result.CopieProdotte = S7.GetDWordAt(Buffer, 40);
                result.MetriLineariLavorati = S7.GetDWordAt(Buffer, 44);

                result.OreMinutiOreMacchinaAccesa = S7.GetDWordAt(Buffer, 48);
                result.OreMinutiOreMacchinaInPassaggio = S7.GetDWordAt(Buffer, 52);
                result.OreMinutiOreMacchinaInPressa = S7.GetDWordAt(Buffer, 56);

                result.OreMinutiMinutiMacchinaAccesa = S7.GetDWordAt(Buffer, 106);
                result.OreMinutiMinutiMacchinaInPassaggio = S7.GetDWordAt(Buffer, 114);
                result.OreMinutiMinutiMacchinaInPressa = S7.GetDWordAt(Buffer, 122);

                result.MinutiMacchinaAccesa = S7.GetDWordAt(Buffer, 126);
                result.MinutiMacchinaInPassaggio = S7.GetDWordAt(Buffer, 130);
                result.MinutiMacchinaInPressa = S7.GetDWordAt(Buffer, 134);

                S7Data s7data = db.S7Datas.OrderByDescending(d => d.ID).FirstOrDefault(d => d.OdpMacchina == result.OdP);
                S7Data new_s7data = null;
                if (s7data == null)
                {
                    new_s7data = new S7Data();
                    new_s7data.ID_Odp = Convert.ToInt32(result.OdP);
                    new_s7data.OdpMacchina = labextimOdp;
                    new_s7data.Titolo = (titoloOdp.Length > 30 ? titoloOdp.Substring(0, 30) : titoloOdp);
                    new_s7data.CopieDaProdurre = copieDaProdurre;
                    new_s7data.DataOraInizio = DateTime.Now.AddMinutes(-Convert.ToInt32(result.MinutiMacchinaAccesa));

                    new_s7data.DataOraFine = DateTime.Now;
                    new_s7data.Stato = 1;
                    new_s7data.CopieProdotte = Convert.ToInt32(result.CopieProdotte);
                    new_s7data.MetriLineariLavorati = Convert.ToInt32(result.MetriLineariLavorati);
                    new_s7data.MinutiMacchinaAccesa = Convert.ToInt32(result.MinutiMacchinaAccesa);
                    new_s7data.MinutiMacchinaInPassaggio = Convert.ToInt32(result.MinutiMacchinaInPassaggio);
                    new_s7data.MinutiMacchinaInPressa = Convert.ToInt32(result.MinutiMacchinaInPressa);

                    db.S7Datas.InsertOnSubmit(new_s7data);
                }
                if (s7data != null && s7data.Stato == 0)
                {
                    s7data.DataOraFine = DateTime.Now;
                    s7data.Stato = 1;
                    s7data.CopieProdotte = Convert.ToInt32(result.CopieProdotte);
                    s7data.MetriLineariLavorati = Convert.ToInt32(result.MetriLineariLavorati);
                    s7data.MinutiMacchinaAccesa = Convert.ToInt32(result.MinutiMacchinaAccesa);
                    s7data.MinutiMacchinaInPassaggio = Convert.ToInt32(result.MinutiMacchinaInPassaggio);
                    s7data.MinutiMacchinaInPressa = Convert.ToInt32(result.MinutiMacchinaInPressa);

                }

                // Azzero tutti i campi sulla macchina

                S7.SetUDIntAt(Buffer, 0, 0);
                S7.SetUDIntAt(Buffer, 4, 0);
                S7.SetStringAt(Buffer, 8, 30, "");

                S7.SetUDIntAt(Buffer, 40, 0);
                S7.SetUDIntAt(Buffer, 44, 0);

                S7.SetUDIntAt(Buffer, 48, 0);
                S7.SetUDIntAt(Buffer, 52, 0);
                S7.SetUDIntAt(Buffer, 56, 0);

                S7.SetUDIntAt(Buffer, 106, 0);
                S7.SetUDIntAt(Buffer, 114, 0);
                S7.SetUDIntAt(Buffer, 122, 0);

                S7.SetUDIntAt(Buffer, 126, 0);
                S7.SetUDIntAt(Buffer, 130, 0);
                S7.SetUDIntAt(Buffer, 134, 0);

                PlcDBWrite();

                Disconnect();

            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.Message);
            }
            //#endif

        }


        public void UpdateLastDataFromSilkFoil1(QuotationDataContext db)
        {
            try
            {
                // Aggiornamento dei dati dell'ultimo Odp per sopperire al ritardo di trascrizione dei dati dalla macchina al database del Siemens S7
                //#if !DEBUG
                PlcDBRead();
                S7_DATI result = new S7_DATI();
                result.OdP = S7.GetDWordAt(Buffer, 0);
                result.CopieDaProdurre = S7.GetDWordAt(Buffer, 4);
                result.TitoloOdp = Convert.ToString(S7.GetStringAt(Buffer, 8));

                // campi azzerati ad inizio lavorazione OdP
                result.CopieProdotte = S7.GetDWordAt(Buffer, 40);
                result.MetriLineariLavorati = S7.GetDWordAt(Buffer, 44);

                result.OreMinutiOreMacchinaAccesa = S7.GetDWordAt(Buffer, 48);
                result.OreMinutiOreMacchinaInPassaggio = S7.GetDWordAt(Buffer, 52);
                result.OreMinutiOreMacchinaInPressa = S7.GetDWordAt(Buffer, 56);

                result.OreMinutiMinutiMacchinaAccesa = S7.GetDWordAt(Buffer, 106);
                result.OreMinutiMinutiMacchinaInPassaggio = S7.GetDWordAt(Buffer, 114);
                result.OreMinutiMinutiMacchinaInPressa = S7.GetDWordAt(Buffer, 122);

                result.MinutiMacchinaAccesa = S7.GetDWordAt(Buffer, 126);
                result.MinutiMacchinaInPassaggio = S7.GetDWordAt(Buffer, 130);
                result.MinutiMacchinaInPressa = S7.GetDWordAt(Buffer, 134);

                Disconnect();

                //string number = db.ProductionOrders.FirstOrDefault(d => d.ID == labextimOdp).Number;
                //int idCompany = db.ProductionOrders.FirstOrDefault(d => d.ID == labextimOdp).ID_Company.GetValueOrDefault(1);
                //int machineOdp = Convert.ToInt32(idCompany.ToString() + number.Substring(2).Replace("/", ""));

                S7Data s7data = db.S7Datas.OrderByDescending(d => d.ID).FirstOrDefault(d => d.OdpMacchina == result.OdP);
                ProductionOrder po = db.ProductionOrders.FirstOrDefault(d => d.ID == result.OdP);
                //S7Data new_s7data = null;
                //if (po != null && s7data == null)
                //{
                //    new_s7data = new S7Data();
                //    new_s7data.ID_Odp = Convert.ToInt32(result.OdP);
                //    new_s7data.OdpMacchina = Convert.ToInt32(result.OdP);
                //    new_s7data.Titolo = (po.Description.Length > 30 ? po.Description.Substring(0, 30) : po.Description);
                //    new_s7data.CopieDaProdurre = Convert.ToInt32(po.Quantity);
                //    new_s7data.DataOraInizio = DateTime.Now.AddMinutes(-Convert.ToInt32(result.MinutiMacchinaAccesa));

                //    new_s7data.DataOraFine = DateTime.Now;
                //    new_s7data.Stato = 1;
                //    new_s7data.CopieProdotte = Convert.ToInt32(result.CopieProdotte);
                //    new_s7data.MetriLineariLavorati = Convert.ToInt32(result.MetriLineariLavorati);
                //    new_s7data.MinutiMacchinaAccesa = Convert.ToInt32(result.MinutiMacchinaAccesa);
                //    new_s7data.MinutiMacchinaInPassaggio = Convert.ToInt32(result.MinutiMacchinaInPassaggio);
                //    new_s7data.MinutiMacchinaInPressa = Convert.ToInt32(result.MinutiMacchinaInPressa);

                //    db.S7Datas.InsertOnSubmit(new_s7data);
                //}
                if (s7data != null)
                {
                    s7data.CopieProdotte = Convert.ToInt32(result.CopieProdotte);
                    s7data.MetriLineariLavorati = Convert.ToInt32(result.MetriLineariLavorati);
                    s7data.MinutiMacchinaAccesa = Convert.ToInt32(result.MinutiMacchinaAccesa);
                    s7data.MinutiMacchinaInPassaggio = Convert.ToInt32(result.MinutiMacchinaInPassaggio);
                    s7data.MinutiMacchinaInPressa = Convert.ToInt32(result.MinutiMacchinaInPressa);

                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.Message);
            }
            //#endif

        }

        public void SetPauseSignal(int labextimOdp, bool pause)
        {
            try
            {
                PlcDBRead();
                if (S7.GetDWordAt(Buffer, 0) == labextimOdp)
                {
                    S7.SetDWordAt(Buffer, 138, (pause == true ? (uint)1 : (uint)0));
                    PlcDBWrite();
                }

                Disconnect();
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.Message);
            }

        }

        public uint? GetPauseSignal()
        {
            uint? paused = null;
            try
            {
                PlcDBRead();
                paused = S7.GetDWordAt(Buffer, 138);
                Disconnect();
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.Message);
            }
            return paused;

        }


        public static OdPBag GetCurOdP()
        {

            Snap7Gateway current = new Snap7Gateway();

            OdPBag result = new OdPBag { Id= -1, CopieRichieste = 0, CopieLavorate = 0 };
            try
            {
                current.PlcDBRead();
                int id= -1;
                Int32.TryParse(S7.GetDWordAt(current.Buffer, 0).ToString(), out id);
                if (id > 0)
                {
                    result.Id = id;
                    result.CopieRichieste = Convert.ToInt32(S7.GetDWordAt(current.Buffer, 4));
                    result.CopieLavorate = Convert.ToInt32(S7.GetDWordAt(current.Buffer, 40));
                }
                current.Disconnect();
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.Message);
            }
            return result;

        }

        public static OdPBag GetOdPHistoricalData(int poId, QuotationDataContext db)
        {

            OdPBag result = new OdPBag { Id = -1, CopieRichieste = 0, CopieLavorate = 0 };
            try
            {
                List<S7Data> found = db.S7Datas.Where(d => d.ID_Odp == poId).ToList();
                if (found != null)
                {
                    result.CopieRichieste = found[0].CopieDaProdurre.GetValueOrDefault();
                    result.CopieLavorate = found.Sum(d => d.CopieProdotte).GetValueOrDefault();
                }
            }
            catch (Exception ex)
            {
                Log.WriteMessage(ex.Message);
            }
            return result;

        }




    }

}
