using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chartoscope.Core.Library.UnitTest
{
    [TestClass]
    public class LookBehindPoolPrimitiveTypeTest
    {
        [TestMethod]
        public void LookBehindPool_PrimitiveType_Constructor_Works()
        {
            //Arrange & Act
            int capacity = 10;        
            var numberPool = new PrimitiveTypePool(capacity);
            
            //Assert
            Assert.IsNotNull(numberPool.Length == 0);
            Assert.IsNotNull(numberPool.Capacity == capacity);
        }

        [TestMethod]
        public void LookBehindPool_PrimitiveType_Write_Works()
        {
            //Arrange
            var poolValue = 45;
            var stringPool = new PrimitiveTypePool(10);

            //Act
            stringPool.Write(poolValue);

            //Assert
            Assert.AreEqual(stringPool.Length, 1);            
            Assert.AreEqual(stringPool[0], poolValue);
            Assert.AreEqual(stringPool.Current, poolValue);
        }

        [TestMethod]
        public void LookBehindPool_PrimitiveType_WriteRollover_Works()
        {
            //Arrange
            var boolPool = new PrimitiveTypePool(2);

            //Act
            boolPool.Write(1);
            boolPool.Write(1);
            boolPool.Write(0);

            //Assert
            Assert.AreEqual(boolPool[0], 0);
            Assert.AreEqual(boolPool.Previous, 1);
            Assert.AreEqual(boolPool.Length, 2);           
        }

        [TestMethod]
        public void LookBehindPool_PrimitiveType_OutOfRangeIndexThrowsAnException()
        {
            //Arrange
            var floatPool = new PrimitiveTypePool(10);
            floatPool.Write(1);

            //Act & Assert

            Assert.ThrowsException<IndexOutOfRangeException>(() => floatPool[1]); 
        }
    }
}
