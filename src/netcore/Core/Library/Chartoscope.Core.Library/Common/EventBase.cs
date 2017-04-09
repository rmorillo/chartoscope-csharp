using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public abstract class EventBase
    {
        public EventStateOption Status { get; protected set; }        
    }
}
