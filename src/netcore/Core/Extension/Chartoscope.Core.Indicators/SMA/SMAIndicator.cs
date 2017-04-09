using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Indicators
{
    public class SMAIndicator : LookBehindPool<ISMAItem>
    {
        private SMACalculator _smaPool;
        public SMAIndicator(int capacity, int period) : base(capacity)
        {
            _smaPool = new SMACalculator(capacity, period);
        }

        public void Write(long timestamp, double value)
        {
            ((SMAItem)NextPoolItem).Update(timestamp, value);
            MoveNext(); 
        }

        protected override ISMAItem CreatePoolItem()
        {
            return new SMAItem(DateTime.MinValue.Ticks, long.MinValue);
        }
    }
}
