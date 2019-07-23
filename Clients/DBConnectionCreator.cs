using System.IO;
using System.Reflection;
using Mono.Data.Sqlite;
using NLog;

namespace TLO.local
{
    public class DBConnectionCreator
    {
        public SqliteConnection Connection { get; }

        private static Logger _logger { get; set; }

        string FileDatabase => Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Database.db");

        public DBConnectionCreator()
        {
            if (_logger == null)
                _logger = LogManager.GetLogger("ClientServer");
            
            var db = $"URI=file:{FileDatabase},version=3;";
            if (InMemory())
            {
                SqliteConnection tmpConnection = new SqliteConnection(db);
                tmpConnection.Open();
                Connection = new SqliteConnection("URI=file::memory:,version=3");
                Connection.Open();
                _logger.Info("Загрузка базы в память...");
//                tmpConnection.BackupDatabase(Connection, "main", "main", -1, null, -1);
                tmpConnection.Close();
                _logger.Info("Загрузка базы в память завершена.");
            }
            else
            {
                _logger.Info("Подключение к файлу бд...");
                Connection = new SqliteConnection(db);
                Connection.Open();
            }
        }

        public void Close()
        {
            Connection.Close();
        }

        public static bool InMemory()
        {
            return false;
            return Settings.Current.LoadDBInMemory.GetValueOrDefault(true);
        }
    }
}