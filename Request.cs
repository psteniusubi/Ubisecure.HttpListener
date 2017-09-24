namespace SimpleHttpListener
{
    using System;
    using System.Collections.Specialized;
    using System.Net;
    using System.Text;

    public class Request : IDisposable
    {
        private readonly AtomicBool closed = new AtomicBool();
        private readonly Listener listener;
        private readonly HttpListenerContext cx;

        public Request(Listener listener, HttpListenerContext cx)
        {
            this.listener = listener;
            this.cx = cx;
            this.IsLocal = cx.Request.IsLocal;
            this.HttpMethod = cx.Request.HttpMethod;
            this.Url = cx.Request.Url;
            this.QueryString = cx.Request.QueryString;
            if(cx.Request.HasEntityBody)
            {
                this.ContentType = cx.Request.ContentType;
                byte[] b = new byte[cx.Request.ContentLength64];
                cx.Request.InputStream.Read(b, 0, b.Length);
                Encoding encoding = cx.Request.ContentEncoding;
                if (encoding == null) encoding = Encoding.UTF8;
                this.Body = encoding.GetString(b);
            }
            this.Response = new Response(this, cx.Response);
        }
        internal Listener Listener {  get { return listener; } }
        internal Response Response { get; private set; }
        public bool IsLocal { get; private set; }
        public string HttpMethod { get; private set; }
        public Uri Url { get; private set; }
        public string ContentType { get; private set; }
        public NameValueCollection QueryString { get; private set; }
        public string Body { get; private set; }

        internal void Close()
        {
            if(!closed.GetAndSet(true)) 
            {
                Response.Close();
            }
        }

        public override string ToString()
        {
            return HttpMethod + " " + Url;
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
