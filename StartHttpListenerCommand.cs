using System.Management.Automation;

namespace SimpleHttpListener
{
    [Cmdlet(VerbsLifecycle.Start,"Listener")]
    [OutputType(typeof(Listener))]
    public class StartHttpListenerCommand : PSCmdlet
    {
        private Listener listener;

        [Parameter(Mandatory = true)]
        [ValidateNotNull()]
        public string Prefix { get; set; }

        protected override void BeginProcessing()
        {
            listener = new Listener(Prefix);
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
        }
    }
}
