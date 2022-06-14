using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            const int port = 4099;

            Console.WriteLine("============");
            var server = new TcpListener(IPAddress.Any, port);

            try
            {
                Console.WriteLine("Server Start");
                server.Start();

                Console.WriteLine("Waiting for client...");
                var client = server.AcceptTcpClient();

                var address = client.Client.RemoteEndPoint.ToString();
                Console.WriteLine($"Client has connect {address}");

                client.Close();
                Console.WriteLine("Client disconnected");
            
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketException: {e}");
            }
            finally
            {
                server.Stop();
                Console.WriteLine("Server shut down");
            }
        }
    }
}
