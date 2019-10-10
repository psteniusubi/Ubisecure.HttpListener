[CmdletBinding()]
param()

Import-Module (Join-Path -Path $PSScriptRoot -ChildPath "Ubisecure.HttpListener.psd1") -Force

$listener = Start-HttpListener -Prefix "http://localhost:42388/application/redirect/" -RandomPort:$false

$html = "<p>Operation completed</p>"

# Start-Process -FilePath "curl" -ArgumentList "$($listener.Prefix)?name=value" -NoNewWindow 
#$job = Start-Job { param($listener) Invoke-Http -Get "$($listener.Prefix)?name=value" } -ArgumentList $listener

$listener | Read-HttpRequest -PipelineVariable "request" -Verbose | % {
    $request.ToString()
    #$request.QueryString
    $request | Write-HttpResponse -Body $html -Stop -Verbose
} 

#$job | Receive-Job -Wait -AutoRemoveJob
