namespace SimpleHttpListener
{
    using System;
    using System.Collections;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Security.Cryptography;
    using System.Threading;
    using System.Threading.Tasks;

    public class Listener : IDisposable
    {
        public static int FindFreePort(int min, bool random)
        {
            BitArray free = new BitArray(UInt16.MaxValue);
            for (var i = min; i < UInt16.MaxValue; i++)
            {
                free.Set(i, true);
            }
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            foreach (var i in properties.GetActiveTcpConnections())
            {
                free.Set(i.LocalEndPoint.Port, false);
            }
            foreach (var i in properties.GetActiveTcpListeners())
            {
                free.Set(i.Port, false);
            }
            foreach (var i in properties.GetActiveUdpListeners())
            {
                free.Set(i.Port, false);
            }
            if (random)
            {
                RandomNumberGenerator prng = RandomNumberGenerator.Create();
                for (var i = 0; i < UInt16.MaxValue; i++)
                {
                    byte[] b = new byte[2];
                    prng.GetNonZeroBytes(b);
                    int port = BitConverter.ToUInt16(b, 0);
                    if (port >= min && port < UInt16.MaxValue && free[port])
                    {
                        return port;
                    }
                }
            }
            else
            {
                for (var i = min; i < UInt16.MaxValue; i++)
                {
                    if (free[i])
                    {
                        return i;
                    }
                }
            }
            throw new ArgumentException();
        }

        private readonly AtomicBool started = new AtomicBool();
        private readonly AtomicBool closed = new AtomicBool();
        private HttpListener _listener;

        public Listener(Uri prefix, bool anyHost)
        {
            Prefix = prefix;
            _listener = new HttpListener();
            string s = Prefix.ToString();
            if(!s.EndsWith("/"))
            {
                s += "/";
            }
            if(anyHost) {
                UriBuilder u = new UriBuilder(s);
                u.Host = "*";
                _listener.Prefixes.Add(u.ToString());
            } else {
                _listener.Prefixes.Add(s);
            }
        }

        public Uri Prefix { get; private set; }

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
            return Prefix.ToString();
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
