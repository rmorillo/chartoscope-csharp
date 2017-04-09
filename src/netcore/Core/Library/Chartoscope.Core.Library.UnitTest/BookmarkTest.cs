using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chartoscope.Core.Library.UnitTest
{
    [TestClass]
    public class BookmarkTest
    {
        [TestMethod]
        public void Bookmark_Constructor_Works()
        {
            //Arrange
            PrimitiveTypePool pool = new PrimitiveTypePool(10);

            //Act
            Bookmark<double> bm = new Bookmark<double>(pool);

            //Assert
            Assert.AreEqual(0, bm.Sequence);
            Assert.AreEqual(0, bm.Length);
            Assert.AreEqual(0, bm.Position);
        }

        [TestMethod]
        public void Bookmark_Current_MatchesSourcePoolAfterSubsequentWrites()
        {
            //Arrange
            PrimitiveTypePool pool = new PrimitiveTypePool(10);

            pool.Write(1);
            pool.Write(2);
            pool.Write(3);

            Bookmark<double> bm = new Bookmark<double>(pool);

            //Act
            pool.Write(4);
            pool.Write(5);
            pool.Write(6);

            //Assert
            Assert.AreEqual(6, pool.Current);
            Assert.AreEqual(3, bm.Current);
            Assert.AreEqual(2, bm.Previous);
            Assert.AreEqual(3, bm[0]);
        }

        [TestMethod]
        public void Bookmark_Current_ExpirationThrowsAnException()
        {
            //Arrange
            PrimitiveTypePool pool = new PrimitiveTypePool(3);

            pool.Write(1);
            pool.Write(2);
            pool.Write(3);

            Bookmark<double> bm = new Bookmark<double>(pool);

            //Act
            pool.Write(4);
            pool.Write(5);
            pool.Write(6);

            //Assert
            Assert.ThrowsException<Exception>(() => bm.Current);            
        }

        [TestMethod]
        public void Bookmark_Current_BeforeExpirationWorks()
        {
            //Arrange
            PrimitiveTypePool pool = new PrimitiveTypePool(3);

            pool.Write(1);
            pool.Write(2);
            pool.Write(3);

            Bookmark<double> bm = new Bookmark<double>(pool);
            Assert.AreEqual(3, bm.Current);

            //Act
            pool.Write(4);
            pool.Write(5);            

            //Assert
            Assert.AreEqual(3, bm.Current);
        }

        [TestMethod]
        public void Bookmark_Previous_BeforeExpirationWorks()
        {
            //Arrange
            PrimitiveTypePool pool = new PrimitiveTypePool(3);

            pool.Write(1);
            pool.Write(2);
            pool.Write(3);

            Bookmark<double> bm = new Bookmark<double>(pool);
            Assert.AreEqual(3, bm.Current);

            //Act
            pool.Write(4);

            //Assert
            Assert.AreEqual(2, bm.Previous);
        }

        [TestMethod]
        public void Bookmark_Previous_ExpirationThrowsAnException()
        {
            //Arrange
            PrimitiveTypePool pool = new PrimitiveTypePool(3);

            pool.Write(1);
            pool.Write(2);
            pool.Write(3);

            Bookmark<double> bm = new Bookmark<double>(pool);

            //Act
            pool.Write(4);
            pool.Write(5);

            //Assert
            Assert.ThrowsException<Exception>(() => bm.Previous);
        }

        [TestMethod]
        public void Bookmark_Indexer_ExpirationThrowsAnException()
        {
            //Arrange
            PrimitiveTypePool pool = new PrimitiveTypePool(3);

            pool.Write(1);
            pool.Write(2);
            pool.Write(3);

            Bookmark<double> bm = new Bookmark<double>(pool);

            //Act
            pool.Write(4);
            pool.Write(5);

            //Assert
            Assert.ThrowsException<Exception>(() => bm[1]);
        }
    }
}
