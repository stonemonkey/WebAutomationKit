$targetNugetDir = Join-Path $PSScriptRoot '.nuget'
$targetNugetExe = Join-Path $targetNugetDir 'nuget.exe'
$sourceNugetExe = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"

# download NuGet.exe if missing
if (-not (Test-Path $targetNugetExe)) {
	New-Item -ItemType Directory -Force -Path $targetNugetDir
    Write-Host 'Downloading nuget.exe ...'
	# $client = New-Object System.Net.WebClient
	# $client.DownloadFile($sourceNugetExe, $targetNugetExe)
	Invoke-WebRequest $sourceNugetExe -OutFile $targetNugetExe
}

& $targetNugetExe pack WebAutomationKit.nuspec