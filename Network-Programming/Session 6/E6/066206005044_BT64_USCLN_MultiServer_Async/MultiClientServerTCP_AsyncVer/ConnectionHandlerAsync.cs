using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MultiClientServerTCP_AsyncVer
{
    public class ConnectionHandlerAsync
    {
        private static int NumberOfConnections = 0;
        private Socket sck;
        public ConnectionHandlerAsync(Socket s)
        {
            sck = s;
        }

        public async Task Run()
        {
            int activeConnections = Interlocked.Increment(ref NumberOfConnections);
            Console.WriteLine("Client connected. Number of active connections: " + activeConnections);
            byte[] data = new byte[1024];
            try
            {
                while (true)
                {
                    int size = await ReceiveAsync(data);
                    if (size <= 0)
                    {
                        break;
                    }

                    string request = Encoding.ASCII.GetString(data, 0, size).Trim();
                    if (request.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                    {
                        await SendAsync(Encoding.ASCII.GetBytes("EXIT"));
                        break;
                    }

                    string response = ProcessRequest(request);
                    await SendAsync(Encoding.ASCII.GetBytes(response));
                }
            }
            finally
            {
                sck.Close();
                activeConnections = Interlocked.Decrement(ref NumberOfConnections);
                Console.WriteLine("Client disconnected. Number of active connections: " + activeConnections);
            }
        }

        private Task<int> ReceiveAsync(byte[] buffer)
        {
            return Task<int>.Factory.FromAsync(
                (callback, state) => sck.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, callback, state),
                sck.EndReceive,
                null);
        }

        private Task<int> SendAsync(byte[] buffer)
        {
            return Task<int>.Factory.FromAsync(
                (callback, state) => sck.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, callback, state),
                sck.EndSend,
                null);
        }

        private string ProcessRequest(string request)
        {
            string[] parts = request.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                return "Invalid input";
            }

            int a;
            int b;
            if (!int.TryParse(parts[0], out a) || !int.TryParse(parts[1], out b) || a <= 0 || b <= 0)
            {
                return "Invalid input";
            }

            return FindGcd(a, b).ToString();
        }

        private int FindGcd(int a, int b)
        {
            while (b != 0)
            {
                int r = a % b;
                a = b;
                b = r;
            }

            return a;

        }
    }
}
