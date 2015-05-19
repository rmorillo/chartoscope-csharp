using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    [IndicatorIdentity(SMACore.SHORT_NAME, SMACore.DISPLAY_NAME, SMACore.DESCRIPTION)]
    public class SMA: BuiltinIndicatorBase<SMACore>, IIndicatorCore
    {            
        private int periods;

        public int Periods
        {
            get { return periods; }
        }       

        public SMA(BarItemType barType, int periods): base(barType)
        {
            this.periods = periods;
            this.barType = barType;
        }

        public void BuildCore(ISessionIndicator sessionIndicator)
        {
            string coreIdentityCode= SMACore.CreateIdentityCode(periods);
            core = sessionIndicator.CoreIndicators.ContainsKey(coreIdentityCode) ? sessionIndicator.CoreIndicators[coreIdentityCode].IndicatorInstance as SMACore : null;
            if (core == null)
            {               
                core = SMACore.CreateInstance(barType, periods, this.OnCalculationCompleted);
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

        [IndicatorPlotting("sma", ChartTypeOption.Line, ChartRangeOption.PriceActionRange)]
        public double Value(int index = 0)
        {
            return core.HasValue(index) ? core.Last(index).Value : double.NaN;
        }
    }
}
