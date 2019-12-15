﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using NLog;
using TLO.Clients;
using TLO.Info;
using TLO.Tools;

namespace TLO.Forms
{
    internal sealed partial class MainForm : Form
    {
        private readonly Dictionary<BackgroundWorker, Tuple<DateTime, object, string>> _backgroundWorkers =
            new Dictionary<BackgroundWorker, Tuple<DateTime, object, string>>();

        private readonly BindingSource _categorySource = new BindingSource();

        private readonly string _headText;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Timer _tmr;
        private readonly BindingSource _topicsSource = new BindingSource();
        private DateTime _lastRunTimer = DateTime.Now;

        public MainForm()
        {
            InitializeComponent();
            menuTimerSetting.CheckStateChanged += (sender, args) =>
            {
                if (menuTimerSetting.Checked)
                {
                    _lastRunTimer = DateTime.Now;
                    if (!_tmr.Enabled) _tmr.Start();
                }
                else
                {
                    if (_tmr.Enabled) _tmr.Stop();
                }
            };
            // var locationBinding = new Binding("Location", Properties.Settings.Default,
                // "WindowLocation", true, DataSourceUpdateMode.OnPropertyChanged);
            // var sizeBinding = new Binding("Size", Properties.Settings.Default,
                // "WindowSize", true, DataSourceUpdateMode.OnPropertyChanged);
            // locationBinding.ControlUpdateMode = ControlUpdateMode.Never;
            // sizeBinding.ControlUpdateMode = ControlUpdateMode.Never;
            // var bindingAdded = false;

            // void SyncBindings()
            // {
                // if (WindowState == FormWindowState.Normal)
                // {
                    // if (bindingAdded) return;
                    // DataBindings.Add(locationBinding);
                    // DataBindings.Add(sizeBinding);
                    // bindingAdded = true;
                // }
                // else
                // {
                    // if (!bindingAdded) return;
                    // DataBindings.Remove(locationBinding);
                    // DataBindings.Remove(sizeBinding);
                    // bindingAdded = false;
                // }
            // }

            // Resize += (sender, args) => { SyncBindings(); };
            // HandleCreated += (sender, args) => Location = Properties.Settings.Default.WindowLocation;
            // HandleCreated += (sender, args) => SyncBindings();

            _DateRegistration.Value = DateTime.Now.AddDays(-30.0);
            Text = _headText =
                $"TLO {FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion}";
            _cbCountSeeders.Value = new decimal(0);
            _cbCategoryFilters.SelectedItem = "Не скачан торрент и нет хранителя";
            _categorySource.Clear();
            _categorySource.DataSource = ClientLocalDb.Current.GetCategoriesEnable(true);
            _categorySource.CurrentChanged += SelectionChanged;
            _cbCategory.DataSource = _categorySource;
            if (_categorySource.Count > 0)
                _categorySource.Position = 1;
            _topicsSource.CurrentChanged += SelectionChanged;
            _dataGridTopicsList.AutoGenerateColumns = false;
            _dataGridTopicsList.ClearSelection();
            _dataGridTopicsList.DataSource = _topicsSource;
            Type dgvType = _dataGridTopicsList.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(_dataGridTopicsList, true, null);
            Disposed += MainForm_Disposed;
            _tmr = new Timer();
            _tmr.Tick += tmr_Tick;
            _tmr.Interval = 1000;
            _tmr.Start();
            TrayObject.TrayIcon.ContextMenu.MenuItems.Add(new MenuItem(@"Закрыть", DoQuit));
            WriteReports();
        }

        private new Point Location
        {
            set
            {
                if (
                    value.X > 0 &&
                    value.Y > 0 &&
                    value.X < SystemInformation.VirtualScreen.Size.Width - 100 &&
                    value.Y < SystemInformation.VirtualScreen.Size.Height - 100
                )
                    base.Location = value;
            }
        }

        private void MenuClick(object sender, EventArgs e)
        {
            try
            {
                if (sender == menuSettingsToolStripMenuItem)
                {
                    if (new SettingsForm().ShowDialog() == DialogResult.OK)
                    {
                        _categorySource.Clear();
                        _categorySource.DataSource = null;
                        _categorySource.DataSource = ClientLocalDb.Current.GetCategoriesEnable(true);
                        _categorySource.Position = 0;
                    }
                }
                else if (sender == UpdateAll)
                {
                    DwCreateAndRun(WorkerMethods.bwUpdateTopicsByCategories,
                        "Полное обновление информации о топиках (раздачах) по всем категориям...",
                        ClientLocalDb.Current.GetCategoriesEnable());
                    DwCreateAndRun(WorkerMethods.bwUpdateHashFromAllTorrentClients,
                        "Полное обновление информации из Torrent-клиентов...");
                    DwCreateAndRun(WorkerMethods.bwUpdateCountSeedersByAllCategories,
                        "Обновление кол-ва сидов на раздачах...", sender);
                    DwCreateAndRun(WorkerMethods.bwUpdateKeepersByAllCategories,
                        "Обновление данных о хранителях...", sender);
                }
                else if (sender == UpdateCountSeedersToolStripMenuItem)
                {
                    DwCreateAndRun(WorkerMethods.bwUpdateCountSeedersByAllCategories,
                        "Обновление кол-ва сидов на раздачах...", sender);
                }
                else if (sender == UpdateListTopicsToolStripMenuItem)
                {
                    DwCreateAndRun(WorkerMethods.bwUpdateTopicsByCategories,
                        "Полное обновление информации о топиках (раздачах) по всем категориям...",
                        ClientLocalDb.Current.GetCategoriesEnable());
                }
                else if (sender == UpdateKeepTopicsToolStripMenuItem)
                {
                    DwCreateAndRun(WorkerMethods.bwUpdateHashFromAllTorrentClients,
                        "Полное обновление информации из Torrent-клиентов...");
                }
                else if (sender == ClearDatabaseToolStripMenuItem)
                {
                    if (MessageBox.Show(
                            "Вы пытаетесь очистить базу данны от текущих данных (статистику и информацию о топиках).\r\n Продолжить?",
                            "Запрос подтверждения", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk,
                            MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                        return;
                    ClientLocalDb.Current.ClearDatabase();
                }
                else if (sender == ClearKeeperListsToolStripMenuItem)
                {
                    if (MessageBox.Show("Вы пытаетесь очистить базу данны от данных других хранителей.\r\n Продолжить?",
                            "Запрос подтверждения", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk,
                            MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                        return;
                    ClientLocalDb.Current.ClearKeepers();
                    SelectionChanged(_categorySource, null);
                }
                else if (sender == SendReportsToForumToolStripMenuItem)
                {
                    if (MessageBox.Show(
                            "Отправка отчетов на форум может продолжаться продолжительное время.\r\n Продолжить?",
                            "Запрос подтверждения", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk,
                            MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                        return;
                    DwCreateAndRun(WorkerMethods.bwSendReports,
                        "Отправка отчетов на форум...",
                        this);
                }
                else if (sender == CreateReportsToolStripMenuItem)
                {
                    if (MessageBox.Show(
                            "Сборка отчетов может продолжаться продолжительное время и потребуется обновить список раздач и информацию из торрент-клиентов.\r\n Продолжит?",
                            "Запрос подтверждения", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk,
                            MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                        return;
                    DwCreateAndRun(WorkerMethods.bwUpdateCountSeedersByAllCategories,
                        "Обновление кол-ва сидов на раздачах...", sender);
                    DwCreateAndRun(WorkerMethods.bwUpdateHashFromAllTorrentClients,
                        "Обновление информации из Torrent-клиентов...", sender);
                }
                else if (sender == RuningStopingDistributionToolStripMenuItem)
                {
                    DwCreateAndRun(WorkerMethods.bwUpdateCountSeedersByAllCategories,
                        "Обновление кол-ва сидов на раздачах...", sender);
                    DwCreateAndRun(WorkerMethods.bwRuningAndStopingDistributions,
                        "Обновление информации из Torrent-клиентов...", sender);
                    DwCreateAndRun(WorkerMethods.bwCreateReportsTorrentClients,
                        "Построение сводного отчета по торрент-клиентам...", sender);
                }
                else if (sender == CreateConsolidatedReportByTorrentClientsToolStripMenuItem)
                {
                    DwCreateAndRun(WorkerMethods.bwCreateReportsTorrentClients,
                        "Построение сводного отчета по торрент-клиентам...", sender);
                }
                else if (sender == LoadListKeepersToolStripMenuItem)
                {
                    DwCreateAndRun(WorkerMethods.bwUpdateKeepersByAllCategories,
                        "Обновление данных о хранителях...", sender);
                }
                else if (sender == ExitToolStripMenuItem)
                {
                    DoQuit(sender, e);
                }
                else if (sender == _btSaveToFile)
                {
                    SaveSetingsToFile();
                }
                else if (sender == _btLoadSettingsFromFile)
                {
                    ReadSettingsFromFile();
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }

            Cursor.Current = Cursors.Default;
        }

        private void tmr_Tick(object sender, EventArgs e)
        {
            if (_backgroundWorkers.Count > 0)
            {
                Text = $@"{_headText} (Выполняются задачи...)";
                TrayObject.TrayIcon.Text = $@"{_headText} (Выполняются задачи...)";
            }
            else
            {
                var lastRunTimer = _lastRunTimer;
                var now = DateTime.Now;
                var dateTime1 = now.AddMinutes(-Settings.Current.PeriodRunAndStopTorrents);
                var timeSpan = lastRunTimer - dateTime1;
                if (timeSpan.TotalSeconds > 0.0)
                {
                    Text = $"{_headText} ({timeSpan:hh\\:mm\\:ss})";
                    TrayObject.TrayIcon.BalloonTipText =
                        TrayObject.TrayIcon.Text = $"{_headText} ({timeSpan:hh\\:mm\\:ss})";
                    if (Properties.Settings.Default.NotificationInTray)
                    {
                        TrayObject.TrayIcon.ShowBalloonTip(7);
                    }
                }
                else
                {
                    try
                    {
                        var lastUpdateTopics = Settings.Current.LastUpdateTopics;
                        now = DateTime.Now;
                        var dateTime2 = now.AddDays(-1.0);
                        if (lastUpdateTopics < dateTime2)
                        {
                            DwCreateAndRun(WorkerMethods.bwUpdateTopicsByCategories,
                                "Полное обновление информации о топиках (раздачах) по всем категориям...",
                                ClientLocalDb.Current.GetCategoriesEnable());
                            DwCreateAndRun(WorkerMethods.bwUpdateKeepersByAllCategories,
                                "Обновление данных о хранителях...", sender);
                            var current = Settings.Current;
                            now = DateTime.Now;
                            var date = now.Date;
                            current.LastUpdateTopics = date;
                            Settings.Current.Save();
                        }
                        else
                        {
                            DwCreateAndRun(
                                WorkerMethods.bwUpdateCountSeedersByAllCategories,
                                "Обновление информации о кол-ве сидов на раздачах...", sender);
                        }

                        DwCreateAndRun(WorkerMethods.bwRuningAndStopingDistributions,
                            "Запуск/Остановка раздач в Torrent-клиентах...", sender);
                        DwCreateAndRun(WorkerMethods.bwCreateReportsTorrentClients,
                            "Построение сводного отчета по торрент-клиентам...", sender);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message);
                        _logger.Debug(ex, ex.Message);
                    }

                    _lastRunTimer = DateTime.Now;
                }
            }
        }

        private void MainForm_Disposed(object sender, EventArgs e)
        {
            _tmr.Stop();
            _tmr.Dispose();
        }

        private void DoQuit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SelectionChanged(object sender, EventArgs e)
        {
            if (sender == _categorySource || sender == _cbCountSeeders || sender == _cbBlackList ||
                sender == _cbCategoryFilters || sender == _DateRegistration)
            {
                _topicsSource.Clear();
                if (_categorySource.Current != null)
                {
                    var current = _categorySource.Current as Category;
                    var num = (int) _cbCountSeeders.Value;
                    var regTime = _DateRegistration.Value;
                    var isKeep = new bool?();
                    var isKeepers = new bool?();
                    var isDownload = new bool?();
                    var isBlack = new bool?();
                    var isPoster = new bool?();
                    var selectedItem = _cbCategoryFilters.SelectedItem as string;
                    if (!string.IsNullOrWhiteSpace(selectedItem))
                        switch (selectedItem)
                        {
                            case "Есть хранитель":
                                isKeepers = true;
                                break;
                            case "Не скачан торрент":
                                isDownload = false;
                                break;
                            case "Не скачан торрент и есть хранитель":
                                isDownload = false;
                                isKeepers = true;
                                break;
                            case "Не скачан торрент и нет хранителя":
                                isDownload = false;
                                isKeepers = false;
                                break;
                            case "Не скачано":
                                isDownload = false;
                                break;
                            case "Не храню":
                                isKeep = false;
                                break;
                            case "Скачиваю раздачу":
                                isDownload = true;
                                isKeep = false;
                                break;
                            case "Храню":
                                isKeep = true;
                                break;
                            case "Храню и есть хранитель":
                                isKeep = true;
                                isKeepers = true;
                                break;
                            case "Я релизер":
                                isPoster = false;
                                break;
                        }

                    List<TopicInfo> source;

                    if (current.CategoryID != -1)
                    {
                        isBlack = _cbBlackList.Checked;
                        var topicInfoList = new List<TopicInfo>();
                        source = !Settings.Current.IsAvgCountSeeders
                            ? ClientLocalDb.Current.GetTopics(regTime, current.CategoryID,
                                num > -1 ? num : new int?(), new int?(), isKeep, isKeepers, isDownload,
                                isBlack,
                                isPoster)
                            : ClientLocalDb.Current.GetTopics(regTime, current.CategoryID, new int?(),
                                num > -1 ? num : new int?(), isKeep, isKeepers, isDownload, isBlack,
                                isPoster);
                    }
                    else
                    {
                        var torrentClients = ClientLocalDb.Current.GetTorrentClients();
                        var inner = ClientLocalDb.Current.GetTopicsByCategory(-1)
                            .Where(x => !x.IsBlackList);
                        var dictionary = ClientLocalDb.Current.GetCategories()
                            .ToDictionary(x => x.CategoryID, x => x);
                        source = new List<TopicInfo>();
                        foreach (var torrentClientInfo in torrentClients)
                        {
                            var torrentClient = torrentClientInfo.Create();
                            if (torrentClient != null)
                            {
                                var array1 = torrentClient.GetAllTorrentHash().GroupJoin(inner, t => t.Hash,
                                    b => b.Hash, (t, bt) => new
                                    {
                                        t, bt
                                    }).SelectMany(param1 => param1.bt.DefaultIfEmpty(), (param1, b) =>
                                {
                                    var num3 = b != null ? b.CategoryID : -1;
                                    var size = param1.t.Size;
                                    var isRun = param1.t.IsRun;
                                    int num4;
                                    if (!isRun.HasValue)
                                    {
                                        num4 = -1;
                                    }
                                    else
                                    {
                                        isRun = param1.t.IsRun;
                                        num4 = isRun.Value ? 1 : 0;
                                    }

                                    var num5 = param1.t.IsPause ? 1 : 0;
                                    var num6 = b == null ? -1 : b.Seeders;
                                    TopicInfo a;
                                    if (b == null)
                                    {
                                        a = (TopicInfo) param1.t.Clone();
                                        a.CategoryID = num3;
                                        a.Name2 = param1.t.TorrentName;
                                        a.Size = size;
                                        a.IsRun = isRun;
                                        a.IsPause = num5 != 0;
                                        a.Seeders = num6;
                                        a.Label = param1.t.Label;
                                        return a;
                                    }

                                    a = b;

                                    return a;
                                    /*{
                                        CategoryID = num3,
                                        Name = _param1.t.TorrentName,
                                        Size = size,
                                        IsRun = num4,
                                        IsPause = num5 != 0,
                                        Seeders = num6,
                                        Label = _param1.t.Label
                                    };*/
                                });
                                source.AddRange(array1.Where(x => x.CategoryID == -1).ToArray());
                            }
                        }
                    }

                    _lbTotal.Text = string.Format("Кол-во: {0}; Размер: {1}", source.Count(),
                        TopicInfo.sizeToString(source.Sum(x => x.Size)));
                    _topicsSource.DataSource = source;
                }
            }

            if (sender != _categorySource || _categorySource.Current == null)
                return;
            tabReports.Controls.Clear();
            var reports =
                ClientLocalDb.Current.GetReports((_categorySource.Current as Category).CategoryID);
            if (reports.Count() > 0)
            {
                var size = tabReports.Size;
                var tabControl = new TabControl
                {
                    Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                    Location = new Point(0, 0),
                    SelectedIndex = 0,
                    Size = new Size(size.Width, size.Height)
                };
                tabReports.Controls.Add(tabControl);
                foreach (var keyValuePair in reports
                    .OrderBy(
                        x => x.Key.Item2))
                    if (!(keyValuePair.Value.Item2 == "Резерв") && !(keyValuePair.Value.Item2 == "Удалено"))
                    {
                        var tabPage = new TabPage();
                        var textBox = new TextBox();
                        tabPage.Location = new Point(4, 22);
                        tabPage.Padding = new Padding(3);
                        tabPage.Text = string.Format("Сидируемое: отчет № {0}", keyValuePair.Key.Item2);
                        if (keyValuePair.Key.Item2 == 0)
                            tabPage.Text = "Шапка сидируемого";
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
                        tabControl.Controls.Add(tabPage);
                        tabPage.Controls.Add(textBox);
                    }
            }
            else
            {
                var size = tabReports.Size;
                var tabControl = new TabControl();
                tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                tabControl.Location = new Point(0, 0);
                tabControl.SelectedIndex = 0;
                tabControl.Size = new Size(size.Width, size.Height);
                tabReports.Controls.Add(tabControl);
                var tabPage = new TabPage();
                var textBox = new TextBox();
                tabControl.Controls.Add(tabPage);
                tabPage.Controls.Add(textBox);
                tabPage.Location = new Point(4, 22);
                tabPage.Padding = new Padding(3);
                tabPage.Text = "Для информации";
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
            if (_backgroundWorkers.Count != 0 &&
                MessageBox.Show("Выполняются другие задачи. Добавить в очередь новое?", "Внимание",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) !=
                DialogResult.Yes)
                return;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (!(_categorySource.Current is Category current))
                    return;

                if (sender == _llUpdateCountSeedersByCategory)
                {
                    UpdaterMethods.UpdateSeedersByCategory(current);
                }
                else if (sender == _llUpdateTopicsByCategory)
                {
                    DwCreateAndRun(WorkerMethods.bwUpdateTopicsByCategory,
                        "Обновление списков по разделу...", current);
                }
                else if (sender == _llUpdateDataDromTorrentClient)
                {
                    UpdaterMethods.UpdateHashFromClients(current.TorrentClientUID);
                }
                else if (sender == _llDownloadSelectTopics)
                {
                    DwCreateAndRun(WorkerMethods.bwDownloadTorrentFiles,
                        "Скачиваются выделеные торрент-файлы в каталог...",
                        new Tuple<List<TopicInfo>, MainForm>(
                            (_topicsSource.DataSource as List<TopicInfo>)
                            .Where(x => x.Checked).ToList(), this));
                }
                else if (sender == _llSelectedTopicsToTorrentClient)
                {
                    DwCreateAndRun(WorkerMethods.bwSendTorrentFileToTorrentClient,
                        "Скачиваются и добавляются в торрент-клиент выделенные раздачи...",
                        new Tuple<MainForm, List<TopicInfo>, Category>(this,
                            (_topicsSource.DataSource as List<TopicInfo>)
                            .Where(x => x.Checked).ToList(), current));
                    DwCreateAndRun(
                        WorkerMethods.bwUpdateHashFromTorrentClientsByCategoryUID,
                        "Обновляем список раздач из торрент-клиента...", current);
                }
                else if (sender == _llSelectedTopicsToBlackList)
                {
                    var list = (_topicsSource.DataSource as List<TopicInfo>)
                        .Where(x => x.Checked).ToList();
                    list.ForEach(x => x.IsBlackList = true);
                    ClientLocalDb.Current.SaveTopicInfo(list);
                }
                else if (sender == _llSelectedTopicsDeleteFromBlackList)
                {
                    var list = (_topicsSource.DataSource as List<TopicInfo>)
                        .Where(x => x.Checked).ToList();
                    list.ForEach(x => x.IsBlackList = false);
                    ClientLocalDb.Current.SaveTopicInfo(list);
                }
                else if (sender == linkSetNewLabel)
                {
                    var getLabelName = new GetLabelName();
                    getLabelName.Value = string.IsNullOrWhiteSpace(current.Label) ? current.FullName : current.Label;
                    if (getLabelName.ShowDialog() == DialogResult.OK)
                        DwCreateAndRun(WorkerMethods.bwSetLabels,
                            "Установка пользовательских меток...",
                            new Tuple<MainForm, List<TopicInfo>, string>(this,
                                (_topicsSource.DataSource as List<TopicInfo>)
                                .Where(x => x.Checked).ToList(),
                                getLabelName.Value));
                }

                SelectionChanged(_categorySource, null);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                var num = (int) MessageBox.Show("Произошла ошибка:\r\n" + ex.Message, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }

            Cursor.Current = Cursors.Default;
        }

        private void ContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_dataGridTopicsList.Columns[e.ColumnIndex].DataPropertyName == "Name")
            {
                var nullable = _dataGridTopicsList.Rows[e.RowIndex].Cells[0].Value as int?;
                if (!nullable.HasValue)
                    return;
                Process.Start(string.Format("https://{1}/forum/viewtopic.php?t={0}", nullable.Value,
                    Settings.Current.HostRuTrackerOrg));
            }
            else
            {
                if (_dataGridTopicsList.Columns[e.ColumnIndex].DataPropertyName != "Alternative")
                    return;
                var topicId = _dataGridTopicsList.Rows[e.RowIndex].Cells[0].Value as int?;
                if (!topicId.HasValue)
                    return;
                if (!(_topicsSource.DataSource is List<TopicInfo> dataSource))
                    return;
                var topicInfo = dataSource.FirstOrDefault(x => x.TopicID == topicId.Value);
                if (topicInfo == null)
                    return;
                var num1 = topicInfo.Name.IndexOf('/');
                string str1;
                if (topicInfo.Name.IndexOf(']') > num1 && num1 != -1)
                    str1 = topicInfo.Name.Split('/').FirstOrDefault();
                else if (topicInfo.Name.IndexOf(']') < num1 && num1 != -1)
                    str1 = topicInfo.Name.Split('/').FirstOrDefault().Split(']')[1];
                else if (num1 == -1 && topicInfo.Name.IndexOf('[') < 5 && topicInfo.Name.IndexOf('[') != -1)
                    str1 = topicInfo.Name.Split(']')[1].Split('[').FirstOrDefault();
                else if (num1 == -1 && topicInfo.Name.IndexOf('[') != -1)
                    str1 = topicInfo.Name.Split('[').FirstOrDefault();
                else
                    str1 = topicInfo.Name.Split('[').FirstOrDefault();
                var num2 = topicInfo.Name.IndexOf('[', num1 > -1 ? num1 : 0);
                if (num2 < 5)
                {
                    var startIndex = topicInfo.Name.IndexOf(']') + 1;
                    num2 = topicInfo.Name.IndexOf('[', startIndex);
                }

                var str2 = topicInfo.Name.Substring(num2 == -1 ? 0 : num2 + 1);
                if (!string.IsNullOrWhiteSpace(str2))
                    str2 = str2.Split(new char[3]
                    {
                        ',',
                        ' ',
                        ']'
                    }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(str2))
                    str1 = str1 + " " + str2;
                Process.Start(string.Format(
                    "https://{2}/forum/tracker.php?f={0}&nm={1}",
                    topicInfo.CategoryID, str1, Settings.Current.HostRuTrackerOrg));
            }
        }

        private void DwCreateAndRun(DoWorkEventHandler e, string comment = "...", object argument = null)
        {
            var key = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            key.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            key.ProgressChanged += backgroundWorker1_ProgressChanged;
            key.DoWork += e;
            _backgroundWorkers.Add(key, new Tuple<DateTime, object, string>(DateTime.Now, argument, comment));
            if (_backgroundWorkers.Count != 1)
                return;
            key.RunWorkerAsync(argument);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripProgressBar1.Value = 0;
            toolStripProgressBar1.Visible = false;
            toolStripStatusLabel1.Text = "";
            toolStripStatusLabel1.Visible = false;
            statusStrip1.Refresh();
            if (sender != null && sender is BackgroundWorker &&
                _backgroundWorkers.ContainsKey(sender as BackgroundWorker))
            {
                var key = sender as BackgroundWorker;
                if (_backgroundWorkers.ContainsKey(key))
                    _backgroundWorkers.Remove(key);
                key.Dispose();
            }

            if (e.Result != null)
                _logger.Info(e.Result);
            if (_backgroundWorkers.Count > 0)
            {
                // запуск следующей задачи.
                var keyValuePair = _backgroundWorkers.OrderBy(x => x.Value.Item1).First();
                keyValuePair.Key.RunWorkerAsync(keyValuePair.Value.Item2);
            }
            else
            {
                // записываем окончательные изменения в БД после выполнения последней задачи.
                SelectionChanged(_categorySource, null);
                WriteReports();
                ClientLocalDb.Current.SaveToDatabase();
            }

            GC.Collect();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (sender != null && sender is BackgroundWorker &&
                _backgroundWorkers.ContainsKey(sender as BackgroundWorker))
            {
                toolStripStatusLabel1.Text = _backgroundWorkers[sender as BackgroundWorker].Item3;
                toolStripProgressBar1.Visible = true;
                toolStripStatusLabel1.Visible = true;
                statusStrip1.Refresh();
            }

            toolStripProgressBar1.Value =
                e.ProgressPercentage < 0 || e.ProgressPercentage > 100 ? 100 : e.ProgressPercentage;
        }

        private void _dgvReportDownloads_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var column = _dataGridTopicsList.Columns[e.ColumnIndex];
            if (column == ColumnReport1DgvSelect)
            {
                var dataSource = _topicsSource.DataSource as List<TopicInfo>;
                if (dataSource == null)
                    return;
                var list = dataSource.ToList();
                list.ForEach(x =>
                {
                    var topicInfo = x;
                    topicInfo.Checked = !topicInfo.Checked;
                });
                _topicsSource.Clear();
                _topicsSource.DataSource = list;
            }
            else
            {
                var sortedColumn = _dataGridTopicsList.SortedColumn;
                var sortOrder =
                    column.HeaderCell.SortGlyphDirection == SortOrder.None ||
                    column.HeaderCell.SortGlyphDirection == SortOrder.Descending
                        ? SortOrder.Ascending
                        : SortOrder.Descending;
                if (column == null)
                {
                    var num = (int) MessageBox.Show("Select a single column and try again.", "Error: Invalid Selection",
                        MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    var dataSource = _topicsSource.DataSource as List<TopicInfo>;
                    if (dataSource == null)
                        return;
                    var list1 = dataSource.ToList();
                    List<TopicInfo> list2;
                    if (column == ColumnReport1DgvSize)
                    {
                        list2 = (sortOrder == SortOrder.Ascending
                            ? list1.OrderBy(
                                d => d.Size)
                            : list1.OrderByDescending(
                                d => d.Size)).ToList();
                    }
                    else if (column == ColumnReport1DgvName)
                    {
                        list2 = (sortOrder == SortOrder.Ascending
                            ? list1.OrderBy(
                                d => d.Name)
                            : list1.OrderByDescending(
                                d => d.Name)).ToList();
                    }
                    else if (column == ColumnReport1DgvSeeders)
                    {
                        list2 = (sortOrder == SortOrder.Ascending
                                ? list1
                                    .OrderBy(d => d.Seeders)
                                    .ThenBy(d => Name)
                                : list1
                                    .OrderByDescending(d => d.Seeders)
                                    .ThenBy(d => Name))
                            .ToList();
                    }
                    else if (column == ColumnReport1DgvAvgSeeders)
                    {
                        list2 = (sortOrder == SortOrder.Ascending
                                ? list1
                                    .OrderBy(d => d.AvgSeeders)
                                    .ThenBy(d => Name)
                                : list1
                                    .OrderByDescending(
                                        d => d.AvgSeeders)
                                    .ThenBy(d => Name))
                            .ToList();
                    }
                    else if (column == ColumnReport1DgvRegTime)
                    {
                        list2 = (sortOrder == SortOrder.Ascending
                                ? list1
                                    .OrderBy(d => d.RegTime)
                                    .ThenBy(d => Name)
                                : list1
                                    .OrderByDescending(
                                        d => d.RegTime)
                                    .ThenBy(d => Name))
                            .ToList();
                    }
                    else if (column == ColumnReport1DgvKeeperCount)
                    {
                        list2 = (sortOrder == SortOrder.Ascending
                                ? list1
                                    .OrderBy(d => d.KeeperCount)
                                    .ThenBy(d => Name)
                                : list1
                                    .OrderByDescending(d => d.KeeperCount)
                                    .ThenBy(d => Name))
                            .ToList();
                    }
                    else
                    {
                        if (column != ColumnReport1DgvStatus)
                            return;
                        list2 = (sortOrder == SortOrder.Ascending
                                ? list1
                                    .OrderBy(d => d.StatusToString)
                                    .ThenBy(d => Name)
                                : list1
                                    .OrderByDescending(
                                        d => d.StatusToString)
                                    .ThenBy(d => Name))
                            .ToList();
                    }

                    _topicsSource.Clear();
                    _topicsSource.DataSource = list2;
                    column.HeaderCell.SortGlyphDirection = sortOrder;
                }
            }
        }

        private void _dgvReportDownloads_Click(object sender, EventArgs e)
        {
            if (_dataGridTopicsList.Columns.GetColumnCount(DataGridViewElementStates.Selected) == 1)
            {
                var selectedColumn = _dataGridTopicsList.SelectedColumns[0];
            }

            Console.WriteLine("");
        }

        private void WriteReports()
        {
            Reports.CreateReportByRootCategories();
            _tcCetegoriesRootReports.Controls.Clear();
            var reports = ClientLocalDb.Current.GetReports(0);
            var str1 = reports
                .Where(
                    x => x.Key.Item2 == 0)
                .Select(
                    x => x.Value.Item2)
                .FirstOrDefault();
            _txtConsolidatedReport.Text = string.IsNullOrWhiteSpace(str1) ? string.Empty : str1;
            var str2 = reports
                .Where(
                    x => x.Key.Item2 == 1)
                .Select(
                    x => x.Value.Item2)
                .FirstOrDefault();
            _tbConsolidatedTorrentClientsReport.Text = string.IsNullOrWhiteSpace(str2) ? string.Empty : str2;
            var categories = ClientLocalDb.Current.GetCategories()
                .Where(x => x.CategoryID > 100000);
            var size = _tcCetegoriesRootReports.Size;
            foreach (var category in categories)
            {
                var str3 = ClientLocalDb.Current.GetReports(category.CategoryID)
                    .Where(
                        x => x.Key.Item2 == 0)
                    .Select(
                        x => x.Value.Item2)
                    .FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(str3))
                {
                    var tabPage = new TabPage();
                    var textBox = new TextBox();
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
                    _tcCetegoriesRootReports.Controls.Add(tabPage);
                    tabPage.Controls.Add(textBox);
                }
            }
        }

        private void SaveSetingsToFile()
        {
            try
            {
                var empty = string.Empty;
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.DefaultExt = "tloback";
                saveFileDialog.Filter = "Файл архивных настроек|*.tloback";
                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                var fileName = saveFileDialog.FileName;
                if (string.IsNullOrWhiteSpace(fileName))
                    return;
                using (var fileStream = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (var binaryWriter = new BinaryWriter(fileStream, Encoding.UTF8))
                    {
                        foreach (var torrentClient in ClientLocalDb.Current.GetTorrentClients())
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

                        foreach (var category in ClientLocalDb.Current.GetCategoriesEnable())
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

                        var cats = ClientLocalDb.Current.GetCategoriesEnable()
                            .Select(x => x.CategoryID).ToArray();
                        var reports =
                            ClientLocalDb.Current.GetReports(new int?());
                        foreach (var keyValuePair in reports
                            .Where(
                                x =>
                                    cats.Contains(x.Key.Item1)))
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
                _logger.Error(ex.Message);
                _logger.Trace(ex.StackTrace);
            }
        }

        private void ReadSettingsFromFile()
        {
            try
            {
                var empty = string.Empty;
                var openFileDialog = new OpenFileDialog();
                openFileDialog.DefaultExt = "tloback";
                openFileDialog.Filter = "Файл архивных настроек|*.tloback";
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;
                var fileName = openFileDialog.FileName;
                if (string.IsNullOrWhiteSpace(fileName))
                    return;
                var torrentClientInfoList = new List<TorrentClientInfo>();
                var categoryList = new List<Category>();
                var result = new List<Tuple<int, int, string>>();
                using (var fileStream = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    using (var binaryReader = new BinaryReader(fileStream))
                    {
                        while (binaryReader.BaseStream.Length != binaryReader.BaseStream.Position)
                        {
                            var str = binaryReader.ReadString();
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
                                    var category = new Category
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
                                var torrentClientInfo = new TorrentClientInfo
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

                        ClientLocalDb.Current.SaveTorrentClients(torrentClientInfoList);
                        ClientLocalDb.Current.CategoriesSave(categoryList);
                        ClientLocalDb.Current.SaveSettingsReport(result);
                        ClientLocalDb.Current.SaveToDatabase();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Trace(ex.StackTrace);
            }
        }

        private void FireFormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void ExportUnknown_Click(object sender, EventArgs e)
        {
            DwCreateAndRun(
                WorkerMethods.bwCreateUnknownTorrentsReport,
                "Формирование отчета",
                this
            );
        }
    }
}