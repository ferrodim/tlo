﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using NLog;
using TLO.Info;

namespace TLO.Clients
{
    internal class TransmissionClient : ITorrentClient
    {
        public const string ClientId = "Transmission";
        public string Id => ClientId;
        private static Logger _logger;
        private readonly string _url;
        private readonly TloWebClient _webClient;

        public TransmissionClient(string serverName, int port, string userName, string userPass)
        {
            if (_logger == null)
                _logger = LogManager.GetLogger("TransmissionClient");
            _webClient = new TloWebClient(Encoding.UTF8,
                "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:36.0) Gecko/20100101 Firefox/36.0",
                "application/json, text/javascript, */*; q=0.01", true);
            _webClient.Encoding = Encoding.UTF8;
            var svcCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(userName + ":" + userPass));
            _webClient.Headers.Add("Authorization", "Basic " + svcCredentials);
            _url = string.Format("http://{0}:{1}/transmission/rpc", serverName, port);
            try
            {
                Ping();
            }
            catch
            {
                _logger.Debug(string.Format("Имя сервера: {0}; Порт сервера: {1}", serverName, port));
                throw;
            }
        }

        public List<TopicInfo> GetAllTorrentHash()
        {
            try
            {
                var querty = JsonConvert.DeserializeObject<Querty>(Encoding.UTF8.GetString(
                    _webClient.UploadData(_url, "POST",
                        Encoding.UTF8.GetBytes(
                            "{\"method\":\"torrent-get\",\"arguments\":{\"fields\":[\"hashString\", \"totalSize\", \"percentDone\", \"error\", \"status\"]}}"))));
                if (querty == null || querty.Arguments == null || querty.Arguments.Torrents == null ||
                    querty.Arguments.Torrents.Length == 0)
                    return new List<TopicInfo>();
                return querty.Arguments.Torrents.Select(t => new TopicInfo
                {
                    Hash = t.HashString.ToUpper(),
                    IsKeep = t.PercentDone == decimal.One && t.Error == 0,
                    IsDownload = true,
                    IsRun = !(t.PercentDone == decimal.One) || t.Error != 0 ? new bool?() : t.Status == 6
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Debug(ex.StackTrace);
                throw ex;
            }
        }

        public IEnumerable<string> GetFiles(TopicInfo topic)
        {
            yield break;
        }

        public void DistributionStop(IEnumerable<string> data)
        {
            var querty = JsonConvert.DeserializeObject<Querty>(Encoding.UTF8.GetString(_webClient.UploadData(_url,
                "POST",
                Encoding.UTF8.GetBytes(
                    "{\"method\":\"torrent-get\",\"arguments\":{\"fields\":[\"hashString\", \"id\"]}}"))));
            if (querty == null || querty.Arguments == null || querty.Arguments.Torrents == null ||
                querty.Arguments.Torrents.Length == 0)
                return;
            var array = querty.Arguments.Torrents.Join(data, t => t.HashString.ToUpper(), d => d, (t, d) => t.Id)
                .ToArray();
            if (array.Length == 0)
                return;
            _webClient.UploadData(_url, "POST", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new
            {
                method = "torrent-stop",
                arguments = new {ids = array}
            })));
        }

        public void DistributionPause(IEnumerable<string> data)
        {
            var querty = JsonConvert.DeserializeObject<Querty>(Encoding.UTF8.GetString(_webClient.UploadData(_url,
                "POST",
                Encoding.UTF8.GetBytes(
                    "{\"method\":\"torrent-get\",\"arguments\":{\"fields\":[\"hashString\", \"id\"]}}"))));
            if (querty == null || querty.Arguments == null || querty.Arguments.Torrents == null ||
                querty.Arguments.Torrents.Length == 0)
                return;
            var array = querty.Arguments.Torrents.Join(data, t => t.HashString.ToUpper(), d => d, (t, d) => t.Id)
                .ToArray();
            if (array.Length == 0)
                return;
            _webClient.UploadData(_url, "POST", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new
            {
                method = "torrent-stop",
                arguments = new {ids = array}
            })));
        }

        public void DistributionStart(IEnumerable<string> data)
        {
            var querty = JsonConvert.DeserializeObject<Querty>(Encoding.UTF8.GetString(_webClient.UploadData(_url,
                "POST",
                Encoding.UTF8.GetBytes(
                    "{\"method\":\"torrent-get\",\"arguments\":{\"fields\":[\"hashString\", \"id\"]}}"))));
            if (querty == null || querty.Arguments == null || querty.Arguments.Torrents == null ||
                querty.Arguments.Torrents.Length == 0)
                return;
            var array = querty.Arguments.Torrents.Join(data, t => t.HashString.ToUpper(), d => d, (t, d) => t.Id)
                .ToArray();
            if (array.Length == 0)
                return;
            _webClient.UploadData(_url, "POST", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new
            {
                method = "torrent-start",
                arguments = new {ids = array}
            })));
        }

        public bool Ping()
        {
            while (true)
                try
                {
                    _webClient.UploadData(_url, "POST", Encoding.UTF8.GetBytes("{\"method\":\"session-get\"}"));
                    return true;
                }
                catch (WebException ex)
                {
                    _webClient.Headers.Add("X-Transmission-Session-Id",
                        ex.Response.Headers["X-Transmission-Session-Id"]);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    _logger.Debug(ex.StackTrace);
                    throw ex;
                }
        }

        public bool SetDefaultFolder(string dir)
        {
            return true;
        }

        public bool SetDefaultLabel(string label)
        {
            return true;
        }

        public string GetDefaultFolder()
        {
            return string.Empty;
        }

        public void SendTorrentFile(string path, string file)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var fileStream = File.OpenRead(file))
                {
                    fileStream.CopyTo(memoryStream);
                    SendTorrentFile(path, Path.GetFileName(file), memoryStream.ToArray());
                }
            }
        }

        public void SendTorrentFile(string path, string filename, byte[] fdata)
        {
            var base64String = Convert.ToBase64String(fdata);
            _webClient.UploadData(_url, "POST", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new
            {
                method = "torrent-add",
                arguments = new
                {
                    paused = false,
                    downloadDir = path,
                    metainfo = base64String
                }
            }).Replace("downloadDir", "download-dir")));
        }

        public string[] GetTrackers(string hash)
        {
            return null;
        }

        public bool SetTrackers(string hash, string[] trackers)
        {
            return true;
        }

        public bool SetLabel(string hash, string label)
        {
            return true;
        }

        public bool SetLabel(IEnumerable<string> hash, string label)
        {
            return true;
        }

        private class Querty
        {
            public Argument Arguments { get; set; }
        }

        private class Argument
        {
            public Torrent[] Torrents { get; set; }
        }

        private class Torrent
        {
            public int Id { get; set; }

            public string HashString { get; set; }

            public long TotalSize { get; set; }

            public decimal PercentDone { get; set; }

            public int Error { get; set; }

            public int Status { get; set; }
        }
    }
}