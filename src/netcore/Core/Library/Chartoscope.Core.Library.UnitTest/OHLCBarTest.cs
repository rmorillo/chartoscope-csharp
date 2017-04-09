using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chartoscope.Core.Library.UnitTest
{
    [TestClass]
    public class OHLCBarTest
    {
        [TestMethod]
        public void OHLCBar_Constructor_Works()
        {
            //Arrange, Act & Assert
            Assert.IsNotNull(new OHLCBars(10));
        }

        [TestMethod]
        public void OHLCBar_Write_Works()
        {
            //Arrange
            var ohlcBars = new OHLCBars(10);
            var ohlcBar1 = new OHLCBar(DateTime.Now.Ticks, 1, 2, 3, 4);
            var ohlcBar2 = new OHLCBar(DateTime.Now.Ticks, 1, 2, 3, 4);            

            //Act
            ohlcBars.Write(ohlcBar1.Timestamp, ohlcBar1.Open, ohlcBar1.High, ohlcBar1.Low, ohlcBar1.Close);
            ohlcBars.Write(ohlcBar2.Timestamp, ohlcBar2.Open, ohlcBar2.High, ohlcBar2.Low, ohlcBar2.Close);
            ohlcBars.Write(ohlcBar1.Timestamp, ohlcBar1.Open, ohlcBar1.High, ohlcBar1.Low, ohlcBar1.Close);

            //Assert
            Assert.AreEqual(3, ohlcBars.Length);
            Assert.AreEqual(ohlcBars.Current.Timestamp, ohlcBar1.Timestamp);
            Assert.AreEqual(ohlcBars.Current.Open, ohlcBar1.Open);
            Assert.AreEqual(ohlcBars.Current.High, ohlcBar1.High);
            Assert.AreEqual(ohlcBars.Current.Low, ohlcBar1.Low);
            Assert.AreEqual(ohlcBars.Current.Close, ohlcBar1.Close);            

            Assert.AreEqual(ohlcBars.Previous.Timestamp, ohlcBar2.Timestamp);
            Assert.AreEqual(ohlcBars.Previous.Open, ohlcBar2.Open);
            Assert.AreEqual(ohlcBars.Previous.High, ohlcBar2.High);
            Assert.AreEqual(ohlcBars.Previous.Low, ohlcBar2.Low);
            Assert.AreEqual(ohlcBars.Previous.Close, ohlcBar2.Close);            
        }
    }
}
