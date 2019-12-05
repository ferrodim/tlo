
using System.ComponentModel;
using System.Windows.Forms;

namespace TLO.Forms {
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    partial class SettingsForm {
        
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this._btCheck = new System.Windows.Forms.Button();
            this._btCancel = new System.Windows.Forms.Button();
            this._btSave = new System.Windows.Forms.Button();
            this._tpCategories = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this._CategoriesTbLabel = new System.Windows.Forms.TextBox();
            this.label40 = new System.Windows.Forms.Label();
            this._cbIsSaveTorrentFile = new System.Windows.Forms.CheckBox();
            this._cbIsSaveWebPage = new System.Windows.Forms.CheckBox();
            this._CategoriesCbTorrentClient = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this._CategoriesTbFolderDownloads = new System.Windows.Forms.TextBox();
            this._cbSubFolder = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this._CategoriesBtSelectFolder = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this._CategoriesCbStartCountSeeders = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this._CategoriesTbFullName = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this._CategoriesTbCategoryID = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this._btCategoryRemove = new System.Windows.Forms.Button();
            this._btCategoryAdd = new System.Windows.Forms.Button();
            this.dgwCategories = new System.Windows.Forms.DataGridView();
            this.ColumnCategoryCategoryID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tbpTorrentClients = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this._btTorrentClientAdd = new System.Windows.Forms.Button();
            this._btTorrentClientDelete = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this._tcrbRemote = new System.Windows.Forms.RadioButton();
            this._tcrbCurrent = new System.Windows.Forms.RadioButton();
            this._tbTorrentClientHostIP = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this._tbTorrentClientUserPassword = new System.Windows.Forms.TextBox();
            this._tbTorrentClientUserName = new System.Windows.Forms.TextBox();
            this._tbTorrentClientPort = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dgwTorrentClients = new System.Windows.Forms.DataGridView();
            this.UID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FolderName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._cbTorrentClientType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._tbTorrentClientName = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.label47 = new System.Windows.Forms.Label();
            this.showNotificationInTray = new System.Windows.Forms.CheckBox();
            this.showTrayIcon = new System.Windows.Forms.CheckBox();
            this.closeToTray = new System.Windows.Forms.CheckBox();
            this.hideToTray = new System.Windows.Forms.CheckBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.connectionCheck = new System.Windows.Forms.Label();
            this.SystemProxy = new System.Windows.Forms.CheckBox();
            this.ProxyAddButton = new System.Windows.Forms.Button();
            this.ProxyListBox = new System.Windows.Forms.ListBox();
            this.DisableCertVerifyCheck = new System.Windows.Forms.CheckBox();
            this.useProxyCheckBox = new System.Windows.Forms.CheckBox();
            this.label41 = new System.Windows.Forms.Label();
            this.apiHosts = new System.Windows.Forms.ComboBox();
            this.proxyInput = new System.Windows.Forms.TextBox();
            this.label42 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this._appCountSeedersReport = new System.Windows.Forms.NumericUpDown();
            this.label19 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label46 = new System.Windows.Forms.Label();
            this._dbLoadInMemoryCheckbox = new System.Windows.Forms.CheckBox();
            this._appIsNotSaveStatistics = new System.Windows.Forms.CheckBox();
            this.label23 = new System.Windows.Forms.Label();
            this._appLogLevel = new System.Windows.Forms.NumericUpDown();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this._appSelectLessOrEqual = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this._appIsUpdateStatistics = new System.Windows.Forms.CheckBox();
            this._appPeriodRunAndStopTorrents = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._appCountDaysKeepHistory = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this._appIsAvgCountSeeders = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this._appKeeperPass = new System.Windows.Forms.TextBox();
            this._appKeeperName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.templatesTabPage3 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label17 = new System.Windows.Forms.Label();
            this.categoryReportTemplate = new System.Windows.Forms.TextBox();
            this.reportHeaderTemplate = new System.Windows.Forms.TextBox();
            this.label44 = new System.Windows.Forms.Label();
            this._appReportLine = new System.Windows.Forms.TextBox();
            this.summaryReportTemplate = new System.Windows.Forms.TextBox();
            this.label45 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this._appReportTop1 = new System.Windows.Forms.TextBox();
            this._appReportTop2 = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this._appReportBottom = new System.Windows.Forms.TextBox();
            this.label39 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.forumPages1 = new TLO.Forms.ForumPages();
            this._tpAllCategories = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label48 = new System.Windows.Forms.Label();
            this.rutrackerHost = new System.Windows.Forms.TextBox();
            this._tpCategories.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwCategories)).BeginInit();
            this.tbpTorrentClients.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwTorrentClients)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._appCountSeedersReport)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._appLogLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._appPeriodRunAndStopTorrents)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._appCountDaysKeepHistory)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.templatesTabPage3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this._tpAllCategories.SuspendLayout();
            this.SuspendLayout();
            // 
            // _btCheck
            // 
            this._btCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._btCheck.Location = new System.Drawing.Point(4, 619);
            this._btCheck.Name = "_btCheck";
            this._btCheck.Size = new System.Drawing.Size(75, 23);
            this._btCheck.TabIndex = 16;
            this._btCheck.Text = "Проверить";
            this._btCheck.UseVisualStyleBackColor = true;
            this._btCheck.Click += new System.EventHandler(this.ClickButtons);
            // 
            // _btCancel
            // 
            this._btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btCancel.Location = new System.Drawing.Point(901, 619);
            this._btCancel.Name = "_btCancel";
            this._btCancel.Size = new System.Drawing.Size(75, 23);
            this._btCancel.TabIndex = 15;
            this._btCancel.Text = "Отмена";
            this._btCancel.UseVisualStyleBackColor = true;
            this._btCancel.Click += new System.EventHandler(this.ClickButtons);
            // 
            // _btSave
            // 
            this._btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btSave.Location = new System.Drawing.Point(820, 619);
            this._btSave.Name = "_btSave";
            this._btSave.Size = new System.Drawing.Size(75, 23);
            this._btSave.TabIndex = 14;
            this._btSave.Text = "Сохранить";
            this._btSave.UseVisualStyleBackColor = true;
            this._btSave.Click += new System.EventHandler(this.ClickButtons);
            // 
            // _tpCategories
            // 
            this._tpCategories.Controls.Add(this.panel1);
            this._tpCategories.Controls.Add(this._btCategoryRemove);
            this._tpCategories.Controls.Add(this._btCategoryAdd);
            this._tpCategories.Controls.Add(this.dgwCategories);
            this._tpCategories.Location = new System.Drawing.Point(4, 22);
            this._tpCategories.Name = "_tpCategories";
            this._tpCategories.Padding = new System.Windows.Forms.Padding(3);
            this._tpCategories.Size = new System.Drawing.Size(975, 591);
            this._tpCategories.TabIndex = 4;
            this._tpCategories.Text = "Разделы/Подразделы";
            this._tpCategories.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.groupBox7);
            this.panel1.Controls.Add(this.groupBox6);
            this.panel1.Location = new System.Drawing.Point(254, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(721, 550);
            this.panel1.TabIndex = 6;
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.Controls.Add(this._CategoriesTbLabel);
            this.groupBox7.Controls.Add(this.label40);
            this.groupBox7.Controls.Add(this._cbIsSaveTorrentFile);
            this.groupBox7.Controls.Add(this._cbIsSaveWebPage);
            this.groupBox7.Controls.Add(this._CategoriesCbTorrentClient);
            this.groupBox7.Controls.Add(this.label3);
            this.groupBox7.Controls.Add(this._CategoriesTbFolderDownloads);
            this.groupBox7.Controls.Add(this._cbSubFolder);
            this.groupBox7.Controls.Add(this.label22);
            this.groupBox7.Controls.Add(this._CategoriesBtSelectFolder);
            this.groupBox7.Controls.Add(this.label18);
            this.groupBox7.Controls.Add(this.label16);
            this.groupBox7.Controls.Add(this._CategoriesCbStartCountSeeders);
            this.groupBox7.Controls.Add(this.label15);
            this.groupBox7.Location = new System.Drawing.Point(3, 122);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(712, 235);
            this.groupBox7.TabIndex = 6;
            this.groupBox7.TabStop = false;
            // 
            // _CategoriesTbLabel
            // 
            this._CategoriesTbLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._CategoriesTbLabel.Location = new System.Drawing.Point(115, 202);
            this._CategoriesTbLabel.Name = "_CategoriesTbLabel";
            this._CategoriesTbLabel.Size = new System.Drawing.Size(591, 20);
            this._CategoriesTbLabel.TabIndex = 15;
            this._CategoriesTbLabel.Enter += new System.EventHandler(this._Focus_Enter);
            this._CategoriesTbLabel.Leave += new System.EventHandler(this._Focus_Enter);
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(6, 205);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(103, 13);
            this.label40.TabIndex = 14;
            this.label40.Text = "Установить метку:";
            // 
            // _cbIsSaveTorrentFile
            // 
            this._cbIsSaveTorrentFile.AutoSize = true;
            this._cbIsSaveTorrentFile.Location = new System.Drawing.Point(9, 156);
            this._cbIsSaveTorrentFile.Name = "_cbIsSaveTorrentFile";
            this._cbIsSaveTorrentFile.Size = new System.Drawing.Size(305, 17);
            this._cbIsSaveTorrentFile.TabIndex = 13;
            this._cbIsSaveTorrentFile.Text = "Сохранять torrent-файлы в подкаталог \"!!!Torrent-files!!!\"";
            this._cbIsSaveTorrentFile.UseVisualStyleBackColor = true;
            this._cbIsSaveTorrentFile.CheckedChanged += new System.EventHandler(this._Focus_Enter);
            // 
            // _cbIsSaveWebPage
            // 
            this._cbIsSaveWebPage.AutoSize = true;
            this._cbIsSaveWebPage.Location = new System.Drawing.Point(9, 179);
            this._cbIsSaveWebPage.Name = "_cbIsSaveWebPage";
            this._cbIsSaveWebPage.Size = new System.Drawing.Size(354, 17);
            this._cbIsSaveWebPage.TabIndex = 12;
            this._cbIsSaveWebPage.Text = "Сохранять web-страницы раздачи в подкаталог \"!!!Web-pages!!!\"";
            this._cbIsSaveWebPage.UseVisualStyleBackColor = true;
            this._cbIsSaveWebPage.CheckedChanged += new System.EventHandler(this._Focus_Enter);
            // 
            // _CategoriesCbTorrentClient
            // 
            this._CategoriesCbTorrentClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._CategoriesCbTorrentClient.FormattingEnabled = true;
            this._CategoriesCbTorrentClient.Location = new System.Drawing.Point(336, 59);
            this._CategoriesCbTorrentClient.Name = "_CategoriesCbTorrentClient";
            this._CategoriesCbTorrentClient.Size = new System.Drawing.Size(436, 21);
            this._CategoriesCbTorrentClient.TabIndex = 11;
            this._CategoriesCbTorrentClient.SelectionChangeCommitted += new System.EventHandler(this._Focus_Enter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(273, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Торрент-клиент, куда требуется добавлять раздачи:";
            // 
            // _CategoriesTbFolderDownloads
            // 
            this._CategoriesTbFolderDownloads.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._CategoriesTbFolderDownloads.Location = new System.Drawing.Point(9, 103);
            this._CategoriesTbFolderDownloads.Name = "_CategoriesTbFolderDownloads";
            this._CategoriesTbFolderDownloads.Size = new System.Drawing.Size(667, 20);
            this._CategoriesTbFolderDownloads.TabIndex = 5;
            this._CategoriesTbFolderDownloads.Enter += new System.EventHandler(this._Focus_Enter);
            this._CategoriesTbFolderDownloads.Leave += new System.EventHandler(this._Focus_Enter);
            // 
            // _cbSubFolder
            // 
            this._cbSubFolder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbSubFolder.FormattingEnabled = true;
            this._cbSubFolder.Items.AddRange(new object[] {
            "Не нужен",
            "С ID топика",
            "Запрашивать"});
            this._cbSubFolder.Location = new System.Drawing.Point(336, 129);
            this._cbSubFolder.Name = "_cbSubFolder";
            this._cbSubFolder.Size = new System.Drawing.Size(270, 21);
            this._cbSubFolder.TabIndex = 9;
            this._cbSubFolder.SelectionChangeCommitted += new System.EventHandler(this._Focus_Enter);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(6, 132);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(261, 13);
            this.label22.TabIndex = 8;
            this.label22.Text = "Создавать подкаталог для добавляемой раздачи:";
            // 
            // _CategoriesBtSelectFolder
            // 
            this._CategoriesBtSelectFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._CategoriesBtSelectFolder.Location = new System.Drawing.Point(676, 102);
            this._CategoriesBtSelectFolder.Name = "_CategoriesBtSelectFolder";
            this._CategoriesBtSelectFolder.Size = new System.Drawing.Size(30, 22);
            this._CategoriesBtSelectFolder.TabIndex = 7;
            this._CategoriesBtSelectFolder.Text = "...";
            this._CategoriesBtSelectFolder.UseVisualStyleBackColor = true;
            this._CategoriesBtSelectFolder.Click += new System.EventHandler(this.ClickButtons);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 87);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(298, 13);
            this.label18.TabIndex = 6;
            this.label18.Text = "Помещать новые загрузки этого раздела/подраздела в:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label16.Location = new System.Drawing.Point(6, 43);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(359, 13);
            this.label16.TabIndex = 2;
            this.label16.Text = "Остановка происходит при кол-ве сидов больше этого на 2 и больше";
            // 
            // _CategoriesCbStartCountSeeders
            // 
            this._CategoriesCbStartCountSeeders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._CategoriesCbStartCountSeeders.FormattingEnabled = true;
            this._CategoriesCbStartCountSeeders.Items.AddRange(new object[] {
            "-",
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this._CategoriesCbStartCountSeeders.Location = new System.Drawing.Point(336, 19);
            this._CategoriesCbStartCountSeeders.Name = "_CategoriesCbStartCountSeeders";
            this._CategoriesCbStartCountSeeders.Size = new System.Drawing.Size(61, 21);
            this._CategoriesCbStartCountSeeders.TabIndex = 1;
            this._CategoriesCbStartCountSeeders.SelectionChangeCommitted += new System.EventHandler(this._Focus_Enter);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 22);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(225, 13);
            this.label15.TabIndex = 0;
            this.label15.Text = "Запускать раздачу, если сидов не больше:";
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this._CategoriesTbFullName);
            this.groupBox6.Controls.Add(this.label14);
            this.groupBox6.Controls.Add(this._CategoriesTbCategoryID);
            this.groupBox6.Controls.Add(this.label13);
            this.groupBox6.Location = new System.Drawing.Point(3, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(712, 113);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Данные о выбраном разделе";
            // 
            // _CategoriesTbFullName
            // 
            this._CategoriesTbFullName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._CategoriesTbFullName.Location = new System.Drawing.Point(6, 58);
            this._CategoriesTbFullName.Multiline = true;
            this._CategoriesTbFullName.Name = "_CategoriesTbFullName";
            this._CategoriesTbFullName.ReadOnly = true;
            this._CategoriesTbFullName.Size = new System.Drawing.Size(700, 49);
            this._CategoriesTbFullName.TabIndex = 3;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 42);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(128, 13);
            this.label14.TabIndex = 2;
            this.label14.Text = "Полный путь к разделу:";
            // 
            // _CategoriesTbCategoryID
            // 
            this._CategoriesTbCategoryID.Location = new System.Drawing.Point(96, 19);
            this._CategoriesTbCategoryID.Name = "_CategoriesTbCategoryID";
            this._CategoriesTbCategoryID.ReadOnly = true;
            this._CategoriesTbCategoryID.Size = new System.Drawing.Size(100, 20);
            this._CategoriesTbCategoryID.TabIndex = 1;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 22);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(84, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "№ подраздела:";
            // 
            // _btCategoryRemove
            // 
            this._btCategoryRemove.Location = new System.Drawing.Point(131, 6);
            this._btCategoryRemove.Name = "_btCategoryRemove";
            this._btCategoryRemove.Size = new System.Drawing.Size(117, 23);
            this._btCategoryRemove.TabIndex = 4;
            this._btCategoryRemove.Text = "Удалить";
            this._btCategoryRemove.UseVisualStyleBackColor = true;
            this._btCategoryRemove.Click += new System.EventHandler(this.ClickButtons);
            // 
            // _btCategoryAdd
            // 
            this._btCategoryAdd.Location = new System.Drawing.Point(8, 6);
            this._btCategoryAdd.Name = "_btCategoryAdd";
            this._btCategoryAdd.Size = new System.Drawing.Size(117, 23);
            this._btCategoryAdd.TabIndex = 3;
            this._btCategoryAdd.Text = "Добавить";
            this._btCategoryAdd.UseVisualStyleBackColor = true;
            this._btCategoryAdd.Click += new System.EventHandler(this.ClickButtons);
            // 
            // dgwCategories
            // 
            this.dgwCategories.AllowUserToAddRows = false;
            this.dgwCategories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgwCategories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgwCategories.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnCategoryCategoryID,
            this.ColumnCategoryName});
            this.dgwCategories.Location = new System.Drawing.Point(8, 35);
            this.dgwCategories.MultiSelect = false;
            this.dgwCategories.Name = "dgwCategories";
            this.dgwCategories.ReadOnly = true;
            this.dgwCategories.RowHeadersVisible = false;
            this.dgwCategories.Size = new System.Drawing.Size(240, 550);
            this.dgwCategories.TabIndex = 0;
            this.dgwCategories.SelectionChanged += new System.EventHandler(this.SelectionChanged);
            // 
            // ColumnCategoryCategoryID
            // 
            this.ColumnCategoryCategoryID.DataPropertyName = "CategoryID";
            this.ColumnCategoryCategoryID.HeaderText = "CategoryID";
            this.ColumnCategoryCategoryID.Name = "ColumnCategoryCategoryID";
            this.ColumnCategoryCategoryID.ReadOnly = true;
            this.ColumnCategoryCategoryID.Visible = false;
            // 
            // ColumnCategoryName
            // 
            this.ColumnCategoryName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnCategoryName.DataPropertyName = "Name";
            this.ColumnCategoryName.HeaderText = "Раздел";
            this.ColumnCategoryName.Name = "ColumnCategoryName";
            this.ColumnCategoryName.ReadOnly = true;
            // 
            // tbpTorrentClients
            // 
            this.tbpTorrentClients.BackColor = System.Drawing.SystemColors.Control;
            this.tbpTorrentClients.Controls.Add(this.groupBox5);
            this.tbpTorrentClients.Location = new System.Drawing.Point(4, 22);
            this.tbpTorrentClients.Name = "tbpTorrentClients";
            this.tbpTorrentClients.Padding = new System.Windows.Forms.Padding(3);
            this.tbpTorrentClients.Size = new System.Drawing.Size(975, 591);
            this.tbpTorrentClients.TabIndex = 0;
            this.tbpTorrentClients.Text = "Torrent-клиенты";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this._btTorrentClientAdd);
            this.groupBox5.Controls.Add(this._btTorrentClientDelete);
            this.groupBox5.Controls.Add(this.groupBox3);
            this.groupBox5.Controls.Add(this.dgwTorrentClients);
            this.groupBox5.Controls.Add(this._cbTorrentClientType);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this._tbTorrentClientName);
            this.groupBox5.Location = new System.Drawing.Point(8, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(960, 360);
            this.groupBox5.TabIndex = 11;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Torrent-клиенты";
            // 
            // _btTorrentClientAdd
            // 
            this._btTorrentClientAdd.Location = new System.Drawing.Point(6, 19);
            this._btTorrentClientAdd.Name = "_btTorrentClientAdd";
            this._btTorrentClientAdd.Size = new System.Drawing.Size(117, 23);
            this._btTorrentClientAdd.TabIndex = 1;
            this._btTorrentClientAdd.Text = "Добавить";
            this._btTorrentClientAdd.UseVisualStyleBackColor = true;
            this._btTorrentClientAdd.Click += new System.EventHandler(this.ClickButtons);
            // 
            // _btTorrentClientDelete
            // 
            this._btTorrentClientDelete.Location = new System.Drawing.Point(129, 19);
            this._btTorrentClientDelete.Name = "_btTorrentClientDelete";
            this._btTorrentClientDelete.Size = new System.Drawing.Size(117, 23);
            this._btTorrentClientDelete.TabIndex = 2;
            this._btTorrentClientDelete.Text = "Удалить";
            this._btTorrentClientDelete.UseVisualStyleBackColor = true;
            this._btTorrentClientDelete.Click += new System.EventHandler(this.ClickButtons);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this._tcrbRemote);
            this.groupBox3.Controls.Add(this._tcrbCurrent);
            this.groupBox3.Controls.Add(this._tbTorrentClientHostIP);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this._tbTorrentClientUserPassword);
            this.groupBox3.Controls.Add(this._tbTorrentClientUserName);
            this.groupBox3.Controls.Add(this._tbTorrentClientPort);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(255, 98);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(636, 151);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Доступ к torrent-клиенту";
            // 
            // _tcrbRemote
            // 
            this._tcrbRemote.AutoSize = true;
            this._tcrbRemote.Location = new System.Drawing.Point(9, 43);
            this._tcrbRemote.Name = "_tcrbRemote";
            this._tcrbRemote.Size = new System.Drawing.Size(211, 17);
            this._tcrbRemote.TabIndex = 9;
            this._tcrbRemote.Text = "На другом компьютере, его имя/IP: ";
            this._tcrbRemote.UseVisualStyleBackColor = true;
            this._tcrbRemote.CheckedChanged += new System.EventHandler(this._Focus_Enter);
            // 
            // _tcrbCurrent
            // 
            this._tcrbCurrent.AutoSize = true;
            this._tcrbCurrent.Checked = true;
            this._tcrbCurrent.Location = new System.Drawing.Point(9, 20);
            this._tcrbCurrent.Name = "_tcrbCurrent";
            this._tcrbCurrent.Size = new System.Drawing.Size(150, 17);
            this._tcrbCurrent.TabIndex = 8;
            this._tcrbCurrent.TabStop = true;
            this._tcrbCurrent.Text = "На этом же компьютере";
            this._tcrbCurrent.UseVisualStyleBackColor = true;
            this._tcrbCurrent.CheckedChanged += new System.EventHandler(this._Focus_Enter);
            // 
            // _tbTorrentClientHostIP
            // 
            this._tbTorrentClientHostIP.Location = new System.Drawing.Point(226, 42);
            this._tbTorrentClientHostIP.Name = "_tbTorrentClientHostIP";
            this._tbTorrentClientHostIP.Size = new System.Drawing.Size(150, 20);
            this._tbTorrentClientHostIP.TabIndex = 6;
            this._tbTorrentClientHostIP.Enter += new System.EventHandler(this._Focus_Enter);
            this._tbTorrentClientHostIP.Leave += new System.EventHandler(this._Focus_Enter);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 123);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(199, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Пароль пользователя torrent-клиента:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 97);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(183, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Имя пользователя torrent-клиента:";
            // 
            // _tbTorrentClientUserPassword
            // 
            this._tbTorrentClientUserPassword.Location = new System.Drawing.Point(226, 120);
            this._tbTorrentClientUserPassword.Name = "_tbTorrentClientUserPassword";
            this._tbTorrentClientUserPassword.PasswordChar = '*';
            this._tbTorrentClientUserPassword.Size = new System.Drawing.Size(150, 20);
            this._tbTorrentClientUserPassword.TabIndex = 3;
            this._tbTorrentClientUserPassword.Enter += new System.EventHandler(this._Focus_Enter);
            this._tbTorrentClientUserPassword.Leave += new System.EventHandler(this._Focus_Enter);
            // 
            // _tbTorrentClientUserName
            // 
            this._tbTorrentClientUserName.Location = new System.Drawing.Point(226, 94);
            this._tbTorrentClientUserName.Name = "_tbTorrentClientUserName";
            this._tbTorrentClientUserName.Size = new System.Drawing.Size(150, 20);
            this._tbTorrentClientUserName.TabIndex = 2;
            this._tbTorrentClientUserName.Enter += new System.EventHandler(this._Focus_Enter);
            this._tbTorrentClientUserName.Leave += new System.EventHandler(this._Focus_Enter);
            // 
            // _tbTorrentClientPort
            // 
            this._tbTorrentClientPort.Location = new System.Drawing.Point(226, 68);
            this._tbTorrentClientPort.Name = "_tbTorrentClientPort";
            this._tbTorrentClientPort.Size = new System.Drawing.Size(150, 20);
            this._tbTorrentClientPort.TabIndex = 1;
            this._tbTorrentClientPort.Enter += new System.EventHandler(this._Focus_Enter);
            this._tbTorrentClientPort.Leave += new System.EventHandler(this._Focus_Enter);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(147, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Порт Web/API-интерфейса:";
            // 
            // dgwTorrentClients
            // 
            this.dgwTorrentClients.AllowUserToAddRows = false;
            this.dgwTorrentClients.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgwTorrentClients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgwTorrentClients.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UID,
            this.FolderName});
            this.dgwTorrentClients.Location = new System.Drawing.Point(6, 48);
            this.dgwTorrentClients.MultiSelect = false;
            this.dgwTorrentClients.Name = "dgwTorrentClients";
            this.dgwTorrentClients.RowHeadersVisible = false;
            this.dgwTorrentClients.Size = new System.Drawing.Size(240, 306);
            this.dgwTorrentClients.TabIndex = 0;
            this.dgwTorrentClients.SelectionChanged += new System.EventHandler(this.SelectionChanged);
            // 
            // UID
            // 
            this.UID.DataPropertyName = "UID";
            this.UID.HeaderText = "UID";
            this.UID.Name = "UID";
            this.UID.ReadOnly = true;
            this.UID.Visible = false;
            // 
            // FolderName
            // 
            this.FolderName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FolderName.DataPropertyName = "Name";
            this.FolderName.HeaderText = "Настройки";
            this.FolderName.Name = "FolderName";
            this.FolderName.ReadOnly = true;
            // 
            // _cbTorrentClientType
            // 
            this._cbTorrentClientType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbTorrentClientType.FormattingEnabled = true;
            this._cbTorrentClientType.Items.AddRange(new object[] {
            "uTorrent",
            "Transmission",
            "Vuze (Vuze Web Remote)"});
            this._cbTorrentClientType.Location = new System.Drawing.Point(481, 71);
            this._cbTorrentClientType.Name = "_cbTorrentClientType";
            this._cbTorrentClientType.Size = new System.Drawing.Size(121, 21);
            this._cbTorrentClientType.TabIndex = 6;
            this._cbTorrentClientType.Enter += new System.EventHandler(this._Focus_Enter);
            this._cbTorrentClientType.Leave += new System.EventHandler(this._Focus_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(252, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Тип torrent-клиента";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(252, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(226, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Название группы настроек torrent-клиента:";
            // 
            // _tbTorrentClientName
            // 
            this._tbTorrentClientName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tbTorrentClientName.Location = new System.Drawing.Point(481, 45);
            this._tbTorrentClientName.Name = "_tbTorrentClientName";
            this._tbTorrentClientName.Size = new System.Drawing.Size(473, 20);
            this._tbTorrentClientName.TabIndex = 4;
            this._tbTorrentClientName.Enter += new System.EventHandler(this._Focus_Enter);
            this._tbTorrentClientName.Leave += new System.EventHandler(this._Focus_Enter);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.templatesTabPage3);
            this.tabControl1.Controls.Add(this.tbpTorrentClients);
            this.tabControl1.Controls.Add(this._tpCategories);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this._tpAllCategories);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(983, 617);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox10);
            this.tabPage2.Controls.Add(this.groupBox9);
            this.tabPage2.Controls.Add(this.groupBox8);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(975, 591);
            this.tabPage2.TabIndex = 6;
            this.tabPage2.Text = "Основные настройки";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.label47);
            this.groupBox10.Controls.Add(this.showNotificationInTray);
            this.groupBox10.Controls.Add(this.showTrayIcon);
            this.groupBox10.Controls.Add(this.closeToTray);
            this.groupBox10.Controls.Add(this.hideToTray);
            this.groupBox10.Location = new System.Drawing.Point(8, 477);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(367, 108);
            this.groupBox10.TabIndex = 18;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Трей";
            // 
            // label47
            // 
            this.label47.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label47.Location = new System.Drawing.Point(3, 62);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(353, 43);
            this.label47.TabIndex = 21;
            this.label47.Text = "Если включить сворачивание или закрытие в трей, то кнопка в панели задач соответс" +
    "твенно будет исчезать, так как программа будет переходить в трей.";
            // 
            // showNotificationInTray
            // 
            this.showNotificationInTray.AutoSize = true;
            this.showNotificationInTray.Checked = global::TLO.Properties.Settings.Default.NotificationInTray;
            this.showNotificationInTray.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::TLO.Properties.Settings.Default, "NotificationInTray", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.showNotificationInTray.Location = new System.Drawing.Point(176, 40);
            this.showNotificationInTray.Name = "showNotificationInTray";
            this.showNotificationInTray.Size = new System.Drawing.Size(131, 17);
            this.showNotificationInTray.TabIndex = 20;
            this.showNotificationInTray.Text = "Уведомления в трее";
            this.showNotificationInTray.UseVisualStyleBackColor = true;
            // 
            // showTrayIcon
            // 
            this.showTrayIcon.AutoSize = true;
            this.showTrayIcon.Checked = global::TLO.Properties.Settings.Default.ShowInTray;
            this.showTrayIcon.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::TLO.Properties.Settings.Default, "ShowInTray", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.showTrayIcon.Location = new System.Drawing.Point(8, 17);
            this.showTrayIcon.Name = "showTrayIcon";
            this.showTrayIcon.Size = new System.Drawing.Size(162, 17);
            this.showTrayIcon.TabIndex = 19;
            this.showTrayIcon.Text = "Показывать значок в трее";
            this.showTrayIcon.UseVisualStyleBackColor = true;
            // 
            // closeToTray
            // 
            this.closeToTray.AutoSize = true;
            this.closeToTray.Checked = global::TLO.Properties.Settings.Default.CloseToTray;
            this.closeToTray.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::TLO.Properties.Settings.Default, "CloseToTray", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.closeToTray.Location = new System.Drawing.Point(176, 17);
            this.closeToTray.Name = "closeToTray";
            this.closeToTray.Size = new System.Drawing.Size(117, 17);
            this.closeToTray.TabIndex = 1;
            this.closeToTray.Text = "Закрывать в трей";
            this.closeToTray.UseVisualStyleBackColor = true;
            // 
            // hideToTray
            // 
            this.hideToTray.AutoSize = true;
            this.hideToTray.Checked = global::TLO.Properties.Settings.Default.HideToTray;
            this.hideToTray.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::TLO.Properties.Settings.Default, "HideToTray", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.hideToTray.Location = new System.Drawing.Point(6, 40);
            this.hideToTray.Name = "hideToTray";
            this.hideToTray.Size = new System.Drawing.Size(126, 17);
            this.hideToTray.TabIndex = 0;
            this.hideToTray.Text = "Сворачивать в трей";
            this.hideToTray.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.rutrackerHost);
            this.groupBox9.Controls.Add(this.label48);
            this.groupBox9.Controls.Add(this.connectionCheck);
            this.groupBox9.Controls.Add(this.SystemProxy);
            this.groupBox9.Controls.Add(this.ProxyAddButton);
            this.groupBox9.Controls.Add(this.ProxyListBox);
            this.groupBox9.Controls.Add(this.DisableCertVerifyCheck);
            this.groupBox9.Controls.Add(this.useProxyCheckBox);
            this.groupBox9.Controls.Add(this.label41);
            this.groupBox9.Controls.Add(this.apiHosts);
            this.groupBox9.Controls.Add(this.proxyInput);
            this.groupBox9.Controls.Add(this.label42);
            this.groupBox9.Location = new System.Drawing.Point(382, 405);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(590, 180);
            this.groupBox9.TabIndex = 17;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Сеть и прокси";
            // 
            // connectionCheck
            // 
            this.connectionCheck.AutoSize = true;
            this.connectionCheck.BackColor = System.Drawing.Color.DarkOrange;
            this.connectionCheck.Location = new System.Drawing.Point(8, 160);
            this.connectionCheck.Name = "connectionCheck";
            this.connectionCheck.Size = new System.Drawing.Size(73, 13);
            this.connectionCheck.TabIndex = 29;
            this.connectionCheck.Text = "Состояние: ?";
            // 
            // SystemProxy
            // 
            this.SystemProxy.Location = new System.Drawing.Point(182, 97);
            this.SystemProxy.Name = "SystemProxy";
            this.SystemProxy.Size = new System.Drawing.Size(165, 21);
            this.SystemProxy.TabIndex = 28;
            this.SystemProxy.Text = "Системный прокси";
            this.SystemProxy.UseVisualStyleBackColor = true;
            // 
            // ProxyAddButton
            // 
            this.ProxyAddButton.Location = new System.Drawing.Point(321, 130);
            this.ProxyAddButton.Name = "ProxyAddButton";
            this.ProxyAddButton.Size = new System.Drawing.Size(29, 22);
            this.ProxyAddButton.TabIndex = 27;
            this.ProxyAddButton.Text = ">>";
            this.ProxyAddButton.UseVisualStyleBackColor = true;
            // 
            // ProxyListBox
            // 
            this.ProxyListBox.FormattingEnabled = true;
            this.ProxyListBox.Location = new System.Drawing.Point(356, 95);
            this.ProxyListBox.Name = "ProxyListBox";
            this.ProxyListBox.Size = new System.Drawing.Size(227, 56);
            this.ProxyListBox.TabIndex = 26;
            // 
            // DisableCertVerifyCheck
            // 
            this.DisableCertVerifyCheck.AutoSize = true;
            this.DisableCertVerifyCheck.Location = new System.Drawing.Point(8, 68);
            this.DisableCertVerifyCheck.Name = "DisableCertVerifyCheck";
            this.DisableCertVerifyCheck.Size = new System.Drawing.Size(247, 17);
            this.DisableCertVerifyCheck.TabIndex = 23;
            this.DisableCertVerifyCheck.Text = "Выключить проверку сертификата сервера";
            this.DisableCertVerifyCheck.UseVisualStyleBackColor = true;
            // 
            // useProxyCheckBox
            // 
            this.useProxyCheckBox.AutoSize = true;
            this.useProxyCheckBox.Location = new System.Drawing.Point(8, 99);
            this.useProxyCheckBox.Name = "useProxyCheckBox";
            this.useProxyCheckBox.Size = new System.Drawing.Size(138, 17);
            this.useProxyCheckBox.TabIndex = 24;
            this.useProxyCheckBox.Text = "Использовать прокси";
            this.useProxyCheckBox.UseVisualStyleBackColor = true;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(5, 130);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(171, 13);
            this.label41.TabIndex = 19;
            this.label41.Text = "Добавить прокси (https, socks5):";
            // 
            // apiHosts
            // 
            this.apiHosts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.apiHosts.FormattingEnabled = true;
            this.apiHosts.Items.AddRange(new object[] {
            "api.t-ru.org",
            "api.rutracker.org"});
            this.apiHosts.Location = new System.Drawing.Point(300, 40);
            this.apiHosts.Name = "apiHosts";
            this.apiHosts.Size = new System.Drawing.Size(287, 21);
            this.apiHosts.TabIndex = 21;
            // 
            // proxyInput
            // 
            this.proxyInput.AutoCompleteCustomSource.AddRange(new string[] {
            "https://",
            "socks5://"});
            this.proxyInput.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.proxyInput.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.proxyInput.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.proxyInput.Location = new System.Drawing.Point(182, 130);
            this.proxyInput.Name = "proxyInput";
            this.proxyInput.Size = new System.Drawing.Size(133, 22);
            this.proxyInput.TabIndex = 18;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(297, 20);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(109, 13);
            this.label42.TabIndex = 20;
            this.label42.Text = "Хост API рутрекера:";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.label9);
            this.groupBox8.Controls.Add(this._appCountSeedersReport);
            this.groupBox8.Controls.Add(this.label19);
            this.groupBox8.Location = new System.Drawing.Point(381, 6);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(591, 72);
            this.groupBox8.TabIndex = 3;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Настройки отчетов";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(431, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "В отчете о сидируемых раздачах отображаются раздачи с кол-вом сидов не более:";
            // 
            // _appCountSeedersReport
            // 
            this._appCountSeedersReport.Location = new System.Drawing.Point(459, 19);
            this._appCountSeedersReport.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this._appCountSeedersReport.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this._appCountSeedersReport.Name = "_appCountSeedersReport";
            this._appCountSeedersReport.Size = new System.Drawing.Size(63, 20);
            this._appCountSeedersReport.TabIndex = 9;
            this._appCountSeedersReport.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label19.Location = new System.Drawing.Point(6, 42);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(557, 13);
            this.label19.TabIndex = 10;
            this.label19.Text = "Если требуется чтобы в отчет попадали все раздачи указанной категории, требуется " +
    "указать значение \"-1\"";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label46);
            this.groupBox4.Controls.Add(this._dbLoadInMemoryCheckbox);
            this.groupBox4.Controls.Add(this._appIsNotSaveStatistics);
            this.groupBox4.Controls.Add(this.label23);
            this.groupBox4.Controls.Add(this._appLogLevel);
            this.groupBox4.Controls.Add(this.label21);
            this.groupBox4.Controls.Add(this.label20);
            this.groupBox4.Controls.Add(this._appSelectLessOrEqual);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this._appIsUpdateStatistics);
            this.groupBox4.Controls.Add(this._appPeriodRunAndStopTorrents);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Location = new System.Drawing.Point(8, 161);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(367, 310);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Настройки программы";
            // 
            // label46
            // 
            this.label46.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label46.Location = new System.Drawing.Point(6, 264);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(355, 42);
            this.label46.TabIndex = 18;
            this.label46.Text = "Загрузка БД в оперативную память позволяет ускорить операции формирования отчетов" +
    " и фильтрации списка раздач. Имеет смысл включать эту опцию если фильтрация спис" +
    "ка в TLO тормозит.";
            // 
            // _dbLoadInMemoryCheckbox
            // 
            this._dbLoadInMemoryCheckbox.Location = new System.Drawing.Point(6, 244);
            this._dbLoadInMemoryCheckbox.Name = "_dbLoadInMemoryCheckbox";
            this._dbLoadInMemoryCheckbox.Size = new System.Drawing.Size(219, 17);
            this._dbLoadInMemoryCheckbox.TabIndex = 17;
            this._dbLoadInMemoryCheckbox.Text = "Загружать БД в оперативную память";
            this._dbLoadInMemoryCheckbox.UseVisualStyleBackColor = true;
            this._dbLoadInMemoryCheckbox.Click += new System.EventHandler(this.ClickButtons);
            // 
            // _appIsNotSaveStatistics
            // 
            this._appIsNotSaveStatistics.AutoSize = true;
            this._appIsNotSaveStatistics.Location = new System.Drawing.Point(6, 46);
            this._appIsNotSaveStatistics.Name = "_appIsNotSaveStatistics";
            this._appIsNotSaveStatistics.Size = new System.Drawing.Size(296, 17);
            this._appIsNotSaveStatistics.TabIndex = 16;
            this._appIsNotSaveStatistics.Text = "Не сохранять статистику о кол-ве сидов на раздачах";
            this._appIsNotSaveStatistics.UseVisualStyleBackColor = true;
            this._appIsNotSaveStatistics.Click += new System.EventHandler(this.SelectionChanged);
            // 
            // label23
            // 
            this.label23.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label23.Location = new System.Drawing.Point(6, 198);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(355, 42);
            this.label23.TabIndex = 15;
            this.label23.Text = "Отвечает за подробность ведения текстового лога. 0 - ошибки, 1 - информационные л" +
    "оги, 2 -  подробные логи, 3 - все логи а также ответы от сервера.";
            // 
            // _appLogLevel
            // 
            this._appLogLevel.Location = new System.Drawing.Point(298, 175);
            this._appLogLevel.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this._appLogLevel.Name = "_appLogLevel";
            this._appLogLevel.Size = new System.Drawing.Size(63, 20);
            this._appLogLevel.TabIndex = 14;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(6, 177);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(231, 13);
            this.label21.TabIndex = 13;
            this.label21.Text = "Уровень ведения логов (значение от 0 до 3)";
            // 
            // label20
            // 
            this.label20.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label20.Location = new System.Drawing.Point(6, 142);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(355, 30);
            this.label20.TabIndex = 12;
            this.label20.Text = "Если флаг не установлен, то на главной форме будет использоваться строгое соответ" +
    "ствие указаному значению";
            // 
            // _appSelectLessOrEqual
            // 
            this._appSelectLessOrEqual.AutoSize = true;
            this._appSelectLessOrEqual.Location = new System.Drawing.Point(6, 122);
            this._appSelectLessOrEqual.Name = "_appSelectLessOrEqual";
            this._appSelectLessOrEqual.Size = new System.Drawing.Size(278, 17);
            this._appSelectLessOrEqual.TabIndex = 11;
            this._appSelectLessOrEqual.Text = "Использовать отбор как <= указанного значения";
            this._appSelectLessOrEqual.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label12.Location = new System.Drawing.Point(6, 89);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(355, 30);
            this.label12.TabIndex = 7;
            this.label12.Text = "Операция по обновлению статистики ресурсоемкая при наличии большого кол-ва раздел" +
    "ов и продолжается значительное время";
            // 
            // _appIsUpdateStatistics
            // 
            this._appIsUpdateStatistics.AutoSize = true;
            this._appIsUpdateStatistics.Location = new System.Drawing.Point(6, 69);
            this._appIsUpdateStatistics.Name = "_appIsUpdateStatistics";
            this._appIsUpdateStatistics.Size = new System.Drawing.Size(301, 17);
            this._appIsUpdateStatistics.TabIndex = 6;
            this._appIsUpdateStatistics.Text = "Обновлять статистику при запуске/остановке раздач";
            this._appIsUpdateStatistics.UseVisualStyleBackColor = true;
            // 
            // _appPeriodRunAndStopTorrents
            // 
            this._appPeriodRunAndStopTorrents.Location = new System.Drawing.Point(259, 20);
            this._appPeriodRunAndStopTorrents.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
            this._appPeriodRunAndStopTorrents.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this._appPeriodRunAndStopTorrents.Name = "_appPeriodRunAndStopTorrents";
            this._appPeriodRunAndStopTorrents.Size = new System.Drawing.Size(102, 20);
            this._appPeriodRunAndStopTorrents.TabIndex = 5;
            this._appPeriodRunAndStopTorrents.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(247, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Период цикла запуска/остановки раздач, мин.";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._appCountDaysKeepHistory);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this._appIsAvgCountSeeders);
            this.groupBox2.Location = new System.Drawing.Point(8, 84);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(367, 71);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Отбор раздач";
            // 
            // _appCountDaysKeepHistory
            // 
            this._appCountDaysKeepHistory.Location = new System.Drawing.Point(218, 42);
            this._appCountDaysKeepHistory.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this._appCountDaysKeepHistory.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._appCountDaysKeepHistory.Name = "_appCountDaysKeepHistory";
            this._appCountDaysKeepHistory.Size = new System.Drawing.Size(102, 20);
            this._appCountDaysKeepHistory.TabIndex = 6;
            this._appCountDaysKeepHistory.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 44);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(206, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Хранить историю о кол-ве сидов, дней:";
            // 
            // _appIsAvgCountSeeders
            // 
            this._appIsAvgCountSeeders.AutoSize = true;
            this._appIsAvgCountSeeders.Location = new System.Drawing.Point(6, 19);
            this._appIsAvgCountSeeders.Name = "_appIsAvgCountSeeders";
            this._appIsAvgCountSeeders.Size = new System.Drawing.Size(302, 17);
            this._appIsAvgCountSeeders.TabIndex = 0;
            this._appIsAvgCountSeeders.Text = "Использовать отбор и сортировку по ср. кол-ву сидов";
            this._appIsAvgCountSeeders.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this._appKeeperPass);
            this.groupBox1.Controls.Add(this._appKeeperName);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(367, 72);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Авторизация на сайте:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(154, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Пароль пользователя сайта:";
            // 
            // _appKeeperPass
            // 
            this._appKeeperPass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._appKeeperPass.Location = new System.Drawing.Point(166, 45);
            this._appKeeperPass.Name = "_appKeeperPass";
            this._appKeeperPass.PasswordChar = '*';
            this._appKeeperPass.Size = new System.Drawing.Size(195, 20);
            this._appKeeperPass.TabIndex = 2;
            // 
            // _appKeeperName
            // 
            this._appKeeperName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._appKeeperName.Location = new System.Drawing.Point(166, 19);
            this._appKeeperName.Name = "_appKeeperName";
            this._appKeeperName.Size = new System.Drawing.Size(195, 20);
            this._appKeeperName.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(147, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Логин пользователя сайта:";
            // 
            // templatesTabPage3
            // 
            this.templatesTabPage3.Controls.Add(this.tableLayoutPanel1);
            this.templatesTabPage3.Controls.Add(this.label39);
            this.templatesTabPage3.Controls.Add(this.label38);
            this.templatesTabPage3.Controls.Add(this.label37);
            this.templatesTabPage3.Controls.Add(this.label36);
            this.templatesTabPage3.Controls.Add(this.label35);
            this.templatesTabPage3.Controls.Add(this.label34);
            this.templatesTabPage3.Controls.Add(this.label33);
            this.templatesTabPage3.Controls.Add(this.label29);
            this.templatesTabPage3.Controls.Add(this.label28);
            this.templatesTabPage3.Controls.Add(this.label27);
            this.templatesTabPage3.Controls.Add(this.label26);
            this.templatesTabPage3.Controls.Add(this.label25);
            this.templatesTabPage3.Controls.Add(this.label24);
            this.templatesTabPage3.Location = new System.Drawing.Point(4, 22);
            this.templatesTabPage3.Name = "templatesTabPage3";
            this.templatesTabPage3.Size = new System.Drawing.Size(975, 591);
            this.templatesTabPage3.TabIndex = 8;
            this.templatesTabPage3.Text = "Шаблоны";
            this.templatesTabPage3.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.Controls.Add(this.label17, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.categoryReportTemplate, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.reportHeaderTemplate, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label44, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this._appReportLine, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.summaryReportTemplate, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label45, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label43, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label30, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label31, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this._appReportTop1, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this._appReportTop2, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.label32, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this._appReportBottom, 0, 5);
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 10;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(890, 490);
            this.tableLayoutPanel1.TabIndex = 59;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(3, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(131, 13);
            this.label17.TabIndex = 32;
            this.label17.Text = "Строка отчета (устарел):";
            // 
            // categoryReportTemplate
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.categoryReportTemplate, 3);
            this.categoryReportTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.categoryReportTemplate.Location = new System.Drawing.Point(3, 408);
            this.categoryReportTemplate.Multiline = true;
            this.categoryReportTemplate.Name = "categoryReportTemplate";
            this.categoryReportTemplate.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.categoryReportTemplate.Size = new System.Drawing.Size(884, 79);
            this.categoryReportTemplate.TabIndex = 56;
            // 
            // reportHeaderTemplate
            // 
            this.reportHeaderTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportHeaderTemplate.Location = new System.Drawing.Point(3, 114);
            this.reportHeaderTemplate.Multiline = true;
            this.reportHeaderTemplate.Name = "reportHeaderTemplate";
            this.reportHeaderTemplate.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.reportHeaderTemplate.Size = new System.Drawing.Size(290, 79);
            this.reportHeaderTemplate.TabIndex = 58;
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(3, 392);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(116, 13);
            this.label44.TabIndex = 55;
            this.label44.Text = "Отчет по подразделу:";
            // 
            // _appReportLine
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._appReportLine, 3);
            this._appReportLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this._appReportLine.Location = new System.Drawing.Point(3, 16);
            this._appReportLine.Multiline = true;
            this._appReportLine.Name = "_appReportLine";
            this._appReportLine.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._appReportLine.Size = new System.Drawing.Size(884, 79);
            this._appReportLine.TabIndex = 33;
            // 
            // summaryReportTemplate
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.summaryReportTemplate, 3);
            this.summaryReportTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.summaryReportTemplate.Location = new System.Drawing.Point(3, 310);
            this.summaryReportTemplate.Multiline = true;
            this.summaryReportTemplate.Name = "summaryReportTemplate";
            this.summaryReportTemplate.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.summaryReportTemplate.Size = new System.Drawing.Size(884, 79);
            this.summaryReportTemplate.TabIndex = 54;
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(595, 98);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(234, 13);
            this.label45.TabIndex = 57;
            this.label45.Text = "Шапки каждого сообщения отчета (устарел):";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(3, 294);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(85, 13);
            this.label43.TabIndex = 53;
            this.label43.Text = "Сводный отчет:";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(3, 98);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(143, 13);
            this.label30.TabIndex = 42;
            this.label30.Text = "Шапка списка хранителей:";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(299, 98);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(232, 13);
            this.label31.TabIndex = 43;
            this.label31.Text = "Шапка первого сообщения отчета (устарел):";
            // 
            // _appReportTop1
            // 
            this._appReportTop1.Dock = System.Windows.Forms.DockStyle.Fill;
            this._appReportTop1.Location = new System.Drawing.Point(299, 114);
            this._appReportTop1.Multiline = true;
            this._appReportTop1.Name = "_appReportTop1";
            this._appReportTop1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._appReportTop1.Size = new System.Drawing.Size(290, 79);
            this._appReportTop1.TabIndex = 40;
            // 
            // _appReportTop2
            // 
            this._appReportTop2.Dock = System.Windows.Forms.DockStyle.Fill;
            this._appReportTop2.Location = new System.Drawing.Point(595, 114);
            this._appReportTop2.Multiline = true;
            this._appReportTop2.Name = "_appReportTop2";
            this._appReportTop2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._appReportTop2.Size = new System.Drawing.Size(292, 79);
            this._appReportTop2.TabIndex = 41;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(3, 196);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(126, 13);
            this.label32.TabIndex = 45;
            this.label32.Text = "Конец отчета (устарел):";
            // 
            // _appReportBottom
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._appReportBottom, 3);
            this._appReportBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this._appReportBottom.Location = new System.Drawing.Point(3, 212);
            this._appReportBottom.Multiline = true;
            this._appReportBottom.Name = "_appReportBottom";
            this._appReportBottom.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._appReportBottom.Size = new System.Drawing.Size(884, 79);
            this._appReportBottom.TabIndex = 44;
            // 
            // label39
            // 
            this.label39.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label39.AutoSize = true;
            this.label39.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label39.Location = new System.Drawing.Point(477, 540);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(177, 13);
            this.label39.TabIndex = 52;
            this.label39.Text = "%%ReportLines%% - Строки отчета";
            // 
            // label38
            // 
            this.label38.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label38.AutoSize = true;
            this.label38.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label38.Location = new System.Drawing.Point(477, 527);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(228, 13);
            this.label38.TabIndex = 51;
            this.label38.Text = "%%NumberTopicsLast%% - Последний номер";
            // 
            // label37
            // 
            this.label37.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label37.AutoSize = true;
            this.label37.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label37.Location = new System.Drawing.Point(477, 514);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(228, 13);
            this.label37.TabIndex = 50;
            this.label37.Text = "%%NumberTopicsFirst%% - Начальный номер";
            // 
            // label36
            // 
            this.label36.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label36.AutoSize = true;
            this.label36.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label36.Location = new System.Drawing.Point(477, 501);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(197, 13);
            this.label36.TabIndex = 49;
            this.label36.Text = "%%Top1%% - Вписать первый шаблон";
            // 
            // label35
            // 
            this.label35.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label35.AutoSize = true;
            this.label35.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label35.Location = new System.Drawing.Point(237, 527);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(196, 13);
            this.label35.TabIndex = 48;
            this.label35.Text = "%%SizeTopics%% - Размер хранимого";
            // 
            // label34
            // 
            this.label34.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label34.AutoSize = true;
            this.label34.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label34.Location = new System.Drawing.Point(237, 514);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(180, 13);
            this.label34.TabIndex = 47;
            this.label34.Text = "%%CountTopics%% - Кол-во раздач";
            // 
            // label33
            // 
            this.label33.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label33.AutoSize = true;
            this.label33.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label33.Location = new System.Drawing.Point(237, 501);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(196, 13);
            this.label33.TabIndex = 46;
            this.label33.Text = "%%CreateDate%% - Дата составления";
            // 
            // label29
            // 
            this.label29.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label29.AutoSize = true;
            this.label29.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label29.Location = new System.Drawing.Point(5, 566);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(161, 13);
            this.label29.TabIndex = 39;
            this.label29.Text = "%%Date%% - дата регистрации";
            // 
            // label28
            // 
            this.label28.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label28.AutoSize = true;
            this.label28.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label28.Location = new System.Drawing.Point(5, 553);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(181, 13);
            this.label28.TabIndex = 38;
            this.label28.Text = "%%CountSeeders%% - кол-во сидов";
            // 
            // label27
            // 
            this.label27.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label27.AutoSize = true;
            this.label27.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label27.Location = new System.Drawing.Point(5, 527);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(151, 13);
            this.label27.TabIndex = 37;
            this.label27.Text = "%%Size%% - Размер раздачи";
            // 
            // label26
            // 
            this.label26.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label26.AutoSize = true;
            this.label26.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label26.Location = new System.Drawing.Point(5, 540);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(155, 13);
            this.label26.TabIndex = 36;
            this.label26.Text = "%%Status%% - статус раздачи";
            // 
            // label25
            // 
            this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label25.AutoSize = true;
            this.label25.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label25.Location = new System.Drawing.Point(5, 514);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(194, 13);
            this.label25.TabIndex = 35;
            this.label25.Text = "%%Name%% - наименование раздачи";
            // 
            // label24
            // 
            this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label24.AutoSize = true;
            this.label24.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label24.Location = new System.Drawing.Point(5, 501);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(181, 13);
            this.label24.TabIndex = 34;
            this.label24.Text = "%%ID%% - идентификатор раздачи";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.forumPages1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(975, 591);
            this.tabPage1.TabIndex = 5;
            this.tabPage1.Text = "Отправка отчетов на форум";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // forumPages1
            // 
            this.forumPages1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.forumPages1.Location = new System.Drawing.Point(0, 0);
            this.forumPages1.Name = "forumPages1";
            this.forumPages1.Size = new System.Drawing.Size(975, 591);
            this.forumPages1.TabIndex = 0;
            // 
            // _tpAllCategories
            // 
            this._tpAllCategories.Controls.Add(this.panel2);
            this._tpAllCategories.Location = new System.Drawing.Point(4, 22);
            this._tpAllCategories.Name = "_tpAllCategories";
            this._tpAllCategories.Padding = new System.Windows.Forms.Padding(3);
            this._tpAllCategories.Size = new System.Drawing.Size(975, 591);
            this._tpAllCategories.TabIndex = 7;
            this._tpAllCategories.Text = "Все категории";
            this._tpAllCategories.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.AutoScroll = true;
            this.panel2.Location = new System.Drawing.Point(0, 54);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(975, 537);
            this.panel2.TabIndex = 0;
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(8, 20);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(89, 13);
            this.label48.TabIndex = 30;
            this.label48.Text = "Хост рутрекера:";
            // 
            // rutrackerHost
            // 
            this.rutrackerHost.Location = new System.Drawing.Point(8, 41);
            this.rutrackerHost.Name = "rutrackerHost";
            this.rutrackerHost.Size = new System.Drawing.Size(286, 20);
            this.rutrackerHost.TabIndex = 31;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 649);
            this.Controls.Add(this._btCancel);
            this.Controls.Add(this._btCheck);
            this.Controls.Add(this._btSave);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1000, 688);
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройки";
            this.Load += new System.EventHandler(this.FormLoaded);
            this._tpCategories.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwCategories)).EndInit();
            this.tbpTorrentClients.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgwTorrentClients)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._appCountSeedersReport)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._appLogLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._appPeriodRunAndStopTorrents)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._appCountDaysKeepHistory)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.templatesTabPage3.ResumeLayout(false);
            this.templatesTabPage3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this._tpAllCategories.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox summaryReportTemplate;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox _appReportLine;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox _appReportTop1;
        private System.Windows.Forms.TextBox _appReportTop2;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TextBox _appReportBottom;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.TabPage templatesTabPage3;
        private System.Windows.Forms.CheckBox DisableCertVerifyCheck;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.ComboBox apiHosts;
        private System.Windows.Forms.TextBox proxyInput;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.CheckBox _dbLoadInMemoryCheckbox;
        private System.Windows.Forms.TabPage _tpAllCategories;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.TextBox _CategoriesTbLabel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.CheckBox _appIsNotSaveStatistics;
        private System.Windows.Forms.RadioButton _tcrbCurrent;
        private System.Windows.Forms.RadioButton _tcrbRemote;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.NumericUpDown _appLogLevel;
        private System.Windows.Forms.CheckBox _appSelectLessOrEqual;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.NumericUpDown _appCountSeedersReport;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.NumericUpDown _appCountDaysKeepHistory;
        private System.Windows.Forms.CheckBox _appIsUpdateStatistics;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown _appPeriodRunAndStopTorrents;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox _appIsAvgCountSeeders;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox _appKeeperName;
        private System.Windows.Forms.TextBox _appKeeperPass;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private TLO.Forms.ForumPages forumPages1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox _cbIsSaveWebPage;
        private System.Windows.Forms.CheckBox _cbIsSaveTorrentFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox _CategoriesCbTorrentClient;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TextBox _tbTorrentClientName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox _cbTorrentClientType;
        private System.Windows.Forms.DataGridViewTextBoxColumn FolderName;
        private System.Windows.Forms.DataGridViewTextBoxColumn UID;
        private System.Windows.Forms.DataGridView dgwTorrentClients;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox _tbTorrentClientPort;
        private System.Windows.Forms.TextBox _tbTorrentClientUserName;
        private System.Windows.Forms.TextBox _tbTorrentClientUserPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox _tbTorrentClientHostIP;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button _btTorrentClientDelete;
        private System.Windows.Forms.Button _btTorrentClientAdd;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TabPage tbpTorrentClients;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCategoryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCategoryCategoryID;
        private System.Windows.Forms.DataGridView dgwCategories;
        private System.Windows.Forms.Button _btCategoryAdd;
        private System.Windows.Forms.Button _btCategoryRemove;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox _CategoriesTbCategoryID;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox _CategoriesTbFullName;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox _CategoriesCbStartCountSeeders;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox _CategoriesTbFolderDownloads;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button _CategoriesBtSelectFolder;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.ComboBox _cbSubFolder;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabPage _tpCategories;
        private System.Windows.Forms.Button _btSave;
        private System.Windows.Forms.Button _btCancel;
        private System.Windows.Forms.Button _btCheck;
        private System.Windows.Forms.TextBox categoryReportTemplate;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.TextBox reportHeaderTemplate;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.CheckBox useProxyCheckBox;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Button ProxyAddButton;
        private System.Windows.Forms.ListBox ProxyListBox;
        private System.Windows.Forms.CheckBox SystemProxy;
        private GroupBox groupBox10;
        private CheckBox showTrayIcon;
        private CheckBox closeToTray;
        private CheckBox hideToTray;
        private CheckBox showNotificationInTray;
        private Label label47;
        private Label connectionCheck;
        private TextBox rutrackerHost;
        private Label label48;
    }
}
