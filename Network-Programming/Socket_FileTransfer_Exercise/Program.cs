using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SimpleFileTranferTCP
{
    class Program
    {
        static int BUFFERSIZE = 1024*1000;
        static void Main(string[] args)
        {
            int choice = 0;
            Console.Write("Please choose mode (1) Receive mode, (2) Send mode: ");
            choice = int.Parse(Console.ReadLine());
            if (choice==2)
            {
                SendMode();
            }
            else
            {
                ReceiveMode();
            }

        }

        static void SendMode()
        {
            Console.WriteLine("------ SEND MODE ------");
            string fileName;
            string pathToStore;//On server
            Console.Write("Enter file to send [C:/Tmp/a.jpg]: ");
            fileName = Console.ReadLine();
            if (fileName.Length == 0)
                fileName = "C:/Tmp/a.jpg";
            Console.Write("Path to store on server [C:/Tmp/upload]: ");
            pathToStore = Console.ReadLine();
            if (pathToStore.Length == 0)
                pathToStore = "C:/Tmp/upload";
            Console.Write("Send to IP [127.0.0.1]: ");
            string IP = Console.ReadLine();
            if (IP.Length == 0)
                IP = "127.0.0.1";
            //
            Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IP), 9999);
            sck.Connect(ep);
            Console.WriteLine("Connected! Start sending file "+fileName + " to " + pathToStore +" on Server "+ IP+ "...");
            byte[] data = new byte[1024];
            //Send fileName
            System.IO.FileInfo fi = new System.IO.FileInfo(fileName);
            sck.Send(Encoding.ASCII.GetBytes(fi.Name));
            sck.Receive(data);
            //Send Path
            sck.Send(Encoding.ASCII.GetBytes(pathToStore));
            sck.Receive(data);
            //Send Content
            long numFrames = fi.Length / BUFFERSIZE;
            if (numFrames * BUFFERSIZE < fi.Length)
                numFrames++;
            //Send numFrames
            sck.Send(Encoding.ASCII.GetBytes(numFrames.ToString()));
            sck.Receive(data);
            System.IO.FileStream fs = fi.OpenRead();
            byte[] buffer = new byte[BUFFERSIZE];
            for (long i = 0; i < numFrames; i++ )
            {
                System.Console.WriteLine("Sending frame " + (i + 1) + "/" + numFrames);
                int numBytes = fs.Read(buffer, 0, BUFFERSIZE);
                sck.Send(buffer, numBytes, SocketFlags.None);
                sck.Receive(data);//Receive string "OK"
            }
            fs.Close();
            sck.Close();
            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        static void ReceiveMode()
        {
            Console.WriteLine("------ RECEIVE MODE ------");
            string fileName, pathToStore;
            Socket sckServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 9999);
            sckServer.Bind(ep);
            sckServer.Listen(5);
            Socket sck = sckServer.Accept();
            Console.WriteLine("Connected! Start Receving...");
            byte[] data = new byte[1024];
            //Receive filename
            int n = sck.Receive(data);
            fileName = Encoding.ASCII.GetString(data, 0, n);
            Console.WriteLine("File name: " + fileName);
            sck.Send(Encoding.ASCII.GetBytes("OK"));
            //Receive pathToStore
            n = sck.Receive(data);
            pathToStore = Encoding.ASCII.GetString(data, 0, n);
            Console.WriteLine("Path to store: " + pathToStore);
            sck.Send(Encoding.ASCII.GetBytes("OK"));
            //Receive number of Frames
            n = sck.Receive(data);
            long numberOfFrames = long.Parse(Encoding.ASCII.GetString(data, 0, n));
            Console.WriteLine("Number of Frames: " + numberOfFrames);
            sck.Send(Encoding.ASCII.GetBytes("OK"));
            byte[] buffer = new byte[BUFFERSIZE];
            //Open file to write
            System.IO.FileStream fs = new System.IO.FileStream(pathToStore + "/" + fileName,System.IO.FileMode.Create);
            for (long i = 0;i<numberOfFrames; i++)
            {
                System.Console.WriteLine("Receiving frame " + (i + 1) + "/" + numberOfFrames);
                n = sck.Receive(buffer);
                fs.Write(buffer, 0, n);
                sck.Send(Encoding.ASCII.GetBytes("OK"));
            }
            fs.Close();
            sck.Close();
            sckServer.Close();
            Console.WriteLine("Done.");
            Console.ReadLine();
        }
    }
}
