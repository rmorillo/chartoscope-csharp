using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class BuiltinIndicatorBase<TIndicatorBase>
    {
        internal TIndicatorBase core;

        protected internal TIndicatorBase Core
        {
            get { return core; }
            set { core = value; }
        }        

        protected BarItemType barType;

        public BarItemType BarType
        {
            get { return barType; }
            set { barType = value; }
        }

        public event IndicatorDelegates.NewIndicatorValueHandler NewIndicatorValue;

        public BuiltinIndicatorBase(BarItemType barType)
        {
            this.barType = barType;
        }

        public void OnCalculationCompleted()
        {
            if (NewIndicatorValue != null)
            {
                NewIndicatorValue(this, barType);
            }
        }             
    }
}
