using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Caching;
using Chartoscope.Common;

namespace Chartoscope.Brokers
{
    public class TimeBasedDataFeed: DataFeedItem
    {
        private DateTime lowerRange;

        public DateTime LowerRange
        {
            get { return lowerRange; }
            set { lowerRange = value; }
        }

        private DateTime upperRange;

        public DateTime UpperRange
        {
            get { return upperRange; }
            set { upperRange = value; }
        }

        private long ticks;

        public long Ticks
        {
            get { return ticks; }
            set { ticks = value; }
        }

        private DateTime currentTime;

        public DateTime CurrentTime
        {
            get { return currentTime; }
            set { currentTime = value; }
        }
                

        private TimeBarItemType timeBarItemType;

        public TimeBarItemType BarType { get { return timeBarItemType; } }

        private PricebarCache cachedPricebar;

        public PricebarCache CachedPricebar
        {
            get { return cachedPricebar; }
        }       

        private long period;
        private DateTime startTime;

        private bool cachingEnabled = true;
        private Guid cacheId = Guid.Empty;

        public TimeBasedDataFeed(TimeBarItemType timeBarItemType, BarItem openingBar, Guid cacheId, bool cachingEnabled= true)
        {
            this.timeBarItemType = timeBarItemType;
            this.period = timeBarItemType.Value;
            this.startTime = openingBar.Time;
            this.Restart(startTime);
            this.ResetPrice(openingBar.Open);
            this.Update(openingBar);
            this.SetNextRange();

            this.cachingEnabled = cachingEnabled;
            this.cacheId = cacheId;
            if (cachingEnabled)
            {
                cachedPricebar = new PricebarCache(timeBarItemType, cacheId);
                cachedPricebar.Initialize();
            }
        }

        public void Restart(DateTime startTime)
        {
            this.lowerRange = TimeframeHelper.LastRolling(startTime.AddTicks(-1), this.timeBarItemType);
            this.upperRange = TimeframeHelper.NextRolling(this.lowerRange, this.timeBarItemType);
            this.currentTime = startTime;
            this.ticks = 0;
        }

        public void SetNextRange()
        {
            this.lowerRange = this.upperRange;
            this.upperRange = TimeframeHelper.NextRolling(this.lowerRange, this.timeBarItemType); ;                     
        }

        public bool WithinCurrentRange(DateTime dateTime)
        {
            return (dateTime >= this.lowerRange && dateTime < this.upperRange);
        }

        public bool WithinNextRange(DateTime dateTime)
        {
            DateTime nextLowerRange = this.upperRange;
            DateTime nextUpperRange = TimeframeHelper.NextRolling(this.upperRange, this.timeBarItemType);
            return (dateTime >= nextLowerRange && dateTime < nextUpperRange);
        }

        public bool BeyondRange(DateTime dateTime)
        {
            return (dateTime >= this.upperRange);
        }

        public bool BelowRange(DateTime dateTime)
        {
            return (dateTime < this.lowerRange);
        }

        public void Update(BarItem barItem)
        {
            ticks++;           
            if (highestPrice < barItem.High)
            {
                highestPrice = barItem.High;
            }

            if (lowestPrice > barItem.Low)
            {
                lowestPrice = barItem.Low;
            }
       
            this.currentPrice = barItem.Close;
            this.currentTime = barItem.Time;
        }

        public void ResetPrice(double openingPrice)
        {
            this.openingPrice= openingPrice;
            this.highestPrice= double.MinValue;
            this.lowestPrice= double.MaxValue;
            this.currentPrice= openingPrice;
            this.ticks = 0;
        }

        public BarItem MoveForward(BarItem barItem)
        {
            BarItem closingBar = new BarItem(this.lowerRange, this.openingPrice, this.currentPrice, this.highestPrice, this.lowestPrice);
            SetNextRange();
            ResetPrice(barItem.Open);
            Update(barItem);
            return closingBar;
        }

        public BarItem SkipForward()
        {
            BarItem closingBar = new BarItem(this.lowerRange, this.openingPrice, this.currentPrice, this.highestPrice, this.lowestPrice);
            SetNextRange();
            //ResetPrice(barItem.Open);
            //Update(barItem);
            return closingBar;
        }    
    }
}
