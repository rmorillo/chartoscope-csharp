using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Metadroids.Analytics.Models;
using Metadroids.Common.Enumerations;

namespace Metadroids.Analytics.Broker
{
    public class BacktestSession: IBacktestSession
    {
        private CurrencyPairOption currencyPair;
        private string barDataFile;

        private Dictionary<string, MultiIndicator> multiIndicators = null;
        private Dictionary<string, SingleIndicator> singleIndicators = null;

        private Guid sessionId;

        public Guid SessionId
        {
            get { return sessionId; }
        }
               
        public BacktestSession(CurrencyPairOption currencyPair, string barDataFile)
        {
            this.currencyPair = currencyPair;
            this.barDataFile = barDataFile;
            this.sessionId = Guid.NewGuid();
        }

        public void Start(Guid accountId)
        {
            foreach (MultiIndicator multiIndicator in multiIndicators.Values)
            {
                
            }
        }

        public void AddIndicator(BuiltinIndicatorOption builtinIndicator, MultiIndicator indicator)
        {
            if (multiIndicators == null)
            {
                multiIndicators = new Dictionary<string, MultiIndicator>();
            }

            multiIndicators.Add(indicator.UniqueShortName, indicator);
        }

        public void AddIndicator(BuiltinIndicatorOption builtinIndicator, SingleIndicator indicator)
        {
            if (singleIndicators == null)
            {
                singleIndicators = new Dictionary<string, SingleIndicator>();
            }

            singleIndicators.Add(indicator.UniqueShortName, indicator);
        }
    }
}
