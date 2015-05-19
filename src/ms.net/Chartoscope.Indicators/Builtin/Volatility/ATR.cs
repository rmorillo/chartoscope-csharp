using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class ATR: BuiltinIndicatorBase<ATRCore>, IIndicatorCore
    {
        private int periods;

        public int Periods
        {
            get { return periods; }
        }

        public ATR(BarItemType barType, int periods)
            : base(barType)
        {
            this.periods = periods;
            this.barType = barType;
        }

        public void BuildCore(ISessionIndicator sessionIndicator)
        {
            string coreIdentityCode = ATRCore.CreateIdentityCode(periods);
            core = sessionIndicator.CoreIndicators.ContainsKey(coreIdentityCode) ? sessionIndicator.CoreIndicators[coreIdentityCode].IndicatorInstance as ATRCore : null;
            if (core == null)
            {
                string trueRangeIdentityCode = TrueRangeCore.CreateIdentityCode();
                TrueRangeCore trueRangeCore = sessionIndicator.CoreIndicators.ContainsKey(trueRangeIdentityCode) ? sessionIndicator.CoreIndicators[trueRangeIdentityCode].IndicatorInstance as TrueRangeCore : null;
                if (trueRangeCore == null)
                {
                    trueRangeCore = TrueRangeCore.CreateInstance(barType);
                    sessionIndicator.CoreIndicators.Add(trueRangeCore.IdentityCode, new CoreIndicator(trueRangeCore));
                }

                core = ATRCore.CreateInstance(barType, periods, trueRangeCore, this.OnCalculationCompleted);
                sessionIndicator.CoreIndicators.Add(core.IdentityCode, new CoreIndicator(core));
            }
        }

        BarItemType IIndicatorCore.BarType
        {
            get { return this.barType; }
        }

        public string IdentityCode
        {
            get { return core.IdentityCode; }
        }
    }
}
