using System;
using System.Diagnostics;
using System.Windows.Forms;
using TLO.Forms;

namespace TLO
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            if (Settings.Current.DontRunCopy)
            {
                var currentProcess = Process.GetCurrentProcess();
                foreach (var process in Process.GetProcessesByName(currentProcess.ProcessName))
                {
                    if (process.Id == currentProcess.Id) continue;
                    process.CloseMainWindow();
                    process.WaitForExit(2000);
                    process.Close();
                }
            }

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var mainForm = new MainForm();
                new WindowTrayAssociation(mainForm).SyncSettings();
                Application.ApplicationExit += (sender, args) => TrayObject.TrayIcon.Dispose();
                Application.Run(mainForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
            }
        }
    }
}