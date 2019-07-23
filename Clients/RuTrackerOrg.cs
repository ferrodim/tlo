// Decompiled with JetBrains decompiler
// Type: TLO.local.RuTrackerOrg
// Assembly: TLO.local, Version=2.6.5944.27906, Culture=neutral, PublicKeyToken=null
// MVID: E76CFDB0-1920-4151-9DD8-5FF51DE7CC23
// Assembly location: C:\Users\root\Downloads\TLO_2.6.2.21\TLO.local.exe

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;

namespace TLO.local
{
  internal class RuTrackerOrg
  {
    private Logger _logger;
    private TLOWebClient _webClient;
    private string _userName;
    private string _userPass;
    private int _keeperid;
    private string _apiid;
    private JsonSerializer jSerializer;
    private static RuTrackerOrg _current;

    public static RuTrackerOrg Current
    {
      get
      {
        if (RuTrackerOrg._current == null)
          RuTrackerOrg._current = new RuTrackerOrg();
        return RuTrackerOrg._current;
      }
    }

    private RuTrackerOrg()
      : this((string) null, (string) null)
    {
    }

    public RuTrackerOrg(string userName, string password)
    {
      this.jSerializer = new JsonSerializer();
      this._userName = userName;
      this._userPass = password;
      if (this._logger == null)
        this._logger = LogManager.GetLogger("RuTrackerOrg");
      if (string.IsNullOrWhiteSpace(this._userName) || string.IsNullOrWhiteSpace(this._userPass))
        return;
      this.ReadKeeperInfo();
    }

    public IEnumerable<Category> GetCategories()
    {
      List<Category> source = new List<Category>();
      var downloadArchivePage = this.DownloadArchivePage($"http://api.{Settings.Current.HostRuTrackerOrg}/v1/static/cat_forum_tree");
      JObject jobject1 = (JsonConvert.DeserializeObject(downloadArchivePage) as JObject)["result"].ToObject<JObject>();
      jobject1["c"].ToObject<JObject>();
      source.AddRange((IEnumerable<Category>) jobject1["c"].ToObject<Dictionary<string, object>>().Select<KeyValuePair<string, object>, Category>((Func<KeyValuePair<string, object>, Category>) (x => new Category()
      {
        CategoryID = 1000000 + int.Parse(x.Key),
        Name = x.Value as string
      })).ToArray<Category>());
      source.AddRange(jobject1["f"].ToObject<Dictionary<string, object>>().Select<KeyValuePair<string, object>, Category>((Func<KeyValuePair<string, object>, Category>) (x => new Category()
      {
        CategoryID = int.Parse(x.Key),
        Name = x.Value as string
      })));
      Dictionary<int, Category> dictionary = source.ToDictionary<Category, int, Category>((Func<Category, int>) (x => x.CategoryID), (Func<Category, Category>) (x => x));
      JObject jobject2 = jobject1["tree"].ToObject<JObject>();
      int num = 0;
      foreach (KeyValuePair<string, JToken> keyValuePair1 in jobject2)
      {
        int key1 = int.Parse(keyValuePair1.Key) + 1000000;
        ++num;
        dictionary[key1].OrderID = num;
        dictionary[key1].FullName = dictionary[key1].Name;
        if (!(keyValuePair1.Value is JObject) || !keyValuePair1.Value.Any())
        {
          continue;
        }
        foreach (KeyValuePair<string, JToken> keyValuePair2 in keyValuePair1.Value.ToObject<JObject>())
        {
          int key2 = int.Parse(keyValuePair2.Key);
          ++num;
          if (dictionary.ContainsKey(key2))
          {
            Category category = dictionary[key2];
            category.ParentID = key1;
            category.OrderID = num;
            category.FullName = string.Format("{0} » {1}", dictionary.ContainsKey(key1) ? (object) dictionary[key1].Name : (object) "", (object) category.Name);
          }
          foreach (JToken jtoken in keyValuePair2.Value.ToObject<JArray>())
          {
            int key3 = (int) jtoken;
            ++num;
            if (dictionary.ContainsKey(key3))
            {
              Category category = dictionary[key3];
              category.ParentID = key2;
              category.OrderID = num;
              category.FullName = string.Format("{0} » {1}", dictionary.ContainsKey(key2) ? (object) dictionary[key2].FullName : (object) "", (object) category.Name);
            }
          }
        }
      }
      return (IEnumerable<Category>) source;
    }

    public IEnumerable<Tuple<int, string>> GetCategoriesFromPost(string postUrl)
    {
      List<Tuple<int, string>> tupleList = new List<Tuple<int, string>>();
      string[] array = ((IEnumerable<string>) this.DownloadWebPage(postUrl).Split(new char[2]
      {
        '\r',
        '\n'
      }, StringSplitOptions.RemoveEmptyEntries)).Where<string>((Func<string, bool>) (x =>
      {
        if (x.Contains($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewforum.php?f="))
          return x.Contains("class=\"postLink\"");
        return false;
      })).ToArray<string>();
      int? nullable1 = new int?();
      string str1 = (string) null;
      HashSet<int> intSet = new HashSet<int>();
      foreach (string str2 in array)
      {
        char[] separator = new char[4]{ '"', '<', '>', ' ' };
        int num1 = 1;
        foreach (string postUrl1 in ((IEnumerable<string>) str2.Split(separator, (StringSplitOptions) num1)).Where<string>((Func<string, bool>) (x =>
        {
          if (!x.Contains($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewforum.php?f=") && !x.Contains($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewtopic.php?t="))
            return x.Contains($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewtopic.php?p=");
          return true;
        })).ToArray<string>())
        {
          if (postUrl1.Contains($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewforum.php?f="))
            nullable1 = new int?(int.Parse(postUrl1.Replace($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewforum.php?f=", "")));
          int? nullable2 = nullable1;
          int num2 = 2020;
          if ((nullable2.GetValueOrDefault() == num2 ? (nullable2.HasValue ? 1 : 0) : 0) != 0)
            Console.Write("");
          if (postUrl1.Contains($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewtopic.php?t=") && postUrl1 != $"https://{Settings.Current.HostRuTrackerOrg}/forum/viewtopic.php?t=")
            str1 = postUrl1;
          if (postUrl1.Contains($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewtopic.php?p="))
            str1 = this.GetTopicUrlByPostUrl(postUrl1);
        }
        if (nullable1.HasValue && !string.IsNullOrWhiteSpace(str1))
          tupleList.Add(new Tuple<int, string>(nullable1.Value, str1));
        nullable1 = new int?();
        str1 = (string) null;
      }
      return (IEnumerable<Tuple<int, string>>) tupleList;
    }

    public string GetTopicUrlByPostUrl(string postUrl)
    {
      string str = this.DownloadWebPage(postUrl);
      if (str.Contains("<div class=\"mrg_16\">Тема не найдена</div>"))
        return (string) null;
      return ((IEnumerable<string>) str.Split(new char[1]
      {
        '"'
      }, StringSplitOptions.RemoveEmptyEntries)).Where<string>((Func<string, bool>) (x => x.Contains($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewtopic.php?t="))).Select<string, string>((Func<string, string>) (x => x)).FirstOrDefault<string>();
    }

    public int[][] GetTopicsStatus(int forumID)
    {
      Dictionary<int, Int64[]> dictionary = JsonConvert.DeserializeObject<JObject>(this.DownloadArchivePage(string.Format("http://api.{1}/v1/static/pvc/f/{0}", (object) forumID, Settings.Current.HostRuTrackerOrg)))["result"].ToObject<Dictionary<int, Int64[]>>();
      int[][] numArray1 = new int[dictionary.Count][];
      int index = 0;
      foreach (KeyValuePair<int, Int64[]> keyValuePair in dictionary)
      {
        Int64[] numArray2 = keyValuePair.Value;
        numArray1[index] = new int[2]
        {
          keyValuePair.Key,
          numArray2.Length > 1 ? (int)numArray2[1] : -1
        };
        ++index;
      }
      return numArray1;
    }

    public List<TopicInfo> GetTopicsInfo(int[] topics)
    {
      if (topics == null || topics.Length == 0 || topics.Length > 100)
        return (List<TopicInfo>) null;
      List<TopicInfo> topicInfoList = new List<TopicInfo>();
      foreach (KeyValuePair<int, Dictionary<string, object>> keyValuePair in JsonConvert.DeserializeObject<JObject>(this.DownloadArchivePage(string.Format("http://api.{0}/v1/get_tor_topic_data?by=topic_id&val={1}", Settings.Current.HostRuTrackerOrg, (object) HttpUtility.UrlEncode(string.Join<int>(",", (IEnumerable<int>) topics)))))["result"].ToObject<Dictionary<int, Dictionary<string, object>>>())
      {
        TopicInfo topicInfo = new TopicInfo();
        topicInfo.TopicID = keyValuePair.Key;
        Dictionary<string, object> dictionary = keyValuePair.Value;
        if (dictionary != null)
        {
          topicInfo.Hash = dictionary["info_hash"] as string;
          topicInfo.CategoryID = int.Parse(dictionary["forum_id"].ToString());
          topicInfo.RegTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds((double) int.Parse(dictionary["reg_time"].ToString()));
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
      if (id == null || ((IEnumerable<int>) id).Count<int>() == 0)
        return (IEnumerable<UserInfo>) null;
      List<UserInfo> userInfoList = new List<UserInfo>();
      List<int>[] intListArray = new List<int>[((IEnumerable<int>) id).Count<int>() % 100 == 0 ? ((IEnumerable<int>) id).Count<int>() / 100 : ((IEnumerable<int>) id).Count<int>() / 100 + 1];
      for (int index1 = 0; index1 < ((IEnumerable<int>) id).Count<int>(); ++index1)
      {
        int index2 = index1 / 100;
        if (intListArray[index2] == null)
          intListArray[index2] = new List<int>();
        intListArray[index2].Add(id[index1]);
      }
      foreach (IEnumerable<int> values in intListArray)
      {
        var url = string.Format("http://api.{0}/v1/get_user_name?by=user_id&val={1}", Settings.Current.HostRuTrackerOrg, (object) HttpUtility.UrlEncode(string.Join<int>(",", values)));
        var getUserNameResult = this.DownloadArchivePage(url);
        foreach (KeyValuePair<int, string> keyValuePair in JsonConvert.DeserializeObject<JObject>(getUserNameResult)["result"].ToObject<Dictionary<int, string>>())
          userInfoList.Add(new UserInfo()
          {
            UserID = keyValuePair.Key,
            Name = keyValuePair.Value
          });
        Thread.Sleep(500);
      }
      return (IEnumerable<UserInfo>) userInfoList;
    }

    public List<int> GetPostsFromTopicId(int topicid)
    {
      string empty = string.Empty;
      int num1 = 0;
      List<int> intList = new List<int>();
      string str1;
      do
      {
        empty = string.Empty;
        str1 = this.DownloadWebPage(string.Format("https://{2}/forum/viewtopic.php?t={0}{1}", (object) topicid, num1 == 0 ? (object) "" : (object) ("&start=" + num1.ToString()), Settings.Current.HostRuTrackerOrg));
        if (str1.Contains("<div class=\"mrg_16\">Тема не найдена</div>"))
        {
          Thread.Sleep(500);
          str1 = this.DownloadWebPage(string.Format("https://{0}/forum/viewtopic.php?p={1}", (object) topicid, Settings.Current.HostRuTrackerOrg));
          if (str1.Contains("<div class=\"mrg_16\">Тема не найдена</div>"))
            return new List<int>();
          string s = ((IEnumerable<string>) str1.Split(new char[1]
          {
            '"'
          }, StringSplitOptions.RemoveEmptyEntries)).Where<string>((Func<string, bool>) (x => x.Contains($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewtopic.php?t="))).Select<string, string>((Func<string, string>) (x => x.Replace("https://rutracker.org/forum/viewtopic.php?t=", ""))).FirstOrDefault<string>();
          if (!string.IsNullOrWhiteSpace(s))
          {
            topicid = int.Parse(s);
            goto label_13;
          }
        }
        string str2 = str1;
        char[] separator1 = new char[2]{ '\r', '\n' };
        int num2 = 1;
        foreach (string str3 in ((IEnumerable<string>) str2.Split(separator1, (StringSplitOptions) num2)).Where<string>((Func<string, bool>) (x => x.Contains("\">[Цитировать]</a>"))).ToArray<string>())
        {
          char[] separator2 = new char[1]{ '"' };
          int num3 = 1;
          string str4 = ((IEnumerable<string>) str3.Split(separator2, (StringSplitOptions) num3)).Where<string>((Func<string, bool>) (x => x.Contains("https://"))).FirstOrDefault<string>();
          if (!string.IsNullOrWhiteSpace(str4))
          {
            string[] strArray = str4.Split('=');
            if (strArray.Length >= 3)
              intList.Add(int.Parse(strArray[2]));
          }
        }
        num1 += 30;
label_13:;
      }
      while (str1.Contains("\">След.</a></b></p>") || num1 == 0);
      return intList;
    }

    internal Tuple<string, int, List<int>> GetTopicsFromReport(int postId, int categoryId)
    {
      Tuple<string, int, List<int>> tuple = (Tuple<string, int, List<int>>) null;
      string[] strArray1 = this.DownloadWebPage(string.Format("https://post.{1}/forum/posting.php?mode=quote&p={0}", (object) postId, Settings.Current.HostRuTrackerOrg)).Split(new string[2]
      {
        "<textarea",
        "</textarea>"
      }, StringSplitOptions.RemoveEmptyEntries);
      if (strArray1.Length < 2)
        return tuple;
      string str1 = strArray1[1];
      char[] separator = new char[2]{ '[', ']' };
      int num = 1;
      foreach (string str2 in ((IEnumerable<string>) str1.Split(separator, (StringSplitOptions) num)).Where<string>((Func<string, bool>) (x =>
      {
        if (!x.Contains($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewtopic.php?t="))
          return x.Contains("quote=");
        return true;
      })).ToArray<string>())
      {
        try
        {
          if (str2.Contains("quote="))
            tuple = new Tuple<string, int, List<int>>(str2.Replace("quote=", "").Replace("\"", ""), categoryId, new List<int>());
          else if (tuple != null)
          {
            string[] strArray2 = str2.Split('=');
            if (strArray2.Length >= 3)
              tuple.Item3.Add(int.Parse(strArray2[2]));
          }
        }
        catch (Exception ex)
        {
          this._logger.Error("Ошибка получения информации о раздаче по адресу \"" + str2 + "\": " + ex.Message);
        }
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
        str1 = this.DownloadWebPage(string.Format("https://{2}/forum/viewtopic.php?t={0}{1}",
          (object) topicid, num == 0 ? (object) "" : (object) ("&start=" + num.ToString()), Settings.Current.HostRuTrackerOrg));
        if (str1.Contains("<div class=\"mrg_16\">Тема не найдена</div>"))
        {
          Thread.Sleep(500);
          str1 = this.DownloadWebPage(
            $"https://{Settings.Current.HostRuTrackerOrg}/forum/viewtopic.php?p={(object) topicid}");
          if (str1.Contains("<div class=\"mrg_16\">Тема не найдена</div>"))
            return dictionary;
          var s = ((IEnumerable<string>) string.Join("\r\n", ((IEnumerable<string>) str1.Split(new char[2]
            {
              '\r',
              '\n'
            })).Where<string>((Func<string, bool>) (x => x.Contains("id=\"topic-title\"")))).Split(new char[4]
            {
              '"',
              '<',
              '>',
              ' '
            }, StringSplitOptions.RemoveEmptyEntries))
            .Where<string>((Func<string, bool>) (x => x.Contains($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewtopic.php?t=")))
            .Select<string, string>((Func<string, string>) (x =>
              x.Replace($"https://{Settings.Current.HostRuTrackerOrg}/forum/viewtopic.php?t=", ""))).FirstOrDefault<string>();
          if (!string.IsNullOrWhiteSpace(s))
          {
            topicid = int.Parse(s);
            goto label_18;
          }
        }

        var array = ((IEnumerable<string>) str1.Split(new char[2]
        {
          '\r',
          '\n'
        }, StringSplitOptions.RemoveEmptyEntries)).Where<string>((Func<string, bool>) (x =>
        {
          if (x.Contains("\t\t<a href=\"#\" onclick=\"return false;\">"))
            return true;
          if (x.Contains("viewtopic.php?t=") && x.Contains("class=\"postLink\""))
            return !x.Contains("<div");
          return false;
        })).ToArray<string>();
        var keeperName = string.Empty;
        foreach (var str2 in array)
        {
          if (str2.Contains("\t\t<a href=\"#\" onclick=\"return false;\">"))
          {
            keeperName = str2.Replace("\t\t<a href=\"#\" onclick=\"return false;\">", "").Replace("</a>", "").Replace("<wbr>", "").Trim();
          }
          else
          {
            if (!dictionary.ContainsKey(keeperName))
              dictionary.Add(keeperName, new Tuple<int, List<int>>(categoryId, new List<int>()));
            var str3 = ((IEnumerable<string>) str2.Split(new char[6]
              {
                '"',
                '<',
                '>',
                ' ',
                '#',
                '&'
              }, StringSplitOptions.RemoveEmptyEntries))
              .Where<string>((Func<string, bool>) (x => x.Contains("viewtopic.php?t="))).FirstOrDefault<string>();
            if (!string.IsNullOrWhiteSpace(str3))
            {
              var strArray = str3.Split('=');
              if (strArray.Length >= 2)
              {
                try
                {
                  dictionary[keeperName].Item2.Add(int.Parse(strArray[1]));
                }
                catch (Exception ex)
                {
                  this._logger.Warn(topicid.ToString() + "\t" + strArray[1] + "\t" + ex.Message);
                }
              }
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
      Dictionary<string, Tuple<int, List<int>>> dictionary = new Dictionary<string, Tuple<int, List<int>>>();
      foreach (int postId in this.GetPostsFromTopicId(topicid))
      {
        Tuple<string, int, List<int>> topicsFromReport = this.GetTopicsFromReport(postId, categoryId);
        if (topicsFromReport != null && topicsFromReport.Item3.Count != 0)
        {
          if (!dictionary.ContainsKey(topicsFromReport.Item1))
            dictionary.Add(topicsFromReport.Item1, new Tuple<int, List<int>>(topicsFromReport.Item2, new List<int>()));
          dictionary[topicsFromReport.Item1].Item2.AddRange((IEnumerable<int>) topicsFromReport.Item3);
        }
      }
      return dictionary;
    }

    private string DownloadArchivePage(string page)
    {
      Exception innerException = (Exception) null;
      for (int index = 0; index < 20; ++index)
      {
        string empty = string.Empty;
        TLOWebClient tloWebClient = new TLOWebClient();
        try
        {
          return tloWebClient.DownloadString(page);
        }
        catch (Exception ex)
        {
          innerException = ex;
          if (ex.Message.Contains("404"))
            throw ex;
          Thread.Sleep(index * 1000);
        }
      }
      throw new Exception("Не удалось скачать WEB-страницу за 20 попыток: " + innerException.Message, innerException);
    }

    public string DownloadWebPage(string page, params object[] param)
    {
      return Encoding.GetEncoding("windows-1251").GetString(this.DownloadWebPages(string.Format(page, param)));
    }

    public byte[] DownloadTorrentFile(int id)
    {
      for (int index = 0; index < 100; ++index)
      {
        byte[] numArray1 = new byte[0];
        string empty = string.Empty;
        TLOWebClient tloWebClient = (TLOWebClient) null;
        try
        {
          if (this._webClient == null)
          {
            tloWebClient = new TLOWebClient();
            string s = string.Format("login_username={0}&login_password={1}&login={2}", (object) HttpUtility.UrlEncode(this._userName, Encoding.GetEncoding(1251)), (object) HttpUtility.UrlEncode(this._userPass, Encoding.GetEncoding(1251)), (object) "Вход");
            empty = Encoding.GetEncoding("windows-1251").GetString(tloWebClient.UploadData("https://" + Settings.Current.HostRuTrackerOrg + "/forum/login.php", "POST", Encoding.GetEncoding(1251).GetBytes(s)));
            Thread.Sleep(500);
          }
        }
        catch (Exception ex)
        {
          this._logger.Warn(ex.Message);
          this._logger.Warn<Exception>(ex);
        }
        if (!string.IsNullOrWhiteSpace(empty))
        {
          if (empty.Contains("https://static." + Settings.Current.HostRuTrackerOrg + "/captcha"))
            throw new Exception("При авторизации требуется ввести текст с картинки. Авторизуйтесь на WEB-сайте, а потом повторите попытку");
          if (empty.Contains("<a href=\"profile.php?mode=register\"><b>Регистрация</b></a>"))
            throw new Exception("Не удалось авторизоваться, проверьте логин и пароль");
          this._webClient = tloWebClient;
        }
        byte[] numArray2;
        try
        {
          if (string.IsNullOrWhiteSpace(this._apiid))
          {
            string str = ((IEnumerable<string>) this.DownloadWebPage(string.Format("https://" + Settings.Current.HostRuTrackerOrg + "/forum/viewtopic.php?t={0}", (object) id)).Split(new char[2]
            {
              '\r',
              '\n'
            }, StringSplitOptions.RemoveEmptyEntries)).Where<string>((Func<string, bool>) (x => x.Contains("form_token: '"))).FirstOrDefault<string>();
            if (!string.IsNullOrWhiteSpace(str))
              str = str.Split(new char[1]{ '\'' }, StringSplitOptions.RemoveEmptyEntries)[1];
            string s = string.Format("form_token={0}", (object) str);
            numArray2 = this._webClient.UploadData(string.Format("https://dl." + Settings.Current.HostRuTrackerOrg + "/forum/dl.php?t={0}", (object) id), "POST", Encoding.GetEncoding(1251).GetBytes(s));
          }
          else
            numArray2 = this._webClient.UploadData("https://" + Settings.Current.HostRuTrackerOrg + "/forum/dl.php", "POST", Encoding.GetEncoding(1251).GetBytes(string.Format("keeper_user_id={0}&keeper_api_key={1}&t={2}&add_retracker_url=0", (object) this._keeperid, (object) this._apiid, (object) id)));
        }
        catch (Exception ex)
        {
          if (index >= 20)
            throw new Exception("Не удалось скачать WEB-страницу за 20 попыток:" + ex.Message, ex);
          Thread.Sleep(index * 1000);
          continue;
        }
        string lower = Encoding.GetEncoding(1251).GetString(numArray2).ToLower();
        if (lower.ToLower().Contains("форум временно отключен") || lower.Contains("форум временно отключен"))
          throw new Exception("Форум временно отключен");
        if (lower.Contains("https://static." + Settings.Current.HostRuTrackerOrg + "/captcha") || lower.Contains("<a href=\"profile.php?mode=register\"><b>регистрация</b></a>"))
        {
          if (this._webClient != null)
            this._webClient.Dispose();
          this._webClient = (TLOWebClient) null;
        }
        else
        {
          if (lower[0] == 'd')
            return numArray2;
          string path = Path.Combine(Settings.Current.Folder, "error_" + id.ToString() + ".html");
          if (File.Exists(path))
            File.Delete(path);
          using (FileStream fileStream = File.Create(path))
            fileStream.Write(numArray2, 0, numArray2.Length);
          return (byte[]) null;
        }
      }
      return (byte[]) null;
    }

    public byte[] DownloadWebPages(string page)
    {
      for (int index = 0; index < 20; ++index)
      {
        string empty = string.Empty;
        TLOWebClient tloWebClient = null;
        try
        {
          if (this._webClient == null)
          {
            tloWebClient = new TLOWebClient(Encoding.GetEncoding(1251));
            if (!string.IsNullOrWhiteSpace(this._userName) && !string.IsNullOrWhiteSpace(this._userPass))
            {
              string s = string.Format("login_username={0}&login_password={1}&login={2}", (object) HttpUtility.UrlEncode(this._userName, Encoding.GetEncoding(1251)), (object) HttpUtility.UrlEncode(this._userPass, Encoding.GetEncoding(1251)), (object) "вход");
              empty = Encoding.GetEncoding("windows-1251").GetString(tloWebClient.UploadData($"https://{Settings.Current.HostRuTrackerOrg}/forum/login.php".Replace("rutracker.org", Settings.Current.HostRuTrackerOrg), "POST", Encoding.GetEncoding(1251).GetBytes(s)));
            }
            Thread.Sleep(500);
          }
        }
        catch (Exception ex)
        {
          _logger.Warn(ex.Message);
        }
        if (!string.IsNullOrWhiteSpace(empty) && !string.IsNullOrWhiteSpace(this._userName) && !string.IsNullOrWhiteSpace(this._userPass))
        {
          if (empty.Contains($"https://static.{Settings.Current.HostRuTrackerOrg}/captcha".Replace("rutracker.org", Settings.Current.HostRuTrackerOrg)))
            throw new Exception("При авторизации требуется ввести текст с картинки. Авторизуйтесь на WEB-сайте, а потом повторите попытку");
          if (empty.Contains("<a href=\"profile.php?mode=register\"><b>Регистрация</b></a>"))
            throw new Exception("Не удалось авторизоваться, проверьте логин и пароль");
          this._webClient = tloWebClient;
        }
        byte[] bytes;
        try
        {
          bytes = this._webClient.DownloadData(page);
        }
        catch(Exception e)
        {
          _logger.Warn(e.Message);
          Thread.Sleep(index * 1000);
          continue;
        }
        string str = Encoding.GetEncoding("windows-1251").GetString(bytes);
        if (str.ToLower().Contains("форум временно отключен") || str.ToLower().Contains("форум временно отключен"))
          throw new Exception("Форум временно отключен");
        if (!str.Contains($"https://static.{Settings.Current.HostRuTrackerOrg}/captcha".Replace("rutracker.org", Settings.Current.HostRuTrackerOrg)) && !str.Contains("<a href=\"profile.php?mode=register\"><b>Регистрация</b></a>"))
          return bytes;
        if (this._webClient != null)
          this._webClient.Dispose();
        this._webClient = (TLOWebClient) null;
      }
      throw new Exception("Не удалось скачать WEB-страницу за 20 попыток");
    }

    public byte[] DownloadArchiveData(string page)
    {
      for (int index = 0; index < 20; ++index)
      {
        byte[] numArray = new byte[0];
        string empty = string.Empty;
        if (this._webClient == null)
          this._webClient = new TLOWebClient();
        byte[] bytes;
        try
        {
          bytes = this._webClient.DownloadData(page);
        }
        catch
        {
          Thread.Sleep(index * 1000);
          continue;
        }
        string lower = Encoding.GetEncoding(1251).GetString(bytes).ToLower();
        if (lower.Contains("введите ваше имя и пароль"))
          return new byte[0];
        if (lower.ToLower().Contains("форум временно отключен") || lower.Contains("введите ваше имя и пароль"))
          throw new Exception("Форум временно отключен");
        if (lower[0] != 'd')
          return new byte[0];
        return bytes;
      }
      throw new Exception("Не удалось скачать WEB-страницу за 20 попыток");
    }

    public void SavePage(string topicID, string folder)
    {
      string str = new TLOWebClient().DownloadString(string.Format("https://rutracker.org/forum/viewtopic.php?t={0}", (object) topicID));
      if (str.Contains("Тема не найдена"))
        return;
      using (FileStream fileStream = File.Create(Path.Combine(folder, string.Format("{0}.html", (object) topicID))))
      {
        using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream, Encoding.GetEncoding(1251)))
          streamWriter.Write(str);
      }
    }

    public void SendReport(string url, string message)
    {
      if (((IEnumerable<string>) url.Split('#')).FirstOrDefault<string>().Split('=').Length < 3)
        throw new ArgumentException("Не корректно указан адрес отправки отчета: " + url);
      string str1 = ((IEnumerable<string>) url.Split('#')).FirstOrDefault<string>().Split('=')[2];
      var page = string.Format("https://{1}/forum/posting.php?mode=editpost&p={0}", (object) str1, Settings.Current.HostRuTrackerOrg);
      string[] strArray = this.DownloadWebPage(page).Split(new char[2]
      {
        '\r',
        '\n'
      }, StringSplitOptions.RemoveEmptyEntries);
      Thread.Sleep(1000);
//      string.Format("align=-1&codeColor=black&codeSize=12&codeUrl2=&decflag=2&f=1584&fontFace=-1&form_token=c2a9bace5d7f3900e2bddbf5f0f0f94a&message=&mode=editpost&p=59972538&submit_mode=submit&t=3985106");
      string str3 = ((IEnumerable<string>) strArray).Where<string>((Func<string, bool>) (x => x.Contains("form_token: '"))).FirstOrDefault<string>();
      if (string.IsNullOrWhiteSpace(str3))
        throw new ArgumentException("Параметр 'form_token' не найден на странице");
      string str4 = ((IEnumerable<string>) strArray).Where<string>((Func<string, bool>) (x => x.Contains("name=\"t\" value=\""))).FirstOrDefault<string>();
      if (string.IsNullOrWhiteSpace(str4))
        throw new ArgumentException($"Параметр 't' не найден на странице '{page}'");
      if (str4.Split('"').Length < 6)
        throw new ArgumentException($"Массив с параметром 't' на странице '{page}' меньше предполагаемого: " + str4);
      if (str3.Split('\'').Length < 2)
        throw new ArgumentException("Массив с параметром 'form_token' меньше предполагаемого: " + str3);
      string str5 = ((IEnumerable<string>) strArray).Where<string>((Func<string, bool>) (x => x.Contains("name=\"subject\" "))).FirstOrDefault<string>();
      if (!string.IsNullOrWhiteSpace(str5))
      {
        if (str5.Split('"').Length < 12)
          throw new ArgumentException("Массив с параметром 'subject' меньше предполагаемого: " + str5);
      }
      string format = "mode=editpost&t={0}&p={1}&submit_mode=submit&form_token={3}{4}&message={2}";
      object[] objArray = new object[5]
      {
        (object) str4.Split('"')[5],
        (object) str1,
        (object) HttpUtility.UrlEncode(message, Encoding.GetEncoding(1251)),
        (object) str3.Split('\'')[1],
        null
      };
      int index1 = 4;
      string str6;
      if (!string.IsNullOrWhiteSpace(str5))
        str6 = string.Format("&subject={0}", (object) HttpUtility.UrlEncode(str5.Split('"')[11], Encoding.GetEncoding(1251)));
      else
        str6 = string.Empty;
      objArray[index1] = (object) str6;
      string s = string.Format(format, objArray);
      for (int index2 = 0; index2 < 20; ++index2)
      {
        try
        {
          if (this._webClient == null)
            this.DownloadWebPage(string.Format("https://{0}/forum/posting.php?mode=editpost&p={1}", (object) Settings.Current.HostRuTrackerOrg, (object) str1));
          this._webClient.UploadData(string.Format("https://{0}/forum/posting.php?mode=editpost&p={1}", (object) Settings.Current.HostRuTrackerOrg, (object) str1), "POST", Encoding.GetEncoding(1251).GetBytes(s));
          break;
        }
        catch (Exception ex)
        {
          if (index2 == 20)
            throw new Exception("Не удалось отправить отчет за 10 попыток. Ошибка " + ex.Message);
          Thread.Sleep(index2 * 1000);
        }
      }
      Thread.Sleep(1000);
    }

    public void ReadKeeperInfo()
    {
        try
        {
            string str = ((IEnumerable<string>)this.DownloadWebPage(string.Format("https://{1}/forum/profile.php?mode=viewprofile&u={0}", (object)this._userName, Settings.Current.HostRuTrackerOrg)).Split(new char[2]
        {
          '\r',
          '\n'
        })).Where<string>((Func<string, bool>)(x =>
        {
            if (x.Contains("bt:"))
                return x.Contains("api:");
            return false;
        })).FirstOrDefault<string>();
            if (string.IsNullOrWhiteSpace(str))
                return;
            this._apiid = str.Split(new string[2]
        {
          "<b>",
          "</b>"
        }, StringSplitOptions.RemoveEmptyEntries)[3];
            this._keeperid = int.Parse(str.Split(new string[2]
        {
          "<b>",
          "</b>"
        }, StringSplitOptions.RemoveEmptyEntries)[5]);
        }
        catch
        {
        }
        this._logger.Info<int, string>("Результат авторизации: KeeperID: {0}; KeeperApiKey: {1}", this._keeperid, this._apiid);
    }
  }
}
