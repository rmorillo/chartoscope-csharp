using System;

namespace Chartoscope.Common
{
	public class Bookmarks<T>: LookBehindPool<Bookmark<T>>
	{
		private ILookBehindReader<T> _reader;
		
		public Bookmarks (int length, ILookBehindReader<T> reader): base(length)
		{
			_reader= reader;
		}		
		
		protected override Bookmark<T> CreatePoolItem ()
		{
			return new Bookmark<T>();
		}
	}
}

