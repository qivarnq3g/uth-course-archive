using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MultiServerTCP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Multi-Client Server ===");
            Socket sckServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sckServer.Bind(new IPEndPoint(IPAddress.Any, 12345));
            sckServer.Listen(10);
            Console.WriteLine("Server is running ...");
            while (true)
            {
                Socket sckClient = sckServer.Accept();
                ConnectionHandler handler = new ConnectionHandler(sckClient);
                Thread th = new Thread(new ThreadStart(handler.Run));
                th.Start();
            }
        }
    }
}
