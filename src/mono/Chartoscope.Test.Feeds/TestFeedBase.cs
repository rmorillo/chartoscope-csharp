using System;
using Chartoscope.Common;
using System.Collections.Generic;

namespace Chartoscope.Test.Feeds
{
	public class TestFeedBase: ITestFeed
	{		
		protected List<PriceBar> _priceBars;
		
		private int _position= 0; 
		
		public event TestFeedDelagates.PushFeed PushFeed;

		private TimeStamper _timeStamper;
		
		internal TestFeedBase (IntervalTypeOption interval, DateTime startDateTime)
		{
			_priceBars= new List<PriceBar>();
			_timeStamper= new TimeStamper(interval, startDateTime);
		}
		
		#region ITestFeed implementation
		public void PushNext (int numberOfBars)
		{
			if (PushFeed!=null)
			{				
				if (_position<_priceBars.Count)
				{					
					PushUntil(_position + numberOfBars);
				}
				else
				{
					throw new Exception("Reached end of feed.");
				}
			}
		}

		public void PushUntil (int position)
		{
			while (_position<position && !EndOfFeed)
			{
				Push (_priceBars[_position]);
			}
		}

		public void PushAll ()
		{
			foreach(PriceBar priceBar in _priceBars)
			{
				Push (priceBar);
			}
		}
		
		public void PushRest ()
		{
			while (!EndOfFeed)
			{
				Push (_priceBars[_position]);
			}
		}
		
		private void Push(PriceBar priceBar)
		{			
			PushFeed(priceBar.TimeStamp, priceBar.Open, priceBar.High, priceBar.Low, priceBar.Close, priceBar.Volume);
			_position++;			
		}

		public int Length {
			get {
				return _priceBars.Count;
			}
		}

		public int Position {
			get {
				return _position;
			}
			set
			{
				_position= value;
			}
		}
		
		public bool EndOfFeed {
			get {
				return _position>=_priceBars.Count;
			}
		}
		
		public void AddPrice(float open, float high, float low, float close, float volume=0)
		{
			PriceBar priceBar= new PriceBar();
			priceBar.Write(_timeStamper.Next, open, high, low, close, volume);
			_priceBars.Add(priceBar);
		}
		#endregion
	}
}

