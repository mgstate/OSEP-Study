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
            Console.WriteLine("Usage: BasicUnzipper C:\\<filepath\\ of\\ the\\ script>");
            return;
        }

        // Get the path of the input file from the command-line argument
        string inputFilePath = args[0];

        // Read the contents of the input file as a string
        string base64String = File.ReadAllText(inputFilePath);

        // Convert the base64 string to a byte array
        byte[] compressedBytes = Convert.FromBase64String(base64String);

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

        // Convert the decompressed bytes to a string
        string scriptContent = Encoding.UTF8.GetString(scriptBytes);

        // Write the script content to a text file in the execution folder
        string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cleartextfile.txt");
        File.WriteAllText(outputPath, scriptContent);

        Console.WriteLine("Output saved to: " + outputPath);
    }
}
