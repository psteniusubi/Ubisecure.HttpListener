namespace SimpleHttpListener
{
    using System.Management.Automation;
    using System.Threading;

    [Cmdlet(VerbsCommunications.Read,"Request")]
    [OutputType(typeof(Request))]
    public class ReadHttpRequestCommand : PSCmdlet
    {
        private CancellationTokenSource cancellation;

        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        [ValidateNotNull()]
        public Listener Listener { get; set; }

        protected override void BeginProcessing()
        {
            cancellation = new CancellationTokenSource();
        }

        protected override void ProcessRecord()
        {
            while(!Stopping)
            {
                using(Request request = Listener.ReadRequest(cancellation.Token))
                {
                    if(request == null)
                    {
                        WriteVerbose("Read-Request EOF");
                        break;
                    }
                    WriteVerbose("Read-Request << " + request);
                    WriteObject(request);
                    //WriteVerbose("Read-Request >> " + request.Response);
                }
            }
        }
        protected override void StopProcessing()
        {
            if(cancellation != null)
            {
                cancellation.Cancel(true);
            }
            if (Listener != null)
            {
                Listener.Close();
            }
            base.StopProcessing();
        }
    }
}
