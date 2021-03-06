﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public interface IPriceItemFeed<T>
    {
        IPriceItemFeed<T> Register(IProbe<T> probe);
    }
}
