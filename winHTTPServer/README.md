## winHTTPServer

Sometimes we need a HTTP Server on windows servers similar to the python `SimpleHTTPServer`. In this case however we would need administrative rights to make it use the HTTP library.

1.Move the application winHTTPServer.exe to the folder that you want to host a directory listing.
2. Execute the file as an administrator
3. This will expose the contents to a remote system over port 8080

```
C:\winHTTPServer\winHTTPServer\bin\x64\Release>winHTTPServer.exe
Web server running at http://+:8080/ serving files from C:\winHTTPServer\winHTTPServer\bin\x64\Release
Client connected from 127.0.0.1
Client connected from 127.0.0.1
Client connected from 127.0.0.1

```
