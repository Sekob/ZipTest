using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipTest.Reader
{
    public class Portion
    {
        private readonly int _portionId;
        private readonly int _amount;
        private readonly byte[] _data;
        
        public Portion (int portionId, int amount, byte[] data)
        {
            _portionId = portionId;
            _amount = amount;
            _data = data;
        }

        public int PortionID => _portionId;
        public int Amount => _amount;
        public byte[] Data => _data;
    }
}
