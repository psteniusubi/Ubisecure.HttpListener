namespace SimpleHttpListener
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Management.Automation;
    using System.Net;
    using System.Text;

    public class Request : IDisposable
    {
        private readonly AtomicBool closed = new AtomicBool();
        private readonly HttpListenerContext cx;

        public Request(Listener listener, HttpListenerContext cx)
        {
            this.cx = cx;
            Listener = listener;
            Response = new Response(this, cx.Response);
            if (HasEntityBody)
            {
                Encoding encoding = cx.Request.ContentEncoding;
                if (encoding == null) encoding = Encoding.UTF8;
                using (var reader = new StreamReader(cx.Request.InputStream, encoding))
                {
                    Body = reader.ReadToEnd();
                }
            }
        }
        [Hidden]
        internal Listener Listener { get; private set; }
        [Hidden]
        internal Response Response { get; private set; }
        [Hidden]
        public HttpListenerRequest HttpRequest { get { return cx.Request; } }
        public bool IsLocal { get { return HttpRequest.IsLocal; } }
        public string HttpMethod { get { return HttpRequest.HttpMethod; } }
        public Uri Url { get { return HttpRequest.Url; } }
        public NameValueCollection QueryString { get { return HttpRequest.QueryString; } }
        public string ContentType { get { return HttpRequest.ContentType; } }
        public NameValueCollection Headers { get { return HttpRequest.Headers; } }
        public bool HasEntityBody { get { return HttpRequest.HasEntityBody; } }
        public string Body { get; private set; }

        internal void Close()
        {
            if (!closed.GetAndSet(true))
            {
                Response.Close();
            }
        }

        public override string ToString()
        {
            string s = HttpMethod + " " + Url;
            if(!string.IsNullOrWhiteSpace(ContentType))
            {
                s += " Content-Type: " + ContentType;
            }
            return s;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Close();
                }
                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
