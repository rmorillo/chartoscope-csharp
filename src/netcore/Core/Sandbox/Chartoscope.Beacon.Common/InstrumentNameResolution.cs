using System;
using System.Collections.Generic;
using System.Text;

namespace Chartoscope.Beacon.Common
{
    public class InstrumentNameResolution
    {
        public InstrumentNameMapping Mapping { get; set; }
        public InstrumentNameResolutionResultOption Status { get; set; }
        public string Description { get; set; }
    }
}
