$ErrorActionPreference = 'Stop';
$toolsDir   = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$pkgName    = "KS"
$url        = "https://github.com/EoflaOE/Kernel-Simulator/releases/download/v0.0.8.11-alpha/0.0.8.11-bin.rar"
$md5check   = "9c27710fd5107370160fb254cc1ca98a"

Write-Output "<*>: for assumptions, <+> for progress, <-> for error"
Write-Output "<*> Installation directory: $toolsDir"
Write-Output "<*> Package Name: $pkgName"
Write-Output "<*> URL: $url"
Write-Output "<*> Expected MD5 Sum: $md5check"
Write-Output "<+> Configuration will be automatically generated on startup."

Install-ChocolateyZipPackage $pkgName $url $toolsDir -ChecksumType "md5" -Checksum $md5check