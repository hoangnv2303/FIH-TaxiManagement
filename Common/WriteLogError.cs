using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class WriteLogError
    {
        public static void Write(string message, string ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString("g") + ": " + message + "-" + ex);
                sw.Flush();
                sw.Close();
            }
            catch
            {
                // ignored
            }
        }
    }
}
