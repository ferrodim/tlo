using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace TLO
{
    public class Settings
    {
        private static readonly Logger Logger = LogManager.GetLogger("Settings");
        private static Settings _data;
        private DateTime _lastWriteTime;

        public Settings()
        {
            KeeperName = string.Empty;
            KeeperPass = string.Empty;
            CountDaysKeepHistory = 7;
            PeriodRunAndStopTorrents = 60;
            CountSeedersReport = 10;
            IsSelectLessOrEqual = true;
            IsNotSaveStatistics = true;
            ReportLine = "[*] %%Status%% [url=viewtopic.php?t=%%ID%%]%%Name%%[/url] %%Size%%";
            ReportTop1 =
                "[b]Актуально на:[/b] %%CreateDate%%\r\n\r\nОбщее количество хранимых раздач подраздела: %%CountTopics%% шт. (%%SizeTopics%%)";
            ReportTop2 =
                "%%Top1%%[spoiler=\"Раздачи, взятые на хранение, №№ %%NumberTopicsFirst%% - %%NumberTopicsLast%%\"]\r\n[list=1]\r\n%%ReportLines%%\r\n[/list]\r\n[/spoiler]";
            ReportBottom = "";
            ReportSummaryTemplate = @"
Актуально на: {{{today}}}

Общее количество хранимых раздач: {{{summary_topics_count}}} шт.
Общий вес хранимых раздач: {{{summary_topics_size}}} GB
[hr]

{{#categories}}
[url={{{url}}}]{{{category_name}}}[/url] - {{{topics_count}}} шт. ({{{topics_size}}} GB)
{{/categories}}
".Trim();
            ReportCategoryHeaderTemplate = @"
[url={{{category_uri}}}][color=darkgreen][b]{{{category_name}}}[/b][/color][/url] | [url={{{category_check_seeds_uri}}}][color=darkgreen][b]Проверка сидов[/b][/color][/url]

[b]Актуально на:[/b] {{{today}}}

[b]Общее количество раздач в подразделе:[/b] {{{topics_count}}} шт.
[b]Общий размер раздач в подразделе:[/b] {{{topics_size}}} GB.
[b]Количество хранителей:[/b] {{{keepers_count}}}
[b]Общее количество хранимых раздач:[/b] {{{keep_topics_count}}} шт.
[b]Общий вес хранимых раздач:[/b] {{{keep_topics_size}}} GB.
[hr]

{{#keepers}}
[b]Хранитель {{{keeper_number}}}:[/b] [url={{{keeper_profile_uri}}}][color=darkgreen][b]{{{keeper_username}}}[/b][/color][/url] - {{{keep_topics_count}}} шт. ({{{keep_topics_size}}} GB)
{{/keepers}}
".Trim();
            ReportCategoriesTemplate = @"
[hr]
[hr]
[b][color=darkgreen][align=center][size=16]Статистика раздела: {{{today}}}[/size][/align][/color][/b][hr]
[hr]

Всего: {{{topics_count}}} шт. ({{{topics_size}}} Гб.)

[hr]
[size=12][b]По хранителям:[/b][/size]
{{#keepers}}
[spoiler=""{{{keeper_number}}}. {{{keeper_username}}} - {{{keep_topics_count}}} шт. ({{{keep_topics_size}}} Гб.)""]
{{#categories}}
{{{keep_category_name}}} - {{{keep_category_topics_count}}} шт. ({{{keep_category_topics_size}}} Гб.)
{{/categories}}
[/spoiler]
{{/keepers}}
[hr]
[size=12][b]По форумам:[/b][/size]
{{#categories}}
[spoiler=""{{{category_name}}} - {{{topics_count}}} шт. ({{{topics_size}}} Гб.)""]
{{#keepers}}
{{{keeper_username}}} - {{{keep_topics_count}}} шт. ({{{keep_topics_size}}} Гб.)
{{/keepers}}
[/spoiler]
{{/categories}}
".Trim();
            HostRuTrackerOrg = "rutracker.org";
            ProxyList = new List<string>();
        }

        private static string FileSettings => Path.Combine(Folder, "TLO.Settings.xml");
        private static string OldFileSettings => Path.Combine(Folder, "TLO.local.Settings.xml");

        public static string Folder => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        public static Settings Current
        {
            get
            {
                if (_data == null)
                    _data = new Settings();
                _data.Checking();
                return _data;
            }
        }

        [XmlElement] public int? LogLevel { get; set; }

        [XmlAttribute] public string KeeperName { get; set; }

        [XmlAttribute] public string KeeperPass { get; set; }

        [XmlAttribute] public bool IsUpdateStatistics { get; set; }

        [XmlAttribute] public int CountDaysKeepHistory { get; set; }

        [XmlAttribute] public int PeriodRunAndStopTorrents { get; set; }

        [XmlAttribute] public int CountSeedersReport { get; set; }

        [XmlAttribute] public bool IsAvgCountSeeders { get; set; }

        [XmlAttribute] public bool IsSelectLessOrEqual { get; set; }

        [XmlAttribute] public bool IsNotSaveStatistics { get; set; }

        [XmlAttribute] public DateTime LastUpdateTopics { get; set; }

        [XmlElement] public string ReportTop1 { get; set; }

        [XmlElement] public string ReportTop2 { get; set; }

        [XmlElement] public string ReportLine { get; set; }

        [XmlElement] public string ReportBottom { get; set; }

        [XmlElement] public string ReportSummaryTemplate { get; set; }

        [XmlElement] public string ReportCategoryHeaderTemplate { get; set; }

        [XmlElement] public string ReportCategoriesTemplate { get; set; }

        [XmlElement] public string HostRuTrackerOrg { get; set; }

        [XmlElement] public bool? LoadDBInMemory { get; set; }

        [XmlElement] public bool? UseProxy { get; set; }
        
        [XmlElement] public bool? SystemProxy { get; set; }
        
        [XmlElement] public string SelectedProxy { get; set; }
        
        [XmlArray] public List<string> ProxyList { get; set; }

        [XmlElement] public bool? DisableServerCertVerify { get; set; }

        [XmlElement] public string ApiHost { get; set; }

        public void Save()
        {
            lock (this)
            {
                try
                {
                    if (!Directory.Exists(Path.GetDirectoryName(FileSettings)))
                        Directory.CreateDirectory(Path.GetDirectoryName(FileSettings));
                    using (Stream stream = File.Open(FileSettings, FileMode.Create, FileAccess.ReadWrite))
                    {
                        LogLevel = LogLevel.HasValue ? LogLevel.Value : 0;
                        new XmlSerializer(typeof(Settings)).Serialize(stream, this);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
                }

                _lastWriteTime = File.GetLastWriteTime(FileSettings);
            }
        }

        private void Read()
        {
            lock (this)
            {
                // Проверка наличия старого файла с настройками
                if (File.Exists(OldFileSettings) && !File.Exists(FileSettings))
                {
                    File.Move(OldFileSettings, FileSettings);
                }

                if (!File.Exists(FileSettings)) Save();

                using (Stream stream = File.Open(FileSettings, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var settings = (Settings) new XmlSerializer(typeof(Settings)).Deserialize(stream);
                    IsUpdateStatistics = settings.IsUpdateStatistics;
                    CountDaysKeepHistory = settings.CountDaysKeepHistory;
                    PeriodRunAndStopTorrents = settings.PeriodRunAndStopTorrents;
                    CountSeedersReport = settings.CountSeedersReport;
                    IsAvgCountSeeders = settings.IsAvgCountSeeders;
                    KeeperName = settings.KeeperName;
                    KeeperPass = settings.KeeperPass;
                    IsSelectLessOrEqual = settings.IsSelectLessOrEqual;
                    IsNotSaveStatistics = settings.IsNotSaveStatistics;
                    LastUpdateTopics = settings.LastUpdateTopics;
                    ReportTop1 = settings.ReportTop1.Replace("\n", "\r\n").Replace("\r\r", "\r");
                    ReportTop2 = settings.ReportTop2.Replace("\n", "\r\n").Replace("\r\r", "\r");
                    ReportLine = settings.ReportLine.Replace("\n", "\r\n").Replace("\r\r", "\r");
                    ReportBottom = settings.ReportBottom.Replace("\n", "\r\n").Replace("\r\r", "\r");
                    ReportSummaryTemplate =
                        settings.ReportSummaryTemplate.Replace("\n", "\r\n").Replace("\r\r", "\r");
                    ReportCategoryHeaderTemplate = settings.ReportCategoryHeaderTemplate.Replace("\n", "\r\n")
                        .Replace("\r\r", "\r");
                    ReportCategoriesTemplate = settings.ReportCategoriesTemplate.Replace("\n", "\r\n")
                        .Replace("\r\r", "\r");
                    HostRuTrackerOrg = settings.HostRuTrackerOrg;
                    SetLogger(settings.LogLevel.HasValue ? settings.LogLevel.Value : 0);
                    _lastWriteTime = File.GetLastWriteTime(FileSettings);
                    LoadDBInMemory = settings.LoadDBInMemory;
                    UseProxy = settings.UseProxy;
                    SystemProxy = settings.SystemProxy;
                    SelectedProxy = settings.SelectedProxy;
                    ProxyList = settings.ProxyList;
                    ApiHost = settings.ApiHost;
                    DisableServerCertVerify = settings.DisableServerCertVerify;
                }
            }
        }

        private void Checking()
        {
            if (!(File.GetLastWriteTime(FileSettings) != _lastWriteTime))
                return;
            Read();
        }

        private void SetLogger(int logLevel)
        {
            if (LogLevel.HasValue && LogLevel.Value == logLevel)
                return;
            var str = "BI.Analytics.Expert.Other";
            if (Assembly.GetEntryAssembly() != null)
                str = Assembly.GetEntryAssembly().ManifestModule.Name;
            var loggingConfiguration = new LoggingConfiguration();
            var fileTarget = new FileTarget();
            fileTarget.Layout = "${date:format=yyyy-MM-dd HH\\:mm\\:ss}\t${level}\t${logger}\t${message}";
            loggingConfiguration.AddTarget("logfile", fileTarget);
            fileTarget.FileName = Path.Combine(Folder, str + ".log");
            fileTarget.Encoding = Encoding.UTF8;
            fileTarget.ArchiveAboveSize = 20971520L;
            if (Environment.UserInteractive)
            {
                var coloredConsoleTarget = new ColoredConsoleTarget();
                loggingConfiguration.AddTarget("console", coloredConsoleTarget);
                coloredConsoleTarget.Layout =
                    "${date:format=yyyy-MM-dd HH\\:mm\\:ss}\t${level}\t${logger}\t${message}\t${file}:${line}";
                var loggingRule = new LoggingRule("*", NLog.LogLevel.Debug, coloredConsoleTarget);
                loggingConfiguration.LoggingRules.Add(loggingRule);
            }

            var loggingRule1 = logLevel > 0
                ? logLevel != 1
                    ? logLevel != 2
                        ? new LoggingRule("*", NLog.LogLevel.Trace, fileTarget)
                        : new LoggingRule("*", NLog.LogLevel.Debug, fileTarget)
                    : new LoggingRule("*", NLog.LogLevel.Info, fileTarget)
                : new LoggingRule("*", NLog.LogLevel.Warn, fileTarget);
            loggingConfiguration.LoggingRules.Add(loggingRule1);
            LogManager.Configuration = loggingConfiguration;
            Logger.Info(string.Format("OS: {0} (Is64BitOperatingSystem: {1}, Version {2})",
                Environment.OSVersion.VersionString, Environment.Is64BitOperatingSystem,
                Environment.OSVersion.Version));
            LogLevel = logLevel;
        }
    }
}