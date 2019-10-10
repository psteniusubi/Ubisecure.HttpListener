namespace SimpleHttpListener
{
    using System;
    using System.Management.Automation;
    using System.Net;
    using System.Text;

    public class Response : IDisposable
    {
        private readonly AtomicBool closed = new AtomicBool();
        private readonly Request request;
        public Response(Request request, HttpListenerResponse response)
        {
            this.request = request;
            HttpResponse = response;
            Status = HttpStatusCode.NotFound;
        }

        [Hidden]
        public HttpListenerResponse HttpResponse { get; private set; }

        public HttpStatusCode Status
        {
            get { return (HttpStatusCode)HttpResponse.StatusCode; }
            set { HttpResponse.StatusCode = (int)value; }
        }

        public string ContentType
        {
            get { return HttpResponse.ContentType; }
            set { HttpResponse.ContentType = value; }
        }

        public string Location
        {
            get { return HttpResponse.RedirectLocation; }
            set { HttpResponse.RedirectLocation = value; }
        }

        public string Body { get; set; }

        private void Write()
        {
            HttpResponse.SendChunked = false;
            HttpResponse.KeepAlive = false;
            if (!string.IsNullOrEmpty(Body))
            {
                HttpResponse.ContentEncoding = Encoding.UTF8;
                byte[] b = Encoding.UTF8.GetBytes(Body);
                HttpResponse.ContentLength64 = b.Length;
                HttpResponse.Close(b, true);
            }
            else
            {
                HttpResponse.ContentLength64 = 0;
                HttpResponse.Close();
            }
        }

        internal void Close()
        {
            if (!closed.GetAndSet(true))
            {
                Write();
                HttpResponse.Close();
            }
            request.Close();
        }

        public override string ToString()
        {
            string s = "HTTP/1.1 " + (int)Status + " " + Status.ToString();
            if (!string.IsNullOrWhiteSpace(ContentType))
            {
                s += " Content-Type: " + ContentType;
            }
            if (!string.IsNullOrWhiteSpace(Location))
            {
                s += " Location: " + Location;
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
