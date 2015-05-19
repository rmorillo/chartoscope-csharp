using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    [IndicatorIdentity(RSICore.SHORT_NAME, RSICore.DISPLAY_NAME, RSICore.DESCRIPTION)]
    public class RSI : BuiltinIndicatorBase<RSICore>, IIndicatorCore
    {
        private int periods;

        public int Periods
        {
            get { return periods; }
        }

        public RSI(BarItemType barType, int periods)
            : base(barType)
        {
            this.periods = periods;
            this.barType = barType;
        }

        public void BuildCore(ISessionIndicator sessionIndicator)
        {
            string coreIdentityCode = RSICore.CreateIdentityCode(periods);
            core = sessionIndicator.CoreIndicators.ContainsKey(coreIdentityCode) ? sessionIndicator.CoreIndicators[coreIdentityCode].IndicatorInstance as RSICore : null;
            if (core == null)
            {
                string priceChangeIdentityCode = PriceChangeCore.CreateIdentityCode();
                PriceChangeCore priceChangeCore = sessionIndicator.CoreIndicators.ContainsKey(priceChangeIdentityCode) ? sessionIndicator.CoreIndicators[priceChangeIdentityCode].IndicatorInstance as PriceChangeCore : null;
                if (priceChangeCore == null)
                {
                    priceChangeCore = PriceChangeCore.CreateInstance(barType);
                    sessionIndicator.CoreIndicators.Add(priceChangeCore.IdentityCode, new CoreIndicator(priceChangeCore));
                }

                core = RSICore.CreateInstance(barType, periods, priceChangeCore, this.OnCalculationCompleted);
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

        [IndicatorPlotting("rsi", ChartTypeOption.Line, ChartRangeOption.PositiveHundredRange)]
        public double Value(int index = 0)
        {
            return core.HasValue(index) ? core.Last(index).Value : double.NaN;
        }
    }
}
