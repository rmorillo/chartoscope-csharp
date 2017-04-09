using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chartoscope.Core.Common
{
    public class StringBuilderPool : LookBehindPool<StringBuilder>
    {
        public StringBuilderPool(int capacity) : base(capacity)
        {
        }

        public void Write(string value)
        {
            _PoolItems[_CurrentPosition].Clear();
            _PoolItems[_CurrentPosition].Append(value);

            MoveNext();
        }

        protected override StringBuilder CreatePoolItem()
        {
            return new StringBuilder();
        }
    }
}
