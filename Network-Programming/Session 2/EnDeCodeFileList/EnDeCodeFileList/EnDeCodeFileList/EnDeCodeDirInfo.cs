using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EnDeCodeFileList
{
    public class EnDeCodeDirInfo
    {
        static public string encode(string path)
        {
            path = (path ?? "").Trim();
            if (path == "")
            {
                throw new ArgumentException("Đường dẫn không được để trống.");
            }

            if (path == ".")
            {
                path = Directory.GetCurrentDirectory();
            }

            path = Path.GetFullPath(path);
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException("Không tìm thấy thư mục: " + path);

            DirectoryInfo dir = new DirectoryInfo(path);
            List<string> list = new List<string>();

            foreach (DirectoryInfo d in dir.GetDirectories())
            {
                list.Add(d.Name);
                list.Add("0");
            }

            foreach (FileInfo f in dir.GetFiles())
            {
                list.Add(f.Name);
                list.Add("1");
            }

            StringBuilder result = new StringBuilder();
            result.Append('$');
            result.Append(list.Count / 2);

            for (int i = 0; i < list.Count; i += 2)
            {
                result.Append(';');
                result.Append(list[i]);
                result.Append(';');
                result.Append(list[i + 1]);
            }

            result.Append('#');
            return result.ToString();
        }

        static public string decode(string S)
        {
            if (string.IsNullOrWhiteSpace(S))
            {
                throw new ArgumentException("Chuỗi mã hoá không hợp lệ.");
            }

            S = S.Trim();
            if (S[0] != '$' || S[S.Length - 1] != '#')
            {
                throw new FormatException("Chuỗi phải bắt đầu bằng '$' và kết thúc bằng '#'.");
            }

            string body = S.Substring(1, S.Length - 2);
            string[] parts = body.Split(';');

            if (parts.Length < 1)
            {
                throw new FormatException("Chuỗi mã hoá rỗng.");
            }

            int total;
            if (!int.TryParse(parts[0], out total) || total < 0)
            {
                throw new FormatException("Thành phần đầu tiên phải là số nguyên không âm.");
            }

            if ((parts.Length - 1) % 2 != 0 || (parts.Length - 1) / 2 != total)
            {
                throw new FormatException("Số lượng phần tử không khớp với tổng khai báo.");
            }

            StringBuilder output = new StringBuilder();
            output.AppendLine("Tổng số mục: " + total);

            for (int i = 1; i < parts.Length; i += 2)
            {
                string name = parts[i];
                string type = parts[i + 1];

                if (type == "0") output.AppendLine("Folder: " + name);
                else if (type == "1") output.AppendLine("File: " + name);
                else throw new FormatException("Giá trị loại phải là 0 hoặc 1.");
            }

            return output.ToString().TrimEnd();
        }
    }
}
