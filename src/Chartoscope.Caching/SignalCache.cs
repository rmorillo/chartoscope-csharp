using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Chartoscope.Common;

namespace Chartoscope.Caching
{
    public class SignalCache : SequenceKeyedCacheBase
    {
        private Guid cacheId = Guid.Empty;
        private BarItemType barType = null;        
        private object signalInstance = null;
        private string identityCode = null;
        private CacheModeOption cacheMode = CacheModeOption.ReadWrite;

        public SignalCache(object signalInstance, Guid cacheId)
            : base(CacheModeOption.Write)
        {
            this.barType = ((ISignal) signalInstance).BarType;
            this.identityCode = ((ISignal)signalInstance).IdentityCode;
            this.cacheId = cacheId;
            this.signalInstance = signalInstance;
            this.cacheMode = CacheModeOption.Write;
        }

        public SignalCache(string identityCode, BarItemType barType, Guid cacheId)
            : base(CacheModeOption.Read)
        {
            this.identityCode = identityCode;
            this.barType = barType;
            this.cacheId = cacheId;
            this.cacheMode = CacheModeOption.Read;

           
            this.Initialize(identityCode, barType, cacheId, CacheTypeOption.Signal);
            this.cacheIndex = CreateIndex();
        }

        public override CacheHeaderInfo GetHeader()
        {
            
            List<CacheColumn> columnList = GetCacheColumns();
            return new CacheHeaderInfo(identityCode, barType.Code, cacheId, CacheTypeOption.Signal, columnList.ToArray(), GetExtendedProperties());
        }

        private Dictionary<string, string> GetExtendedProperties()
        {
            List<AnalyticsItem> analyticsList = ((ISignal)signalInstance).RegisteredAnalytics;
            string[] analytics= new string[analyticsList.Count];
            int index= 0;
            foreach (AnalyticsItem item in analyticsList)
            {
                analytics[index] = item.IdentityCode;
                index++;
            }
            string analyticsIdentities = string.Join(Environment.NewLine, analytics);

            if (analytics.Length > 0)
            {
                Dictionary<string, string> extProp = new Dictionary<string, string>();
                extProp.Add("Indicators", analyticsIdentities);

                return extProp;
            }
            else
            {
                return null;
            }
               
        }

        private List<CacheColumn> GetCacheColumns()
        {
            List<CacheColumn> columns = new List<CacheColumn>();

            columns.Add(new CacheColumn(0, "Sequence", CacheDataTypeOption.Long, 8));
            columns.Add(new CacheColumn(1, "ClosingBarTime", CacheDataTypeOption.Long, 8));
            columns.Add(new CacheColumn(2, "Position", CacheDataTypeOption.Integer, 4));
            columns.Add(new CacheColumn(3, "SignalTime", CacheDataTypeOption.Long, 8));

            return columns;
        }

        public void AppendSignal(long sequence, DateTime closingBarTime, PositionMode position, DateTime signalTime)
        {           
            this.Append(sequence, closingBarTime.Ticks, position, signalTime.Ticks);
        }

        public Dictionary<DateTime, SignalDataItem[]> SelectAllSignals()
        {
            Dictionary<DateTime, SignalDataItem[]> timeKeyedSignals = new Dictionary<DateTime, SignalDataItem[]>();
            CacheRow[] cacheRows = this.Select(0, this.LastRowCount);
            if (cacheRows != null)
            {
                DateTime lastDate = new DateTime((long)cacheRows[0][1]);
                List<SignalDataItem> signalDataItems = new List<SignalDataItem>();
                foreach (CacheRow cacheRow in cacheRows)
                {
                    DateTime currentDate = new DateTime((long)cacheRow[1]);
                    if (currentDate > lastDate)
                    {
                        timeKeyedSignals.Add(lastDate, signalDataItems.ToArray());
                        signalDataItems = new List<SignalDataItem>();
                        lastDate = currentDate;
                    }

                    signalDataItems.Add(new SignalDataItem((long)cacheRow[0], new DateTime((long)cacheRow[1]), (PositionMode)cacheRow[2], new DateTime((long)cacheRow[3])));
                }

                timeKeyedSignals.Add(lastDate, signalDataItems.ToArray());
            }

            return timeKeyedSignals.Count > 0 ? timeKeyedSignals : null;
        }

        public SignalDataItem[] SelectSignals(DateTime startDate, DateTime endDate, int frameSize)
        {
            long closestStartRowNumber = ClosestLowerTargetRowNumber(0); 
            long closestEndRowNumber = ClosestUpperTargetRowNumber(0); 
            
            List<SignalDataItem> signalDataItems = null;

            CacheRow[] cacheRows = this.Select(closestStartRowNumber, closestEndRowNumber-closestStartRowNumber);

            if (cacheRows == null)
            {
                return null;
            }
            else
            {

                signalDataItems = new List<SignalDataItem>();
                int barIndex = 0;
                foreach(CacheRow cacheRow in cacheRows)
                {
                    DateTime closingBarTime= new DateTime((long)cacheRow[1]);
                    if (closingBarTime >= startDate && closingBarTime <= endDate)
                    {
                        signalDataItems.Add(new SignalDataItem((long) cacheRow[0], closingBarTime, (PositionMode)cacheRow[2], new DateTime((long)cacheRow[3])));
                    }
                }
            }

            return signalDataItems.ToArray();


        }
    }
}
