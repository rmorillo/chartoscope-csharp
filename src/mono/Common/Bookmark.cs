using System;

namespace Chartoscope.Common
{
	public class Bookmark<T>: ILookBehindReader<T>
	{
		private long _sequence;
		private ILookBehindReader<T> _reader;
		
		internal Bookmark ()
		{			
		}
		
		public void Set(ILookBehindReader<T> reader)
		{
			_reader= reader;
			_sequence= reader.Sequence;
		}
		
		private int BookmarkedIndex
		{
			get
			{
				int diff= (int) (_reader.Sequence-_sequence);
				if (diff<0)
					return 0;
				else
					return diff;
			}
		}

		#region ILookBehindReader[IPriceBar] implementation
		public T this[int index] {
			get {
				return _reader[BookmarkedIndex+index];
			}
		}

		public T Current {
			get {
				return _reader[BookmarkedIndex];
			}
		}
		
		public T Previous {
			get {
				return _reader[BookmarkedIndex + 1];
			}
		}

		public int Count {
			get {
				return _reader.Count-BookmarkedIndex;
			}
		}
		
		public int Capacity {
			get {
				return _reader.Capacity;
			}
		}
		
		public long Sequence {
			get {
				return _reader.Sequence;
			}
		}
		#endregion
	}
}

