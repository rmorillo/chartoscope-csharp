using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class MACDCore : IndicatorBase<MultiValueIndicatorItem>
    {
        public const string SHORT_NAME = "macd";
        public const string DISPLAY_NAME = "MACD";
        public const string DESCRIPTION = "Moving Average Convergence/Divergence";

        //Multi-value field index
        public static readonly int MACD_FIELD_INDEX = 0;
        public static readonly int SIGNAL_LINE_FIELD_INDEX = 1;
        public static readonly int HISTOGRAM_FIELD_INDEX = 2;

        private EMACore fastEMA = null;
        private EMACore slowEMA = null;
        private int signalLinePeriod = 0;

        private double signalLineSmoothingConstant = 0;

        private MACDCore(BarItemType barItemType, EMACore fastEMACore, EMACore slowEMACore, int signalLinePeriod, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted=null)
            : base(DISPLAY_NAME, SHORT_NAME, DESCRIPTION, barItemType)
        {
            this.barCount = slowEMACore.Period;
            this.maxBarIndex = barCount - 1;
            this.signalLinePeriod = signalLinePeriod;
            this.signalLineSmoothingConstant = (double)2 / (signalLinePeriod + 1);

            this.AddDependency(barItemType, fastEMACore);
            this.AddDependency(barItemType, slowEMACore);

            this.fastEMA = fastEMACore;
            this.slowEMA = slowEMACore;

            this.identityCode = CreateIdentityCode(fastEMA.Period, slowEMA.Period, signalLinePeriod);

            if (onCalculationCompleted != null)
            {
                this.Calculated += onCalculationCompleted;
            }
        }

        public static MACDCore CreateInstance(BarItemType barItemType, EMACore fastEMACore, EMACore slowEMACore, int signalLinePeriod, IndicatorDelegates.CalculationCompletedHandler onCalculationCompleted=null)
        {
            return new MACDCore(barItemType, fastEMACore, slowEMACore, signalLinePeriod, onCalculationCompleted);
        }

        protected override void Calculate(PriceBars priceAction)
        {
            if (priceAction.HasValue(maxBarIndex) && slowEMA.HasValue(maxBarIndex))
            {                
                double macd= fastEMA.LastItem.Value-slowEMA.LastItem.Value;
                double emaOfMACD= double.NaN;
                if (HasValue(signalLinePeriod-1, 0))
                {
                    double previousEMA = double.IsNaN(LastItem.Values[SIGNAL_LINE_FIELD_INDEX]) ? this.Average(signalLinePeriod, 0) : LastItem.Values[SIGNAL_LINE_FIELD_INDEX];
                    emaOfMACD = (macd * signalLineSmoothingConstant) + (previousEMA * (1 - signalLineSmoothingConstant));
                }

                this.Add(new MultiValueIndicatorItem(
                    priceAction.LastItem.Time,
                    macd, 
                    emaOfMACD,
                    double.IsNaN(emaOfMACD)? double.NaN: macd-emaOfMACD //Histogram
                    ));
                lastCalculationSuccessful = true;
            }
        }

        public static string CreateIdentityCode(int fastEMAPeriod, int slowEMAPeriod, int signalLinePeriod)
        {
            return string.Format("{0}({1},{2},{3})", SHORT_NAME, fastEMAPeriod, slowEMAPeriod, signalLinePeriod);
        }
    }
}
