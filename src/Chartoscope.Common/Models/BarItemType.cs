using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public abstract class BarItemType: IBarItemTypeCode
    {
        private BarItemMode mode;

        public BarItemMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        private long _value;

        public long Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private string tag;

        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public string Code
        {
            get { return string.Concat(this.Tag, this.Value.ToString()); }
        }
    }
}
