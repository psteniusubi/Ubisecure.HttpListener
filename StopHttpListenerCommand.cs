namespace SimpleHttpListener
{
    using System.Management.Automation;

    [Cmdlet(VerbsLifecycle.Stop,"Listener")]
    public class StopHttpListenerCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        [ValidateNotNull()]
        public Listener Listener { get; set; }
        protected override void ProcessRecord()
        {
            WriteVerbose("Stop-Listener " + Listener);
            Listener.Close();
        }
    }
}
