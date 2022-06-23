using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections.Generic;

namespace ChatCore
{
    public class ChatServer
    {
        private int port;
        private TcpListener listener;
        private Thread handlerThread;
        private readonly Dictionary<string, UdpClient> clients = new Dictionary<string, UdpClient>();
        private readonly Dictionary<string, string> userNames = new Dictionary<string, string>();

        public ChatServer()
        { }

        public void Bind(int p)
        {
            port = p;
            listener = new TcpListener(IPAddress.Any, port);
            Console.WriteLine($"Server start, port:{port}");
            listener.Start();
        }

        public void Start()
        {
            handlerThread = new Thread(ClientsHandler);
            handlerThread.Start();
            
            /*
            while (true)
            {
                Console.WriteLine("Waiting for client......");
                var c = listener.AcceptTcpClient();

                var clientID = c.Client.RemoteEndPoint.ToString();
                Console.WriteLine($"Client connected from [{clientID}]");

                lock (clients)
                {
                    clients.Add(clientID, c);
                    userNames.Add(clientID, "Unknown");
                }
            }
            */
        }

        public void ClientsHandler()
        {
            while (true)
            {
                var disconnectedClients = new List<string>();

                lock (clients)
                {
                    foreach (string id in clients.Keys)
                    {
                        var c = clients[id];

                        try
                        {
                            //if (!c.Connected) disconnectedClients.Add(id);
                            if (c.Available > 0) RecieveMessage(id);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Client [{id}] exception: {e}");
                        }
                    }
                    //foreach (string id in clients.Keys)
                    //{
                    //    RemoveClient(id);
                    //}
                }
            }
        }
        

        private void RecieveMessage(string clientID)
        {
            var c = clients[clientID];
            //var stream = c.GetStream();

            var numbytes = c.Available;
            var buffer = new byte[numbytes];
            //var bytesRead = stream.Read(buffer, 0, numbytes);
            var request = System.Text.Encoding.ASCII.GetString(buffer).Substring(0, bytesRead);

            if (request.StartsWith("LOGIN:", StringComparison.OrdinalIgnoreCase))
            {
                var tokens = request.Split(':');
                userNames[clientID] = tokens[1];
                Console.WriteLine($"Client {userNames[clientID]} login from {clientID}");
                return;
            }
            
            if(request.StartsWith("MESSAGE:", StringComparison.OrdinalIgnoreCase))
            {
                string[] tokens = request.Split(':');
                string message = tokens[1];
                Console.WriteLine($"{userNames[clientID]} says:[ {message} ]");
                //Boardcast(clientID, message);
                return;
            }
        }

        private void Boardcast(string senderID, string message)
        {
            string data = $"MESSAGE:{userNames[senderID]}:{message}";
            var buffer = System.Text.Encoding.ASCII.GetBytes(data);

            foreach(var clientID in clients.Keys)
            {
                if(clientID!= senderID)
                {
                    try
                    {
                    
                            //.Write(buffer, 0, buffer.Length);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine($"Client {userNames[clientID]} Send Failed, Excetption: {e.Message}");
                    }
                }
            }
        }
    }
}
