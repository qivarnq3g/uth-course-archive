using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace FileFolderUDP_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("MSSV: 066206005044 - BT3.2 - UDP Server File/Folder");
            Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 22333);
            sck.Bind(ep);
            Console.WriteLine("Server dang lang nghe cong 22333");
            byte[] data = new byte[1024];
            EndPoint epClient = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                int size = sck.ReceiveFrom(data, ref epClient);
                string request = Encoding.UTF8.GetString(data, 0, size).Trim();
                string respond = EncodeDirectoryInfo(request);
                sck.SendTo(Encoding.UTF8.GetBytes(respond), epClient);
                Console.WriteLine("Nhan tu " + epClient.ToString() + ": " + request);
            }
        }

        static string EncodeDirectoryInfo(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    return "Đường dẫn không hợp lệ";
                }

                path = path.Trim();
                if (!Directory.Exists(path))
                {
                    return "Đường dẫn không hợp lệ";
                }

                DirectoryInfo dir = new DirectoryInfo(path);
                DirectoryInfo[] dirs = dir.GetDirectories();
                FileInfo[] files = dir.GetFiles();

                StringBuilder result = new StringBuilder();
                result.Append('$');
                result.Append(dirs.Length + files.Length);

                foreach (DirectoryInfo d in dirs)
                {
                    result.Append(';');
                    result.Append(d.Name);
                    result.Append(';');
                    result.Append('0');
                }

                foreach (FileInfo f in files)
                {
                    result.Append(';');
                    result.Append(f.Name);
                    result.Append(';');
                    result.Append('1');
                }

                result.Append('#');
                return result.ToString();
            }
            catch
            {
                return "Đường dẫn không hợp lệ";
            }
        }
    }
}
