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
        private int _bufferSizeInBytes;
        private int _quantityOfBuffers;
        private Queue<byte[]> _buffers;
        private object _bufferLocker = new object();
        //TODO: check for NotEnoughtMemory
        public BufferManager(int bufferSizeInBytes, int initialQuantityOfBuffers, bool initBuffers)
        {
            _bufferSizeInBytes = bufferSizeInBytes;
            _quantityOfBuffers = initialQuantityOfBuffers;

            try
            {
                _buffers = new Queue<byte[]>(initialQuantityOfBuffers);
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
                    _buffers.Enqueue(new byte[_bufferSizeInBytes]);
                }
                //Just for NotEnoughtMemory
                catch (Exception ex)
                {
                    break;
                }
            }
        }

        public byte[] DequeueFreeBuffer()
        {
            while (true)
                try
                {
                    Monitor.Enter(_bufferLocker);
                    if (_buffers.Count == 0)
                    {
                        Thread.Sleep(0);
                        continue;
                    }

                    return _buffers.Dequeue();
                }
                finally
                {
                    Monitor.Exit(_bufferLocker);
                }
        }

        public void EnqueueFreeBuffer(byte[] buffer)
        {
            try
            {
                Monitor.Enter(_bufferLocker);
                _buffers.Enqueue(buffer);
                return;
            }
            finally
            {
                Monitor.Exit(_bufferLocker);
            }
        }
    }
}
