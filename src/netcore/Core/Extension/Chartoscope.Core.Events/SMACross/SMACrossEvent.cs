using Chartoscope.Core.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Events
{
    public class SMACrossEvent: LookBehindPool<ISMACrossValue>
    {
        private SMACalculator _sma1;
        private SMACalculator _sma2;
        private int _maxPeriod;
        private Action _crossed;

        public SMACrossEvent(int capacity, int firstSMAPeriod, int secondSMAPeriod, Action crossed):base(capacity)
        {
            _sma1 = new SMACalculator(capacity, firstSMAPeriod);
            _sma2 = new SMACalculator(capacity, secondSMAPeriod);
            _maxPeriod = Math.Max(firstSMAPeriod, secondSMAPeriod);
            _crossed = crossed;
        }

        public void Evaluate(long timestamp, double value)
        {
            _sma1.Calculate(value);
            _sma2.Calculate(value);

            if (IsPrimed())
            {
                if (_sma1.Previous > _sma2.Current && _sma1.Current > _sma2.Previous)
                {
                    Write(timestamp, EventStateOption.On);
                    _crossed();                    

                }
                else if (_sma2.Previous > _sma1.Current && _sma2.Current > _sma1.Previous)
                {
                    Write(timestamp, EventStateOption.On);
                    _crossed();                    
                }
                else
                {
                    Write(timestamp, EventStateOption.Off);
                }
            }            
        }

        private bool IsPrimed()
        {
            return _sma1.Length >= _maxPeriod && _sma2.Length >= _maxPeriod;
        }

        public void Write(long timestamp, EventStateOption status)
        {
            var smaCrossValue = (SMACrossValue)NextPoolItem;
            smaCrossValue.Write(timestamp, status);
            MoveNext();
        }

        protected override ISMACrossValue CreatePoolItem()
        {
            return new SMACrossValue();
        }
    }
}
