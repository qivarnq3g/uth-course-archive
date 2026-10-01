using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace FileFolderUDP_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("MSSV: 066206005044 - BT3.2 - UDP Client File/Folder");
            Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Console.Write("Nhap IP server (mac dinh 127.0.0.1): ");
            string ip = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = "127.0.0.1";
            }

            EndPoint ep = new IPEndPoint(IPAddress.Parse(ip.Trim()), 22333);
            byte[] data = new byte[1024];

            while (true)
            {
                Console.Write("Nhap duong dan (EXIT de thoat): ");
                string request = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(request))
                {
                    Console.WriteLine("Duong dan khong duoc rong.");
                    continue;
                }

                if (request.Trim().Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                sck.SendTo(Encoding.UTF8.GetBytes(request), ep);
                int size = sck.ReceiveFrom(data, ref ep);
                string response = Encoding.UTF8.GetString(data, 0, size);

                if (response == "Đường dẫn không hợp lệ")
                {
                    Console.WriteLine(response);
                    continue;
                }

                DecodeAndPrint(response);
            }

            sck.Close();
        }

        static void DecodeAndPrint(string message)
        {
            if (string.IsNullOrWhiteSpace(message) || message[0] != '$' || message[message.Length - 1] != '#')
            {
                Console.WriteLine("Du lieu nhan duoc khong hop le: " + message);
                return;
            }

            string body = message.Substring(1, message.Length - 2);
            string[] parts = body.Split(';');
            int count;
            if (!int.TryParse(parts[0], out count))
            {
                Console.WriteLine("Du lieu nhan duoc khong hop le: " + message);
                return;
            }

            if ((parts.Length - 1) % 2 != 0 || ((parts.Length - 1) / 2) != count)
            {
                Console.WriteLine("Du lieu nhan duoc khong hop le: " + message);
                return;
            }

            Console.WriteLine("Danh sach thu muc:");
            for (int i = 1; i < parts.Length; i += 2)
            {
                if (parts[i + 1] == "0")
                {
                    Console.WriteLine(parts[i].ToUpper());
                }
            }

            Console.WriteLine("Danh sach tap tin:");
            for (int i = 1; i < parts.Length; i += 2)
            {
                if (parts[i + 1] == "1")
                {
                    Console.WriteLine(parts[i].ToLower());
                }
            }
        }
    }
}
