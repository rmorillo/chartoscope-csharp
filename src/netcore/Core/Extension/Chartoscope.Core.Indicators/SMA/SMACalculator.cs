using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Indicators
{
    public class SMACalculator : LookBehindPool<double>
    {
        private NumericDoublePool _valuePool;
        private int _period;
        public SMACalculator(int capacity, int period) : base(capacity)
        {
            _valuePool = new NumericDoublePool(period);
            _period = period;
        }

        public double Calculate(double value)
        {
            _valuePool.Write(value);            

            if (_valuePool.Length>=_period)
            {
                double sum = 0;
                for (int i = 0; i < _period; i++)
                {
                    sum += _valuePool[i];
                }

                double average = sum / _period;
                WriteCopy(average);

                return average;
            }
            else
            {
                return double.NaN;
            }
        }

        protected override double CreatePoolItem()
        {
            return double.NaN;
        }
    }
}
