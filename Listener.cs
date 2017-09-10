using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleHttpListener
{
    public class Listener : IDisposable
    {
        private readonly AtomicBool started = new AtomicBool();
        private readonly AtomicBool closed = new AtomicBool();
        private readonly string prefix;
        private HttpListener _listener;

        public Listener(string prefix)
        {
            this.prefix = prefix;
            _listener = new HttpListener();
            _listener.Prefixes.Add(prefix);
        }

        internal Request ReadRequest(CancellationToken cancellationToken)
        {
            if(_listener == null)
            {
                return null;
            }
            if (closed)
            {
                return null;
            }
            if(!started.GetAndSet(true)) 
            {
                _listener.Start();
            }
            try
            {
                Task<HttpListenerContext> t = _listener.GetContextAsync();
                t.Wait(cancellationToken);
                if (t.IsCompleted && t.Result != null)
                {
                    return new Request(this, t.Result);
                }
            }
            catch (OperationCanceledException)
            {
                // ignore
            }
            catch (HttpListenerException)
            {
                // ignore
            }
            return null;
        }

        internal void Close()
        {
            if(!closed.GetAndSet(true))
            {
                _listener.Abort();
                _listener.Close();
                _listener = null;
            }
        }

        public override string ToString()
        {
            return "Listener(" + prefix + ")";
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
