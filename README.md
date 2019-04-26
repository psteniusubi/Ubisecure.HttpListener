# PowerShell Http Listener

PowerShell bindings for System.Net.HttpListener

Used by [Ubisecure.OAuth2](../../../Ubisecure.OAuth2)

These commands are intendend to support *Loopback Interface Redirection* as defined in *OAuth 2.0 for Native Apps*

https://tools.ietf.org/html/draft-ietf-oauth-native-apps-12#section-7.3

## Install from github.com

Windows

```cmd
cd /d %USERPROFILE%\Documents\WindowsPowerShell\Modules
git clone --recurse-submodules https://github.com/psteniusubi/Ubisecure.HttpListener.git
```

Linux

```bash
cd ~/.local/share/powershell/Modules
git clone --recurse-submodules https://github.com/psteniusubi/Ubisecure.HttpListener.git
```

## Example use

```powershell
$listener = Start-HttpListener -Prefix "http://localhost/application/redirect/" -RandomPort

Start-Process $listener.Prefix | Out-Null

$request = $listener | Read-HttpRequest | Write-HttpResponse -Body "<p>Hello World</p>" -Stop -PassThru

Write-Host "$($request.HttpMethod) $($request.Url)"
```
