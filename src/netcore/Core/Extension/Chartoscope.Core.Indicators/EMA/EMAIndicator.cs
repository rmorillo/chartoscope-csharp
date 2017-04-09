using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Indicators
{
    public class EMAIndicator : LookBehindPool<IEMAItem>
    {
        private EMACalculator _emaPool;
        
        public EMAIndicator(int capacity, int period):base(capacity)
        {
            _emaPool = new EMACalculator(capacity, period);
        }
       

        public IEMAItem Calculate(long timestamp, double price)
        {
            double result = _emaPool.Calculate(price);

            if (result==double.NaN)
            {
                return null;
            }
            else
            {
                ((EMAItem)NextPoolItem).Write(timestamp, result);
                MoveNext();

                return Current;
            }            
        }

        protected override IEMAItem CreatePoolItem()
        {
            return new EMAItem();
        }
    }
}
