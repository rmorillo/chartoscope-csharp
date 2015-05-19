using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public static class Timeframes
    {
        public readonly static BarItemType M1 = new TimeBarItemType(TimeframeUnitOption.M1);
        public readonly static BarItemType M2 = new TimeBarItemType(TimeframeUnitOption.M2);
        public readonly static BarItemType M3 = new TimeBarItemType(TimeframeUnitOption.M3);
        public readonly static BarItemType M4 = new TimeBarItemType(TimeframeUnitOption.M4);
        public readonly static BarItemType M5 = new TimeBarItemType(TimeframeUnitOption.M5);
        public readonly static BarItemType M6 = new TimeBarItemType(TimeframeUnitOption.M6);
        public readonly static BarItemType M10 = new TimeBarItemType(TimeframeUnitOption.M10);
        public readonly static BarItemType M12 = new TimeBarItemType(TimeframeUnitOption.M12);
        public readonly static BarItemType M15 = new TimeBarItemType(TimeframeUnitOption.M15);
        public readonly static BarItemType M20 = new TimeBarItemType(TimeframeUnitOption.M20);
        public readonly static BarItemType M25 = new TimeBarItemType(TimeframeUnitOption.M25);
        public readonly static BarItemType M30 = new TimeBarItemType(TimeframeUnitOption.M30);
        public readonly static BarItemType H1 = new TimeBarItemType(TimeframeUnitOption.H1);
        public readonly static BarItemType H2 = new TimeBarItemType(TimeframeUnitOption.H2);
        public readonly static BarItemType H3 = new TimeBarItemType(TimeframeUnitOption.H3);
        public readonly static BarItemType H4 = new TimeBarItemType(TimeframeUnitOption.H4);
        public readonly static BarItemType H6 = new TimeBarItemType(TimeframeUnitOption.H6);
        public readonly static BarItemType H8 = new TimeBarItemType(TimeframeUnitOption.H8);
        public readonly static BarItemType H12 = new TimeBarItemType(TimeframeUnitOption.H12);
        public readonly static BarItemType D1 = new TimeBarItemType(TimeframeUnitOption.D1);
        public readonly static BarItemType W1 = new TimeBarItemType(TimeframeUnitOption.W1);
        public readonly static BarItemType MN1 = new TimeBarItemType(TimeframeUnitOption.MN1);
    }
}
