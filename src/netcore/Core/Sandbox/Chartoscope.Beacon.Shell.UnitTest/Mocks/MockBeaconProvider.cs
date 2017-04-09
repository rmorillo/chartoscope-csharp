using Chartoscope.Beacon.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chartoscope.Beacon.Shell.UnitTest
{
    public class MockBeaconProvider : IBeaconProvider
    {
        public IInstrumentNameResolver InstrumentNameResolver => throw new NotImplementedException();

        public event DelegateDefinitions.TickDataHandler TickReceived;

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }

        public void Startup()
        {
            throw new NotImplementedException();
        }
    }
}
