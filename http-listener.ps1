[CmdletBinding()]
param()

Import-Module (Join-Path -Path $PSScriptRoot -ChildPath "HttpListener.psm1") 

$listener = Start-HttpListener -Prefix "http://localhost/application/redirect/" -RandomPort

$html = "<p>Operation completed</p>"

Start-Process -FilePath "curl" -ArgumentList "$($listener.Prefix)?name=value" -NoNewWindow 

$listener | Read-HttpRequest | Write-HttpResponse -Body $html -Stop -PassThru
