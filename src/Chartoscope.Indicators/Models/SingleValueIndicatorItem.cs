using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class SingleValueIndicatorItem: IIndicatorItem
    {
        public SingleValueIndicatorItem(DateTime timeStamp, double value)
        {        
            this._key = timeStamp;
            this._value = value;
        }

        private double _value = double.NaN;
        public double Value
        {
            get
            {
                return _value;
            }
        }

        public DateTime _key = DateTime.MinValue;
        public DateTime Key
        {
            get { return _key; }
        }


        public double[] Values
        {
            get
            {
                return new double[] {_value} ;
            }
        }
    }
}
