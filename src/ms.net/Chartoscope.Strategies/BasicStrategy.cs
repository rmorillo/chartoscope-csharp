using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Strategies
{
    public class BasicStrategy: ExtensibleStrategyBase
    {
        public const string IDENTITY_CODE = "basic_strategy";
        private ISignal signal = null;

        public BasicStrategy(ISignal signal)
        {
            this.signal = signal;

            this.barType = signal.BarType;
           
            this.identityCode = string.Format("{0}({1},{2})", IDENTITY_CODE, signal.IdentityCode, barType.Code);

            Register(signal);
        }

        private bool IsLongSetup(PriceBars priceBar)
        {
            return signal.IsInPosition && signal.Position == PositionMode.Long;
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            return signal.IsInPosition && signal.Position == PositionMode.Short;
        }

        protected override void OutPosition(PriceBars priceBar, BarItemType barType)
        {           
            if (IsLongSetup(priceBar))
            {
                Enter(PositionMode.Long);
            }
            else if (IsShortSetup(priceBar))
            {
                Enter(PositionMode.Short);
            }

        }

        protected override void InPosition(PositionMode position, PriceBars priceBar, BarItemType barType)
        {
            if (signal.IsOutPosition)
            {
                Exit();
            }
        }

    }
}
