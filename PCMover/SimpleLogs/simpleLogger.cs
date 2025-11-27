using System;
using System.IO;
using System.Text;

namespace PCMover.SimpleLogs
{ 
    public enum logLevel { traceLog, debugLog, infoLog, warningLog, errorLog, criticalLog }

    public static class Logger
    {
        private static readonly object sync = new object();
        public static logLevel minLog_Lvl = logLevel.debugLog;
        public static Encoding Encoding = Encoding.UTF8;

        public static void Write(string message, logLevel level = logLevel.infoLog)
        {
            if (level < minLog_Lvl) return;
            try
            {
                string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                if (!Directory.Exists(pathToLog)) Directory.CreateDirectory(pathToLog);

                string filename = Path.Combine(pathToLog, $"{AppDomain.CurrentDomain.FriendlyName}_{DateTime.Now:yyyy-MM-dd}.log");
                string fullText = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{level}] {message}\r\n";

                lock (sync)
                {
                    File.AppendAllText(filename, fullText, Encoding);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Logger is failed: " + ex);
            }
        }

        public static void Write(Exception ex, logLevel level = logLevel.errorLog)
        {
            string message = $"{ex.TargetSite?.DeclaringType}.{ex.TargetSite?.Name}() - {ex.Message}\n{ex.StackTrace}";
            Write(message, level);
        }
    }

}
