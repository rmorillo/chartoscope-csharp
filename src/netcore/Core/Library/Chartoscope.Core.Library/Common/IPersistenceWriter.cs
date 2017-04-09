using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public interface IPersistenceWriter
    {
        void Initialize(byte[] header);
        void Append(byte[] data);        
        void Close();
    }
}
