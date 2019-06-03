using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomBuffers
{
    public class BufferManager
    {
        public class Buffer
        {
            private byte[] _data;
            private int _amountOfData;

            public Buffer(int bufferSize)
            {
                _data = new byte[bufferSize];
                _amountOfData = 0;
            }

            public byte[] Data => _data;
            public int AmountOfData => _amountOfData;

            public byte[] SwapData (byte[] newData, int amount)
            {
                _amountOfData = amount;
                byte[] tempData = _data;
                _data = newData;
                return tempData;
            }
        }

        private int _bufferSizeInBytes;
        private int _quantityOfBuffers;
        private Queue<Buffer> _buffers;
        private object _bufferLocker = new object();
        //TODO: check for NotEnoughtMemory
        public BufferManager(int bufferSizeInBytes, int initialQuantityOfBuffers, bool initBuffers)
        {
            _bufferSizeInBytes = bufferSizeInBytes;
            _quantityOfBuffers = initialQuantityOfBuffers;

            try
            {
                _buffers = new Queue<Buffer>(initialQuantityOfBuffers);
            }
            catch (Exception ex)
            {
                //
            }

            if (!initBuffers)
                return;

            for (int i = 0; i < initialQuantityOfBuffers; i++)
            {
                try
                {
                    _buffers.Enqueue(new Buffer(_bufferSizeInBytes));
                }
                //Just for NotEnoughtMemory
                catch (Exception ex)
                {
                    break;
                }
            }
        }
        // TODO: need to think about Monitor.Exit
        public Buffer DequeueBuffer()
        {
            while (true)
            {
                if (!Monitor.TryEnter(_bufferLocker))
                {
                    Thread.Sleep(0);
                    continue;
                }

                if (_buffers.Count == 0)
                {
                    Monitor.Exit(_bufferLocker);
                    Thread.Sleep(0);
                    continue;
                }

                try
                {
                    return _buffers.Dequeue();
                }
                finally
                {
                    Monitor.Exit(_bufferLocker);
                }
            }
        }

        public void EnqueueBuffer(Buffer buffer)
        {
            try
            {
                while (!Monitor.TryEnter(_bufferLocker))
                    Thread.Sleep(0);
                _buffers.Enqueue(buffer);
            }
            finally
            {
                Monitor.Exit(_bufferLocker);
            }
        }
    }
}
