[CmdletBinding()]
param()

Import-Module (Join-Path -Path $PSScriptRoot -ChildPath "Ubisecure.HttpListener.psd1") -Force

$listener = Start-HttpListener -Prefix "http://localhost:4321/"

$listener | Read-HttpRequest -PipelineVariable "request" | % {
    Write-Host "$($request.Method) $($request.Uri)"
    $request | Write-HttpResponse -Body "<hello />" -ContentType "application/xml" 
}
