using System;
using System.Collections.Generic;
using System.Text;

namespace Oanda.Common.Data
{
    public class IsOptionalAttribute : Attribute
    {
        public override string ToString()
        {
            return "Is Optional";
        }
    }
}
