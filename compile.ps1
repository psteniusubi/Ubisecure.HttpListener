$local:cs = Get-ChildItem -Path $PSScriptRoot -Filter "*.cs" | Select-Object -ExpandProperty "FullName" | Sort-Object
$local:module = Add-Type -Path $local:cs -PassThru -ErrorAction Stop | Select-Object -ExpandProperty "Assembly" -Unique
Import-Module -Assembly $local:module -Prefix "Http" -Global -ErrorAction Stop 
