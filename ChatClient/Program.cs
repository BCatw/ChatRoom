using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using ChatCore;

namespace ChatClient
{
    class Program
    {
        static void Main(string[] args)
        {
            const string hostIP = "127.0.0.1";
            const int port = 4099;

            Console.WriteLine("============");
            var client = new ChatCore.ChatClient();

            Console.WriteLine("<Please enter your name...>");
            var name = Console.ReadLine();

            bool succeed = client.Connect(hostIP, port);

            if (!succeed)
            {
                Console.WriteLine("Connect faile");
                return;
            }

            client.SetName(name);
            Console.WriteLine("Connect success");

            Console.WriteLine("Plaese erter message...");

            while (true)
            {
                string message = Console.ReadLine();

                if (message == "exit")
                {
                    Console.WriteLine("<Bye...>");
                    client.Disconnect();
                    break;
                }

                client.SendMessage(message);
                Console.WriteLine($"<Message sent>");

            }
        }
    }
}