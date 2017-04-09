using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public interface IPriceFeedSubscriber
    {
        void Subscribe(IPriceFeedService priceFeedServce);

    }
}
