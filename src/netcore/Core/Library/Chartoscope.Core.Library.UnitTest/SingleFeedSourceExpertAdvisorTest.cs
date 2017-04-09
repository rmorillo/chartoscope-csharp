using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Library.UnitTest
{
    [TestClass]
    public class SingleFeedSourceExpertAdvisorTest
    {
        [TestMethod]
        public void SingleFeedSourceExpertAdvisor_Works()
        {
            var marketFeed = new MarketFeedService();
            var marketOrder = new MarketOrderService();
            var expertAdvisor = new ExpertAdvisor(marketFeed, marketOrder);
        }
    }
}
