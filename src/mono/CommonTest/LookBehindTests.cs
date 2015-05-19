using System;
using NUnit.Framework;

namespace Chartoscope.Common.Test
{
	[TestFixture]
	public class LookBehindTests
	{
		public LookBehindTests ()
		{
		}
		
		[Test]
        public void BasicTest()
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

        [Test]
        public void RolloverTest()
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

     
        [Test]
        public void DoubleRolloverTest()
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

        [Test]
        public void InvalidLengthExceptionTest()
        {
            try
            {
                LookBehindPool<byte> buf = new LookBehindPool<byte>(0);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "Length must be greater than zero.");
            }
        }

        [Test]
        public void OutOfRangeExceptionTest()
        {
            LookBehindPool<byte> buf = new LookBehindPool<byte>(3);

            buf.Write(1);

            try
            {
                int errorLine = buf[1];
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "Index out of range. Value must be from 0 to 0");
            }
        }

        [Test]
        public void RolleOverOutOfRangeExceptionTest()
        {
            LookBehindPool<byte> buf = new LookBehindPool<byte>(3);

            buf.Write(1);
            buf.Write(2);
            buf.Write(3);
            buf.Write(4);
            try
            {
                int errorLine = buf[3];
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message == "Index out of range. Value must be from 0 to 2");
            }
        }
			
	}
}

