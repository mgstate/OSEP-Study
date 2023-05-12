using System;
using System.IO.Pipes;
using System.Net;
using System.Text;

class Dream
{
    static void Main(string[] args)
    {
        string banner = "This is a Dream";


        Console.WriteLine(banner);


        if (args.Length < 1)
        {
            Console.WriteLine("Please specify the IP address where Pipe is setup!.");
            return;
        }

        string ipAddressString = args[0];
        if (!IPAddress.TryParse(ipAddressString, out IPAddress ipAddress))
        {
            Console.WriteLine($"Invalid IP address specified: {ipAddressString}");
            return;
        }

        var output_namedpipe = new NamedPipeClientStream(ipAddressString, "outpipe", PipeDirection.InOut);
        var input_namedpipe = new NamedPipeClientStream(ipAddressString, "inpipe", PipeDirection.InOut);

        Console.WriteLine("namedpipe client started.");

        output_namedpipe.Connect();
        Console.WriteLine("Connected to output namedpipe.");

        input_namedpipe.Connect();
        Console.WriteLine("Connected to input namedpipe.");

        while (true)
        {
            Console.Write("]>: ");
            string command = Console.ReadLine();

            if (string.IsNullOrEmpty(command))
            {
                Console.WriteLine("Empty command received. Skipping...");
                continue;
            }

            if (string.IsNullOrWhiteSpace(command))
            {
                Console.WriteLine("Empty command received. Skipping...");
                continue;
            }

            byte[] buffer = Encoding.ASCII.GetBytes(command);
            input_namedpipe.Write(buffer, 0, buffer.Length);

            byte[] outputBuffer = new byte[4096];
            int totalBytesRead = 0;
            int bytesRead;
            while ((bytesRead = output_namedpipe.Read(outputBuffer, totalBytesRead, outputBuffer.Length - totalBytesRead)) > 0)
            {
                totalBytesRead += bytesRead;

                // Check for null byte
                if (Array.IndexOf(outputBuffer, (byte)0) >= 0)
                {
                    break;
                }
            }

            string output = Encoding.ASCII.GetString(outputBuffer, 0, totalBytesRead);

            // Truncate at null byte
            int nullIndex = output.IndexOf('\0');
            if (nullIndex >= 0)
            {
                output = output.Substring(0, nullIndex);
            }

            output = output.TrimEnd(); // Trim trailing whitespace
            Console.WriteLine(output);
        }
    }
}

