using System;
using Chartoscope.Common;

namespace Chartoscope.UI.Common
{
	public interface IPriceActionChart
	{
		void NewPriceBar(DateTime timeStamp, double open, double high, double low, double close, double volume= 0);
		void LoadPriceBars(Bookmark<IPriceBar> priceBars);
	}
}

