using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;


namespace Chartoscope.Common
{
    public abstract class RingBufferBase<TKey, TItem> : IEnumerable<TItem>, IRingNavigator<TItem>
    {

        protected KeyedCollection<TKey, TItem> _buffer;
        //0-based
        private int current;
        //0-based
        private int nextFree;
        //1-based
        private int maxLength;

        //1-based
        private int addCounter;

        public RingBufferBase(KeyedCollection<TKey, TItem> buffer, int length)
        {
            _buffer = buffer;
            nextFree = 0;
            maxLength = length;
            addCounter = 0;
            current = -1;
        }

        public void Add(TItem item)
        {
            if (_buffer.Count < maxLength)
            {
                _buffer.Add(item);
                current = nextFree;
                nextFree = _buffer.Count;
            }
            else
            {
                _buffer[nextFree] = item;
                current = nextFree;
                nextFree += 1;
            }

            if (nextFree >= maxLength)
            {
                current = nextFree - 1;
                nextFree = 0;
            }

            //Just in case
            if (addCounter == int.MaxValue)
            {
                addCounter = 0;
            }

            addCounter += 1;
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            return _buffer.GetEnumerator();
        }

        public IEnumerator GetEnumerator1()
        {
            return _buffer.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator1();
        }

        //1-based
        public int NextCount
        {
            get { return addCounter + 1; }
        }

        //0-based
        public int NextRingIndex
        {
            get { return nextFree; }
        }

        public int CurrentRingIndex
        {
            get { return current; }
        }

        //ringIndex is 0-based
        public TItem GetItem(int ringIndex)
        {
            return ringIndex<0? default(TItem): _buffer[ringIndex];
        }

        public int PreviousRingIndex(int offset)
        {
            int retVal= int.MinValue;

            int diff= current - offset; //TODO:  Consider rolling over if diff > maxLength
            if (diff < 0 && _buffer.Count==maxLength)
            {
                retVal= _buffer.Count + diff;
            }
            else if (diff >= 0)
            {
                retVal = diff;
            }

            return retVal;
        }

        public TItem GetItem(TKey key)
        {
            TItem item = default(TItem);
            if (_buffer.Contains(key))
            {
                item = _buffer[key];
            }

            return item;
        }

        public TItem Last(int count = 0)
        {

            TItem item= default(TItem);

            int targetRingIndex= PreviousRingIndex(count);
            if (HasValue(count))
            {
                item = GetItem(targetRingIndex);
            }

            return item;
        }

        public TItem LastItem { get { return this.Last(); } }


        public bool HasValue(int count=0)
        {
            return PreviousRingIndex(count) >=0;
        }
    }
}