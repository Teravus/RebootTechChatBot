using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using HttpServer;
using System.Threading;

namespace RebootTechBotLib.Infrastructure
{
    public class OSHttpResponse
    {
        protected IHttpResponse _httpResponse;
        private IHttpClientContext _httpClientContext;

        public Encoding ContentEncoding
        {
            get
            {
                return _httpResponse.Encoding;
            }
            set
            {
                _httpResponse.Encoding = value;
            }
        }

        public string ContentType
        {
            get
            {
                return _httpResponse.ContentType;
            }
            set
            {
                _httpResponse.ContentType = value;
            }
        }

        public bool KeepAlive
        {
            get
            {
                return _httpResponse.Connection == ConnectionType.KeepAlive;
            }
            set
            {
                if (value)
                    _httpResponse.Connection = ConnectionType.KeepAlive;
                else
                    _httpResponse.Connection = ConnectionType.Close;
            }
        }

        public int KeepAliveTimeout
        {
            get
            {
                return _httpResponse.KeepAlive;
            }
            set
            {
                if (value == 0)
                {
                    _httpResponse.Connection = ConnectionType.Close;
                    _httpResponse.KeepAlive = 0;
                }
                else
                {
                    _httpResponse.Connection = ConnectionType.KeepAlive;
                    _httpResponse.KeepAlive = value;
                }
            }

        }

        public Stream OutputStream
        {
            get
            {
                return _httpResponse.Body;
            }
        }
        public string ProtocolVersion
        {
            get
            {
                return _httpResponse.ProtocolVersion;
            }
            set
            {
                _httpResponse.ProtocolVersion = value;
            }
        }

        public Stream Body
        {
            get
            {
                return _httpResponse.Body;
            }
        }
        public string RedirectLocation
        {
            set
            {
                _httpResponse.Redirect(value);
            }
        }
        public bool SendChunked
        {
            get
            {
                return _httpResponse.Chunked;
            }
            set
            {
                _httpResponse.Chunked = value;
            }
        }

        public virtual int StatusCode
        {
            get
            {
                return (int)_httpResponse.Status;
            }
            set
            {
                _httpResponse.Status = (HttpStatusCode)value;
            }
        }

        public string StatusDescription
        {
            get
            {
                return _httpResponse.Reason;
            }
            set
            {
                _httpResponse.Reason = value;
            }
        }

        public bool ReuseContext
        {
            get
            {
                return true;
            }
            set
            {

            }
        }
        public long ContentLength
        {
            get
            {
                return _httpResponse.ContentLength;
            }
            set
            {
                _httpResponse.ContentLength = value;
            }
        }
        public long ContentLength64
        {
            get
            {
                return _httpResponse.ContentLength;
            }
            set
            {
                _httpResponse.ContentLength = value;
            }
        }
        public OSHttpResponse()
        { }

        public OSHttpResponse(IHttpResponse resp)
        {
            _httpResponse = resp;
        }

        public OSHttpResponse(OSHttpRequest req)
        {
            _httpResponse = new HttpServer.HttpResponse(req.IHttpClientContext, req.IHttpRequest);
            _httpClientContext = req.IHttpClientContext;
        }
        public OSHttpResponse(HttpServer.HttpResponse resp, IHttpClientContext clientContext)
        {
            _httpResponse = resp;
            _httpClientContext = clientContext;
        }

        public void AddHeader(string key, string value)
        {
            _httpResponse.AddHeader(key, value);
        }

        public void Send()
        {
            _httpResponse.Body.Flush();
            _httpResponse.Send();
        }

        public void FreeContext()
        {
            if (_httpClientContext != null)
                _httpClientContext.Close();
        }

    }

    public class OSHttpRequest
    {
        protected IHttpRequest _request = null;
        protected IHttpClientContext _context = null;
        private Encoding _contentEncoding;
        private string _contentType;
        private NameValueCollection _queryString;
        private Hashtable _query;
        private IPEndPoint _remoteIPEndPoint;
        private string _userAgent;
        private BotOutput _botOutput;

        public string[] AcceptTypes
        {
            get { return _request.AcceptTypes; }
        }

        public Encoding ContentEncoding
        {
            get { return _contentEncoding; }
        }

        public long ContentLength
        {
            get { return _request.ContentLength; }
        }

        public long ContentLength64
        {
            get { return ContentLength; }
        }

        public string ContentType
        {
            get { return _contentType; }
        }

        public HttpCookieCollection Cookies
        {
            get
            {
                RequestCookies cookies = _request.Cookies;
                HttpCookieCollection httpCookies = new HttpCookieCollection();
                foreach (RequestCookie cookie in cookies)
                    httpCookies.Add(new HttpCookie(cookie.Name, cookie.Value));
                return httpCookies;
            }
        }
        public bool HasEntityBody
        {
            get { return _request.ContentLength != 0; }

        }

        public NameValueCollection Headers
        {
            get { return _request.Headers; }
        }

        public string HttpMethod
        {
            get { return _request.Method; }
        }

        public Stream InputStream
        {
            get { return _request.Body; }
        }

        public HttpForm Form
        {
            get { return _request.Form; }
        }

        public bool IsSecured
        {
            get { return _context.Secured; }
        }

        public bool KeepAlive
        {
            get { return ConnectionType.KeepAlive == _request.Connection; }
        }

        public NameValueCollection QueryString
        {
            get { return _queryString; }
        }

        public Hashtable Query
        {
            get { return _query; }
        }

        public string RawUrl
        {
            get { return _request.Uri.AbsolutePath; }
        }

        public IPEndPoint RemoteIPEndPoint
        {
            get { return _remoteIPEndPoint; }
        }

        public Uri Url
        {
            get { return _request.Uri; }
        }

        public string UserAgent
        {
            get { return _userAgent;  }
        }

        internal IHttpRequest IHttpRequest
        {
            get { return _request; }
        }

        internal IHttpClientContext IHttpClientContext
        {
            get { return _context; }
        }

        public OSHttpRequest()
        {
            _botOutput = BotOutput.Instance;
        }
        public OSHttpRequest(IHttpClientContext context, IHttpRequest req)
        {
            _botOutput = BotOutput.Instance;

            _request = req;
            _context = context;

            if (req.Headers["content-encoding"] != null)
                _contentEncoding = Encoding.GetEncoding(_request.Headers["content-encoding"]);
            if (req.Headers["content-type"] != null)
                _contentType = _request.Headers["content-type"];
            if (req.Headers["user-agent"] != null)
                _userAgent = _request.Headers["user-agent"];

            if (req.Headers["remote_addr"] != null)
            {
                try
                {
                    IPAddress addr = IPAddress.Parse(req.Headers["remote_addr"]);

                    string[] strPorts = _request.Headers["remote_port"].Split(new char[] { ',' });
                    if (strPorts.Length > 1)
                    {
                        _botOutput.LogMessage("error", string.Format("[HttpRequest]: format exception on addr/port {0}:{1}, ignoring {2}", _request.Headers["remote_addr"], _request.Headers["remote_port"], Environment.NewLine));
                    }
                    int port = Int32.Parse(strPorts[0]);
                    _remoteIPEndPoint = new IPEndPoint(addr, port);
                }
                catch (FormatException)
                {
                    // 
                    _botOutput.LogMessage("error", string.Format("[HttpRequest]: format exception on addr/port {0}:{1}, ignoring {2}", _request.Headers["remote_addr"], _request.Headers["remote_port"], Environment.NewLine));
                }
            }

            _queryString = new NameValueCollection();
            _query = new Hashtable();
            try
            {
                foreach (HttpInputItem item in req.QueryString)
                {
                    try
                    {
                        _queryString.Add(item.Name, item.Value);
                        _query[item.Name] = item.Value;
                    }
                    catch (InvalidCastException)
                    {
                        _botOutput.LogMessage("error", string.Format("[HttpRequest]: error parsing {0} query item, skipping it {1}", item.Name, Environment.NewLine));
                    }
                }
            } 
            catch (Exception)
            {
                _botOutput.LogMessage("error", "[HttpRequest]: error parsing querystring.");
            }
      

        
        }
        public override string ToString()
        {
            StringBuilder me = new StringBuilder();
                me.Append(string.Format("OSHttpRequest: {0} {1}{2}", HttpMethod, RawUrl, Environment.NewLine));
            foreach (string k in Headers.AllKeys)
            {
                me.Append(String.Format("     {0}: {1}{2}", k, Headers[k], Environment.NewLine));
            }
            if (RemoteIPEndPoint != null)
            {
                me.Append(string.Format("     IP: {0}{1}", RemoteIPEndPoint, Environment.NewLine));
            }
            me.Append(base.ToString());
            return me.ToString();
        }
    }
}
