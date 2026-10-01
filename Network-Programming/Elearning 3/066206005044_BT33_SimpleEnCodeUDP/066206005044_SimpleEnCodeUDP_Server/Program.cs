using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace _066206005044_SimpleEnCodeUDP_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("MSSV: 066206005044 - BT3.3 - UDP Server EnCode/DeCode");
            Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 22334);
            sck.Bind(ep);
            Console.WriteLine("Server dang lang nghe cong 22334");
            byte[] data = new byte[1024];
            EndPoint epClient = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                int size = sck.ReceiveFrom(data, ref epClient);
                string request = Encoding.UTF8.GetString(data, 0, size).Trim();
                string respond = ProcessRequest(request);
                sck.SendTo(Encoding.UTF8.GetBytes(respond), epClient);
                Console.WriteLine("Nhan tu " + epClient.ToString() + ": " + request + " => " + respond);
            }
        }

        static string ProcessRequest(string request)
        {
            string[] parts = request.Split(new char[] { ';' }, 3);
            if (parts.Length != 3)
            {
                return "Loi dinh dang du lieu";
            }

            int mode;
            int n;
            if (!int.TryParse(parts[0], out mode) || (mode != 0 && mode != 1))
            {
                return "Mode khong hop le";
            }

            if (!int.TryParse(parts[1], out n) || n < 0 || n > 5)
            {
                return "N khong hop le";
            }

            return ShiftAscii(parts[2], mode, n);
        }

        static string ShiftAscii(string input, int mode, int n)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                int code = (int)input[i];
                if (mode == 0)
                {
                    code = code + n;
                }
                else
                {
                    code = code - n;
                }

                result.Append((char)code);
            }

            return result.ToString();
        }
    }
}
