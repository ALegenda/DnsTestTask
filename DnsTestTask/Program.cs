using System;
using System.IO;

namespace DnsTestTask
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintFiles(new DirectoryInfo(Directory.GetCurrentDirectory()), 0);
        }

        private static void PrintFiles(DirectoryInfo info, int nesting)
        {
            Console.WriteLine($"{Indent(nesting)}└───{info.Name}");

            foreach (var fileInfo in info.GetFiles())
            {
                Console.WriteLine($"{Indent(nesting + 1)}├───{fileInfo.Name} ({fileInfo.Length} B)");
            }

            foreach (var directoryInfo in info.GetDirectories())
            {
                PrintFiles(directoryInfo, nesting + 1);
            }

        }

        public static string Indent(int count)
        {
            return "".PadLeft(count * 4);
        }
    }
}
