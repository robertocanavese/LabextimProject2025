using System;
using System.IO;

namespace CMLabExtim
{
    public class Log
    {
        public static void Write(string header, Exception exception)
        {
            var logFile = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\exception.log", FileMode.Append);
            var writer = new StreamWriter(logFile) {AutoFlush = true};
            writer.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " - " + header +
                             (exception != null ? ": " + exception.Message : string.Empty));
            writer.Close();

        }
        public static void WriteMessage( string eventMessage)
        {
            var logFile = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\message.log", FileMode.Append);
            var writer = new StreamWriter(logFile) { AutoFlush = true };
            writer.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " - " +
                             (eventMessage != null ? ": " + eventMessage : string.Empty));
            writer.Close();

        }
    }
}