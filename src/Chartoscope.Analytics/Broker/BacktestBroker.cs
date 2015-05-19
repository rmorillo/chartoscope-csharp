using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Droidworks.Persistence;
using Metadroids.Common.Enumerations;

namespace Metadroids.Analytics.Broker
{
    public class BacktestBroker: BrokerBase
    {
        public Dictionary<Guid, BacktestSession> backtestSessions = null;

        public IBacktestSession CreateSession(CurrencyPairOption currencyPair, string barDataFile)
        {
            if (backtestSessions == null)
            {
                backtestSessions = new Dictionary<Guid, BacktestSession>();
            }
            
            //this line should raise an exception if file is invalid
            BarItemFile.ValidateFile(barDataFile);
            
            BacktestSession backtestSession= new BacktestSession(currencyPair, barDataFile);

            backtestSessions.Add(backtestSession.SessionId, backtestSession);

            return backtestSession;
        }        
    }
}
