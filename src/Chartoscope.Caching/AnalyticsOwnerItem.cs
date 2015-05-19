using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public class AnalyticsOwnerItem
    {
        private string identityCode;

        public string IdentityCode
        {
            get { return identityCode; }
        }

        private AnalyticsTypeOption analyticsType;

        public AnalyticsTypeOption AnalyticsType
        {
            get { return analyticsType; }
        }

        public AnalyticsOwnerItem(string identityCode, AnalyticsTypeOption analyticsType)
        {
            this.analyticsType = analyticsType;
            this.identityCode = identityCode;
        }

    }
}
