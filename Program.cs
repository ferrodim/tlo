// Decompiled with JetBrains decompiler
// Type: TLO.local.Program
// Assembly: TLO.local, Version=2.6.5944.27906, Culture=neutral, PublicKeyToken=null
// MVID: E76CFDB0-1920-4151-9DD8-5FF51DE7CC23
// Assembly location: C:\Users\root\Downloads\TLO_2.6.2.21\TLO.local.exe

using System;
using System.Windows.Forms;

namespace TLO.local
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      try
      {
        Settings current = Settings.Current;
        ClientLocalDB.Current.GetCategoriesEnable();
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(true);
        Application.Run((Form) new MainForm());
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
      }
    }
  }
}
