using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public interface INotifyUpdate<TBar, TOption>
    {
        event Delegates.BarUpdateNotificationEventHandler<TBar> BarUpdated;

        event Delegates.PriceUpdateNotificationEventHandler<TOption, double> PriceUpdated;
    }
}
