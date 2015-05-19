using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public interface ICaching
    {
        CacheHeaderInfo Header { get; }
        CacheColumn[] Columns { get; }
    }
}
