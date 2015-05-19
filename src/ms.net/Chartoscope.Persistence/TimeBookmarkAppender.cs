using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Persistence
{
    public class BookmarkAppender : BinaryAppender
    {
        internal BookmarkAppender(string binaryFileName, byte[] fileHeader, BinaryField[] fields)
            : base(binaryFileName, fileHeader, fields)
        {
        }

        public void AppendBookmark(DateTime time, long position)
        {
            object[] item = new object[] { time.Ticks, position };
            this.Append(item);
        }
    }
}
