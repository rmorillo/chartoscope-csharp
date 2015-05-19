using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public interface ISessionIndicator
    {
        BarItemType BarType { get; set; }
        Dictionary<string, ICoreIndicator> CoreIndicators { get; set; }
    }
}
