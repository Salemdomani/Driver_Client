using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Driver_Client
{
    class Reporter
    {
        public static void Report(string text)
        {
            try
            {
                using (var stream = new StreamWriter("Log.txt", true))
                    stream.WriteLineAsync(DateTime.Now + " : " + text);
            }
            catch{ }
            
        }

        public static async Task ReportAsync(string text)
        {
            try
            {
                using (var stream = new StreamWriter("Log.txt", true))
                    await stream.WriteLineAsync(DateTime.Now + " : " + text);
            }
            catch { }
        }
    }
}
