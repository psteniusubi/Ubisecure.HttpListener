$listener = Start-HttpListener -Prefix "http://localhost/application/redirect/" -RandomPort

Start-Process $listener.Prefix | Out-Null

$request = $listener | Read-HttpRequest | Write-HttpResponse -Body "<p>Hello World</p>" -Stop -PassThru

Write-Host "$($request.HttpMethod) $($request.Url)"
