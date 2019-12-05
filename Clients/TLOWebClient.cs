using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using MihaZupan;
using NLog;

namespace TLO.Clients
{
    internal class TloWebClient : WebClient
    {
        private static Logger _logger;
        private readonly string _accept = string.Empty;
        private readonly string _userAgent;
        private bool _isJson;
        private readonly bool _enableProxy;

        public TloWebClient(bool enableProxy = false)
            : this(null, null, null, enableProxy: enableProxy)
        {
        }

        public TloWebClient(Encoding encoding)
            : this(encoding, null, null, enableProxy: true)
        {
        }

        public TloWebClient(Encoding encoding, string userAgent, string accept, bool isJson = false,
            bool enableProxy = false)
        {
            if (_logger == null)
                _logger = LogManager.GetCurrentClassLogger();
            Encoding encoding1;
            if (encoding != null)
                encoding1 = encoding;
            else
                encoding = encoding1 = Encoding.UTF8;
            Encoding = encoding1;
            _userAgent = string.IsNullOrWhiteSpace(userAgent)
                ? "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:35.0) Gecko/20100101 Firefox/35.0"
                : userAgent;
            _accept = string.IsNullOrWhiteSpace(accept)
                ? "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
                : accept;
            CookieContainer = new CookieContainer();
            _isJson = isJson;
            _enableProxy = enableProxy;
        }

        public TloWebClient(string userAgent)
        {
            _userAgent = userAgent;
        }

        public CookieContainer CookieContainer { get; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var webRequest = (HttpWebRequest) base.GetWebRequest(address);
            if (webRequest != null && Settings.Current.UseProxy == true && _enableProxy)
            {
                if (Settings.Current.SystemProxy == true)
                {
                    webRequest.Proxy = WebRequest.GetSystemWebProxy();
                }
                else
                {
                    var proxy = Settings.Current.SelectedProxy;
                    if (proxy.Contains("http://"))
                    {
                        webRequest.Proxy = new WebProxy(proxy);
                    }
                    else
                    {
                        var uri = new Uri(proxy);
                        webRequest.Proxy = new HttpToSocks5Proxy(uri.Host, uri.Port);
                    }
                }
            }

            webRequest.Accept = _isJson ? "application/json" : _accept;
            webRequest.UserAgent = _userAgent;
            webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            webRequest.Headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");
            if (_isJson)
            {
                webRequest.Headers.Add("X-Request", "JSON");
                webRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            }

            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.KeepAlive = true;
            webRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            webRequest.Headers.Add("Pragma", "no-cache");
            webRequest.Timeout = 60000;
//            if (address.Host == "dl.rutracker.org" && address.AbsoluteUri.Contains("="))
//            {
//                var strArray = address.AbsoluteUri.Split(new char[1]
//                {
//                    '='
//                }, StringSplitOptions.RemoveEmptyEntries);
//                CookieContainer.Add(address, new Cookie("bb_dl", strArray[1]));
//                webRequest.Referer = string.Format("https://{1}/forum/viewtopic.php?t={0}", strArray[1],
//                    Settings.Current.HostRuTrackerOrg);
//            }

            webRequest.CookieContainer = CookieContainer;
            if (Settings.Current.DisableServerCertVerify.GetValueOrDefault(false))
                webRequest.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

            return webRequest;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response;
            try
            {
                response = base.GetWebResponse(request);
            }
            catch (Exception e)
            {
                if (!(e.InnerException is WebException))
                {
                    throw;
                }

                logResponse((HttpWebResponse) request.GetResponse());

                throw;
            }

            logResponse((HttpWebResponse) response);

            return response;
        }


        private static void logResponse(HttpWebResponse response)
        {
            var webResponse = response;
            var responseStream = webResponse.GetResponseStream();
            var headersText = "";
            var items = Enumerable
                .Range(0, webResponse.Headers.Count)
                .SelectMany(i => webResponse.Headers.GetValues(i)
                    .Select(v => Tuple.Create(webResponse.Headers.GetKey(i), v))
                );
            foreach (var header in items)
            {
                headersText += $"{header.Item1}: {header.Item2}\r\n";
            }

            if (responseStream != null)
            {
                Stream streamReplace = new MemoryStream();
                responseStream.CopyTo(streamReplace);
                streamReplace.Seek(0, SeekOrigin.Begin);

                var length = streamReplace.Length;
                var buffer = new byte[length];
                var read = 0;
                do
                {
                    var readed = streamReplace.Read(buffer, 0, (int) length);
                    read += readed;
                } while (read < length);

                var text = Encoding.GetEncoding(1251).GetString(buffer);

                streamReplace.Seek(0, SeekOrigin.Begin);

                var fieldInfo = webResponse
                    .GetType()
                    .GetField(
                        "m_ConnectStream",
                        BindingFlags.Instance | BindingFlags.NonPublic
                    );
                if (fieldInfo != null) fieldInfo.SetValue(webResponse, streamReplace);
                var httpWebResponse = webResponse;
                _logger.Trace(
                    $"\r\nHTTP/{httpWebResponse.ProtocolVersion} {httpWebResponse.StatusCode} {httpWebResponse.StatusDescription}\r\n" +
                    headersText +
                    "\r\n\r\n" +
                    text);
            }
        }

        public string GetString(string url)
        {
            _isJson = false;
            return DownloadString(url);
        }

        public string GetJson(string url)
        {
            _isJson = true;
            return DownloadString(url);
        }
    }
}