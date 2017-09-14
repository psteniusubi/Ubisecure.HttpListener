#
# Module manifest for module 'HttpListener'
#

@{
RootModule = 'bin/Debug/net47/HttpListener.dll'
ModuleVersion = '1.0'
GUID = '99ac7788-d9e4-428c-971f-e2fb555e7925'
DefaultCommandPrefix = 'Http'
FunctionsToExport = @()
CmdletsToExport = @(
    "Start-Listener",
    "Read-Request",
    "Write-Response",
    "Stop-Listener"
)
VariablesToExport = @()
AliasesToExport = @()
ScriptsToProcess = @(
    "build.ps1"
)
}
