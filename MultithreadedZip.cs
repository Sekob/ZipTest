using CustomBuffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZipTest
{
    public class MultithreadedZip
    {
        private readonly int _threadCount;
        private readonly BufferManager _bufferManager;
        private readonly int _workingPortion = 1024 * 1024;
        private Thread _reader;
        private Thread _writer;
        private readonly Thread[] _workers;

        public MultithreadedZip()
        {
            _threadCount = Environment.ProcessorCount; // additional threads for reading and writing
            _bufferManager = new BufferManager(2 * _workingPortion, _threadCount + 2, true);
            _workers = new Thread[_threadCount];
        }

        public int ThreadCount => _threadCount;


    }
}
