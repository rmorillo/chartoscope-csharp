using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Library.UnitTest
{
    public class DerivedTypePool: LookBehindPool<DerivedTypePoolItem>
    {
        public DerivedTypePool(int capacity):base(capacity)
        {
            _MaxSequence = 100;
        }

        protected override DerivedTypePoolItem CreatePoolItem()
        {
            return new DerivedTypePoolItem() { Value = 0 };
        }

        public void Write(int value)
        {
            var item = this.NextPoolItem;
            item.Value = value;
            this.MoveNext();
        }
    }
}
