using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace UDPTimeClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=====UDP Time Client=====");
            Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //IPEndPoint cua server
            EndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            //Gui message dau tien den server de yeu cau dich vu
            sck.SendTo(Encoding.ASCII.GetBytes("TIMECLIENT v1.0"), ep);
            byte[] data = new byte[1024];
            //////////////////////////////////////////////
            while (true)
            {
                //Nhap yeu cau tu ban phim
                Console.Write("Nhap yeu cau (1/2/3): ");
                string request = Console.ReadLine();
                //Gui yeu cau den server
                sck.SendTo(Encoding.ASCII.GetBytes(request),ep);
                //Nhan dap ung tu server
                int size = sck.ReceiveFrom(data, ref ep);
                //In noi dung nhan duoc ra man hinh
                Console.WriteLine(Encoding.ASCII.GetString(data, 0, size));
            }
            /////////////////////////////////////////////
            sck.Close();
            Console.ReadLine();
        }
    }
}
