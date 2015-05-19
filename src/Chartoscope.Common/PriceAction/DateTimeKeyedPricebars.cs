using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public class DateTimeKeyedPricebars : KeyedCollection<DateTime, BarItem>
    {

        protected override DateTime GetKeyForItem(BarItem item)
        {
            return item.Time;
        }
    }
}
