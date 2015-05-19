using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public class CacheFileExtension : Attribute
    {
        private string extension;

        public string Extension
        {
            get { return extension; }
        }

        public CacheFileExtension(string extension)
        {
            this.extension = extension;
        }
    }
}
