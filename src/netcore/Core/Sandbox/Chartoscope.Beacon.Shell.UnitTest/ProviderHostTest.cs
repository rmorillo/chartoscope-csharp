using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chartoscope.Beacon.Common;
using System.Collections.Generic;

namespace Chartoscope.Beacon.Shell.UnitTest
{
    [TestClass]
    public class ProviderHostTest
    {
        [TestMethod]
        public void ProviderHost_Constructor_Works()
        {
            var providerHost = new ProviderHost(typeof(MockBeaconProvider));

            IEnumerable<InstrumentNameResolution> resolutions= providerHost.LoadInstruments(new string[] { "EURUSD", "USDJPY" });

            providerHost.Startup();

            providerHost.Shutdown();
        }
    }
}
