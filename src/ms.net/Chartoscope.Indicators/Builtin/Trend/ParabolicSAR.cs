using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    [IndicatorIdentity(ParabolicSARCore.SHORT_NAME, ParabolicSARCore.DISPLAY_NAME, ParabolicSARCore.DESCRIPTION)]
    public class ParabolicSAR: BuiltinIndicatorBase<ParabolicSARCore>, IIndicatorCore
    {            
        private int periods;

        public int Periods
        {
            get { return periods; }
        }

        public ParabolicSAR(BarItemType barType, int periods)
            : base(barType)
        {
            this.periods = periods;
            this.barType = barType;
        }

        public void BuildCore(ISessionIndicator sessionIndicator)
        {
            string coreIdentityCode= ParabolicSARCore.CreateIdentityCode(periods);
            core = sessionIndicator.CoreIndicators.ContainsKey(coreIdentityCode) ? sessionIndicator.CoreIndicators[coreIdentityCode].IndicatorInstance as ParabolicSARCore : null;
            if (core == null)
            {
                core = ParabolicSARCore.CreateInstance(barType, periods, this.OnCalculationCompleted);
                sessionIndicator.CoreIndicators.Add(core.IdentityCode, new CoreIndicator(core));
            }
        }

        BarItemType IIndicatorCore.BarType
        {
            get { return this.barType; }
        }

        public string IdentityCode
        {
            get 
            { 
                return core.IdentityCode; 
            }
        }

        [IndicatorPlotting("sar", ChartTypeOption.Point, ChartRangeOption.PriceActionRange)]
        public double CurrentSAR(int index = 0)
        {
            return core.HasValue(index) ? core.Last(index).Values[ParabolicSARCore.CURRENT_SAR_FIELD_INDEX] : double.NaN ;
        }

        
        public double NextSAR(int index = 0)
        {
            return core.HasValue(index)? core.Last(index).Values[ParabolicSARCore.NEXT_SAR_FIELD_INDEX]: double.NaN;
        }

        public double EP(int index = 0)
        {
            return core.HasValue(index) ? core.Last(index).Values[ParabolicSARCore.EP_FIELD_INDEX] : double.NaN;
        }

        public double DeltaEP_SAR(int index = 0)
        {
            return core.HasValue(index) ? core.Last(index).Values[ParabolicSARCore.DELTA_EP_SAR_FIELD_INDEX] : double.NaN;
        }

        public double AF(int index = 0)
        {
            return core.HasValue(index) ? core.Last(index).Values[ParabolicSARCore.AF_FIELD_INDEX] : double.NaN;
        }

        public double Direction(int index = 0)
        {
            return core.HasValue(index) ? core.Last(index).Values[ParabolicSARCore.DIRECTION_FIELD_INDEX] : double.NaN;
        }

        public double DeltaSAR(int index = 0)
        {
            return core.HasValue(index) ? core.Last(index).Values[ParabolicSARCore.DELTA_SAR_FIELD_INDEX] : double.NaN;
        }        
    }
}
