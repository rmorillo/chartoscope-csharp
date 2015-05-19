using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chartoscope.Persistence
{
    public abstract class BinaryFileBase<TAppender, TNavigator>
    {
        protected BinaryField[] fields = null;
        private FileStream fileStream = null;
        private BinaryWriter writer = null;
        protected string binaryFileName = null;

        protected void InitializeColumns(params BinaryField[] fields)
        {
            this.fields = fields;
        }

        public virtual TAppender GetWriter() { return default(TAppender); }

        public virtual TNavigator GetReader() { return default(TNavigator); }
    }
}
