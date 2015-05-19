using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public static class DebugHelper
    {
        public static void WriteBarItem(BarItem barItem, bool gapped= false)
        {
            Debug.WriteLine(string.Format("Time: {0}, Opening: {1}, Closing: {2}, High: {3}, Low: {4}, Quality: {5}",
                barItem.Time.ToShortTimeString(), barItem.Open, barItem.Close, barItem.High, barItem.Low, gapped ? "NG" : "OK"));
        }

        public static void WriteBarItemTabDelimited(BarItem barItem, bool gapped= false)
        {
            Debug.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}",
                barItem.Time.ToShortTimeString(), barItem.Open, barItem.Close, barItem.High, barItem.Low, gapped ? "NG" : "OK"));
        }

        public static void WriteBarItemGap(DateTime dateTime)
        {
            Debug.WriteLine(string.Format("Time: {0}, gap detected.  Copying previous bar.", dateTime.ToShortTimeString()));
        }
    }
}
