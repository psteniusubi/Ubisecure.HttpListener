$local:cs = Get-ChildItem -Path $PSScriptRoot -Filter "*.cs" | Select-Object -ExpandProperty "FullName" | Sort-Object
$local:dll = Add-Type -Path $local:cs -PassThru -ErrorAction Stop | Select-Object -ExpandProperty "Assembly" -Unique
Import-Module -Assembly $local:dll -Prefix "Http" -Cmdlet "Start-Listener","Read-Request","Write-Response","Stop-Listener" -ErrorAction Stop 
