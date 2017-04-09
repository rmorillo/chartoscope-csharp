using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Indicators.UnitTest
{
    public class MockFeedProvider : IMarketFeedProvider
    {
        public MockFeedProvider()
        {
        }

        public string CodeName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string DisplayName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Id
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
