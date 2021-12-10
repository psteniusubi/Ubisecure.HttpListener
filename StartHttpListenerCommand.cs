namespace SimpleHttpListener
{
    using System;
    using System.Management.Automation;

    [Cmdlet(VerbsLifecycle.Start,"Listener")]
    [OutputType(typeof(Listener))]
    public class StartHttpListenerCommand : PSCmdlet
    {
        private Listener listener;

        [Parameter(Mandatory = true)]
        [ValidateNotNull()]
        public Uri Prefix { get; set; }

        [Parameter()]
        public SwitchParameter AnyHost { get; set; }

        [Parameter()]
        public SwitchParameter RandomPort { get; set; }

        public StartHttpListenerCommand() 
        {
        }

        protected override void BeginProcessing()
        {
            Uri prefix = Prefix;
            if(RandomPort)
            {
                UriBuilder b = new UriBuilder(prefix);
                b.Port = Listener.FindFreePort(16384, true);
                prefix  = b.Uri;
            }
            listener = new Listener(prefix, AnyHost);
        }
        protected override void ProcessRecord()
        {
            if (listener != null)
            {
                WriteVerbose("Start-Listener " + listener);
                WriteObject(listener);
            }
        }
        protected override void StopProcessing()
        {
            if (listener != null)
            {
                listener.Close();
            }
            base.StopProcessing();
        }
    }
}
