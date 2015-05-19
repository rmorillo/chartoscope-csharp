using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Droidworks.Common;
using Metadroids.Analytics.Models;
using Metadroids.Common.Models;

namespace Metadroids.Analytics.Builtin
{
    public class SMACore: SingleIndicator
    {
        public SMACore(TimeframeMode timeframe, int barCount)
            : base("SimpleMovingAverage","sma","Simple Moving Average", timeframe)
        {
            this.barCount = barCount;

        }

        public void Calculate(PriceAction priceAction)
        {
            this._buffer.Add(new SingleValueIndicator(priceAction.Average(barCount), priceAction.FirstToLast.Time));
        }

    }
}
