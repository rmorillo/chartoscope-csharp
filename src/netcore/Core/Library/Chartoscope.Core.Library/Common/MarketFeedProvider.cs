using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class MarketFeedProvider: IMarketFeedProvider
    {        
        public MarketFeedProvider(int id, string codeName, string displayName)
        {
            Id = id;
            CodeName = codeName;
            DisplayName = displayName;
        }

        public int Id { get; private set; }

        public string CodeName { get; private set; }

        public string DisplayName { get; private set; }       
    }
}
