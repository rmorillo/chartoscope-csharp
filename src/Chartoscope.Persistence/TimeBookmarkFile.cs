using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Persistence
{
    public class BookmarkFile : BinaryFileBase<BookmarkAppender, BookmarkNavigator>
    {
        private string barFile = null;
        private TimeBarItemType timeFrame;

        public BookmarkFile(string barFile, TimeBarItemType timeFrame)
        {
            this.barFile = barFile;
            this.timeFrame = timeFrame;
            this.InitializeColumns(new BinaryField[] {new BinaryField("Time",BinaryColumnTypeOption.Long), 
                new BinaryField("Position", BinaryColumnTypeOption.Long)});
        }

        public override BookmarkAppender GetWriter()
        {
            return new BookmarkAppender(barFile, null, this.fields);
        }

        public override BookmarkNavigator GetReader()
        {
            string fileName = barFile;
            return new BookmarkNavigator(fileName, this.fields);
        }
    }
}
