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
            while (true)
            {
                Console.Write("Enter the number connections to create: ");
                int n = int.Parse(Console.ReadLine().Trim());
                for (int i = 0; i < n; i++)
                {
                    Socket sckClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    sckClient.Connect(new IPEndPoint(IPAddress.Loopback, 12345));
                    sckClient.Send(Encoding.ASCII.GetBytes("You have been hacked!"));
                    Console.WriteLine("Created Connection number " + i + "/" + n);
                }
            }
            //Console.ReadLine();
        }
    }
}
