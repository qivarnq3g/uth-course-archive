using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;//
namespace MyDNS
{
    class Program
    {
        static void Main(string[] args)
        {
            string s;
            if (args.Length > 0)
                s = args[0];
            else
            {
                Console.Write("Nhap ten mien: ");
                s= Console.ReadLine();
            }
            IPHostEntry entry = Dns.GetHostEntry(s);
            foreach(IPAddress add in entry.AddressList)
                System.Console.WriteLine(add.ToString());
            Console.ReadLine();
        }
    }
}
