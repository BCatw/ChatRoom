using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace ChatCore
{
    public class ChatClient
    {
        private TcpClient client;
        List<KeyValuePair<string, string>> messageList;

        public ChatClient()
        {
            messageList = new List<KeyValuePair<string, string>>();
        }

        public bool Connect(string address, int port)
        {
            client = new TcpClient();

            try
            {
                Console.WriteLine($"Connexting to chat server {address}:{port}");
                client.Connect(address, port);

                Console.WriteLine("Connected to chat server");
                return client.Connected;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine($"ArgumentNullException {e.Message}");
                return false;
            }
            catch(SocketException e)
            {
                Console.WriteLine($"SocketException {e.Message}");
                return false;
            }
        }

        public void Disconnect()
        {
            client.Close();
            Console.WriteLine("Client disconnect");
        }

        void Refresh()
        {
            if (client.Available > 0)
            {
                HandleRecieveMessage(client);
            }
        }

        void HandleRecieveMessage(TcpClient c)
        {
            var stream = c.GetStream();

            var numbytes = c.Available;
            var buffer = new byte[numbytes];
            var bytesRead = stream.Read(buffer, 0, numbytes);
            var request = System.Text.Encoding.ASCII.GetString(buffer).Substring(0, bytesRead);

            if (request.StartsWith("MESSAGE:", StringComparison.OrdinalIgnoreCase))
            {
                var tokens = request.Split(':');
                var sender = tokens[1];
                var message = tokens[2];
                messageList.Add(new KeyValuePair<string, string>(sender, message));
            }
        }

        public void SetName(string message)
        {
            var data = "LOGIN:" + message;
            SendData(data);
        }

        public void SendMessage(string message)
        {
            var data = "MESSAGE:" + message;
            SendData(data);
        }

        public void SendData(string message)
        {
            var requestBuffer = System.Text.Encoding.ASCII.GetBytes(message);

            client.GetStream().Write(requestBuffer, 0, requestBuffer.Length);
        }
    }
}
