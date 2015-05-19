using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Persistence;

namespace Chartoscope.Brokers
{
    public class BacktestDataFeeder: IDataFeederEvents
    {
        private string barDataFile = null;
        private List<BarItemType> barItemTypeList = null;
        private Dictionary<string, TimeBasedDataFeed> timeBasedDataFeeds = null;

        public event DataFeederDelegates.NewBarHandler NewBar;
        public event DataFeederDelegates.BarGapHandler BarGap;
        public event DataFeederDelegates.NewQuoteHandler NewQuote;

        private bool cachingEnabled = true;
        private Guid cacheId = Guid.Empty;

        public BacktestDataFeeder(string barDataFile, List<BarItemType> barItemTypeList, Guid cacheId, bool cachingEnabled= true)
        {
            this.barDataFile = barDataFile;
            this.barItemTypeList = barItemTypeList;
            this.cacheId = cacheId;
            this.cachingEnabled = cachingEnabled;
        }

        private void Initialize(BarItem openingBar)
        {
            timeBasedDataFeeds = new Dictionary<string, TimeBasedDataFeed>();

            foreach (BarItemType barItemType in barItemTypeList)
            {
                if (barItemType.Mode == BarItemMode.Time)
                {
                    TimeframeUnitOption timeframeUnit= (TimeframeUnitOption) Enum.Parse(typeof(TimeframeUnitOption), barItemType.Code);
                    TimeBasedDataFeed dataFeed= new TimeBasedDataFeed(new TimeBarItemType(timeframeUnit), openingBar, cacheId, cachingEnabled);
                    dataFeed.Restart(openingBar.Time);
                    timeBasedDataFeeds.Add(barItemType.Code, dataFeed);
                }
            }
        }

        private BarItemNavigator navigator = null;
        private bool paused = false;

        private BarItem lastPrice = null;
        public BarItem LastPrice { get { return lastPrice; } }

        public void Start()
        {            
            BarItemFile barItemFile = new BarItemFile(barDataFile);
            navigator = barItemFile.GetReader();

            lastPrice = navigator.ReadBarItem();

            Initialize(lastPrice);

            if (cachingEnabled)
            {
                OpenCache();
            }

            ForwardDataFeed(lastPrice);

            while (!navigator.EOF() && !paused)
            {
                lastPrice = navigator.ReadBarItem();
                //WriteBarItem2(barItem);
                ForwardDataFeed(lastPrice);
            }

            if (cachingEnabled)
            {
                CloseCache();
            }
        }

        public void Resume()
        {
            BarItem barItem = null;

            while (!navigator.EOF() && !paused)
            {
                barItem = navigator.ReadBarItem();
                //WriteBarItem2(barItem);
                ForwardDataFeed(barItem);
            }
        }

        public void Pause()
        {
            this.paused = true;
        }

        private void ForwardDataFeed(BarItem barItem)
        {
            if (NewQuote != null)
            {
                NewQuote(barItem.Close, barItem.Close);
            }

            foreach (TimeBasedDataFeed dataFeed in this.timeBasedDataFeeds.Values)
            {               
                if (dataFeed.WithinCurrentRange(barItem.Time))
                {
                    dataFeed.Update(barItem);
                }
                else if (dataFeed.BeyondRange(barItem.Time))
                {
                    BarItem closingBar = null;
                    if (dataFeed.WithinNextRange(barItem.Time))
                    {
                        closingBar= dataFeed.MoveForward(barItem);
                        //WriteBarItem(closingBar, false);
                        //WriteBarItem2(closingBar);
                        RaiseNewBarEvent(dataFeed.BarType, closingBar);
                    }
                    else
                    {
                        bool skippingNext = false;

                        while(!dataFeed.WithinNextRange(barItem.Time))
                        {
                            closingBar = dataFeed.SkipForward();

                            if (skippingNext)
                            {
                                RaiseBarGapEvent(dataFeed.BarType, closingBar.Time);
                            }


                            //RaiseNewBarEvent(dataFeed.BarType, closingBar);
                            
                            
                            //WriteBarItem2(skippedBar, !skippingNext);
                            //WriteBarItem(skippedBar, skippingNext);
                            skippingNext = true;
                        }

                        closingBar = dataFeed.MoveForward(barItem);

                        RaiseBarGapEvent(dataFeed.BarType, closingBar.Time);
                        //RaiseNewBarEvent(dataFeed.BarType, closingBar);
                        //WriteBarItem(closingBar, skippingNext);
                        //WriteBarItem2(closingBar, !skippingNext);
                    }
                }
            }
        }

        private void RaiseNewBarEvent(BarItemType barItemType, BarItem barItem)
        {
            if (NewBar != null)
            {      
                if (cachingEnabled)
                {
                    UpdateCache(barItemType, barItem);
                }

                NewBar(barItemType, barItem);
            }
        }

        private void UpdateCache(BarItemType barItemType, BarItem barItem)
        {
            timeBasedDataFeeds[barItemType.Code].CachedPricebar.Append(barItem.Time, barItem.Open, barItem.Close, barItem.High, barItem.Low);
        }

        private void OpenCache()
        {
            foreach (TimeBasedDataFeed dataFeed in timeBasedDataFeeds.Values)
            {
                dataFeed.CachedPricebar.Open(CachingModeOption.Writing);
            }
        }

        private void CloseCache()
        {
            foreach (TimeBasedDataFeed dataFeed in timeBasedDataFeeds.Values)
            {
                dataFeed.CachedPricebar.Close();
            }
        }

        private void RaiseBarGapEvent(BarItemType barItemType, DateTime dateTime)
        {
            if (BarGap != null)
            {
                BarGap(barItemType, dateTime);
            }
        }

        private void WriteBarItem(BarItem barItem, bool skipped)
        {
            Debug.WriteLine(string.Format("Time: {0}, Opening: {1}, Closing: {2}, High: {3}, Low: {4}, Quality: {5}", 
                barItem.Time.ToShortTimeString(), barItem.Open, barItem.Close, barItem.High, barItem.Low, skipped? "Skipped":"OK"));
        }

        private void WriteBarItem2(BarItem barItem, bool valid=true)
        {
            Debug.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}",
                barItem.Time.ToShortTimeString(), barItem.Open, barItem.Close, barItem.High, barItem.Low, valid? "OK":"Missing"));
        }        
    }
}
