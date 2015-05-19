using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Persistence
{
    public class BinaryField
    {
        private string name;
        public string Name { get { return this.name; } set { this.name = value; } }

        private int size;
        public int Size { get { return this.size; } set { this.size = value; } }

        private BinaryColumnTypeOption dataType;
        public BinaryColumnTypeOption DataType { get { return this.dataType; } set { this.dataType = value; } }

        public BinaryField(string name, BinaryColumnTypeOption dataType, int size)
        {
            Init(name, dataType, size);
        }

        public BinaryField(string name, BinaryColumnTypeOption dataType)
        {
            Init(name, dataType, 0);
        }

        private void Init(string name, BinaryColumnTypeOption dataType, int size)
        {
            this.name = name;
            this.dataType = dataType;
            if (dataType == BinaryColumnTypeOption.String)
            {
                this.size = size;
            }
            else
            {
                this.size = AttributeHelper.GetAttribute<BinaryColumnTypeIdentity>(dataType).Size;
            }
        }
    }
}
