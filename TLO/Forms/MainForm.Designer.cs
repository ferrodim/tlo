﻿using System.ComponentModel;
using System.Windows.Forms;

namespace TLO.Forms
{
    sealed partial class MainForm
    {
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
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 =
                new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 =
                new System.Windows.Forms.DataGridViewCellStyle();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.CreateConsolidatedReportByTorrentClientsToolStripMenuItem =
                new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.UpdateAll = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateCountSeedersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateListTopicsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateKeepTopicsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadListKeepersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ClearKeeperListsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ClearDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuTimerSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._cbCategory = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this._tpReportDownloads = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
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
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this._tpReportDownloads.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this._cbCountSeeders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this._dataGridTopicsList)).BeginInit();
            this.tabConsolidatedReport.SuspendLayout();
            this.ConsolidatedTorrentClientsReport.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this._tcCetegoriesRootReports.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                this.файлToolStripMenuItem, this.отчетыToolStripMenuItem, this.задачиToolStripMenuItem,
                this.menuSettingsToolStripMenuItem
            });
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(984, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                this.toolStripSeparator4, this._btSaveToFile, this._btLoadSettingsFromFile, this.toolStripSeparator3,
                this.ExitToolStripMenuItem
            });
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
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
            this.отчетыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
                {this.SendReportsToForumToolStripMenuItem, this.CreateReportsToolStripMenuItem});
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
            this.задачиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                this.RuningStopingDistributionToolStripMenuItem,
                this.CreateConsolidatedReportByTorrentClientsToolStripMenuItem, this.toolStripSeparator1,
                this.UpdateAll, this.UpdateCountSeedersToolStripMenuItem, this.UpdateListTopicsToolStripMenuItem,
                this.UpdateKeepTopicsToolStripMenuItem, this.LoadListKeepersToolStripMenuItem, this.toolStripSeparator2,
                this.ClearKeeperListsToolStripMenuItem, this.ClearDatabaseToolStripMenuItem, this.toolStripSeparator5,
                this.menuTimerSetting
            });
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
            this.CreateConsolidatedReportByTorrentClientsToolStripMenuItem.Name =
                "CreateConsolidatedReportByTorrentClientsToolStripMenuItem";
            this.CreateConsolidatedReportByTorrentClientsToolStripMenuItem.Size = new System.Drawing.Size(379, 22);
            this.CreateConsolidatedReportByTorrentClientsToolStripMenuItem.Text =
                "Построить сводный отчет по торрент-клиентам";
            this.CreateConsolidatedReportByTorrentClientsToolStripMenuItem.Click +=
                new System.EventHandler(this.MenuClick);
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
            this.UpdateAll.Click += new System.EventHandler(this.MenuClick);
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
            this.ClearKeeperListsToolStripMenuItem.Text = "Очистить списки хранителей со свод.значениями";
            this.ClearKeeperListsToolStripMenuItem.Click += new System.EventHandler(this.MenuClick);
            // 
            // ClearDatabaseToolStripMenuItem
            // 
            this.ClearDatabaseToolStripMenuItem.Name = "ClearDatabaseToolStripMenuItem";
            this.ClearDatabaseToolStripMenuItem.Size = new System.Drawing.Size(379, 22);
            this.ClearDatabaseToolStripMenuItem.Text = "Очистить списки разделов (удалить топики)";
            this.ClearDatabaseToolStripMenuItem.Click += new System.EventHandler(this.MenuClick);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(376, 6);
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
            // menuSettingsToolStripMenuItem
            // 
            this.menuSettingsToolStripMenuItem.Image =
                ((System.Drawing.Image) (resources.GetObject("menuSettingsToolStripMenuItem.Image")));
            this.menuSettingsToolStripMenuItem.Name = "menuSettingsToolStripMenuItem";
            this.menuSettingsToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.menuSettingsToolStripMenuItem.Text = "Настройки";
            this.menuSettingsToolStripMenuItem.Click += new System.EventHandler(this.MenuClick);
            // 
            // _cbCategory
            // 
            this._cbCategory.Anchor =
                ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top |
                                                        System.Windows.Forms.AnchorStyles.Left) |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this._cbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbCategory.FormattingEnabled = true;
            this._cbCategory.Location = new System.Drawing.Point(117, 27);
            this._cbCategory.Name = "_cbCategory";
            this._cbCategory.Size = new System.Drawing.Size(855, 23);
            this._cbCategory.TabIndex = 1;
            this._cbCategory.SelectionChangeCommitted += new System.EventHandler(this.SelectionChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Выберите раздел:";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top |
                                                         System.Windows.Forms.AnchorStyles.Bottom) |
                                                        System.Windows.Forms.AnchorStyles.Left) |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this._tpReportDownloads);
            this.tabControl1.Controls.Add(this.tabReports);
            this.tabControl1.Controls.Add(this.tabConsolidatedReport);
            this.tabControl1.Controls.Add(this.ConsolidatedTorrentClientsReport);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(0, 54);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(984, 483);
            this.tabControl1.TabIndex = 3;
            // 
            // _tpReportDownloads
            // 
            this._tpReportDownloads.Controls.Add(this.label7);
            this._tpReportDownloads.Controls.Add(this.label6);
            this._tpReportDownloads.Controls.Add(this.button1);
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
            this._tpReportDownloads.Location = new System.Drawing.Point(4, 24);
            this._tpReportDownloads.Name = "_tpReportDownloads";
            this._tpReportDownloads.Padding = new System.Windows.Forms.Padding(3);
            this._tpReportDownloads.Size = new System.Drawing.Size(976, 455);
            this._tpReportDownloads.TabIndex = 2;
            this._tpReportDownloads.Text = "Обработка раздела";
            this._tpReportDownloads.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(780, 412);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 15);
            this.label7.TabIndex = 35;
            this.label7.Text = "Прочее";
            // 
            // label6
            // 
            this.label6.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(780, 202);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(121, 15);
            this.label6.TabIndex = 34;
            this.label6.Text = "Действия по разделу";
            // 
            // button1
            // 
            this.button1.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(780, 428);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 23);
            this.button1.TabIndex = 33;
            this.button1.Text = "Неизвестные в файл";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ExportUnknown_Click);
            // 
            // _DateRegistration
            // 
            this._DateRegistration.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._DateRegistration.Location = new System.Drawing.Point(63, 6);
            this._DateRegistration.Name = "_DateRegistration";
            this._DateRegistration.Size = new System.Drawing.Size(93, 23);
            this._DateRegistration.TabIndex = 32;
            this._DateRegistration.ValueChanged += new System.EventHandler(this.SelectionChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 15);
            this.label5.TabIndex = 31;
            this.label5.Text = "Дата до:";
            // 
            // _cbCountSeeders
            // 
            this._cbCountSeeders.Location = new System.Drawing.Point(245, 6);
            this._cbCountSeeders.Minimum = new decimal(new int[] {1, 0, 0, -2147483648});
            this._cbCountSeeders.Name = "_cbCountSeeders";
            this._cbCountSeeders.Size = new System.Drawing.Size(40, 23);
            this._cbCountSeeders.TabIndex = 30;
            this._cbCountSeeders.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._cbCountSeeders.ValueChanged += new System.EventHandler(this.SelectionChanged);
            // 
            // _lbTotal
            // 
            this._lbTotal.AutoSize = true;
            this._lbTotal.Location = new System.Drawing.Point(5, 30);
            this._lbTotal.Name = "_lbTotal";
            this._lbTotal.Size = new System.Drawing.Size(43, 15);
            this._lbTotal.TabIndex = 29;
            this._lbTotal.Text = "Итого:";
            // 
            // _llUpdateTopicsByCategory
            // 
            this._llUpdateTopicsByCategory.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this._llUpdateTopicsByCategory.AutoSize = true;
            this._llUpdateTopicsByCategory.Location = new System.Drawing.Point(780, 267);
            this._llUpdateTopicsByCategory.Name = "_llUpdateTopicsByCategory";
            this._llUpdateTopicsByCategory.Size = new System.Drawing.Size(166, 15);
            this._llUpdateTopicsByCategory.TabIndex = 28;
            this._llUpdateTopicsByCategory.TabStop = true;
            this._llUpdateTopicsByCategory.Text = "Обновить список по разделу";
            this._llUpdateTopicsByCategory.Click += new System.EventHandler(this.LinkClick);
            // 
            // _llUpdateCountSeedersByCategory
            // 
            this._llUpdateCountSeedersByCategory.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this._llUpdateCountSeedersByCategory.AutoSize = true;
            this._llUpdateCountSeedersByCategory.Location = new System.Drawing.Point(780, 244);
            this._llUpdateCountSeedersByCategory.Name = "_llUpdateCountSeedersByCategory";
            this._llUpdateCountSeedersByCategory.Size = new System.Drawing.Size(200, 15);
            this._llUpdateCountSeedersByCategory.TabIndex = 27;
            this._llUpdateCountSeedersByCategory.TabStop = true;
            this._llUpdateCountSeedersByCategory.Text = "Обновить кол-во сидов по разделу";
            this._llUpdateCountSeedersByCategory.Click += new System.EventHandler(this.LinkClick);
            // 
            // _llUpdateDataDromTorrentClient
            // 
            this._llUpdateDataDromTorrentClient.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this._llUpdateDataDromTorrentClient.AutoSize = true;
            this._llUpdateDataDromTorrentClient.Location = new System.Drawing.Point(780, 290);
            this._llUpdateDataDromTorrentClient.Name = "_llUpdateDataDromTorrentClient";
            this._llUpdateDataDromTorrentClient.Size = new System.Drawing.Size(201, 15);
            this._llUpdateDataDromTorrentClient.TabIndex = 26;
            this._llUpdateDataDromTorrentClient.TabStop = true;
            this._llUpdateDataDromTorrentClient.Text = "Обновить инф. из торрент-клиента";
            this._llUpdateDataDromTorrentClient.Click += new System.EventHandler(this.LinkClick);
            // 
            // label4
            // 
            this.label4.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(780, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(149, 15);
            this.label4.TabIndex = 23;
            this.label4.Text = "Действия с выделенными";
            // 
            // linkLabel5
            // 
            this.linkLabel5.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel5.AutoSize = true;
            this.linkLabel5.Location = new System.Drawing.Point(780, 126);
            this.linkLabel5.Name = "linkLabel5";
            this.linkLabel5.Size = new System.Drawing.Size(201, 15);
            this.linkLabel5.TabIndex = 22;
            this.linkLabel5.TabStop = true;
            this.linkLabel5.Text = "Удалить из Torrent-клиента+файлы";
            this.linkLabel5.Visible = false;
            // 
            // linkSetNewLabel
            // 
            this.linkSetNewLabel.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this.linkSetNewLabel.AutoSize = true;
            this.linkSetNewLabel.Location = new System.Drawing.Point(780, 104);
            this.linkSetNewLabel.Name = "linkSetNewLabel";
            this.linkSetNewLabel.Size = new System.Drawing.Size(104, 15);
            this.linkSetNewLabel.TabIndex = 21;
            this.linkSetNewLabel.TabStop = true;
            this.linkSetNewLabel.Text = "Установить метку";
            this.linkSetNewLabel.Click += new System.EventHandler(this.LinkClick);
            // 
            // _llSelectedTopicsDeleteFromBlackList
            // 
            this._llSelectedTopicsDeleteFromBlackList.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this._llSelectedTopicsDeleteFromBlackList.AutoSize = true;
            this._llSelectedTopicsDeleteFromBlackList.Location = new System.Drawing.Point(780, 170);
            this._llSelectedTopicsDeleteFromBlackList.Name = "_llSelectedTopicsDeleteFromBlackList";
            this._llSelectedTopicsDeleteFromBlackList.Size = new System.Drawing.Size(156, 15);
            this._llSelectedTopicsDeleteFromBlackList.TabIndex = 20;
            this._llSelectedTopicsDeleteFromBlackList.TabStop = true;
            this._llSelectedTopicsDeleteFromBlackList.Text = "Удалить из черного списка";
            this._llSelectedTopicsDeleteFromBlackList.Click += new System.EventHandler(this.LinkClick);
            // 
            // _llSelectedTopicsToTorrentClient
            // 
            this._llSelectedTopicsToTorrentClient.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this._llSelectedTopicsToTorrentClient.AutoSize = true;
            this._llSelectedTopicsToTorrentClient.Location = new System.Drawing.Point(780, 81);
            this._llSelectedTopicsToTorrentClient.Name = "_llSelectedTopicsToTorrentClient";
            this._llSelectedTopicsToTorrentClient.Size = new System.Drawing.Size(151, 15);
            this._llSelectedTopicsToTorrentClient.TabIndex = 19;
            this._llSelectedTopicsToTorrentClient.TabStop = true;
            this._llSelectedTopicsToTorrentClient.Text = "Добавить в Torrent-клиент";
            this._llSelectedTopicsToTorrentClient.Click += new System.EventHandler(this.LinkClick);
            // 
            // _llDownloadSelectTopics
            // 
            this._llDownloadSelectTopics.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this._llDownloadSelectTopics.AutoSize = true;
            this._llDownloadSelectTopics.Location = new System.Drawing.Point(780, 58);
            this._llDownloadSelectTopics.Name = "_llDownloadSelectTopics";
            this._llDownloadSelectTopics.Size = new System.Drawing.Size(134, 15);
            this._llDownloadSelectTopics.TabIndex = 18;
            this._llDownloadSelectTopics.TabStop = true;
            this._llDownloadSelectTopics.Text = "Скачать Torrent-файлы";
            this._llDownloadSelectTopics.Click += new System.EventHandler(this.LinkClick);
            // 
            // _llSelectedTopicsToBlackList
            // 
            this._llSelectedTopicsToBlackList.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this._llSelectedTopicsToBlackList.AutoSize = true;
            this._llSelectedTopicsToBlackList.Location = new System.Drawing.Point(780, 148);
            this._llSelectedTopicsToBlackList.Name = "_llSelectedTopicsToBlackList";
            this._llSelectedTopicsToBlackList.Size = new System.Drawing.Size(156, 15);
            this._llSelectedTopicsToBlackList.TabIndex = 17;
            this._llSelectedTopicsToBlackList.TabStop = true;
            this._llSelectedTopicsToBlackList.Text = "Добавить в черный список";
            this._llSelectedTopicsToBlackList.Click += new System.EventHandler(this.LinkClick);
            // 
            // _cbBlackList
            // 
            this._cbBlackList.AutoSize = true;
            this._cbBlackList.Location = new System.Drawing.Point(522, 9);
            this._cbBlackList.Name = "_cbBlackList";
            this._cbBlackList.Size = new System.Drawing.Size(112, 19);
            this._cbBlackList.TabIndex = 14;
            this._cbBlackList.Text = "Черный список";
            this._cbBlackList.UseVisualStyleBackColor = true;
            this._cbBlackList.CheckedChanged += new System.EventHandler(this.SelectionChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(290, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 15);
            this.label2.TabIndex = 12;
            this.label2.Text = "Фильтр:";
            // 
            // _cbCategoryFilters
            // 
            this._cbCategoryFilters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbCategoryFilters.FormattingEnabled = true;
            this._cbCategoryFilters.Items.AddRange(new object[]
            {
                "Все", "Не скачан торрент и нет хранителя", "Не скачан торрент", "Храню", "Храню и есть хранитель",
                "Не храню", "Скачиваю раздачу", "Я релизер", "Не скачано"
            });
            this._cbCategoryFilters.Location = new System.Drawing.Point(346, 5);
            this._cbCategoryFilters.Name = "_cbCategoryFilters";
            this._cbCategoryFilters.Size = new System.Drawing.Size(170, 23);
            this._cbCategoryFilters.TabIndex = 11;
            this._cbCategoryFilters.SelectionChangeCommitted += new System.EventHandler(this.SelectionChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(162, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "Кол-во сидов:";
            // 
            // _dataGridTopicsList
            // 
            this._dataGridTopicsList.AllowUserToAddRows = false;
            this._dataGridTopicsList.AllowUserToDeleteRows = false;
            this._dataGridTopicsList.AllowUserToResizeRows = false;
            this._dataGridTopicsList.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top |
                                                         System.Windows.Forms.AnchorStyles.Bottom) |
                                                        System.Windows.Forms.AnchorStyles.Left) |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this._dataGridTopicsList.ColumnHeadersHeightSizeMode =
                System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dataGridTopicsList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                this.ColumnReport1DgvTopicID, this.ColumnReport1DgvSelect, this.ColumnReport1DgvStatus,
                this.ColumnReport1DgvSize, this.ColumnReport1DgvName, this.ColumnReport1DgvAlternative,
                this.ColumnReport1DgvSeeders, this.ColumnReport1DgvAvgSeeders, this.ColumnReport1DgvRegTime,
                this.ColumnReport1DgvKeeperCount, this.ColumnReport1DgvBlack
            });
            this._dataGridTopicsList.Location = new System.Drawing.Point(8, 48);
            this._dataGridTopicsList.MultiSelect = false;
            this._dataGridTopicsList.Name = "_dataGridTopicsList";
            this._dataGridTopicsList.RowHeadersVisible = false;
            this._dataGridTopicsList.Size = new System.Drawing.Size(766, 401);
            this._dataGridTopicsList.TabIndex = 0;
            this._dataGridTopicsList.CellContentClick +=
                new System.Windows.Forms.DataGridViewCellEventHandler(this.ContentClick);
            this._dataGridTopicsList.CellDoubleClick +=
                new System.Windows.Forms.DataGridViewCellEventHandler(this._dgvReportDownloads_CellDoubleClick);
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
            this.ColumnReport1DgvSize.Width = 72;
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
            this.ColumnReport1DgvSeeders.AutoSizeMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColumnReport1DgvSeeders.DataPropertyName = "Seeders";
            this.ColumnReport1DgvSeeders.HeaderText = "Сиды";
            this.ColumnReport1DgvSeeders.Name = "ColumnReport1DgvSeeders";
            this.ColumnReport1DgvSeeders.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ColumnReport1DgvSeeders.Width = 62;
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
            this.tabReports.Size = new System.Drawing.Size(976, 457);
            this.tabReports.TabIndex = 3;
            this.tabReports.Text = "Отчеты";
            this.tabReports.UseVisualStyleBackColor = true;
            // 
            // tabConsolidatedReport
            // 
            this.tabConsolidatedReport.Controls.Add(this._txtConsolidatedReport);
            this.tabConsolidatedReport.Location = new System.Drawing.Point(4, 22);
            this.tabConsolidatedReport.Name = "tabConsolidatedReport";
            this.tabConsolidatedReport.Size = new System.Drawing.Size(976, 457);
            this.tabConsolidatedReport.TabIndex = 0;
            this.tabConsolidatedReport.Text = "Сводный отчет";
            this.tabConsolidatedReport.UseVisualStyleBackColor = true;
            // 
            // _txtConsolidatedReport
            // 
            this._txtConsolidatedReport.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top |
                                                         System.Windows.Forms.AnchorStyles.Bottom) |
                                                        System.Windows.Forms.AnchorStyles.Left) |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this._txtConsolidatedReport.Location = new System.Drawing.Point(0, 0);
            this._txtConsolidatedReport.Multiline = true;
            this._txtConsolidatedReport.Name = "_txtConsolidatedReport";
            this._txtConsolidatedReport.Size = new System.Drawing.Size(976, 461);
            this._txtConsolidatedReport.TabIndex = 0;
            // 
            // ConsolidatedTorrentClientsReport
            // 
            this.ConsolidatedTorrentClientsReport.Controls.Add(this._tbConsolidatedTorrentClientsReport);
            this.ConsolidatedTorrentClientsReport.Location = new System.Drawing.Point(4, 22);
            this.ConsolidatedTorrentClientsReport.Name = "ConsolidatedTorrentClientsReport";
            this.ConsolidatedTorrentClientsReport.Padding = new System.Windows.Forms.Padding(3);
            this.ConsolidatedTorrentClientsReport.Size = new System.Drawing.Size(976, 457);
            this.ConsolidatedTorrentClientsReport.TabIndex = 5;
            this.ConsolidatedTorrentClientsReport.Text = "Отчет torrent-клиентов";
            this.ConsolidatedTorrentClientsReport.UseVisualStyleBackColor = true;
            // 
            // _tbConsolidatedTorrentClientsReport
            // 
            this._tbConsolidatedTorrentClientsReport.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top |
                                                         System.Windows.Forms.AnchorStyles.Bottom) |
                                                        System.Windows.Forms.AnchorStyles.Left) |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this._tbConsolidatedTorrentClientsReport.Location = new System.Drawing.Point(0, 0);
            this._tbConsolidatedTorrentClientsReport.Multiline = true;
            this._tbConsolidatedTorrentClientsReport.Name = "_tbConsolidatedTorrentClientsReport";
            this._tbConsolidatedTorrentClientsReport.ReadOnly = true;
            this._tbConsolidatedTorrentClientsReport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._tbConsolidatedTorrentClientsReport.Size = new System.Drawing.Size(976, 454);
            this._tbConsolidatedTorrentClientsReport.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this._tcCetegoriesRootReports);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(976, 457);
            this.tabPage1.TabIndex = 4;
            this.tabPage1.Text = "Отчет по разделам";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // _tcCetegoriesRootReports
            // 
            this._tcCetegoriesRootReports.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top |
                                                         System.Windows.Forms.AnchorStyles.Bottom) |
                                                        System.Windows.Forms.AnchorStyles.Left) |
                                                       System.Windows.Forms.AnchorStyles.Right)));
            this._tcCetegoriesRootReports.Controls.Add(this.tabPage2);
            this._tcCetegoriesRootReports.Controls.Add(this.tabPage3);
            this._tcCetegoriesRootReports.Location = new System.Drawing.Point(1, 1);
            this._tcCetegoriesRootReports.Name = "_tcCetegoriesRootReports";
            this._tcCetegoriesRootReports.SelectedIndex = 0;
            this._tcCetegoriesRootReports.Size = new System.Drawing.Size(975, 459);
            this._tcCetegoriesRootReports.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(967, 431);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(967, 433);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
                {this.toolStripStatusLabel1, this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 539);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(984, 22);
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
            // MainForm
            // 
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._cbCategory);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(3840, 2160);
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TLO";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FireFormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this._tpReportDownloads.ResumeLayout(false);
            this._tpReportDownloads.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this._cbCountSeeders)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this._dataGridTopicsList)).EndInit();
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

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem отчетыToolStripMenuItem;
        private System.Windows.Forms.ComboBox _cbCategory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage _tpReportDownloads;
        private System.Windows.Forms.CheckBox _cbBlackList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox _cbCategoryFilters;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel _llSelectedTopicsToTorrentClient;
        private System.Windows.Forms.LinkLabel _llDownloadSelectTopics;
        private System.Windows.Forms.LinkLabel _llSelectedTopicsToBlackList;
        private System.Windows.Forms.LinkLabel _llSelectedTopicsDeleteFromBlackList;
        private System.Windows.Forms.LinkLabel linkSetNewLabel;
        private System.Windows.Forms.LinkLabel linkLabel5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel _llUpdateTopicsByCategory;
        private System.Windows.Forms.LinkLabel _llUpdateCountSeedersByCategory;
        private System.Windows.Forms.LinkLabel _llUpdateDataDromTorrentClient;
        private System.Windows.Forms.ToolStripMenuItem задачиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem UpdateCountSeedersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem UpdateListTopicsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem UpdateKeepTopicsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ClearDatabaseToolStripMenuItem;
        private System.Windows.Forms.Label _lbTotal;
        private System.Windows.Forms.ToolStripMenuItem SendReportsToForumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CreateReportsToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripMenuItem RuningStopingDistributionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.NumericUpDown _cbCountSeeders;
        private System.Windows.Forms.TabPage tabReports;
        private System.Windows.Forms.ToolStripMenuItem ClearKeeperListsToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabConsolidatedReport;
        private System.Windows.Forms.TextBox _txtConsolidatedReport;
        private System.Windows.Forms.TabPage ConsolidatedTorrentClientsReport;
        private System.Windows.Forms.TextBox _tbConsolidatedTorrentClientsReport;
        private System.Windows.Forms.ToolStripMenuItem CreateConsolidatedReportByTorrentClientsToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnReport1DgvTopicID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnReport1DgvSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnReport1DgvStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnReport1DgvSize;
        private System.Windows.Forms.DataGridViewLinkColumn ColumnReport1DgvName;
        private System.Windows.Forms.DataGridViewLinkColumn ColumnReport1DgvAlternative;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnReport1DgvSeeders;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnReport1DgvAvgSeeders;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnReport1DgvRegTime;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnReport1DgvBlack;
        private System.Windows.Forms.DateTimePicker _DateRegistration;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl _tcCetegoriesRootReports;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ToolStripMenuItem LoadListKeepersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _btSaveToFile;
        private System.Windows.Forms.ToolStripMenuItem _btLoadSettingsFromFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.DataGridView _dataGridTopicsList;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem UpdateAll;
        private System.Windows.Forms.ToolStripMenuItem menuTimerSetting;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnReport1DgvKeeperCount;
    }
}