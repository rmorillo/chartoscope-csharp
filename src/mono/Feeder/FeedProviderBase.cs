using System;

namespace Chartoscope.Feeder
{
	public abstract class FeedProviderBase: IFeedProvider
	{			
		public FeedProviderBase ()
		{
		}

		#region IFeedProvider implementation
		public event FeederDelegates.MarketOpenedHandler MarketOpened;

		public event FeederDelegates.MarketClosedHandler MarketClosed;

		public event FeederDelegates.FeedWarningHandler FeedWarning;

		public event FeederDelegates.FeedErrorHandler FeedError;

		public abstract void Start ();

		public abstract void Stop ();
		
		public virtual void BackFill()
		{
		}
		
		private IBlackBox _blackBox;
		public IBlackBox BlackBox {
			get {
				return _blackBox;
			}
			set {
				_blackBox= value;
			}
		}
		#endregion
	}
}

