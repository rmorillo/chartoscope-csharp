using System;

namespace Chartoscope.Test.Feeds
{
	public interface ITestFeed
	{
		event TestFeedDelagates.PushFeed PushFeed;
		int Length { get; }
		int Position { get; set;}
		void PushNext(int numberOfBars= 1);
		void PushUntil(int position);
		void PushAll();
		void PushRest();
		bool EndOfFeed { get; }
		void AddPrice(float open, float high, float low, float close, float volume=0);
	}
}

