using System;
using System.Collections.Generic;
using System.Text;

namespace Chartoscope.Beacon.Common
{
    public interface IInstrumentNameResolver
    {
        InstrumentNameResolution Resolve(string beaconInstrumentCode);
    }
}
