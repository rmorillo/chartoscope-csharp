using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chartoscope.Common.Tests
{
    [TestClass]
    public class LookBehindPoolTest
    {
        [TestMethod]
         public void TestWritesShouldStoreValues()
        {
            LookBehindPool<byte> buf = new LookBehindPool<byte>(10);
            buf.Write(1);
            buf.Write(2);
            buf.Write(3);

            Assert.AreEqual(3, buf[0]);
            Assert.AreEqual(2, buf[1]);
            Assert.AreEqual(1, buf[2]);
            Assert.AreEqual(3, buf.Count);
        }

        [TestMethod]
        public void TestWriteBeyondCapacityShouldRollOver()
        {
            LookBehindPool<byte> buf = new LookBehindPool<byte>(3);

            buf.Write(1);
            buf.Write(2);
            buf.Write(3);
            buf.Write(4);

            Assert.AreEqual(4, buf[0]);
            Assert.AreEqual(3, buf[1]);
            Assert.AreEqual(2, buf[2]);
            Assert.AreEqual(3, buf.Count);

            Assert.AreEqual(4, buf.Current);
        }


        [TestMethod]
        public void TestWriteBeyondCapacityTwiceShouldRollOverTwice()
        {
            LookBehindPool<byte> buf = new LookBehindPool<byte>(3);
            buf.Write(1);
            buf.Write(2);
            buf.Write(3);
            buf.Write(4);
            Assert.AreEqual(4, buf[0]);
            Assert.AreEqual(3, buf[1]);
            Assert.AreEqual(2, buf[2]);
            Assert.AreEqual(3, buf.Count);

            Assert.AreEqual(4, buf.Current);

            buf.Write(5);
            buf.Write(6);
            buf.Write(7);

            Assert.AreEqual(7, buf[0]);
            Assert.AreEqual(6, buf[1]);
            Assert.AreEqual(5, buf[2]);
            Assert.AreEqual(3, buf.Count);

            Assert.AreEqual(7, buf.Current);

        }

        [TestMethod, ExpectedException(typeof(Exception), "Capacity must be greater than zero")]
        public void TestShouldThrowAnExceptionWhenInstantiatedWithZeroCapacity()
        {
            new LookBehindPool<byte>(0);            
        }

        [TestMethod, ExpectedException(typeof(Exception), "Index out of range")]
        public void TestShouldThrowAnExceptionWhenIndexIsOutOfRange()
        {
            LookBehindPool<byte> buf = new LookBehindPool<byte>(3);

            buf.Write(1);

            int errorLine = buf[1];
        }

        [TestMethod, ExpectedException(typeof(Exception), "Index out of range")]
        public void TestShouldThrowAnExceptionWhenIndexIsOutOfRangeAfterRollOver()
        {
            LookBehindPool<byte> buf = new LookBehindPool<byte>(3);

            buf.Write(1);
            buf.Write(2);
            buf.Write(3);
            buf.Write(4);
          
            int errorLine = buf[3];            
        }
    }
}