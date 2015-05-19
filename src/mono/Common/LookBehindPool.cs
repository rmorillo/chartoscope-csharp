using System;
using System.Linq;

namespace Chartoscope.Common
{
	/// <summary>
	/// A special kind of object pool where items are accessed from the last position and is useful for storing historical and time series data.
	/// Works like a circular buffer such that when maximum capacity reached, the last position rolls over to the start of the collection and overwrites the previous data.
	/// </summary>
    public class LookBehindPool<T>: ILookBehindReader<T>
    {
#region Constants
		private const string InvalidLengthException="Length must be greater than zero.";
		private const string IndexOutOfRangeException="Index out of range. Value must be from 0 to {0}";
#endregion
		
#region Member variables
		private T[] _poolItems = null;
        private int _position = 0;
		private long _sequence= 0;
        private int _lastPosition = -1;
        private int _capacity = 0;
        private int _count = 0;
        private bool _rolledOver = false;

        protected int offset = 1;
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
            if (capacity <= 0)
                throw new Exception(InvalidLengthException);
			
			_poolItems= new T[capacity];
			
			//Fills the object pool
			for(int i=0; i<capacity; i++)
			{
				_poolItems[i]= CreatePoolItem();
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
        public void Write(T value)
        {
            _poolItems[_position] = value;

            MoveNext();
        }
		
		/// <summary>
		/// Moves object pool index to the next position.  Rolls over to index zero when Capacity is reached.
		/// </summary>
        public void MoveNext()
        {
            _lastPosition = _position;

            if (!_rolledOver)
            {
                _count++;
            }

            if (_position < (_capacity - 1))
            {
                _position += offset;
            }
            else
            {
                _position = 0;
                _rolledOver = true;
            }
			
			if (_sequence==long.MaxValue)
			{
				_sequence=1;
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
		protected virtual T CreatePoolItem()
        {
            return default(T);
        }
		
		/// <summary>
		/// Resets the object pool item with the specified instance.
		/// </summary>
		/// <returns>
		/// The object pool item.
		/// </returns>
		/// <param name='poolItem'>
		/// Object pool item.
		/// </param>
		protected virtual T ResetPoolItem(T poolItem)
        {
            return poolItem;
        }
		
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
            int targetIndex = _lastPosition - relativeIndex;

            if (targetIndex < 0)
            {
                int absoluteIndex = targetIndex + _capacity;

                if (_rolledOver && absoluteIndex > _lastPosition)
                {
                    targetIndex = absoluteIndex;
                }
                else
                    throw new Exception(string.Format(IndexOutOfRangeException, _count - 1));
            }

            return targetIndex;
        }
		
		/// <summary>
		/// Determines whether specified index is valid and accessible
		/// </summary>
		/// <returns>
		/// true if specified index is valid and accessible; otherwise, false.
		/// </returns>
		/// <param name='relativeIndex'>
		/// The index relative to last position
		/// </param>
        private bool IsValidIndex(int relativeIndex)
        {
            return (_lastPosition - relativeIndex) >= 0 || (_rolledOver && (_lastPosition - relativeIndex + _capacity) > _lastPosition);
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
                return _poolItems[GetAbsoluteIndex(index)];
            }
        }
		
		/// <summary>
		/// Gets the next item in the object pool
		/// </summary>
		/// <value>
		/// The next object pool item
		/// </value>
		public T NextPoolItem
        {
            get { return ResetPoolItem(_poolItems[_position]); }
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
                return _poolItems[_lastPosition];
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
                return _poolItems[GetAbsoluteIndex(1)];
            }
        }
		
		/// <summary>
		/// Gets the length of filled items in the object pool.  Count will stop incrementing until Capacity is reached.
		/// </summary>
		/// <value>
		/// The count.
		/// </value>
		public int Count { get { return _count; } }
		
		/// <summary>
		/// Gets the position of the current object pool item.
		/// </summary>
		/// <value>
		/// The position of the current object pool item.
		/// </value>
        public long Sequence { get { return _position; } }
		
		/// <summary>
		/// Gets the capacity or size of the object pool.
		/// </summary>
		/// <value>
		/// The capacity or size of the object pool.
		/// </value>
		public int Capacity { get { return _capacity; } }	
#endregion        
    }
}


