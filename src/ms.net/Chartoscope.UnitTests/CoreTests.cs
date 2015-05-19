using System;
using Chartoscope.Brokers;
using Chartoscope.Common;
using Chartoscope.Indicators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chartoscope.UnitTests
{
    [TestClass]
    public class CoreTests
    {
        [TestMethod]
        public void FullCycle()
        {
            BacktestBroker backtestBroker = new BacktestBroker();

            BrokerAccount brokerAccount = new BrokerAccount();

            backtestBroker.LoadAccount(new BrokerAccount(), Guid.Empty);

            BacktestSession backtestSession = backtestBroker.CreateSession(brokerAccount.AccountId, SpotForex.EURUSD, Timeframes.M1, @"data\EURUSD-M1.bar", Guid.Empty);

            BollingerBands bollingerBands = new BollingerBands(Timeframes.M1, 21);

            backtestSession.Indicators.Add<BollingerBands>(bollingerBands);

            backtestSession.Start();
        }

        [TestMethod]
        public void SMATest()
        {
            BacktestBroker backtestBroker = new BacktestBroker();

            BrokerAccount brokerAccount = new BrokerAccount();

            backtestBroker.LoadAccount(new BrokerAccount(), Guid.Empty);

            BacktestSession backtestSession = backtestBroker.CreateSession(brokerAccount.AccountId, SpotForex.EURUSD, Timeframes.M1, @"data\EURUSD-M1.bar", Guid.Empty);

            SMA sma = new SMA(Timeframes.M1, 100);

            backtestSession.Indicators.Add(sma);

            backtestSession.Start();
        }

        [TestMethod]
        public void EMATest()
        {
            BacktestBroker backtestBroker = new BacktestBroker();

            BrokerAccount brokerAccount = new BrokerAccount();

            backtestBroker.LoadAccount(new BrokerAccount(), Guid.Empty);

            BacktestSession backtestSession = backtestBroker.CreateSession(brokerAccount.AccountId, SpotForex.EURUSD, Timeframes.M1, @"data\EURUSD-M1.bar", Guid.Empty);

            EMA ema = new EMA(Timeframes.M1, 100);

            backtestSession.Indicators.Add(ema);

            backtestSession.Start();
        }

        [TestMethod]
        public void MACDTest()
        {
            BacktestBroker backtestBroker = new BacktestBroker();

            BrokerAccount brokerAccount = new BrokerAccount();

            backtestBroker.LoadAccount(new BrokerAccount(), Guid.Empty);

            BacktestSession backtestSession = backtestBroker.CreateSession(brokerAccount.AccountId, SpotForex.EURUSD, Timeframes.M1, @"data\EURUSD-M1.bar", Guid.Empty);

            MACD macd = new MACD(Timeframes.M1, 12, 26, 9);

            backtestSession.Indicators.Add(macd);

            backtestSession.Start();
        }

        [TestMethod]
        public void M2Timeframe()
        {
            BacktestBroker backtestBroker = new BacktestBroker();

            BrokerAccount brokerAccount = new BrokerAccount();

            backtestBroker.LoadAccount(new BrokerAccount(), Guid.Empty);

            BacktestSession backtestSession = backtestBroker.CreateSession(brokerAccount.AccountId, SpotForex.EURUSD, Timeframes.M1, @"data\EURUSD-M1.bar", Guid.Empty);

            BollingerBands bollingerBands = new BollingerBands(Timeframes.M2, 21);
            backtestSession.Indicators.Add(bollingerBands);

            backtestSession.Start();
        }
    }
}
