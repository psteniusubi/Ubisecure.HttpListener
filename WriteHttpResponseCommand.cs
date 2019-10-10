namespace SimpleHttpListener
{
    using System.Management.Automation;
    using System.Net;

    [Cmdlet(VerbsCommunications.Write,"Response",DefaultParameterSetName="Status")]
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

        [Parameter(ParameterSetName="Status")]
        [PSDefaultValue(Value = HttpStatusCode.OK)]
        public HttpStatusCode Status { get; set; } 

        [Parameter(ParameterSetName = "Status")]
        [PSDefaultValue(Value = "text/html")]
        [AllowNull()]
        public string ContentType { get; set; } 

        [Parameter(ParameterSetName = "Status")]
        [AllowEmptyString()]
        [AllowNull()]
        public string Body { get; set; }

        [Parameter(ParameterSetName = "Location",Mandatory = true)]
        [ValidateNotNull()]
        public string Location { get; set; }

        [Parameter()]
        public SwitchParameter Stop { get; set; }

        [Parameter()]
        public SwitchParameter PassThru { get; set; }

        protected override void ProcessRecord()
        {
            switch(ParameterSetName)
            {
                case "Status":
                    Request.Response.Status = Status;
                    Request.Response.ContentType = ContentType;
                    Request.Response.Body = Body;
                    break;
                case "Location":
                    Request.Response.Status = HttpStatusCode.Found;
                    Request.Response.Location = Location;
                    Request.Response.ContentType = null;
                    Request.Response.Body = null;
                    break;
            }
            WriteVerbose("Write-Response >> " + Request.Response);
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
            base.StopProcessing();
        }
    }
}
