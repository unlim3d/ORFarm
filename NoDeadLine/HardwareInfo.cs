using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace NoDeadLine
{
    class HardwareInfo
    {
        private string fileName = "";
        public void StartCmd()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.WorkingDirectory= @"C:\Windows\System32";
            startInfo.FileName = @"C:\Windows\System32\cmd.exe"; 
            fileName= DateTime.Now.ToString("yyyyMMddHHmmss");
            startInfo.Arguments = @"/C " + "cd "+Directory.GetDirectories(FarmSettings.Root, "Resources")[0]+ "\\OpenHardwareMonitorReport"+" & "+"OpenHardwareMonitorReport.exe "+"> "+ fileName + ".txt";
            process.StartInfo = startInfo;
            
            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;

            process.Start();
        }


        private void Process_Exited(object sender, EventArgs e)
        {
            string pathStart = Directory.GetDirectories(FarmSettings.Root, "Resources")[0] +
                               "\\OpenHardwareMonitorReport" + "\\" + fileName + ".txt";
            string pathEnd = Program.DeadLineReportFolderWin + "\\" + fileName + ".txt";
            try
            {
                if (File.Exists(pathStart))
                {
                    File.Move(pathStart, pathEnd);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("The process failed: {0}", exception.ToString());
            }
        }
    }
}
