using System;

namespace Chartoscope.Feeder
{
	public class ManualFeeder: IPriceBarFeed, IQuoteFeed, IBackFill
	{
		private FeederDelegates.ReceiveFeed _receiveFeed;
		
		private FeederDelegates.ReceiveFeed _backFillFeed;
		
		
		internal ManualFeeder ()
		{
			
		}

		#region IPriceBarFeed implementation
		public void Feed (DateTime timeStamp, float open, float high, float low, float close, float volume= 0)
		{			
			_receiveFeed(timeStamp, open, high, low, close, volume);
		}
		
		public void OnReceiveFeed(FeederDelegates.ReceiveFeed receiveFeed)
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
			_backFillFeed= backFill;
		}
		#endregion
		
		public void BackFill(DateTime timeStamp, float open, float high, float low, float close, float volume= 0)
		{
			_backFillFeed(timeStamp, open, high, low, close, volume);
		}
	}
}

