using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("----TCP Client----");
            //Tao socket
            Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // Ket noi den server
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            sck.Connect(ep);
            Console.WriteLine("Ket noi thanh cong!");
            //Nhan du lieu
            byte[] data = new byte[1024]; //tao buffer de nhan
            int size = sck.Receive(data);
            //Doi thanh kieu string
            string s = Encoding.ASCII.GetString(data, 0, size);
            Console.WriteLine(s);
            ////////////////////////////////////////
            while(true)
            {
                //Client gui message den server
                Console.Write("Client: ");
                sck.Send(Encoding.ASCII.GetBytes(Console.ReadLine()));
                //Client nhan message tu server
                size = sck.Receive(data);
                Console.WriteLine("Server: " + Encoding.ASCII.GetString(data, 0, size));
            }
            ///////////////////////////////////////
            sck.Close();
            Console.WriteLine("Ket thuc thanh cong!");
            Console.ReadLine();
        }
    }
}
