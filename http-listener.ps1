[CmdletBinding()]
param()

#$files = Get-ChildItem -Path "C:\Users\jeps\source\repos\SimpleHttpListener\SimpleHttpListener" -Filter "*.cs" | Select-Object -ExpandProperty FullName
#$type = Add-Type -Path $files -PassThru
#Import-Module -Assembly $type.Assembly -Prefix "Http" -Force

Import-Module -Name (Split-Path -Path $PSCommandPath -Parent | Join-Path -ChildPath "bin/Debug/net47/HttpListener.dll") -Prefix "Http" -Force

$listener = Start-HttpListener -Prefix "http://localhost:8080/hello/"
$req = $listener | Read-HttpRequest | 
#    % { Write-Host $_.Url.LocalPath; $_ }
    ? { $_.Url.LocalPath -eq "/hello/world" } |
    Write-HttpResponse -Body "<p>hello</p>" -Stop -PassThru
$listener | Stop-HttpListener
$req 

