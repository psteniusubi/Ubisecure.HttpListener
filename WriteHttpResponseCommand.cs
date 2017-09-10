using System.Management.Automation;
using System.Net;

namespace SimpleHttpListener
{
    [Cmdlet(VerbsCommunications.Write,"Response")]
    [OutputType(typeof(Request))]
    public class WriteHttpResponseCommand : PSCmdlet
    {
        public WriteHttpResponseCommand()
        {
            this.Status = HttpStatusCode.OK;
            this.ContentType = "text/html";
        }

        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        [ValidateNotNull()]
        public Request Request { get; set; }

        [Parameter()]
        [PSDefaultValue(Value = HttpStatusCode.OK)]
        public HttpStatusCode Status { get; set; } 

        [Parameter()]
        [PSDefaultValue(Value = "text/html")]
        [AllowNull()]
        public string ContentType { get; set; } 

        [Parameter()]
        [AllowEmptyString()]
        [AllowNull()]
        public string Body { get; set; }

        [Parameter()]
        public SwitchParameter Stop { get; set; }

        [Parameter()]
        public SwitchParameter PassThru { get; set; }

        protected override void ProcessRecord()
        {
            Request.Response.Status = Status;
            Request.Response.ContentType = ContentType;
            Request.Response.Body = Body;
            Request.Response.Close();
            Request.Close();
            if (PassThru)
            {
                WriteObject(Request);
            }
            if (Stop)
            {
                Request.Listener.Close();
            }
        }
        protected override void StopProcessing()
        {
            if (Request != null)
            {
                Request.Close();
            }
        }
    }
}
