[CmdletBinding()]
param()

$project = Join-Path -Path $PSScriptRoot -ChildPath "HttpListener.csproj" -Resolve -ErrorAction Stop
$module = Join-Path -Path $PSScriptRoot -ChildPath "bin/Debug/net47/HttpListener.dll"

Write-Verbose "Test-Path $module"
if(-not (Test-Path -Path $module)) {
    Write-Verbose "dotnet build $project"
    & dotnet build $project -f net47 -v q | Out-Null
}
