using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using TLO.Info;

namespace TLO.Clients
{
    internal class RuTrackerOrg
    {
        private static RuTrackerOrg? _current;
        private readonly Logger _logger;
        private readonly string _userName;
        private readonly string _userPass;
        private string _apiId;
        private JsonSerializer _jSerializer;
        private int _keeperId;
        private TloWebClient _webClient;

        public RuTrackerOrg(string userName, string password)
        {
            _jSerializer = new JsonSerializer();
            _userName = userName;
            _userPass = password;
            if (_logger == null) _logger = LogManager.GetLogger("RuTrackerOrg");
            if (string.IsNullOrWhiteSpace(_userName) || string.IsNullOrWhiteSpace(_userPass)) return;

            ReadKeeperInfo();
        }

        public static RuTrackerOrg Current =>
            _current ??= new RuTrackerOrg(Settings.Current.KeeperName, Settings.Current.KeeperPass);

        public IEnumerable<Category> GetCategories()
        {
            var source = new List<Category>();
            var downloadArchivePage =
                DownloadArchivePage($"https://{Settings.Current.ApiHost}/v1/static/cat_forum_tree");
            var jObject1 = (JsonConvert.DeserializeObject(downloadArchivePage) as JObject)["result"]
                .ToObject<JObject>();
            jObject1["c"].ToObject<JObject>();
            source.AddRange(jObject1["c"].ToObject<Dictionary<string, object>>().Select(x => new Category
            {
                CategoryID = 1000000 + int.Parse(x.Key),
                Name = x.Value as string
            }).ToArray());
            source.AddRange(jObject1["f"].ToObject<Dictionary<string, object>>().Select(x => new Category
            {
                CategoryID = int.Parse(x.Key),
                Name = x.Value as string
            }));
            var dictionary = source.ToDictionary(x => x.CategoryID, x => x);
            var jObject2 = jObject1["tree"].ToObject<JObject>();
            var num = 0;
            foreach (var keyValuePair1 in jObject2)
            {
                var key1 = int.Parse(keyValuePair1.Key) + 1000000;
                ++num;
                dictionary[key1].OrderID = num;
                dictionary[key1].FullName = dictionary[key1].Name;
                if (!(keyValuePair1.Value is JObject) || !keyValuePair1.Value.Any()) continue;

                foreach (var keyValuePair2 in keyValuePair1.Value.ToObject<JObject>())
                {
                    var key2 = int.Parse(keyValuePair2.Key);
                    ++num;
                    if (dictionary.ContainsKey(key2))
                    {
                        var category = dictionary[key2];
                        category.ParentID = key1;
                        category.OrderID = num;
                        category.FullName =
                            $"{(dictionary.ContainsKey(key1) ? dictionary[key1].Name : "")} » {category.Name}";
                    }

                    foreach (var jToken in keyValuePair2.Value.ToObject<JArray>())
                    {
                        var key3 = (int) jToken;
                        ++num;
                        if (dictionary.ContainsKey(key3))
                        {
                            var category = dictionary[key3];
                            category.ParentID = key2;
                            category.OrderID = num;
                            category.FullName =
                                $"{(dictionary.ContainsKey(key2) ? dictionary[key2].FullName : "")} » {category.Name}";
                        }
                    }
                }
            }

            return source;
        }

        public IEnumerable<Tuple<int, string>> GetCategoriesFromPost(string postUrl)
        {
            var tupleList = new List<Tuple<int, string>>();
            var array = DownloadWebPage(postUrl).Split(new char[2]
            {
                '\r',
                '\n'
            }, StringSplitOptions.RemoveEmptyEntries).Where(x =>
            {
                if (x.Contains($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewforum.php?f="))
                    return x.Contains("class=\"postLink\"");
                return false;
            }).ToArray();
            var nullable1 = new int?();
            string str1 = null;
            foreach (var str2 in array)
            {
                var separator = new[] {'"', '<', '>', ' '};
                var num1 = 1;
                foreach (var postUrl1 in str2.Split(separator, (StringSplitOptions) num1).Where(x =>
                {
                    if (!x.Contains($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewforum.php?f=") &&
                        !x.Contains($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewtopic.php?t="))
                        return x.Contains($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewtopic.php?p=");
                    return true;
                }).ToArray())
                {
                    if (postUrl1.Contains($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewforum.php?f="))
                        nullable1 = int.Parse(
                            postUrl1.Replace($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewforum.php?f=",
                                ""));
                    var nullable2 = nullable1;
                    var num2 = 2020;
                    if ((nullable2.GetValueOrDefault() == num2 ? nullable2.HasValue ? 1 : 0 : 0) != 0)
                        Console.Write("");
                    if (postUrl1.Contains($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewtopic.php?t=") &&
                        postUrl1 != $"https://{Settings.Current.HostRuTrackerOrg}/forum/viewtopic.php?t=")
                        str1 = postUrl1;
                    if (postUrl1.Contains($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewtopic.php?p="))
                        str1 = GetTopicUrlByPostUrl(postUrl1);
                }

                if (nullable1.HasValue && !string.IsNullOrWhiteSpace(str1))
                    tupleList.Add(new Tuple<int, string>(nullable1.Value, str1));
                nullable1 = new int?();
                str1 = null;
            }

            return tupleList;
        }

        public string GetTopicUrlByPostUrl(string postUrl)
        {
            var str = DownloadWebPage(postUrl);
            if (str.Contains("<div class=\"mrg_16\">Тема не найдена</div>"))
                return null;
            return str.Split(new char[1]
                {
                    '"'
                }, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => x.Contains($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewtopic.php?t="))
                .Select(x => x).FirstOrDefault();
        }

        public int[][] GetTopicsStatus(int forumId)
        {
            var dictionary =
                JsonConvert.DeserializeObject<JObject>(DownloadArchivePage(
                        string.Format("https://{1}/v1/static/pvc/f/{0}", forumId, Settings.Current.ApiHost)))["result"]
                    .ToObject<Dictionary<int, long[]>>();
            var numArray1 = new int[dictionary.Count][];
            var index = 0;
            foreach (var keyValuePair in dictionary)
            {
                var numArray2 = keyValuePair.Value;
                numArray1[index] = new int[2]
                {
                    keyValuePair.Key,
                    numArray2.Length > 1 ? (int) numArray2[1] : -1
                };
                ++index;
            }

            return numArray1;
        }

        public List<TopicInfo> GetTopicsInfo(int[] topics)
        {
            if (topics == null || topics.Length == 0 || topics.Length > 100)
                return null;
            var topicInfoList = new List<TopicInfo>();
            foreach (var keyValuePair in
                JsonConvert.DeserializeObject<JObject>(DownloadArchivePage(string.Format(
                        "https://{0}/v1/get_tor_topic_data?by=topic_id&val={1}", Settings.Current.ApiHost,
                        HttpUtility.UrlEncode(string.Join(",", topics)))))["result"]
                    .ToObject<Dictionary<int, Dictionary<string, object>>>())
            {
                var topicInfo = new TopicInfo();
                topicInfo.TopicID = keyValuePair.Key;
                var dictionary = keyValuePair.Value;
                if (dictionary != null)
                {
                    topicInfo.Hash = dictionary["info_hash"] as string;
                    topicInfo.CategoryID = int.Parse(dictionary["forum_id"].ToString());
                    topicInfo.RegTime =
                        new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(int.Parse(dictionary["reg_time"].ToString()));
                    topicInfo.Size = long.Parse(dictionary["size"].ToString());
                    topicInfo.Status = int.Parse(dictionary["tor_status"].ToString());
                    topicInfo.Seeders = int.Parse(dictionary["seeders"].ToString());
                    topicInfo.Name2 = dictionary["topic_title"] as string;
                    topicInfo.PosterID = int.Parse(dictionary["poster_id"].ToString());
                }

                topicInfoList.Add(topicInfo);
            }

            Thread.Sleep(500);
            return topicInfoList;
        }

        public IEnumerable<UserInfo> GetUsers(int[] id)
        {
            if (id == null || !id.Any())
                return null;
            var userInfoList = new List<UserInfo>();
            var intListArray = new List<int>[id.Count() % 100 == 0 ? id.Count() / 100 : id.Count() / 100 + 1];
            for (var index1 = 0; index1 < id.Count(); ++index1)
            {
                var index2 = index1 / 100;
                if (intListArray[index2] == null)
                    intListArray[index2] = new List<int>();
                intListArray[index2].Add(id[index1]);
            }

            foreach (IEnumerable<int> values in intListArray)
            {
                var url = string.Format("https://{0}/v1/get_user_name?by=user_id&val={1}", Settings.Current.ApiHost,
                    HttpUtility.UrlEncode(string.Join(",", values)));
                var getUserNameResult = DownloadArchivePage(url);
                foreach (var keyValuePair in
                    JsonConvert.DeserializeObject<JObject>(getUserNameResult)["result"]
                        .ToObject<Dictionary<int, string>>())
                    userInfoList.Add(new UserInfo
                    {
                        UserID = keyValuePair.Key,
                        Name = keyValuePair.Value
                    });
                Thread.Sleep(500);
            }

            return userInfoList;
        }

        private List<int> GetPostsFromTopicId(int topicId)
        {
            var num1 = 0;
            var intList = new List<int>();
            string str1;
            do
            {
                str1 = DownloadWebPage(string.Format("https://{2}/forum/viewtopic.php?t={0}{1}", topicId,
                    num1 == 0 ? "" : "&start=" + num1, Settings.Current.HostRuTrackerOrg));
                if (str1.Contains("<div class=\"mrg_16\">Тема не найдена</div>"))
                {
                    Thread.Sleep(500);
                    str1 = DownloadWebPage(string.Format("https://{0}/forum/viewtopic.php?p={1}", topicId,
                        Settings.Current.HostRuTrackerOrg));
                    if (str1.Contains("<div class=\"mrg_16\">Тема не найдена</div>"))
                        return new List<int>();
                    var s = str1.Split(new char[1]
                        {
                            '"'
                        }, StringSplitOptions.RemoveEmptyEntries)
                        .Where(x => x.Contains($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewtopic.php?t="))
                        .Select(x => x.Replace("https://rutracker.org/forum/viewtopic.php?t=", "")).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(s))
                    {
                        topicId = int.Parse(s);
                        goto label_13;
                    }
                }

                var str2 = str1;
                var separator1 = new char[2] {'\r', '\n'};
                var num2 = 1;
                foreach (var str3 in str2.Split(separator1, (StringSplitOptions) num2)
                    .Where(x => x.Contains("\">[Цитировать]</a>")).ToArray())
                {
                    var separator2 = new char[1] {'"'};
                    var num3 = 1;
                    var str4 = str3.Split(separator2, (StringSplitOptions) num3)
                        .FirstOrDefault(x => x.Contains("https://"));
                    if (!string.IsNullOrWhiteSpace(str4))
                    {
                        var strArray = str4.Split('=');
                        if (strArray.Length >= 3)
                            intList.Add(int.Parse(strArray[2]));
                    }
                }

                num1 += 30;
                label_13: ;
            } while (str1.Contains("\">След.</a></b></p>") || num1 == 0);

            return intList;
        }

        private Tuple<string, int, List<int>> GetTopicsFromReport(int postId, int categoryId)
        {
            Tuple<string, int, List<int>> tuple = null;
            var strArray1 = DownloadWebPage(string.Format("https://post.{1}/forum/posting.php?mode=quote&p={0}",
                postId, Settings.Current.HostRuTrackerOrg)).Split(new string[2]
            {
                "<textarea",
                "</textarea>"
            }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray1.Length < 2)
                return tuple;
            var str1 = strArray1[1];
            var separator = new char[2] {'[', ']'};
            var num = 1;
            foreach (var str2 in str1
                .Split(separator, (StringSplitOptions) num)
                .Where(x => x.Contains($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewtopic.php?t=") ||
                            x.Contains("quote=")).ToArray())
                try
                {
                    if (str2.Contains("quote="))
                    {
                        tuple = new Tuple<string, int, List<int>>(str2.Replace("quote=", "").Replace("\"", ""),
                            categoryId, new List<int>());
                    }
                    else if (tuple != null)
                    {
                        var strArray2 = str2.Split('=');
                        if (strArray2.Length >= 3)
                            tuple.Item3.Add(int.Parse(strArray2[2]));
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error("Ошибка получения информации о раздаче по адресу \"" + str2 + "\": " + ex.Message);
                    _logger.Warn(ex.StackTrace);
                    _logger.Debug(ex);
                }

            return tuple;
        }

        public Dictionary<string, Tuple<int, List<int>>> GetKeeps(int topicid, int categoryId)
        {
            Dictionary<string, Tuple<int, List<int>>> dictionary;
            dictionary = new Dictionary<string, Tuple<int, List<int>>>();
            var empty = string.Empty;
            var num = 0;
            string str1;
            do
            {
                empty = string.Empty;
                str1 = DownloadWebPage(string.Format("https://{2}/forum/viewtopic.php?t={0}{1}",
                    topicid, num == 0 ? "" : "&start=" + num, Settings.Current.HostRuTrackerOrg));
                if (str1.Contains("<div class=\"mrg_16\">Тема не найдена</div>"))
                {
                    Thread.Sleep(500);
                    str1 = DownloadWebPage(
                        $"https://{Settings.Current.HostRuTrackerOrg}/forum/viewtopic.php?p={(object) topicid}");
                    if (str1.Contains("<div class=\"mrg_16\">Тема не найдена</div>"))
                        return dictionary;
                    var s = string.Join("\r\n", str1.Split('\r', '\n').Where(x => x.Contains("id=\"topic-title\"")))
                        .Split(new char[4]
                        {
                            '"',
                            '<',
                            '>',
                            ' '
                        }, StringSplitOptions.RemoveEmptyEntries)
                        .Where(x => x.Contains($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewtopic.php?t="))
                        .Select(x =>
                            x.Replace($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewtopic.php?t=", ""))
                        .FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(s))
                    {
                        topicid = int.Parse(s);
                        goto label_18;
                    }
                }

                var array = str1.Split(new char[2]
                {
                    '\r',
                    '\n'
                }, StringSplitOptions.RemoveEmptyEntries).Where(x =>
                {
                    if (x.Contains("\t\t<a href=\"#\" onclick=\"return false;\">"))
                        return true;
                    if (x.Contains("viewtopic.php?t=") && x.Contains("class=\"postLink\""))
                        return !x.Contains("<div");
                    return false;
                }).ToArray();
                var keeperName = string.Empty;
                foreach (var str2 in array)
                    if (str2.Contains("\t\t<a href=\"#\" onclick=\"return false;\">"))
                    {
                        keeperName = str2.Replace("\t\t<a href=\"#\" onclick=\"return false;\">", "")
                            .Replace("</a>", "").Replace("<wbr>", "").Trim();
                    }
                    else
                    {
                        if (!dictionary.ContainsKey(keeperName))
                            dictionary.Add(keeperName, new Tuple<int, List<int>>(categoryId, new List<int>()));
                        var str3 = str2.Split(new char[6]
                            {
                                '"',
                                '<',
                                '>',
                                ' ',
                                '#',
                                '&'
                            }, StringSplitOptions.RemoveEmptyEntries)
                            .Where(x => x.Contains("viewtopic.php?t=")).FirstOrDefault();
                        if (!string.IsNullOrWhiteSpace(str3))
                        {
                            var strArray = str3.Split('=');
                            if (strArray.Length >= 2)
                                try
                                {
                                    dictionary[keeperName].Item2.Add(int.Parse(strArray[1]));
                                }
                                catch (Exception ex)
                                {
                                    _logger.Warn(topicid + "\t" + strArray[1] + "\t" + ex.Message);
                                }
                        }
                    }

                num += 30;
                label_18: ;
            } while (str1.Contains("\">След.</a></b></p>") || num == 0);

            return dictionary;
        }

        public Dictionary<string, Tuple<int, List<int>>> GetKeeps2(int topicid, int categoryId)
        {
            var dictionary = new Dictionary<string, Tuple<int, List<int>>>();
            foreach (var postId in GetPostsFromTopicId(topicid))
            {
                var topicsFromReport = GetTopicsFromReport(postId, categoryId);
                if (topicsFromReport != null && topicsFromReport.Item3.Count != 0)
                {
                    if (!dictionary.ContainsKey(topicsFromReport.Item1))
                        dictionary.Add(topicsFromReport.Item1,
                            new Tuple<int, List<int>>(topicsFromReport.Item2, new List<int>()));
                    dictionary[topicsFromReport.Item1].Item2.AddRange(topicsFromReport.Item3);
                }
            }

            return dictionary;
        }

        private string DownloadArchivePage(string page)
        {
            var tloWebClient = new TloWebClient(enableProxy: true);
            return tloWebClient.DownloadString(page);
        }

        public string DownloadWebPage(string page, params object[] param)
        {
            return Encoding.GetEncoding("windows-1251").GetString(DownloadWebPages(string.Format(page, param)));
        }

        public byte[] DownloadTorrentFile(int id)
        {
            for (var index = 0; index < 100; ++index)
            {
                var numArray1 = new byte[0];
                var empty = string.Empty;
                TloWebClient tloWebClient = null;
                try
                {
                    if (_webClient == null)
                    {
                        tloWebClient = new TloWebClient();
                        var s = string.Format("login_username={0}&login_password={1}&login={2}",
                            HttpUtility.UrlEncode(_userName, Encoding.GetEncoding(1251)),
                            HttpUtility.UrlEncode(_userPass, Encoding.GetEncoding(1251)), "Вход");
                        empty = Encoding.GetEncoding("windows-1251").GetString(
                            tloWebClient.UploadData("https://" + Settings.Current.HostRuTrackerOrg + "/forum/login.php",
                                "POST", Encoding.GetEncoding(1251).GetBytes(s)));
                        Thread.Sleep(500);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Warn(ex.Message);
                    _logger.Warn(ex);
                }

                if (!string.IsNullOrWhiteSpace(empty))
                {
                    if (empty.Contains("https://static." + Settings.Current.HostRuTrackerOrg + "/captcha"))
                        throw new Exception(
                            "При авторизации требуется ввести текст с картинки. Авторизуйтесь на WEB-сайте, а потом повторите попытку");
                    if (empty.Contains("<a href=\"profile.php?mode=register\"><b>Регистрация</b></a>"))
                        throw new Exception("Не удалось авторизоваться, проверьте логин и пароль");
                    _webClient = tloWebClient;
                }

                byte[] numArray2;
                if (string.IsNullOrWhiteSpace(_apiId))
                {
                    var str =
                        DownloadWebPage(string.Format(
                                "https://" + Settings.Current.HostRuTrackerOrg + "/forum/viewtopic.php?t={0}", id))
                            .Split(new char[2]
                            {
                                '\r',
                                '\n'
                            }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.Contains("form_token: '"))
                            .FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(str))
                        str = str.Split(new char[1] {'\''}, StringSplitOptions.RemoveEmptyEntries)[1];
                    var s = string.Format("form_token={0}", str);
                    numArray2 = _webClient.UploadData(
                        string.Format("https://" + Settings.Current.HostRuTrackerOrg + "/forum/dl.php?t={0}",
                            id), "POST", Encoding.GetEncoding(1251).GetBytes(s));
                }
                else
                {
                    numArray2 = _webClient.UploadData(
                        "https://" + Settings.Current.HostRuTrackerOrg + "/forum/dl.php", "POST",
                        Encoding.GetEncoding(1251).GetBytes(string.Format(
                            "keeper_user_id={0}&keeper_api_key={1}&t={2}&add_retracker_url=0", _keeperId, _apiId,
                            id)));
                }

                var lower = Encoding.GetEncoding(1251).GetString(numArray2).ToLower();
                if (lower.ToLower().Contains("форум временно отключен") || lower.Contains("форум временно отключен"))
                    throw new Exception("Форум временно отключен");
                if (lower.Contains("https://static." + Settings.Current.HostRuTrackerOrg + "/captcha") ||
                    lower.Contains("<a href=\"profile.php?mode=register\"><b>регистрация</b></a>"))
                {
                    if (_webClient != null)
                        _webClient.Dispose();
                    _webClient = null;
                }
                else
                {
                    if (lower[0] == 'd')
                        return numArray2;
                    var path = Path.Combine(Settings.Folder, "error_" + id + ".html");
                    if (File.Exists(path))
                        File.Delete(path);
                    using (var fileStream = File.Create(path))
                    {
                        fileStream.Write(numArray2, 0, numArray2.Length);
                    }

                    return null;
                }
            }

            return null;
        }

        public byte[] DownloadWebPages(string page)
        {
            for (var index = 0; index < 1; ++index)
            {
                var empty = string.Empty;
                TloWebClient tloWebClient = null;
                try
                {
                    if (_webClient == null)
                    {
                        tloWebClient = new TloWebClient(Encoding.GetEncoding(1251));
                        if (!string.IsNullOrWhiteSpace(_userName) && !string.IsNullOrWhiteSpace(_userPass))
                        {
                            var s = string.Format("login_username={0}&login_password={1}&login={2}",
                                HttpUtility.UrlEncode(_userName, Encoding.GetEncoding(1251)),
                                HttpUtility.UrlEncode(_userPass, Encoding.GetEncoding(1251)), "вход");
                            empty = Encoding.GetEncoding("windows-1251").GetString(
                                tloWebClient.UploadData(
                                    $"https://{Settings.Current.HostRuTrackerOrg}/forum/login.php".Replace(
                                        "rutracker.org", Settings.Current.HostRuTrackerOrg), "POST",
                                    Encoding.GetEncoding(1251).GetBytes(s)));
                        }

                        Thread.Sleep(500);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    _logger.Warn(ex.StackTrace);
                    _logger.Debug(ex);
                }

                if (!string.IsNullOrWhiteSpace(empty) && !string.IsNullOrWhiteSpace(_userName) &&
                    !string.IsNullOrWhiteSpace(_userPass))
                {
                    if (empty.Contains(
                        $"https://static.{Settings.Current.HostRuTrackerOrg}/captcha".Replace("rutracker.org",
                            Settings.Current.HostRuTrackerOrg)))
                        throw new Exception(
                            "При авторизации требуется ввести текст с картинки. Авторизуйтесь на WEB-сайте, а потом повторите попытку");
                    if (empty.Contains("<a href=\"profile.php?mode=register\"><b>Регистрация</b></a>"))
                        throw new Exception("Не удалось авторизоваться, проверьте логин и пароль");
                    _webClient = tloWebClient;
                }

                byte[] bytes;
                try
                {
                    bytes = _webClient.DownloadData(page);
                }
                catch (Exception e)
                {
                    _logger.Error(e.Message);
                    _logger.Warn(e.StackTrace);
                    _logger.Debug(e);
                    continue;
                }

                var str = Encoding.GetEncoding("windows-1251").GetString(bytes);
                if (str.ToLower().Contains("форум временно отключен") ||
                    str.ToLower().Contains("форум временно отключен"))
                    throw new Exception("Форум временно отключен");
                if (!str.Contains(
                        $"https://static.{Settings.Current.HostRuTrackerOrg}/captcha".Replace("rutracker.org",
                            Settings.Current.HostRuTrackerOrg)) &&
                    !str.Contains("<a href=\"profile.php?mode=register\"><b>Регистрация</b></a>"))
                    return bytes;
                if (_webClient != null)
                    _webClient.Dispose();
                _webClient = null;
            }

            throw new Exception("Не удалось скачать WEB-страницу за 1 попытку");
        }

        public byte[] DownloadArchiveData(string page)
        {
            for (var index = 0; index < 1; ++index)
            {
                var numArray = new byte[0];
                var empty = string.Empty;
                if (_webClient == null)
                    _webClient = new TloWebClient();
                byte[] bytes;
                try
                {
                    bytes = _webClient.DownloadData(page);
                }
                catch
                {
                    Thread.Sleep(index * 200);
                    continue;
                }

                var lower = Encoding.GetEncoding(1251).GetString(bytes).ToLower();
                if (lower.Contains("введите ваше имя и пароль"))
                    return new byte[0];
                if (lower.ToLower().Contains("форум временно отключен") || lower.Contains("введите ваше имя и пароль"))
                    throw new Exception("Форум временно отключен");
                if (lower[0] != 'd')
                    return new byte[0];
                return bytes;
            }

            throw new Exception("Не удалось скачать WEB-страницу за 1 попытку");
        }

        public void SavePage(string topicId, string folder)
        {
            var str =
                new TloWebClient().DownloadString(string.Format("https://rutracker.org/forum/viewtopic.php?t={0}",
                    topicId));
            if (str.Contains("Тема не найдена"))
                return;
            using (var fileStream = File.Create(Path.Combine(folder, string.Format("{0}.html", topicId))))
            {
                using (var streamWriter = new StreamWriter(fileStream, Encoding.GetEncoding(1251)))
                {
                    streamWriter.Write(str);
                }
            }
        }

        public void SendReport(string url, string message)
        {
            if (url.Split('#').FirstOrDefault().Split('=').Length < 3)
                throw new ArgumentException("Не корректно указан адрес отправки отчета: " + url);
            var str1 = url.Split('#').FirstOrDefault().Split('=')[2];
            var page = string.Format("https://{1}/forum/posting.php?mode=editpost&p={0}", str1,
                Settings.Current.HostRuTrackerOrg);
            var strArray = DownloadWebPage(page).Split(new char[2]
            {
                '\r',
                '\n'
            }, StringSplitOptions.RemoveEmptyEntries);
            Thread.Sleep(200);
//      string.Format("align=-1&codeColor=black&codeSize=12&codeUrl2=&decflag=2&f=1584&fontFace=-1&form_token=c2a9bace5d7f3900e2bddbf5f0f0f94a&message=&mode=editpost&p=59972538&submit_mode=submit&t=3985106");
            var str3 = strArray.Where(x => x.Contains("form_token: '")).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(str3))
                throw new ArgumentException("Параметр 'form_token' не найден на странице");
            var str4 = strArray.Where(x => x.Contains("name=\"t\" value=\"")).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(str4))
                throw new ArgumentException($"Параметр 't' не найден на странице '{page}'");
            if (str4.Split('"').Length < 6)
                throw new ArgumentException($"Массив с параметром 't' на странице '{page}' меньше предполагаемого: " +
                                            str4);
            if (str3.Split('\'').Length < 2)
                throw new ArgumentException("Массив с параметром 'form_token' меньше предполагаемого: " + str3);
            var str5 = strArray.Where(x => x.Contains("name=\"subject\" ")).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(str5))
                if (str5.Split('"').Length < 12)
                    throw new ArgumentException("Массив с параметром 'subject' меньше предполагаемого: " + str5);

            var format = "mode=editpost&t={0}&p={1}&submit_mode=submit&form_token={3}{4}&message={2}";
            var objArray = new object[5]
            {
                str4.Split('"')[5],
                str1,
                HttpUtility.UrlEncode(message, Encoding.GetEncoding(1251)),
                str3.Split('\'')[1],
                null
            };
            var index1 = 4;
            string str6;
            if (!string.IsNullOrWhiteSpace(str5))
                str6 = string.Format("&subject={0}",
                    HttpUtility.UrlEncode(str5.Split('"')[11], Encoding.GetEncoding(1251)));
            else
                str6 = string.Empty;
            objArray[index1] = str6;
            var s = string.Format(format, objArray);
            for (var index2 = 0; index2 < 1;)
                try
                {
                    if (_webClient == null)
                        DownloadWebPage(string.Format("https://{0}/forum/posting.php?mode=editpost&p={1}",
                            Settings.Current.HostRuTrackerOrg, str1));
                    _webClient.UploadData(
                        string.Format("https://{0}/forum/posting.php?mode=editpost&p={1}",
                            Settings.Current.HostRuTrackerOrg, str1), "POST", Encoding.GetEncoding(1251).GetBytes(s));
                    break;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    _logger.Warn(ex.StackTrace);
                    _logger.Debug(ex);
                    //if (index2 == 20)
                    throw new Exception("Не удалось отправить отчет за 1 попытку. Ошибка " + ex.Message);
//                    Thread.Sleep(index2 * 1000);
                }

            Thread.Sleep(1000);
        }

        public void ReadKeeperInfo()
        {
            var str = DownloadWebPage(string.Format("https://{1}/forum/profile.php?mode=viewprofile&u={0}",
                _userName, Settings.Current.HostRuTrackerOrg)).Split('\r', '\n').Where(x =>
            {
                if (x.Contains("bt:"))
                    return x.Contains("api:");
                return false;
            }).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(str))
                return;
            _apiId = str.Split(new string[2]
            {
                "<b>",
                "</b>"
            }, StringSplitOptions.RemoveEmptyEntries)[3];
            _keeperId = int.Parse(str.Split(new string[2]
            {
                "<b>",
                "</b>"
            }, StringSplitOptions.RemoveEmptyEntries)[5]);

            _logger.Info("Результат авторизации: KeeperID: {0}; KeeperApiKey: {1}", _keeperId, _apiId);
        }
    }
}