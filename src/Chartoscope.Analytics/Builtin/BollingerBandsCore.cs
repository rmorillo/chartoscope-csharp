using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Droidworks.Common;
using Metadroids.Analytics.Models;
using Metadroids.Common.Models;

namespace Metadroids.Analytics.Builtin
{
    public class BollingerBandsCore: MultiIndicator
    {
        private SMACore smaCore = null;

        public BollingerBandsCore(TimeframeMode timeframe, int barCount, SMACore smaCore)
            : base("BollingerBands", "bb", "Bollinger Bands", timeframe)
        {
            this.barCount = barCount;
            this.smaCore = smaCore;

        }

        public void Calculate(PriceAction priceAction)
        {
            this._buffer.Add(new MultiValueIndicator(priceAction.FirstToLast.Time,priceAction.Average(barCount)));
        }
    }
}
