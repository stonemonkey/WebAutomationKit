$NuGetExe = Join-Path $PSScriptRoot '.nuget\nuget.exe'

# download NuGet.exe if missing
if (-not (Test-Path $NuGetExe)) {
    Write-Host 'Downloading nuget.exe ...'
	$client = New-Object System.Net.WebClient
	$client.DownloadFile('https://dist.nuget.org/win-x86-commandline/v3.3.0/nuget.exe', $NuGetExe)
}

& $NuGetExe pack WebAutomationKit.nuspec