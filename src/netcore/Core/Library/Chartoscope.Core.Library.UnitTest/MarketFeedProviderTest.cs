using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Library.UnitTest
{
    [TestClass]
    public class MarketFeedProviderTest
    {
        [TestMethod]
        public void MarketFeedProvider_Constructor_Works()
        {
            Assert.IsNotNull(new MarketFeedProvider(0, "abc", "def"));
        }

    }
}
