// Decompiled with JetBrains decompiler
// Type: TLO.local.TopicInfo
// Assembly: TLO.local, Version=2.6.5944.27906, Culture=neutral, PublicKeyToken=null
// MVID: E76CFDB0-1920-4151-9DD8-5FF51DE7CC23
// Assembly location: C:\Users\root\Downloads\TLO_2.6.2.21\TLO.local.exe

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;

namespace TLO.local
{
  internal class TopicInfo
  {
    protected static CultureInfo _cultureUsInfo = new CultureInfo("en-US");

    public int TopicID { get; set; }

    public int Seeders { get; set; }

    public int Leechers { get; set; }

    public string Hash { get; set; }

    public int CategoryID { get; set; }

    public string Name
    {
      get
      {
        return (this.Name2);
      }
    }

    public string Name2 { get; set; }

    public string TorrentName { get; set; }

    public List<string> Files { get; set; }

    public int Status { get; set; }

    public long Size { get; set; }

    public DateTime RegTime { get; set; }

    public Decimal? AvgSeeders { get; set; }

    public bool IsKeeper { get; set; }

    public bool IsKeep { get; set; }

    public bool IsDownload { get; set; }

    public bool IsBlackList { get; set; }

    public bool IsSelected { get; set; }

    public string Alternative
    {
      get
      {
        return ">>>>";
      }
    }

    public bool? IsRun { get; set; }

    public bool IsPause { get; set; }

    public bool[] TorrentClientStatus { get; set; }

    public Decimal PercentComplite { get; set; }

    public bool Checked { get; set; }

    public static string sizeToString(long size)
    {
      if ((Decimal) size >= new Decimal(int.MinValue, 2, 0, false, (byte) 1))
        return Math.Round((Decimal) size / new Decimal(int.MinValue, 2, 0, false, (byte) 1), 2).ToString() + " GB";
      if ((Decimal) size >= new Decimal(10485760, 0, 0, false, (byte) 1))
        return Math.Round((Decimal) size / new Decimal(10485760, 0, 0, false, (byte) 1), 2).ToString() + " MB";
      if ((Decimal) size >= new Decimal(10240, 0, 0, false, (byte) 1))
        return Math.Round((Decimal) size / new Decimal(10240, 0, 0, false, (byte) 1), 2).ToString() + " KB";
      return Math.Round((Decimal) size, 2).ToString() + " B";
    }

    public string SizeToString
    {
      get
      {
        return TopicInfo.sizeToString(this.Size);
      }
    }

    public string StatusToString
    {
      get
      {
        switch (this.Status)
        {
          case 0:
            return "*";
          case 1:
            return "x";
          case 2:
            return "√";
          case 3:
            return "?";
          case 4:
            return "!";
          case 5:
            return "D";
          case 6:
            return "©";
          case 7:
            return "∑";
          case 8:
            return "#";
          case 9:
            return "%";
          case 10:
            return "T";
          case 11:
            return "∏";
          default:
            return "-";
        }
      }
    }

    public string RegTimeToString
    {
      get
      {
        return this.RegTime.ToString("dd.MM.yyyy");
      }
    }

    public int PosterID { get; set; }

    public bool IsPoster { get; set; }
    
    public int? KeeperCount { get; set; }
  }
}
