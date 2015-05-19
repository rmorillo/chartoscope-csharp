using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    [IndicatorIdentity(StochasticsCore.SHORT_NAME, StochasticsCore.DISPLAY_NAME, StochasticsCore.DESCRIPTION)]
    public class Stochastics: BuiltinIndicatorBase<StochasticsCore>, IIndicatorCore
    {        
        private int periods;

        public int Periods
        {
            get { return periods; }
        }

        private bool smoothedPercentK = false;
        private int smoothedPercentKPeriods = int.MinValue;
        private int smoothedPercentDPeriods = int.MinValue;

        public Stochastics(BarItemType barType, int periods, bool smoothedPercentK=false)
            : base(barType)
        {
            this.periods = periods;
            this.barType = barType;
            this.smoothedPercentK = smoothedPercentK;
            this.smoothedPercentKPeriods = 3;
            this.smoothedPercentDPeriods = 3;
        }

        public Stochastics(BarItemType barType, int periods, int smoothedPercentKPeriods, int smoothedPercentDPeriods)
            : base(barType)
        {
            this.periods = periods;
            this.barType = barType;

            this.smoothedPercentK = true;
            this.smoothedPercentKPeriods = smoothedPercentKPeriods;
            this.smoothedPercentDPeriods = smoothedPercentDPeriods;
        }

        public void BuildCore(ISessionIndicator sessionIndicator)
        {
            string coreIdentityCode = StochasticsCore.CreateIdentityCode(periods);
            core = sessionIndicator.CoreIndicators.ContainsKey(coreIdentityCode) ? sessionIndicator.CoreIndicators[coreIdentityCode].IndicatorInstance as StochasticsCore : null;
            if (core == null)
            {
                string priceChangeIdentityCode = PriceChangeCore.CreateIdentityCode();
                PriceChangeCore priceChangeCore = sessionIndicator.CoreIndicators.ContainsKey(priceChangeIdentityCode) ? sessionIndicator.CoreIndicators[priceChangeIdentityCode].IndicatorInstance as PriceChangeCore : null;
                if (priceChangeCore == null)
                {
                    priceChangeCore = PriceChangeCore.CreateInstance(barType);
                    sessionIndicator.CoreIndicators.Add(priceChangeCore.IdentityCode, new CoreIndicator(priceChangeCore));
                }

                string highsAndLowsIdentityCode = HighsAndLowsCore.CreateIdentityCode(periods);
                HighsAndLowsCore highsAndLowsCore = sessionIndicator.CoreIndicators.ContainsKey(highsAndLowsIdentityCode) ? sessionIndicator.CoreIndicators[highsAndLowsIdentityCode].IndicatorInstance as HighsAndLowsCore : null;
                if (highsAndLowsCore == null)
                {
                    highsAndLowsCore = HighsAndLowsCore.CreateInstance(barType, periods);
                    sessionIndicator.CoreIndicators.Add(highsAndLowsCore.IdentityCode, new CoreIndicator(highsAndLowsCore));
                }

                if (smoothedPercentK)
                    core = StochasticsCore.CreateInstance(barType, periods, smoothedPercentKPeriods, smoothedPercentDPeriods, priceChangeCore, highsAndLowsCore, this.OnCalculationCompleted);
                else
                    core = StochasticsCore.CreateInstance(barType, periods, smoothedPercentK, priceChangeCore, highsAndLowsCore, this.OnCalculationCompleted);

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

        [IndicatorPlotting("%K", ChartTypeOption.Line, ChartRangeOption.PositiveHundredRange)]
        public double PercentK(int index = 0)
        {
            return core.HasValue(index) ? core.Last(index).Values[StochasticsCore.PERCENT_K_FIELD_INDEX] : double.NaN;
        }

        [IndicatorPlotting("%D", ChartTypeOption.Line, ChartRangeOption.PositiveHundredRange)]
        public double PercentD(int index = 0)
        {
            return core.HasValue(index) ? core.Last(index).Values[StochasticsCore.PERCENT_D_FIELD_INDEX] : double.NaN;
        }
    }
}
