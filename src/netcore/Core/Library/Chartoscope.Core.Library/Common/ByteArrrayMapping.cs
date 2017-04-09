using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class ByteArrayMapping
    {
        private byte[] _byteData;
        private Dictionary<int, ByteMapItem> _mapper;
        public ByteArrayMapping(int size)
        {
            _byteData = new byte[size];

            _mapper = new Dictionary<int, ByteMapItem>();
        }

        public ByteArrayMapping Map<T>(EntityField field, Func<T> getter, Action<T> setter)
        {
            var nextOffset = 0;

            if (_mapper.Count>0)
            {
                nextOffset = _mapper[_mapper.Count - 1].Offset + _mapper[_mapper.Count - 1].Size;
            }

            _mapper.Add(field.Sequence, new ByteMapItem(typeof(T), field.Size, nextOffset , getter, setter));
            return this;
        }
        
        public byte[] Encode()
        {
            foreach(ByteMapItem byteMap in _mapper.Values)
            {
                if (byteMap.Type==typeof(double))
                {
                    Array.Copy(BitConverter.GetBytes((double) byteMap.Getter.DynamicInvoke()), 0, _byteData, byteMap.Offset, byteMap.Size);
                }
                else if (byteMap.Type == typeof(long))
                {
                    Array.Copy(BitConverter.GetBytes((long)byteMap.Getter.DynamicInvoke()), 0, _byteData, byteMap.Offset, byteMap.Size);
                }
                else if (byteMap.Type == typeof(bool))
                {
                    Array.Copy(BitConverter.GetBytes((bool)byteMap.Getter.DynamicInvoke()), 0, _byteData, byteMap.Offset, byteMap.Size);
                }                
            }

            return _byteData;
        }

        public void Restore(byte[] encoded)
        {
            foreach (ByteMapItem byteMap in _mapper.Values)
            {
                if (byteMap.Type == typeof(double))
                {
                    byteMap.Setter.DynamicInvoke(BitConverter.ToDouble(encoded, byteMap.Offset));
                }
                else if (byteMap.Type == typeof(long))
                {
                    byteMap.Setter.DynamicInvoke(BitConverter.ToInt64(encoded, byteMap.Offset));
                }
                else if (byteMap.Type == typeof(bool))
                {
                    byteMap.Setter.DynamicInvoke(BitConverter.ToBoolean(encoded, byteMap.Offset));
                }
            }
        }

        public byte[] Value
        {
            get
            {
                return _byteData;
            }
        }
    }
}
