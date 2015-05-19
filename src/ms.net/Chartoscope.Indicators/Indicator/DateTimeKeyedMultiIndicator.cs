using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class DateTimeKeyedIndicator<T>: KeyedCollection<DateTime, T> where T : IBufferItemKey<DateTime>
    {
        protected override DateTime GetKeyForItem(T item)
        {
            return item.Key;
        }
    }
}
