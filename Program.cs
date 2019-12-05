using System;
using System.Windows.Forms;
using TLO.Forms;

namespace TLO
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
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