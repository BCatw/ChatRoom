using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("============");
            int port = 4099;

            var server = new ChatCore.ChatServer();
            server.Bind(port);
            server.Start();
        }

        #region oldway
        //    static HashSet<TcpClient> clients = new HashSet<TcpClient>();

        //    static void Main(string[] args)
        //    {
        //        const int port = 4099;

        //        Console.WriteLine("============");
        //        var server = new TcpListener(IPAddress.Any, port);

        //        var messageHandler = new Thread(HandleMessage);

        //        try
        //        {
        //            Console.WriteLine("Server Start");
        //            server.Start();

        //            Console.WriteLine("Waiting for client...");
        //            var client = server.AcceptTcpClient();

        //            var address = client.Client.RemoteEndPoint.ToString();
        //            Console.WriteLine($"Client has connect {address}");

        //            #region RecieveTest
        //            while (true)
        //            {
        //                Recieve(client);
        //                System.Threading.Thread.Sleep(1000);
        //            }
        //            #endregion

        //            client.Close();
        //            Console.WriteLine("Client disconnected");

        //        }
        //        catch (SocketException e)
        //        {
        //            Console.WriteLine($"SocketException: {e}");
        //        }
        //        finally
        //        {
        //            server.Stop();
        //            Console.WriteLine("Server shut down");
        //        }
        //    }

        //    private static void Recieve(TcpClient client)
        //    {
        //        var stream = client.GetStream();

        //        var numByte = client.Available;

        //        if (numByte == 0)
        //        {
        //            return;
        //        }
        //        var buffer = new byte[numByte];
        //        var bytesRead = stream.Read(buffer, 0, numByte);

        //        var request = System.Text.Encoding.ASCII.GetString(buffer).Substring(0, bytesRead);
        //        Console.WriteLine($"Recieve text: {request}");
        //    }

        //    private static void HandleMessage()
        //    {
        //        while (true)
        //        {
        //            lock (clients)
        //            {
        //                foreach(var client in clients)
        //                {

        //                }
        //            }
        //        }
        //    }
        #endregion
    }
}
