$local:cs = Get-ChildItem -Path $PSScriptRoot -Filter "*.cs" | Select-Object -ExpandProperty "FullName"
$local:module = Add-Type -Path $local:cs -PassThru -ErrorAction Stop | Select-Object -ExpandProperty "Assembly" -Unique
Import-Module $local:module -Prefix "Http" -ErrorAction Stop 
