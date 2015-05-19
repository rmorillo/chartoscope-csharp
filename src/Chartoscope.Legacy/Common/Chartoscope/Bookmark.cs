using System;

namespace Chartoscope.Common
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
		
		public Bookmark ()
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
				int diff= (int) (_reader.Position-_sequence);
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
				if (_reader.Sequence/_reader.Count>=2)
				{
					throw new Exception("Bookmark has expired");
				}

				return _reader[BookmarkedIndex];
			}
		}
		
		public T Previous {
			get {
				if ((_reader.Sequence+1)/_reader.Count>=2)
				{
					throw new Exception("Bookmark has expired");
				}
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

