using System;
using System.IO;

namespace DnsTestTask
{
    enum Mode
    {
        Size,
        HumanReadable,
        Default
    }

    class Program
    {
        static void Main(string[] args)
        {
            var depth = -1;
            var mode = Mode.Default;

            var dirInfo = new DirectoryInfo(Directory.GetCurrentDirectory());

            if (args.Length > 0)
            {
                for (var i = 0; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "-d":
                        case "--depth":
                            depth = int.Parse(args[i+1]);
                            break;
                        case "-s":
                        case "--size":
                            mode = Mode.Size;
                            break;
                        case "-h":
                        case "--human-readable":
                            mode = Mode.HumanReadable;
                            break;
                        case "--help":
                            PrintHelp();
                            return;       
                    }
                }
            }

            PrintFiles(dirInfo, 0, depth, mode);
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Ключ -d или --depth для установки максимальной глубины рекурсии");
            Console.WriteLine("Ключ -s или --size для отображения размеров файлов");
            Console.WriteLine("Ключ -h или --human-readable для отображения размеров файлов в понятном для человека формате");
        }

        private static void PrintFiles(DirectoryInfo info, int nesting, int maxDepth, Mode mode)
        {
            if(maxDepth != -1 && nesting > maxDepth)
                return;

            Console.WriteLine($"{Indent(nesting)}└───{info.Name}");

            foreach (var fileInfo in info.GetFiles())
            {
                PrintFileWithMode(fileInfo,nesting+1,mode);
            }

            foreach (var directoryInfo in info.GetDirectories())
            {
                PrintFiles(directoryInfo, nesting + 1, maxDepth, mode);
            }

        }

        private static void PrintFileWithMode(FileInfo fileInfo, int nesting, Mode mode)
        {
            Console.WriteLine($"{Indent(nesting)}├───{fileInfo.Name} {GetModeLength(mode, fileInfo.Length)}");
        }

        private static string GetModeLength(Mode mode, long fileInfoLength)
        {
            switch (mode)
            {
                case Mode.Size:
                    return $"({fileInfoLength} B)";
                case Mode.HumanReadable:
                    if (fileInfoLength == 0)
                        return "(empty)";
                    string[] sizes = { "B", "KB", "MB", "GB", "TB" };
                    var order = 0;
                    while (fileInfoLength >= 1024 && order < sizes.Length - 1)
                    {
                        order++;
                        fileInfoLength = fileInfoLength / 1024;
                        return $"({fileInfoLength:0.##} {sizes[order]})";
                    }
                    break;
                case Mode.Default:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
            return "";
        }

        public static string Indent(int count)
        {
            return "".PadLeft(count * 4);
        }
    }
}
