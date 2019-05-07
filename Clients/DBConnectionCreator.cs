using Mono.Data.Sqlite;
using NLog;
using TLO.local.Forms;

namespace TLO.local
{
    public class DBConnectionCreator
    {
        public SqliteConnection Connection { get; }

        private static Logger _logger { get; set; }

        string FileDatabase => System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Database.db");

        public DBConnectionCreator()
        {
            if (_logger == null)
                _logger = LogManager.GetLogger("ClientServer");
            
            var db = $"Data Source={FileDatabase};Version=3;";
            if (InMemory())
            {
                SqliteConnection tmpConnection = new SqliteConnection(db);
                tmpConnection.Open();
                Connection = new SqliteConnection("Data Source=:memory:;Version=3;");
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
