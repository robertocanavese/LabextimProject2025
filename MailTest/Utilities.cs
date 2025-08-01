using System;
using System.Net;
using System.Net.Mail;
using System.Linq;
using System.Configuration;

namespace CMLabExtim
{
    public static class Utilities
    {

        public static void SendMail(string recipentsCSVList, string CcCSVList, string CcnCSVList, string subject, string body, string senderAddress)
        {

            MailMessage message = new MailMessage();
            message.Body = body;
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.From = new MailAddress(senderAddress);
            message.To.Add(recipentsCSVList);
            if (CcCSVList != null) message.CC.Add(CcCSVList);
            if (CcnCSVList != null) message.Bcc.Add(CcnCSVList);

            try
            {
                System.Net.Mail.SmtpClient smtp = new SmtpClient();
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
                if (ex.InnerException != null)
                    mess += (" - " + ex.InnerException.Message);
                if (ex.InnerException.InnerException != null)
                    mess += (" - " + ex.InnerException.InnerException.Message);
                Log.WriteMessage(mess);
                throw new Exception(mess);
            }

        }


        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }
}