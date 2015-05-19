using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public interface IBufferItemKey<TKey>
    {
        TKey Key { get; }
    }
}
