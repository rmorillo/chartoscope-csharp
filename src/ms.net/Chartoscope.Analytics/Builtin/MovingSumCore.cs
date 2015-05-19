using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Droidworks.Common;
using Metadroids.Analytics.Models;
using Metadroids.Common.Models;
using Metadroids.Common.RingBuffers;

namespace Metadroids.Analytics.Builtin
{
    public class MovingSum: SingleIndicator
    {
        public MovingSum(TimeframeMode timeframe, int barCount): base("MovingSum","ms","Moving Sum",timeframe)
        {
            this.barCount = barCount;
        }

        public void Calculate(PriceAction priceAction)
        {
            this._buffer.Add(new SingleValueIndicator(priceAction.Sum(barCount), priceAction.FirstToLast.Time));
        }

    }
}
