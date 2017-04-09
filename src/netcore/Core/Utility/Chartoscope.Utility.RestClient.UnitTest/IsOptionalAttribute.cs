using System;
using System.Collections.Generic;
using System.Text;

namespace Chartoscope.Utility.RestClient.UnitTest
{
    public class IsOptionalAttribute : Attribute
    {
        public override string ToString()
        {
            return "Is Optional";
        }
    }
}
