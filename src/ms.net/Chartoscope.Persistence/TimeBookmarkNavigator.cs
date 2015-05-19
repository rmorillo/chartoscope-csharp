using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Persistence
{
    public class BookmarkNavigator : BinaryNavigator
    {
        public BookmarkNavigator(string binaryFileName, BinaryField[] fields)
            : base(binaryFileName, fields)
        {

        }

        public long Locate(DateTime time)
        {
            long lastPosition = long.MinValue;

            DateTime lastBookmarkTime = DateTime.MinValue;

            this.MoveToFirst();
            object[] row = Read();
            if (row.Length > 0)
            {
                lastPosition = (long)row[1];
                while (!EOF())
                {
                    DateTime bookmarkTime = new DateTime((long)row[0]);
                    if (bookmarkTime > time)
                    {
                        break;
                    }
                    else
                    {
                        lastBookmarkTime = bookmarkTime;
                        lastPosition = (long)row[1];
                    }

                    row = Read();
                }
            }

            return lastPosition;
        }
    }
}
