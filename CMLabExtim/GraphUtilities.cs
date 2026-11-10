using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace CMLabExtim
{
    public static class GraphUtilities
    {
        // --- cache del token (condivisa da tutte le richieste dell'applicazione) ---
        private static readonly object _tokenLock = new object();
        private static string _accessToken;
        private static DateTime _tokenExpiresUtc = DateTime.MinValue;

        private static string GetGraphToken()
        {
            lock (_tokenLock)
            {
                // riusa il token se valido per almeno altri 5 minuti
                if (_accessToken != null && DateTime.UtcNow < _tokenExpiresUtc.AddMinutes(-5))
                    return _accessToken;

                string tenantId = ConfigurationManager.AppSettings["Graph.TenantId"];
                string clientId = ConfigurationManager.AppSettings["Graph.ClientId"];
                string secret = ConfigurationManager.AppSettings["Graph.ClientSecret"];

                string form =
                    "client_id=" + HttpUtility.UrlEncode(clientId) +
                    "&client_secret=" + HttpUtility.UrlEncode(secret) +
                    "&scope=" + HttpUtility.UrlEncode("https://graph.microsoft.com/.default") +
                    "&grant_type=client_credentials";

                string json = HttpPost(
                    "https://login.microsoftonline.com/" + tenantId + "/oauth2/v2.0/token",
                    "application/x-www-form-urlencoded",
                    form,
                    null);

                var result = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(json);
                _accessToken = (string)result["access_token"];
                _tokenExpiresUtc = DateTime.UtcNow.AddSeconds(Convert.ToInt32(result["expires_in"]));
                return _accessToken;
            }
        }

        // --- POST HTTP sincrona, restituisce il corpo della risposta ---
        private static string HttpPost(string url, string contentType, string body, string bearerToken)
        {
            // prima (richiede target 4.5+)
            //ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            // dopo (compila con target 4.0)
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;   // 3072 = TLS 1.2

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = contentType;
            request.Timeout = 30000;
            if (bearerToken != null)
                request.Headers[HttpRequestHeader.Authorization] = "Bearer " + bearerToken;

            byte[] data = Encoding.UTF8.GetBytes(body);
            request.ContentLength = data.Length;
            using (Stream s = request.GetRequestStream())
                s.Write(data, 0, data.Length);

            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                    return reader.ReadToEnd();       // per sendMail è vuoto (202 Accepted)
            }
            catch (WebException ex)
            {
                // Microsoft restituisce il motivo dell'errore nel corpo: lo riportiamo nel messaggio
                string detail = "";
                if (ex.Response != null)
                    using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                        detail = reader.ReadToEnd();
                throw new Exception(ex.Message + " " + detail, ex);
            }
        }

        // --- invio mail: stessa firma di prima, i chiamanti non cambiano ---
        public static void SendMail(string recipentsCSVList, string CcCSVList, string CcnCSVList, string subject, string body, string senderAddress)
        {
            try
            {
                var mail = new
                {
                    message = new
                    {
                        subject = subject,
                        body = new { contentType = "HTML", content = body },
                        toRecipients = ToRecipients(recipentsCSVList),
                        ccRecipients = ToRecipients(CcCSVList),
                        bccRecipients = ToRecipients(CcnCSVList)
                    },
                    saveToSentItems = true
                };
                string json = new JavaScriptSerializer().Serialize(mail);

                string sender = ConfigurationManager.AppSettings["Graph.Sender"];
                HttpPost(
                    "https://graph.microsoft.com/v1.0/users/" + sender + "/sendMail",
                    "application/json",
                    json,
                    GetGraphToken());
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
                if (ex.InnerException != null)
                    mess += (" - " + ex.InnerException.Message);
                Log.WriteMessage(mess);
                throw new Exception(mess);
            }
        }

        private static object[] ToRecipients(string csv)
        {
            if (string.IsNullOrWhiteSpace(csv)) return new object[0];
            return csv.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                      .Select(a => (object)new { emailAddress = new { address = a.Trim() } })
                      .ToArray();
        }

        // ... resto della classe invariato (GetNextWorkDate, IsLocalHoliday, ecc.)
    }
}