using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Persistence
{
    public class BarItemNavigator : BinaryNavigator
    {
        private CurrencyPairOption currencyPair;

        internal BarItemNavigator(string binaryFileName, BinaryField[] fields)
            : base(binaryFileName, fields)
        {

        }

        public BarItem ReadBarItem()
        {
            object[] row = Read();
            BarItem barItem = null;
            if (row.Length > 0)
            {
                barItem = new BarItem(new DateTime((long)row[columnIndex["Time"]]), (double)row[columnIndex["Open"]], (double)row[columnIndex["Close"]], (double)row[columnIndex["High"]], (double)row[columnIndex["Low"]]);
            }

            return barItem;
        }
    }
}
