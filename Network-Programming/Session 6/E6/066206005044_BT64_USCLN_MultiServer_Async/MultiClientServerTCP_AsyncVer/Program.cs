using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace MultiClientServerTCP_AsyncVer
{
    class Program
    {
        static void Main(string[] args)
        {
            RunServer().GetAwaiter().GetResult();
        }

        private static async Task RunServer()
        {
            Console.WriteLine("=== Multi-Client Server - Using Async ===");
            Socket sckServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sckServer.Bind(new IPEndPoint(IPAddress.Any, 12345));
            sckServer.Listen(10);
            Console.WriteLine("Server is running ...");

            while (true)
            {
                Socket sckClient = await Task<Socket>.Factory.FromAsync(sckServer.BeginAccept, sckServer.EndAccept, null);
                ConnectionHandlerAsync handler = new ConnectionHandlerAsync(sckClient);
                Task.Run(async () => await handler.Run());
            }
        }
    }
}
