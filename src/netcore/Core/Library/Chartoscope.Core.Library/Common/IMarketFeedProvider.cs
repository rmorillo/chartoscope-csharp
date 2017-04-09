using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public interface IMarketFeedProvider
    {
        int Id { get; }

        string CodeName { get; }

        string DisplayName { get; }
    }
}
