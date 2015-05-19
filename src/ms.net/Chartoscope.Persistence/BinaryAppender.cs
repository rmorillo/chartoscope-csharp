using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chartoscope.Persistence
{
    public class BinaryAppender
    {
        private BinaryWriter writer = null;
        private Dictionary<string, BinaryField> fields = null;
        private int rowSize = 0;
        private Dictionary<int, string> columnOrders = null;
        private string binaryFileName;

        public long Position { get { return this.writer.BaseStream.Position; } }


        internal BinaryAppender(string binaryFileName, byte[] fileHeader, BinaryField[] fields)
        {
            this.binaryFileName = binaryFileName;
            this.fields = new Dictionary<string, BinaryField>();
            this.columnOrders = new Dictionary<int, string>();
            int order = 0;
            foreach (BinaryField field in fields)
            {
                this.fields.Add(field.Name, field);
                columnOrders.Add(order, field.Name);
                rowSize += field.Size;
                order++;
            }
            FileHelper.CheckMissingFolders(binaryFileName);
            writer = new BinaryWriter(File.Open(binaryFileName, FileMode.Create));
            writer.Write(fileHeader);
        }        

        public void Append(object[] values)
        {
            int offset = 0;
            byte[] row = new byte[rowSize];
            for (int index = 0; index < fields.Count; index++)
            {
                string columnName = columnOrders[index];
                BinaryField field = fields[columnName];
                switch (field.DataType)
                {
                    case BinaryColumnTypeOption.Long:
                        BitConverter.GetBytes(Convert.ToInt64(values[index])).CopyTo(row, offset);
                        break;
                    case BinaryColumnTypeOption.Double:
                        BitConverter.GetBytes((double)values[index]).CopyTo(row, offset);
                        break;
                    case BinaryColumnTypeOption.Integer:
                        BitConverter.GetBytes((int)values[index]).CopyTo(row, offset);
                        break;
                    case BinaryColumnTypeOption.String:
                        Encoding.ASCII.GetBytes(values[index].ToString().Substring(0, field.Size)).CopyTo(row, offset);
                        break;
                    case BinaryColumnTypeOption.Guid:
                        ((Guid) values[index]).ToByteArray().CopyTo(row, offset);
                        break;
                }
                offset += field.Size;
            }
            writer.Write(row);
        }

        public void Close()
        {
            writer.Close();
        }
    }
}
