using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chartoscope.Core.Library.UnitTest
{
    [TestClass]
    public class RenkoBarTest
    {
        [TestMethod]
        public void Renko_Constructor_Works()
        {            
            Assert.IsNotNull(new RenkoBars(100, 10, 1));
            //TODO Test with mocked IPersistenceWriter 
            //Assert.IsNotNull(new RenkoBars(100, 10, 1, mockPersistenceWriter.Objevt));
        }

        [TestMethod]
        public void Renko_Write_Works()
        {
            //Arrange
            var renko = new RenkoBars(100, 10, 1);

            //Act
            renko.Write(DateTime.Now.Ticks, 1);
            renko.Write(DateTime.Now.Ticks, 2);

            //Assert
            Assert.AreEqual(0, renko.Length);            
        }

        [TestMethod]
        public void Renko_Write_GeneratesBullishRenkoBar()
        {
            //Arrange
            var renko = new RenkoBars(100, 10, 1);

            //Act
            renko.Write(DateTime.Now.Ticks, 1);
            renko.Write(DateTime.Now.Ticks, 2);
            renko.Write(DateTime.Now.Ticks, 5);
            renko.Write(DateTime.Now.Ticks, 10);

            //Assert
            Assert.AreEqual(1, renko.Length);
            Assert.IsTrue(renko.Current.Open < renko.Current.Close);
        }

        [TestMethod]
        public void Renko_Write_GeneratesBearishRenkoBar()
        {
            //Arrange
            var renko = new RenkoBars(100, 10, 1);

            //Act
            renko.Write(DateTime.Now.Ticks, 10);
            renko.Write(DateTime.Now.Ticks, 2);
            renko.Write(DateTime.Now.Ticks, 5);
            renko.Write(DateTime.Now.Ticks, 1);

            //Assert
            Assert.AreEqual(1, renko.Length);
            Assert.IsTrue(renko.Current.Open > renko.Current.Close);
        }
    }
}
