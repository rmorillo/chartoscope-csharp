using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    [IndicatorIdentity(ADXCore.SHORT_NAME, ADXCore.DISPLAY_NAME, ADXCore.DESCRIPTION)]
    public class ADX : BuiltinIndicatorBase<ADXCore>, IIndicatorCore
    {
        private int periods;

        public int Periods
        {
            get { return periods; }
        }

        public ADX(BarItemType barType, int periods)
            : base(barType)
        {
            this.periods = periods;
            this.barType = barType;
        }

        public void BuildCore(ISessionIndicator sessionIndicator)
        {
            string coreIdentityCode = ADXCore.CreateIdentityCode(periods);
            core = sessionIndicator.CoreIndicators.ContainsKey(coreIdentityCode) ? sessionIndicator.CoreIndicators[coreIdentityCode].IndicatorInstance as ADXCore : null;
            if (core == null)
            {
                string trueRangeIdentityCode = TrueRangeCore.CreateIdentityCode();
                TrueRangeCore trueRangeCore = sessionIndicator.CoreIndicators.ContainsKey(trueRangeIdentityCode) ? sessionIndicator.CoreIndicators[trueRangeIdentityCode].IndicatorInstance as TrueRangeCore : null;
                if (trueRangeCore == null)
                {
                    trueRangeCore = TrueRangeCore.CreateInstance(barType);
                    sessionIndicator.CoreIndicators.Add(trueRangeCore.IdentityCode, new CoreIndicator(trueRangeCore));
                }

                string dmIdentityCode = DirectionalMovementCore.CreateIdentityCode();
                DirectionalMovementCore dmCore = sessionIndicator.CoreIndicators.ContainsKey(dmIdentityCode) ? sessionIndicator.CoreIndicators[dmIdentityCode].IndicatorInstance as DirectionalMovementCore : null;
                if (dmCore == null)
                {
                    dmCore = DirectionalMovementCore.CreateInstance(barType);
                    sessionIndicator.CoreIndicators.Add(dmCore.IdentityCode, new CoreIndicator(dmCore));
                }

                string dxIdentityCode = DirectionalMovementIndexCore.CreateIdentityCode(periods);
                DirectionalMovementIndexCore dxCore = sessionIndicator.CoreIndicators.ContainsKey(dxIdentityCode) ? sessionIndicator.CoreIndicators[dxIdentityCode].IndicatorInstance as DirectionalMovementIndexCore : null;
                if (dxCore == null)
                {
                    dxCore = DirectionalMovementIndexCore.CreateInstance(barType, periods, trueRangeCore, dmCore);
                    sessionIndicator.CoreIndicators.Add(dxCore.IdentityCode, new CoreIndicator(dxCore));
                }

                core = ADXCore.CreateInstance(barType, periods, dxCore, this.OnCalculationCompleted);
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

        [IndicatorPlotting("[adx]", ChartTypeOption.Line, ChartRangeOption.PositiveHundredRange)]
        public double ADXValue(int index = 0)
        {
            return core.HasValue(index)? core.Last(index).Values[ADXCore.ADX_FIELD_INDEX]:double.NaN;
        }

        [IndicatorPlotting("[+di]", ChartTypeOption.Line, ChartRangeOption.PositiveHundredRange)]
        public double PlusDI(int index = 0)
        {
            return core.HasValue(index)? core.Last(index).Values[ADXCore.PLUS_DI_FIELD_INDEX]:double.NaN;
        }

        [IndicatorPlotting("[-di]", ChartTypeOption.Line, ChartRangeOption.PositiveHundredRange)]
        public double MinusDI(int index = 0)
        {
            return core.HasValue(index)? core.Last(index).Values[ADXCore.MINUS_DI_FIELD_INDEX]:double.NaN;
        }   
    }
}
