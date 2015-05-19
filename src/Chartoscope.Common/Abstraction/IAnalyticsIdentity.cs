using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public interface IAnalyticsIdentity
    {
        string IdentityCode { get; }
        string IdentityName { get; }
        string Description { get; }
    }
}
