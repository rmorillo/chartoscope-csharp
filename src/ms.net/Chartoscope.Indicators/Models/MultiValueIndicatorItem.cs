using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Indicators
{
    public class MultiValueIndicatorItem : IIndicatorItem
    {
        private int _defaultValueIndex = 0;

        public MultiValueIndicatorItem(DateTime timeStamp, int defaultValueIndex, params double[] values)
        {
            this._defaultValueIndex = defaultValueIndex;
            this._key = timeStamp;
            this._values = values;
        }

        public MultiValueIndicatorItem(DateTime timeStamp, params double[] values)
        {        
            this._key = timeStamp;
            this._values = values;
        }

        private double _value = double.NaN;
        public double Value
        {
            get
            {
                return _values[_defaultValueIndex];
            }          
        }

        public DateTime _key = DateTime.MinValue;
        public DateTime Key
        {
            get { return _key; }
        }


        private double[] _values;
        public double[] Values
        {
            get
            {
                return _values;
            }          
        }
    }
}
