using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Persistence
{
    public class BinaryColumnTypeIdentity : Attribute
    {
        private int size;
        public int Size { get { return this.size; } }

        private string name;
        public string Name { get { return this.name; } }

        private object defaultValue;
        public object DefaultValue { get { return this.defaultValue; } }

        public BinaryColumnTypeIdentity(string name, int size, object defaultValue)
        {
            this.size = size;
            this.name = name;
            this.defaultValue = defaultValue;
        }

        public BinaryColumnTypeIdentity(string name, object defaultValue)
        {
            this.size = int.MinValue;
            this.name = name;
            this.defaultValue = defaultValue;
        }
    }
}
