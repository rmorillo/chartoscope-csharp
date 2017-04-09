using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Indicators
{
    public class EMACalculator : LookBehindPool<double>
    {
        private SMACalculator sma;
                
        public EMACalculator(int capacity, int period) : base(capacity)
        {
            sma = new SMACalculator(capacity, period);
        }

        public double Calculate(double price)
        {
            var average= sma.Calculate(price);   
            if (double.IsNaN(average))
            {
                return double.NaN;
            }
            else
            {
                WriteCopy(average);

                return average;
            }
            
        }

        protected override double CreatePoolItem()
        {
            return double.NaN;
        }
    }
}
