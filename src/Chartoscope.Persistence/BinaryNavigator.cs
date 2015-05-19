using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Persistence
{
    public class BinaryNavigator
    {
        private BinaryReader reader = null;
        protected Dictionary<string, BinaryField> fields = null;
        private int rowSize = 0;
        private Dictionary<int, string> columnOrders = null;
        protected Dictionary<string, int> columnIndex = null;
        private string binaryFileName;

        public long Position { get { return this.reader.BaseStream.Position; } }
        
        private int dataPosition= 0;

        private CacheHeaderInfo headerInfo = null;

        internal BinaryNavigator(string binaryFileName, BinaryField[] fields)
        {
            this.binaryFileName = binaryFileName;

            this.fields = new Dictionary<string, BinaryField>();
            this.columnOrders = new Dictionary<int, string>();
            this.columnIndex = new Dictionary<string, int>();
            int order = 0;
            foreach (BinaryField field in fields)
            {
                this.fields.Add(field.Name, field);
                columnOrders.Add(order, field.Name);
                columnIndex.Add(field.Name, order);
                rowSize += field.Size;
                order++;
            }

            FileHelper.CheckMissingFolders(binaryFileName);
             
            reader = new BinaryReader(File.Open(binaryFileName, FileMode.OpenOrCreate));

            this.headerInfo = FileHelper.ReadBinaryFileHeader(reader, ref this.dataPosition);
        }

        public void MoveToFirst()
        {
            reader.BaseStream.Position = dataPosition;
        }

        public long RowNumber { get { return (reader.BaseStream.Position - this.dataPosition) / rowSize; } }

        public void MoveToNext()
        {
            if (!EOF())
            {
                reader.BaseStream.Position += this.rowSize;
            }
        }

        public bool Seek(long rowNumber)
        {
            MoveToPosition(this.dataPosition + (rowNumber * rowSize));
            return !EOF();
        }
         
        public long RowCount
        {
            get
            {
                long lastPosition = reader.BaseStream.Position;
                MoveToFirst();
                int count = -1;
                while (!EOF())
                {
                    count++;
                    Skip();
                }

                MoveToPosition(lastPosition);

                return count;
            }
        }

        public void Skip()
        {
            reader.ReadBytes(this.rowSize);
        }

        public void MoveToPrevious()
        {
            if (!BOF())
            {
                reader.BaseStream.Position -= this.rowSize;
            }
        }

        public bool BOF()
        {
            return reader.BaseStream.Position == this.dataPosition;
        }

        public bool EOF()
        {
            return reader.BaseStream.Position == reader.BaseStream.Length;
        }

        public void MoveToLast()
        {
            reader.BaseStream.Position = reader.BaseStream.Length - this.rowSize;
        }

        public void MoveToPosition(long position)
        {
            reader.BaseStream.Position = position;
        }

        public object[] Read()
        {
            int offset = 0;
            byte[] row = reader.ReadBytes(this.rowSize);

            List<object> values = new List<object>();

            if (row.Length > 0)
            {
                for (int index = 0; index < fields.Count; index++)
                {
                    string columnName = columnOrders[index];
                    BinaryField field = fields[columnName];
                    switch (field.DataType)
                    {
                        case BinaryColumnTypeOption.Long:
                            values.Add(BitConverter.ToInt64(row, offset));
                            break;
                        case BinaryColumnTypeOption.Double:
                            values.Add(BitConverter.ToDouble(row, offset));
                            break;
                        case BinaryColumnTypeOption.Integer:
                            values.Add(BitConverter.ToInt32(row, offset));
                            break;
                        case BinaryColumnTypeOption.Guid:
                            byte[] guidBytes= new byte[16];
                            for (int byteIndex = 0; byteIndex < 16; byteIndex++)
                            {
                                guidBytes[byteIndex]= row[offset + byteIndex];
                            }
                            values.Add(guidBytes);
                            break;
                        case BinaryColumnTypeOption.String:
                            values.Add(BitConverter.ToString(row, offset));
                            break;
                    }
                    offset += field.Size;
                }

            }
            return values.Count>0? values.ToArray(): null;
        }

        public void Close()
        {
            reader.Close();
        }
    }
}
