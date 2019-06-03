using System;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace ZipTest
{
    class Program
    {
        class Chunk
        {
            public int Count { get; private set; }
            public int Capacity { get; private set; }
            public byte[] Data { get; private set; }

            public Chunk(int capacity)
            {
                Data = new byte[capacity];
                Capacity = capacity;
                Count = 0;
            }

            public void SetCount(int count)
            {
                Count = count;
            }
        }

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

            const int amountToRead = 1024 * 1024 * 50;
            int numberOfchunks = Environment.ProcessorCount;
            Thread[] workingThreads = new Thread[numberOfchunks];
            // Reading speed test
            using (var file = File.OpenRead(fromFileName))
            using (var toFile = File.OpenWrite(toFileName))
            {
                bool[] compressingDone = new bool[numberOfchunks];
                Chunk[] chunks = new Chunk[numberOfchunks];
                for (int i = 0; i < numberOfchunks; i++)
                {
                    chunks[i] = new Chunk(amountToRead);
                }

                for (int i = 0; i < numberOfchunks; i++)
                {
                    workingThreads[i] = new Thread(() =>
                    {
                        int numberOfThread = i;
                        bool workDone = false;

                        using()
                        while (!workDone)
                        {
                            if (compressingDone[i] == true)
                                Thread.Sleep(0);

                        }
                    });
                }

                for (int i = 0; i < numberOfchunks; i++)
                {
                    file.Read(chunks[i].Data, 0, amountToRead);
                    chunks[i].SetCount(amountToRead);
                }
            }

            //var zipService = new MultithreadedZip();
        }

        private static bool ValidateInputFileName(string fileName, string operation)
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
