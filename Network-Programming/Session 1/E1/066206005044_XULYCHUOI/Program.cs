using System;
using System.Text;

namespace BaitapUSCLN
{
	class Program
	{
		static string toUpperCaseText(string input)
		{
			return input.ToUpper();
		}

		static string insertStudentIdAtMiddle(string input, string studentId)
		{
			int middleIndex = input.Length / 2;
			return input.Insert(middleIndex, studentId);
		}

		static string[] splitWords(string input)
		{
			char[] separators = { ' ', '.', ',' };
			return input.Split(separators, StringSplitOptions.RemoveEmptyEntries);
		}

		static string findLongestWord(string[] words)
		{
			if (words.Length == 0)
			{
				return string.Empty;
			}

			string longestWord = words[0];
			for (int i = 1; i < words.Length; i++)
			{
				if (words[i].Length > longestWord.Length)
				{
					longestWord = words[i];
				}
			}

			return longestWord;
		}

		static string sortWordsAlphabetically(string[] words)
		{
			Array.Sort(words, StringComparer.CurrentCultureIgnoreCase);
			return string.Join(" ", words);
		}

		static void Main(string[] args)
		{
			const string mssv = "066206005044";

			Console.OutputEncoding = Encoding.UTF8;
			Console.WriteLine("MSSV: " + mssv + " - Họ tên SV: Hoàng Việt Quang - Tên bài tập: BT1.3 - Xử lý chuỗi");
			Console.Write("Nhập chuỗi: ");

			string chuoiGoc = Console.ReadLine() ?? string.Empty;

			string chuoiInHoa = toUpperCaseText(chuoiGoc);
			string chuoiChenMssv = insertStudentIdAtMiddle(chuoiGoc, mssv);
			string[] mangTu = splitWords(chuoiGoc);

			int soTu = mangTu.Length;
			string tuDaiNhat = findLongestWord(mangTu);
			string chuoiSapXep = sortWordsAlphabetically(mangTu);

			Console.WriteLine();
			Console.WriteLine("1. Chuỗi in hoa: " + chuoiInHoa);
			Console.WriteLine("2. Chuỗi sau khi chèn MSSV: " + chuoiChenMssv);
			Console.WriteLine("3. Số từ trong chuỗi: " + soTu);
			Console.WriteLine("4. Từ dài nhất: " + (tuDaiNhat.Length > 0 ? tuDaiNhat : "(không có)"));
			Console.WriteLine("5. Chuỗi sau khi sắp xếp ABC: " + chuoiSapXep);

			if (!Console.IsInputRedirected)
			{
				Console.ReadKey();
			}
		}
	}
}
