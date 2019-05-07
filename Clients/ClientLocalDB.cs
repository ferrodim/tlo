// Decompiled with JetBrains decompiler
// Type: TLO.local.ClientLocalDB
// Assembly: TLO.local, Version=2.6.5944.27906, Culture=neutral, PublicKeyToken=null
// MVID: E76CFDB0-1920-4151-9DD8-5FF51DE7CC23
// Assembly location: C:\Users\root\Downloads\TLO_2.6.2.21\TLO.local.exe

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Data.Sqlite;
using NLog;

namespace TLO.local
{
  internal class ClientLocalDB
  {
    private static ClientLocalDB _current;
    private SqliteConnection _conn;
    private static Logger _logger;

    public static ClientLocalDB Current
    {
      get
      {
        if (ClientLocalDB._current == null)
          ClientLocalDB._current = new ClientLocalDB();
        return ClientLocalDB._current;
      }
    }

    public string FileDatabase
    {
      get
      {
        return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Database.db");
      }
    }

    private ClientLocalDB()
    {
      if (ClientLocalDB._logger == null)
        ClientLocalDB._logger = LogManager.GetLogger("ClientServer");
      bool flag = false;
      if (!File.Exists(this.FileDatabase))
        flag = true;
      try
      {
        _conn = new DBConnectionCreator().Connection;
      }
      catch
      {
        flag = true;
      }
      if (flag)
        this.CreateDatabase();
      this.SaveToDatabase();
      this.UpdateDataBase();
    }

    public void SaveToDatabase()
    {
      if (!DBConnectionCreator.InMemory())
      {
        // не нужно пересохранять в БД если, она не хранится в памяти.
        return;
      }
      try
      {
        if (File.Exists(this.FileDatabase + ".tmp"))
          File.Delete(this.FileDatabase + ".tmp");
        using (SqliteConnection destination = new SqliteConnection(string.Format("Data Source={0};Version=3;", (object) (this.FileDatabase + ".tmp"))))
        {
          destination.Open();
//          this._conn.BackupDatabase(destination, "main", "main", -1, null, -1);
          destination.Close();
        }
      }
      catch (Exception ex)
      {
        ClientLocalDB._logger.Error(ex.Message + "\r\n" + ex.StackTrace);
      }
      if (!File.Exists(this.FileDatabase + ".tmp"))
        return;
      _conn?.Close();
      if (File.Exists(this.FileDatabase))
        File.Delete(this.FileDatabase);
      File.Move(this.FileDatabase + ".tmp", this.FileDatabase);
      this._conn = new DBConnectionCreator().Connection;
    }

    private void CreateDatabase()
    {
      _conn?.Close();
      if (File.Exists(this.FileDatabase))
        File.Delete(this.FileDatabase);
      _conn = new DBConnectionCreator().Connection;
      using (SqliteCommand command = this._conn.CreateCommand())
      {
        command.CommandText = "\r\nCREATE TABLE Category(CategoryID INTEGER PRIMARY KEY ASC, ParentID INTEGER, OrderID INT, Name TEXT NOT NULL, FullName TEXT NOT NULL, IsEnable BIT, CountSeeders int, \r\n    TorrentClientUID TEXT, Folder TEXT, AutoDownloads INT, LastUpdateTopics DATETIME, LastUpdateStatus DATETIME, Label TEXT, ReportTopicID INT);\r\nCREATE TABLE Topic (TopicID INT PRIMARY KEY ASC, CategoryID INT, Name TEXT, Hash TEXT, Size INTEGER, Seeders INT, AvgSeeders DECIMAL(18,4), Status INT, IsActive BIT, IsDeleted BIT, IsKeep BIT, IsKeepers BIT, IsBlackList BIT, IsDownload BIT, RegTime DATETIME, PosterID INT);\r\nCREATE INDEX IX_Topic__Hash ON Topic (Hash);\r\nCREATE TABLE TopicStatusHystory (TopicID INT NOT NULL, Date DateTime NOT NULL, Seeders INT, PRIMARY KEY(TopicID ASC, Date ASC));\r\nCREATE TABLE TorrentClient(UID NVARCHAR(50) PRIMARY KEY ASC NOT NULL, Name NVARCHAR(100) NOT NULL, Type VARCHAR(50) NOT NULL, ServerName NVARCHAR(50) NOT NULL, ServerPort INT NOT NULL, UserName NVARCHAR(50), UserPassword NVARCHAR(50), LastReadHash DATETIME);\r\nCREATE TABLE Report(CategoryID INT NOT NULL, ReportNo INT NOT NULL, URL TEXT, Report TEXT, PRIMARY KEY(CategoryID ASC, ReportNo ASC));\r\nCREATE TABLE Keeper (KeeperName nvarchar(100) not null, CategoryID int not null, Count INT NOT NULL, Size DECIMAL(18,4) NOT NULL, PRIMARY KEY(KeeperName ASC, CategoryID ASC));\r\nCREATE TABLE KeeperToTopic(KeeperName NVARCHAR(50) NOT NULL, CategoryID INT NULL, TopicID INT NOT NULL, PRIMARY KEY(KeeperName ASC, TopicID ASC));\r\nCREATE TABLE User (UserID INT PRIMARY KEY ASC NOT NULL, Name NVARCHAR(100) NOT NULL);\r\n";
        command.ExecuteNonQuery();
      }
    }

    public void ClearDatabase()
    {
      using (SqliteTransaction sqLiteTransaction = this._conn.BeginTransaction())
      {
        using (SqliteCommand command = this._conn.CreateCommand())
        {
          HashSet<int> intSet = new HashSet<int>();
          command.Transaction = sqLiteTransaction;
          command.CommandText = "DELETE FROM Topic";
          command.ExecuteNonQuery();
          command.CommandText = "DELETE FROM TopicStatusHystory";
          command.ExecuteNonQuery();
          command.CommandText = "UPDATE Report SET Report = ''";
          command.ExecuteNonQuery();
        }
        sqLiteTransaction.Commit();
        using (SqliteCommand command = this._conn.CreateCommand())
        {
          command.CommandText = "vacuum;";
          command.ExecuteNonQuery();
        }
      }
    }

    public void UpdateDataBase()
    {
      using (var command = _conn.CreateCommand())
      {
        var updated = false;
        command.CommandText = "PRAGMA user_version";
        var result = (long)command.ExecuteScalar();
        for (var i = 0; i <= 1; i++, result++)
        {
          switch (result)
          {
            case 0:
              command.CommandText = @"CREATE INDEX keepername_topicid_idx ON KeeperToTopic (TopicID, KeeperName)";
              command.ExecuteNonQuery();
              updated = true;
              continue;
            default:
              command.CommandText = $"PRAGMA user_version={i}";
              command.ExecuteNonQuery();
              break;
          }
        }

        if (updated)
        {
          this.SaveToDatabase();
        }
      }
    }

    public IEnumerable<UserInfo> GetUsers()
    {
      List<UserInfo> userInfoList = new List<UserInfo>();
      using (SqliteCommand command = this._conn.CreateCommand())
      {
        command.CommandText = "SELECT * FROM User";
        using (SqliteDataReader SqliteDataReader = command.ExecuteReader())
        {
          while (SqliteDataReader.Read())
            userInfoList.Add(new UserInfo()
            {
              UserID = SqliteDataReader.GetInt32(0),
              Name = SqliteDataReader.GetString(1)
            });
        }
      }
      return (IEnumerable<UserInfo>) userInfoList;
    }

    public void SaveUsers(IEnumerable<UserInfo> data)
    {
      if (data == null || data.Count<UserInfo>() == 0)
        return;
      using (SqliteTransaction sqLiteTransaction = this._conn.BeginTransaction())
      {
        using (SqliteCommand command = this._conn.CreateCommand())
        {
          command.Transaction = sqLiteTransaction;
          command.CommandText = "INSERT OR REPLACE INTO User(UserID, Name) VALUES(@UserID, @Name);";
          command.Parameters.Add("@UserID", DbType.Int32);
          command.Parameters.Add("@Name", DbType.String);
          command.Prepare();
          foreach (UserInfo userInfo in data)
          {
            command.Parameters[0].Value = (object) userInfo.UserID;
            command.Parameters[1].Value = (object) (userInfo.Name ?? "<Удален>");
            command.ExecuteNonQuery();
          }
        }
        sqLiteTransaction.Commit();
      }
    }

    public int[] GetNoUsers()
    {
      List<int> intList = new List<int>();
      using (SqliteCommand command = this._conn.CreateCommand())
      {
        command.CommandText = "\r\nSELECT DISTINCT t.PosterID\r\nFROM \r\n     Topic AS t     \r\n     LEFT JOIN User AS u ON (t.PosterID = u.UserID)     \r\nWHERE\r\n     t.PosterID IS NOT NULL AND u.Name IS NULL";
        using (SqliteDataReader SqliteDataReader = command.ExecuteReader())
        {
          while (SqliteDataReader.Read())
          {
            var posterId = SqliteDataReader.GetInt32(0);
            if (posterId > 0)
            {
              intList.Add(posterId);
            }
          }
        }
      }
      return intList.ToArray();
    }

    public void CategoriesSave(IEnumerable<Category> data, bool isLoad = false)
    {
      using (SqliteTransaction sqLiteTransaction = this._conn.BeginTransaction())
      {
        using (SqliteCommand command = this._conn.CreateCommand())
        {
          HashSet<int> hash = new HashSet<int>();
          command.Transaction = sqLiteTransaction;
          command.CommandText = string.Format("select CategoryID FROM Category WHERE CategoryID IN ({0})", (object) string.Join<int>(",", data.Select<Category, int>((Func<Category, int>) (x => x.CategoryID))));
          using (SqliteDataReader SqliteDataReader = command.ExecuteReader())
          {
            while (SqliteDataReader.Read())
              hash.Add(SqliteDataReader.GetInt32(0));
          }
          if (isLoad)
          {
            command.CommandText = "UPDATE Category SET ParentID = @ParentID, OrderID = @OrderID, Name = @Name, FullName = @FullName WHERE CategoryID = @ID";
            IEnumerable<Category> source = data;
            foreach (Category category in source.Where<Category>((Func<Category, bool>) (x => hash.Contains(x.CategoryID))))
            {
              command.Parameters.Clear();
              command.Parameters.AddWithValue("@ID", (object) category.CategoryID);
              command.Parameters.AddWithValue("@ParentID", (object) category.ParentID);
              command.Parameters.AddWithValue("@OrderID", (object) category.OrderID);
              command.Parameters.AddWithValue("@Name", (object) category.Name);
              command.Parameters.AddWithValue("@FullName", string.IsNullOrWhiteSpace(category.FullName) ? (object) category.Name : (object) category.FullName);
              command.ExecuteNonQuery();
            }
          }
          else
          {
            command.CommandText = "UPDATE Category SET IsEnable = 0 WHERE IsEnable = 1";
            command.ExecuteNonQuery();
            command.CommandText = "UPDATE Category SET IsEnable = @IsEnable, Folder = @Folder, LastUpdateTopics = @LastUpdateTopics, LastUpdateStatus = @LastUpdateStatus, CountSeeders = @CountSeeders, TorrentClientUID = @TorrentClientUID, Label = @Label WHERE CategoryID = @ID";
            IEnumerable<Category> source = data;
            foreach (Category category in source.Where<Category>((Func<Category, bool>) (x => hash.Contains(x.CategoryID))))
            {
              string str = string.Format("{0}|{1}|{2}|{3}", (object) category.Folder, (object) category.CreateSubFolder, category.IsSaveTorrentFiles ? (object) "1" : (object) "0", category.IsSaveWebPage ? (object) "1" : (object) "0");
              command.Parameters.Clear();
              command.Parameters.AddWithValue("@ID", (object) category.CategoryID);
              command.Parameters.AddWithValue("@IsEnable", (object) category.IsEnable);
              command.Parameters.AddWithValue("@CountSeeders", (object) category.CountSeeders);
              command.Parameters.AddWithValue("@TorrentClientUID", (object) category.TorrentClientUID.ToString());
              command.Parameters.AddWithValue("@Folder", (object) str);
              command.Parameters.AddWithValue("@LastUpdateTopics", (object) category.LastUpdateTopics);
              command.Parameters.AddWithValue("@LastUpdateStatus", (object) category.LastUpdateStatus);
              command.Parameters.AddWithValue("@Label", (object) category.Label);
              command.ExecuteNonQuery();
            }
          }
          command.CommandText = "INSERT OR REPLACE INTO Category (CategoryID, ParentID, OrderID, Name, FullName, IsEnable, Folder, LastUpdateTopics, LastUpdateStatus, Label) \r\nVALUES(@ID, @ParentID, @OrderID, @Name, @FullName, @IsEnable, @Folder, @LastUpdateTopics, @LastUpdateStatus, @Label)";
          IEnumerable<Category> source1 = data;
          foreach (Category category in source1.Where<Category>((Func<Category, bool>) (x => !hash.Contains(x.CategoryID))))
          {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@ID", (object) category.CategoryID);
            command.Parameters.AddWithValue("@ParentID", (object) category.ParentID);
            command.Parameters.AddWithValue("@OrderID", (object) category.OrderID);
            command.Parameters.AddWithValue("@Name", (object) category.Name);
            command.Parameters.AddWithValue("@FullName", string.IsNullOrWhiteSpace(category.FullName) ? (object) category.Name : (object) category.FullName);
            command.Parameters.AddWithValue("@IsEnable", (object) category.IsEnable);
            command.Parameters.AddWithValue("@CountSeeders", (object) category.CountSeeders);
            command.Parameters.AddWithValue("@Folder", (object) category.Folder);
            command.Parameters.AddWithValue("@LastUpdateTopics", (object) category.LastUpdateTopics);
            command.Parameters.AddWithValue("@LastUpdateStatus", (object) category.LastUpdateStatus);
            command.Parameters.AddWithValue("@Label", (object) category.Label);
            command.ExecuteNonQuery();
          }
        }
        sqLiteTransaction.Commit();
      }
    }

    public List<Category> GetCategories()
    {
      List<Category> categoryList = new List<Category>();
      using (SqliteCommand command = this._conn.CreateCommand())
      {
        command.CommandText = "\r\nSELECT CategoryID, ParentID, OrderID, Name, FullName, IsEnable, Folder, LastUpdateTopics, LastUpdateStatus, CountSeeders, TorrentClientUID, ReportTopicID, Label FROM Category";
        using (SqliteDataReader SqliteDataReader = command.ExecuteReader())
        {
          while (SqliteDataReader.Read())
          {
            Category category = new Category()
            {
              CategoryID = SqliteDataReader.GetInt32(0),
              ParentID = SqliteDataReader.GetInt32(1),
              OrderID = SqliteDataReader.GetInt32(2),
              Name = SqliteDataReader.GetString(3),
              FullName = SqliteDataReader.GetString(4),
              IsEnable = SqliteDataReader.GetBoolean(5),
              CountSeeders = SqliteDataReader.IsDBNull(9) ? 2 : SqliteDataReader.GetInt32(9),
              TorrentClientUID = SqliteDataReader.IsDBNull(10) ? Guid.Empty : Guid.Parse(SqliteDataReader.GetString(10)),
              LastUpdateTopics = SqliteDataReader.GetDateTime(7),
              LastUpdateStatus = SqliteDataReader.GetDateTime(8),
              ReportList = SqliteDataReader.IsDBNull(11) ? string.Empty : SqliteDataReader.GetString(11),
              Label = SqliteDataReader.IsDBNull(12) ? string.Empty : SqliteDataReader.GetString(12)
            };
            string str = SqliteDataReader.IsDBNull(6) ? (string) null : SqliteDataReader.GetString(6);
            if (!string.IsNullOrWhiteSpace(str))
            {
              string[] strArray = str.Split('|');
              if (strArray.Length >= 1)
                category.Folder = strArray[0];
              if (strArray.Length >= 2)
                category.CreateSubFolder = int.Parse(strArray[1]);
              if (strArray.Length >= 3)
              {
                category.IsSaveTorrentFiles = strArray[2] == "1";
                category.FolderTorrentFile = Path.Combine(category.Folder, "!!!Torrent-files!!!");
              }
              if (strArray.Length >= 4)
              {
                category.IsSaveWebPage = strArray[3] == "1";
                category.FolderSavePageForum = Path.Combine(category.Folder, "!!!Web-pages!!!");
              }
            }
            categoryList.Add(category);
          }
        }
      }
      return categoryList;
    }

    public List<Category> GetCategoriesEnable()
    {
      List<Category> categoryList = new List<Category>();
      using (SqliteCommand command = this._conn.CreateCommand())
      {
        command.CommandText = "\r\nSELECT CategoryID, ParentID, OrderID, Name, FullName, IsEnable, Folder, LastUpdateTopics, LastUpdateStatus, CountSeeders, TorrentClientUID, Label FROM Category WHERE IsEnable = 1 ORDER BY FullName";
        using (SqliteDataReader SqliteDataReader = command.ExecuteReader())
        {
          while (SqliteDataReader.Read())
          {
            Category category = new Category()
            {
              CategoryID = SqliteDataReader.GetInt32(0),
              ParentID = SqliteDataReader.GetInt32(1),
              OrderID = SqliteDataReader.GetInt32(2),
              Name = SqliteDataReader.GetString(3),
              FullName = SqliteDataReader.GetString(4),
              IsEnable = SqliteDataReader.GetBoolean(5),
              CountSeeders = SqliteDataReader.IsDBNull(9) ? 2 : SqliteDataReader.GetInt32(9),
              TorrentClientUID = SqliteDataReader.IsDBNull(10) ? Guid.Empty : Guid.Parse(SqliteDataReader.GetString(10)),
              LastUpdateTopics = SqliteDataReader.GetDateTime(7),
              LastUpdateStatus = SqliteDataReader.GetDateTime(8),
              Label = SqliteDataReader.IsDBNull(11) ? string.Empty : SqliteDataReader.GetString(11)
            };
            string str = SqliteDataReader.IsDBNull(6) ? (string) null : SqliteDataReader.GetString(6);
            if (!string.IsNullOrWhiteSpace(str))
            {
              string[] strArray = str.Split('|');
              if (strArray.Length >= 1)
                category.Folder = strArray[0];
              if (strArray.Length >= 2)
                category.CreateSubFolder = int.Parse(strArray[1]);
              if (strArray.Length >= 3)
              {
                category.IsSaveTorrentFiles = strArray[2] == "1";
                category.FolderTorrentFile = Path.Combine(category.Folder, "!!!Torrent-files!!!");
              }
              if (strArray.Length >= 4)
              {
                category.IsSaveWebPage = strArray[3] == "1";
                category.FolderSavePageForum = Path.Combine(category.Folder, "!!!Web-pages!!!");
              }
            }
            categoryList.Add(category);
          }
        }
      }
      return categoryList;
    }

    public void ResetFlagsTopicDownloads()
    {
      using (SqliteCommand command = this._conn.CreateCommand())
      {
        command.CommandText = "UPDATE Topic SET IsKeep = 0, IsDownload = 0";
        command.ExecuteNonQuery();
      }
    }

    public void SaveTopicInfo(List<TopicInfo> data, bool isUpdateTopic = false)
    {
      DateTime dateTime = new DateTime(2000, 1, 1);
      using (SqliteTransaction sqLiteTransaction = this._conn.BeginTransaction())
      {
        using (SqliteCommand command = this._conn.CreateCommand())
        {
          command.Transaction = sqLiteTransaction;
          if (isUpdateTopic)
          {
            command.CommandText = "UPDATE Category SET LastUpdateTopics = @LastUpdateTopics WHERE CategoryID = @CategoryID";
            foreach (int num in data.Select<TopicInfo, int>((Func<TopicInfo, int>) (x => x.CategoryID)).Distinct<int>())
            {
              command.Parameters.Clear();
              command.Parameters.AddWithValue("@CategoryID", (object) num);
              command.Parameters.AddWithValue("@LastUpdateTopics", (object) DateTime.Now);
              command.ExecuteNonQuery();
            }
          }
          command.CommandText = string.Format("SELECT TopicID FROM Topic WHERE TopicID IN ({0})", (object) string.Join<int>(",", data.Select<TopicInfo, int>((Func<TopicInfo, int>) (x => x.TopicID))));
          List<int> list = new List<int>();
          using (SqliteDataReader SqliteDataReader = command.ExecuteReader())
          {
            while (SqliteDataReader.Read())
              list.Add(SqliteDataReader.GetInt32(0));
          }
          if (isUpdateTopic)
          {
            command.CommandText = "UPDATE Topic SET CategoryID = @CategoryID, Name = @Name, Hash = @Hash, Size = @Size, Seeders = @Seeders, Status = @Status, IsDeleted = @IsDeleted, RegTime = @RegTime, PosterID = @PosterID WHERE TopicID = @TopicID;";
            List<TopicInfo> source = data;
            foreach (TopicInfo topicInfo in source.Where<TopicInfo>((Func<TopicInfo, bool>) (x => list.Contains(x.TopicID))))
            {
              command.Parameters.Clear();
              command.Parameters.AddWithValue("@TopicID", (object) topicInfo.TopicID);
              command.Parameters.AddWithValue("@CategoryID", (object) topicInfo.CategoryID);
              command.Parameters.AddWithValue("@Name", (object) topicInfo.Name2);
              command.Parameters.AddWithValue("@Hash", (object) topicInfo.Hash);
              command.Parameters.AddWithValue("@Size", (object) topicInfo.Size);
              command.Parameters.AddWithValue("@Seeders", (object) topicInfo.Seeders);
              command.Parameters.AddWithValue("@Status", (object) topicInfo.Status);
              command.Parameters.AddWithValue("@IsDeleted", (object) 0);
              command.Parameters.AddWithValue("@RegTime", (object) (topicInfo.RegTime < dateTime ? dateTime : topicInfo.RegTime));
              command.Parameters.AddWithValue("@PosterID", (object) topicInfo.PosterID);
              command.ExecuteNonQuery();
            }
          }
          else
          {
            command.CommandText = "UPDATE Topic SET CategoryID = @CategoryID, Name = @Name, Hash = @Hash, Size = @Size, Seeders = @Seeders, Status = @Status, IsDeleted = @IsDeleted, IsKeep = @IsKeep, IsKeepers = @IsKeepers, IsBlackList = @IsBlackList, IsDownload = @IsDownload WHERE TopicID = @TopicID;";
            List<TopicInfo> source = data;
            foreach (TopicInfo topicInfo in source.Where<TopicInfo>((Func<TopicInfo, bool>) (x => list.Contains(x.TopicID))))
            {
              command.Parameters.Clear();
              command.Parameters.AddWithValue("@TopicID", (object) topicInfo.TopicID);
              command.Parameters.AddWithValue("@CategoryID", (object) topicInfo.CategoryID);
              command.Parameters.AddWithValue("@Name", (object) topicInfo.Name2);
              command.Parameters.AddWithValue("@Hash", (object) topicInfo.Hash);
              command.Parameters.AddWithValue("@Size", (object) topicInfo.Size);
              command.Parameters.AddWithValue("@Seeders", (object) topicInfo.Seeders);
              command.Parameters.AddWithValue("@Status", (object) topicInfo.Status);
              command.Parameters.AddWithValue("@IsDeleted", (object) 0);
              command.Parameters.AddWithValue("@IsKeep", (object) topicInfo.IsKeep);
              command.Parameters.AddWithValue("@IsKeepers", (object) topicInfo.IsKeeper);
              command.Parameters.AddWithValue("@IsBlackList", (object) topicInfo.IsBlackList);
              command.Parameters.AddWithValue("@IsDownload", (object) topicInfo.IsDownload);
              command.ExecuteNonQuery();
            }
          }
          command.CommandText = "\r\nINSERT OR REPLACE INTO Topic (TopicID, CategoryID, Name, Hash, Size, Seeders, Status, IsActive, IsDeleted, IsKeep, IsKeepers, IsBlackList, IsDownload, RegTime, PosterID)\r\nVALUES(@TopicID, @CategoryID, @Name, @Hash, @Size, @Seeders, @Status, @IsActive, @IsDeleted, @IsKeep, @IsKeepers, @IsBlackList, @IsDownload, @RegTime, @PosterID);";
          List<TopicInfo> source1 = data;
          foreach (TopicInfo topicInfo in source1.Where<TopicInfo>((Func<TopicInfo, bool>) (x => !list.Contains(x.TopicID))))
          {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@TopicID", (object) topicInfo.TopicID);
            command.Parameters.AddWithValue("@CategoryID", (object) topicInfo.CategoryID);
            command.Parameters.AddWithValue("@Name", (object) topicInfo.Name2);
            command.Parameters.AddWithValue("@Hash", (object) topicInfo.Hash);
            command.Parameters.AddWithValue("@Size", (object) topicInfo.Size);
            command.Parameters.AddWithValue("@Seeders", (object) topicInfo.Seeders);
            command.Parameters.AddWithValue("@Status", (object) topicInfo.Status);
            command.Parameters.AddWithValue("@IsActive", (object) 1);
            command.Parameters.AddWithValue("@IsDeleted", (object) 0);
            command.Parameters.AddWithValue("@IsKeep", (object) topicInfo.IsKeep);
            command.Parameters.AddWithValue("@IsKeepers", (object) topicInfo.IsKeeper);
            command.Parameters.AddWithValue("@IsBlackList", (object) topicInfo.IsBlackList);
            command.Parameters.AddWithValue("@IsDownload", (object) topicInfo.IsDownload);
            command.Parameters.AddWithValue("@RegTime", (object) (topicInfo.RegTime < dateTime ? dateTime : topicInfo.RegTime));
            command.Parameters.AddWithValue("@PosterID", (object) topicInfo.PosterID);
            command.ExecuteNonQuery();
          }
        }
        sqLiteTransaction.Commit();
      }
      this.SaveStatus(data.Select<TopicInfo, int[]>((Func<TopicInfo, int[]>) (x => new int[2]
      {
        x.TopicID,
        x.Seeders
      })).ToArray<int[]>(), true);
    }

    internal void DeleteTopicsByCategoryId(int categoryID)
    {
      using (SqliteTransaction sqLiteTransaction = this._conn.BeginTransaction())
      {
        using (SqliteCommand command = this._conn.CreateCommand())
        {
          command.Transaction = sqLiteTransaction;
          command.Parameters.AddWithValue("@categoryID", (object) categoryID);
          command.CommandText = "UPDATE Topic SET IsDeleted = 1 WHERE CategoryID = @categoryID;";
          command.ExecuteNonQuery();
        }
        sqLiteTransaction.Commit();
      }
    }

    public void ClearHistoryStatus()
    {
      using (SqliteTransaction sqLiteTransaction = this._conn.BeginTransaction())
      {
        using (SqliteCommand command = this._conn.CreateCommand())
        {
          command.Transaction = sqLiteTransaction;
          command.Parameters.Add("@Date", DbType.DateTime);
          command.Parameters[0].Value = (object) DateTime.Now.Date.AddDays((double) -Settings.Current.CountDaysKeepHistory);
          command.CommandText = "DELETE FROM TopicStatusHystory WHERE Date <= @Date;";
          command.ExecuteNonQuery();
        }
        sqLiteTransaction.Commit();
      }
    }

    public void SaveStatus(int[][] data, bool isUpdateStatus = false)
    {
      using (SqliteTransaction sqLiteTransaction = this._conn.BeginTransaction())
      {
        using (SqliteCommand command = this._conn.CreateCommand())
        {
          command.Transaction = sqLiteTransaction;
          command.CommandText = "\r\nUPDATE Topic SET Seeders = @Seeders WHERE TopicID = @TopicID;\r\nINSERT OR REPLACE INTO TopicStatusHystory VALUES(@TopicID, @Date, @Seeders);\r\n";
          if (Settings.Current.IsNotSaveStatistics)
            command.CommandText = "UPDATE Topic SET Seeders = @Seeders WHERE TopicID = @TopicID;";
          command.Parameters.Clear();
          command.Parameters.Add("@Date", DbType.DateTime);
          command.Parameters.Add("@TopicID", DbType.Int32);
          command.Parameters.Add("@Seeders", DbType.Int32);
          command.Parameters[0].Value = (object) DateTime.Now;
          foreach (int[] numArray in data)
          {
            command.Parameters[1].Value = (object) numArray[0];
            command.Parameters[2].Value = (object) numArray[1];
            command.ExecuteNonQuery();
          }
        }
        sqLiteTransaction.Commit();
      }
    }

    public List<TopicInfo> GetTopicsByCategory(int categoyid)
    {
      DateTime dateTime = new DateTime(2000, 1, 1);
      List<TopicInfo> topicInfoList = new List<TopicInfo>();
      using (SqliteCommand command = this._conn.CreateCommand())
      {
        command.CommandText = "SELECT TopicID, CategoryID, Name, Hash, Size, Seeders, Status, IsActive, IsDeleted, IsKeep, IsKeepers, IsBlackList, IsDownload, AvgSeeders, RegTime, PosterID\r\nFROM Topic WHERE (CategoryID = @CategoryID OR @CategoryID = -1) AND IsDeleted = 0 AND Status NOT IN (7,4,11,5) and Hash IS NOT NULL";
        command.Parameters.AddWithValue("@CategoryID", (object) categoyid);
        using (SqliteDataReader SqliteDataReader = command.ExecuteReader())
        {
          while (SqliteDataReader.Read())
            topicInfoList.Add(new TopicInfo()
            {
              TopicID = SqliteDataReader.GetInt32(0),
              CategoryID = SqliteDataReader.GetInt32(1),
              Name2 = SqliteDataReader.GetString(2),
              Hash = SqliteDataReader.GetString(3),
              Size = SqliteDataReader.GetInt64(4),
              Seeders = SqliteDataReader.GetInt32(5),
              Status = SqliteDataReader.GetInt32(6),
              IsKeep = SqliteDataReader.GetBoolean(9),
              IsKeeper = SqliteDataReader.GetBoolean(10),
              IsBlackList = SqliteDataReader.GetBoolean(11),
              IsDownload = SqliteDataReader.GetBoolean(12),
              AvgSeeders = SqliteDataReader.IsDBNull(13) ? new Decimal?() : new Decimal?(SqliteDataReader.GetDecimal(13)),
              RegTime = SqliteDataReader.IsDBNull(14) ? dateTime : SqliteDataReader.GetDateTime(14),
              PosterID = SqliteDataReader.IsDBNull(15) ? 0 : SqliteDataReader.GetInt32(15)
            });
        }
      }
      return topicInfoList;
    }

    public List<TopicInfo> GetTopicsAllByCategory(int categoyid)
    {
      DateTime dateTime = new DateTime(2000, 1, 1);
      List<TopicInfo> topicInfoList = new List<TopicInfo>();
      using (SqliteCommand command = this._conn.CreateCommand())
      {
        command.CommandText = "SELECT TopicID, CategoryID, Name, Hash, Size, Seeders, Status, IsActive, IsDeleted, IsKeep, IsKeepers, IsBlackList, IsDownload, AvgSeeders, RegTime, PosterID\r\nFROM Topic WHERE (CategoryID = @CategoryID OR @CategoryID = -1) and Hash is null";
        command.Parameters.AddWithValue("@CategoryID", (object) categoyid);
        using (SqliteDataReader SqliteDataReader = command.ExecuteReader())
        {
          while (SqliteDataReader.Read())
            topicInfoList.Add(new TopicInfo()
            {
              TopicID = SqliteDataReader.GetInt32(0),
              CategoryID = SqliteDataReader.GetInt32(1),
              Name2 = SqliteDataReader.GetString(2),
              Hash = SqliteDataReader.GetString(3),
              Size = SqliteDataReader.GetInt64(4),
              Seeders = SqliteDataReader.GetInt32(5),
              Status = SqliteDataReader.GetInt32(6),
              IsKeep = SqliteDataReader.GetBoolean(9),
              IsKeeper = SqliteDataReader.GetBoolean(10),
              IsBlackList = SqliteDataReader.GetBoolean(11),
              IsDownload = SqliteDataReader.GetBoolean(12),
              AvgSeeders = SqliteDataReader.IsDBNull(13) ? new Decimal?() : new Decimal?(SqliteDataReader.GetDecimal(13)),
              RegTime = SqliteDataReader.IsDBNull(14) ? dateTime : SqliteDataReader.GetDateTime(14),
              PosterID = SqliteDataReader.IsDBNull(15) ? 0 : SqliteDataReader.GetInt32(15)
            });
        }
      }
      return topicInfoList;
    }

    public List<TopicInfo> GetTopics(DateTime regTime, int categoyid, int? countSeeders, int? avgCountSeeders, bool? isKeep, bool? isKeepers, bool? isDownload, bool? isBlack, bool? isPoster)
    {
      DateTime dateTime = new DateTime(2000, 1, 1);
      List<TopicInfo> topicInfoList = new List<TopicInfo>();
      using (SqliteCommand command = this._conn.CreateCommand())
      {
        command.CommandText = @"
SELECT DISTINCT t.TopicID, t.CategoryID, t.Name, Hash, Size, Seeders, Status, IsActive, IsDeleted, IsKeep, IsKeepers, IsBlackList, IsDownload, AvgSeeders, RegTime, CAST(CASE WHEN @UserName = u.Name THEN 1 ELSE 0 END AS BIT), 
COUNT(kt.TopicID) AS KeepersCount
FROM Topic AS t    
LEFT JOIN User AS u ON (t.PosterID = u.UserID)
LEFT JOIN KeeperToTopic AS kt ON (kt.TopicID = t.TopicID AND kt.KeeperName <> @UserName)
WHERE 
    t.CategoryID = @CategoryID 
    AND t.RegTime < @RegTime
    AND Status NOT IN (7,4,11,5)
" + (
    countSeeders.HasValue
      ? string.Format(" AND Seeders {1} {0}", (object) countSeeders.Value,
        Settings.Current.IsSelectLessOrEqual ? (object) " <= " : (object) " = ")
      : ""
  )
  + (avgCountSeeders.HasValue
    ? string.Format(" AND AvgSeeders {1} {0}", (object) avgCountSeeders.Value,
      Settings.Current.IsSelectLessOrEqual ? (object) " <= " : (object) " = ")
    : "")
  + (isKeep.HasValue ? string.Format(" AND IsKeep = {0}", (object) (isKeep.Value ? 1 : 0)) : "")
  + (isKeepers.HasValue
    ? string.Format(" AND CAST(CASE WHEN kt.TopicID IS NOT NULL THEN 1 ELSE 0 END AS BIT) = {0}",
      (object) (isKeepers.Value ? 1 : 0))
    : "")
  + (isDownload.HasValue ? string.Format(" AND IsDownload = {0}", (object) (isDownload.Value ? 1 : 0)) : "")
  + (isPoster.HasValue ? string.Format(" AND @UserName = u.Name", (object) (isPoster.Value ? 1 : 0)) : "")
  + string.Format(" AND IsBlackList = {0}", (object) (!isBlack.HasValue || !isBlack.Value ? 0 : 1))
  + " AND IsDeleted = 0 GROUP BY t.TopicID HAVING t.TopicID IS NOT NULL ORDER BY t.Seeders, t.Name";
        command.Parameters.AddWithValue("@CategoryID", (object) categoyid);
        command.Parameters.AddWithValue("@RegTime", (object) regTime);
        command.Parameters.AddWithValue("@UserName", string.IsNullOrWhiteSpace(Settings.Current.KeeperName) ? (object) "-" : (object) Settings.Current.KeeperName);
        using (SqliteDataReader SqliteDataReader = command.ExecuteReader())
        {
          while (SqliteDataReader.Read())
            topicInfoList.Add(new TopicInfo()
            {
              TopicID = SqliteDataReader.GetInt32(0),
              CategoryID = SqliteDataReader.GetInt32(1),
              Name2 = SqliteDataReader.GetString(2),
              Hash = SqliteDataReader.IsDBNull(3) ? string.Empty : SqliteDataReader.GetString(3),
              Size = SqliteDataReader.GetInt64(4),
              Seeders = SqliteDataReader.GetInt32(5),
              Status = SqliteDataReader.GetInt32(6),
              IsKeep = SqliteDataReader.GetBoolean(9),
              KeeperCount = SqliteDataReader.GetInt32(16),
              IsBlackList = SqliteDataReader.GetBoolean(11),
              IsDownload = SqliteDataReader.GetBoolean(12),
              AvgSeeders = SqliteDataReader.IsDBNull(13) ? new Decimal?() : new Decimal?(Math.Round(SqliteDataReader.GetDecimal(13), 3)),
              RegTime = SqliteDataReader.IsDBNull(14) ? dateTime : SqliteDataReader.GetDateTime(14),
              IsPoster = SqliteDataReader.GetBoolean(15)
            });
        }
      }
      return topicInfoList;
    }

    public void SetTorrentClientHash(List<TopicInfo> data)
    {
      using (SqliteTransaction sqLiteTransaction = this._conn.BeginTransaction())
      {
        using (SqliteCommand command = this._conn.CreateCommand())
        {
          command.Transaction = sqLiteTransaction;
          command.CommandText = "UPDATE Topic SET IsDownload = @IsDownload, IsKeep = @IsKeep WHERE Hash = @Hash;";
          foreach (TopicInfo topicInfo in data)
          {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@Hash", (object) topicInfo.Hash);
            command.Parameters.AddWithValue("@IsDownload", (object) topicInfo.IsDownload);
            command.Parameters.AddWithValue("@IsKeep", (object) topicInfo.IsKeep);
            command.ExecuteNonQuery();
          }
        }
        sqLiteTransaction.Commit();
      }
    }

    public void SaveTorrentClients(IEnumerable<TorrentClientInfo> data, bool isUpdateList = false)
    {
      List<TorrentClientInfo> tc = this.GetTorrentClients();
      using (SqliteTransaction sqLiteTransaction = this._conn.BeginTransaction())
      {
        using (SqliteCommand command = this._conn.CreateCommand())
        {
          command.Transaction = sqLiteTransaction;
          if (isUpdateList)
          {
            command.CommandText = "DELETE FROM TorrentClient WHERE UID = @UID";
            List<TorrentClientInfo> source = tc;
            foreach (TorrentClientInfo torrentClientInfo in source.Where<TorrentClientInfo>((Func<TorrentClientInfo, bool>) (x => !data.Select<TorrentClientInfo, Guid>((Func<TorrentClientInfo, Guid>) (y => y.UID)).Contains<Guid>(x.UID))))
            {
              command.Parameters.Clear();
              command.Parameters.AddWithValue("@UID", (object) torrentClientInfo.UID.ToString());
              command.ExecuteNonQuery();
            }
          }
          command.CommandText = "UPDATE TorrentClient SET Name = @Name, Type = @Type, ServerName = @ServerName, ServerPort = @ServerPort, UserName = @UserName, UserPassword = @UserPassword, LastReadHash = @LastReadHash WHERE UID = @UID";
          IEnumerable<TorrentClientInfo> source1 = data;
          foreach (TorrentClientInfo torrentClientInfo in source1.Where<TorrentClientInfo>((Func<TorrentClientInfo, bool>) (x => tc.Select<TorrentClientInfo, Guid>((Func<TorrentClientInfo, Guid>) (y => y.UID)).Contains<Guid>(x.UID))))
          {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@UID", (object) torrentClientInfo.UID.ToString());
            command.Parameters.AddWithValue("@Name", (object) torrentClientInfo.Name);
            command.Parameters.AddWithValue("@Type", (object) torrentClientInfo.Type);
            command.Parameters.AddWithValue("@ServerName", (object) torrentClientInfo.ServerName);
            command.Parameters.AddWithValue("@ServerPort", (object) torrentClientInfo.ServerPort);
            command.Parameters.AddWithValue("@UserName", (object) torrentClientInfo.UserName);
            command.Parameters.AddWithValue("@UserPassword", (object) torrentClientInfo.UserPassword);
            command.Parameters.AddWithValue("@LastReadHash", (object) torrentClientInfo.LastReadHash);
            command.ExecuteNonQuery();
          }
          command.CommandText = "INSERT INTO TorrentClient (UID, Name, Type, ServerName, ServerPort, UserName, UserPassword, LastReadHash) VALUES(@UID, @Name, @Type, @ServerName, @ServerPort, @UserName, @UserPassword, @LastReadHash)";
          IEnumerable<TorrentClientInfo> source2 = data;
          foreach (TorrentClientInfo torrentClientInfo in source2.Where<TorrentClientInfo>((Func<TorrentClientInfo, bool>) (x => !tc.Select<TorrentClientInfo, Guid>((Func<TorrentClientInfo, Guid>) (y => y.UID)).Contains<Guid>(x.UID))))
          {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@UID", (object) torrentClientInfo.UID.ToString());
            command.Parameters.AddWithValue("@Name", (object) torrentClientInfo.Name);
            command.Parameters.AddWithValue("@Type", (object) torrentClientInfo.Type);
            command.Parameters.AddWithValue("@ServerName", (object) torrentClientInfo.ServerName);
            command.Parameters.AddWithValue("@ServerPort", (object) torrentClientInfo.ServerPort);
            command.Parameters.AddWithValue("@UserName", (object) torrentClientInfo.UserName);
            command.Parameters.AddWithValue("@UserPassword", (object) torrentClientInfo.UserPassword);
            command.Parameters.AddWithValue("@LastReadHash", (object) torrentClientInfo.LastReadHash);
            command.ExecuteNonQuery();
          }
        }
        sqLiteTransaction.Commit();
      }
    }

    public List<TorrentClientInfo> GetTorrentClients()
    {
      List<TorrentClientInfo> torrentClientInfoList = new List<TorrentClientInfo>();
      using (SqliteCommand command = this._conn.CreateCommand())
      {
        command.CommandText = "SELECT * FROM TorrentClient";
        using (SqliteDataReader SqliteDataReader = command.ExecuteReader())
        {
          while (SqliteDataReader.Read())
            torrentClientInfoList.Add(new TorrentClientInfo()
            {
              UID = Guid.Parse(SqliteDataReader.GetString(0)),
              Name = SqliteDataReader.GetString(1),
              Type = SqliteDataReader.GetString(2),
              ServerName = SqliteDataReader.GetString(3),
              ServerPort = SqliteDataReader.GetInt32(4),
              UserName = SqliteDataReader.GetString(5),
              UserPassword = SqliteDataReader.GetString(6),
              LastReadHash = SqliteDataReader.GetDateTime(7)
            });
        }
      }
      return torrentClientInfoList;
    }

    public void SaveKeepOtherKeepers(Dictionary<string, Tuple<int, List<int>>> data)
    {
      if (data == null)
        return;
      using (SqliteTransaction sqLiteTransaction = this._conn.BeginTransaction())
      {
        using (SqliteCommand command = this._conn.CreateCommand())
        {
          command.Transaction = sqLiteTransaction;
          command.CommandTimeout = 60000;
          foreach (KeyValuePair<string, Tuple<int, List<int>>> keyValuePair in data)
          {
            KeyValuePair<string, Tuple<int, List<int>>> dt = keyValuePair;
            int[] array = dt.Value.Item2.Distinct<int>().ToArray<int>();
            List<int>[] intListArray = new List<int>[array.Length % 500 == 0 ? array.Length / 500 : array.Length / 500 + 1];
            for (int index1 = 0; index1 < array.Length; ++index1)
            {
              int index2 = index1 / 500;
              if (intListArray[index2] == null)
                intListArray[index2] = new List<int>();
              intListArray[index2].Add(array[index1]);
            }
            foreach (List<int> source in intListArray)
            {
              command.Parameters.Clear();
              command.CommandText = "INSERT OR REPLACE INTO KeeperToTopic(KeeperName, CategoryID, TopicID)\r\n" + string.Join("UNION ", source.Select<int, string>((Func<int, string>) (x => string.Format("SELECT @KeeperName, {2}, {1}\r\n", (object) dt.Key, (object) x, (object) dt.Value.Item1))));
              command.Parameters.AddWithValue("@KeeperName", (object) dt.Key.Replace("<wbr>", "").Trim());
              command.ExecuteNonQuery();
            }
          }
        }
        sqLiteTransaction.Commit();
      }
    }

    private void SaveKeepStatus(string keepName, List<Tuple<int, int, Decimal>> data)
    {
      using (SqliteTransaction sqLiteTransaction = this._conn.BeginTransaction())
      {
        using (SqliteCommand command = this._conn.CreateCommand())
        {
          command.Transaction = sqLiteTransaction;
          command.CommandText = "INSERT OR REPLACE INTO Keeper VALUES(@KeeperName, @CategoryID, @Count, @Size)";
          command.Parameters.Add("@KeeperName", DbType.String);
          command.Parameters.Add("@CategoryID", DbType.Int32);
          command.Parameters.Add("@Count", DbType.Int64);
          command.Parameters.Add("@Size", DbType.Decimal);
          if (string.IsNullOrWhiteSpace(keepName))
            keepName = Settings.Current.KeeperName;
          if (string.IsNullOrWhiteSpace(keepName))
            return;
          foreach (Tuple<int, int, Decimal> tuple in data)
          {
            command.Parameters[0].Value = (object) keepName.Replace("<wbr>", "").Trim();
            command.Parameters[1].Value = (object) tuple.Item1;
            command.Parameters[2].Value = (object) tuple.Item2;
            command.Parameters[3].Value = (object) Math.Round(tuple.Item3, 2);
            command.ExecuteNonQuery();
          }
        }
        sqLiteTransaction.Commit();
      }
    }

    public void ClearReports()
    {
      using (SqliteTransaction sqLiteTransaction = this._conn.BeginTransaction())
      {
        using (SqliteCommand command = this._conn.CreateCommand())
        {
          command.Transaction = sqLiteTransaction;
          command.CommandText = "UPDATE Report SET Report = @Report WHERE ReportNo <> 0";
          command.Parameters.AddWithValue("@Report", (object) "Удалено");
          command.ExecuteNonQuery();
        }
        sqLiteTransaction.Commit();
      }
    }

    public List<Tuple<int, string, int, Decimal>> GetStatisticsByAllUsers()
    {
      List<Tuple<int, string, int, Decimal>> tupleList = new List<Tuple<int, string, int, Decimal>>();
      bool flag = this.GetTorrentClients().Any<TorrentClientInfo>();
      using (SqliteCommand command = this._conn.CreateCommand())
      {
        command.CommandText = "INSERT OR REPLACE INTO Keeper SELECT 'All', CategoryID, COUNT(*) Cnt, SUM(Size) / 1073741824.0 Size FROM Topic WHERE IsDeleted = 0 AND CategoryID <> 0 GROUP BY CategoryID;\r\nINSERT OR REPLACE INTO Keeper SELECT kt.KeeperName, kt.CategoryID, COUNT(*),  CAST(SUM(t.Size) / 1073741824.0  AS NUMERIC(18,4)) Size \r\n    FROM KeeperToTopic AS kt JOIN Topic AS t ON (kt.TopicID = t.TopicID AND kt.KeeperName <> @KeeperName) group by kt.KeeperName, kt.CategoryID;\r\nINSERT OR REPLACE INTO Keeper SELECT @KeeperName, CategoryID,  COUNT(*) Cnt, CAST(SUM(Size) / 1073741824.0 AS NUMERIC(18,4)) Size FROM Topic \r\n        WHERE IsDeleted = 0 AND IsKeep = 1 AND (Seeders <= @Seeders OR @Seeders = -1) AND Status NOT IN (7, 4,11,5) AND IsBlackList = 0 GROUP BY CategoryID;\r\n";
        command.Parameters.AddWithValue("@KeeperName", flag ? (object) Settings.Current.KeeperName : (object) "<no>");
        command.Parameters.AddWithValue("@Seeders", (object) Settings.Current.CountSeedersReport);
        command.ExecuteNonQuery();
        command.CommandText = "SELECT KeeperName, CategoryID, Count, Size FROM Keeper";
        using (SqliteDataReader SqliteDataReader = command.ExecuteReader())
        {
          while (SqliteDataReader.Read())
            tupleList.Add(new Tuple<int, string, int, Decimal>(SqliteDataReader.GetInt32(1), SqliteDataReader.GetString(0), SqliteDataReader.GetInt32(2), Math.Round(SqliteDataReader.GetDecimal(3), 3)));
        }
      }
      return tupleList;
    }

    public void SaveReports(Dictionary<int, Dictionary<int, string>> reports)
    {
      foreach (KeyValuePair<int, Dictionary<int, string>> report in reports)
      {
        int num = report.Value.Keys.Max<int>((Func<int, int>) (x => x));
        report.Value.Add(num + 1, "Резерв");
        report.Value.Add(num + 2, "Резерв");
      }
      using (SqliteTransaction sqLiteTransaction = this._conn.BeginTransaction())
      {
        using (SqliteCommand command = this._conn.CreateCommand())
        {
          command.Transaction = sqLiteTransaction;
          Dictionary<Tuple<int, int>, Tuple<string, string>> reps = this.GetReports(new int?());
          if (reports.Any<KeyValuePair<int, Dictionary<int, string>>>((Func<KeyValuePair<int, Dictionary<int, string>>, bool>) (x => !x.Value.ContainsKey(0))))
          {
            command.CommandText = "UPDATE Report SET Report = @Report WHERE CategoryID = @CategoryID AND ReportNo <> 0";
            foreach (KeyValuePair<int, Dictionary<int, string>> report in reports)
            {
              command.Parameters.Clear();
              command.Parameters.AddWithValue("@CategoryID", (object) report.Key);
              command.Parameters.AddWithValue("@Report", (object) "Резерв");
              command.ExecuteNonQuery();
            }
          }
          command.CommandText = "UPDATE Report SET Report = @Report WHERE CategoryID = @CategoryID AND ReportNo = @ReportNo";
          foreach (KeyValuePair<int, Dictionary<int, string>> report in reports)
          {
            KeyValuePair<int, Dictionary<int, string>> r1 = report;
            Dictionary<int, string> source = r1.Value;
            foreach (KeyValuePair<int, string> keyValuePair in source.Where<KeyValuePair<int, string>>((Func<KeyValuePair<int, string>, bool>) (x => reps.ContainsKey(new Tuple<int, int>(r1.Key, x.Key)))))
            {
              command.Parameters.Clear();
              command.Parameters.AddWithValue("@CategoryID", (object) r1.Key);
              command.Parameters.AddWithValue("@ReportNo", (object) keyValuePair.Key);
              command.Parameters.AddWithValue("@Report", (object) keyValuePair.Value);
              command.ExecuteNonQuery();
            }
          }
          command.CommandText = "INSERT OR REPLACE INTO Report VALUES(@CategoryID, @ReportNo, @URL, @Report)";
          foreach (KeyValuePair<int, Dictionary<int, string>> report in reports)
          {
            KeyValuePair<int, Dictionary<int, string>> r1 = report;
            Dictionary<int, string> source = r1.Value;
            foreach (KeyValuePair<int, string> keyValuePair in source.Where<KeyValuePair<int, string>>((Func<KeyValuePair<int, string>, bool>) (x => !reps.ContainsKey(new Tuple<int, int>(r1.Key, x.Key)))))
            {
              command.Parameters.Clear();
              command.Parameters.AddWithValue("@CategoryID", (object) r1.Key);
              command.Parameters.AddWithValue("@ReportNo", (object) keyValuePair.Key);
              command.Parameters.AddWithValue("@URL", (object) string.Empty);
              command.Parameters.AddWithValue("@Report", (object) keyValuePair.Value);
              command.ExecuteNonQuery();
            }
          }
        }
        sqLiteTransaction.Commit();
      }
    }

    public Dictionary<Tuple<int, int>, Tuple<string, string>> GetReports(int? categoryID = null)
    {
      Dictionary<Tuple<int, int>, Tuple<string, string>> dictionary = new Dictionary<Tuple<int, int>, Tuple<string, string>>();
      using (SqliteCommand command = this._conn.CreateCommand())
      {
        command.CommandText = "SELECT * FROM Report";
        if (categoryID.HasValue)
        {
          command.CommandText += " WHERE CategoryID = @CategoryID";
          command.Parameters.AddWithValue("@CategoryID", (object) categoryID.Value);
        }
        using (SqliteDataReader SqliteDataReader = command.ExecuteReader())
        {
          while (SqliteDataReader.Read())
          {
            if (!dictionary.ContainsKey(new Tuple<int, int>(SqliteDataReader.GetInt32(0), SqliteDataReader.GetInt32(1))))
              dictionary.Add(new Tuple<int, int>(SqliteDataReader.GetInt32(0), SqliteDataReader.GetInt32(1)), new Tuple<string, string>(SqliteDataReader.GetString(2), SqliteDataReader.GetString(3)));
          }
        }
      }
      return dictionary;
    }

    public void SaveSettingsReport(List<Tuple<int, int, string>> result)
    {
      using (SqliteTransaction sqLiteTransaction = this._conn.BeginTransaction())
      {
        using (SqliteCommand command = this._conn.CreateCommand())
        {
          command.Transaction = sqLiteTransaction;
          command.CommandText = "SELECT DISTINCT CategoryID FROM Report WHERE ReportNo = 0";
          HashSet<int> filter = new HashSet<int>();
          using (SqliteDataReader SqliteDataReader = command.ExecuteReader())
          {
            while (SqliteDataReader.Read())
              filter.Add(SqliteDataReader.GetInt32(0));
          }
          command.CommandText = "UPDATE Report SET URL = @url WHERE CategoryID = @CategoryID AND ReportNo = @ReportNo";
          command.Parameters.Add("@CategoryID", DbType.Int32);
          command.Parameters.Add("@ReportNo", DbType.Int32);
          command.Parameters.Add("@url", DbType.String);
          command.Prepare();
          foreach (Tuple<int, int, string> tuple in result)
          {
            command.Parameters["@CategoryID"].Value = (object) tuple.Item1;
            command.Parameters["@ReportNo"].Value = (object) tuple.Item2;
            command.Parameters["@url"].Value = (object) tuple.Item3;
            command.ExecuteNonQuery();
          }
          command.CommandText = "INSERT OR REPLACE INTO Report VALUES(@CategoryID, @ReportNo, @URL, '')";
          command.Prepare();
          List<Tuple<int, int, string>> source = result;
          foreach (Tuple<int, int, string> tuple in source.Where<Tuple<int, int, string>>((Func<Tuple<int, int, string>, bool>) (x => !filter.Contains(x.Item1))))
          {
            command.Parameters["@CategoryID"].Value = (object) tuple.Item1;
            command.Parameters["@ReportNo"].Value = (object) tuple.Item2;
            command.Parameters["@url"].Value = (object) tuple.Item3;
            command.ExecuteNonQuery();
          }
        }
        sqLiteTransaction.Commit();
      }
    }

    public void UpdateStatistics()
    {
      using (SqliteTransaction sqLiteTransaction = this._conn.BeginTransaction())
      {
        using (SqliteCommand command = this._conn.CreateCommand())
        {
          command.Transaction = sqLiteTransaction;
          List<Decimal[]> numArrayList = new List<Decimal[]>();
          command.CommandText = "update Topic SET AvgSeeders = (SELECT AVG(Seeders) FROM TopicStatusHystory AS st WHERE st.TopicID = Topic.TopicID)";
          command.ExecuteNonQuery();
          command.CommandText = "UPDATE Topic SET AvgSeeders = @Seeders WHERE TopicID = @TopicID";
          foreach (Decimal[] numArray in numArrayList)
          {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@TopicID", (object) numArray[0]);
            command.Parameters.AddWithValue("@Seeders", (object) numArray[1]);
            command.ExecuteNonQuery();
          }
        }
        sqLiteTransaction.Commit();
      }
    }

    public void ClearKeepers()
    {
      using (SqliteCommand command = this._conn.CreateCommand())
      {
        command.CommandText = "DELETE FROM Keeper;\r\nDELETE FROM KeeperToTopic;\r\nUPDATE Report SET Report = '' WHERE ReportNo = 0";
        command.ExecuteNonQuery();
      }
    }

    public void CreateReportByRootCategories()
    {
      try
      {
        using (SqliteCommand command = this._conn.CreateCommand())
        {
          this.GetStatisticsByAllUsers();
          Dictionary<int, Dictionary<int, string>> reports = new Dictionary<int, Dictionary<int, string>>();
          Dictionary<int, Tuple<string, Decimal, Decimal>> source1 = new Dictionary<int, Tuple<string, Decimal, Decimal>>();
          Dictionary<Tuple<int, string>, Tuple<string, Decimal, Decimal>> dictionary1 = new Dictionary<Tuple<int, string>, Tuple<string, Decimal, Decimal>>();
          Dictionary<Tuple<int, string, int>, Tuple<string, Decimal, Decimal>> dictionary2 = new Dictionary<Tuple<int, string, int>, Tuple<string, Decimal, Decimal>>();
          List<Tuple<int, int, string, Decimal, Decimal>> tupleList = new List<Tuple<int, int, string, Decimal, Decimal>>();
          command.CommandText = "\r\nSELECT c.CategoryID, c.FullName, SUM(Count)Count, SUM(Size)Size\r\nFROM\r\n    (\r\n       SELECT CategoryID, ParentID FROM Category WHERE ParentID > 1000000 UNION\r\n       SELECT c1.CategoryID, c2.ParentID FROM Category AS c1 JOIN Category AS c2 ON (c1.ParentID = c2.CategoryID) WHERE c2.ParentID > 1000000       \r\n    ) AS t    \r\n    JOIN Category AS c ON (t.ParentID = c.CategoryID)    \r\n    JOIN Keeper AS k ON (k.CategoryID = t.CategoryID AND k.KeeperName <> 'All')\r\nGROUP BY\r\n      c.CategoryID, c.FullName\r\nORDER BY c.FullName";
          using (SqliteDataReader SqliteDataReader = command.ExecuteReader())
          {
            while (SqliteDataReader.Read())
              source1.Add(SqliteDataReader.GetInt32(0), new Tuple<string, Decimal, Decimal>(SqliteDataReader.GetString(1), SqliteDataReader.GetDecimal(2), SqliteDataReader.GetDecimal(3)));
          }
          command.CommandText = "\r\nSELECT c.CategoryID, c.FullName, k.KeeperName, SUM(Count)Count, SUM(Size)Size\r\nFROM\r\n    (\r\n       SELECT CategoryID, ParentID FROM Category WHERE ParentID > 1000000 UNION\r\n       SELECT c1.CategoryID, c2.ParentID FROM Category AS c1 JOIN Category AS c2 ON (c1.ParentID = c2.CategoryID) WHERE c2.ParentID > 1000000       \r\n    ) AS t    \r\n    JOIN Category AS c ON (t.ParentID = c.CategoryID)    \r\n    JOIN Keeper AS k ON (k.CategoryID = t.CategoryID AND k.KeeperName <> 'All')\r\nGROUP BY\r\n      c.CategoryID, c.FullName, k.KeeperName\r\nORDER BY c.FullName, k.KeeperName";
          using (SqliteDataReader SqliteDataReader = command.ExecuteReader())
          {
            while (SqliteDataReader.Read())
              dictionary1.Add(new Tuple<int, string>(SqliteDataReader.GetInt32(0), SqliteDataReader.GetString(2)), new Tuple<string, Decimal, Decimal>(SqliteDataReader.GetString(1), SqliteDataReader.GetDecimal(3), SqliteDataReader.GetDecimal(4)));
          }
          command.CommandText = "\r\nSELECT t.ParentID, c.CategoryID, c.FullName, k.KeeperName, SUM(Count)Count, SUM(Size)Size\r\nFROM\r\n    (\r\n       SELECT CategoryID, ParentID FROM Category WHERE ParentID > 1000000 UNION\r\n       SELECT c1.CategoryID, c2.ParentID FROM Category AS c1 JOIN Category AS c2 ON (c1.ParentID = c2.CategoryID) WHERE c2.ParentID > 1000000       \r\n    ) AS t    \r\n    JOIN Category AS c ON (t.CategoryID = c.CategoryID)    \r\n    JOIN Keeper AS k ON (k.CategoryID = t.CategoryID AND k.KeeperName <> 'All')\r\nGROUP BY\r\n      t.ParentID, c.FullName, k.KeeperName, c.CategoryID\r\nORDER BY c.FullName, k.KeeperName";
          using (SqliteDataReader SqliteDataReader = command.ExecuteReader())
          {
            while (SqliteDataReader.Read())
              dictionary2.Add(new Tuple<int, string, int>(SqliteDataReader.GetInt32(0), SqliteDataReader.GetString(3), SqliteDataReader.GetInt32(1)), new Tuple<string, Decimal, Decimal>(SqliteDataReader.GetString(2), SqliteDataReader.GetDecimal(4), SqliteDataReader.GetDecimal(5)));
          }
          command.CommandText = "\r\nSELECT t.ParentID, c.CategoryID, c.FullName,SUM(Count)Count, SUM(Size)Size\r\nFROM\r\n    (\r\n       SELECT CategoryID, ParentID FROM Category WHERE ParentID > 1000000 UNION\r\n       SELECT c1.CategoryID, c2.ParentID FROM Category AS c1 JOIN Category AS c2 ON (c1.ParentID = c2.CategoryID) WHERE c2.ParentID > 1000000       \r\n    ) AS t    \r\n    JOIN Category AS c ON (t.CategoryID = c.CategoryID)    \r\n    JOIN Keeper AS k ON (k.CategoryID = t.CategoryID AND k.KeeperName <> 'All')\r\nGROUP BY\r\n      c.CategoryID, c.FullName\r\nORDER BY c.FullName";
          using (SqliteDataReader SqliteDataReader = command.ExecuteReader())
          {
            while (SqliteDataReader.Read())
              tupleList.Add(new Tuple<int, int, string, Decimal, Decimal>(SqliteDataReader.GetInt32(0), SqliteDataReader.GetInt32(1), SqliteDataReader.GetString(2), SqliteDataReader.GetDecimal(3), SqliteDataReader.GetDecimal(4)));
          }
          foreach (int num1 in source1.Select<KeyValuePair<int, Tuple<string, Decimal, Decimal>>, int>((Func<KeyValuePair<int, Tuple<string, Decimal, Decimal>>, int>) (x => x.Key)))
          {
            int c = num1;
            StringBuilder stringBuilder1 = new StringBuilder();
            StringBuilder stringBuilder2 = stringBuilder1;
            string format = "[hr]\r\n[hr]\r\n[b][color=darkgreen][align=center][size=16]Статистика раздела: {0}[/size][/align][/color][/b][hr]\r\n[hr]\r\n\r\n";
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.Date;
            string str = dateTime.ToString("dd.MM.yyyy");
            stringBuilder2.AppendFormat(format, (object) str);
            stringBuilder1.AppendFormat("Всего: {0} шт. ({1:0.00} Гб.)\r\n\r\n", (object) source1[c].Item2, (object) source1[c].Item3);
            stringBuilder1.AppendLine("[hr]");
            stringBuilder1.AppendLine("[size=12][b]По хранителям:[/b][/size]");
            int num2 = 1;
            Dictionary<Tuple<int, string>, Tuple<string, Decimal, Decimal>> source2 = dictionary1;
            foreach (KeyValuePair<Tuple<int, string>, Tuple<string, Decimal, Decimal>> keyValuePair1 in source2.Where<KeyValuePair<Tuple<int, string>, Tuple<string, Decimal, Decimal>>>((Func<KeyValuePair<Tuple<int, string>, Tuple<string, Decimal, Decimal>>, bool>) (x => x.Key.Item1 == c)))
            {
              KeyValuePair<Tuple<int, string>, Tuple<string, Decimal, Decimal>> k = keyValuePair1;
              stringBuilder1.AppendFormat("[spoiler=\"{0}. {1} - {2} шт. ({3:0.00} Гб.)\"]\r\n", (object) num2, (object) k.Key.Item2, (object) k.Value.Item2, (object) k.Value.Item3);
              Dictionary<Tuple<int, string, int>, Tuple<string, Decimal, Decimal>> source3 = dictionary2;
              foreach (KeyValuePair<Tuple<int, string, int>, Tuple<string, Decimal, Decimal>> keyValuePair2 in source3.Where<KeyValuePair<Tuple<int, string, int>, Tuple<string, Decimal, Decimal>>>((Func<KeyValuePair<Tuple<int, string, int>, Tuple<string, Decimal, Decimal>>, bool>) (x =>
              {
                if (x.Key.Item2 == k.Key.Item2)
                  return x.Key.Item1 == c;
                return false;
              })))
                stringBuilder1.AppendFormat("{0} - {1} шт. ({2:0.00} Гб.)\r\n", (object) keyValuePair2.Value.Item1, (object) keyValuePair2.Value.Item2, (object) keyValuePair2.Value.Item3);
              stringBuilder1.AppendLine("[/spoiler]");
              ++num2;
            }
            stringBuilder1.AppendLine("[hr]");
            stringBuilder1.AppendLine("[size=12][b]По форумам:[/b][/size]");
            List<Tuple<int, int, string, Decimal, Decimal>> source4 = tupleList;
            foreach (Tuple<int, int, string, Decimal, Decimal> tuple in (IEnumerable<Tuple<int, int, string, Decimal, Decimal>>) source4.Where<Tuple<int, int, string, Decimal, Decimal>>((Func<Tuple<int, int, string, Decimal, Decimal>, bool>) (x => x.Item1 == c)).OrderBy<Tuple<int, int, string, Decimal, Decimal>, string>((Func<Tuple<int, int, string, Decimal, Decimal>, string>) (x => x.Item3)))
            {
              Tuple<int, int, string, Decimal, Decimal> k = tuple;
              stringBuilder1.AppendFormat("[spoiler=\"{0} - {1} шт. ({2:0.00} Гб.)\"]\r\n", (object) k.Item3, (object) k.Item4, (object) k.Item5);
              Dictionary<Tuple<int, string, int>, Tuple<string, Decimal, Decimal>> source3 = dictionary2;
              foreach (KeyValuePair<Tuple<int, string, int>, Tuple<string, Decimal, Decimal>> keyValuePair in (IEnumerable<KeyValuePair<Tuple<int, string, int>, Tuple<string, Decimal, Decimal>>>) source3.Where<KeyValuePair<Tuple<int, string, int>, Tuple<string, Decimal, Decimal>>>((Func<KeyValuePair<Tuple<int, string, int>, Tuple<string, Decimal, Decimal>>, bool>) (x => x.Key.Item3 == k.Item2)).OrderBy<KeyValuePair<Tuple<int, string, int>, Tuple<string, Decimal, Decimal>>, string>((Func<KeyValuePair<Tuple<int, string, int>, Tuple<string, Decimal, Decimal>>, string>) (x => x.Key.Item2)))
                stringBuilder1.AppendFormat("{0} - {1} шт. ({2:0.00} Гб.)\r\n", (object) keyValuePair.Key.Item2, (object) keyValuePair.Value.Item2, (object) keyValuePair.Value.Item3);
              stringBuilder1.AppendLine("[/spoiler]");
            }
            reports.Add(c, new Dictionary<int, string>());
            reports[c].Add(0, stringBuilder1.ToString().Replace("<wbr>", "").Trim());
          }
          this.SaveReports(reports);
        }
      }
      catch (Exception ex)
      {
      }
    }
  }
}
