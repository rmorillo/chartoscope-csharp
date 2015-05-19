using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Droidworks.Indicators
{
    public class DateTimeKeyedSingleIndicator : KeyedCollection<DateTime, SingleValueIndicator>
    {

        protected override DateTime GetKeyForItem(SingleValueIndicator item)
        {
            return item.Timestamp;
        }
    }
}
