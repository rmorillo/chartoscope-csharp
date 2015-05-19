using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class StochasticsCore: IndicatorBase<MultiValueIndicatorItem>
    {
        public const string SHORT_NAME = "sto";
        public const string DISPLAY_NAME = "Stochastics";
        public const string DESCRIPTION = "Stochastic Oscillator";

        //Multi-value field index
        public static readonly int PERCENT_K_FIELD_INDEX = 0;
        public static readonly int PERCENT_D_FIELD_INDEX = 1;

        private PriceChangeCore priceChange = null;
        private HighsAndLowsCore highsAndLows = null;

        private double percentDSmoothingConstant = 0;

        private bool smoothedPercentK = false;
        private int smoothedPercentKPeriods = int.MinValue;
        private int smoothedPercentDPeriods = int.MinValue;

        //Fast or Slow stochastics depending on smoothedPercentK
        private StochasticsCore(BarItemType barItemType, int periods, bool smoothedPercentK, PriceChangeCore priceChangeCore, HighsAndLowsCore highsAndLowsCore, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
            : base(DISPLAY_NAME, SHORT_NAME, DESCRIPTION, barItemType)
        {
            this.smoothedPercentK = smoothedPercentK;
            this.smoothedPercentKPeriods = 3;
            this.smoothedPercentDPeriods = 3;
            Init(barItemType, periods, priceChangeCore, highsAndLowsCore, onCalculationCompleted);
        }

        //Full stochastics
        private StochasticsCore(BarItemType barItemType, int periods, int smoothedPercentKPeriods, int smoothedPercentDPeriods, PriceChangeCore priceChangeCore, HighsAndLowsCore highsAndLowsCore, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
            : base(DISPLAY_NAME, SHORT_NAME, DESCRIPTION, barItemType)
        {
            this.smoothedPercentK = true;
            this.smoothedPercentKPeriods = smoothedPercentKPeriods;
            this.smoothedPercentDPeriods = smoothedPercentDPeriods;
            Init(barItemType, periods, priceChangeCore, highsAndLowsCore, onCalculationCompleted);
        }

        private void Init(BarItemType barItemType, int periods, PriceChangeCore priceChangeCore, HighsAndLowsCore highsAndLowsCore, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)   
        {
            this.barCount = periods;
            this.maxBarIndex = barCount - 1;
            this.percentDSmoothingConstant = (double)2 / (smoothedPercentDPeriods + 1);           

            this.AddDependency(barItemType, highsAndLowsCore);
            this.AddDependency(barItemType, priceChangeCore);

            this.highsAndLows = highsAndLowsCore;
            this.priceChange = priceChangeCore;

            this.identityCode = CreateIdentityCode(periods);

            if (onCalculationCompleted != null)
            {
                this.Calculated += onCalculationCompleted;
            }
        }

        public static StochasticsCore CreateInstance(BarItemType barItemType, int periods, bool smoothedPercentK, PriceChangeCore priceChangeCore, HighsAndLowsCore highsAndLowsCore, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
        {
            return new StochasticsCore(barItemType, periods, smoothedPercentK, priceChangeCore, highsAndLowsCore, onCalculationCompleted);
        }

        public static StochasticsCore CreateInstance(BarItemType barItemType, int periods, int smoothedPercentKPeriods, int smoothedPercentDPeriods, PriceChangeCore priceChangeCore, HighsAndLowsCore highsAndLowsCore, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted = null)
        {
            return new StochasticsCore(barItemType, periods, smoothedPercentKPeriods, smoothedPercentDPeriods, priceChangeCore, highsAndLowsCore, onCalculationCompleted);
        }

        protected override void Calculate(PriceBars priceAction)
        {
            if (priceAction.HasValue(maxBarIndex))
            {                
                double percentK = 100 * ((priceAction.LastItem.Close - highsAndLows.PeriodsLow())/(highsAndLows.PeriodsHigh()-highsAndLows.PeriodsLow()));
                if (smoothedPercentK)
                {
                    if (smoothedPercentK && HasValue(smoothedPercentKPeriods-2))
                    {
                        percentK = (Sum(smoothedPercentKPeriods - 1, 0) + percentK) / smoothedPercentKPeriods;
                    }
                }

                double emaOfPercentK= double.NaN;
                if (HasValue(smoothedPercentDPeriods - 1, 0))
                {
                    double previousEMA = double.IsNaN(LastItem.Values[1]) ? this.Average(smoothedPercentDPeriods, 0) : LastItem.Values[1];
                    emaOfPercentK = (percentK * percentDSmoothingConstant) + (previousEMA * (1 - percentDSmoothingConstant));
                }

                this.Add(new MultiValueIndicatorItem(
                    priceAction.LastItem.Time,
                    percentK,  //%K
                    emaOfPercentK //%D
                    ));
                lastCalculationSuccessful = true;
            }
        }

        public static string CreateIdentityCode(int periods)
        {
            return string.Format("{0}({1})", SHORT_NAME, periods);
        }

        public double GetPercentK(int index = 0)
        {
            return this.Last(index).Values[0];
        }

        public double GetPercentD(int index = 0)
        {
            return this.Last(index).Values[1];
        }
    }
}
