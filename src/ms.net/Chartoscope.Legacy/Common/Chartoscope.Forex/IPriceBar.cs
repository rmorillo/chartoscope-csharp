using System;

namespace Chartoscope.Common
{
	public interface IPriceBar
	{
		DateTime TimeStamp { get; }
		float Open { get; }
		float High { get;  }
		float Low { get; }
		float Close { get;  }
	}
}

