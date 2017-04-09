using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class FeedInterval
    {
        public IntervalOption Unit { get; protected set; }

        public int UnitId { get; protected set; }

        public Type UnitType { get; protected set; }
    }
}
