using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public class IndicatorIdentity: Attribute
    {
        private string code;

        public string Code
        {
            get { return code; }
        }

        private string name;

        public string Name
        {
            get { return name; }
        }

        private string description;

        public string Description
        {
            get { return description; }
        }

        public IndicatorIdentity(string code, string name, string description)
        {
            this.code = code;
            this.name = name;
            this.description = description;
        }
    }
}
