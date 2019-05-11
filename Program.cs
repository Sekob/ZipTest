using System;
using System.IO;

namespace ZipTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Wrong number of arguments. Please press any key to exit...");
                Console.ReadKey();
                return;
            }

            switch (args[0].ToLower())
            {
                case "compress":
                    Compress(args[1], args[2]);
                    break;
                case "decompress":
                    Decompress(args[1], args[2]);
                    break;
                default:
                    Console.WriteLine(@"Wrong first arguement. Must be 'compress' or 'decompress'. " +
                                       "Please press any key to exit...");
                    Console.ReadKey();
                    break;
            }
        }

        private static void Decompress(string fromFileName, string toFileName)
        {
            if (!ValidateInputFileName(fromFileName, "decompress"))
                return;
        }

        private static void Compress(string fromFileName, string toFileName)
        {
            if (!ValidateInputFileName(fromFileName, "compress"))
                return;

            var zipService = new MultithreadedZip();
            Console.WriteLine(zipService.ThreadCount);
        }

        private static bool ValidateInputFileName (string fileName, string operation)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"Wrong file name to {operation} '{fileName}'. Please press any key to exit...");
                Console.ReadKey();
                return false;
            }

            return true;
        }
    }
}
