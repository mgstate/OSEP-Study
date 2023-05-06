using System;
using System.IO;
using System.IO.Compression;
using System.Text;

class Program
{
    static void Main(string[] args)
    {


        if (args.Length != 1 || !File.Exists(args[0]))
        {
            Console.WriteLine("[+] Usage: BasicZipper.exe <filepath>");
            return;
        }

        string scriptFilePath = args[0];
        string scriptContent = File.ReadAllText(scriptFilePath);
        byte[] scriptBytes = Encoding.UTF8.GetBytes(scriptContent);
        byte[] compressedBytes;
        using (var memoryStream = new MemoryStream())
        {
            using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
            {
                gzipStream.Write(scriptBytes, 0, scriptBytes.Length);
            }
            compressedBytes = memoryStream.ToArray();
        }

        string base64String = Convert.ToBase64String(compressedBytes);
        string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "base64encodedGzip.txt");
        File.WriteAllText(outputPath, base64String);
        Console.WriteLine("Output saved to: " + outputPath);
    }
}
