using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chartoscope.Core.Library.UnitTest
{
    [TestClass]
    public class PriceBarTest
    {
        [TestMethod]
        public void PriceBar_Constructor_Works()
        {
            //Arrange, Act & Assert
            Assert.IsNotNull(new PriceBars(10));            
        }

        [TestMethod]
        public void PriceBar_Write_Works()
        {
            //Arrange
            var priceBars = new PriceBars(10);
            var priceBar1 = new PriceBar(DateTime.Now.Ticks, 1, 2, 3, 4);
            var priceBar2 = new PriceBar();
            priceBar2.Write(DateTime.Now.Ticks, 1, 2, 3, 4);
            
            //Act
            priceBars.Write(priceBar1.Timestamp, priceBar1.Open, priceBar1.High, priceBar1.Low, priceBar1.Close);            
            priceBars.Write(priceBar2.Timestamp, priceBar2.Open, priceBar2.High, priceBar2.Low, priceBar2.Close);
            priceBars.Write(priceBar1.Timestamp, priceBar1.Open, priceBar1.High, priceBar1.Low, priceBar1.Close);

            //Assert
            Assert.AreEqual(3, priceBars.Length);
            Assert.AreEqual(priceBars.Current.Timestamp, priceBar1.Timestamp);
            Assert.AreEqual(priceBars.Current.Open, priceBar1.Open);
            Assert.AreEqual(priceBars.Current.High, priceBar1.High);
            Assert.AreEqual(priceBars.Current.Low, priceBar1.Low);
            Assert.AreEqual(priceBars.Current.Close, priceBar1.Close);
            Assert.AreEqual(priceBars.Current.Volume, 0);

            Assert.AreEqual(priceBars.Previous.Timestamp, priceBar2.Timestamp);
            Assert.AreEqual(priceBars.Previous.Open, priceBar2.Open);
            Assert.AreEqual(priceBars.Previous.High, priceBar2.High);
            Assert.AreEqual(priceBars.Previous.Low, priceBar2.Low);
            Assert.AreEqual(priceBars.Previous.Close, priceBar2.Close);
            Assert.AreEqual(priceBars.Previous.Volume, 0);
        }
    }
}
