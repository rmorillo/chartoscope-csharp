using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class IndicatorProbe<TValue, TIndicator> : ILookBehindReader<TValue> where TIndicator : ILookBehindReader<TValue>
    {
        protected TIndicator _Probe;
        public TValue this[int index]
        {
            get
            {
                return _Probe[index];
            }
        }

        public TValue Current
        {
            get
            {
                return _Probe.Current;
            }
        }

        public int Length
        {
            get
            {
                return _Probe.Length;
            }
        }

        public int Position
        {
            get
            {
                return _Probe.Position;
            }
        }

        public TValue Previous
        {
            get
            {
                return _Probe.Previous;
            }
        }

        public long Sequence
        {
            get
            {
                return _Probe.Sequence;
            }
        }
    }
}
