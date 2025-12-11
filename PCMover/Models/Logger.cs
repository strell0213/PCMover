using System;
using System.IO;
using System.Text;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace PCMover.SimpleLogs
{ 
    public enum logLevel { traceLog, debugLog, infoLog, warningLog, errorLog, criticalLog }

    public class Logger
    {
        public string Titul {  get; set; }
        public string Text { get; set; }
        public logLevel Level { get; set; }

        private static logLevel minLog_Lvl = logLevel.debugLog;

        public Logger(string Titul,  string Text, logLevel level)
        {
            this.Titul = Titul;
            this.Text = Text;
            this.Level = level;
        }

        public void Write()
        {
            if (Level < minLog_Lvl) return;
            try
            {
                Text = ValidateMessage(Text);

                string pathToLogFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");

                Directory.CreateDirectory(pathToLogFolder);

                string filename = System.IO.Path.Combine(pathToLogFolder, $"PCMover_{DateTime.Now:yyyy-MM-dd}.log");

                string fullText = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{Titul}] [{Level}]: {Text}\r\n";

                FileStream fs = new FileStream(filename, FileMode.OpenOrCreate);
                fs.Close();

                using (var w = System.IO.File.AppendText(filename))
                {
                    w.WriteLine(fullText);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Logger is failed: " + ex);
            }
        }

        public string ValidateMessage(string message)
        {
            message = message.Trim();
            message = message.Replace("\n", "");
            message = message.Replace("\r", "");
            return message;
        }
    }

}
