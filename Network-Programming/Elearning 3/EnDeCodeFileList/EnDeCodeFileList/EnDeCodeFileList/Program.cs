using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace EnDeCodeFileList
{
    class Program
    {
        static void Main(string[] args)
        {
            String s1 = "$3;folder1;0;folder2;0;file1;1#";
            decode(s1);
            String s2 = "$4;folder1;0;folder2;0;file1;1;folder3;0#";
            decode(s2);
            String s3 = getDirInfo(@"C:\");
            Console.WriteLine(s3);
            decode(s3);
        }

        static void decode(string s)
        {
            //$3;folder1;0;folder2;0;file1;1#
            if (s[0]!='$')
            {
                System.Console.WriteLine("The first char is invalid!");
                return;// quit; nothing to do next
            }
            if (s[s.Length-1]!='#') //last char at (length - 1)
            {
                System.Console.WriteLine("The first char is invalid!");
                return;
            }
            s = s.Substring(1, s.Length - 2); //remove the first ($) and the last (#) char
            //3;folder1;0;folder2;0;file1;1
            string[] part = s.Split(new char[] { ';' });
            int n = 0;
            try //test the first part is numeric or not
            {
                n = int.Parse(part[0]); // n will be 3 in this case
            }
            catch(Exception ex)
            {
                System.Console.WriteLine("Invalid message format! \n" + ex.Message);
                return;
            }
            //now, test if n is equal the number of items in the list
            if (n != ((part.Length - 1)/2))
            {
                System.Console.WriteLine("The number of items is inconsistent!");
                return;
            }
            ArrayList lstFolders = new ArrayList();
            ArrayList lstFiles = new ArrayList();
            for (int i = 1; i <= (part.Length - 1) / 2; i++) // think about this carefully!!!
            {
                if (part[i*2].CompareTo("0")==0) //this is a folder
                {
                    lstFolders.Add(part[i*2-1]);
                } else if (part[i*2].CompareTo("1")==0) //this is a file
                {
                    lstFiles.Add(part[i*2-1]);
                }
                else 
                {
                    System.Console.WriteLine("Invalid message format! Error at item: " + i);
                    return;
                }
            }
            //Now output
            Console.WriteLine("Folder List:");
            foreach (string folder in lstFolders)
                Console.WriteLine(" +" + folder);
            Console.WriteLine("File List:");
            foreach (string file in lstFiles)
                Console.WriteLine(" ." + file);
        }

        static string getDirInfo(string path)
        {
            string s = "$";
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
            int n = dir.GetDirectories().Length + dir.GetFiles().Length;
            s += n;
            foreach (System.IO.DirectoryInfo d in dir.GetDirectories())
                s += ";" + d.Name + ";0";
            foreach (System.IO.FileInfo f in dir.GetFiles())
                s += ";" + f.Name + ";1";
            s += "#";
            return s;
        }
    }
}
