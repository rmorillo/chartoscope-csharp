using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public class CacheConfigItem
    {
        private int datafeedHashKey;

        public int DatafeedHashKey
        {
            get { return datafeedHashKey; }
        }

        private Guid sessionId;

        public Guid SessionId
        {
            get { return sessionId; }
        }

        private SessionModeOption sessionMode;

        public SessionModeOption SessionMode
        {
            get { return sessionMode; }
        }

        public CacheConfigItem(int datafeedHashKey, Guid sessionId, SessionModeOption sessionMode)
        {
            this.datafeedHashKey = datafeedHashKey;
            this.sessionId = sessionId;
            this.sessionMode = sessionMode;
        }
        
    }
}
