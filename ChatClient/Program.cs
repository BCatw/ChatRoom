using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ChatClient
{
    class Program
    {
        static void Main(string[] args)
        {
            const string hostIP = "127.0.0.1";
            const int port = 4099;

            Console.WriteLine("============");
            var client = new TcpClient();

            try
            {
                Console.WriteLine($"Connect to server {hostIP}:{port}");
                client.Connect(hostIP, port);

                if (!client.Connected)
                {
                    Console.WriteLine("Connect faile");
                    return;
                }

                Console.WriteLine("Connect success");
            }
            catch(ArgumentNullException e)
            {
                Console.WriteLine($"ArgumentNullException: {e}");
            }
            catch(SocketException e)
            {
                Console.WriteLine($"SocketException: {e}");
            }
            finally
            {
                client.Close();
                Console.WriteLine("Disconnected");
            }
        }
    }
}
