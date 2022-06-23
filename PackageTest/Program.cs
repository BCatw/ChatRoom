using System;

namespace PackageTest
{
    class Program
    {
        private static byte[] package;
        private static uint posit;


        static void Main(string[] args)
        {
            package = new byte[1024];
            posit = 0;

            Write(123.4f);
            Write(321);
            Write("HeyYouMr.Q");

            Console.WriteLine($"Output Byte array (length = {posit}): ");
            for(int i = 0; i < posit; i++)
            {
                Console.Write($"{package[i]}, ");
            }
        }

        static bool Write(float value)
        {
            byte[] bytesData = BitConverter.GetBytes(value);
            WriteInPackage(bytesData);

            return true;
        }

        static bool Write(int value)
        {
            byte[] bytesData = BitConverter.GetBytes(value);
            WriteInPackage(bytesData);

            return true;
        }

        static bool Write(string value)
        {
            byte[] bytesData = System.Text.Encoding.Unicode.GetBytes(value);
            WriteInPackage(bytesData);
            
            if(Write(bytesData.Length) == false)
            {
                return false;
            }

            WriteInPackage(bytesData);
            return true;
        }

        static void WriteInPackage(byte[] byteData)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(byteData);
            }

            byteData.CopyTo(package, posit);
            posit += (uint)byteData.Length;
        }
    }
}
