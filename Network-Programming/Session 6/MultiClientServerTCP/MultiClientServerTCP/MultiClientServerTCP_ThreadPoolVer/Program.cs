using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace MultiClientServerTCP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Multi-Client Server - Using ThreadPool ===");
            Socket sckServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sckServer.Bind(new IPEndPoint(IPAddress.Any, 12345));
            sckServer.Listen(10);
            Console.WriteLine("Server is running ...");
            while (true)
            {
                Socket sckClient = sckServer.Accept();
                ConnectionHandlerV2 handler = new ConnectionHandlerV2(sckClient);
                ThreadPool.QueueUserWorkItem(new WaitCallback(handler.Run));
            }
            
        }
    }
}
