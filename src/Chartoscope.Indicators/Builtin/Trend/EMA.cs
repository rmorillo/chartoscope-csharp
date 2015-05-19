using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    [IndicatorIdentity(EMACore.SHORT_NAME, EMACore.DISPLAY_NAME, EMACore.DESCRIPTION)]
    public class EMA: BuiltinIndicatorBase<EMACore>, IIndicatorCore
    {            
        private int periods;

        public int Periods
        {
            get { return periods; }
        }

        public EMA(BarItemType barType, int periods): base(barType)
        {
            this.periods = periods;
            this.barType = barType;
        }

        public void BuildCore(ISessionIndicator sessionIndicator)
        {
            string coreIdentityCode= EMACore.CreateIdentityCode(periods);
            core = sessionIndicator.CoreIndicators.ContainsKey(coreIdentityCode)? sessionIndicator.CoreIndicators[coreIdentityCode].IndicatorInstance as EMACore: null;
            if (core == null)
            {               
                core = EMACore.CreateInstance(barType, periods, this.OnCalculationCompleted);
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

        [IndicatorPlotting("ema", ChartTypeOption.Line, ChartRangeOption.PriceActionRange)]
        public double Value(int index = 0)
        {
            return core.HasValue(index) ? core.Last(index).Value : double.NaN;
        }
    }
}
