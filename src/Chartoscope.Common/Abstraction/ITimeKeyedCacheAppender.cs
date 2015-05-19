using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public interface ITimeKeyedCacheAppender: ICacheAppender
    {
        void Append(DateTime time, object[] values);
    }
}
