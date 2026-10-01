using System;
using System.Text;

namespace BaitapUSCLN
{
    class Program
    {
        static string readDomainName(string[] args)
        {
            if (args.Length > 0)
            {
                return args[0];
            }

            Console.Write("Nhập tên miền: ");
            return Console.ReadLine() ?? string.Empty;
        }

        static void printDomainInfo(System.Net.IPHostEntry domainInfo)
        {
            Console.WriteLine("\nTên miền: " + domainInfo.HostName);
            Console.WriteLine("Các địa chỉ IP:");

            foreach (System.Net.IPAddress ipAddress in domainInfo.AddressList)
            {
                Console.WriteLine(ipAddress.ToString());
            }

            if (domainInfo.Aliases.Length > 0)
            {
                Console.WriteLine("Các bí danh:");
                foreach (string alias in domainInfo.Aliases)
                {
                    Console.WriteLine(alias);
                }
            }
        }

        static void Main(string[] args)
        {
            const string mssv = "066206005044";

            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("MSSV: " + mssv + " - Họ tên SV: Hoàng Việt Quang - Tên bài tập: BT1.4 - DNS Lookup");

            string tenMien = readDomainName(args);

            if (string.IsNullOrWhiteSpace(tenMien))
            {
                Console.WriteLine("Tên miền không hợp lệ!");
                return;
            }

            try
            {
                System.Net.IPHostEntry thongTinTenMien = System.Net.Dns.GetHostEntry(tenMien);
                printDomainInfo(thongTinTenMien);
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                Console.WriteLine("Lỗi: Không tìm thấy tên miền '" + tenMien + "'");
                Console.WriteLine("Chi tiết: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }

            if (!Console.IsInputRedirected)
            {
                Console.ReadKey();
            }
        }
    }
}
