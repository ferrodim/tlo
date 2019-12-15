﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using TLO.Info;

namespace TLO.Clients
{
    internal class UTorrentClient : ITorrentClient
    {
        public const string ClientId = "uTorrent";
        public string Id => ClientId;
        private static Logger _logger;
        private readonly string _serverName;
        private readonly int _serverPort;
        private readonly TloWebClient _webClient;
        private string _token;

        public UTorrentClient(string serverName, int port, string userName, string userPass)
        {
            if (_logger == null)
                _logger = LogManager.GetLogger("uTorrentClient");
            _webClient = new TloWebClient();
            _webClient.Encoding = Encoding.UTF8;
            var svcCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(userName + ":" + userPass));
            _webClient.Headers.Add("Authorization", "Basic " + svcCredentials);
            _serverName = serverName;
            _serverPort = port;
            try
            {
                Ping();
            }
            catch
            {
                _logger.Debug(string.Format("Имя сервера: {0}; Порт сервера: {1}", _serverName, _serverPort));
                throw;
            }
        }

        public bool Ping()
        {
            var array = _webClient.GetString(string.Format("http://{0}:{1}/gui/", _serverName, _serverPort)).Split(
                new string[1]
                {
                    "div"
                }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.Contains("token")).ToArray();
            if (array.Length != 0)
                _token = array[0].Split(new char[2]
                {
                    '>',
                    '<'
                }, StringSplitOptions.RemoveEmptyEntries)[1];
            else
                _token = null;
            return true;
        }

        public List<TopicInfo> GetAllTorrentHash()
        {
            var source = new List<string[]>();
            if (!string.IsNullOrWhiteSpace(_token))
                source.Add(new string[2] {"token", _token});
            source.Add(new string[2] {"list", "1"});
            var objArray =
                JsonConvert.DeserializeObject<JObject>(_webClient.GetJson(string.Format("http://{0}:{1}/gui/?{2}",
                        _serverName, _serverPort, string.Join("&", source.Select(x => string.Join("=", x))))))[
                        "torrents"]
                    .ToObject<object[][]>();
            if (objArray == null)
                return new List<TopicInfo>();
            return objArray.Select(x => new
            {
                Hash = (x[0] as string).ToUpper(),
                Name = x[2] as string,
                Size = x[3].GetType() == typeof(int)
                    ? (long) (int) x[3]
                    : x[3].GetType() == typeof(long)
                        ? (long) x[3]
                        : 0L,
                Status = IntToArrayBool((long) x[1]),
                PercentComplite = (decimal) (long) x[4] * new decimal(1, 0, 0, false, 1),
                Label = x[11] as string
            }).Select(x => new TopicInfo
            {
                Hash = x.Hash,
                TorrentName = x.Name,
                Size = x.Size,
                IsKeep = x.Status != null && x.PercentComplite == new decimal(100) && x.Status[3] && !x.Status[4] &&
                         x.Status[7],
                IsDownload = true,
                IsPause = x.Status[5],
                IsRun = !(x.PercentComplite == new decimal(100)) || !x.Status[3] || x.Status[4] || !x.Status[7]
                    ? new bool?()
                    : x.Status[0],
                Label = x.Label
            }).ToList();
        }

        public IEnumerable<string> GetFiles(TopicInfo topic)
        {
            if (topic != null && !string.IsNullOrEmpty(topic.Hash))
            {
                var source = new List<string[]>();
                if (!string.IsNullOrWhiteSpace(_token))
                    source.Add(new string[2] {"token", _token});
                source.Add(new string[2] {"action", "getfiles"});
                source.Add(new string[2] {"hash", topic.Hash});
                var jtoken1 = JsonConvert.DeserializeObject<JObject>(_webClient.GetJson(
                    string.Format("http://{0}:{1}/gui/?{2}", _serverName, _serverPort,
                        string.Join("&", source.Select(x => string.Join("=", x))))))["files"];
                if (jtoken1 != null)
                    foreach (var jtoken2 in (IEnumerable<JToken>) jtoken1.ToObject<JArray>()[1].ToObject<JArray>())
                        yield return jtoken2.ToObject<object[]>()[0].ToString();
            }
        }

        public void DistributionStop(IEnumerable<string> data)
        {
            var source = new List<string[]>();
            if (!string.IsNullOrWhiteSpace(_token))
                source.Add(new string[2] {"token", _token});
            source.Add(new string[2] {"action", "stop"});
            source.AddRange(data.Select(x => new string[2]
            {
                "hash",
                x
            }));
            _webClient.GetJson(string.Format("http://{0}:{1}/gui/?{2}", _serverName, _serverPort,
                string.Join("&", source.Select(x => string.Join("=", x)))));
        }

        public void DistributionPause(IEnumerable<string> data)
        {
            var source = new List<string[]>();
            if (!string.IsNullOrWhiteSpace(_token))
                source.Add(new string[2] {"token", _token});
            source.Add(new string[2] {"action", "pause"});
            source.AddRange(data.Select(x => new string[2]
            {
                "hash",
                x
            }));
            _webClient.GetJson(string.Format("http://{0}:{1}/gui/?{2}", _serverName, _serverPort,
                string.Join("&", source.Select(x => string.Join("=", x)))));
        }

        public void DistributionStart(IEnumerable<string> data)
        {
            var source = new List<string[]>();
            if (!string.IsNullOrWhiteSpace(_token))
                source.Add(new string[2] {"token", _token});
            source.Add(new string[2] {"action", "start"});
            source.AddRange(data.Select(x => new string[2]
            {
                "hash",
                x
            }));
            _webClient.GetJson(string.Format("http://{0}:{1}/gui/?{2}", _serverName, _serverPort,
                string.Join("&", source.Select(x => string.Join("=", x)))));
        }

        public bool SetDefaultFolder(string dir)
        {
            try
            {
                var source = new List<string[]>();
                if (!string.IsNullOrWhiteSpace(_token))
                    source.Add(new string[2] {"token", _token});
                source.Add(new string[2] {"action", "setsetting"});
                source.Add(new string[2]
                {
                    "s",
                    "dir_active_download"
                });
                source.Add(new string[2]
                {
                    "v",
                    HttpUtility.UrlEncode(dir)
                });
                _webClient.GetJson(string.Format("http://{0}:{1}/gui/?{2}", _serverName, _serverPort,
                    string.Join("&", source.Select(x => string.Join("=", x)))));
                Thread.Sleep(100);
                return GetDefaultFolder() == dir;
            }
            catch
            {
                return false;
            }
        }

        public string GetDefaultFolder()
        {
            try
            {
                var source = new List<string[]>();
                if (!string.IsNullOrWhiteSpace(_token))
                    source.Add(new string[2] {"token", _token});
                source.Add(new string[2] {"action", "getsettings"});
                var objArray1 =
                    JsonConvert.DeserializeObject<JObject>(_webClient.GetJson(string.Format("http://{0}:{1}/gui/?{2}",
                            _serverName, _serverPort, string.Join("&", source.Select(x => string.Join("=", x))))))
                        ["settings"].ToObject<object[][]>();
                if (objArray1 == null)
                    return string.Empty;
                var objArray2 = objArray1.Where(x => x[0] as string == "dir_active_download").FirstOrDefault();
                if (objArray2 == null)
                    return string.Empty;
                return objArray2[2] as string;
            }
            catch
            {
                return null;
            }
        }

        public bool SetDefaultLabel(string label)
        {
            try
            {
                var source = new List<string[]>();
                if (!string.IsNullOrWhiteSpace(_token))
                    source.Add(new string[2] {"token", _token});
                source.Add(new string[2] {"action", "setsetting"});
                source.Add(new string[2] {"s", "dir_add_label"});
                source.Add(new string[2]
                {
                    "v",
                    HttpUtility.UrlEncode(label)
                });
                _webClient.GetJson(string.Format("http://{0}:{1}/gui/?{2}", _serverName, _serverPort,
                    string.Join("&", source.Select(x => string.Join("=", x)))));
                Thread.Sleep(200);
                return GetDefaultLabel() == label;
            }
            catch
            {
                return false;
            }
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
            var str = "----WebKitFormBoundary1vZaMilolI9TchBt";
            using (var memoryStream = new MemoryStream())
            {
                var bytes1 = Encoding.ASCII.GetBytes(string.Format(
                    "--{0}\r\nContent-Disposition: form-data; name=\"torrent_file\"; filename=\"{1}\"\r\nContent-Type: application/x-bittorrent\r\n\r\n",
                    str, filename));
                memoryStream.Write(bytes1, 0, bytes1.Length);
                memoryStream.Write(fdata, 0, fdata.Length);
                var bytes2 = Encoding.ASCII.GetBytes(string.Format("\r\n--{0}--\r\n", str));
                memoryStream.Write(bytes2, 0, bytes2.Length);
                var source = new List<string[]>();
                if (!string.IsNullOrWhiteSpace(_token))
                    source.Add(new string[2] {"token", _token});
                source.Add(new string[2] {"action", "add-file"});
                var httpWebRequest = (HttpWebRequest) WebRequest.Create(
                    string.Format("http://{0}:{1}/gui/?{2}", _serverName, _serverPort,
                        string.Join("&", source.Select(x => string.Join("=", x)))));
                httpWebRequest.Method = "POST";
                httpWebRequest.KeepAlive = true;
                httpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                httpWebRequest.ContentType = "multipart/form-data; boundary=" + str;
                httpWebRequest.Headers.Add("Authorization", _webClient.Headers.Get("Authorization"));
                httpWebRequest.Headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");
                var array = memoryStream.ToArray();
                httpWebRequest.ContentLength = array.Length;
                using (var requestStream = httpWebRequest.GetRequestStream())
                {
                    requestStream.Write(array, 0, array.Length);
                }
            }
        }

        public string[] GetTrackers(string hash)
        {
            return null;
        }

        public bool SetTrackers(string hash, string[] trackers)
        {
            try
            {
                var str = string.Join("\r\n\r\n", trackers) + "\r\n";
                var source = new List<string[]>();
                if (!string.IsNullOrWhiteSpace(_token))
                    source.Add(new string[2] {"token", _token});
                source.Add(new string[2] {"setprops", "setprops"});
                source.Add(new string[2] {"hash", hash});
                source.Add(new string[2] {"s", "trackers"});
                source.Add(new string[2]
                {
                    "v",
                    HttpUtility.UrlEncode(str)
                });
                _webClient.GetJson(string.Format("http://{0}:{1}/gui/?{2}", _serverName, _serverPort,
                    string.Join("&", source.Select(x => string.Join("=", x)))));
                Thread.Sleep(100);
                var trackers1 = GetTrackers(hash);
                if (trackers1 == null)
                    return false;
                return string.Join("\r\n\r\n", trackers1) + "\r\n" == str;
            }
            catch
            {
                return false;
            }
        }

        public bool SetLabel(string hash, string label)
        {
            Thread.Sleep(100);
            var source = new List<string[]>();
            if (!string.IsNullOrWhiteSpace(_token))
                source.Add(new string[2] {"token", _token});
            source.Add(new string[2] {"action", "setprops"});
            source.Add(new string[2] {"hash", hash});
            source.Add(new string[2] {"s", "label"});
            source.Add(new string[2] {"v", label});
            _webClient.GetJson(string.Format("http://{0}:{1}/gui/?{2}", _serverName, _serverPort,
                string.Join("&", source.Select(x => string.Join("=", x)))));
            Thread.Sleep(100);
            return true;
        }

        public bool SetLabel(IEnumerable<string> hashs, string label)
        {
            if (hashs == null || hashs.Count() == 0)
                return true;
            var source = new List<string[]>();
            foreach (var str in hashs)
            {
                if (source.Count() == 0)
                {
                    if (!string.IsNullOrWhiteSpace(_token))
                        source.Add(new string[2] {"token", _token});
                    source.Add(new string[2] {"action", "setprops"});
                }

                source.Add(new string[2] {"s", "label"});
                source.Add(new string[2] {"hash", str});
                source.Add(new string[2] {"v", label});
                if (source.Count() > 150)
                {
                    _webClient.GetJson(string.Format("http://{0}:{1}/gui/?{2}", _serverName, _serverPort,
                        string.Join("&", source.Select(x => string.Join("=", x)))));
                    source.Clear();
                }

                Thread.Sleep(100);
            }

            if (source.Count() != 0)
                _webClient.GetJson(string.Format("http://{0}:{1}/gui/?{2}", _serverName, _serverPort,
                    string.Join("&", source.Select(x => string.Join("=", x)))));
            return true;
        }

        private bool[] IntToArrayBool(long value)
        {
            var bitArray = new BitArray(new int[1]
            {
                (int) value
            });
            var flagArray = new bool[bitArray.Count];
            bitArray.CopyTo(flagArray, 0);
            return flagArray;
        }

        public string GetDefaultLabel()
        {
            try
            {
                var source = new List<string[]>();
                if (!string.IsNullOrWhiteSpace(_token))
                    source.Add(new string[2] {"token", _token});
                source.Add(new string[2] {"action", "getsettings"});
                var objArray1 =
                    JsonConvert.DeserializeObject<JObject>(_webClient.GetJson(string.Format("http://{0}:{1}/gui/?{2}",
                            _serverName, _serverPort, string.Join("&", source.Select(x => string.Join("=", x))))))
                        ["settings"].ToObject<object[][]>();
                if (objArray1 == null)
                    return string.Empty;
                var objArray2 = objArray1.Where(x => x[0] as string == "dir_add_label").FirstOrDefault();
                if (objArray2 == null)
                    return string.Empty;
                return objArray2[2] as string;
            }
            catch
            {
                return null;
            }
        }

        private class Tt
        {
            public int Build { get; set; }

            public List<object> Files { get; set; }
        }
    }
}