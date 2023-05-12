## PipeDream ##

A tool that uses namedpipe for communication between client and server. The pipe is the server, dream is the client application. 

You will need to execute Pipe.exe in someway and it will listen on SMB Named pipe, then on a workstation within the same LAN execute dream.exe <IP address> which will allow executing commands were pipe is listening.

Not opsec safe. Just something i used as a learning within the OSEP lab to execute commands between workstations. :D


Setting up the server part is as easy-as

```
C:\PipeDream\Pipe\bin\Release>Pipe.exe
Pipe Server has been created, waiting for a dream .
Waiting for client connection...
Client connected to input namepipe.
Waiting for client connection...
Client connected to output namepipe.
Received command: hostname
Received command: ipconfig
Received command: systeminfo


```

executing commands on the server is as easy-as

```
C:\PipeDream\Dream\bin\x64\Release>Dream.exe 127.0.0.1
This is a Dream
namedpipe client started.
Connected to output namedpipe.
Connected to input namedpipe.
]>: hostname
DESKTOP-CALUSI2
]>: ipconfig
Windows IP Configuration


Ethernet adapter Ethernet0:

   Connection-specific DNS Suffix  . : localdomain
   Link-local IPv6 Address . . . . . : fe80::7296:5231:a507:143%8
   IPv4 Address. . . . . . . . . . . : 192.168.19.136
   Subnet Mask . . . . . . . . . . . : 255.255.255.0
   Default Gateway . . . . . . . . . : 192.168.19.2
]>: systeminfo
Host Name:                 DESKTOP-CALUSI2
OS Name:                   Microsoft Windows 10 Pro
OS Version:                10.0.19045 N/A Build 19045
OS Manufacturer:           Microsoft Corporation
<redacted>
]>:
```
