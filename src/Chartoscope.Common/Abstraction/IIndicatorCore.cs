using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public interface IIndicatorCore
    {
        BarItemType BarType { get; }
        string IdentityCode { get; }
        void BuildCore(ISessionIndicator sessionIndicator);
    }
}
