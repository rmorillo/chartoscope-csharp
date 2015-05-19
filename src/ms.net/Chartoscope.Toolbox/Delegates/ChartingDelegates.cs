using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Toolbox
{
    public static class ChartingDelegates
    {
        public delegate void ChangeCurrentDateHandler(DateTime newDateTime);
        public delegate bool RequestUpdateHandler(DateTime currentDateTime, DateTime startTime, DateTime endTime, ChartScrollDirectionMode scrollDirection);
    }
}
