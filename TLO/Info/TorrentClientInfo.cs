using System;
using System.Collections.Generic;
using TLO.Clients;

namespace TLO.Info
{
    internal class TorrentClientInfo
    {
        public TorrentClientInfo()
        {
            UID = Guid.NewGuid();
            Name = string.Empty;
            Type = UTorrentClient.ClientId;
            ServerName = string.Empty;
            ServerPort = 8080;
            UserName = string.Empty;
            UserPassword = string.Empty;
            LastReadHash = new DateTime(2000, 1, 1);
        }

        public string Id { get; }

        public Guid UID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string ServerName { get; set; }

        public int ServerPort { get; set; }

        public string UserName { get; set; }

        public string UserPassword { get; set; }

        public DateTime LastReadHash { get; set; }

        public override string ToString()
        {
            return Name;
        }

        private static Dictionary<String, ITorrentClient> _clients = new Dictionary<string, ITorrentClient>();

        public ITorrentClient Create()
        {
            if (_clients.ContainsKey(Type))
            {
                return _clients[Type];
            }

            ITorrentClient client = _create();
            _clients[Type] = client;

            return client;
        }

        private ITorrentClient _create()
        {
            return Type switch
            {
                UTorrentClient.ClientId => new UTorrentClient(ServerName, ServerPort, UserName, UserPassword),
                TransmissionClient.ClientId => new TransmissionClient(ServerName, ServerPort, UserName, UserPassword),
                QBitTorrentClient.ClientId => new QBitTorrentClient(ServerName, ServerPort, UserName, UserPassword),
                _ => throw new NotSupportedException()
            };
        }
    }
}