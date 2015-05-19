using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public static class PersistenceDelegates
    {
        public delegate void BookmarkNotificationHandler(DateTime time, long position);
    }
}
