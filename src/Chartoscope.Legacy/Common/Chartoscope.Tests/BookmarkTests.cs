using System;
using Chartoscope.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.Test
{
    [TestClass]
	public class BookmarkTests
	{
        [TestMethod]
		public void TestBookmarkShouldRetainIndexReference()
		{
			LookBehindPool<byte> lbp= new LookBehindPool<byte>(10);

			lbp.Write(1);
			lbp.Write(2);
			lbp.Write(3);

			Bookmark<byte> bm= new Bookmark<byte>();
			bm.Set(lbp);

			lbp.Write(4);
			lbp.Write(5);
			lbp.Write(6);

			Assert.AreEqual(6, lbp.Current);
			Assert.AreEqual(3, bm.Current);
		}

        [TestMethod, ExpectedException(typeof(Exception), "Bookmark has expired")]
		public void TestAccessToExpiredBookmarkUsingPreviousShouldThrowAnException()
		{
			LookBehindPool<byte> lbp= new LookBehindPool<byte>(3);

			lbp.Write(1);
			lbp.Write(2);
			lbp.Write(3);

			Bookmark<byte> bm= new Bookmark<byte>();
			bm.Set(lbp);

			lbp.Write(4);
			lbp.Write(5);

			Assert.AreEqual(5, lbp.Current);
			Assert.AreEqual(2, bm.Previous);
		}

        [TestMethod, ExpectedException(typeof(Exception), "Bookmark has expired")]
        public void TestAccessToExpiredBookmarkUsingCurrentShouldThrowAnException()
		{
			LookBehindPool<byte> lbp= new LookBehindPool<byte>(3);

			lbp.Write(1);
			lbp.Write(2);
			lbp.Write(3);

			Bookmark<byte> bm= new Bookmark<byte>();
			bm.Set(lbp);

			lbp.Write(4);
			lbp.Write(5);
			lbp.Write(6);

			Assert.AreEqual(6, lbp.Current);
			Assert.AreEqual(6, bm.Current);
		}
	}
}

