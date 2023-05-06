using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

class Program
{

    static void BytePrinter(byte[] inputArray)

    {
        int count = 0;
        foreach (byte b in inputArray)
        {
            count++;

            if (count % 15 == 0)
            {

                Console.WriteLine("\r");
            }
            if (b == inputArray.Last())
            {
                Console.Write("0x{0:X2}", b);

            }

            else
            {
                Console.Write("0x{0:X2}, ", b);



            }

        }
        Console.WriteLine();
        
    }


    static void WriteByteArrayToFile(byte[] byteArray, string filename)
    {
        FileStream fileStream = new FileStream(filename, FileMode.Create);
        StreamWriter streamWriter = new StreamWriter(fileStream);


        int count = 0;
        foreach (byte b in byteArray)
        {
            count++;

            if (count % 15 == 0)
            {
                streamWriter.WriteLine();
            }
            if (b == byteArray.Last())
            {
                streamWriter.Write("0x{0:X2}", b);

            }



            else
            {
                streamWriter.Write("0x{0:X2}, ", b);

            }


        }

        streamWriter.Close();
        fileStream.Close();
    }

    static void Main(string[] args)
    {
        byte[] key = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0xFE, 0xDC, 0xBA, 0x98, 0x76, 0x54, 0x32, 0x10 };

        if (args.Length == 0)
        {
            Console.WriteLine("Please provide a filename as input argument.");
            return;
        }
        string filename = args[0];

        if (!File.Exists(filename))
        {
            Console.WriteLine("The specified file does not exist.");
            return;
        }


        string inputString = File.ReadAllText(filename);
        string[] stringArray = inputString.Split(',');

        byte[] byteArray = new byte[stringArray.Length];

        for (int i = 0; i < stringArray.Length; i++)
        {
            byteArray[i] = Convert.ToByte(stringArray[i].Trim(), 16);
        }

        //https[:]//learn.microsoft.com/en-us/dotnet/standard/security/encrypting-data 

        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = new byte[aes.BlockSize / 8];

            ICryptoTransform encryptor = aes.CreateEncryptor();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(byteArray, 0, byteArray.Length);
                }

                byte[] encryptedArray = memoryStream.ToArray();

                Console.WriteLine("Original byte array:");
                BytePrinter(byteArray);

                Console.WriteLine("Encrypted byte array:");
                BytePrinter(encryptedArray);




            ICryptoTransform decryptor = aes.CreateDecryptor();
            using (MemoryStream memoryStreamd = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStreamd, decryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(encryptedArray, 0, encryptedArray.Length);
                }

                byte[] decryptedArray = memoryStreamd.ToArray();

                    Console.WriteLine("decrypted byte array:");

                    BytePrinter(decryptedArray);

                    string ofilename = "output.txt";

                    WriteByteArrayToFile(encryptedArray, ofilename);

                }





            }
            Console.WriteLine();
            }
        }
    }
