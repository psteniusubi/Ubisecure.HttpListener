if($false) {
    $local:module = New-Module -Name "HttpListener" -ScriptBlock {
        $local:cs = Get-ChildItem -Path $PSScriptRoot -Filter "*.cs" | Select-Object -ExpandProperty "FullName" | Sort-Object
        $local:dll = Add-Type -Path $local:cs -PassThru -ErrorAction Stop | Select-Object -ExpandProperty "Assembly" -Unique
        Import-Module -Assembly $local:dll -Prefix "Http" -Cmdlet "Start-Listener","Read-Request","Write-Response","Stop-Listener" -ErrorAction Stop 
    }
    Import-Module $local:module 
} else {
    $local:cs = Get-ChildItem -Path $PSScriptRoot -Filter "*.cs" | Select-Object -ExpandProperty "FullName" | Sort-Object
    $local:dll = Add-Type -Path $local:cs -PassThru -ErrorAction Stop | Select-Object -ExpandProperty "Assembly" -Unique
    Import-Module -Assembly $local:dll -Prefix "Http" -Cmdlet "Start-Listener","Read-Request","Write-Response","Stop-Listener" -ErrorAction Stop 
}
