using System;
using Chartoscope.Core.Library.Properties;

namespace Chartoscope.Core
{
    /// <summary>
    /// Pre-allocated object pool where items are accessed from the last position and is useful for storing historical and time series data.
    /// Works like a circular buffer such that when maximum capacity reached, the last position rolls over to the start of the collection and overwrites the previous data.
    /// </summary>
    public abstract class LookBehindPool<T> : ILookBehindReader<T>
    {
        #region Member variables
               
        private long _sequence = 0;        
        private int _capacity = 0;        
        private bool _rolledOver = false;

        protected T[] _PoolItems = null;
        protected int _CurrentPosition = 0;
        protected int _LastPosition = -1;
        protected long _MaxSequence = long.MaxValue;
        protected int _Length = 0;
        protected int _Offset = 1;
        
        #endregion

        #region Constructor	
        /// <summary>
        /// Initializes a new instance of the LookBehind class.
        /// </summary>
        /// <param name='capacity'>
        /// The size of the buffer pool
        /// </param>
        public LookBehindPool(int capacity)
        {
            _PoolItems = new T[capacity];

            //Fills the object pool
            for (int i = 0; i < capacity; i++)
            {
                _PoolItems[i] = CreatePoolItem();
            }

            _capacity = capacity;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Write the specified value to the current buffer.
        /// </summary>
        /// <param name='value'>
        /// Value.
        /// </param>
        protected void WriteCopy(T value)
        {
            _PoolItems[_CurrentPosition] = value;

            MoveNext();
        }

        /// <summary>
        /// Moves object pool index to the next position.  Rolls over to index zero when Capacity is reached.
        /// </summary>
        public void MoveNext()
        {
            _LastPosition = _CurrentPosition;

            if (!_rolledOver)
            {
                _Length++;
            }

            if (_CurrentPosition < (_capacity - 1))
            {
                _CurrentPosition += _Offset;
            }
            else
            {
                _CurrentPosition = 0;
                _rolledOver = true;
            }

            if (_sequence == _MaxSequence)
            {
                throw new Exception("Exceeded maximum sequence value");
            }
            else
            {
                _sequence++;
            }
        }

        /// <summary>
        /// Creates the object pool item instance.
        /// </summary>
        /// <returns>
        /// The object pool item instance.
        /// </returns>
        protected abstract T CreatePoolItem();       

        /// <summary>
        /// Gets the absolute object pool index given the relative index
        /// </summary>
        /// <returns>
        /// The absolute object pool index.
        /// </returns>
        /// <param name='relativeIndex'>
        /// Relative index.
        /// </param>
        private int GetAbsoluteIndex(int relativeIndex)
        {
            int targetIndex = _LastPosition - relativeIndex;

            if (targetIndex < 0)
            {
                int absoluteIndex = targetIndex + _capacity;

                if (_rolledOver && absoluteIndex > _LastPosition)
                {
                    targetIndex = absoluteIndex;
                }
                else
                    throw new IndexOutOfRangeException(string.Format(Resource.LookBehindPool_IndexOutOfRangeException, _Length - 1));
            }

            return targetIndex;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets the given type at the specified index.
        /// </summary>
        /// <param name='index'>
        /// Zero-based index
        /// </param>
        public T this[int index]
        {
            get
            {
                return _PoolItems[GetAbsoluteIndex(index)];
            }
        }

        /// <summary>
        /// Gets the next item in the object pool
        /// </summary>
        /// <value>
        /// The next object pool item
        /// </value>
        protected T NextPoolItem
        {
            get { return _PoolItems[_CurrentPosition]; }
        }

        /// <summary>
        /// Gets the current or last object pool item.
        /// </summary>
        /// <value>
        /// The current or last object pool item
        /// </value>
        public T Current
        {
            get
            {
                return _PoolItems[_LastPosition];
            }
        }

        /// <summary>
        /// Gets the previous or 2nd to last object pool item
        /// </summary>
        /// <value>
        /// The previous or 2nd to last object pool item
        /// </value>
        public T Previous
        {
            get
            {
                return _PoolItems[GetAbsoluteIndex(1)];
            }
        }

        /// <summary>
        /// Gets the length of filled items in the object pool.  Count will stop incrementing until Capacity is reached.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Length { get { return _Length; } }
		
		/// <summary>
		/// Gets the position of the current object pool item.
		/// </summary>
		/// <value>
		/// The position of the current object pool item.
		/// </value>
        public int Position { get { return _CurrentPosition; } }
		
        public long Sequence { get { return _sequence; }}

        public int Capacity { get { return _capacity; } }
        #endregion
    }
}


