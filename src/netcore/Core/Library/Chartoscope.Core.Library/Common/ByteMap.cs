using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class ByteMap
    {
        protected ByteArrayMapping Encoding;
        
        public ByteMap(int size)
        {
            Encoding = new ByteArrayMapping(size);
        }

        public byte[] ToByteArray()
        {
            return Encoding.Encode();
        }

        public void Write(byte[] encoded)
        {
            Encoding.Restore(encoded);
        }
    }
}
