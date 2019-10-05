using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using CoolHTTPListener = HttpServer.HttpListener;
using HttpServer;
using HttpServer.FormDecoders;

namespace RebootTechBotLib.Infrastructure
{
    public delegate Hashtable GenericHTTPMethod(Hashtable request);
    public delegate void WebSocketRequestDelegate(string servicepath, WebSocketHttpServerHandler handler);

    public class BaseHttpServer
    {
        BotOutput m_Output = null;
        

        protected Dictionary<string, GenericHTTPMethod> m_HTTPHandlers = new Dictionary<string, GenericHTTPMethod>();
        protected Dictionary<string, IRequestHandler> m_streamHandlers = new Dictionary<string, IRequestHandler>();
        protected Dictionary<string, WebSocketRequestDelegate> m_WebSocketHandlers = new Dictionary<string, WebSocketRequestDelegate>();
       

        protected CoolHTTPListener m_HttpListener;

        protected volatile bool HTTPRunning = false;

        protected IPAddress m_listenIPAdress = IPAddress.Any;
        protected uint m_port = 0;

        /// <summary>
        /// MIME types supported by extension
        /// </summary>
        private Dictionary<string, MimeType> m_Mimes = new Dictionary<string, MimeType>();

        private Config.BotConfig m_BotConfig = null;
        private FileRequest m_fileRequestHandler = null;

        public IPAddress ListenIPAddress
        {
            get { return m_listenIPAdress;  }
            set { m_listenIPAdress = value; }
        }


        public BaseHttpServer(Config.BotConfig pConfig)
        {
            m_Output = BotOutput.Instance;
            m_port = (uint)pConfig.httpserver.Port;
            m_BotConfig = pConfig;
            m_fileRequestHandler = new FileRequest(pConfig.httpserver);
            RegisterMimeTypes(m_Mimes);
        }

        private void RegisterMimeTypes(Dictionary<string, MimeType> m_Mimes)
        {
            m_Mimes.Add(".aac", new MimeType() { binary = true, Description= "AAC audio", Extension=".aac", MIME= "audio/aac" });
            m_Mimes.Add(".abw", new MimeType() { binary = true, Description = "AbiWord Document", Extension = ".abw", MIME = "application/x-abiword" });
            m_Mimes.Add(".arc", new MimeType() { binary = true, Description = "Archive document (multiple files embedded)", Extension = ".arc", MIME = "application/x-freearc" });
            m_Mimes.Add(".avi", new MimeType() { binary = true, Description = "AVI Audio Video Interleave", Extension = ".avi", MIME = "video/x-msvideo" });
            m_Mimes.Add(".azw", new MimeType() { binary = true, Description = "Amazon Kindle eBook format", Extension = ".azw", MIME = "application/vnd.amazon.ebook" });
            m_Mimes.Add(".bin", new MimeType() { binary = true, Description = "Any kind of binary data", Extension = ".bin", MIME = "application/octet-stream" });
            m_Mimes.Add(".bmp", new MimeType() { binary = true, Description = "Windows OS/2 Bitmap Graphics", Extension = ".bmp", MIME = "image/bmp" });
            m_Mimes.Add(".bz", new MimeType() { binary = true, Description = "BZip Archive", Extension = ".bz", MIME = "application/x-bzip" });
            m_Mimes.Add(".bz2", new MimeType() { binary = true, Description = "BZip2 Archive", Extension = ".bz2", MIME = "application/x-bzip2" });
            m_Mimes.Add(".csh", new MimeType() { binary = false, Description = "C-Shell script", Extension = ".csh", MIME = "application/x-csh", Encoding=Encoding.ASCII });
            m_Mimes.Add(".css", new MimeType() { binary = false, Description = "Cascading Style Sheets (CSS)", Extension = ".css", MIME = "text/css" });
            m_Mimes.Add(".csv", new MimeType() { binary = false, Description = "Comma Separated Values", Extension = ".csv", MIME = "text/csv" });
            m_Mimes.Add(".doc", new MimeType() { binary = true, Description = "Microsoft Word", Extension = ".doc", MIME = "application/msword" });
            m_Mimes.Add(".docx", new MimeType() { binary = true, Description = "Microsoft Word (OpenXML)", Extension = ".docx", MIME = "application/vnd.openxmlformats-officedocument.wordprocessingml.document" });
            m_Mimes.Add(".eot", new MimeType() { binary = true, Description = "MS Embedded OpenType font", Extension = ".eot", MIME = "application/vnd.ms-fontobject" });
            m_Mimes.Add(".epub", new MimeType() { binary = true, Description = "Electronic publication (EPUB)", Extension = ".epub", MIME = "application/epub+zip" });
            m_Mimes.Add(".gif", new MimeType() { binary = true, Description = "Graphics Interchange Format", Extension = ".gif", MIME = "image/gif" });
            m_Mimes.Add(".htm", new MimeType() { binary = false, Description = "HyperText Markup Language (HTM)", Extension = ".htm", MIME = "text/html" });
            m_Mimes.Add(".html", new MimeType() { binary = false, Description = "HyperText Markup Language (HTML)", Extension = ".html", MIME = "text/html" });
            m_Mimes.Add(".ico", new MimeType() { binary = true, Description = "Icon format", Extension = ".ico", MIME = "image/vnd.microsoft.icon" });
            m_Mimes.Add(".ics", new MimeType() { binary = false, Description = "iCalendar format", Extension = ".ics", MIME = "text/calendar" });
            m_Mimes.Add(".jar", new MimeType() { binary = true, Description = "Java Archive", Extension = ".jar", MIME = "application/java-archive" });
            m_Mimes.Add(".jpg", new MimeType() { binary = true, Description = "JPEG image", Extension = ".jpg", MIME = "image/jpeg" });
            m_Mimes.Add(".jpeg", new MimeType() { binary = true, Description = "JPEG image", Extension = ".jpeg", MIME = "image/jpeg" });
            m_Mimes.Add(".js", new MimeType() { binary = false, Description = "JavaScript", Extension = ".js", MIME = "text/javascript" });
            m_Mimes.Add(".json", new MimeType() { binary = false, Description = "JavaScript Object Notation", Extension = ".json", MIME = "application/json" });
            m_Mimes.Add(".jsonld", new MimeType() { binary = false, Description = "JSON-LD format", Extension = ".jsonld", MIME = "application/ld+json" });
            m_Mimes.Add(".mid", new MimeType() { binary = true, Description = "Musical Instrument Digital Interface (MIDI)", Extension = ".mid", MIME = "audio/midi" });
            m_Mimes.Add(".midi", new MimeType() { binary = true, Description = "Musical Instrument Digital Interface (MIDI)", Extension = ".midi", MIME = "audio/x-midi" });
            m_Mimes.Add(".mjs", new MimeType() { binary = false, Description = "JavaScript module", Extension = ".mjs", MIME = "text/javascript" });
            m_Mimes.Add(".mp3", new MimeType() { binary = true, Description = "MP3 Audio", Extension = ".mp3", MIME = "audio/mpeg" });
            m_Mimes.Add(".mpeg", new MimeType() { binary = true, Description = "MPEG Video", Extension = ".mpeg", MIME = "video/mpeg" });
            m_Mimes.Add(".mpkg", new MimeType() { binary = true, Description = "Apple Installer Package", Extension = ".mpkg", MIME = "application/vnd.apple.installer+xml" });
            m_Mimes.Add(".odp", new MimeType() { binary = true, Description = "OpenDocument presentation document", Extension = ".odp", MIME = "application/vnd.oasis.opendocument.presentation" });
            m_Mimes.Add(".ods", new MimeType() { binary = true, Description = "OpenDocument spreadsheet document", Extension = ".ods", MIME = "application/vnd.oasis.opendocument.spreadsheet" });
            m_Mimes.Add(".odt", new MimeType() { binary = true, Description = "OpenDocument text document", Extension = ".odt", MIME = "application/vnd.oasis.opendocument.text" });
            m_Mimes.Add(".oga", new MimeType() { binary = true, Description = "OGG audio", Extension = ".oga", MIME = "audio/ogg" });
            m_Mimes.Add(".ogv", new MimeType() { binary = true, Description = "OGG video", Extension = ".ogv", MIME = "video/ogg" });
            m_Mimes.Add(".ogx", new MimeType() { binary = true, Description = "OGG", Extension = ".ogx", MIME = "application/ogg" });
            m_Mimes.Add(".otf", new MimeType() { binary = true, Description = "OpenType font", Extension = ".otf", MIME = "font/otf" });
            m_Mimes.Add(".png", new MimeType() { binary = true, Description = "Portable Network Graphics", Extension = ".png", MIME = "image/png" });
            m_Mimes.Add(".pdf", new MimeType() { binary = true, Description = "Adobe Portable Document Format", Extension = ".pdf", MIME = "application/pdf" });
            m_Mimes.Add(".ppt", new MimeType() { binary = true, Description = "Microsoft PowerPoint", Extension = ".ppt", MIME = "application/vnd.ms-powerpoint" });
            m_Mimes.Add(".pptx", new MimeType() { binary = true, Description = "Microsoft PowerPoint (OpenXML)", Extension = ".pptx", MIME = "application/vnd.openxmlformats-officedocument.presentationml.presentation" });
            m_Mimes.Add(".rar", new MimeType() { binary = true, Description = "Rarsoft RAR archive", Extension = ".rar", MIME = "application/x-rar-compressed" });
            m_Mimes.Add(".rtf", new MimeType() { binary = true, Description = "Rich Text Format", Extension = ".rtf", MIME = "application/rtf" });
            m_Mimes.Add(".sh", new MimeType() { binary = false, Description = "Borne shell script", Extension = ".sh", MIME = "application/x-sh" });
            m_Mimes.Add(".svg", new MimeType() { binary = false, Description = "Scalable Vector Graphics", Extension = ".svg", MIME = "image/svg+xml" });
            m_Mimes.Add(".swf", new MimeType() { binary = true, Description = "Small Web Format Adobe Flash Document", Extension = ".swf", MIME = "application/x-shockwave-flash" });
            m_Mimes.Add(".tar", new MimeType() { binary = true, Description = "Tape Archive", Extension = ".tar", MIME = "application/x-tar" });
            m_Mimes.Add(".tif", new MimeType() { binary = true, Description = "Tagged Image File Format", Extension = ".tif", MIME = "image/tiff" });
            m_Mimes.Add(".tiff", new MimeType() { binary = true, Description = "Tagged Image File Format", Extension = ".tiff", MIME = "image/tiff" });
            m_Mimes.Add(".ttf", new MimeType() { binary = true, Description = "TrueType Font", Extension = ".ttf", MIME = "font/ttf" });
            m_Mimes.Add(".txt", new MimeType() { binary = false, Description = "Text Plain Typically ASCII or ISO 8859 Encoded", Extension = ".txt", MIME = "text/plain", Encoding = Encoding.ASCII });
            m_Mimes.Add(".vsd", new MimeType() { binary = true, Description = "Microsoft Visio", Extension = ".vsd", MIME = "application/vnd.visio" });
            m_Mimes.Add(".wav", new MimeType() { binary = true, Description = "Waveform Audio Format", Extension = ".wav", MIME = "audio/wav" });
            m_Mimes.Add(".weba", new MimeType() { binary = true, Description = "WEBM Audio", Extension = ".weba", MIME = "audio/webm" });
            m_Mimes.Add(".webm", new MimeType() { binary = true, Description = "WEBM Video", Extension = ".webm", MIME = "video/webm" });
            m_Mimes.Add(".webp", new MimeType() { binary = true, Description = "WEBM image", Extension = ".webp", MIME = "image/webp" });
            m_Mimes.Add(".woff", new MimeType() { binary = false, Description = "Web Open Font Format", Extension = ".woff", MIME = "font/woff" });
            m_Mimes.Add(".woff2", new MimeType() { binary = true, Description = "Web Open Font v2 Format", Extension = ".woff2", MIME = "font/woff2" });
            m_Mimes.Add(".xhtml", new MimeType() { binary = false, Description = "XML formatted HTML", Extension = ".xhtml", MIME = "application/xhtml+xml" });
            m_Mimes.Add(".xls", new MimeType() { binary = true, Description = "Microsoft Excel", Extension = ".xls", MIME = "application/vnd.ms-excel" });
            m_Mimes.Add(".xlsx", new MimeType() { binary = true, Description = "Microsoft Excel (OpenXML)", Extension = ".xlsx", MIME = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
            m_Mimes.Add(".xml", new MimeType() { binary = true, Description = "Xtensible Markup Language", Extension = ".xml", MIME = "application/xml" });
            m_Mimes.Add(".xul", new MimeType() { binary = true, Description = "Xtensible UI Language", Extension = ".xul", MIME = "application/vnd.mozilla.xul+xml" });
            m_Mimes.Add(".zip", new MimeType() { binary = true, Description = "Zipped File Archive", Extension = ".zip", MIME = "application/zip" });
            m_Mimes.Add(".3gp", new MimeType() { binary = true, Description = "3GPP audio/video container", Extension = ".3gp", MIME = "video/3gpp" });
            m_Mimes.Add(".3g2", new MimeType() { binary = true, Description = "3GPP2 audio/video container", Extension = ".3g2", MIME = "video/3gpp2" });
            m_Mimes.Add(".7z", new MimeType() { binary = true, Description = "7Zip File Archive", Extension = ".7z", MIME = "application/x-7z-compressed" });
            //m_Mimes.Add(".aac", new MimeType() { binary = true, Description = "", Extension = ".aac", MIME = "" });
        }

        public bool TryGetRegisteredMIMETypeByExtension(string extension, out MimeType mimetype)
        {
            return m_Mimes.TryGetValue(extension, out mimetype);
        }

        public void Start()
        {
            StartHTTP();

        }
        public bool IsStarted
        {
            get { return HTTPRunning; }
        }
        private void StartHTTP()
        {
            if (HTTPRunning)
                return;
            int m_port_original = 0;
            int m_optional_port = 0;
            try
            {
                m_port_original = (int)m_port;
                m_optional_port = m_port_original + 1;
                m_HttpListener = CoolHTTPListener.Create(IPAddress.Any, (int)m_port);
                m_HttpListener.ExceptionThrown += httpServerException;
                m_HttpListener.RequestReceived += OnRequest;
                m_HttpListener.Start(m_BotConfig.httpserver.BacklogQueue);
                HTTPRunning = true;



            }
            catch (Exception e)
            {
                m_Output.LogMessage("error", "[HTTPD]: Failed to start HTTPD with " + e.Message + ".  Trying alternate port." + Environment.NewLine);
                try
                {

                    m_HttpListener = CoolHTTPListener.Create(IPAddress.Any, (int)m_optional_port);
                    m_HttpListener.ExceptionThrown += httpServerException;
                    m_HttpListener.RequestReceived += OnRequest;
                    m_HttpListener.Start(m_BotConfig.httpserver.BacklogQueue);
                    HTTPRunning = true;



                }
                catch (Exception f)
                {
                    m_Output.LogMessage("error", "[HTTPD]: Failed to start HTTPD with " + f.Message + Environment.NewLine);
                }
            }
        }

        private void OnRequest(object source, RequestEventArgs args)
        {
            try
            {
                IHttpClientContext context = (IHttpClientContext)source;
                IHttpRequest request = args.Request;

                OnHandleRequestIOCPThread(context, request);
            }
            catch (InvalidCastException e )
            {
                m_Output.LogMessage("error", "[HTTPD]: OnRequest failed with " + e.Message + Environment.NewLine);
            }
        }
        private void OnHandleRequestIOCPThread(IHttpClientContext context, IHttpRequest request)
        {
            OSHttpRequest req = new OSHttpRequest(context, request);

            WebSocketRequestDelegate dWebSocketRequestDelegate = null;
            lock (m_WebSocketHandlers)
            {
                if (m_WebSocketHandlers.ContainsKey(req.RawUrl))
                    dWebSocketRequestDelegate = m_WebSocketHandlers[req.RawUrl];
            }
            if (dWebSocketRequestDelegate != null)
            {
                dWebSocketRequestDelegate(req.Url.AbsolutePath, new WebSocketHttpServerHandler(req, context, 8192));
                return;
            }

            if (req.ContentType != null && req.ContentType.ToLowerInvariant() == "application/x-www-form-urlencoded")
            {
                IHttpClientContext postcontext = (IHttpClientContext)context;
                FormDecoderProvider provider = new FormDecoderProvider();
                provider.Add(new UrlDecoder());
            }
            OSHttpResponse resp = new OSHttpResponse(new HttpResponse(context, request), context);
            HandleRequest(req, resp);
        }

        public void Stop()
        {
            StopHTTP();
        }
        private void StopHTTP()
        {
            if (!HTTPRunning)
                return;
            m_HttpListener.Stop();
            m_HttpListener.ExceptionThrown -= httpServerException;
            m_HttpListener.RequestReceived -= OnRequest;
            HTTPRunning = false;
        }

        private void httpServerException(object source, Exception exception)
        {
            m_Output.LogMessage("error", "[HTTPServer]: Underlying server exception " + exception.Message + Environment.NewLine);
        }

        public void AddStreamHandler(IRequestHandler handler)
        {
            string httpMethod = handler.HttpMethod;
            string path = handler.Path;
            string handlerKey = GetHandlerKey(httpMethod, path);

            lock (m_streamHandlers)
            {
                if (!m_streamHandlers.ContainsKey(handlerKey))
                {
                    // m_log.DebugFormat("[BASE HTTP SERVER]: Adding handler key {0}", handlerKey);
                    m_streamHandlers.Add(handlerKey, handler);
                }
            }
        }
        public bool AddHTTPHandler(string methodName, GenericHTTPMethod handler)
        {
            lock (m_HTTPHandlers)
            {
                if (!m_HTTPHandlers.ContainsKey(methodName))
                {
                    m_HTTPHandlers.Add(methodName, handler);
                    return true;
                }
            }
            return false;
        }
        public void AddWebSocketHandler(string servicepath, WebSocketRequestDelegate handler)
        {
            lock (m_WebSocketHandlers)
            {
                if (!m_WebSocketHandlers.ContainsKey(servicepath))
                    m_WebSocketHandlers.Add(servicepath, handler);
            }
        }

        public void RemoveWebSocketHandler(string servicepath)
        {
            lock (m_WebSocketHandlers)
                if (m_WebSocketHandlers.ContainsKey(servicepath))
                    m_WebSocketHandlers.Remove(servicepath);
        }

        private bool TryGetStreamHandler(string handlerKey, out IRequestHandler streamHandler)
        {
            string bestMatch = null;

            lock (m_streamHandlers)
            {
                foreach (string pattern in m_streamHandlers.Keys)
                {
                    if (handlerKey.StartsWith(pattern))
                    {
                        if (String.IsNullOrEmpty(bestMatch) || pattern.Length > bestMatch.Length)
                        {
                            bestMatch = pattern;
                        }
                    }
                }

                if (String.IsNullOrEmpty(bestMatch))
                {
                    streamHandler = null;
                    return false;
                }
                else
                {
                    streamHandler = m_streamHandlers[bestMatch];
                    return true;
                }
            }
        }
        private bool TryGetHTTPHandler(string handlerKey, out GenericHTTPMethod HTTPHandler)
        {
            string bestMatch = null;
            lock (m_HTTPHandlers)
            {
                foreach (string pattern in m_HTTPHandlers.Keys)
                {
                    if (handlerKey.StartsWith(pattern))
                    {
                        if (string.IsNullOrEmpty(bestMatch) || pattern.Length > bestMatch.Length)
                        {
                            bestMatch = pattern;
                        }
                    }

                }
                if (string.IsNullOrEmpty(bestMatch))
                {
                    HTTPHandler = null;
                    return false;

                }
                else
                {
                    HTTPHandler = m_HTTPHandlers[bestMatch];
                    return true;
                }

            }
        }

        private bool TryGetHTTPHandlerPathBased(string path, out GenericHTTPMethod httpHandler)
        {
            string[] pathbase = path.Split('/');
            string searchquery = "/";
            httpHandler = null;

            if (pathbase.Length < 1)
                return false;

            for (int i=1; i<pathbase.Length; i++)
            {
                searchquery += pathbase[i];
                if (pathbase.Length - 1 != i)
                    searchquery += "/";
            }
            string bestMatch = null;

            lock (m_HTTPHandlers)
            {
                foreach (string pattern in m_HTTPHandlers.Keys)
                {
                    if (searchquery.ToLowerInvariant().StartsWith(pattern.ToLowerInvariant()))
                    {
                        if (string.IsNullOrEmpty(bestMatch) || searchquery.Length > bestMatch.Length)
                        {
                            if (pattern == "/" && searchquery == "/" || pattern != "/")
                                bestMatch = pattern;
                        }
                    }
                }
                if (string.IsNullOrEmpty(bestMatch))
                {
                    httpHandler = null;
                    return false;
                }
                else
                {
                    if (bestMatch == "/" && searchquery != "/")
                    {
                        return false;
                    }
                    httpHandler = m_HTTPHandlers[bestMatch];
                    return true;
                }
            }

        }

        private bool DoWeHaveAHTTPHandler(string path)
        {
            string[] pathbase = path.Split('/');
            string searchquery = "/";

            if (pathbase.Length < 1)
                return false;

            for (int i = 1; i < pathbase.Length; i++)
            {
                searchquery += pathbase[i];
                if (pathbase.Length - 1 != i)
                    searchquery += "/";
            }

            string bestMatch = null;

            lock (m_HTTPHandlers)
            {
                foreach (string pattern in m_HTTPHandlers.Keys)
                {
                    if (searchquery.StartsWith(pattern) && searchquery.Length >= pattern.Length)
                    {
                        bestMatch = pattern;
                    }
                }
            }
            if (path == "/")
                return false;

            if (string.IsNullOrEmpty(bestMatch))
            {
                return false;

            }
            else
            {
                return true;
            }

        }
        public static string GetHandlerKey(string httpMethod, string path)
        {
            return string.Join(httpMethod, ":", path);
        }
        public virtual void HandleRequest(OSHttpRequest request, OSHttpResponse response)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", true);
                response.SendChunked = false;
                IRequestHandler requestHandler;

                string path = request.RawUrl;
                string handlerKey = GetHandlerKey(request.HttpMethod, path);
                
                if (TryGetStreamHandler(handlerKey, out requestHandler))
                {

                    byte[] buffer = null;
                    response.ContentType = request.ContentType;
                    if (requestHandler is IGenericHTTPHandler)
                    {
                        IGenericHTTPHandler HttpRequestHandler = requestHandler as IGenericHTTPHandler;
                        Stream requestStream = request.InputStream;
                        Encoding encoding = Encoding.UTF8;
                        string requestBody = string.Empty;
                        using (StreamReader reader = new StreamReader(requestStream, encoding))
                        {
                            requestBody = reader.ReadToEnd();
                        }
                        Hashtable keysvals = new Hashtable();
                        Hashtable headervals = new Hashtable();
                        string[] querystringkeys = request.QueryString.AllKeys;
                        string[] rHeaders = request.Headers.AllKeys;

                        foreach (string queryname in querystringkeys)
                            keysvals.Add(queryname, request.Query[queryname]);
                        foreach (string headername in rHeaders)
                            headervals.Add(headername, request.Headers[headername]);
                        keysvals.Add("requestbody", requestBody);
                        keysvals.Add("headers", headervals);
                        keysvals.Add("form", request.Form);
                        keysvals.Add("uri", request.RawUrl);
                        keysvals.Add("contenttype", request.ContentType);
                        if (request.ContentEncoding != null && !keysvals.ContainsKey("encoding"))
                            keysvals.Add("encoding", request.ContentEncoding);
                        keysvals.Add("accepttypes", request.AcceptTypes);
                        DoHTTPGruntWork(HttpRequestHandler.Handle(path, keysvals), response);
                        return;
                    }
                    request.InputStream.Close();
                    if (buffer == null)
                        return;

                    if (!response.SendChunked)
                        response.ContentLength64 = buffer.LongLength;

                    try
                    {
                        response.OutputStream.Write(buffer, 0, buffer.Length);
                    }
                    catch (HttpListenerException)
                    {
                        m_Output.LogMessage("error", "[HTTPD]: HTTPRequest terminated abnormally " + Environment.NewLine);
                    }

                    try
                    {
                        response.Send();
                    }
                    catch (SocketException)
                    {
                        m_Output.LogMessage("error", "[HTTPD]: HTTPRequest terminated abnormally " + Environment.NewLine);
                    }
                    catch (IOException)
                    {
                        m_Output.LogMessage("error", "[HTTPD]: HTTPRequest terminated abnormally " + Environment.NewLine);
                    }


                }
                switch (request.ContentType)
                {
                    case null:
                    case "text/html":
                        HandleHTTPRequest(request, response);
                        return;
                    default:
                        if (DoWeHaveAHTTPHandler(request.RawUrl))
                        {
                            HandleHTTPRequest(request, response);
                            return;
                        }
                        return;
                }

            }
            catch (SocketException e)
            {
                m_Output.LogMessage("warn", "[HTTPD]: HandleRequest threw " + e.Message + Environment.NewLine);
            }
            catch (IOException e)
            {
                m_Output.LogMessage("warn", "[HTTPD]: HandleRequest threw " + e.Message + Environment.NewLine);
            }
            catch (InvalidOperationException e)
            {
                m_Output.LogMessage("warn", "[HTTPD]: HandleRequest threw " + e.Message + Environment.NewLine);
            }
            return;
        }

        public void HandleHTTPRequest(OSHttpRequest request, OSHttpResponse response)
        {
            switch (request.HttpMethod)
            {
                case "OPTIONS":
                    response.StatusCode = (int)HttpStatusCode.OK;
                    return;
                default:
                    HandleContentVerbs(request, response);
                    return;
            }
        }

        private void HandleContentVerbs(OSHttpRequest request, OSHttpResponse response)
        {
            Stream requestStream = request.InputStream;

            Encoding encoding = Encoding.UTF8;
            string requestBody = string.Empty;
            string host = string.Empty;
            using (StreamReader reader = new StreamReader(requestStream, encoding))
            {
                requestBody = reader.ReadToEnd();
            }

            Hashtable keysvals = new Hashtable();
            Hashtable headervals = new Hashtable();
            Hashtable requestVars = new Hashtable();
            string[] querystringkeys = request.QueryString.AllKeys;
            string[] rHeaders = request.Headers.AllKeys;

            foreach (string queryname in querystringkeys)
                keysvals.Add(queryname, request.Query[queryname]);
            foreach (string headername in rHeaders)
                headervals.Add(headername, request.Headers[headername]);

            keysvals.Add("body", requestBody);
            keysvals.Add("uri", request.RawUrl);
            keysvals.Add("content-type", request.ContentType);
            keysvals.Add("http-method", request.HttpMethod);

            if (request.ContentEncoding != null && !keysvals.ContainsKey("encoding"))
                keysvals.Add("encoding", request.ContentEncoding);

            keysvals.Add("accepttypes", request.AcceptTypes);

            foreach (string queryname in querystringkeys)
            {
                if (!keysvals.ContainsKey(queryname))
                    keysvals.Add(queryname, request.QueryString[queryname]);
                if (!requestVars.ContainsKey(queryname))
                    requestVars.Add(queryname, keysvals[queryname]);
            }

            foreach (string headername in rHeaders)
            {
                 headervals[headername] = request.Headers[headername];
            }

            if (headervals.Contains("Host"))
            {
                host = (string)headervals["Host"];
            }
            keysvals.Add("headers", headervals);
            keysvals.Add("form", request.Form);
            foreach (HttpInputItem item in request.Form)
            {
                if (!keysvals.ContainsKey(item.Name))
                    keysvals.Add(item.Name, item.Value);
                //m_log.Output(string.Format("{0}={1}\r\n", item.Name, item.Value));
            }

            if (keysvals.Contains("method"))
            {
                string method = (string)keysvals["method"];
                GenericHTTPMethod requestProcessor;
                bool foundHandler = TryGetHTTPHandler(method, out requestProcessor);
                if (foundHandler)
                {
                    Hashtable responsedata1 = requestProcessor(keysvals);
                    DoHTTPGruntWork(responsedata1, response);
                }
                else
                {
                    FileRequest.FileResponse fileoutput = null;
                    Exception exceptionout = null;
                    if (m_fileRequestHandler.TryGetFile(request, out fileoutput, out exceptionout))
                    {
                        if (exceptionout == null)
                            SendFile(request, response, fileoutput);
                        else
                        {
                            if (exceptionout is System.Security.SecurityException || exceptionout is UnauthorizedAccessException)
                            {
                                SendHTML403(response);
                            }
                            else if (exceptionout is FileNotFoundException || exceptionout is DirectoryNotFoundException || exceptionout is ArgumentException || exceptionout is ArgumentNullException)
                            {
                                SendHTML404(response, host);
                            }
                            else
                            {
                                SendHTML500(response);
                            }
                        }
                           
                    }
                    else
                        SendHTML404(response, host);
                }
            }
            else
            {
                GenericHTTPMethod requestProcessor;
                bool foundHandler = TryGetHTTPHandlerPathBased(request.RawUrl, out requestProcessor);
                if (foundHandler)
                {
                    Hashtable responsedata2 = requestProcessor(keysvals);
                    DoHTTPGruntWork(responsedata2, response);
                }
                else
                {
                    FileRequest.FileResponse fileoutput = null;
                    Exception exceptionout = null;
                    if (m_fileRequestHandler.TryGetFile(request, out fileoutput, out exceptionout))
                    {
                        if (exceptionout == null)
                            SendFile(request, response, fileoutput);
                        else
                        {
                            if (exceptionout is System.Security.SecurityException || exceptionout is UnauthorizedAccessException)
                            {
                                SendHTML403(response);
                            }
                            else if (exceptionout is FileNotFoundException || exceptionout is DirectoryNotFoundException || exceptionout is ArgumentException || exceptionout is ArgumentNullException)
                            {
                                SendHTML404(response, host);
                            }
                            else
                            {
                                SendHTML500(response);
                            }
                        }

                    }
                    else
                        SendHTML404(response, host);
                }
            }
        }
        internal void SendFile(OSHttpRequest req, OSHttpResponse response, FileRequest.FileResponse filedata)
        {
            response.StatusCode = 200;
            response.SendChunked = false;
            response.ContentType = filedata.MimeType.MIME;
            long filelength = filedata.Data.LongLength;
            response.ContentLength64 = filelength;
            if (!filedata.MimeType.binary)
                response.ContentEncoding = filedata.MimeType.Encoding;

            try
            {
                response.OutputStream.Write(filedata.Data, 0, (int)filelength);
            }
            catch (Exception ex)
            {
                m_Output.LogMessage("error", "[HTTPD]: Error - " + ex.Message + Environment.NewLine);
            }
            finally
            {
                try
                {
                    response.OutputStream.Flush();
                    response.Send();
                }
                catch (SocketException e)
                {
                    m_Output.LogMessage("warn", "There was an underlying socket error.  Perhaps the socket disconnected." + e.Message + Environment.NewLine);
                }
                catch (IOException e)
                {
                    m_Output.LogMessage("warn", "There was an IO issue: " + e.Message + Environment.NewLine);
                }
            }
        }
        internal void DoHTTPGruntWork(Hashtable responsedata, OSHttpResponse response)
        {
            int responsecode = (int)responsedata["int_response_code"];
            string responseString = string.Empty;

            if (responsedata.ContainsKey("str_response_string"))
                responseString = (string)responsedata["str_response_string"];

            string contentType = string.Empty;

            if (responsedata.ContainsKey("content_type"))
                contentType = (string)responsedata["content_type"];

            string RedirectLocationHeader = string.Empty;
            if (responsedata.ContainsKey("redirect_location"))
                RedirectLocationHeader = (string)responsedata["redirect_location"];

            if (responsedata.ContainsKey("error_status_text"))
            {
                response.StatusDescription = (string)responsedata["error_status_text"];
            }
            if (responsedata.ContainsKey("http_protocol_version"))
            {
                response.ProtocolVersion = (string)responsedata["http_protocolversion_version"];
            }
            if (responsedata.ContainsKey("keepalive"))
            {
                bool keepalive = (bool)responsedata["keepalive"];
                response.KeepAlive = keepalive;
            }

            if (responsedata.ContainsKey("reusecontext"))
            {
                response.ReuseContext = (bool)responsedata["reusecontext"];
            }

            if (string.IsNullOrEmpty(contentType))
            {
                contentType = "text/html";
            }
            response.StatusCode = responsecode;
            
            switch (responsecode)
            {
                case 302:
                    response.AddHeader("Location", RedirectLocationHeader);
                    break;
                default:
                    response.AddHeader("Content-Type", contentType);
                    break;
            }

            

            byte[] buffer;
            if (!contentType.Contains("image"))
            {
                buffer = Encoding.UTF8.GetBytes(responseString);
            }
            else
            {
                buffer = Convert.FromBase64String(responseString);
            }
            response.SendChunked = false;
            response.ContentLength64 = buffer.LongLength;
            response.ContentEncoding = Encoding.UTF8;

            try
            {
                response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                m_Output.LogMessage("error", "[HTTPD]: Error - " + ex.Message + Environment.NewLine);
            }
            finally
            {
                try
                {
                    response.OutputStream.Flush();
                    response.Send();
                }
                catch (SocketException e)
                {
                    m_Output.LogMessage("warn", "There was an underlying socket error.  Perhaps the socket disconnected." + e.Message + Environment.NewLine);
                }
                catch (IOException e)
                {
                    m_Output.LogMessage("warn", "There was an IO issue: " + e.Message + Environment.NewLine);
                }
            }

        }

        public void SendHTML404(OSHttpResponse response, string host)
        {
            response.StatusCode = 404;

            response.AddHeader("Content-Type", "text/html");
            string responseString = "<html><head><title>OMG 404 FailWhale OMEGALOL Kappa</title></head><body><H1>It didnt work!</H1><p>There is no module handling this request Kappa</p></body></html>";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            response.SendChunked = false;
            response.ContentLength64 = buffer.LongLength;
            response.ContentEncoding = Encoding.UTF8;
            try
            {
                response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                m_Output.LogMessage("error", "[HTTPD]: Error - " + ex.Message + Environment.NewLine);
            }
            finally
            {
                try
                {
                    response.Send();
                }
                catch (SocketException e)
                {
                    m_Output.LogMessage("error", "[HTTPD]: SocketError - " + e.Message + Environment.NewLine);
                }
            }
        }
        public void SendHTML403(OSHttpResponse response)
        {
            // I know this statuscode is dumb, but the client doesn't respond to 404s and 500s
            response.StatusCode = (int)HttpStatusCode.Forbidden;
            response.AddHeader("Content-type", "text/html");

            string responseString = "<html><head><title>OMG FORBIDDEN</title></head><body><H1>This is not something I can share!</H1><p>I cannot let you view this file, sorry.   Only thing.  Sometimes it do be like that.</p></body></html>"; ;
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);

            response.SendChunked = false;
            response.ContentLength64 = buffer.Length;
            response.ContentEncoding = Encoding.UTF8;
            try
            {
                response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                m_Output.LogMessage("error", "[HTTPD]: Error - " + ex.Message + Environment.NewLine);
            }
            finally
            {
                //response.OutputStream.Close();
                try
                {
                    response.Send();
                    //response.FreeContext();
                }
                catch (SocketException e)
                {
                    // This has to be here to prevent a Linux/Mono crash
                    m_Output.LogMessage("error", "[HTTPD] Socket issue " + e.Message + Environment.NewLine);
                }
            }
        }
        public void SendHTML500(OSHttpResponse response)
        {
            // I know this statuscode is dumb, but the client doesn't respond to 404s and 500s
            response.StatusCode = (int)HttpStatusCode.OK;
            response.AddHeader("Content-type", "text/html");

            string responseString = "<html><head><title>OMG 500 It esploded</title></head><body><H1>Something had a problem!</H1><p>There was a problem doing the thing.   Only thing.  Sometimes it do be like that.</p></body></html>"; ;
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);

            response.SendChunked = false;
            response.ContentLength64 = buffer.Length;
            response.ContentEncoding = Encoding.UTF8;
            try
            {
                response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                m_Output.LogMessage("error", "[HTTPD]: Error - " + ex.Message + Environment.NewLine);
            }
            finally
            {
                //response.OutputStream.Close();
                try
                {
                    response.Send();
                    //response.FreeContext();
                }
                catch (SocketException e)
                {
                    // This has to be here to prevent a Linux/Mono crash
                    m_Output.LogMessage("error", "[HTTPD] Socket issue " + e.Message + Environment.NewLine);
                }
            }
        }

    }
}
