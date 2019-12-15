using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using TLO.Info;

namespace TLO.Clients
{
    internal class QBitTorrentClient : ITorrentClient
    {
        public const string ClientId = "QBitTorrent";
        public string Id => ClientId;
        private readonly TloWebClient _client;
        private readonly Uri _baseUri;
        private readonly UriTemplate _uriTemplate;
        private readonly string _host;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        private readonly Logger Logger = LogManager.GetLogger("QBitTorrentClient");

        public QBitTorrentClient(string host, int port, string username, string password)
        {
            _baseUri = new Uri($"http://{host}:{port}/");
            _uriTemplate = new UriTemplate("api/v2/{section}/{methodName}");
            _client = new TloWebClient(Encoding.UTF8, "TLO", "text/*,application/*", true);
            _host = host;
            _port = port;
            _username = username;
            _password = password;
            _client.Headers.Add("Referer", _baseUri.ToString());
            _client.UseDefaultCredentials = true;

            authorize();
        }

        private void authorize()
        {
            try
            {
                var auth = _uriTemplate.BindByName(_baseUri, new Dictionary<string, string>
                {
                    {"section", "auth"}, {"methodName", "login"}
                });

                var builder = new UriBuilder(auth);
                builder.Query = $"username={_username}&password={_password}";
                var result = _client.DownloadString(builder.Uri);
                Logger.Log(LogLevel.Debug, "Auth: " + result);
            }
            catch (WebException e)
            {
                Logger.Error(e);
                Logger.Error(e.StackTrace);
            }
        }

        public List<TopicInfo> GetAllTorrentHash()
        {
            var uri = _uriTemplate.BindByName(_baseUri, new Dictionary<string, string>
            {
                {"section", "torrents"}, {"methodName", "info"}
            });
            string result;
            try
            {
                result = _client.DownloadString(uri);
            }
            catch (WebException e)
            {
                Logger.Error(e);
                Logger.Error(e.StackTrace);

                return new List<TopicInfo>();
            }

            try
            {
                var torrents = JsonConvert.DeserializeObject<List<JObject>>(result);
                return torrents.ConvertAll(input =>
                    {
                        var info = new TopicInfo();
                        info.Hash = input.GetValue("hash").ToString().ToUpper();
                        info.Name2 = input.GetValue("name").ToString();
                        info.TorrentName = input.GetValue("name").ToString();
                        info.Size = (long) input.GetValue("size").ToObject(typeof(long));
                        info.Seeders = (int) input.GetValue("num_complete").ToObject(typeof(int));
                        info.Leechers = (int) input.GetValue("num_incomplete").ToObject(typeof(int));
                        info.Label = (string) input.GetValue("tags").ToObject(typeof(string));
                        info.IsRun = input.GetValue("state").ToString() == "stalledUP";
                        info.IsKeep = input.GetValue("state").ToString().Contains("UP");
                        info.IsPause = input.GetValue("state").ToString() == "pausedUP" ||
                                       input.GetValue("state").ToString() == "pausedDL";
                        info.IsDownload = input.GetValue("state").ToString() == "pausedUP";
                        return info;
                    }
                );
            }
            catch (Exception e)
            {
                Logger.Error(e);
                Logger.Error(e.StackTrace);
                return new List<TopicInfo>();
            }
        }

        public IEnumerable<string> GetFiles(TopicInfo topic)
        {
            return new string[0]; // TODO
            throw new NotImplementedException();
        }

        public void DistributionStop(IEnumerable<string> data)
        {
            return; // TODO
            throw new NotImplementedException();
        }

        public void DistributionPause(IEnumerable<string> data)
        {
            return; // TODO
            throw new NotImplementedException();
        }

        public void DistributionStart(IEnumerable<string> data)
        {
            return; // TODO
            throw new NotImplementedException();
        }

        public bool Ping()
        {
            try
            {
                var uri = _uriTemplate.BindByName(_baseUri, new Dictionary<string, string>
                {
                    {"section", "app"}, {"methodName", "version"}
                });
                Logger.Debug("Version: " + _client.DownloadString(uri));

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SetDefaultFolder(string dir)
        {
            return true; // TODO
            throw new NotImplementedException();
        }

        public bool SetDefaultLabel(string label)
        {
            return true; // TODO
            throw new NotImplementedException();
        }

        public string GetDefaultFolder()
        {
            return ""; // TODO
            throw new NotImplementedException();
        }

        public void SendTorrentFile(string path, string file)
        {
            return; // TODO
            throw new NotImplementedException();
        }

        public void SendTorrentFile(string path, string filename, byte[] fdata)
        {
            var uri = _uriTemplate.BindByName(_baseUri, new Dictionary<string, string>
            {
                {"section", "torrents"}, {"methodName", "add"}
            });

            var str = "----WebKitFormBoundary1vZaMilolI9TchBt";
            using (var memoryStream = new MemoryStream())
            {
                var bytes0 = Encoding.UTF8.GetBytes(
                    $"--{str}\r\nContent-Disposition: form-data; name=\"savepath\"\r\n\r\n{path}");
                var bytes1 = Encoding.UTF8.GetBytes(
                    $"--{str}\r\nContent-Disposition: form-data; name=\"torrent_file\"; filename=\"{filename}\"\r\nContent-Type: application/x-bittorrent\r\n\r\n");
                memoryStream.Write(bytes0, 0, bytes0.Length);
                memoryStream.Write(bytes1, 0, bytes1.Length);
                memoryStream.Write(fdata, 0, fdata.Length);
                var bytes2 = Encoding.UTF8.GetBytes($"\r\n--{str}--\r\n");
                memoryStream.Write(bytes2, 0, bytes2.Length);
                var httpWebRequest = (HttpWebRequest) WebRequest.Create(uri);
                httpWebRequest.Method = "POST";
                httpWebRequest.KeepAlive = true;
                httpWebRequest.ContentType = "multipart/form-data; boundary=" + str;
                httpWebRequest.CookieContainer = _client.CookieContainer;
                var array = memoryStream.ToArray();
                httpWebRequest.ContentLength = array.Length;
                using (var requestStream = httpWebRequest.GetRequestStream())
                {
                    requestStream.Write(array, 0, array.Length);
                    requestStream.Flush();
                    requestStream.Close();
                }

                var response = httpWebRequest.GetResponse();
                var responseStream = response.GetResponseStream();
                var reader = new StreamReader(responseStream);
                reader.ReadToEnd();
                reader.Close();
            }
        }

        public string[] GetTrackers(string hash)
        {
            return new string[0]; // TODO
            throw new NotImplementedException();
        }

        public bool SetTrackers(string hash, string[] trackers)
        {
            return true; // TODO
            throw new NotImplementedException();
        }

        public bool SetLabel(string hash, string label)
        {
            try
            {
                var uri = _uriTemplate.BindByName(_baseUri, new Dictionary<string, string>
                {
                    {"section", "torrents"}, {"methodName", "addTags"}
                });
                var data = $"hashes={hash.ToLower()}&tags={WebUtility.UrlEncode(label.Replace(",", "‚"))}";
                var bytes = Encoding.UTF8.GetBytes(data);

                var httpWebRequest = (HttpWebRequest) WebRequest.Create(uri);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.CookieContainer = _client.CookieContainer;
                httpWebRequest.ContentLength = bytes.Length;
                var stream = httpWebRequest.GetRequestStream();
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
                stream.Close();

                var response = httpWebRequest.GetResponse();
                var responseStream = response.GetResponseStream();
                var reader = new StreamReader(responseStream);
                reader.ReadToEnd();
                reader.Close();
                return true;
            }
            catch (WebException e)
            {
                Logger.Error(e.Message);
                Logger.Error(e.StackTrace);
                return false;
            }
        }

        public bool SetLabel(IEnumerable<string> hash, string label)
        {
            try
            {
                var uri = _uriTemplate.BindByName(_baseUri, new Dictionary<string, string>
                {
                    {"section", "torrents"}, {"methodName", "addTags"}
                });
                var data =
                    $"hashes={string.Join("|", hash.ToList().ConvertAll((input => input.ToLower())))}&tags={WebUtility.UrlEncode(label.Replace(",", "‚"))}";
                var bytes = Encoding.UTF8.GetBytes(data);

                var httpWebRequest = (HttpWebRequest) WebRequest.Create(uri);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.CookieContainer = _client.CookieContainer;
                httpWebRequest.ContentLength = bytes.Length;
                var stream = httpWebRequest.GetRequestStream();
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
                stream.Close();

                var response = httpWebRequest.GetResponse();
                var responseStream = response.GetResponseStream();
                var reader = new StreamReader(responseStream);
                reader.ReadToEnd();
                reader.Close();

                return true;
            }
            catch (WebException e)
            {
                Logger.Error(e.Message);
                Logger.Error(e.StackTrace);
                return false;
            }
        }
    }
}