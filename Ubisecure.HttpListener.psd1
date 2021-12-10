#
# Module manifest for module "Ubisecure.HttpListener"
#

@{
RootModule = "Ubisecure.HttpListener.psm1"
ModuleVersion = "1.2.0"
GUID = "f94e8814-3091-4ee4-bb63-660ae73471ba"
Author = "petteri.stenius@ubisecure.com"
Description = "Simple Http Listener"
PowerShellVersion = "5.1"
CompatiblePSEditions = "Desktop","Core"
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
