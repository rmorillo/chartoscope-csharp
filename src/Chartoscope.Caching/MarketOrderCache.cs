using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public class MarketOrderCache : SequenceKeyedCacheBase
    {
        private Guid cacheId = Guid.Empty;
        private BarItemType barType = null;
        private string identityCode = null;
        private CacheModeOption cacheMode = CacheModeOption.ReadWrite;

        public MarketOrderCache(BarItemType barType, Guid sessionId, Guid cacheId)
            : base(CacheModeOption.Write)
        {
            this.identityCode = sessionId.ToString();
            this.barType = barType;
            this.cacheId = cacheId;
            this.cacheMode = CacheModeOption.Write;
        }

        public MarketOrderCache(BarItemType barType, string identityCode, Guid cacheId)
            : base(CacheModeOption.Read)
        {
            this.identityCode = identityCode;
            this.barType = barType;
            this.cacheId = cacheId;
            this.cacheMode = CacheModeOption.Read;


            this.Initialize(identityCode, barType, cacheId, CacheTypeOption.Orders);
            this.cacheIndex = CreateIndex();
        }

        public override CacheHeaderInfo GetHeader()
        {

            List<CacheColumn> columnList = GetCacheColumns();
            return new CacheHeaderInfo(identityCode, barType.Code, cacheId, CacheTypeOption.Orders, columnList.ToArray(), GetExtendedProperties());
        }

        private Dictionary<string, string> GetExtendedProperties()
        {
            //List<AnalyticsItem> analyticsList = ((IStrategy)signalInstance).RegisteredAnalytics;
            //string[] analytics = new string[analyticsList.Count];
            //int index = 0;
            //foreach (AnalyticsItem item in analyticsList)
            //{
            //    analytics[index] = item.IdentityCode;
            //    index++;
            //}
            //string analyticsIdentities = string.Join(Environment.NewLine, analytics);

            //if (analytics.Length > 0)
            //{
            //    Dictionary<string, string> extProp = new Dictionary<string, string>();
            //    extProp.Add("Signals", analyticsIdentities);

            //    return extProp;
            //}
            //else
            //{
                return null;
            //}

        }


         //this.orderId = orderId;
         //   this.orderTime = time;
         //   this.ticker = tickerType;
         //   this.orderState = orderState;
         //   this.openingBidPrice = openingBidPrice;
         //   this.openingAskPrice = openingAskPrice;
         //   this.openingPriceBar = openingPriceBar;
        //this.closedTime= time;
        //    this.closingBidPrice= closingBidPrice;
        //    this.closingAskPrice= closingAskPrice;
        //    this.closingPriceBar = barItem;
        private List<CacheColumn> GetCacheColumns()
        {
            List<CacheColumn> columns = new List<CacheColumn>();

            columns.Add(new CacheColumn(0, "Sequence", CacheDataTypeOption.Long, 8));                        
            columns.Add(new CacheColumn(1, "Position", CacheDataTypeOption.Integer, 4));
            columns.Add(new CacheColumn(2, "OpeningExecutionTime", CacheDataTypeOption.Long, 8));
            columns.Add(new CacheColumn(3, "OpeningBarTime", CacheDataTypeOption.Long, 8));
            columns.Add(new CacheColumn(4, "OpeningBidPrice", CacheDataTypeOption.Double, 8));
            columns.Add(new CacheColumn(5, "OpeningAskPrice", CacheDataTypeOption.Double, 8));
            columns.Add(new CacheColumn(6, "ClosingExecutionTime", CacheDataTypeOption.Long, 8));
            columns.Add(new CacheColumn(7, "ClosingBarTime", CacheDataTypeOption.Long, 8));
            columns.Add(new CacheColumn(8, "ClosingBidPrice", CacheDataTypeOption.Double, 8));
            columns.Add(new CacheColumn(9, "ClosingAskPrice", CacheDataTypeOption.Double, 8));

            return columns;
        }

        public void AppendMarketOrder(long sequence, PositionMode position, DateTime openingExecutionTime, DateTime openingBarTime, double openingBidPrice, double openingAskPrice,
             DateTime closingExecutionTime, DateTime closingBarTime, double closingBidPrice, double closingAskPrice)
        {
            this.Append(sequence, position, openingExecutionTime.Ticks, openingBarTime.Ticks, openingBidPrice, openingAskPrice, 
                closingExecutionTime.Ticks, closingBarTime.Ticks, closingBidPrice, closingAskPrice);
        }

        public Dictionary<DateTime, StrategyDataItem[]> SelectAllStrategies()
        {
            Dictionary<DateTime, StrategyDataItem[]> timeKeyedSignals = new Dictionary<DateTime, StrategyDataItem[]>();
            CacheRow[] cacheRows = this.Select(0, this.LastRowCount);
            if (cacheRows != null)
            {
                DateTime lastDate = new DateTime((long)cacheRows[0][1]);
                List<StrategyDataItem> signalDataItems = new List<StrategyDataItem>();
                foreach (CacheRow cacheRow in cacheRows)
                {
                    DateTime currentDate = new DateTime((long)cacheRow[1]);
                    if (currentDate > lastDate)
                    {
                        timeKeyedSignals.Add(lastDate, signalDataItems.ToArray());
                        signalDataItems = new List<StrategyDataItem>();
                        lastDate = currentDate;
                    }

                    signalDataItems.Add(new StrategyDataItem((long)cacheRow[0], new DateTime((long)cacheRow[1]), (PositionMode)cacheRow[2], new DateTime((long)cacheRow[3])));
                }

                timeKeyedSignals.Add(lastDate, signalDataItems.ToArray());
            }

            return timeKeyedSignals.Count > 0 ? timeKeyedSignals : null;
        }


        public StrategyDataItem[] SelectStrategies(DateTime startDate, DateTime endDate, int frameSize)
        {
            long closestStartRowNumber = ClosestLowerTargetRowNumber(0);
            long closestEndRowNumber = ClosestUpperTargetRowNumber(0);

            if (closestEndRowNumber - closestStartRowNumber < frameSize)
            {
                closestEndRowNumber = closestStartRowNumber + frameSize;
            }


            List<StrategyDataItem> signalDataItems = null;

            CacheRow[] cacheRows = this.Select(closestStartRowNumber, closestEndRowNumber - closestStartRowNumber);

            if (cacheRows == null)
            {
                return null;
            }
            else
            {

                signalDataItems = new List<StrategyDataItem>();
                foreach (CacheRow cacheRow in cacheRows)
                {
                    DateTime closingBarTime = new DateTime((long)cacheRow[1]);
                    if (closingBarTime >= startDate && closingBarTime <= endDate)
                    {
                        signalDataItems.Add(new StrategyDataItem((long)cacheRow[0], closingBarTime, (PositionMode)cacheRow[2], new DateTime((long)cacheRow[3])));
                    }
                }
            }

            return signalDataItems.ToArray();
        }
    }
}
