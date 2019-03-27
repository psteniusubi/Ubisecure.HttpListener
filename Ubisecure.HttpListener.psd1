#
# Module manifest for module 'Ubisecure.HttpListener'
#

@{
RootModule = "Ubisecure.HttpListener.psm1"
ModuleVersion = '1.0.0'
GUID = 'f94e8814-3091-4ee4-bb63-660ae73471ba'
Author = 'petteri.stenius@ubisecure.com'
Description = 'Simple Http Listener'
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
