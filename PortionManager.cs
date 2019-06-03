using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomBuffers
{
    public class PortionManager
    {
        public class Portion
        {
            private readonly int _portionId;
            private BufferManager.Buffer _buffer;
            public Portion(int portionId, BufferManager.Buffer buffer)
            {
                _portionId = portionId;
                _buffer = buffer;
            }

            public BufferManager.Buffer Buffer => _buffer;
            public int PortionId => _portionId;
        }

        private Queue<Portion> _portionsForProcess;
        private Queue<Portion> _portionsForWrite;
        private Queue<Portion> _portionsForRead;
        private object _operationLocker = new object();
        private object _writeLocker = new object();
        private object _readLocker = new object();

        // TODO: need to think about Monitor.Exit
        public Portion DequeuePortionForProcess()
        {
            return DequeuePortionBase(_operationLocker, _portionsForProcess);
        }

        public Portion DequeuePortionForWrite()
        {
            return DequeuePortionBase(_writeLocker, _portionsForWrite);
        }

        public Portion DequeuePortionForRead()
        {
            return DequeuePortionBase(_readLocker, _portionsForRead);
        }

        public void EnqueuePortionForRead(Portion portion)
        {
            EnqueueBase(portion, _readLocker, _portionsForRead);
        }

        public void EnqueuePortionForProcess(Portion portion)
        {
            EnqueueBase(portion, _operationLocker, _portionsForProcess);
        }

        public void EnqueuePortionForWrite(Portion portion)
        {
            EnqueueBase(portion, _writeLocker, _portionsForWrite);
        }

        private Portion DequeuePortionBase(object locker, Queue<Portion> queue)
        {
            while (true)
            {
                if (!Monitor.TryEnter(locker))
                {
                    Thread.Sleep(0);
                    continue;
                }

                if (queue.Count == 0)
                {
                    Monitor.Exit(locker);
                    Thread.Sleep(0);
                    continue;
                }

                try
                {
                    return queue.Dequeue();
                }
                finally
                {
                    Monitor.Exit(locker);
                }
            }
        }
        private void EnqueueBase(Portion portion, object locker, Queue<Portion> queue)
        {
            try
            {
                while (!Monitor.TryEnter(locker))
                    Thread.Sleep(0);
                queue.Enqueue(portion);
            }
            finally
            {
                Monitor.Exit(locker);
            }
        }
    }
}
