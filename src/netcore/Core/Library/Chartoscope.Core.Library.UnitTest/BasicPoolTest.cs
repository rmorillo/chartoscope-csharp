using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chartoscope.Core.Library.UnitTest
{
    [TestClass]
    public class BasicPoolTest
    {
        [TestMethod]
        public void NumericBytePool_Write_Works()
        {
            //Arrange
            var pool = new NumericBytePool(10);

            //Act
            pool.Write(89);

            //Assert
            Assert.AreEqual(pool[0], 89);
        }

        [TestMethod]
        public void NumericInt16Pool_Write_Works()
        {
            //Arrange
            var pool = new NumericInt16Pool(10);

            //Act
            pool.Write(89);

            //Assert
            Assert.AreEqual(pool[0], 89);
        }

        [TestMethod]
        public void NumericInt32Pool_Write_Works()
        {
            //Arrange
            var pool = new NumericInt32Pool(10);

            //Act
            pool.Write(89);

            //Assert
            Assert.AreEqual(pool[0], 89);
        }

        [TestMethod]
        public void NumericInt64Pool_Write_Works()
        {
            //Arrange
            var pool = new NumericInt64Pool(10);

            //Act
            pool.Write(89);

            //Assert
            Assert.AreEqual(pool[0], 89);
        }

        [TestMethod]
        public void NumericDoublePool_Write_Works()
        {
            //Arrange
            var pool = new NumericDoublePool(10);

            //Act
            pool.Write(89);

            //Assert
            Assert.AreEqual(pool[0], 89);
        }

        [TestMethod]
        public void NumericFloatPool_Write_Works()
        {
            //Arrange
            var pool = new NumericFloatPool(10);

            //Act
            pool.Write(89);

            //Assert
            Assert.AreEqual(pool[0], 89);
        }

        [TestMethod]
        public void NumericDecimalPool_Write_Works()
        {
            //Arrange
            var pool = new NumericDecimalPool(10);

            //Act
            pool.Write(89);

            //Assert
            Assert.AreEqual(pool[0], 89);
        }

        [TestMethod]
        public void BooleanLogicPool_Write_Works()
        {
            //Arrange
            var pool = new BooleanLogicPool(10);

            //Act
            pool.Write(true);

            //Assert
            Assert.AreEqual(pool[0], true);
        }

        [TestMethod]
        public void StringBuilderPool_Write_Works()
        {
            //Arrange
            var pool = new StringBuilderPool(10);

            //Act
            pool.Write("abc");            

            //Assert            
            Assert.AreEqual(pool[0].ToString(), "abc");
        }

        [TestMethod]
        public void TimestampPool_Write_Works()
        {
            //Arrange
            var pool = new TimestampPool(10);
            var currentTime = DateTime.Now;
            //Act
            pool.Write(currentTime);

            //Assert
            Assert.AreEqual(pool[0], currentTime);
        }
    }
}
