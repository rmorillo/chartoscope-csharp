using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class CCI: BuiltinIndicatorBase<CCICore>, IIndicatorCore
    {
        private int periods;

        public int Periods
        {
            get { return periods; }
        }

        public CCI(BarItemType barType, int periods)
            : base(barType)
        {
            this.periods = periods;
            this.barType = barType;
        }

        public void BuildCore(ISessionIndicator sessionIndicator)
        {
            string coreIdentityCode = CCICore.CreateIdentityCode(periods);
            core = sessionIndicator.CoreIndicators.ContainsKey(coreIdentityCode) ? sessionIndicator.CoreIndicators[coreIdentityCode].IndicatorInstance as CCICore : null;
            if (core == null)
            {
                string tpmaIdentityCode = TPMACore.CreateIdentityCode(periods);
                TPMACore tpmaCore = sessionIndicator.CoreIndicators.ContainsKey(tpmaIdentityCode) ? sessionIndicator.CoreIndicators[tpmaIdentityCode].IndicatorInstance as TPMACore : null;
                if (tpmaCore == null)
                {
                    tpmaCore = TPMACore.CreateInstance(barType, periods);
                    sessionIndicator.CoreIndicators.Add(tpmaCore.IdentityCode, new CoreIndicator(tpmaCore));
                }

                core = CCICore.CreateInstance(barType, periods, tpmaCore, this.OnCalculationCompleted);
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

        [IndicatorPlotting("cci", ChartTypeOption.Line, ChartRangeOption.PositiveHundredRange)]
        public double Value(int index = 0)
        {
            return core.HasValue(index) ? core.Last(index).Value : double.NaN;
        }
    }
}
