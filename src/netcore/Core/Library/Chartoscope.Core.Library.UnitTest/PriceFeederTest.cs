using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Library.UnitTest
{
    [TestClass]
    public class PriceFeederTest
    {
        [TestMethod]
        public void BeaconPriceFeeder_Constructor_Works()
        {
            Assert.IsNotNull(new BeaconPriceFeeder(null));
        }

        [TestMethod]
        public void CSVPriceFeeder_Constructor_Works()
        {
            Assert.IsNotNull(new CSVPriceFeeder(null));
        }

        [TestMethod]
        public void PriceBarFileFeeder_Constructor_Works()
        {
            Assert.IsNotNull(new PriceBarFilePriceFeeder(null));
        }
    }
}
