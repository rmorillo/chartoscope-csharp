using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Library.UnitTest
{
    [TestClass]
    public class PriceFeedServiceTest
    {
        [TestMethod]
        public void PriceFeedService_Constructor_Works()
        {
            Assert.IsNotNull(new PriceFeedService(new ManualPriceFeeder(new MockFeedProvider())));
        }
    }
}
