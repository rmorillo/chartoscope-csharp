using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Persistence
{
    public class BarItemFile : BinaryFileBase<BarItemAppender, BarItemNavigator>
    {
        private BookmarkFile bookMark = null;
        private TimeBarItemType bookmarkTimeFrame;
        private string barItemFile = null;
        private string bookmarkFile = null;
        private static BinaryField[] binaryFields = null;

        static BarItemFile()
        {
            binaryFields = new BinaryField[] {new BinaryField("Time",BinaryColumnTypeOption.Long), 
                new BinaryField("Open", BinaryColumnTypeOption.Double),
                new BinaryField("Close", BinaryColumnTypeOption.Double),
                new BinaryField("High", BinaryColumnTypeOption.Double),
                new BinaryField("Low", BinaryColumnTypeOption.Double)};
        }

        public BarItemFile(string barItemFile, BarItemType timeFrame, TimeBarItemType bookmarkTimeFrame)
        {
            this.barItemFile = barItemFile;
            this.bookmarkTimeFrame = bookmarkTimeFrame;
            this.bookmarkFile = Path.Combine(Path.GetDirectoryName(barItemFile), Path.GetFileNameWithoutExtension(barItemFile) + ".bmk");
            this.bookMark = new BookmarkFile(bookmarkFile, bookmarkTimeFrame);

            Initialize();
        }

        public BarItemFile(string barItemFile)
        {
            this.barItemFile = barItemFile;

            Initialize();
        }

        private void Initialize()
        {
            this.InitializeColumns(binaryFields);
        }

        public override BarItemAppender GetWriter()
        {      
            return new BarItemAppender(barItemFile, null, this.fields, bookmarkTimeFrame);
        }

        public override BarItemNavigator GetReader()
        {
            return new BarItemNavigator(barItemFile, this.fields);
        }

        public static void ValidateFile(string file)
        {
            if (!File.Exists(file))
            {
                throw new Exception(string.Concat("Bar data file '", file, "' does not exist!"));
            }
            else
            {
                BinaryNavigator binaryNavigator = null;
                try
                {
                    binaryNavigator = new BinaryNavigator(file, binaryFields);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Concat("Error reading data file '", file, "'!'"), ex);
                }                

                object[] record = binaryNavigator.Read();

                //time must be valid
                try
                {
                    DateTime time = new DateTime((long)record[0]);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Concat("Invalid time format of data file '", file, "'!'"), ex);
                }
                finally
                {
                    binaryNavigator.Close();
                }

                binaryNavigator.Close();

                //prices must not be less than or equal to zero
                //open and closing prices must be anywhere between high and low prices
                if ((double)record[1] <= 0 || (double)record[2] <= 0 || (double)record[3] <= 0 || (double)record[4] <= 0
                    || (double)record[1] > (double)record[3] || (double)record[1] < (double)record[4]
                    || (double)record[2] > (double)record[3] || (double)record[2] < (double)record[4])
                {
                    throw new Exception(string.Concat("Invalid price format of data file '", file, "'!'"));
                }
            }
        }       
    }
}
