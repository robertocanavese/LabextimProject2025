using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMLabExtim;
using System.Configuration;

namespace MailTest
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                var securityProtocol = (int)System.Net.ServicePointManager.SecurityProtocol;

                // 0 = SystemDefault in .NET 4.7+
                if (securityProtocol != 0)
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                }

                Utilities.SendMail(ConfigurationManager.AppSettings["mailAddressToCSV_Azienda01"], null, null, string.Format("Labextim - Segnalazione ({0}) da operatore {1} a Direzione", DateTime.Now.ToString("dd/MM/yyyy"), "Roberto Canavese"),"Testo mail di prova", ConfigurationManager.AppSettings["mailAddressFrom"]);

                Log.WriteMessage("MailTest - Mail inviata con successo!");
            }
            catch (Exception ex)
            {
                Log.WriteMessage("MailTest - " + ex.Message);
            }


        }
    }
}
