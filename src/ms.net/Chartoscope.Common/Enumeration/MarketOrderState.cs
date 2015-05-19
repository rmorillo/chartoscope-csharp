﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public enum MarketOrderState
    {
        NoOrder,
        Long,
        Short,
        Closed,
        OrderUpdate,
        LongReversal,
        ShortReversal,        
    }
}
