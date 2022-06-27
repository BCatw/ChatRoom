using System;
using System.Text;
using System.IO;

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
            Console.WriteLine("");
            Console.WriteLine($"Read result: {Read(package)}");
        }

        static bool Write(double value)
        {
            byte[] bytesData = BitConverter.GetBytes(value);
            WriteIntoPackage(bytesData);

            return true;
        }

        static bool Write(int value)
        {
            byte[] bytesData = BitConverter.GetBytes(value);
            WriteIntoPackage(bytesData);

            return true;
        }

        static bool Write(string value)
        {
            byte[] bytesData = System.Text.Encoding.ASCII.GetBytes(value);
            if(Write(bytesData.Length) == false)
            {
                return false;
            }

            WriteIntoPackage(bytesData);
            return true;
        }

        static void WriteIntoPackage(byte[] byteData)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(byteData);
            }

            byteData.CopyTo(package, posit);
            posit += (uint)byteData.Length;
        }

        static string Read(byte[] byteData)
        {
            string reuslt = "";

            int readPosit = 0;

            byte[] floatPart = TakeByteArrayPart(byteData, readPosit, 8);
            readPosit += floatPart.Length;

            byte[] intPart = TakeByteArrayPart(byteData, readPosit, 4);
            readPosit += intPart.Length;

            byte[] stringPart = TakeByteArrayPart(byteData, readPosit, (int)(posit - readPosit));

            reuslt += ReadFloat(floatPart) + ", ";
            reuslt += ReadInt(intPart) + ", ";
            reuslt += ReadString(stringPart);

            return reuslt;
        }

        static float ReadFloat(byte[] data)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
            }

            return (float)BitConverter.ToDouble(data, 0);
        }

        static string ReadString(byte[] data)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
            }

            return Encoding.ASCII.GetString(data);
        }

        static int ReadInt(byte[] data)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
            }

            return BitConverter.ToInt32(data,0);
        }

        static byte[] TakeByteArrayPart(byte[] origin, int startIndex, int length)
        {
            byte[] bytes = new byte[length];
            for(int i = 0; i<bytes.Length; i++)
            {
                bytes[i] = origin[startIndex + i];
            }
            return bytes;
        }
    }
}
