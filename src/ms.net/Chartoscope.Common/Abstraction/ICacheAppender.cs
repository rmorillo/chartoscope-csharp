using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public interface ICacheAppender
    {
        void Initialize();
        void Open(CachingModeOption cachingMode);
        void Close();
    }
}
