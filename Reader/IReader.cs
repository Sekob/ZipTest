using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipTest.Reader
{
    public interface IReader
    {
        Portion Read();
    }
}
