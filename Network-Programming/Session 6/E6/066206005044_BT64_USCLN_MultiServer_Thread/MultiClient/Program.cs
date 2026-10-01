using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MultiClient
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("=== TCP MultiClient ===");
            byte[] data = new byte[1024];
            while (true)
            {
                Console.Write("Enter the number connections to create: ");
                int n = int.Parse(Console.ReadLine().Trim());
                for (int i = 0; i < n; i++)
                {
                    Socket sckClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    sckClient.Connect(new IPEndPoint(IPAddress.Loopback, 12345));
                    int a = 100 + i;
                    int b = 50 + i;
                    string request = a + " " + b;
                    sckClient.Send(Encoding.ASCII.GetBytes(request));
                    int size = sckClient.Receive(data);
                    string response = Encoding.ASCII.GetString(data, 0, size);
                    Console.WriteLine("Connection " + (i + 1) + "/" + n + " | " + request + " => " + response);
                    sckClient.Close();
                }
            }
        }
    }
}
