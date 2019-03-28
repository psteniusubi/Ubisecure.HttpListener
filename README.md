# HttpListener

PowerShell bindings for System.Net.HttpListener

Used by [Ubisecure.OAuth2](../Ubisecure.OAuth2)

These commands are intendend to support *Loopback Interface Redirection* as defined in *OAuth 2.0 for Native Apps*

https://tools.ietf.org/html/draft-ietf-oauth-native-apps-12#section-7.3

## Example use

```powershell

$listener = Start-HttpListener -Prefix "http://localhost/application/redirect/" -RandomPort

$authorizationrequest = "$($authorizationrequest)&redirect_uri=$($listener.Prefix)"

$html = "<p>Operation completed</p>"

$request = $listener | Read-HttpRequest | Write-HttpResponse -Body $html -Stop -PassThru

$authorizationcode = $request.Query["code"]

```
