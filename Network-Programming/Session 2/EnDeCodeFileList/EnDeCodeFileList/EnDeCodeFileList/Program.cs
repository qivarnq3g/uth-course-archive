using System;

namespace EnDeCodeFileList
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string rawPath;
            if (args != null && args.Length > 0)
            {
                rawPath = args[0];
            }
            else
            {
                Console.Write("Nhập đường dẫn thư mục P: ");
                rawPath = Console.ReadLine();
            }

            string path = (rawPath ?? string.Empty).Trim();

            try
            {
                if (path == ".")
                {
                    path = System.IO.Directory.GetCurrentDirectory();
                }

                path = System.IO.Path.GetFullPath(path);
                string encoded = EnDeCodeDirInfo.encode(path);
                Console.WriteLine("Chuỗi mã hoá:");
                Console.WriteLine(encoded);
                Console.WriteLine();
                Console.WriteLine("Giải mã:");
                Console.WriteLine(EnDeCodeDirInfo.decode(encoded));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
        }
    }
}
