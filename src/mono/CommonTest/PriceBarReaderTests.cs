using System;
using NUnit.Framework;

namespace Chartoscope.Common.Test
{
	[TestFixture()]
	public class PriceBarReaderTests
	{
		[Test()]
		public void BasicTest ()
		{
			PriceBars priceBars= new PriceBars(10);
			PriceBarReader reader= new PriceBarReader(priceBars);
			
			priceBars.NextPoolItem.Write(DateTime.Now, 1, 2, 3, 4);
			priceBars.MoveNext();
			
			Bookmark<IPriceBar> bookmark= reader.GetBookmark();
			
			Assert.AreEqual(priceBars.Current.Open, bookmark.Current.Open);
			
			priceBars.NextPoolItem.Write(DateTime.Now, 5, 6, 7, 8);
			priceBars.MoveNext();	
			
			Assert.AreNotEqual(priceBars.Current.Open, bookmark.Current.Open);
			
		}
	}
}

