using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class ByteMapItem
    {
        private int _offset;
        private int _size;
        private Type _type;

        public Type Type
        {
            get { return _type; }
            set { _type = value; }
        }


        public ByteMapItem(Type type, int size, int offset, Delegate getter, Delegate setter)
        {
            _type = type;
            _size = size;
            _offset = offset;
            _getter = getter;
            _setter = setter;
        }

        public int Size { get { return _size; } }

        public int Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        private Delegate _getter;

        public Delegate Getter
        {
            get { return _getter; }            
        }

        private Delegate _setter;

        public Delegate Setter
        {
            get { return _setter; }            
        }               
    }
}
