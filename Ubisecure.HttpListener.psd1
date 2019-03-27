#
# Module manifest for module 'HttpListener'
#

@{
RootModule = "HttpListener.psm1"
ModuleVersion = '1.0'
GUID = '99ac7788-d9e4-428c-971f-e2fb555e7925'
DefaultCommandPrefix = $null
FunctionsToExport = @()
CmdletsToExport = @(
    "Start-HttpListener",
    "Read-HttpRequest",
    "Write-HttpResponse",
    "Stop-HttpListener"
)
VariablesToExport = @()
AliasesToExport = @()
NestedModules = @()
ScriptsToProcess = @()
}
