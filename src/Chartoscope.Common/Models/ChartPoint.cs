using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public struct ChartPoint
    {
        private double y;

        public double Y
        {
            get { return y; }
        }

        private double x;

        public double X
        {
            get { return x; }
        }

        public ChartPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

    }
}
