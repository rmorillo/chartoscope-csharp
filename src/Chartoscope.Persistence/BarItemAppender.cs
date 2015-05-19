using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Persistence
{
    public class BarItemAppender: BinaryAppender
    {
        public event PersistenceDelegates.BookmarkNotificationHandler BookmarkNotification;
        private DateTime nextRollingDate = DateTime.MinValue;
        private TimeBarItemType bookmarkBarItemType;

        internal BarItemAppender(string binaryFileName, byte[] fileHeader, BinaryField[] fields, TimeBarItemType bookmarkBarItemType)
            : base(binaryFileName, fileHeader, fields)
        {
            this.bookmarkBarItemType = bookmarkBarItemType;
        }

        public void AppendBarItem(DateTime time, double open, double close, double high, double low)
        {
            long lastPosition = this.Position;
            object[] items = new object[] { time.Ticks, open, close, high, low };
           
            this.Append(items);

            if (BookmarkNotification != null)
            {
                if (lastPosition == 0)
                {
                    BookmarkNotification(time, lastPosition);
                }

                if (nextRollingDate == DateTime.MinValue)
                {
                    nextRollingDate = TimeframeHelper.NextRolling(time, bookmarkBarItemType);
                }

                if (time >= nextRollingDate)
                {
                    BookmarkNotification(time, lastPosition);
                    nextRollingDate = TimeframeHelper.NextRolling(time, bookmarkBarItemType);
                }
            }
        }
    }
}
