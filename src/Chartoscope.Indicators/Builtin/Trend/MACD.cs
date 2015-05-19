using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class MACD : BuiltinIndicatorBase<MACDCore>, IIndicatorCore
    {        
        private int fastEMAPeriod;

        public int FastEMAPeriod
        {
            get { return fastEMAPeriod; }
        }

        private int slowEMAPeriod;

        public int SlowEMAPeriod
        {
            get { return slowEMAPeriod; }
        }

        private int signalLinePeriod;

        public int SignalLinePeriod
        {
            get { return signalLinePeriod; }
        }
        

        public MACD(BarItemType barType, int fastEMAPeriod, int slowEMAPeriod, int signalLinePeriod): base(barType)
        {
            this.fastEMAPeriod = fastEMAPeriod;
            this.slowEMAPeriod = slowEMAPeriod;
            this.signalLinePeriod = signalLinePeriod;
            this.barType = barType;
        }

        public void BuildCore(ISessionIndicator sessionIndicator)
        {
            string coreIdentityCode = MACDCore.CreateIdentityCode(fastEMAPeriod, slowEMAPeriod, signalLinePeriod);
            core = sessionIndicator.CoreIndicators.ContainsKey(coreIdentityCode) ? sessionIndicator.CoreIndicators[coreIdentityCode].IndicatorInstance as MACDCore : null;
            if (core == null)
            {
                string fastEMAIdentityCode = EMACore.CreateIdentityCode(fastEMAPeriod);
                EMACore fastEMACore = sessionIndicator.CoreIndicators.ContainsKey(fastEMAIdentityCode) ? sessionIndicator.CoreIndicators[fastEMAIdentityCode].IndicatorInstance as EMACore : null;
                if (fastEMACore == null)
                {
                    fastEMACore = EMACore.CreateInstance(barType, fastEMAPeriod);
                    sessionIndicator.CoreIndicators.Add(fastEMACore.IdentityCode, new CoreIndicator(fastEMACore));
                }

                string slowEMAIdentityCode = EMACore.CreateIdentityCode(slowEMAPeriod);
                EMACore slowEMACore = sessionIndicator.CoreIndicators.ContainsKey(slowEMAIdentityCode) ? sessionIndicator.CoreIndicators[slowEMAIdentityCode].IndicatorInstance as EMACore : null;
                if (slowEMACore == null)
                {
                    slowEMACore = EMACore.CreateInstance(barType, slowEMAPeriod);
                    sessionIndicator.CoreIndicators.Add(slowEMACore.IdentityCode, new CoreIndicator(slowEMACore));
                }

                core = MACDCore.CreateInstance(barType, fastEMACore, slowEMACore, signalLinePeriod, this.OnCalculationCompleted);
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

        [IndicatorPlotting("macd", ChartTypeOption.Line, ChartRangeOption.PipRange)]
        public double MACDValue(int index = 0)
        {
            return core.HasValue(index) ? core.Last(index).Values[MACDCore.MACD_FIELD_INDEX] : double.NaN;
        }

        [IndicatorPlotting("signal line", ChartTypeOption.Line, ChartRangeOption.PipRange)]
        public double SignalLine(int index = 0)
        {
            return core.HasValue(index) ? core.Last(index).Values[MACDCore.SIGNAL_LINE_FIELD_INDEX] : double.NaN;
        }

        [IndicatorPlotting("histogram", ChartTypeOption.Histogram, ChartRangeOption.PipRange)]
        public double Histogram(int index = 0)
        {
            return core.HasValue(index) ? core.Last(index).Values[MACDCore.HISTOGRAM_FIELD_INDEX] : double.NaN;
        }
    }
}
