using System;

namespace Chartoscope.Core.Common
{
	public class Bookmark<T>: ILookBehindReader<T>
	{
		#region ILookBehindReader implementation

		public int Position {
			get {
				return _reader.Position;
			}
		}

		#endregion

		private long _sequence;
		private ILookBehindReader<T> _reader;
		
		public Bookmark (ILookBehindReader<T> reader)
		{
            _reader = reader;
            _sequence = reader.Sequence;
        }
		
		
		private int BookmarkedIndex
		{
			get
			{
				int diff= (int) (_reader.Position-_sequence);
				if (diff<0)
					return diff + _reader.Length;
				else
					return diff;
			}
		}

		#region ILookBehindReader[IPriceItem] implementation
		public T this[int index] {
			get
            {
                CheckExpiry(index);
                return _reader[BookmarkedIndex+index];
			}
		}

		public T Current {
			get
            {
                CheckExpiry(0);
                return _reader[BookmarkedIndex];
			}
		}
		
		public T Previous {
			get
            {
                CheckExpiry(1);
				return _reader[BookmarkedIndex + 1];
			}
		}

        private void CheckExpiry(int index)
        {
            if (_reader.Sequence / _reader.Length >= 2 || BookmarkedIndex + index > _reader.Length - 1)
            {
                throw new Exception("Bookmark has expired");
            }            
        }
		
		
		public long Sequence {
			get {
				return _reader.Sequence;
			}
		}


        public int Length
        {
            get
            {
                return _reader.Length;
            }
        }
        #endregion
    }
}

