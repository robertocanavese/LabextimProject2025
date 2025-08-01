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
                Log.WriteMessage(mess);
                throw new Exception(mess);
            }

        }


        public static DateTime GetNextWorkDate(int daySkip, DateTime startDate)
        {
            var newDate = startDate;
            for (var i = 0; i < Math.Abs(daySkip); i++)
            {
                newDate = newDate.AddDays(daySkip >= 0 ? 1 : -1);
                if (newDate.DayOfWeek != DayOfWeek.Saturday && newDate.DayOfWeek != DayOfWeek.Sunday) continue;
                //newDate.AddDays(daySkip >= 0 ? 1 : -1); 
                i -= 1;
            }
            return newDate;
        }

        public static decimal DecimalHoursToTicks(decimal decimalHours)
        {
            var hours = Math.Truncate(decimalHours);
            var minutes = Math.Truncate((decimalHours - hours) / (60m / 100m));
            var seconds = ((((decimalHours - hours) / (60m / 100m)) - minutes) / (60m / 100m));
            return new TimeSpan(Convert.ToInt32(hours), Convert.ToInt32(minutes), Convert.ToInt32(seconds)).Ticks;
        }

        public static string DisplayAsTimeSpan(decimal? timeValue)
        {
            if (Convert.IsDBNull(timeValue))
                return "N/A";
            var ts = new TimeSpan(Convert.ToInt64(timeValue));
            return string.Format("{0:#0}:{1:00}", ts.Days * 24 + ts.Hours, ts.Minutes + (ts.Seconds > 29 ? 1 : 0));
        }

        public static string DisplayAsCentiHours(decimal? timeValue)
        {
            if (Convert.IsDBNull(timeValue))
                return "N/A";
            var ts = new TimeSpan(Convert.ToInt64(timeValue));
            return ((ts.Days * 24 + ts.Hours) + ((ts.Minutes + (ts.Seconds > 29 ? 1 : 0)) * 100m / 60m) / 100m).ToString("N2");
        }

        public static string DisplayAsCentiHours(Int64? timeValue)
        {
            if (Convert.IsDBNull(timeValue))
                return "N/A";
            var ts = new TimeSpan(timeValue.Value);
            return ((ts.Days * 24 + ts.Hours) + ((ts.Minutes + (ts.Seconds > 29 ? 1 : 0)) * 100m / 60m) / 100m).ToString("N2");
        }

        public static decimal ConvertToCentiHours(decimal? timeValue)
        {
            if (Convert.IsDBNull(timeValue))
                return 0m;
            var ts = new TimeSpan(Convert.ToInt64(timeValue));
            return ((ts.Days * 24 + ts.Hours) + ((ts.Minutes + (ts.Seconds > 29 ? 1 : 0)) * 100m / 60m) / 100m);
        }

        public static decimal ConvertToCentiHours(Int64? timeValue)
        {
            if (Convert.IsDBNull(timeValue))
                return 0m;
            var ts = new TimeSpan(timeValue.Value);
            return ((ts.Days * 24 + ts.Hours) + ((ts.Minutes + (ts.Seconds > 29 ? 1 : 0)) * 100m / 60m) / 100m);
        }

        public static string GetIPAddress(this System.Web.HttpRequest Request)
        {
            if (Request.Headers["CF-CONNECTING-IP"] != null)
                return Request.Headers["CF-CONNECTING-IP"].ToString();

            if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!string.IsNullOrEmpty(ipAddress))
                {
                    string[] addresses = ipAddress.Split(',');

                    if (addresses.Length != 0)
                        return addresses[0];
                }
            }

            return Request.UserHostAddress;
        }

        public static bool IsNumeric(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            char c;

            for (int i = 0; i <= str.Length - 1; i++)
            {
                c = str[i];
                if (!char.IsNumber(c) & c != '-')
                    return false;
            }

            return true;
        }

        public static string ControlCharsCleaned(this string input)
        {

            if (input != null)
            {
                string output = "";
                //output = new string(input.Where(c => !char.IsControl(c)).ToArray());
                foreach (char inChar in input)
                {
                    if (Char.IsControl(inChar))
                        output = output + " ";
                    else
                        output = output + inChar;
                }
                return output;
            }
            else
            {
                return input;
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

        public static string RemoveFileName(this string ftpFilePathAndName)
        {

            int _pos = ftpFilePathAndName.LastIndexOf('/');
            if (_pos > -1)
                return ftpFilePathAndName.Substring(0, _pos + 1);
            else
                return string.Empty;

        }
        public static string RemoveUriFileName(this string ftpFilePathAndName)
        {

            int _pos = ftpFilePathAndName.LastIndexOf('\\');
            if (_pos > -1)
                return ftpFilePathAndName.Substring(0, _pos + 1);
            else
                return string.Empty;

        }

        public static string ChangeFileNameExtension(this string fileName, string newExtension)
        {

            int _pos = fileName.LastIndexOf('.');
            if (_pos > -1)
                return string.Format("{0}.{1}", fileName.Substring(0, _pos), newExtension);
            else
                return string.Empty;

        }

        public static string SubstringWithMaxLen(this string input, int maxLen)
        {
            return (input.Length > maxLen ? input.Substring(0, maxLen) : input);

        }


    }
}