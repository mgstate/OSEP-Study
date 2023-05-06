# ATTACKVMS

Setting up AttackVM's locally based on ZeroPoint Security RTOVM scripts https[:]//github.com/ZeroPointSecurity/RTOVMSetup 

## Windows

<!----

This script is based on https://github.com/ZeroPointSecurity/RTOVMSetup but modified for working with OSEP with local windows dev

--->

### 1. Install Boxstarter
```
. { Invoke-WebRequest -useb https://boxstarter.org/bootstrapper.ps1 } | iex; Get-Boxstarter -Force
```

### 2. Install Boxstarter Package
```
$Cred = Get-Credential $env:USERNAME
Install-BoxstarterPackage -PackageName https://raw.githubusercontent.com/EvilEnigma/OSEP-Study/master/windows-choco.choco -Credential $Cred
```

## Kali
```
kali@kali:~$ curl -sS https://raw.githubusercontent.com/EvilEnigma/OSEP-Study/master/AttackVMS/AttackVMS/kaliprep.sh | sudo bash -
```
