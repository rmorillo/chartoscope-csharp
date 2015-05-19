using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Metadroids.Analytics.Models;
using Metadroids.Common.Enumerations;

namespace Metadroids.Analytics.Broker
{
    public interface IBacktestSession
    {
        void Start(Guid accountId);
        void AddIndicator(MultiIndicator indicator);
        void AddIndicator(SingleIndicator indicator);
    }
}
