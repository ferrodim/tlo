using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NLog;
using TLO.Clients;
using TLO.Forms;
using TLO.Info;

namespace TLO
{
    internal static class WorkerMethods
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static Logger logger = LogManager.GetCurrentClassLogger();

        public static void bwDownloadTorrentFiles(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            var num1 = new decimal(0, 0, 0, false, 1);
            backgroundWorker.ReportProgress((int) num1);
            try
            {
                var tuple = e.Argument as Tuple<List<TopicInfo>, MainForm>;
                var topicInfoList = tuple.Item1;
                var folder = string.Empty;
                if (topicInfoList == null || topicInfoList.Count == 0)
                    return;
                tuple.Item2.Invoke((MethodInvoker) delegate
                {
                    var folderBrowserDialog = new FolderBrowserDialog();
                    if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                        return;
                    folder = folderBrowserDialog.SelectedPath;
                });
                if (string.IsNullOrWhiteSpace(folder))
                {
                    int num2;
                    tuple.Item2.Invoke((MethodInvoker) delegate
                    {
                        num2 = (int) MessageBox.Show("Не указан каталог для сохранения торрент-файлов", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                    });
                }
                else
                {
                    foreach (var topicInfo in topicInfoList)
                    {
                        var buffer = RuTrackerOrg.Current.DownloadTorrentFile(topicInfo.TopicID);
                        if (buffer != null)
                        {
                            using (var fileStream = File.Create(Path.Combine(folder,
                                string.Format("[rutracker.org].t{0}.torrent", topicInfo.TopicID))))
                            {
                                fileStream.Write(buffer, 0, buffer.Count());
                            }

                            num1 += new decimal(1000, 0, 0, false, 1) / topicInfoList.Count;
                            backgroundWorker.ReportProgress((int) num1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                Logger.Debug(ex.StackTrace);
                Logger.Debug(ex);
                var num2 = (int) MessageBox.Show("Произошла ошибка при скачивании торрент-файлов:\r\n" + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        public static void bwSendTorrentFileToTorrentClient(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            var num1 = new decimal(0, 0, 0, false, 1);
            backgroundWorker.ReportProgress((int) num1);
            try
            {
                var tuple =
                    e.Argument as Tuple<MainForm, List<TopicInfo>, Category>;
                var topicInfoList = tuple.Item2;
                var category = tuple.Item3;
                Logger.Info("Запущена задача на скачивание и добавление торрент-файлов в торрент-клиент...");
                Logger.Trace(string.Format("\tКол-во раздач для скачивания торрент-файлов: {0}",
                    topicInfoList.Count));
                var torrentClientInfo = ClientLocalDb.Current.GetTorrentClients()
                    .Where(x => x.UID == category.TorrentClientUID)
                    .FirstOrDefault();
                var source = torrentClientInfo.Create().GetAllTorrentHash()
                    .Where(x => !string.IsNullOrWhiteSpace(x.Hash));
                foreach (var topicInfo1 in topicInfoList)
                {
                    var t = topicInfo1;
                    var topicInfo2 = source.Where(x => x.Hash == t.Hash)
                        .FirstOrDefault();
                    if (topicInfo2 != null)
                        t.TorrentName = topicInfo2.TorrentName;
                }

                var list = source.Select(x => x.Hash)
                    .ToList();
                if (torrentClientInfo == null)
                {
                    Logger.Warn("Не указан торрент-клиент в категории/подфоруме");
                }
                else
                {
                    var folder = category.Folder;
                    if (string.IsNullOrWhiteSpace(folder))
                        tuple.Item1.Invoke((MethodInvoker) delegate
                        {
                            var folderBrowserDialog = new FolderBrowserDialog();
                            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                                return;
                            folder = folderBrowserDialog.SelectedPath;
                        });
                    if (string.IsNullOrWhiteSpace(folder))
                        throw new Exception("Не указан каталог для загрузки");
                    foreach (var topicInfo in topicInfoList)
                    {
                        var t = topicInfo;
                        try
                        {
                            if (t.Status != 7)
                            {
                                if (t.Status != 4)
                                {
                                    var folder2 = string.Empty;
                                    if (category.CreateSubFolder == 0)
                                    {
                                        folder2 = folder;
                                    }
                                    else if (category.CreateSubFolder == 1)
                                    {
                                        folder2 = Path.Combine(folder, t.TopicID.ToString());
                                    }
                                    else
                                    {
                                        if (category.CreateSubFolder != 2)
                                            throw new Exception(
                                                "Не поддерживается указаный метод создания подкаталога");
                                        var result = DialogResult.None;
                                        tuple.Item1.Invoke((MethodInvoker) delegate
                                        {
                                            var folderNameDialog = new FolderNameDialog();
                                            folderNameDialog.SelectedPath = t.Name;
                                            result = folderNameDialog.ShowDialog();
                                            folder2 = Path.Combine(folder, folderNameDialog.SelectedPath);
                                        });
                                        if (result == DialogResult.Abort)
                                            return;
                                        if (result != DialogResult.Cancel)
                                        {
                                            if (result != DialogResult.OK)
                                                throw new Exception("result != DialogResult.OK");
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }

                                    if (!list.Contains(t.Hash))
                                    {
                                        var torrentClient = torrentClientInfo.Create();
                                        if (torrentClient == null)
                                            throw new ArgumentException(
                                                "Не удалось создать подключение к торрент-клиенту \"" +
                                                torrentClientInfo.Name + "\"");
                                        torrentClient.SetDefaultFolder(folder2);
                                        var numArray = RuTrackerOrg.Current.DownloadTorrentFile(t.TopicID);
                                        if (numArray == null)
                                        {
                                            Logger.Warn("Не удалось скачать торрент-файл для раздачи \"" +
                                                        t.Name + "\". Статус раздачи: " + t.Status);
                                            continue;
                                        }

                                        torrentClient.SendTorrentFile(folder2,
                                            string.Format("[rutracker.org].t{0}.torrent", t.TopicID),
                                            numArray);
                                        torrentClient.SetLabel(t.Hash,
                                            string.IsNullOrWhiteSpace(category.Label)
                                                ? category.FullName
                                                : category.Label);
                                        if (category.IsSaveTorrentFiles)
                                        {
                                            if (!Directory.Exists(category.FolderTorrentFile))
                                                Directory.CreateDirectory(category.FolderTorrentFile);
                                            using (var fileStream = File.Create(
                                                Path.Combine(category.FolderTorrentFile,
                                                    string.Format("[rutracker.org].t{0}.torrent", t.TopicID))))
                                            {
                                                fileStream.Write(numArray, 0,
                                                    numArray.Count());
                                            }
                                        }
                                    }

                                    if (category.IsSaveWebPage)
                                    {
                                        Thread.Sleep(500);
                                        var buffer = RuTrackerOrg.Current.DownloadWebPages(
                                            string.Format("https://{1}/forum/viewtopic.php?t={0}", t.TopicID,
                                                Settings.Current.HostRuTrackerOrg));
                                        if (!Directory.Exists(category.FolderSavePageForum))
                                            Directory.CreateDirectory(category.FolderSavePageForum);
                                        using (var fileStream = File.Create(
                                            Path.Combine(category.FolderSavePageForum,
                                                string.Format("[rutracker.org].t{0}.html", t.TopicID))))
                                        {
                                            fileStream.Write(buffer, 0, buffer.Count());
                                        }
                                    }

                                    // TODO понять нужна ли ниже закомментированная логика
                                    // if (!string.IsNullOrWhiteSpace(t.TorrentName))
                                    // {
                                    //     if (Directory.Exists(Path.Combine(category.Folder, t.TorrentName)))
                                    //     {
                                    //         if (!Directory.Exists(Path.Combine(category.Folder,
                                    //             t.TopicID.ToString())))
                                    //             Directory.CreateDirectory(Path.Combine(category.Folder,
                                    //                 t.TopicID.ToString()));
                                    //         Directory.Move(Path.Combine(category.Folder, t.TorrentName),
                                    //             Path.Combine(category.Folder, t.TopicID.ToString(), t.TorrentName));
                                    //         continue;
                                    //     }
                                    //
                                    //     if (File.Exists(Path.Combine(category.Folder, t.TorrentName)))
                                    //     {
                                    //         if (!Directory.Exists(Path.Combine(category.Folder,
                                    //             t.TopicID.ToString())))
                                    //             Directory.CreateDirectory(Path.Combine(category.Folder,
                                    //                 t.TopicID.ToString()));
                                    //         File.Move(Path.Combine(category.Folder, t.TorrentName),
                                    //             Path.Combine(category.Folder, t.TopicID.ToString(), t.TorrentName));
                                    //         continue;
                                    //     }
                                    // }
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex);
                            Logger.Error(ex.StackTrace);
                            Logger.Warn(
                                "Не удалось скачать или добавить в торрент-клиент торрент-файл для раздачи \"" +
                                t.Name + "\". Статус раздачи: " + t.Status + "\t\t" + ex.Message);
                        }

                        num1 += new decimal(1000, 0, 0, false, 1) / topicInfoList.Count;
                        backgroundWorker.ReportProgress((int) num1);
                    }

                    Logger.Info("Завершена задача на скачивание и добавление торрент-файлов в торрент-клиент.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Произошла ошибка при скачивании и добавлении торрент-файлов в торрент-клиент: " +
                             ex.Message);
                Logger.Debug(ex);
                var num2 = (int) MessageBox.Show("Поизошла ошибка при скачивании торрент-файлов:\r\n" + ex.Message,
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        public static void bwSetLabels(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            var num1 = new decimal(0, 0, 0, false, 1);
            backgroundWorker.ReportProgress((int) num1);
            try
            {
                var tuple = e.Argument as Tuple<MainForm, List<TopicInfo>, string>;
                var topicInfoList = tuple.Item2;
                var label = tuple.Item3;
//        logger.Info("Запущена задача на установку пользовательских меток в торрент-клиент...");
                var torrentClients = ClientLocalDb.Current.GetTorrentClients();
                foreach (var torrentClientInfo in torrentClients)
                {
                    var torrentClient = torrentClientInfo.Create();
                    torrentClient.SetLabel(
                        torrentClient.GetAllTorrentHash()
                            .Join(topicInfoList, tc => tc.Hash, tp => tp.Hash, (tc, tp) => tp.Hash).ToArray(),
                        label);
                    num1 += new decimal(1000, 0, 0, false, 1) / torrentClients.Count();
                    if (num1 <= new decimal(100))
                        backgroundWorker.ReportProgress((int) num1);
                }

                backgroundWorker.ReportProgress(100);
            }
            catch (Exception ex)
            {
                logger.Error("Произошла ошибка при установке пользовательских меток в торрент-клиент: " + ex.Message);
                logger.Debug(ex);
                var num2 = (int) MessageBox.Show("Произошла ошибка:\r\n" + ex.Message, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        public static void bwUpdateCountSeedersByAllCategories(object sender, DoWorkEventArgs e)
        {
            logger.Info("Запущена задача на обновление информации о кол-ве сидов на раздачах...");
            var backgroundWorker = sender as BackgroundWorker;
            var num = new decimal(0, 0, 0, false, 1);
            backgroundWorker.ReportProgress((int) num);
            try
            {
                logger.Trace("\t Очищаем историю о кол-ве сидов на раздаче...");
                ClientLocalDb.Current.ClearHistoryStatus();
                var categoriesEnable = ClientLocalDb.Current.GetCategoriesEnable();
                foreach (var category in categoriesEnable)
                {
                    logger.Trace("\t " + category.Name + "...");
                    try
                    {
                        ClientLocalDb.Current.SaveStatus(RuTrackerOrg.Current.GetTopicsStatus(category.CategoryID),
                            true);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn("Не удалось обновить кол-во сидов по разделу \"" + category.Name + "\"");
                        logger.Debug(ex);
                    }

                    num += new decimal(1000, 0, 0, false, 1) / categoriesEnable.Count;
                    backgroundWorker.ReportProgress((int) num);
                }

                if (Settings.Current.IsUpdateStatistics)
                {
                    logger.Trace("\t Обновление статистики...");
                    ClientLocalDb.Current.UpdateStatistics();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Debug(ex);
            }

            logger.Info("Завершена задача по обновлению информации о кол-ве сидов на раздачах.");
        }

        public static void bwUpdateHashFromAllTorrentClients(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            var num1 = new decimal(0, 0, 0, false, 1);
            backgroundWorker.ReportProgress((int) num1);
            try
            {
                ClientLocalDb.Current.ResetFlagsTopicDownloads();
                var torrentClients = ClientLocalDb.Current.GetTorrentClients();
                foreach (var torrentClientInfo in torrentClients)
                {
                    try
                    {
                        var torrentClient = torrentClientInfo.Create();
                        if (torrentClient != null)
                            ClientLocalDb.Current.SetTorrentClientHash(torrentClient.GetAllTorrentHash());
                    }
                    catch (Exception ex)
                    {
                        logger.Warn("Не удалось загрузить список статусов раздач из torrent-клиента \"" +
                                    torrentClientInfo.Name + "\": \"" + ex.Message +
                                    "\". Возможно клиент не запущен или нет доступа.");
                        logger.Debug(ex);
                    }

                    num1 += new decimal(1000, 0, 0, false, 1) / torrentClients.Count;
                    backgroundWorker.ReportProgress((int) num1);
                }

                Reports.CreateReports();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Debug(ex);
                var num2 = (int) MessageBox.Show("Произошла ошибка при отправке отчетов:\r\n" + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        public static void bwUpdateHashFromTorrentClientsByCategoryUID(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            var num1 = new decimal(0, 0, 0, false, 1);
            backgroundWorker.ReportProgress((int) num1);
            try
            {
                var category = e.Argument as Category;
                if (category == null)
                    return;
                logger.Info("Обновление списка хранимого из торрент-клиента (по разделу)...");
                var list = ClientLocalDb.Current.GetTorrentClients()
                    .Where(x => x.UID == category.TorrentClientUID).ToList();
                foreach (var torrentClientInfo in list)
                {
                    try
                    {
                        var torrentClient = torrentClientInfo.Create();
                        if (torrentClient != null)
                            ClientLocalDb.Current.SetTorrentClientHash(torrentClient.GetAllTorrentHash());
                    }
                    catch (Exception ex)
                    {
                        logger.Warn("Не удалось загрузить список статусов раздач из torrent-клиента \"" +
                                    torrentClientInfo.Name + "\": \"" + ex.Message +
                                    "\". Возможно клиент не запущен или нет доступа.");
                        logger.Debug(ex);
                    }

                    num1 += new decimal(1000, 0, 0, false, 1) / list.Count;
                    backgroundWorker.ReportProgress((int) num1);
                }

                Reports.CreateReports();
                logger.Info("Завершена задача по обновлению списка хранимого из торрент-клиента (по разделу).");
            }
            catch (Exception ex)
            {
                logger.Error("Произошла ошибка при обновлении списка хранимого из торрент-клиента: " + ex.Message);
                logger.Debug(ex);
                var num2 = (int) MessageBox.Show("Произошла ошибка при отправке отчетов:\r\n" + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        public static void bwUpdateTopicsByCategory(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            var category = e.Argument as Category;
            var num1 = new decimal(0, 0, 0, false, 1);
            backgroundWorker.ReportProgress((int) num1);
            try
            {
                var array = RuTrackerOrg.Current.GetTopicsStatus(category.CategoryID).Select(x => x[0]).Distinct()
                    .ToArray();
                var intListArray =
                    new List<int>[array.Length % 100 == 0 ? array.Length / 100 : array.Length / 100 + 1];
                for (var index1 = 0; index1 < array.Length; ++index1)
                {
                    var index2 = index1 / 100;
                    if (intListArray[index2] == null)
                        intListArray[index2] = new List<int>();
                    intListArray[index2].Add(array[index1]);
                }

                foreach (var intList in intListArray)
                {
                    ClientLocalDb.Current.SaveTopicInfo(RuTrackerOrg.Current.GetTopicsInfo(intList.ToArray()), true);
                    num1 += new decimal(1000, 0, 0, false, 1) / intListArray.Length;
                    backgroundWorker.ReportProgress((int) num1);
                }

                ClientLocalDb.Current.SaveUsers(RuTrackerOrg.Current.GetUsers(ClientLocalDb.Current.GetNoUsers()));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Debug(ex);
                var num2 = (int) MessageBox.Show("Произошла ошибка при отправке отчетов:\r\n" + ex.Message, "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        public static void bwUpdateTopicsByCategories(object sender, DoWorkEventArgs e)
        {
            logger.Info("Запущена задача по обновлению топиков...");
            var backgroundWorker = sender as BackgroundWorker;
            var categoryList = e.Argument as List<Category>;
            var num1 = new decimal(0, 0, 0, false, 1);
            backgroundWorker.ReportProgress((int) num1);
            try
            {
                foreach (var category in categoryList)
                {
                    logger.Trace("\t Обрабатывается форум \"" + category.Name + "\"...");
                    try
                    {
                        var array = RuTrackerOrg.Current.GetTopicsStatus(category.CategoryID).Select(x => x[0])
                            .Distinct().ToArray();
                        var intListArray =
                            new List<int>[array.Length % 100 == 0 ? array.Length / 100 : array.Length / 100 + 1];
                        for (var index1 = 0; index1 < array.Length; ++index1)
                        {
                            var index2 = index1 / 100;
                            if (intListArray[index2] == null)
                                intListArray[index2] = new List<int>();
                            intListArray[index2].Add(array[index1]);
                        }

                        ClientLocalDb.Current.DeleteTopicsByCategoryId(category.CategoryID);
                        foreach (var intList in intListArray)
                        {
                            ClientLocalDb.Current.SaveTopicInfo(RuTrackerOrg.Current.GetTopicsInfo(intList.ToArray()),
                                true);
                            num1 += new decimal(1000, 0, 0, false, 1) / (categoryList.Count * intListArray.Length);
                            backgroundWorker.ReportProgress((int) num1);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Ошибка при обновлении топиков: " + ex.Message);
                        logger.Debug(ex);
                    }

                    ClientLocalDb.Current.SaveUsers(RuTrackerOrg.Current.GetUsers(ClientLocalDb.Current.GetNoUsers()));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Debug(ex);
                var num2 = (int) MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand,
                    MessageBoxDefaultButton.Button1);
            }

            logger.Info("Завершена задача по обновлению топиков.");
        }

        public static void bwUpdateKeepersByAllCategories(object sender, DoWorkEventArgs e)
        {
            logger.Info("Запущена задача по обновлению информации о хранителях...");
            var backgroundWorker = sender as BackgroundWorker;
            var num1 = new decimal(0, 0, 0, false, 1);
            backgroundWorker.ReportProgress((int) num1);
            try
            {
                ClientLocalDb.Current.ClearKeepers();
                var categories = ClientLocalDb.Current.GetCategoriesEnable().Select(x => x.CategoryID).OrderBy(x => x)
                    .ToArray();
                var array = ClientLocalDb.Current.GetReports(new int?()).Where(x =>
                {
                    if (x.Key.Item2 == 0 && x.Key.Item1 != 0 && !string.IsNullOrWhiteSpace(x.Value.Item1))
                        return categories.Any(z => z == x.Key.Item1);
                    return false;
                }).Select(x =>
                {
                    var strArray = x.Value.Item1.Split('=');
                    if (strArray.Length == 3)
                        return new
                        {
                            TopicID = int.Parse(strArray[2]),
                            CategoryID = x.Key.Item1
                        };
                    if (strArray.Length == 2)
                        return new
                        {
                            TopicID = int.Parse(strArray[1]),
                            CategoryID = x.Key.Item1
                        };
                    return new
                    {
                        TopicID = 0,
                        CategoryID = x.Key.Item1
                    };
                }).Where(x => (uint) x.TopicID > 0U).OrderBy(x => x.CategoryID).ToArray();
                var ruTrackerOrg = new RuTrackerOrg(Settings.Current.KeeperName, Settings.Current.KeeperPass);
                foreach (var data in array)
                {
                    logger.Trace("\t" + data.CategoryID);
                    ClientLocalDb.Current.SaveKeepOtherKeepers(ruTrackerOrg.GetKeeps(data.TopicID, data.CategoryID));
                    num1 += new decimal(1000, 0, 0, false, 1) / array.Count();
                    backgroundWorker.ReportProgress((int) num1);
                }

                Reports.CreateReportByRootCategories();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Debug(ex);
                var num2 = (int) MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand,
                    MessageBoxDefaultButton.Button1);
            }

            logger.Info("Завершена задача по обновлению информации о хранителях.");
        }

        public static void bwRuningAndStopingDistributions(object sender, DoWorkEventArgs e)
        {
            logger.Info("Запущена задача по запуску/остановке раздач в торрент-клиентах...");
            var backgroundWorker = sender as BackgroundWorker;
            var obj = e.Argument;
            var num = new decimal(0, 0, 0, false, 1);
            backgroundWorker.ReportProgress((int) num);
            var countSeedersBycategories = new Dictionary<int, int>();
            try
            {
                var inner = ClientLocalDb.Current.GetTopicsByCategory(-1).Where(x => !x.IsBlackList);
                foreach (var category in ClientLocalDb.Current.GetCategoriesEnable())
                    if (!countSeedersBycategories.ContainsKey(category.CategoryID))
                        countSeedersBycategories.Add(category.CategoryID, category.CountSeeders);

                ClientLocalDb.Current.ResetFlagsTopicDownloads();
                var torrentClients = ClientLocalDb.Current.GetTorrentClients();
                foreach (var torrentClientInfo in torrentClients)
                {
                    try
                    {
                        var torrentClient = torrentClientInfo.Create();
                        if (torrentClient != null)
                        {
                            var allTorrentHash = torrentClient.GetAllTorrentHash();
                            logger.Info("\t Кол-во раздач в торрент-клиенте \"" + torrentClientInfo.Name + "\": " +
                                        allTorrentHash.Count);
                            ClientLocalDb.Current.SetTorrentClientHash(allTorrentHash);
                            var list = allTorrentHash.Join(inner, c => c.Hash, a => a.Hash, (c, a) => new
                            {
                                c, a
                            }).Where(_param1 => _param1.c.IsRun.HasValue).Select(_param1 => new
                            {
                                _param1.a.Hash,
                                IsRun = _param1.c.IsRun.Value,
                                _param1.c.IsPause,
                                _param1.a.Seeders,
                                MaxSeeders = countSeedersBycategories.ContainsKey(_param1.a.CategoryID)
                                    ? countSeedersBycategories[_param1.a.CategoryID]
                                    : new int?()
                            }).ToList();
                            var array1 = list.Where(x =>
                            {
                                if (x.IsRun && x.MaxSeeders.HasValue)
                                    return x.Seeders > x.MaxSeeders.Value + 1;
                                return false;
                            }).Select(x => x.Hash).ToArray();
                            logger.Info("\t Кол-во раздач в торрент-клиенте \"" + torrentClientInfo.Name +
                                        "\" которые требуется остановить: " + array1.Length + ". Останавливаем...");
                            var stringListArray1 =
                                new List<string>[array1.Length / 50 + (array1.Length % 50 != 0 ? 1 : 0)];
                            for (var index1 = 0; index1 < array1.Length; ++index1)
                            {
                                var index2 = index1 / 50;
                                if (stringListArray1[index2] == null)
                                    stringListArray1[index2] = new List<string>();
                                stringListArray1[index2].Add(array1[index1]);
                            }

                            if (stringListArray1.Length == 0)
                            {
                                num += new decimal(1000, 0, 0, false, 1) / (2 * torrentClients.Count);
                                backgroundWorker.ReportProgress((int) num);
                            }

                            foreach (var stringList in stringListArray1)
                            {
                                if (stringList != null)
                                    torrentClient.DistributionStop(stringList);
                                num += new decimal(1000, 0, 0, false, 1) /
                                       (2 * torrentClients.Count * stringListArray1.Length);
                                backgroundWorker.ReportProgress((int) num);
                            }

                            var array2 = list.Where(x =>
                            {
                                if ((!x.IsRun || x.IsPause) && x.MaxSeeders.HasValue)
                                    return x.Seeders <= x.MaxSeeders.Value;
                                return false;
                            }).Select(x => x.Hash).ToArray();
                            var stringListArray2 =
                                new List<string>[array2.Length / 50 + (array2.Length % 50 != 0 ? 1 : 0)];
                            logger.Info("\t Кол-во раздач в торрент-клиенте \"" + torrentClientInfo.Name +
                                        "\" которые требуется запустить: " + array2.Length + ". Запускаем...");
                            for (var index1 = 0; index1 < array2.Length; ++index1)
                            {
                                var index2 = index1 / 50;
                                if (stringListArray2[index2] == null)
                                    stringListArray2[index2] = new List<string>();
                                stringListArray2[index2].Add(array2[index1]);
                            }

                            if (stringListArray2.Length == 0)
                            {
                                num += new decimal(1000, 0, 0, false, 1) / (2 * torrentClients.Count);
                                backgroundWorker.ReportProgress((int) num);
                            }

                            foreach (var stringList in stringListArray2)
                            {
                                if (stringList != null)
                                    torrentClient.DistributionStart(stringList);
                                num += new decimal(1000, 0, 0, false, 1) /
                                       (2 * torrentClients.Count * stringListArray2.Length);
                                backgroundWorker.ReportProgress((int) num);
                            }
                        }
                        else
                        {
                            num += new decimal(1000, 0, 0, false, 1) / torrentClients.Count;
                        }

                        backgroundWorker.ReportProgress((int) num);
                    }
                    catch (Exception ex)
                    {
                        logger.Warn("Не удалось запустить/остановить раздачи на клиенте \"" + torrentClientInfo.Name +
                                    "\": " + ex.Message);
                        logger.Debug(ex);
                        num += new decimal(1000, 0, 0, false, 1) / torrentClients.Count;
                    }

                    backgroundWorker.ReportProgress((int) num);
                }

                logger.Info("Строим отчеты о хранимом...");
                Reports.CreateReports();
                logger.Info("Отчеты о хранимом построены.");
            }
            catch (Exception ex)
            {
                logger.Warn("Произошла критическая ошибка при запуске/остановке раздач");
                logger.Debug(ex);
            }

            logger.Info("Завершена задача по запуску/остановке раздач в торрент-клиентах.");
            logger.Debug(string.Format("Размер ОЗУ 1: {0}", GC.GetTotalMemory(false)));
            GC.Collect(2);
            logger.Debug(string.Format("Размер ОЗУ 2: {0}", GC.GetTotalMemory(false)));
        }

        public static void bwCreateReportsTorrentClients(object sender, DoWorkEventArgs e)
        {
            var torrentClients = ClientLocalDb.Current.GetTorrentClients();
            var inner = ClientLocalDb.Current.GetTopicsByCategory(-1).Where(x => !x.IsBlackList);
            logger.Info("Строим отчет о статистике в торрент-клиенте...");
            var stringBuilder = new StringBuilder();
            var dictionary =
                ClientLocalDb.Current.GetCategories().ToDictionary(x => x.CategoryID, x => x);
            var num1 = Math.Max(dictionary.Count == 0 ? 20 : dictionary.Values.Max(x => x.FullName.Length),
                torrentClients.Count == 0 ? 20 : torrentClients.Max(x => x.Name.Length));
            var empty = string.Empty;
            for (var index = 0; index < num1; ++index)
                empty += "*";
            var backgroundWorker = sender as BackgroundWorker;
            var num2 = new decimal(0, 0, 0, false, 1);
            backgroundWorker.ReportProgress((int) num2);
            foreach (var torrentClientInfo in torrentClients)
            {
                logger.Debug("\t" + torrentClientInfo.Name + "...");
                try
                {
                    stringBuilder.AppendLine(empty);
                    stringBuilder.AppendFormat("*\t{0}\r\n", torrentClientInfo.Name);
                    stringBuilder.AppendLine(empty);
                    var torrentClient = torrentClientInfo.Create();
                    if (torrentClient != null)
                    {
                        var array1 = torrentClient.GetAllTorrentHash().GroupJoin(inner, t => t.Hash, b => b.Hash,
                            (t, bt) => new
                            {
                                t, bt
                            }).SelectMany(_param1 => _param1.bt.DefaultIfEmpty(), (_param1, b) =>
                        {
                            var num3 = b != null ? b.CategoryID : -1;
                            var size = _param1.t.Size;
                            var isRun = _param1.t.IsRun;
                            int num4;
                            if (!isRun.HasValue)
                            {
                                num4 = -1;
                            }
                            else
                            {
                                isRun = _param1.t.IsRun;
                                num4 = isRun.Value ? 1 : 0;
                            }

                            var num5 = _param1.t.IsPause ? 1 : 0;
                            var num6 = b == null ? -1 : b.Seeders;
                            return new
                            {
                                CategoryID = num3,
                                Size = size,
                                IsRun = num4,
                                IsPause = num5 != 0,
                                Seeders = num6
                            };
                        }).GroupBy(x => new
                        {
                            x.CategoryID,
                            x.IsRun,
                            x.IsPause,
                            x.Seeders
                        }).Select(x => new
                        {
                            x.Key.CategoryID,
                            x.Key.IsRun,
                            x.Key.IsPause,
                            Size = x.Sum(y => y.Size),
                            Count = x.Count(),
                            x.Key.Seeders
                        }).ToArray();
                        stringBuilder.AppendFormat("\tВсего:\t\t{0,6} шт. ({1})\r\n", array1.Sum(x => x.Count),
                            TopicInfo.sizeToString(array1.Sum(x => x.Size)));
                        stringBuilder.AppendFormat("\tРаздаются:\t{0,6} шт. ({1})\r\n",
                            array1.Where(x => x.IsRun == 1).Sum(x => x.Count),
                            TopicInfo.sizeToString(array1.Where(x => x.IsRun == 1).Sum(x => x.Size)));
                        stringBuilder.AppendFormat("\tОстановлены:\t{0,6} шт. ({1})\r\n",
                            array1.Where(x => x.IsRun == 0).Sum(x => x.Count),
                            TopicInfo.sizeToString(array1.Where(x => x.IsRun == 0).Sum(x => x.Size)));
                        stringBuilder.AppendFormat("\tПрочие:\t\t{0,6} шт. ({1})\r\n",
                            array1.Where(x => x.IsRun == -1).Sum(x => x.Count),
                            TopicInfo.sizeToString(array1.Where(x => x.IsRun == -1).Sum(x => x.Size)));
                        stringBuilder.AppendFormat("\tНеизвестные:\t{0,6} шт. ({1})\r\n",
                            array1.Where(x => x.CategoryID == -1).Sum(x => x.Count),
                            TopicInfo.sizeToString(array1.Where(x => x.CategoryID == -1).Sum(x => x.Size)));
                        stringBuilder.AppendLine();
                        stringBuilder.AppendFormat("\tПо кол-ву сидов:\r\n");
                        foreach (var data in array1.GroupBy(x => x.Seeders).Select(x => new
                        {
                            Seeders = x.Key,
                            Count = x.Sum(z => z.Count),
                            Size = x.Sum(z => z.Size)
                        }).OrderBy(x => x.Seeders))
                            stringBuilder.AppendFormat("\t{2}:\t\t{0,5} шт. ({1})\r\n", data.Count,
                                TopicInfo.sizeToString(data.Size), data.Seeders);
                        stringBuilder.AppendLine();
                        foreach (var num3 in array1.Select(x => x.CategoryID).Distinct().OrderBy(x => x).ToArray())
                        {
                            var c = num3;
                            var array2 = array1.Where(x => x.CategoryID == c).ToArray();
                            var str = "Неизвестные";
                            if (dictionary.ContainsKey(c))
                                str = dictionary[c].FullName;
                            stringBuilder.AppendFormat("{0}:\r\n", str);
                            stringBuilder.AppendFormat("\tВсего:\t\t{0,5} шт. ({1})\r\n", array2.Sum(x => x.Count),
                                TopicInfo.sizeToString(array2.Sum(x => x.Size)));
                            stringBuilder.AppendFormat("\tРаздаются:\t{0,5} шт. ({1})\r\n",
                                array2.Where(x => x.IsRun == 1).Sum(x => x.Count),
                                TopicInfo.sizeToString(array2.Where(x => x.IsRun == 1).Sum(x => x.Size)));
                            stringBuilder.AppendFormat("\tОстановлены:\t{0,5} шт. ({1})\r\n",
                                array2.Where(x => x.IsRun == 0).Sum(x => x.Count),
                                TopicInfo.sizeToString(array2.Where(x => x.IsRun == 0).Sum(x => x.Size)));
                            stringBuilder.AppendFormat("\tПрочие:\t\t{0,5} шт. ({1})\r\n",
                                array2.Where(x => x.IsRun == -1).Sum(x => x.Count),
                                TopicInfo.sizeToString(array2.Where(x => x.IsRun == -1).Sum(x => x.Size)));
                        }

                        stringBuilder.AppendLine();
                    }
                }
                catch (Exception ex)
                {
                    stringBuilder.AppendFormat("Ошибка: {0}\r\n\r\n\r\n", ex.Message);
                }

                stringBuilder.AppendLine();
                num2 += new decimal(1000, 0, 0, false, 1) / torrentClients.Count();
                if (num2 <= new decimal(100))
                    backgroundWorker.ReportProgress((int) num2);
            }

            var reports = new Dictionary<int, Dictionary<int, string>>();
            reports.Add(0, new Dictionary<int, string>());
            reports[0].Add(1, stringBuilder.ToString());
            try
            {
                ClientLocalDb.Current.SaveReports(reports);
                logger.Info("Отчет о статистике в торрент-клиенте построен.");
            }
            catch (Exception ex)
            {
                logger.Error("Произошла ошибка при сохранении отчета в базу данных: " + ex.Message);
                logger.Trace(ex.StackTrace);
            }
        }

        public static void bwCreateUnknownTorrentsReport(object sender, DoWorkEventArgs e)
        {
            var torrentClients = ClientLocalDb.Current.GetTorrentClients();
            var inner = ClientLocalDb.Current.GetTopicsByCategory(-1).Where(x => !x.IsBlackList);
            logger.Info("Строим отчет о статистике в торрент-клиенте...");
            var stringBuilder = new StringBuilder();
            var dictionary =
                ClientLocalDb.Current.GetCategories().ToDictionary(x => x.CategoryID, x => x);
            var num1 = Math.Max(dictionary.Count == 0 ? 20 : dictionary.Values.Max(x => x.FullName.Length),
                torrentClients.Count == 0 ? 20 : torrentClients.Max(x => x.Name.Length));
            var empty = string.Empty;
            for (var index = 0; index < num1; ++index)
                empty += "*";
            var backgroundWorker = sender as BackgroundWorker;
            var num2 = new decimal(0, 0, 0, false, 1);
            backgroundWorker.ReportProgress((int) num2);
            var listUnknown = new StringBuilder();
            listUnknown.AppendLine("Клиент;Метка;Торрент;Размер");
            foreach (var torrentClientInfo in torrentClients)
            {
                logger.Debug("\t" + torrentClientInfo.Name + "...");
                try
                {
                    var torrentClient = torrentClientInfo.Create();
                    if (torrentClient != null)
                    {
                        var array1 = torrentClient.GetAllTorrentHash().GroupJoin(inner, t => t.Hash, b => b.Hash,
                            (t, bt) => new
                            {
                                t, bt
                            }).SelectMany(_param1 => _param1.bt.DefaultIfEmpty(), (_param1, b) =>
                        {
                            var num3 = b != null ? b.CategoryID : -1;
                            var size = _param1.t.Size;
                            var isRun = _param1.t.IsRun;
                            int num4;
                            if (!isRun.HasValue)
                            {
                                num4 = -1;
                            }
                            else
                            {
                                isRun = _param1.t.IsRun;
                                num4 = isRun.Value ? 1 : 0;
                            }

                            var num5 = _param1.t.IsPause ? 1 : 0;
                            var num6 = b == null ? -1 : b.Seeders;
                            return new
                            {
                                CategoryID = num3,
                                Name = _param1.t.TorrentName,
                                Size = size,
                                IsRun = num4,
                                IsPause = num5 != 0,
                                Seeders = num6,
                                _param1.t.Label
                            };
                        }).GroupBy(x => new
                        {
                            x.CategoryID,
                            x.Name,
                            x.IsRun,
                            x.IsPause,
                            x.Seeders,
                            x.Label
                        }).Select(x => new
                        {
                            x.Key.CategoryID,
                            x.Key.Name,
                            x.Key.IsRun,
                            x.Key.IsPause,
                            Size = x.Sum(y => y.Size),
                            Count = x.Count(),
                            x.Key.Seeders,
                            x.Key.Label
                        }).ToArray();
                        var countUnknown = array1.Where(x => x.CategoryID == -1).Sum(x => x.Count);
                        foreach (var info in array1.Where(x => x.CategoryID == -1).ToList())
                            listUnknown.AppendLine(string.Join(";", torrentClientInfo.Name, info.Label, info.Name,
                                TopicInfo.sizeToString(info.Size)));
                    }
                }
                catch (Exception ex)
                {
                    listUnknown.AppendFormat("Ошибка: {0}\r\n\r\n\r\n", ex.Message);
                }

                num2 += new decimal(1000, 0, 0, false, 1) / torrentClients.Count();
                if (num2 <= new decimal(100))
                    backgroundWorker.ReportProgress((int) num2);
            }

            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.DefaultExt = "csv";
            saveFileDialog.Filter = @".csv|CSV файл|.txt|Текстовый документ";
            saveFileDialog.OverwritePrompt = true;
            var form = (MainForm) e.Argument;
            form.Invoke((MethodInvoker) delegate
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var file = saveFileDialog.OpenFile();
                    var writer = new StreamWriter(file, Encoding.UTF8);
                    writer.Write(listUnknown.ToString());
                    writer.Flush();
                    file.Close();
                }
            });
        }

        public static void bwSendReports(object sender, DoWorkEventArgs e)
        {
            logger.Info("Запущена задача на отправку отчетов на форум....");
            var num1 = new decimal(0, 0, 0, false, 1);
            var backgroundWorker = sender as BackgroundWorker;
            var array = ClientLocalDb.Current.GetReports(new int?())
                .Where(x => !string.IsNullOrWhiteSpace(x.Value.Item1)).OrderBy(x => x.Key.Item1).Select(x => x.Value)
                .Where(x => x.Item1.Split('=').Length == 3).ToArray();
            if (array.Where(x => !string.IsNullOrWhiteSpace(x.Item1)).Count() == 0)
            {
                var num2 = (int) MessageBox.Show("Нет ни одного отчета c указанным URL для отправки на форум");
            }
            else
            {
                foreach (var tuple in array.Where(x => !string.IsNullOrWhiteSpace(x.Item1)))
                {
                    logger.Info(tuple.Item1);
                    try
                    {
                        RuTrackerOrg.Current.SendReport(tuple.Item1, tuple.Item2);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                        logger.Debug(ex);
                        var num3 = (int) MessageBox.Show("Произошла ошибка при отправке отчетов:\r\n" + ex.Message,
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                    }

                    num1 += new decimal(1000, 0, 0, false, 1) / array.Length;
                    backgroundWorker.ReportProgress((int) num1);
                }

                logger.Info("Завершена задача на отправку отчетов на форум.");
            }
        }
    }
}