using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net; //khai bao namespace cho lap trinh socket
using System.Net.Sockets;//...

namespace TCPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("----TCP Server----");
            //Tao socket
            Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //Gan ket socket den port
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 12345);
            sck.Bind(ep);
            //Tao hang doi
            sck.Listen(5);
            //Cho Client ket noi den
            Console.Write("Dang cho ket noi tu Client...");
            Socket sckClient = sck.Accept();
            Console.WriteLine("Ket noi thanh cong!");
            //Goi du lieu
            byte[] data = Encoding.ASCII.GetBytes("Chat Server phien ban 1.0");
            sckClient.Send(data);//luu y sckClient dung de gui nhan du lieu con
                                 // sck dung de cho ket noi den
            //////////////////////////////////////
            data = new byte[1024]; //buffer nhan 1KB
            while (true)
            {
                //Server nhan message tu client
                int size = sckClient.Receive(data);
                Console.WriteLine("Client: " + Encoding.ASCII.GetString(data, 0, size));
                //Server gui message cho client
                Console.Write("Server: ");
                string s = Console.ReadLine();//Nhap chuoi tu ban phim
                byte[] tmp = Encoding.ASCII.GetBytes(s);// doi thanh mang byte[]
                sckClient.Send(tmp);
                //hoac 
                // sckClient.Send(Encoding.ASCII.GetBytes(Console.ReadLine()));
            }
            /////////////////////////////////////
            //Dong socket
            sck.Close();
            sckClient.Close();
            Console.WriteLine("Ket thuc thanh cong!");
            Console.ReadLine();
        }
    }
}
