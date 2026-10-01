using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace _066206005044_SimpleEnCodeUDP_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("MSSV: 066206005044 - BT3.3 - UDP Client EnCode/DeCode");
            Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Console.Write("Nhap IP server (mac dinh 127.0.0.1): ");
            string ip = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = "127.0.0.1";
            }

            EndPoint ep = new IPEndPoint(IPAddress.Parse(ip.Trim()), 22334);
            byte[] data = new byte[1024];

            while (true)
            {
                Console.Write("Chon 0-ma hoa, 1-giai ma, EXIT-thoat: ");
                string modeText = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(modeText))
                {
                    Console.WriteLine("Mode khong hop le");
                    continue;
                }

                modeText = modeText.Trim();
                if (modeText.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                if (modeText != "0" && modeText != "1")
                {
                    Console.WriteLine("Mode khong hop le");
                    continue;
                }

                Console.Write("Nhap chuoi S: ");
                string input = Console.ReadLine();
                if (input == null)
                {
                    input = string.Empty;
                }

                Console.Write("Nhap so nguyen N (0..5): ");
                string nText = Console.ReadLine();
                int n;
                if (!int.TryParse(nText, out n) || n < 0 || n > 5)
                {
                    Console.WriteLine("N khong hop le");
                    continue;
                }

                string request = modeText + ";" + n + ";" + input;
                sck.SendTo(Encoding.UTF8.GetBytes(request), ep);
                int size = sck.ReceiveFrom(data, ref ep);
                string response = Encoding.UTF8.GetString(data, 0, size);
                Console.WriteLine("Ket qua tu server: " + response);
            }

            sck.Close();
        }
    }
}
