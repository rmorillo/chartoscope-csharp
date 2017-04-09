using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chartoscope.Core.Library.UnitTest
{
    [TestClass]
    public class CandlestickBarTest
    {
        [TestMethod]
        public void CandlestickBar_Write_Works()
        {
            //Arrange
            var CandlestickBars = new CandlestickBars(1000);
            CandlestickBars.BarUpdated += delegate(ICandlestickBar current) { } ;            
            var priceBar1 = new PriceBar(DateTime.Now.Ticks, 2, 3, 4, 0);
            var priceBar2 = new PriceBar();
            var priceBar3 = new PriceBar(DateTime.Now.Ticks, 1, 2, 3, 4);
            priceBar2.Write(DateTime.Now.Ticks, priceBar3.Open, priceBar3.High, priceBar3.Low, priceBar3.Close);

            //Act
            CandlestickBars.Write(priceBar1);
            CandlestickBars.Write(priceBar3);
            CandlestickBars.Write(priceBar2);
            CandlestickBars.Write(priceBar3.Timestamp, priceBar3.Open, priceBar3.High, priceBar3.Low, priceBar3.Close);

            //Assert
            Assert.AreEqual(4, CandlestickBars.Length);
            Assert.AreEqual(CandlestickBars.Current.Timestamp, priceBar3.Timestamp);
            Assert.AreEqual(CandlestickBars.Previous.Timestamp, priceBar2.Timestamp);
        }

        [TestMethod]
        public void CandlestickBar_ByteMapping_ToByteArray_Works()
        {
            //Arrange
            var timeStamp = DateTime.Now;
            var CandlestickBar = new CandlestickBar(timeStamp.Ticks,1,2,3,4,5,6,7,true);

            //Act
            byte[] byteArray = CandlestickBar.ToByteArray();

            var ticks= BitConverter.ToInt64(byteArray, 0);
            var actualOpen= BitConverter.ToDouble(byteArray, 8);
            var actualHigh = BitConverter.ToDouble(byteArray, 8 * 2);
            var actualLow = BitConverter.ToDouble(byteArray, 8 * 3);
            var actualClose = BitConverter.ToDouble(byteArray, 8 * 4);
            var actualUpperWick = BitConverter.ToDouble(byteArray, 8 * 5);
            var actualBody = BitConverter.ToDouble(byteArray, 8 * 6);            
            var actualLowerWick = BitConverter.ToDouble(byteArray, 8 * 7);
            var actualIsFilled = BitConverter.ToBoolean(byteArray, 8 * 8);

            //Assert
            Assert.AreEqual(timeStamp.Ticks, ticks);            
            Assert.AreEqual(1, actualOpen);
            Assert.AreEqual(2, actualHigh);
            Assert.AreEqual(3, actualLow);
            Assert.AreEqual(4, actualClose);
            Assert.AreEqual(5, actualUpperWick);
            Assert.AreEqual(6, actualBody);
            Assert.AreEqual(7, actualLowerWick);
            Assert.IsTrue(actualIsFilled);
        }

        [TestMethod]
        public void CandlestickBar_ByteMapping_ByteArrayWrite_Works()
        {
            //Arrange
            var timeStamp = DateTime.Now;
            var CandlestickBar = new CandlestickBar(timeStamp.Ticks, 1, 2, 3, 4, 5, 6, 7, true);
            byte[] byteArray = CandlestickBar.ToByteArray();
            var CandlestickBarCopy = new CandlestickBar(DateTime.Now.AddMinutes(12345).Ticks, 0, 0, 0, 0, 0, 0, 0, false);

            //Act
            CandlestickBarCopy.Write(byteArray);

            //Assert
            Assert.AreEqual(timeStamp.Ticks, CandlestickBarCopy.Timestamp);
            Assert.AreEqual(1, CandlestickBarCopy.Open);
            Assert.AreEqual(2, CandlestickBarCopy.High);
            Assert.AreEqual(3, CandlestickBarCopy.Low);
            Assert.AreEqual(4, CandlestickBarCopy.Close);
            Assert.AreEqual(5, CandlestickBarCopy.UpperWick);
            Assert.AreEqual(6, CandlestickBarCopy.Body);
            Assert.AreEqual(7, CandlestickBarCopy.LowerWick);
            Assert.IsTrue(CandlestickBarCopy.IsFilled);
        }
    }
}
