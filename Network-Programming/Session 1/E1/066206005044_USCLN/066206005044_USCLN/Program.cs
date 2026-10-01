using System;
using System.Text;

namespace BaitapUSCLN
{
    class Program
    {
        static int readPositiveInt(string variableName)
        {
            int value;

            do
            {
                Console.Write("Nhập số nguyên dương " + variableName + ": ");
            }
            while (!int.TryParse(Console.ReadLine(), out value) || value <= 0);

            return value;
        }

        static int findGcdBySubtraction(int a, int b)
        {
            while (a != b)
            {
                if (a > b)
                {
                    a = a - b;
                }
                else
                {
                    b = b - a;
                }
            }

            return a;
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("MSSV: 066206005044 - Họ tên SV: Hoàng Việt Quang - Tên bài tập: BT1.1 - Làm quen với C# - Giải thuật tìm USCLN");

            int a = readPositiveInt("a");
            int b = readPositiveInt("b");
            int uscln = findGcdBySubtraction(a, b);

            Console.WriteLine("USCLN(" + a + ", " + b + ") = " + uscln);
            Console.ReadKey();
        }
    }
}
