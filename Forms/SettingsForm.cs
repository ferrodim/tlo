using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using NLog;
using TLO.Clients;
using TLO.Info;

namespace TLO.Forms
{
    internal sealed partial class SettingsForm : Form
    {
        private readonly BindingSource _categoriesSource;
        private readonly BindingSource _torrentClientsSource;
        private BindingSource _proxyListSource;

        public SettingsForm()
        {
            InitializeComponent();
            var current = Settings.Current;
            _tbTorrentClientName.Enabled = false;
            _cbTorrentClientType.Enabled = false;
            _tbTorrentClientHostIP.Enabled = false;
            _tbTorrentClientPort.Enabled = false;
            _tbTorrentClientUserName.Enabled = false;
            _tbTorrentClientUserPassword.Enabled = false;
            dgwTorrentClients.AutoGenerateColumns = false;
            dgwTorrentClients.ClearSelection();
            dgwTorrentClients.DataSource = null;
            _torrentClientsSource = new BindingSource {DataSource = ClientLocalDb.Current.GetTorrentClients()};
            dgwTorrentClients.DataSource = _torrentClientsSource;
            dgwCategories.AutoGenerateColumns = false;
            dgwCategories.ClearSelection();
            dgwCategories.DataSource = null;
            _categoriesSource = new BindingSource {DataSource = ClientLocalDb.Current.GetCategoriesEnable()};
            dgwCategories.DataSource = _categoriesSource;
            if (_categoriesSource.Count > 0) _categoriesSource.Position = 0;
            if (_torrentClientsSource.Count > 0) _torrentClientsSource.Position = 0;
            forumPages1.LoadSettings();
            CreatePageAllCategories();
            _appKeeperName.Text = current.KeeperName;
            _appKeeperPass.Text = current.KeeperPass;
            _appIsUpdateStatistics.Checked = current.IsUpdateStatistics;
            _appCountDaysKeepHistory.Value = current.CountDaysKeepHistory;
            _appPeriodRunAndStopTorrents.Value = current.PeriodRunAndStopTorrents;
            _appCountSeedersReport.Value = current.CountSeedersReport;
            _appIsAvgCountSeeders.Checked = current.IsAvgCountSeeders;
            _appSelectLessOrEqual.Checked = current.IsSelectLessOrEqual;
            {
                CheckState checkState;
                if (Settings.Current.LoadDBInMemory == null)
                    checkState = CheckState.Indeterminate;
                else if (current.LoadDBInMemory != null && (bool) current.LoadDBInMemory)
                    checkState = CheckState.Checked;
                else
                    checkState = CheckState.Unchecked;

                _dbLoadInMemoryCheckbox.AutoSize = true;
                _dbLoadInMemoryCheckbox.Checked = current.LoadDBInMemory.GetValueOrDefault(false);
                _dbLoadInMemoryCheckbox.CheckState = checkState;
            }
            DisableCertVerifyCheck.Checked = current.DisableServerCertVerify.GetValueOrDefault(false);
            DisableCertVerifyCheck.CheckState = current.DisableServerCertVerify.GetValueOrDefault(false)
                ? CheckState.Checked
                : CheckState.Unchecked;
            if (current.ApiHost != "")
                foreach (string item in apiHosts.Items)
                    if (item == current.ApiHost)
                        apiHosts.SelectedItem = item;
            rutrackerHost.Text = current.HostRuTrackerOrg;

            var appLogLevel = _appLogLevel;
            var logLevel = current.LogLevel;
            int num1;
            if (!logLevel.HasValue)
            {
                num1 = 0;
            }
            else
            {
                logLevel = current.LogLevel;
                num1 = logLevel.Value;
            }

            decimal num2 = num1;
            appLogLevel.Value = num2;
            _appIsNotSaveStatistics.Checked = current.IsNotSaveStatistics;
            _appReportTop1.Text = current.ReportTop1;
            _appReportTop2.Text = current.ReportTop2;
            _appReportLine.Text = current.ReportLine;
            _appReportBottom.Text = current.ReportBottom;
            summaryReportTemplate.Text = current.ReportSummaryTemplate;
            categoryReportTemplate.Text = current.ReportCategoriesTemplate;
            reportHeaderTemplate.Text = current.ReportCategoryHeaderTemplate;
            _proxyListSource = new BindingSource {DataSource = current.ProxyList};
            ProxyListBox.DataSource = _proxyListSource;
            ProxyListBox.SelectedItem = current.SelectedProxy;
            if (current.UseProxy == true)
            {
                useProxyCheckBox.CheckState = CheckState.Checked;
                if (current.SystemProxy == true)
                {
                    SystemProxy.CheckState = CheckState.Checked;
                }
            }
            else
            {
                useProxyCheckBox.CheckState = CheckState.Unchecked;
            }

            ProxyAddButton.Click += (sender, args) => { _proxyListSource.Add(proxyInput.Text); };
            useProxyCheckBox.CheckStateChanged += (sender, args) => ProxySettingsSync();
            SystemProxy.CheckStateChanged += (sender, args) => ProxySettingsSync();
            ProxyListBox.SelectedValueChanged += (sender, args) => ProxySettingsSync();
            ProxySettingsSync();
        }

        public new Point Location
        {
            get => base.Location;
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

        private void _Focus_Enter(object sender, EventArgs e)
        {
            if (_torrentClientsSource.Current != null)
            {
                var current = _torrentClientsSource.Current as TorrentClientInfo;
                if (sender == _tbTorrentClientName)
                {
                    current.Name = _tbTorrentClientName.Text;
                }
                else if (sender == _cbTorrentClientType)
                {
                    current.Type = _cbTorrentClientType.Text;
                }
                else if (sender == _tbTorrentClientHostIP)
                {
                    current.ServerName = _tbTorrentClientHostIP.Text;
                }
                else if (sender == _tbTorrentClientPort)
                {
                    var result = 0;
                    if (int.TryParse(_tbTorrentClientPort.Text, out result))
                        current.ServerPort = result;
                    else
                        _tbTorrentClientPort.Text = "0";
                }
                else if (sender == _tbTorrentClientUserName)
                {
                    current.UserName = _tbTorrentClientUserName.Text;
                }
                else if (sender == _tbTorrentClientUserPassword)
                {
                    current.UserPassword = _tbTorrentClientUserPassword.Text;
                }
                else if (sender == _tcrbCurrent && _tcrbCurrent.Checked)
                {
                    current.ServerName = "127.0.0.1";
                    _tbTorrentClientHostIP.Enabled = false;
                }
                else if (sender == _tcrbRemote && _tcrbRemote.Checked)
                {
                    current.ServerName = _tbTorrentClientHostIP.Text;
                    _tbTorrentClientHostIP.Enabled = true;
                }
            }

            if (_categoriesSource.Current == null)
                return;
            var current1 = _categoriesSource.Current as Category;
            if (sender == _CategoriesCbTorrentClient)
            {
                var selectedItem = _CategoriesCbTorrentClient.SelectedItem as TorrentClientInfo;
                if (selectedItem == null)
                    return;
                current1.TorrentClientUID = selectedItem.UID;
            }
            else if (sender == _CategoriesCbStartCountSeeders)
            {
                var result = 0;
                if (!int.TryParse(_CategoriesCbStartCountSeeders.SelectedItem as string, out result))
                    return;
                current1.CountSeeders = result;
            }
            else if (sender == _CategoriesTbFolderDownloads)
            {
                current1.Folder = _CategoriesTbFolderDownloads.Text;
            }
            else if (sender == _cbIsSaveTorrentFile)
            {
                current1.IsSaveTorrentFiles = _cbIsSaveTorrentFile.Checked;
            }
            else if (sender == _cbIsSaveWebPage)
            {
                current1.IsSaveWebPage = _cbIsSaveWebPage.Checked;
            }
            else if (sender == _cbSubFolder)
            {
                var selectedItem = _cbSubFolder.SelectedItem as string;
                if (string.IsNullOrWhiteSpace(selectedItem))
                    return;
                if (selectedItem != "Не нужен")
                {
                    if (selectedItem != "С ID топика")
                    {
                        if (selectedItem != "Запрашивать")
                            return;
                        current1.CreateSubFolder = 2;
                    }
                    else
                    {
                        current1.CreateSubFolder = 1;
                    }
                }
                else
                {
                    current1.CreateSubFolder = 0;
                }
            }
            else
            {
                if (sender != _CategoriesTbLabel)
                    return;
                current1.Label = string.IsNullOrWhiteSpace(_CategoriesTbLabel.Text)
                    ? current1.FullName
                    : _CategoriesTbLabel.Text.Trim();
            }
        }

        private void SelectionChanged(object sender, EventArgs e)
        {
            if (sender == dgwTorrentClients)
            {
                if (_torrentClientsSource.Current == null)
                {
                    _tbTorrentClientName.Enabled = false;
                    _cbTorrentClientType.Enabled = false;
                    _tbTorrentClientHostIP.Enabled = false;
                    _tbTorrentClientPort.Enabled = false;
                    _tbTorrentClientUserName.Enabled = false;
                    _tbTorrentClientUserPassword.Enabled = false;
                    _tbTorrentClientName.Text = string.Empty;
                    _cbTorrentClientType.Text = string.Empty;
                    _tbTorrentClientHostIP.Text = string.Empty;
                    _tbTorrentClientPort.Text = string.Empty;
                    _tbTorrentClientUserName.Text = string.Empty;
                    _tbTorrentClientUserPassword.Text = string.Empty;
                    _tcrbRemote.Checked = false;
                    _tcrbCurrent.Checked = true;
                    _tbTorrentClientHostIP.Enabled = false;
                }
                else
                {
                    var current = _torrentClientsSource.Current as TorrentClientInfo;
                    _tbTorrentClientName.Enabled = true;
                    _cbTorrentClientType.Enabled = true;
                    _tbTorrentClientHostIP.Enabled = true;
                    _tbTorrentClientPort.Enabled = true;
                    _tbTorrentClientUserName.Enabled = true;
                    _tbTorrentClientUserPassword.Enabled = true;
                    _tbTorrentClientName.Text = current.Name;
                    _cbTorrentClientType.Text = current.Type;
                    if (current.ServerName == "127.0.0.1")
                    {
                        _tcrbRemote.Checked = false;
                        _tcrbCurrent.Checked = true;
                    }
                    else
                    {
                        _tbTorrentClientHostIP.Text = current.ServerName;
                        _tcrbCurrent.Checked = false;
                        _tcrbRemote.Checked = true;
                    }

                    _tbTorrentClientPort.Text = current.ServerPort.ToString();
                    _tbTorrentClientUserName.Text = current.UserName;
                    _tbTorrentClientUserPassword.Text = current.UserPassword;
                }
            }

            if (sender == dgwCategories)
            {
                if (_categoriesSource.Current == null)
                {
                    _CategoriesTbCategoryID.Text = string.Empty;
                    _CategoriesTbFullName.Text = string.Empty;
                    _CategoriesCbStartCountSeeders.Enabled = false;
                    _CategoriesTbLabel.Text = string.Empty;
                }
                else
                {
                    var obj = _categoriesSource.Current as Category;
                    _CategoriesTbCategoryID.Text = obj.CategoryID.ToString();
                    _CategoriesTbFullName.Text = obj.FullName;
                    _CategoriesCbStartCountSeeders.Enabled = true;
                    var startCountSeeders = _CategoriesCbStartCountSeeders;
                    int num;
                    string str;
                    if (obj.CountSeeders < 0)
                    {
                        str = "-";
                    }
                    else
                    {
                        num = obj.CountSeeders;
                        str = num.ToString();
                    }

                    startCountSeeders.SelectedItem = str;
                    _CategoriesTbFolderDownloads.Text = obj.Folder;
                    _CategoriesCbTorrentClient.DataSource = null;
                    _CategoriesCbTorrentClient.DataSource = _torrentClientsSource.DataSource;
                    _CategoriesCbTorrentClient.SelectedItem =
                        (_CategoriesCbTorrentClient.DataSource as List<TorrentClientInfo>)
                        .Where(x => x.UID == obj.TorrentClientUID)
                        .FirstOrDefault();
                    num = obj.CreateSubFolder;
                    switch (num)
                    {
                        case 0:
                            _cbSubFolder.SelectedItem = "Не нужен";
                            break;
                        case 1:
                            _cbSubFolder.SelectedItem = "С ID топика";
                            break;
                        case 2:
                            _cbSubFolder.SelectedItem = "Запрашивать";
                            break;
                    }

                    _cbIsSaveWebPage.Checked = obj.IsSaveWebPage;
                    _cbIsSaveTorrentFile.Checked = obj.IsSaveTorrentFiles;
                    _CategoriesTbLabel.Text = string.IsNullOrWhiteSpace(obj.Label) ? obj.FullName : obj.Label;
                }
            }

            if (sender != _appIsNotSaveStatistics)
                return;
            if (_appIsNotSaveStatistics.Checked)
            {
                _appIsUpdateStatistics.Checked = false;
                _appIsUpdateStatistics.Enabled = false;
            }
            else
            {
                _appIsUpdateStatistics.Enabled = true;
            }
        }

        private void ClickButtons(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (sender == _btTorrentClientAdd)
            {
                _torrentClientsSource.Add(new TorrentClientInfo());
                _torrentClientsSource.Position = _torrentClientsSource.Count;
            }
            else if (sender == _btTorrentClientDelete)
            {
                if (_torrentClientsSource.Current == null)
                    return;
                var current = _torrentClientsSource.Current as TorrentClientInfo;
                if (MessageBox.Show("Вы хотите удалить из списка torrent-клиент \"" + current.Name + "\"?",
                        "Запрос подтверждения", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) ==
                    DialogResult.Yes)
                    _torrentClientsSource.Remove(current);
            }

            if (sender == _btCategoryAdd)
            {
                var dialog = new SelectCategory();
                dialog.Read();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (dialog.SelectedCategories.Count() > 0)
                    {
                        dialog.SelectedCategories.ForEach(x =>
                        {
                            x.IsEnable = true;
                            _categoriesSource.Add(x);
                        });
                        _categoriesSource.Position = _categoriesSource.Count;
                    }

                    if (dialog.SelectedCategory == null)
                        return;
                    if ((_categoriesSource.DataSource as List<Category>).Any(
                        x => x.CategoryID == dialog.SelectedCategory.CategoryID))
                    {
                        var num = (int) MessageBox.Show("Выбранная категория уже присутствует");
                    }
                    else
                    {
                        dialog.SelectedCategory.IsEnable = true;
                        _categoriesSource.Add(dialog.SelectedCategory);
                        _categoriesSource.Position = _categoriesSource.Count;
                    }
                }
            }
            else if (sender == _btCategoryRemove)
            {
                if (_categoriesSource.Current == null)
                    return;
                var current = _categoriesSource.Current as Category;
                if (MessageBox.Show("Удалить из обработки раздел \"" + current.Name + "\"?", "Подтверждение",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                    _categoriesSource.Remove(current);
            }
            else if (sender == _CategoriesBtSelectFolder)
            {
                if (_categoriesSource.Current == null)
                    return;
                var current = _categoriesSource.Current as Category;
                var folderBrowserDialog = new FolderBrowserDialog();
                folderBrowserDialog.SelectedPath =
                    string.IsNullOrWhiteSpace(current.Folder) ? "c:\\" : current.Folder;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    current.Folder = folderBrowserDialog.SelectedPath;
                    _CategoriesTbFolderDownloads.Text = current.Folder;
                }
            }


            if (sender == _btSave)
            {
                ClientLocalDb.Current.SaveTorrentClients(
                    _torrentClientsSource.DataSource as List<TorrentClientInfo>, true);
                ClientLocalDb.Current.CategoriesSave(
                    _categoriesSource.DataSource as List<Category>);
                forumPages1.Save();
                DialogResult = DialogResult.OK;
                var current = setSettings();
                current.Save();
                ClientLocalDb.Current.Reconnect();
                Close();
            }
            else if (sender == _btCancel)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
            else
            {
                if (_btCheck != sender)
                    return;
                var stringList = new List<string>();
                foreach (var torrentClientInfo in _torrentClientsSource.DataSource as List<TorrentClientInfo>)
                    try
                    {
                        var torrentClient = torrentClientInfo.Create();
                        if (torrentClient == null)
                            stringList.Add(
                                $"Торрент-клиент \"{torrentClientInfo.Name}\": Не удалось определить тип torrent-клиента");
                        else
                            torrentClient.Ping();
                    }
                    catch
                    {
                        stringList.Add($"Не удалось подключиться к торрент-клиенту \"{torrentClientInfo.Name}\"");
                    }

                foreach (var text in stringList) MessageBox.Show(text, "Проверка");

                MessageBox.Show("Подключение к torrent-клиентам проверено.", "Проверка");
            }
        }

        private Settings setSettings()
        {
            var current = Settings.Current;
            current.KeeperName = _appKeeperName.Text;
            current.KeeperPass = _appKeeperPass.Text;
            current.IsUpdateStatistics = _appIsUpdateStatistics.Checked;
            current.CountDaysKeepHistory = (int) _appCountDaysKeepHistory.Value;
            current.PeriodRunAndStopTorrents = (int) _appPeriodRunAndStopTorrents.Value;
            current.CountSeedersReport = (int) _appCountSeedersReport.Value;
            current.IsAvgCountSeeders = _appIsAvgCountSeeders.Checked;
            current.IsSelectLessOrEqual = _appSelectLessOrEqual.Checked;
            current.LogLevel = (int) _appLogLevel.Value;
            current.IsNotSaveStatistics = _appIsNotSaveStatistics.Checked;
            current.ReportTop1 = _appReportTop1.Text.Replace("\n", "\r\n").Replace("\r\r", "\r");
            current.ReportTop2 = _appReportTop2.Text.Replace("\n", "\r\n").Replace("\r\r", "\r");
            current.ReportLine = _appReportLine.Text.Replace("\n", "\r\n").Replace("\r\r", "\r");
            current.ReportBottom = _appReportBottom.Text.Replace("\n", "\r\n").Replace("\r\r", "\r");
            current.ReportCategoryHeaderTemplate =
                reportHeaderTemplate.Text.Replace("\n", "\r\n").Replace("\r\r", "\r");
            current.ReportCategoriesTemplate =
                categoryReportTemplate.Text.Replace("\n", "\r\n").Replace("\r\r", "\r");
            current.ReportSummaryTemplate =
                summaryReportTemplate.Text.Replace("\n", "\r\n").Replace("\r\r", "\r");
            if (_dbLoadInMemoryCheckbox.CheckState != CheckState.Indeterminate)
                current.LoadDBInMemory = _dbLoadInMemoryCheckbox.Checked;

            current.UseProxy = useProxyCheckBox.Checked;
            current.SystemProxy = SystemProxy.Checked;
            current.SelectedProxy = ProxyListBox.SelectedItem?.ToString();
            current.ProxyList.Clear();
            foreach (var item in ProxyListBox.Items)
            {
                current.ProxyList.Add((string) item);
            }

            current.DisableServerCertVerify = DisableCertVerifyCheck.Checked;
            current.ApiHost = apiHosts.SelectedItem?.ToString();
            current.HostRuTrackerOrg = rutrackerHost.Text;
            return current;
        }

        private void CreatePageAllCategories()
        {
            Control control = panel2;
            var dictionary1 = ClientLocalDb.Current.GetCategories().ToDictionary(x => x.CategoryID, x => x);
            var categoriesEnable = ClientLocalDb.Current.GetCategoriesEnable().ToDictionary(x => x.CategoryID, x => x);
            categoriesEnable.ToDictionary(x => x.Key, x => x.Value.ParentID);
            for (var index = 0; index < 3; ++index)
                foreach (var category in categoriesEnable.Values.ToArray())
                    if (!categoriesEnable.ContainsKey(category.ParentID) && dictionary1.ContainsKey(category.ParentID))
                        categoriesEnable.Add(dictionary1[category.ParentID].CategoryID, dictionary1[category.ParentID]);

            for (var index = 0; index < 3; ++index)
            {
                var list = dictionary1.Values.ToList();
                foreach (var category1 in categoriesEnable.Values.ToList())
                {
                    var c = category1;
                    foreach (var category2 in list
                        .Where(x => !categoriesEnable.ContainsKey(x.CategoryID) && x.ParentID == c.CategoryID)
                        .ToArray())
                        if (!categoriesEnable.ContainsKey(category2.CategoryID) &&
                            dictionary1.ContainsKey(category2.CategoryID))
                            categoriesEnable.Add(category2.CategoryID, category2);
                }
            }

            var dictionary2 = ClientLocalDb.Current.GetReports(new int?())
                .Where(x => x.Key.Item2 == 0 && (uint) x.Key.Item1 > 0U)
                .ToDictionary(x => x.Key.Item1, x => x.Value.Item1);
            var num = 0;
            var y1 = 10;
            foreach (var category in categoriesEnable.Values.OrderBy(x => x.FullName))
            {
                var label1Local = new Label();
                label1Local.AutoSize = true;
                label1Local.Location = new Point(3, y1);
                label1Local.Size = new Size(35, 13);
                label1Local.TabIndex = num;
                label1Local.Text = category.FullName;
                control.Controls.Add(label1Local);
                var y2 = y1 + 16;
                var label2Local = new Label();
                label2Local.Location = new Point(6, y2);
                label2Local.Size = new Size(123, 20);
                label2Local.Text = "Списки хранимого";
                control.Controls.Add(label2Local);
                ++num;
                var textBox = new TextBox();
                textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                textBox.Location = new Point(135, y2);
                textBox.Size = new Size(panel1.Size.Width - 135, 20);
                textBox.TabIndex = num;
                textBox.Text = !string.IsNullOrWhiteSpace(category.ReportList) ||
                               !dictionary2.ContainsKey(category.CategoryID)
                    ? category.ReportList
                    : dictionary2[category.CategoryID];
                control.Controls.Add(textBox);
                y1 = y2 + 26;
            }
        }

        private void FormLoaded(object sender, EventArgs eventArgs)
        {
            DataBindings.Add(new Binding("Location", Properties.Settings.Default,
                "SettingsWindowLocation", true, DataSourceUpdateMode.OnPropertyChanged));
            DataBindings.Add(new Binding("Size", Properties.Settings.Default,
                "SettingsWindowSize", true, DataSourceUpdateMode.OnPropertyChanged));
        }

        private void ProxySettingsSync()
        {
            setSettings();
            if (useProxyCheckBox.CheckState == CheckState.Checked)
            {
                SystemProxy.Enabled = true;
                if (SystemProxy.CheckState != CheckState.Checked)
                {
                    ProxyListBox.Enabled = true;
                    proxyInput.Enabled = true;
                    ProxyAddButton.Enabled = true;
                }
                else
                {
                    ProxyListBox.Enabled = false;
                    proxyInput.Enabled = false;
                    ProxyAddButton.Enabled = false;
                }
            }
            else
            {
                SystemProxy.Enabled = false;
                ProxyListBox.Enabled = false;
                proxyInput.Enabled = false;
                ProxyAddButton.Enabled = false;
            }

            connectionCheck.Text = "Состояние: ПРОВЕРЯЕМ...";
            connectionCheck.BackColor = Color.Orange;
            new Thread(o =>
            {
                try
                {
                    var page = new TloWebClient(true)
                        .DownloadString(string.Format(
                        "https://{1}/forum/profile.php?mode=viewprofile&u={0}",
                        Settings.Current.KeeperName,
                        Settings.Current.HostRuTrackerOrg
                    )).Replace("<wbr>", "");
                    if (!page.Contains(Settings.Current.KeeperName))
                    {
                        connectionCheck.Text = "Состояние: ПЛОХОЙ ОТВЕТ";
                        connectionCheck.BackColor = Color.Red;
                    }
                    else
                    {
                        connectionCheck.Text = "Состояние: РАБОТАЕТ";
                        connectionCheck.BackColor = Color.Green;
                    }
                }
                catch (Exception e)
                {
                    LogManager.GetLogger("ConnectionCheck").Trace(e.Message);
                    if (e.InnerException != null)
                        LogManager.GetLogger("ConnectionCheck").Trace(e.InnerException.Message);
                    connectionCheck.Text = "Состояние: ОШИБКА";
                    connectionCheck.BackColor = Color.Red;
                }
            }).Start();
        }
    }
}