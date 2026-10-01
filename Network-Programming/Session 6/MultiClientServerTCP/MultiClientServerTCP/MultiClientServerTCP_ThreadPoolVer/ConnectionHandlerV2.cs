using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace MultiClientServerTCP
{
    public class ConnectionHandlerV2
    {
        private static int NumberOfConnections = 0;
        private Socket sck;
        public ConnectionHandlerV2(Socket s)
        {
            sck = s;
        }

        public void Run(Object state)//Notice here
        {
            NumberOfConnections++;
            Console.WriteLine("Client connected. Number of active connections: " + NumberOfConnections);
            byte[] data = new byte[1024];
            int size;
            while(true)
            {
                size = sck.Receive(data);
                string request = Encoding.ASCII.GetString(data, 0, size);
                //Convert to uppper case
                string response = request.ToUpper();
                sck.Send(Encoding.ASCII.GetBytes(response));
                if (response == "EXIT") break;
            }
            sck.Close();
            NumberOfConnections--;
            Console.WriteLine("Client disconnected. Number of active connections: " + NumberOfConnections);

        }
    }
}
