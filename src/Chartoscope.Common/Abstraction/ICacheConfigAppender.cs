using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public interface ICacheConfigAppender: ICacheAppender
    {
        void Append(long datafeedHashKey, Guid sessionId, SessionModeOption sessionMode);
        void Append(CacheConfigItem item);
    }
}
