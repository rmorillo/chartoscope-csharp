using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public sealed class BollingerBands : BuiltinIndicatorBase<BollingerBandsCore>, IIndicatorCore
    {            
        private int periods;

        public int Periods
        {
            get { return periods; }
        }

        private double stdMultiplier = 2;

        public double StdMultiplier
        {
            get { return stdMultiplier; }           
        }

        public BollingerBands(BarItemType barType, int periods, double stdMultiplier=2): base(barType)
        {
            this.periods = periods;
            this.barType = barType;
            this.stdMultiplier = stdMultiplier;
        }

        public void BuildCore(ISessionIndicator sessionIndicator)
        {
            string coreIdentityCode= BollingerBandsCore.CreateIdentityCode(periods);
            core = sessionIndicator.CoreIndicators.ContainsKey(coreIdentityCode) ? sessionIndicator.CoreIndicators[coreIdentityCode].IndicatorInstance as BollingerBandsCore : null;
            if (core == null)
            {
                string smaIdentityCode= SMACore.CreateIdentityCode(periods);
                SMACore smaCore= sessionIndicator.CoreIndicators.ContainsKey(smaIdentityCode)? sessionIndicator.CoreIndicators[smaIdentityCode].IndicatorInstance as SMACore: null;
                if (smaCore==null)
                {
                    smaCore= SMACore.CreateInstance(barType, periods);                       
                    sessionIndicator.CoreIndicators.Add(smaCore.IdentityCode, new CoreIndicator(smaCore));
                }

                core = BollingerBandsCore.CreateInstance(barType, periods, smaCore, this.OnCalculationCompleted, stdMultiplier);
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

        [IndicatorPlotting("upper", ChartTypeOption.Line, ChartRangeOption.PriceActionRange)]
        public double UpperBand(int index = 0)
        {
            return core.HasValue(index) ? core.Last(index).Values[BollingerBandsCore.UPPER_BAND_FIELD_INDEX] : double.NaN;
        }

        [IndicatorPlotting("middle", ChartTypeOption.Line, ChartRangeOption.PriceActionRange)]
        public double MiddleBand(int index = 0)
        {
            return core.HasValue(index) ? core.Last(index).Values[BollingerBandsCore.MIDDLE_BAND_FIELD_INDEX] : double.NaN;
        }

        [IndicatorPlotting("lower", ChartTypeOption.Line, ChartRangeOption.PriceActionRange)]
        public double LowerBand(int index = 0)
        {
            return core.HasValue(index) ? core.Last(index).Values[BollingerBandsCore.LOWER_BAND_FIELD_INDEX] : double.NaN;
        }
    }
}
