using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using CMLabExtim;

namespace DLLabExtim
{
    public class BIWMISFtpWebRequest
    {

        public static FtpWebResponse GetResponse(FtpWebRequest request)
        {
            try
            {
                return (FtpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                Log.WriteMessage(String.Format("BIWMIntegrationService - ERRORE FTP (VERIFICARE CHE I SERVER FTP RISPONDANO E CHE NON SI RIPETA AD OLTRANZA!) (Dati richiesta: Uri: {0}, Operazione: {1}, Errore: {2})", request.RequestUri , request.Method, ex.Message.ToString()));
                throw;
            }
        }

        public static Stream GetRequestStream(FtpWebRequest request)
        {
            try
            {
                return request.GetRequestStream();
            }
            catch (Exception ex)
            {
                Log.WriteMessage(String.Format("BIWMIntegrationService - ERRORE FTP (VERIFICARE CHE I SERVER FTP RISPONDANO E CHE NON SI RIPETA AD OLTRANZA!) (Dati richiesta: Uri: {0}, Operazione: {1}, Errore: {2})", request.RequestUri, request.Method, ex.Message.ToString()));
                throw;
            }
        }

    }

}
