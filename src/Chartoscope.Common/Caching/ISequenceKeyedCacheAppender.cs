using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public interface ISequenceKeyedCacheAppender : ICacheAppender
    {
        void Append(long sequence, object[] values);
    }
}
