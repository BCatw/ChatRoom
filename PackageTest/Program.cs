using System;
using System.Text;

namespace PackageTest
{
    class Program
    {
        private static byte[] package;
        private static uint posit;
        private static int doubleSize = 8;
        private static int intSize = 4;

        static void Main(string[] args)
        {
            package = new byte[1024];
            posit = 0;

            Write((int)posit);
            Write(123.4f);
            Write(321);
            Write("HeyYouMr.Q");
            CoverPackageContentFrom(BitConverter.GetBytes((int)posit-intSize), 0);
            
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
            byte[] bytesData = Encoding.ASCII.GetBytes(value);
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

        static void CoverPackageContentFrom(byte[] newData, int startIndex)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(newData);
            }

            for (int i = 0; i < newData.Length; i++)
            {
                package[startIndex + i] = newData[i];
            }
        }

        static string Read(byte[] byteData)
        {
            string reuslt = "";
            int readPosit = 0;

            reuslt += "\nSize: " + ReadIntFrom(byteData, readPosit) + " (not include size data) \n";
            readPosit += intSize;

            reuslt += ReadFloatFrom(byteData, readPosit) + ", ";
            readPosit += doubleSize; 

            reuslt += ReadIntFrom(byteData, readPosit) + ", ";
            readPosit += intSize;

            int stringLength = (int)(posit - readPosit);
            reuslt += ReadStringFrom(byteData, readPosit,stringLength);

            return reuslt;
        }

        static float ReadFloatFrom(byte[] data, int startIndex)
        {
            byte[] readPart = TakeByteArrayPart(data, startIndex, doubleSize);
            return (float)BitConverter.ToDouble(readPart, 0);
        }

        static string ReadStringFrom(byte[] data, int startIndex, int length)
        {
            byte[] readPart = TakeByteArrayPart(data, startIndex, length);
            return Encoding.ASCII.GetString(readPart);
        }

        static int ReadIntFrom(byte[] data, int startIndex)
        {
            byte[] readPart = TakeByteArrayPart(data, startIndex, intSize);
            return BitConverter.ToInt32(readPart, 0);
        }

        static byte[] TakeByteArrayPart(byte[] origin, int startIndex, int length)
        {
            byte[] bytes = new byte[length];
            for(int i = 0; i<bytes.Length; i++)
            {
                bytes[i] = origin[startIndex + i];
            }

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return bytes;
        }
    }
}
