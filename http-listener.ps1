[CmdletBinding()]
param()

Import-Module "HttpListener" 

$listener = Start-HttpListener -Prefix "http://localhost:12340/hello/" -RandomPort 

$b = [uribuilder]::new($listener.Prefix)
$b.Path = "/hello/begin"

$completed = @"
<body onload="window.close()">
<p>The operation was completed.</p>
<p><input type="button" onclick="window.close()" value="Close"></input></p>
</body>
"@

Start-Process -Verb "open" -FilePath $b.Uri | Out-Null

$req = $listener | Read-HttpRequest | % {
    $local:r = $_
	switch($r.Url.LocalPath) {
		"/hello/begin" { $r | Write-HttpResponse -Location "/hello/" }
		"/hello/" { $r | Write-HttpResponse -Body $completed -Stop -PassThru }
	}
}
$listener | Stop-HttpListener
$req 

