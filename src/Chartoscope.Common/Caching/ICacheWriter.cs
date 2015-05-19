using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public interface ICacheWriter
    {
        void RegisterHeader(CacheHeaderInfo header);
        void BeginWriting(string identityCode);
        void Write(string identityCode, params object[] values);
        void EndWriting(string identityCode);
    }
}
