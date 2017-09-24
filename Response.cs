namespace SimpleHttpListener
{
    using System;
    using System.Net;
    using System.Text;

    public class Response : IDisposable
    {
        private readonly AtomicBool closed = new AtomicBool();
        private readonly Request request;
        private readonly HttpListenerResponse response;
        public Response(Request request, HttpListenerResponse response)
        {
            this.request = request;
            this.response = response;
            this.Status = HttpStatusCode.NotFound;
        }

        public HttpStatusCode Status { get; set; }

        public string ContentType { get; set; }

        public string Body { get; set; }
        public string Location { get; set; }

        private void Write()
        {
            response.SendChunked = false;
            response.KeepAlive = false;
            response.StatusCode = (int)Status;
            response.RedirectLocation = Location;
            if (!string.IsNullOrEmpty(Body))
            {
                response.ContentEncoding = Encoding.UTF8;
                response.ContentType = response.ContentType;
                byte[] b = Encoding.UTF8.GetBytes(Body);
                response.ContentLength64 = b.Length;
                response.Close(b, true);
            }
            else
            {
                response.ContentLength64 = 0;
                response.Close();
            }
        }

        internal void Close()
        {
            if(!closed.GetAndSet(true))
            {
                Write();
                response.Close();
            }
            request.Close();
        }

        public override string ToString()
        {
            string s = "HTTP/1.1 " + (int)Status + " " + Status.ToString();
            if(!string.IsNullOrWhiteSpace(ContentType))
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
