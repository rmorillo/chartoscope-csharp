using System;
using System.Diagnostics;

namespace Chartoscope.Feeder
{
	public class MockFeeder:IPriceBarFeed, IQuoteFeed, IBackFill
    {
		private FeederDelegates.ReceiveFeed _receiveFeed;
		
		private FeederDelegates.ReceiveFeed _backFillFeed;
		
        private int _secondsDuration;
        private int _millisecondInterval;
        private bool _stopFlag = false;
        public MockFeeder(int secondsDuration, int millisecondInterval)
        {
            _secondsDuration = secondsDuration;
            _millisecondInterval = millisecondInterval;
        }

        public void Start()
        {
            float closingPrice = 1.80121f;

            _stopFlag = false;
            Stopwatch runTime = new Stopwatch();
            runTime.Start();
            while (!_stopFlag && runTime.Elapsed.Seconds<_secondsDuration)
            {
                float openPrice = closingPrice;
                float highPrice = closingPrice;
                float lowPrice = closingPrice;
                float currentPrice = closingPrice;
                long volume = 0;
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                while (stopWatch.ElapsedMilliseconds < _millisecondInterval && !_stopFlag)
                {                    
                    currentPrice = GetNextPrice(currentPrice, 0.00001f);
                    if (currentPrice > highPrice)
                    {
                        highPrice = currentPrice;
                    }
                    else if (currentPrice < lowPrice)
                    {
                        lowPrice = currentPrice;
                    }

                    volume++;
                }
                stopWatch.Stop();
                closingPrice = currentPrice;
                if (_receiveFeed != null && !_stopFlag)
                {
                    _receiveFeed(DateTime.Now, (float) Math.Round(openPrice, 5), (float) Math.Round(highPrice, 5), (float) Math.Round(lowPrice, 5), 
					             (float) Math.Round(closingPrice, 5), volume);
                }
            }
            runTime.Stop();
        }

        private float GetNextPrice(float previousPrice, float volatility)
        {
            float rnd = (float) new Random().NextDouble() ; // generate number, 0 <= x < 1.0
            float changePercent = 2 * volatility * rnd;
            if (changePercent > volatility)
                changePercent -= (2 * volatility);
            float changeAmount = previousPrice * changePercent;
            return previousPrice + changeAmount;
        }

        public void Stop()
        {
            _stopFlag = true;
        }

		#region IPriceBarFeed implementation
		public void Feed (DateTime timeStamp, float open, float high, float low, float close, float volume)
		{
			throw new NotImplementedException ();
		}

		public void OnReceiveFeed (FeederDelegates.ReceiveFeed receiveFeed)
		{
			_receiveFeed+= receiveFeed;
		}
		#endregion

		#region IQuoteFeed implementation
		public void Feed (DateTime timeStamp, float bid, float ask)
		{
			throw new NotImplementedException ();
		}
		#endregion

		#region IBackFill implementation
		public void OnReceiveBackFill (FeederDelegates.ReceiveFeed backFill, int minBars, int maxBars)
		{
			throw new NotImplementedException ();
		}
		#endregion
    }
}

