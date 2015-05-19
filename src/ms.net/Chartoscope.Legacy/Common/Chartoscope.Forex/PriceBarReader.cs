using System;

namespace Chartoscope.Common
{
	public class PriceBarReader: ILookBehindReader<IPriceBar>, IBookmarkable<IPriceBar>
	{
		private Bookmarks<IPriceBar> _bookMarks;
		private LookBehindPool<PriceBar> _priceBars;
					
		public PriceBarReader (LookBehindPool<PriceBar> priceBars)
		{
			_priceBars= priceBars;
			_bookMarks= new Bookmarks<IPriceBar>(priceBars.Capacity, this);
		}				

		public Bookmark<IPriceBar> GetBookmark()
		{
			_bookMarks.MoveNext();
			_bookMarks.Current.Set(this);			
			return _bookMarks.Current;
		}
		
		#region ILookbackReader[IPriceBar] implementation
		public IPriceBar this[int index] {
			get {
				return _priceBars[index];
			}
		}

		public IPriceBar Current {
			get {
				return _priceBars.Current;
			}
		}
		
		public IPriceBar Previous {
			get {
				return _priceBars.Previous;
			}
		}

		public int Count {
			get {
				return _priceBars.Count;
			}
		}
		
		public int Capacity {
			get {
				return _priceBars.Capacity;
			}
		}
		
		public long Sequence {
			get {
				return _priceBars.Sequence;
			}
		}

        public int Position
        {
            get { return _priceBars.Position; }
        }
		#endregion        
    }
}

