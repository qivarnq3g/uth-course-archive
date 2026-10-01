using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TCPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== TCP Client ===");
            Socket sckClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sckClient.Connect(new IPEndPoint(IPAddress.Loopback, 12345));
            byte[] data = new byte[1024];
            int size;
            while (true)
            {
                Console.Write("Enter a message for Server: ");
                string s = Console.ReadLine();
                sckClient.Send(Encoding.ASCII.GetBytes(s));
                size = sckClient.Receive(data);
                s = Encoding.ASCII.GetString(data, 0, size);
                if (s == "EXIT") break;
                Console.WriteLine("Server Response: " +s);
            }
        }
    }
}
