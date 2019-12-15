using Mono.Data.Sqlite;
using NLog;

namespace TLO.Clients
{
    public class DbConnectionCreator
    {
        private DbConnectionCreator()
        {
            if (Logger == null)
                Logger = LogManager.GetLogger("SQLite");

            var db = $"Data Source={ClientLocalDb.FileDatabase};Version=3;";
            if (InMemory())
            {
                _internalConnection = new SqliteConnection(db);
                _internalConnection.Open();
                Connection = new SqliteConnection("Data Source=:memory:;Version=3;");
                Connection.Open();
                Logger.Info("Загрузка базы в память...");
                // _internalConnection.BackupDatabase(Connection, "main", "main", -1, null, -1);
                Logger.Info("Загрузка базы в память завершена.");
            }
            else
            {
                Logger.Info("Подключение к файлу бд...");
                _internalConnection = new SqliteConnection(db);
                _internalConnection.Open();
                Connection = _internalConnection;
            }
        }

        public void Persist()
        {
            if (_inMemory)
            {
                Logger.Info("Сохранение базы в файл из памяти...");
                var command = Connection.CreateCommand();
                command.CommandText = "VACUUM \"main\";";
                command.ExecuteNonQuery();
                // Connection.BackupDatabase(_internalConnection, "main", "main", -1, null, -1);
                Logger.Info("Сохранение завершено.");
            }
        }

        public bool Reconnect()
        {
            bool configChanged = _inMemory != InMemory();
            if (configChanged)
            {
                if (_inMemory)
                {
                    Persist();
                    // Почистим инстанс, если конфигурация изменилась.
                    _internalConnection.Close();
                    Connection.Close();
                    _instance = null;

                    return true;
                }
                else
                {
                    _internalConnection.Close();
                    // Почистим инстанс, так как конфигурация изменилась.
                    _instance = null;
                    return true;
                }
            }

            return false;
        }

        private static DbConnectionCreator _instance;
        public static DbConnectionCreator Instance => _instance ??= new DbConnectionCreator();

        private readonly bool _inMemory = InMemory();
        private readonly SqliteConnection _internalConnection;
        public SqliteConnection Connection { get; }

        private static Logger? Logger { get; set; }

        public void Close()
        {
            Connection.Close();
        }

        private static bool InMemory()
        {
            return false; // Settings.Current.LoadDBInMemory.GetValueOrDefault(true);
        }
    }
}