// Decompiled with JetBrains decompiler
// Type: TLO.local.MainForm
// Assembly: TLO.local, Version=2.6.5944.27906, Culture=neutral, PublicKeyToken=null
// MVID: E76CFDB0-1920-4151-9DD8-5FF51DE7CC23
// Assembly location: C:\Users\root\Downloads\TLO_2.6.2.21\TLO.local.exe

using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using TLO.local.Forms;

namespace TLO.local
{
    public class MainForm : Form
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private DateTime _LastRunTimer = DateTime.Now;
        private BindingSource _CategorySource = new BindingSource();
        private BindingSource _TopicsSource = new BindingSource();

        private Dictionary<BackgroundWorker, Tuple<DateTime, object, string>> backgroundWorkers =
            new Dictionary<BackgroundWorker, Tuple<DateTime, object, string>>();

        private string headText;
        private Timer tmr;
        private NotifyIcon notifyIcon;
        private IContainer components;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem файлToolStripMenuItem;
        private ToolStripMenuItem menuSettingsToolStripMenuItem;
        private ToolStripMenuItem ExitToolStripMenuItem;
        private ToolStripMenuItem отчетыToolStripMenuItem;
        private ComboBox _cbCategory;
        private Label label1;
        private TabControl tabControl1;
        private TabPage _tpReportDownloads;
        private CheckBox _cbBlackList;
        private Label label2;
        private ComboBox _cbCategoryFilters;
        private Label label3;
        private LinkLabel _llSelectedTopicsToTorrentClient;
        private LinkLabel _llDownloadSelectTopics;
        private LinkLabel _llSelectedTopicsToBlackList;
        private LinkLabel _llSelectedTopicsDeleteFromBlackList;
        private LinkLabel linkSetNewLabel;
        private LinkLabel linkLabel5;
        private Label label4;
        private LinkLabel _llUpdateTopicsByCategory;
        private LinkLabel _llUpdateCountSeedersByCategory;
        private LinkLabel _llUpdateDataDromTorrentClient;
        private ToolStripMenuItem задачиToolStripMenuItem;
        private ToolStripMenuItem UpdateCountSeedersToolStripMenuItem;
        private ToolStripMenuItem UpdateListTopicsToolStripMenuItem;
        private ToolStripMenuItem UpdateKeepTopicsToolStripMenuItem;
        private ToolStripMenuItem ClearDatabaseToolStripMenuItem;
        private Label _lbTotal;
        private ToolStripMenuItem SendReportsToForumToolStripMenuItem;
        private ToolStripMenuItem CreateReportsToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripProgressBar toolStripProgressBar1;
        private ToolStripMenuItem RuningStopingDistributionToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private NumericUpDown _cbCountSeeders;
        private TabPage tabReports;
        private ToolStripMenuItem DevlToolStripMenuItem;
        private ToolStripMenuItem ClearKeeperListsToolStripMenuItem;
        private TabPage tabPage1;
        private TabPage tabConsolidatedReport;
        private TextBox _txtConsolidatedReport;
        private TabPage ConsolidatedTorrentClientsReport;
        private TextBox _tbConsolidatedTorrentClientsReport;
        private ToolStripMenuItem CreateConsolidatedReportByTorrentClientsToolStripMenuItem;
        private DateTimePicker _DateRegistration;
        private Label label5;
        private TabControl _tcCetegoriesRootReports;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private ToolStripMenuItem LoadListKeepersToolStripMenuItem;
        private ToolStripMenuItem _btSaveToFile;
        private ToolStripMenuItem _btLoadSettingsFromFile;
        private ToolStripSeparator toolStripSeparator4;
        private DataGridViewTextBoxColumn ColumnReport1DgvTopicID;
        private DataGridViewCheckBoxColumn ColumnReport1DgvSelect;
        private DataGridViewTextBoxColumn ColumnReport1DgvStatus;
        private DataGridViewTextBoxColumn ColumnReport1DgvSize;
        private DataGridViewLinkColumn ColumnReport1DgvName;
        private DataGridViewLinkColumn ColumnReport1DgvAlternative;
        private DataGridViewTextBoxColumn ColumnReport1DgvSeeders;
        private DataGridViewTextBoxColumn ColumnReport1DgvAvgSeeders;
        private DataGridViewTextBoxColumn ColumnReport1DgvRegTime;
        private DataGridViewTextBoxColumn ColumnReport1DgvKeeperCount;
        private DataGridViewCheckBoxColumn ColumnReport1DgvBlack;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem menuTimerSetting;
        private ToolStripMenuItem UpdateAll;
        private ToolStripSeparator toolStripSeparator5;
        private DataGridView _dataGridTopicsList;

        private bool IsClose { get; set; }

        public MainForm()
        {
            this.InitializeComponent();
            this.menuTimerSetting.CheckStateChanged += (sender, args) =>
            {
                if (this.menuTimerSetting.Checked)
                {
                    _LastRunTimer = DateTime.Now;
                    if (!tmr.Enabled) tmr.Start();
                }
                else
                {
                    if (tmr.Enabled) tmr.Stop();
                }
            };
            this._DateRegistration.Value = DateTime.Now.AddDays(-30.0);
            this.Text = this.headText = string.Format("TLO {0}",
                (object) FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
            this._cbCountSeeders.Value = new Decimal(0);
            this._cbCategoryFilters.SelectedItem = (object)"Не скачан торрент и нет хранителя";
            this._CategorySource.Clear();
            this._CategorySource.DataSource = (object) ClientLocalDB.Current.GetCategoriesEnable();
            this._CategorySource.CurrentChanged += new EventHandler(this.SelectionChanged);
            this._cbCategory.DataSource = (object) this._CategorySource;
            if (this._CategorySource.Count > 0)
                this._CategorySource.Position = 1;
            this._TopicsSource.CurrentChanged += new EventHandler(this.SelectionChanged);
            this._dataGridTopicsList.AutoGenerateColumns = false;
            this._dataGridTopicsList.ClearSelection();
            this._dataGridTopicsList.DataSource = (object) this._TopicsSource;
            this.Disposed += new EventHandler(this.MainForm_Disposed);
            this.Resize += new EventHandler(this.MainForm_Resize);
            this.tmr = new Timer();
            this.tmr.Tick += new EventHandler(this.tmr_Tick);
            this.tmr.Interval = 1000;
            this.tmr.Start();
            this.IsClose = false;
            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.Icon = (Icon) new ComponentResourceManager(typeof(MainForm)).GetObject("$this.Icon");
            this.notifyIcon.MouseDoubleClick += new MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            this.notifyIcon.Visible = true;
            this.WriteReports();
        }

        private void MenuClick(object sender, EventArgs e)
        {
            try
            {
                if (sender == this.menuSettingsToolStripMenuItem)
                {
                    if (new SettingsForm().ShowDialog() == DialogResult.OK)
                    {
                        this._CategorySource.Clear();
                        this._CategorySource.DataSource = (object) null;
                        this._CategorySource.DataSource = (object) ClientLocalDB.Current.GetCategoriesEnable();
                        this._CategorySource.Position = 0;
                        if (MessageBox.Show(
                                "Запустить загрузку/обновление информации о топиках (раздачах) по всем категориям?",
                                "Обновление данных", MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button1) == DialogResult.OK)
                            this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwUpdateTopicsByCategories),
                                "Полное обновление информации о топиках (раздачах) по всем категориям...",
                                (object) ClientLocalDB.Current.GetCategoriesEnable());
                    }
                }
                else if (sender == UpdateCountSeedersToolStripMenuItem)
                {
                    this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwUpdateTopicsByCategories),
                        "Полное обновление информации о топиках (раздачах) по всем категориям...",
                        (object) ClientLocalDB.Current.GetCategoriesEnable());
                    this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwUpdateHashFromAllTorrentClients),
                        "Полное обновление информации из Torrent-клиентов...", (object) null);
                    this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwUpdateCountSeedersByAllCategories),
                        "Обновление кол-ва сидов на раздачах...", sender);
                    this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwUpdateKeepersByAllCategories),
                        "Обновление данных о хранителях...", sender);
                }
                else if (sender == this.UpdateCountSeedersToolStripMenuItem)
                    this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwUpdateCountSeedersByAllCategories),
                        "Обновление кол-ва сидов на раздачах...", sender);
                else if (sender == this.UpdateListTopicsToolStripMenuItem)
                    this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwUpdateTopicsByCategories),
                        "Полное обновление информации о топиках (раздачах) по всем категориям...",
                        (object) ClientLocalDB.Current.GetCategoriesEnable());
                else if (sender == this.UpdateKeepTopicsToolStripMenuItem)
                    this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwUpdateHashFromAllTorrentClients),
                        "Полное обновление информации из Torrent-клиентов...", (object) null);
                else if (sender == this.ClearDatabaseToolStripMenuItem)
                {
                    if (MessageBox.Show(
                            "Вы пытаетесь очистить базу данны от текущих данных (статистику и информацию о топиках).\r\n Продолжить?",
                            "Запрос подтверждения", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk,
                            MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                        return;
                    ClientLocalDB.Current.ClearDatabase();
                }
                else if (sender == this.ClearKeeperListsToolStripMenuItem)
                {
                    if (MessageBox.Show("Вы пытаетесь очистить базу данны от данных других хранителей.\r\n Продолжить?",
                            "Запрос подтверждения", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk,
                            MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                        return;
                    ClientLocalDB.Current.ClearKeepers();
                    this.SelectionChanged((object) this._CategorySource, (EventArgs) null);
                }
                else if (sender == this.SendReportsToForumToolStripMenuItem)
                {
                    if (MessageBox.Show(
                            "Отправка отчетов на форум может продолжаться продолжительное время.\r\n Продолжить?",
                            "Запрос подтверждения", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk,
                            MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                        return;
                    this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwSendReports), "Отправка отчетов на форум...",
                        (object) this);
                }
                else if (sender == this.CreateReportsToolStripMenuItem)
                {
                    if (MessageBox.Show(
                            "Сборка отчетов может продолжаться продолжительное время и потребуется обновить список раздач и информацию из торрент-клиентов.\r\n Продолжит?",
                            "Запрос подтверждения", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk,
                            MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                        return;
                    this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwUpdateCountSeedersByAllCategories),
                        "Обновление кол-ва сидов на раздачах...", sender);
                    this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwUpdateHashFromAllTorrentClients),
                        "Обновление информации из Torrent-клиентов...", sender);
                }
                else if (sender == this.RuningStopingDistributionToolStripMenuItem)
                {
                    this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwUpdateCountSeedersByAllCategories),
                        "Обновление кол-ва сидов на раздачах...", sender);
                    this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwRuningAndStopingDistributions),
                        "Обновление информации из Torrent-клиентов...", sender);
                    this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwCreateReportsTorrentClients),
                        "Построение сводного отчета по торрент-клиентам...", sender);
                }
                else if (sender == this.CreateConsolidatedReportByTorrentClientsToolStripMenuItem)
                    this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwCreateReportsTorrentClients),
                        "Построение сводного отчета по торрент-клиентам...", sender);
                else if (sender == this.LoadListKeepersToolStripMenuItem)
                    this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwUpdateKeepersByAllCategories),
                        "Обновление данных о хранителях...", sender);
                else if (sender == this.ExitToolStripMenuItem)
                {
                    this.IsClose = true;
                    this.Close();
                }
                else if (sender == this._btSaveToFile)
                    this.SaveSetingsToFile();
                else if (sender == this._btLoadSettingsFromFile)
                    this.ReadSettingsFromFile();
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                int num = (int) MessageBox.Show(ex.Message);
            }

            Cursor.Current = Cursors.Default;
        }

        private void tmr_Tick(object sender, EventArgs e)
        {
            if (this.backgroundWorkers.Count > 0)
            {
                this.Text = string.Format("{0} ({1})", (object) this.headText, (object) "Выполняются задачи...");
                this.notifyIcon.Text =
                    string.Format("{0} ({1})", (object) this.headText, (object) "Выполняются задачи...");
            }
            else
            {
                DateTime lastRunTimer = this._LastRunTimer;
                DateTime now = DateTime.Now;
                DateTime dateTime1 = now.AddMinutes((double) -Settings.Current.PeriodRunAndStopTorrents);
                TimeSpan timeSpan = lastRunTimer - dateTime1;
                if (timeSpan.TotalSeconds > 0.0)
                {
                    this.Text = string.Format("{0} ({1:hh\\:mm\\:ss})", (object) this.headText, (object) timeSpan);
                    this.notifyIcon.Text =
                        string.Format("{0} ({1:hh\\:mm\\:ss})", (object) this.headText, (object) timeSpan);
                }
                else
                {
                    try
                    {
                        DateTime lastUpdateTopics = Settings.Current.LastUpdateTopics;
                        now = DateTime.Now;
                        DateTime dateTime2 = now.AddDays(-1.0);
                        if (lastUpdateTopics < dateTime2)
                        {
                            this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwUpdateTopicsByCategories),
                                "Полное обновление информации о топиках (раздачах) по всем категориям...",
                                (object) ClientLocalDB.Current.GetCategoriesEnable());
                            this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwUpdateKeepersByAllCategories),
                                "Обновление данных о хранителях...", sender);
                            Settings current = Settings.Current;
                            now = DateTime.Now;
                            DateTime date = now.Date;
                            current.LastUpdateTopics = date;
                            Settings.Current.Save();
                        }
                        else
                            this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwUpdateCountSeedersByAllCategories),
                                "Обновление информации о кол-ве сидов на раздачах...", sender);

                        this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwRuningAndStopingDistributions),
                            "Запуск/Остановка раздач в Torrent-клиентах...", sender);
                        this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwCreateReportsTorrentClients),
                            "Построение сводного отчета по торрент-клиентам...", sender);
                    }
                    catch (Exception ex)
                    {
                        this._logger.Error(ex.Message);
                        this._logger.Debug(ex.Message, ex);
                    }

                    this._LastRunTimer = DateTime.Now;
                }
            }
        }

        private void MainForm_Disposed(object sender, EventArgs e)
        {
            this.tmr.Stop();
            this.tmr.Dispose();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
                return;
            this.Hide();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void SelectionChanged(object sender, EventArgs e)
        {
            if (sender == this._CategorySource || sender == this._cbCountSeeders ||
                (sender == this._cbBlackList || sender == this._cbCategoryFilters) || sender == this._DateRegistration)
            {
                this._TopicsSource.Clear();
                if (this._CategorySource.Current != null)
                {
                    Category current = this._CategorySource.Current as Category;
                    int num = (int) this._cbCountSeeders.Value;
                    DateTime regTime = this._DateRegistration.Value;
                    bool? isKeep = new bool?();
                    bool? isKeepers = new bool?();
                    bool? isDownload = new bool?();
                    bool? isBlack = new bool?();
                    bool? isPoster = new bool?();
                    string selectedItem = this._cbCategoryFilters.SelectedItem as string;
                    if (!string.IsNullOrWhiteSpace(selectedItem))
                    {
                        switch (selectedItem)
                        {
                            case "Есть хранитель":
                                isKeepers = new bool?(true);
                                break;
                            case "Не скачан торрент":
                                isDownload = new bool?(false);
                                break;
                            case "Не скачан торрент и есть хранитель":
                                isDownload = new bool?(false);
                                isKeepers = new bool?(true);
                                break;
                            case "Не скачан торрент и нет хранителя":
                                isDownload = new bool?(false);
                                isKeepers = new bool?(false);
                                break;
                            case "Не скачано":
                                isDownload = new bool?(false);
                                break;
                            case "Не храню":
                                isKeep = new bool?(false);
                                break;
                            case "Скачиваю раздачу":
                                isDownload = new bool?(true);
                                isKeep = new bool?(false);
                                break;
                            case "Храню":
                                isKeep = new bool?(true);
                                break;
                            case "Храню и есть хранитель":
                                isKeep = new bool?(true);
                                isKeepers = new bool?(true);
                                break;
                            case "Я релизер":
                                isPoster = new bool?(false);
                                break;
                        }
                    }

                    isBlack = new bool?(this._cbBlackList.Checked);
                    List<TopicInfo> topicInfoList = new List<TopicInfo>();
                    List<TopicInfo> source = !Settings.Current.IsAvgCountSeeders
                        ? ClientLocalDB.Current.GetTopics(regTime, current.CategoryID,
                            num > -1 ? new int?(num) : new int?(), new int?(), isKeep, isKeepers, isDownload, isBlack,
                            isPoster)
                        : ClientLocalDB.Current.GetTopics(regTime, current.CategoryID, new int?(),
                            num > -1 ? new int?(num) : new int?(), isKeep, isKeepers, isDownload, isBlack, isPoster);
                    this._lbTotal.Text = string.Format("Кол-во: {0}; Размер: {1}", (object) source.Count<TopicInfo>(),
                        (object) TopicInfo.sizeToString(source.Sum<TopicInfo>((Func<TopicInfo, long>) (x => x.Size))));
                    this._TopicsSource.DataSource = (object) source;
                }
            }

            if (sender != this._CategorySource || this._CategorySource.Current == null)
                return;
            this.tabReports.Controls.Clear();
            Dictionary<Tuple<int, int>, Tuple<string, string>> reports =
                ClientLocalDB.Current.GetReports(new int?((this._CategorySource.Current as Category).CategoryID));
            if (reports.Count<KeyValuePair<Tuple<int, int>, Tuple<string, string>>>() > 0)
            {
                Size size = this.tabReports.Size;
                TabControl tabControl = new TabControl();
                tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                tabControl.Location = new Point(0, 0);
                tabControl.SelectedIndex = 0;
                tabControl.Size = new Size(size.Width, size.Height);
                this.tabReports.Controls.Add((Control) tabControl);
                foreach (KeyValuePair<Tuple<int, int>, Tuple<string, string>> keyValuePair in (
                    IEnumerable<KeyValuePair<Tuple<int, int>, Tuple<string, string>>>) reports
                    .OrderBy<KeyValuePair<Tuple<int, int>, Tuple<string, string>>, int>(
                        (Func<KeyValuePair<Tuple<int, int>, Tuple<string, string>>, int>) (x => x.Key.Item2)))
                {
                    if (!(keyValuePair.Value.Item2 == "Резерв") && !(keyValuePair.Value.Item2 == "Удалено"))
                    {
                        TabPage tabPage = new TabPage();
                        TextBox textBox = new TextBox();
                        tabPage.Location = new Point(4, 22);
                        tabPage.Padding = new Padding(3);
                        tabPage.Text = string.Format("Сидируемое: отчет № {0}", (object) keyValuePair.Key.Item2);
                        if (keyValuePair.Key.Item2 == 0)
                            tabPage.Text = string.Format("Шапка сидируемого");
                        tabPage.UseVisualStyleBackColor = true;
                        tabPage.AutoScroll = true;
                        textBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left |
                                         AnchorStyles.Right;
                        textBox.Location = new Point(0, 0);
                        textBox.Multiline = true;
                        textBox.ReadOnly = true;
                        textBox.ScrollBars = ScrollBars.Both;
                        textBox.Size = new Size(size.Width - 8, size.Height - 20);
                        textBox.Text = keyValuePair.Value.Item2;
                        tabControl.Controls.Add((Control) tabPage);
                        tabPage.Controls.Add((Control) textBox);
                    }
                }
            }
            else
            {
                Size size = this.tabReports.Size;
                TabControl tabControl = new TabControl();
                tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                tabControl.Location = new Point(0, 0);
                tabControl.SelectedIndex = 0;
                tabControl.Size = new Size(size.Width, size.Height);
                this.tabReports.Controls.Add((Control) tabControl);
                TabPage tabPage = new TabPage();
                TextBox textBox = new TextBox();
                tabControl.Controls.Add((Control) tabPage);
                tabPage.Controls.Add((Control) textBox);
                tabPage.Location = new Point(4, 22);
                tabPage.Padding = new Padding(3);
                tabPage.Text = string.Format("Для информации");
                tabPage.UseVisualStyleBackColor = true;
                textBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                textBox.Location = new Point(0, 0);
                textBox.Multiline = true;
                textBox.ReadOnly = true;
                textBox.ScrollBars = ScrollBars.Both;
                textBox.Size = new Size(size.Width - 8, size.Height - 20);
                textBox.Text =
                    "Здесь должен быть отчет о сидируемом, но его нет.\r\nВозможные причины: сервис не успел обработать задачи сервера на скачивание страниц, прочитать torrent-файлы или не смог подключиться к torrent-клиенту\r\n\r\nЕсли на вкладке \"Не скачано\" есть раздачи которые Вы храните, то попробуйте сформировать отчет принудительно из пункта меню";
            }
        }

        private void LinkClick(object sender, EventArgs e)
        {
            if (this.backgroundWorkers.Count != 0 &&
                MessageBox.Show("Выполняются другие задачи. Добавить в очередь новое?", "Внимание",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) !=
                DialogResult.Yes)
                return;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                Category current = this._CategorySource.Current as Category;
                if (current == null)
                    return;
                if (sender == this._llUpdateCountSeedersByCategory)
                    Logic.UpdateSeedersByCategory(current);
                else if (sender == this._llUpdateTopicsByCategory)
                    this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwUpdateTopicsByCategory),
                        "Обновление списков по разделу...", (object) current);
                else if (sender == this._llUpdateDataDromTorrentClient)
                    Logic.LoadHashFromClients(current.TorrentClientUID);
                else if (sender == this._llDownloadSelectTopics)
                    this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwDownloadTorrentFiles),
                        "Скачиваются выделеные торрент-файлы в каталог...",
                        (object) new Tuple<List<TopicInfo>, MainForm>(
                            (this._TopicsSource.DataSource as List<TopicInfo>)
                            .Where<TopicInfo>((Func<TopicInfo, bool>) (x => x.Checked)).ToList<TopicInfo>(), this));
                else if (sender == this._llSelectedTopicsToTorrentClient)
                {
                    this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwSendTorrentFileToTorrentClient),
                        "Скачиваются и добавляются в торрент-клиент выделенные раздачи...",
                        (object) new Tuple<MainForm, List<TopicInfo>, Category>(this,
                            (this._TopicsSource.DataSource as List<TopicInfo>)
                            .Where<TopicInfo>((Func<TopicInfo, bool>) (x => x.Checked)).ToList<TopicInfo>(), current));
                    this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwUpdateHashFromTorrentClientsByCategoryUID),
                        "Обновляем список раздач из торрент-клиента...", (object) current);
                }
                else if (sender == this._llSelectedTopicsToBlackList)
                {
                    List<TopicInfo> list = (this._TopicsSource.DataSource as List<TopicInfo>)
                        .Where<TopicInfo>((Func<TopicInfo, bool>) (x => x.Checked)).ToList<TopicInfo>();
                    list.ForEach((Action<TopicInfo>) (x => x.IsBlackList = true));
                    ClientLocalDB.Current.SaveTopicInfo(list, false);
                }
                else if (sender == this._llSelectedTopicsDeleteFromBlackList)
                {
                    List<TopicInfo> list = (this._TopicsSource.DataSource as List<TopicInfo>)
                        .Where<TopicInfo>((Func<TopicInfo, bool>) (x => x.Checked)).ToList<TopicInfo>();
                    list.ForEach((Action<TopicInfo>) (x => x.IsBlackList = false));
                    ClientLocalDB.Current.SaveTopicInfo(list, false);
                }
                else if (sender == this.linkSetNewLabel)
                {
                    if (current == null)
                        return;
                    GetLableName getLableName = new GetLableName();
                    getLableName.Value = string.IsNullOrWhiteSpace(current.Label) ? current.FullName : current.Label;
                    if (getLableName.ShowDialog() == DialogResult.OK)
                        this.dwCreateAndRun(new DoWorkEventHandler(Logic.bwSetLabels),
                            "Установка пользовательских меток...",
                            (object) new Tuple<MainForm, List<TopicInfo>, string>(this,
                                (this._TopicsSource.DataSource as List<TopicInfo>)
                                .Where<TopicInfo>((Func<TopicInfo, bool>) (x => x.Checked)).ToList<TopicInfo>(),
                                getLableName.Value));
                }

                this.SelectionChanged((object) this._CategorySource, (EventArgs) null);
            }
            catch (Exception ex)
            {
                this._logger.Error<Exception>(ex);
                int num = (int) MessageBox.Show("Произошла ошибка:\r\n" + ex.Message, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }

            Cursor.Current = Cursors.Default;
        }

        private void ContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this._dataGridTopicsList.Columns[e.ColumnIndex].DataPropertyName == "Name")
            {
                try
                {
                    int? nullable = this._dataGridTopicsList.Rows[e.RowIndex].Cells[0].Value as int?;
                    if (!nullable.HasValue)
                        return;
                    Process.Start(string.Format("https://{1}/forum/viewtopic.php?t={0}", (object) nullable.Value,
                        Settings.Current.HostRuTrackerOrg));
                }
                catch
                {
                }
            }
            else
            {
                if (!(this._dataGridTopicsList.Columns[e.ColumnIndex].DataPropertyName == "Alternative"))
                    return;
                try
                {
                    int? topicId = this._dataGridTopicsList.Rows[e.RowIndex].Cells[0].Value as int?;
                    if (!topicId.HasValue)
                        return;
                    List<TopicInfo> dataSource = this._TopicsSource.DataSource as List<TopicInfo>;
                    if (dataSource == null)
                        return;
                    TopicInfo topicInfo = dataSource
                        .Where<TopicInfo>((Func<TopicInfo, bool>) (x => x.TopicID == topicId.Value))
                        .FirstOrDefault<TopicInfo>();
                    if (topicInfo == null)
                        return;
                    string empty1 = string.Empty;
                    string empty2 = string.Empty;
                    int num1 = topicInfo.Name.IndexOf('/');
                    string str1;
                    if (topicInfo.Name.IndexOf(']') > num1 && num1 != -1)
                        str1 = ((IEnumerable<string>) topicInfo.Name.Split('/')).FirstOrDefault<string>();
                    else if (topicInfo.Name.IndexOf(']') < num1 && num1 != -1)
                        str1 = ((IEnumerable<string>) topicInfo.Name.Split('/')).FirstOrDefault<string>().Split(']')[1];
                    else if (num1 == -1 && topicInfo.Name.IndexOf('[') < 5 && topicInfo.Name.IndexOf('[') != -1)
                        str1 = ((IEnumerable<string>) topicInfo.Name.Split(']')[1].Split('[')).FirstOrDefault<string>();
                    else if (num1 == -1 && topicInfo.Name.IndexOf('[') != -1)
                        str1 = ((IEnumerable<string>) topicInfo.Name.Split('[')).FirstOrDefault<string>();
                    else
                        str1 = ((IEnumerable<string>) topicInfo.Name.Split('[')).FirstOrDefault<string>();
                    int num2 = topicInfo.Name.IndexOf('[', num1 > -1 ? num1 : 0);
                    if (num2 < 5)
                    {
                        int startIndex = topicInfo.Name.IndexOf(']') + 1;
                        num2 = topicInfo.Name.IndexOf('[', startIndex);
                    }

                    string str2 = topicInfo.Name.Substring(num2 == -1 ? 0 : num2 + 1);
                    if (!string.IsNullOrWhiteSpace(str2))
                        str2 = ((IEnumerable<string>) str2.Split(new char[3]
                        {
                            ',',
                            ' ',
                            ']'
                        }, StringSplitOptions.RemoveEmptyEntries)).FirstOrDefault<string>();
                    if (!string.IsNullOrWhiteSpace(str2))
                        str1 = str1 + " " + str2;
                    Process.Start(string.Format("https://{2}/forum/tracker.php?f={0}&nm={1}",
                        (object) topicInfo.CategoryID, (object) str1, Settings.Current.HostRuTrackerOrg));
                }
                catch
                {
                }
            }
        }

        private void dwCreateAndRun(DoWorkEventHandler e, string comment = "...", object argument = null)
        {
            BackgroundWorker key = new BackgroundWorker();
            key.WorkerReportsProgress = true;
            key.WorkerSupportsCancellation = true;
            key.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            key.ProgressChanged += new ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            key.DoWork += e;
            this.backgroundWorkers.Add(key, new Tuple<DateTime, object, string>(DateTime.Now, argument, comment));
            if (this.backgroundWorkers.Count != 1)
                return;
            key.RunWorkerAsync(argument);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.toolStripProgressBar1.Value = 0;
            this.toolStripProgressBar1.Visible = false;
            this.toolStripStatusLabel1.Text = "";
            this.toolStripStatusLabel1.Visible = false;
            this.statusStrip1.Refresh();
            if (sender != null && sender is BackgroundWorker &&
                this.backgroundWorkers.ContainsKey(sender as BackgroundWorker))
            {
                BackgroundWorker key = sender as BackgroundWorker;
                if (this.backgroundWorkers.ContainsKey(key))
                    this.backgroundWorkers.Remove(key);
                key.Dispose();
            }

            if (e.Result != null)
                this._logger.Info(e.Result);
            if (this.backgroundWorkers.Count > 0)
            {
                // запуск следующей задачи.
                KeyValuePair<BackgroundWorker, Tuple<DateTime, object, string>> keyValuePair = this.backgroundWorkers
                    .OrderBy<KeyValuePair<BackgroundWorker, Tuple<DateTime, object, string>>, DateTime>(
                        (Func<KeyValuePair<BackgroundWorker, Tuple<DateTime, object, string>>, DateTime>) (x =>
                            x.Value.Item1)).First<KeyValuePair<BackgroundWorker, Tuple<DateTime, object, string>>>();
                keyValuePair.Key.RunWorkerAsync(keyValuePair.Value.Item2);
            }
            else
            {
                // записываем окончательные изменения в БД после выполнения последней задачи.
                this.SelectionChanged((object) this._CategorySource, (EventArgs) null);
                this.WriteReports();
                ClientLocalDB.Current.SaveToDatabase();
            }

            GC.Collect();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (sender != null && sender is BackgroundWorker &&
                this.backgroundWorkers.ContainsKey(sender as BackgroundWorker))
            {
                this.toolStripStatusLabel1.Text = this.backgroundWorkers[sender as BackgroundWorker].Item3;
                this.toolStripProgressBar1.Visible = true;
                this.toolStripStatusLabel1.Visible = true;
                this.statusStrip1.Refresh();
            }

            this.toolStripProgressBar1.Value =
                e.ProgressPercentage < 0 || e.ProgressPercentage > 100 ? 100 : e.ProgressPercentage;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!this.IsClose)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
            }
            else
                this.notifyIcon.Visible = false;
        }

        private void _dgvReportDownloads_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn column = this._dataGridTopicsList.Columns[e.ColumnIndex];
            if (column == this.ColumnReport1DgvSelect)
            {
                List<TopicInfo> dataSource = this._TopicsSource.DataSource as List<TopicInfo>;
                if (dataSource == null)
                    return;
                List<TopicInfo> list = dataSource.ToList<TopicInfo>();
                list.ForEach((Action<TopicInfo>) (x =>
                {
                    TopicInfo topicInfo = x;
                    topicInfo.Checked = !topicInfo.Checked;
                }));
                this._TopicsSource.Clear();
                this._TopicsSource.DataSource = (object) list;
            }
            else
            {
                DataGridViewColumn sortedColumn = this._dataGridTopicsList.SortedColumn;
                SortOrder sortOrder =
                    column.HeaderCell.SortGlyphDirection == SortOrder.None ||
                    column.HeaderCell.SortGlyphDirection == SortOrder.Descending
                        ? SortOrder.Ascending
                        : SortOrder.Descending;
                if (column == null)
                {
                    int num = (int) MessageBox.Show("Select a single column and try again.", "Error: Invalid Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    List<TopicInfo> dataSource = this._TopicsSource.DataSource as List<TopicInfo>;
                    if (dataSource == null)
                        return;
                    List<TopicInfo> list1 = dataSource.ToList<TopicInfo>();
                    List<TopicInfo> list2;
                    if (column == this.ColumnReport1DgvSize)
                        list2 = (sortOrder == SortOrder.Ascending
                            ? (IEnumerable<TopicInfo>) list1.OrderBy<TopicInfo, long>(
                                (Func<TopicInfo, long>) (d => d.Size))
                            : (IEnumerable<TopicInfo>) list1.OrderByDescending<TopicInfo, long>(
                                (Func<TopicInfo, long>) (d => d.Size))).ToList<TopicInfo>();
                    else if (column == this.ColumnReport1DgvName)
                        list2 = (sortOrder == SortOrder.Ascending
                            ? (IEnumerable<TopicInfo>) list1.OrderBy<TopicInfo, string>(
                                (Func<TopicInfo, string>) (d => d.Name))
                            : (IEnumerable<TopicInfo>) list1.OrderByDescending<TopicInfo, string>(
                                (Func<TopicInfo, string>) (d => d.Name))).ToList<TopicInfo>();
                    else if (column == this.ColumnReport1DgvSeeders)
                        list2 = (sortOrder == SortOrder.Ascending
                                ? (IEnumerable<TopicInfo>) list1
                                    .OrderBy<TopicInfo, int>((Func<TopicInfo, int>) (d => d.Seeders))
                                    .ThenBy<TopicInfo, string>((Func<TopicInfo, string>) (d => this.Name))
                                : (IEnumerable<TopicInfo>) list1
                                    .OrderByDescending<TopicInfo, int>((Func<TopicInfo, int>) (d => d.Seeders))
                                    .ThenBy<TopicInfo, string>((Func<TopicInfo, string>) (d => this.Name)))
                            .ToList<TopicInfo>();
                    else if (column == this.ColumnReport1DgvAvgSeeders)
                        list2 = (sortOrder == SortOrder.Ascending
                                ? (IEnumerable<TopicInfo>) list1
                                    .OrderBy<TopicInfo, Decimal?>((Func<TopicInfo, Decimal?>) (d => d.AvgSeeders))
                                    .ThenBy<TopicInfo, string>((Func<TopicInfo, string>) (d => this.Name))
                                : (IEnumerable<TopicInfo>) list1
                                    .OrderByDescending<TopicInfo, Decimal?>(
                                        (Func<TopicInfo, Decimal?>) (d => d.AvgSeeders))
                                    .ThenBy<TopicInfo, string>((Func<TopicInfo, string>) (d => this.Name)))
                            .ToList<TopicInfo>();
                    else if (column == this.ColumnReport1DgvRegTime)
                    {
                        list2 = (sortOrder == SortOrder.Ascending
                                ? (IEnumerable<TopicInfo>) list1
                                    .OrderBy<TopicInfo, DateTime>((Func<TopicInfo, DateTime>) (d => d.RegTime))
                                    .ThenBy<TopicInfo, string>((Func<TopicInfo, string>) (d => this.Name))
                                : (IEnumerable<TopicInfo>) list1
                                    .OrderByDescending<TopicInfo, DateTime>(
                                        (Func<TopicInfo, DateTime>) (d => d.RegTime))
                                    .ThenBy<TopicInfo, string>((Func<TopicInfo, string>) (d => this.Name)))
                            .ToList<TopicInfo>();
                    }
                    else if (column == this.ColumnReport1DgvKeeperCount)
                    {
                        list2 = (sortOrder == SortOrder.Ascending
                                ? (IEnumerable<TopicInfo>) list1
                                    .OrderBy<TopicInfo, int?>((Func<TopicInfo, int?>) (d => d.KeeperCount))
                                    .ThenBy<TopicInfo, string>((Func<TopicInfo, string>) (d => this.Name))
                                : (IEnumerable<TopicInfo>) list1
                                    .OrderByDescending<TopicInfo, int?>((Func<TopicInfo, int?>) (d => d.KeeperCount))
                                    .ThenBy<TopicInfo, string>((Func<TopicInfo, string>) (d => this.Name)))
                            .ToList<TopicInfo>();
                    }
                    else
                    {
                        if (column != this.ColumnReport1DgvStatus)
                            return;
                        list2 = (sortOrder == SortOrder.Ascending
                                ? (IEnumerable<TopicInfo>) list1
                                    .OrderBy<TopicInfo, string>((Func<TopicInfo, string>) (d => d.StatusToString))
                                    .ThenBy<TopicInfo, string>((Func<TopicInfo, string>) (d => this.Name))
                                : (IEnumerable<TopicInfo>) list1
                                    .OrderByDescending<TopicInfo, string>(
                                        (Func<TopicInfo, string>) (d => d.StatusToString))
                                    .ThenBy<TopicInfo, string>((Func<TopicInfo, string>) (d => this.Name)))
                            .ToList<TopicInfo>();
                    }

                    this._TopicsSource.Clear();
                    this._TopicsSource.DataSource = (object) list2;
                    column.HeaderCell.SortGlyphDirection = sortOrder;
                }
            }
        }

        private void _dgvReportDownloads_Click(object sender, EventArgs e)
        {
            if (this._dataGridTopicsList.Columns.GetColumnCount(DataGridViewElementStates.Selected) == 1)
            {
                DataGridViewColumn selectedColumn = this._dataGridTopicsList.SelectedColumns[0];
            }

            Console.WriteLine("");
        }

        private void WriteReports()
        {
            ClientLocalDB.Current.CreateReportByRootCategories();
            this._tcCetegoriesRootReports.Controls.Clear();
            Dictionary<Tuple<int, int>, Tuple<string, string>> reports = ClientLocalDB.Current.GetReports(new int?(0));
            string str1 = reports
                .Where<KeyValuePair<Tuple<int, int>, Tuple<string, string>>>(
                    (Func<KeyValuePair<Tuple<int, int>, Tuple<string, string>>, bool>) (x => x.Key.Item2 == 0))
                .Select<KeyValuePair<Tuple<int, int>, Tuple<string, string>>, string>(
                    (Func<KeyValuePair<Tuple<int, int>, Tuple<string, string>>, string>) (x => x.Value.Item2))
                .FirstOrDefault<string>();
            this._txtConsolidatedReport.Text = string.IsNullOrWhiteSpace(str1) ? string.Empty : str1;
            string str2 = reports
                .Where<KeyValuePair<Tuple<int, int>, Tuple<string, string>>>(
                    (Func<KeyValuePair<Tuple<int, int>, Tuple<string, string>>, bool>) (x => x.Key.Item2 == 1))
                .Select<KeyValuePair<Tuple<int, int>, Tuple<string, string>>, string>(
                    (Func<KeyValuePair<Tuple<int, int>, Tuple<string, string>>, string>) (x => x.Value.Item2))
                .FirstOrDefault<string>();
            this._tbConsolidatedTorrentClientsReport.Text = string.IsNullOrWhiteSpace(str2) ? string.Empty : str2;
            IEnumerable<Category> categories = ClientLocalDB.Current.GetCategories()
                .Where<Category>((Func<Category, bool>) (x => x.CategoryID > 100000));
            Size size = this._tcCetegoriesRootReports.Size;
            foreach (Category category in categories)
            {
                string str3 = ClientLocalDB.Current.GetReports(new int?(category.CategoryID))
                    .Where<KeyValuePair<Tuple<int, int>, Tuple<string, string>>>(
                        (Func<KeyValuePair<Tuple<int, int>, Tuple<string, string>>, bool>) (x => x.Key.Item2 == 0))
                    .Select<KeyValuePair<Tuple<int, int>, Tuple<string, string>>, string>(
                        (Func<KeyValuePair<Tuple<int, int>, Tuple<string, string>>, string>) (x => x.Value.Item2))
                    .FirstOrDefault<string>();
                if (!string.IsNullOrWhiteSpace(str3))
                {
                    TabPage tabPage = new TabPage();
                    TextBox textBox = new TextBox();
                    tabPage.Location = new Point(4, 22);
                    tabPage.Padding = new Padding(3);
                    tabPage.Text = category.Name;
                    tabPage.UseVisualStyleBackColor = true;
                    tabPage.AutoScroll = true;
                    tabPage.Size = size;
                    textBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                    textBox.Location = new Point(0, 0);
                    textBox.Multiline = true;
                    textBox.ReadOnly = true;
                    textBox.ScrollBars = ScrollBars.Both;
                    textBox.Size = new Size(size.Width - 8, size.Height - 20);
                    textBox.Text = str3;
                    this._tcCetegoriesRootReports.Controls.Add((Control) tabPage);
                    tabPage.Controls.Add((Control) textBox);
                }
            }
        }

        private void SaveSetingsToFile()
        {
            try
            {
                string empty = string.Empty;
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.DefaultExt = "tloback";
                saveFileDialog.Filter = "Файл архивных настроек|*.tloback";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                string fileName = saveFileDialog.FileName;
                if (string.IsNullOrWhiteSpace(fileName))
                    return;
                using (FileStream fileStream = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream, Encoding.UTF8))
                    {
                        foreach (TorrentClientInfo torrentClient in ClientLocalDB.Current.GetTorrentClients())
                        {
                            binaryWriter.Write("TorrentClientInfo");
                            binaryWriter.Write(torrentClient.UID.ToString());
                            binaryWriter.Write(torrentClient.Name);
                            binaryWriter.Write(torrentClient.Type);
                            binaryWriter.Write(torrentClient.ServerName);
                            binaryWriter.Write(torrentClient.ServerPort);
                            binaryWriter.Write(torrentClient.UserName);
                            binaryWriter.Write(torrentClient.UserPassword);
                        }

                        foreach (Category category in ClientLocalDB.Current.GetCategoriesEnable())
                        {
                            binaryWriter.Write("Category");
                            binaryWriter.Write(category.CategoryID);
                            binaryWriter.Write(category.CountSeeders);
                            binaryWriter.Write(category.TorrentClientUID.ToString());
                            binaryWriter.Write(category.Folder);
                            binaryWriter.Write(category.CreateSubFolder);
                            binaryWriter.Write(category.IsSaveTorrentFiles);
                            binaryWriter.Write(category.IsSaveWebPage);
                            binaryWriter.Write(category.Label);
                        }

                        int[] cats = ClientLocalDB.Current.GetCategoriesEnable()
                            .Select<Category, int>((Func<Category, int>) (x => x.CategoryID)).ToArray<int>();
                        Dictionary<Tuple<int, int>, Tuple<string, string>> reports =
                            ClientLocalDB.Current.GetReports(new int?());
                        foreach (KeyValuePair<Tuple<int, int>, Tuple<string, string>> keyValuePair in reports
                            .Where<KeyValuePair<Tuple<int, int>, Tuple<string, string>>>(
                                (Func<KeyValuePair<Tuple<int, int>, Tuple<string, string>>, bool>) (x =>
                                    ((IEnumerable<int>) cats).Contains<int>(x.Key.Item1))))
                        {
                            binaryWriter.Write("Report");
                            binaryWriter.Write(keyValuePair.Key.Item1);
                            binaryWriter.Write(keyValuePair.Key.Item2);
                            binaryWriter.Write(keyValuePair.Value.Item1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger.Error(ex.Message);
                this._logger.Trace(ex.StackTrace);
            }
        }

        private void ReadSettingsFromFile()
        {
            try
            {
                string empty = string.Empty;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.DefaultExt = "tloback";
                openFileDialog.Filter = "Файл архивных настроек|*.tloback";
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                string fileName = openFileDialog.FileName;
                if (string.IsNullOrWhiteSpace(fileName))
                    return;
                List<TorrentClientInfo> torrentClientInfoList = new List<TorrentClientInfo>();
                List<Category> categoryList = new List<Category>();
                List<Tuple<int, int, string>> result = new List<Tuple<int, int, string>>();
                using (FileStream fileStream = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream))
                    {
                        while (binaryReader.BaseStream.Length != binaryReader.BaseStream.Position)
                        {
                            string str = binaryReader.ReadString();
                            if (!(str == "TorrentClientInfo"))
                            {
                                if (!(str == "Category"))
                                {
                                    if (str == "Report")
                                        result.Add(new Tuple<int, int, string>(binaryReader.ReadInt32(),
                                            binaryReader.ReadInt32(), binaryReader.ReadString()));
                                }
                                else
                                {
                                    Category category = new Category()
                                    {
                                        CategoryID = binaryReader.ReadInt32(),
                                        IsEnable = true,
                                        CountSeeders = binaryReader.ReadInt32(),
                                        TorrentClientUID = Guid.Parse(binaryReader.ReadString()),
                                        Folder = binaryReader.ReadString(),
                                        CreateSubFolder = binaryReader.ReadInt32(),
                                        IsSaveTorrentFiles = binaryReader.ReadBoolean(),
                                        IsSaveWebPage = binaryReader.ReadBoolean(),
                                        Label = binaryReader.ReadString()
                                    };
                                    categoryList.Add(category);
                                }
                            }
                            else
                            {
                                TorrentClientInfo torrentClientInfo = new TorrentClientInfo()
                                {
                                    UID = Guid.Parse(binaryReader.ReadString()),
                                    Name = binaryReader.ReadString(),
                                    Type = binaryReader.ReadString(),
                                    ServerName = binaryReader.ReadString(),
                                    ServerPort = binaryReader.ReadInt32(),
                                    UserName = binaryReader.ReadString(),
                                    UserPassword = binaryReader.ReadString()
                                };
                                torrentClientInfoList.Add(torrentClientInfo);
                            }
                        }

                        ClientLocalDB.Current.SaveTorrentClients((IEnumerable<TorrentClientInfo>) torrentClientInfoList,
                            false);
                        ClientLocalDB.Current.CategoriesSave((IEnumerable<Category>) categoryList, false);
                        ClientLocalDB.Current.SaveSettingsReport(result);
                        ClientLocalDB.Current.SaveToDatabase();
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger.Error(ex.Message);
                this._logger.Trace(ex.StackTrace);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this._btSaveToFile = new System.Windows.Forms.ToolStripMenuItem();
            this._btLoadSettingsFromFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отчетыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SendReportsToForumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CreateReportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.задачиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RuningStopingDistributionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CreateConsolidatedReportByTorrentClientsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.UpdateAll = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateCountSeedersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateListTopicsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateKeepTopicsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadListKeepersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ClearKeeperListsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ClearDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTimerSetting = new System.Windows.Forms.ToolStripMenuItem();
            this._cbCategory = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this._tpReportDownloads = new System.Windows.Forms.TabPage();
            this._DateRegistration = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this._cbCountSeeders = new System.Windows.Forms.NumericUpDown();
            this._lbTotal = new System.Windows.Forms.Label();
            this._llUpdateTopicsByCategory = new System.Windows.Forms.LinkLabel();
            this._llUpdateCountSeedersByCategory = new System.Windows.Forms.LinkLabel();
            this._llUpdateDataDromTorrentClient = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.linkLabel5 = new System.Windows.Forms.LinkLabel();
            this.linkSetNewLabel = new System.Windows.Forms.LinkLabel();
            this._llSelectedTopicsDeleteFromBlackList = new System.Windows.Forms.LinkLabel();
            this._llSelectedTopicsToTorrentClient = new System.Windows.Forms.LinkLabel();
            this._llDownloadSelectTopics = new System.Windows.Forms.LinkLabel();
            this._llSelectedTopicsToBlackList = new System.Windows.Forms.LinkLabel();
            this._cbBlackList = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this._cbCategoryFilters = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this._dataGridTopicsList = new System.Windows.Forms.DataGridView();
            this.ColumnReport1DgvTopicID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnReport1DgvSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnReport1DgvStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnReport1DgvSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnReport1DgvName = new System.Windows.Forms.DataGridViewLinkColumn();
            this.ColumnReport1DgvAlternative = new System.Windows.Forms.DataGridViewLinkColumn();
            this.ColumnReport1DgvSeeders = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnReport1DgvAvgSeeders = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnReport1DgvRegTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnReport1DgvKeeperCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnReport1DgvBlack = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tabReports = new System.Windows.Forms.TabPage();
            this.tabConsolidatedReport = new System.Windows.Forms.TabPage();
            this._txtConsolidatedReport = new System.Windows.Forms.TextBox();
            this.ConsolidatedTorrentClientsReport = new System.Windows.Forms.TabPage();
            this._tbConsolidatedTorrentClientsReport = new System.Windows.Forms.TextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this._tcCetegoriesRootReports = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this._tpReportDownloads.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._cbCountSeeders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dataGridTopicsList)).BeginInit();
            this.tabConsolidatedReport.SuspendLayout();
            this.ConsolidatedTorrentClientsReport.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this._tcCetegoriesRootReports.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.отчетыToolStripMenuItem,
            this.задачиToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1040, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSettingsToolStripMenuItem,
            this.toolStripSeparator4,
            this._btSaveToFile,
            this._btLoadSettingsFromFile,
            this.toolStripSeparator3,
            this.ExitToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // menuSettingsToolStripMenuItem
            // 
            this.menuSettingsToolStripMenuItem.Name = "menuSettingsToolStripMenuItem";
            this.menuSettingsToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.menuSettingsToolStripMenuItem.Text = "Настройки";
            this.menuSettingsToolStripMenuItem.Click += new System.EventHandler(this.MenuClick);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(239, 6);
            // 
            // _btSaveToFile
            // 
            this._btSaveToFile.Name = "_btSaveToFile";
            this._btSaveToFile.Size = new System.Drawing.Size(242, 22);
            this._btSaveToFile.Text = "Сохранить настройки в файл";
            this._btSaveToFile.Click += new System.EventHandler(this.MenuClick);
            // 
            // _btLoadSettingsFromFile
            // 
            this._btLoadSettingsFromFile.Name = "_btLoadSettingsFromFile";
            this._btLoadSettingsFromFile.Size = new System.Drawing.Size(242, 22);
            this._btLoadSettingsFromFile.Text = "Загрузить настройки из файла";
            this._btLoadSettingsFromFile.Click += new System.EventHandler(this.MenuClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(239, 6);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.ExitToolStripMenuItem.Text = "Выход";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.MenuClick);
            // 
            // отчетыToolStripMenuItem
            // 
            this.отчетыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SendReportsToForumToolStripMenuItem,
            this.CreateReportsToolStripMenuItem});
            this.отчетыToolStripMenuItem.Name = "отчетыToolStripMenuItem";
            this.отчетыToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.отчетыToolStripMenuItem.Text = "Отчеты";
            // 
            // SendReportsToForumToolStripMenuItem
            // 
            this.SendReportsToForumToolStripMenuItem.Name = "SendReportsToForumToolStripMenuItem";
            this.SendReportsToForumToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.SendReportsToForumToolStripMenuItem.Text = "Отправить отчеты на форум";
            this.SendReportsToForumToolStripMenuItem.Click += new System.EventHandler(this.MenuClick);
            // 
            // CreateReportsToolStripMenuItem
            // 
            this.CreateReportsToolStripMenuItem.Name = "CreateReportsToolStripMenuItem";
            this.CreateReportsToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.CreateReportsToolStripMenuItem.Text = "Сформировать отчеты";
            this.CreateReportsToolStripMenuItem.Click += new System.EventHandler(this.MenuClick);
            // 
            // задачиToolStripMenuItem
            // 
            this.задачиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RuningStopingDistributionToolStripMenuItem,
            this.CreateConsolidatedReportByTorrentClientsToolStripMenuItem,
            this.toolStripSeparator1,
            this.UpdateAll,
            this.UpdateCountSeedersToolStripMenuItem,
            this.UpdateListTopicsToolStripMenuItem,
            this.UpdateKeepTopicsToolStripMenuItem,
            this.LoadListKeepersToolStripMenuItem,
            this.toolStripSeparator2,
            this.ClearKeeperListsToolStripMenuItem,
            this.ClearDatabaseToolStripMenuItem,
            this.toolStripSeparator5,
            this.menuTimerSetting});
            this.задачиToolStripMenuItem.Name = "задачиToolStripMenuItem";
            this.задачиToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.задачиToolStripMenuItem.Text = "Задачи";
            // 
            // RuningStopingDistributionToolStripMenuItem
            // 
            this.RuningStopingDistributionToolStripMenuItem.Name = "RuningStopingDistributionToolStripMenuItem";
            this.RuningStopingDistributionToolStripMenuItem.Size = new System.Drawing.Size(379, 22);
            this.RuningStopingDistributionToolStripMenuItem.Text = "Запуск/Остановка раздач в торрент-клиентах";
            this.RuningStopingDistributionToolStripMenuItem.Click += new System.EventHandler(this.MenuClick);
            // 
            // CreateConsolidatedReportByTorrentClientsToolStripMenuItem
            // 
            this.CreateConsolidatedReportByTorrentClientsToolStripMenuItem.Name = "CreateConsolidatedReportByTorrentClientsToolStripMenuItem";
            this.CreateConsolidatedReportByTorrentClientsToolStripMenuItem.Size = new System.Drawing.Size(379, 22);
            this.CreateConsolidatedReportByTorrentClientsToolStripMenuItem.Text = "Построить сводный отчет по торрент-клиентам";
            this.CreateConsolidatedReportByTorrentClientsToolStripMenuItem.Click += new System.EventHandler(this.MenuClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(376, 6);
            // 
            // UpdateAll
            // 
            this.UpdateAll.Name = "UpdateAll";
            this.UpdateAll.Size = new System.Drawing.Size(379, 22);
            this.UpdateAll.Text = "Обновить всё и сразу";
            // 
            // UpdateCountSeedersToolStripMenuItem
            // 
            this.UpdateCountSeedersToolStripMenuItem.Name = "UpdateCountSeedersToolStripMenuItem";
            this.UpdateCountSeedersToolStripMenuItem.Size = new System.Drawing.Size(379, 22);
            this.UpdateCountSeedersToolStripMenuItem.Text = "Обновить кол-во сидов по всем разделам";
            this.UpdateCountSeedersToolStripMenuItem.Click += new System.EventHandler(this.MenuClick);
            // 
            // UpdateListTopicsToolStripMenuItem
            // 
            this.UpdateListTopicsToolStripMenuItem.Name = "UpdateListTopicsToolStripMenuItem";
            this.UpdateListTopicsToolStripMenuItem.Size = new System.Drawing.Size(379, 22);
            this.UpdateListTopicsToolStripMenuItem.Text = "Обновить список топиков по всем разделам";
            this.UpdateListTopicsToolStripMenuItem.Click += new System.EventHandler(this.MenuClick);
            // 
            // UpdateKeepTopicsToolStripMenuItem
            // 
            this.UpdateKeepTopicsToolStripMenuItem.Name = "UpdateKeepTopicsToolStripMenuItem";
            this.UpdateKeepTopicsToolStripMenuItem.Size = new System.Drawing.Size(379, 22);
            this.UpdateKeepTopicsToolStripMenuItem.Text = "Обновить списки хранимого по всем Torrent-клиентам";
            this.UpdateKeepTopicsToolStripMenuItem.Click += new System.EventHandler(this.MenuClick);
            // 
            // LoadListKeepersToolStripMenuItem
            // 
            this.LoadListKeepersToolStripMenuItem.Name = "LoadListKeepersToolStripMenuItem";
            this.LoadListKeepersToolStripMenuItem.Size = new System.Drawing.Size(379, 22);
            this.LoadListKeepersToolStripMenuItem.Text = "Обновить данные о других хранителях";
            this.LoadListKeepersToolStripMenuItem.Click += new System.EventHandler(this.MenuClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(376, 6);
            // 
            // ClearKeeperListsToolStripMenuItem
            // 
            this.ClearKeeperListsToolStripMenuItem.Name = "ClearKeeperListsToolStripMenuItem";
            this.ClearKeeperListsToolStripMenuItem.Size = new System.Drawing.Size(379, 22);
            this.ClearKeeperListsToolStripMenuItem.Text = "Очистить списки хранителей со свод. значениями";
            this.ClearKeeperListsToolStripMenuItem.Click += new System.EventHandler(this.MenuClick);
            // 
            // ClearDatabaseToolStripMenuItem
            // 
            this.ClearDatabaseToolStripMenuItem.Name = "ClearDatabaseToolStripMenuItem";
            this.ClearDatabaseToolStripMenuItem.Size = new System.Drawing.Size(379, 22);
            this.ClearDatabaseToolStripMenuItem.Text = "Очистить списки разделов (удалить топики)";
            this.ClearDatabaseToolStripMenuItem.Click += new System.EventHandler(this.MenuClick);
            // 
            // menuTimerSetting
            // 
            this.menuTimerSetting.Checked = true;
            this.menuTimerSetting.CheckOnClick = true;
            this.menuTimerSetting.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuTimerSetting.Name = "menuTimerSetting";
            this.menuTimerSetting.Size = new System.Drawing.Size(379, 22);
            this.menuTimerSetting.Text = "Таймер";
            // 
            // _cbCategory
            // 
            this._cbCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._cbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbCategory.FormattingEnabled = true;
            this._cbCategory.Location = new System.Drawing.Point(117, 27);
            this._cbCategory.Name = "_cbCategory";
            this._cbCategory.Size = new System.Drawing.Size(911, 21);
            this._cbCategory.TabIndex = 1;
            this._cbCategory.SelectionChangeCommitted += new System.EventHandler(this.SelectionChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Выберите раздел:";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this._tpReportDownloads);
            this.tabControl1.Controls.Add(this.tabReports);
            this.tabControl1.Controls.Add(this.tabConsolidatedReport);
            this.tabControl1.Controls.Add(this.ConsolidatedTorrentClientsReport);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(0, 54);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1040, 462);
            this.tabControl1.TabIndex = 3;
            this.tabControl1.VisibleChanged += new System.EventHandler(this.SelectionChanged);
            // 
            // _tpReportDownloads
            // 
            this._tpReportDownloads.Controls.Add(this._DateRegistration);
            this._tpReportDownloads.Controls.Add(this.label5);
            this._tpReportDownloads.Controls.Add(this._cbCountSeeders);
            this._tpReportDownloads.Controls.Add(this._lbTotal);
            this._tpReportDownloads.Controls.Add(this._llUpdateTopicsByCategory);
            this._tpReportDownloads.Controls.Add(this._llUpdateCountSeedersByCategory);
            this._tpReportDownloads.Controls.Add(this._llUpdateDataDromTorrentClient);
            this._tpReportDownloads.Controls.Add(this.label4);
            this._tpReportDownloads.Controls.Add(this.linkLabel5);
            this._tpReportDownloads.Controls.Add(this.linkSetNewLabel);
            this._tpReportDownloads.Controls.Add(this._llSelectedTopicsDeleteFromBlackList);
            this._tpReportDownloads.Controls.Add(this._llSelectedTopicsToTorrentClient);
            this._tpReportDownloads.Controls.Add(this._llDownloadSelectTopics);
            this._tpReportDownloads.Controls.Add(this._llSelectedTopicsToBlackList);
            this._tpReportDownloads.Controls.Add(this._cbBlackList);
            this._tpReportDownloads.Controls.Add(this.label2);
            this._tpReportDownloads.Controls.Add(this._cbCategoryFilters);
            this._tpReportDownloads.Controls.Add(this.label3);
            this._tpReportDownloads.Controls.Add(this._dataGridTopicsList);
            this._tpReportDownloads.Location = new System.Drawing.Point(4, 22);
            this._tpReportDownloads.Name = "_tpReportDownloads";
            this._tpReportDownloads.Padding = new System.Windows.Forms.Padding(3);
            this._tpReportDownloads.Size = new System.Drawing.Size(1032, 436);
            this._tpReportDownloads.TabIndex = 2;
            this._tpReportDownloads.Text = "Обработка раздела";
            this._tpReportDownloads.UseVisualStyleBackColor = true;
            // 
            // _DateRegistration
            // 
            this._DateRegistration.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._DateRegistration.Location = new System.Drawing.Point(65, 10);
            this._DateRegistration.Name = "_DateRegistration";
            this._DateRegistration.Size = new System.Drawing.Size(93, 20);
            this._DateRegistration.TabIndex = 32;
            this._DateRegistration.ValueChanged += new System.EventHandler(this.SelectionChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "Дата до:";
            // 
            // _cbCountSeeders
            // 
            this._cbCountSeeders.Location = new System.Drawing.Point(247, 10);
            this._cbCountSeeders.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this._cbCountSeeders.Name = "_cbCountSeeders";
            this._cbCountSeeders.Size = new System.Drawing.Size(40, 20);
            this._cbCountSeeders.TabIndex = 30;
            this._cbCountSeeders.ValueChanged += new System.EventHandler(this.SelectionChanged);
            // 
            // _lbTotal
            // 
            this._lbTotal.AutoSize = true;
            this._lbTotal.Location = new System.Drawing.Point(8, 32);
            this._lbTotal.Name = "_lbTotal";
            this._lbTotal.Size = new System.Drawing.Size(40, 13);
            this._lbTotal.TabIndex = 29;
            this._lbTotal.Text = "Итого:";
            // 
            // _llUpdateTopicsByCategory
            // 
            this._llUpdateTopicsByCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._llUpdateTopicsByCategory.AutoSize = true;
            this._llUpdateTopicsByCategory.Location = new System.Drawing.Point(836, 399);
            this._llUpdateTopicsByCategory.Name = "_llUpdateTopicsByCategory";
            this._llUpdateTopicsByCategory.Size = new System.Drawing.Size(154, 13);
            this._llUpdateTopicsByCategory.TabIndex = 28;
            this._llUpdateTopicsByCategory.TabStop = true;
            this._llUpdateTopicsByCategory.Text = "Обновить список по разделу";
            this._llUpdateTopicsByCategory.Click += new System.EventHandler(this.LinkClick);
            // 
            // _llUpdateCountSeedersByCategory
            // 
            this._llUpdateCountSeedersByCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._llUpdateCountSeedersByCategory.AutoSize = true;
            this._llUpdateCountSeedersByCategory.Location = new System.Drawing.Point(836, 382);
            this._llUpdateCountSeedersByCategory.Name = "_llUpdateCountSeedersByCategory";
            this._llUpdateCountSeedersByCategory.Size = new System.Drawing.Size(184, 13);
            this._llUpdateCountSeedersByCategory.TabIndex = 27;
            this._llUpdateCountSeedersByCategory.TabStop = true;
            this._llUpdateCountSeedersByCategory.Text = "Обновить кол-во сидов по разделу";
            this._llUpdateCountSeedersByCategory.Click += new System.EventHandler(this.LinkClick);
            // 
            // _llUpdateDataDromTorrentClient
            // 
            this._llUpdateDataDromTorrentClient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._llUpdateDataDromTorrentClient.AutoSize = true;
            this._llUpdateDataDromTorrentClient.Location = new System.Drawing.Point(836, 416);
            this._llUpdateDataDromTorrentClient.Name = "_llUpdateDataDromTorrentClient";
            this._llUpdateDataDromTorrentClient.Size = new System.Drawing.Size(184, 13);
            this._llUpdateDataDromTorrentClient.TabIndex = 26;
            this._llUpdateDataDromTorrentClient.TabStop = true;
            this._llUpdateDataDromTorrentClient.Text = "Обновить инф. из торрент-клиента";
            this._llUpdateDataDromTorrentClient.Click += new System.EventHandler(this.LinkClick);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(836, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Действия с выделенными";
            // 
            // linkLabel5
            // 
            this.linkLabel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel5.AutoSize = true;
            this.linkLabel5.Location = new System.Drawing.Point(836, 102);
            this.linkLabel5.Name = "linkLabel5";
            this.linkLabel5.Size = new System.Drawing.Size(186, 13);
            this.linkLabel5.TabIndex = 22;
            this.linkLabel5.TabStop = true;
            this.linkLabel5.Text = "Удалить из Torrent-клиента+файлы";
            this.linkLabel5.Visible = false;
            // 
            // linkSetNewLabel
            // 
            this.linkSetNewLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkSetNewLabel.AutoSize = true;
            this.linkSetNewLabel.Location = new System.Drawing.Point(836, 85);
            this.linkSetNewLabel.Name = "linkSetNewLabel";
            this.linkSetNewLabel.Size = new System.Drawing.Size(100, 13);
            this.linkSetNewLabel.TabIndex = 21;
            this.linkSetNewLabel.TabStop = true;
            this.linkSetNewLabel.Text = "Установить метку";
            this.linkSetNewLabel.Click += new System.EventHandler(this.LinkClick);
            // 
            // _llSelectedTopicsDeleteFromBlackList
            // 
            this._llSelectedTopicsDeleteFromBlackList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._llSelectedTopicsDeleteFromBlackList.AutoSize = true;
            this._llSelectedTopicsDeleteFromBlackList.Location = new System.Drawing.Point(836, 136);
            this._llSelectedTopicsDeleteFromBlackList.Name = "_llSelectedTopicsDeleteFromBlackList";
            this._llSelectedTopicsDeleteFromBlackList.Size = new System.Drawing.Size(147, 13);
            this._llSelectedTopicsDeleteFromBlackList.TabIndex = 20;
            this._llSelectedTopicsDeleteFromBlackList.TabStop = true;
            this._llSelectedTopicsDeleteFromBlackList.Text = "Удалить из черного списка";
            this._llSelectedTopicsDeleteFromBlackList.Click += new System.EventHandler(this.LinkClick);
            // 
            // _llSelectedTopicsToTorrentClient
            // 
            this._llSelectedTopicsToTorrentClient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._llSelectedTopicsToTorrentClient.AutoSize = true;
            this._llSelectedTopicsToTorrentClient.Location = new System.Drawing.Point(836, 68);
            this._llSelectedTopicsToTorrentClient.Name = "_llSelectedTopicsToTorrentClient";
            this._llSelectedTopicsToTorrentClient.Size = new System.Drawing.Size(141, 13);
            this._llSelectedTopicsToTorrentClient.TabIndex = 19;
            this._llSelectedTopicsToTorrentClient.TabStop = true;
            this._llSelectedTopicsToTorrentClient.Text = "Добавить в Torrent-клиент";
            this._llSelectedTopicsToTorrentClient.Click += new System.EventHandler(this.LinkClick);
            // 
            // _llDownloadSelectTopics
            // 
            this._llDownloadSelectTopics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._llDownloadSelectTopics.AutoSize = true;
            this._llDownloadSelectTopics.Location = new System.Drawing.Point(836, 51);
            this._llDownloadSelectTopics.Name = "_llDownloadSelectTopics";
            this._llDownloadSelectTopics.Size = new System.Drawing.Size(122, 13);
            this._llDownloadSelectTopics.TabIndex = 18;
            this._llDownloadSelectTopics.TabStop = true;
            this._llDownloadSelectTopics.Text = "Скачать Torrent-файлы";
            this._llDownloadSelectTopics.Click += new System.EventHandler(this.LinkClick);
            // 
            // _llSelectedTopicsToBlackList
            // 
            this._llSelectedTopicsToBlackList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._llSelectedTopicsToBlackList.AutoSize = true;
            this._llSelectedTopicsToBlackList.Location = new System.Drawing.Point(836, 119);
            this._llSelectedTopicsToBlackList.Name = "_llSelectedTopicsToBlackList";
            this._llSelectedTopicsToBlackList.Size = new System.Drawing.Size(145, 13);
            this._llSelectedTopicsToBlackList.TabIndex = 17;
            this._llSelectedTopicsToBlackList.TabStop = true;
            this._llSelectedTopicsToBlackList.Text = "Добавить в черный список";
            this._llSelectedTopicsToBlackList.Click += new System.EventHandler(this.LinkClick);
            // 
            // _cbBlackList
            // 
            this._cbBlackList.AutoSize = true;
            this._cbBlackList.Location = new System.Drawing.Point(525, 11);
            this._cbBlackList.Name = "_cbBlackList";
            this._cbBlackList.Size = new System.Drawing.Size(105, 17);
            this._cbBlackList.TabIndex = 14;
            this._cbBlackList.Text = "Черный список";
            this._cbBlackList.UseVisualStyleBackColor = true;
            this._cbBlackList.CheckedChanged += new System.EventHandler(this.SelectionChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(293, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Фильтр:";
            // 
            // _cbCategoryFilters
            // 
            this._cbCategoryFilters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbCategoryFilters.FormattingEnabled = true;
            this._cbCategoryFilters.Items.AddRange(new object[] {
            "Все",
            "Не скачан торрент и нет хранителя",
            "Не скачан торрент",
            "Храню",
            "Храню и есть хранитель",
            "Не храню",
            "Скачиваю раздачу",
            "Я релизер",
            "Не скачано"});
            this._cbCategoryFilters.Location = new System.Drawing.Point(349, 9);
            this._cbCategoryFilters.Name = "_cbCategoryFilters";
            this._cbCategoryFilters.Size = new System.Drawing.Size(170, 21);
            this._cbCategoryFilters.TabIndex = 11;
            this._cbCategoryFilters.SelectionChangeCommitted += new System.EventHandler(this.SelectionChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(164, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Кол-во сидов:";
            // 
            // _dataGridTopicsList
            // 
            this._dataGridTopicsList.AllowUserToAddRows = false;
            this._dataGridTopicsList.AllowUserToDeleteRows = false;
            this._dataGridTopicsList.AllowUserToResizeRows = false;
            this._dataGridTopicsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._dataGridTopicsList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this._dataGridTopicsList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dataGridTopicsList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnReport1DgvTopicID,
            this.ColumnReport1DgvSelect,
            this.ColumnReport1DgvStatus,
            this.ColumnReport1DgvSize,
            this.ColumnReport1DgvName,
            this.ColumnReport1DgvAlternative,
            this.ColumnReport1DgvSeeders,
            this.ColumnReport1DgvAvgSeeders,
            this.ColumnReport1DgvRegTime,
            this.ColumnReport1DgvKeeperCount,
            this.ColumnReport1DgvBlack});
            this._dataGridTopicsList.Location = new System.Drawing.Point(8, 48);
            this._dataGridTopicsList.MultiSelect = false;
            this._dataGridTopicsList.Name = "_dataGridTopicsList";
            this._dataGridTopicsList.RowHeadersVisible = false;
            this._dataGridTopicsList.Size = new System.Drawing.Size(822, 382);
            this._dataGridTopicsList.TabIndex = 0;
            this._dataGridTopicsList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ContentClick);
            this._dataGridTopicsList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this._dgvReportDownloads_CellDoubleClick);
            this._dataGridTopicsList.Click += new System.EventHandler(this._dgvReportDownloads_Click);
            // 
            // ColumnReport1DgvTopicID
            // 
            this.ColumnReport1DgvTopicID.DataPropertyName = "TopicID";
            this.ColumnReport1DgvTopicID.HeaderText = "Column1";
            this.ColumnReport1DgvTopicID.Name = "ColumnReport1DgvTopicID";
            this.ColumnReport1DgvTopicID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ColumnReport1DgvTopicID.Visible = false;
            this.ColumnReport1DgvTopicID.Width = 10;
            // 
            // ColumnReport1DgvSelect
            // 
            this.ColumnReport1DgvSelect.DataPropertyName = "Checked";
            this.ColumnReport1DgvSelect.FalseValue = "false";
            this.ColumnReport1DgvSelect.HeaderText = "";
            this.ColumnReport1DgvSelect.Name = "ColumnReport1DgvSelect";
            this.ColumnReport1DgvSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnReport1DgvSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ColumnReport1DgvSelect.TrueValue = "true";
            this.ColumnReport1DgvSelect.Width = 19;
            // 
            // ColumnReport1DgvStatus
            // 
            this.ColumnReport1DgvStatus.DataPropertyName = "StatusToString";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColumnReport1DgvStatus.DefaultCellStyle = dataGridViewCellStyle1;
            this.ColumnReport1DgvStatus.HeaderText = "";
            this.ColumnReport1DgvStatus.Name = "ColumnReport1DgvStatus";
            this.ColumnReport1DgvStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ColumnReport1DgvStatus.Width = 19;
            // 
            // ColumnReport1DgvSize
            // 
            this.ColumnReport1DgvSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColumnReport1DgvSize.DataPropertyName = "SizeToString";
            this.ColumnReport1DgvSize.HeaderText = "Размер";
            this.ColumnReport1DgvSize.Name = "ColumnReport1DgvSize";
            this.ColumnReport1DgvSize.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ColumnReport1DgvSize.Width = 71;
            // 
            // ColumnReport1DgvName
            // 
            this.ColumnReport1DgvName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnReport1DgvName.DataPropertyName = "Name";
            this.ColumnReport1DgvName.HeaderText = "Наименование";
            this.ColumnReport1DgvName.Name = "ColumnReport1DgvName";
            this.ColumnReport1DgvName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnReport1DgvName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // ColumnReport1DgvAlternative
            // 
            this.ColumnReport1DgvAlternative.DataPropertyName = "Alternative";
            this.ColumnReport1DgvAlternative.HeaderText = "Альтернативы";
            this.ColumnReport1DgvAlternative.Name = "ColumnReport1DgvAlternative";
            this.ColumnReport1DgvAlternative.ReadOnly = true;
            this.ColumnReport1DgvAlternative.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnReport1DgvAlternative.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ColumnReport1DgvAlternative.Width = 105;
            // 
            // ColumnReport1DgvSeeders
            // 
            this.ColumnReport1DgvSeeders.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColumnReport1DgvSeeders.DataPropertyName = "Seeders";
            this.ColumnReport1DgvSeeders.HeaderText = "Сиды";
            this.ColumnReport1DgvSeeders.Name = "ColumnReport1DgvSeeders";
            this.ColumnReport1DgvSeeders.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ColumnReport1DgvSeeders.Width = 59;
            // 
            // ColumnReport1DgvAvgSeeders
            // 
            this.ColumnReport1DgvAvgSeeders.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnReport1DgvAvgSeeders.DataPropertyName = "AvgSeeders";
            this.ColumnReport1DgvAvgSeeders.HeaderText = "Ср. кол-во сидов";
            this.ColumnReport1DgvAvgSeeders.Name = "ColumnReport1DgvAvgSeeders";
            this.ColumnReport1DgvAvgSeeders.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ColumnReport1DgvAvgSeeders.Width = 85;
            // 
            // ColumnReport1DgvRegTime
            // 
            this.ColumnReport1DgvRegTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnReport1DgvRegTime.DataPropertyName = "RegTimeToString";
            this.ColumnReport1DgvRegTime.HeaderText = "Дата";
            this.ColumnReport1DgvRegTime.Name = "ColumnReport1DgvRegTime";
            this.ColumnReport1DgvRegTime.ReadOnly = true;
            this.ColumnReport1DgvRegTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ColumnReport1DgvRegTime.Width = 80;
            // 
            // ColumnReport1DgvKeeperCount
            // 
            this.ColumnReport1DgvKeeperCount.DataPropertyName = "KeeperCount";
            dataGridViewCellStyle2.Format = "N0";
            this.ColumnReport1DgvKeeperCount.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColumnReport1DgvKeeperCount.HeaderText = "Хранителей";
            this.ColumnReport1DgvKeeperCount.MaxInputLength = 64;
            this.ColumnReport1DgvKeeperCount.Name = "ColumnReport1DgvKeeperCount";
            this.ColumnReport1DgvKeeperCount.ReadOnly = true;
            this.ColumnReport1DgvKeeperCount.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnReport1DgvKeeperCount.ToolTipText = "Всего хранителей (без учёта Вас)";
            this.ColumnReport1DgvKeeperCount.Width = 92;
            // 
            // ColumnReport1DgvBlack
            // 
            this.ColumnReport1DgvBlack.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnReport1DgvBlack.DataPropertyName = "IsBlackList";
            this.ColumnReport1DgvBlack.FalseValue = "false";
            this.ColumnReport1DgvBlack.HeaderText = "Black";
            this.ColumnReport1DgvBlack.Name = "ColumnReport1DgvBlack";
            this.ColumnReport1DgvBlack.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnReport1DgvBlack.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ColumnReport1DgvBlack.TrueValue = "true";
            this.ColumnReport1DgvBlack.Visible = false;
            this.ColumnReport1DgvBlack.Width = 40;
            // 
            // tabReports
            // 
            this.tabReports.Location = new System.Drawing.Point(4, 22);
            this.tabReports.Name = "tabReports";
            this.tabReports.Padding = new System.Windows.Forms.Padding(3);
            this.tabReports.Size = new System.Drawing.Size(1032, 436);
            this.tabReports.TabIndex = 3;
            this.tabReports.Text = "Отчеты";
            this.tabReports.UseVisualStyleBackColor = true;
            // 
            // tabConsolidatedReport
            // 
            this.tabConsolidatedReport.Controls.Add(this._txtConsolidatedReport);
            this.tabConsolidatedReport.Location = new System.Drawing.Point(4, 22);
            this.tabConsolidatedReport.Name = "tabConsolidatedReport";
            this.tabConsolidatedReport.Size = new System.Drawing.Size(1032, 436);
            this.tabConsolidatedReport.TabIndex = 0;
            this.tabConsolidatedReport.Text = "Сводный отчет";
            this.tabConsolidatedReport.UseVisualStyleBackColor = true;
            // 
            // _txtConsolidatedReport
            // 
            this._txtConsolidatedReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtConsolidatedReport.Location = new System.Drawing.Point(0, 0);
            this._txtConsolidatedReport.Multiline = true;
            this._txtConsolidatedReport.Name = "_txtConsolidatedReport";
            this._txtConsolidatedReport.Size = new System.Drawing.Size(1032, 440);
            this._txtConsolidatedReport.TabIndex = 0;
            // 
            // ConsolidatedTorrentClientsReport
            // 
            this.ConsolidatedTorrentClientsReport.Controls.Add(this._tbConsolidatedTorrentClientsReport);
            this.ConsolidatedTorrentClientsReport.Location = new System.Drawing.Point(4, 22);
            this.ConsolidatedTorrentClientsReport.Name = "ConsolidatedTorrentClientsReport";
            this.ConsolidatedTorrentClientsReport.Padding = new System.Windows.Forms.Padding(3);
            this.ConsolidatedTorrentClientsReport.Size = new System.Drawing.Size(1032, 436);
            this.ConsolidatedTorrentClientsReport.TabIndex = 5;
            this.ConsolidatedTorrentClientsReport.Text = "Отчет torrent-клиентов";
            this.ConsolidatedTorrentClientsReport.UseVisualStyleBackColor = true;
            // 
            // _tbConsolidatedTorrentClientsReport
            // 
            this._tbConsolidatedTorrentClientsReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tbConsolidatedTorrentClientsReport.Location = new System.Drawing.Point(0, 0);
            this._tbConsolidatedTorrentClientsReport.Multiline = true;
            this._tbConsolidatedTorrentClientsReport.Name = "_tbConsolidatedTorrentClientsReport";
            this._tbConsolidatedTorrentClientsReport.ReadOnly = true;
            this._tbConsolidatedTorrentClientsReport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._tbConsolidatedTorrentClientsReport.Size = new System.Drawing.Size(1032, 436);
            this._tbConsolidatedTorrentClientsReport.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this._tcCetegoriesRootReports);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1032, 436);
            this.tabPage1.TabIndex = 4;
            this.tabPage1.Text = "????";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // _tcCetegoriesRootReports
            // 
            this._tcCetegoriesRootReports.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tcCetegoriesRootReports.Controls.Add(this.tabPage2);
            this._tcCetegoriesRootReports.Controls.Add(this.tabPage3);
            this._tcCetegoriesRootReports.Location = new System.Drawing.Point(1, 1);
            this._tcCetegoriesRootReports.Name = "_tcCetegoriesRootReports";
            this._tcCetegoriesRootReports.SelectedIndex = 0;
            this._tcCetegoriesRootReports.Size = new System.Drawing.Size(1031, 438);
            this._tcCetegoriesRootReports.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1023, 412);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1023, 412);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 518);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1040, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Visible = false;
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(376, 6);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1040, 540);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._cbCategory);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this._tpReportDownloads.ResumeLayout(false);
            this._tpReportDownloads.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._cbCountSeeders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dataGridTopicsList)).EndInit();
            this.tabConsolidatedReport.ResumeLayout(false);
            this.tabConsolidatedReport.PerformLayout();
            this.ConsolidatedTorrentClientsReport.ResumeLayout(false);
            this.ConsolidatedTorrentClientsReport.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this._tcCetegoriesRootReports.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}