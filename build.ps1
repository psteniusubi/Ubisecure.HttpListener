[CmdletBinding()]
param()

$module = Split-Path -Path $PSCommandPath -Parent | Join-Path -ChildPath "bin/Debug/net47/HttpListener.dll"

if(-not (Test-Path -Path $module)) {
    & dotnet build -f net47 -v q | Out-Null
}
