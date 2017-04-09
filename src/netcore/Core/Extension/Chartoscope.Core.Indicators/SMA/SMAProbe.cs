using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Indicators
{
    public class SMA: IndicatorProbe<ISMAItem, SMAIndicator>
    {
        public SMA(int period)
        {
            _Probe = new SMAIndicator(PoolSizeConfig.GetPoolSize(typeof(SMAIndicator)), period);
        }
    }
}
