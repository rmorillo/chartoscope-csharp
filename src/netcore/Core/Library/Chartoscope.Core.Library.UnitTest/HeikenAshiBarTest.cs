using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chartoscope.Core.Library.UnitTest
{
    [TestClass]
    public class HeikenAshiBarTest
    {
        [TestMethod]
        public void HeikenAshiBar_Write_Works()
        {
            //Arrange
            var haBars = new HeikenAshiBars(10);
            haBars.BarUpdated += delegate (IHeikenAshiBar current) { };
            var priceBar1 = new PriceBar(DateTime.Now.Ticks, 1, 2, 3, 4);
            var priceBar2= new PriceBar(DateTime.Now.Ticks, 2, 3, 4, 0);            
            var priceBar3 = new PriceBar();
            priceBar3.Write(DateTime.Now.Ticks, 1, 2, 3, 4);

            //Act
            haBars.Write(priceBar2);            
            haBars.Write(priceBar1);
            haBars.Write(priceBar3);
            haBars.Write(priceBar1.Timestamp, priceBar1.Open, priceBar1.High, priceBar1.Low, priceBar1.Close);

            //Assert
            Assert.AreEqual(4, haBars.Length);
            Assert.AreEqual(haBars.Current.Timestamp, priceBar1.Timestamp);
            Assert.AreEqual(haBars.Previous.Timestamp, priceBar3.Timestamp);            
        }

        [TestMethod]
        public void HeikenAshiBar_ByteMapping_ToByteArray_Works()
        {
            //Arrange
            var timeStamp = DateTime.Now;
            var heikenAshiBar = new HeikenAshiBar(timeStamp.Ticks, 1, 2, 3, 4, 5, 6, 7, true);

            //Act
            byte[] byteArray = heikenAshiBar.ToByteArray();

            var ticks = BitConverter.ToInt64(byteArray, 0);
            var actualOpen = BitConverter.ToDouble(byteArray, 8);
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
        public void HeikenAshiBar_ByteMapping_ByteArrayWrite_Works()
        {
            //Arrange
            var timeStamp = DateTime.Now;
            var heikenAshiBar = new HeikenAshiBar(timeStamp.Ticks, 1, 2, 3, 4, 5, 6, 7, true);
            byte[] byteArray = heikenAshiBar.ToByteArray();
            var heikenAshiBarCopy = new HeikenAshiBar(DateTime.Now.AddMinutes(12345).Ticks, 0, 0, 0, 0, 0, 0, 0, false);

            //Act
            heikenAshiBarCopy.Write(byteArray);

            //Assert
            Assert.AreEqual(timeStamp.Ticks, heikenAshiBarCopy.Timestamp);
            Assert.AreEqual(1, heikenAshiBarCopy.Open);
            Assert.AreEqual(2, heikenAshiBarCopy.High);
            Assert.AreEqual(3, heikenAshiBarCopy.Low);
            Assert.AreEqual(4, heikenAshiBarCopy.Close);
            Assert.AreEqual(5, heikenAshiBarCopy.UpperWick);
            Assert.AreEqual(6, heikenAshiBarCopy.Body);
            Assert.AreEqual(7, heikenAshiBarCopy.LowerWick);
            Assert.IsTrue(heikenAshiBarCopy.IsFilled);
        }
    }
}
